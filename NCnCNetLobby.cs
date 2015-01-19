/// @author Rampastring
/// http://www.moddb.com/members/rampastring
/// @version 12. 1. 2015

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.Net;
using System.Diagnostics;
using System.Threading;
using System.Linq;
using ClientCore;
using ClientCore.cncnet5;
using WMPLib;

namespace ClientGUI
{
    public partial class NCnCNetLobby : Form
    {
        /// <summary>
        /// Creates a new instance of the CnCNet lobby.
        /// Called by the main client executable when connecting to CnCNet.
        /// </summary>
        /// <param name="gameVersion">The version of the game.</param>
        public NCnCNetLobby(string gameVersion)
        {
            Application.VisualStyleState = System.Windows.Forms.VisualStyles.VisualStyleState.NonClientAreaEnabled;
            InitializeComponent();
            ProgramConstants.GAME_VERSION = gameVersion;
        }

        public delegate void ConversationOpenedCallback(string userName);
        public static event ConversationOpenedCallback ConversationOpened;

        public delegate void UserListReceivedCallback(string[] userNames, string channelName);
        public static event UserListReceivedCallback UserListReceived;

        public delegate void ColorChangedEventHandler(int colorId);
        public static event ColorChangedEventHandler OnColorChanged;

        public static void DoConversationOpened(string userName)
        {
            if (ConversationOpened != null)
                ConversationOpened(userName);
        }

        public static void DoUserListReceived(string[] userNames, string channelName)
        {
            if (UserListReceived != null)
                UserListReceived(userNames, channelName);
        }

        /// <summary>
        /// Used for informing the Game Lobby about the user changing their color.
        /// </summary>
        /// <param name="colorId"></param>
        public static void DoColorChanged(int colorId)
        {
            if (OnColorChanged != null)
                OnColorChanged(colorId);
        }

        // Various callbacks needed for thread-safety

        private delegate void StringCallback(string message);
        private delegate void UserListCallback(string[] users, string channelName);
        private delegate void UserJoinChannelCallback(string channelName, string userName, string ipAddress);
        private delegate void UserLeaveChannelCallback(string channelName, string userName);
        private delegate void TopicMessageParsedCallback(string channelName, string message);
        private delegate void PrivmsgParsedCallback(string channelName, string message, string sender);
        private delegate void PrivateMessageParsedCallback(string message, string sender);
        private delegate void PrivateMessageSentCallback(string message, string receiver);
        private delegate void AwayMessageCallback(string userName, string reason);
        private delegate void CtcpGameCallback(string sender, string channelName, string ctcpMessage);
        private delegate void UserKickedCallback(string channelName, string userName);

        List<List<MessageInfo>> MessageInfos = new List<List<MessageInfo>>();
        List<List<string>> UserLists = new List<List<string>>();
        List<List<Color>> UserColors = new List<List<Color>>();
        List<Color> ChatColors = new List<Color>();
        List<string> ChannelNames = new List<string>();
        List<string> ChannelUINames = new List<string>();

        /// <summary>
        /// List of colors that the current games are displayed in.
        /// </summary>
        List<Color> GameColors = new List<Color>();

        Color cPlayerNameColor;
        Color cAdminNameColor;
        Color cDefaultChatColor;
        Color cPmOtherUserColor;
        Color cLockedGameColor;
        Color cListBoxFocusColor;

        /// <summary>
        /// The ID of the currently viewed chat channel.
        /// </summary>
        int currentChannelId = 0;

        WindowsMediaPlayer wmPlayer;
        SoundPlayer sp;
        SoundPlayer sndGameCreated;

        Image btn92px;
        Image btn92px_c;
        Image btn121px;
        Image btn121px_c;
        Image btnHideChannelsUp;
        Image btnHideChannelsDown;

        Image dtaIcon;
        Image tiIcon;
        Image tsIcon;
        Image lockedGameIcon;
        Image incompatibleGameIcon;
        Image passwordedGameIcon;

        /// <summary>
        /// The ID string of the current game.
        /// </summary>
        string myGame = "DTA";

        /// <summary>
        /// Contains the identifier of the game channel that the user is attempting to join.
        /// </summary>
        string joinGameChannelName = String.Empty;

        /// <summary>
        /// Character used for CTCP messages.
        /// </summary>
        char weirdChar1 = (char)58;

        /// <summary>
        /// Character used for CTCP messages.
        /// </summary>
        char weirdChar2 = (char)01;

        /// <summary>
        /// True if a game is in progress, otherwise false.
        /// </summary>
        bool gameInProgress = false;

        /// <summary>
        /// Used for blocking some UI functions until a welcome message
        /// from the server has been received.
        /// </summary>
        bool welcomeMessageReceived = false;

        /// <summary>
        /// The game lobby window.
        /// </summary>
        NGameLobby gameLobbyWindow;

        /// <summary>
        /// Whether to attempt retrieving WHOIS info for players in the player list box.
        /// Disable when automatically scrolling the players list box.
        /// </summary>
        bool requestWhois = true;

        /// <summary>
        /// Initializes the lobby. Loads graphics and subscribes to events triggered
        /// by the networking part of the client.
        /// </summary>
        private void NCnCNetLobby_Load(object sender, EventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.ThreadException += Application_ThreadException;

            this.Font = SharedLogic.getCommonFont();

            Font listBoxFont = SharedLogic.getListBoxFont();

            lbChatMessages.Font = listBoxFont;
            lbGameList.Font = listBoxFont;
            lbPlayerList.Font = listBoxFont;

            MessageInfos.Add(new List<MessageInfo>()); // DTA
            MessageInfos.Add(new List<MessageInfo>()); // TI
            MessageInfos.Add(new List<MessageInfo>()); // TO
            MessageInfos.Add(new List<MessageInfo>()); // TS
            MessageInfos.Add(new List<MessageInfo>()); // CnCNet

            UserLists.Add(new List<string>()); // DTA
            UserLists.Add(new List<string>()); // TI
            UserLists.Add(new List<string>()); // TO
            UserLists.Add(new List<string>()); // TS
            UserLists.Add(new List<string>()); // CnCNet

            UserColors.Add(new List<Color>()); // DTA
            UserColors.Add(new List<Color>()); // TI
            UserColors.Add(new List<Color>()); // TO
            UserColors.Add(new List<Color>()); // TS
            UserColors.Add(new List<Color>()); // CnCNet

            ChannelNames.Add("#dta");
            ChannelNames.Add("#ti");
            ChannelNames.Add("#to");
            ChannelNames.Add("#ts");
            ChannelNames.Add("#cncnet");

            ChannelUINames.Add("Dawn of the Tiberium Age");
            ChannelUINames.Add("Twisted Insurrection");
            ChannelUINames.Add("Tiberian Odyssey");
            ChannelUINames.Add("Tiberian Sun");
            ChannelUINames.Add("General CnCNet Chat");

            chkChannelDTA.LabelText = "The Dawn of the Tiberium Age";
            chkChannelTI.LabelText = "Twisted Insurrection";
            chkChannelTO.LabelText = "Tiberian Odyssey";
            chkChannelTS.LabelText = "Tiberian Sun";
            chkChannelCnCNet.LabelText = "General CnCNet Chat";

            SetDefaultGame();
            LoadSettings();

            wmPlayer = new WindowsMediaPlayer();
            wmPlayer.URL = ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "lobbymusic.wav";
            wmPlayer.settings.setMode("loop", true);
            if (DomainController.Instance().getMainMenuMusicStatus())
                wmPlayer.controls.play();
            else
            {
                btnMusicToggle.Text = "Music OFF";
                wmPlayer.controls.stop();
            }

            sp = new SoundPlayer(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "button.wav");
            sndGameCreated = new SoundPlayer(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "gamecreated.wav");

            this.Icon = Icon.ExtractAssociatedIcon(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "clienticon.ico");
            this.BackgroundImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "cncnetlobbybg.png");

            string backgroundImageLayout = DomainController.Instance().getCnCNetLobbyBackgroundImageLayout();
            switch (backgroundImageLayout)
            {
                case "Center":
                    this.BackgroundImageLayout = ImageLayout.Center;
                    break;
                case "Stretch":
                    this.BackgroundImageLayout = ImageLayout.Stretch;
                    break;
                case "Zoom":
                    this.BackgroundImageLayout = ImageLayout.Zoom;
                    break;
                default:
                case "Tile":
                    this.BackgroundImageLayout = ImageLayout.Tile;
                    break;
            }

            string[] adminNameColor = DomainController.Instance().getAdminNameColor().Split(',');
            cAdminNameColor = Color.FromArgb(Convert.ToByte(adminNameColor[0]), Convert.ToByte(adminNameColor[1]), Convert.ToByte(adminNameColor[2]));

            string[] playerNameColor = DomainController.Instance().getPlayerNameColor().Split(',');
            cPlayerNameColor = Color.FromArgb(Convert.ToByte(playerNameColor[0]), Convert.ToByte(playerNameColor[1]), Convert.ToByte(playerNameColor[2]));

            string[] defChatColor = DomainController.Instance().getDefaultChatColor().Split(',');
            cDefaultChatColor = Color.FromArgb(Convert.ToByte(defChatColor[0]), Convert.ToByte(defChatColor[1]), Convert.ToByte(defChatColor[2]));

            string[] pmOtherUserColorString = DomainController.Instance().getReceivedPMColor().Split(',');
            cPmOtherUserColor = Color.FromArgb(Convert.ToByte(pmOtherUserColorString[0]), Convert.ToByte(pmOtherUserColorString[1]), Convert.ToByte(pmOtherUserColorString[2]));

            string[] lockedGameColor = DomainController.Instance().getLockedGameColor().Split(',');
            cLockedGameColor = Color.FromArgb(Convert.ToByte(lockedGameColor[0]), Convert.ToByte(lockedGameColor[1]), Convert.ToByte(lockedGameColor[2]));

            string[] listBoxFocusColor = DomainController.Instance().getListBoxFocusColor().Split(',');
            cListBoxFocusColor = Color.FromArgb(Convert.ToByte(listBoxFocusColor[0]), Convert.ToByte(listBoxFocusColor[1]), Convert.ToByte(listBoxFocusColor[2]));

            ChatColors.Add(cDefaultChatColor);
            ChatColors.Add(cDefaultChatColor);
            ChatColors.Add(Color.LightBlue);
            ChatColors.Add(Color.LimeGreen);
            ChatColors.Add(Color.IndianRed);
            ChatColors.Add(Color.Red);
            ChatColors.Add(Color.MediumOrchid);
            ChatColors.Add(Color.Orange);
            ChatColors.Add(Color.Yellow);
            ChatColors.Add(Color.Lime);
            ChatColors.Add(Color.Turquoise);
            ChatColors.Add(Color.LightCyan);
            ChatColors.Add(Color.LightSkyBlue);
            ChatColors.Add(Color.Fuchsia);
            ChatColors.Add(Color.Gray);
            ChatColors.Add(Color.Gray);

            cmbMessageColor.AddItem("Light Blue", ChatColors[2]);
            cmbMessageColor.AddItem("Green", ChatColors[3]);
            cmbMessageColor.AddItem("Dark Red", ChatColors[4]);
            cmbMessageColor.AddItem("Red", ChatColors[5]);
            cmbMessageColor.AddItem("Purple", ChatColors[6]);
            cmbMessageColor.AddItem("Orange", ChatColors[7]);
            cmbMessageColor.AddItem("Yellow", ChatColors[8]);
            cmbMessageColor.AddItem("Lime Green", ChatColors[9]);
            cmbMessageColor.AddItem("Turquoise", ChatColors[10]);
            cmbMessageColor.AddItem("Light Cyan", ChatColors[11]);
            cmbMessageColor.AddItem("Sky Blue", ChatColors[12]);
            cmbMessageColor.AddItem("Pink", ChatColors[13]);
            cmbMessageColor.AddItem("Light Gray", ChatColors[14]);

            Logger.Log("Default personal chat color: " + DomainController.Instance().getDefaultPersonalChatColor());
            cmbMessageColor.SelectedIndex = DomainController.Instance().getDefaultPersonalChatColor();

            int savedChatColor = DomainController.Instance().getCnCNetChatColor();
            if (savedChatColor > -1)
                cmbMessageColor.SelectedIndex = savedChatColor;

            string[] labelColor = DomainController.Instance().getUILabelColor().Split(',');
            Color cLabelColor = Color.FromArgb(Convert.ToByte(labelColor[0]), Convert.ToByte(labelColor[1]), Convert.ToByte(labelColor[2]));
            lblMessageColor.ForeColor = cLabelColor;
            lblFollowChannels.ForeColor = cLabelColor;
            lblGameInfo.ForeColor = cLabelColor;
            lblGames.ForeColor = cLabelColor;
            lblHost.ForeColor = cLabelColor;
            lblMapName.ForeColor = cLabelColor;
            lblGameMode.ForeColor = cLabelColor;
            lblPassworded.ForeColor = cLabelColor;
            lblPlayerList.ForeColor = cLabelColor;
            lblPlayers.ForeColor = cLabelColor;
            lblPlayersValue.ForeColor = cLabelColor;
            lblStarted.ForeColor = cLabelColor;
            lblVersion.ForeColor = cLabelColor;
            lblChannel.ForeColor = cLabelColor;
            chkChannelCnCNet.ForeColor = cLabelColor;
            chkChannelDTA.ForeColor = cLabelColor;
            chkChannelTI.ForeColor = cLabelColor;
            chkChannelTO.ForeColor = cLabelColor;
            chkChannelTS.ForeColor = cLabelColor;
            chkChannelCnCNet.BaseColor = cLabelColor;
            chkChannelDTA.BaseColor = cLabelColor;
            chkChannelTI.BaseColor = cLabelColor;
            chkChannelTO.BaseColor = cLabelColor;
            chkChannelTS.BaseColor = cLabelColor;

            string[] altUiColor = DomainController.Instance().getUIAltColor().Split(',');
            Color cAltUiColor = Color.FromArgb(Convert.ToByte(altUiColor[0]), Convert.ToByte(altUiColor[1]), Convert.ToByte(altUiColor[2]));
            chkChannelCnCNet.HoverColor = cAltUiColor;
            chkChannelDTA.HoverColor = cAltUiColor;
            chkChannelTI.HoverColor = cAltUiColor;
            chkChannelTO.HoverColor = cAltUiColor;
            chkChannelTS.HoverColor = cAltUiColor;
            lbPlayerList.ForeColor = cAltUiColor;
            lbChatMessages.ForeColor = cAltUiColor;
            lbGameList.ForeColor = cAltUiColor;
            btnJoinGame.ForeColor = cAltUiColor;
            btnNewGame.ForeColor = cAltUiColor;
            btnReturnToMenu.ForeColor = cAltUiColor;
            btnSend.ForeColor = cAltUiColor;
            btnHideChannels.ForeColor = cAltUiColor;
            btnMusicToggle.ForeColor = cAltUiColor;
            tbChatInput.ForeColor = cAltUiColor;
            cmbCurrentChannel.ForeColor = cAltUiColor;
            cmbMessageColor.ForeColor = cAltUiColor;

            string[] backgroundColor = DomainController.Instance().getUIAltBackgroundColor().Split(',');
            Color cBackColor = Color.FromArgb(Convert.ToByte(backgroundColor[0]), Convert.ToByte(backgroundColor[1]), Convert.ToByte(backgroundColor[2]));
            btnJoinGame.BackColor = cBackColor;
            btnNewGame.BackColor = cBackColor;
            btnReturnToMenu.BackColor = cBackColor;
            btnSend.BackColor = cBackColor;
            btnHideChannels.BackColor = cBackColor;
            btnMusicToggle.BackColor = cBackColor;
            lbChatMessages.BackColor = cBackColor;
            lbGameList.BackColor = cBackColor;
            lbPlayerList.BackColor = cBackColor;
            tbChatInput.BackColor = cBackColor;
            cmbCurrentChannel.BackColor = cBackColor;
            cmbMessageColor.BackColor = cBackColor;

            panel1.BackgroundImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "cncnetlobbypanelbg.png");
            panel2.BackgroundImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "cncnetlobbypanelbg.png");

            btn92px = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "92pxbtn.png");
            btn92px_c = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "92pxbtn_c.png");

            btn121px = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "121pxbtn.png");
            btn121px_c = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "121pxbtn_c.png");

            btnHideChannelsUp = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "hideChannels_Up.png");
            btnHideChannelsDown = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "hideChannels_Down.png");

            dtaIcon = Image.FromFile(ProgramConstants.gamepath + "Resources\\dtaicon.png");
            tiIcon = Image.FromFile(ProgramConstants.gamepath + "Resources\\tiicon.png");
            tsIcon = Image.FromFile(ProgramConstants.gamepath + "Resources\\tsicon.png");

            lockedGameIcon = Image.FromFile(ProgramConstants.gamepath + "Resources\\lockedgame.png");
            incompatibleGameIcon = Image.FromFile(ProgramConstants.gamepath + "Resources\\incompatible.png");
            passwordedGameIcon = Image.FromFile(ProgramConstants.gamepath + "Resources\\passwordedgame.png");

            btnNewGame.BackgroundImage = btn92px;
            btnJoinGame.BackgroundImage = btn92px;
            btnMusicToggle.BackgroundImage = btn92px;

            btnReturnToMenu.BackgroundImage = btn121px;

            btnHideChannels.BackgroundImage = btnHideChannelsUp;

            int displayedItems = lbChatMessages.DisplayRectangle.Height / lbChatMessages.ItemHeight;

            sbChat.ThumbBottomImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "sbThumbBottom.png");
            sbChat.ThumbBottomSpanImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "sbThumbBottomSpan.png");
            sbChat.ThumbMiddleImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "sbMiddle.png");
            sbChat.ThumbTopImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "sbThumbTop.png");
            sbChat.ThumbTopSpanImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "sbThumbTopSpan.png");
            sbChat.UpArrowImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "sbUpArrow.png");
            sbChat.DownArrowImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "sbDownArrow.png");
            sbChat.BackgroundImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "sbBackground.png");
            sbChat.Scroll += sbChat_Scroll;
            sbChat.Maximum = lbChatMessages.Items.Count - Convert.ToInt32(displayedItems * 0.2);
            sbChat.Minimum = 0;
            sbChat.ChannelColor = cBackColor;
            sbChat.LargeChange = 27;
            sbChat.SmallChange = 9;
            sbChat.Value = 0;

            lbChatMessages.MouseWheel += lbChatMessages_MouseWheel;

            int displayedPItems = lbPlayerList.DisplayRectangle.Height / lbPlayerList.ItemHeight;

            sbPlayers.ThumbBottomImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "sbThumbBottom.png");
            sbPlayers.ThumbBottomSpanImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "sbThumbBottomSpan.png");
            sbPlayers.ThumbMiddleImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "sbMiddle.png");
            sbPlayers.ThumbTopImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "sbThumbTop.png");
            sbPlayers.ThumbTopSpanImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "sbThumbTopSpan.png");
            sbPlayers.UpArrowImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "sbUpArrow.png");
            sbPlayers.DownArrowImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "sbDownArrow.png");
            sbPlayers.BackgroundImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "sbBackground.png");
            sbPlayers.Scroll += sbPlayers_Scroll;
            sbPlayers.Maximum = lbPlayerList.Items.Count - Convert.ToInt32(displayedPItems * 0.2);
            sbPlayers.Minimum = 0;
            sbPlayers.ChannelColor = cBackColor;
            sbPlayers.LargeChange = 27;
            sbPlayers.SmallChange = 9;
            sbPlayers.Value = 0;

            lbPlayerList.MouseWheel += lbPlayerList_MouseWheel;

            lbGameList.SelectedIndex = -1;

            CnCNetData.ConnectionBridge.WelcomeMessageParsed += new RConnectionBridge.StringEventHandler(Instance_WelcomeMessageParsed);
            CnCNetData.ConnectionBridge.ServerMessageParsed += new RConnectionBridge.StringEventHandler(Instance_ServerMessageParsed);
            CnCNetData.ConnectionBridge.UserListReceived += new RConnectionBridge.UserListEventHandler(Instance_UserListReceived);
            CnCNetData.ConnectionBridge.OnUserJoinedChannel += new RConnectionBridge.ChannelJoinEventHandler(Instance_OnUserJoinedChannel);
            CnCNetData.ConnectionBridge.OnUserLeaveChannel += new RConnectionBridge.ChannelLeaveEventHandler(Instance_OnUserLeaveChannel);
            CnCNetData.ConnectionBridge.TopicMessageParsed += new RConnectionBridge.TopicMessageParsedEventHandler(Instance_TopicMessageParsed);
            CnCNetData.ConnectionBridge.OnUserQuit += new RConnectionBridge.StringEventHandler(Instance_OnUserQuit);
            CnCNetData.ConnectionBridge.PrivmsgParsed += new RConnectionBridge.PrivmsgParsedEventHandler(Instance_PrivmsgParsed);
            CnCNetData.ConnectionBridge.PrivateMessageParsed += new RConnectionBridge.PrivateMessageParsedEventHandler(Instance_PrivateMessageParsed);
            CnCNetData.ConnectionBridge.PrivateMessageSent += new RConnectionBridge.PrivateMessageSentEventHandler(Instance_PrivateMessageSent);
            CnCNetData.ConnectionBridge.OnErrorReceived += new RConnectionBridge.ErrorEventHandler(Instance_OnErrorReceived);
            CnCNetData.ConnectionBridge.OnAwayMessageReceived += new RConnectionBridge.AwayEventHandler(Instance_OnAwayMessageReceived);
            CnCNetData.ConnectionBridge.OnConnectionLost += new RConnectionBridge.ConnectionLostEventHandler(Instance_OnConnectionLost);
            CnCNetData.ConnectionBridge.OnCtcpGameParsed += new RConnectionBridge.CTCPGameParsedEventHandler(Instance_OnCtcpGameParsed);
            CnCNetData.ConnectionBridge.OnIncorrectPassword += new RConnectionBridge.IncorrectPasswordEventHandler(Instance_OnIncorrectPassword);
            CnCNetData.ConnectionBridge.OnUserKicked += new RConnectionBridge.UserKickedEventHandler(Instance_OnUserKicked);
            CnCNetData.OnGameStarted += new CnCNetData.GameStartedEventHandler(CnCNetData_OnGameStarted);
            CnCNetData.OnGameStopped += new CnCNetData.GameStoppedEventHandler(CnCNetData_OnGameStopped);
            CnCNetData.OnGameLobbyClosed += new CnCNetData.GameLobbyClosedEventHandler(CnCNetData_OnGameLobbyClosed);

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 2000;
            timer.Tick += new EventHandler(RefreshGameList);
            timer.Start();

            this.Text = string.Format("[{0}] CnCNet Lobby ({1} / {2})", myGame, ProgramConstants.GAME_VERSION, Application.ProductVersion);

            // Needs to be called once initialization is complete.
            CnCNetData.ConnectionBridge.DoUIInitialized();
        }

        /// <summary>
        /// Executed when the user scrolls the player list with the mouse wheel.
        /// </summary>
        private void lbPlayerList_MouseWheel(object sender, MouseEventArgs e)
        {
            sbPlayers.Value += e.Delta / -40;
            sbPlayers_Scroll(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Executed when the user scrolls the chat with the mouse wheel.
        /// </summary>
        private void lbChatMessages_MouseWheel(object sender, MouseEventArgs e)
        {
            sbChat.Value += e.Delta / -40;
            sbChat_Scroll(sender, EventArgs.Empty);
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Exception ex = e.Exception;

            DisplayUnhandledExceptionMessage(sender, ex);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;

            DisplayUnhandledExceptionMessage(sender, ex);
        }

        /// <summary>
        /// Handles otherwise uncaught exceptions occuring in the UI thread.
        /// </summary>
        private static void DisplayUnhandledExceptionMessage(object sender, Exception ex)
        {
            MessageBox.Show("The CnCNet Client has crashed. Error Message: " + Environment.NewLine +
                ex.Message + Environment.NewLine + Environment.NewLine +
                "See cncnetclient.log for further info. If you can reproduce this crash, " + Environment.NewLine +
                "please report about it to your mod's authors or directly to Rampastring " + Environment.NewLine +
                "(the creator of this client) at " + Environment.NewLine +
                "http://www.moddb.com/members/rampastring", "KABOOOOOOOM22");
            Logger.Log("Unhandled exception!!!");
            Logger.Log(ex.Message);
            Logger.Log(ex.Source);
            Logger.Log(ex.StackTrace);
            Environment.Exit(0);
        }

        /// <summary>
        /// Executed when an user is kicked from a IRC channel.
        /// </summary>
        /// <param name="channelName">The name of the channel where the user was kicked from.</param>
        /// <param name="userName">The name of the user who was kicked.</param>
        private void Instance_OnUserKicked(string channelName, string userName)
        {
            if (this.InvokeRequired)
            {
                UserKickedCallback d = new UserKickedCallback(Instance_OnUserKicked);
                this.BeginInvoke(d, channelName, userName);
                return;
            }

            bool updatePlayers = false;

            for (int chId = 0; chId < ChannelNames.Count; chId++)
            {
                if (ChannelNames[chId] == channelName)
                {
                    int userIndex = UserLists[chId].FindIndex(c => c == userName);
                    if (userIndex > -1)
                    {
                        UserLists[chId].RemoveAt(userIndex);
                        UserColors[chId].RemoveAt(userIndex);
                        MessageInfos[chId].Add(new MessageInfo(Color.White,
                            string.Format("{0} has been kicked from the {1} channel.", userName, ChannelUINames[chId])));
                    }

                    if (currentChannelId == chId)
                        updatePlayers = true;
                    break;
                }
            }

            if (updatePlayers)
            {
                UpdatePlayerList();
            }
        }

        /// <summary>
        /// Handles an incorrect channel password message from the server.
        /// </summary>
        /// <param name="channelName">The name of the channel for which an invalid password was entered.</param>
        private void Instance_OnIncorrectPassword(string channelName)
        {
            if (this.InvokeRequired)
            {
                StringCallback d = new StringCallback(Instance_OnIncorrectPassword);
                this.BeginInvoke(d, channelName);
                return;
            }

            // Look for game in the list of games by searching with its channel name
            bool channelFound = false;
            for (int gameId = 0; gameId < CnCNetData.Games.Count; gameId++)
            {
                if (CnCNetData.Games[gameId].ChannelName == channelName)
                {
                    MessageInfos[currentChannelId].Add(new MessageInfo(Color.White,
                        "Unable to join game " + CnCNetData.Games[gameId].RoomName + "; incorrect password"));
                    AddChannelMessageToListBox(currentChannelId);
                    channelFound = true;
                    break;
                }
            }

            // If not found (meaning the game was disbanded / hidden after receiving the message), display a generic message
            if (!channelFound)
            {
                MessageInfos[currentChannelId].Add(new MessageInfo(Color.White,
                        "Unable to join game; incorrect password"));
                AddChannelMessageToListBox(currentChannelId);
            }
        }

        /// <summary>
        /// Executed when the Game Lobby window is closed.
        /// </summary>
        private void CnCNetData_OnGameLobbyClosed()
        {
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;

            CnCNetData.ConnectionBridge.SendMessage("AWAY");

            gameLobbyWindow.Dispose();
        }

        /// <summary>
        /// Executed when a game is finished.
        /// </summary>
        private void CnCNetData_OnGameStopped()
        {
            try
            {
                wmPlayer.URL = ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "lobbymusic.wav";
                wmPlayer.settings.setMode("loop", true);
                if (DomainController.Instance().getMainMenuMusicStatus())
                    wmPlayer.controls.play();
                else
                    wmPlayer.controls.stop();
            }
            catch
            {

            }

            gameLobbyWindow.Show();
            gameInProgress = false;
        }

        /// <summary>
        /// Executed when a game is started.
        /// </summary>
        private void CnCNetData_OnGameStarted()
        {
            wmPlayer.controls.stop();
            gameLobbyWindow.Hide();
            gameInProgress = true;
        }

        /// <summary>
        /// Sets up settings related to the default game specified in DTACnCNetClient.ini.
        /// </summary>
        private void SetDefaultGame()
        {
            string defaultGame = DomainController.Instance().getDefaultGame();
            bool defaultGameSet = false;

            switch (defaultGame)
            {
                case "DTA":
                    chkChannelDTA.Enabled = false;
                    defaultGameSet = true;
                    cmbCurrentChannel.SelectedIndex = 0;
                    currentChannelId = 0;
                    myGame = "DTA";
                    break;
                case "TI":
                    chkChannelTI.Enabled = false;
                    defaultGameSet = true;
                    cmbCurrentChannel.SelectedIndex = 1;
                    currentChannelId = 1;
                    myGame = "TI";
                    break;
                case "TO":
                    chkChannelTO.Enabled = false;
                    defaultGameSet = true;
                    cmbCurrentChannel.SelectedIndex = 2;
                    currentChannelId = 2;
                    myGame = "TO";
                    break;
                case "TS":
                    chkChannelTS.Enabled = false;
                    chkChannelTS.Checked = true;
                    defaultGameSet = true;
                    cmbCurrentChannel.SelectedIndex = 2;
                    currentChannelId = 3;
                    myGame = "TS";
                    break;
                case "YR":
                    chkChannelDTA.Enabled = true;
                    chkChannelTI.Enabled = true;
                    chkChannelTO.Enabled = true;
                    chkChannelTS.Enabled = true;
                    defaultGameSet = true;
                    cmbCurrentChannel.SelectedIndex = 3;
                    currentChannelId = 4;
                    myGame = "YR";
                    break;
            }

            // If someone tries using the client for an unsupported game, let's
            // force them to join all channels used by supported games.
            if (!defaultGameSet)
            {
                chkChannelDTA.Enabled = false;
                chkChannelTI.Enabled = false;
                chkChannelTO.Enabled = false;
                chkChannelTS.Enabled = false;
            }
        }

        /// <summary>
        /// Handles the event generated by the CTCP GAME message, which is used for broadcasting.
        /// Forwards the message on to the real handler function; this is for thread-safety.
        /// </summary>
        /// <param name="sender">The name of the user who sent the message.</param>
        /// <param name="channelName">The name of the channel into which the message was sent.</param>
        /// <param name="message">The message itself.</param>
        private void Instance_OnCtcpGameParsed(string sender, string channelName, string message)
        {
            if (this.InvokeRequired)
            {
                CtcpGameCallback d = new CtcpGameCallback(Instance_DoCtcpGameParsed);
                this.BeginInvoke(d, sender, channelName, message);
                return;
            }

            Instance_DoCtcpGameParsed(sender, channelName, message);
        }

        /// <summary>
        /// Handles the CTCP GAME message, which is used for game broadcasting.
        /// </summary>
        /// <param name="sender">The name of the user who sent the message.</param>
        /// <param name="channelName">The name of the channel into which the message was sent.</param>
        /// <param name="message">The message itself.</param>
        private void Instance_DoCtcpGameParsed(string sender, string channelName, string message)
        {
            message = message.Substring(5); // Cut out GAME part
            string[] parsedMessage = message.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            if (parsedMessage.Length != 9)
            {
                Logger.Log("Ignoring CTCP game message because of an invalid amount of parameters.");
                return;
            }

            string revision = parsedMessage[0];
            if (revision != ProgramConstants.CNCNET_PROTOCOL_REVISION)
                return;
            string gameVersion = parsedMessage[1];
            int maxPlayers = Convert.ToInt32(parsedMessage[2]);
            string gameRoomChannelName = parsedMessage[3];
            string gameRoomDisplayName = parsedMessage[4];
            bool locked = Convert.ToBoolean(Convert.ToInt32(parsedMessage[5].Substring(0, 1)));
            bool isCustomPassword = Convert.ToBoolean(Convert.ToInt32(parsedMessage[5].Substring(1, 1)));
            bool isClosed = Convert.ToBoolean(Convert.ToInt32(parsedMessage[5].Substring(2, 1)));
            string[] players = parsedMessage[6].Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> playerNames = new List<string>();
            foreach (string player in players)
                playerNames.Add(player);
            string adminName = players[0];
            string mapName = parsedMessage[7];
            string gameMode = parsedMessage[8];

            string gameId = "unk";

            switch (channelName)
            {
                case "#dta-games":
                    gameId = "DTA";
                    break;
                case "#ti-games":
                    gameId = "TI";
                    break;
                case "#to-games":
                    gameId = "TO";
                    break;
                case "#ts-games":
                    gameId = "TS";
                    break;
                case "#yr-games":
                    gameId = "YR";
                    break;
            }

            if (gameId == "unk")
                return;

            Game game = new Game(gameRoomChannelName, revision, gameId, gameVersion, maxPlayers,
                gameRoomDisplayName, isCustomPassword, false, locked, true, false, false, players,
                players[0], mapName, gameMode);
            game.LastRefreshTime = DateTime.Now;

            if (CnCNetData.Games.Count == 0 && !isClosed)
            {
                if (gameId == myGame && !ProgramConstants.IsInGame)
                    sndGameCreated.Play();
                CnCNetData.Games.Add(game);
            }
            else
            {
                bool gameFound = false;

                for (int gId = 0; gId < CnCNetData.Games.Count; gId++)
                {
                    // Seek for the game in the internal game list based on its channel name;
                    // if found, then refresh that game's information
                    if (CnCNetData.Games[gId].ChannelName == gameRoomChannelName)
                    {
                        gameFound = true;
                        CnCNetData.Games.RemoveAt(gId);
                        if (!isClosed)
                            CnCNetData.Games.Insert(gId, game);
                        break;
                    }
                }

                if (!gameFound)
                {
                    // If the game wasn't found in our previous search, then add it to
                    // the internal game list as a new game

                    if (gameId == myGame && !ProgramConstants.IsInGame)
                        sndGameCreated.Play();
                    CnCNetData.Games.Add(game);
                }
            }

            RefreshGameList(null, EventArgs.Empty);
        }

        /// <summary>
        /// Refreshes the internal game list, applying statuses to the displayed names
        /// of all games and removing games that haven't been updated for too long by their authors.
        /// </summary>
        private void RefreshGameList(object sender, EventArgs e)
        {
            int sIndex = lbGameList.SelectedIndex;
            lbGameList.Items.Clear();
            GameColors.Clear();

            for (int gId = 0; gId < CnCNetData.Games.Count; gId++)
            {
                if (DateTime.Now - CnCNetData.Games[gId].LastRefreshTime > TimeSpan.FromSeconds(15.0))
                {
                    CnCNetData.Games.RemoveAt(gId);
                    if (sIndex == gId)
                        sIndex = -1;
                    else if (sIndex > gId)
                        sIndex--;
                }
            }

            CnCNetData.Games.OrderBy(g => g.Version == ProgramConstants.GAME_VERSION);
            CnCNetData.Games.OrderBy(g => g.GameIdentifier == myGame);
            CnCNetData.Games.OrderBy(g => !g.Started);

            foreach (Game game in CnCNetData.Games)
            {
                Color foreColor = lbGameList.ForeColor;

                string item = game.RoomName;

                if (game.Started)
                {
                    foreColor = cLockedGameColor;
                }
                else if (game.Version != ProgramConstants.GAME_VERSION && game.GameIdentifier == myGame)
                {
                    foreColor = cLockedGameColor;
                }

                GameColors.Add(foreColor);
                lbGameList.Items.Add(item);
            }

            if (sIndex >= lbGameList.Items.Count)
                lbGameList.SelectedIndex = -1;
            else
                lbGameList.SelectedIndex = sIndex;
            lbGameList_SelectedIndexChanged(sender, e);
        }

        /// <summary>
        /// Handles the "connection lost" event fired by the networking part of the client.
        /// </summary>
        /// <param name="errorMessage">The error message related to the connection error.</param>
        private void Instance_OnConnectionLost(string errorMessage)
        {
            if (this.InvokeRequired)
            {
                // Necessary for thread-safety
                StringCallback d = new StringCallback(Instance_OnConnectionLost);
                this.BeginInvoke(d, errorMessage);
                return;
            }

            Logger.Log("CnCNet connection lost: " + errorMessage);
            Logger.Log("Setting auto-login to false and saving settings.");
            ProgramConstants.CNCNET_AUTOLOGIN = false;
            DomainController.Instance().saveCnCNetSettings();

            MessageBox.Show("Your connection to the CnCNet server has been lost. " + Environment.NewLine + Environment.NewLine +
                "Error message: " + Environment.NewLine + errorMessage + Environment.NewLine + Environment.NewLine +
                "The CnCNet Client will now exit.", "CnCNet Server Connection Lost", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Environment.Exit(0);
        }

        /// <summary>
        /// Handles the AWAY message sent by an IRC server.
        /// </summary>
        /// <param name="userName">The name of the user who is away.</param>
        /// <param name="reason">The reason why the user is away.</param>
        private void Instance_OnAwayMessageReceived(string userName, string reason)
        {
            if (this.InvokeRequired)
            {
                // Necessary for thread-safety
                AwayMessageCallback d = new AwayMessageCallback(Instance_OnAwayMessageReceived);
                this.BeginInvoke(d, userName, reason);
                return;
            }

            int index = CnCNetData.PMInfos.FindIndex(c => c.UserName == userName);
            if (index == -1 || !CnCNetData.isPMWindowOpen)
            {
                MessageInfos[currentChannelId].Add(new MessageInfo(Color.White, userName + " is currently away: " + reason));
                AddChannelMessageToListBox(currentChannelId);
            }
        }

        /// <summary>
        /// Handles an ERROR message sent by the IRC server.
        /// </summary>
        /// <param name="message">The error the server gave us.</param>
        private void Instance_OnErrorReceived(string message)
        {
            if (this.InvokeRequired)
            {
                // Necessary for thread-safety
                StringCallback d = new StringCallback(Instance_OnErrorReceived);
                this.BeginInvoke(d, message);
                return;
            }

            Logger.Log("CnCNet error received: " + message);
            Logger.Log("Setting auto-login to false and saving settings.");
            ProgramConstants.CNCNET_AUTOLOGIN = false;
            DomainController.Instance().saveCnCNetSettings();

            MessageBox.Show(message, "Server disconnected", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Environment.Exit(0);
        }

        /// <summary>
        /// Executed when a private message is sent from the PrivateMessageForm.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="receiver">The receiver of the message.</param>
        private void Instance_PrivateMessageSent(string message, string receiver)
        {
            if (this.InvokeRequired)
            {
                // Necessary for thread-safety
                PrivateMessageSentCallback d = new PrivateMessageSentCallback(Instance_PrivateMessageSent);
                this.BeginInvoke(d, message, receiver);
                return;
            }

            int index = CnCNetData.PMInfos.FindIndex(c => c.UserName == receiver);
            if (index == -1)
            {
                PrivateMessageInfo pmInfo = new PrivateMessageInfo();
                pmInfo.UserName = receiver;
                pmInfo.Messages.Add(new MessageInfo(cDefaultChatColor, message));
                pmInfo.IsSelfSent.Add(true);
                CnCNetData.PMInfos.Add(pmInfo);
            }
            else
            {
                CnCNetData.PMInfos[index].Messages.Add(new MessageInfo(cDefaultChatColor, message));
                CnCNetData.PMInfos[index].IsSelfSent.Add(true);
            }

            if (!CnCNetData.isPMWindowOpen)
            {
                PrivateMessageForm pmWindow = new PrivateMessageForm(cDefaultChatColor, receiver);
                pmWindow.Show();
                CnCNetData.isPMWindowOpen = true;
            }

            CnCNetData.ConnectionBridge.SendMessage("PRIVMSG " + receiver + " " + message);
        }

        /// <summary>
        /// Executed when someone sends a private message to the user.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="sender">The sender of the message.</param>
        private void Instance_PrivateMessageParsed(string message, string sender)
        {
            if (this.InvokeRequired)
            {
                // Necessary for thread-safety
                PrivateMessageParsedCallback d = new PrivateMessageParsedCallback(Instance_PrivateMessageParsed);
                this.BeginInvoke(d, message, sender);
                return;
            }

            int index = CnCNetData.PMInfos.FindIndex(c => c.UserName == sender);
            if (index == -1)
            {
                // If the isn't in the private conversation list, add the user to it
                PrivateMessageInfo pmInfo = new PrivateMessageInfo();
                pmInfo.UserName = sender;
                pmInfo.Messages.Add(new MessageInfo(cPmOtherUserColor, message));
                pmInfo.IsSelfSent.Add(false);
                CnCNetData.PMInfos.Add(pmInfo);
            }
            else
            {
                // If we've talked with the user before, just add the message itself to the list of messages
                // we've had with that user
                CnCNetData.PMInfos[index].Messages.Add(new MessageInfo(cPmOtherUserColor, message));
                CnCNetData.PMInfos[index].IsSelfSent.Add(false);
            }

            if (!CnCNetData.isPMWindowOpen)
            {
                if (gameInProgress)
                {
                    // If a game is in progress, tell the user that they received a private message
                    // so they are aware of checking it out later
                    MessageInfos[currentChannelId].Add(new MessageInfo(Color.White,
                        "You've received a private message from " + sender));
                    AddChannelMessageToListBox(currentChannelId);
                }
                else
                {
                    // If a game isn't in progress, open the PrivateMessageForm
                    PrivateMessageForm pmWindow = new PrivateMessageForm(cPmOtherUserColor, sender);
                    pmWindow.Show();
                    CnCNetData.isPMWindowOpen = true;
                }
            }
        }

        /// <summary>
        /// Called when receiving a chat message to a channel sent by another user.
        /// </summary>
        /// <param name="channelName">The name of the channel which received the message.</param>
        /// <param name="message">The message itself.</param>
        /// <param name="sender">The sender of the message.</param>
        private void Instance_PrivmsgParsed(string channelName, string message, string sender)
        {
            if (this.InvokeRequired)
            {
                // Necessary for thread-safety
                PrivmsgParsedCallback d = new PrivmsgParsedCallback(Instance_PrivmsgParsed);
                this.BeginInvoke(d, channelName, message, sender);
                return;
            }

            // Handle /me
            bool hasAction = message.Contains("ACTION");
            if (hasAction)
            {
                message = message.Remove(0, 7);
                message = "====> " + sender + message;
                sender = String.Empty;

                // Replace Funky's game identifiers with real game names because
                // his sucky client only sends the game identifiers around
                message = message.Replace("new ra game", "new Red Alert game");
                message = message.Replace("new td game", "new Tiberian Dawn game");
                message = message.Replace("new d2 game", "new Dune 2000 game");

                for (int chId = 0; chId < ChannelNames.Count; chId++)
                {
                    if (channelName == ChannelNames[chId])
                    {
                        MessageInfos[chId].Add(new MessageInfo(cDefaultChatColor, message));
                        AddChannelMessageToListBox(chId);
                    }
                }

                return;
            }
            else
            {
                Color foreColor;

                // Color parsing
                if (message.Contains(Convert.ToString((char)03)))
                {
                    if (message.Length < 3)
                    {
                        foreColor = cDefaultChatColor;
                    }
                    else
                    {
                        string colorString = message.Substring(1, 2);
                        message = message.Remove(0, 3);
                        int colorIndex = -1;
                        // Try to parse message color info; if fails, use default color
                        bool success = Int32.TryParse(colorString, out colorIndex);
                        if (success && colorIndex < ChatColors.Count && colorIndex > -1)
                            foreColor = ChatColors[colorIndex];
                        else
                            foreColor = cDefaultChatColor;
                    }
                }
                else
                    foreColor = cDefaultChatColor;

                int channelIndex = ChannelNames.FindIndex(n => n == channelName);

                if (channelIndex > -1)
                {
                    MessageInfos[channelIndex].Add(new MessageInfo(foreColor, sender + ": " + message));
                    AddChannelMessageToListBox(channelIndex);
                }
            }
        }

        /// <summary>
        /// Run when a QUIT message is received and parsed.
        /// </summary>
        /// <param name="message">The name of the quitting user.</param>
        private void Instance_OnUserQuit(string message)
        {
            if (this.InvokeRequired)
            {
                // Necessary for thread-safety
                StringCallback d = new StringCallback(Instance_OnUserQuit);
                this.BeginInvoke(d, message);
                return;
            }

            string userName = message;

            bool updatePlayerList = false;

            if (chkChannelDTA.Checked)
            {
                RemoveUserFromChannel(userName, 0, " has quit.", out updatePlayerList);
            }

            if (chkChannelTI.Checked)
            {
                RemoveUserFromChannel(userName, 1, " has quit.", out updatePlayerList);
            }

            if (chkChannelTO.Checked)
            {
                RemoveUserFromChannel(userName, 2, " has quit.", out updatePlayerList);
            }

            if (chkChannelTS.Checked)
            {
                RemoveUserFromChannel(userName, 3, " has quit.", out updatePlayerList);
            }

            if (chkChannelCnCNet.Checked)
            {
                RemoveUserFromChannel(userName, 4, " has quit.", out updatePlayerList);
            }

            if (updatePlayerList)
            {
                UpdatePlayerList();
            }

            AddChannelMessageToListBox(currentChannelId);

            int index = CnCNetData.PMInfos.FindIndex(c => c.UserName == userName);
            if (index > -1)
            {
                CnCNetData.PMInfos[index].Messages.Add(new MessageInfo(Color.White, "has quit CnCNet."));
                CnCNetData.PMInfos[index].IsSelfSent.Add(false);
            }
        }

        /// <summary>
        /// Removes a leaving user from a channel. Ran when an user quits CnCNet or leaves a channel.
        /// </summary>
        /// <param name="userName">The name of the user who's leaving.</param>
        /// <param name="channelIndex">The index of the channel where the user left from.</param>
        /// <param name="message">A message shown to the user of the client,
        /// telling what the leaving user was exactly doing (quitting CnCNet or leaving).</param>
        /// <param name="updatePlayerList">Whether the list of players should be refreshed.</param>
        private void RemoveUserFromChannel(string userName, int channelIndex, string message, out bool updatePlayerList)
        {
            updatePlayerList = false;

            int userIndex = UserLists[channelIndex].IndexOf(userName);
            if (userIndex == -1)
            {
                userIndex = UserLists[channelIndex].IndexOf("@" + userName + " (Admin)");
            }

            if (userIndex > -1)
            {
                UserLists[channelIndex].RemoveAt(userIndex);
                UserColors[channelIndex].RemoveAt(userIndex);

                MessageInfos[channelIndex].Add(new MessageInfo(Color.White, userName + message));

                //if (currentChannelId == channelIndex)
                    updatePlayerList = true;
            }
        }

        /// <summary>
        /// Run when a channel topic message is received and parsed.
        /// </summary>
        /// <param name="channelName">The name of the channel which had its topic changed.</param>
        /// <param name="message">The topic itself.</param>
        private void Instance_TopicMessageParsed(string channelName, string message)
        {
            if (this.InvokeRequired)
            {
                // Necessary for thread-safety
                TopicMessageParsedCallback d = new TopicMessageParsedCallback(Instance_TopicMessageParsed);
                this.BeginInvoke(d, channelName, message);
                return;
            }

            int channelIndex = ChannelNames.FindIndex(n => n == channelName);

            if (channelIndex > -1)
            {
                MessageInfos[channelIndex].Add(new MessageInfo(Color.White, "Current channel topic: " + message));
                AddChannelMessageToListBox(channelIndex);
            }
        }

        /// <summary>
        /// Executed when an user leaves a channel.
        /// </summary>
        /// <param name="channelName">The internal name of the channel where the user left from.</param>
        /// <param name="userName">The name of the user who left the channel.</param>
        private void Instance_OnUserLeaveChannel(string channelName, string userName)
        {
            if (this.InvokeRequired)
            {
                // Necessary for thread-safety
                UserLeaveChannelCallback d = new UserLeaveChannelCallback(Instance_OnUserLeaveChannel);
                this.BeginInvoke(d, channelName, userName);
                return;
            }

            bool updatePlayerList = false;

            int channelIndex = ChannelNames.FindIndex(n => n == channelName);

            if (channelIndex > -1)
            {
                RemoveUserFromChannel(userName, channelIndex,
                    string.Format(" has left the {0} lobby.", ChannelUINames[channelIndex]),
                    out updatePlayerList);
            }

            if (updatePlayerList)
            {
                UpdatePlayerList();
            }
        }

        /// <summary>
        /// Executed when an user joins a channel.
        /// </summary>
        /// <param name="channelName">The name of the channel that the user joins to.</param>
        /// <param name="userName">The name of the user who joins the channel.</param>
        /// <param name="ipAddress">UNUSED! Reserved for containing the joining user's IP address
        /// but this client doesn't actually parse it since it's not necessary for tunneled CnCNet games.</param>
        private void Instance_OnUserJoinedChannel(string channelName, string userName, string ipAddress)
        {
            if (this.InvokeRequired)
            {
                // Necessary for thread-safety
                UserJoinChannelCallback d = new UserJoinChannelCallback(Instance_OnUserJoinedChannel);
                this.BeginInvoke(d, channelName, userName, ipAddress);
                return;
            }

            string name = userName;
            if (userName.StartsWith("@"))
            {
                name = name + " (Admin)";
            }

            bool updatePlayerList = false;
            bool channelFound = false;

            for (int chId = 0; chId < ChannelNames.Count; chId++)
            {
                if (ChannelNames[chId] == channelName)
                {
                    if (userName.StartsWith("@"))
                    {
                        UserLists[chId].Add(name);
                        UserColors[chId].Add(cAdminNameColor);
                    }
                    else
                    {
                        if (userName != ProgramConstants.CNCNET_PLAYERNAME)
                        {
                            UserLists[chId].Add(userName);
                            UserColors[chId].Add(cPlayerNameColor);
                        }
                    }

                    if (currentChannelId == chId)
                    {
                        // Update player list if necessary
                        updatePlayerList = true;
                    }

                    MessageInfos[chId].Add(new MessageInfo(Color.White, 
                        string.Format("{0} has joined the {1} lobby.", userName, ChannelUINames[chId])));
                    AddChannelMessageToListBox(chId);
                    channelFound = true;

                    break; // Optimization
                }
            }

            // If the channel the user joined to wasn't found, check if we just joined a game channel
            if (!channelFound && channelName == joinGameChannelName && userName == ProgramConstants.CNCNET_PLAYERNAME)
            {
                Game game = CnCNetData.Games.Find(c => c.ChannelName == channelName);
                if (game != null)
                {
                    Logger.Log("Joining game " + game.RoomName + "; channel name " + channelName);
                    CnCNetData.IsGameLobbyOpen = true;
                    gameLobbyWindow = new NGameLobby(channelName, false, game.MaxPlayers, game.Admin, game.RoomName, "",
                        false, ChatColors, cDefaultChatColor, cmbMessageColor.SelectedIndex + 2);
                    gameLobbyWindow.Show();
                    CnCNetData.ConnectionBridge.SendMessage("AWAY " + weirdChar1 + "In game [" + myGame.ToUpper() + "] " + game.RoomName);
                    this.WindowState = FormWindowState.Minimized;
                    joinGameChannelName = String.Empty;
                }
                else
                    Logger.Log("WARNING: No game data found for channel " + channelName);
            }

            if (updatePlayerList)
            {
                UpdatePlayerList();
            }
        }

        /// <summary>
        /// Executed when a channel user list is received from the IRC server.
        /// </summary>
        /// <param name="userNames">The list of user names on the channel in a string array.</param>
        /// <param name="channelName">The name of the channel for which we received the user list.</param>
        private void Instance_UserListReceived(string[] userNames, string channelName)
        {
            if (this.InvokeRequired)
            {
                // Necessary for thread-safety
                UserListCallback d = new UserListCallback(Instance_UserListReceived);
                this.BeginInvoke(d, userNames, channelName);
                return;
            }

            bool updatePlayerList = false;
            bool channelFound = false;

            int channelIndex = ChannelNames.FindIndex(n => n == channelName);
            
            if (channelIndex > -1)
            {
                foreach (string userName in userNames)
                {
                    if (CnCNetData.players.FindIndex(c => c == userName) == -1)
                        CnCNetData.players.Add(userName);

                    if (userName.StartsWith("@"))
                    {
                        string name = userName;
                        name = name + " (Admin)";
                        UserLists[channelIndex].Add(name);
                        UserColors[channelIndex].Add(cAdminNameColor);
                    }
                    else
                    {
                        UserLists[channelIndex].Add(userName);
                        UserColors[channelIndex].Add(cPlayerNameColor);
                    }
                }

                if (cmbCurrentChannel.SelectedIndex == channelIndex)
                    updatePlayerList = true;

                channelFound = true;
            }

            if (!channelFound)
            {
                DoUserListReceived(userNames, channelName);
            }
            else if (updatePlayerList)
            {
                UpdatePlayerList();
            }
        }

        /// <summary>
        /// Refreshes the list of players displayed in the UI.
        /// </summary>
        private void UpdatePlayerList()
        {
            List<string> Players = UserLists[currentChannelId];
            Players.Sort();
            lbPlayerList.Items.Clear();

            foreach (string playerName in Players)
            {
                if (!String.IsNullOrEmpty(playerName))
                    lbPlayerList.Items.Add(playerName);
            }

            ScrollPlayersListbox();
        }

        /// <summary>
        /// Refreshes the list of chat messages displayed in the UI.
        /// </summary>
        private void UpdateMessages()
        {
            lbChatMessages.Items.Clear();

            List<MessageInfo> msgInfos = MessageInfos[currentChannelId];

            foreach (MessageInfo msgInfo in msgInfos)
            {
                lbChatMessages.Items.Add("[" + msgInfo.Time.ToShortTimeString() + "] " + msgInfo.Message);
            }

            ScrollChatListbox();
        }

        /// <summary>
        /// Executed when the initial welcome message is received from the IRC server.
        /// The UI shouldn't be usable before this is run.
        /// </summary>
        /// <param name="message">The received welcome message.</param>
        private void Instance_WelcomeMessageParsed(string message)
        {
            if (this.InvokeRequired)
            {
                // Necessary for thread-safety
                StringCallback d = new StringCallback(Instance_WelcomeMessageParsed);
                this.BeginInvoke(d, message);
                return;
            }

            Logger.Log("Welcome message received.");
            welcomeMessageReceived = true;

            CnCNetData.ConnectionBridge.SendMessage("MODE " + ProgramConstants.CNCNET_PLAYERNAME + " +x");

            Instance_ServerMessageParsed(message);
            JoinInitialChannels();

            this.Text = string.Format("[{0}] CnCNet Lobby: {1} ({2} / {3})", myGame, 
                ProgramConstants.CNCNET_PLAYERNAME, ProgramConstants.GAME_VERSION, Application.ProductVersion);
            btnJoinGame.Enabled = true;
            btnNewGame.Enabled = true;
            btnSend.Enabled = true;

            DomainController.Instance().saveCnCNetSettings();

            this.TopMost = true;
            this.Focus();
            this.BringToFront();
            this.TopMost = false;
        }

        /// <summary>
        /// Joins the initial channel that the user is subscribed to.
        /// </summary>
        private void JoinInitialChannels()
        {
            if (chkChannelDTA.Checked)
            {
                CnCNetData.ConnectionBridge.SendMessage("JOIN #dta");
                CnCNetData.ConnectionBridge.SendMessage("JOIN #dta-games");
            }
            if (chkChannelTI.Checked)
            {
                CnCNetData.ConnectionBridge.SendMessage("JOIN #ti");
                CnCNetData.ConnectionBridge.SendMessage("JOIN #ti-games");
            }
            if (chkChannelTO.Checked)
            {
                CnCNetData.ConnectionBridge.SendMessage("JOIN #to");
                CnCNetData.ConnectionBridge.SendMessage("JOIN #to-games");
            }
            if (chkChannelTS.Checked)
            {
                CnCNetData.ConnectionBridge.SendMessage("JOIN #ts");
                CnCNetData.ConnectionBridge.SendMessage("JOIN #ts-games");
            }
            if (chkChannelCnCNet.Checked)
            {
                CnCNetData.ConnectionBridge.SendMessage("JOIN #cncnet");
            }

            if (myGame == "YR")
                CnCNetData.ConnectionBridge.SendMessage("JOIN #yr-games");
        }

        /// <summary>
        /// Adds a global "server-sent" message seen in all channels.
        /// </summary>
        /// <param name="message">The message received from the server.</param>
        private void Instance_ServerMessageParsed(string message)
        {
            if (cmbCurrentChannel.InvokeRequired)
            {
                // Necessary for thread-safety
                StringCallback d = new StringCallback(Instance_ServerMessageParsed);
                this.BeginInvoke(d, message);
                return;
            }

            MessageInfos[0].Add(new MessageInfo(Color.White, message));
            MessageInfos[1].Add(new MessageInfo(Color.White, message));
            MessageInfos[2].Add(new MessageInfo(Color.White, message));
            MessageInfos[3].Add(new MessageInfo(Color.White, message));
            MessageInfos[4].Add(new MessageInfo(Color.White, message));

            AddChannelMessageToListBox(cmbCurrentChannel.SelectedIndex);
        }

        /// <summary>
        /// Adds a chat message to the chat message listbox if the message belongs
        /// to the current channel.
        /// </summary>
        /// <param name="channel">The ID of the channel that received the message.</param>
        private void AddChannelMessageToListBox(int channel)
        {
            if (channel != currentChannelId)
            {
                return;
            }

            List<MessageInfo> messageInfos = MessageInfos[channel];

            for (int index = lbChatMessages.Items.Count; index < messageInfos.Count; index++)
            {
                lbChatMessages.Items.Add("[" + messageInfos[index].Time.ToShortTimeString() + "] " + messageInfos[index].Message);
            }

            lbChatMessages.SelectedIndex = lbChatMessages.Items.Count - 1;
            lbChatMessages.SelectedIndex = -1;
            ScrollChatListbox();
        }

        /// <summary>
        /// Loads user settings (subscribed channels).
        /// </summary>
        private void LoadSettings()
        {
            if (myGame == "YR")
            {
                chkChannelDTA.Checked = false;
                chkChannelTI.Checked = false;
                chkChannelTO.Checked = false;
                chkChannelTS.Checked = false;
                chkChannelCnCNet.Checked = true;
                return;
            }

            if (chkChannelDTA.Enabled)
            {
                chkChannelDTA.Checked = DomainController.Instance().getChannelDTAEnabledStatus();
            }

            if (chkChannelTI.Enabled)
            {
                chkChannelTI.Checked = DomainController.Instance().getChannelTIEnabledStatus();
            }

            //if (chkChannelTO.Enabled)
            //{
            //    if (DomainController.Instance().getChannelTOEnabledStatus())
            //        chkChannelTO.Checked = true;
            //}

            if (chkChannelTS.Enabled)
            {
                chkChannelTS.Checked = DomainController.Instance().getChannelTSEnabledStatus();
            }

            chkChannelCnCNet.Checked = DomainController.Instance().getChannelCnCNetEnabledStatus();
        }

        /// <summary>
        /// Executed when the user clicks the "Return to Main Menu" button.
        /// </summary>
        private void btnReturnToMenu_Click(object sender, EventArgs e)
        {
            if (btnNewGame.Enabled)
                SaveSettings();
            CnCNetData.ConnectionBridge.SendMessage("QUIT");
            Application.DoEvents();
            Environment.Exit(0);
        }

        /// <summary>
        /// Saves user settings.
        /// </summary>
        private void SaveSettings()
        {
            Logger.Log("Saving settings.");

            DomainController.Instance().saveCnCNetColorSetting(cmbMessageColor.SelectedIndex);
        }

        /// <summary>
        /// Executed when the "Hide Followed Channels and Games" "toggle-button" is pressed.
        /// </summary>
        private void btnHideChannels_Click(object sender, EventArgs e)
        {
            sp.Play();

            this.SuspendLayout();
            if (panel1.Visible)
            {
                panel1.Visible = false;
                lbPlayerList.Height = lbChatMessages.Height - 20;
                lbPlayerList.Location = new Point(lbPlayerList.Location.X, lbChatMessages.Location.Y + 20);
                lblPlayerList.Location = new Point(lbPlayerList.Location.X, lbChatMessages.Location.Y);
                btnHideChannels.BackgroundImage = btnHideChannelsDown;
            }
            else
            {
                lbPlayerList.Height = this.Height - 255;
                lbPlayerList.Location = new Point(lbPlayerList.Location.X, panel1.Location.Y + panel1.Size.Height + 27);
                lblPlayerList.Location = new Point(lbPlayerList.Location.X, lbPlayerList.Location.Y - 20);
                panel1.Visible = true;
                btnHideChannels.BackgroundImage = btnHideChannelsUp;
            }

            sbPlayers.Height = lbPlayerList.Height;
            sbPlayers.Location = new Point(sbPlayers.Location.X, lbPlayerList.Location.Y);

            this.ResumeLayout();
        }

        /// <summary>
        /// Used for automatically scaling UI components as the window's size is changed.
        /// TODO: Is this actually necessary with anchors properly set?
        /// </summary>
        private void NCnCNetLobby_SizeChanged(object sender, EventArgs e)
        {
            this.SuspendLayout();

            if (!panel1.Visible)
            {
                lbPlayerList.Height = lbChatMessages.Height - 20;
            }
            else
            {
                lbPlayerList.Height = this.Height - 235;
            }

            lbChatMessages.Refresh();
            this.ResumeLayout();
        }

        /// <summary>
        /// Executed when the "Dawn of the Tiberium Age" channel checkbox is (un)checked.
        /// </summary>
        private void chkChannelDTA_CheckedChanged()
        {
            if (!welcomeMessageReceived)
                return;

            if (chkChannelDTA.Checked)
            {
                CnCNetData.ConnectionBridge.SendMessage("JOIN #dta");
                CnCNetData.ConnectionBridge.SendMessage("JOIN #dta-games");
                AddChannelMessageToListBox(0);
            }
            else
            {
                CnCNetData.ConnectionBridge.SendMessage("PART #dta");
                CnCNetData.ConnectionBridge.SendMessage("PART #dta-games");
                UserLists[0].Clear();
                UserColors[0].Clear();
            }
        }

        /// <summary>
        /// Executed when the "Twisted Insurrection" channel checkbox is (un)checked.
        /// </summary>
        private void chkChannelTI_CheckedChanged()
        {
            if (!welcomeMessageReceived)
                return;

            if (chkChannelTI.Checked)
            {
                CnCNetData.ConnectionBridge.SendMessage("JOIN #ti");
                CnCNetData.ConnectionBridge.SendMessage("JOIN #ti-games");
                AddChannelMessageToListBox(1);
            }
            else
            {
                CnCNetData.ConnectionBridge.SendMessage("PART #ti");
                CnCNetData.ConnectionBridge.SendMessage("PART #ti-games");
                UserLists[1].Clear();
                UserColors[1].Clear();
            }
        }

        /// <summary>
        /// Executed when the "Tiberian Sun" channel checkbox is (un)checked.
        /// </summary>
        private void chkChannelTS_CheckedChanged()
        {
            if (!welcomeMessageReceived)
                return;

            if (chkChannelTS.Checked)
            {
                CnCNetData.ConnectionBridge.SendMessage("JOIN #ts");
                CnCNetData.ConnectionBridge.SendMessage("JOIN #ts-games");
                AddChannelMessageToListBox(3);
            }
            else
            {
                CnCNetData.ConnectionBridge.SendMessage("PART #ts");
                CnCNetData.ConnectionBridge.SendMessage("PART #ts-games");
                UserLists[3].Clear();
                UserColors[3].Clear();
            }
        }

        /// <summary>
        /// Executed when the "General CnCNet Chat" channel checkbox is (un)checked.
        /// </summary>
        private void chkChannelCnCNet_CheckedChanged()
        {
            if (!welcomeMessageReceived)
                return;

            if (chkChannelCnCNet.Checked)
            {
                CnCNetData.ConnectionBridge.SendMessage("JOIN #cncnet");
                AddChannelMessageToListBox(4);
            }
            else
            {
                CnCNetData.ConnectionBridge.SendMessage("PART #cncnet");
                UserLists[4].Clear();
                UserColors[4].Clear();
            }
        }

        /// <summary>
        /// Executed when the user switches the active channel via the Current Chat Channel list.
        /// </summary>
        private void cmbCurrentChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbCurrentChannel.SelectedIndex)
            {
                case 0:
                    if (!chkChannelDTA.Checked)
                    {
                        chkChannelDTA.Checked = true;
                        MessageInfos[0].Add(new MessageInfo(Color.White, "Attempting to join The Dawn of the Tiberium Age's channel..."));
                    }
                    break;
                case 1:
                    if (!chkChannelTI.Checked)
                    {
                        chkChannelTI.Checked = true;
                        MessageInfos[1].Add(new MessageInfo(Color.White, "Attempting to join Twisted Insurrection's channel..."));
                    }
                    break;
                //case 2:
                //    if (!chkChannelTO.Checked)
                //    {
                //        chkChannelTO.Checked = true;
                //        MessageInfos[2].Add(new MessageInfo(Color.White, "Attempting to join Tiberian Odyssey's channel..."));
                //    }
                //    break;
                case 2:
                    if (!chkChannelTS.Checked)
                    {
                        chkChannelTS.Checked = true;
                        MessageInfos[3].Add(new MessageInfo(Color.White, "Attempting to join Tiberian Sun's channel..."));
                    }
                    break;
                case 3:
                    if (!chkChannelCnCNet.Checked)
                    {
                        chkChannelCnCNet.Checked = true;
                        MessageInfos[4].Add(new MessageInfo(Color.White, "Attempting to join the General CnCNet Chat channel..."));
                    }
                    break;
            }

            // Hack to hide TO
            if (cmbCurrentChannel.SelectedIndex > 1)
                currentChannelId = cmbCurrentChannel.SelectedIndex + 1;
            else
                currentChannelId = cmbCurrentChannel.SelectedIndex;

            UpdatePlayerList();
            UpdateMessages();
        }

        /// <summary>
        /// Used for measuring the size of drawn chat messages.
        /// </summary>
        private void lbChatMessages_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = (int)e.Graphics.MeasureString(lbChatMessages.Items[e.Index].ToString(),
                lbChatMessages.Font, lbChatMessages.Width).Height;
            e.ItemWidth = lbChatMessages.Width;
        }

        /// <summary>
        /// Used for manually drawing chat messages in the chat message list box.
        /// </summary>
        private void lbChatMessages_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index > -1 && e.Index < lbChatMessages.Items.Count)
            {
                Color foreColor;

                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                    e = new DrawItemEventArgs(e.Graphics,
                                              e.Font,
                                              e.Bounds,
                                              e.Index,
                                              e.State ^ DrawItemState.Selected,
                                              e.ForeColor,
                                              cListBoxFocusColor);

                e.DrawBackground();
                e.DrawFocusRectangle();

                if (MessageInfos[currentChannelId].Count <= e.Index)
                    foreColor = Color.White;
                else
                    foreColor = MessageInfos[currentChannelId][e.Index].Color;
                e.Graphics.DrawString(lbChatMessages.Items[e.Index].ToString(), e.Font, new SolidBrush(foreColor), e.Bounds);
            }
        }

        /// <summary>
        /// Used for manually drawing items of the "Your Chat Color" combo box.
        /// </summary>
        private void cmbMessageColor_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            if (e.Index > -1 && e.Index < cmbMessageColor.Items.Count)
                e.Graphics.DrawString(cmbMessageColor.Items[e.Index].ToString(), e.Font, new SolidBrush(ChatColors[e.Index + 2]), e.Bounds);
        }

        /// <summary>
        /// Used for measuring the size of items in the player list box.
        /// </summary>
        private void lbPlayerList_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = (int)e.Graphics.MeasureString(lbPlayerList.Items[e.Index].ToString(),
                lbPlayerList.Font, lbPlayerList.Width).Height;
        }
        
        /// <summary>
        /// Used for manually drawing items of the player list box.
        /// </summary>
        private void lbPlayerList_DrawItem(object sender, DrawItemEventArgs e)
        {
            int currentChannel = cmbCurrentChannel.SelectedIndex;

            if (e.Index > -1 && e.Index < lbPlayerList.Items.Count)
            {
                Color foreColor;

                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                    e = new DrawItemEventArgs(e.Graphics,
                                              e.Font,
                                              e.Bounds,
                                              e.Index,
                                              e.State ^ DrawItemState.Selected,
                                              e.ForeColor,
                                              cListBoxFocusColor);

                e.DrawBackground();
                e.DrawFocusRectangle();

                string nameToDraw = lbPlayerList.Items[e.Index].ToString();

                if (nameToDraw[0] == '@')
                    foreColor = cAdminNameColor;
                else
                    foreColor = cPlayerNameColor;

                e.Graphics.DrawString(lbPlayerList.Items[e.Index].ToString(), e.Font, new SolidBrush(foreColor), e.Bounds);
            }
        }

        /// <summary>
        /// Executed when the form is closed.
        /// </summary>
        private void NCnCNetLobby_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// Sends a chat message to the IRC server.
        /// </summary>
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbChatInput.Text))
                return;

            if (tbChatInput.Text.StartsWith("/"))
            {
                // Allow sending raw commands to the server with /

                string messageToSend = tbChatInput.Text.Remove(0, 1);

                string[] messageParts = messageToSend.Split(' ');

                if (messageParts[0] == "PRIVMSG")
                {
                    // Let's send a private message to a user if a matching one is found
                    string receiver = messageParts[1];
                    int index = receiver.IndexOf(',');
                    if (index > -1)
                        receiver = receiver.Substring(0, index);

                    int pIndex = CnCNetData.players.FindIndex(c => c == receiver);
                    if (pIndex > -1)
                        Instance_PrivateMessageSent(messageParts[2], receiver);
                }
                else if (messageParts[0] == "JOIN")
                {
                    // Prevent joining channels since doing that could mess up the client's internal workings
                    MessageInfos[currentChannelId].Add(new MessageInfo(Color.White, "Please join channels using the Join Game button."));
                    AddChannelMessageToListBox(currentChannelId);
                    return;
                }

                CnCNetData.ConnectionBridge.SendMessage(messageToSend);
                tbChatInput.Text = String.Empty;
            }
            else
            {
                string channel = ChannelNames[currentChannelId];
                int colorId = cmbMessageColor.SelectedIndex + 2;
                string colorString = Convert.ToString((char)03);
                if (colorId < 10)
                    colorString = colorString + "0" + Convert.ToString(colorId);
                else
                    colorString = colorString + Convert.ToString(colorId);

                string messageToSend = "PRIVMSG " + channel + " " + colorString + tbChatInput.Text;
                CnCNetData.ConnectionBridge.SendMessage(messageToSend);
                MessageInfos[currentChannelId].Add(new MessageInfo(ChatColors[colorId], ProgramConstants.CNCNET_PLAYERNAME + ": " + tbChatInput.Text));
                AddChannelMessageToListBox(currentChannelId);
                tbChatInput.Text = String.Empty;
            }
        }

        /// <summary>
        /// Opens the private message window when the user double-clicks on a player's
        /// name in the player list box.
        /// </summary>
        private void lbPlayerList_DoubleClick(object sender, EventArgs e)
        {
            if (lbPlayerList.SelectedIndex == -1)
                return;

            string userName = lbPlayerList.Items[lbPlayerList.SelectedIndex].ToString();
            if (userName[0] == '@')
                userName = userName.Remove(0, 1);

            if (userName.Contains(" (Admin)"))
                userName = userName.Substring(0, userName.Length - 8);

            // Let the user send PMs by doubleclicking players in the player list
            if (!CnCNetData.isPMWindowOpen)
            {
                PrivateMessageForm pmForm = new PrivateMessageForm(cDefaultChatColor, userName);
                pmForm.Show();
                CnCNetData.isPMWindowOpen = true;
            }
            else
            {
                DoConversationOpened(userName);
            }
        }

        /// <summary>
        /// Executed whenever the player list's selected index is changed
        /// (usually because the user clicks on a player's name).
        /// </summary>
        private void lbPlayerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbPlayerList.SelectedIndex == -1)
                return;

            if (!requestWhois)
                return;

            string userName = lbPlayerList.Items[lbPlayerList.SelectedIndex].ToString();
            if (userName[0] == '@')
                userName = userName.Remove(0, 1);

            if (userName.Contains(" (Admin)"))
                userName = userName.Substring(0, userName.Length - 8);

            CnCNetData.ConnectionBridge.SendMessage("WHOIS " + userName);
        }

        private void btnNewGame_MouseEnter(object sender, EventArgs e)
        {
            btnNewGame.BackgroundImage = btn92px_c;
            sp.Play();
        }

        private void btnJoinGame_MouseEnter(object sender, EventArgs e)
        {
            btnJoinGame.BackgroundImage = btn92px_c;
            sp.Play();
        }

        private void btnNewGame_MouseLeave(object sender, EventArgs e)
        {
            btnNewGame.BackgroundImage = btn92px;
        }

        private void btnJoinGame_MouseLeave(object sender, EventArgs e)
        {
            btnJoinGame.BackgroundImage = btn92px;
        }

        private void btnReturnToMenu_MouseEnter(object sender, EventArgs e)
        {
            btnReturnToMenu.BackgroundImage = btn121px_c;
            sp.Play();
        }

        private void btnReturnToMenu_MouseLeave(object sender, EventArgs e)
        {
            btnReturnToMenu.BackgroundImage = btn121px;
        }

        /// <summary>
        /// Lets the user host a new game.
        /// </summary>
        private void btnNewGame_Click(object sender, EventArgs e)
        {
            if (CnCNetData.IsGameLobbyOpen)
            {
                // Prevent the user from hosting a game if they already are in a game
                // and flash the game lobby window
                WindowFlasher.FlashWindowEx(gameLobbyWindow);
                MessageInfos[currentChannelId].Add(new MessageInfo(Color.White, "You already are in a game!"));
                AddChannelMessageToListBox(currentChannelId);
                return;
            }

            CreateGameForm cgf = new CreateGameForm();
            DialogResult dr = cgf.ShowDialog();

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                string channelName = randomizeChannelName();
                CnCNetData.IsGameLobbyOpen = true;
                AddChannelMessageToListBox(currentChannelId);

                bool customPassword = true;
                string password = cgf.rtnPassword;
                if (password == String.Empty)
                {
                    // If the user didn't enter a custom password, generate one
                    // following CnCNet protocol specification
                    customPassword = false;
                    password = Utilities.calculateMD5ForBytes(Encoding.Unicode.GetBytes(cgf.rtnGameRoomName)).Substring(0, 10);
                }

                for (int gId = 0; gId < CnCNetData.Games.Count; gId++)
                {
                    if (CnCNetData.Games[gId].RoomName == cgf.rtnGameRoomName)
                    {
                        MessageBox.Show("A game room with the specified name already exists.");
                        cgf.Dispose();
                        return;
                    }
                }

                Logger.Log("Creating a game named " + cgf.rtnGameRoomName + "; channel name " + channelName);

                gameLobbyWindow = new NGameLobby(channelName, true, cgf.rtnMaxPlayers, ProgramConstants.CNCNET_PLAYERNAME, cgf.rtnGameRoomName, password,
                    customPassword, ChatColors, cDefaultChatColor, cmbMessageColor.SelectedIndex + 2);
                gameLobbyWindow.Show();
                CnCNetData.ConnectionBridge.SendMessage("JOIN " + channelName);
                gameLobbyWindow.Initialize(cgf.rtnTunnelAddress, cgf.rtnTunnelPort);
                CnCNetData.ConnectionBridge.SendMessage("AWAY " + weirdChar1 + "In game [" + myGame.ToUpper() + "] " + cgf.rtnGameRoomName);
                BroadcastGameCreation();
                this.WindowState = FormWindowState.Minimized;
                cgf.Dispose();
            }
        }

        /// <summary>
        /// Generates and returns a random, unused cannel name.
        /// </summary>
        /// <returns>A random channel name based on the currently played game.</returns>
        private string randomizeChannelName()
        {
            while (true)
            {
                string channelName = "#" + myGame + "-game" + new Random().Next(100000, 999999);
                int index = CnCNetData.Games.FindIndex(c => c.ChannelName == channelName);
                if (index == -1)
                    return channelName;
            }
        }

        /// <summary>
        /// Broadcasts game creation to let others know that the user hosted a game.
        /// </summary>
        private void BroadcastGameCreation()
        {
            string gameName = "The Dawn of the Tiberium Age";
            switch (myGame)
            {
                case "DTA":
                    gameName = "The Dawn of the Tiberium Age";
                    break;
                case "TI":
                    gameName = "Twisted Insurrection";
                    break;
                case "TO":
                    gameName = "Tiberian Odyssey";
                    break;
                case "TS":
                    gameName = "Tiberian Sun";
                    break;
                case "YR":
                    gameName = "Yuri's Revenge";
                    break;
            }

            if (chkChannelDTA.Checked)
            {
                CnCNetData.ConnectionBridge.SendMessage("PRIVMSG #dta " + weirdChar1 + weirdChar2 + "ACTION has hosted a new " + gameName + " game." + weirdChar2);
                MessageInfos[0].Add(new MessageInfo(cDefaultChatColor, "====> " + ProgramConstants.CNCNET_PLAYERNAME + " has hosted a new " + gameName + " game."));
            }

            if (chkChannelTI.Checked)
            {
                CnCNetData.ConnectionBridge.SendMessage("PRIVMSG #ti " + weirdChar1 + weirdChar2 + "ACTION has hosted a new " + gameName + " game." + weirdChar2);
                MessageInfos[1].Add(new MessageInfo(cDefaultChatColor, "====> " + ProgramConstants.CNCNET_PLAYERNAME + " has hosted a new " + gameName + " game."));
            }

            if (chkChannelTO.Checked)
            {
                CnCNetData.ConnectionBridge.SendMessage("PRIVMSG #to " + weirdChar1 + weirdChar2 + "ACTION has hosted a new " + gameName + " game." + weirdChar2);
                MessageInfos[2].Add(new MessageInfo(cDefaultChatColor, "====> " + ProgramConstants.CNCNET_PLAYERNAME + " has hosted a new " + gameName + " game."));
            }

            if (chkChannelTS.Checked)
            {
                CnCNetData.ConnectionBridge.SendMessage("PRIVMSG #ts " + weirdChar1 + weirdChar2 + "ACTION has hosted a new " + gameName + " game." + weirdChar2);
                MessageInfos[3].Add(new MessageInfo(cDefaultChatColor, "====> " + ProgramConstants.CNCNET_PLAYERNAME + " has hosted a new " + gameName + " game."));
            }

            if (chkChannelCnCNet.Checked)
            {
                CnCNetData.ConnectionBridge.SendMessage("PRIVMSG #cncnet " + weirdChar1 + weirdChar2 + "ACTION has hosted a new " + gameName + " game." + weirdChar2);
                MessageInfos[4].Add(new MessageInfo(cDefaultChatColor, "====> " + ProgramConstants.CNCNET_PLAYERNAME + " has hosted a new " + gameName + " game."));
            }

            AddChannelMessageToListBox(currentChannelId);
        }

        /// <summary>
        /// Updates game information displayed in the UI.
        /// Called whenever the selected index of lbGameList is changed.
        /// </summary>
        private void lbGameList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbGameList.SelectedIndex == -1)
            {
                // No game selected - clear info

                lblPassworded.Text = "Requires Password: -";
                lblMapName.Text = "Map: -";
                lblGameMode.Text = "Game Mode: -";
                lblVersion.Text = "Game Version: -";
                lblStarted.Text = "Locked: -";
                lblHost.Text = "Host: -";
                lblPlayers.Text = "Players (- / -):";
                lblPlayersValue.Text = String.Empty;
            }
            else
            {
                // Game selected - fill info

                Game game = CnCNetData.Games[lbGameList.SelectedIndex];
                if (game.Passworded)
                    lblPassworded.Text = "Requires Password: Yes";
                else
                    lblPassworded.Text = "Requires Password: No";

                lblGameMode.Text = "Game Mode: " + game.GameMode;
                lblMapName.Text = "Map: " + game.MapName;

                if (game.GameIdentifier == myGame)
                {
                    if (game.Version == ProgramConstants.GAME_VERSION)
                        lblVersion.Text = "Game Version: " + game.Version + " (compatible)";
                    else
                        lblVersion.Text = "Game Version: " + game.Version + " (incompatible)";
                }
                else
                    lblVersion.Text = "Game Version: " + game.Version;

                if (game.Started)
                    lblStarted.Text = "Locked: Yes";
                else
                    lblStarted.Text = "Locked: No";

                lblHost.Text = "Host: " + game.Admin;

                lblPlayers.Text = "Players (" + game.Players.Count + " / " + game.MaxPlayers + ") :";
                lblPlayersValue.Text = String.Empty;
                foreach (string playerName in game.Players)
                    lblPlayersValue.Text = lblPlayersValue.Text + playerName + Environment.NewLine;
            }
        }

        /// <summary>
        /// Executed when the user attempts to join a game.
        /// </summary>
        private void btnJoinGame_Click(object sender, EventArgs e)
        {
            if (lbGameList.SelectedIndex == -1)
            {
                MessageInfos[currentChannelId].Add(new MessageInfo(Color.White, "No game selected!"));
                AddChannelMessageToListBox(currentChannelId);
                return;
            }

            if (CnCNetData.IsGameLobbyOpen)
            {
                WindowFlasher.FlashWindowEx(gameLobbyWindow);
                MessageInfos[currentChannelId].Add(new MessageInfo(Color.White, "You cannot join a game while already being in a game!"));
                AddChannelMessageToListBox(currentChannelId);
                return;
            }

            Game game = CnCNetData.Games[lbGameList.SelectedIndex];

            if (game.Started)
            {
                MessageInfos[currentChannelId].Add(new MessageInfo(Color.White, "The selected game is locked!"));
                AddChannelMessageToListBox(currentChannelId);
                return;
            }

            if (game.GameIdentifier != myGame)
            {
                // If the game we're trying to join is for a different game, let's
                // check if the other game is installed and if yes, then launch that
                // game's client; otherwise prompt the user for installation
                // DTA and TI multiplayer affiliation \ o /

                string installPath = getInstallPath(game.GameIdentifier);

                if (!String.IsNullOrEmpty(installPath))
                {
                    ClientSwitchConfirmation(game.GameIdentifier, installPath);
                    return;
                }
                else
                {
                    InstallConfirmation(game.GameIdentifier);
                    return;
                }
            }

            if (game.Passworded)
            {
                // Query the user for the game's password
                PasswordQueryForm pqf = new PasswordQueryForm();
                DialogResult dr = pqf.ShowDialog();

                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    string password = pqf.rtnPassword;
                    pqf.Dispose();
                    joinGameChannelName = game.ChannelName;
                    CnCNetData.ConnectionBridge.SendMessage("JOIN " + game.ChannelName + " " + password);
                }
                else
                    pqf.Dispose();
            }
            else
            {
                // If the selected game doesn't have a custom password, let's generate one
                // following the CnCNet protocol specification
                string password = Utilities.calculateMD5ForBytes(Encoding.Unicode.GetBytes(game.RoomName)).Substring(0, 10);
                joinGameChannelName = game.ChannelName;
                CnCNetData.ConnectionBridge.SendMessage("JOIN " + game.ChannelName + " " + password);
            }
        }

        /// <summary>
        /// Gets the installation path for a supported game.
        /// Returns an empty string if the specified game isn't installed.
        /// </summary>
        /// <param name="gameId">The ID of the game.</param>
        /// <returns>The installation path of the game. An empty string if the game isn't found.</returns>
        private string getInstallPath(string gameId)
        {
            Logger.Log(string.Format("Detecting whether {0} is installed.", gameId));

            Microsoft.Win32.RegistryKey key;
            switch (gameId)
            {
                case "DTA":
                    key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\TheDawnOfTheTiberiumAge");
                    break;
                case "TI":
                    key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\TwistedInsurrection");
                    break;
                case "TO":
                    key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\TiberianOdyssey");
                    break;
                case "TS":
                    key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\TiberianSun");
                    break;
                default:
                    return String.Empty;
            }

            if (key == null)
                return String.Empty;

            object value = key.GetValue("InstallPath");
            if (value == null)
                return String.Empty;

            string valueString = (string)value;

            if (System.IO.Directory.Exists(valueString))
                return valueString;

            return String.Empty;
        }

        /// <summary>
        /// Asks the user if they want to switch the client for a different game.
        /// If the confirmation is positive, then starts the other game's client and exits.
        /// </summary>
        /// <param name="gameId">The internal ID (short name) of the supported game.</param>
        /// <param name="installPath">The installation directory of the selected game.</param>
        private void ClientSwitchConfirmation(string gameId, string installPath)
        {
            string fullName = String.Empty;
            string executableName = String.Empty;

            switch (gameId)
            {
                case "DTA":
                    fullName = "The Dawn of the Tiberium Age";
                    executableName = "DTA.exe";
                    break;
                case "TI":
                    fullName = "Twisted Insurrection";
                    executableName = "TI_Launcher.exe";
                    break;
                case "TO":
                    fullName = "Tiberian Odyssey";
                    executableName = "TiberianOdyssey.exe";
                    break;
                case "TS":
                    fullName = "Tiberian Sun";
                    executableName = "TiberianSun.exe";
                    break;
                default:
                    fullName = "Unknown Game " + gameId;
                    break;
            }

            DialogResult dr = MessageBox.Show(string.Format(
                "The selected game is for {0}. Do you wish to switch to the {0} client in order to join the game?", fullName),
                string.Format("{0} game", fullName), MessageBoxButtons.YesNo);

            if (dr == DialogResult.No)
            {
                return;
            }

            ProcessStartInfo startInfo = new ProcessStartInfo(installPath + executableName);
            startInfo.Arguments = "-RUNCLIENT -NOUACPOPUP";
            startInfo.WorkingDirectory = installPath;
            Process process = new Process();
            process.StartInfo = startInfo;

            Logger.Log(string.Format("Starting the {0} client and exiting.", fullName));

            SaveSettings();

            process.Start();

            CnCNetData.ConnectionBridge.SendMessage("QUIT");

            // Set exit code which the main client can read and quit accordingly
            Environment.Exit(1337);
        }

        /// <summary>
        /// Asks the user if they want to install a different supported game than what is
        /// currently being run.
        /// </summary>
        /// <param name="gameId">The internal ID (short name) of the supported game.</param>
        private void InstallConfirmation(string gameId)
        {
            string fullName = String.Empty;
            string pageUrl = String.Empty;

            switch (gameId)
            {
                case "DTA":
                    fullName = "The Dawn of the Tiberium Age";
                    pageUrl = "http://www.moddb.com/mods/the-dawn-of-the-tiberium-age";
                    break;
                case "TI":
                    fullName = "Twisted Insurrection";
                    pageUrl = "http://www.moddb.com/mods/twisted-insurrection";
                    break;
                case "TO":
                    fullName = "Tiberian Odyssey";
                    pageUrl = "http://www.moddb.com/mods/tiberian-odyssey";
                    break;
                case "TS":
                    fullName = "Tiberian Sun";
                    pageUrl = "http://www.moddb.com/mods/the-dawn-of-the-tiberium-age";
                    break;
                default:
                    fullName = "Unknown Game " + gameId;
                    Logger.Log(string.Format("InstallConfirmation: Unknown gameId {0}", gameId));
                    return;
            }

            Logger.Log(string.Format("Offering the installation of {0} to the user.", gameId));

            DialogResult dr = MessageBox.Show(string.Format("The selected game room is for {0}." +
                Environment.NewLine + Environment.NewLine +
                "Would you like to visit the home page of {0} where you can download and install the it?", fullName),
                string.Format("{0} game", fullName), MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dr == DialogResult.No)
                return;

            Logger.Log(string.Format("Opening the home page of {0}.", gameId));

            ParameterizedThreadStart ts = new ParameterizedThreadStart(OpenPage);
            Thread thread = new Thread(ts);
            thread.Start(pageUrl);
        }

        /// <summary>
        /// Opens a URL in the default web browser.
        /// </summary>
        /// <param name="url">The URL to open.</param>
        private void OpenPage(object url)
        {
            string sUrl = (string)url;

            Process.Start(sUrl);
        }

        /// <summary>
        /// Executed when the user changes their chat color via cmbMessageColor.
        /// Used to transfer the new color to the possibly running game lobby.
        /// </summary>
        private void cmbMessageColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            DoColorChanged(cmbMessageColor.SelectedIndex + 2);
        }

        /// <summary>
        /// Makes it possible for an user to join a game by double-clicking on it.
        /// </summary>
        private void lbGameList_DoubleClick(object sender, EventArgs e)
        {
            btnJoinGame.PerformClick();
        }

        private void btnMusicToggle_MouseEnter(object sender, EventArgs e)
        {
            btnMusicToggle.BackgroundImage = btn92px_c;
            sp.Play();
        }

        private void btnMusicToggle_MouseLeave(object sender, EventArgs e)
        {
            btnMusicToggle.BackgroundImage = btn92px;
        }

        /// <summary>
        /// Enables the user to toggle the music on and off.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMusicToggle_Click(object sender, EventArgs e)
        {
            if (wmPlayer.playState == WMPPlayState.wmppsPlaying)
            {
                wmPlayer.controls.stop();
                DomainController.Instance().saveLobbyMusicSettings(false);
                btnMusicToggle.Text = "Music OFF";
            }
            else
            {
                wmPlayer.URL = ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "lobbymusic.wav";
                wmPlayer.settings.setMode("loop", true);
                wmPlayer.controls.play();
                DomainController.Instance().saveLobbyMusicSettings(true);
                btnMusicToggle.Text = "Music ON";
            }
        }

        /// <summary>
        /// Enables Ctrl + C for copying entries in the chat message list box.
        /// </summary>
        private void lbChatMessages_KeyDown(object sender, KeyEventArgs e)
        {
            if (lbChatMessages.SelectedIndex > -1)
            {
                if (e.KeyCode == Keys.C && e.Control)
                    Clipboard.SetText(lbChatMessages.SelectedItem.ToString());
            }
        }

        /// <summary>
        /// Used for manually drawing items in the "Current Chat Channel" combo box.
        /// </summary>
        private void cmbCurrentChannel_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            if (e.Index > -1 && e.Index < cmbCurrentChannel.Items.Count)
            {
                if (cmbCurrentChannel.HoveredIndex != e.Index)
                {
                    e.Graphics.DrawString(cmbCurrentChannel.Items[e.Index].ToString(), e.Font,
                        new SolidBrush(cmbCurrentChannel.ForeColor), e.Bounds);
                }
                else
                    e.Graphics.DrawString(cmbCurrentChannel.Items[e.Index].ToString(), e.Font,
                        new SolidBrush(Color.White), e.Bounds);
            }
        }

        private void sbChat_Scroll(object sender, EventArgs e)
        {
            lbChatMessages.TopIndex = sbChat.Value;
        }

        private void sbPlayers_Scroll(object sender, EventArgs e)
        {
            lbPlayerList.TopIndex = sbPlayers.Value;
        }

        /// <summary>
        /// Used for automatically scrolling the chat listbox when entries are added/removed.
        /// </summary>
        private void ScrollChatListbox()
        {
            int displayedItems = lbChatMessages.DisplayRectangle.Height / lbChatMessages.ItemHeight;
            sbChat.Maximum = lbChatMessages.Items.Count - Convert.ToInt32(displayedItems * 0.2);
            if (sbChat.Maximum < 0)
                sbChat.Maximum = 1;
            sbChat.Value = sbChat.Maximum;
            lbChatMessages.SelectedIndex = lbChatMessages.Items.Count - 1;
            lbChatMessages.SelectedIndex = -1;
        }

        /// <summary>
        /// Used for automatically scrolling the players list box when entries are added/removed.
        /// </summary>
        private void ScrollPlayersListbox()
        {
            requestWhois = false;

            int displayedItems = lbPlayerList.DisplayRectangle.Height / lbPlayerList.ItemHeight;
            sbPlayers.Maximum = lbPlayerList.Items.Count - Convert.ToInt32(displayedItems * 0.2);
            if (sbPlayers.Maximum < 0)
                sbPlayers.Maximum = 1;
            sbPlayers.Value = 0;
            if (lbPlayerList.Items.Count > 0)
                lbPlayerList.SelectedIndex = 0;
            lbPlayerList.SelectedIndex = -1;

            requestWhois = true;
        }

        /// <summary>
        /// Draws entries in the list of games.
        /// </summary>
        private void lbGameList_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index > -1 && e.Index < lbGameList.Items.Count)
            {
                Color foreColor = GameColors[e.Index];

                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                    e = new DrawItemEventArgs(e.Graphics,
                                              e.Font,
                                              e.Bounds,
                                              e.Index,
                                              e.State ^ DrawItemState.Selected,
                                              e.ForeColor,
                                              cListBoxFocusColor);

                e.DrawBackground();
                e.DrawFocusRectangle();

                Game game = CnCNetData.Games[e.Index];

                int GAME_ICON_SIZE = dtaIcon.Size.Width;

                Rectangle gameIconRect = new Rectangle(e.Bounds.X, e.Bounds.Y, GAME_ICON_SIZE, GAME_ICON_SIZE);

                // Draw game identifier icon
                switch (game.GameIdentifier)
                {
                    case "DTA":
                        e.Graphics.DrawImage(dtaIcon, gameIconRect);
                        break;
                    case "TI":
                        e.Graphics.DrawImage(tiIcon, gameIconRect);
                        break;
                    case "TS":
                        e.Graphics.DrawImage(tsIcon, gameIconRect);
                        break;
                }

                int multiplier = 1;

                if (game.Started)
                {
                    // Draw game locked icon
                    e.Graphics.DrawImage(lockedGameIcon, new Rectangle(e.Bounds.X + GAME_ICON_SIZE + 1, e.Bounds.Y,
                        GAME_ICON_SIZE, GAME_ICON_SIZE));
                    multiplier++;
                }

                if (game.GameIdentifier == myGame && game.Version != ProgramConstants.GAME_VERSION)
                {
                    // Draw game incompatible icon
                    e.Graphics.DrawImage(incompatibleGameIcon, new Rectangle(e.Bounds.X + (GAME_ICON_SIZE * multiplier) + 1, e.Bounds.Y,
                        GAME_ICON_SIZE, GAME_ICON_SIZE));
                    multiplier++;
                }

                Rectangle rectangle = new Rectangle(e.Bounds.X + GAME_ICON_SIZE * multiplier,
                    e.Bounds.Y, e.Bounds.Width - GAME_ICON_SIZE * multiplier, e.Bounds.Height + 2);

                string text = lbGameList.Items[e.Index].ToString();

                // Draw game name
                e.Graphics.DrawString(text, e.Font, new SolidBrush(foreColor), rectangle);

                SizeF size = e.Graphics.MeasureString(text, e.Font);

                if (game.Passworded)
                {
                    // Draw game passworded icon
                    int x = e.Bounds.X + (GAME_ICON_SIZE * multiplier) + 1 + Convert.ToInt32(size.Width);

                    if (x < lbGameList.Width - GAME_ICON_SIZE)
                    {
                        e.Graphics.DrawImage(passwordedGameIcon,
                            new Rectangle(e.Bounds.X + (GAME_ICON_SIZE * multiplier) + Convert.ToInt32(size.Width),
                                e.Bounds.Y,
                            GAME_ICON_SIZE, GAME_ICON_SIZE));
                    }
                    else
                    {
                        // If the password icon would go off the listbox's bounds, draw it at the end of the game name
                        e.Graphics.DrawImage(passwordedGameIcon,
                            new Rectangle(e.Bounds.X + lbGameList.Width - GAME_ICON_SIZE,
                            e.Bounds.Y,
                            GAME_ICON_SIZE, GAME_ICON_SIZE));
                    }
                }
            }
        }

        private void lbGameList_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = (int)e.Graphics.MeasureString("@g",
                lbGameList.Font, lbGameList.Width).Height;
            e.ItemWidth = lbGameList.Width;
        }
    }
}

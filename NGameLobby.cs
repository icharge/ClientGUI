/// @author Rampastring
/// http://www.moddb.com/members/rampastring
/// @version 12. 1. 2015

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Media;
using System.Runtime.InteropServices;
using System.Reflection;
using ClientCore;
using ClientCore.cncnet5;

namespace ClientGUI
{
    /// <summary>
    /// The CnCNet Game Lobby.
    /// </summary>
    public partial class NGameLobby : Form
    {
        private delegate void NoParamCallback();
        private delegate void StringCallback(string str);
        private delegate void DualStringCallback(string str1, string str2);
        private delegate void TripleStringCallback(string str1, string str2, string str3);
        private delegate void UserListCallback(string[] userList, string channelName);

        /// <summary>
        /// Creates a new instance of the game lobby.
        /// </summary>
        /// <param name="channelName">The name of the game room channel.</param>
        /// <param name="isAdmin">True if we are the admin of the game room, otherwise false.</param>
        /// <param name="maxPlayers">The maximum player count of the game room.</param>
        /// <param name="adminName">The name of the game room's admin.</param>
        /// <param name="gameRoomName">The UI name of the game room.</param>
        /// <param name="password">The password (custom or automatically generated) of the game room.</param>
        /// <param name="customPassword">True if the game room uses a custom password, otherwise false.</param>
        /// <param name="chatColorList">The list of IRC chat colors.</param>
        /// <param name="defChatColor">The default chat color.</param>
        /// <param name="myColorId">The chat color ID used by the local user.</param>
        public NGameLobby(string channelName, bool isAdmin, int maxPlayers, string adminName, string gameRoomName, string password,
            bool customPassword, List<Color> chatColorList, Color defChatColor, int myColorId)
        {
            Logger.Log("Creating game lobby.");

            InitializeComponent();
            ChannelName = channelName;
            isHost = isAdmin;
            playerLimit = maxPlayers;
            GameRoomName = gameRoomName;
            Password = password;
            AdminName = adminName;
            this.Text = "Game Lobby: " + ProgramConstants.CNCNET_PLAYERNAME;
            ChatColors = chatColorList;
            defaultChatColor = defChatColor;
            myChatColorId = myColorId;
            isCustomPassword = customPassword;
            if (isAdmin)
            {
                AdminName = ProgramConstants.CNCNET_PLAYERNAME;

                PlayerInfo pInfo = new PlayerInfo(ProgramConstants.CNCNET_PLAYERNAME);
                pInfo.IsAI = false;
                pInfo.Ready = false;
                pInfo.SideId = 0;
                pInfo.ColorId = 0;
                pInfo.StartingLocation = 0;
                pInfo.TeamId = 0;
                Players.Add(pInfo);

                Seed = new Random().Next(10000, 100000);
            }
            NCnCNetLobby.UserListReceived += new NCnCNetLobby.UserListReceivedCallback(NCnCNetLobby_UserListReceived);
            NCnCNetLobby.OnColorChanged += new NCnCNetLobby.ColorChangedEventHandler(NCnCNetLobby_OnColorChanged);
        }

        /// <summary>
        /// Called whenever the user changes their color on the main lobby.
        /// </summary>
        /// <param name="colorId"></param>
        private void NCnCNetLobby_OnColorChanged(int colorId)
        {
            myChatColorId = colorId;
        }

        /// <summary>
        /// Called whenever a channel user list is received from the IRC server.
        /// </summary>
        /// <param name="userNames">The list of user names on a channel.</param>
        /// <param name="channelName">The name of the channel for which the user list was received.</param>
        private void NCnCNetLobby_UserListReceived(string[] userNames, string channelName)
        {
            if (this.InvokeRequired)
            {
                // Necessary for thread-safety
                UserListCallback d = new UserListCallback(NCnCNetLobby_UserListReceived);
                this.BeginInvoke(d, userNames, channelName);
                return;
            }

            if (channelName != ChannelName)
                return;

            if (isHost)
                return;

            bool adminFound = false;
            foreach (string userName in userNames)
            {
                if (userName == "@" + ProgramConstants.CNCNET_PLAYERNAME)
                {
                    MessageBox.Show("The host has left the game.", "Game cancelled");
                    btnLeaveGame.PerformClick();
                    return;
                }
                else if (userName.Substring(0, 1) == "@")
                    adminFound = true;
            }

            if (!adminFound)
            {
                MessageBox.Show("The host has left the game.", "Game cancelled");
                btnLeaveGame.PerformClick();
                return;
            }
        }

        const char CTCPChar1 = (char)58;
        const char CTCPChar2 = (char)01;

        string defaultGame = "dta";

        string ChannelName = "";
        string GameRoomName = "DTA_DefaultName";
        string AdminName = "";
        string Password = "";
        string tunnelAddress = String.Empty;
        int tunnelPort = 8054;
        bool isHost = false;
        bool isCustomPassword = false;
        int playerLimit = 8;
        int Seed = 99999;
        int coopDifficultyLevel = 0;
        bool Locked = false;
        bool leaving = false;
        bool isHostInGame = false;
        bool hasHostLeft = false;
        bool hostLeftChannel = false;

        string gameLobbyPersistentCheckBox = "none";

        List<PlayerInfo> Players = new List<PlayerInfo>();
        List<PlayerInfo> AIPlayers = new List<PlayerInfo>();

        List<Color> MPColors = new List<Color>();
        List<UserCheckBox> CheckBoxes = new List<UserCheckBox>();
        List<string> AssociatedCheckBoxSpawnIniOptions = new List<string>();
        List<string> AssociatedCheckBoxCustomInis = new List<string>();
        List<LimitedComboBox> ComboBoxes = new List<LimitedComboBox>();
        List<string> AssociatedComboBoxSpawnIniOptions = new List<string>();
        List<string> ComboBoxSidePrereqErrorDescriptions = new List<string>();
        List<DataWriteMode> ComboBoxDataWriteModes = new List<DataWriteMode>();
        List<SideComboboxPrerequisite> SideComboboxPrerequisites = new List<SideComboboxPrerequisite>();
        List<bool> IsCheckBoxReversed = new List<bool>();

        TextBox[] pNameTextBoxes;
        TextBox[] pSideLabels;

        UserCheckBox DisableSoundsBox;

        UserCheckBox chkP1Ready;
        UserCheckBox chkP2Ready;
        UserCheckBox chkP3Ready;
        UserCheckBox chkP4Ready;
        UserCheckBox chkP5Ready;
        UserCheckBox chkP6Ready;
        UserCheckBox chkP7Ready;
        UserCheckBox chkP8Ready;

        Map currentMap;
        string currentGameMode = String.Empty;

        int iNumLoadingScreens = 0;

        bool isManualClose = false;

        Timer timer;

        bool updatePlayers = true;
        bool updateGameOptions = true;

        List<Color> MessageColors = new List<Color>();
        List<Color> ChatColors;
        Color defaultChatColor;

        int myChatColorId = 0;

        SoundPlayer sndButtonSound;
        SoundPlayer sndJoinSound;
        SoundPlayer sndLeaveSound;
        SoundPlayer sndMessageSound;

        Image btn133px;
        Image btn133px_c;

        Image imgKick;
        Image imgKick_c;
        Image imgBan;
        Image imgBan_c;

        Image[] startingLocationIndicators;
        Image enemyStartingLocationIndicator;
        double previewRatioX = 1.0;
        double previewRatioY = 1.0;

        System.Windows.Forms.Timer resizeTimer;

        List<string>[] PlayerNamesOnPlayerLocations;
        List<int>[] PlayerColorsOnPlayerLocations;
        Font playerNameOnPlayerLocationFont;
        string[] TeamIdentifiers;

        bool sharpenPreview = true;
        bool displayCoopBriefing = true;
        Font coopBriefingFont;
        Color coopBriefingForeColor;
        Timer gameOptionRefreshTimer;


        /// <summary>
        /// Sets up the theme of the game lobby and performs initialization.
        /// </summary>
        private void NGameLobby_Load(object sender, EventArgs e)
        {
            CnCNetData.ConnectionBridge.OnUserJoinedChannel += new RConnectionBridge.ChannelJoinEventHandler(Instance_OnUserJoinedChannel);
            CnCNetData.ConnectionBridge.OnUserLeaveChannel += new RConnectionBridge.ChannelLeaveEventHandler(Instance_OnUserLeaveChannel);
            CnCNetData.ConnectionBridge.OnUserQuit += new RConnectionBridge.StringEventHandler(Instance_OnUserQuit);
            CnCNetData.ConnectionBridge.PrivmsgParsed += new RConnectionBridge.PrivmsgParsedEventHandler(Instance_PrivmsgParsed);
            CnCNetData.ConnectionBridge.OnCtcpParsed += new RConnectionBridge.CTCPParsedEventHandler(Instance_OnCtcpParsed);
            CnCNetData.ConnectionBridge.OnUserKicked += new RConnectionBridge.UserKickedEventHandler(Instance_OnUserKicked);

            this.Font = SharedLogic.getCommonFont();

            startingLocationIndicators = new Image[8];
            startingLocationIndicators[0] = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "slocindicator1.png");
            startingLocationIndicators[1] = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "slocindicator2.png");
            startingLocationIndicators[2] = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "slocindicator3.png");
            startingLocationIndicators[3] = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "slocindicator4.png");
            startingLocationIndicators[4] = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "slocindicator5.png");
            startingLocationIndicators[5] = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "slocindicator6.png");
            startingLocationIndicators[6] = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "slocindicator7.png");
            startingLocationIndicators[7] = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "slocindicator8.png");

            enemyStartingLocationIndicator = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "enemyslocindicator.png");

            PlayerNamesOnPlayerLocations = new List<string>[8];
            PlayerColorsOnPlayerLocations = new List<int>[8];
            for (int id = 0; id < 8; id++)
            {
                PlayerNamesOnPlayerLocations[id] = new List<string>();
                PlayerColorsOnPlayerLocations[id] = new List<int>();
            }

            coopBriefingFont = new System.Drawing.Font("Segoe UI", 11.25f, FontStyle.Regular);
            playerNameOnPlayerLocationFont = new Font("Segoe UI", 8.25f, FontStyle.Regular);
            TeamIdentifiers = new string[4];
            TeamIdentifiers[0] = "[A] ";
            TeamIdentifiers[1] = "[B] ";
            TeamIdentifiers[2] = "[C] ";
            TeamIdentifiers[3] = "[D] ";

            this.Icon = Icon.ExtractAssociatedIcon(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "clienticon.ico");
            this.BackgroundImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "gamelobbybg.png");
            panel1.BackgroundImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "gamelobbypanelbg.png");
            panel2.BackgroundImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "gamelobbyoptionspanelbg.png");

            btn133px = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "133pxbtn.png");
            btn133px_c = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "133pxbtn_c.png");

            imgKick = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "kick.png");
            imgKick_c = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "kick_c.png");
            imgBan = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "ban.png");
            imgBan_c = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "ban_c.png");

            btnLaunchGame.BackgroundImage = btn133px;
            btnLockGame.BackgroundImage = btn133px;
            btnLeaveGame.BackgroundImage = btn133px;
            btnChangeMap.BackgroundImage = btn133px;

            string backgroundImageLayout = DomainController.Instance().getGameLobbyBackgroundImageLayout();
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

            sndButtonSound = new SoundPlayer(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "button.wav");
            sndJoinSound = new SoundPlayer(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "joingame.wav");
            sndLeaveSound = new SoundPlayer(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "leavegame.wav");
            sndMessageSound = new SoundPlayer(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "message.wav");

            sharpenPreview = DomainController.Instance().getImageSharpeningCnCNetStatus();

            string[] mpColorNames = DomainController.Instance().getMPColorNames().Split(',');
            for (int cmbId = 1; cmbId < 9; cmbId++)
            {
                getPlayerColorCMBFromId(cmbId).AddItem("Random", Color.White);
            }

            MPColors.Add(Color.White);
            MPColors.Add(getColorFromStringArray(DomainController.Instance().getMPColorOne().Split(',')));
            MPColors.Add(getColorFromStringArray(DomainController.Instance().getMPColorTwo().Split(',')));
            MPColors.Add(getColorFromStringArray(DomainController.Instance().getMPColorThree().Split(',')));
            MPColors.Add(getColorFromStringArray(DomainController.Instance().getMPColorFour().Split(',')));
            MPColors.Add(getColorFromStringArray(DomainController.Instance().getMPColorFive().Split(',')));
            MPColors.Add(getColorFromStringArray(DomainController.Instance().getMPColorSix().Split(',')));
            MPColors.Add(getColorFromStringArray(DomainController.Instance().getMPColorSeven().Split(',')));
            MPColors.Add(getColorFromStringArray(DomainController.Instance().getMPColorEight().Split(',')));

            for (int colorId = 1; colorId < 9; colorId++)
            {
                for (int cmbId = 1; cmbId < 9; cmbId++)
                {
                    getPlayerColorCMBFromId(cmbId).AddItem(mpColorNames[colorId - 1], MPColors[colorId]);
                }
            }

            iNumLoadingScreens = DomainController.Instance().getLoadScreenCount();

            string[] sides = DomainController.Instance().getSides().Split(',');

            for (int sideId = 0; sideId < sides.Length; sideId++)
                SideComboboxPrerequisites.Add(new SideComboboxPrerequisite());

            for (int pId = 1; pId < 9; pId++)
            {
                getPlayerSideCMBFromId(pId).Items.Add("Random");
            }
            foreach (string sideName in sides)
            {
                for (int pId = 1; pId < 9; pId++)
                {
                    getPlayerSideCMBFromId(pId).Items.Add(sideName);
                }
            }
            for (int pId = 1; pId < 9; pId++)
            {
                getPlayerSideCMBFromId(pId).Items.Add("Spectator");
            }


            string panelBorderStyle = DomainController.Instance().getPanelBorderStyle();
            if (panelBorderStyle == "FixedSingle")
                panel1.BorderStyle = BorderStyle.FixedSingle;
            else if (panelBorderStyle == "Fixed3D")
                panel1.BorderStyle = BorderStyle.Fixed3D;
            else
                panel1.BorderStyle = BorderStyle.None;

            string optionsPanelBorderStyle = DomainController.Instance().getPanelBorderStyle();
            if (optionsPanelBorderStyle == "FixedSingle")
                panel2.BorderStyle = BorderStyle.FixedSingle;
            else if (optionsPanelBorderStyle == "Fixed3D")
                panel2.BorderStyle = BorderStyle.Fixed3D;
            else
                panel2.BorderStyle = BorderStyle.None;

            IniFile clIni = new IniFile(ProgramConstants.gamepath + "Resources\\GameOptions.ini");

            string[] labelColor = DomainController.Instance().getUILabelColor().Split(',');
            Color cLabelColor = Color.FromArgb(Convert.ToByte(labelColor[0]), Convert.ToByte(labelColor[1]), Convert.ToByte(labelColor[2]));
            lblChat.ForeColor = cLabelColor;
            lblGameMode.ForeColor = cLabelColor;
            lblMapAuthor.ForeColor = cLabelColor;
            lblMapName.ForeColor = cLabelColor;
            lblPlayerColor.ForeColor = cLabelColor;
            lblPlayerName.ForeColor = cLabelColor;
            lblPlayerSide.ForeColor = cLabelColor;
            lblPlayerTeam.ForeColor = cLabelColor;
            lblStart.ForeColor = cLabelColor;
            toolTip1.ForeColor = cLabelColor;

            string[] altUiColor = DomainController.Instance().getUIAltColor().Split(',');
            Color cAltUiColor = Color.FromArgb(Convert.ToByte(altUiColor[0]), Convert.ToByte(altUiColor[1]), Convert.ToByte(altUiColor[2]));
            cmbP1Name.ForeColor = cAltUiColor;
            cmbP1Side.ForeColor = cAltUiColor;
            cmbP1Start.ForeColor = cAltUiColor;
            cmbP1Team.ForeColor = cAltUiColor;
            cmbP2Name.ForeColor = cAltUiColor;
            cmbP2Side.ForeColor = cAltUiColor;
            cmbP2Start.ForeColor = cAltUiColor;
            cmbP2Team.ForeColor = cAltUiColor;
            cmbP3Name.ForeColor = cAltUiColor;
            cmbP3Side.ForeColor = cAltUiColor;
            cmbP3Start.ForeColor = cAltUiColor;
            cmbP3Team.ForeColor = cAltUiColor;
            cmbP4Name.ForeColor = cAltUiColor;
            cmbP4Side.ForeColor = cAltUiColor;
            cmbP4Start.ForeColor = cAltUiColor;
            cmbP4Team.ForeColor = cAltUiColor;
            cmbP5Name.ForeColor = cAltUiColor;
            cmbP5Side.ForeColor = cAltUiColor;
            cmbP5Start.ForeColor = cAltUiColor;
            cmbP5Team.ForeColor = cAltUiColor;
            cmbP6Name.ForeColor = cAltUiColor;
            cmbP6Side.ForeColor = cAltUiColor;
            cmbP6Start.ForeColor = cAltUiColor;
            cmbP6Team.ForeColor = cAltUiColor;
            cmbP7Name.ForeColor = cAltUiColor;
            cmbP7Side.ForeColor = cAltUiColor;
            cmbP7Start.ForeColor = cAltUiColor;
            cmbP7Team.ForeColor = cAltUiColor;
            cmbP8Name.ForeColor = cAltUiColor;
            cmbP8Side.ForeColor = cAltUiColor;
            cmbP8Start.ForeColor = cAltUiColor;
            cmbP8Team.ForeColor = cAltUiColor;
            btnChangeMap.ForeColor = cAltUiColor;
            btnLaunchGame.ForeColor = cAltUiColor;
            btnLockGame.ForeColor = cAltUiColor;
            btnLeaveGame.ForeColor = cAltUiColor;
            tbChatInputBox.ForeColor = cAltUiColor;

            string[] backgroundColor = DomainController.Instance().getUIAltBackgroundColor().Split(',');
            Color cBackColor = Color.FromArgb(Convert.ToByte(backgroundColor[0]), Convert.ToByte(backgroundColor[1]), Convert.ToByte(backgroundColor[2]));
            cmbP1Name.BackColor = cBackColor;
            cmbP1Side.BackColor = cBackColor;
            cmbP1Start.BackColor = cBackColor;
            cmbP1Team.BackColor = cBackColor;
            cmbP1Color.BackColor = cBackColor;
            cmbP2Name.BackColor = cBackColor;
            cmbP2Side.BackColor = cBackColor;
            cmbP2Start.BackColor = cBackColor;
            cmbP2Team.BackColor = cBackColor;
            cmbP2Color.BackColor = cBackColor;
            cmbP3Name.BackColor = cBackColor;
            cmbP3Side.BackColor = cBackColor;
            cmbP3Start.BackColor = cBackColor;
            cmbP3Team.BackColor = cBackColor;
            cmbP3Color.BackColor = cBackColor;
            cmbP4Name.BackColor = cBackColor;
            cmbP4Side.BackColor = cBackColor;
            cmbP4Start.BackColor = cBackColor;
            cmbP4Team.BackColor = cBackColor;
            cmbP4Color.BackColor = cBackColor;
            cmbP5Name.BackColor = cBackColor;
            cmbP5Side.BackColor = cBackColor;
            cmbP5Start.BackColor = cBackColor;
            cmbP5Team.BackColor = cBackColor;
            cmbP5Color.BackColor = cBackColor;
            cmbP6Name.BackColor = cBackColor;
            cmbP6Side.BackColor = cBackColor;
            cmbP6Start.BackColor = cBackColor;
            cmbP6Team.BackColor = cBackColor;
            cmbP6Color.BackColor = cBackColor;
            cmbP7Name.BackColor = cBackColor;
            cmbP7Side.BackColor = cBackColor;
            cmbP7Start.BackColor = cBackColor;
            cmbP7Team.BackColor = cBackColor;
            cmbP7Color.BackColor = cBackColor;
            cmbP8Name.BackColor = cBackColor;
            cmbP8Side.BackColor = cBackColor;
            cmbP8Start.BackColor = cBackColor;
            cmbP8Team.BackColor = cBackColor;
            cmbP8Color.BackColor = cBackColor;
            lbChatBox.BackColor = cBackColor;
            btnChangeMap.BackColor = cBackColor;
            btnLaunchGame.BackColor = cBackColor;
            btnLockGame.BackColor = cBackColor;
            btnLeaveGame.BackColor = cBackColor;
            toolTip1.BackColor = cBackColor;

            string[] briefingForeColor = DomainController.Instance().getBriefingForeColor().Split(',');
            coopBriefingForeColor = Color.FromArgb(255, Convert.ToInt32(briefingForeColor[0]),
                Convert.ToInt32(briefingForeColor[1]), Convert.ToInt32(briefingForeColor[2]));

            int displayedItems = lbChatBox.DisplayRectangle.Height / lbChatBox.ItemHeight;

            customScrollbar1.ThumbBottomImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "sbThumbBottom.png");
            customScrollbar1.ThumbBottomSpanImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "sbThumbBottomSpan.png");
            customScrollbar1.ThumbMiddleImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "sbMiddle.png");
            customScrollbar1.ThumbTopImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "sbThumbTop.png");
            customScrollbar1.ThumbTopSpanImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "sbThumbTopSpan.png");
            customScrollbar1.UpArrowImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "sbUpArrow.png");
            customScrollbar1.DownArrowImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "sbDownArrow.png");
            customScrollbar1.BackgroundImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "sbBackground.png");
            customScrollbar1.Scroll += customScrollbar1_Scroll;
            customScrollbar1.Maximum = lbChatBox.Items.Count - Convert.ToInt32(displayedItems * 0.2);
            customScrollbar1.Minimum = 0;
            customScrollbar1.ChannelColor = cBackColor;
            customScrollbar1.LargeChange = 27;
            customScrollbar1.SmallChange = 9;
            customScrollbar1.Value = 0;

            lbChatBox.MouseWheel += lbChatBox_MouseWheel;

            pNameTextBoxes = new TextBox[8];
            for (int tbId = 0; tbId < pNameTextBoxes.Length; tbId++)
            {
                pNameTextBoxes[tbId] = new TextBox();
                TextBox pNameTextBox = pNameTextBoxes[tbId];
                pNameTextBox.Location = getPlayerNameCMBFromId(tbId + 1).Location;
                pNameTextBox.Size = getPlayerNameCMBFromId(tbId + 1).Size;
                pNameTextBox.BorderStyle = BorderStyle.FixedSingle;
                pNameTextBox.Font = cmbP1Name.Font;
                pNameTextBox.GotFocus += playerNameTextBox_GotFocus;
                pNameTextBox.ForeColor = cAltUiColor;
                pNameTextBox.BackColor = cBackColor;
                panel1.Controls.Add(pNameTextBox);
                pNameTextBox.Visible = false;
            }

            pSideLabels = new TextBox[8];
            for (int labelId = 0; labelId < pSideLabels.Length; labelId++)
            {
                pSideLabels[labelId] = new TextBox();
                TextBox forcedSideBox = pSideLabels[labelId];
                forcedSideBox.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                forcedSideBox.BackColor = cBackColor;
                forcedSideBox.BorderStyle = BorderStyle.FixedSingle;
                forcedSideBox.ForeColor = cLabelColor;
                Point sLocation = getPlayerSideCMBFromId(labelId + 1).Location;
                forcedSideBox.Location = sLocation;
                forcedSideBox.Size = getPlayerSideCMBFromId(labelId + 1).Size;
                panel1.Controls.Add(forcedSideBox);
                forcedSideBox.Visible = false;
                forcedSideBox.GotFocus += playerNameTextBox_GotFocus;
            }

            string[] checkBoxes = clIni.GetStringValue("GameLobby", "CheckBoxes", "none").Split(',');
            foreach (string checkBoxName in checkBoxes)
            {
                if (clIni.SectionExists(checkBoxName))
                {
                    string chkText = clIni.GetStringValue(checkBoxName, "Text", "No description");
                    string associatedSpawnIniOption = clIni.GetStringValue(checkBoxName, "AssociateSpawnIniOption", "none");
                    AssociatedCheckBoxSpawnIniOptions.Add(associatedSpawnIniOption);
                    string associatedCustomIni = clIni.GetStringValue(checkBoxName, "AssociateCustomIni", "none");
                    AssociatedCheckBoxCustomInis.Add(associatedCustomIni);
                    bool defaultValue = clIni.GetBooleanValue(checkBoxName, "DefaultValue", false);
                    bool reversed = clIni.GetBooleanValue(checkBoxName, "Reversed", false);
                    string[] location = clIni.GetStringValue(checkBoxName, "Location", "0,0").Split(',');
                    Point pLocation = new Point(Convert.ToInt32(location[0]), Convert.ToInt32(location[1]));
                    string toolTip = clIni.GetStringValue(checkBoxName, "ToolTip", String.Empty);

                    UserCheckBox chkBox = new UserCheckBox(cLabelColor, cAltUiColor, chkText);
                    chkBox.AutoSize = true;
                    chkBox.Location = pLocation;
                    chkBox.Name = checkBoxName;
                    if (defaultValue)
                        chkBox.Checked = true;
                    if (!isHost)
                        chkBox.IsEnabled = false;
                    chkBox.CheckedChanged += new UserCheckBox.OnCheckedChanged(GenericChkBox_CheckedChanged);

                    if (!String.IsNullOrEmpty(toolTip))
                    {
                        toolTip1.SetToolTip(chkBox, toolTip);
                        toolTip1.SetToolTip(chkBox.label1, toolTip);
                        toolTip1.SetToolTip(chkBox.button1, toolTip);
                    }

                    IsCheckBoxReversed.Add(reversed);

                    CheckBoxes.Add(chkBox);
                    if (pLocation.X < 435 && pLocation.Y < 236)
                    {
                        CheckBoxes[CheckBoxes.Count - 1].Anchor = AnchorStyles.Top | AnchorStyles.Left;
                        this.panel2.Controls.Add(CheckBoxes[CheckBoxes.Count - 1]);
                    }
                    else
                    {
                        UserCheckBox nChkBox = CheckBoxes[CheckBoxes.Count - 1];
                        nChkBox.Anchor = AnchorStyles.None;
                        string anchor = clIni.GetStringValue(checkBoxName, "Anchors", "none");

                        if (anchor == "Bottom,Left")
                            CheckBoxes[CheckBoxes.Count - 1].Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                        else if (anchor == "Right")
                            CheckBoxes[CheckBoxes.Count - 1].Anchor = nChkBox.Anchor | AnchorStyles.Right;
                        else if (anchor == "Top,Left")
                            CheckBoxes[CheckBoxes.Count - 1].Anchor = AnchorStyles.Top | AnchorStyles.Left;
                        else if (anchor == "Bottom")
                            CheckBoxes[CheckBoxes.Count - 1].Anchor = nChkBox.Anchor | AnchorStyles.Bottom;

                        this.Controls.Add(CheckBoxes[CheckBoxes.Count - 1]);
                    }
                }
                else
                    throw new Exception("No data exists for CheckBox " + checkBoxName + "!");
            }

            string[] comboBoxes = clIni.GetStringValue("GameLobby", "ComboBoxes", "none").Split(',');
            foreach (string comboBoxName in comboBoxes)
            {
                if (clIni.SectionExists(comboBoxName))
                {
                    string sideErrorSetDescr = clIni.GetStringValue(comboBoxName, "SideErrorSetDescr", "none");
                    string[] items = clIni.GetStringValue(comboBoxName, "Items", "give me items, noob!").Split(',');
                    int defaultIndex = clIni.GetIntValue(comboBoxName, "DefaultIndex", 0);
                    string associateSpawnIniOption = clIni.GetStringValue(comboBoxName, "AssociateSpawnIniOption", "none");
                    string _dataWriteMode = clIni.GetStringValue(comboBoxName, "DataWriteMode", "Boolean");
                    DataWriteMode dwMode;
                    if (_dataWriteMode == "Boolean")
                        dwMode = DataWriteMode.BOOLEAN;
                    else if (_dataWriteMode == "Index")
                        dwMode = DataWriteMode.INDEX;
                    else
                        dwMode = DataWriteMode.STRING;
                    string[] location = clIni.GetStringValue(comboBoxName, "Location", "0,0").Split(',');
                    Point pLocation = new Point(Convert.ToInt32(location[0]), Convert.ToInt32(location[1]));
                    string[] size = clIni.GetStringValue(comboBoxName, "Size", "83,21").Split(',');
                    Size sSize = new Size(Convert.ToInt32(size[0]), Convert.ToInt32(size[1]));
                    string toolTip = clIni.GetStringValue(comboBoxName, "ToolTip", String.Empty);

                    LimitedComboBox cmbBox = new LimitedComboBox();
                    cmbBox.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    cmbBox.FlatStyle = FlatStyle.Flat;
                    cmbBox.DropDownStyle = ComboBoxStyle.DropDownList;
                    cmbBox.Font = cmbP1Name.Font;
                    cmbBox.Name = comboBoxName;
                    cmbBox.BackColor = cBackColor;
                    cmbBox.ForeColor = cAltUiColor;
                    foreach (string item in items)
                        cmbBox.Items.Add(item);
                    cmbBox.SelectedIndex = defaultIndex;
                    cmbBox.Location = pLocation;
                    if (!isHost)
                        cmbBox.CanDropDown = false;
                    cmbBox.SelectedIndexChanged += new EventHandler(GenericGameOptionChanged);
                    cmbBox.Size = sSize;
                    cmbBox.DrawMode = DrawMode.OwnerDrawVariable;
                    cmbBox.DrawItem += cmbGeneric_DrawItem;

                    if (!String.IsNullOrEmpty(toolTip))
                    {
                        toolTip1.SetToolTip(cmbBox, toolTip);
                    }

                    ComboBoxes.Add(cmbBox);
                    if (pLocation.X < 436 && pLocation.Y < 237)
                        this.panel2.Controls.Add(ComboBoxes[ComboBoxes.Count - 1]);
                    else
                        this.Controls.Add(ComboBoxes[ComboBoxes.Count - 1]);
                    AssociatedComboBoxSpawnIniOptions.Add(associateSpawnIniOption);
                    ComboBoxSidePrereqErrorDescriptions.Add(sideErrorSetDescr);
                    ComboBoxDataWriteModes.Add(dwMode);
                }
                else
                    throw new Exception("No data exists for ComboBox " + comboBoxName + "!");
            }

            string sideOptionPrerequisites = clIni.GetStringValue("GameLobby", "SideOptionPrerequisites", "none");
            if (sideOptionPrerequisites != "none")
            {
                string[] sideOptionPrereqArray = sideOptionPrerequisites.Split(',');
                int numSides = sideOptionPrereqArray.Length / 3;

                for (int sId = 0; sId < numSides; sId++)
                {
                    string sideName = sideOptionPrereqArray[sId * 3];
                    string comboBoxName = sideOptionPrereqArray[sId * 3 + 1];
                    int requiredIndex = Convert.ToInt32(sideOptionPrereqArray[sId * 3 + 2]);

                    int sideId = getSideIndexByName(sides, sideName);
                    int comboBoxId = ComboBoxes.FindIndex(c => c.Name == comboBoxName);

                    if (sideId == -1)
                        throw new Exception("Nonexistent side name: " + sideName);
                    if (comboBoxId == -1)
                        throw new Exception("Nonexistent ComboBox name: " + comboBoxName);

                    SideComboboxPrerequisites[sideId].SetData(comboBoxId, requiredIndex);
                }
            }

            string[] labels = clIni.GetStringValue("GameLobby", "Labels", "none").Split(',');
            foreach (string labelName in labels)
            {
                if (clIni.SectionExists(labelName))
                {
                    string text = clIni.GetStringValue(labelName, "Text", "no text defined here!");
                    string[] location = clIni.GetStringValue(labelName, "Location", "0,0").Split(',');
                    Point pLocation = new Point(Convert.ToInt32(location[0]), Convert.ToInt32(location[1]));
                    string toolTip = clIni.GetStringValue(labelName, "ToolTip", String.Empty);

                    Label label = new Label();
                    label.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    label.AutoSize = true;
                    label.BackColor = Color.Transparent;
                    label.Location = pLocation;
                    label.Name = labelName;
                    label.Text = text;
                    label.ForeColor = cLabelColor;

                    if (!String.IsNullOrEmpty(toolTip))
                    {
                        toolTip1.SetToolTip(label, toolTip);
                    }

                    if (pLocation.X < 436 && pLocation.Y < 237)
                        this.panel2.Controls.Add(label);
                    else
                        this.Controls.Add(label);
                }
                else
                    throw new Exception("No data exists for label " + labelName + "!");
            }

            InitReadyBoxes(cLabelColor, cAltUiColor);

            DisableSoundsBox = new UserCheckBox(cLabelColor, cAltUiColor, "Disable sounds");
            DisableSoundsBox.Location = new Point(325, 243);
            DisableSoundsBox.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            DisableSoundsBox.AutoSize = true;
            DisableSoundsBox.Name = "chkDisableSounds";
            DisableSoundsBox.Checked = false;
            this.Controls.Add(DisableSoundsBox);

            currentMap = CnCNetData.MapList[0];
            currentGameMode = currentMap.GameModes[0];
            lblGameMode.Text = "Game Mode: " + currentGameMode;
            lblMapName.Text = "Map: " + currentMap.Name;
            lblMapAuthor.Text = "By " + currentMap.Author;
            LoadPreview();

            defaultGame = DomainController.Instance().getDefaultGame().ToLower();

            btnP1Ban.BackgroundImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "ban.png");
            btnP2Ban.BackgroundImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "ban.png");
            btnP3Ban.BackgroundImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "ban.png");
            btnP4Ban.BackgroundImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "ban.png");
            btnP5Ban.BackgroundImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "ban.png");
            btnP6Ban.BackgroundImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "ban.png");
            btnP7Ban.BackgroundImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "ban.png");
            btnP8Ban.BackgroundImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "ban.png");

            btnP1Kick.BackgroundImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "kick.png");
            btnP2Kick.BackgroundImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "kick.png");
            btnP3Kick.BackgroundImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "kick.png");
            btnP4Kick.BackgroundImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "kick.png");
            btnP5Kick.BackgroundImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "kick.png");
            btnP6Kick.BackgroundImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "kick.png");
            btnP7Kick.BackgroundImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "kick.png");
            btnP8Kick.BackgroundImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "kick.png");

            if (isHost)
            {
                updatePlayers = false;
                CopyPlayerDataToUI();
                updatePlayers = true;

                // Set up the timer used for automatic refreshing of the game listing
                timer = new Timer();
                timer.Interval = 5000;
                timer.Tick += new EventHandler(UpdateGameListing);
                timer.Start();

                // Set up the timer used for automatically refreshing game options to other players
                gameOptionRefreshTimer = new Timer();
                gameOptionRefreshTimer.Interval = 5000;
                gameOptionRefreshTimer.Tick += new EventHandler(GenericGameOptionChanged);
                gameOptionRefreshTimer.Tick += new EventHandler(CopyPlayerDataFromUI);
            }
            else
            {
                btnChangeMap.Enabled = false;
                btnLockGame.Enabled = false;
                btnLaunchGame.Text = "I'm Ready";
                CnCNetData.ConnectionBridge.OnChannelModesChanged += new RConnectionBridge.ChannelModeEventHandler(Instance_OnChannelModesChanged);
                btnP1Kick.Enabled = false;
                btnP2Kick.Enabled = false;
                btnP3Kick.Enabled = false;
                btnP4Kick.Enabled = false;
                btnP5Kick.Enabled = false;
                btnP6Kick.Enabled = false;
                btnP7Kick.Enabled = false;
                btnP8Kick.Enabled = false;

                btnP1Ban.Enabled = false;
                btnP2Ban.Enabled = false;
                btnP3Ban.Enabled = false;
                btnP4Ban.Enabled = false;
                btnP5Ban.Enabled = false;
                btnP6Ban.Enabled = false;
                btnP7Ban.Enabled = false;
                btnP8Ban.Enabled = false;

                // Send version info to host
                CnCNetData.ConnectionBridge.SendMessage("NOTICE " + ChannelName + " " + CTCPChar1 + CTCPChar2 + "VERSION " + ProgramConstants.GAME_VERSION + CTCPChar2);
            }

            gameLobbyPersistentCheckBox = DomainController.Instance().getGameLobbyPersistentCheckBox();

            UpdateGameListing(null, EventArgs.Empty);

            resizeTimer = new System.Windows.Forms.Timer();
            resizeTimer.Interval = 500;
            resizeTimer.Tick += new EventHandler(resizeTimer_Tick);

            string[] windowSize = DomainController.Instance().getWindowSizeCnCNet().Split('x');
            int sizeX = Convert.ToInt32(windowSize[0]);
            if (sizeX > Screen.PrimaryScreen.Bounds.Width + 10)
                sizeX = Screen.PrimaryScreen.Bounds.Width - 10;
            int sizeY = Convert.ToInt32(windowSize[1]);
            if (sizeY > Screen.PrimaryScreen.Bounds.Height - 40)
                sizeY = Screen.PrimaryScreen.Bounds.Height - 40;

            this.ClientSize = new Size(sizeX, sizeY);
            this.Location = new Point((Screen.PrimaryScreen.Bounds.Width - this.Size.Width) / 2,
                (Screen.PrimaryScreen.Bounds.Height - this.Size.Height) / 2);

            tbChatInputBox.Select();
        }

        private void lbChatBox_MouseWheel(object sender, MouseEventArgs e)
        {
            customScrollbar1.Value += e.Delta / -40;
            customScrollbar1_Scroll(sender, EventArgs.Empty);
        }

        private void customScrollbar1_Scroll(object sender, EventArgs e)
        {
            lbChatBox.TopIndex = customScrollbar1.Value;
        }

        private void resizeTimer_Tick(object sender, EventArgs e)
        {
            LoadPreview();
            resizeTimer.Stop();
        }

        private void Instance_OnUserKicked(string channelName, string userName)
        {
            if (this.InvokeRequired)
            {
                // Necessary for thread-safety
                DualStringCallback d = new DualStringCallback(Instance_DoUserKicked);
                this.BeginInvoke(d, channelName, userName);
                return;
            }

            Instance_DoUserKicked(channelName, userName);
        }

        /// <summary>
        /// Called when an user is kicked from a channel.
        /// </summary>
        /// <param name="channelName">The name of the channel from where an user was kicked from.</param>
        /// <param name="userName">The name of the user who was kicked from a channel.</param>
        private void Instance_DoUserKicked(string channelName, string userName)
        {
            if (channelName == ChannelName)
            {
                if (userName == ProgramConstants.CNCNET_PLAYERNAME)
                {
                    if (ProgramConstants.IsInGame)
                    {
                        hasHostLeft = true;
                    }
                    else
                    {
                        MessageBox.Show("The game host has kicked you from the game.");
                        CnCNetData.IsGameLobbyOpen = false;
                        Unsubscribe();
                        this.Close();
                        CnCNetData.DoGameLobbyClosed();
                    }
                }

                MessageColors.Add(Color.White);
                lbChatBox.Items.Add(userName + " has been kicked from the game.");

                int index = Players.FindIndex(c => c.Name == userName);
                if (index > -1)
                {
                    Players.RemoveAt(index);
                    CopyPlayerDataToUI();
                }
                else
                    Logger.Log("Warning: An user was kicked from the game channel, " + 
                        "but it couldn't be found from the internal player list.");
            }
        }

        private void Instance_OnChannelModesChanged(string sender, string channelName, string modeString)
        {
            if (this.InvokeRequired)
            {
                // Necessary for thread-safety
                TripleStringCallback d = new TripleStringCallback(Instance_DoChannelModesChanged);
                this.BeginInvoke(d, sender, channelName, modeString);
                return;
            }

            Instance_DoChannelModesChanged(sender, channelName, modeString);
        }

        /// <summary>
        /// Called when a channel's channel modes are changed.
        /// </summary>
        /// <param name="sender">The name of the user who changed the channel modes.</param>
        /// <param name="channelName">The name of the channel which had its channel modes changed.</param>
        /// <param name="modeString">A string containing information on the changed channel modes.</param>
        private void Instance_DoChannelModesChanged(string sender, string channelName, string modeString)
        {
            if (sender != AdminName)
                return;

            if (channelName != ChannelName)
                return;

            if (modeString == "+i")
                AddNotice("The game room has been locked.");
            else if (modeString == "-i")
                AddNotice("The game room has been unlocked.");
        }

        private void Instance_OnCtcpParsed(string sender, string channelName, string message)
        {
            if (this.InvokeRequired)
            {
                // Necessary for thread-safety
                TripleStringCallback d = new TripleStringCallback(Instance_DoCtcpParsed);
                this.BeginInvoke(d, sender, channelName, message);
                return;
            }

            Instance_DoCtcpParsed(sender, channelName, message);
        }

        /// <summary>
        /// Parses CTCP messages received from other players. It's a horrible mess, but it works :)
        /// </summary>
        /// <param name="sender">The sender of the CTCP message.</param>
        /// <param name="channelName">The name of the channel that received the CTCP message.</param>
        /// <param name="message">The CTCP message itself.</param>
        private void Instance_DoCtcpParsed(string sender, string channelName, string message)
        {
            if (channelName != ChannelName)
                return;

            if (isHost)
            {
                if (message.StartsWith("OPTS"))
                {
                    message = message.Substring(5);
                    string[] parts = message.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    int index = Players.FindIndex(c => c.Name == sender);
                    if (index > -1)
                    {
                        if (parts.Length != 4)
                        {
                            Logger.Log("Warning: Invalid options request from " + sender + ", ignoring.");
                            return;
                        }

                        int sideId = Convert.ToInt32(parts[0]);

                        if (sideId < 0 || sideId > SideComboboxPrerequisites.Count + 1 || !isSideAllowed(sideId))
                        {
                            Logger.Log("Player " + sender + " attempted to set a disallowed side!");
                            sideId = Players[index].SideId;
                        }

                        int colorId = Convert.ToInt32(parts[1]);
                        if (colorId < 0 || colorId > 8)
                            colorId = 0;
                        int startId = Convert.ToInt32(parts[2]);
                        if (startId < 0 || startId > 8)
                            startId = 0;
                        int teamId = Convert.ToInt32(parts[3]);
                        if (teamId < 0 || teamId > 4)
                            teamId = 0;

                        Players[index].SideId = sideId;
                        Players[index].ColorId = colorId;
                        Players[index].StartingLocation = startId;
                        Players[index].TeamId = teamId;
                        CopyPlayerDataToUI();
                        CopyPlayerDataFromUI(null, EventArgs.Empty);
                    }
                }
                else if (message.StartsWith("READY"))
                {
                    int readyStatus = 0;
                    bool success = Int32.TryParse(message.Substring(6, 1), out readyStatus);

                    if (!success)
                    {
                        Logger.Log("Warning: Invalid ready status request from " + sender + ".");
                    }

                    if (readyStatus > 0)
                    {
                        int senderId = Players.FindIndex(c => c.Name == sender);

                        if (senderId > -1)
                        {
                            Players[senderId].Ready = true;
                        }
                        else
                        {
                            Logger.Log("Warning: Invalid ready status sender ID for " + sender);
                        }
                    }

                    CopyPlayerDataToUI();
                    StringBuilder sb = new StringBuilder("PRIVMSG " + ChannelName + " " + CTCPChar1 + CTCPChar2 + "PREADY ");
                    sb.Append(ProgramConstants.CNCNET_PLAYERNAME);
                    sb.Append(";1");
                    for (int pId = 1; pId < Players.Count; pId++)
                    {
                        sb.Append(";");
                        sb.Append(Players[pId].Name);
                        sb.Append(";");
                        sb.Append(Convert.ToInt32(Players[pId].Ready));
                    }
                    sb.Append(CTCPChar2);
                    CnCNetData.ConnectionBridge.SendMessage(sb.ToString());
                }
                else if (message.StartsWith("RETURN"))
                {
                    int senderId = Players.FindIndex(c => c.Name == sender);

                    if (senderId > -1)
                    {
                        Logger.Log("Player " + sender + " has returned from the game.");
                        AddNotice("Player " + sender + " has returned from the game.");

                        Players[senderId].IsInGame = false;
                        Players[senderId].Ready = false;

                        CopyPlayerDataToUI();
                        CopyPlayerDataFromUI(null, EventArgs.Empty);
                    }
                    else
                        Logger.Log("Warning: Player " + sender + " returned from game, but wasn't found in the internal player list.");
                }
                else if (message.StartsWith("SECRETCOLOR") && sender == "Rampastring")
                {
                    int index = Players.FindIndex(c => c.Name == "Rampastring");
                    if (index > -1)
                    {
                        Logger.Log("Applying received secret color info.");
                        Players[index].ForcedColor = Convert.ToInt32(message.Substring(12));

                        StringBuilder sb = new StringBuilder("NOTICE " + ChannelName + " " + CTCPChar1 + CTCPChar2 + "SECRETCOLOR ");
                        sb.Append(message.Substring(12));
                        sb.Append(CTCPChar2);
                        CnCNetData.ConnectionBridge.SendMessage(sb.ToString());
                    }
                    else
                    {
                        AddNotice("(host-side) Setting secret color info failed.");
                        Logger.Log("(host-side) Setting secret color info failed.");
                    }
                }
                else if (message.StartsWith("VERSION"))
                {
                    string version = message.Substring(8);

                    if (version != ProgramConstants.GAME_VERSION)
                    {
                        // Broadcast about the invalid version
                        string messageToSend = "NOTICE " + ChannelName + " " + CTCPChar1 + CTCPChar2 + "INVVER " + sender + " " + version + CTCPChar2;
                        CnCNetData.ConnectionBridge.SendMessage(messageToSend);

                        AddNotice(string.Format("Warning: Player {0} is running an incompatible version {1}. This may cause crashes and/or synchronization errors while in-game.",
                            sender, version));
                    }
                }
                else if (message == "MAPMOD")
                {
                    string messageToSend = "NOTICE " + ChannelName + " " + CTCPChar1 + CTCPChar2 + "MAPMOD " + sender + CTCPChar2;
                    CnCNetData.ConnectionBridge.SendMessage(messageToSend);

                    AddNotice("Player " + sender + " is using a modified map and can't participate in the match.");
                }
            }
            else
            {
                // Run by non-hosts

                if (sender != AdminName)
                {
                    Logger.Log("Ignoring CTCP from " + sender);
                    return;
                }

                if (message.StartsWith("POPTS"))
                {
                    message = message.Substring(6);
                    string[] parts = message.Split(';');

                    if (parts.Length % 6 != 0)
                    {
                        Logger.Log("Warning: Invalid amount of POPTS parts: " + parts.Length);
                        return;
                    }

                    int secretColorIndex = -1;
                    int secretColorId = 0;
                    int i = 0;
                    foreach (PlayerInfo player in Players)
                    {
                        if (player.ForcedColor > 0)
                        {
                            secretColorIndex = i;
                            secretColorId = player.ForcedColor;
                        }
                        i++;
                    }

                    Players.Clear();
                    AIPlayers.Clear();

                    int pId = 0;
                    while (pId < parts.Length)
                    {
                        string pName = parts[pId];
                        int sideId = Convert.ToInt32(parts[pId + 1]);
                        int colorId = Convert.ToInt32(parts[pId + 2]);
                        int startLocId = Convert.ToInt32(parts[pId + 3]);
                        int teamId = Convert.ToInt32(parts[pId + 4]);
                        bool isAI = Convert.ToBoolean(Convert.ToInt32(parts[pId + 5]));

                        pId = pId + 6;

                        if (!isAI)
                        {
                            Players.Add(new PlayerInfo(pName, sideId, startLocId, colorId, teamId));
                        }
                        else
                        {
                            AIPlayers.Add(new PlayerInfo(pName, sideId, startLocId, colorId, teamId));
                        }
                    }

                    for (int plId = 0; plId < Players.Count; plId++)
                    {
                        Players[plId].Ready = false;
                    }

                    if (secretColorIndex > -1)
                        Players[secretColorIndex].ForcedColor = secretColorId;

                    CopyPlayerDataToUI();
                }
                else if (message.StartsWith("GOPTS"))
                {
                    message = message.Substring(6);
                    string[] parts = message.Split(';');

                    // Bounds check
                    if (parts.Length != CheckBoxes.Count + ComboBoxes.Count + 3)
                    {
                        MessageBox.Show("The game host has sent an invalid options message. " +
                            "This usually happens when the game host is using a different client/game version than you." +
                            Environment.NewLine + Environment.NewLine +
                            "You will be unable to participate in this match.", "Communication error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        btnLeaveGame.PerformClick();
                        return;
                    }

                    for (int chkId = 0; chkId < CheckBoxes.Count; chkId++)
                    {
                        if (Convert.ToBoolean(Convert.ToInt32(parts[chkId])))
                            CheckBoxes[chkId].Checked = true;
                        else
                            CheckBoxes[chkId].Checked = false;
                    }
                    int initialId = CheckBoxes.Count;
                    for (int cmbId = 0; cmbId < ComboBoxes.Count; cmbId++)
                    {
                        int index = Convert.ToInt32(parts[initialId + cmbId]);
                        ComboBoxes[cmbId].SelectedIndex = index;
                    }
                    initialId = CheckBoxes.Count + ComboBoxes.Count;
                    string mapMD5 = parts[initialId];
                    Map map = getMapFromMD5(mapMD5);
                    if (map == null)
                    {
                        MessageBox.Show("The host has selected a map that isn't located on your installation. " + Environment.NewLine +
                            "You will be unable to participate in this match.");
                        btnLeaveGame.PerformClick();
                        return;
                    }
                    else
                    {
                        currentMap = map;
                        lblMapName.Text = "Map: " + map.Name;
                        lblMapAuthor.Text = "By " + map.Author;
                        LoadPreview();
                    }
                    string gameMode = parts[initialId + 1];
                    if (CnCNetData.GameTypes.Contains(gameMode))
                    {
                        if (map.GameModes.Contains(gameMode))
                            lblGameMode.Text = "Game Mode: " + gameMode;
                        else
                        {
                            MessageBox.Show("The specified map isn't valid for this game mode." + Environment.NewLine +
                                "You will be unable to participate in this match.");
                            btnLeaveGame.PerformClick();
                            return;
                        }
                    }
                    currentGameMode = gameMode;
                    LockOptions();
                    int seed = Convert.ToInt32(parts[initialId + 2]);
                    Seed = seed;

                    for (int plId = 0; plId < Players.Count; plId++)
                    {
                        Players[plId].Ready = false;
                    }

                    CopyPlayerDataToUI();
                }
                else if (message.StartsWith("PREADY"))
                {
                    if (message.Length < 8)
                    {
                        Logger.Log("Invalid PREADY message received from the host.");
                        return;
                    }

                    message = message.Substring(7);
                    string[] parts = message.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    int numPlayers = parts.Length / 2;
                    for (int pId = 0; pId < numPlayers; pId++)
                    {
                        string name = parts[pId * 2];
                        int index = Players.FindIndex(c => c.Name == name);
                        if (index > -1 || index < Players.Count)
                        {
                            Players[index].Ready = Convert.ToBoolean(Convert.ToInt32(parts[pId * 2 + 1]));
                        }
                        else
                        {
                            Logger.Log("Warning: Invalid PREADY index for " + name);
                        }
                    }
                    CopyPlayerDataToUI();
                }
                else if (message.StartsWith("START"))
                {
                    message = message.Substring(6);
                    string[] parts = message.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    string[] tunnelParts = parts[0].Split(':');
                    tunnelAddress = tunnelParts[0];
                    tunnelPort = Convert.ToInt32(tunnelParts[1]);
                    int numPlayers = (parts.Length - 1) / 2;
                    List<int> pPorts = new List<int>();
                    for (int pId = 0; pId < numPlayers; pId++)
                    {
                        string[] playerAddressAndPort = parts[1 + (pId * 2 + 1)].Split(':');
                        int port = Convert.ToInt32(playerAddressAndPort[1]);
                        pPorts.Add(port);
                    }
                    isHostInGame = true;
                    lbChatBox.Enabled = false;
                    btnLaunchGame.Enabled = false;
                    tbChatInputBox.Enabled = false;
                    StartGame(pPorts);
                }
                else if (message.StartsWith("GETREADY"))
                {
                    AddNotice("The host wants to start the game but cannot because not all players are ready!");
                    WindowFlasher.FlashWindowEx(this);
                }
                else if (message.StartsWith("RETURN"))
                {
                    AddNotice("The game host has returned from the game.");
                    isHostInGame = false;
                    lbChatBox.Enabled = true;
                    btnLaunchGame.Enabled = true;
                    tbChatInputBox.Enabled = true;
                }
                else if (message.StartsWith("INGAME"))
                {
                    if (message.Length < 8)
                    {
                        Logger.Log("Invalid INGAME message received from the host.");
                        return;
                    }

                    int pId = Convert.ToInt32(message.Substring(7));
                    AddNotice("Unable to launch game: player " + Players[pId].Name + " is still playing the game you started previously.");
                }
                else if (message.StartsWith("INVSTART"))
                {
                    if (message.Length < 10)
                    {
                        Logger.Log("Invalid INVSTART message received from the host.");
                        return;
                    }

                    int pId = Convert.ToInt32(message.Substring(9));
                    AddNotice("Unable to launch game: player " + Players[pId].Name + " has an invalid starting location.");
                }
                else if (message.StartsWith("INVAISTART"))
                {
                    if (message.Length < 12)
                    {
                        Logger.Log("Invalid INVAISTART message received from the host.");
                        return;
                    }

                    int aiId = Convert.ToInt32(message.Substring(11));
                    AddNotice("Unable to launch game: AI player " + aiId + " has an invalid starting location.");
                }
                else if (message == "TMPLAYERS")
                {
                    AddNotice("Unable to launch game: this map cannot be played with more than " + currentMap.AmountOfPlayers + " players.");
                }
                else if (message == "INFSPLAYERS")
                {
                    AddNotice("Unable to launch game: this map cannot be played with fewer than " + currentMap.MinPlayers + " players.");
                }
                else if (message == "SAMESTARTLOC")
                {
                    AddNotice("Multiple players cannot share the same starting location on this map.");
                }
                else if (message == "SAMECOLOR")
                {
                    AddNotice("Multiple human players cannot share the same color.");
                }
                else if (message == "AISPECS")
                {
                    AddNotice("Why would you want an AI player to spectate your match? It won't record it for you.");
                }
                else if (message == "COOPSPECS")
                {
                    AddNotice("Co-op missions cannot be spectated. You'll have to show a bit more effort to cheat here.");
                }
                else if (message.StartsWith("SECRETCOLOR"))
                {
                    int index = Players.FindIndex(c => c.Name == "Rampastring");
                    if (index > -1)
                    {
                        Players[index].ForcedColor = Convert.ToInt32(message.Substring(12));
                        Logger.Log("Secret color succesfully applied.");

                        if (ProgramConstants.CNCNET_PLAYERNAME == "Rampastring")
                            AddNotice("Secret color applied succesfully.");
                    }
                    else
                    {
                        Logger.Log("Setting secret color failed.");
                    }
                }
                else if (message.StartsWith("INVVER"))
                {
                    if (message.Length < 8)
                    {
                        Logger.Log("Invalid INVVER message received from the host.");
                        return;
                    }

                    string offenderAndVersion = message.Substring(7);
                    string[] parts = message.Split(' ');
                    string offender = parts[1];
                    string version = "Unknown";

                    if (parts.Length > 1)
                        version = parts[2];

                    AddNotice(string.Format("Warning: Player {0} is running an incompatible version {1}. This may cause crashes and/or synchronization errors while in-game.",
                        offender, version));
                }
                else if (message.StartsWith("MAPMOD"))
                {
                    if (message.Length < 8)
                    {
                        Logger.Log("Invalid MAPMOD message received from the host.");
                        return;
                    }

                    string offender = message.Substring(7);
                }
            }
        }

        /// <summary>
        /// Gets a map from the internal map list based on the map's MD5.
        /// </summary>
        /// <param name="md5">The MD5 of the map to search for.</param>
        /// <returns>The map if a matching MD5 was found, otherwise null.</returns>
        private Map getMapFromMD5(string md5)
        {
            foreach (Map map in CnCNetData.MapList)
            {
                if (map.SHA1 == md5)
                    return map;
            }

            return null;
        }

        private void Instance_PrivmsgParsed(string channelName, string message, string sender)
        {
            if (this.InvokeRequired)
            {
                // Necessary for thread-safety
                TripleStringCallback d = new TripleStringCallback(Instance_DoPrivmsgParsed);
                this.BeginInvoke(d, channelName, message, sender);
                return;
            }

            Instance_DoPrivmsgParsed(channelName, message, sender);
        }

        /// <summary>
        /// Called when a chat message is parsed.
        /// </summary>
        /// <param name="channelName">The name of the channel that received the chat message.</param>
        /// <param name="message">The chat message itself.</param>
        /// <param name="sender">The sender of the chat message.</param>
        private void Instance_DoPrivmsgParsed(string channelName, string message, string sender)
        {
            if (channelName == ChannelName)
            {
                Color foreColor;

                // Parse color info
                if (message.Contains(Convert.ToString((char)03)))
                {
                    string colorString = message.Substring(1, 2);
                    message = message.Remove(0, 3);
                    int colorIndex = Convert.ToInt32(colorString);
                    foreColor = ChatColors[colorIndex];
                }
                else
                    foreColor = defaultChatColor;

                if (!DisableSoundsBox.Checked)
                    sndMessageSound.Play();

                MessageColors.Add(foreColor);
                DateTime now = DateTime.Now;
                lbChatBox.Items.Add("[" + now.ToShortTimeString() + "] " + sender + ": " + message);
                ScrollListbox();
            }
        }

        /// <summary>
        /// Sets up channel modes and information on the tunnel server used by the game.
        /// </summary>
        /// <param name="_tunnelAddress">The IP address of the tunnel server to use for the game.</param>
        /// <param name="_tunnelPort">The port of the tunnel server to use for the game.</param>
        public void Initialize(string _tunnelAddress, int _tunnelPort)
        {
            Logger.Log("Initializing game lobby.");
            CnCNetData.ConnectionBridge.SendMessage(string.Format("MODE {0} +k {1}", ChannelName, Password)); // set password
            CnCNetData.ConnectionBridge.SendMessage(string.Format("MODE {0} +l {1}", ChannelName, playerLimit)); // set channel member limit
            CnCNetData.ConnectionBridge.SendMessage(string.Format("MODE {0} +nN", ChannelName)); // prevent nickname changes
            CnCNetData.ConnectionBridge.SendMessage(string.Format("TOPIC {0} :{1}", ChannelName, ProgramConstants.CNCNET_PROTOCOL_REVISION + ";" + defaultGame));
            tunnelAddress = _tunnelAddress;
            tunnelPort = _tunnelPort;
        }

        private void Instance_OnUserQuit(string message)
        {
            if (this.InvokeRequired)
            {
                // Necessary for thread-safety
                StringCallback d = new StringCallback(Instance_DoUserQuit);
                this.BeginInvoke(d, message);
                return;
            }

            Instance_DoUserQuit(message);
        }

        /// <summary>
        /// Called when an user quits CnCNet and, as such, potentially the game room we are in.
        /// </summary>
        /// <param name="userName">The name of the user who left CnCNet.</param>
        private void Instance_DoUserQuit(string userName)
        {
            int index = Players.FindIndex(c => c.Name == userName);

            if (index > -1)
            {
                Logger.Log("Player quit CnCNet: " + userName);

                AddNotice(userName + " has quit CnCNet.");
                sndLeaveSound.Play();

                if (isHost)
                {
                    Players.RemoveAt(index);
                    CopyPlayerDataToUI();
                    CopyPlayerDataFromUI(null, EventArgs.Empty);

                    if (Locked && !ProgramConstants.IsInGame)
                    {
                        UnlockGame(true);
                    }
                }
                else
                {
                    if (userName == AdminName)
                    {
                        if (!ProgramConstants.IsInGame)
                        {
                            if (!hostLeftChannel)
                            {
                                Logger.Log("Host has quit CnCNet - leaving.");
                                MessageBox.Show("The game host has quit CnCNet.", "Host quit");
                                btnLeaveGame.PerformClick();
                            }

                            return;
                        }
                        else
                            hasHostLeft = true;
                    }
                }
            }
        }

        private void Instance_OnUserLeaveChannel(string channelName, string userName)
        {
            if (this.InvokeRequired)
            {
                // Necessary for thread-safety
                DualStringCallback d = new DualStringCallback(Instance_DoUserLeaveChannel);
                this.BeginInvoke(d, channelName, userName);
                return;
            }

            Instance_DoUserLeaveChannel(channelName, userName);
        }

        /// <summary>
        /// Called whenever an user leaves a channel.
        /// Used for detecting when a player leaves the game room.
        /// </summary>
        /// <param name="channelName">The name of the channel that an user leaves from.</param>
        /// <param name="userName">The name of the user who is leaving a channel.</param>
        private void Instance_DoUserLeaveChannel(string channelName, string userName)
        {
            if (leaving)
                return;
            
            if (channelName != ChannelName)
                return;

            Logger.Log("Player left: " + userName);

            AddNotice(userName + " has left the game.");
            sndLeaveSound.Play();

            if (isHost)
            {
                int index = Players.FindIndex(c => c.Name == userName);
                if (index > -1)
                {
                    Players.RemoveAt(index);
                    CopyPlayerDataToUI();
                    CopyPlayerDataFromUI(null, EventArgs.Empty);

                    if (Locked && !ProgramConstants.IsInGame)
                    {
                        UnlockGame(true);
                    }
                }
                else
                    Logger.Log("WARNING: No player data found for " + userName);
            }
            else
            {
                if (userName == AdminName)
                {
                    if (!ProgramConstants.IsInGame)
                    {
                        hostLeftChannel = true;
                        Logger.Log("Host has left the game - leaving.");
                        MessageBox.Show("The game host has left the game.", "Host left");
                        btnLeaveGame.PerformClick();
                        return;
                    }
                    else
                        hasHostLeft = true;
                }
            }
        }

        private void Instance_OnUserJoinedChannel(string channelName, string userName, string ipAddress)
        {
            if (this.InvokeRequired)
            {
                // Necessary for thread-safety
                TripleStringCallback d = new TripleStringCallback(Instance_DoUserJoinedChannel);
                this.BeginInvoke(d, channelName, userName, ipAddress);
                return;
            }

            Instance_DoUserJoinedChannel(channelName, userName, ipAddress);
        }

        /// <summary>
        /// Executed when an user joins a channel.
        /// Used for detecting when a player joins the game room.
        /// </summary>
        /// <param name="channelName">The name of the channel that the user joined to.</param>
        /// <param name="userName">The name of the user.</param>
        /// <param name="ipAddress">The IP address of the user. Usually hidden.</param>
        private void Instance_DoUserJoinedChannel(string channelName, string userName, string ipAddress)
        {
            if (channelName == ChannelName)
            {
                Logger.Log("Player joined: " + userName);
                AddNotice(userName + " has joined the game.");
                WindowFlasher.FlashWindowEx(this);
                sndJoinSound.Play();

                if (isHost)
                {
                    if (userName == ProgramConstants.CNCNET_PLAYERNAME)
                        return;

                    PlayerInfo player = new PlayerInfo(userName, 0, 0, 0, 0);
                    player.IsAI = false;
                    player.Ready = false;
                    player.IPAddress = ipAddress;

                    Players.Add(player);

                    if (Players.Count + AIPlayers.Count > 8)
                        AIPlayers.RemoveAt(0);

                    CopyPlayerDataToUI();
                    GenericGameOptionChanged(null, EventArgs.Empty);
                    CopyPlayerDataFromUI(null, EventArgs.Empty);
                    if (!gameOptionRefreshTimer.Enabled)
                        gameOptionRefreshTimer.Start();

                    if (Players.Count > playerLimit - 1)
                    {
                        AddNotice("Player limit reached; the game room has been locked.");
                        LockGame(false);
                    }
                }
            }
        }

        /// <summary>
        /// Adds a white notice message into the chat list box.
        /// </summary>
        /// <param name="message">The message to add.</param>
        private void AddNotice(string message)
        {
            MessageColors.Add(Color.White);
            lbChatBox.Items.Add(message);
            ScrollListbox();
        }

        /// <summary>
        /// Refreshes this game room's listing on CnCNet.
        /// </summary>
        private void UpdateGameListing(object sender, EventArgs e)
        {
            if (!isHost)
                return;

            StringBuilder sb;
            
            sb = new StringBuilder("NOTICE " + "#" + defaultGame + "-games" +  " " + CTCPChar1 + CTCPChar2 + "GAME ");

            sb.Append(ProgramConstants.CNCNET_PROTOCOL_REVISION + ";");
            sb.Append(ProgramConstants.GAME_VERSION);
            sb.Append(";");
            sb.Append(playerLimit);
            sb.Append(";");
            sb.Append(ChannelName);
            sb.Append(";");
            sb.Append(GameRoomName);
            sb.Append(";");
            if (Locked)
                sb.Append("1");
            else
                sb.Append("0");
            if (isCustomPassword)
                sb.Append("1");
            else
                sb.Append("0");
            if (leaving)
                sb.Append("1");
            else
                sb.Append("0");
            sb.Append(";");
            foreach (PlayerInfo player in Players)
            {
                sb.Append(player.Name);
                sb.Append(",");
            }
            sb.Append(";");
            sb.Append(currentMap.Name);
            sb.Append(";");
            sb.Append(currentGameMode);
            sb.Append(";");
            sb.Append(CTCPChar2);

            string messageToSend = sb.ToString();
            CnCNetData.ConnectionBridge.SendMessage(messageToSend);

            if (!leaving)
                timer.Enabled = true;
        }

        private int getSideIndexByName(string[] sides, string sideName)
        {
            for (int sId = 0; sId < sides.Length; sId++)
            {
                if (sideName == sides[sId])
                    return sId;
            }

            return -1;
        }

        /// <summary>
        /// Creates new player ready boxes and adds them to the player options panel.
        /// </summary>
        /// <param name="cLabelColor">Color of labels in the UI.</param>
        /// <param name="cAltUiColor">Color of highlighted items in the UI.</param>
        private void InitReadyBoxes(Color cLabelColor, Color cAltUiColor)
        {
            chkP1Ready = new UserCheckBox(cLabelColor, cAltUiColor, "Ready");
            chkP1Ready.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkP1Ready.AutoSize = true;
            chkP1Ready.Location = new Point(498, 26);
            chkP1Ready.Name = "chkP1Ready";
            panel1.Controls.Add(chkP1Ready);

            chkP2Ready = new UserCheckBox(cLabelColor, cAltUiColor, "Ready");
            chkP2Ready.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkP2Ready.AutoSize = true;
            chkP2Ready.Location = new Point(498, 52);
            chkP2Ready.Name = "chkP2Ready";
            panel1.Controls.Add(chkP2Ready);

            chkP3Ready = new UserCheckBox(cLabelColor, cAltUiColor, "Ready");
            chkP3Ready.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkP3Ready.AutoSize = true;
            chkP3Ready.Location = new Point(498, 78);
            chkP3Ready.Name = "chkP3Ready";
            panel1.Controls.Add(chkP3Ready);

            chkP4Ready = new UserCheckBox(cLabelColor, cAltUiColor, "Ready");
            chkP4Ready.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkP4Ready.AutoSize = true;
            chkP4Ready.Location = new Point(498, 105);
            chkP4Ready.Name = "chkP4Ready";
            panel1.Controls.Add(chkP4Ready);

            chkP5Ready = new UserCheckBox(cLabelColor, cAltUiColor, "Ready");
            chkP5Ready.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkP5Ready.AutoSize = true;
            chkP5Ready.Location = new Point(498, 131);
            chkP5Ready.Name = "chkP5Ready";
            panel1.Controls.Add(chkP5Ready);

            chkP6Ready = new UserCheckBox(cLabelColor, cAltUiColor, "Ready");
            chkP6Ready.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkP6Ready.AutoSize = true;
            chkP6Ready.Location = new Point(498, 157);
            chkP6Ready.Name = "chkP6Ready";
            panel1.Controls.Add(chkP6Ready);

            chkP7Ready = new UserCheckBox(cLabelColor, cAltUiColor, "Ready");
            chkP7Ready.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkP7Ready.AutoSize = true;
            chkP7Ready.Location = new Point(498, 183);
            chkP7Ready.Name = "chkP7Ready";
            panel1.Controls.Add(chkP7Ready);

            chkP8Ready = new UserCheckBox(cLabelColor, cAltUiColor, "Ready");
            chkP8Ready.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkP8Ready.AutoSize = true;
            chkP8Ready.Location = new Point(498, 209);
            chkP8Ready.Name = "chkP8Ready";
            panel1.Controls.Add(chkP8Ready);
        }

        /// <summary>
        /// Generic function for customized drawing of items in player color combo boxes.
        /// </summary>
        private void cmbPXColor_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            if (e.Index > -1 && e.Index < cmbP1Color.Items.Count)
                e.Graphics.DrawString(cmbP1Color.Items[e.Index].ToString(), e.Font, new SolidBrush(MPColors[e.Index]), e.Bounds);
        }

        /// <summary>
        /// Generic function for customized drawing of combo box items.
        /// </summary>
        private void cmbGeneric_DrawItem(object sender, DrawItemEventArgs e)
        {
            LimitedComboBox comboBox = (LimitedComboBox)sender;
            e.DrawBackground();
            e.DrawFocusRectangle();
            if (e.Index > -1 && e.Index < comboBox.Items.Count)
            {
                if (comboBox.HoveredIndex != e.Index)
                    e.Graphics.DrawString(comboBox.Items[e.Index].ToString(), e.Font, new SolidBrush(comboBox.ForeColor), e.Bounds);
                else
                    e.Graphics.DrawString(comboBox.Items[e.Index].ToString(), e.Font, new SolidBrush(Color.White), e.Bounds);
            }
        }

        private Color getColorFromStringArray(string[] stringArray)
        {
            return Color.FromArgb(Convert.ToInt32(stringArray[0]), Convert.ToInt32(stringArray[1]), Convert.ToInt32(stringArray[2]));
        }

        /// <summary>
        /// Returns a player's name combo box based on the player's index.
        /// </summary>
        /// <param name="id">The index of the player, starting from 1.</param>
        private LimitedComboBox getPlayerNameCMBFromId(int id)
        {
            switch (id)
            {
                case 1:
                    return cmbP1Name;
                case 2:
                    return cmbP2Name;
                case 3:
                    return cmbP3Name;
                case 4:
                    return cmbP4Name;
                case 5:
                    return cmbP5Name;
                case 6:
                    return cmbP6Name;
                case 7:
                    return cmbP7Name;
                case 8:
                    return cmbP8Name;
            }

            return null;
        }

        /// <summary>
        /// Returns a player's side combo box based on the player's index.
        /// </summary>
        /// <param name="id">The index of the player, starting from 1.</param>
        private LimitedComboBox getPlayerSideCMBFromId(int id)
        {
            switch (id)
            {
                case 1:
                    return cmbP1Side;
                case 2:
                    return cmbP2Side;
                case 3:
                    return cmbP3Side;
                case 4:
                    return cmbP4Side;
                case 5:
                    return cmbP5Side;
                case 6:
                    return cmbP6Side;
                case 7:
                    return cmbP7Side;
                case 8:
                    return cmbP8Side;
            }

            return null;
        }

        /// <summary>
        /// Returns a player's color combo box based on the player's index.
        /// </summary>
        /// <param name="id">The index of the player, starting from 1.</param>
        private LimitedComboBox getPlayerColorCMBFromId(int id)
        {
            switch (id)
            {
                case 1:
                    return cmbP1Color;
                case 2:
                    return cmbP2Color;
                case 3:
                    return cmbP3Color;
                case 4:
                    return cmbP4Color;
                case 5:
                    return cmbP5Color;
                case 6:
                    return cmbP6Color;
                case 7:
                    return cmbP7Color;
                case 8:
                    return cmbP8Color;
            }

            return null;
        }

        /// <summary>
        /// Returns a player's starting location combo box based on the player's index.
        /// </summary>
        /// <param name="id">The index of the player, starting from 1.</param>
        private LimitedComboBox getPlayerStartCMBFromId(int id)
        {
            switch (id)
            {
                case 1:
                    return cmbP1Start;
                case 2:
                    return cmbP2Start;
                case 3:
                    return cmbP3Start;
                case 4:
                    return cmbP4Start;
                case 5:
                    return cmbP5Start;
                case 6:
                    return cmbP6Start;
                case 7:
                    return cmbP7Start;
                case 8:
                    return cmbP8Start;
            }

            return null;
        }

        /// <summary>
        /// Returns a player's team combo box based on the player's index.
        /// </summary>
        /// <param name="id">The index of the player, starting from 1.</param>
        private LimitedComboBox getPlayerTeamCMBFromId(int id)
        {
            switch (id)
            {
                case 1:
                    return cmbP1Team;
                case 2:
                    return cmbP2Team;
                case 3:
                    return cmbP3Team;
                case 4:
                    return cmbP4Team;
                case 5:
                    return cmbP5Team;
                case 6:
                    return cmbP6Team;
                case 7:
                    return cmbP7Team;
                case 8:
                    return cmbP8Team;
            }

            return null;
        }

        /// <summary>
        /// Returns a player's ready check box based on the player's index.
        /// </summary>
        /// <param name="id">The index of the player, starting from 1.</param>
        private UserCheckBox getPlayerReadyBoxFromId(int id)
        {
            switch (id)
            {
                case 1:
                    return chkP1Ready;
                case 2:
                    return chkP2Ready;
                case 3:
                    return chkP3Ready;
                case 4:
                    return chkP4Ready;
                case 5:
                    return chkP5Ready;
                case 6:
                    return chkP6Ready;
                case 7:
                    return chkP7Ready;
                case 8:
                    return chkP8Ready;
            }

            return null;
        }

        /// <summary>
        /// Leaves the game room.
        /// </summary>
        private void btnLeaveGame_Click(object sender, EventArgs e)
        {
            leaving = true;
            UpdateGameListing(null, EventArgs.Empty);
            CnCNetData.ConnectionBridge.SendMessage("PART " + ChannelName);
            CnCNetData.IsGameLobbyOpen = false;
            if (isHost)
            {
                timer.Stop();
                timer.Dispose();
            }
            isManualClose = true;
            Unsubscribe();
            this.Close();
            CnCNetData.DoGameLobbyClosed();
        }

        /// <summary>
        /// Copies the player data in the UI to the internal player data in memory.
        /// Also broadcasts player options to all players as host and requests option changes
        /// as a non-host player.
        /// </summary>
        private void CopyPlayerDataFromUI(object sender, EventArgs e)
        {
            if (!updatePlayers)
                return;

            if (!isHost)
            {
                int index = Players.FindIndex(c => c.Name == ProgramConstants.CNCNET_PLAYERNAME);
                if (index > -1)
                {
                    int sideId = getPlayerSideCMBFromId(index + 1).SelectedIndex;

                    if (!isSideAllowed(sideId))
                    {
                        sideId = Players[index].SideId;
                    }

                    int colorId = getPlayerColorCMBFromId(index + 1).SelectedIndex;
                    int startId = getPlayerStartCMBFromId(index + 1).SelectedIndex;
                    int teamId = getPlayerTeamCMBFromId(index + 1).SelectedIndex;

                    StringBuilder sbc = new StringBuilder("PRIVMSG " + ChannelName + " " + CTCPChar1 + CTCPChar2 + "OPTS ");
                    sbc.Append(sideId);
                    sbc.Append(";");
                    sbc.Append(colorId);
                    sbc.Append(";");
                    sbc.Append(startId);
                    sbc.Append(";");
                    sbc.Append(teamId);
                    sbc.Append(CTCPChar2);
                    CnCNetData.ConnectionBridge.SendMessage(sbc.ToString());
                }

                CopyPlayerDataToUI();
                return;
            }

            StringBuilder sb = new StringBuilder("NOTICE " + ChannelName + " " + CTCPChar1 + CTCPChar2 + "POPTS ");

            // Human players
            for (int pId = 0; pId < Players.Count; pId++)
            {
                Players[pId].Name = getPlayerNameCMBFromId(pId + 1).Text;
                getPlayerNameCMBFromId(pId + 1).CanDropDown = false;
                Players[pId].ColorId = getPlayerColorCMBFromId(pId + 1).SelectedIndex;
                int sideId = getPlayerSideCMBFromId(pId + 1).SelectedIndex;

                if (sideId < 0 || !isSideAllowed(sideId))
                    getPlayerSideCMBFromId(pId + 1).SelectedIndex = Players[pId].SideId;
                else
                    Players[pId].SideId = sideId;
                Players[pId].StartingLocation = getPlayerStartCMBFromId(pId + 1).SelectedIndex;
                Players[pId].TeamId = getPlayerTeamCMBFromId(pId + 1).SelectedIndex;
                Players[pId].Ready = false;

                sb.Append(Players[pId].Name);
                sb.Append(";");
                sb.Append(Players[pId].SideId);
                sb.Append(";");
                sb.Append(Players[pId].ColorId);
                sb.Append(";");
                sb.Append(Players[pId].StartingLocation);
                sb.Append(";");
                sb.Append(Players[pId].TeamId);
                sb.Append(";");
                sb.Append("0");
                sb.Append(";");
            }

            // AI players
            AIPlayers.Clear();
            int playerCount = Players.Count;
            for (int cmbId = Players.Count; cmbId < 8; cmbId++)
            {
                LimitedComboBox cmb = getPlayerNameCMBFromId(cmbId + 1);

                if (cmb.SelectedIndex < 1)
                    continue;

                int sideId = getPlayerSideCMBFromId(cmbId + 1).SelectedIndex;
                AIPlayers.Add(new PlayerInfo(cmb.Text,
                sideId,
                getPlayerStartCMBFromId(cmbId + 1).SelectedIndex,
                getPlayerColorCMBFromId(cmbId + 1).SelectedIndex,
                getPlayerTeamCMBFromId(cmbId + 1).SelectedIndex));

                int aiIndex = AIPlayers.Count - 1;
                PlayerInfo aiPlayer = AIPlayers[aiIndex];

                if (sideId < 0 || !isSideAllowed(sideId))
                    getPlayerSideCMBFromId(cmbId + 1).SelectedIndex = aiPlayer.SideId;
                else
                    aiPlayer.SideId = sideId;

                if (aiPlayer.SideId == -1)
                    aiPlayer.SideId = 0;
                if (aiPlayer.StartingLocation == -1)
                    aiPlayer.StartingLocation = 0;
                if (aiPlayer.ColorId == -1)
                    aiPlayer.ColorId = 0;
                if (aiPlayer.TeamId == -1)
                    aiPlayer.TeamId = 0;

                sb.Append(aiPlayer.Name);
                sb.Append(";");
                sb.Append(aiPlayer.SideId);
                sb.Append(";");
                sb.Append(aiPlayer.ColorId);
                sb.Append(";");
                sb.Append(aiPlayer.StartingLocation);
                sb.Append(";");
                sb.Append(aiPlayer.TeamId);
                sb.Append(";");
                sb.Append("1");
                sb.Append(";");
            }

            string messageToSend = sb.ToString();
            messageToSend = messageToSend.Remove(messageToSend.Length - 1);
            messageToSend = messageToSend + CTCPChar2;

            CnCNetData.ConnectionBridge.SendMessage(messageToSend);

            CopyPlayerDataToUI();
        }

        /// <summary>
        /// Checks if a side is allowed to be used.
        /// </summary>
        /// <param name="sideId">The index of the side in the side list combobox.
        /// Side indexes start from 1 (0 = random).</param>
        private bool isSideAllowed(int sideId)
        {
            if (sideId != 0 && sideId != SideComboboxPrerequisites.Count + 1)
            {
                SideComboboxPrerequisite prereq = SideComboboxPrerequisites[sideId - 1];
                if (prereq.Valid)
                {
                    int comboBoxId = prereq.ComboBoxId;
                    int requiredIndexId = prereq.RequiredIndexId;

                    if (ComboBoxes[comboBoxId].SelectedIndex != requiredIndexId)
                    {
                        AddNotice(ComboBoxSidePrereqErrorDescriptions[comboBoxId] + " must be set for playing as " +
                            cmbP1Side.Items[sideId].ToString() + " to be allowed.");
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Applies all player data stored in memory to the user-interface.
        /// </summary>
        private void CopyPlayerDataToUI()
        {
            updatePlayers = false;

            // Clear dynamic preview display
            for (int id = 0; id < 8; id++)
            {
                PlayerColorsOnPlayerLocations[id].Clear();
                PlayerNamesOnPlayerLocations[id].Clear();
            }

            // Human players
            for (int pId = 0; pId < Players.Count; pId++)
            {
                LimitedComboBox lcb = getPlayerNameCMBFromId(pId + 1);
                lcb.DropDownStyle = ComboBoxStyle.DropDown;
                lcb.Text = Players[pId].Name;
                lcb.CanDropDown = false;
                lcb.Enabled = false;
                lcb.Visible = false;
                pNameTextBoxes[pId].Visible = true;
                pNameTextBoxes[pId].Enabled = true;
                pNameTextBoxes[pId].Text = Players[pId].Name;
                LimitedComboBox sideBox = getPlayerSideCMBFromId(pId + 1);
                sideBox.SelectedIndex = Players[pId].SideId;
                sideBox.Enabled = true;
                LimitedComboBox colorBox = getPlayerColorCMBFromId(pId + 1);
                colorBox.SelectedIndex = Players[pId].ColorId;
                colorBox.Enabled = true;
                LimitedComboBox startBox = getPlayerStartCMBFromId(pId + 1);
                startBox.SelectedIndex = Players[pId].StartingLocation;
                if (startBox.SelectedIndex > 0)
                {
                    // Add info to the dynamic preview display
                    if (Players[pId].TeamId == 0)
                        PlayerNamesOnPlayerLocations[startBox.SelectedIndex - 1].Add(Players[pId].Name);
                    else
                        PlayerNamesOnPlayerLocations[startBox.SelectedIndex - 1].Add(TeamIdentifiers[Players[pId].TeamId - 1] + Players[pId].Name);

                    PlayerColorsOnPlayerLocations[startBox.SelectedIndex - 1].Add(Players[pId].ColorId);
                }
                startBox.Enabled = true;
                LimitedComboBox teamBox = getPlayerTeamCMBFromId(pId + 1);
                teamBox.SelectedIndex = Players[pId].TeamId;
                teamBox.Enabled = true;
                UserCheckBox readyBox = getPlayerReadyBoxFromId(pId + 1);
                readyBox.Checked = Players[pId].Ready;
                readyBox.Enabled = true;
                readyBox.IsEnabled = false;

                if (!isHost && lcb.Text != ProgramConstants.CNCNET_PLAYERNAME)
                {
                    sideBox.CanDropDown = false;
                    colorBox.CanDropDown = false;
                    startBox.CanDropDown = false;
                    teamBox.CanDropDown = false;
                }
                else
                {
                    sideBox.CanDropDown = true;
                    colorBox.CanDropDown = true;
                    startBox.CanDropDown = true;
                    teamBox.CanDropDown = true;
                }
            }

            // AI players
            int playerCount = Players.Count;
            for (int aiId = 0; aiId < AIPlayers.Count; aiId++)
            {
                int index = playerCount + aiId + 1;
                LimitedComboBox lcb = getPlayerNameCMBFromId(index);
                lcb.Text = AIPlayers[aiId].Name;
                lcb.Enabled = true;
                lcb.DropDownStyle = ComboBoxStyle.DropDownList;
                lcb.Visible = true;
                pNameTextBoxes[index - 1].Visible = false;
                pNameTextBoxes[index - 1].Enabled = false;
                LimitedComboBox sideBox = getPlayerSideCMBFromId(index);
                sideBox.SelectedIndex = AIPlayers[aiId].SideId;
                sideBox.Enabled = true;
                LimitedComboBox colorBox = getPlayerColorCMBFromId(index);
                colorBox.SelectedIndex = AIPlayers[aiId].ColorId;
                colorBox.Enabled = true;
                LimitedComboBox startBox = getPlayerStartCMBFromId(index);
                startBox.SelectedIndex = AIPlayers[aiId].StartingLocation;
                startBox.Enabled = true;
                if (startBox.SelectedIndex > 0)
                {
                    // Add info to the dynamic preview display
                    if (AIPlayers[aiId].TeamId == 0)
                        PlayerNamesOnPlayerLocations[startBox.SelectedIndex - 1].Add(AIPlayers[aiId].Name);
                    else
                        PlayerNamesOnPlayerLocations[startBox.SelectedIndex - 1].Add(TeamIdentifiers[AIPlayers[aiId].TeamId - 1] + AIPlayers[aiId].Name);

                    PlayerColorsOnPlayerLocations[startBox.SelectedIndex - 1].Add(AIPlayers[aiId].ColorId);
                }
                LimitedComboBox teamBox = getPlayerTeamCMBFromId(index);
                teamBox.SelectedIndex = AIPlayers[aiId].TeamId;
                teamBox.Enabled = true;
                UserCheckBox readyBox = getPlayerReadyBoxFromId(index);
                readyBox.Checked = true;
                readyBox.Enabled = false;

                if (!isHost)
                {
                    lcb.CanDropDown = false;
                    sideBox.CanDropDown = false;
                    colorBox.CanDropDown = false;
                    startBox.CanDropDown = false;
                    teamBox.CanDropDown = false;
                }
                else
                {
                    lcb.CanDropDown = true;
                    sideBox.CanDropDown = true;
                    colorBox.CanDropDown = true;
                    startBox.CanDropDown = true;
                    teamBox.CanDropDown = true;
                }
            }

            // Unused slots
            for (int cmbId = Players.Count + AIPlayers.Count + 1; cmbId < 9; cmbId++)
            {
                LimitedComboBox lcb = getPlayerNameCMBFromId(cmbId);
                lcb.Visible = true;
                if (cmbId == Players.Count + AIPlayers.Count + 1)
                {
                    if (isHost)
                    {
                        lcb.CanDropDown = true;
                        lcb.Enabled = true;
                    }
                    else
                    {
                        lcb.Enabled = false;
                    }
                }
                else
                {
                    lcb.Enabled = false;
                }

                lcb.DropDownStyle = ComboBoxStyle.DropDownList;
                lcb.SelectedIndex = -1;
                lcb.Text = String.Empty;

                pNameTextBoxes[cmbId - 1].Visible = false;
                pNameTextBoxes[cmbId - 1].Enabled = false;

                LimitedComboBox sideBox = getPlayerSideCMBFromId(cmbId);
                sideBox.SelectedIndex = -1;
                sideBox.Enabled = false;
                LimitedComboBox colorBox = getPlayerColorCMBFromId(cmbId);
                colorBox.SelectedIndex = -1;
                colorBox.Enabled = false;
                LimitedComboBox startBox = getPlayerStartCMBFromId(cmbId);
                startBox.SelectedIndex = -1;
                startBox.Enabled = false;
                LimitedComboBox teamBox = getPlayerTeamCMBFromId(cmbId);
                teamBox.SelectedIndex = -1;
                teamBox.Enabled = false;
                UserCheckBox readyBox = getPlayerReadyBoxFromId(cmbId);
                readyBox.Checked = false;
                readyBox.Enabled = false;
            }

            updatePlayers = true;

            // Re-draw the preview
            pbPreview.Refresh();
        }

        /// <summary>
        /// Broadcasts game options to all players whenever a check box's Checked status is changed.
        /// </summary>
        private void GenericChkBox_CheckedChanged()
        {
            if (updateGameOptions)
                GenericGameOptionChanged(null, EventArgs.Empty);
        }

        /// <summary>
        /// Broadcasts game options to all players.
        /// </summary>
        private void GenericGameOptionChanged(object sender, EventArgs e)
        {
            if (!isHost || !updateGameOptions)
                return;

            StringBuilder sb = new StringBuilder("NOTICE " + ChannelName + " " + CTCPChar1 + CTCPChar2 + "GOPTS ");
            for (int chkId = 0; chkId < CheckBoxes.Count; chkId++)
            {
                sb.Append(Convert.ToInt32(CheckBoxes[chkId].Checked));
                sb.Append(";");
            }
            for (int cmbId = 0; cmbId < ComboBoxes.Count; cmbId++)
            {
                sb.Append(ComboBoxes[cmbId].SelectedIndex);
                sb.Append(";");
            }
            if (currentMap == null)
            {
                sb.Append("none;");
                sb.Append("none;");
            }
            else
            {
                sb.Append(currentMap.SHA1);
                sb.Append(";");
                sb.Append(currentGameMode);
                sb.Append(";");
            }
            sb.Append(Seed); // seed
            sb.Append(CTCPChar2);

            for (int pId = 0; pId < Players.Count; pId++)
            {
                Players[pId].Ready = false;
            }

            CopyPlayerDataToUI();

            string messageToSend = sb.ToString();
            CnCNetData.ConnectionBridge.SendMessage(messageToSend);

            if (gameOptionRefreshTimer.Enabled)
                gameOptionRefreshTimer.Stop();
        }

        /// <summary>
        /// Called when the game lobby form is closed.
        /// </summary>
        private void NGameLobby_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!isManualClose)
            {
                leaving = true;
                UpdateGameListing(null, EventArgs.Empty);
                CnCNetData.ConnectionBridge.SendMessage("PART " + ChannelName);
                CnCNetData.IsGameLobbyOpen = false;
                if (isHost)
                {
                    timer.Stop();
                    timer.Dispose();
                }
                Unsubscribe();
                CnCNetData.DoGameLobbyClosed();
            }
        }

        /// <summary>
        /// Unsubscribes from events and disposes resources. Call when the form is closed
        /// to prevent memory leaks.
        /// </summary>
        private void Unsubscribe()
        {
            CnCNetData.ConnectionBridge.OnUserJoinedChannel -= Instance_OnUserJoinedChannel;
            CnCNetData.ConnectionBridge.OnUserLeaveChannel -= Instance_OnUserLeaveChannel;
            CnCNetData.ConnectionBridge.OnUserQuit -= Instance_OnUserQuit;
            CnCNetData.ConnectionBridge.PrivmsgParsed -= Instance_PrivmsgParsed;
            CnCNetData.ConnectionBridge.OnCtcpParsed -= Instance_OnCtcpParsed;
            NCnCNetLobby.UserListReceived -= NCnCNetLobby_UserListReceived;
            NCnCNetLobby.OnColorChanged -= NCnCNetLobby_OnColorChanged;

            btn133px.Dispose();
            btn133px_c.Dispose();
            imgKick.Dispose();
            imgKick_c.Dispose();
            imgBan.Dispose();
            imgBan_c.Dispose();
        }

        /// <summary>
        /// Opens the map selection dialog.
        /// </summary>
        private void btnChangeMap_Click(object sender, EventArgs e)
        {
            MapSelectionForm msf = new MapSelectionForm();
            DialogResult dr = msf.ShowDialog();

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                currentMap = CnCNetData.MapList[msf.rtnMapIndex];
                currentGameMode = msf.rtnGameMode;
                lblGameMode.Text = "Game Mode: " + currentGameMode;
                lblMapAuthor.Text = "By " + currentMap.Author;
                lblMapName.Text = "Map: " + currentMap.Name;

                LoadPreview();

                LockOptions();

                GenericGameOptionChanged(sender, e);
            }
        }

        /// <summary>
        /// Parses and applies forced game options. Called when the map is changed.
        /// </summary>
        private void LockOptions()
        {
            this.SuspendLayout();

            foreach (UserCheckBox ucb in CheckBoxes)
            {
                ucb.Enabled = true;
            }

            foreach (LimitedComboBox lcb in ComboBoxes)
            {
                lcb.Enabled = true;
            }

            IniFile lockedOptionsIni = new IniFile(ProgramConstants.gamepath + "INI\\" + currentGameMode + "_ForcedOptions.ini");
            ParseLockedOptionsFromIni(lockedOptionsIni);

            string mapPath = ProgramConstants.gamepath + currentMap.Path;
            string mapCodePath = mapPath.Substring(0, mapPath.Length - 3) + "ini";

            lockedOptionsIni = new IniFile(mapCodePath);
            ParseLockedOptionsFromIni(lockedOptionsIni);

            if (currentMap.IsCoop)
            {
                coopDifficultyLevel = lockedOptionsIni.GetIntValue("ForcedOptions", "CoopDifficultyLevel", 2);

                // If the coop mission forces players to have a specific side, let's show it in the UI
                int forcedPlayerSide = lockedOptionsIni.GetIntValue("CoopInfo", "ForcedPlayerSide", -1);
                if (forcedPlayerSide > -1)
                {
                    foreach (TextBox pSideLabel in pSideLabels)
                    {
                        pSideLabel.Text = cmbP1Side.Items[forcedPlayerSide + 1].ToString();
                        pSideLabel.Visible = true;
                    }

                    foreach (PlayerInfo player in Players)
                    {
                        if (player.SideId == cmbP1Side.Items.Count - 1)
                            player.SideId = 0;
                    }

                    foreach (PlayerInfo aiPlayer in AIPlayers)
                    {
                        if (aiPlayer.SideId == cmbP1Side.Items.Count - 1)
                            aiPlayer.SideId = 0;
                    }

                    if (isHost)
                        CopyPlayerDataToUI();

                    SetSideComboBoxesVisibility(false);
                }
                else
                {
                    // If the coop mission doesn't force a side, show the usual combo boxes

                    foreach (TextBox pSideLabel in pSideLabels)
                    {
                        pSideLabel.Visible = false;
                    }

                    SetSideComboBoxesVisibility(true);
                }

                lblPlayerTeam.Visible = false;
                cmbP1Team.Visible = false;
                cmbP2Team.Visible = false;
                cmbP3Team.Visible = false;
                cmbP4Team.Visible = false;
                cmbP5Team.Visible = false;
                cmbP6Team.Visible = false;
                cmbP7Team.Visible = false;
                cmbP8Team.Visible = false;
            }
            else
            {
                // For non-coop missions we'll show all options as usual

                foreach (TextBox pSideLabel in pSideLabels)
                {
                    pSideLabel.Visible = false;
                }

                SetSideComboBoxesVisibility(true);

                lblPlayerTeam.Visible = true;
                cmbP1Team.Visible = true;
                cmbP2Team.Visible = true;
                cmbP3Team.Visible = true;
                cmbP4Team.Visible = true;
                cmbP5Team.Visible = true;
                cmbP6Team.Visible = true;
                cmbP7Team.Visible = true;
                cmbP8Team.Visible = true;
            }

            this.ResumeLayout();
        }

        /// <summary>
        /// Sets the visibility of all player side combo boxes to true or false.
        /// </summary>
        /// <param name="visible">Whether player side combo boxes should be displayed or not.</param>
        private void SetSideComboBoxesVisibility(bool visible)
        {
            cmbP1Side.Visible = visible;
            cmbP2Side.Visible = visible;
            cmbP3Side.Visible = visible;
            cmbP4Side.Visible = visible;
            cmbP5Side.Visible = visible;
            cmbP6Side.Visible = visible;
            cmbP7Side.Visible = visible;
            cmbP8Side.Visible = visible;
        }

        /// <summary>
        /// Parses and applies forced game options from an INI file.
        /// </summary>
        /// <param name="lockedOptionsIni">The INI file to read and apply the forced game options from.</param>
        private void ParseLockedOptionsFromIni(IniFile lockedOptionsIni)
        {
            // 2. 11. 2014: prevent a million game option broadcasts when changing settings
            updateGameOptions = false;

            SharedUILogic.ParseLockedOptionsFromIni(CheckBoxes, ComboBoxes, lockedOptionsIni);

            updateGameOptions = true;
        }

        /// <summary>
        /// Loads the map preview, adjusts starting location indicator positions
        /// and displays the preview on the image box.
        /// </summary>
        private void LoadPreview()
        {
            PictureBoxSizeMode sizeMode;
            bool success;

            Image previewImg = SharedLogic.LoadPreview(currentMap, out sizeMode, out success);

            pbPreview.SizeMode = sizeMode;

            if (!success)
            {
                pbPreview.Image = previewImg;
                return;
            }

            int x = previewImg.Width;
            int y = previewImg.Height;

            double prRatioX = pbPreview.Size.Width / Convert.ToDouble(x);
            double prRatioY = pbPreview.Size.Height / Convert.ToDouble(y);

            if (prRatioX > prRatioY)
            {
                previewRatioX = prRatioY;
                previewRatioY = prRatioY;
            }
            else
            {
                previewRatioX = prRatioX;
                previewRatioY = prRatioX;
            }

            if (sharpenPreview && !currentMap.StaticPreviewSize)
            {
                if (x < pbPreview.Size.Width && y < pbPreview.Size.Height)
                    pbPreview.Image = previewImg;
                else
                {
                    if (this.WindowState == FormWindowState.Minimized)
                    {
                        pbPreview.Image = previewImg;
                        return;
                    }

                    Image newImage = SharedLogic.ResizeImage(previewImg, pbPreview.Size.Width, pbPreview.Size.Height);

                    double factorX = x / Convert.ToDouble(pbPreview.Size.Width);
                    double factorY = y / Convert.ToDouble(pbPreview.Size.Height);

                    double sharpeningFactor = 1.0;
                    if (factorX > factorY)
                        sharpeningFactor = factorX;
                    else
                        sharpeningFactor = factorY;

                    pbPreview.Image = SharedLogic.Sharpen(newImage, sharpeningFactor);
                }
            }
            else
                pbPreview.Image = previewImg;
        }

        private void tbChatInputBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (string.IsNullOrEmpty(tbChatInputBox.Text))
                return;

            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Sends a chat message if the user presses the ENTER key on the chat input box.
        /// </summary>
        private void tbChatInputBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Return)
            {
                e.Handled = true;

                if (string.IsNullOrEmpty(tbChatInputBox.Text))
                {
                    return;
                }

                if (tbChatInputBox.Text[0] == '/')
                {
                    try
                    {
                        string commandToParse = tbChatInputBox.Text.Substring(1).ToUpper();

                        // Some quick, hacky text commands just for the fun of it
                        if (commandToParse == "UNLOCK" && Locked && isHost)
                        {
                            AddNotice("You've unlocked the game room.");
                            UnlockGame(false);
                        }
                        else if (commandToParse == "LOCK" && !Locked && isHost)
                        {
                            AddNotice("You've locked the game room.");
                            LockGame(false);
                        }
                        else if (commandToParse == "PRIVMSG")
                        {
                            string[] messageParts = commandToParse.Split(' ');
                            string receiver = messageParts[1];
                            int index = receiver.IndexOf(',');
                            if (index > -1)
                                receiver = receiver.Substring(0, index);

                            int pIndex = CnCNetData.players.FindIndex(c => c == receiver);
                            if (pIndex > -1)
                                CnCNetData.ConnectionBridge.SendMessage("PRIVMSG " + receiver + " " + messageParts[2]);
                        }
                        else if (commandToParse == "EXIT")
                        {
                            btnLeaveGame.PerformClick();
                        }
                        else if (commandToParse == "QUIT")
                        {
                            Environment.Exit(0);
                        }
                        else if (commandToParse.StartsWith("SECRETCOLOR"))
                        {
                            if (ProgramConstants.CNCNET_PLAYERNAME != "Rampastring")
                                AddNotice("Tsk, wrong identity given. No extra colors for you.");
                            else
                            {
                                if (isHost)
                                {
                                    int index = Players.FindIndex(c => c.Name == "Rampastring");
                                    if (index > -1)
                                        Players[index].ForcedColor = Convert.ToInt32(commandToParse.Substring(12));
                                }

                                StringBuilder sb = new StringBuilder("NOTICE " + ChannelName + " " + CTCPChar1 + CTCPChar2 + "SECRETCOLOR ");
                                sb.Append(commandToParse.Substring(12));
                                sb.Append(CTCPChar2);
                                CnCNetData.ConnectionBridge.SendMessage(sb.ToString());
                                AddNotice("Transferring secret color.");
                            }
                        }
                        else
                        {
                            AddNotice("Unknown command " + commandToParse);
                        }

                        tbChatInputBox.Clear();

                        return;
                    }
                    catch
                    {
                        AddNotice("Syntax error: an error occured while attempting to parse the given command.");
                        tbChatInputBox.Clear();
                        return;
                    }
                }

                string colorString = Convert.ToString((char)03);
                if (myChatColorId < 10)
                    colorString = colorString + "0" + Convert.ToString(myChatColorId);
                else
                    colorString = colorString + Convert.ToString(myChatColorId);

                string messageToSend = "PRIVMSG " + ChannelName + " " + colorString + tbChatInputBox.Text;
                CnCNetData.ConnectionBridge.SendMessage(messageToSend);
                MessageColors.Add(ChatColors[myChatColorId]);
                DateTime now = DateTime.Now;
                lbChatBox.Items.Add("[" + now.ToShortTimeString() + "] " + ProgramConstants.CNCNET_PLAYERNAME + ": " + tbChatInputBox.Text);
                tbChatInputBox.Clear();
                ScrollListbox();

                if (!DisableSoundsBox.Checked)
                    sndMessageSound.Play();
            }
        }

        /// <summary>
        /// Used for automatic scrolling of the chat list box as new entries are added.
        /// </summary>
        private void ScrollListbox()
        {
            int displayedItems = lbChatBox.DisplayRectangle.Height / lbChatBox.ItemHeight;
            customScrollbar1.Maximum = lbChatBox.Items.Count - Convert.ToInt32(displayedItems * 0.2);
            if (customScrollbar1.Maximum < 0)
                customScrollbar1.Maximum = 1;
            customScrollbar1.Value = customScrollbar1.Maximum;
            lbChatBox.SelectedIndex = lbChatBox.Items.Count - 1;
            lbChatBox.SelectedIndex = -1;
        }

        /// <summary>
        /// Measures entries in the chat message list box.
        /// </summary>
        private void lbChatBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = (int)e.Graphics.MeasureString(lbChatBox.Items[e.Index].ToString(),
                lbChatBox.Font, lbChatBox.Width - 30).Height;
            e.ItemWidth = lbChatBox.Width - 30;
        }

        /// <summary>
        /// Used for manually drawing chat messages in the chat message list box.
        /// </summary>
        private void lbChatBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            if (e.Index > -1 && e.Index < lbChatBox.Items.Count)
            {
                Color foreColor = MessageColors[e.Index];
                e.Graphics.DrawString(lbChatBox.Items[e.Index].ToString(), e.Font, new SolidBrush(foreColor), e.Bounds);
            }
        }

        /// <summary>
        /// Disallows changing the visible player name.
        /// </summary>
        private void cmbPXName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        /// <summary>
        /// Allows the game host to launch the game (and non-hosting players to accept).
        /// Performs various checks related to starting the game, and if the checks pass,
        /// gathers the information necessary for starting the game and starts it.
        /// </summary>
        private void btnLaunchGame_Click(object sender, EventArgs e)
        {
            if (!isHost)
            {
                string messageToSend = "NOTICE " + ChannelName + " " + CTCPChar1 + CTCPChar2 + "READY 1" + CTCPChar2;
                CnCNetData.ConnectionBridge.SendMessage(messageToSend);
                return;
            }

            if (!Locked)
            {
                AddNotice("You need to lock the game room before launching the game.");
                return;
            }

            // Launch game
            // Various failsafe checks

            List<int> occupiedColorIds = new List<int>();
            foreach (PlayerInfo player in Players)
            {
                if (!occupiedColorIds.Contains(player.ColorId) || player.ColorId == 0)
                    occupiedColorIds.Add(player.ColorId);
                else
                {
                    AddNotice("Multiple human players cannot share the same color.");
                    string messageToSend = "NOTICE " + ChannelName + " " + CTCPChar1 + CTCPChar2 + "SAMECOLOR" + CTCPChar2;
                    CnCNetData.ConnectionBridge.SendMessage(messageToSend);
                    return;
                }
            }

            // Prevent AI players from being spectators
            foreach (PlayerInfo aiPlayer in AIPlayers)
            {
                if (aiPlayer.SideId == SideComboboxPrerequisites.Count + 1)
                {
                    AddNotice("Why would you want an AI player to spectate your match? It won't record it for you.");
                    string messageToSend = "NOTICE " + ChannelName + " " + CTCPChar1 + CTCPChar2 + "AISPECS" + CTCPChar2;
                    CnCNetData.ConnectionBridge.SendMessage(messageToSend);
                    return;
                }
            }

            // Prevent spectating in co-op missions
            if (currentMap.IsCoop)
            {
                foreach (PlayerInfo player in Players)
                {
                    if (player.SideId == SideComboboxPrerequisites.Count + 1)
                    {
                        AddNotice("Co-op missions cannot be spectated. You'll have to show a bit more effort to cheat here.");
                        string messageToSend = "NOTICE " + ChannelName + " " + CTCPChar1 + CTCPChar2 + "COOPSPECS" + CTCPChar2;
                        CnCNetData.ConnectionBridge.SendMessage(messageToSend);
                        return;
                    }
                }
            }

            // Prevent multiple players from sharing the same starting location
            if (currentMap.EnforceMaxPlayers)
            {
                foreach (PlayerInfo player in Players)
                {
                    if (player.StartingLocation == 0)
                        continue;

                    int index = Players.FindIndex(p => p.StartingLocation == player.StartingLocation && p.Name != player.Name);
                    if (index > -1)
                    {
                        AddNotice("Multiple players cannot share the same starting location on this map.");
                        string messageToSend = "NOTICE " + ChannelName + " " + CTCPChar1 + CTCPChar2 + "SAMESTARTLOC" + CTCPChar2;
                        CnCNetData.ConnectionBridge.SendMessage(messageToSend);
                        return;
                    }

                    index = AIPlayers.FindIndex(p => p.StartingLocation == player.StartingLocation);

                    if (index > -1)
                    {
                        AddNotice("Multiple players cannot share the same starting location on this map.");
                        string messageToSend = "NOTICE " + ChannelName + " " + CTCPChar1 + CTCPChar2 + "SAMESTARTLOC" + CTCPChar2;
                        CnCNetData.ConnectionBridge.SendMessage(messageToSend);
                        return;
                    }
                }

                for (int aiId = 0; aiId < AIPlayers.Count; aiId++)
                {
                    int startingLocation = AIPlayers[aiId].StartingLocation;

                    if (startingLocation == 0)
                        continue;

                    int index = AIPlayers.FindIndex(aip => aip.StartingLocation == startingLocation);

                    if (index > -1 && index != aiId)
                    {
                        AddNotice("Multiple players cannot share the same starting location on this map.");
                        string messageToSend = "NOTICE " + ChannelName + " " + CTCPChar1 + CTCPChar2 + "SAMESTARTLOC" + CTCPChar2;
                        CnCNetData.ConnectionBridge.SendMessage(messageToSend);
                        return;
                    }
                }
            }

            foreach (PlayerInfo player in Players)
            {
                if (!player.Ready && player.Name != ProgramConstants.CNCNET_PLAYERNAME)
                {
                    AddNotice("The host wants to start the game but cannot because not all players are ready!");
                    string messageToSend = "NOTICE " + ChannelName + " " + CTCPChar1 + CTCPChar2 + "GETREADY" + CTCPChar2;
                    CnCNetData.ConnectionBridge.SendMessage(messageToSend);
                    return;
                }
            }

            int iId = 0;
            foreach (PlayerInfo player in Players)
            {
                if (!player.Ready && player.Name != ProgramConstants.CNCNET_PLAYERNAME)
                {
                    if (player.IsInGame)
                    {
                        AddNotice("Unable to launch game;" + player.Name + " is still playing the game you started previously.");
                        string messageToSend = "NOTICE " + ChannelName + " " + CTCPChar1 + CTCPChar2 + "INGAME " + iId + CTCPChar2;
                        CnCNetData.ConnectionBridge.SendMessage(messageToSend);
                        return;
                    }
                }

                iId++;
            }

            if (Players.Count == 1)
            {
                DialogResult dr = MessageBox.Show("It would surely be more fun to have others playing with you."
                    + Environment.NewLine + Environment.NewLine +
                    "Do you really want to play alone?", "Only one player?", MessageBoxButtons.YesNo);

                if (dr == System.Windows.Forms.DialogResult.No)
                    return;
            }

            if (currentMap.EnforceMaxPlayers)
            {
                if (Players.Count + AIPlayers.Count > currentMap.AmountOfPlayers)
                {
                    AddNotice("Unable to launch game: this map cannot be played with more than " + currentMap.AmountOfPlayers + " players.");
                    string messageToSend = "NOTICE " + ChannelName + " " + CTCPChar1 + CTCPChar2 + "TMPLAYERS" + CTCPChar2;
                    CnCNetData.ConnectionBridge.SendMessage(messageToSend);
                    return;
                }
            }

            if (currentMap.MinPlayers > Players.Count + AIPlayers.Count)
            {
                AddNotice("Unable to launch game: this map cannot be played with fewer than " + currentMap.MinPlayers + " players.");
                string messageToSend = "NOTICE " + ChannelName + " " + CTCPChar1 + CTCPChar2 + "INFSPLAYERS" + CTCPChar2;
                CnCNetData.ConnectionBridge.SendMessage(messageToSend);
                return;
            }

            if (currentMap == null)
            {
                MessageBox.Show("Unable to start the game: the selected map is invalid!", "Invalid map", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            for (int pId = 0; pId < Players.Count; pId++)
            {
                if (Players[pId].StartingLocation > currentMap.AmountOfPlayers)
                {
                    AddNotice("Unable to launch game: player " + Players[pId].Name + " has an invalid starting location.");
                    string messageToSend = "NOTICE " + ChannelName + " " + CTCPChar1 + CTCPChar2 + "INVSTART " + pId + CTCPChar2;
                    CnCNetData.ConnectionBridge.SendMessage(messageToSend);
                    return;
                }
            }

            for (int aiId = 0; aiId < AIPlayers.Count; aiId++)
            {
                if (AIPlayers[aiId].StartingLocation > currentMap.AmountOfPlayers)
                {
                    AddNotice("Unable to launch game: AI player " + aiId + " has an invalid starting location.");
                    string messageToSend = "NOTICE " + ChannelName + " " + CTCPChar1 + CTCPChar2 + "INVAISTART " + aiId + CTCPChar2;
                    CnCNetData.ConnectionBridge.SendMessage(messageToSend);
                    return;
                }
            }

            List<int> playerPorts;

            if (Players.Count > 1)
            {
                playerPorts = GetPlayerPortInfoFromTunnel();

                Logger.Log("Count of player ports: " + playerPorts.Count);

                if (playerPorts.Count < Players.Count)
                {
                    MessageBox.Show("An error occured while contacting the specified CnCNet tunnel server. Please try using a different tunnel server " +
                        "(accessible through the advanced options in the game creation window).", "Error connecting to tunnel", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                StringBuilder sb = new StringBuilder("PRIVMSG " + ChannelName + " " + CTCPChar1 + CTCPChar2 + "START ");
                sb.Append(tunnelAddress);
                sb.Append(":");
                sb.Append(tunnelPort);
                for (int pId = 0; pId < Players.Count; pId++)
                {
                    sb.Append(";");
                    sb.Append(Players[pId].Name);
                    sb.Append(";");
                    sb.Append("0.0.0.0:");
                    sb.Append(playerPorts[pId]);
                }
                sb.Append(CTCPChar2);
                CnCNetData.ConnectionBridge.SendMessage(sb.ToString());
            }
            else
            {
                Logger.Log("One player MP -- starting!");
                playerPorts = new List<int>();
            }

            for (int pId = 0; pId < Players.Count; pId++)
            {
                Players[pId].IsInGame = true;
            }

            StartGame(playerPorts);
        }

        /// <summary>
        /// Gets a list of player ports to use from a specific tunnel server.
        /// </summary>
        /// <returns>A list of player ports to use.</returns>
        private List<int> GetPlayerPortInfoFromTunnel()
        {
            try
            {
                Logger.Log("Contacting tunnel at " + tunnelAddress + ":" + tunnelPort);

                string addressString = string.Format("http://{0}:{1}/request?clients={2}",
                    tunnelAddress, tunnelPort, Players.Count);
                Logger.Log("Downloading from " + addressString);

                WebClient client = new WebClient();
                string data = client.DownloadString(addressString);

                data = data.Replace("[", String.Empty);
                data = data.Replace("]", String.Empty);

                string[] portIDs = data.Split(new char[] { ',' });
                List<int> playerPorts = new List<int>();

                foreach (string _port in portIDs)
                {
                    playerPorts.Add(Convert.ToInt32(_port));
                    Logger.Log("Added port " + _port);
                }

                return playerPorts;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to connect to the specified tunnel server." + Environment.NewLine + Environment.NewLine + 
                "Returned error message: " + ex.Message, "Cannot start game", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return new List<int>();
        }

        /// <summary>
        /// Writes spawn.ini and spawnmap.ini and starts the game.
        /// </summary>
        /// <param name="playerPorts">The list of port numbers to use for players.</param>
        private void StartGame(List<int> playerPorts)
        {
            string mapPath = ProgramConstants.gamepath + currentMap.Path;
            if (!File.Exists(mapPath))
            {
                MessageBox.Show("Unable to locate scenario map!" + Environment.NewLine + mapPath,
                    "Cannot read scenario", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnLeaveGame.PerformClick();
            }

            // 28. 12. 2014 No editing the map after setting for it!
            string mapSHA1 = Utilities.CalculateSHA1ForFile(mapPath);
            if (mapSHA1 != currentMap.SHA1)
            {
                MessageBox.Show("Map modification detected! Please restart the Client." + Environment.NewLine + mapPath,
                    "Cannot read scenario", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string messageToSend = "NOTICE " + ChannelName + " " + CTCPChar1 + CTCPChar2 + "MAPMOD" + CTCPChar2;
                CnCNetData.ConnectionBridge.SendMessage(messageToSend);
                btnLeaveGame.PerformClick();
                return;
            }

            string mapCodePath = currentMap.Path.Substring(0, currentMap.Path.Length - 3) + "ini";

            List<int> playerSides = new List<int>();
            List<bool> isPlayerSpectator = new List<bool>();
            List<int> playerColors = new List<int>();
            List<int> playerStartingLocations = new List<int>();

            SharedUILogic.Randomize(Players, AIPlayers, currentMap, Seed, playerSides,
                isPlayerSpectator, playerColors, playerStartingLocations,
                SharedUILogic.getAllowedSides(ComboBoxes, SideComboboxPrerequisites),
                SideComboboxPrerequisites.Count);

            List<int> MultiCmbIndexes;

            SharedUILogic.WriteSpawnIni(Players, AIPlayers, currentMap, currentGameMode, Seed,
                iNumLoadingScreens, isHost, playerPorts, tunnelAddress, tunnelPort, ComboBoxes,
                CheckBoxes, IsCheckBoxReversed, AssociatedCheckBoxSpawnIniOptions,
                AssociatedComboBoxSpawnIniOptions, ComboBoxDataWriteModes,
                playerSides, isPlayerSpectator, playerColors, playerStartingLocations,
                out MultiCmbIndexes);

            SharedLogic.WriteCoopDataToSpawnIni(currentMap, Players, AIPlayers, MultiCmbIndexes,
                coopDifficultyLevel, SideComboboxPrerequisites.Count, mapCodePath, Seed);

            List<bool> isCheckBoxChecked = new List<bool>();

            foreach (UserCheckBox chkBox in CheckBoxes)
                isCheckBoxChecked.Add(chkBox.Checked);

            File.Copy(mapPath, ProgramConstants.gamepath + ProgramConstants.SPAWNMAP_INI);

            IniFile mapIni = new IniFile(ProgramConstants.gamepath + ProgramConstants.SPAWNMAP_INI);

            SharedLogic.WriteMap(currentGameMode, isCheckBoxChecked, IsCheckBoxReversed, AssociatedCheckBoxCustomInis, mapIni);

            Logger.Log("About to launch main executable.");

            StartGameProcess();
        }

        /// <summary>
        /// Starts the game process and changes some internal variables so other client components know it as well.
        /// </summary>
        private void StartGameProcess()
        {
            ProgramConstants.IsInGame = true;

            if (DomainController.Instance().getWindowedStatus())
            {
                Logger.Log("Windowed mode is enabled - using QRes.");
                Process QResProcess = new Process();
                QResProcess.StartInfo.FileName = ProgramConstants.QRES_EXECUTABLE;
                QResProcess.StartInfo.UseShellExecute = false;
                QResProcess.StartInfo.Arguments = "c=16 /R " + "\"" + ProgramConstants.gamepath + ProgramConstants.MAIN_EXECUTABLE + "\"" + " -SPAWN";
                QResProcess.EnableRaisingEvents = true;
                QResProcess.Exited += new EventHandler(QResProcess_Exited);
                QResProcess.Start();

                if (Environment.ProcessorCount > 1)
                    QResProcess.ProcessorAffinity = (IntPtr)2;
            }
            else
            {
                Process DtaProcess = new Process();
                DtaProcess.StartInfo.FileName = ProgramConstants.MAIN_EXECUTABLE;
                DtaProcess.StartInfo.UseShellExecute = false;
                DtaProcess.StartInfo.Arguments = "-SPAWN";
                DtaProcess.EnableRaisingEvents = true;
                DtaProcess.Exited += new EventHandler(DtaProcess_Exited);
                DtaProcess.Start();

                if (Environment.ProcessorCount > 1)
                    DtaProcess.ProcessorAffinity = (System.IntPtr)2;
            }

            Logger.Log("Waiting for qres.dat or " + ProgramConstants.MAIN_EXECUTABLE + " to exit.");

            if (isHost)
            {
                timer.Stop();
                timer.Tick -= UpdateGameListing;
                timer.Dispose();
                timer = new Timer();
                timer.Tick += UpdateGameListing;
                timer.Interval = 10000;
                timer.Start();
            }

            CnCNetData.DoGameStarted();
        }

        /// <summary>
        /// Executed when game.dat has exited.
        /// </summary>
        private void DtaProcess_Exited(object sender, EventArgs e)
        {
            Generic_GameProcessExited();
        }

        /// <summary>
        /// Executed when qres.dat has exited.
        /// </summary>
        private void QResProcess_Exited(object sender, EventArgs e)
        {
            Generic_GameProcessExited();
        }

        /// <summary>
        /// Executed when the 'game process' (game.dat in fullscreen mode, qres.dat in windowed) has exited.
        /// </summary>
        private void Generic_GameProcessExited()
        {
            if (cmbP1Name.InvokeRequired)
            {
                NoParamCallback d = new NoParamCallback(Generic_GameProcessExited);
                this.Invoke(d, null);
                return;
            }

            Logger.Log("The game process has exited; displaying game lobby.");

            DomainController.Instance().ReloadSettings();

            ProgramConstants.IsInGame = false;
            CnCNetData.DoGameStopped();

            if (gameLobbyPersistentCheckBox != "none")
            {
                int chkId = CheckBoxes.FindIndex(c => c.Name == gameLobbyPersistentCheckBox);
                if (chkId > -1)
                {
                    if (!CheckBoxes[chkId].Checked)
                    {
                        btnLeaveGame.PerformClick();
                        return;
                    }
                }
            }

            if (isHost)
            {
                string returnMessage = "NOTICE " + ChannelName + " " + CTCPChar1 + CTCPChar2 + "RETURN" + CTCPChar2;
                CnCNetData.ConnectionBridge.SendMessage(returnMessage);

                Seed = new Random().Next(10000, 99999);
                GenericGameOptionChanged(null, EventArgs.Empty);

                if (Players.Count < playerLimit)
                {
                    UnlockGame(true);
                }

                timer.Stop();
                timer.Tick -= UpdateGameListing;
                timer.Dispose();
                timer = new Timer();
                timer.Interval = 5000;
                timer.Tick += UpdateGameListing;
                timer.Start();

                UpdateGameListing(null, EventArgs.Empty);
            }
            else
            {
                if (hasHostLeft)
                    btnLeaveGame.PerformClick();

                string returnMessage = "NOTICE " + ChannelName + " " + CTCPChar1 + CTCPChar2 + "RETURN" + CTCPChar2;
                CnCNetData.ConnectionBridge.SendMessage(returnMessage);

                if (isHostInGame)
                {
                    DisplayHostInGameBox();
                }
            }
        }

        private void DisplayHostInGameBox()
        {
            if (this.InvokeRequired)
            {
                NoParamCallback d = new NoParamCallback(DisplayHostInGameBox);
                this.BeginInvoke(d, null);
                return;
            }

            AddNotice("The game host is still playing the game you previously started. " +
                "Lobby chat is disabled until the host has returned from the game." +
                "You can either wait for the host to return or leave the game room " +
                "by clicking Leave Game.");
        }

        private void btnChangeMap_MouseEnter(object sender, EventArgs e)
        {
            sndButtonSound.Play();
            btnChangeMap.BackgroundImage = btn133px_c;
        }

        private void btnChangeMap_MouseLeave(object sender, EventArgs e)
        {
            btnChangeMap.BackgroundImage = btn133px;
        }

        private void btnLeaveGame_MouseEnter(object sender, EventArgs e)
        {
            sndButtonSound.Play();
            btnLeaveGame.BackgroundImage = btn133px_c;
        }

        private void btnLeaveGame_MouseLeave(object sender, EventArgs e)
        {
            btnLeaveGame.BackgroundImage = btn133px;
        }

        private void btnLaunchGame_MouseEnter(object sender, EventArgs e)
        {
            sndButtonSound.Play();
            btnLaunchGame.BackgroundImage = btn133px_c;
        }

        private void btnLaunchGame_MouseLeave(object sender, EventArgs e)
        {
            btnLaunchGame.BackgroundImage = btn133px;
        }

        private void btnLockGame_MouseEnter(object sender, EventArgs e)
        {
            sndButtonSound.Play();
            btnLockGame.BackgroundImage = btn133px_c;
        }

        private void btnLockGame_MouseLeave(object sender, EventArgs e)
        {
            btnLockGame.BackgroundImage = btn133px;
        }

        /// <summary>
        /// Automatically scrolls the chat list box when the user resizes
        /// the game lobby window.
        /// </summary>
        private void NGameLobby_SizeChanged(object sender, EventArgs e)
        {
            lbChatBox.SelectedIndex = lbChatBox.Items.Count - 1;
            lbChatBox.SelectedIndex = -1;
        }

        private void btnP1Kick_Click(object sender, EventArgs e)
        {
            AddNotice("You can't kick yourself! You might develop serious self-esteem problems.");
        }

        private void btnP1Ban_Click(object sender, EventArgs e)
        {
            AddNotice("Banning yourself could cause serious mental issues. Luckily, we're taking care of you and not allowing you to do that.");
        }

        /// <summary>
        /// Kicks a player from the game room.
        /// </summary>
        /// <param name="index">The index of the player to be kicked.</param>
        private void KickPlayer(int index)
        {
            if (Players.Count > index)
            {
                AddNotice("Kicking " + Players[index].Name + " from the game...");
                CnCNetData.ConnectionBridge.SendMessage("KICK " + ChannelName + " " + Players[index].Name);
            }
            else if (Players.Count + AIPlayers.Count > index)
            {
                // If the host is kicking an AI player, just remove it
                AIPlayers.RemoveAt(index - Players.Count);
                CopyPlayerDataToUI();
                CopyPlayerDataFromUI(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Bans a player from the game room.
        /// </summary>
        /// <param name="index">The index of the player to be banned.</param>
        private void BanPlayer(int index)
        {
            if (Players.Count > index)
            {
                AddNotice("Banning " + Players[index].Name + " from the game...");
                CnCNetData.ConnectionBridge.SendMessage("MODE " + ChannelName + " +b *!*" + Players[index].Name + "@" + Players[index].IPAddress);
                CnCNetData.ConnectionBridge.SendMessage("KICK " + ChannelName + " " + Players[index].Name);
            }
            else if (Players.Count + AIPlayers.Count > index)
            {
                // If the host is banning an AI player, just remove it
                AIPlayers.RemoveAt(index - Players.Count);
                CopyPlayerDataToUI();
                CopyPlayerDataFromUI(null, EventArgs.Empty);
            }
        }

        private void btnP2Kick_Click(object sender, EventArgs e)
        {
            KickPlayer(1);
        }

        private void btnP3Kick_Click(object sender, EventArgs e)
        {
            KickPlayer(2);
        }

        private void btnP4Kick_Click(object sender, EventArgs e)
        {
            KickPlayer(3);
        }

        private void btnP5Kick_Click(object sender, EventArgs e)
        {
            KickPlayer(4);
        }

        private void btnP6Kick_Click(object sender, EventArgs e)
        {
            KickPlayer(5);
        }

        private void btnP7Kick_Click(object sender, EventArgs e)
        {
            KickPlayer(6);
        }

        private void btnP8Kick_Click(object sender, EventArgs e)
        {
            KickPlayer(7);
        }

        private void btnP2Ban_Click(object sender, EventArgs e)
        {
            BanPlayer(1);
        }

        private void btnP3Ban_Click(object sender, EventArgs e)
        {
            BanPlayer(2);
        }

        private void btnP4Ban_Click(object sender, EventArgs e)
        {
            BanPlayer(3);
        }

        private void btnP5Ban_Click(object sender, EventArgs e)
        {
            BanPlayer(4);
        }

        private void btnP6Ban_Click(object sender, EventArgs e)
        {
            BanPlayer(5);
        }

        private void btnP7Ban_Click(object sender, EventArgs e)
        {
            BanPlayer(6);
        }

        private void btnP8Ban_Click(object sender, EventArgs e)
        {
            BanPlayer(7);
        }

        private void btnP1Kick_MouseEnter(object sender, EventArgs e)
        {
            sndButtonSound.Play();
            btnP1Kick.BackgroundImage = imgKick_c;
        }

        private void btnP1Kick_MouseLeave(object sender, EventArgs e)
        {
            btnP1Kick.BackgroundImage = imgKick;
        }

        private void btnP2Kick_MouseEnter(object sender, EventArgs e)
        {
            sndButtonSound.Play();
            btnP2Kick.BackgroundImage = imgKick_c;
        }

        private void btnP2Kick_MouseLeave(object sender, EventArgs e)
        {
            btnP2Kick.BackgroundImage = imgKick;
        }

        private void btnP3Kick_MouseEnter(object sender, EventArgs e)
        {
            sndButtonSound.Play();
            btnP3Kick.BackgroundImage = imgKick_c;
        }

        private void btnP3Kick_MouseLeave(object sender, EventArgs e)
        {
            btnP3Kick.BackgroundImage = imgKick;
        }

        private void btnP4Kick_MouseEnter(object sender, EventArgs e)
        {
            sndButtonSound.Play();
            btnP4Kick.BackgroundImage = imgKick_c;
        }

        private void btnP4Kick_MouseLeave(object sender, EventArgs e)
        {
            btnP4Kick.BackgroundImage = imgKick;
        }

        private void btnP5Kick_MouseEnter(object sender, EventArgs e)
        {
            sndButtonSound.Play();
            btnP5Kick.BackgroundImage = imgKick_c;
        }

        private void btnP5Kick_MouseLeave(object sender, EventArgs e)
        {
            btnP5Kick.BackgroundImage = imgKick;
        }

        private void btnP6Kick_MouseEnter(object sender, EventArgs e)
        {
            sndButtonSound.Play();
            btnP6Kick.BackgroundImage = imgKick_c;
        }

        private void btnP6Kick_MouseLeave(object sender, EventArgs e)
        {
            btnP6Kick.BackgroundImage = imgKick;
        }

        private void btnP7Kick_MouseEnter(object sender, EventArgs e)
        {
            sndButtonSound.Play();
            btnP7Kick.BackgroundImage = imgKick_c;
        }

        private void btnP7Kick_MouseLeave(object sender, EventArgs e)
        {
            btnP7Kick.BackgroundImage = imgKick;
        }

        private void btnP8Kick_MouseEnter(object sender, EventArgs e)
        {
            sndButtonSound.Play();
            btnP8Kick.BackgroundImage = imgKick_c;
        }

        private void btnP8Kick_MouseLeave(object sender, EventArgs e)
        {
            btnP8Kick.BackgroundImage = imgKick;
        }

        private void btnP1Ban_MouseEnter(object sender, EventArgs e)
        {
            sndButtonSound.Play();
            btnP1Ban.BackgroundImage = imgBan_c;
        }

        private void btnP1Ban_MouseLeave(object sender, EventArgs e)
        {
            btnP1Ban.BackgroundImage = imgBan;
        }

        private void btnP2Ban_MouseEnter(object sender, EventArgs e)
        {
            sndButtonSound.Play();
            btnP2Ban.BackgroundImage = imgBan_c;
        }

        private void btnP2Ban_MouseLeave(object sender, EventArgs e)
        {
            btnP2Ban.BackgroundImage = imgBan;
        }

        private void btnP3Ban_MouseEnter(object sender, EventArgs e)
        {
            sndButtonSound.Play();
            btnP3Ban.BackgroundImage = imgBan_c;
        }

        private void btnP3Ban_MouseLeave(object sender, EventArgs e)
        {
            btnP3Ban.BackgroundImage = imgBan;
        }

        private void btnP4Ban_MouseEnter(object sender, EventArgs e)
        {
            sndButtonSound.Play();
            btnP4Ban.BackgroundImage = imgBan_c;
        }

        private void btnP4Ban_MouseLeave(object sender, EventArgs e)
        {
            btnP4Ban.BackgroundImage = imgBan;
        }

        private void btnP5Ban_MouseEnter(object sender, EventArgs e)
        {
            sndButtonSound.Play();
            btnP5Ban.BackgroundImage = imgBan_c;
        }

        private void btnP5Ban_MouseLeave(object sender, EventArgs e)
        {
            btnP5Ban.BackgroundImage = imgBan;
        }

        private void btnP6Ban_MouseEnter(object sender, EventArgs e)
        {
            sndButtonSound.Play();
            btnP6Ban.BackgroundImage = imgBan_c;
        }

        private void btnP6Ban_MouseLeave(object sender, EventArgs e)
        {
            btnP6Ban.BackgroundImage = imgBan;
        }

        private void btnP7Ban_MouseEnter(object sender, EventArgs e)
        {
            sndButtonSound.Play();
            btnP7Ban.BackgroundImage = imgBan_c;
        }

        private void btnP7Ban_MouseLeave(object sender, EventArgs e)
        {
            btnP7Ban.BackgroundImage = imgBan;
        }

        private void btnP8Ban_MouseEnter(object sender, EventArgs e)
        {
            sndButtonSound.Play();
            btnP8Ban.BackgroundImage = imgBan_c;
        }

        private void btnP8Ban_MouseLeave(object sender, EventArgs e)
        {
            btnP8Ban.BackgroundImage = imgBan;
        }

        private void pbPreview_SizeChanged(object sender, EventArgs e)
        {
            if (resizeTimer == null)
                return;

            resizeTimer.Stop();
            resizeTimer.Start();
        }

        PropertyInfo imageRectangleProperty = typeof(PictureBox).GetProperty("ImageRectangle", BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance);

        /// <summary>
        /// Used for painting starting locations to the map preview box.
        /// http://stackoverflow.com/questions/18210030/get-pixelvalue-when-click-on-a-picturebox
        /// </summary>
        private void pbPreview_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rectangle = (Rectangle)imageRectangleProperty.GetValue(pbPreview, null);

            SharedUILogic.PaintPreview(currentMap, rectangle, e, coopBriefingFont,
                playerNameOnPlayerLocationFont, coopBriefingForeColor, displayCoopBriefing,
                previewRatioY, previewRatioX, PlayerNamesOnPlayerLocations, MPColors,
                PlayerColorsOnPlayerLocations, startingLocationIndicators, enemyStartingLocationIndicator);
        }

        private void cmbPXName_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        /// <summary>
        /// Makes it possible to copy text to the clipboard from the chat box by Ctrl + C.
        /// </summary>
        private void lbChatBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (lbChatBox.SelectedIndex > -1)
            {
                if (e.KeyCode == Keys.C && e.Control)
                    Clipboard.SetText(lbChatBox.SelectedItem.ToString());
            }
        }

        private void pbPreview_MouseEnter(object sender, EventArgs e)
        {
            displayCoopBriefing = false;
            pbPreview.Refresh();
        }

        private void pbPreview_MouseLeave(object sender, EventArgs e)
        {
            displayCoopBriefing = true;
            pbPreview.Refresh();
        }

        /// <summary>
        /// Prevents the player from changing their name.
        /// </summary>
        private void playerNameTextBox_GotFocus(object sender, EventArgs e)
        {
            tbChatInputBox.Focus();
        }

        /// <summary>
        /// (Un)locks the game room if possible.
        /// </summary>
        private void btnLockGame_Click(object sender, EventArgs e)
        {
            if (!Locked)
            {
                AddNotice("You've locked the game room.");
                LockGame(false);
            }
            else
            {
                if (Players.Count < playerLimit)
                {
                    AddNotice("You've unlocked the game room.");
                    UnlockGame(false);
                }
                else
                    AddNotice(string.Format("Cannot unlock game; the player limit ({0}) has been reached.", playerLimit));
            }
        }

        /// <summary>
        /// Locks the game room.
        /// </summary>
        /// <param name="announce">Whether to inform the player that the game room was locked.</param>
        private void LockGame(bool announce)
        {
            Locked = true;
            if (announce)
                AddNotice("The game room has been locked.");
            btnLockGame.Text = "Unlock Game";
            CnCNetData.ConnectionBridge.SendMessage(string.Format("MODE {0} +i", ChannelName));
        }

        /// <summary>
        /// Unlocks the game room.
        /// </summary>
        /// <param name="announce">Whether to inform the player that the game room was unlocked.</param>
        private void UnlockGame(bool announce)
        {
            Locked = false;
            if (announce)
                AddNotice("The game room has been unlocked.");
            btnLockGame.Text = "Lock Game";
            CnCNetData.ConnectionBridge.SendMessage(string.Format("MODE {0} -i", ChannelName));
        }
    }
}

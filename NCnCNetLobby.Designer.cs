namespace ClientGUI
{
    partial class NCnCNetLobby
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NCnCNetLobby));
            this.lblFollowChannels = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkChannelCnCNet = new ClientGUI.UserCheckBox();
            this.chkChannelTS = new ClientGUI.UserCheckBox();
            this.chkChannelTI = new ClientGUI.UserCheckBox();
            this.chkChannelDTA = new ClientGUI.UserCheckBox();
            this.btnReturnToMenu = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblPlayer8 = new System.Windows.Forms.Label();
            this.lblPlayer7 = new System.Windows.Forms.Label();
            this.lblPlayer6 = new System.Windows.Forms.Label();
            this.lblPlayer5 = new System.Windows.Forms.Label();
            this.lblPlayer4 = new System.Windows.Forms.Label();
            this.lblPlayer3 = new System.Windows.Forms.Label();
            this.lblPlayer2 = new System.Windows.Forms.Label();
            this.lblPlayer1 = new System.Windows.Forms.Label();
            this.lblGameMode = new System.Windows.Forms.Label();
            this.lblPlayers = new System.Windows.Forms.Label();
            this.lblHost = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblMapName = new System.Windows.Forms.Label();
            this.lblGameInfo = new System.Windows.Forms.Label();
            this.btnNewGame = new System.Windows.Forms.Button();
            this.btnJoinGame = new System.Windows.Forms.Button();
            this.tbChatInput = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.lblGames = new System.Windows.Forms.Label();
            this.lblPlayerList = new System.Windows.Forms.Label();
            this.btnHideChannels = new System.Windows.Forms.Button();
            this.lblChannel = new System.Windows.Forms.Label();
            this.lblMessageColor = new System.Windows.Forms.Label();
            this.btnMusicToggle = new System.Windows.Forms.Button();
            this.sbChat = new CustomControls.CustomScrollbar();
            this.sbPlayers = new CustomControls.CustomScrollbar();
            this.chkChannelTO = new ClientGUI.UserCheckBox();
            this.cmbMessageColor = new ClientGUI.LimitedComboBox();
            this.cmbCurrentChannel = new ClientGUI.LimitedComboBox();
            this.lbChatMessages = new ClientGUI.ScrollbarlessListBox();
            this.lbPlayerList = new ClientGUI.ScrollbarlessListBox();
            this.lbGameList = new ClientGUI.ScrollbarlessListBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblFollowChannels
            // 
            this.lblFollowChannels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFollowChannels.AutoSize = true;
            this.lblFollowChannels.BackColor = System.Drawing.Color.Transparent;
            this.lblFollowChannels.Location = new System.Drawing.Point(788, 9);
            this.lblFollowChannels.Name = "lblFollowChannels";
            this.lblFollowChannels.Size = new System.Drawing.Size(182, 15);
            this.lblFollowChannels.TabIndex = 2;
            this.lblFollowChannels.Text = "Followed Channels and Games:";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.chkChannelCnCNet);
            this.panel1.Controls.Add(this.chkChannelTS);
            this.panel1.Controls.Add(this.chkChannelTI);
            this.panel1.Controls.Add(this.chkChannelDTA);
            this.panel1.Location = new System.Drawing.Point(765, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(215, 109);
            this.panel1.TabIndex = 3;
            // 
            // chkChannelCnCNet
            // 
            this.chkChannelCnCNet.AutoSize = true;
            this.chkChannelCnCNet.BackColor = System.Drawing.Color.Transparent;
            this.chkChannelCnCNet.IsEnabled = true;
            this.chkChannelCnCNet.Location = new System.Drawing.Point(13, 78);
            this.chkChannelCnCNet.Name = "chkChannelCnCNet";
            this.chkChannelCnCNet.Size = new System.Drawing.Size(197, 17);
            this.chkChannelCnCNet.TabIndex = 26;
            this.chkChannelCnCNet.Tag = "";
            this.chkChannelCnCNet.CheckedChanged += new ClientGUI.UserCheckBox.OnCheckedChanged(this.chkChannelCnCNet_CheckedChanged);
            // 
            // chkChannelTS
            // 
            this.chkChannelTS.BackColor = System.Drawing.Color.Transparent;
            this.chkChannelTS.IsEnabled = true;
            this.chkChannelTS.Location = new System.Drawing.Point(13, 56);
            this.chkChannelTS.Name = "chkChannelTS";
            this.chkChannelTS.Size = new System.Drawing.Size(197, 15);
            this.chkChannelTS.TabIndex = 25;
            this.chkChannelTS.Tag = "";
            this.chkChannelTS.CheckedChanged += new ClientGUI.UserCheckBox.OnCheckedChanged(this.chkChannelTS_CheckedChanged);
            // 
            // chkChannelTI
            // 
            this.chkChannelTI.BackColor = System.Drawing.Color.Transparent;
            this.chkChannelTI.IsEnabled = true;
            this.chkChannelTI.Location = new System.Drawing.Point(13, 35);
            this.chkChannelTI.Name = "chkChannelTI";
            this.chkChannelTI.Size = new System.Drawing.Size(194, 15);
            this.chkChannelTI.TabIndex = 23;
            this.chkChannelTI.Tag = "";
            this.chkChannelTI.CheckedChanged += new ClientGUI.UserCheckBox.OnCheckedChanged(this.chkChannelTI_CheckedChanged);
            // 
            // chkChannelDTA
            // 
            this.chkChannelDTA.BackColor = System.Drawing.Color.Transparent;
            this.chkChannelDTA.IsEnabled = true;
            this.chkChannelDTA.Location = new System.Drawing.Point(13, 12);
            this.chkChannelDTA.Name = "chkChannelDTA";
            this.chkChannelDTA.Size = new System.Drawing.Size(197, 15);
            this.chkChannelDTA.TabIndex = 22;
            this.chkChannelDTA.Tag = "The Dawn of the Tiberium Age";
            this.chkChannelDTA.CheckedChanged += new ClientGUI.UserCheckBox.OnCheckedChanged(this.chkChannelDTA_CheckedChanged);
            // 
            // btnReturnToMenu
            // 
            this.btnReturnToMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReturnToMenu.FlatAppearance.BorderSize = 0;
            this.btnReturnToMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReturnToMenu.Location = new System.Drawing.Point(838, 520);
            this.btnReturnToMenu.Name = "btnReturnToMenu";
            this.btnReturnToMenu.Size = new System.Drawing.Size(142, 23);
            this.btnReturnToMenu.TabIndex = 4;
            this.btnReturnToMenu.Text = "Return to Main Menu";
            this.btnReturnToMenu.UseVisualStyleBackColor = true;
            this.btnReturnToMenu.Click += new System.EventHandler(this.btnReturnToMenu_Click);
            this.btnReturnToMenu.MouseEnter += new System.EventHandler(this.btnReturnToMenu_MouseEnter);
            this.btnReturnToMenu.MouseLeave += new System.EventHandler(this.btnReturnToMenu_MouseLeave);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.lblPlayer8);
            this.panel2.Controls.Add(this.lblPlayer7);
            this.panel2.Controls.Add(this.lblPlayer6);
            this.panel2.Controls.Add(this.lblPlayer5);
            this.panel2.Controls.Add(this.lblPlayer4);
            this.panel2.Controls.Add(this.lblPlayer3);
            this.panel2.Controls.Add(this.lblPlayer2);
            this.panel2.Controls.Add(this.lblPlayer1);
            this.panel2.Controls.Add(this.lblGameMode);
            this.panel2.Controls.Add(this.lblPlayers);
            this.panel2.Controls.Add(this.lblHost);
            this.panel2.Controls.Add(this.lblVersion);
            this.panel2.Controls.Add(this.lblMapName);
            this.panel2.Location = new System.Drawing.Point(12, 349);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(215, 166);
            this.panel2.TabIndex = 6;
            // 
            // lblPlayer8
            // 
            this.lblPlayer8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblPlayer8.AutoSize = true;
            this.lblPlayer8.BackColor = System.Drawing.Color.Transparent;
            this.lblPlayer8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlayer8.Location = new System.Drawing.Point(106, 145);
            this.lblPlayer8.Name = "lblPlayer8";
            this.lblPlayer8.Size = new System.Drawing.Size(97, 13);
            this.lblPlayer8.TabIndex = 35;
            this.lblPlayer8.Text = "Player 8 XXXXXXX";
            // 
            // lblPlayer7
            // 
            this.lblPlayer7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblPlayer7.AutoSize = true;
            this.lblPlayer7.BackColor = System.Drawing.Color.Transparent;
            this.lblPlayer7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlayer7.Location = new System.Drawing.Point(106, 132);
            this.lblPlayer7.Name = "lblPlayer7";
            this.lblPlayer7.Size = new System.Drawing.Size(97, 13);
            this.lblPlayer7.TabIndex = 34;
            this.lblPlayer7.Text = "Player 7 XXXXXXX";
            // 
            // lblPlayer6
            // 
            this.lblPlayer6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblPlayer6.AutoSize = true;
            this.lblPlayer6.BackColor = System.Drawing.Color.Transparent;
            this.lblPlayer6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlayer6.Location = new System.Drawing.Point(106, 119);
            this.lblPlayer6.Name = "lblPlayer6";
            this.lblPlayer6.Size = new System.Drawing.Size(97, 13);
            this.lblPlayer6.TabIndex = 33;
            this.lblPlayer6.Text = "Player 6 XXXXXXX";
            // 
            // lblPlayer5
            // 
            this.lblPlayer5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblPlayer5.AutoSize = true;
            this.lblPlayer5.BackColor = System.Drawing.Color.Transparent;
            this.lblPlayer5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlayer5.Location = new System.Drawing.Point(106, 106);
            this.lblPlayer5.Name = "lblPlayer5";
            this.lblPlayer5.Size = new System.Drawing.Size(97, 13);
            this.lblPlayer5.TabIndex = 32;
            this.lblPlayer5.Text = "Player 5 XXXXXXX";
            // 
            // lblPlayer4
            // 
            this.lblPlayer4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblPlayer4.AutoSize = true;
            this.lblPlayer4.BackColor = System.Drawing.Color.Transparent;
            this.lblPlayer4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlayer4.Location = new System.Drawing.Point(3, 145);
            this.lblPlayer4.Name = "lblPlayer4";
            this.lblPlayer4.Size = new System.Drawing.Size(97, 13);
            this.lblPlayer4.TabIndex = 31;
            this.lblPlayer4.Text = "Player 4 XXXXXXX";
            // 
            // lblPlayer3
            // 
            this.lblPlayer3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblPlayer3.AutoSize = true;
            this.lblPlayer3.BackColor = System.Drawing.Color.Transparent;
            this.lblPlayer3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlayer3.Location = new System.Drawing.Point(3, 132);
            this.lblPlayer3.Name = "lblPlayer3";
            this.lblPlayer3.Size = new System.Drawing.Size(97, 13);
            this.lblPlayer3.TabIndex = 30;
            this.lblPlayer3.Text = "Player 3 XXXXXXX";
            // 
            // lblPlayer2
            // 
            this.lblPlayer2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblPlayer2.AutoSize = true;
            this.lblPlayer2.BackColor = System.Drawing.Color.Transparent;
            this.lblPlayer2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlayer2.Location = new System.Drawing.Point(3, 119);
            this.lblPlayer2.Name = "lblPlayer2";
            this.lblPlayer2.Size = new System.Drawing.Size(97, 13);
            this.lblPlayer2.TabIndex = 29;
            this.lblPlayer2.Text = "Player 2 XXXXXXX";
            // 
            // lblPlayer1
            // 
            this.lblPlayer1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblPlayer1.AutoSize = true;
            this.lblPlayer1.BackColor = System.Drawing.Color.Transparent;
            this.lblPlayer1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlayer1.Location = new System.Drawing.Point(3, 106);
            this.lblPlayer1.Name = "lblPlayer1";
            this.lblPlayer1.Size = new System.Drawing.Size(97, 13);
            this.lblPlayer1.TabIndex = 28;
            this.lblPlayer1.Text = "Player 1 XXXXXXX";
            // 
            // lblGameMode
            // 
            this.lblGameMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblGameMode.AutoSize = true;
            this.lblGameMode.BackColor = System.Drawing.Color.Transparent;
            this.lblGameMode.Location = new System.Drawing.Point(3, 3);
            this.lblGameMode.Name = "lblGameMode";
            this.lblGameMode.Size = new System.Drawing.Size(86, 15);
            this.lblGameMode.TabIndex = 27;
            this.lblGameMode.Text = "Game Mode: -";
            // 
            // lblPlayers
            // 
            this.lblPlayers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblPlayers.AutoSize = true;
            this.lblPlayers.BackColor = System.Drawing.Color.Transparent;
            this.lblPlayers.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlayers.Location = new System.Drawing.Point(3, 93);
            this.lblPlayers.Name = "lblPlayers";
            this.lblPlayers.Size = new System.Drawing.Size(70, 13);
            this.lblPlayers.TabIndex = 26;
            this.lblPlayers.Text = "Players (- / -):";
            // 
            // lblHost
            // 
            this.lblHost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblHost.AutoSize = true;
            this.lblHost.BackColor = System.Drawing.Color.Transparent;
            this.lblHost.Location = new System.Drawing.Point(3, 71);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(42, 15);
            this.lblHost.TabIndex = 25;
            this.lblHost.Text = "Host: -";
            // 
            // lblVersion
            // 
            this.lblVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblVersion.AutoSize = true;
            this.lblVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblVersion.Location = new System.Drawing.Point(3, 50);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(95, 15);
            this.lblVersion.TabIndex = 23;
            this.lblVersion.Text = "Game Version: -";
            // 
            // lblMapName
            // 
            this.lblMapName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMapName.AutoSize = true;
            this.lblMapName.BackColor = System.Drawing.Color.Transparent;
            this.lblMapName.Location = new System.Drawing.Point(3, 27);
            this.lblMapName.Name = "lblMapName";
            this.lblMapName.Size = new System.Drawing.Size(42, 15);
            this.lblMapName.TabIndex = 21;
            this.lblMapName.Text = "Map: -";
            // 
            // lblGameInfo
            // 
            this.lblGameInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblGameInfo.AutoSize = true;
            this.lblGameInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblGameInfo.Location = new System.Drawing.Point(9, 331);
            this.lblGameInfo.Name = "lblGameInfo";
            this.lblGameInfo.Size = new System.Drawing.Size(109, 15);
            this.lblGameInfo.TabIndex = 7;
            this.lblGameInfo.Text = "Game information:";
            // 
            // btnNewGame
            // 
            this.btnNewGame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNewGame.Enabled = false;
            this.btnNewGame.FlatAppearance.BorderSize = 0;
            this.btnNewGame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewGame.Location = new System.Drawing.Point(12, 520);
            this.btnNewGame.Name = "btnNewGame";
            this.btnNewGame.Size = new System.Drawing.Size(92, 23);
            this.btnNewGame.TabIndex = 8;
            this.btnNewGame.Text = "New Game";
            this.btnNewGame.UseVisualStyleBackColor = true;
            this.btnNewGame.Click += new System.EventHandler(this.btnNewGame_Click);
            this.btnNewGame.MouseEnter += new System.EventHandler(this.btnNewGame_MouseEnter);
            this.btnNewGame.MouseLeave += new System.EventHandler(this.btnNewGame_MouseLeave);
            // 
            // btnJoinGame
            // 
            this.btnJoinGame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnJoinGame.Enabled = false;
            this.btnJoinGame.FlatAppearance.BorderSize = 0;
            this.btnJoinGame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJoinGame.Location = new System.Drawing.Point(135, 520);
            this.btnJoinGame.Name = "btnJoinGame";
            this.btnJoinGame.Size = new System.Drawing.Size(92, 23);
            this.btnJoinGame.TabIndex = 9;
            this.btnJoinGame.Text = "Join Game";
            this.btnJoinGame.UseVisualStyleBackColor = true;
            this.btnJoinGame.Click += new System.EventHandler(this.btnJoinGame_Click);
            this.btnJoinGame.MouseEnter += new System.EventHandler(this.btnJoinGame_MouseEnter);
            this.btnJoinGame.MouseLeave += new System.EventHandler(this.btnJoinGame_MouseLeave);
            // 
            // tbChatInput
            // 
            this.tbChatInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbChatInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbChatInput.Location = new System.Drawing.Point(234, 522);
            this.tbChatInput.Name = "tbChatInput";
            this.tbChatInput.Size = new System.Drawing.Size(525, 21);
            this.tbChatInput.TabIndex = 11;
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Enabled = false;
            this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSend.Location = new System.Drawing.Point(765, 520);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(67, 23);
            this.btnSend.TabIndex = 12;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // lblGames
            // 
            this.lblGames.AutoSize = true;
            this.lblGames.BackColor = System.Drawing.Color.Transparent;
            this.lblGames.Location = new System.Drawing.Point(12, 9);
            this.lblGames.Name = "lblGames";
            this.lblGames.Size = new System.Drawing.Size(50, 15);
            this.lblGames.TabIndex = 13;
            this.lblGames.Text = "Games:";
            // 
            // lblPlayerList
            // 
            this.lblPlayerList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPlayerList.AutoSize = true;
            this.lblPlayerList.BackColor = System.Drawing.Color.Transparent;
            this.lblPlayerList.Location = new System.Drawing.Point(762, 146);
            this.lblPlayerList.Name = "lblPlayerList";
            this.lblPlayerList.Size = new System.Drawing.Size(50, 15);
            this.lblPlayerList.TabIndex = 15;
            this.lblPlayerList.Text = "Players:";
            // 
            // btnHideChannels
            // 
            this.btnHideChannels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHideChannels.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnHideChannels.BackColor = System.Drawing.SystemColors.Control;
            this.btnHideChannels.FlatAppearance.BorderSize = 0;
            this.btnHideChannels.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHideChannels.Location = new System.Drawing.Point(765, 8);
            this.btnHideChannels.Name = "btnHideChannels";
            this.btnHideChannels.Size = new System.Drawing.Size(16, 16);
            this.btnHideChannels.TabIndex = 16;
            this.btnHideChannels.UseVisualStyleBackColor = false;
            this.btnHideChannels.Click += new System.EventHandler(this.btnHideChannels_Click);
            // 
            // lblChannel
            // 
            this.lblChannel.AutoSize = true;
            this.lblChannel.BackColor = System.Drawing.Color.Transparent;
            this.lblChannel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblChannel.Location = new System.Drawing.Point(488, 9);
            this.lblChannel.Name = "lblChannel";
            this.lblChannel.Size = new System.Drawing.Size(111, 13);
            this.lblChannel.TabIndex = 17;
            this.lblChannel.Text = "Current Chat Channel:";
            // 
            // lblMessageColor
            // 
            this.lblMessageColor.AutoSize = true;
            this.lblMessageColor.BackColor = System.Drawing.Color.Transparent;
            this.lblMessageColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblMessageColor.Location = new System.Drawing.Point(230, 9);
            this.lblMessageColor.Name = "lblMessageColor";
            this.lblMessageColor.Size = new System.Drawing.Size(58, 13);
            this.lblMessageColor.TabIndex = 19;
            this.lblMessageColor.Text = "Your color:";
            // 
            // btnMusicToggle
            // 
            this.btnMusicToggle.FlatAppearance.BorderSize = 0;
            this.btnMusicToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMusicToggle.Location = new System.Drawing.Point(390, 4);
            this.btnMusicToggle.Name = "btnMusicToggle";
            this.btnMusicToggle.Size = new System.Drawing.Size(92, 23);
            this.btnMusicToggle.TabIndex = 21;
            this.btnMusicToggle.Text = "Music ON";
            this.btnMusicToggle.UseVisualStyleBackColor = true;
            this.btnMusicToggle.Click += new System.EventHandler(this.btnMusicToggle_Click);
            this.btnMusicToggle.MouseEnter += new System.EventHandler(this.btnMusicToggle_MouseEnter);
            this.btnMusicToggle.MouseLeave += new System.EventHandler(this.btnMusicToggle_MouseLeave);
            // 
            // sbChat
            // 
            this.sbChat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sbChat.ChannelColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(166)))), ((int)(((byte)(3)))));
            this.sbChat.DownArrowImage = ((System.Drawing.Image)(resources.GetObject("sbChat.DownArrowImage")));
            this.sbChat.LargeChange = 10;
            this.sbChat.Location = new System.Drawing.Point(744, 28);
            this.sbChat.Maximum = 100;
            this.sbChat.Minimum = 0;
            this.sbChat.MinimumSize = new System.Drawing.Size(15, 92);
            this.sbChat.Name = "sbChat";
            this.sbChat.Size = new System.Drawing.Size(15, 487);
            this.sbChat.SmallChange = 1;
            this.sbChat.TabIndex = 22;
            this.sbChat.ThumbBottomImage = ((System.Drawing.Image)(resources.GetObject("sbChat.ThumbBottomImage")));
            this.sbChat.ThumbBottomSpanImage = ((System.Drawing.Image)(resources.GetObject("sbChat.ThumbBottomSpanImage")));
            this.sbChat.ThumbMiddleImage = ((System.Drawing.Image)(resources.GetObject("sbChat.ThumbMiddleImage")));
            this.sbChat.ThumbTopImage = ((System.Drawing.Image)(resources.GetObject("sbChat.ThumbTopImage")));
            this.sbChat.ThumbTopSpanImage = ((System.Drawing.Image)(resources.GetObject("sbChat.ThumbTopSpanImage")));
            this.sbChat.UpArrowImage = ((System.Drawing.Image)(resources.GetObject("sbChat.UpArrowImage")));
            this.sbChat.Value = 0;
            // 
            // sbPlayers
            // 
            this.sbPlayers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sbPlayers.ChannelColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(166)))), ((int)(((byte)(3)))));
            this.sbPlayers.DownArrowImage = ((System.Drawing.Image)(resources.GetObject("sbPlayers.DownArrowImage")));
            this.sbPlayers.LargeChange = 10;
            this.sbPlayers.Location = new System.Drawing.Point(965, 162);
            this.sbPlayers.Maximum = 100;
            this.sbPlayers.Minimum = 0;
            this.sbPlayers.MinimumSize = new System.Drawing.Size(15, 92);
            this.sbPlayers.Name = "sbPlayers";
            this.sbPlayers.Size = new System.Drawing.Size(15, 353);
            this.sbPlayers.SmallChange = 1;
            this.sbPlayers.TabIndex = 23;
            this.sbPlayers.ThumbBottomImage = ((System.Drawing.Image)(resources.GetObject("sbPlayers.ThumbBottomImage")));
            this.sbPlayers.ThumbBottomSpanImage = ((System.Drawing.Image)(resources.GetObject("sbPlayers.ThumbBottomSpanImage")));
            this.sbPlayers.ThumbMiddleImage = ((System.Drawing.Image)(resources.GetObject("sbPlayers.ThumbMiddleImage")));
            this.sbPlayers.ThumbTopImage = ((System.Drawing.Image)(resources.GetObject("sbPlayers.ThumbTopImage")));
            this.sbPlayers.ThumbTopSpanImage = ((System.Drawing.Image)(resources.GetObject("sbPlayers.ThumbTopSpanImage")));
            this.sbPlayers.UpArrowImage = ((System.Drawing.Image)(resources.GetObject("sbPlayers.UpArrowImage")));
            this.sbPlayers.Value = 0;
            // 
            // chkChannelTO
            // 
            this.chkChannelTO.BackColor = System.Drawing.Color.Transparent;
            this.chkChannelTO.IsEnabled = true;
            this.chkChannelTO.Location = new System.Drawing.Point(124, 332);
            this.chkChannelTO.Name = "chkChannelTO";
            this.chkChannelTO.Size = new System.Drawing.Size(115, 15);
            this.chkChannelTO.TabIndex = 24;
            this.chkChannelTO.Tag = "";
            this.chkChannelTO.Visible = false;
            // 
            // cmbMessageColor
            // 
            this.cmbMessageColor.CanDropDown = true;
            this.cmbMessageColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbMessageColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMessageColor.FormattingEnabled = true;
            this.cmbMessageColor.HoveredIndex = -1;
            this.cmbMessageColor.Location = new System.Drawing.Point(294, 6);
            this.cmbMessageColor.MaxDropDownItems = 13;
            this.cmbMessageColor.Name = "cmbMessageColor";
            this.cmbMessageColor.Size = new System.Drawing.Size(90, 22);
            this.cmbMessageColor.TabIndex = 20;
            this.cmbMessageColor.UseCustomDrawingCode = true;
            this.cmbMessageColor.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbMessageColor_DrawItem);
            this.cmbMessageColor.SelectedIndexChanged += new System.EventHandler(this.cmbMessageColor_SelectedIndexChanged);
            // 
            // cmbCurrentChannel
            // 
            this.cmbCurrentChannel.CanDropDown = true;
            this.cmbCurrentChannel.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbCurrentChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCurrentChannel.FormattingEnabled = true;
            this.cmbCurrentChannel.HoveredIndex = -1;
            this.cmbCurrentChannel.Items.AddRange(new object[] {
            "Dawn of the Tiberium Age",
            "Twisted Insurrection",
            "Tiberian Sun",
            "General CnCNet Chat"});
            this.cmbCurrentChannel.Location = new System.Drawing.Point(605, 6);
            this.cmbCurrentChannel.MaxDropDownItems = 13;
            this.cmbCurrentChannel.Name = "cmbCurrentChannel";
            this.cmbCurrentChannel.Size = new System.Drawing.Size(154, 22);
            this.cmbCurrentChannel.TabIndex = 18;
            this.cmbCurrentChannel.UseCustomDrawingCode = true;
            this.cmbCurrentChannel.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbCurrentChannel_DrawItem);
            this.cmbCurrentChannel.SelectedIndexChanged += new System.EventHandler(this.cmbCurrentChannel_SelectedIndexChanged);
            // 
            // lbChatMessages
            // 
            this.lbChatMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbChatMessages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbChatMessages.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lbChatMessages.FormattingEnabled = true;
            this.lbChatMessages.IntegralHeight = false;
            this.lbChatMessages.Location = new System.Drawing.Point(233, 28);
            this.lbChatMessages.Name = "lbChatMessages";
            this.lbChatMessages.ShowScrollbar = false;
            this.lbChatMessages.Size = new System.Drawing.Size(512, 487);
            this.lbChatMessages.TabIndex = 10;
            this.lbChatMessages.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbChatMessages_DrawItem);
            this.lbChatMessages.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.lbChatMessages_MeasureItem);
            this.lbChatMessages.DoubleClick += new System.EventHandler(this.lbChatMessages_DoubleClick);
            this.lbChatMessages.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbChatMessages_KeyDown);
            this.lbChatMessages.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lbChatMessages_MouseMove);
            // 
            // lbPlayerList
            // 
            this.lbPlayerList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPlayerList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbPlayerList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lbPlayerList.FormattingEnabled = true;
            this.lbPlayerList.IntegralHeight = false;
            this.lbPlayerList.Location = new System.Drawing.Point(765, 162);
            this.lbPlayerList.Name = "lbPlayerList";
            this.lbPlayerList.ShowScrollbar = false;
            this.lbPlayerList.Size = new System.Drawing.Size(203, 353);
            this.lbPlayerList.TabIndex = 1;
            this.lbPlayerList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbPlayerList_DrawItem);
            this.lbPlayerList.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.lbPlayerList_MeasureItem);
            this.lbPlayerList.SelectedIndexChanged += new System.EventHandler(this.lbPlayerList_SelectedIndexChanged);
            this.lbPlayerList.DoubleClick += new System.EventHandler(this.lbPlayerList_DoubleClick);
            // 
            // lbGameList
            // 
            this.lbGameList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbGameList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbGameList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lbGameList.FormattingEnabled = true;
            this.lbGameList.IntegralHeight = false;
            this.lbGameList.Location = new System.Drawing.Point(12, 28);
            this.lbGameList.Name = "lbGameList";
            this.lbGameList.ShowScrollbar = true;
            this.lbGameList.Size = new System.Drawing.Size(215, 298);
            this.lbGameList.TabIndex = 0;
            this.lbGameList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbGameList_DrawItem);
            this.lbGameList.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.lbGameList_MeasureItem);
            this.lbGameList.SelectedIndexChanged += new System.EventHandler(this.lbGameList_SelectedIndexChanged);
            this.lbGameList.DoubleClick += new System.EventHandler(this.lbGameList_DoubleClick);
            // 
            // NCnCNetLobby
            // 
            this.AcceptButton = this.btnSend;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(985, 544);
            this.Controls.Add(this.sbPlayers);
            this.Controls.Add(this.sbChat);
            this.Controls.Add(this.chkChannelTO);
            this.Controls.Add(this.btnMusicToggle);
            this.Controls.Add(this.cmbMessageColor);
            this.Controls.Add(this.lblMessageColor);
            this.Controls.Add(this.cmbCurrentChannel);
            this.Controls.Add(this.lblChannel);
            this.Controls.Add(this.btnHideChannels);
            this.Controls.Add(this.lblPlayerList);
            this.Controls.Add(this.lblGames);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.tbChatInput);
            this.Controls.Add(this.lbChatMessages);
            this.Controls.Add(this.btnJoinGame);
            this.Controls.Add(this.btnNewGame);
            this.Controls.Add(this.lblGameInfo);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btnReturnToMenu);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblFollowChannels);
            this.Controls.Add(this.lbPlayerList);
            this.Controls.Add(this.lbGameList);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.MaximumSize = new System.Drawing.Size(10000, 10000);
            this.MinimumSize = new System.Drawing.Size(985, 583);
            this.Name = "NCnCNetLobby";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CnCNet Lobby";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.NCnCNetLobby_FormClosed);
            this.Load += new System.EventHandler(this.NCnCNetLobby_Load);
            this.SizeChanged += new System.EventHandler(this.NCnCNetLobby_SizeChanged);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ScrollbarlessListBox lbGameList;
        private ScrollbarlessListBox lbPlayerList;
        private System.Windows.Forms.Label lblFollowChannels;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnReturnToMenu;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblPlayers;
        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblMapName;
        private System.Windows.Forms.Label lblGameInfo;
        private System.Windows.Forms.Button btnNewGame;
        private System.Windows.Forms.Button btnJoinGame;
        private ScrollbarlessListBox lbChatMessages;
        private System.Windows.Forms.TextBox tbChatInput;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label lblGames;
        private System.Windows.Forms.Label lblPlayerList;
        private System.Windows.Forms.Button btnHideChannels;
        private System.Windows.Forms.Label lblChannel;
        private LimitedComboBox cmbCurrentChannel;
        private LimitedComboBox cmbMessageColor;
        private System.Windows.Forms.Label lblMessageColor;
        private System.Windows.Forms.Button btnMusicToggle;
        private UserCheckBox chkChannelDTA;
        private UserCheckBox chkChannelTI;
        private UserCheckBox chkChannelCnCNet;
        private UserCheckBox chkChannelTS;
        private UserCheckBox chkChannelTO;
        private CustomControls.CustomScrollbar sbChat;
        private CustomControls.CustomScrollbar sbPlayers;
        private System.Windows.Forms.Label lblGameMode;
        private System.Windows.Forms.Label lblPlayer8;
        private System.Windows.Forms.Label lblPlayer7;
        private System.Windows.Forms.Label lblPlayer6;
        private System.Windows.Forms.Label lblPlayer5;
        private System.Windows.Forms.Label lblPlayer4;
        private System.Windows.Forms.Label lblPlayer3;
        private System.Windows.Forms.Label lblPlayer2;
        private System.Windows.Forms.Label lblPlayer1;
    }
}
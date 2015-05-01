namespace ClientGUI
{
    partial class SkirmishLobby
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SkirmishLobby));
            this.lblMapName = new System.Windows.Forms.Label();
            this.lblMapAuthor = new System.Windows.Forms.Label();
            this.btnLeaveGame = new System.Windows.Forms.Button();
            this.btnLaunchGame = new System.Windows.Forms.Button();
            this.lblPlayerName = new System.Windows.Forms.Label();
            this.lblPlayerSide = new System.Windows.Forms.Label();
            this.lblPlayerColor = new System.Windows.Forms.Label();
            this.lblPlayerTeam = new System.Windows.Forms.Label();
            this.lblStart = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmbP8Name = new ClientGUI.LimitedComboBox();
            this.cmbP7Name = new ClientGUI.LimitedComboBox();
            this.cmbP6Name = new ClientGUI.LimitedComboBox();
            this.cmbP5Name = new ClientGUI.LimitedComboBox();
            this.cmbP4Name = new ClientGUI.LimitedComboBox();
            this.cmbP3Name = new ClientGUI.LimitedComboBox();
            this.cmbP2Name = new ClientGUI.LimitedComboBox();
            this.cmbP1Name = new ClientGUI.LimitedComboBox();
            this.cmbP8Start = new ClientGUI.LimitedComboBox();
            this.cmbP7Start = new ClientGUI.LimitedComboBox();
            this.cmbP6Start = new ClientGUI.LimitedComboBox();
            this.cmbP5Start = new ClientGUI.LimitedComboBox();
            this.cmbP4Start = new ClientGUI.LimitedComboBox();
            this.cmbP3Start = new ClientGUI.LimitedComboBox();
            this.cmbP2Start = new ClientGUI.LimitedComboBox();
            this.cmbP1Start = new ClientGUI.LimitedComboBox();
            this.cmbP8Team = new ClientGUI.LimitedComboBox();
            this.cmbP8Color = new ClientGUI.LimitedComboBox();
            this.cmbP8Side = new ClientGUI.LimitedComboBox();
            this.cmbP7Team = new ClientGUI.LimitedComboBox();
            this.cmbP7Color = new ClientGUI.LimitedComboBox();
            this.cmbP7Side = new ClientGUI.LimitedComboBox();
            this.cmbP6Team = new ClientGUI.LimitedComboBox();
            this.cmbP6Color = new ClientGUI.LimitedComboBox();
            this.cmbP6Side = new ClientGUI.LimitedComboBox();
            this.cmbP5Team = new ClientGUI.LimitedComboBox();
            this.cmbP5Color = new ClientGUI.LimitedComboBox();
            this.cmbP5Side = new ClientGUI.LimitedComboBox();
            this.cmbP4Team = new ClientGUI.LimitedComboBox();
            this.cmbP4Color = new ClientGUI.LimitedComboBox();
            this.cmbP4Side = new ClientGUI.LimitedComboBox();
            this.cmbP3Team = new ClientGUI.LimitedComboBox();
            this.cmbP3Color = new ClientGUI.LimitedComboBox();
            this.cmbP3Side = new ClientGUI.LimitedComboBox();
            this.cmbP2Team = new ClientGUI.LimitedComboBox();
            this.cmbP2Color = new ClientGUI.LimitedComboBox();
            this.cmbP2Side = new ClientGUI.LimitedComboBox();
            this.cmbP1Team = new ClientGUI.LimitedComboBox();
            this.cmbP1Color = new ClientGUI.LimitedComboBox();
            this.cmbP1Side = new ClientGUI.LimitedComboBox();
            this.lblGameMode = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.customScrollbar1 = new CustomControls.CustomScrollbar();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cmbCurrGameMode = new ClientGUI.LimitedComboBox();
            this.lbMapList = new ClientGUI.ScrollbarlessListBox();
            this.pbPreview = new ClientGUI.EnhancedPictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMapName
            // 
            this.lblMapName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMapName.AutoSize = true;
            this.lblMapName.BackColor = System.Drawing.Color.Transparent;
            this.lblMapName.Location = new System.Drawing.Point(393, 791);
            this.lblMapName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMapName.Name = "lblMapName";
            this.lblMapName.Size = new System.Drawing.Size(114, 13);
            this.lblMapName.TabIndex = 75;
            this.lblMapName.Text = "Map: No map selected";
            // 
            // lblMapAuthor
            // 
            this.lblMapAuthor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMapAuthor.AutoSize = true;
            this.lblMapAuthor.BackColor = System.Drawing.Color.Transparent;
            this.lblMapAuthor.Location = new System.Drawing.Point(1352, 791);
            this.lblMapAuthor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMapAuthor.Name = "lblMapAuthor";
            this.lblMapAuthor.Size = new System.Drawing.Size(102, 13);
            this.lblMapAuthor.TabIndex = 74;
            this.lblMapAuthor.Text = "By Unknown Author";
            // 
            // btnLeaveGame
            // 
            this.btnLeaveGame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLeaveGame.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnLeaveGame.FlatAppearance.BorderSize = 0;
            this.btnLeaveGame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLeaveGame.ForeColor = System.Drawing.Color.LimeGreen;
            this.btnLeaveGame.Location = new System.Drawing.Point(1452, 808);
            this.btnLeaveGame.Margin = new System.Windows.Forms.Padding(4);
            this.btnLeaveGame.Name = "btnLeaveGame";
            this.btnLeaveGame.Size = new System.Drawing.Size(133, 23);
            this.btnLeaveGame.TabIndex = 77;
            this.btnLeaveGame.TabStop = false;
            this.btnLeaveGame.Text = "Return to Main Menu";
            this.btnLeaveGame.UseVisualStyleBackColor = true;
            this.btnLeaveGame.Click += new System.EventHandler(this.btnLeaveGame_Click);
            this.btnLeaveGame.MouseEnter += new System.EventHandler(this.btnLeaveGame_MouseEnter);
            this.btnLeaveGame.MouseLeave += new System.EventHandler(this.btnLeaveGame_MouseLeave);
            // 
            // btnLaunchGame
            // 
            this.btnLaunchGame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLaunchGame.FlatAppearance.BorderSize = 0;
            this.btnLaunchGame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLaunchGame.ForeColor = System.Drawing.Color.LimeGreen;
            this.btnLaunchGame.Location = new System.Drawing.Point(5, 808);
            this.btnLaunchGame.Margin = new System.Windows.Forms.Padding(4);
            this.btnLaunchGame.Name = "btnLaunchGame";
            this.btnLaunchGame.Size = new System.Drawing.Size(133, 23);
            this.btnLaunchGame.TabIndex = 80;
            this.btnLaunchGame.TabStop = false;
            this.btnLaunchGame.Text = "Launch Game";
            this.btnLaunchGame.UseVisualStyleBackColor = true;
            this.btnLaunchGame.Click += new System.EventHandler(this.btnLaunchGame_Click);
            this.btnLaunchGame.MouseEnter += new System.EventHandler(this.btnLaunchGame_MouseEnter);
            this.btnLaunchGame.MouseLeave += new System.EventHandler(this.btnLaunchGame_MouseLeave);
            // 
            // lblPlayerName
            // 
            this.lblPlayerName.AutoSize = true;
            this.lblPlayerName.BackColor = System.Drawing.Color.Transparent;
            this.lblPlayerName.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblPlayerName.Location = new System.Drawing.Point(-1, 5);
            this.lblPlayerName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPlayerName.Name = "lblPlayerName";
            this.lblPlayerName.Size = new System.Drawing.Size(65, 13);
            this.lblPlayerName.TabIndex = 40;
            this.lblPlayerName.Text = "Player name";
            // 
            // lblPlayerSide
            // 
            this.lblPlayerSide.AutoSize = true;
            this.lblPlayerSide.BackColor = System.Drawing.Color.Transparent;
            this.lblPlayerSide.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblPlayerSide.Location = new System.Drawing.Point(122, 5);
            this.lblPlayerSide.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPlayerSide.Name = "lblPlayerSide";
            this.lblPlayerSide.Size = new System.Drawing.Size(28, 13);
            this.lblPlayerSide.TabIndex = 41;
            this.lblPlayerSide.Text = "Side";
            // 
            // lblPlayerColor
            // 
            this.lblPlayerColor.AutoSize = true;
            this.lblPlayerColor.BackColor = System.Drawing.Color.Transparent;
            this.lblPlayerColor.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblPlayerColor.Location = new System.Drawing.Point(211, 5);
            this.lblPlayerColor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPlayerColor.Name = "lblPlayerColor";
            this.lblPlayerColor.Size = new System.Drawing.Size(31, 13);
            this.lblPlayerColor.TabIndex = 42;
            this.lblPlayerColor.Text = "Color";
            // 
            // lblPlayerTeam
            // 
            this.lblPlayerTeam.AutoSize = true;
            this.lblPlayerTeam.BackColor = System.Drawing.Color.Transparent;
            this.lblPlayerTeam.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblPlayerTeam.Location = new System.Drawing.Point(358, 5);
            this.lblPlayerTeam.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPlayerTeam.Name = "lblPlayerTeam";
            this.lblPlayerTeam.Size = new System.Drawing.Size(34, 13);
            this.lblPlayerTeam.TabIndex = 43;
            this.lblPlayerTeam.Text = "Team";
            // 
            // lblStart
            // 
            this.lblStart.AutoSize = true;
            this.lblStart.BackColor = System.Drawing.Color.Transparent;
            this.lblStart.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblStart.Location = new System.Drawing.Point(300, 5);
            this.lblStart.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(29, 13);
            this.lblStart.TabIndex = 52;
            this.lblStart.Text = "Start";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.cmbP8Name);
            this.panel1.Controls.Add(this.cmbP7Name);
            this.panel1.Controls.Add(this.cmbP6Name);
            this.panel1.Controls.Add(this.cmbP5Name);
            this.panel1.Controls.Add(this.cmbP4Name);
            this.panel1.Controls.Add(this.cmbP3Name);
            this.panel1.Controls.Add(this.cmbP2Name);
            this.panel1.Controls.Add(this.cmbP1Name);
            this.panel1.Controls.Add(this.cmbP8Start);
            this.panel1.Controls.Add(this.cmbP7Start);
            this.panel1.Controls.Add(this.cmbP6Start);
            this.panel1.Controls.Add(this.cmbP5Start);
            this.panel1.Controls.Add(this.cmbP4Start);
            this.panel1.Controls.Add(this.cmbP3Start);
            this.panel1.Controls.Add(this.cmbP2Start);
            this.panel1.Controls.Add(this.cmbP1Start);
            this.panel1.Controls.Add(this.lblStart);
            this.panel1.Controls.Add(this.lblPlayerTeam);
            this.panel1.Controls.Add(this.lblPlayerColor);
            this.panel1.Controls.Add(this.lblPlayerSide);
            this.panel1.Controls.Add(this.lblPlayerName);
            this.panel1.Controls.Add(this.cmbP8Team);
            this.panel1.Controls.Add(this.cmbP8Color);
            this.panel1.Controls.Add(this.cmbP8Side);
            this.panel1.Controls.Add(this.cmbP7Team);
            this.panel1.Controls.Add(this.cmbP7Color);
            this.panel1.Controls.Add(this.cmbP7Side);
            this.panel1.Controls.Add(this.cmbP6Team);
            this.panel1.Controls.Add(this.cmbP6Color);
            this.panel1.Controls.Add(this.cmbP6Side);
            this.panel1.Controls.Add(this.cmbP5Team);
            this.panel1.Controls.Add(this.cmbP5Color);
            this.panel1.Controls.Add(this.cmbP5Side);
            this.panel1.Controls.Add(this.cmbP4Team);
            this.panel1.Controls.Add(this.cmbP4Color);
            this.panel1.Controls.Add(this.cmbP4Side);
            this.panel1.Controls.Add(this.cmbP3Team);
            this.panel1.Controls.Add(this.cmbP3Color);
            this.panel1.Controls.Add(this.cmbP3Side);
            this.panel1.Controls.Add(this.cmbP2Team);
            this.panel1.Controls.Add(this.cmbP2Color);
            this.panel1.Controls.Add(this.cmbP2Side);
            this.panel1.Controls.Add(this.cmbP1Team);
            this.panel1.Controls.Add(this.cmbP1Color);
            this.panel1.Controls.Add(this.cmbP1Side);
            this.panel1.Location = new System.Drawing.Point(442, 1);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(458, 235);
            this.panel1.TabIndex = 73;
            // 
            // cmbP8Name
            // 
            this.cmbP8Name.BackColor = System.Drawing.Color.Black;
            this.cmbP8Name.CanDropDown = true;
            this.cmbP8Name.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbP8Name.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP8Name.ForeColor = System.Drawing.Color.LimeGreen;
            this.cmbP8Name.FormattingEnabled = true;
            this.cmbP8Name.HoveredIndex = -1;
            this.cmbP8Name.Items.AddRange(new object[] {
            "---",
            "Easy AI",
            "Medium AI",
            "Hard AI"});
            this.cmbP8Name.Location = new System.Drawing.Point(3, 204);
            this.cmbP8Name.Name = "cmbP8Name";
            this.cmbP8Name.Size = new System.Drawing.Size(118, 21);
            this.cmbP8Name.TabIndex = 92;
            this.cmbP8Name.UseCustomDrawingCode = true;
            this.cmbP8Name.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGeneric_DrawItem);
            this.cmbP8Name.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP7Name
            // 
            this.cmbP7Name.BackColor = System.Drawing.Color.Black;
            this.cmbP7Name.CanDropDown = true;
            this.cmbP7Name.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbP7Name.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP7Name.ForeColor = System.Drawing.Color.LimeGreen;
            this.cmbP7Name.FormattingEnabled = true;
            this.cmbP7Name.HoveredIndex = -1;
            this.cmbP7Name.Items.AddRange(new object[] {
            "---",
            "Easy AI",
            "Medium AI",
            "Hard AI"});
            this.cmbP7Name.Location = new System.Drawing.Point(3, 178);
            this.cmbP7Name.Margin = new System.Windows.Forms.Padding(2);
            this.cmbP7Name.Name = "cmbP7Name";
            this.cmbP7Name.Size = new System.Drawing.Size(118, 21);
            this.cmbP7Name.TabIndex = 91;
            this.cmbP7Name.UseCustomDrawingCode = true;
            this.cmbP7Name.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGeneric_DrawItem);
            this.cmbP7Name.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP6Name
            // 
            this.cmbP6Name.BackColor = System.Drawing.Color.Black;
            this.cmbP6Name.CanDropDown = true;
            this.cmbP6Name.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbP6Name.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP6Name.ForeColor = System.Drawing.Color.LimeGreen;
            this.cmbP6Name.FormattingEnabled = true;
            this.cmbP6Name.HoveredIndex = -1;
            this.cmbP6Name.Items.AddRange(new object[] {
            "---",
            "Easy AI",
            "Medium AI",
            "Hard AI"});
            this.cmbP6Name.Location = new System.Drawing.Point(3, 152);
            this.cmbP6Name.Name = "cmbP6Name";
            this.cmbP6Name.Size = new System.Drawing.Size(118, 21);
            this.cmbP6Name.TabIndex = 90;
            this.cmbP6Name.UseCustomDrawingCode = true;
            this.cmbP6Name.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGeneric_DrawItem);
            this.cmbP6Name.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP5Name
            // 
            this.cmbP5Name.BackColor = System.Drawing.Color.Black;
            this.cmbP5Name.CanDropDown = true;
            this.cmbP5Name.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbP5Name.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP5Name.ForeColor = System.Drawing.Color.LimeGreen;
            this.cmbP5Name.FormattingEnabled = true;
            this.cmbP5Name.HoveredIndex = -1;
            this.cmbP5Name.Items.AddRange(new object[] {
            "---",
            "Easy AI",
            "Medium AI",
            "Hard AI"});
            this.cmbP5Name.Location = new System.Drawing.Point(3, 126);
            this.cmbP5Name.Margin = new System.Windows.Forms.Padding(2);
            this.cmbP5Name.Name = "cmbP5Name";
            this.cmbP5Name.Size = new System.Drawing.Size(118, 21);
            this.cmbP5Name.TabIndex = 89;
            this.cmbP5Name.UseCustomDrawingCode = true;
            this.cmbP5Name.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGeneric_DrawItem);
            this.cmbP5Name.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP4Name
            // 
            this.cmbP4Name.BackColor = System.Drawing.Color.Black;
            this.cmbP4Name.CanDropDown = true;
            this.cmbP4Name.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbP4Name.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP4Name.ForeColor = System.Drawing.Color.LimeGreen;
            this.cmbP4Name.FormattingEnabled = true;
            this.cmbP4Name.HoveredIndex = -1;
            this.cmbP4Name.Items.AddRange(new object[] {
            "---",
            "Easy AI",
            "Medium AI",
            "Hard AI"});
            this.cmbP4Name.Location = new System.Drawing.Point(3, 100);
            this.cmbP4Name.Name = "cmbP4Name";
            this.cmbP4Name.Size = new System.Drawing.Size(118, 21);
            this.cmbP4Name.TabIndex = 88;
            this.cmbP4Name.UseCustomDrawingCode = true;
            this.cmbP4Name.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGeneric_DrawItem);
            this.cmbP4Name.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP3Name
            // 
            this.cmbP3Name.BackColor = System.Drawing.Color.Black;
            this.cmbP3Name.CanDropDown = true;
            this.cmbP3Name.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbP3Name.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP3Name.ForeColor = System.Drawing.Color.LimeGreen;
            this.cmbP3Name.FormattingEnabled = true;
            this.cmbP3Name.HoveredIndex = -1;
            this.cmbP3Name.Items.AddRange(new object[] {
            "---",
            "Easy AI",
            "Medium AI",
            "Hard AI"});
            this.cmbP3Name.Location = new System.Drawing.Point(3, 74);
            this.cmbP3Name.Margin = new System.Windows.Forms.Padding(2);
            this.cmbP3Name.Name = "cmbP3Name";
            this.cmbP3Name.Size = new System.Drawing.Size(118, 21);
            this.cmbP3Name.TabIndex = 87;
            this.cmbP3Name.UseCustomDrawingCode = true;
            this.cmbP3Name.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGeneric_DrawItem);
            this.cmbP3Name.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP2Name
            // 
            this.cmbP2Name.BackColor = System.Drawing.Color.Black;
            this.cmbP2Name.CanDropDown = true;
            this.cmbP2Name.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbP2Name.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP2Name.ForeColor = System.Drawing.Color.LimeGreen;
            this.cmbP2Name.FormattingEnabled = true;
            this.cmbP2Name.HoveredIndex = -1;
            this.cmbP2Name.Items.AddRange(new object[] {
            "---",
            "Easy AI",
            "Medium AI",
            "Hard AI"});
            this.cmbP2Name.Location = new System.Drawing.Point(3, 48);
            this.cmbP2Name.Name = "cmbP2Name";
            this.cmbP2Name.Size = new System.Drawing.Size(118, 21);
            this.cmbP2Name.TabIndex = 86;
            this.cmbP2Name.UseCustomDrawingCode = true;
            this.cmbP2Name.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGeneric_DrawItem);
            this.cmbP2Name.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP1Name
            // 
            this.cmbP1Name.BackColor = System.Drawing.Color.Black;
            this.cmbP1Name.CanDropDown = false;
            this.cmbP1Name.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbP1Name.ForeColor = System.Drawing.Color.LimeGreen;
            this.cmbP1Name.FormattingEnabled = true;
            this.cmbP1Name.HoveredIndex = -1;
            this.cmbP1Name.Location = new System.Drawing.Point(3, 22);
            this.cmbP1Name.Margin = new System.Windows.Forms.Padding(2);
            this.cmbP1Name.MaxLength = 16;
            this.cmbP1Name.Name = "cmbP1Name";
            this.cmbP1Name.Size = new System.Drawing.Size(118, 21);
            this.cmbP1Name.TabIndex = 85;
            this.cmbP1Name.UseCustomDrawingCode = true;
            this.cmbP1Name.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGeneric_DrawItem);
            this.cmbP1Name.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            this.cmbP1Name.TextChanged += new System.EventHandler(this.cmbP1Name_TextChanged);
            // 
            // cmbP8Start
            // 
            this.cmbP8Start.BackColor = System.Drawing.Color.Black;
            this.cmbP8Start.CanDropDown = true;
            this.cmbP8Start.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbP8Start.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP8Start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbP8Start.ForeColor = System.Drawing.Color.LimeGreen;
            this.cmbP8Start.FormattingEnabled = true;
            this.cmbP8Start.HoveredIndex = -1;
            this.cmbP8Start.Items.AddRange(new object[] {
            "Rndm",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.cmbP8Start.Location = new System.Drawing.Point(303, 204);
            this.cmbP8Start.MaxDropDownItems = 9;
            this.cmbP8Start.Name = "cmbP8Start";
            this.cmbP8Start.Size = new System.Drawing.Size(50, 21);
            this.cmbP8Start.TabIndex = 60;
            this.cmbP8Start.UseCustomDrawingCode = true;
            this.cmbP8Start.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGeneric_DrawItem);
            this.cmbP8Start.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP7Start
            // 
            this.cmbP7Start.BackColor = System.Drawing.Color.Black;
            this.cmbP7Start.CanDropDown = true;
            this.cmbP7Start.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbP7Start.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP7Start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbP7Start.ForeColor = System.Drawing.Color.LimeGreen;
            this.cmbP7Start.FormattingEnabled = true;
            this.cmbP7Start.HoveredIndex = -1;
            this.cmbP7Start.Items.AddRange(new object[] {
            "Rndm",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.cmbP7Start.Location = new System.Drawing.Point(303, 178);
            this.cmbP7Start.Margin = new System.Windows.Forms.Padding(2);
            this.cmbP7Start.MaxDropDownItems = 9;
            this.cmbP7Start.Name = "cmbP7Start";
            this.cmbP7Start.Size = new System.Drawing.Size(50, 21);
            this.cmbP7Start.TabIndex = 59;
            this.cmbP7Start.UseCustomDrawingCode = true;
            this.cmbP7Start.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGeneric_DrawItem);
            this.cmbP7Start.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP6Start
            // 
            this.cmbP6Start.BackColor = System.Drawing.Color.Black;
            this.cmbP6Start.CanDropDown = true;
            this.cmbP6Start.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbP6Start.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP6Start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbP6Start.ForeColor = System.Drawing.Color.LimeGreen;
            this.cmbP6Start.FormattingEnabled = true;
            this.cmbP6Start.HoveredIndex = -1;
            this.cmbP6Start.Items.AddRange(new object[] {
            "Rndm",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.cmbP6Start.Location = new System.Drawing.Point(303, 152);
            this.cmbP6Start.MaxDropDownItems = 9;
            this.cmbP6Start.Name = "cmbP6Start";
            this.cmbP6Start.Size = new System.Drawing.Size(50, 21);
            this.cmbP6Start.TabIndex = 58;
            this.cmbP6Start.UseCustomDrawingCode = true;
            this.cmbP6Start.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGeneric_DrawItem);
            this.cmbP6Start.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP5Start
            // 
            this.cmbP5Start.BackColor = System.Drawing.Color.Black;
            this.cmbP5Start.CanDropDown = true;
            this.cmbP5Start.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbP5Start.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP5Start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbP5Start.ForeColor = System.Drawing.Color.LimeGreen;
            this.cmbP5Start.FormattingEnabled = true;
            this.cmbP5Start.HoveredIndex = -1;
            this.cmbP5Start.Items.AddRange(new object[] {
            "Rndm",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.cmbP5Start.Location = new System.Drawing.Point(303, 126);
            this.cmbP5Start.Margin = new System.Windows.Forms.Padding(2);
            this.cmbP5Start.MaxDropDownItems = 9;
            this.cmbP5Start.Name = "cmbP5Start";
            this.cmbP5Start.Size = new System.Drawing.Size(50, 21);
            this.cmbP5Start.TabIndex = 57;
            this.cmbP5Start.UseCustomDrawingCode = true;
            this.cmbP5Start.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGeneric_DrawItem);
            this.cmbP5Start.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP4Start
            // 
            this.cmbP4Start.BackColor = System.Drawing.Color.Black;
            this.cmbP4Start.CanDropDown = true;
            this.cmbP4Start.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbP4Start.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP4Start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbP4Start.ForeColor = System.Drawing.Color.LimeGreen;
            this.cmbP4Start.FormattingEnabled = true;
            this.cmbP4Start.HoveredIndex = -1;
            this.cmbP4Start.Items.AddRange(new object[] {
            "Rndm",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.cmbP4Start.Location = new System.Drawing.Point(303, 100);
            this.cmbP4Start.MaxDropDownItems = 9;
            this.cmbP4Start.Name = "cmbP4Start";
            this.cmbP4Start.Size = new System.Drawing.Size(50, 21);
            this.cmbP4Start.TabIndex = 56;
            this.cmbP4Start.UseCustomDrawingCode = true;
            this.cmbP4Start.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGeneric_DrawItem);
            this.cmbP4Start.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP3Start
            // 
            this.cmbP3Start.BackColor = System.Drawing.Color.Black;
            this.cmbP3Start.CanDropDown = true;
            this.cmbP3Start.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbP3Start.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP3Start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbP3Start.ForeColor = System.Drawing.Color.LimeGreen;
            this.cmbP3Start.FormattingEnabled = true;
            this.cmbP3Start.HoveredIndex = -1;
            this.cmbP3Start.Items.AddRange(new object[] {
            "Rndm",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.cmbP3Start.Location = new System.Drawing.Point(303, 74);
            this.cmbP3Start.Margin = new System.Windows.Forms.Padding(2);
            this.cmbP3Start.MaxDropDownItems = 9;
            this.cmbP3Start.Name = "cmbP3Start";
            this.cmbP3Start.Size = new System.Drawing.Size(50, 21);
            this.cmbP3Start.TabIndex = 55;
            this.cmbP3Start.UseCustomDrawingCode = true;
            this.cmbP3Start.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGeneric_DrawItem);
            this.cmbP3Start.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP2Start
            // 
            this.cmbP2Start.BackColor = System.Drawing.Color.Black;
            this.cmbP2Start.CanDropDown = true;
            this.cmbP2Start.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbP2Start.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP2Start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbP2Start.ForeColor = System.Drawing.Color.LimeGreen;
            this.cmbP2Start.FormattingEnabled = true;
            this.cmbP2Start.HoveredIndex = -1;
            this.cmbP2Start.Items.AddRange(new object[] {
            "Rndm",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.cmbP2Start.Location = new System.Drawing.Point(303, 48);
            this.cmbP2Start.MaxDropDownItems = 9;
            this.cmbP2Start.Name = "cmbP2Start";
            this.cmbP2Start.Size = new System.Drawing.Size(50, 21);
            this.cmbP2Start.TabIndex = 54;
            this.cmbP2Start.UseCustomDrawingCode = true;
            this.cmbP2Start.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGeneric_DrawItem);
            this.cmbP2Start.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP1Start
            // 
            this.cmbP1Start.BackColor = System.Drawing.Color.Black;
            this.cmbP1Start.CanDropDown = true;
            this.cmbP1Start.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbP1Start.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP1Start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbP1Start.ForeColor = System.Drawing.Color.LimeGreen;
            this.cmbP1Start.FormattingEnabled = true;
            this.cmbP1Start.HoveredIndex = -1;
            this.cmbP1Start.Items.AddRange(new object[] {
            "Rndm",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.cmbP1Start.Location = new System.Drawing.Point(303, 22);
            this.cmbP1Start.Margin = new System.Windows.Forms.Padding(2);
            this.cmbP1Start.MaxDropDownItems = 9;
            this.cmbP1Start.Name = "cmbP1Start";
            this.cmbP1Start.Size = new System.Drawing.Size(50, 21);
            this.cmbP1Start.TabIndex = 53;
            this.cmbP1Start.UseCustomDrawingCode = true;
            this.cmbP1Start.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGeneric_DrawItem);
            this.cmbP1Start.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP8Team
            // 
            this.cmbP8Team.BackColor = System.Drawing.Color.Black;
            this.cmbP8Team.CanDropDown = true;
            this.cmbP8Team.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbP8Team.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP8Team.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbP8Team.ForeColor = System.Drawing.Color.LimeGreen;
            this.cmbP8Team.FormattingEnabled = true;
            this.cmbP8Team.HoveredIndex = -1;
            this.cmbP8Team.Items.AddRange(new object[] {
            "None",
            "Team A",
            "Team B",
            "Team C",
            "Team D"});
            this.cmbP8Team.Location = new System.Drawing.Point(361, 204);
            this.cmbP8Team.Name = "cmbP8Team";
            this.cmbP8Team.Size = new System.Drawing.Size(70, 21);
            this.cmbP8Team.TabIndex = 38;
            this.cmbP8Team.UseCustomDrawingCode = true;
            this.cmbP8Team.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGeneric_DrawItem);
            this.cmbP8Team.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP8Color
            // 
            this.cmbP8Color.BackColor = System.Drawing.Color.Black;
            this.cmbP8Color.CanDropDown = true;
            this.cmbP8Color.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbP8Color.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP8Color.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbP8Color.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.cmbP8Color.FormattingEnabled = true;
            this.cmbP8Color.HoveredIndex = -1;
            this.cmbP8Color.ItemHeight = 15;
            this.cmbP8Color.Location = new System.Drawing.Point(214, 204);
            this.cmbP8Color.MaxDropDownItems = 9;
            this.cmbP8Color.Name = "cmbP8Color";
            this.cmbP8Color.Size = new System.Drawing.Size(80, 21);
            this.cmbP8Color.TabIndex = 37;
            this.cmbP8Color.UseCustomDrawingCode = true;
            this.cmbP8Color.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbPXColor_DrawItem);
            this.cmbP8Color.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP8Side
            // 
            this.cmbP8Side.BackColor = System.Drawing.Color.Black;
            this.cmbP8Side.CanDropDown = true;
            this.cmbP8Side.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbP8Side.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP8Side.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbP8Side.ForeColor = System.Drawing.Color.LimeGreen;
            this.cmbP8Side.FormattingEnabled = true;
            this.cmbP8Side.HoveredIndex = -1;
            this.cmbP8Side.Location = new System.Drawing.Point(126, 204);
            this.cmbP8Side.Name = "cmbP8Side";
            this.cmbP8Side.Size = new System.Drawing.Size(80, 21);
            this.cmbP8Side.TabIndex = 36;
            this.cmbP8Side.UseCustomDrawingCode = true;
            this.cmbP8Side.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGeneric_DrawItem);
            this.cmbP8Side.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP7Team
            // 
            this.cmbP7Team.BackColor = System.Drawing.Color.Black;
            this.cmbP7Team.CanDropDown = true;
            this.cmbP7Team.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbP7Team.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP7Team.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbP7Team.ForeColor = System.Drawing.Color.LimeGreen;
            this.cmbP7Team.FormattingEnabled = true;
            this.cmbP7Team.HoveredIndex = -1;
            this.cmbP7Team.Items.AddRange(new object[] {
            "None",
            "Team A",
            "Team B",
            "Team C",
            "Team D"});
            this.cmbP7Team.Location = new System.Drawing.Point(361, 178);
            this.cmbP7Team.Margin = new System.Windows.Forms.Padding(2);
            this.cmbP7Team.Name = "cmbP7Team";
            this.cmbP7Team.Size = new System.Drawing.Size(70, 21);
            this.cmbP7Team.TabIndex = 33;
            this.cmbP7Team.UseCustomDrawingCode = true;
            this.cmbP7Team.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGeneric_DrawItem);
            this.cmbP7Team.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP7Color
            // 
            this.cmbP7Color.BackColor = System.Drawing.Color.Black;
            this.cmbP7Color.CanDropDown = true;
            this.cmbP7Color.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbP7Color.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP7Color.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbP7Color.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.cmbP7Color.FormattingEnabled = true;
            this.cmbP7Color.HoveredIndex = -1;
            this.cmbP7Color.ItemHeight = 15;
            this.cmbP7Color.Location = new System.Drawing.Point(214, 178);
            this.cmbP7Color.Margin = new System.Windows.Forms.Padding(2);
            this.cmbP7Color.MaxDropDownItems = 9;
            this.cmbP7Color.Name = "cmbP7Color";
            this.cmbP7Color.Size = new System.Drawing.Size(80, 21);
            this.cmbP7Color.TabIndex = 32;
            this.cmbP7Color.UseCustomDrawingCode = true;
            this.cmbP7Color.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbPXColor_DrawItem);
            this.cmbP7Color.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP7Side
            // 
            this.cmbP7Side.BackColor = System.Drawing.Color.Black;
            this.cmbP7Side.CanDropDown = true;
            this.cmbP7Side.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbP7Side.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP7Side.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbP7Side.ForeColor = System.Drawing.Color.LimeGreen;
            this.cmbP7Side.FormattingEnabled = true;
            this.cmbP7Side.HoveredIndex = -1;
            this.cmbP7Side.Location = new System.Drawing.Point(126, 178);
            this.cmbP7Side.Margin = new System.Windows.Forms.Padding(2);
            this.cmbP7Side.Name = "cmbP7Side";
            this.cmbP7Side.Size = new System.Drawing.Size(80, 21);
            this.cmbP7Side.TabIndex = 31;
            this.cmbP7Side.UseCustomDrawingCode = true;
            this.cmbP7Side.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGeneric_DrawItem);
            this.cmbP7Side.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP6Team
            // 
            this.cmbP6Team.BackColor = System.Drawing.Color.Black;
            this.cmbP6Team.CanDropDown = true;
            this.cmbP6Team.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbP6Team.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP6Team.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbP6Team.ForeColor = System.Drawing.Color.LimeGreen;
            this.cmbP6Team.FormattingEnabled = true;
            this.cmbP6Team.HoveredIndex = -1;
            this.cmbP6Team.Items.AddRange(new object[] {
            "None",
            "Team A",
            "Team B",
            "Team C",
            "Team D"});
            this.cmbP6Team.Location = new System.Drawing.Point(361, 152);
            this.cmbP6Team.Name = "cmbP6Team";
            this.cmbP6Team.Size = new System.Drawing.Size(70, 21);
            this.cmbP6Team.TabIndex = 28;
            this.cmbP6Team.UseCustomDrawingCode = true;
            this.cmbP6Team.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGeneric_DrawItem);
            this.cmbP6Team.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP6Color
            // 
            this.cmbP6Color.BackColor = System.Drawing.Color.Black;
            this.cmbP6Color.CanDropDown = true;
            this.cmbP6Color.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbP6Color.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP6Color.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbP6Color.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.cmbP6Color.FormattingEnabled = true;
            this.cmbP6Color.HoveredIndex = -1;
            this.cmbP6Color.ItemHeight = 15;
            this.cmbP6Color.Location = new System.Drawing.Point(214, 152);
            this.cmbP6Color.MaxDropDownItems = 9;
            this.cmbP6Color.Name = "cmbP6Color";
            this.cmbP6Color.Size = new System.Drawing.Size(80, 21);
            this.cmbP6Color.TabIndex = 27;
            this.cmbP6Color.UseCustomDrawingCode = true;
            this.cmbP6Color.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbPXColor_DrawItem);
            this.cmbP6Color.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP6Side
            // 
            this.cmbP6Side.BackColor = System.Drawing.Color.Black;
            this.cmbP6Side.CanDropDown = true;
            this.cmbP6Side.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbP6Side.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP6Side.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbP6Side.ForeColor = System.Drawing.Color.LimeGreen;
            this.cmbP6Side.FormattingEnabled = true;
            this.cmbP6Side.HoveredIndex = -1;
            this.cmbP6Side.Location = new System.Drawing.Point(126, 152);
            this.cmbP6Side.Name = "cmbP6Side";
            this.cmbP6Side.Size = new System.Drawing.Size(80, 21);
            this.cmbP6Side.TabIndex = 26;
            this.cmbP6Side.UseCustomDrawingCode = true;
            this.cmbP6Side.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGeneric_DrawItem);
            this.cmbP6Side.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP5Team
            // 
            this.cmbP5Team.BackColor = System.Drawing.Color.Black;
            this.cmbP5Team.CanDropDown = true;
            this.cmbP5Team.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbP5Team.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP5Team.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbP5Team.ForeColor = System.Drawing.Color.LimeGreen;
            this.cmbP5Team.FormattingEnabled = true;
            this.cmbP5Team.HoveredIndex = -1;
            this.cmbP5Team.Items.AddRange(new object[] {
            "None",
            "Team A",
            "Team B",
            "Team C",
            "Team D"});
            this.cmbP5Team.Location = new System.Drawing.Point(361, 126);
            this.cmbP5Team.Margin = new System.Windows.Forms.Padding(2);
            this.cmbP5Team.Name = "cmbP5Team";
            this.cmbP5Team.Size = new System.Drawing.Size(70, 21);
            this.cmbP5Team.TabIndex = 23;
            this.cmbP5Team.UseCustomDrawingCode = true;
            this.cmbP5Team.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGeneric_DrawItem);
            this.cmbP5Team.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP5Color
            // 
            this.cmbP5Color.BackColor = System.Drawing.Color.Black;
            this.cmbP5Color.CanDropDown = true;
            this.cmbP5Color.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbP5Color.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP5Color.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbP5Color.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.cmbP5Color.FormattingEnabled = true;
            this.cmbP5Color.HoveredIndex = -1;
            this.cmbP5Color.ItemHeight = 15;
            this.cmbP5Color.Location = new System.Drawing.Point(214, 126);
            this.cmbP5Color.Margin = new System.Windows.Forms.Padding(2);
            this.cmbP5Color.MaxDropDownItems = 9;
            this.cmbP5Color.Name = "cmbP5Color";
            this.cmbP5Color.Size = new System.Drawing.Size(80, 21);
            this.cmbP5Color.TabIndex = 22;
            this.cmbP5Color.UseCustomDrawingCode = true;
            this.cmbP5Color.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbPXColor_DrawItem);
            this.cmbP5Color.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP5Side
            // 
            this.cmbP5Side.BackColor = System.Drawing.Color.Black;
            this.cmbP5Side.CanDropDown = true;
            this.cmbP5Side.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbP5Side.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP5Side.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbP5Side.ForeColor = System.Drawing.Color.LimeGreen;
            this.cmbP5Side.FormattingEnabled = true;
            this.cmbP5Side.HoveredIndex = -1;
            this.cmbP5Side.Location = new System.Drawing.Point(126, 126);
            this.cmbP5Side.Margin = new System.Windows.Forms.Padding(2);
            this.cmbP5Side.Name = "cmbP5Side";
            this.cmbP5Side.Size = new System.Drawing.Size(80, 21);
            this.cmbP5Side.TabIndex = 21;
            this.cmbP5Side.UseCustomDrawingCode = true;
            this.cmbP5Side.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGeneric_DrawItem);
            this.cmbP5Side.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP4Team
            // 
            this.cmbP4Team.BackColor = System.Drawing.Color.Black;
            this.cmbP4Team.CanDropDown = true;
            this.cmbP4Team.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbP4Team.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP4Team.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbP4Team.ForeColor = System.Drawing.Color.LimeGreen;
            this.cmbP4Team.FormattingEnabled = true;
            this.cmbP4Team.HoveredIndex = -1;
            this.cmbP4Team.Items.AddRange(new object[] {
            "None",
            "Team A",
            "Team B",
            "Team C",
            "Team D"});
            this.cmbP4Team.Location = new System.Drawing.Point(361, 100);
            this.cmbP4Team.Name = "cmbP4Team";
            this.cmbP4Team.Size = new System.Drawing.Size(70, 21);
            this.cmbP4Team.TabIndex = 18;
            this.cmbP4Team.UseCustomDrawingCode = true;
            this.cmbP4Team.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGeneric_DrawItem);
            this.cmbP4Team.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP4Color
            // 
            this.cmbP4Color.BackColor = System.Drawing.Color.Black;
            this.cmbP4Color.CanDropDown = true;
            this.cmbP4Color.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbP4Color.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP4Color.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbP4Color.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.cmbP4Color.FormattingEnabled = true;
            this.cmbP4Color.HoveredIndex = -1;
            this.cmbP4Color.ItemHeight = 15;
            this.cmbP4Color.Location = new System.Drawing.Point(214, 100);
            this.cmbP4Color.MaxDropDownItems = 9;
            this.cmbP4Color.Name = "cmbP4Color";
            this.cmbP4Color.Size = new System.Drawing.Size(80, 21);
            this.cmbP4Color.TabIndex = 17;
            this.cmbP4Color.UseCustomDrawingCode = true;
            this.cmbP4Color.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbPXColor_DrawItem);
            this.cmbP4Color.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP4Side
            // 
            this.cmbP4Side.BackColor = System.Drawing.Color.Black;
            this.cmbP4Side.CanDropDown = true;
            this.cmbP4Side.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbP4Side.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP4Side.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbP4Side.ForeColor = System.Drawing.Color.LimeGreen;
            this.cmbP4Side.FormattingEnabled = true;
            this.cmbP4Side.HoveredIndex = -1;
            this.cmbP4Side.Location = new System.Drawing.Point(126, 100);
            this.cmbP4Side.Name = "cmbP4Side";
            this.cmbP4Side.Size = new System.Drawing.Size(80, 21);
            this.cmbP4Side.TabIndex = 16;
            this.cmbP4Side.UseCustomDrawingCode = true;
            this.cmbP4Side.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGeneric_DrawItem);
            this.cmbP4Side.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP3Team
            // 
            this.cmbP3Team.BackColor = System.Drawing.Color.Black;
            this.cmbP3Team.CanDropDown = true;
            this.cmbP3Team.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbP3Team.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP3Team.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbP3Team.ForeColor = System.Drawing.Color.LimeGreen;
            this.cmbP3Team.FormattingEnabled = true;
            this.cmbP3Team.HoveredIndex = -1;
            this.cmbP3Team.Items.AddRange(new object[] {
            "None",
            "Team A",
            "Team B",
            "Team C",
            "Team D"});
            this.cmbP3Team.Location = new System.Drawing.Point(361, 74);
            this.cmbP3Team.Margin = new System.Windows.Forms.Padding(2);
            this.cmbP3Team.Name = "cmbP3Team";
            this.cmbP3Team.Size = new System.Drawing.Size(70, 21);
            this.cmbP3Team.TabIndex = 13;
            this.cmbP3Team.UseCustomDrawingCode = true;
            this.cmbP3Team.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGeneric_DrawItem);
            this.cmbP3Team.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP3Color
            // 
            this.cmbP3Color.BackColor = System.Drawing.Color.Black;
            this.cmbP3Color.CanDropDown = true;
            this.cmbP3Color.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbP3Color.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP3Color.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbP3Color.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.cmbP3Color.FormattingEnabled = true;
            this.cmbP3Color.HoveredIndex = -1;
            this.cmbP3Color.ItemHeight = 15;
            this.cmbP3Color.Location = new System.Drawing.Point(214, 74);
            this.cmbP3Color.Margin = new System.Windows.Forms.Padding(2);
            this.cmbP3Color.MaxDropDownItems = 9;
            this.cmbP3Color.Name = "cmbP3Color";
            this.cmbP3Color.Size = new System.Drawing.Size(80, 21);
            this.cmbP3Color.TabIndex = 12;
            this.cmbP3Color.UseCustomDrawingCode = true;
            this.cmbP3Color.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbPXColor_DrawItem);
            this.cmbP3Color.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP3Side
            // 
            this.cmbP3Side.BackColor = System.Drawing.Color.Black;
            this.cmbP3Side.CanDropDown = true;
            this.cmbP3Side.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbP3Side.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP3Side.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbP3Side.ForeColor = System.Drawing.Color.LimeGreen;
            this.cmbP3Side.FormattingEnabled = true;
            this.cmbP3Side.HoveredIndex = -1;
            this.cmbP3Side.Location = new System.Drawing.Point(126, 74);
            this.cmbP3Side.Margin = new System.Windows.Forms.Padding(2);
            this.cmbP3Side.Name = "cmbP3Side";
            this.cmbP3Side.Size = new System.Drawing.Size(80, 21);
            this.cmbP3Side.TabIndex = 11;
            this.cmbP3Side.UseCustomDrawingCode = true;
            this.cmbP3Side.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGeneric_DrawItem);
            this.cmbP3Side.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP2Team
            // 
            this.cmbP2Team.BackColor = System.Drawing.Color.Black;
            this.cmbP2Team.CanDropDown = true;
            this.cmbP2Team.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbP2Team.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP2Team.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbP2Team.ForeColor = System.Drawing.Color.LimeGreen;
            this.cmbP2Team.FormattingEnabled = true;
            this.cmbP2Team.HoveredIndex = -1;
            this.cmbP2Team.Items.AddRange(new object[] {
            "None",
            "Team A",
            "Team B",
            "Team C",
            "Team D"});
            this.cmbP2Team.Location = new System.Drawing.Point(361, 48);
            this.cmbP2Team.Name = "cmbP2Team";
            this.cmbP2Team.Size = new System.Drawing.Size(70, 21);
            this.cmbP2Team.TabIndex = 8;
            this.cmbP2Team.UseCustomDrawingCode = true;
            this.cmbP2Team.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGeneric_DrawItem);
            this.cmbP2Team.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP2Color
            // 
            this.cmbP2Color.BackColor = System.Drawing.Color.Black;
            this.cmbP2Color.CanDropDown = true;
            this.cmbP2Color.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbP2Color.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP2Color.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbP2Color.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.cmbP2Color.FormattingEnabled = true;
            this.cmbP2Color.HoveredIndex = -1;
            this.cmbP2Color.ItemHeight = 15;
            this.cmbP2Color.Location = new System.Drawing.Point(214, 48);
            this.cmbP2Color.MaxDropDownItems = 9;
            this.cmbP2Color.Name = "cmbP2Color";
            this.cmbP2Color.Size = new System.Drawing.Size(80, 21);
            this.cmbP2Color.TabIndex = 7;
            this.cmbP2Color.UseCustomDrawingCode = true;
            this.cmbP2Color.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbPXColor_DrawItem);
            this.cmbP2Color.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP2Side
            // 
            this.cmbP2Side.BackColor = System.Drawing.Color.Black;
            this.cmbP2Side.CanDropDown = true;
            this.cmbP2Side.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbP2Side.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP2Side.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbP2Side.ForeColor = System.Drawing.Color.LimeGreen;
            this.cmbP2Side.FormattingEnabled = true;
            this.cmbP2Side.HoveredIndex = -1;
            this.cmbP2Side.Location = new System.Drawing.Point(126, 48);
            this.cmbP2Side.Name = "cmbP2Side";
            this.cmbP2Side.Size = new System.Drawing.Size(80, 21);
            this.cmbP2Side.TabIndex = 6;
            this.cmbP2Side.UseCustomDrawingCode = true;
            this.cmbP2Side.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGeneric_DrawItem);
            this.cmbP2Side.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP1Team
            // 
            this.cmbP1Team.BackColor = System.Drawing.Color.Black;
            this.cmbP1Team.CanDropDown = true;
            this.cmbP1Team.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbP1Team.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP1Team.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbP1Team.ForeColor = System.Drawing.Color.LimeGreen;
            this.cmbP1Team.FormattingEnabled = true;
            this.cmbP1Team.HoveredIndex = -1;
            this.cmbP1Team.Items.AddRange(new object[] {
            "None",
            "Team A",
            "Team B",
            "Team C",
            "Team D"});
            this.cmbP1Team.Location = new System.Drawing.Point(361, 22);
            this.cmbP1Team.Margin = new System.Windows.Forms.Padding(2);
            this.cmbP1Team.Name = "cmbP1Team";
            this.cmbP1Team.Size = new System.Drawing.Size(70, 21);
            this.cmbP1Team.TabIndex = 3;
            this.cmbP1Team.UseCustomDrawingCode = true;
            this.cmbP1Team.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGeneric_DrawItem);
            this.cmbP1Team.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP1Color
            // 
            this.cmbP1Color.BackColor = System.Drawing.Color.Black;
            this.cmbP1Color.CanDropDown = true;
            this.cmbP1Color.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbP1Color.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP1Color.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbP1Color.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.cmbP1Color.FormattingEnabled = true;
            this.cmbP1Color.HoveredIndex = -1;
            this.cmbP1Color.ItemHeight = 15;
            this.cmbP1Color.Location = new System.Drawing.Point(214, 22);
            this.cmbP1Color.Margin = new System.Windows.Forms.Padding(2);
            this.cmbP1Color.MaxDropDownItems = 9;
            this.cmbP1Color.Name = "cmbP1Color";
            this.cmbP1Color.Size = new System.Drawing.Size(80, 21);
            this.cmbP1Color.TabIndex = 2;
            this.cmbP1Color.UseCustomDrawingCode = true;
            this.cmbP1Color.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbPXColor_DrawItem);
            this.cmbP1Color.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // cmbP1Side
            // 
            this.cmbP1Side.BackColor = System.Drawing.Color.Black;
            this.cmbP1Side.CanDropDown = true;
            this.cmbP1Side.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbP1Side.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP1Side.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbP1Side.ForeColor = System.Drawing.Color.LimeGreen;
            this.cmbP1Side.FormattingEnabled = true;
            this.cmbP1Side.HoveredIndex = -1;
            this.cmbP1Side.Location = new System.Drawing.Point(126, 22);
            this.cmbP1Side.Margin = new System.Windows.Forms.Padding(2);
            this.cmbP1Side.Name = "cmbP1Side";
            this.cmbP1Side.Size = new System.Drawing.Size(80, 21);
            this.cmbP1Side.TabIndex = 1;
            this.cmbP1Side.UseCustomDrawingCode = true;
            this.cmbP1Side.SelectedIndexChanged += new System.EventHandler(this.CopyPlayerDataFromUI);
            // 
            // lblGameMode
            // 
            this.lblGameMode.AutoSize = true;
            this.lblGameMode.BackColor = System.Drawing.Color.Transparent;
            this.lblGameMode.Location = new System.Drawing.Point(6, 252);
            this.lblGameMode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGameMode.Name = "lblGameMode";
            this.lblGameMode.Size = new System.Drawing.Size(68, 13);
            this.lblGameMode.TabIndex = 82;
            this.lblGameMode.Text = "Game Mode:";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Location = new System.Drawing.Point(1, 1);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(433, 235);
            this.panel2.TabIndex = 86;
            // 
            // customScrollbar1
            // 
            this.customScrollbar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.customScrollbar1.ChannelColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(166)))), ((int)(((byte)(3)))));
            this.customScrollbar1.DownArrowImage = ((System.Drawing.Image)(resources.GetObject("customScrollbar1.DownArrowImage")));
            this.customScrollbar1.LargeChange = 10;
            this.customScrollbar1.Location = new System.Drawing.Point(374, 276);
            this.customScrollbar1.Maximum = 100;
            this.customScrollbar1.Minimum = 0;
            this.customScrollbar1.MinimumSize = new System.Drawing.Size(15, 92);
            this.customScrollbar1.Name = "customScrollbar1";
            this.customScrollbar1.Size = new System.Drawing.Size(16, 522);
            this.customScrollbar1.SmallChange = 1;
            this.customScrollbar1.TabIndex = 87;
            this.customScrollbar1.ThumbBottomImage = ((System.Drawing.Image)(resources.GetObject("customScrollbar1.ThumbBottomImage")));
            this.customScrollbar1.ThumbBottomSpanImage = ((System.Drawing.Image)(resources.GetObject("customScrollbar1.ThumbBottomSpanImage")));
            this.customScrollbar1.ThumbMiddleImage = ((System.Drawing.Image)(resources.GetObject("customScrollbar1.ThumbMiddleImage")));
            this.customScrollbar1.ThumbTopImage = ((System.Drawing.Image)(resources.GetObject("customScrollbar1.ThumbTopImage")));
            this.customScrollbar1.ThumbTopSpanImage = ((System.Drawing.Image)(resources.GetObject("customScrollbar1.ThumbTopSpanImage")));
            this.customScrollbar1.UpArrowImage = ((System.Drawing.Image)(resources.GetObject("customScrollbar1.UpArrowImage")));
            this.customScrollbar1.Value = 0;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 20000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // cmbCurrGameMode
            // 
            this.cmbCurrGameMode.CanDropDown = true;
            this.cmbCurrGameMode.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbCurrGameMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCurrGameMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbCurrGameMode.FormattingEnabled = true;
            this.cmbCurrGameMode.HoveredIndex = -1;
            this.cmbCurrGameMode.ItemHeight = 15;
            this.cmbCurrGameMode.Location = new System.Drawing.Point(237, 249);
            this.cmbCurrGameMode.Margin = new System.Windows.Forms.Padding(0);
            this.cmbCurrGameMode.MaxDropDownItems = 20;
            this.cmbCurrGameMode.Name = "cmbCurrGameMode";
            this.cmbCurrGameMode.Size = new System.Drawing.Size(150, 21);
            this.cmbCurrGameMode.TabIndex = 83;
            this.cmbCurrGameMode.UseCustomDrawingCode = true;
            this.cmbCurrGameMode.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGeneric_DrawItem);
            this.cmbCurrGameMode.SelectedIndexChanged += new System.EventHandler(this.cmbCurrGameMode_SelectedIndexChanged);
            // 
            // lbMapList
            // 
            this.lbMapList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbMapList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbMapList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lbMapList.FormattingEnabled = true;
            this.lbMapList.IntegralHeight = false;
            this.lbMapList.Location = new System.Drawing.Point(6, 276);
            this.lbMapList.Margin = new System.Windows.Forms.Padding(4);
            this.lbMapList.Name = "lbMapList";
            this.lbMapList.ShowScrollbar = false;
            this.lbMapList.Size = new System.Drawing.Size(369, 522);
            this.lbMapList.TabIndex = 81;
            this.lbMapList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbMapList_DrawItem);
            this.lbMapList.SelectedIndexChanged += new System.EventHandler(this.lbMapList_SelectedIndexChanged);
            // 
            // pbPreview
            // 
            this.pbPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbPreview.BackColor = System.Drawing.Color.Black;
            this.pbPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbPreview.Location = new System.Drawing.Point(396, 244);
            this.pbPreview.Margin = new System.Windows.Forms.Padding(4);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new System.Drawing.Size(1190, 543);
            this.pbPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbPreview.TabIndex = 76;
            this.pbPreview.TabStop = false;
            this.pbPreview.SizeChanged += new System.EventHandler(this.pbPreview_SizeChanged);
            this.pbPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.pbPreview_Paint);
            this.pbPreview.MouseEnter += new System.EventHandler(this.pbPreview_MouseEnter);
            this.pbPreview.MouseLeave += new System.EventHandler(this.pbPreview_MouseLeave);
            // 
            // SkirmishLobby
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btnLeaveGame;
            this.ClientSize = new System.Drawing.Size(1590, 837);
            this.Controls.Add(this.customScrollbar1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.cmbCurrGameMode);
            this.Controls.Add(this.lblGameMode);
            this.Controls.Add(this.lbMapList);
            this.Controls.Add(this.btnLaunchGame);
            this.Controls.Add(this.btnLeaveGame);
            this.Controls.Add(this.pbPreview);
            this.Controls.Add(this.lblMapName);
            this.Controls.Add(this.lblMapAuthor);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(898, 560);
            this.Name = "SkirmishLobby";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Skirmish Lobby";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.NGameLobby_FormClosed);
            this.Load += new System.EventHandler(this.NGameLobby_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMapName;
        private System.Windows.Forms.Label lblMapAuthor;
        private ClientGUI.EnhancedPictureBox pbPreview;
        private System.Windows.Forms.Button btnLeaveGame;
        private System.Windows.Forms.Button btnLaunchGame;
        private LimitedComboBox cmbP1Side;
        private LimitedComboBox cmbP1Color;
        private LimitedComboBox cmbP1Team;
        private LimitedComboBox cmbP2Side;
        private LimitedComboBox cmbP2Color;
        private LimitedComboBox cmbP2Team;
        private LimitedComboBox cmbP3Side;
        private LimitedComboBox cmbP3Color;
        private LimitedComboBox cmbP3Team;
        private LimitedComboBox cmbP4Side;
        private LimitedComboBox cmbP4Color;
        private LimitedComboBox cmbP4Team;
        private LimitedComboBox cmbP5Side;
        private LimitedComboBox cmbP5Color;
        private LimitedComboBox cmbP5Team;
        private LimitedComboBox cmbP6Side;
        private LimitedComboBox cmbP6Color;
        private LimitedComboBox cmbP6Team;
        private LimitedComboBox cmbP7Side;
        private LimitedComboBox cmbP7Color;
        private LimitedComboBox cmbP7Team;
        private LimitedComboBox cmbP8Side;
        private LimitedComboBox cmbP8Color;
        private LimitedComboBox cmbP8Team;
        private System.Windows.Forms.Label lblPlayerName;
        private System.Windows.Forms.Label lblPlayerSide;
        private System.Windows.Forms.Label lblPlayerColor;
        private System.Windows.Forms.Label lblPlayerTeam;
        private System.Windows.Forms.Label lblStart;
        private LimitedComboBox cmbP1Start;
        private LimitedComboBox cmbP2Start;
        private LimitedComboBox cmbP3Start;
        private LimitedComboBox cmbP4Start;
        private LimitedComboBox cmbP5Start;
        private LimitedComboBox cmbP6Start;
        private LimitedComboBox cmbP7Start;
        private LimitedComboBox cmbP8Start;
        private LimitedComboBox cmbP1Name;
        private LimitedComboBox cmbP2Name;
        private LimitedComboBox cmbP3Name;
        private LimitedComboBox cmbP4Name;
        private LimitedComboBox cmbP5Name;
        private LimitedComboBox cmbP6Name;
        private LimitedComboBox cmbP7Name;
        private LimitedComboBox cmbP8Name;
        private System.Windows.Forms.Panel panel1;
        private ScrollbarlessListBox lbMapList;
        private System.Windows.Forms.Label lblGameMode;
        private System.Windows.Forms.Panel panel2;
        private LimitedComboBox cmbCurrGameMode;
        private CustomControls.CustomScrollbar customScrollbar1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
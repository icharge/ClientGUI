﻿/// @author Rampastring
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
using System.Threading;
using System.Runtime.InteropServices;
using System.Reflection;
using ClientCore;
using ClientCore.CnCNet5;
using ClientCore.Statistics;

namespace ClientGUI
{
    /// <summary>
    /// The Skirmish Game Lobby.
    /// </summary>
    public partial class SkirmishLobby : Form
    {
        /// <summary>
        /// Creates a new instance of the Skirmish lobby.
        /// </summary>
        public SkirmishLobby()
        {
            Logger.Log("Entering Skirmish lobby.");

            Application.VisualStyleState = System.Windows.Forms.VisualStyles.VisualStyleState.NonClientAreaEnabled;

            resizeTimer = new System.Windows.Forms.Timer();
            resizeTimer.Interval = 500;
            resizeTimer.Tick += new EventHandler(resizeTimer_Tick);

            InitializeComponent();
            ProgramConstants.CNCNET_PLAYERNAME = DomainController.Instance().getMpHandle();
            Players.Add(new PlayerInfo(ProgramConstants.CNCNET_PLAYERNAME, 0, 0, 0, 0));
        }

        PropertyInfo imageRectangleProperty = typeof(PictureBox).GetProperty("ImageRectangle", BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance);

        bool updatePlayers = true;

        List<PlayerInfo> Players = new List<PlayerInfo>();
        List<PlayerInfo> AIPlayers = new List<PlayerInfo>();

        List<Color> MPColors;
        List<UserCheckBox> CheckBoxes = new List<UserCheckBox>();
        List<string> AssociatedCheckBoxSpawnIniOptions = new List<string>();
        List<string> AssociatedCheckBoxCustomInis = new List<string>();
        List<LimitedComboBox> ComboBoxes = new List<LimitedComboBox>();
        List<string> AssociatedComboBoxSpawnIniOptions = new List<string>();
        List<string> ComboBoxSidePrereqErrorDescriptions = new List<string>();
        List<DataWriteMode> ComboBoxDataWriteModes = new List<DataWriteMode>();
        List<MessageInfo> MessageInfos = new List<MessageInfo>();
        List<SideComboboxPrerequisite> SideComboboxPrerequisites = new List<SideComboboxPrerequisite>();
        List<bool> IsCheckBoxReversed = new List<bool>();

        TextBox pNameTextBox;
        TextBox[] pSideLabels;

        Map currentMap;

        int iNumLoadingScreens = 0;
        int coopDifficultyLevel = 0;

        SoundPlayer sndButtonSound;

        Image btn133px;
        Image btn133px_c;

        Image[] startingLocationIndicators;
        Image enemyStartingLocationIndicator;
        double previewRatioX = 1.0;
        double previewRatioY = 1.0;

        Image missingPreviewImage;
        List<int> mapIndexesInList = new List<int>();

        List<string>[] playerNamesOnPlayerLocations;
        List<int>[] playerColorsOnPlayerLocations;
        Font playerNameOnPlayerLocationFont;
        string[] TeamIdentifiers;

        System.Windows.Forms.Timer resizeTimer;

        bool sharpenPreview = true;

        bool displayCoopBriefing = true;
        Font coopBriefingFont;
        Color coopBriefingForeColor;

        Color cListBoxFocusColor;

        MatchStatistics ms;

        /// <summary>
        /// Sets up the theme of the skirmish lobby and performs initialization.
        /// </summary>
        private void NGameLobby_Load(object sender, EventArgs e)
        {
            SharedUILogic.GameProcessExited += GameProcessExited;

            foreach (string gameMode in CnCNetData.GameTypes)
                cmbCurrGameMode.Items.Add(gameMode);

            cmbP1Name.Text = ProgramConstants.CNCNET_PLAYERNAME;

            this.Font = SharedLogic.getCommonFont();

            startingLocationIndicators = SharedUILogic.LoadStartingLocationIndicators();

            enemyStartingLocationIndicator = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "enemyslocindicator.png");

            playerNamesOnPlayerLocations = new List<string>[8];
            playerColorsOnPlayerLocations = new List<int>[8];
            for (int id = 0; id < 8; id++)
            {
                playerNamesOnPlayerLocations[id] = new List<string>();
                playerColorsOnPlayerLocations[id] = new List<int>();
            }

            coopBriefingFont = new System.Drawing.Font("Segoe UI", 11.25f, FontStyle.Regular);
            playerNameOnPlayerLocationFont = new Font("Segoe UI", 8.25f, FontStyle.Regular);
            TeamIdentifiers = SharedUILogic.GetTeamIdentifiers();

            missingPreviewImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "nopreview.png");

            this.Icon = Icon.ExtractAssociatedIcon(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "clienticon.ico");
            this.BackgroundImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "gamelobbybg.png");
            panel1.BackgroundImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "gamelobbypanelbg.png");
            panel2.BackgroundImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "gamelobbyoptionspanelbg.png");

            btn133px = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "133pxbtn.png");
            btn133px_c = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "133pxbtn_c.png");

            btnLaunchGame.BackgroundImage = btn133px;
            btnLeaveGame.BackgroundImage = btn133px;

            SharedUILogic.SetBackgroundImageLayout(this);

            sndButtonSound = new SoundPlayer(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "button.wav");

            sharpenPreview = DomainController.Instance().getImageSharpeningSkirmishStatus();

            string[] mpColorNames = DomainController.Instance().getMPColorNames().Split(',');
            for (int cmbId = 1; cmbId < 9; cmbId++)
            {
                getPlayerColorCMBFromId(cmbId).AddItem("Random", Color.White);
            }

            MPColors = SharedUILogic.GetMPColors();

            for (int colorId = 1; colorId < 9; colorId++)
            {
                for (int cmbId = 1; cmbId < 9; cmbId++)
                {
                    getPlayerColorCMBFromId(cmbId).AddItem(mpColorNames[colorId - 1], MPColors[colorId]);
                }
            }

            cListBoxFocusColor = SharedUILogic.GetColorFromString(DomainController.Instance().getListBoxFocusColor());

            iNumLoadingScreens = DomainController.Instance().getLoadScreenCount();

            string[] sides = DomainController.Instance().getSides().Split(',');

            for (int sideId = 0; sideId < sides.Length; sideId++)
                SideComboboxPrerequisites.Add(new SideComboboxPrerequisite());

            Image[] sideImages = SharedUILogic.LoadSideImages();

            for (int pId = 1; pId < 9; pId++)
            {
                LimitedComboBox cmb = getPlayerSideCMBFromId(pId);
                cmb.Items.Add(new DropDownItem("Random", sideImages[0]));
                cmb.FocusColor = cListBoxFocusColor;
            }

            int i = 1;
            foreach (string sideName in sides)
            {
                for (int pId = 1; pId < 9; pId++)
                {
                    getPlayerSideCMBFromId(pId).Items.Add(new DropDownItem(sideName, sideImages[i]));
                }

                i++;
            }

            cmbP1Side.Items.Add(new DropDownItem("Spectator", sideImages[sides.Length + 1]));

            string panelBorderStyle = DomainController.Instance().getPanelBorderStyle();
            if (panelBorderStyle == "FixedSingle")
            {
                panel1.BorderStyle = BorderStyle.FixedSingle;
                panel2.BorderStyle = BorderStyle.FixedSingle;
            }
            else if (panelBorderStyle == "Fixed3D")
            {
                panel1.BorderStyle = BorderStyle.Fixed3D;
                panel2.BorderStyle = BorderStyle.Fixed3D;
            }
            else
            {
                panel1.BorderStyle = BorderStyle.None;
                panel2.BorderStyle = BorderStyle.None;
            }

            IniFile clIni = new IniFile(ProgramConstants.gamepath + "Resources\\GameOptions.ini");

            Color cLabelColor = SharedUILogic.GetColorFromString(DomainController.Instance().getUILabelColor());

            Color cAltUiColor = SharedUILogic.GetColorFromString(DomainController.Instance().getUIAltColor());

            Color cBackColor = SharedUILogic.GetColorFromString(DomainController.Instance().getUIAltBackgroundColor());

            toolTip1.BackColor = cBackColor;
            toolTip1.ForeColor = cLabelColor;

            coopBriefingForeColor = SharedUILogic.GetColorFromString(DomainController.Instance().getBriefingForeColor());

            int displayedItems = lbMapList.DisplayRectangle.Height / lbMapList.ItemHeight;

            customScrollbar1.ThumbBottomImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "sbThumbBottom.png");
            customScrollbar1.ThumbBottomSpanImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "sbThumbBottomSpan.png");
            customScrollbar1.ThumbMiddleImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "sbMiddle.png");
            customScrollbar1.ThumbTopImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "sbThumbTop.png");
            customScrollbar1.ThumbTopSpanImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "sbThumbTopSpan.png");
            customScrollbar1.UpArrowImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "sbUpArrow.png");
            customScrollbar1.DownArrowImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "sbDownArrow.png");
            customScrollbar1.BackgroundImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "sbBackground.png");
            customScrollbar1.Scroll += customScrollbar1_Scroll;
            customScrollbar1.Maximum = lbMapList.Items.Count - Convert.ToInt32(displayedItems * 0.2);
            customScrollbar1.Minimum = 0;
            customScrollbar1.ChannelColor = cBackColor;
            customScrollbar1.LargeChange = 30;
            customScrollbar1.SmallChange = 10;
            customScrollbar1.Value = 0;

            lbMapList.MouseWheel += lbMapList_MouseWheel;

            pNameTextBox = new TextBox();
            pNameTextBox.Location = getPlayerNameCMBFromId(1).Location;
            pNameTextBox.Size = getPlayerNameCMBFromId(1).Size;
            pNameTextBox.BorderStyle = BorderStyle.FixedSingle;
            pNameTextBox.Font = cmbP1Name.Font;
            pNameTextBox.ForeColor = cAltUiColor;
            pNameTextBox.BackColor = cBackColor;
            pNameTextBox.TextChanged += pNameTextBox_TextChanged;
            pNameTextBox.Text = ProgramConstants.CNCNET_PLAYERNAME;
            pNameTextBox.MaxLength = 16;
            panel1.Controls.Add(pNameTextBox);

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
                forcedSideBox.GotFocus += forcedSideBox_GotFocus;
            }

            string[] checkBoxes = clIni.GetStringValue("SkirmishLobby", "CheckBoxes", "none").Split(',');

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
                    chkBox.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    chkBox.AutoSize = true;
                    chkBox.Location = pLocation;
                    chkBox.Name = checkBoxName;
                    if (defaultValue)
                        chkBox.Checked = true;
                    else
                        chkBox.Checked = false;

                    if (!String.IsNullOrEmpty(toolTip))
                    {
                        toolTip1.SetToolTip(chkBox, toolTip);
                        toolTip1.SetToolTip(chkBox.label1, toolTip);
                        toolTip1.SetToolTip(chkBox.button1, toolTip);
                    }

                    CheckBoxes.Add(chkBox);
                    this.panel2.Controls.Add(CheckBoxes[CheckBoxes.Count - 1]);

                    IsCheckBoxReversed.Add(reversed);
                }
                else
                    throw new Exception("No data exists for CheckBox " + checkBoxName + "!");
            }

            string[] comboBoxes = clIni.GetStringValue("SkirmishLobby", "ComboBoxes", "none").Split(',');
            foreach (string comboBoxName in comboBoxes)
            {
                if (clIni.SectionExists(comboBoxName))
                {
                    LimitedComboBox cmbBox = SharedUILogic.SetUpComboBoxFromIni(comboBoxName, clIni,
                        toolTip1, cmbP1Name.Font, cAltUiColor, cAltUiColor);
                    cmbBox.DrawItem += cmbGeneric_DrawItem;
                    string sideErrorSetDescr = clIni.GetStringValue(comboBoxName, "SideErrorSetDescr", "none");
                    string associateSpawnIniOption = clIni.GetStringValue(comboBoxName, "AssociateSpawnIniOption", "none");

                    if (sideErrorSetDescr != "none")
                        cmbBox.SelectedIndexChanged += CopyPlayerDataFromUI;

                    ComboBoxes.Add(cmbBox);
                    this.panel2.Controls.Add(cmbBox);
                    AssociatedComboBoxSpawnIniOptions.Add(associateSpawnIniOption);
                    ComboBoxSidePrereqErrorDescriptions.Add(sideErrorSetDescr);
                    ComboBoxDataWriteModes.Add((DataWriteMode)cmbBox.Tag);
                }
                else
                    throw new Exception("No data exists for ComboBox " + comboBoxName + "!");
            }

            string sideOptionPrerequisites = clIni.GetStringValue("SkirmishLobby", "SideOptionPrerequisites", "none");
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

            string[] labels = clIni.GetStringValue("SkirmishLobby", "Labels", "none").Split(',');
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

                    this.panel2.Controls.Add(label);
                }
                else
                    throw new Exception("No data exists for label " + labelName + "!");
            }

            SharedUILogic.SetControlColor(cLabelColor, cBackColor, cAltUiColor, cListBoxFocusColor, this);

            LoadDefaultMap();
            LoadSettings();

            CopyPlayerDataToUI();

            resizeTimer = new System.Windows.Forms.Timer();
            resizeTimer.Interval = 500;
            resizeTimer.Tick += new EventHandler(resizeTimer_Tick);

            string[] windowSize = DomainController.Instance().getWindowSizeSkirmish().Split('x');
            int sizeX = Convert.ToInt32(windowSize[0]);
            if (sizeX > Screen.PrimaryScreen.Bounds.Width + 10)
                sizeX = Screen.PrimaryScreen.Bounds.Width - 10;
            int sizeY = Convert.ToInt32(windowSize[1]);
            if (sizeY > Screen.PrimaryScreen.Bounds.Height - 40)
                sizeY = Screen.PrimaryScreen.Bounds.Height - 40;

            this.ClientSize = new Size(sizeX, sizeY);
            this.Location = new Point((Screen.PrimaryScreen.Bounds.Width - this.Size.Width) / 2,
                (Screen.PrimaryScreen.Bounds.Height - this.Size.Height) / 2);

            SharedUILogic.ParseClientThemeIni(this);

            lbMapList.Select();
        }

        /// <summary>
        /// Enables scrolling the map list with the mouse wheel.
        /// </summary>
        private void lbMapList_MouseWheel(object sender, MouseEventArgs e)
        {
            customScrollbar1.Value += e.Delta / -40;
            if (customScrollbar1.Value > customScrollbar1.Maximum)
                customScrollbar1.Value = customScrollbar1.Maximum;

            if (customScrollbar1.Value < customScrollbar1.Minimum)
                customScrollbar1.Value = customScrollbar1.Minimum;

            customScrollbar1_Scroll(sender, EventArgs.Empty);
            //MessageBox.Show(e.Delta.ToString() + " " + customScrollbar1.Value.ToString());
        }

        private void customScrollbar1_Scroll(object sender, EventArgs e)
        {
            lbMapList.TopIndex = customScrollbar1.Value;
        }

        /// <summary>
        /// Applies the new player name if it is changed.
        /// </summary>
        private void pNameTextBox_TextChanged(object sender, EventArgs e)
        {
            ProgramConstants.CNCNET_PLAYERNAME = pNameTextBox.Text;
            Players[0].Name = pNameTextBox.Text;
        }

        /// <summary>
        /// Re-loads the preview to sharpen it some time after the window has been resized.
        /// By using a timer we avoid sharpening the preview for each pixel the user resizes the window by.
        /// </summary>
        private void resizeTimer_Tick(object sender, EventArgs e)
        {
            LoadPreview();
            resizeTimer.Stop();
        }

        /// <summary>
        /// Loads the default map.
        /// </summary>
        private void LoadDefaultMap()
        {
            cmbCurrGameMode.SelectedIndex = 0;
            ListMaps();
            lbMapList.SelectedIndex = 0;
        }

        /// <summary>
        /// Clears the map list and adds maps of the current game mode into the list.
        /// </summary>
        private void ListMaps()
        {
            lbMapList.Items.Clear();
            mapIndexesInList.Clear();
            if (cmbCurrGameMode.SelectedIndex == -1)
            {
                return;
            }

            string gameMode = cmbCurrGameMode.SelectedItem.ToString();

            List<Map> MapList = CnCNetData.MapList;
            for (int mId = 0; mId < MapList.Count; mId++)
            {
                if (MapList[mId].GameModes.Contains(gameMode))
                {
                    lbMapList.Items.Add(MapList[mId].Name);
                    mapIndexesInList.Add(mId);
                }
            }
        }

        /// <summary>
        /// Refreshes the UI when the map is changed.
        /// </summary>
        private void lbMapList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbMapList.SelectedIndex == -1)
                return;

            Map map = CnCNetData.MapList[mapIndexesInList[lbMapList.SelectedIndex]];

            currentMap = map;
            lblMapName.Text = "Map: " + currentMap.Name;
            lblMapAuthor.Text = "By " + currentMap.Author;

            LockOptions();

            LoadPreview();

            if (currentMap.IsCoop)
            {
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

            customScrollbar1.Value = lbMapList.TopIndex;
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

            // Scale and sharpen the preview if we should do so according to GameOptions.ini
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
        /// Generic function for customized drawing of items in player color combo boxes.
        /// </summary>
        private void cmbPXColor_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index > -1 && e.Index < cmbP1Color.Items.Count)
            {
                e.DrawBackground();
                e.DrawFocusRectangle();

                e.Graphics.DrawString(cmbP1Color.Items[e.Index].ToString(), e.Font, new SolidBrush(MPColors[e.Index]), e.Bounds);
            }
        }

        /// <summary>
        /// Generic function for customized drawing of combo box items.
        /// </summary>
        private void cmbGeneric_DrawItem(object sender, DrawItemEventArgs e)
        {
            LimitedComboBox comboBox = (LimitedComboBox)sender;
            if (e.Index > -1 && e.Index < comboBox.Items.Count)
            {
                e.DrawBackground();
                e.DrawFocusRectangle();

                e.Graphics.DrawString(comboBox.Items[e.Index].ToString(), e.Font,
                    new SolidBrush(comboBox.ForeColor), e.Bounds);
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

        private void btnLeaveGame_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// Copies the player data in the UI to the internal player data in memory.
        /// Used when player/AI options are changed by the user.
        /// </summary>
        private void CopyPlayerDataFromUI(object sender, EventArgs e)
        {
            if (!updatePlayers)
                return;

            for (int pId = 0; pId < Players.Count; pId++)
            {
                Players[pId].Name = ProgramConstants.CNCNET_PLAYERNAME;
                getPlayerNameCMBFromId(pId + 1).CanDropDown = false;
                Players[pId].ColorId = getPlayerColorCMBFromId(pId + 1).SelectedIndex;
                int sideId = getPlayerSideCMBFromId(pId + 1).SelectedIndex;
                if (!isSideAllowed(sideId) || sideId < 0)
                    getPlayerSideCMBFromId(pId + 1).SelectedIndex = Players[pId].SideId;
                else
                    Players[pId].SideId = sideId;
                Players[pId].StartingLocation = getPlayerStartCMBFromId(pId + 1).SelectedIndex;
                Players[pId].TeamId = getPlayerTeamCMBFromId(pId + 1).SelectedIndex;
            }

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
                    getPlayerSideCMBFromId(cmbId + 1).SelectedIndex = 0;
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
            }

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
                        MessageBox.Show(ComboBoxSidePrereqErrorDescriptions[comboBoxId] + " must be set for playing as " +
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
                playerColorsOnPlayerLocations[id].Clear();
                playerNamesOnPlayerLocations[id].Clear();
            }

            // Human players
            for (int pId = 0; pId < Players.Count; pId++)
            {
                LimitedComboBox lcb = getPlayerNameCMBFromId(pId + 1);
                lcb.Text = Players[pId].Name;
                lcb.CanDropDown = false;
                lcb.Enabled = false;
                lcb.Visible = false;
                pNameTextBox.Text = Players[pId].Name;
                LimitedComboBox sideBox = getPlayerSideCMBFromId(pId + 1);
                sideBox.SelectedIndex = Players[pId].SideId;
                sideBox.Enabled = true;
                LimitedComboBox colorBox = getPlayerColorCMBFromId(pId + 1);
                colorBox.SelectedIndex = Players[pId].ColorId;
                colorBox.Enabled = true;
                LimitedComboBox startBox = getPlayerStartCMBFromId(pId + 1);
                startBox.SelectedIndex = Players[pId].StartingLocation;
                startBox.Enabled = true;
                if (startBox.SelectedIndex > 0)
                {
                    // Add info to the dynamic preview display
                    if (Players[pId].TeamId == 0)
                        playerNamesOnPlayerLocations[startBox.SelectedIndex - 1].Add(Players[pId].Name);
                    else
                        playerNamesOnPlayerLocations[startBox.SelectedIndex - 1].Add(TeamIdentifiers[Players[pId].TeamId - 1] + Players[pId].Name);

                    playerColorsOnPlayerLocations[startBox.SelectedIndex - 1].Add(Players[pId].ColorId);
                }
                LimitedComboBox teamBox = getPlayerTeamCMBFromId(pId + 1);
                teamBox.SelectedIndex = Players[pId].TeamId;
                teamBox.Enabled = true;

                sideBox.CanDropDown = true;
                colorBox.CanDropDown = true;
                startBox.CanDropDown = true;
                teamBox.CanDropDown = true;
            }

            // AI players
            int playerCount = Players.Count;
            for (int aiId = 0; aiId < AIPlayers.Count; aiId++)
            {
                int index = playerCount + aiId + 1;
                LimitedComboBox lcb = getPlayerNameCMBFromId(index);
                lcb.Text = AIPlayers[aiId].Name;
                lcb.Enabled = true;
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
                        playerNamesOnPlayerLocations[startBox.SelectedIndex - 1].Add(AIPlayers[aiId].Name);
                    else
                        playerNamesOnPlayerLocations[startBox.SelectedIndex - 1].Add(TeamIdentifiers[AIPlayers[aiId].TeamId - 1] + AIPlayers[aiId].Name);

                    playerColorsOnPlayerLocations[startBox.SelectedIndex - 1].Add(AIPlayers[aiId].ColorId);
                }
                LimitedComboBox teamBox = getPlayerTeamCMBFromId(index);
                teamBox.SelectedIndex = AIPlayers[aiId].TeamId;
                teamBox.Enabled = true;

                lcb.CanDropDown = true;
                sideBox.CanDropDown = true;
                colorBox.CanDropDown = true;
                startBox.CanDropDown = true;
                teamBox.CanDropDown = true;
            }

            // Unused player slots
            for (int cmbId = Players.Count + AIPlayers.Count + 1; cmbId < 9; cmbId++)
            {
                LimitedComboBox lcb = getPlayerNameCMBFromId(cmbId);
                if (cmbId == Players.Count + AIPlayers.Count + 1)
                {
                    lcb.CanDropDown = true;
                    lcb.Enabled = true;
                }
                else
                {
                    lcb.Enabled = false;
                }

                lcb.SelectedIndex = -1;
                lcb.Text = String.Empty;

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
            }

            updatePlayers = true;

            // Re-draw the preview
            pbPreview.Refresh();
        }

        private void NGameLobby_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// Checks if the current game options are valid and if they are,
        /// saves settings and starts the game.
        /// </summary>
        private void btnLaunchGame_Click(object sender, EventArgs e)
        {
            if (currentMap == null)
            {
                MessageBox.Show("Unable to start the game: the selected map is invalid!", "Invalid map", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Prevent spectating in co-op missions
            if (currentMap.IsCoop)
            {
                foreach (PlayerInfo player in Players)
                {
                    if (player.SideId == SideComboboxPrerequisites.Count + 1)
                    {
                        MessageBox.Show("Co-op missions cannot be spectated. You'll have to show a bit more effort to cheat here.",
                            "Spectating is not allowed on this map", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
            }

            // Prevent multiple players from sharing the same starting location
            if (currentMap.EnforceMaxPlayers)
            {
                int sLocIndex = Players[0].StartingLocation;
                if (sLocIndex > 0)
                {
                    int index = AIPlayers.FindIndex(p => p.StartingLocation == sLocIndex);

                    if (index > -1)
                    {
                        MessageBox.Show("Multiple players cannot share the same starting location on this map.", "Cannot start game",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                        MessageBox.Show("Multiple players cannot share the same starting location on this map.", "Cannot start game",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
            }

            if (Players[0].StartingLocation > currentMap.AmountOfPlayers)
            {
                MessageBox.Show("Unable to launch game: you have selected an invalid starting location.",
                    "Invalid starting location", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            for (int aiId = 0; aiId < AIPlayers.Count; aiId++)
            {
                if (AIPlayers[aiId].StartingLocation > currentMap.AmountOfPlayers)
                {
                    MessageBox.Show("Unable to launch game: AI player " + aiId + " has an invalid starting location.",
                        "Invalid AI starting location", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            if (currentMap.EnforceMaxPlayers)
            {
                if (Players.Count + AIPlayers.Count > currentMap.AmountOfPlayers)
                {
                    MessageBox.Show("Unable to launch game: this map cannot be played with more than " + currentMap.AmountOfPlayers + " players.",
                        "Too many players", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            if (currentMap.MinPlayers > Players.Count + AIPlayers.Count)
            {
                MessageBox.Show("Unable to launch game: this map cannot be played with fewer than " + currentMap.MinPlayers + " players.",
                    "Too few players", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            SaveSettings();

            Logger.Log("Skirmish -- starting!");

            StartGame();
        }

        /// <summary>
        /// Starts the game, writing spawn.ini, spawnmap.ini and then launching the main game executable.
        /// </summary>
        private void StartGame()
        {
            string mapPath = ProgramConstants.gamepath + currentMap.Path;
            string gameMode = cmbCurrGameMode.SelectedItem.ToString();
            if (!File.Exists(mapPath))
            {
                MessageBox.Show("Unable to locate scenario map!" + Environment.NewLine + mapPath,
                    "Cannot read scenario", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnLeaveGame.PerformClick();
            }

            string mapCodePath = currentMap.Path.Substring(0, currentMap.Path.Length - 3) + "ini";

            List<int> playerSides = new List<int>();
            List<bool> isPlayerSpectator = new List<bool>();
            List<int> playerColors = new List<int>();
            List<int> playerStartingLocations = new List<int>();

            int seed = new Random().Next(10000, 100000);

            SharedUILogic.Randomize(Players, AIPlayers, currentMap, seed, playerSides,
                isPlayerSpectator, playerColors, playerStartingLocations,
                SharedUILogic.getAllowedSides(ComboBoxes, SideComboboxPrerequisites),
                SideComboboxPrerequisites.Count);

            List<int> MultiCmbIndexes;

            SharedUILogic.WriteSpawnIni(Players, AIPlayers, currentMap, gameMode, seed,
                iNumLoadingScreens, true, new List<int>(), String.Empty, 0, ComboBoxes,
                CheckBoxes, IsCheckBoxReversed, AssociatedCheckBoxSpawnIniOptions,
                AssociatedComboBoxSpawnIniOptions, ComboBoxDataWriteModes,
                playerSides, isPlayerSpectator, playerColors, playerStartingLocations,
                out MultiCmbIndexes);

            SharedLogic.WriteCoopDataToSpawnIni(currentMap, Players, AIPlayers, MultiCmbIndexes,
                coopDifficultyLevel, SideComboboxPrerequisites.Count, mapCodePath, seed);

            List<bool> isCheckBoxChecked = new List<bool>();

            foreach (UserCheckBox chkBox in CheckBoxes)
                isCheckBoxChecked.Add(chkBox.Checked);

            File.Copy(mapPath, ProgramConstants.gamepath + ProgramConstants.SPAWNMAP_INI);

            IniFile mapIni = new IniFile(ProgramConstants.gamepath + ProgramConstants.SPAWNMAP_INI);

            SharedLogic.WriteMap(gameMode, isCheckBoxChecked, IsCheckBoxReversed, AssociatedCheckBoxCustomInis, mapIni);

            if (DomainController.Instance().getWindowMinimizingStatus())
            {
                this.Enabled = false;
                this.WindowState = FormWindowState.Minimized;
            }

            Logger.Log("About to launch main executable.");

            ms = new MatchStatistics(ProgramConstants.GAME_VERSION, currentMap.Name, gameMode, 1);
            ms.AddPlayer(Players[0].Name, true, false, Players[0].SideId == SideComboboxPrerequisites.Count + 1,
                playerSides[0], Players[0].TeamId, 10);

            int id = 1;

            foreach (PlayerInfo ai in AIPlayers)
            {
                ms.AddPlayer("Computer", false, true, false, playerSides[id], ai.TeamId, GetAILevel(ai));
                id++;
            }

            SharedUILogic.StartGameProcess(0);
        }

        private int GetAILevel(PlayerInfo aiInfo)
        {
            if (aiInfo.Name == "Hard AI")
                return 2;
            if (aiInfo.Name == "Medium AI")
                return 1;

            return 0;
        }

        private void GameProcessExited()
        {
            ms.ParseStatistics(ProgramConstants.gamepath, DomainController.Instance().getDefaultGame());
            StatisticsManager.Instance.AddMatchAndSaveDatabase(ms);

            Logger.Log("The game process has exited; displaying game lobby.");

            DomainController.Instance().ReloadSettings();

            this.Enabled = true;
            this.WindowState = FormWindowState.Normal;
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

        /// <summary>
        /// Refreshes the map list when the current game mode is changed.
        /// </summary>
        private void cmbCurrGameMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListMaps();
            int displayedItems = lbMapList.DisplayRectangle.Height / lbMapList.ItemHeight;
            customScrollbar1.Maximum = lbMapList.Items.Count - Convert.ToInt32(displayedItems * 0.2);
            customScrollbar1.Value = 0;
        }

        private void cmbP1Name_TextChanged(object sender, EventArgs e)
        {
            ProgramConstants.CNCNET_PLAYERNAME = cmbP1Name.Text;
            Players[0].Name = cmbP1Name.Text;
        }

        /// <summary>
        /// Saves Skirmish session settings.
        /// </summary>
        private void SaveSettings()
        {
            Logger.Log("Saving Skirmish settings.");

            string mapmd5 = currentMap.SHA1;

            string difficulties = String.Empty;
            for (int aiId = 0; aiId < AIPlayers.Count; aiId++)
            {
                int difficulty = getPlayerNameCMBFromId(aiId + 2).SelectedIndex - 1;
                difficulties = difficulties + difficulty;
            }

            string sides = Convert.ToString(Players[0].SideId);
            for (int aiId = 0; aiId < AIPlayers.Count; aiId++)
            {
                sides = sides + getPlayerSideCMBFromId(aiId + 2).SelectedIndex;
            }

            string colors = Convert.ToString(Players[0].ColorId);
            for (int aiId = 0; aiId < AIPlayers.Count; aiId++)
            {
                colors = colors + getPlayerColorCMBFromId(aiId + 2).SelectedIndex;
            }

            string startLocs = Convert.ToString(Players[0].StartingLocation);
            for (int aiId = 0; aiId < AIPlayers.Count; aiId++)
            {
                startLocs = startLocs + getPlayerStartCMBFromId(aiId + 2).SelectedIndex;
            }

            string teams = Convert.ToString(Players[0].TeamId);
            for (int aiId = 0; aiId < AIPlayers.Count; aiId++)
            {
                teams = teams + getPlayerTeamCMBFromId(aiId + 2).SelectedIndex;
            }

            string settings = String.Empty;
            foreach (UserCheckBox chkBox in CheckBoxes)
            {
                settings = settings + Convert.ToInt32(chkBox.Checked) + ",";
            }
            foreach (LimitedComboBox cmbBox in ComboBoxes)
            {
                settings = settings + cmbBox.SelectedIndex + ",";
            }
            settings = settings.Remove(settings.Length - 1);

            DomainController.Instance().saveSkirmishSettings(mapmd5, cmbCurrGameMode.SelectedItem.ToString(), difficulties, sides, colors, startLocs, teams, settings);
        }

        /// <summary>
        /// Loads saved Skirmish session settings.
        /// </summary>
        private void LoadSettings()
        {
            try
            {
                string mapmd5 = DomainController.Instance().getSkirmishMapMD5();
                string gameMode = DomainController.Instance().getSkirmishGameMode();

                int gameModeIndex = getGMIndex(gameMode);
                if (gameModeIndex > -1)
                {
                    cmbCurrGameMode.SelectedIndex = gameModeIndex;
                    ListMaps();
                    Map map = getMapFromMD5(mapmd5);
                    if (map != null)
                    {
                        if (map.GameModes.Contains(gameMode))
                        {
                            int index = getMapIndexInList(map.Name);

                            if (index > -1)
                            {
                                lbMapList.SelectedIndex = index;
                                //lbMapList_SelectedIndexChanged(null, EventArgs.Empty);
                            }
                        }
                    }
                }

                string difficulties = DomainController.Instance().getSkirmishDifficulties();
                for (int aiId = 0; aiId < difficulties.Length; aiId++)
                {
                    int difficulty = Convert.ToInt32(difficulties.Substring(aiId, 1));
                    if (difficulty == 0)
                        AIPlayers.Add(new PlayerInfo("Easy AI", 0, 0, 0, 0));
                    else if (difficulty == 1)
                        AIPlayers.Add(new PlayerInfo("Medium AI", 0, 0, 0, 0));
                    else
                        AIPlayers.Add(new PlayerInfo("Hard AI", 0, 0, 0, 0));
                }

                if (String.IsNullOrEmpty(difficulties))
                    throw new Exception();

                string sides = DomainController.Instance().getSkirmishSides();

                Players[0].SideId = Convert.ToInt32(sides.Substring(0, 1));

                for (int aiId = 0; aiId < AIPlayers.Count; aiId++)
                {
                    AIPlayers[aiId].SideId = Convert.ToInt32(sides.Substring(aiId + 1, 1));
                }

                string colors = DomainController.Instance().getSkirmishColors();

                Players[0].ColorId = Convert.ToInt32(colors.Substring(0, 1));

                for (int aiId = 0; aiId < AIPlayers.Count; aiId++)
                {
                    AIPlayers[aiId].ColorId = Convert.ToInt32(colors.Substring(aiId + 1, 1));
                }

                string startLocs = DomainController.Instance().getSkirmishStartingLocations();

                Players[0].StartingLocation = Convert.ToInt32(startLocs.Substring(0, 1));

                for (int aiId = 0; aiId < AIPlayers.Count; aiId++)
                {
                    AIPlayers[aiId].ColorId = Convert.ToInt32(startLocs.Substring(aiId + 1, 1));
                }

                string teams = DomainController.Instance().getSkirmishTeams();

                Players[0].TeamId = Convert.ToInt32(teams.Substring(0, 1));

                for (int aiId = 0; aiId < AIPlayers.Count; aiId++)
                {
                    AIPlayers[aiId].TeamId = Convert.ToInt32(teams.Substring(aiId + 1, 1));
                }

                string[] settings = DomainController.Instance().getSkirmishSettings().Split(',');
                if (settings.Length == CheckBoxes.Count + ComboBoxes.Count)
                {
                    for (int chkId = 0; chkId < CheckBoxes.Count; chkId++)
                    {
                        CheckBoxes[chkId].Checked = Convert.ToBoolean(Convert.ToInt32(settings[chkId]));
                    }

                    updatePlayers = false;
                    for (int cmbId = 0; cmbId < ComboBoxes.Count; cmbId++)
                    {
                        ComboBoxes[cmbId].SelectedIndex = Convert.ToInt32(settings[CheckBoxes.Count + cmbId]);
                    }
                    updatePlayers = true;
                }

                CopyPlayerDataToUI();
            }
            catch
            {
                Logger.Log("Loading skirmish settings failed.");
                Players[0].ColorId = 0;
                Players[0].TeamId = 0;
                Players[0].SideId = 0;
                Players[0].StartingLocation = 0;
                AIPlayers.Clear();
                AIPlayers.Add(new PlayerInfo("Medium AI", 0, 0, 0, 0));
                CopyPlayerDataToUI();
                LoadDefaultMap();
            }
        }

        private int getGMIndex(string gameMode)
        {
            for (int gmId = 0; gmId < cmbCurrGameMode.Items.Count; gmId++)
            {
                if (gameMode == cmbCurrGameMode.Items[gmId].ToString())
                    return gmId;
            }

            return -1;
        }

        private int getMapIndexInList(string mapName)
        {
            for (int mapId = 0; mapId < lbMapList.Items.Count; mapId++)
            {
                if (mapName == lbMapList.Items[mapId].ToString())
                    return mapId;
            }

            return -1;
        }

        /// <summary>
        /// Parses and applies forced game options. Executed when the map is changed.
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

            IniFile lockedOptionsIni = new IniFile(ProgramConstants.gamepath + "INI\\" + cmbCurrGameMode.SelectedItem.ToString() + "_ForcedOptions.ini");
            SharedUILogic.ParseLockedOptionsFromIni(CheckBoxes, ComboBoxes, lockedOptionsIni);

            string mapPath = ProgramConstants.gamepath + currentMap.Path;
            string mapCodePath = mapPath.Substring(0, mapPath.Length - 3) + "ini";

            lockedOptionsIni = new IniFile(mapCodePath);
            SharedUILogic.ParseLockedOptionsFromIni(CheckBoxes, ComboBoxes, lockedOptionsIni);

            if (currentMap.IsCoop)
            {
                coopDifficultyLevel = lockedOptionsIni.GetIntValue("ForcedOptions", "CoopDifficultyLevel", 2);

                // If the coop mission forces players to have a specific side, let's show it in the UI
                int forcedPlayerSide = lockedOptionsIni.GetIntValue("CoopInfo", "ForcedPlayerSide", -1);
                if (forcedPlayerSide > -1)
                {
                    cmbP1Side.SelectedIndex = 0;

                    foreach (TextBox pSideLabel in pSideLabels)
                    {
                        pSideLabel.Text = cmbP1Side.Items[forcedPlayerSide + 1].ToString();
                        pSideLabel.Visible = true;
                    }

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

        private void pbPreview_SizeChanged(object sender, EventArgs e)
        {
            if (resizeTimer == null)
                return;

            resizeTimer.Stop();
            resizeTimer.Start();
        }

        /// <summary>
        /// Used for painting starting locations to the map preview box.
        /// http://stackoverflow.com/questions/18210030/get-pixelvalue-when-click-on-a-picturebox
        /// </summary>
        private void pbPreview_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rectangle = (Rectangle)imageRectangleProperty.GetValue(pbPreview, null);

            SharedUILogic.PaintPreview(currentMap, rectangle, e, coopBriefingFont,
                playerNameOnPlayerLocationFont, coopBriefingForeColor, displayCoopBriefing,
                previewRatioY, previewRatioX, playerNamesOnPlayerLocations, MPColors,
                playerColorsOnPlayerLocations, startingLocationIndicators, enemyStartingLocationIndicator);
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
        /// Prevent the player from changing the name of their forced side
        /// </summary>
        private void forcedSideBox_GotFocus(object sender, EventArgs e)
        {
            pNameTextBox.Focus();
        }

        private void lbMapList_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index > -1 && e.Index < lbMapList.Items.Count)
            {
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

                Color foreColor = lbMapList.ForeColor;
                e.Graphics.DrawString(lbMapList.Items[e.Index].ToString(), e.Font, new SolidBrush(foreColor), e.Bounds);
            }
        }
    }
}

/// @author Rami "Rampastring" Pasanen
/// @version 30. 12. 2014
/// http://www.moddb.com/members/rampastring

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Media;
using ClientCore;
using ClientCore.CnCNet5;

namespace ClientGUI
{
    /// <summary>
    /// The external map selection screen for the CnCNet Game Lobby.
    /// </summary>
    public partial class MapSelectionForm : Form
    {
        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        public MapSelectionForm()
        {
            InitializeComponent();
        }

        PropertyInfo imageRectangleProperty = typeof(PictureBox).GetProperty("ImageRectangle", BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance);

        Image[] startingLocationIndicators;
        Image enemyStartingLocationIndicator;

        double previewRatioX = 1.0;
        double previewRatioY = 1.0;

        Image btn121px;
        Image btn121px_c;

        SoundPlayer buttonSound;

        Image missingPreviewImage;
        List<int> MapIndexesInList = new List<int>();

        public int rtnMapIndex = -1;
        public string rtnGameMode = "none";

        Font coopBriefingFont;

        bool displayCoopBriefing = true;
        Color coopBriefingForeColor;

        Color cListBoxFocusColor;

        /// <summary>
        /// Initializes the map selection screen.
        /// </summary>
        private void MapSelectionForm_Load(object sender, EventArgs e)
        {
            Image mapSelBg = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "mapselbg.png");
            this.BackgroundImage = mapSelBg;

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

            btn121px = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "121pxbtn.png");
            btn121px_c = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "121pxbtn_c.png");

            buttonSound = new SoundPlayer(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "button.wav");

            btnAccept.BackgroundImage = btn121px;
            btnCancel.BackgroundImage = btn121px;

            foreach (string gameMode in CnCNetData.GameTypes)
                cmbGameMode.Items.Add(gameMode);

            coopBriefingFont = new System.Drawing.Font("Segoe UI", 11.25f, FontStyle.Regular);

            Color cLabelColor = SharedUILogic.GetColorFromString(DomainController.Instance().getUILabelColor());

            Color cAltUiColor = SharedUILogic.GetColorFromString(DomainController.Instance().getUIAltColor());

            Color cBackColor = SharedUILogic.GetColorFromString(DomainController.Instance().getUIAltBackgroundColor());

            coopBriefingForeColor = cAltUiColor;
            pbMapPreview.BackColor = cBackColor;

            cListBoxFocusColor = SharedUILogic.GetColorFromString(DomainController.Instance().getListBoxFocusColor());

            SharedUILogic.SetControlColor(cLabelColor, cBackColor, cAltUiColor, cListBoxFocusColor, this);

            missingPreviewImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "nopreview.png");

            cmbGameMode.SelectedIndex = 0;
            ListMaps();
            lbMapList.SelectedIndex = 0;

            string[] windowSize = DomainController.Instance().getWindowSizeMapSelection().Split('x');

            int width = Convert.ToInt32(windowSize[0]);
            int height = Convert.ToInt32(windowSize[1]);

            if (width > Screen.PrimaryScreen.Bounds.Width - 16)
                width = Screen.PrimaryScreen.Bounds.Width - 16;
            if (height > Screen.PrimaryScreen.Bounds.Height - 40)
                height = Screen.PrimaryScreen.Bounds.Height - 40;

            int differenceX = this.ClientSize.Width - width;
            int differenceY = this.ClientSize.Height - height;

            this.ClientSize = new Size(width, height);

            this.Location = new Point(this.Location.X + (differenceX / 2), this.Location.Y + (differenceY / 2));

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
            customScrollbar1.LargeChange = 27;
            customScrollbar1.SmallChange = 9;
            customScrollbar1.Value = 0;

            lbMapList.MouseWheel += lbMapList_MouseWheel;

            SharedUILogic.ParseClientThemeIni(this);
        }


        /// <summary>
        /// Handles scaling of the form's controls to any window size. Handled manually here because
        /// of some odd bug in Visual Studio where the designer enlarges the form every time it's opened.
        /// </summary>
        private void MapSelectionForm_SizeChanged(object sender, EventArgs e)
        {
            pbMapPreview.Size = new Size(this.Size.Width - 242, this.Size.Height - 80);
            lbMapList.Size = new Size(lbMapList.Size.Width, this.Height - 109);
            customScrollbar1.Size = new System.Drawing.Size(customScrollbar1.Size.Width, lbMapList.Height);
            btnAccept.Location = new Point(btnAccept.Location.X, this.ClientSize.Height - 24);
            btnCancel.Location = new Point(btnCancel.Location.X, this.ClientSize.Height - 24);
            lblMapAuthor.Location = new Point(lblMapAuthor.Location.X, this.ClientSize.Height - 23);
        }


        /// <summary>
        /// Updates the map list.
        /// </summary>
        private void ListMaps()
        {
            lbMapList.Items.Clear();
            MapIndexesInList.Clear();
            if (cmbGameMode.SelectedIndex == -1)
            {
                return;
            }

            string gameMode = cmbGameMode.SelectedItem.ToString();

            List<Map> MapList = CnCNetData.MapList;
            for (int mId = 0; mId < MapList.Count; mId++)
            {
                if (MapList[mId].GameModes.Contains(gameMode))
                {
                    lbMapList.Items.Add(MapList[mId].Name);
                    MapIndexesInList.Add(mId);
                }
            }
        }


        /// <summary>
        /// Loads the preview when a map has been selected.
        /// </summary>
        private void lbMapList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbMapList.SelectedIndex == -1)
                return;

            Map map = CnCNetData.MapList[MapIndexesInList[lbMapList.SelectedIndex]];
            lblMapAuthor.Text = "By " + map.Author;

            PictureBoxSizeMode sizeMode;
            bool success;

            Image previewImg = SharedLogic.LoadPreview(map, out sizeMode, out success);

            pbMapPreview.SizeMode = sizeMode;

            if (!success)
            {
                pbMapPreview.Image = previewImg;
                return;
            }

            int x = previewImg.Width;
            int y = previewImg.Height;

            double prRatioX = pbMapPreview.Size.Width / Convert.ToDouble(x);
            double prRatioY = pbMapPreview.Size.Height / Convert.ToDouble(y);

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

            pbMapPreview.Image = previewImg;
        }


        /// <summary>
        /// Executed when the Cancel button is clicked.
        /// Closes the window with Cancel as the DialogResult.
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        /// <summary>
        /// Called when the OK button is clicked.
        /// If a valid map is selected, closes the window with OK as the dialog result
        /// and sets the variables rtnMapIndex and rtnGameMode, which the main lobby
        /// uses for determining which map was picked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (lbMapList.SelectedIndex == -1)
            {
                MessageBox.Show("No map selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            rtnMapIndex = MapIndexesInList[lbMapList.SelectedIndex];
            rtnGameMode = cmbGameMode.SelectedItem.ToString();

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }


        /// <summary>
        /// Refreshes the map list when the game mode is changed.
        /// </summary>
        private void cmbGameMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListMaps();
            ScrollListbox();
        }

        /// <summary>
        /// Used for painting starting locations to the map preview box.
        /// http://stackoverflow.com/questions/18210030/get-pixelvalue-when-click-on-a-picturebox
        /// </summary>
        private void pbMapPreview_Paint(object sender, PaintEventArgs e)
        {
            if (lbMapList.SelectedIndex == -1)
                return;

            Map map = CnCNetData.MapList[MapIndexesInList[lbMapList.SelectedIndex]];

            if (map != null)
            {
                Rectangle rectangle = (Rectangle)imageRectangleProperty.GetValue(pbMapPreview, null);

                SharedUILogic.PaintPreview(map, rectangle, e, coopBriefingFont,
                    null, coopBriefingForeColor, displayCoopBriefing, previewRatioY, previewRatioX,
                    null, null, null, startingLocationIndicators, enemyStartingLocationIndicator);
            }
        }

        /// <summary>
        /// When the map preview box's size has changed, reloads the map preview.
        /// </summary>
        private void pbMapPreview_SizeChanged(object sender, EventArgs e)
        {
            lbMapList_SelectedIndexChanged(null, EventArgs.Empty);
        }

        private void pbMapPreview_MouseEnter(object sender, EventArgs e)
        {
            displayCoopBriefing = false;
            pbMapPreview.Refresh();
        }

        private void pbMapPreview_MouseLeave(object sender, EventArgs e)
        {
            displayCoopBriefing = true;
            pbMapPreview.Refresh();
        }

        /// <summary>
        /// Manual code for drawing items into a combo box.
        /// </summary>
        private void cmbGameMode_DrawItem(object sender, DrawItemEventArgs e)
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

        void lbMapList_MouseWheel(object sender, MouseEventArgs e)
        {
            customScrollbar1.Value += e.Delta / -40;
            customScrollbar1_Scroll(sender, EventArgs.Empty);
        }

        private void customScrollbar1_Scroll(object sender, EventArgs e)
        {
            lbMapList.TopIndex = customScrollbar1.Value;
        }

        private void ScrollListbox()
        {
            int displayedItems = lbMapList.DisplayRectangle.Height / lbMapList.ItemHeight;
            customScrollbar1.Maximum = lbMapList.Items.Count - Convert.ToInt32(displayedItems * 0.2);
            if (customScrollbar1.Maximum < 0)
                customScrollbar1.Maximum = 1;
            customScrollbar1.Value = customScrollbar1.Minimum;
            lbMapList.SelectedIndex = 0;
            lbMapList.SelectedIndex = -1;
        }

        private void btnAccept_MouseEnter(object sender, EventArgs e)
        {
            btnAccept.BackgroundImage = btn121px_c;
            buttonSound.Play();
        }

        private void btnAccept_MouseLeave(object sender, EventArgs e)
        {
            btnAccept.BackgroundImage = btn121px;
        }

        private void btnCancel_MouseEnter(object sender, EventArgs e)
        {
            btnCancel.BackgroundImage = btn121px_c;
            buttonSound.Play();
        }

        private void btnCancel_MouseLeave(object sender, EventArgs e)
        {
            btnCancel.BackgroundImage = btn121px;
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

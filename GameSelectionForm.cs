using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using ClientCore;
using ClientCore.CnCNet5;
using ClientCore.CnCNet5.Games;

namespace ClientGUI
{
    /// <summary>
    /// A form where you can select which CnCNet supported games to follow.
    /// </summary>
    public partial class GameSelectionForm : Form
    {
        private Image[] GameImages;

        private Color cLabelColor;
        private Color cAltUIColor;
        private Color cBackColor;

        private List<int> SupportedGameIndexes = new List<int>();

        private SoundPlayer sp;

        private Image btn92px;
        private Image btn92px_c;

        public GameSelectionForm()
        {
            InitializeComponent();
        }

        private void GameSelectionForm_Load(object sender, EventArgs e)
        {
            cLabelColor = SharedUILogic.GetColorFromString(DomainController.Instance().getUILabelColor());
            cAltUIColor = SharedUILogic.GetColorFromString(DomainController.Instance().getUIAltColor());
            cBackColor = SharedUILogic.GetColorFromString(DomainController.Instance().getUIAltBackgroundColor());

            this.BackgroundImage = Image.FromFile(ProgramConstants.RESOURCES_DIR + "followedgamesbg.png");

            sp = new SoundPlayer(ProgramConstants.RESOURCES_DIR + "button.wav");
            btnOK.BackColor = cBackColor;
            btnOK.ForeColor = cAltUIColor;

            btn92px = Image.FromFile(ProgramConstants.RESOURCES_DIR + "92pxbtn.png");
            btn92px_c = Image.FromFile(ProgramConstants.RESOURCES_DIR + "92pxbtn_c.png");
            btnOK.BackgroundImage = btn92px;

            lblDescription.ForeColor = cLabelColor;

            string myGame = DomainController.Instance().getDefaultGame();

            // Generate selection boxes for games
            GameCollection gc = GameCollection.Instance;

            GameImages = gc.GetGameImages();

            int gameCount = gc.GetGameCount();

            int yPoint = lblDescription.Location.Y + lblDescription.Height + 10;
            int xPoint = lblDescription.Location.X;

            for (int i = 0; i < gameCount; i++)
            {
                if (!gc.IsGameSupported(i))
                    continue;

                if (String.IsNullOrEmpty(gc.GetGameBroadcastingChannelNameFromIndex(i)))
                    continue;

                string gameName = gc.GetFullGameNameFromIndex(i);

                PictureBox pb = new PictureBox();
                pb.Name = "pb" + gameName;
                pb.Location = new Point(xPoint, yPoint);
                pb.Size = GameImages[i].Size;
                pb.Image = GameImages[i];
                pb.BackColor = Color.Transparent;
                this.Controls.Add(pb);

                UserCheckBox checkBox = new UserCheckBox(cLabelColor, cAltUIColor, gameName);
                checkBox.LabelText = gameName;
                checkBox.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                checkBox.AutoSize = true;
                checkBox.Location = new Point(xPoint + pb.Size.Width + 3, yPoint);
                checkBox.Name = "chk" + i;
                checkBox.Tag = i;
                checkBox.label1.BackColor = Color.Transparent;
                checkBox.Checked = gc.IsGameFollowed(i);
                checkBox.CheckedChanged += checkBox_CheckedChanged;
                this.Controls.Add(checkBox);

                checkBox.Initialize();

                if (gc.GetGameIdentifierFromIndex(i).Equals(myGame.ToLower()))
                {
                    checkBox.Enabled = false; // You cannot unfollow the game that you're playing
                }

                yPoint += pb.Size.Height + 3;
            }

            this.Size = new Size(this.Width, yPoint + GameImages[0].Size.Height + btnOK.Height + 16);
        }

        /// <summary>
        /// Called whenever the user (un)follows a game by clicking on a checkbox in the form.
        /// </summary>
        /// <param name="sender">The UserCheckBox that was ticked / unticked.</param>
        private void checkBox_CheckedChanged(object sender)
        {
            UserCheckBox chkBox = (UserCheckBox)sender;
            int gameIndex = (int)chkBox.Tag;

            GameCollection gc = GameCollection.Instance;
            string gameBroadcastingChannel = gc.GetGameBroadcastingChannelNameFromIndex(gameIndex);

            if (chkBox.Checked)
            {
                gc.FollowGame(gameIndex);
                CnCNetData.ConnectionBridge.SendMessage("JOIN " + gameBroadcastingChannel);
            }
            else
            {
                gc.UnfollowGame(gameIndex);
                CnCNetData.ConnectionBridge.SendMessage("PART " + gameBroadcastingChannel);
            }
        }

        /// <summary>
        /// Closes the dialog.
        /// </summary>
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();

            this.BackgroundImage.Dispose();
            btn92px.Dispose();
            btn92px_c.Dispose();

            for (int i = 0; i < GameImages.Length; i++)
            {
                GameImages[i].Dispose();
            }
        }

        private void btnOK_MouseEnter(object sender, EventArgs e)
        {
            btnOK.BackgroundImage = btn92px_c;
            sp.Play();
        }

        private void btnOK_MouseLeave(object sender, EventArgs e)
        {
            btnOK.BackgroundImage = btn92px;
        }
    }
}

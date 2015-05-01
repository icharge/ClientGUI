using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ClientCore;
using ClientCore.CnCNet5;

namespace ClientGUI
{
    public partial class PrivateMessageForm : Form
    {
        delegate void UserQuitDelegate(string userName);
        delegate void DualStringDelegate(string message, string sender);

        List<Color> MessageColors = new List<Color>();
        Color cReceivedPMColor;
        Color cListBoxFocusColor;

        string defRecipient = String.Empty;

        public PrivateMessageForm(Color foreColor, string recipient)
        {
            InitializeComponent();
            lbChatMessages.ForeColor = foreColor;
            defRecipient = recipient;
        }

        private void PrivateMessageForm_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "pm.ico");
            this.BackgroundImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "privatemessagebg.png");

            this.Font = SharedLogic.getCommonFont();

            string[] labelColor = DomainController.Instance().getUILabelColor().Split(',');
            Color cLabelColor = Color.FromArgb(Convert.ToByte(labelColor[0]), Convert.ToByte(labelColor[1]), Convert.ToByte(labelColor[2]));
            lblSelectUser.ForeColor = cLabelColor;

            string[] altUiColor = DomainController.Instance().getUIAltColor().Split(',');
            Color cAltUiColor = Color.FromArgb(Convert.ToByte(altUiColor[0]), Convert.ToByte(altUiColor[1]), Convert.ToByte(altUiColor[2]));
            cmbPMRecipients.ForeColor = cAltUiColor;
            tbChatMessage.ForeColor = cAltUiColor;
            lbChatMessages.ForeColor = cAltUiColor;
            btnSend.ForeColor = cAltUiColor;

            string[] backgroundColor = DomainController.Instance().getUIAltBackgroundColor().Split(',');
            Color cBackColor = Color.FromArgb(Convert.ToByte(backgroundColor[0]), Convert.ToByte(backgroundColor[1]), Convert.ToByte(backgroundColor[2]));
            cmbPMRecipients.BackColor = cBackColor;
            tbChatMessage.BackColor = cBackColor;
            lbChatMessages.BackColor = cBackColor;
            btnSend.BackColor = cBackColor;

            string[] receivedColor = DomainController.Instance().getReceivedPMColor().Split(',');
            cReceivedPMColor = Color.FromArgb(Convert.ToByte(receivedColor[0]), Convert.ToByte(receivedColor[1]), Convert.ToByte(receivedColor[2]));

            string[] listBoxFocusColor = DomainController.Instance().getListBoxFocusColor().Split(',');
            cListBoxFocusColor = Color.FromArgb(Convert.ToByte(listBoxFocusColor[0]), Convert.ToByte(listBoxFocusColor[1]), Convert.ToByte(listBoxFocusColor[2]));

            foreach (PrivateMessageInfo pmInfo in CnCNetData.PMInfos)
            {
                if (!cmbPMRecipients.Items.Contains(pmInfo.UserName))
                {
                    cmbPMRecipients.Items.Add(pmInfo.UserName);
                }
            }

            if (cmbPMRecipients.Items.Count > 0)
            {
                int index = FindNameIndex(defRecipient);

                if (index == -1)
                {
                    if (String.IsNullOrEmpty(defRecipient))
                        cmbPMRecipients.SelectedIndex = 0;
                    else
                    {
                        cmbPMRecipients.Items.Add(defRecipient);
                        cmbPMRecipients.SelectedIndex = cmbPMRecipients.Items.Count - 1;
                    }
                }
                else
                    cmbPMRecipients.SelectedIndex = index;
            }
            else
            {
                cmbPMRecipients.Items.Add(defRecipient);
                cmbPMRecipients.SelectedIndex = 0;
            }

            CnCNetData.ConnectionBridge.PrivateMessageParsed += new RConnectionBridge.PrivateMessageParsedEventHandler(Instance_PrivateMessageParsed);
            CnCNetData.ConnectionBridge.PrivateMessageSent += new RConnectionBridge.PrivateMessageSentEventHandler(Instance_PrivateMessageSent);
            CnCNetData.ConnectionBridge.OnUserQuit += new RConnectionBridge.StringEventHandler(Instance_OnUserQuit);
            CnCNetData.ConnectionBridge.OnAwayMessageReceived += Instance_OnAwayMessageReceived;
            NCnCNetLobby.ConversationOpened += new NCnCNetLobby.ConversationOpenedCallback(NCnCNetLobby_ConversationOpened);
            Flash();

            SharedUILogic.ParseClientThemeIni(this);

            tbChatMessage.Focus();
        }

        private void Instance_OnAwayMessageReceived(string userName, string reason)
        {
            if (this.InvokeRequired)
            {
                DualStringDelegate d = new DualStringDelegate(Instance_OnAwayMessageReceived);
                this.BeginInvoke(d, userName, reason);
                return;
            }

            if (cmbPMRecipients.SelectedIndex == -1)
                return;

            if (FindNameIndex(userName) == cmbPMRecipients.SelectedIndex)
            {
                lbChatMessages.Items.Add(userName + " is currently away: " + reason);
                MessageColors.Add(Color.White);
                lbChatMessages.SelectedIndex = lbChatMessages.Items.Count - 1;
                lbChatMessages.SelectedIndex = -1;
            }
        }

        private void NCnCNetLobby_ConversationOpened(string userName)
        {
            int itemIndex = FindNameIndex(userName);
            if (itemIndex == -1)
            {
                cmbPMRecipients.Items.Add(userName);
                cmbPMRecipients.SelectedIndex = cmbPMRecipients.Items.Count - 1;
            }
            else
            {
                cmbPMRecipients.SelectedIndex = itemIndex;
                if (this.WindowState == FormWindowState.Minimized)
                    this.WindowState = FormWindowState.Normal;
                this.Activate();
            }
        }

        private int FindNameIndex(string name)
        {
            for (int itemId = 0; itemId < cmbPMRecipients.Items.Count; itemId++)
            {
                if (cmbPMRecipients.Items[itemId].ToString() == name)
                    return itemId;
            }

            return -1;
        }

        private void lbChatMessages_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = (int)e.Graphics.MeasureString(lbChatMessages.Items[e.Index].ToString(),
                lbChatMessages.Font, lbChatMessages.Width - 10).Height;
        }

        private void lbChatMessages_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index > -1 && e.Index < lbChatMessages.Items.Count)
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

                e.Graphics.DrawString(lbChatMessages.Items[e.Index].ToString(), e.Font, new SolidBrush(MessageColors[e.Index]), e.Bounds);
            }
        }

        private void Instance_PrivateMessageParsed(string message, string sender)
        {
            if (this.InvokeRequired)
            {
                DualStringDelegate d = new DualStringDelegate(Instance_PrivateMessageParsed);
                this.BeginInvoke(d, message, sender);
                return;
            }

            if (cmbPMRecipients.SelectedIndex == -1)
                return;

            if (sender == cmbPMRecipients.Items[cmbPMRecipients.SelectedIndex].ToString())
            {
                lbChatMessages.Items.Add("[" + DateTime.Now.ToShortTimeString() + "] " +
                    sender + ": " + message);
                MessageColors.Add(cReceivedPMColor);
                lbChatMessages.SelectedIndex = lbChatMessages.Items.Count - 1;
                lbChatMessages.SelectedIndex = -1;
                Flash();
            }
            else
            {
                if (!userExists(sender))
                    cmbPMRecipients.Items.Add(sender);
                lbChatMessages.Items.Add(sender + " has sent you a private message. Open the Player selection to switch between conversations.");
                MessageColors.Add(Color.White);
                lbChatMessages.SelectedIndex = lbChatMessages.Items.Count - 1;
                lbChatMessages.SelectedIndex = -1;
                Flash();
            }
        }

        private bool userExists(string userName)
        {
            for (int userId = 0; userId < cmbPMRecipients.Items.Count; userId++)
            {
                if (userName == cmbPMRecipients.Items[userId].ToString())
                    return true;
            }

            return false;
        }

        private void Instance_PrivateMessageSent(string message, string receiver)
        {
            if (this.InvokeRequired)
            {
                DualStringDelegate d = new DualStringDelegate(Instance_PrivateMessageSent);
                this.BeginInvoke(d, message, receiver);
                return;
            }

            if (cmbPMRecipients.SelectedIndex == -1)
                return;

            if (receiver == cmbPMRecipients.Items[cmbPMRecipients.SelectedIndex].ToString())
            {
                lbChatMessages.Items.Add("[" + DateTime.Now.ToShortTimeString() + "] " +
                    ProgramConstants.CNCNET_PLAYERNAME + ": " + message);
                MessageColors.Add(lbChatMessages.ForeColor);
            }
        }

        private void Instance_OnUserQuit(string message)
        {
            if (this.InvokeRequired)
            {
                UserQuitDelegate d = new UserQuitDelegate(Instance_OnUserQuit);
                this.BeginInvoke(d, message);
                return;
            }

            if (cmbPMRecipients.SelectedIndex == -1)
                return;

            if (message == cmbPMRecipients.Items[cmbPMRecipients.SelectedIndex].ToString())
            {
                lbChatMessages.Items.Add(message + " has quit CnCNet.");
                MessageColors.Add(Color.White);
                Flash();
            }
        }

        private void PrivateMessageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CnCNetData.ConnectionBridge.PrivateMessageParsed -= Instance_PrivateMessageParsed;
            CnCNetData.ConnectionBridge.PrivateMessageSent -= Instance_PrivateMessageSent;
            CnCNetData.ConnectionBridge.OnUserQuit -= Instance_OnUserQuit;
            NCnCNetLobby.ConversationOpened -= NCnCNetLobby_ConversationOpened;
            CnCNetData.isPMWindowOpen = false;
        }

        private void cmbPMRecipients_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbChatMessages.Items.Clear();
            MessageColors.Clear();
            string recipientName = cmbPMRecipients.Items[cmbPMRecipients.SelectedIndex].ToString();

            int index = CnCNetData.PMInfos.FindIndex(c => c.UserName == recipientName);
            if (index > -1)
            {
                PrivateMessageInfo pmInfo = CnCNetData.PMInfos[index];

                for (int msgId = 0; msgId < pmInfo.Messages.Count; msgId++)
                {
                    MessageInfo msgInfo = pmInfo.Messages[msgId];

                    if (pmInfo.IsSelfSent[msgId])
                    {
                        lbChatMessages.Items.Add("[" + msgInfo.Time.ToShortTimeString() + "] " +
                            ProgramConstants.CNCNET_PLAYERNAME + ": " + msgInfo.Message);
                        MessageColors.Add(msgInfo.Color);
                    }
                    else
                    {
                        lbChatMessages.Items.Add("[" + msgInfo.Time.ToShortTimeString() + "] " + 
                            recipientName + ": " + msgInfo.Message);
                        MessageColors.Add(msgInfo.Color);
                    }
                }
            }
        }

        private void PrivateMessageForm_SizeChanged(object sender, EventArgs e)
        {
            cmbPMRecipients.Size = new Size(this.Width - 165, cmbPMRecipients.Size.Height);
            lbChatMessages.Size = new Size(this.Width - 15, this.Height - 108);
            tbChatMessage.Size = new Size(this.Width - 106, tbChatMessage.Size.Height);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (cmbPMRecipients.SelectedIndex == -1)
            {
                lbChatMessages.Items.Add("No player selected!");
                MessageColors.Add(Color.White);
                return;
            }

            if (String.IsNullOrEmpty(tbChatMessage.Text))
            {
                lbChatMessages.Items.Add("Type your message into the field next to the Send button.");
                MessageColors.Add(Color.White);
                return;
            }

            string recipient = cmbPMRecipients.Items[cmbPMRecipients.SelectedIndex].ToString();
            CnCNetData.ConnectionBridge.SendChatMessage(recipient, -1, tbChatMessage.Text);
            lbChatMessages.Items.Add("[" + DateTime.Now.ToShortTimeString() + "] " + ProgramConstants.CNCNET_PLAYERNAME + ": " + tbChatMessage.Text);
            MessageColors.Add(lbChatMessages.ForeColor);

            lbChatMessages.SelectedIndex = lbChatMessages.Items.Count - 1;
            lbChatMessages.SelectedIndex = -1;

            int index = CnCNetData.PMInfos.FindIndex(c => c.UserName == recipient);

            if (index == -1)
            {
                PrivateMessageInfo pmInfo = new PrivateMessageInfo();
                pmInfo.UserName = recipient;
                pmInfo.Messages.Add(new MessageInfo(lbChatMessages.ForeColor, tbChatMessage.Text));
                pmInfo.IsSelfSent.Add(true);
                CnCNetData.PMInfos.Add(pmInfo);
            }
            else
            {
                CnCNetData.PMInfos[index].Messages.Add(new MessageInfo(lbChatMessages.ForeColor, tbChatMessage.Text));
                CnCNetData.PMInfos[index].IsSelfSent.Add(true);
            }

            tbChatMessage.Text = "";
        }

        private void Flash()
        {
            WindowFlasher.FlashWindowEx(this);
        }

        private void cmbPMRecipients_DrawItem(object sender, DrawItemEventArgs e)
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

        private void lbChatMessages_KeyDown(object sender, KeyEventArgs e)
        {
            if (lbChatMessages.SelectedIndex > -1)
            {
                if (e.KeyCode == Keys.C && e.Control)
                    Clipboard.SetText(lbChatMessages.SelectedItem.ToString());
            }
        }
    }
}

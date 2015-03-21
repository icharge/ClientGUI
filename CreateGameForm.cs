/// @author Rampastring
/// http://www.moddb.com/members/rampastring
/// @version 30. 12. 2014

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Media;
using ClientCore;
using ClientCore.CnCNet5;

namespace ClientGUI
{
    public partial class CreateGameForm : Form
    {
        public CreateGameForm()
        {
            InitializeComponent();
        }

        private delegate void TunnelPingedDelegate(int tunnelId, int pingInMs, int clients, int maxclients, string tunnelName);

        public string rtnGameRoomName = String.Empty;
        public string rtnPassword = String.Empty;
        public int rtnMaxPlayers = 8;
        public string rtnTunnelAddress = String.Empty;
        public int rtnTunnelPort = 0;

        Color backColor;
        Color altColor;

        List<CnCNetTunnel> Tunnels;
        bool advancedOptionsDisplayed = false;

        Image btn133px;
        Image btn133px_c;

        SoundPlayer sp;

        private void CreateGameForm_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "gamecreationoptionsbg.png");
            panel1.BackgroundImage = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "gamecreationpanelbg.png");
            this.Icon = Icon.ExtractAssociatedIcon(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "clienticon.ico");

            this.Font = SharedLogic.getCommonFont();

            sp = new SoundPlayer(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "button.wav");
            btn133px = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "133pxbtn.png");
            btn133px_c = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "133pxbtn_c.png");

            btnCancel.BackgroundImage = btn133px;
            btnOK.BackgroundImage = btn133px;

            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            listView1.MultiSelect = false;
            listView1.OwnerDraw = true;
            listView1.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(listView1_DrawColumnHeader);
            listView1.DrawItem += new DrawListViewItemEventHandler(listView1_DrawItem);
            listView1.DrawSubItem += new DrawListViewSubItemEventHandler(listView1_DrawSubItem);

            // Add column header
            listView1.Columns.Add("Name", 200);
            listView1.Columns.Add("Official", 70);
            listView1.Columns.Add("Ping", 69);
            listView1.Columns.Add("Players", 80);

            string[] labelColor = DomainController.Instance().getUILabelColor().Split(',');
            Color cLabelColor = Color.FromArgb(Convert.ToByte(labelColor[0]), Convert.ToByte(labelColor[1]), Convert.ToByte(labelColor[2]));
            lblGameName.ForeColor = cLabelColor;
            lblMaxPlayers.ForeColor = cLabelColor;
            lblPassword.ForeColor = cLabelColor;
            lblSelectTunnel.ForeColor = cLabelColor;

            string[] altUiColor = DomainController.Instance().getUIAltColor().Split(',');
            Color cAltUiColor = Color.FromArgb(Convert.ToByte(altUiColor[0]), Convert.ToByte(altUiColor[1]), Convert.ToByte(altUiColor[2]));
            cmbMaxPlayers.ForeColor = cAltUiColor;
            tbGameName.ForeColor = cAltUiColor;
            tbPassword.ForeColor = cAltUiColor;
            btnCancel.ForeColor = cAltUiColor;
            btnOK.ForeColor = cAltUiColor;
            btnDispTunnels.ForeColor = cAltUiColor;
            listView1.ForeColor = cAltUiColor;
            altColor = cAltUiColor;

            string[] backgroundColor = DomainController.Instance().getUIAltBackgroundColor().Split(',');
            Color cBackColor = Color.FromArgb(Convert.ToByte(backgroundColor[0]), Convert.ToByte(backgroundColor[1]), Convert.ToByte(backgroundColor[2]));
            cmbMaxPlayers.BackColor = cBackColor;
            tbGameName.BackColor = cBackColor;
            tbPassword.BackColor = cBackColor;
            btnCancel.BackColor = cBackColor;
            btnOK.BackColor = cBackColor;
            btnDispTunnels.BackColor = cBackColor;
            listView1.BackColor = cBackColor;
            backColor = cBackColor;

            cmbMaxPlayers.SelectedIndex = 0;
            tbGameName.Text = ProgramConstants.CNCNET_PLAYERNAME + "'s Game";

            SharedUILogic.ParseClientThemeIni(this);

            Tunnels = CnCNetTunnel.GetTunnels(true);
            bool pingCustomTunnels = DomainController.Instance().getCustomTunnelPingStatus();
            int lowestPingId = -1;
            int lowestPing = 99999;
            for (int tunnelId = 0; tunnelId < Tunnels.Count; tunnelId++)
            {
                CnCNetTunnel tunnel = Tunnels[tunnelId];
                if (tunnel.Official)
                {
                    if (tunnel.PingInMs == -1)
                    {
                        string[] array = new string[4];
                        ListViewItem itm;
                        array[0] = tunnel.Name;
                        array[1] = "Yes";
                        array[2] = "Unknown";
                        array[3] = tunnel.Clients + " / " + tunnel.MaxClients;
                        itm = new ListViewItem(array);
                        listView1.Items.Add(itm);
                    }
                    else
                    {
                        string[] array = new string[4];
                        ListViewItem itm;
                        array[0] = tunnel.Name;
                        array[1] = "Yes";
                        array[2] = tunnel.PingInMs + " ms";
                        array[3] = tunnel.Clients + " / " + tunnel.MaxClients;
                        itm = new ListViewItem(array);
                        listView1.Items.Add(itm);
                        if (tunnel.PingInMs < lowestPing)
                        {
                            lowestPing = tunnel.PingInMs;
                            lowestPingId = tunnelId;
                        }
                    }
                }
                else
                {
                    if (pingCustomTunnels)
                    {
                        string[] array = new string[4];
                        array[0] = tunnel.Name;
                        array[1] = "No";
                        array[2] = "Pinging..";
                        array[3] = tunnel.Clients + " / " + tunnel.MaxClients;
                        listView1.Items.Add(new ListViewItem(array));
                    }
                    else
                    {
                        string[] array = new string[4];
                        array[0] = tunnel.Name;
                        array[1] = "No";
                        array[2] = "-";
                        array[3] = tunnel.Clients + " / " + tunnel.MaxClients;
                        listView1.Items.Add(new ListViewItem(array));
                    }
                }
            }

            if (lowestPingId > -1)
            {
                listView1.SelectedIndices.Add(lowestPingId);
            }
            else
            {
                listView1.SelectedIndices.Add(0);
            }

            if (pingCustomTunnels)
            {
                CnCNetTunnel.TunnelPinged += new CnCNetTunnel.TunnelPingedEventHandler(CnCNetTunnel_TunnelPinged);
                ParameterizedThreadStart ts = new ParameterizedThreadStart(CnCNetTunnel.PingTunnels);
                Thread thread = new Thread(ts);
                thread.Start((object)Tunnels);
            }
        }

        private void listView1_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView1_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void CnCNetTunnel_TunnelPinged(int tunnelId, int ping, int clients, int maxclients, string tunnelName)
        {
            if (this.InvokeRequired)
            {
                TunnelPingedDelegate d = new TunnelPingedDelegate(CnCNetTunnel_TunnelPinged);
                this.BeginInvoke(d, tunnelId, ping, clients, maxclients, tunnelName);
                return;
            }

            if (ping == -1)
            {
                string[] array = new string[4];
                array[0] = tunnelName;
                array[1] = "No";
                array[2] = "Unknown";
                array[3] = clients + " / " + maxclients;
                listView1.Items[tunnelId] = new ListViewItem(array);
            }
            else
            {
                string[] array = new string[4];
                array[0] = tunnelName;
                array[1] = "No";
                array[2] = ping + " ms";
                array[3] = clients + " / " + maxclients;
                listView1.Items[tunnelId] = new ListViewItem(array);
            }
        }

        private void listView1_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            if (e.ColumnIndex > 0)
            {
                e.Graphics.FillRectangle(Brushes.Gray, new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
                e.Graphics.FillRectangle(new SolidBrush(backColor), new Rectangle(e.Bounds.X + 1, e.Bounds.Y - 1, e.Bounds.Width - 1, e.Bounds.Height));
            }
            else
            {
                e.Graphics.FillRectangle(Brushes.Gray, new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
                e.Graphics.FillRectangle(new SolidBrush(backColor), new Rectangle(e.Bounds.X, e.Bounds.Y - 1, e.Bounds.Width, e.Bounds.Height));
            }

            e.Graphics.DrawString(e.Header.Text, new Font(e.Font.FontFamily, e.Font.SizeInPoints, FontStyle.Bold), new SolidBrush(altColor), new RectangleF(e.Bounds.X, e.Bounds.Y + 3.0f, e.Bounds.Width, e.Bounds.Height));
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            if (DomainController.Instance().getCustomTunnelPingStatus())
                CnCNetTunnel.TunnelPinged -= CnCNetTunnel_TunnelPinged;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            rtnGameRoomName = tbGameName.Text;
            rtnMaxPlayers = Convert.ToInt32(cmbMaxPlayers.SelectedItem.ToString());
            rtnPassword = tbPassword.Text;
            int selectedIndex = listView1.SelectedIndices[0];
            rtnTunnelAddress = Tunnels[selectedIndex].Address;
            rtnTunnelPort = Tunnels[selectedIndex].Port;

            if (DomainController.Instance().getCustomTunnelPingStatus())
                CnCNetTunnel.TunnelPinged -= CnCNetTunnel_TunnelPinged;

            btn133px.Dispose();
            btn133px_c.Dispose();
            sp.Dispose();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnDispTunnels_Click(object sender, EventArgs e)
        {
            if (!advancedOptionsDisplayed)
            {
                btnDispTunnels.Text = "Hide advanced options";
                this.Size = new Size(490, 411);
                advancedOptionsDisplayed = true;
                listView1.Select();
            }
            else
            {
                btnDispTunnels.Text = "Display advanced options";
                this.Size = new Size(490, 224);
                advancedOptionsDisplayed = false;
            }
        }

        private void btnOK_MouseEnter(object sender, EventArgs e)
        {
            btnOK.BackgroundImage = btn133px_c;
            sp.Play();
        }

        private void btnOK_MouseLeave(object sender, EventArgs e)
        {
            btnOK.BackgroundImage = btn133px;
        }

        private void btnCancel_MouseEnter(object sender, EventArgs e)
        {
            btnCancel.BackgroundImage = btn133px_c;
            sp.Play();
        }

        private void btnCancel_MouseLeave(object sender, EventArgs e)
        {
            btnCancel.BackgroundImage = btn133px;
        }

        private void cmbMaxPlayers_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            if (e.Index > -1 && e.Index < cmbMaxPlayers.Items.Count)
            {
                if (cmbMaxPlayers.HoveredIndex != e.Index)
                {
                    e.Graphics.DrawString(cmbMaxPlayers.Items[e.Index].ToString(), e.Font,
                        new SolidBrush(cmbMaxPlayers.ForeColor), e.Bounds);
                }
                else
                {
                    e.Graphics.DrawString(cmbMaxPlayers.Items[e.Index].ToString(), e.Font,
                        new SolidBrush(Color.White), e.Bounds);
                }
            }
        }
    }
}

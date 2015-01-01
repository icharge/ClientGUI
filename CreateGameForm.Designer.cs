namespace ClientGUI
{
    partial class CreateGameForm
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
            this.lblGameName = new System.Windows.Forms.Label();
            this.lblMaxPlayers = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.tbGameName = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.cmbMaxPlayers = new ClientGUI.LimitedComboBox();
            this.btnDispTunnels = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.lblSelectTunnel = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblGameName
            // 
            this.lblGameName.AutoSize = true;
            this.lblGameName.BackColor = System.Drawing.Color.Transparent;
            this.lblGameName.Location = new System.Drawing.Point(12, 18);
            this.lblGameName.Name = "lblGameName";
            this.lblGameName.Size = new System.Drawing.Size(93, 13);
            this.lblGameName.TabIndex = 0;
            this.lblGameName.Text = "Game room name:";
            // 
            // lblMaxPlayers
            // 
            this.lblMaxPlayers.AutoSize = true;
            this.lblMaxPlayers.BackColor = System.Drawing.Color.Transparent;
            this.lblMaxPlayers.Location = new System.Drawing.Point(12, 52);
            this.lblMaxPlayers.Name = "lblMaxPlayers";
            this.lblMaxPlayers.Size = new System.Drawing.Size(140, 13);
            this.lblMaxPlayers.TabIndex = 1;
            this.lblMaxPlayers.Text = "Maximum amount of players:";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.BackColor = System.Drawing.Color.Transparent;
            this.lblPassword.Location = new System.Drawing.Point(12, 88);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(162, 13);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "Password (leave blank for none):";
            // 
            // tbGameName
            // 
            this.tbGameName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbGameName.Location = new System.Drawing.Point(326, 16);
            this.tbGameName.MaxLength = 23;
            this.tbGameName.Name = "tbGameName";
            this.tbGameName.Size = new System.Drawing.Size(136, 20);
            this.tbGameName.TabIndex = 3;
            this.tbGameName.Text = "Playername\'s Game";
            // 
            // tbPassword
            // 
            this.tbPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbPassword.Location = new System.Drawing.Point(326, 86);
            this.tbPassword.MaxLength = 15;
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(136, 20);
            this.tbPassword.TabIndex = 5;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(329, 160);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(133, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnCancel.MouseEnter += new System.EventHandler(this.btnCancel_MouseEnter);
            this.btnCancel.MouseLeave += new System.EventHandler(this.btnCancel_MouseLeave);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.FlatAppearance.BorderSize = 0;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Location = new System.Drawing.Point(12, 159);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(133, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "Create Game";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            this.btnOK.MouseEnter += new System.EventHandler(this.btnOK_MouseEnter);
            this.btnOK.MouseLeave += new System.EventHandler(this.btnOK_MouseLeave);
            // 
            // cmbMaxPlayers
            // 
            this.cmbMaxPlayers.CanDropDown = true;
            this.cmbMaxPlayers.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbMaxPlayers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMaxPlayers.FormattingEnabled = true;
            this.cmbMaxPlayers.HoveredIndex = -1;
            this.cmbMaxPlayers.Items.AddRange(new object[] {
            "8",
            "7",
            "6",
            "5",
            "4",
            "3",
            "2"});
            this.cmbMaxPlayers.Location = new System.Drawing.Point(326, 50);
            this.cmbMaxPlayers.Name = "cmbMaxPlayers";
            this.cmbMaxPlayers.Size = new System.Drawing.Size(136, 21);
            this.cmbMaxPlayers.TabIndex = 8;
            this.cmbMaxPlayers.UseCustomDrawingCode = true;
            this.cmbMaxPlayers.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbMaxPlayers_DrawItem);
            // 
            // btnDispTunnels
            // 
            this.btnDispTunnels.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDispTunnels.Location = new System.Drawing.Point(15, 120);
            this.btnDispTunnels.Name = "btnDispTunnels";
            this.btnDispTunnels.Size = new System.Drawing.Size(143, 23);
            this.btnDispTunnels.TabIndex = 9;
            this.btnDispTunnels.Text = "Display advanced options";
            this.btnDispTunnels.UseVisualStyleBackColor = true;
            this.btnDispTunnels.Click += new System.EventHandler(this.btnDispTunnels_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.listView1);
            this.panel1.Controls.Add(this.lblSelectTunnel);
            this.panel1.Location = new System.Drawing.Point(12, 192);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(450, 159);
            this.panel1.TabIndex = 10;
            // 
            // listView1
            // 
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(6, 26);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(439, 128);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // lblSelectTunnel
            // 
            this.lblSelectTunnel.AutoSize = true;
            this.lblSelectTunnel.BackColor = System.Drawing.Color.Transparent;
            this.lblSelectTunnel.Location = new System.Drawing.Point(3, 9);
            this.lblSelectTunnel.Name = "lblSelectTunnel";
            this.lblSelectTunnel.Size = new System.Drawing.Size(104, 13);
            this.lblSelectTunnel.TabIndex = 0;
            this.lblSelectTunnel.Text = "Select tunnel server:";
            // 
            // CreateGameForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(474, 185);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnDispTunnels);
            this.Controls.Add(this.cmbMaxPlayers);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.tbGameName);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblMaxPlayers);
            this.Controls.Add(this.lblGameName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateGameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Game Creation Options";
            this.Load += new System.EventHandler(this.CreateGameForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblGameName;
        private System.Windows.Forms.Label lblMaxPlayers;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox tbGameName;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private LimitedComboBox cmbMaxPlayers;
        private System.Windows.Forms.Button btnDispTunnels;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblSelectTunnel;
        private System.Windows.Forms.ListView listView1;
    }
}
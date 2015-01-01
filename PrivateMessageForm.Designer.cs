namespace ClientGUI
{
    partial class PrivateMessageForm
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
            this.cmbPMRecipients = new ClientGUI.LimitedComboBox();
            this.lblSelectUser = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.tbChatMessage = new System.Windows.Forms.TextBox();
            this.lbChatMessages = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // cmbPMRecipients
            // 
            this.cmbPMRecipients.CanDropDown = true;
            this.cmbPMRecipients.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbPMRecipients.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPMRecipients.FormattingEnabled = true;
            this.cmbPMRecipients.HoveredIndex = -1;
            this.cmbPMRecipients.Location = new System.Drawing.Point(64, 12);
            this.cmbPMRecipients.Name = "cmbPMRecipients";
            this.cmbPMRecipients.Size = new System.Drawing.Size(283, 21);
            this.cmbPMRecipients.TabIndex = 0;
            this.cmbPMRecipients.UseCustomDrawingCode = true;
            this.cmbPMRecipients.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbPMRecipients_DrawItem);
            this.cmbPMRecipients.SelectedIndexChanged += new System.EventHandler(this.cmbPMRecipients_SelectedIndexChanged);
            // 
            // lblSelectUser
            // 
            this.lblSelectUser.AutoSize = true;
            this.lblSelectUser.BackColor = System.Drawing.Color.Transparent;
            this.lblSelectUser.Location = new System.Drawing.Point(3, 15);
            this.lblSelectUser.Name = "lblSelectUser";
            this.lblSelectUser.Size = new System.Drawing.Size(39, 13);
            this.lblSelectUser.TabIndex = 1;
            this.lblSelectUser.Text = "Player:";
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSend.Location = new System.Drawing.Point(272, 293);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // tbChatMessage
            // 
            this.tbChatMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbChatMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbChatMessage.Location = new System.Drawing.Point(6, 295);
            this.tbChatMessage.Name = "tbChatMessage";
            this.tbChatMessage.Size = new System.Drawing.Size(260, 20);
            this.tbChatMessage.TabIndex = 4;
            // 
            // lbChatMessages
            // 
            this.lbChatMessages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbChatMessages.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lbChatMessages.FormattingEnabled = true;
            this.lbChatMessages.Location = new System.Drawing.Point(6, 39);
            this.lbChatMessages.Name = "lbChatMessages";
            this.lbChatMessages.Size = new System.Drawing.Size(341, 249);
            this.lbChatMessages.TabIndex = 5;
            this.lbChatMessages.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbChatMessages_DrawItem);
            this.lbChatMessages.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.lbChatMessages_MeasureItem);
            this.lbChatMessages.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbChatMessages_KeyDown);
            // 
            // PrivateMessageForm
            // 
            this.AcceptButton = this.btnSend;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(350, 319);
            this.Controls.Add(this.lbChatMessages);
            this.Controls.Add(this.tbChatMessage);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.lblSelectUser);
            this.Controls.Add(this.cmbPMRecipients);
            this.MinimumSize = new System.Drawing.Size(366, 357);
            this.Name = "PrivateMessageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Private messaging";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PrivateMessageForm_FormClosing);
            this.Load += new System.EventHandler(this.PrivateMessageForm_Load);
            this.SizeChanged += new System.EventHandler(this.PrivateMessageForm_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LimitedComboBox cmbPMRecipients;
        private System.Windows.Forms.Label lblSelectUser;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox tbChatMessage;
        private System.Windows.Forms.ListBox lbChatMessages;
    }
}
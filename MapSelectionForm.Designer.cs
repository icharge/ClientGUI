namespace ClientGUI
{
    partial class MapSelectionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapSelectionForm));
            this.lblGameMode = new System.Windows.Forms.Label();
            this.cmbGameMode = new ClientGUI.LimitedComboBox();
            this.lbMapList = new ClientGUI.ScrollbarlessListBox();
            this.btnAccept = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblMapAuthor = new System.Windows.Forms.Label();
            this.pbMapPreview = new ClientGUI.EnhancedPictureBox();
            this.customScrollbar1 = new CustomControls.CustomScrollbar();
            ((System.ComponentModel.ISupportInitialize)(this.pbMapPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // lblGameMode
            // 
            this.lblGameMode.AutoSize = true;
            this.lblGameMode.BackColor = System.Drawing.Color.Transparent;
            this.lblGameMode.Location = new System.Drawing.Point(13, 13);
            this.lblGameMode.Name = "lblGameMode";
            this.lblGameMode.Size = new System.Drawing.Size(68, 13);
            this.lblGameMode.TabIndex = 0;
            this.lblGameMode.Text = "Game Mode:";
            // 
            // cmbGameMode
            // 
            this.cmbGameMode.CanDropDown = true;
            this.cmbGameMode.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbGameMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGameMode.FormattingEnabled = true;
            this.cmbGameMode.HoveredIndex = -1;
            this.cmbGameMode.Location = new System.Drawing.Point(87, 10);
            this.cmbGameMode.MaxDropDownItems = 20;
            this.cmbGameMode.Name = "cmbGameMode";
            this.cmbGameMode.Size = new System.Drawing.Size(121, 21);
            this.cmbGameMode.TabIndex = 1;
            this.cmbGameMode.UseCustomDrawingCode = true;
            this.cmbGameMode.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbGameMode_DrawItem);
            this.cmbGameMode.SelectedIndexChanged += new System.EventHandler(this.cmbGameMode_SelectedIndexChanged);
            // 
            // lbMapList
            // 
            this.lbMapList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbMapList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lbMapList.FormattingEnabled = true;
            this.lbMapList.Location = new System.Drawing.Point(12, 37);
            this.lbMapList.Name = "lbMapList";
            this.lbMapList.ShowScrollbar = false;
            this.lbMapList.Size = new System.Drawing.Size(196, 383);
            this.lbMapList.TabIndex = 2;
            this.lbMapList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbMapList_DrawItem);
            this.lbMapList.SelectedIndexChanged += new System.EventHandler(this.lbMapList_SelectedIndexChanged);
            // 
            // btnAccept
            // 
            this.btnAccept.FlatAppearance.BorderSize = 0;
            this.btnAccept.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAccept.Location = new System.Drawing.Point(12, 424);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(121, 23);
            this.btnAccept.TabIndex = 4;
            this.btnAccept.Text = "OK";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            this.btnAccept.MouseEnter += new System.EventHandler(this.btnAccept_MouseEnter);
            this.btnAccept.MouseLeave += new System.EventHandler(this.btnAccept_MouseLeave);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(701, 424);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(121, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnCancel.MouseEnter += new System.EventHandler(this.btnCancel_MouseEnter);
            this.btnCancel.MouseLeave += new System.EventHandler(this.btnCancel_MouseLeave);
            // 
            // lblMapAuthor
            // 
            this.lblMapAuthor.AutoSize = true;
            this.lblMapAuthor.BackColor = System.Drawing.Color.Transparent;
            this.lblMapAuthor.Location = new System.Drawing.Point(214, 425);
            this.lblMapAuthor.Name = "lblMapAuthor";
            this.lblMapAuthor.Size = new System.Drawing.Size(102, 13);
            this.lblMapAuthor.TabIndex = 7;
            this.lblMapAuthor.Text = "By Unknown Author";
            // 
            // pbMapPreview
            // 
            this.pbMapPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbMapPreview.Location = new System.Drawing.Point(214, 10);
            this.pbMapPreview.Name = "pbMapPreview";
            this.pbMapPreview.Size = new System.Drawing.Size(608, 408);
            this.pbMapPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbMapPreview.TabIndex = 6;
            this.pbMapPreview.TabStop = false;
            this.pbMapPreview.SizeChanged += new System.EventHandler(this.pbMapPreview_SizeChanged);
            this.pbMapPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.pbMapPreview_Paint);
            this.pbMapPreview.MouseEnter += new System.EventHandler(this.pbMapPreview_MouseEnter);
            this.pbMapPreview.MouseLeave += new System.EventHandler(this.pbMapPreview_MouseLeave);
            // 
            // customScrollbar1
            // 
            this.customScrollbar1.ChannelColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(166)))), ((int)(((byte)(3)))));
            this.customScrollbar1.DownArrowImage = ((System.Drawing.Image)(resources.GetObject("customScrollbar1.DownArrowImage")));
            this.customScrollbar1.LargeChange = 10;
            this.customScrollbar1.Location = new System.Drawing.Point(192, 37);
            this.customScrollbar1.Maximum = 100;
            this.customScrollbar1.Minimum = 0;
            this.customScrollbar1.MinimumSize = new System.Drawing.Size(15, 92);
            this.customScrollbar1.Name = "customScrollbar1";
            this.customScrollbar1.Size = new System.Drawing.Size(16, 383);
            this.customScrollbar1.SmallChange = 1;
            this.customScrollbar1.TabIndex = 87;
            this.customScrollbar1.ThumbBottomImage = ((System.Drawing.Image)(resources.GetObject("customScrollbar1.ThumbBottomImage")));
            this.customScrollbar1.ThumbBottomSpanImage = ((System.Drawing.Image)(resources.GetObject("customScrollbar1.ThumbBottomSpanImage")));
            this.customScrollbar1.ThumbMiddleImage = ((System.Drawing.Image)(resources.GetObject("customScrollbar1.ThumbMiddleImage")));
            this.customScrollbar1.ThumbTopImage = ((System.Drawing.Image)(resources.GetObject("customScrollbar1.ThumbTopImage")));
            this.customScrollbar1.ThumbTopSpanImage = ((System.Drawing.Image)(resources.GetObject("customScrollbar1.ThumbTopSpanImage")));
            this.customScrollbar1.UpArrowImage = ((System.Drawing.Image)(resources.GetObject("customScrollbar1.UpArrowImage")));
            this.customScrollbar1.Value = 0;
            this.customScrollbar1.Scroll += new System.EventHandler(this.customScrollbar1_Scroll);
            // 
            // MapSelectionForm
            // 
            this.AcceptButton = this.btnAccept;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(834, 472);
            this.ControlBox = false;
            this.Controls.Add(this.customScrollbar1);
            this.Controls.Add(this.lblMapAuthor);
            this.Controls.Add(this.pbMapPreview);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.lbMapList);
            this.Controls.Add(this.cmbGameMode);
            this.Controls.Add(this.lblGameMode);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(850, 488);
            this.Name = "MapSelectionForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Map";
            this.Load += new System.EventHandler(this.MapSelectionForm_Load);
            this.SizeChanged += new System.EventHandler(this.MapSelectionForm_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.pbMapPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblGameMode;
        private LimitedComboBox cmbGameMode;
        private ScrollbarlessListBox lbMapList;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Button btnCancel;
        private ClientGUI.EnhancedPictureBox pbMapPreview;
        private System.Windows.Forms.Label lblMapAuthor;
        private CustomControls.CustomScrollbar customScrollbar1;
    }
}
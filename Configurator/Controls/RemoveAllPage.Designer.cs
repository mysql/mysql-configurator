namespace WexInstaller
{
    partial class RemoveAllPage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemoveAllPage));
            this.titleLabel = new System.Windows.Forms.Label();
            this.packageListLabel = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.productList = new System.Windows.Forms.ListView();
            this.productHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.progressHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.detailedLog = new System.Windows.Forms.TextBox();
            this.sideBarControl1 = new WexInstaller.SideBarControl();
            this.enableDetails = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.titleLabel.Location = new System.Drawing.Point(241, 43);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(180, 23);
            this.titleLabel.TabIndex = 1;
            this.titleLabel.Text = "Remove All Products";
            // 
            // packageListLabel
            // 
            this.packageListLabel.AutoSize = true;
            this.packageListLabel.Font = new System.Drawing.Font("Tahoma", 10F);
            this.packageListLabel.Location = new System.Drawing.Point(243, 86);
            this.packageListLabel.Name = "packageListLabel";
            this.packageListLabel.Size = new System.Drawing.Size(244, 17);
            this.packageListLabel.TabIndex = 3;
            this.packageListLabel.Text = "The following products will be removed";
            // 
            // startButton
            // 
            this.startButton.BackColor = System.Drawing.SystemColors.Control;
            this.startButton.Location = new System.Drawing.Point(494, 520);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(86, 26);
            this.startButton.TabIndex = 6;
            this.startButton.Text = "&Start";
            this.startButton.UseVisualStyleBackColor = false;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.BackColor = System.Drawing.SystemColors.Control;
            this.cancelBtn.Location = new System.Drawing.Point(586, 520);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(86, 26);
            this.cancelBtn.TabIndex = 9;
            this.cancelBtn.Text = "&Cancel";
            this.cancelBtn.UseVisualStyleBackColor = false;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // closeButton
            // 
            this.closeButton.BackColor = System.Drawing.SystemColors.Control;
            this.closeButton.Enabled = false;
            this.closeButton.Location = new System.Drawing.Point(586, 520);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(86, 26);
            this.closeButton.TabIndex = 10;
            this.closeButton.Text = "&Close";
            this.closeButton.UseVisualStyleBackColor = false;
            this.closeButton.Visible = false;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::WexInstaller.Properties.Resources.linkpanel_divider;
            this.pictureBox1.Location = new System.Drawing.Point(246, 72);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(513, 11);
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // productList
            // 
            this.productList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.productList.CheckBoxes = true;
            this.productList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.productHeader,
            this.progressHeader});
            this.productList.Location = new System.Drawing.Point(246, 121);
            this.productList.Name = "productList";
            this.productList.Size = new System.Drawing.Size(513, 176);
            this.productList.TabIndex = 12;
            this.productList.UseCompatibleStateImageBehavior = false;
            this.productList.View = System.Windows.Forms.View.Details;
            this.productList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.productList_ColumnClick);
            // 
            // productHeader
            // 
            this.productHeader.Text = "      Product";
            this.productHeader.Width = 443;
            // 
            // progressHeader
            // 
            this.progressHeader.Text = "Progress";
            this.progressHeader.Width = 69;
            // 
            // detailedLog
            // 
            this.detailedLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.detailedLog.Location = new System.Drawing.Point(246, 303);
            this.detailedLog.Multiline = true;
            this.detailedLog.Name = "detailedLog";
            this.detailedLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.detailedLog.Size = new System.Drawing.Size(513, 192);
            this.detailedLog.TabIndex = 13;
            this.detailedLog.Visible = false;
            // 
            // sideBarControl1
            // 
            this.sideBarControl1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("sideBarControl1.BackgroundImage")));
            this.sideBarControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.sideBarControl1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.sideBarControl1.Location = new System.Drawing.Point(0, 0);
            this.sideBarControl1.Name = "sideBarControl1";
            this.sideBarControl1.Size = new System.Drawing.Size(220, 562);
            this.sideBarControl1.TabIndex = 8;
            // 
            // enableDetails
            // 
            this.enableDetails.AutoSize = true;
            this.enableDetails.Location = new System.Drawing.Point(245, 303);
            this.enableDetails.Name = "enableDetails";
            this.enableDetails.Size = new System.Drawing.Size(89, 23);
            this.enableDetails.TabIndex = 14;
            this.enableDetails.Text = "&Show Details >";
            this.enableDetails.UseVisualStyleBackColor = true;
            this.enableDetails.Visible = false;
            this.enableDetails.Click += new System.EventHandler(this.enableDetails_Click);
            // 
            // RemoveAllPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.enableDetails);
            this.Controls.Add(this.detailedLog);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.packageListLabel);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.sideBarControl1);
            this.Controls.Add(this.productList);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.closeButton);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "RemoveAllPage";
            this.Size = new System.Drawing.Size(784, 562);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label packageListLabel;
        private System.Windows.Forms.Button startButton;
        private SideBarControl sideBarControl1;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ListView productList;
        private System.Windows.Forms.ColumnHeader productHeader;
        private System.Windows.Forms.ColumnHeader progressHeader;
        private System.Windows.Forms.TextBox detailedLog;
        private System.Windows.Forms.Button enableDetails;
    }
}

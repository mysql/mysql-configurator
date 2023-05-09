/* Copyright (c) 2011, 2022, Oracle and/or its affiliates.

 This program is free software; you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation; version 2 of the License.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with this program; if not, write to the Free Software
 Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301  USA */

namespace MySql.Configurator.Wizards.Server
{
  partial class ServerConfigSecurityPage
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerConfigSecurityPage));
      this.ServerFilePermissionsFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
      this.PermissionsMainDescriptionLabel = new System.Windows.Forms.Label();
      this.DataDirectoryPathLabel = new System.Windows.Forms.Label();
      this.PermissionsQuestionLabel = new System.Windows.Forms.Label();
      this.YesRadioButton = new System.Windows.Forms.RadioButton();
      this.YesReviewRadioButton = new System.Windows.Forms.RadioButton();
      this.ReviewChangesPanel = new System.Windows.Forms.Panel();
      this.AccessLevelLabel = new System.Windows.Forms.Label();
      this.NoAccessListView = new System.Windows.Forms.ListView();
      this.NoAccessPrincipalColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.NoAccessTypeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.FullControlListView = new System.Windows.Forms.ListView();
      this.FullControlPrincipalColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.FullControlTypeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.NoAccessLabel = new System.Windows.Forms.Label();
      this.FullControlLabel = new System.Windows.Forms.Label();
      this.MoveSelectedLeftButton = new System.Windows.Forms.Button();
      this.MoveSelectedRightButton = new System.Windows.Forms.Button();
      this.WarningPictureBox = new System.Windows.Forms.PictureBox();
      this.WarningAccessLevelLabel = new System.Windows.Forms.Label();
      this.NoRadioButton = new System.Windows.Forms.RadioButton();
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).BeginInit();
      this.ServerFilePermissionsFlowLayoutPanel.SuspendLayout();
      this.ReviewChangesPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.WarningPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // subCaptionLabel
      // 
      this.subCaptionLabel.Location = new System.Drawing.Point(25, 66);
      this.subCaptionLabel.Visible = false;
      // 
      // captionLabel
      // 
      this.captionLabel.Size = new System.Drawing.Size(203, 25);
      this.captionLabel.Text = "Server File Permissions";
      // 
      // ServerFilePermissionsFlowLayoutPanel
      // 
      this.ServerFilePermissionsFlowLayoutPanel.AccessibleDescription = "A panel containing the controls for accessing the data directory";
      this.ServerFilePermissionsFlowLayoutPanel.AccessibleName = "Server File Permissions";
      this.ServerFilePermissionsFlowLayoutPanel.Controls.Add(this.PermissionsMainDescriptionLabel);
      this.ServerFilePermissionsFlowLayoutPanel.Controls.Add(this.DataDirectoryPathLabel);
      this.ServerFilePermissionsFlowLayoutPanel.Controls.Add(this.PermissionsQuestionLabel);
      this.ServerFilePermissionsFlowLayoutPanel.Controls.Add(this.YesRadioButton);
      this.ServerFilePermissionsFlowLayoutPanel.Controls.Add(this.YesReviewRadioButton);
      this.ServerFilePermissionsFlowLayoutPanel.Controls.Add(this.ReviewChangesPanel);
      this.ServerFilePermissionsFlowLayoutPanel.Controls.Add(this.NoRadioButton);
      this.ServerFilePermissionsFlowLayoutPanel.Location = new System.Drawing.Point(24, 63);
      this.ServerFilePermissionsFlowLayoutPanel.Name = "ServerFilePermissionsFlowLayoutPanel";
      this.ServerFilePermissionsFlowLayoutPanel.Size = new System.Drawing.Size(507, 473);
      this.ServerFilePermissionsFlowLayoutPanel.TabIndex = 57;
      // 
      // PermissionsMainDescriptionLabel
      // 
      this.PermissionsMainDescriptionLabel.AccessibleDescription = "A label displaying a description of the configuration page";
      this.PermissionsMainDescriptionLabel.AccessibleName = "Permissions Description";
      this.PermissionsMainDescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.PermissionsMainDescriptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.PermissionsMainDescriptionLabel.Location = new System.Drawing.Point(3, 0);
      this.PermissionsMainDescriptionLabel.Name = "PermissionsMainDescriptionLabel";
      this.PermissionsMainDescriptionLabel.Size = new System.Drawing.Size(504, 40);
      this.PermissionsMainDescriptionLabel.TabIndex = 1;
      this.PermissionsMainDescriptionLabel.Text = "MySQL Configurator can secure the server\'s data directory by updating the permission" +
    "s of files and folders located at:";
      // 
      // DataDirectoryPathLabel
      // 
      this.DataDirectoryPathLabel.AccessibleDescription = "A label displaying the introductory text for the server files permissions configu" +
    "ration page";
      this.DataDirectoryPathLabel.AccessibleName = "Data Directory Path Text";
      this.DataDirectoryPathLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.DataDirectoryPathLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.DataDirectoryPathLabel.Location = new System.Drawing.Point(3, 40);
      this.DataDirectoryPathLabel.Name = "DataDirectoryPathLabel";
      this.DataDirectoryPathLabel.Size = new System.Drawing.Size(300, 25);
      this.DataDirectoryPathLabel.TabIndex = 16;
      this.DataDirectoryPathLabel.Text = "...";
      this.DataDirectoryPathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // PermissionsQuestionLabel
      // 
      this.PermissionsQuestionLabel.AccessibleDescription = "A label displaying a question to the user";
      this.PermissionsQuestionLabel.AccessibleName = "Permissions Question";
      this.PermissionsQuestionLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.PermissionsQuestionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.PermissionsQuestionLabel.Location = new System.Drawing.Point(3, 75);
      this.PermissionsQuestionLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
      this.PermissionsQuestionLabel.Name = "PermissionsQuestionLabel";
      this.PermissionsQuestionLabel.Size = new System.Drawing.Size(504, 28);
      this.PermissionsQuestionLabel.TabIndex = 47;
      this.PermissionsQuestionLabel.Text = "Do you want MySQL Configurator to update the server file permissions for you?";
      // 
      // YesRadioButton
      // 
      this.YesRadioButton.AccessibleDescription = "An option to allow Configurator to update access to the data directory";
      this.YesRadioButton.AccessibleName = "Yes";
      this.YesRadioButton.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.YesRadioButton.Checked = true;
      this.YesRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.YesRadioButton.Location = new System.Drawing.Point(8, 106);
      this.YesRadioButton.Margin = new System.Windows.Forms.Padding(8, 3, 3, 3);
      this.YesRadioButton.Name = "YesRadioButton";
      this.YesRadioButton.Size = new System.Drawing.Size(504, 35);
      this.YesRadioButton.TabIndex = 49;
      this.YesRadioButton.TabStop = true;
      this.YesRadioButton.Text = "Yes, grant full access to the user running the Windows Service (if applicable) an" +
    "d the administrators group only. Other users and groups will not have access.";
      this.YesRadioButton.UseVisualStyleBackColor = true;
      this.YesRadioButton.CheckedChanged += new System.EventHandler(this.YesRadioButton_CheckedChanged);
      // 
      // YesReviewRadioButton
      // 
      this.YesReviewRadioButton.AccessibleDescription = "An option permitting MySQL Configurator to update data directory access based on the" +
    " list of user-selected windows users and groups";
      this.YesReviewRadioButton.AccessibleName = "Yes Review";
      this.YesReviewRadioButton.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.YesReviewRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.YesReviewRadioButton.Location = new System.Drawing.Point(8, 147);
      this.YesReviewRadioButton.Margin = new System.Windows.Forms.Padding(8, 3, 3, 3);
      this.YesReviewRadioButton.Name = "YesReviewRadioButton";
      this.YesReviewRadioButton.Size = new System.Drawing.Size(504, 35);
      this.YesReviewRadioButton.TabIndex = 52;
      this.YesReviewRadioButton.Text = "Yes, but let me review and configure the level of access.";
      this.YesReviewRadioButton.UseVisualStyleBackColor = true;
      this.YesReviewRadioButton.CheckedChanged += new System.EventHandler(this.YesReviewRadioButton_CheckedChanged);
      // 
      // ReviewChangesPanel
      // 
      this.ReviewChangesPanel.AccessibleDescription = "A panel containing the controls to define which users and groups have full contro" +
    "l or no access to the data directory";
      this.ReviewChangesPanel.AccessibleName = "Review Changes";
      this.ReviewChangesPanel.Controls.Add(this.AccessLevelLabel);
      this.ReviewChangesPanel.Controls.Add(this.NoAccessListView);
      this.ReviewChangesPanel.Controls.Add(this.FullControlListView);
      this.ReviewChangesPanel.Controls.Add(this.NoAccessLabel);
      this.ReviewChangesPanel.Controls.Add(this.FullControlLabel);
      this.ReviewChangesPanel.Controls.Add(this.MoveSelectedLeftButton);
      this.ReviewChangesPanel.Controls.Add(this.MoveSelectedRightButton);
      this.ReviewChangesPanel.Controls.Add(this.WarningPictureBox);
      this.ReviewChangesPanel.Controls.Add(this.WarningAccessLevelLabel);
      this.ReviewChangesPanel.Location = new System.Drawing.Point(3, 193);
      this.ReviewChangesPanel.Name = "ReviewChangesPanel";
      this.ReviewChangesPanel.Size = new System.Drawing.Size(504, 210);
      this.ReviewChangesPanel.TabIndex = 53;
      // 
      // AccessLevelLabel
      // 
      this.AccessLevelLabel.AccessibleDescription = "A label displaying instructions to the user";
      this.AccessLevelLabel.AccessibleName = "Access Level Text";
      this.AccessLevelLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.AccessLevelLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.AccessLevelLabel.Location = new System.Drawing.Point(16, 0);
      this.AccessLevelLabel.Name = "AccessLevelLabel";
      this.AccessLevelLabel.Size = new System.Drawing.Size(504, 28);
      this.AccessLevelLabel.TabIndex = 64;
      this.AccessLevelLabel.Text = "Select the level of access for Windows users and groups:";
      // 
      // NoAccessListView
      // 
      this.NoAccessListView.AccessibleDescription = "A list view displaying the users and groups that have no access to the data direc" +
    "tory";
      this.NoAccessListView.AccessibleName = "No Access";
      this.NoAccessListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NoAccessPrincipalColumnHeader,
            this.NoAccessTypeColumnHeader});
      this.NoAccessListView.Location = new System.Drawing.Point(253, 100);
      this.NoAccessListView.MultiSelect = false;
      this.NoAccessListView.Name = "NoAccessListView";
      this.NoAccessListView.Size = new System.Drawing.Size(210, 102);
      this.NoAccessListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
      this.NoAccessListView.TabIndex = 63;
      this.NoAccessListView.UseCompatibleStateImageBehavior = false;
      this.NoAccessListView.View = System.Windows.Forms.View.Details;
      this.NoAccessListView.SelectedIndexChanged += new System.EventHandler(this.NoAccessListView_SelectedIndexChanged);
      // 
      // NoAccessPrincipalColumnHeader
      // 
      this.NoAccessPrincipalColumnHeader.Text = "Principal";
      this.NoAccessPrincipalColumnHeader.Width = 120;
      // 
      // NoAccessTypeColumnHeader
      // 
      this.NoAccessTypeColumnHeader.Text = "Type";
      // 
      // FullControlListView
      // 
      this.FullControlListView.AccessibleDescription = "A list view displaying the users and groups that have full control of the data di" +
    "rectory";
      this.FullControlListView.AccessibleName = "Full Control";
      this.FullControlListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.FullControlPrincipalColumnHeader,
            this.FullControlTypeColumnHeader});
      this.FullControlListView.Location = new System.Drawing.Point(15, 100);
      this.FullControlListView.MultiSelect = false;
      this.FullControlListView.Name = "FullControlListView";
      this.FullControlListView.Size = new System.Drawing.Size(210, 102);
      this.FullControlListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
      this.FullControlListView.TabIndex = 62;
      this.FullControlListView.UseCompatibleStateImageBehavior = false;
      this.FullControlListView.View = System.Windows.Forms.View.Details;
      this.FullControlListView.SelectedIndexChanged += new System.EventHandler(this.FullControlListView_SelectedIndexChanged);
      // 
      // FullControlPrincipalColumnHeader
      // 
      this.FullControlPrincipalColumnHeader.Text = "Principal";
      this.FullControlPrincipalColumnHeader.Width = 120;
      // 
      // FullControlTypeColumnHeader
      // 
      this.FullControlTypeColumnHeader.Text = "Type";
      // 
      // NoAccessLabel
      // 
      this.NoAccessLabel.AccessibleDescription = "A label displaying the text no-access";
      this.NoAccessLabel.AccessibleName = "No Access Text";
      this.NoAccessLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.NoAccessLabel.AutoSize = true;
      this.NoAccessLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.NoAccessLabel.Location = new System.Drawing.Point(254, 80);
      this.NoAccessLabel.Name = "NoAccessLabel";
      this.NoAccessLabel.Size = new System.Drawing.Size(100, 25);
      this.NoAccessLabel.TabIndex = 61;
      this.NoAccessLabel.Text = "No-Access:";
      // 
      // FullControlLabel
      // 
      this.FullControlLabel.AccessibleDescription = "A label displaying the text full control";
      this.FullControlLabel.AccessibleName = "Full Control Text";
      this.FullControlLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.FullControlLabel.AutoSize = true;
      this.FullControlLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FullControlLabel.Location = new System.Drawing.Point(17, 80);
      this.FullControlLabel.Name = "FullControlLabel";
      this.FullControlLabel.Size = new System.Drawing.Size(107, 25);
      this.FullControlLabel.TabIndex = 60;
      this.FullControlLabel.Text = "Full Control:";
      // 
      // MoveSelectedLeftButton
      // 
      this.MoveSelectedLeftButton.AccessibleDescription = "A button to select a user or group for having full control over the data director" +
    "y";
      this.MoveSelectedLeftButton.AccessibleName = "Remove Selected";
      this.MoveSelectedLeftButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("MoveSelectedLeftButton.BackgroundImage")));
      this.MoveSelectedLeftButton.FlatAppearance.BorderSize = 0;
      this.MoveSelectedLeftButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.MoveSelectedLeftButton.Location = new System.Drawing.Point(231, 160);
      this.MoveSelectedLeftButton.Name = "MoveSelectedLeftButton";
      this.MoveSelectedLeftButton.Size = new System.Drawing.Size(16, 16);
      this.MoveSelectedLeftButton.TabIndex = 59;
      this.MoveSelectedLeftButton.UseVisualStyleBackColor = true;
      this.MoveSelectedLeftButton.Click += new System.EventHandler(this.MoveSelectedLeftButton_Click);
      // 
      // MoveSelectedRightButton
      // 
      this.MoveSelectedRightButton.AccessibleDescription = "A button to select a user or group for having no access to the data directory";
      this.MoveSelectedRightButton.AccessibleName = "Add Selected";
      this.MoveSelectedRightButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("MoveSelectedRightButton.BackgroundImage")));
      this.MoveSelectedRightButton.FlatAppearance.BorderSize = 0;
      this.MoveSelectedRightButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.MoveSelectedRightButton.Location = new System.Drawing.Point(231, 132);
      this.MoveSelectedRightButton.Name = "MoveSelectedRightButton";
      this.MoveSelectedRightButton.Size = new System.Drawing.Size(16, 16);
      this.MoveSelectedRightButton.TabIndex = 58;
      this.MoveSelectedRightButton.UseVisualStyleBackColor = true;
      this.MoveSelectedRightButton.Click += new System.EventHandler(this.MoveSelectedRightButton_Click);
      // 
      // WarningPictureBox
      // 
      this.WarningPictureBox.AccessibleDescription = "A picture box displaying a big warning icon";
      this.WarningPictureBox.AccessibleName = "Big Warning Icon";
      this.WarningPictureBox.Image = global::MySql.Configurator.Properties.Resources.BigWarning;
      this.WarningPictureBox.Location = new System.Drawing.Point(21, 25);
      this.WarningPictureBox.Name = "WarningPictureBox";
      this.WarningPictureBox.Size = new System.Drawing.Size(38, 38);
      this.WarningPictureBox.TabIndex = 57;
      this.WarningPictureBox.TabStop = false;
      // 
      // WarningAccessLevelLabel
      // 
      this.WarningAccessLevelLabel.AccessibleDescription = "A label displaying the limitations for selecting the access level users and group" +
    "s";
      this.WarningAccessLevelLabel.AccessibleName = "Warning Access Level";
      this.WarningAccessLevelLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.WarningAccessLevelLabel.ForeColor = System.Drawing.Color.Black;
      this.WarningAccessLevelLabel.Location = new System.Drawing.Point(65, 25);
      this.WarningAccessLevelLabel.Name = "WarningAccessLevelLabel";
      this.WarningAccessLevelLabel.Size = new System.Drawing.Size(404, 55);
      this.WarningAccessLevelLabel.TabIndex = 56;
      this.WarningAccessLevelLabel.Text = "The user running the Windows Service (if applicable) and the Administrators Group" +
    " are required to have full control. All permissions related to membership in the" +
    " Users Group will be removed.";
      // 
      // NoRadioButton
      // 
      this.NoRadioButton.AccessibleDescription = "An option to for MySQL Configurator to skip making updates to permissions of the dat" +
    "a directory";
      this.NoRadioButton.AccessibleName = "No";
      this.NoRadioButton.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.NoRadioButton.AutoSize = true;
      this.NoRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.NoRadioButton.Location = new System.Drawing.Point(8, 409);
      this.NoRadioButton.Margin = new System.Windows.Forms.Padding(8, 3, 3, 3);
      this.NoRadioButton.Name = "NoRadioButton";
      this.NoRadioButton.Size = new System.Drawing.Size(581, 29);
      this.NoRadioButton.TabIndex = 54;
      this.NoRadioButton.Text = "No, I will manage the permissions after the server configuration.";
      this.NoRadioButton.UseVisualStyleBackColor = true;
      this.NoRadioButton.CheckedChanged += new System.EventHandler(this.NoRadioButton_CheckedChanged);
      // 
      // ServerConfigSecurityPage
      // 
      this.AccessibleDescription = "A configuration wizard page to allow or skip updating permissions for the data fo" +
    "lder and related server files";
      this.AccessibleName = "Security Settings Page";
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
      this.Caption = "Server File Permissions";
      this.Controls.Add(this.ServerFilePermissionsFlowLayoutPanel);
      this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "ServerConfigSecurityPage";
      this.Size = new System.Drawing.Size(566, 573);
      this.SubCaption = "Please use this dialog to specify reconfiguration options.";
      this.Controls.SetChildIndex(this.subCaptionLabel, 0);
      this.Controls.SetChildIndex(this.captionLabel, 0);
      this.Controls.SetChildIndex(this.ServerFilePermissionsFlowLayoutPanel, 0);
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).EndInit();
      this.ServerFilePermissionsFlowLayoutPanel.ResumeLayout(false);
      this.ServerFilePermissionsFlowLayoutPanel.PerformLayout();
      this.ReviewChangesPanel.ResumeLayout(false);
      this.ReviewChangesPanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.WarningPictureBox)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.FlowLayoutPanel ServerFilePermissionsFlowLayoutPanel;
    private System.Windows.Forms.Label PermissionsMainDescriptionLabel;
    private System.Windows.Forms.Label DataDirectoryPathLabel;
    private System.Windows.Forms.Label PermissionsQuestionLabel;
    private System.Windows.Forms.RadioButton YesRadioButton;
    private System.Windows.Forms.RadioButton YesReviewRadioButton;
    private System.Windows.Forms.Panel ReviewChangesPanel;
    private System.Windows.Forms.Label AccessLevelLabel;
    private System.Windows.Forms.ListView NoAccessListView;
    private System.Windows.Forms.ColumnHeader NoAccessPrincipalColumnHeader;
    private System.Windows.Forms.ColumnHeader NoAccessTypeColumnHeader;
    private System.Windows.Forms.ListView FullControlListView;
    private System.Windows.Forms.ColumnHeader FullControlPrincipalColumnHeader;
    private System.Windows.Forms.ColumnHeader FullControlTypeColumnHeader;
    private System.Windows.Forms.Label NoAccessLabel;
    private System.Windows.Forms.Label FullControlLabel;
    private System.Windows.Forms.Button MoveSelectedLeftButton;
    private System.Windows.Forms.Button MoveSelectedRightButton;
    private System.Windows.Forms.PictureBox WarningPictureBox;
    private System.Windows.Forms.Label WarningAccessLevelLabel;
    private System.Windows.Forms.RadioButton NoRadioButton;
  }
}

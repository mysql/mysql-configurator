/* Copyright (c) 2011, 2020, Oracle and/or its affiliates.

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
  partial class ServerConfigUpgradePage
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerConfigUpgradePage));
      this.UpgradeDatabaseFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
      this.CheckAndUpgradeMainDescriptionPanel = new System.Windows.Forms.Panel();
      this.CheckAndUpgradeMainDescriptionLabel = new System.Windows.Forms.Label();
      this.UpgradeExternalPanel = new System.Windows.Forms.Panel();
      this.ResultLabel = new System.Windows.Forms.Label();
      this.BackupDatabaseCheckBox = new System.Windows.Forms.CheckBox();
      this.BackupDatabaseLabel = new System.Windows.Forms.Label();
      this.CheckButton = new System.Windows.Forms.Button();
      this.CredentialRequirementLabel = new System.Windows.Forms.Label();
      this.UpgradePasswordLabel = new System.Windows.Forms.Label();
      this.UpgradeUserNameLabel = new System.Windows.Forms.Label();
      this.ExistingRootPasswordTextBox = new System.Windows.Forms.TextBox();
      this.UpgradeUserNameValueLabel = new System.Windows.Forms.Label();
      this.SelfContainedUpgradePanel = new System.Windows.Forms.Panel();
      this.SelfContainedUpgradeLabel = new System.Windows.Forms.Label();
      this.SkipUpgradePanel = new System.Windows.Forms.Panel();
      this.SkipUpgradeCheckBox = new System.Windows.Forms.CheckBox();
      this.ConnectionErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).BeginInit();
      this.UpgradeDatabaseFlowLayoutPanel.SuspendLayout();
      this.CheckAndUpgradeMainDescriptionPanel.SuspendLayout();
      this.UpgradeExternalPanel.SuspendLayout();
      this.SelfContainedUpgradePanel.SuspendLayout();
      this.SkipUpgradePanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ConnectionErrorProvider)).BeginInit();
      this.SuspendLayout();
      // 
      // subCaptionLabel
      // 
      this.subCaptionLabel.Location = new System.Drawing.Point(25, 66);
      this.subCaptionLabel.Size = new System.Drawing.Size(440, 10);
      this.subCaptionLabel.Text = "Please use this dialog to specify reconfiguration options.";
      this.subCaptionLabel.Visible = false;
      // 
      // captionLabel
      // 
      this.captionLabel.Size = new System.Drawing.Size(298, 25);
      this.captionLabel.Text = "Check and Upgrade System Tables";
      // 
      // UpgradeDatabaseFlowLayoutPanel
      // 
      this.UpgradeDatabaseFlowLayoutPanel.AccessibleDescription = "A panel containing inner panels with options appearing depending on the upgrading" +
    " process handled by MySQL Server.";
      this.UpgradeDatabaseFlowLayoutPanel.AccessibleName = "Upgrade Database Flow Panel";
      this.UpgradeDatabaseFlowLayoutPanel.Controls.Add(this.CheckAndUpgradeMainDescriptionPanel);
      this.UpgradeDatabaseFlowLayoutPanel.Controls.Add(this.UpgradeExternalPanel);
      this.UpgradeDatabaseFlowLayoutPanel.Controls.Add(this.SelfContainedUpgradePanel);
      this.UpgradeDatabaseFlowLayoutPanel.Controls.Add(this.SkipUpgradePanel);
      this.UpgradeDatabaseFlowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
      this.UpgradeDatabaseFlowLayoutPanel.Location = new System.Drawing.Point(3, 58);
      this.UpgradeDatabaseFlowLayoutPanel.Name = "UpgradeDatabaseFlowLayoutPanel";
      this.UpgradeDatabaseFlowLayoutPanel.Size = new System.Drawing.Size(563, 510);
      this.UpgradeDatabaseFlowLayoutPanel.TabIndex = 1;
      // 
      // CheckAndUpgradeMainDescriptionPanel
      // 
      this.CheckAndUpgradeMainDescriptionPanel.AccessibleDescription = "A panel containing an introduction text about the check and upgrade process.";
      this.CheckAndUpgradeMainDescriptionPanel.AccessibleName = "Check And Upgrade Description Group";
      this.CheckAndUpgradeMainDescriptionPanel.Controls.Add(this.CheckAndUpgradeMainDescriptionLabel);
      this.CheckAndUpgradeMainDescriptionPanel.Location = new System.Drawing.Point(3, 3);
      this.CheckAndUpgradeMainDescriptionPanel.Name = "CheckAndUpgradeMainDescriptionPanel";
      this.CheckAndUpgradeMainDescriptionPanel.Size = new System.Drawing.Size(560, 36);
      this.CheckAndUpgradeMainDescriptionPanel.TabIndex = 4;
      // 
      // CheckAndUpgradeMainDescriptionLabel
      // 
      this.CheckAndUpgradeMainDescriptionLabel.AccessibleDescription = "A label displaying a description about the configuration page";
      this.CheckAndUpgradeMainDescriptionLabel.AccessibleName = "Check And Upgrade Description";
      this.CheckAndUpgradeMainDescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.CheckAndUpgradeMainDescriptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.CheckAndUpgradeMainDescriptionLabel.Location = new System.Drawing.Point(19, 2);
      this.CheckAndUpgradeMainDescriptionLabel.Name = "CheckAndUpgradeMainDescriptionLabel";
      this.CheckAndUpgradeMainDescriptionLabel.Size = new System.Drawing.Size(517, 34);
      this.CheckAndUpgradeMainDescriptionLabel.TabIndex = 0;
      this.CheckAndUpgradeMainDescriptionLabel.Text = "To maintain data integrity following a server upgrade, MySQL Configurator must check" +
    " your database and upgrade its system and data dictionary tables if necessary.";
      // 
      // UpgradeExternalPanel
      // 
      this.UpgradeExternalPanel.AccessibleDescription = "A panel containing controls for upgrading the system tables calling the external " +
    "mysql_upgrade cllient.";
      this.UpgradeExternalPanel.AccessibleName = "Upgrade External Group";
      this.UpgradeExternalPanel.Controls.Add(this.ResultLabel);
      this.UpgradeExternalPanel.Controls.Add(this.BackupDatabaseCheckBox);
      this.UpgradeExternalPanel.Controls.Add(this.BackupDatabaseLabel);
      this.UpgradeExternalPanel.Controls.Add(this.CheckButton);
      this.UpgradeExternalPanel.Controls.Add(this.CredentialRequirementLabel);
      this.UpgradeExternalPanel.Controls.Add(this.UpgradePasswordLabel);
      this.UpgradeExternalPanel.Controls.Add(this.UpgradeUserNameLabel);
      this.UpgradeExternalPanel.Controls.Add(this.ExistingRootPasswordTextBox);
      this.UpgradeExternalPanel.Controls.Add(this.UpgradeUserNameValueLabel);
      this.UpgradeExternalPanel.Location = new System.Drawing.Point(3, 45);
      this.UpgradeExternalPanel.Name = "UpgradeExternalPanel";
      this.UpgradeExternalPanel.Size = new System.Drawing.Size(560, 191);
      this.UpgradeExternalPanel.TabIndex = 6;
      // 
      // ResultLabel
      // 
      this.ResultLabel.AccessibleDescription = "A label displaying the result of the root credentials check";
      this.ResultLabel.AccessibleName = "Root Credentials Check Result";
      this.ResultLabel.AutoSize = true;
      this.ResultLabel.Location = new System.Drawing.Point(195, 161);
      this.ResultLabel.Name = "ResultLabel";
      this.ResultLabel.Size = new System.Drawing.Size(0, 25);
      this.ResultLabel.TabIndex = 0;
      // 
      // BackupDatabaseCheckBox
      // 
      this.BackupDatabaseCheckBox.AccessibleDescription = "A check box to back up the database before running the upgrade process";
      this.BackupDatabaseCheckBox.AccessibleName = "Back Up MySQL Database";
      this.BackupDatabaseCheckBox.AutoSize = true;
      this.BackupDatabaseCheckBox.Location = new System.Drawing.Point(22, 30);
      this.BackupDatabaseCheckBox.Name = "BackupDatabaseCheckBox";
      this.BackupDatabaseCheckBox.Size = new System.Drawing.Size(550, 29);
      this.BackupDatabaseCheckBox.TabIndex = 1;
      this.BackupDatabaseCheckBox.Text = "Back up the MySQL database before upgrading its system tables";
      this.BackupDatabaseCheckBox.UseVisualStyleBackColor = true;
      this.BackupDatabaseCheckBox.CheckedChanged += new System.EventHandler(this.BackupDatabaseCheckBox_CheckedChanged);
      // 
      // BackupDatabaseLabel
      // 
      this.BackupDatabaseLabel.AccessibleDescription = "A label displaying instructions about backing up the database before running the " +
    "upgrade process";
      this.BackupDatabaseLabel.AccessibleName = "Backup MySQL Database Description";
      this.BackupDatabaseLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.BackupDatabaseLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.BackupDatabaseLabel.Location = new System.Drawing.Point(19, 4);
      this.BackupDatabaseLabel.Name = "BackupDatabaseLabel";
      this.BackupDatabaseLabel.Size = new System.Drawing.Size(520, 23);
      this.BackupDatabaseLabel.TabIndex = 0;
      this.BackupDatabaseLabel.Text = "Before performing an upgrade, back up the MySQL database to ensure it can be rest" +
    "ored later.";
      // 
      // CheckButton
      // 
      this.CheckButton.AccessibleDescription = "A button to check the root password is correct";
      this.CheckButton.AccessibleName = "Root Credentials Check";
      this.CheckButton.Location = new System.Drawing.Point(90, 157);
      this.CheckButton.Name = "CheckButton";
      this.CheckButton.Size = new System.Drawing.Size(75, 23);
      this.CheckButton.TabIndex = 7;
      this.CheckButton.Text = "Check";
      this.CheckButton.UseVisualStyleBackColor = true;
      this.CheckButton.Click += new System.EventHandler(this.CheckButton_Click);
      // 
      // CredentialRequirementLabel
      // 
      this.CredentialRequirementLabel.AccessibleDescription = "A label displaying instructions about root credentials being needed for the upgra" +
    "de";
      this.CredentialRequirementLabel.AccessibleName = "Root Credentials Required";
      this.CredentialRequirementLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.CredentialRequirementLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.CredentialRequirementLabel.Location = new System.Drawing.Point(19, 64);
      this.CredentialRequirementLabel.Name = "CredentialRequirementLabel";
      this.CredentialRequirementLabel.Size = new System.Drawing.Size(520, 35);
      this.CredentialRequirementLabel.TabIndex = 2;
      this.CredentialRequirementLabel.Text = "The check and upgrade process needs to be performed using the MySQL root user acc" +
    "ount created when MySQL Server was initially installed. Please enter the current" +
    " password for this user.";
      // 
      // UpgradePasswordLabel
      // 
      this.UpgradePasswordLabel.AccessibleDescription = "A label displaying the text password";
      this.UpgradePasswordLabel.AccessibleName = "Password Text";
      this.UpgradePasswordLabel.AutoSize = true;
      this.UpgradePasswordLabel.Location = new System.Drawing.Point(24, 132);
      this.UpgradePasswordLabel.Name = "UpgradePasswordLabel";
      this.UpgradePasswordLabel.Size = new System.Drawing.Size(91, 25);
      this.UpgradePasswordLabel.TabIndex = 5;
      this.UpgradePasswordLabel.Text = "Password:";
      // 
      // UpgradeUserNameLabel
      // 
      this.UpgradeUserNameLabel.AccessibleDescription = "A label displaying the text user";
      this.UpgradeUserNameLabel.AccessibleName = "User Text";
      this.UpgradeUserNameLabel.AutoSize = true;
      this.UpgradeUserNameLabel.Location = new System.Drawing.Point(51, 109);
      this.UpgradeUserNameLabel.Name = "UpgradeUserNameLabel";
      this.UpgradeUserNameLabel.Size = new System.Drawing.Size(51, 25);
      this.UpgradeUserNameLabel.TabIndex = 3;
      this.UpgradeUserNameLabel.Text = "User:";
      // 
      // ExistingRootPasswordTextBox
      // 
      this.ExistingRootPasswordTextBox.AccessibleDescription = "A text box to input the password of the root account";
      this.ExistingRootPasswordTextBox.AccessibleName = "Root Password";
      this.ExistingRootPasswordTextBox.Location = new System.Drawing.Point(90, 129);
      this.ExistingRootPasswordTextBox.Name = "ExistingRootPasswordTextBox";
      this.ExistingRootPasswordTextBox.Size = new System.Drawing.Size(217, 31);
      this.ExistingRootPasswordTextBox.TabIndex = 6;
      this.ExistingRootPasswordTextBox.UseSystemPasswordChar = true;
      this.ExistingRootPasswordTextBox.TextChanged += new System.EventHandler(this.TextChangedHandler);
      this.ExistingRootPasswordTextBox.Validated += new System.EventHandler(this.ValidatedHandler);
      // 
      // UpgradeUserNameValueLabel
      // 
      this.UpgradeUserNameValueLabel.AccessibleDescription = "A label displaying the root user account";
      this.UpgradeUserNameValueLabel.AccessibleName = "Root User";
      this.UpgradeUserNameValueLabel.AutoSize = true;
      this.UpgradeUserNameValueLabel.Location = new System.Drawing.Point(90, 109);
      this.UpgradeUserNameValueLabel.Name = "UpgradeUserNameValueLabel";
      this.UpgradeUserNameValueLabel.Size = new System.Drawing.Size(134, 25);
      this.UpgradeUserNameValueLabel.TabIndex = 4;
      this.UpgradeUserNameValueLabel.Text = "root@localhost";
      // 
      // SelfContainedUpgradePanel
      // 
      this.SelfContainedUpgradePanel.AccessibleDescription = "A panel containing controls for upgrading the system tables without the need of r" +
    "unning an external client.";
      this.SelfContainedUpgradePanel.AccessibleName = "Self Contained Upgrade Group";
      this.SelfContainedUpgradePanel.Controls.Add(this.SelfContainedUpgradeLabel);
      this.SelfContainedUpgradePanel.Location = new System.Drawing.Point(3, 242);
      this.SelfContainedUpgradePanel.Name = "SelfContainedUpgradePanel";
      this.SelfContainedUpgradePanel.Size = new System.Drawing.Size(560, 64);
      this.SelfContainedUpgradePanel.TabIndex = 7;
      // 
      // SelfContainedUpgradeLabel
      // 
      this.SelfContainedUpgradeLabel.AccessibleDescription = "A label displaying a description about a self contained system tables upgrade.";
      this.SelfContainedUpgradeLabel.AccessibleName = "Self Contained Upgrade Description";
      this.SelfContainedUpgradeLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.SelfContainedUpgradeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.SelfContainedUpgradeLabel.Location = new System.Drawing.Point(19, 0);
      this.SelfContainedUpgradeLabel.Name = "SelfContainedUpgradeLabel";
      this.SelfContainedUpgradeLabel.Size = new System.Drawing.Size(520, 64);
      this.SelfContainedUpgradeLabel.TabIndex = 0;
      this.SelfContainedUpgradeLabel.Text = resources.GetString("SelfContainedUpgradeLabel.Text");
      // 
      // SkipUpgradePanel
      // 
      this.SkipUpgradePanel.AccessibleDescription = "A panel containing controls about skipping the system tables upgrade";
      this.SkipUpgradePanel.AccessibleName = "Skip Database Upgrade Group";
      this.SkipUpgradePanel.Controls.Add(this.SkipUpgradeCheckBox);
      this.SkipUpgradePanel.Location = new System.Drawing.Point(3, 312);
      this.SkipUpgradePanel.Name = "SkipUpgradePanel";
      this.SkipUpgradePanel.Size = new System.Drawing.Size(560, 22);
      this.SkipUpgradePanel.TabIndex = 8;
      // 
      // SkipUpgradeCheckBox
      // 
      this.SkipUpgradeCheckBox.AccessibleDescription = "A check box to skip the system tables check and upgrade which is not recommended";
      this.SkipUpgradeCheckBox.AccessibleName = "Skip Database Upgrade";
      this.SkipUpgradeCheckBox.AutoSize = true;
      this.SkipUpgradeCheckBox.Location = new System.Drawing.Point(22, 2);
      this.SkipUpgradeCheckBox.Name = "SkipUpgradeCheckBox";
      this.SkipUpgradeCheckBox.Size = new System.Drawing.Size(577, 29);
      this.SkipUpgradeCheckBox.TabIndex = 1;
      this.SkipUpgradeCheckBox.Text = "Skip system tables upgrade check and process. (Not recommended)";
      this.SkipUpgradeCheckBox.UseVisualStyleBackColor = true;
      this.SkipUpgradeCheckBox.CheckedChanged += new System.EventHandler(this.SkipUpgradeCheckBox_CheckedChanged);
      // 
      // ConnectionErrorProvider
      // 
      this.ConnectionErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
      this.ConnectionErrorProvider.ContainerControl = this;
      // 
      // ServerConfigUpgradePage
      // 
      this.AccessibleDescription = "A configuration wizard page to upgrade the database upon a server upgrade or recr" +
    "eate an existing sandbox cluster";
      this.AccessibleName = "Check And Upgrade Database Page";
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
      this.Caption = "Check and Upgrade System Tables";
      this.Controls.Add(this.UpgradeDatabaseFlowLayoutPanel);
      this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "ServerConfigUpgradePage";
      this.Size = new System.Drawing.Size(566, 573);
      this.SubCaption = "Please use this dialog to specify reconfiguration options.";
      this.Controls.SetChildIndex(this.subCaptionLabel, 0);
      this.Controls.SetChildIndex(this.UpgradeDatabaseFlowLayoutPanel, 0);
      this.Controls.SetChildIndex(this.captionLabel, 0);
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).EndInit();
      this.UpgradeDatabaseFlowLayoutPanel.ResumeLayout(false);
      this.CheckAndUpgradeMainDescriptionPanel.ResumeLayout(false);
      this.UpgradeExternalPanel.ResumeLayout(false);
      this.UpgradeExternalPanel.PerformLayout();
      this.SelfContainedUpgradePanel.ResumeLayout(false);
      this.SkipUpgradePanel.ResumeLayout(false);
      this.SkipUpgradePanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ConnectionErrorProvider)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.FlowLayoutPanel UpgradeDatabaseFlowLayoutPanel;
    private System.Windows.Forms.Panel CheckAndUpgradeMainDescriptionPanel;
    private System.Windows.Forms.Label CheckAndUpgradeMainDescriptionLabel;
    private System.Windows.Forms.Panel UpgradeExternalPanel;
    private System.Windows.Forms.CheckBox BackupDatabaseCheckBox;
    private System.Windows.Forms.Label BackupDatabaseLabel;
    private System.Windows.Forms.Label ResultLabel;
    private System.Windows.Forms.Button CheckButton;
    private System.Windows.Forms.Label CredentialRequirementLabel;
    private System.Windows.Forms.Label UpgradePasswordLabel;
    private System.Windows.Forms.Label UpgradeUserNameLabel;
    private System.Windows.Forms.TextBox ExistingRootPasswordTextBox;
    private System.Windows.Forms.Label UpgradeUserNameValueLabel;
    private System.Windows.Forms.Panel SelfContainedUpgradePanel;
    private System.Windows.Forms.Label SelfContainedUpgradeLabel;
    private System.Windows.Forms.Panel SkipUpgradePanel;
    private System.Windows.Forms.ErrorProvider ConnectionErrorProvider;
    private System.Windows.Forms.CheckBox SkipUpgradeCheckBox;
  }
}

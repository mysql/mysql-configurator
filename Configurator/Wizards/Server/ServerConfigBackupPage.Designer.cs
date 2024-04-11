/* Copyright (c) 2023, 2024, Oracle and/or its affiliates.

  This program is free software; you can redistribute it and/or modify 
  it under the terms of the GNU General Public License, version 2.0, as 
  published by the Free Software Foundation.

  This program is designed to work with certain software (including
  but not limited to OpenSSL) that is licensed under separate terms, as
  designated in a particular file or component or in included license
  documentation. The authors of MySQL hereby grant you an additional
  permission to link the program and your derivative works with the
  separately licensed software that they have either included with
  the program or referenced in the documentation.

  This program is distributed in the hope that it will be useful, but 
  WITHOUT ANY WARRANTY; without even the implied warranty of 
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
  See the GNU General Public License, version 2.0, for more details. 

  You should have received a copy of the GNU General Public License 
  along with this program; if not, write to the Free Software Foundation, Inc., 
  51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA */

namespace MySql.Configurator.Wizards.Server
{
  partial class ServerConfigBackupPage
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerConfigBackupPage));
      this.UpgradeDatabaseFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
      this.UpgradeExternalPanel = new System.Windows.Forms.Panel();
      this.BackupDatabaseLabel = new System.Windows.Forms.Label();
      this.RunBackupRadioButton = new System.Windows.Forms.RadioButton();
      this.CredentialsPanel = new System.Windows.Forms.Panel();
      this.ConnectDescriptionLabel = new System.Windows.Forms.Label();
      this.ResultLabel = new System.Windows.Forms.Label();
      this.RootUserLabel = new System.Windows.Forms.Label();
      this.PasswordTextBox = new System.Windows.Forms.TextBox();
      this.ConnectButton = new System.Windows.Forms.Button();
      this.UserLabel = new System.Windows.Forms.Label();
      this.RootPasswordLabel = new System.Windows.Forms.Label();
      this.SkipBackupRadioButton = new System.Windows.Forms.RadioButton();
      this.ConnectionErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).BeginInit();
      this.UpgradeDatabaseFlowLayoutPanel.SuspendLayout();
      this.UpgradeExternalPanel.SuspendLayout();
      this.CredentialsPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ConnectionErrorProvider)).BeginInit();
      this.SuspendLayout();
      // 
      // subCaptionLabel
      // 
      this.subCaptionLabel.Font = new System.Drawing.Font("Segoe UI", 4F);
      this.subCaptionLabel.Location = new System.Drawing.Point(25, 66);
      this.subCaptionLabel.Size = new System.Drawing.Size(440, 10);
      this.subCaptionLabel.Text = "Please use this dialog to specify reconfiguration options.";
      this.subCaptionLabel.Visible = false;
      // 
      // captionLabel
      // 
      this.captionLabel.Location = new System.Drawing.Point(26, 30);
      this.captionLabel.Size = new System.Drawing.Size(117, 25);
      this.captionLabel.Text = "Backup Data";
      // 
      // UpgradeDatabaseFlowLayoutPanel
      // 
      this.UpgradeDatabaseFlowLayoutPanel.AccessibleDescription = "A panel containing inner panels with options appearing depending on the upgrading" +
    " process handled by MySQL Server.";
      this.UpgradeDatabaseFlowLayoutPanel.AccessibleName = "Upgrade database flow layout";
      this.UpgradeDatabaseFlowLayoutPanel.Controls.Add(this.UpgradeExternalPanel);
      this.UpgradeDatabaseFlowLayoutPanel.Controls.Add(this.RunBackupRadioButton);
      this.UpgradeDatabaseFlowLayoutPanel.Controls.Add(this.CredentialsPanel);
      this.UpgradeDatabaseFlowLayoutPanel.Controls.Add(this.SkipBackupRadioButton);
      this.UpgradeDatabaseFlowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
      this.UpgradeDatabaseFlowLayoutPanel.Location = new System.Drawing.Point(27, 58);
      this.UpgradeDatabaseFlowLayoutPanel.Name = "UpgradeDatabaseFlowLayoutPanel";
      this.UpgradeDatabaseFlowLayoutPanel.Size = new System.Drawing.Size(514, 510);
      this.UpgradeDatabaseFlowLayoutPanel.TabIndex = 1;
      // 
      // UpgradeExternalPanel
      // 
      this.UpgradeExternalPanel.AccessibleDescription = "A panel containing controls for upgrading the system tables calling the external " +
    "mysql_upgrade cllient.";
      this.UpgradeExternalPanel.AccessibleName = "Upgrade External Group";
      this.UpgradeExternalPanel.Controls.Add(this.BackupDatabaseLabel);
      this.UpgradeExternalPanel.Location = new System.Drawing.Point(3, 3);
      this.UpgradeExternalPanel.Name = "UpgradeExternalPanel";
      this.UpgradeExternalPanel.Size = new System.Drawing.Size(511, 67);
      this.UpgradeExternalPanel.TabIndex = 6;
      // 
      // BackupDatabaseLabel
      // 
      this.BackupDatabaseLabel.AccessibleDescription = "A label displaying instructions about backing up the database before running the " +
    "upgrade process";
      this.BackupDatabaseLabel.AccessibleName = "Backup MySQL databases description";
      this.BackupDatabaseLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.BackupDatabaseLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.BackupDatabaseLabel.Location = new System.Drawing.Point(-1, 10);
      this.BackupDatabaseLabel.Name = "BackupDatabaseLabel";
      this.BackupDatabaseLabel.Size = new System.Drawing.Size(520, 79);
      this.BackupDatabaseLabel.TabIndex = 0;
      this.BackupDatabaseLabel.Text = resources.GetString("BackupDatabaseLabel.Text");
      // 
      // RunBackupRadioButton
      // 
      this.RunBackupRadioButton.AccessibleDescription = "An option to specify that a backup";
      this.RunBackupRadioButton.AccessibleName = "Backup databses using mysqldump option";
      this.RunBackupRadioButton.AutoSize = true;
      this.RunBackupRadioButton.Checked = true;
      this.RunBackupRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.RunBackupRadioButton.Location = new System.Drawing.Point(4, 78);
      this.RunBackupRadioButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.RunBackupRadioButton.Name = "RunBackupRadioButton";
      this.RunBackupRadioButton.Size = new System.Drawing.Size(402, 29);
      this.RunBackupRadioButton.TabIndex = 1;
      this.RunBackupRadioButton.TabStop = true;
      this.RunBackupRadioButton.Text = "Run a mysqldump backup prior to upgrade";
      this.RunBackupRadioButton.UseVisualStyleBackColor = true;
      this.RunBackupRadioButton.CheckedChanged += new System.EventHandler(this.BackupDatabaseCheckBox_CheckedChanged);
      this.RunBackupRadioButton.Validated += new System.EventHandler(this.ValidatedHandler);
      // 
      // CredentialsPanel
      // 
      this.CredentialsPanel.AccessibleDescription = "A panel containing the controls to connect to the MySQL Server instance that is b" +
    "eing upgraded.";
      this.CredentialsPanel.AccessibleName = "Credentials";
      this.CredentialsPanel.Controls.Add(this.ConnectDescriptionLabel);
      this.CredentialsPanel.Controls.Add(this.ResultLabel);
      this.CredentialsPanel.Controls.Add(this.RootUserLabel);
      this.CredentialsPanel.Controls.Add(this.PasswordTextBox);
      this.CredentialsPanel.Controls.Add(this.ConnectButton);
      this.CredentialsPanel.Controls.Add(this.UserLabel);
      this.CredentialsPanel.Controls.Add(this.RootPasswordLabel);
      this.CredentialsPanel.Location = new System.Drawing.Point(3, 115);
      this.CredentialsPanel.Name = "CredentialsPanel";
      this.CredentialsPanel.Size = new System.Drawing.Size(511, 151);
      this.CredentialsPanel.TabIndex = 9;
      // 
      // ConnectDescriptionLabel
      // 
      this.ConnectDescriptionLabel.AccessibleDescription = "A label displaying a description about the need to provide the credentials for th" +
    "e root user.";
      this.ConnectDescriptionLabel.AccessibleName = "Need for credentials description";
      this.ConnectDescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.ConnectDescriptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.ConnectDescriptionLabel.Location = new System.Drawing.Point(17, 3);
      this.ConnectDescriptionLabel.Name = "ConnectDescriptionLabel";
      this.ConnectDescriptionLabel.Size = new System.Drawing.Size(494, 44);
      this.ConnectDescriptionLabel.TabIndex = 9;
      this.ConnectDescriptionLabel.Text = "To create the backup, a connection to the MySQL Server instance must be made. Ple" +
    "ase provide the password for the root user and then click the Connect button:";
      // 
      // ResultLabel
      // 
      this.ResultLabel.AccessibleDescription = "A label displaying the result of the root credentials check";
      this.ResultLabel.AccessibleName = "Root Credentials Check Result";
      this.ResultLabel.AutoSize = true;
      this.ResultLabel.Location = new System.Drawing.Point(146, 116);
      this.ResultLabel.Name = "ResultLabel";
      this.ResultLabel.Size = new System.Drawing.Size(0, 25);
      this.ResultLabel.TabIndex = 7;
      // 
      // RootUserLabel
      // 
      this.RootUserLabel.AccessibleDescription = "A text with the word root";
      this.RootUserLabel.AccessibleName = "Root text";
      this.RootUserLabel.AutoSize = true;
      this.RootUserLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.RootUserLabel.Location = new System.Drawing.Point(88, 48);
      this.RootUserLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.RootUserLabel.Name = "RootUserLabel";
      this.RootUserLabel.Size = new System.Drawing.Size(46, 25);
      this.RootUserLabel.TabIndex = 3;
      this.RootUserLabel.Text = "root";
      // 
      // PasswordTextBox
      // 
      this.PasswordTextBox.AccessibleDescription = "A field to input the password of the root user.";
      this.PasswordTextBox.AccessibleName = "Root password";
      this.PasswordTextBox.Location = new System.Drawing.Point(90, 73);
      this.PasswordTextBox.Name = "PasswordTextBox";
      this.PasswordTextBox.PasswordChar = '*';
      this.PasswordTextBox.Size = new System.Drawing.Size(126, 31);
      this.PasswordTextBox.TabIndex = 5;
      this.PasswordTextBox.TextChanged += new System.EventHandler(this.TextChangedHandler);
      this.PasswordTextBox.Validated += new System.EventHandler(this.ValidatedHandler);
      // 
      // ConnectButton
      // 
      this.ConnectButton.AccessibleDescription = "A button to connect to the existing MySQL Server instance.";
      this.ConnectButton.AccessibleName = "Connect";
      this.ConnectButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ConnectButton.Location = new System.Drawing.Point(20, 110);
      this.ConnectButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.ConnectButton.Name = "ConnectButton";
      this.ConnectButton.Size = new System.Drawing.Size(97, 28);
      this.ConnectButton.TabIndex = 6;
      this.ConnectButton.Text = "Connect";
      this.ToolTip.SetToolTip(this.ConnectButton, "Tests the connection to the MySQL Server installation to upgrade.");
      this.ConnectButton.UseVisualStyleBackColor = true;
      this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
      // 
      // UserLabel
      // 
      this.UserLabel.AccessibleDescription = "A text to specify the user that will connect to the MySQL Server instance.";
      this.UserLabel.AccessibleName = "User text";
      this.UserLabel.AutoSize = true;
      this.UserLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.UserLabel.Location = new System.Drawing.Point(18, 48);
      this.UserLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.UserLabel.Name = "UserLabel";
      this.UserLabel.Size = new System.Drawing.Size(51, 25);
      this.UserLabel.TabIndex = 2;
      this.UserLabel.Text = "User:";
      // 
      // RootPasswordLabel
      // 
      this.RootPasswordLabel.AccessibleDescription = "A text to ask for the password of the root user.";
      this.RootPasswordLabel.AccessibleName = "Root password text";
      this.RootPasswordLabel.AutoSize = true;
      this.RootPasswordLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.RootPasswordLabel.Location = new System.Drawing.Point(18, 77);
      this.RootPasswordLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.RootPasswordLabel.Name = "RootPasswordLabel";
      this.RootPasswordLabel.Size = new System.Drawing.Size(91, 25);
      this.RootPasswordLabel.TabIndex = 4;
      this.RootPasswordLabel.Text = "Password:";
      // 
      // SkipBackupRadioButton
      // 
      this.SkipBackupRadioButton.AccessibleDescription = "An option to replace a selected MySQL Server installation, meaning its data direc" +
    "tory will be used for the installation being configured.";
      this.SkipBackupRadioButton.AccessibleName = "Skip backup option";
      this.SkipBackupRadioButton.AutoSize = true;
      this.SkipBackupRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.SkipBackupRadioButton.Location = new System.Drawing.Point(4, 274);
      this.SkipBackupRadioButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.SkipBackupRadioButton.Name = "SkipBackupRadioButton";
      this.SkipBackupRadioButton.Size = new System.Drawing.Size(370, 29);
      this.SkipBackupRadioButton.TabIndex = 8;
      this.SkipBackupRadioButton.Text = "No thanks, I have already run a backup";
      this.SkipBackupRadioButton.UseVisualStyleBackColor = true;
      this.SkipBackupRadioButton.CheckedChanged += new System.EventHandler(this.BackupDatabaseCheckBox_CheckedChanged);
      this.SkipBackupRadioButton.Validated += new System.EventHandler(this.ValidatedHandler);
      // 
      // ConnectionErrorProvider
      // 
      this.ConnectionErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
      this.ConnectionErrorProvider.ContainerControl = this;
      // 
      // ServerConfigBackupPage
      // 
      this.AccessibleDescription = "A configuration wizard page to backup the existing databases";
      this.AccessibleName = "Backup Page";
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
      this.Caption = "Backup Data";
      this.Controls.Add(this.UpgradeDatabaseFlowLayoutPanel);
      this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "ServerConfigBackupPage";
      this.Size = new System.Drawing.Size(566, 573);
      this.SubCaption = "Please use this dialog to specify reconfiguration options.";
      this.Controls.SetChildIndex(this.UpgradeDatabaseFlowLayoutPanel, 0);
      this.Controls.SetChildIndex(this.subCaptionLabel, 0);
      this.Controls.SetChildIndex(this.captionLabel, 0);
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).EndInit();
      this.UpgradeDatabaseFlowLayoutPanel.ResumeLayout(false);
      this.UpgradeDatabaseFlowLayoutPanel.PerformLayout();
      this.UpgradeExternalPanel.ResumeLayout(false);
      this.CredentialsPanel.ResumeLayout(false);
      this.CredentialsPanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ConnectionErrorProvider)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.FlowLayoutPanel UpgradeDatabaseFlowLayoutPanel;
    private System.Windows.Forms.Panel UpgradeExternalPanel;
    private System.Windows.Forms.Label BackupDatabaseLabel;
    private System.Windows.Forms.ErrorProvider ConnectionErrorProvider;
    private System.Windows.Forms.RadioButton RunBackupRadioButton;
    private System.Windows.Forms.Panel CredentialsPanel;
    private System.Windows.Forms.Label UserLabel;
    private System.Windows.Forms.Label RootPasswordLabel;
    private System.Windows.Forms.RadioButton SkipBackupRadioButton;
    private System.Windows.Forms.Button ConnectButton;
    private System.Windows.Forms.TextBox PasswordTextBox;
    private System.Windows.Forms.Label RootUserLabel;
    private System.Windows.Forms.Label ResultLabel;
    private System.Windows.Forms.Label ConnectDescriptionLabel;
  }
}

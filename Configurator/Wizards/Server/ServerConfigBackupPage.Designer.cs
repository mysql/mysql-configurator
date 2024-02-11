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
      this.SkipBackupRadioButton = new System.Windows.Forms.RadioButton();
      this.RunBackupRadioButton = new System.Windows.Forms.RadioButton();
      this.BackupDatabaseLabel = new System.Windows.Forms.Label();
      this.ConnectionErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).BeginInit();
      this.UpgradeDatabaseFlowLayoutPanel.SuspendLayout();
      this.UpgradeExternalPanel.SuspendLayout();
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
      this.UpgradeDatabaseFlowLayoutPanel.AccessibleName = "Upgrade Database Flow Panel";
      this.UpgradeDatabaseFlowLayoutPanel.Controls.Add(this.UpgradeExternalPanel);
      this.UpgradeDatabaseFlowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
      this.UpgradeDatabaseFlowLayoutPanel.Location = new System.Drawing.Point(3, 58);
      this.UpgradeDatabaseFlowLayoutPanel.Name = "UpgradeDatabaseFlowLayoutPanel";
      this.UpgradeDatabaseFlowLayoutPanel.Size = new System.Drawing.Size(563, 510);
      this.UpgradeDatabaseFlowLayoutPanel.TabIndex = 1;
      // 
      // UpgradeExternalPanel
      // 
      this.UpgradeExternalPanel.AccessibleDescription = "A panel containing controls for upgrading the system tables calling the external " +
    "mysql_upgrade cllient.";
      this.UpgradeExternalPanel.AccessibleName = "Upgrade External Group";
      this.UpgradeExternalPanel.Controls.Add(this.SkipBackupRadioButton);
      this.UpgradeExternalPanel.Controls.Add(this.RunBackupRadioButton);
      this.UpgradeExternalPanel.Controls.Add(this.BackupDatabaseLabel);
      this.UpgradeExternalPanel.Location = new System.Drawing.Point(3, 3);
      this.UpgradeExternalPanel.Name = "UpgradeExternalPanel";
      this.UpgradeExternalPanel.Size = new System.Drawing.Size(560, 212);
      this.UpgradeExternalPanel.TabIndex = 6;
      // 
      // SkipBackupRadioButton
      // 
      this.SkipBackupRadioButton.AccessibleDescription = "An option to replace a selected MySQL Server installation, meaning its data direc" +
    "tory will be used for the installation being configured.";
      this.SkipBackupRadioButton.AccessibleName = "Replace MySQL Server installation option";
      this.SkipBackupRadioButton.AutoSize = true;
      this.SkipBackupRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.SkipBackupRadioButton.Location = new System.Drawing.Point(26, 112);
      this.SkipBackupRadioButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.SkipBackupRadioButton.Name = "SkipBackupRadioButton";
      this.SkipBackupRadioButton.Size = new System.Drawing.Size(360, 29);
      this.SkipBackupRadioButton.TabIndex = 6;
      this.SkipBackupRadioButton.Text = "No thanks, I have already run a backup";
      this.SkipBackupRadioButton.UseVisualStyleBackColor = true;
      // 
      // RunBackupRadioButton
      // 
      this.RunBackupRadioButton.AccessibleDescription = "An option to replace a selected MySQL Server installation, meaning its data direc" +
    "tory will be used for the installation being configured.";
      this.RunBackupRadioButton.AccessibleName = "Replace MySQL Server installation option";
      this.RunBackupRadioButton.AutoSize = true;
      this.RunBackupRadioButton.Checked = true;
      this.RunBackupRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.RunBackupRadioButton.Location = new System.Drawing.Point(26, 79);
      this.RunBackupRadioButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.RunBackupRadioButton.Name = "RunBackupRadioButton";
      this.RunBackupRadioButton.Size = new System.Drawing.Size(402, 29);
      this.RunBackupRadioButton.TabIndex = 5;
      this.RunBackupRadioButton.TabStop = true;
      this.RunBackupRadioButton.Text = "Run a mysqldump backup prior to upgrade";
      this.RunBackupRadioButton.UseVisualStyleBackColor = true;
      // 
      // BackupDatabaseLabel
      // 
      this.BackupDatabaseLabel.AccessibleDescription = "A label displaying instructions about backing up the database before running the " +
    "upgrade process";
      this.BackupDatabaseLabel.AccessibleName = "Backup MySQL Database Description";
      this.BackupDatabaseLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.BackupDatabaseLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.BackupDatabaseLabel.Location = new System.Drawing.Point(21, 4);
      this.BackupDatabaseLabel.Name = "BackupDatabaseLabel";
      this.BackupDatabaseLabel.Size = new System.Drawing.Size(520, 79);
      this.BackupDatabaseLabel.TabIndex = 0;
      this.BackupDatabaseLabel.Text = resources.GetString("BackupDatabaseLabel.Text");
      // 
      // ConnectionErrorProvider
      // 
      this.ConnectionErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
      this.ConnectionErrorProvider.ContainerControl = this;
      // 
      // ServerConfigBackupPage
      // 
      this.AccessibleDescription = "A configuration wizard page to upgrade the database upon a server upgrade or recr" +
    "eate an existing sandbox cluster";
      this.AccessibleName = "Check And Upgrade Database Page";
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
      this.UpgradeExternalPanel.ResumeLayout(false);
      this.UpgradeExternalPanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ConnectionErrorProvider)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.FlowLayoutPanel UpgradeDatabaseFlowLayoutPanel;
    private System.Windows.Forms.Panel UpgradeExternalPanel;
    private System.Windows.Forms.Label BackupDatabaseLabel;
    private System.Windows.Forms.ErrorProvider ConnectionErrorProvider;
    private System.Windows.Forms.RadioButton SkipBackupRadioButton;
    private System.Windows.Forms.RadioButton RunBackupRadioButton;
  }
}

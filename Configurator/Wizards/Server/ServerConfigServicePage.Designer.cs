/* Copyright (c) 2023, Oracle and/or its affiliates.

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

using System;

namespace MySql.Configurator.Wizards.Server
{
  partial class ServerConfigServicePage
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
      this.ConfigureAsServiceCheckBox = new System.Windows.Forms.CheckBox();
      this.WindowsServiceDetailsPanel = new System.Windows.Forms.Panel();
      this.WindowsServiceLabel = new System.Windows.Forms.Label();
      this.CustomUserPasswordTextBox = new System.Windows.Forms.TextBox();
      this.CustomUserUsernameTextBox = new System.Windows.Forms.TextBox();
      this.CustomUserPasswordLabel = new System.Windows.Forms.Label();
      this.CustomUserUsernameLabel = new System.Windows.Forms.Label();
      this.CustomUserDescriptionLabel = new System.Windows.Forms.Label();
      this.StandardSystemAccountDescriptionLabel = new System.Windows.Forms.Label();
      this.CustomUserRadioButton = new System.Windows.Forms.RadioButton();
      this.StandardSystemAccountRadioButton = new System.Windows.Forms.RadioButton();
      this.RunWindowsServiceAsDescriptionLabel = new System.Windows.Forms.Label();
      this.RunWindowsServiceAsLabel = new System.Windows.Forms.Label();
      this.StartWindowsServiceAtStartupCheckBox = new System.Windows.Forms.CheckBox();
      this.WindowsServiceNameLabel = new System.Windows.Forms.Label();
      this.WindowsServiceNameTextBox = new System.Windows.Forms.TextBox();
      this.WindowsServiceDescriptionLabel = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).BeginInit();
      this.WindowsServiceDetailsPanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // subCaptionLabel
      // 
      this.subCaptionLabel.Location = new System.Drawing.Point(24, 66);
      this.subCaptionLabel.Size = new System.Drawing.Size(522, 16);
      this.subCaptionLabel.Visible = false;
      // 
      // captionLabel
      // 
      this.captionLabel.Size = new System.Drawing.Size(155, 25);
      this.captionLabel.Text = "Windows Service";
      // 
      // ConfigureAsServiceCheckBox
      // 
      this.ConfigureAsServiceCheckBox.AccessibleDescription = "A check box to create a windows service for the MySQL server";
      this.ConfigureAsServiceCheckBox.AccessibleName = "Configure As Windows Service";
      this.ConfigureAsServiceCheckBox.AutoSize = true;
      this.ConfigureAsServiceCheckBox.Checked = true;
      this.ConfigureAsServiceCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
      this.ConfigureAsServiceCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.ConfigureAsServiceCheckBox.Location = new System.Drawing.Point(30, 67);
      this.ConfigureAsServiceCheckBox.Name = "ConfigureAsServiceCheckBox";
      this.ConfigureAsServiceCheckBox.Size = new System.Drawing.Size(286, 19);
      this.ConfigureAsServiceCheckBox.TabIndex = 2;
      this.ConfigureAsServiceCheckBox.Text = "Configure MySQL Server as a Windows Service";
      this.ConfigureAsServiceCheckBox.UseVisualStyleBackColor = true;
      this.ConfigureAsServiceCheckBox.CheckedChanged += new System.EventHandler(this.ConfigureAsService_CheckedChanged);
      // 
      // WindowsServiceDetailsPanel
      // 
      this.WindowsServiceDetailsPanel.AccessibleDescription = "A panel containing controls about configuring MySQL Server as a Windows service";
      this.WindowsServiceDetailsPanel.AccessibleName = "Windows Service Group";
      this.WindowsServiceDetailsPanel.Controls.Add(this.WindowsServiceLabel);
      this.WindowsServiceDetailsPanel.Controls.Add(this.CustomUserPasswordTextBox);
      this.WindowsServiceDetailsPanel.Controls.Add(this.CustomUserUsernameTextBox);
      this.WindowsServiceDetailsPanel.Controls.Add(this.CustomUserPasswordLabel);
      this.WindowsServiceDetailsPanel.Controls.Add(this.CustomUserUsernameLabel);
      this.WindowsServiceDetailsPanel.Controls.Add(this.CustomUserDescriptionLabel);
      this.WindowsServiceDetailsPanel.Controls.Add(this.StandardSystemAccountDescriptionLabel);
      this.WindowsServiceDetailsPanel.Controls.Add(this.CustomUserRadioButton);
      this.WindowsServiceDetailsPanel.Controls.Add(this.StandardSystemAccountRadioButton);
      this.WindowsServiceDetailsPanel.Controls.Add(this.RunWindowsServiceAsDescriptionLabel);
      this.WindowsServiceDetailsPanel.Controls.Add(this.RunWindowsServiceAsLabel);
      this.WindowsServiceDetailsPanel.Controls.Add(this.StartWindowsServiceAtStartupCheckBox);
      this.WindowsServiceDetailsPanel.Controls.Add(this.WindowsServiceNameLabel);
      this.WindowsServiceDetailsPanel.Controls.Add(this.WindowsServiceNameTextBox);
      this.WindowsServiceDetailsPanel.Controls.Add(this.WindowsServiceDescriptionLabel);
      this.WindowsServiceDetailsPanel.Location = new System.Drawing.Point(26, 92);
      this.WindowsServiceDetailsPanel.Name = "WindowsServiceDetailsPanel";
      this.WindowsServiceDetailsPanel.Size = new System.Drawing.Size(520, 396);
      this.WindowsServiceDetailsPanel.TabIndex = 3;
      // 
      // WindowsServiceLabel
      // 
      this.WindowsServiceLabel.AccessibleDescription = "A label displaying the text Windows service details";
      this.WindowsServiceLabel.AccessibleName = "Windows Service Text";
      this.WindowsServiceLabel.AutoSize = true;
      this.WindowsServiceLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.WindowsServiceLabel.Location = new System.Drawing.Point(1, 13);
      this.WindowsServiceLabel.Name = "WindowsServiceLabel";
      this.WindowsServiceLabel.Size = new System.Drawing.Size(144, 15);
      this.WindowsServiceLabel.TabIndex = 0;
      this.WindowsServiceLabel.Text = "Windows Service Details";
      // 
      // CustomUserPasswordTextBox
      // 
      this.CustomUserPasswordTextBox.AccessibleDescription = "A text box to input the password of the custom user account";
      this.CustomUserPasswordTextBox.AccessibleName = "Password";
      this.CustomUserPasswordTextBox.Location = new System.Drawing.Point(116, 341);
      this.CustomUserPasswordTextBox.Name = "CustomUserPasswordTextBox";
      this.CustomUserPasswordTextBox.Size = new System.Drawing.Size(240, 23);
      this.CustomUserPasswordTextBox.TabIndex = 14;
      this.CustomUserPasswordTextBox.UseSystemPasswordChar = true;
      this.CustomUserPasswordTextBox.Visible = false;
      this.CustomUserPasswordTextBox.TextChanged += new System.EventHandler(this.TextChangedHandler);
      this.CustomUserPasswordTextBox.Validated += new System.EventHandler(this.ValidatedHandler);
      // 
      // CustomUserUsernameTextBox
      // 
      this.CustomUserUsernameTextBox.AccessibleDescription = "A text box to input the name of the custom user account";
      this.CustomUserUsernameTextBox.AccessibleName = "User Name";
      this.CustomUserUsernameTextBox.Location = new System.Drawing.Point(116, 313);
      this.CustomUserUsernameTextBox.Name = "CustomUserUsernameTextBox";
      this.CustomUserUsernameTextBox.Size = new System.Drawing.Size(240, 23);
      this.CustomUserUsernameTextBox.TabIndex = 12;
      this.CustomUserUsernameTextBox.Visible = false;
      this.CustomUserUsernameTextBox.TextChanged += new System.EventHandler(this.TextChangedHandler);
      this.CustomUserUsernameTextBox.Validated += new System.EventHandler(this.ValidatedHandler);
      // 
      // CustomUserPasswordLabel
      // 
      this.CustomUserPasswordLabel.AccessibleDescription = "A label displaying the text password";
      this.CustomUserPasswordLabel.AccessibleName = "Password Text";
      this.CustomUserPasswordLabel.AutoSize = true;
      this.CustomUserPasswordLabel.Location = new System.Drawing.Point(50, 344);
      this.CustomUserPasswordLabel.Name = "CustomUserPasswordLabel";
      this.CustomUserPasswordLabel.Size = new System.Drawing.Size(60, 15);
      this.CustomUserPasswordLabel.TabIndex = 13;
      this.CustomUserPasswordLabel.Text = "Password:";
      this.CustomUserPasswordLabel.Visible = false;
      // 
      // CustomUserUsernameLabel
      // 
      this.CustomUserUsernameLabel.AccessibleDescription = "A label displaying the text user name";
      this.CustomUserUsernameLabel.AccessibleName = "User Name Text";
      this.CustomUserUsernameLabel.AutoSize = true;
      this.CustomUserUsernameLabel.Location = new System.Drawing.Point(42, 315);
      this.CustomUserUsernameLabel.Name = "CustomUserUsernameLabel";
      this.CustomUserUsernameLabel.Size = new System.Drawing.Size(68, 15);
      this.CustomUserUsernameLabel.TabIndex = 11;
      this.CustomUserUsernameLabel.Text = "User Name:";
      this.CustomUserUsernameLabel.Visible = false;
      // 
      // CustomUserDescriptionLabel
      // 
      this.CustomUserDescriptionLabel.AccessibleDescription = "A label displaying a description about the custom user option";
      this.CustomUserDescriptionLabel.AccessibleName = "Custom User Description";
      this.CustomUserDescriptionLabel.AutoSize = true;
      this.CustomUserDescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.CustomUserDescriptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.CustomUserDescriptionLabel.Location = new System.Drawing.Point(43, 291);
      this.CustomUserDescriptionLabel.Name = "CustomUserDescriptionLabel";
      this.CustomUserDescriptionLabel.Size = new System.Drawing.Size(347, 15);
      this.CustomUserDescriptionLabel.TabIndex = 10;
      this.CustomUserDescriptionLabel.Text = "An existing user account can be selected for advanced scenarios.";
      // 
      // StandardSystemAccountDescriptionLabel
      // 
      this.StandardSystemAccountDescriptionLabel.AccessibleDescription = "A label displaying a description about the standard system account option";
      this.StandardSystemAccountDescriptionLabel.AccessibleName = "Standard System Account Description";
      this.StandardSystemAccountDescriptionLabel.AutoSize = true;
      this.StandardSystemAccountDescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.StandardSystemAccountDescriptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.StandardSystemAccountDescriptionLabel.Location = new System.Drawing.Point(43, 242);
      this.StandardSystemAccountDescriptionLabel.Name = "StandardSystemAccountDescriptionLabel";
      this.StandardSystemAccountDescriptionLabel.Size = new System.Drawing.Size(191, 15);
      this.StandardSystemAccountDescriptionLabel.TabIndex = 8;
      this.StandardSystemAccountDescriptionLabel.Text = "Recommended for most scenarios.";
      // 
      // CustomUserRadioButton
      // 
      this.CustomUserRadioButton.AccessibleDescription = "An option to run the window service using a given user account";
      this.CustomUserRadioButton.AccessibleName = "Custom User";
      this.CustomUserRadioButton.AutoSize = true;
      this.CustomUserRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.CustomUserRadioButton.Location = new System.Drawing.Point(24, 266);
      this.CustomUserRadioButton.Name = "CustomUserRadioButton";
      this.CustomUserRadioButton.Size = new System.Drawing.Size(93, 19);
      this.CustomUserRadioButton.TabIndex = 9;
      this.CustomUserRadioButton.TabStop = true;
      this.CustomUserRadioButton.Text = "Custom User";
      this.CustomUserRadioButton.UseVisualStyleBackColor = true;
      this.CustomUserRadioButton.CheckedChanged += new System.EventHandler(this.ServiceAccountTypeCheckChanged);
      // 
      // StandardSystemAccountRadioButton
      // 
      this.StandardSystemAccountRadioButton.AccessibleDescription = "An option to use the standard system account to run the windows service";
      this.StandardSystemAccountRadioButton.AccessibleName = "Standard System Account";
      this.StandardSystemAccountRadioButton.AutoSize = true;
      this.StandardSystemAccountRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.StandardSystemAccountRadioButton.Location = new System.Drawing.Point(24, 218);
      this.StandardSystemAccountRadioButton.Name = "StandardSystemAccountRadioButton";
      this.StandardSystemAccountRadioButton.Size = new System.Drawing.Size(161, 19);
      this.StandardSystemAccountRadioButton.TabIndex = 7;
      this.StandardSystemAccountRadioButton.TabStop = true;
      this.StandardSystemAccountRadioButton.Text = "Standard System Account";
      this.StandardSystemAccountRadioButton.UseVisualStyleBackColor = true;
      this.StandardSystemAccountRadioButton.CheckedChanged += new System.EventHandler(this.ServiceAccountTypeCheckChanged);
      // 
      // RunWindowsServiceAsDescriptionLabel
      // 
      this.RunWindowsServiceAsDescriptionLabel.AccessibleDescription = "A label displaying a description about the run windows service as option";
      this.RunWindowsServiceAsDescriptionLabel.AccessibleName = "Run Windows Service As Description";
      this.RunWindowsServiceAsDescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.RunWindowsServiceAsDescriptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.RunWindowsServiceAsDescriptionLabel.Location = new System.Drawing.Point(1, 181);
      this.RunWindowsServiceAsDescriptionLabel.Name = "RunWindowsServiceAsDescriptionLabel";
      this.RunWindowsServiceAsDescriptionLabel.Size = new System.Drawing.Size(502, 34);
      this.RunWindowsServiceAsDescriptionLabel.TabIndex = 6;
      this.RunWindowsServiceAsDescriptionLabel.Text = "The MySQL Server needs to run under a given user account. Based on the security r" +
    "equirements of your system you need to pick one of the options below.";
      // 
      // RunWindowsServiceAsLabel
      // 
      this.RunWindowsServiceAsLabel.AccessibleDescription = "A label displaying the text run windows service as";
      this.RunWindowsServiceAsLabel.AccessibleName = "Run Windows Service As Text";
      this.RunWindowsServiceAsLabel.AutoSize = true;
      this.RunWindowsServiceAsLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.RunWindowsServiceAsLabel.Location = new System.Drawing.Point(1, 164);
      this.RunWindowsServiceAsLabel.Name = "RunWindowsServiceAsLabel";
      this.RunWindowsServiceAsLabel.Size = new System.Drawing.Size(154, 15);
      this.RunWindowsServiceAsLabel.TabIndex = 5;
      this.RunWindowsServiceAsLabel.Text = "Run Windows Service as ...";
      // 
      // StartWindowsServiceAtStartupCheckBox
      // 
      this.StartWindowsServiceAtStartupCheckBox.AccessibleDescription = "A check box to start the MySQL Windows service automatically";
      this.StartWindowsServiceAtStartupCheckBox.AccessibleName = "Start Service Automatically";
      this.StartWindowsServiceAtStartupCheckBox.AutoSize = true;
      this.StartWindowsServiceAtStartupCheckBox.Checked = true;
      this.StartWindowsServiceAtStartupCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
      this.StartWindowsServiceAtStartupCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.StartWindowsServiceAtStartupCheckBox.Location = new System.Drawing.Point(24, 101);
      this.StartWindowsServiceAtStartupCheckBox.Name = "StartWindowsServiceAtStartupCheckBox";
      this.StartWindowsServiceAtStartupCheckBox.Size = new System.Drawing.Size(241, 19);
      this.StartWindowsServiceAtStartupCheckBox.TabIndex = 4;
      this.StartWindowsServiceAtStartupCheckBox.Text = "Start the MySQL Server at System Startup";
      this.StartWindowsServiceAtStartupCheckBox.UseVisualStyleBackColor = true;
      // 
      // WindowsServiceNameLabel
      // 
      this.WindowsServiceNameLabel.AccessibleDescription = "A label displaying the text Windows service name";
      this.WindowsServiceNameLabel.AccessibleName = "Windows Service Name Text";
      this.WindowsServiceNameLabel.AutoSize = true;
      this.WindowsServiceNameLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.WindowsServiceNameLabel.Location = new System.Drawing.Point(1, 75);
      this.WindowsServiceNameLabel.Name = "WindowsServiceNameLabel";
      this.WindowsServiceNameLabel.Size = new System.Drawing.Size(134, 15);
      this.WindowsServiceNameLabel.TabIndex = 2;
      this.WindowsServiceNameLabel.Text = "Windows Service Name:";
      // 
      // WindowsServiceNameTextBox
      // 
      this.WindowsServiceNameTextBox.AccessibleDescription = "A text box to input the Windows service name";
      this.WindowsServiceNameTextBox.AccessibleName = "Windows Service Name";
      this.WindowsServiceNameTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.WindowsServiceNameTextBox.Location = new System.Drawing.Point(141, 72);
      this.WindowsServiceNameTextBox.Name = "WindowsServiceNameTextBox";
      this.WindowsServiceNameTextBox.Size = new System.Drawing.Size(194, 23);
      this.WindowsServiceNameTextBox.TabIndex = 3;
      this.WindowsServiceNameTextBox.WordWrap = false;
      this.WindowsServiceNameTextBox.TextChanged += new System.EventHandler(this.TextChangedHandler);
      this.WindowsServiceNameTextBox.Validated += new System.EventHandler(this.ValidatedHandler);
      // 
      // WindowsServiceDescriptionLabel
      // 
      this.WindowsServiceDescriptionLabel.AccessibleDescription = "A label displaying a description about the Windows service";
      this.WindowsServiceDescriptionLabel.AccessibleName = "Windows Service Description";
      this.WindowsServiceDescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.WindowsServiceDescriptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.WindowsServiceDescriptionLabel.Location = new System.Drawing.Point(1, 32);
      this.WindowsServiceDescriptionLabel.Name = "WindowsServiceDescriptionLabel";
      this.WindowsServiceDescriptionLabel.Size = new System.Drawing.Size(502, 36);
      this.WindowsServiceDescriptionLabel.TabIndex = 1;
      this.WindowsServiceDescriptionLabel.Text = "Please specify a Windows Service name to be used for this MySQL Server instance.\r" +
    "\nA unique name is required for each instance.";
      // 
      // ServerConfigServicePage
      // 
      this.AccessibleDescription = "A configuration wizard page to configure MySQL Server as a Windows service";
      this.AccessibleName = "Windows Service Page";
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
      this.Caption = "Windows Service";
      this.Controls.Add(this.ConfigureAsServiceCheckBox);
      this.Controls.Add(this.WindowsServiceDetailsPanel);
      this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "ServerConfigServicePage";
      this.Controls.SetChildIndex(this.captionLabel, 0);
      this.Controls.SetChildIndex(this.subCaptionLabel, 0);
      this.Controls.SetChildIndex(this.WindowsServiceDetailsPanel, 0);
      this.Controls.SetChildIndex(this.ConfigureAsServiceCheckBox, 0);
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).EndInit();
      this.WindowsServiceDetailsPanel.ResumeLayout(false);
      this.WindowsServiceDetailsPanel.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.CheckBox ConfigureAsServiceCheckBox;
    private System.Windows.Forms.Panel WindowsServiceDetailsPanel;
    private System.Windows.Forms.Label WindowsServiceLabel;
    private System.Windows.Forms.TextBox CustomUserPasswordTextBox;
    private System.Windows.Forms.TextBox CustomUserUsernameTextBox;
    private System.Windows.Forms.Label CustomUserPasswordLabel;
    private System.Windows.Forms.Label CustomUserUsernameLabel;
    private System.Windows.Forms.Label CustomUserDescriptionLabel;
    private System.Windows.Forms.Label StandardSystemAccountDescriptionLabel;
    private System.Windows.Forms.RadioButton CustomUserRadioButton;
    private System.Windows.Forms.RadioButton StandardSystemAccountRadioButton;
    private System.Windows.Forms.Label RunWindowsServiceAsDescriptionLabel;
    private System.Windows.Forms.Label RunWindowsServiceAsLabel;
    private System.Windows.Forms.CheckBox StartWindowsServiceAtStartupCheckBox;
    private System.Windows.Forms.Label WindowsServiceNameLabel;
    private System.Windows.Forms.TextBox WindowsServiceNameTextBox;
    private System.Windows.Forms.Label WindowsServiceDescriptionLabel;
  }
}

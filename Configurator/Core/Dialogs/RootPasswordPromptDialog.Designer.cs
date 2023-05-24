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

namespace MySql.Configurator.Core.Dialogs
{
  partial class RootPasswordPromptDialog
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
      this.OkButton = new System.Windows.Forms.Button();
      this.DialogCancelButton = new System.Windows.Forms.Button();
      this.PasswordTextBox = new System.Windows.Forms.TextBox();
      this.PasswordLabel = new System.Windows.Forms.Label();
      this.RootUsernameLabel = new System.Windows.Forms.Label();
      this.UsernameTextBox = new System.Windows.Forms.TextBox();
      this.LogoPictureBox = new System.Windows.Forms.PictureBox();
      this.LoginInstructionsLabel = new System.Windows.Forms.Label();
      this.LoginTitleLabel = new System.Windows.Forms.Label();
      this.ConnectionResultPictureBox = new System.Windows.Forms.PictureBox();
      this.ConnectionResultLabel = new System.Windows.Forms.Label();
      this.TestConnectionButton = new System.Windows.Forms.Button();
      this.ContentAreaPanel.SuspendLayout();
      this.CommandAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.ConnectionResultPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // FootnoteAreaPanel
      // 
      this.FootnoteAreaPanel.Location = new System.Drawing.Point(0, 292);
      this.FootnoteAreaPanel.Size = new System.Drawing.Size(634, 0);
      // 
      // ContentAreaPanel
      // 
      this.ContentAreaPanel.Controls.Add(this.ConnectionResultLabel);
      this.ContentAreaPanel.Controls.Add(this.ConnectionResultPictureBox);
      this.ContentAreaPanel.Controls.Add(this.PasswordTextBox);
      this.ContentAreaPanel.Controls.Add(this.PasswordLabel);
      this.ContentAreaPanel.Controls.Add(this.RootUsernameLabel);
      this.ContentAreaPanel.Controls.Add(this.UsernameTextBox);
      this.ContentAreaPanel.Controls.Add(this.LogoPictureBox);
      this.ContentAreaPanel.Controls.Add(this.LoginInstructionsLabel);
      this.ContentAreaPanel.Controls.Add(this.LoginTitleLabel);
      this.ContentAreaPanel.Size = new System.Drawing.Size(443, 268);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.Controls.Add(this.TestConnectionButton);
      this.CommandAreaPanel.Controls.Add(this.OkButton);
      this.CommandAreaPanel.Controls.Add(this.DialogCancelButton);
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 223);
      this.CommandAreaPanel.Size = new System.Drawing.Size(443, 45);
      // 
      // OkButton
      // 
      this.OkButton.AccessibleDescription = "A button to accept the given credentials and close the dialog.";
      this.OkButton.AccessibleName = "OK";
      this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.OkButton.Location = new System.Drawing.Point(275, 10);
      this.OkButton.Name = "OkButton";
      this.OkButton.Size = new System.Drawing.Size(75, 23);
      this.OkButton.TabIndex = 0;
      this.OkButton.Text = "OK";
      this.OkButton.UseVisualStyleBackColor = true;
      // 
      // DialogCancelButton
      // 
      this.DialogCancelButton.AccessibleDescription = "A button to dismiss the dialog";
      this.DialogCancelButton.AccessibleName = "Cancel";
      this.DialogCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.DialogCancelButton.Location = new System.Drawing.Point(356, 10);
      this.DialogCancelButton.Name = "DialogCancelButton";
      this.DialogCancelButton.Size = new System.Drawing.Size(75, 23);
      this.DialogCancelButton.TabIndex = 1;
      this.DialogCancelButton.Text = "Cancel";
      this.DialogCancelButton.UseVisualStyleBackColor = true;
      // 
      // PasswordTextBox
      // 
      this.PasswordTextBox.AccessibleDescription = "A text box to input the My Oracle Support password";
      this.PasswordTextBox.AccessibleName = "Password";
      this.PasswordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.PasswordTextBox.Location = new System.Drawing.Point(171, 141);
      this.PasswordTextBox.Name = "PasswordTextBox";
      this.PasswordTextBox.Size = new System.Drawing.Size(260, 23);
      this.PasswordTextBox.TabIndex = 5;
      this.PasswordTextBox.UseSystemPasswordChar = true;
      // 
      // PasswordLabel
      // 
      this.PasswordLabel.AccessibleDescription = "A label displaying the text password";
      this.PasswordLabel.AccessibleName = "Password Text";
      this.PasswordLabel.AutoSize = true;
      this.PasswordLabel.Location = new System.Drawing.Point(105, 144);
      this.PasswordLabel.Name = "PasswordLabel";
      this.PasswordLabel.Size = new System.Drawing.Size(60, 15);
      this.PasswordLabel.TabIndex = 4;
      this.PasswordLabel.Text = "Password:";
      // 
      // RootUsernameLabel
      // 
      this.RootUsernameLabel.AccessibleDescription = "A label displaying the text root account user name";
      this.RootUsernameLabel.AccessibleName = "Root User Name Text";
      this.RootUsernameLabel.AutoSize = true;
      this.RootUsernameLabel.Location = new System.Drawing.Point(21, 114);
      this.RootUsernameLabel.Name = "RootUsernameLabel";
      this.RootUsernameLabel.Size = new System.Drawing.Size(144, 15);
      this.RootUsernameLabel.TabIndex = 2;
      this.RootUsernameLabel.Text = "Root Account User Name:";
      // 
      // UsernameTextBox
      // 
      this.UsernameTextBox.AccessibleDescription = "A text box to input the root account user name";
      this.UsernameTextBox.AccessibleName = "Root User Name";
      this.UsernameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.UsernameTextBox.Location = new System.Drawing.Point(171, 111);
      this.UsernameTextBox.Name = "UsernameTextBox";
      this.UsernameTextBox.Size = new System.Drawing.Size(260, 23);
      this.UsernameTextBox.TabIndex = 3;
      // 
      // LogoPictureBox
      // 
      this.LogoPictureBox.AccessibleDescription = "A picture box displaying a MySQL Server security logo";
      this.LogoPictureBox.AccessibleName = "Server Security Logo";
      this.LogoPictureBox.Image = global::MySql.Configurator.Properties.Resources.Server_Reflection;
      this.LogoPictureBox.Location = new System.Drawing.Point(20, 20);
      this.LogoPictureBox.Name = "LogoPictureBox";
      this.LogoPictureBox.Size = new System.Drawing.Size(59, 63);
      this.LogoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.LogoPictureBox.TabIndex = 9;
      this.LogoPictureBox.TabStop = false;
      // 
      // LoginInstructionsLabel
      // 
      this.LoginInstructionsLabel.AccessibleDescription = "A label displaying instructions to log into the MySQL Server";
      this.LoginInstructionsLabel.AccessibleName = "Login Instructions Text";
      this.LoginInstructionsLabel.AutoSize = true;
      this.LoginInstructionsLabel.Location = new System.Drawing.Point(91, 68);
      this.LoginInstructionsLabel.Name = "LoginInstructionsLabel";
      this.LoginInstructionsLabel.Size = new System.Drawing.Size(238, 15);
      this.LoginInstructionsLabel.TabIndex = 1;
      this.LoginInstructionsLabel.Text = "Login as a user with root account privileges.";
      // 
      // LoginTitleLabel
      // 
      this.LoginTitleLabel.AccessibleDescription = "A label displaying a title for the dialog";
      this.LoginTitleLabel.AccessibleName = "Title Text";
      this.LoginTitleLabel.AutoSize = true;
      this.LoginTitleLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.LoginTitleLabel.ForeColor = System.Drawing.Color.Navy;
      this.LoginTitleLabel.Location = new System.Drawing.Point(90, 20);
      this.LoginTitleLabel.Name = "LoginTitleLabel";
      this.LoginTitleLabel.Size = new System.Drawing.Size(277, 20);
      this.LoginTitleLabel.TabIndex = 0;
      this.LoginTitleLabel.Text = "Please provide a MySQL Server 5.5 Login";
      // 
      // ConnectionResultPictureBox
      // 
      this.ConnectionResultPictureBox.AccessibleDescription = "An icon representing the result of testing a connection with the given credential" +
    "s.";
      this.ConnectionResultPictureBox.AccessibleName = "Connection Result Icon";
      this.ConnectionResultPictureBox.Image = global::MySql.Configurator.Properties.Resources.ok_sign;
      this.ConnectionResultPictureBox.Location = new System.Drawing.Point(171, 170);
      this.ConnectionResultPictureBox.Name = "ConnectionResultPictureBox";
      this.ConnectionResultPictureBox.Size = new System.Drawing.Size(16, 16);
      this.ConnectionResultPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.ConnectionResultPictureBox.TabIndex = 10;
      this.ConnectionResultPictureBox.TabStop = false;
      this.ConnectionResultPictureBox.Visible = false;
      // 
      // ConnectionResultLabel
      // 
      this.ConnectionResultLabel.AccessibleDescription = "The result of testing a connection with the given credentials.";
      this.ConnectionResultLabel.AccessibleName = "Connection Result";
      this.ConnectionResultLabel.AutoSize = true;
      this.ConnectionResultLabel.Location = new System.Drawing.Point(193, 171);
      this.ConnectionResultLabel.Name = "ConnectionResultLabel";
      this.ConnectionResultLabel.Size = new System.Drawing.Size(110, 15);
      this.ConnectionResultLabel.TabIndex = 6;
      this.ConnectionResultLabel.Text = "Connection result...";
      this.ConnectionResultLabel.Visible = false;
      // 
      // TestConnectionButton
      // 
      this.TestConnectionButton.AccessibleDescription = "A button to perform a connection test with the specified credentials.";
      this.TestConnectionButton.AccessibleName = "Test Connection";
      this.TestConnectionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.TestConnectionButton.Location = new System.Drawing.Point(12, 10);
      this.TestConnectionButton.Name = "TestConnectionButton";
      this.TestConnectionButton.Size = new System.Drawing.Size(128, 23);
      this.TestConnectionButton.TabIndex = 2;
      this.TestConnectionButton.Text = "Test Connection";
      this.TestConnectionButton.UseVisualStyleBackColor = true;
      this.TestConnectionButton.Click += new System.EventHandler(this.TestConnectionButton_Click);
      // 
      // RootPasswordPromptDialog
      // 
      this.AcceptButton = this.OkButton;
      this.AccessibleDescription = "A modal dialog to input MySQL Server root credentials";
      this.AccessibleName = "Root Password Prompt Dialog";
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
      this.CancelButton = this.DialogCancelButton;
      this.ClientSize = new System.Drawing.Size(443, 268);
      this.CommandAreaVisible = true;
      this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FootnoteAreaHeight = 0;
      this.Name = "RootPasswordPromptDialog";
      this.Text = "Root User Login";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RootPasswordPromptDialog_FormClosing);
      this.ContentAreaPanel.ResumeLayout(false);
      this.ContentAreaPanel.PerformLayout();
      this.CommandAreaPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.ConnectionResultPictureBox)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button OkButton;
    private System.Windows.Forms.Button DialogCancelButton;
    private System.Windows.Forms.TextBox PasswordTextBox;
    private System.Windows.Forms.Label PasswordLabel;
    private System.Windows.Forms.Label RootUsernameLabel;
    private System.Windows.Forms.TextBox UsernameTextBox;
    private System.Windows.Forms.PictureBox LogoPictureBox;
    private System.Windows.Forms.Label LoginInstructionsLabel;
    private System.Windows.Forms.Label LoginTitleLabel;
    private System.Windows.Forms.Label ConnectionResultLabel;
    private System.Windows.Forms.PictureBox ConnectionResultPictureBox;
    private System.Windows.Forms.Button TestConnectionButton;
  }
}

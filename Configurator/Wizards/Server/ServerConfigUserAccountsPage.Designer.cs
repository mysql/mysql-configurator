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

using MySql.Configurator.Core.Controls;

namespace MySql.Configurator.Wizards.Server
{
  partial class ServerConfigUserAccountsPage
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerConfigUserAccountsPage));
      this.UserAccountsListView = new System.Windows.Forms.ListView();
      this.UserTypeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.UserNameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.FromHostColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.UserRoleColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.UserTypesImageList = new System.Windows.Forms.ImageList(this.components);
      this.AddUserButton = new System.Windows.Forms.Button();
      this.DeleteUserButton = new System.Windows.Forms.Button();
      this.EditUserButton = new System.Windows.Forms.Button();
      this.RootAccountDescriptionLabel = new System.Windows.Forms.Label();
      this.PasswordsTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
      this.PasswordStrengthLabel = new MySql.Configurator.Core.Controls.PasswordStrengthLabel();
      this.RootPasswordLabel = new System.Windows.Forms.Label();
      this.CurrentRootPasswordLabel = new System.Windows.Forms.Label();
      this.RootPasswordTextBox = new System.Windows.Forms.TextBox();
      this.RepeatPasswordLabel = new System.Windows.Forms.Label();
      this.RepeatPasswordTextBox = new System.Windows.Forms.TextBox();
      this.CurrentRootPasswordTextBox = new System.Windows.Forms.TextBox();
      this.PasswordCheckButton = new System.Windows.Forms.Button();
      this.RootAccountPasswordLabel = new System.Windows.Forms.Label();
      this.UserAccountsDescriptionLabel = new System.Windows.Forms.Label();
      this.UserAccountsLabel = new System.Windows.Forms.Label();
      this.UserAccountPanel = new System.Windows.Forms.Panel();
      this.ConnectionErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
      this.RolesDefinedErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
      this.PasswordsTableLayoutPanel.SuspendLayout();
      this.UserAccountPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ConnectionErrorProvider)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.RolesDefinedErrorProvider)).BeginInit();
      this.SuspendLayout();
      // 
      // subCaptionLabel
      // 
      this.subCaptionLabel.Location = new System.Drawing.Point(24, 66);
      this.subCaptionLabel.Size = new System.Drawing.Size(524, 19);
      this.subCaptionLabel.Text = "Please use this dialog to add MySQL User accounts to the new server installation." +
    "";
      this.subCaptionLabel.Visible = false;
      // 
      // captionLabel
      // 
      this.captionLabel.Size = new System.Drawing.Size(175, 25);
      this.captionLabel.Text = "Accounts and Roles";
      // 
      // UserAccountsListView
      // 
      this.UserAccountsListView.AccessibleDescription = "A list view containing a list of user accounts to be created in the server";
      this.UserAccountsListView.AccessibleName = "User Accounts List";
      this.UserAccountsListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.UserAccountsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.UserTypeColumnHeader,
            this.UserNameColumnHeader,
            this.FromHostColumnHeader,
            this.UserRoleColumnHeader});
      this.UserAccountsListView.FullRowSelect = true;
      this.UserAccountsListView.HideSelection = false;
      this.UserAccountsListView.Location = new System.Drawing.Point(1, 56);
      this.UserAccountsListView.MultiSelect = false;
      this.UserAccountsListView.Name = "UserAccountsListView";
      this.UserAccountsListView.ShowItemToolTips = true;
      this.UserAccountsListView.Size = new System.Drawing.Size(433, 172);
      this.UserAccountsListView.StateImageList = this.UserTypesImageList;
      this.UserAccountsListView.TabIndex = 2;
      this.UserAccountsListView.UseCompatibleStateImageBehavior = false;
      this.UserAccountsListView.View = System.Windows.Forms.View.Details;
      this.UserAccountsListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.UserAccountsListView_ColumnClick);
      this.UserAccountsListView.SelectedIndexChanged += new System.EventHandler(this.UserAccountsListView_SelectedIndexChanged);
      this.UserAccountsListView.DoubleClick += new System.EventHandler(this.UserAccountsListView_DoubleClick);
      // 
      // UserTypeColumnHeader
      // 
      this.UserTypeColumnHeader.Text = "";
      this.UserTypeColumnHeader.Width = 28;
      // 
      // UserNameColumnHeader
      // 
      this.UserNameColumnHeader.Text = "MySQL User Name";
      this.UserNameColumnHeader.Width = 150;
      // 
      // FromHostColumnHeader
      // 
      this.FromHostColumnHeader.Text = "Host";
      this.FromHostColumnHeader.Width = 89;
      // 
      // UserRoleColumnHeader
      // 
      this.UserRoleColumnHeader.Text = "User Role";
      this.UserRoleColumnHeader.Width = 120;
      // 
      // UserTypesImageList
      // 
      this.UserTypesImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("UserTypesImageList.ImageStream")));
      this.UserTypesImageList.TransparentColor = System.Drawing.Color.Transparent;
      this.UserTypesImageList.Images.SetKeyName(0, "ConfigUserMySQL24.png");
      this.UserTypesImageList.Images.SetKeyName(1, "ConfigUserWindows24.png");
      // 
      // AddUserButton
      // 
      this.AddUserButton.AccessibleDescription = "A button to add a new database user";
      this.AddUserButton.AccessibleName = "Add User";
      this.AddUserButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.AddUserButton.BackColor = System.Drawing.SystemColors.Control;
      this.AddUserButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.AddUserButton.Location = new System.Drawing.Point(440, 56);
      this.AddUserButton.Name = "AddUserButton";
      this.AddUserButton.Size = new System.Drawing.Size(72, 26);
      this.AddUserButton.TabIndex = 3;
      this.AddUserButton.Text = "&Add User";
      this.AddUserButton.UseVisualStyleBackColor = false;
      this.AddUserButton.Click += new System.EventHandler(this.AddUserButton_Click);
      // 
      // DeleteUserButton
      // 
      this.DeleteUserButton.AccessibleDescription = "A button to remove a database user from the list";
      this.DeleteUserButton.AccessibleName = "Delete User";
      this.DeleteUserButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.DeleteUserButton.BackColor = System.Drawing.SystemColors.Control;
      this.DeleteUserButton.Enabled = false;
      this.DeleteUserButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.DeleteUserButton.Location = new System.Drawing.Point(440, 120);
      this.DeleteUserButton.Name = "DeleteUserButton";
      this.DeleteUserButton.Size = new System.Drawing.Size(72, 26);
      this.DeleteUserButton.TabIndex = 5;
      this.DeleteUserButton.Text = "&Delete User";
      this.DeleteUserButton.UseVisualStyleBackColor = false;
      this.DeleteUserButton.Click += new System.EventHandler(this.DeleteUserButton_Click);
      // 
      // EditUserButton
      // 
      this.EditUserButton.AccessibleDescription = "A button to edit the information of a database user";
      this.EditUserButton.AccessibleName = "Edit User";
      this.EditUserButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.EditUserButton.BackColor = System.Drawing.SystemColors.Control;
      this.EditUserButton.Enabled = false;
      this.EditUserButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.EditUserButton.Location = new System.Drawing.Point(440, 88);
      this.EditUserButton.Name = "EditUserButton";
      this.EditUserButton.Size = new System.Drawing.Size(72, 26);
      this.EditUserButton.TabIndex = 4;
      this.EditUserButton.Text = "&Edit User";
      this.EditUserButton.UseVisualStyleBackColor = false;
      this.EditUserButton.Click += new System.EventHandler(this.EditUserButton_Click);
      // 
      // RootAccountDescriptionLabel
      // 
      this.RootAccountDescriptionLabel.AccessibleDescription = "A label displaying instructions about the configuration page";
      this.RootAccountDescriptionLabel.AccessibleName = "Root Account Instructions";
      this.RootAccountDescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.RootAccountDescriptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.RootAccountDescriptionLabel.Location = new System.Drawing.Point(24, 81);
      this.RootAccountDescriptionLabel.Name = "RootAccountDescriptionLabel";
      this.RootAccountDescriptionLabel.Size = new System.Drawing.Size(521, 31);
      this.RootAccountDescriptionLabel.TabIndex = 3;
      this.RootAccountDescriptionLabel.Text = "Enter the password for the root account.  Please remember to store this password " +
    "in a secure place.";
      // 
      // PasswordsTableLayoutPanel
      // 
      this.PasswordsTableLayoutPanel.AccessibleDescription = "A table layout panel containing all password related controls";
      this.PasswordsTableLayoutPanel.AccessibleName = "Password Layout";
      this.PasswordsTableLayoutPanel.AutoSize = true;
      this.PasswordsTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.PasswordsTableLayoutPanel.ColumnCount = 4;
      this.PasswordsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
      this.PasswordsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.PasswordsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
      this.PasswordsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
      this.PasswordsTableLayoutPanel.Controls.Add(this.PasswordStrengthLabel, 1, 4);
      this.PasswordsTableLayoutPanel.Controls.Add(this.RootPasswordLabel, 0, 1);
      this.PasswordsTableLayoutPanel.Controls.Add(this.CurrentRootPasswordLabel, 0, 0);
      this.PasswordsTableLayoutPanel.Controls.Add(this.RootPasswordTextBox, 1, 1);
      this.PasswordsTableLayoutPanel.Controls.Add(this.RepeatPasswordLabel, 0, 2);
      this.PasswordsTableLayoutPanel.Controls.Add(this.RepeatPasswordTextBox, 1, 2);
      this.PasswordsTableLayoutPanel.Controls.Add(this.CurrentRootPasswordTextBox, 1, 0);
      this.PasswordsTableLayoutPanel.Controls.Add(this.PasswordCheckButton, 2, 0);
      this.PasswordsTableLayoutPanel.Location = new System.Drawing.Point(27, 113);
      this.PasswordsTableLayoutPanel.Name = "PasswordsTableLayoutPanel";
      this.PasswordsTableLayoutPanel.RowCount = 5;
      this.PasswordsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.PasswordsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.PasswordsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.PasswordsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.PasswordsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
      this.PasswordsTableLayoutPanel.Size = new System.Drawing.Size(461, 107);
      this.PasswordsTableLayoutPanel.TabIndex = 44;
      // 
      // PasswordStrengthLabel
      // 
      this.PasswordStrengthLabel.AccessibleDescription = "A control showing the estimated password strength";
      this.PasswordStrengthLabel.AccessibleName = "Password Strength";
      this.PasswordStrengthLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PasswordStrengthLabel.Location = new System.Drawing.Point(153, 90);
      this.PasswordStrengthLabel.Name = "PasswordStrengthLabel";
      this.PasswordStrengthLabel.Size = new System.Drawing.Size(200, 13);
      this.PasswordStrengthLabel.TabIndex = 7;
      // 
      // RootPasswordLabel
      // 
      this.RootPasswordLabel.AccessibleDescription = "A label displaying the text MySQL root password";
      this.RootPasswordLabel.AccessibleName = "Root Password Text";
      this.RootPasswordLabel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.RootPasswordLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.RootPasswordLabel.Location = new System.Drawing.Point(0, 29);
      this.RootPasswordLabel.Margin = new System.Windows.Forms.Padding(0);
      this.RootPasswordLabel.Name = "RootPasswordLabel";
      this.RootPasswordLabel.Size = new System.Drawing.Size(150, 29);
      this.RootPasswordLabel.TabIndex = 3;
      this.RootPasswordLabel.Text = "MySQL Root Password:";
      this.RootPasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // CurrentRootPasswordLabel
      // 
      this.CurrentRootPasswordLabel.AccessibleDescription = "A label displaying the text current root password";
      this.CurrentRootPasswordLabel.AccessibleName = "Current Root Password Text";
      this.CurrentRootPasswordLabel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.CurrentRootPasswordLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.CurrentRootPasswordLabel.Location = new System.Drawing.Point(0, 0);
      this.CurrentRootPasswordLabel.Margin = new System.Windows.Forms.Padding(0);
      this.CurrentRootPasswordLabel.Name = "CurrentRootPasswordLabel";
      this.CurrentRootPasswordLabel.Size = new System.Drawing.Size(150, 29);
      this.CurrentRootPasswordLabel.TabIndex = 0;
      this.CurrentRootPasswordLabel.Text = "Current Root Password:";
      this.CurrentRootPasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.CurrentRootPasswordLabel.Visible = false;
      // 
      // RootPasswordTextBox
      // 
      this.RootPasswordTextBox.AccessibleDescription = "A text box to input the MySQL root account password";
      this.RootPasswordTextBox.AccessibleName = "Root Password";
      this.PasswordsTableLayoutPanel.SetColumnSpan(this.RootPasswordTextBox, 2);
      this.RootPasswordTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.RootPasswordTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.RootPasswordTextBox.Location = new System.Drawing.Point(153, 32);
      this.RootPasswordTextBox.Name = "RootPasswordTextBox";
      this.RootPasswordTextBox.Size = new System.Drawing.Size(280, 23);
      this.RootPasswordTextBox.TabIndex = 4;
      this.RootPasswordTextBox.UseSystemPasswordChar = true;
      this.RootPasswordTextBox.TextChanged += new System.EventHandler(this.TextChangedHandler);
      this.RootPasswordTextBox.Validated += new System.EventHandler(this.ValidatedHandler);
      // 
      // RepeatPasswordLabel
      // 
      this.RepeatPasswordLabel.AccessibleDescription = "A label displaying the text repeat password";
      this.RepeatPasswordLabel.AccessibleName = "Repeat Password Text";
      this.RepeatPasswordLabel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.RepeatPasswordLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.RepeatPasswordLabel.Location = new System.Drawing.Point(0, 58);
      this.RepeatPasswordLabel.Margin = new System.Windows.Forms.Padding(0);
      this.RepeatPasswordLabel.Name = "RepeatPasswordLabel";
      this.RepeatPasswordLabel.Size = new System.Drawing.Size(150, 29);
      this.RepeatPasswordLabel.TabIndex = 5;
      this.RepeatPasswordLabel.Text = "Repeat Password:";
      this.RepeatPasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // RepeatPasswordTextBox
      // 
      this.RepeatPasswordTextBox.AccessibleDescription = "A text box to input the password confirmation";
      this.RepeatPasswordTextBox.AccessibleName = "Password Confirmation";
      this.PasswordsTableLayoutPanel.SetColumnSpan(this.RepeatPasswordTextBox, 2);
      this.RepeatPasswordTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.RepeatPasswordTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.RepeatPasswordTextBox.Location = new System.Drawing.Point(153, 61);
      this.RepeatPasswordTextBox.Name = "RepeatPasswordTextBox";
      this.RepeatPasswordTextBox.Size = new System.Drawing.Size(280, 23);
      this.RepeatPasswordTextBox.TabIndex = 6;
      this.RepeatPasswordTextBox.UseSystemPasswordChar = true;
      this.RepeatPasswordTextBox.TextChanged += new System.EventHandler(this.TextChangedHandler);
      this.RepeatPasswordTextBox.Validated += new System.EventHandler(this.ValidatedHandler);
      // 
      // CurrentRootPasswordTextBox
      // 
      this.CurrentRootPasswordTextBox.AccessibleDescription = "A text box to input the current root password";
      this.CurrentRootPasswordTextBox.AccessibleName = "Current Root Password";
      this.CurrentRootPasswordTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.CurrentRootPasswordTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.CurrentRootPasswordTextBox.Location = new System.Drawing.Point(153, 3);
      this.CurrentRootPasswordTextBox.Name = "CurrentRootPasswordTextBox";
      this.CurrentRootPasswordTextBox.Size = new System.Drawing.Size(200, 23);
      this.CurrentRootPasswordTextBox.TabIndex = 1;
      this.CurrentRootPasswordTextBox.UseSystemPasswordChar = true;
      this.CurrentRootPasswordTextBox.Visible = false;
      this.CurrentRootPasswordTextBox.TextChanged += new System.EventHandler(this.TextChangedHandler);
      this.CurrentRootPasswordTextBox.Validated += new System.EventHandler(this.ValidatedHandler);
      // 
      // PasswordCheckButton
      // 
      this.PasswordCheckButton.AccessibleDescription = "A button to check the connection with the current root password";
      this.PasswordCheckButton.AccessibleName = "Root Password Check";
      this.PasswordCheckButton.Location = new System.Drawing.Point(359, 3);
      this.PasswordCheckButton.Name = "PasswordCheckButton";
      this.PasswordCheckButton.Size = new System.Drawing.Size(74, 23);
      this.PasswordCheckButton.TabIndex = 2;
      this.PasswordCheckButton.Text = "Check";
      this.PasswordCheckButton.UseVisualStyleBackColor = true;
      this.PasswordCheckButton.Visible = false;
      this.PasswordCheckButton.Click += new System.EventHandler(this.PasswordCheckButton_Click);
      // 
      // RootAccountPasswordLabel
      // 
      this.RootAccountPasswordLabel.AccessibleDescription = "A label displaying the text root account password";
      this.RootAccountPasswordLabel.AccessibleName = "Root Account Password Text";
      this.RootAccountPasswordLabel.AutoSize = true;
      this.RootAccountPasswordLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.RootAccountPasswordLabel.Location = new System.Drawing.Point(24, 66);
      this.RootAccountPasswordLabel.Name = "RootAccountPasswordLabel";
      this.RootAccountPasswordLabel.Size = new System.Drawing.Size(138, 15);
      this.RootAccountPasswordLabel.TabIndex = 2;
      this.RootAccountPasswordLabel.Text = "Root Account Password";
      // 
      // UserAccountsDescriptionLabel
      // 
      this.UserAccountsDescriptionLabel.AccessibleDescription = "A label displaying a description about user accounts";
      this.UserAccountsDescriptionLabel.AccessibleName = "User Accounts Description";
      this.UserAccountsDescriptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.UserAccountsDescriptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.UserAccountsDescriptionLabel.Location = new System.Drawing.Point(1, 22);
      this.UserAccountsDescriptionLabel.Name = "UserAccountsDescriptionLabel";
      this.UserAccountsDescriptionLabel.Size = new System.Drawing.Size(511, 31);
      this.UserAccountsDescriptionLabel.TabIndex = 1;
      this.UserAccountsDescriptionLabel.Text = "Create MySQL user accounts for your users and applications. Assign a role to the " +
    "user that consists of a set of privileges.";
      // 
      // UserAccountsLabel
      // 
      this.UserAccountsLabel.AccessibleDescription = "A label displaying the text MySQL user accounts";
      this.UserAccountsLabel.AccessibleName = "User Accounts Text";
      this.UserAccountsLabel.AutoSize = true;
      this.UserAccountsLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.UserAccountsLabel.Location = new System.Drawing.Point(1, 1);
      this.UserAccountsLabel.Name = "UserAccountsLabel";
      this.UserAccountsLabel.Size = new System.Drawing.Size(129, 15);
      this.UserAccountsLabel.TabIndex = 0;
      this.UserAccountsLabel.Text = "MySQL User Accounts";
      // 
      // UserAccountPanel
      // 
      this.UserAccountPanel.AccessibleDescription = "A panel containing controls related to user accounts";
      this.UserAccountPanel.AccessibleName = "User Accounts Group";
      this.UserAccountPanel.Controls.Add(this.UserAccountsLabel);
      this.UserAccountPanel.Controls.Add(this.UserAccountsListView);
      this.UserAccountPanel.Controls.Add(this.DeleteUserButton);
      this.UserAccountPanel.Controls.Add(this.AddUserButton);
      this.UserAccountPanel.Controls.Add(this.EditUserButton);
      this.UserAccountPanel.Controls.Add(this.UserAccountsDescriptionLabel);
      this.UserAccountPanel.Location = new System.Drawing.Point(30, 271);
      this.UserAccountPanel.Name = "UserAccountPanel";
      this.UserAccountPanel.Size = new System.Drawing.Size(513, 229);
      this.UserAccountPanel.TabIndex = 4;
      // 
      // ConnectionErrorProvider
      // 
      this.ConnectionErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
      this.ConnectionErrorProvider.ContainerControl = this;
      // 
      // RolesDefinedErrorProvider
      // 
      this.RolesDefinedErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
      this.RolesDefinedErrorProvider.ContainerControl = this;
      // 
      // ServerConfigUserAccountsPage
      // 
      this.AccessibleDescription = "A configuration wizard page to add user accounts";
      this.AccessibleName = "Accounts And Roles Page";
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
      this.Caption = "Accounts and Roles";
      this.Controls.Add(this.RootAccountDescriptionLabel);
      this.Controls.Add(this.RootAccountPasswordLabel);
      this.Controls.Add(this.PasswordsTableLayoutPanel);
      this.Controls.Add(this.UserAccountPanel);
      this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "ServerConfigUserAccountsPage";
      this.SubCaption = "Please use this dialog to add MySQL User accounts to the new server installation." +
    "";
      this.Controls.SetChildIndex(this.UserAccountPanel, 0);
      this.Controls.SetChildIndex(this.PasswordsTableLayoutPanel, 0);
      this.Controls.SetChildIndex(this.RootAccountPasswordLabel, 0);
      this.Controls.SetChildIndex(this.RootAccountDescriptionLabel, 0);
      this.Controls.SetChildIndex(this.subCaptionLabel, 0);
      this.Controls.SetChildIndex(this.captionLabel, 0);
      this.PasswordsTableLayoutPanel.ResumeLayout(false);
      this.PasswordsTableLayoutPanel.PerformLayout();
      this.UserAccountPanel.ResumeLayout(false);
      this.UserAccountPanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ConnectionErrorProvider)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.RolesDefinedErrorProvider)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ListView UserAccountsListView;
    private System.Windows.Forms.ColumnHeader UserNameColumnHeader;
    private System.Windows.Forms.ColumnHeader UserRoleColumnHeader;
    private System.Windows.Forms.Button AddUserButton;
    private System.Windows.Forms.Button DeleteUserButton;
    private System.Windows.Forms.Button EditUserButton;
    private System.Windows.Forms.ColumnHeader UserTypeColumnHeader;
    private System.Windows.Forms.ImageList UserTypesImageList;
    private System.Windows.Forms.Label RootAccountDescriptionLabel;
    private System.Windows.Forms.TableLayoutPanel PasswordsTableLayoutPanel;
    private System.Windows.Forms.Label RootPasswordLabel;
    private System.Windows.Forms.Label CurrentRootPasswordLabel;
    private System.Windows.Forms.TextBox RootPasswordTextBox;
    private System.Windows.Forms.Label RepeatPasswordLabel;
    private System.Windows.Forms.TextBox RepeatPasswordTextBox;
    private System.Windows.Forms.TextBox CurrentRootPasswordTextBox;
    private System.Windows.Forms.Label RootAccountPasswordLabel;
    private System.Windows.Forms.Label UserAccountsDescriptionLabel;
    private System.Windows.Forms.Label UserAccountsLabel;
    private PasswordStrengthLabel PasswordStrengthLabel;
    private System.Windows.Forms.ColumnHeader FromHostColumnHeader;
    private System.Windows.Forms.Button PasswordCheckButton;
    private System.Windows.Forms.Panel UserAccountPanel;
    private System.Windows.Forms.ErrorProvider ConnectionErrorProvider;
    private System.Windows.Forms.ErrorProvider RolesDefinedErrorProvider;
  }
}

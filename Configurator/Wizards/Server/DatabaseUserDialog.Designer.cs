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

using MySql.Configurator.Core.Controls;

namespace MySql.Configurator.Wizards.Server
{
  partial class DatabaseUserDialog
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseUserDialog));
      this.MainPanel = new System.Windows.Forms.Panel();
      this.MySqlAuthenticationGroupBox = new System.Windows.Forms.GroupBox();
      this.PasswordStrengthLabel = new MySql.Configurator.Core.Controls.PasswordStrengthLabel();
      this.PasswordLabel = new System.Windows.Forms.Label();
      this.ConfirmPasswordTextBox = new System.Windows.Forms.TextBox();
      this.PasswordTextBox = new System.Windows.Forms.TextBox();
      this.ConfirmPasswordLabel = new System.Windows.Forms.Label();
      this.HostComboBox = new System.Windows.Forms.ComboBox();
      this.HostLabel = new System.Windows.Forms.Label();
      this.UserRoleComboBox = new MySql.Configurator.Core.Controls.ImageComboBox();
      this.WindowsAuthenticationRadioButton = new System.Windows.Forms.RadioButton();
      this.MySqlAuthenticationRadioButton = new System.Windows.Forms.RadioButton();
      this.AuthenticationTypeLabel = new System.Windows.Forms.Label();
      this.CaptionLabel = new System.Windows.Forms.Label();
      this.UsernameTextBox = new System.Windows.Forms.TextBox();
      this.UserRoleLabel = new System.Windows.Forms.Label();
      this.UsernameLabel = new System.Windows.Forms.Label();
      this.MySqlAuthenticationPictureBox = new System.Windows.Forms.PictureBox();
      this.WindowsAuthenticationPictureBox = new System.Windows.Forms.PictureBox();
      this.WindowsAuthenticationGroupBox = new System.Windows.Forms.GroupBox();
      this.DomainAdministratorCredentialsHelpLabel = new System.Windows.Forms.Label();
      this.TestSecurityTokensButton = new System.Windows.Forms.Button();
      this.ActiveDirectoryValidationCheckBox = new System.Windows.Forms.CheckBox();
      this.DomainAdministratorPasswordLabel = new System.Windows.Forms.Label();
      this.DomainAdministratorConfirmPasswordLabel = new System.Windows.Forms.Label();
      this.DomainAdministratorPasswordTextBox = new System.Windows.Forms.TextBox();
      this.DomainAdministratorUserNameTextBox = new System.Windows.Forms.TextBox();
      this.WindowsTokensDocumentationLinkLabel = new System.Windows.Forms.LinkLabel();
      this.WindowsTokensHelpLabel = new System.Windows.Forms.Label();
      this.WindowsTokensRichTextBox = new System.Windows.Forms.RichTextBox();
      this.OkButton = new System.Windows.Forms.Button();
      this.DialogCancelButton = new System.Windows.Forms.Button();
      this.ValidationsTimer = new System.Windows.Forms.Timer(this.components);
      this.ValidationsErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
      this.HostErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
      this.UserPasswordToolTip = new System.Windows.Forms.ToolTip(this.components);
      this.EmptyPasswordWarningErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
      this.MainPanel.SuspendLayout();
      this.MySqlAuthenticationGroupBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.MySqlAuthenticationPictureBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.WindowsAuthenticationPictureBox)).BeginInit();
      this.WindowsAuthenticationGroupBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.HostErrorProvider)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.EmptyPasswordWarningErrorProvider)).BeginInit();
      this.SuspendLayout();
      // 
      // MainPanel
      // 
      this.MainPanel.AccessibleDescription = "A panel representing the area where the main controls are placed";
      this.MainPanel.AccessibleName = "Content Area";
      this.MainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.MainPanel.BackColor = System.Drawing.SystemColors.Window;
      this.MainPanel.Controls.Add(this.MySqlAuthenticationGroupBox);
      this.MainPanel.Controls.Add(this.HostComboBox);
      this.MainPanel.Controls.Add(this.HostLabel);
      this.MainPanel.Controls.Add(this.UserRoleComboBox);
      this.MainPanel.Controls.Add(this.WindowsAuthenticationRadioButton);
      this.MainPanel.Controls.Add(this.MySqlAuthenticationRadioButton);
      this.MainPanel.Controls.Add(this.AuthenticationTypeLabel);
      this.MainPanel.Controls.Add(this.CaptionLabel);
      this.MainPanel.Controls.Add(this.UsernameTextBox);
      this.MainPanel.Controls.Add(this.UserRoleLabel);
      this.MainPanel.Controls.Add(this.UsernameLabel);
      this.MainPanel.Controls.Add(this.MySqlAuthenticationPictureBox);
      this.MainPanel.Controls.Add(this.WindowsAuthenticationPictureBox);
      this.MainPanel.Controls.Add(this.WindowsAuthenticationGroupBox);
      this.MainPanel.Location = new System.Drawing.Point(1, 0);
      this.MainPanel.Name = "MainPanel";
      this.MainPanel.Size = new System.Drawing.Size(446, 463);
      this.MainPanel.TabIndex = 0;
      // 
      // MySqlAuthenticationGroupBox
      // 
      this.MySqlAuthenticationGroupBox.AccessibleDescription = "A group box containing controls related to MySQL authentication";
      this.MySqlAuthenticationGroupBox.AccessibleName = "MySQL Authentication";
      this.MySqlAuthenticationGroupBox.Controls.Add(this.PasswordStrengthLabel);
      this.MySqlAuthenticationGroupBox.Controls.Add(this.PasswordLabel);
      this.MySqlAuthenticationGroupBox.Controls.Add(this.ConfirmPasswordTextBox);
      this.MySqlAuthenticationGroupBox.Controls.Add(this.PasswordTextBox);
      this.MySqlAuthenticationGroupBox.Controls.Add(this.ConfirmPasswordLabel);
      this.MySqlAuthenticationGroupBox.Location = new System.Drawing.Point(23, 161);
      this.MySqlAuthenticationGroupBox.Name = "MySqlAuthenticationGroupBox";
      this.MySqlAuthenticationGroupBox.Size = new System.Drawing.Size(400, 100);
      this.MySqlAuthenticationGroupBox.TabIndex = 41;
      this.MySqlAuthenticationGroupBox.TabStop = false;
      this.MySqlAuthenticationGroupBox.Text = "MySQL user credentials";
      // 
      // PasswordStrengthLabel
      // 
      this.PasswordStrengthLabel.AccessibleDescription = "A control showing the estimated password strength";
      this.PasswordStrengthLabel.AccessibleName = "Password Strength";
      this.PasswordStrengthLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PasswordStrengthLabel.Location = new System.Drawing.Point(164, 76);
      this.PasswordStrengthLabel.Name = "PasswordStrengthLabel";
      this.PasswordStrengthLabel.Size = new System.Drawing.Size(200, 13);
      this.PasswordStrengthLabel.TabIndex = 9;
      // 
      // PasswordLabel
      // 
      this.PasswordLabel.AccessibleDescription = "A label displaying the text password";
      this.PasswordLabel.AccessibleName = "Password Text";
      this.PasswordLabel.AutoSize = true;
      this.PasswordLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PasswordLabel.Location = new System.Drawing.Point(98, 23);
      this.PasswordLabel.Name = "PasswordLabel";
      this.PasswordLabel.Size = new System.Drawing.Size(91, 25);
      this.PasswordLabel.TabIndex = 5;
      this.PasswordLabel.Text = "Password:";
      // 
      // ConfirmPasswordTextBox
      // 
      this.ConfirmPasswordTextBox.AccessibleDescription = "A text box to input a password confirmation";
      this.ConfirmPasswordTextBox.AccessibleName = "Password Confirmation";
      this.ConfirmPasswordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.ConfirmPasswordTextBox.Location = new System.Drawing.Point(164, 47);
      this.ConfirmPasswordTextBox.Name = "ConfirmPasswordTextBox";
      this.ConfirmPasswordTextBox.Size = new System.Drawing.Size(184, 31);
      this.ConfirmPasswordTextBox.TabIndex = 8;
      this.ConfirmPasswordTextBox.UseSystemPasswordChar = true;
      this.ConfirmPasswordTextBox.TextChanged += new System.EventHandler(this.TextChangedHandler);
      this.ConfirmPasswordTextBox.Validated += new System.EventHandler(this.TextBoxValidated);
      // 
      // PasswordTextBox
      // 
      this.PasswordTextBox.AccessibleDescription = "A text box to input the user password";
      this.PasswordTextBox.AccessibleName = "Password";
      this.PasswordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.PasswordTextBox.Location = new System.Drawing.Point(164, 20);
      this.PasswordTextBox.Name = "PasswordTextBox";
      this.PasswordTextBox.Size = new System.Drawing.Size(184, 31);
      this.PasswordTextBox.TabIndex = 6;
      this.PasswordTextBox.UseSystemPasswordChar = true;
      this.PasswordTextBox.TextChanged += new System.EventHandler(this.TextChangedHandler);
      this.PasswordTextBox.Validated += new System.EventHandler(this.TextBoxValidated);
      // 
      // ConfirmPasswordLabel
      // 
      this.ConfirmPasswordLabel.AccessibleDescription = "A label displaying the text confirm password";
      this.ConfirmPasswordLabel.AccessibleName = "Confirm Password Text";
      this.ConfirmPasswordLabel.AutoSize = true;
      this.ConfirmPasswordLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ConfirmPasswordLabel.Location = new System.Drawing.Point(51, 50);
      this.ConfirmPasswordLabel.Name = "ConfirmPasswordLabel";
      this.ConfirmPasswordLabel.Size = new System.Drawing.Size(160, 25);
      this.ConfirmPasswordLabel.TabIndex = 7;
      this.ConfirmPasswordLabel.Text = "Confirm Password:";
      // 
      // HostComboBox
      // 
      this.HostComboBox.AccessibleDescription = "A combo box to select or input the host or wildcard the user will be granted acce" +
    "ss to";
      this.HostComboBox.AccessibleName = "Host";
      this.HostComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.HostComboBox.FormattingEnabled = true;
      this.HostComboBox.Items.AddRange(new object[] {
            "<All Hosts (%)>",
            "localhost"});
      this.HostComboBox.Location = new System.Drawing.Point(195, 75);
      this.HostComboBox.Name = "HostComboBox";
      this.HostComboBox.Size = new System.Drawing.Size(205, 33);
      this.HostComboBox.TabIndex = 4;
      this.HostComboBox.SelectedIndexChanged += new System.EventHandler(this.HostComboBox_SelectedIndexChanged);
      this.HostComboBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.HostComboBox_KeyUp);
      this.HostComboBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.HostComboBox_PreviewKeyDown);
      this.HostComboBox.Validated += new System.EventHandler(this.HostComboBox_Validated);
      // 
      // HostLabel
      // 
      this.HostLabel.AccessibleDescription = "A label displaying the text host";
      this.HostLabel.AccessibleName = "Host Text";
      this.HostLabel.AutoSize = true;
      this.HostLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.HostLabel.Location = new System.Drawing.Point(154, 78);
      this.HostLabel.Name = "HostLabel";
      this.HostLabel.Size = new System.Drawing.Size(54, 25);
      this.HostLabel.TabIndex = 3;
      this.HostLabel.Text = "Host:";
      // 
      // UserRoleComboBox
      // 
      this.UserRoleComboBox.AccessibleDescription = "A combo box to select a predefined user role";
      this.UserRoleComboBox.AccessibleName = "Role";
      this.UserRoleComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.UserRoleComboBox.BackColor = System.Drawing.SystemColors.Window;
      this.UserRoleComboBox.DescriptionFont = new System.Drawing.Font("Segoe UI", 7F);
      this.UserRoleComboBox.DescriptionForeColor = System.Drawing.Color.Gray;
      this.UserRoleComboBox.DescriptionMargin = new System.Windows.Forms.Padding(5, 1, 3, 5);
      this.UserRoleComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
      this.UserRoleComboBox.DropDownHeight = 600;
      this.UserRoleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.UserRoleComboBox.DropDownWidth = 400;
      this.UserRoleComboBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.UserRoleComboBox.ForeColor = System.Drawing.SystemColors.WindowText;
      this.UserRoleComboBox.FormattingEnabled = true;
      this.UserRoleComboBox.ImageMargin = new System.Windows.Forms.Padding(3);
      this.UserRoleComboBox.IntegralHeight = false;
      this.UserRoleComboBox.Location = new System.Drawing.Point(195, 104);
      this.UserRoleComboBox.MaxDropDownItems = 10;
      this.UserRoleComboBox.Name = "UserRoleComboBox";
      this.UserRoleComboBox.Size = new System.Drawing.Size(205, 32);
      this.UserRoleComboBox.TabIndex = 6;
      this.UserRoleComboBox.TitleFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.UserRoleComboBox.TitleForeColor = System.Drawing.Color.Black;
      this.UserRoleComboBox.TitleMargin = new System.Windows.Forms.Padding(5);
      // 
      // WindowsAuthenticationRadioButton
      // 
      this.WindowsAuthenticationRadioButton.AccessibleDescription = "An option to choose a Windows authentication";
      this.WindowsAuthenticationRadioButton.AccessibleName = "Windows Authentication";
      this.WindowsAuthenticationRadioButton.AutoSize = true;
      this.WindowsAuthenticationRadioButton.Location = new System.Drawing.Point(285, 134);
      this.WindowsAuthenticationRadioButton.Name = "WindowsAuthenticationRadioButton";
      this.WindowsAuthenticationRadioButton.Size = new System.Drawing.Size(111, 29);
      this.WindowsAuthenticationRadioButton.TabIndex = 9;
      this.WindowsAuthenticationRadioButton.Text = "Windows";
      this.WindowsAuthenticationRadioButton.UseVisualStyleBackColor = true;
      this.WindowsAuthenticationRadioButton.CheckedChanged += new System.EventHandler(this.SwitchAuthTypes);
      // 
      // MySqlAuthenticationRadioButton
      // 
      this.MySqlAuthenticationRadioButton.AccessibleDescription = "An option to choose a MySQL authentication";
      this.MySqlAuthenticationRadioButton.AccessibleName = "MySQL Authentication";
      this.MySqlAuthenticationRadioButton.AutoSize = true;
      this.MySqlAuthenticationRadioButton.Location = new System.Drawing.Point(195, 134);
      this.MySqlAuthenticationRadioButton.Name = "MySqlAuthenticationRadioButton";
      this.MySqlAuthenticationRadioButton.Size = new System.Drawing.Size(94, 29);
      this.MySqlAuthenticationRadioButton.TabIndex = 8;
      this.MySqlAuthenticationRadioButton.Text = "MySQL";
      this.MySqlAuthenticationRadioButton.UseVisualStyleBackColor = true;
      this.MySqlAuthenticationRadioButton.CheckedChanged += new System.EventHandler(this.SwitchAuthTypes);
      // 
      // AuthenticationTypeLabel
      // 
      this.AuthenticationTypeLabel.AccessibleDescription = "A label displaying the text authentication";
      this.AuthenticationTypeLabel.AccessibleName = "Authentication Text";
      this.AuthenticationTypeLabel.AutoSize = true;
      this.AuthenticationTypeLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.AuthenticationTypeLabel.Location = new System.Drawing.Point(100, 138);
      this.AuthenticationTypeLabel.Name = "AuthenticationTypeLabel";
      this.AuthenticationTypeLabel.Size = new System.Drawing.Size(131, 25);
      this.AuthenticationTypeLabel.TabIndex = 7;
      this.AuthenticationTypeLabel.Text = "Authentication:";
      // 
      // CaptionLabel
      // 
      this.CaptionLabel.AccessibleDescription = "A label displaying a title for the dialog";
      this.CaptionLabel.AccessibleName = "Title";
      this.CaptionLabel.AutoSize = true;
      this.CaptionLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.CaptionLabel.ForeColor = System.Drawing.Color.Navy;
      this.CaptionLabel.Location = new System.Drawing.Point(11, 9);
      this.CaptionLabel.Name = "CaptionLabel";
      this.CaptionLabel.Size = new System.Drawing.Size(616, 31);
      this.CaptionLabel.TabIndex = 0;
      this.CaptionLabel.Text = "Please specify the user name, password, and database role.";
      // 
      // UsernameTextBox
      // 
      this.UsernameTextBox.AccessibleDescription = "A text box to input the user name";
      this.UsernameTextBox.AccessibleName = "User Name";
      this.UsernameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.UsernameTextBox.Location = new System.Drawing.Point(195, 46);
      this.UsernameTextBox.Name = "UsernameTextBox";
      this.UsernameTextBox.Size = new System.Drawing.Size(205, 31);
      this.UsernameTextBox.TabIndex = 2;
      this.UsernameTextBox.TextChanged += new System.EventHandler(this.TextChangedHandler);
      this.UsernameTextBox.Validated += new System.EventHandler(this.TextBoxValidated);
      // 
      // UserRoleLabel
      // 
      this.UserRoleLabel.AccessibleDescription = "A label displaying the text role";
      this.UserRoleLabel.AccessibleName = "Role Text";
      this.UserRoleLabel.AutoSize = true;
      this.UserRoleLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.UserRoleLabel.Location = new System.Drawing.Point(156, 107);
      this.UserRoleLabel.Name = "UserRoleLabel";
      this.UserRoleLabel.Size = new System.Drawing.Size(50, 25);
      this.UserRoleLabel.TabIndex = 5;
      this.UserRoleLabel.Text = "Role:";
      // 
      // UsernameLabel
      // 
      this.UsernameLabel.AccessibleDescription = "A label displaying the text user name";
      this.UsernameLabel.AccessibleName = "User Name Text";
      this.UsernameLabel.AutoSize = true;
      this.UsernameLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.UsernameLabel.Location = new System.Drawing.Point(123, 49);
      this.UsernameLabel.Name = "UsernameLabel";
      this.UsernameLabel.Size = new System.Drawing.Size(103, 25);
      this.UsernameLabel.TabIndex = 1;
      this.UsernameLabel.Text = "User Name:";
      // 
      // MySqlAuthenticationPictureBox
      // 
      this.MySqlAuthenticationPictureBox.AccessibleDescription = "A picture box displaying a secure MySQL Server logo";
      this.MySqlAuthenticationPictureBox.AccessibleName = "MySQL Authentication Logo";
      this.MySqlAuthenticationPictureBox.Image = global::MySql.Configurator.Properties.Resources.Server_Reflection;
      this.MySqlAuthenticationPictureBox.Location = new System.Drawing.Point(31, 46);
      this.MySqlAuthenticationPictureBox.Name = "MySqlAuthenticationPictureBox";
      this.MySqlAuthenticationPictureBox.Size = new System.Drawing.Size(59, 63);
      this.MySqlAuthenticationPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.MySqlAuthenticationPictureBox.TabIndex = 3;
      this.MySqlAuthenticationPictureBox.TabStop = false;
      // 
      // WindowsAuthenticationPictureBox
      // 
      this.WindowsAuthenticationPictureBox.AccessibleDescription = "A picture box displaying a secure Windows logo";
      this.WindowsAuthenticationPictureBox.AccessibleName = "Windows Authentication Logo";
      this.WindowsAuthenticationPictureBox.Image = global::MySql.Configurator.Properties.Resources.config_win_integration;
      this.WindowsAuthenticationPictureBox.Location = new System.Drawing.Point(31, 46);
      this.WindowsAuthenticationPictureBox.Name = "WindowsAuthenticationPictureBox";
      this.WindowsAuthenticationPictureBox.Size = new System.Drawing.Size(56, 43);
      this.WindowsAuthenticationPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.WindowsAuthenticationPictureBox.TabIndex = 37;
      this.WindowsAuthenticationPictureBox.TabStop = false;
      // 
      // WindowsAuthenticationGroupBox
      // 
      this.WindowsAuthenticationGroupBox.AccessibleDescription = "A group box containing controls related to Windows authentication";
      this.WindowsAuthenticationGroupBox.AccessibleName = "Windows Authentication";
      this.WindowsAuthenticationGroupBox.Controls.Add(this.DomainAdministratorCredentialsHelpLabel);
      this.WindowsAuthenticationGroupBox.Controls.Add(this.TestSecurityTokensButton);
      this.WindowsAuthenticationGroupBox.Controls.Add(this.ActiveDirectoryValidationCheckBox);
      this.WindowsAuthenticationGroupBox.Controls.Add(this.DomainAdministratorPasswordLabel);
      this.WindowsAuthenticationGroupBox.Controls.Add(this.DomainAdministratorConfirmPasswordLabel);
      this.WindowsAuthenticationGroupBox.Controls.Add(this.DomainAdministratorPasswordTextBox);
      this.WindowsAuthenticationGroupBox.Controls.Add(this.DomainAdministratorUserNameTextBox);
      this.WindowsAuthenticationGroupBox.Controls.Add(this.WindowsTokensDocumentationLinkLabel);
      this.WindowsAuthenticationGroupBox.Controls.Add(this.WindowsTokensHelpLabel);
      this.WindowsAuthenticationGroupBox.Controls.Add(this.WindowsTokensRichTextBox);
      this.WindowsAuthenticationGroupBox.Location = new System.Drawing.Point(23, 161);
      this.WindowsAuthenticationGroupBox.Name = "WindowsAuthenticationGroupBox";
      this.WindowsAuthenticationGroupBox.Size = new System.Drawing.Size(400, 285);
      this.WindowsAuthenticationGroupBox.TabIndex = 42;
      this.WindowsAuthenticationGroupBox.TabStop = false;
      this.WindowsAuthenticationGroupBox.Text = "Windows Security Tokens";
      // 
      // DomainAdministratorCredentialsHelpLabel
      // 
      this.DomainAdministratorCredentialsHelpLabel.AccessibleDescription = "A label displaying text about when it is required to provide domain administrator" +
    " credentials";
      this.DomainAdministratorCredentialsHelpLabel.AccessibleName = "Domain Administrator Credentials Help Text";
      this.DomainAdministratorCredentialsHelpLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DomainAdministratorCredentialsHelpLabel.Location = new System.Drawing.Point(19, 105);
      this.DomainAdministratorCredentialsHelpLabel.Name = "DomainAdministratorCredentialsHelpLabel";
      this.DomainAdministratorCredentialsHelpLabel.Size = new System.Drawing.Size(353, 60);
      this.DomainAdministratorCredentialsHelpLabel.TabIndex = 27;
      this.DomainAdministratorCredentialsHelpLabel.Text = resources.GetString("DomainAdministratorCredentialsHelpLabel.Text");
      // 
      // TestSecurityTokensButton
      // 
      this.TestSecurityTokensButton.AccessibleDescription = "A button to validate the security tokens";
      this.TestSecurityTokensButton.AccessibleName = "Test Security Tokens";
      this.TestSecurityTokensButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.TestSecurityTokensButton.Location = new System.Drawing.Point(227, 252);
      this.TestSecurityTokensButton.Name = "TestSecurityTokensButton";
      this.TestSecurityTokensButton.Size = new System.Drawing.Size(145, 23);
      this.TestSecurityTokensButton.TabIndex = 26;
      this.TestSecurityTokensButton.Text = "Test Security Tokens";
      this.TestSecurityTokensButton.UseVisualStyleBackColor = true;
      this.TestSecurityTokensButton.Click += new System.EventHandler(this.TestSecurityTokensButton_Click);
      // 
      // ActiveDirectoryValidationCheckBox
      // 
      this.ActiveDirectoryValidationCheckBox.AccessibleDescription = "A check box to enable the active directory user validation with different credent" +
    "ials";
      this.ActiveDirectoryValidationCheckBox.AccessibleName = "Active Directory User Validation";
      this.ActiveDirectoryValidationCheckBox.AutoSize = true;
      this.ActiveDirectoryValidationCheckBox.Checked = true;
      this.ActiveDirectoryValidationCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
      this.ActiveDirectoryValidationCheckBox.Location = new System.Drawing.Point(22, 171);
      this.ActiveDirectoryValidationCheckBox.Name = "ActiveDirectoryValidationCheckBox";
      this.ActiveDirectoryValidationCheckBox.Size = new System.Drawing.Size(318, 29);
      this.ActiveDirectoryValidationCheckBox.TabIndex = 25;
      this.ActiveDirectoryValidationCheckBox.Text = "Validate Active Directory users with:";
      this.ActiveDirectoryValidationCheckBox.UseVisualStyleBackColor = true;
      this.ActiveDirectoryValidationCheckBox.CheckedChanged += new System.EventHandler(this.ActiveDirectoryValidationCheckBox_CheckedChanged);
      // 
      // DomainAdministratorPasswordLabel
      // 
      this.DomainAdministratorPasswordLabel.AccessibleDescription = "A label displaying the text domain administrator user name";
      this.DomainAdministratorPasswordLabel.AccessibleName = "Domain Administrator User Name Text";
      this.DomainAdministratorPasswordLabel.AutoSize = true;
      this.DomainAdministratorPasswordLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DomainAdministratorPasswordLabel.Location = new System.Drawing.Point(28, 199);
      this.DomainAdministratorPasswordLabel.Name = "DomainAdministratorPasswordLabel";
      this.DomainAdministratorPasswordLabel.Size = new System.Drawing.Size(229, 25);
      this.DomainAdministratorPasswordLabel.TabIndex = 23;
      this.DomainAdministratorPasswordLabel.Text = "Domain Admin User Name:";
      // 
      // DomainAdministratorConfirmPasswordLabel
      // 
      this.DomainAdministratorConfirmPasswordLabel.AccessibleDescription = "A label displaying the text domain administrator password";
      this.DomainAdministratorConfirmPasswordLabel.AccessibleName = "Domain Administrator Password Text";
      this.DomainAdministratorConfirmPasswordLabel.AutoSize = true;
      this.DomainAdministratorConfirmPasswordLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DomainAdministratorConfirmPasswordLabel.Location = new System.Drawing.Point(36, 226);
      this.DomainAdministratorConfirmPasswordLabel.Name = "DomainAdministratorConfirmPasswordLabel";
      this.DomainAdministratorConfirmPasswordLabel.Size = new System.Drawing.Size(217, 25);
      this.DomainAdministratorConfirmPasswordLabel.TabIndex = 24;
      this.DomainAdministratorConfirmPasswordLabel.Text = "Domain Admin Password:";
      // 
      // DomainAdministratorPasswordTextBox
      // 
      this.DomainAdministratorPasswordTextBox.AccessibleDescription = "A text box to input the domain administrator password";
      this.DomainAdministratorPasswordTextBox.AccessibleName = "Domain Administrator Password";
      this.DomainAdministratorPasswordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.DomainAdministratorPasswordTextBox.Location = new System.Drawing.Point(187, 223);
      this.DomainAdministratorPasswordTextBox.Name = "DomainAdministratorPasswordTextBox";
      this.DomainAdministratorPasswordTextBox.PasswordChar = '●';
      this.DomainAdministratorPasswordTextBox.Size = new System.Drawing.Size(184, 31);
      this.DomainAdministratorPasswordTextBox.TabIndex = 22;
      this.DomainAdministratorPasswordTextBox.TextChanged += new System.EventHandler(this.DomainAdministratorPasswordTextBox_TextChanged);
      // 
      // DomainAdministratorUserNameTextBox
      // 
      this.DomainAdministratorUserNameTextBox.AccessibleDescription = "A text box to input the domain administrator user name";
      this.DomainAdministratorUserNameTextBox.AccessibleName = "Domain Administrator User Name";
      this.DomainAdministratorUserNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.DomainAdministratorUserNameTextBox.Location = new System.Drawing.Point(187, 196);
      this.DomainAdministratorUserNameTextBox.Name = "DomainAdministratorUserNameTextBox";
      this.DomainAdministratorUserNameTextBox.Size = new System.Drawing.Size(184, 31);
      this.DomainAdministratorUserNameTextBox.TabIndex = 21;
      this.DomainAdministratorUserNameTextBox.TextChanged += new System.EventHandler(this.DomainAdministratorUserNameTextBox_TextChanged);
      // 
      // WindowsTokensDocumentationLinkLabel
      // 
      this.WindowsTokensDocumentationLinkLabel.AccessibleDescription = "A link label to open a web page with documentation about Windows tokens";
      this.WindowsTokensDocumentationLinkLabel.AccessibleName = "Windows Tokens Help Link";
      this.WindowsTokensDocumentationLinkLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.WindowsTokensDocumentationLinkLabel.Location = new System.Drawing.Point(19, 62);
      this.WindowsTokensDocumentationLinkLabel.Name = "WindowsTokensDocumentationLinkLabel";
      this.WindowsTokensDocumentationLinkLabel.Size = new System.Drawing.Size(100, 16);
      this.WindowsTokensDocumentationLinkLabel.TabIndex = 20;
      this.WindowsTokensDocumentationLinkLabel.TabStop = true;
      this.WindowsTokensDocumentationLinkLabel.Text = "Documentation";
      this.WindowsTokensDocumentationLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.WindowsTokensDocumentationLinkLabel_LinkClicked);
      // 
      // WindowsTokensHelpLabel
      // 
      this.WindowsTokensHelpLabel.AccessibleDescription = "A label displaying text about how to access documentation related to Windows toke" +
    "ns";
      this.WindowsTokensHelpLabel.AccessibleName = "Windows Tokens Help Text";
      this.WindowsTokensHelpLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.WindowsTokensHelpLabel.Location = new System.Drawing.Point(19, 23);
      this.WindowsTokensHelpLabel.Name = "WindowsTokensHelpLabel";
      this.WindowsTokensHelpLabel.Size = new System.Drawing.Size(146, 36);
      this.WindowsTokensHelpLabel.TabIndex = 18;
      this.WindowsTokensHelpLabel.Text = "Enter Windows Users and Groups:";
      // 
      // WindowsTokensRichTextBox
      // 
      this.WindowsTokensRichTextBox.AccessibleDescription = "A text box to input windows tokens to apply to the Windows authentication";
      this.WindowsTokensRichTextBox.AccessibleName = "Windows Tokens";
      this.WindowsTokensRichTextBox.DetectUrls = false;
      this.WindowsTokensRichTextBox.Location = new System.Drawing.Point(175, 25);
      this.WindowsTokensRichTextBox.Name = "WindowsTokensRichTextBox";
      this.WindowsTokensRichTextBox.Size = new System.Drawing.Size(200, 70);
      this.WindowsTokensRichTextBox.TabIndex = 19;
      this.WindowsTokensRichTextBox.Text = "";
      // 
      // OkButton
      // 
      this.OkButton.AccessibleDescription = "A button to apply any changes and close the dialog";
      this.OkButton.AccessibleName = "OK";
      this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.OkButton.Enabled = false;
      this.OkButton.Location = new System.Drawing.Point(286, 469);
      this.OkButton.Name = "OkButton";
      this.OkButton.Size = new System.Drawing.Size(75, 23);
      this.OkButton.TabIndex = 1;
      this.OkButton.Text = "&OK";
      this.OkButton.UseVisualStyleBackColor = true;
      // 
      // DialogCancelButton
      // 
      this.DialogCancelButton.AccessibleDescription = "A button to dismiss any changes and close the dialog";
      this.DialogCancelButton.AccessibleName = "Cancel";
      this.DialogCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.DialogCancelButton.Location = new System.Drawing.Point(367, 469);
      this.DialogCancelButton.Name = "DialogCancelButton";
      this.DialogCancelButton.Size = new System.Drawing.Size(75, 23);
      this.DialogCancelButton.TabIndex = 2;
      this.DialogCancelButton.Text = "&Cancel";
      this.DialogCancelButton.UseVisualStyleBackColor = true;
      // 
      // ValidationsTimer
      // 
      this.ValidationsTimer.Interval = 800;
      this.ValidationsTimer.Tick += new System.EventHandler(this.ValidationsTimer_Tick);
      // 
      // ValidationsErrorProvider
      // 
      this.ValidationsErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
      this.ValidationsErrorProvider.ContainerControl = this;
      // 
      // HostErrorProvider
      // 
      this.HostErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
      this.HostErrorProvider.ContainerControl = this;
      // 
      // EmptyPasswordWarningErrorProvider
      // 
      this.EmptyPasswordWarningErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
      this.EmptyPasswordWarningErrorProvider.ContainerControl = this;
      // 
      // DatabaseUserDialog
      // 
      this.AcceptButton = this.OkButton;
      this.AccessibleDescription = "A modal dialog to create or edit a MySQL user account";
      this.AccessibleName = "MySQL User Account";
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.CancelButton = this.DialogCancelButton;
      this.ClientSize = new System.Drawing.Size(448, 497);
      this.Controls.Add(this.DialogCancelButton);
      this.Controls.Add(this.OkButton);
      this.Controls.Add(this.MainPanel);
      this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "DatabaseUserDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "MySQL User Account";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DatabaseUserDialog_FormClosing);
      this.MainPanel.ResumeLayout(false);
      this.MainPanel.PerformLayout();
      this.MySqlAuthenticationGroupBox.ResumeLayout(false);
      this.MySqlAuthenticationGroupBox.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.MySqlAuthenticationPictureBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.WindowsAuthenticationPictureBox)).EndInit();
      this.WindowsAuthenticationGroupBox.ResumeLayout(false);
      this.WindowsAuthenticationGroupBox.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.HostErrorProvider)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.EmptyPasswordWarningErrorProvider)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel MainPanel;
    private System.Windows.Forms.PictureBox MySqlAuthenticationPictureBox;
    private System.Windows.Forms.TextBox UsernameTextBox;
    private System.Windows.Forms.Label UserRoleLabel;
    private System.Windows.Forms.Label UsernameLabel;
    private System.Windows.Forms.Button OkButton;
    private System.Windows.Forms.Button DialogCancelButton;
    private System.Windows.Forms.Label CaptionLabel;
    private System.Windows.Forms.RadioButton WindowsAuthenticationRadioButton;
    private System.Windows.Forms.RadioButton MySqlAuthenticationRadioButton;
    private System.Windows.Forms.Label AuthenticationTypeLabel;
    private System.Windows.Forms.PictureBox WindowsAuthenticationPictureBox;
    private ImageComboBox UserRoleComboBox;
    private System.Windows.Forms.Label HostLabel;
    private System.Windows.Forms.ComboBox HostComboBox;
    private System.Windows.Forms.Timer ValidationsTimer;
    private System.Windows.Forms.ErrorProvider ValidationsErrorProvider;
    private System.Windows.Forms.ErrorProvider HostErrorProvider;
    private System.Windows.Forms.GroupBox WindowsAuthenticationGroupBox;
    private System.Windows.Forms.Button TestSecurityTokensButton;
    private System.Windows.Forms.CheckBox ActiveDirectoryValidationCheckBox;
    private System.Windows.Forms.Label DomainAdministratorPasswordLabel;
    private System.Windows.Forms.Label DomainAdministratorConfirmPasswordLabel;
    private System.Windows.Forms.TextBox DomainAdministratorPasswordTextBox;
    private System.Windows.Forms.TextBox DomainAdministratorUserNameTextBox;
    private System.Windows.Forms.LinkLabel WindowsTokensDocumentationLinkLabel;
    private System.Windows.Forms.Label WindowsTokensHelpLabel;
    private System.Windows.Forms.RichTextBox WindowsTokensRichTextBox;
    private System.Windows.Forms.GroupBox MySqlAuthenticationGroupBox;
    private System.Windows.Forms.Label PasswordLabel;
    private System.Windows.Forms.TextBox ConfirmPasswordTextBox;
    private System.Windows.Forms.TextBox PasswordTextBox;
    private System.Windows.Forms.Label ConfirmPasswordLabel;
    private System.Windows.Forms.Label DomainAdministratorCredentialsHelpLabel;
    private System.Windows.Forms.ToolTip UserPasswordToolTip;
    private System.Windows.Forms.ErrorProvider EmptyPasswordWarningErrorProvider;
    private PasswordStrengthLabel PasswordStrengthLabel;
  }
}

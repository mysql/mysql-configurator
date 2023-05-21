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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.MySql;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Properties;

namespace MySql.Configurator.Wizards.Server
{
  public partial class DatabaseUserDialog : Form
  {
    private const int MYSQL_AUTHENTICATION_FORM_HEIGHT = 350;
    private const int WINDOWS_AUTHENTICATION_FORM_HEIGHT = 535;

    #region Fields

    private readonly MySqlAuthenticationPluginType _defaultAuthenticationPlugin;
    private ServerUser _serverUser;
    private bool _windowsSecurityTokensAreValid;

    #endregion Fields

    public DatabaseUserDialog(bool showWinAuth, IEnumerable<Role> roles, MySqlAuthenticationPluginType defaultAuthenticationPlugin, Version serverVersion)
    {
      InitializeComponent();
      if (!showWinAuth)
      {
        WindowsAuthenticationRadioButton.Enabled = false;
        WindowsAuthenticationRadioButton.Visible = false;
      }

      ServerVersion = serverVersion;
      _defaultAuthenticationPlugin = defaultAuthenticationPlugin;
      MySqlAuthenticationRadioButton.Checked = true;
      ActiveDirectoryValidationCheckBox.Checked = false;
      _windowsSecurityTokensAreValid = false;
      _serverUser = new ServerUser();
      HostComboBox.SelectedIndex = 0;

      foreach (var r in roles)
      {
        UserRoleComboBox.AddItem(Resources.ConfigUserMySQL241, r.Display, r.Description, r);

        if (r.ID == "DBA")
        {
          UserRoleComboBox.SelectedIndex = (UserRoleComboBox.Items.Count - 1);
        }
      }

      FireAllValidations();
    }

    #region Properties

    /// <summary>
    /// Gets or sets the <see cref="ServerUser"/> to be created in the database.
    /// </summary>
    public ServerUser ServerUser
    {
      get => _serverUser;

      set
      {
        _serverUser = value;
        UsernameTextBox.Text = _serverUser.Username;

        switch (_serverUser.Host)
        {
          case "%":
            HostComboBox.SelectedIndex = 0;
            break;

          case MySqlServerUser.LOCALHOST:
            HostComboBox.SelectedIndex = 1;
            break;

          default:
            HostComboBox.SelectedIndex = -1;
            HostComboBox.Text = _serverUser.Host;
            break;
        }

        UserRoleComboBox.SelectedIndex = UserRoleComboBox.Find(_serverUser.UserRole.Display);
        if (_serverUser.AuthenticationPlugin == MySqlAuthenticationPluginType.Windows)
        {
          WindowsTokensRichTextBox.Text = _serverUser.WindowsSecurityTokenList;
          WindowsAuthenticationRadioButton.Checked = true;
        }
        else
        {
          PasswordTextBox.Text = _serverUser.Password;
          ConfirmPasswordTextBox.Text = _serverUser.Password;
          MySqlAuthenticationRadioButton.Checked = true;
          TextBoxValidated(PasswordTextBox, EventArgs.Empty);
        }
      }
    }

    /// <summary>
    /// Gets the Server version.
    /// </summary>
    public Version ServerVersion { get; }

    #endregion Properties

    /// <summary>
    /// Event delegate method firing when the <see cref="DatabaseUserDialog"/> form is being closed.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DatabaseUserDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (DialogResult == DialogResult.Cancel)
      {
        return;
      }

      if (WindowsAuthenticationRadioButton.Checked)
      {
        _serverUser.AuthenticationPlugin = MySqlAuthenticationPluginType.Windows;
        _serverUser.WindowsSecurityTokenList = WindowsTokensRichTextBox.Text;
      }
      else if (MySqlAuthenticationRadioButton.Checked)
      {
        _serverUser.AuthenticationPlugin = _defaultAuthenticationPlugin;
        _serverUser.Password = PasswordTextBox.Text;
      }

      _serverUser.Username = UsernameTextBox.Text.Trim();
      _serverUser.UserRole = (UserRoleComboBox.Items[UserRoleComboBox.SelectedIndex] as ImageComboBoxItem)?.Tag as Role;
    }

    protected override void OnLoad(EventArgs e)
    {
      Core.Classes.Utilities.NormalizeFont(this);
      base.OnLoad(e);
    }

    /// <summary>
    /// Fire validations for fields with no data, just for the sake of displaying their related error providers.
    /// </summary>
    private void FireAllValidations()
    {
      var textBoxes = this.GetChildControlsOfType<TextBoxBase>();
      foreach (var textBox in textBoxes)
      {
        TextBoxValidated(textBox, EventArgs.Empty);
      }

      TestSecurityTokensButton_Click(WindowsTokensRichTextBox, EventArgs.Empty);
      UpdateOkButton();
    }

    private void HostComboBox_KeyUp(object sender, KeyEventArgs e)
    {
      if (HostComboBox.SelectedIndex != 0)
      {
        _serverUser.Host = HostComboBox.Text;
      }
    }

    private void HostComboBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
    {
      if (!(e.Alt || e.Control || e.Shift || e.KeyCode == Keys.Tab))
      {
        HostComboBox.SelectedIndex = -1;
      }
    }

    private void HostComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      switch (HostComboBox.SelectedIndex)
      {
        case -1:
          break;

        case 0:
          _serverUser.Host = "%";
          break;

        default:
          _serverUser.Host = HostComboBox.Items[HostComboBox.SelectedIndex].ToString();
          break;
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="HostComboBox"/> is validated.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void HostComboBox_Validated(object sender, EventArgs e)
    {
      string errorMessage = string.Empty;
      if (_serverUser.Host == "%"
          && ServerVersion.ServerIncludesAnonymousUser())
      {
        errorMessage = Resources.ServerUserAnonymousAccountWithAllHostsWarning;
      }

      var providerProperties = new ErrorProviderProperties(errorMessage, Resources.warning_sign_icon);
      HostErrorProvider.SetProperties(HostComboBox, providerProperties);
    }

    private void ResetValidationsTimer()
    {
      ValidationsTimer.Stop();
      ValidationsTimer.Start();
    }

    private void SwitchAuthTypes(object sender, EventArgs e)
    {
      var mysqlAuthentication = MySqlAuthenticationRadioButton.Checked;
      MySqlAuthenticationGroupBox.Visible = mysqlAuthentication;
      MySqlAuthenticationPictureBox.Visible = mysqlAuthentication;
      WindowsAuthenticationGroupBox.Visible = !mysqlAuthentication;
      WindowsAuthenticationPictureBox.Visible = !mysqlAuthentication;
      Size = new Size(Width, mysqlAuthentication ? MYSQL_AUTHENTICATION_FORM_HEIGHT : WINDOWS_AUTHENTICATION_FORM_HEIGHT);
      FireAllValidations();
    }

    /// <summary>
    /// Handles the TextChanged event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void TextChangedHandler(object sender, EventArgs e)
    {
      ResetValidationsTimer();
    }

    /// <summary>
    /// Handles the text validation event for TextBox and RichTextBox controls.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void TextBoxValidated(object sender, EventArgs e)
    {
      ValidationsTimer.Stop();
      if (!(sender is Control textBox))
      {
        return;
      }

      string errorMessage;
      // Passwords shouldn't be trimmed since they can consist of blanks
      if (textBox.Name != nameof(PasswordTextBox) && textBox.Name != nameof(ConfirmPasswordTextBox))
      {
        textBox.Text = textBox.Text.Trim();
      }

      var mysqlAuthentication = MySqlAuthenticationRadioButton.Checked;
      switch (textBox.Name)
      {
        case nameof(UsernameTextBox):
          errorMessage = MySqlServerInstance.ValidateUserName(textBox.Text, false);
          break;

        case nameof(PasswordTextBox):
        case nameof(ConfirmPasswordTextBox):
          if (mysqlAuthentication)
          {
            EmptyPasswordWarningErrorProvider.Clear();
            textBox.Text = textBox.Text;
            var isPasswordEmptyOrBlank = string.IsNullOrWhiteSpace(textBox.Text);
            // No need for validation if the password is an empty string
            errorMessage = textBox.Text == string.Empty
              ? string.Empty
              : Core.Classes.Utilities.ValidatePasswords(PasswordTextBox.Text, ConfirmPasswordTextBox.Text, PasswordStrengthLabel);
            if (isPasswordEmptyOrBlank)
            {
              EmptyPasswordWarningErrorProvider.SetProperties(textBox, new ErrorProviderProperties(Resources.ServerConfigEmptyOrBlankPasswordWarning, Resources.warning_sign_icon));
            }

            if (textBox == ConfirmPasswordTextBox)
            {
              // If the control being validated is the ConfirmPasswordTextBox, set the control to place the error provider to PasswordTextBox
              textBox = PasswordTextBox;
            }
          }
          else
          {
            errorMessage = string.Empty;
          }

          break;

        default:
          return;
      }

      ValidationsErrorProvider.SetProperties(textBox, new ErrorProviderProperties(errorMessage));
      UpdateOkButton();
    }

    private void UpdateOkButton()
    {
      OkButton.Enabled = !ValidationsErrorProvider.HasErrors()
        && (!WindowsAuthenticationRadioButton.Checked
            || (WindowsAuthenticationRadioButton.Checked
                && _windowsSecurityTokensAreValid));
    }

    /// <summary>
    /// Validates that the Windows security tokens are well formed.
    /// </summary>
    /// <returns>An empty string if the Windows security tokens are well formed, otherwise an error message.</returns>
    private string ValidateSecurityTokens()
    {
      bool firstString = true;
      var errorMessageBuilder = new StringBuilder();
      var defaultFont = WindowsTokensRichTextBox.Font;
      var defaultColor = WindowsTokensRichTextBox.ForeColor;
      char[] validSeparators = { ';', ' ', ',' };
      var winAuthTokensText = WindowsTokensRichTextBox.Text.Trim();
      if (string.IsNullOrEmpty(winAuthTokensText))
      {
        return Resources.ServerConfigEmptySecurityTokens;
      }

      string[] winAuthTokens = WindowsTokensRichTextBox.Text.Trim().Split(validSeparators);
      WindowsTokensRichTextBox.Text = string.Empty;
      Cursor = Cursors.WaitCursor;
      foreach (string possibleToken in winAuthTokens)
      {
        bool tokenExists;
        if (possibleToken == string.Empty)
        {
          continue;
        }

        try
        {
          tokenExists = DirectoryServicesWrapper.TokenExists(possibleToken);
          if (!tokenExists)
          {
            errorMessageBuilder.Append($" - {possibleToken}: {Resources.ServerConfigWindowsUserOrGroupNotFound}{Environment.NewLine}");
          }
        }
        catch(Exception ex)
        {
          tokenExists = false;
          // Attempting to query the Active Directory may raise an error with the "Unspecified error" message
          // which can indicate different issues, in this case a more user friendly error message is required
          var exceptionMessage = ex.Message == Resources.ServerConfigUnspecifiedError
            ? Resources.ServerConfigUserFriendlyUnspecifiedError
            : ex.Message;
          errorMessageBuilder.Append($"- {possibleToken}: {exceptionMessage}");
        }

        if (!firstString)
        {
          WindowsTokensRichTextBox.AppendText("; ");
        }
        else
        {
          firstString = false;
        }

        if (!tokenExists)
        {
          WindowsTokensRichTextBox.SelectionFont = new Font(defaultFont.FontFamily, defaultFont.Size, FontStyle.Underline);
          WindowsTokensRichTextBox.SelectionColor = Color.Red;
        }
        else
        {
          WindowsTokensRichTextBox.SelectionFont = defaultFont;
          WindowsTokensRichTextBox.SelectionColor = Color.Green;
        }

        WindowsTokensRichTextBox.AppendText(possibleToken);
      }

      // Make sure any new text entered uses the default Font and Color.
      WindowsTokensRichTextBox.SelectionFont = defaultFont;
      WindowsTokensRichTextBox.SelectionColor = defaultColor;

      Cursor = Cursors.Default;
      var errorMessage = errorMessageBuilder.ToString();
      return !string.IsNullOrEmpty(errorMessage)
        ? string.Format(Resources.ServerConfigInvalidWindowsSecurityTokens, errorMessage)
        : string.Empty;
    }

    /// <summary>
    /// Resets all relevant items to indicate that the Windows security tokens require to be validated once more.
    /// </summary>
    private void ResetSecurityTokensValidStatus()
    {
      _windowsSecurityTokensAreValid = false;
      TestSecurityTokensButton.Enabled = !_windowsSecurityTokensAreValid;
      int selectionStart = WindowsTokensRichTextBox.SelectionStart;
      WindowsTokensRichTextBox.SelectAll();
      WindowsTokensRichTextBox.SelectionColor = Color.Black;
      WindowsTokensRichTextBox.SelectionFont = new Font(WindowsTokensRichTextBox.Font, FontStyle.Regular);
      WindowsTokensRichTextBox.Select(selectionStart, 0);
      UpdateOkButton();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ValidationsTimer"/> timer's elapses.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ValidationsTimer_Tick(object sender, EventArgs e)
    {
      var focusedTextBox = this.GetChildControlsOfType<TextBoxBase>().FirstOrDefault(control => control.Focused);
      if (focusedTextBox != null)
      {
        TextBoxValidated(focusedTextBox, EventArgs.Empty);
      }
    }

    private void WindowsTokensDocumentationLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Core.Classes.Utilities.OpenBrowser("https://dev.mysql.com/doc/refman/en/windows-pluggable-authentication.html#windows-pluggable-authentication-usage");
    }

    private void ActiveDirectoryValidationCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      var checkBox = (CheckBox)sender;
      DomainAdministratorUserNameTextBox.Enabled =
      DomainAdministratorPasswordTextBox.Enabled = checkBox.Checked;
      ResetSecurityTokensValidStatus();
    }

    private void TestSecurityTokensButton_Click(object sender, EventArgs e)
    {
      if (MySqlAuthenticationRadioButton.Checked)
      {
        ValidationsErrorProvider.SetError(WindowsTokensRichTextBox, string.Empty);
        return;
      }

      if (ActiveDirectoryValidationCheckBox.Checked)
      {
        DirectoryServicesWrapper.AdministratorUserName = DomainAdministratorUserNameTextBox.Text.Trim();
        DirectoryServicesWrapper.AdministratorPassword = DomainAdministratorPasswordTextBox.Text.Trim();
      }
      else
      {
        DirectoryServicesWrapper.AdministratorUserName =
        DirectoryServicesWrapper.AdministratorPassword = null;
      }

      WindowsTokensRichTextBox.TextChanged -= WindowsTokensRichTextBox_TextChanged;
      string errorMessage = ValidateSecurityTokens();
      WindowsTokensRichTextBox.TextChanged += WindowsTokensRichTextBox_TextChanged;
      ValidationsErrorProvider.SetProperties(WindowsTokensRichTextBox, new ErrorProviderProperties(errorMessage));
      _windowsSecurityTokensAreValid = string.IsNullOrEmpty(errorMessage);
      TestSecurityTokensButton.Enabled = !_windowsSecurityTokensAreValid;
      UpdateOkButton();
    }

    private void WindowsTokensRichTextBox_TextChanged(object sender, EventArgs e)
    {
      ResetSecurityTokensValidStatus();
    }

    private void DomainAdministratorUserNameTextBox_TextChanged(object sender, EventArgs e)
    {
      ResetSecurityTokensValidStatus();
    }

    private void DomainAdministratorPasswordTextBox_TextChanged(object sender, EventArgs e)
    {
      ResetSecurityTokensValidStatus();
    }
  }
}

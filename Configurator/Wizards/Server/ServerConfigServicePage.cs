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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Core.Wizard;
using MySql.Configurator.Properties;

namespace MySql.Configurator.Wizards.Server
{
  public partial class ServerConfigServicePage : ConfigWizardPage
  {
    #region Fields

    /// <summary>
    /// Flag to indicate if the provided service name is valid.
    /// </summary>
    private bool _isValidServiceName;

    /// <summary>
    /// Flag to indicate if the provided user is valid.
    /// </summary>
    private bool _isValidUser;

    private readonly ServerConfigurationController _controller;
    private readonly MySqlServerSettings _settings;

    #endregion Fields

    public ServerConfigServicePage(ServerConfigurationController controller)
    {
      _isValidServiceName = false;
      _isValidUser = false;
      InitializeComponent();
      _controller = controller;
      _settings = controller.Settings;
    }

    #region Properties

    public override bool NextOk => !ConfigureAsServiceCheckBox.Checked
                                   || (_isValidUser
                                       && _isValidServiceName);

    #endregion Properties

    public override void Activate()
    {
      WindowsServiceNameTextBox.Text = _settings.ServiceName;
      StartWindowsServiceAtStartupCheckBox.Checked = _settings.ServiceStartAtStartup;
      ConfigureAsServiceCheckBox.Checked = _settings.ConfigureAsService;
      CustomUserUsernameTextBox.Text = _settings.ServiceAccountUsername;
      CustomUserPasswordTextBox.Text = _settings.ServiceAccountPassword;
      if (_settings.ServiceAccountUsername == string.Empty
          || _settings.ServiceAccountUsername == MySqlServiceControlManager.STANDARD_SERVICE_ACCOUNT)
      {
        StandardSystemAccountRadioButton.Checked = true;
      }
      else
      {
        CustomUserRadioButton.Checked = true;
      }

      FireAllValidations();
      base.Activate();
    }

    public override bool Next()
    {
      _settings.ConfigureAsService = ConfigureAsServiceCheckBox.Checked;
      _settings.ServiceStartAtStartup = StartWindowsServiceAtStartupCheckBox.Checked;
      _settings.ServiceName = WindowsServiceNameTextBox.Text.Trim();
      _settings.ServiceAccountUsername = StandardSystemAccountRadioButton.Checked
        ? MySqlServiceControlManager.STANDARD_SERVICE_ACCOUNT
        : CustomUserUsernameTextBox.Text.Trim();
      _settings.ServiceAccountPassword = StandardSystemAccountRadioButton.Checked
        ? string.Empty
        : CustomUserPasswordTextBox.Text.Trim();
      var logMessage = new StringBuilder("Advancing to next step using:\n\t");
      logMessage.AppendFormat("Service Name: {0}\n\t", _settings.ServiceName);
      logMessage.AppendFormat("Auto-start Service at boot: {0}\n\t", _settings.ServiceStartAtStartup.ToString());
      logMessage.AppendFormat("Service Account User: {0}", _settings.ServiceAccountUsername == string.Empty ? "Network Service" : _settings.ServiceAccountUsername);
      Logger.LogInformation(logMessage.ToString());

      // Check if the Server File Permissions configuration should be shown.
      var serverFilePermissionsPage = _controller.Pages.FirstOrDefault(page => page is ServerConfigSecurityPage);
      if (serverFilePermissionsPage != null)
      {
        var permissionsNeedToBeUpdated = !_controller.ValidateServerFilesHaveRecommendedPermissions();
        serverFilePermissionsPage.PageVisible = permissionsNeedToBeUpdated;
      }
      return base.Next();
    }

    private string ValidateCustomUser()
    {
      ErrorProviderControl = CustomUserUsernameTextBox;
      _isValidUser = false;
      string errorMessage = null;
      Cursor = Cursors.WaitCursor;
      var result = MySqlServiceControlManager.ValidateServiceAccountUser(CustomUserUsernameTextBox.Text.Trim(), CustomUserPasswordTextBox.Text, _controller.DataDirectory);

      switch (result)
      {
        case ServiceAccountMissingRequirement.None:
          _isValidUser = true;
          break;

        case ServiceAccountMissingRequirement.MatchedUsernamePassword:
          errorMessage = Resources.ServerConfigCustomUserInvalidCredentials;
          break;

        case ServiceAccountMissingRequirement.LogonAsAServiceRight:
          errorMessage = Resources.ServerConfigCustomUserInvalidRights;
          break;

        case ServiceAccountMissingRequirement.DirectoryPermissions:
          errorMessage = Resources.ServerConfigCustomUserInvalidPermissions;
          break;

        case ServiceAccountMissingRequirement.Undefined:
          errorMessage = Resources.ServerConfigCustomUserUnknowError;
          break;
      }

      Cursor = Cursors.Default;
      return errorMessage;
    }

    private void ServiceAccountTypeCheckChanged(object sender, EventArgs e)
    {
      _isValidUser = StandardSystemAccountRadioButton.Checked;
      CustomUserUsernameLabel.Visible = CustomUserRadioButton.Checked;
      CustomUserUsernameTextBox.Visible = CustomUserRadioButton.Checked;
      CustomUserPasswordLabel.Visible = CustomUserRadioButton.Checked;
      CustomUserPasswordTextBox.Visible = CustomUserRadioButton.Checked;
      if (CustomUserRadioButton.Checked)
      {
        CustomUserUsernameTextBox.Validate(false);
        CustomUserPasswordTextBox.Validate(false);
      }

      UpdateButtons();
    }

    /// <summary>
    /// Handles the TextChanged event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    protected override void TextChangedHandler(object sender, EventArgs e)
    {
      // Looks like we could get rid of this empty override, but it is necessary to avoid an error of:
      // The method 'xxx' cannot be the method for an event because a class this class derives from already defines the method
      base.TextChangedHandler(sender, e);
    }

    /// <summary>
    /// Handles the TextValidated event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    /// <remarks>This event method is meant to be used with the <see cref="Control.Validated"/> event.</remarks>
    protected override void ValidatedHandler(object sender, EventArgs e)
    {
      // Looks like we could get rid of this empty override, but it is necessary to avoid an error of:
      // The method 'xxx' cannot be the method for an event because a class this class derives from already defines the method
      base.ValidatedHandler(sender, e);
    }

    /// <summary>
    /// Contains calls to methods that validate the given control's value.
    /// </summary>
    /// <returns>An error message or <c>null</c> / <see cref="string.Empty"/> if everything is valid.</returns>
    protected override string ValidateFields()
    {
      string errorMessage = base.ValidateFields();
      switch (ErrorProviderControl.Name)
      {
        case nameof(WindowsServiceNameTextBox):
          var serviceName = WindowsServiceNameTextBox.Text.Trim();
          if (_controller.ConfigurationType != ConfigurationType.Reconfiguration
              || !serviceName.Equals(_controller.Settings.ServiceName, StringComparison.OrdinalIgnoreCase))
          {
            errorMessage = MySqlServiceControlManager.ValidateServiceName(serviceName);
          }

          _isValidServiceName = string.IsNullOrEmpty(errorMessage);
          break;

        case nameof(CustomUserUsernameTextBox):
        case nameof(CustomUserPasswordTextBox):
            errorMessage = ValidateCustomUser();
          break;
      }

      return errorMessage;
    }

    private void ConfigureAsService_CheckedChanged(object sender, EventArgs e)
    {
      WindowsServiceDetailsPanel.Visible = ConfigureAsServiceCheckBox.Checked;
      UpdateButtons();
    }
  }
}

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
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Core.Wizard;
using MySql.Configurator.Properties;

namespace MySql.Configurator.Wizards.Server
{
  /// <summary>
  /// Configuration page to ask for credentials for a database upgrade.
  /// </summary>
  public partial class ServerConfigUpgradePage : ConfigWizardPage
  {
    #region Fields

    /// <summary>
    /// The <seealso cref="ServerConfigurationController"/> used to perform actions.
    /// </summary>
    private readonly ServerConfigurationController _controller;

    /// <summary>
    /// Flag indicating whether a connection could be established with the given credentials.
    /// </summary>
    private bool _credsOk;

    /// <summary>
    /// The <seealso cref="MySqlServerSettings"/> where Server settings are stored.
    /// </summary>
    private readonly MySqlServerSettings _settings;

    /// <summary>
    /// The password that has been validated by testing the connection.
    /// </summary>
    private string _validatedPassword;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerConfigUpgradePage"/> class.
    /// </summary>
    /// <param name="controller">The <seealso cref="ServerConfigurationController"/> used to perform actions.</param>
    public ServerConfigUpgradePage(ServerConfigurationController controller)
    {
      BackupDatabase = true;
      InitializeComponent();
      _controller = controller;
      _settings = _controller.Settings;
      _credsOk = false;
      _validatedPassword = null;
      UpdatePanelsVisibility();
    }

    #region Properties

    /// <summary>
    /// Gets a value indicating whether a backup of the database is done before upgrading the system tables.
    /// </summary>
    public bool BackupDatabase { get; private set; }

    /// <summary>
    /// Gets a value indicating whether an upgrade to a local stand-alone Server will be performed.
    /// </summary>
    public bool DoStandAloneUpgrade => SkipUpgradeCheckBox.Visible && !SkipUpgradeCheckBox.Checked;

    /// <summary>
    /// Gets a value indicating if Next button is available.
    /// </summary>
    public override bool NextOk => !SkipUpgradePanel.Visible
                                   || base.NextOk
                                      && (SkipUpgradeCheckBox.Checked
                                          || _credsOk);

    #endregion Properties

    /// <summary>
    /// Activates this instance.
    /// </summary>
    public override void Activate()
    {
      BackupDatabaseCheckBox.Checked = BackupDatabase;
      UpdatePanelsVisibility();
      base.Activate();
    }

    /// <summary>
    /// Executes actions performed when the Next button is clicked.
    /// </summary>
    /// <returns><c>true</c> if it the configuration should proceed to the next panel, <c>false</c> otherwise.</returns>
    public override bool Next()
    {
      _controller.UpdateConfigurationState(DoStandAloneUpgrade);
      if (DoStandAloneUpgrade)
      {
        // Reassign to settings the sanitized password
        if (UpgradeExternalPanel.Visible)
        {
          _settings.ExistingRootPassword =
          _settings.RootPassword =
            _validatedPassword;
        }

        _controller.IsBackupDatabaseStepNeeded = BackupDatabase
                                                 && _controller.IsThereServerDataFiles;
      }

      _controller.Settings.SystemTablesUpgraded = !SkipUpgradePanel.Visible
        ? SystemTablesUpgradedType.None
        : SkipUpgradeCheckBox.Checked
          ? SystemTablesUpgradedType.No
          : SystemTablesUpgradedType.Yes;

      _controller.UpdateUpgradeConfigSteps();
      return base.Next();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="BackupDatabaseCheckBox"/> checked property changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void BackupDatabaseCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      BackupDatabase = BackupDatabaseCheckBox.Checked;
    }

    /// <summary>
    /// Event delegate method fired when the <seealso cref="CheckButton"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void CheckButton_Click(object sender, EventArgs e)
    {
      Cursor = Cursors.WaitCursor;
      _credsOk = false;
      ResultLabel.Text = Resources.StartingServerAndTestingConnection;
      var providerProperties = new ErrorProviderProperties(Resources.StartingServerAndTestingConnection, Resources.MySQLInstallerConfig_InProgress, true);
      ConnectionErrorProvider.SetProperties(CheckButton, providerProperties);
      _validatedPassword = ExistingRootPasswordTextBox.Text.Sanitize();
      var connectionResult = LocalServerInstance.CanConnect(_controller, out string errorMessage, _validatedPassword, true, true);
      _credsOk = connectionResult == ConnectionResultType.ConnectionSuccess;
      if (!_credsOk)
      {
        _validatedPassword = null;
      }

      providerProperties.ErrorMessage = string.IsNullOrEmpty(errorMessage)
        ? connectionResult.GetDescription()
        : errorMessage;
      providerProperties.ErrorIcon = _credsOk
        ? Resources.MySQLInstallerConfig_DoneIcon
        : Resources.MySQLInstallerConfig_ErrorIcon;
      ConnectionErrorProvider.SetProperties(CheckButton, providerProperties);
      ResultLabel.Text = providerProperties.ErrorMessage;
      UpdateButtons();
      Cursor = Cursors.Default;
    }

    /// <summary>
    /// Resets the state of controls related to the credentials check, making credentials needed to be re-tested.
    /// </summary>
    /// <param name="clearPassword">Flag indicating whether the password text is cleared.</param>
    private void ResetCheckCredentialsState(bool clearPassword)
    {
      bool state = !SkipUpgradeCheckBox.Checked;
      _credsOk = false;
      ResultLabel.Text = string.Empty;
      ConnectionErrorProvider.Clear();
      CheckButton.Enabled = state;
      if (clearPassword)
      {
        ExistingRootPasswordTextBox.Clear();
      }

      ExistingRootPasswordTextBox.Enabled = state;
      UpdateButtons();
    }

    /// <summary>
    /// Event delegate method fired when the checked state in the <seealso cref="SkipUpgradeCheckBox"/> changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void SkipUpgradeCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      if (!UpgradeExternalPanel.Visible)
      {
        return;
      }

      ResetCheckCredentialsState(true);
      BackupDatabaseCheckBox.Checked = !SkipUpgradeCheckBox.Checked && BackupDatabase;
      BackupDatabaseCheckBox.Enabled = !SkipUpgradeCheckBox.Checked;
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
        case nameof(ExistingRootPasswordTextBox):
          var sanitizedPassword = ExistingRootPasswordTextBox.Text.Sanitize();
          if (!string.Equals(sanitizedPassword, _validatedPassword))
          {
            ResetCheckCredentialsState(false);
          }

          break;
      }

      return errorMessage;
    }

    /// <summary>
    /// Updates the main panels visibility.
    /// </summary>
    private void UpdatePanelsVisibility()
    {
      var dataFilesPresent = _controller.IsThereServerDataFiles;
      var serverSupportsSelfContainedUpgrade = _controller.ServerVersion.ServerSupportsSelfContainedUpgrade();
      CheckAndUpgradeMainDescriptionPanel.Visible = dataFilesPresent;
      UpgradeExternalPanel.Visible = dataFilesPresent
                                     && !serverSupportsSelfContainedUpgrade;
      SelfContainedUpgradePanel.Visible = dataFilesPresent
                                          && serverSupportsSelfContainedUpgrade;
      SkipUpgradePanel.Visible = dataFilesPresent;
      SkipUpgradeCheckBox.Enabled = _controller.Settings.SystemTablesUpgraded != SystemTablesUpgradedType.No;
      captionLabel.Text = Resources.ServerReconfigCheckAndUpgradeDatabase;
      if (serverSupportsSelfContainedUpgrade)
      {
        _credsOk = true;
      }
    }
  }
}
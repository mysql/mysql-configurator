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

using System;
using MySql.Configurator.Base.Classes;
using MySql.Configurator.Base.Enums;
using System.Windows.Forms;
using MySql.Configurator.Core.Server;
using MySql.Configurator.Properties;
using Action = System.Action;

namespace MySql.Configurator.UI.Wizards.ServerConfigPages
{
  /// <summary>
  /// Configuration page to ask for credentials for a database upgrade.
  /// </summary>
  public partial class ServerConfigBackupPage : ConfigWizardPage
  {
    #region Fields

    /// <summary>
    /// The connection result obtained when pressing the Connect button.
    /// </summary>
    private ConnectionResultType _connectionResult;

    /// <summary>
    /// The <seealso cref="ServerConfigurationController"/> used to perform actions.
    /// </summary>
    private readonly ServerConfigurationController _controller;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerConfigBackupPage"/> class.
    /// </summary>
    /// <param name="controller">The <seealso cref="ServerConfigurationController"/> used to perform actions.</param>
    public ServerConfigBackupPage(ServerConfigurationController controller)
    {
      BackupDatabase = true;
      InitializeComponent();
      _connectionResult = ConnectionResultType.None;
      _controller = controller;
    }

    #region Properties

    /// <summary>
    /// Gets a value indicating whether a backup of the database is done before upgrading the system tables.
    /// </summary>
    public bool BackupDatabase { get; private set; }

    /// <summary>
    /// Gets a value indicating if it is allowed to go to the next configuration page.
    /// </summary>
    public override bool NextOk => ((_controller.IsSameDirectoryUpgrade
                                     && ((RunBackupRadioButton.Checked
                                          && _connectionResult == ConnectionResultType.ConnectionSuccess)
                                         || SkipBackupRadioButton.Checked))
                                    || !_controller.IsSameDirectoryUpgrade)
                                   && base.NextOk;

    #endregion Properties

    /// <summary>
    /// Activates this instance.
    /// </summary>
    public override void Activate()
    {
      CredentialsPanel.Visible = !_controller.RootUserCredentialsSet;
      base.Activate();
    }

    /// <summary>
    /// Executes actions performed when the Next button is clicked.
    /// </summary>
    /// <returns><c>true</c> if it the configuration should proceed to the next panel, <c>false</c> otherwise.</returns>
    public override bool Next()
    {
      _controller.IsBackupDatabaseStepNeeded = RunBackupRadioButton.Checked;
      _controller.UpdateUpgradeConfigSteps();
      if (!_controller.RootUserCredentialsSet)
      {
        _controller.Settings.ExistingRootPassword = PasswordTextBox.Text;
      }

      return base.Next();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="BackupDatabaseCheckBox"/> checked property changes.
    /// </summary>
    /// <param name="sender">The sender object.</param>
    /// <param name="e">The event arguments.</param>
    private void BackupDatabaseCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      BackupDatabase = RunBackupRadioButton.Checked;
      ValidationsErrorProvider.Clear();
      ValidatedHandler(sender, e);
    }

    /// <summary>
    /// Event delegate method fired when the Connect button is clicked.
    /// </summary>
    /// <param name="sender">The sender object.</param>
    /// <param name="e">The event arguments.</param>
    private void ConnectButton_Click(object sender, EventArgs e)
    {
      Action action;
      action = delegate
      {
        ResultLabel.Text = Resources.StartingServerAndTestingConnection;
        var providerProperties = new ErrorProviderProperties(Resources.StartingServerAndTestingConnection, Resources.Config_InProgressIcon, true);
        ConnectionErrorProvider.SetProperties(ConnectButton, providerProperties);
        _connectionResult = MySqlServerInstance.CanConnect(_controller, out string errorMessage, PasswordTextBox.Text, true, true);
        providerProperties.ErrorMessage = string.IsNullOrEmpty(errorMessage)
          ? _connectionResult.GetDescription()
          : errorMessage;
        providerProperties.ErrorIcon = _connectionResult == ConnectionResultType.ConnectionSuccess
          ? Resources.Config_DoneIcon
          : Resources.Config_ErrorIcon;
        ConnectionErrorProvider.SetProperties(ConnectButton, providerProperties);
        ResultLabel.Text = providerProperties.ErrorMessage;
        UpdateButtons();
      };

      ExecuteLongRunningOperation(action);
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
        case nameof(PasswordTextBox):
          _connectionResult = ConnectionResultType.None;
          ConnectionErrorProvider.Clear();
          ResultLabel.Text = string.Empty;
          break;
      }

      return errorMessage;
    }
  }
}

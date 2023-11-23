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
using System.IO;
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Core.Classes.MySql;
using MySql.Configurator.Core.Controllers;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Core.IniFile;
using MySql.Configurator.Core.Product;
using MySql.Configurator.Core.Wizard;
using MySql.Configurator.Dialogs;
using MySql.Configurator.Properties;

namespace MySql.Configurator.Wizards.Server
{
  /// <summary>
  /// A Wizard page that shows existing MySQL Server installations and offers users options to reuse an existing server's data directory or not.
  /// </summary>
  public partial class ServerConfigServerInstallationsPage : ConfigWizardPage
  {
    #region Fields

    /// <summary>
    /// A configuration controller for a MySQL Server configuration.
    /// </summary>
    private readonly ServerConfigurationController _controller;

    /// <summary>
    /// A MySQL Server instance corresponding to an existing server installation differnt to the one being configured.
    /// </summary>
    private LocalServerInstance _existingServerInstallationInstance;

    /// <summary>
    /// A dictionary containing configuration wizard pages and their corresponding original visibility values.
    /// </summary>
    private Dictionary<ConfigWizardPage, bool> _originalPagesVisibility;

    /// <summary>
    /// A flag indicating whether the root password has been validated.
    /// </summary>
    private bool _rootPasswordOk;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerConfigAdvancedOptionsPage"/> class.
    /// </summary>
    /// <param name="controller">A <see cref="ServerConfigurationController"/> instance.</param>
    public ServerConfigServerInstallationsPage(ServerConfigurationController controller)
    {
      InitializeComponent();
      _controller = controller;
      _rootPasswordOk = false;
      _originalPagesVisibility = new Dictionary<ConfigWizardPage, bool>();
      PortTextBox.Text = BaseServerSettings.DEFAULT_PORT.ToString();
      PipeOrSharedMemoryNameTextBox.Text = MySqlServerSettings.DEFAULT_PIPE_OR_SHARED_MEMORY_NAME;
    }

    #region Properties

    /// <summary>
    /// Gets a value indicating whether the Connect button should be enabled.
    /// </summary>
    public bool ConnectEnabled => ((ProtocolComboBox.SelectedIndex == 0
                                    && PortTextBox.Text.Length > 0)
                                   || (ProtocolComboBox.SelectedIndex > 0
                                       && PipeOrSharedMemoryNameTextBox.Text.Length > 0))
                                  && !ValidationsErrorProvider.HasErrors();

    /// <summary>
    /// Gets a value indicating whether the Next button in the wizard page is enabled.
    /// </summary>
    public override bool NextOk => ((ReplaceServerInstallationRadioButton.Checked
                                     && VersionTextBox.Text.Length > 0
                                     && InstallDirectoryTextBox.Text.Length > 0
                                     && ExistingDataDirectoryTextBox.Text.Length > 0
                                     && _rootPasswordOk
                                     && _existingServerInstallationInstance != null
                                     && !VersionErrorProvider.HasErrors())
                                    || (SideBySideInstallationRadioButton.Checked
                                        && NewDataDirectoryTextBox.Text.Length > 0))
                                    && base.NextOk;

    #endregion Properties

    public override void Activate()
    {
      NewDataDirectoryTextBox.Text = _controller.DataDirectory;
      UpdateExistingServerInstallationInstance(_controller.ExistingServerInstallationInstance, _controller, true);
      FireAllValidations();
      base.Activate();
    }

    public override bool Next()
    {
      if (ReplaceServerInstallationRadioButton.Checked)
      {
        _controller.ConfigurationType = ConfigurationType.Upgrade;
        var dataDirectory = new DirectoryInfo(ExistingDataDirectoryTextBox.Text);
        _controller.Settings.DataDirectory = dataDirectory.Parent.FullName;
        _controller.ExistingServerInstallationInstance = _existingServerInstallationInstance;
        _controller.Settings.ExistingRootPassword = RootPasswordTextBox.Text;
        _controller.IsRemoveExistingServerInstallationStepNeeded = true;

        // Find if existing instance is configured as service.
        var serviceNames = MySqlServiceControlManager.FindServiceNamesWithBaseDirectory(_existingServerInstallationInstance.BaseDir);
        if (serviceNames.Length > 0)
        {
          _existingServerInstallationInstance.ServiceName = serviceNames[0];
          _controller.Settings.ServiceName = _existingServerInstallationInstance.ServiceName;
          _controller.Settings.ConfigureAsService = true;
        }

        _controller.IsServiceRenameNeeded = _existingServerInstallationInstance.IsServiceNameDefault(
          _existingServerInstallationInstance.ServiceName,
          _existingServerInstallationInstance.ServerVersion);
        _controller.IsDataDirectoryRenameNeeded = DataDirectoryRenameWarningProvider.HasErrors();
        DetermineExistingServerPersistedVariablesToReset();
      }
      else
      {
        _controller.ConfigurationType = ConfigurationType.New;
        _controller.Settings.DataDirectory = NewDataDirectoryTextBox.Text;
        _controller.ExistingServerInstallationInstance = null;
        _controller.IsDataDirectoryRenameNeeded = false;
        _controller.IsRemoveExistingServerInstallationStepNeeded = false;
      }

      _controller.Settings.IniDirectory = _controller.Settings.DataDirectory;
      _controller.Settings.SecureFilePrivFolder = Path.Combine(_controller.Settings.IniDirectory, MySqlServerSettings.SECURE_FILE_PRIV_DIRECTORY);
      if (Wizard is ConfigWizard.ConfigWizard configWizard)
      {
        configWizard.ConfigurationType = _controller.ConfigurationType;
      }

      RefreshConfigurationPagesVisibility();
      var mainForm = FindForm() as MainForm;
      if (mainForm != null)
      {
        mainForm.DataDirectoryLabel.Text = _controller.IsDataDirectoryRenameNeeded
          ? $"Data Directory: {_controller.Settings.DataDirectory} -> {$"MySQL Server {_controller.ServerVersion.ToString(2)}"}"
          : $"Data Directory: {_controller.Settings.DataDirectory}";
        mainForm.VersionLabel.Text = _controller.ConfigurationType == ConfigurationType.Upgrade
          ? $"MySQL Server {VersionTextBox.Text} -> {_controller.Package.Version}"
          : $"MySQL Server {_controller.Package.Version}";
        mainForm.ConfigurationTypeLabel.Text = $"{_controller.ConfigurationType}{(_controller.ConfigurationType == ConfigurationType.New ? " configuration" : string.Empty)}";
        mainForm.StatusStrip.Refresh();
      }

      _controller.UpdateConfigurationSteps();
      return base.Next();
    }

    /// <summary>
    /// Updates the state of the buttons on the wizard.
    /// </summary>
    protected override void UpdateButtons()
    {
      ConnectButton.Enabled = ConnectEnabled;
      base.UpdateButtons();
    }

    /// <summary>
    /// Contains calls to methods that validate the given control's value.
    /// </summary>
    /// <returns>An error message or <c>null</c> / <see cref="string.Empty"/> if everything is valid.</returns>
    protected override string ValidateFields()
    {
      string errorMessage = base.ValidateFields();
      if (ErrorProviderControl.Enabled)
      {
        ErrorProviderControl.Text = ErrorProviderControl.Text.Trim();
        switch (ErrorProviderControl.Name)
        {
          case nameof(SideBySideInstallationRadioButton):
          case nameof(NewDataDirectoryTextBox):
            if (SideBySideInstallationRadioButton.Checked)
            {
              var newDataDirectory = NewDataDirectoryTextBox.Text.Trim();
              errorMessage = Core.Classes.Utilities.ValidateAbsoluteFilePath(newDataDirectory, true);
              if (string.IsNullOrEmpty(errorMessage)
                  && _controller.HasDataSubDirectoryWithFiles(newDataDirectory))
              {
                errorMessage = Resources.DataDirectoryNotEmptyError;
              }
            }

            break;

          case nameof(PipeOrSharedMemoryNameTextBox):
            if (ReplaceServerInstallationRadioButton.Checked)
            {
              errorMessage = MySqlServerInstance.ValidatePipeOrSharedMemoryName(ProtocolComboBox.SelectedIndex == 1, PipeOrSharedMemoryNameTextBox.Text.Trim());
            }

            break;

          case nameof(PortTextBox):
            if (ReplaceServerInstallationRadioButton.Checked)
            {
              errorMessage = MySqlServerInstance.ValidatePortNumber(ErrorProviderControl.Text, false);
            }

            break;
        }
      }

      return errorMessage;
    }

    /// <summary>
    /// Determines if there are persisted system variables that have been removed on any version up to the one being configured, and if so sets those variables for resetting.
    /// </summary>
    private void DetermineExistingServerPersistedVariablesToReset()
    {
      if (_existingServerInstallationInstance == null)
      {
        return;
      }

      // If the existing MySQL Server installation's version supports persisted variables, fetch them from the performance_schema and see if there are
      // any removed variables from the version where persisting variables was first suppoerted to the version of the server being configured.
      var persistedVariables = new List<string>();
      if (_existingServerInstallationInstance.ServerVersion.ServerSupportsPersistedSystemVariables())
      {
        var persistedVariablesTable = _existingServerInstallationInstance.ExecuteQuery("SELECT * FROM performance_schema.persisted_variables;", out var error);
        if (!string.IsNullOrEmpty(error))
        {
          Logger.LogError(string.Format(Resources.PersistedVariablesQueryError, error));
        }
        else
        {
          // Here we DO NOT use the existing server version, but the version of the server being configured to determine all of the variables removed
          // up to that version.
          var removedVariables = _controller.ServerVersion.GetUnsupportedPersistedServerVariables();
          foreach (System.Data.DataRow dataRow in persistedVariablesTable.Rows)
          {
            var persistedVariableName = dataRow["VARIABLE_NAME"].ToString();
            if (removedVariables.Contains(persistedVariableName))
            {
              persistedVariables.Add(persistedVariableName);
            }
          }
        }

        if (persistedVariables.Count > 0)
        {
          _controller.PersistedVariablesToReset = persistedVariables;
        }
      }
    }

    /// <summary>
    /// Refreshes the visibility of configuration pages depening on the selections of this page.
    /// </summary>
    private void RefreshConfigurationPagesVisibility()
    {
      foreach (var configPage in _controller.Pages)
      {
        if (configPage is ServerConfigServerInstallationsPage
            || configPage is ServerConfigSecurityPage)
        {
          continue;
        }

        if (configPage is ServerConfigBackupPage)
        {
          configPage.PageVisible = ReplaceServerInstallationRadioButton.Checked;
          continue;
        }

        if (!_originalPagesVisibility.ContainsKey(configPage))
        {
          _originalPagesVisibility.Add(configPage, configPage.PageVisible);
        }

        configPage.PageVisible = ReplaceServerInstallationRadioButton.Checked ? false : _originalPagesVisibility[configPage];
      }

      Parent.Refresh();
      if (!(Wizard is ConfigWizard.ConfigWizard configWizard))
      {
        return;
      }

      configWizard.RefreshSideBar();
    }

    /// <summary>
    /// Resets the status of the connection test so it has to be performed again.
    /// </summary>
    private void ResetConnectionTest()
    {
      ConnectionErrorProvider.Clear();
      _rootPasswordOk = false;
      UpdateExistingServerInstallationInstance(null, null);
      ConnectButton.Enabled = ConnectEnabled;
    }

    /// <summary>
    /// Updates the existing MySQL Server installation instance and the corresponding controls with related values.
    /// </summary>
    /// <param name="serverInstance">A <see cref="MySqlServerInstance"/> instance.</param>
    /// <param name="updateConnectionValues">Flag indicating whether controls that show connection values are updated or not.</param>
    private void UpdateExistingServerInstallationInstance(MySqlServerInstance serverInstance, ServerConfigurationController controller, bool updateConnectionValues = false)
    {
      if (updateConnectionValues)
      {
        var connectionProtocolValue = serverInstance != null ? (int)serverInstance.ConnectionProtocol : 1;
        ProtocolComboBox.SelectedIndex = connectionProtocolValue == 2 ? 1 : connectionProtocolValue == 4 ? 2 : 0;
        if (serverInstance != null)
        {
          PortTextBox.Text = serverInstance?.Port.ToString();
          PipeOrSharedMemoryNameTextBox.Text = serverInstance?.PipeOrSharedMemoryName;
          RootPasswordTextBox.Text = serverInstance?.UserAccount?.Password;
        }
      }

      VersionTextBox.Text = serverInstance?.ServerVersion?.ToString();
      var versionErrorMessage = serverInstance != null ?
        _controller.Package.NormalizedVersion.ServerSupportsInPlaceUpgrades(serverInstance.ServerVersion) 
        : null;
      var errorInVersionTextbox = serverInstance != null && serverInstance.ServerVersion == null;
      ValidationsErrorProvider.SetProperties(VersionTextBox, new ErrorProviderProperties(errorInVersionTextbox 
          ? Resources.ServerInstanceGetServerVersionError 
          : string.Empty));
      if (!errorInVersionTextbox
          && !string.IsNullOrEmpty(versionErrorMessage))
      {
        if (!versionErrorMessage.Equals(Resources.UpgradeHigherVersionError, StringComparison.InvariantCultureIgnoreCase)
              && !versionErrorMessage.Equals(Resources.SameVersionError, StringComparison.InvariantCultureIgnoreCase))
        {
          VersionWarningProvider.SetProperties(VersionTextBox, new ErrorProviderProperties(versionErrorMessage, Resources.warning_sign_icon));
          VersionErrorProvider.Clear();
        }
        else
        {
          VersionErrorProvider.SetProperties(VersionTextBox, new ErrorProviderProperties(versionErrorMessage, Resources.Config_ErrorIcon));
          VersionWarningProvider.Clear();
        }
      }
      else
      {
        VersionErrorProvider.Clear();
        VersionWarningProvider.Clear();
      }

      InstallDirectoryTextBox.Text = serverInstance?.BaseDir;
      ValidationsErrorProvider.SetProperties(InstallDirectoryTextBox, new ErrorProviderProperties(serverInstance != null &&  string.IsNullOrEmpty(serverInstance.BaseDir) ? Resources.ServerInstanceFailedToRetrieveBaseDir : string.Empty));
      ExistingDataDirectoryTextBox.Text = serverInstance?.DataDir;
      DataDirectoryRenameWarningProvider.SetProperties(ExistingDataDirectoryTextBox, new ErrorProviderProperties(serverInstance != null 
        && serverInstance.IsDataDirNameDefault(_controller.ServerVersion)
          ? string.Format(Resources.ExistingDataDirectoryIsDefaultAndWillBeRenamed, $"MySQL Server {_controller.ServerVersion.ToString(2)}") 
          : string.Empty, Resources.warning_sign_icon));
      ValidationsErrorProvider.SetProperties(ExistingDataDirectoryTextBox, new ErrorProviderProperties(serverInstance != null &&  string.IsNullOrEmpty(serverInstance.DataDir) ? Resources.ServerInstanceFailedToRetrieveDataDir : string.Empty));
      
      if (!string.IsNullOrEmpty(serverInstance?.DataDir))
      {
        string dataDirectory = null;
        if (!Directory.Exists(serverInstance?.DataDir))
        {
          return;
        }

        dataDirectory = new DirectoryInfo(serverInstance?.DataDir).Parent.FullName;
        var defaultConfigFile = Path.Combine(dataDirectory, BaseServerSettings.DEFAULT_CONFIG_FILE_NAME);
        var alternateConfigFile = Path.Combine(dataDirectory, BaseServerSettings.ALTERNATE_CONFIG_FILE_NAME);
        ExistingConfigFilePathTextBox.Text = File.Exists(defaultConfigFile)
          ? defaultConfigFile
          : File.Exists(alternateConfigFile)
            ? alternateConfigFile
            : null;
      }
      else
      {
        ExistingConfigFilePathTextBox.Text = string.Empty;
      }

      // Set existing instance relevant properties for rollback.
      if (serverInstance != null)
      {
        if (!errorInVersionTextbox)
        {
          controller.ServerVersion = serverInstance.ServerVersion;
          controller.Package.Version = controller.ServerVersion.ToString();
        }

        if (!string.IsNullOrEmpty(serverInstance.BaseDir))
        {
          var serviceNames = MySqlServiceControlManager.FindServiceNamesWithBaseDirectory(_existingServerInstallationInstance.BaseDir);
          if (serviceNames.Length > 0)
          {
            controller.Settings.ServiceName = serviceNames[0];
            controller.Settings.ConfigureAsService = true;
          }
        }

        // Load ini template and set data dir and error log paths.
        var iniFile = new IniFileEngine(ExistingConfigFilePathTextBox.Text).Load();
        controller.Settings.DataDirectory = new DirectoryInfo(iniFile.FindValue("mysqld", "datadir", false)).Parent.FullName;
        controller.Settings.ErrorLogFileName = iniFile.FindValue("mysqld", "log-error", false);
      }
    }

    #region Event Handlers

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
      base.ValidatedHandler(sender, e);
    }

    private void ConnectButton_Click(object sender, EventArgs e)
    {
      _rootPasswordOk = false;
      var providerProperties = new ErrorProviderProperties(Resources.ConnectionTestingText, Resources.Config_InProgressIcon, true);
      try
      {
        ReplaceInstallationControlsPanel.Enabled = false;
        Cursor = Cursors.WaitCursor;
        ConnectionErrorProvider.SetProperties(ConnectButton, providerProperties);
        if (!uint.TryParse(PortTextBox.Text.Trim(), out var port))
        {
          port = MySqlServerInstance.DEFAULT_PORT;
        }

        // Create controller and local server instance for running instance.
        var controller = new ServerConfigurationController();
        controller.Package = ProductManager.LoadGenericPackage();
        controller.Init();
        switch (ProtocolComboBox.SelectedIndex)
        {
          case 0:
            controller.Settings.Port = port;
            _controller.Settings.Port = controller.Settings.Port;
            controller.Settings.EnableTcpIp = true;
            _controller.Settings.EnableTcpIp = controller.Settings.EnableTcpIp;
            break;

          case 1:
            controller.Settings.PipeName = PipeOrSharedMemoryNameTextBox.Text.Trim();
            _controller.Settings.PipeName = controller.Settings.PipeName;
            controller.Settings.EnableNamedPipe = true;
            _controller.Settings.EnableNamedPipe = controller.Settings.EnableNamedPipe;
            break;

          case 2:
            controller.Settings.SharedMemoryName = PipeOrSharedMemoryNameTextBox.Text.Trim();
            _controller.Settings.SharedMemoryName = controller.Settings.SharedMemoryName;
            controller.Settings.EnableSharedMemory = true;
            _controller.Settings.EnableSharedMemory = controller.Settings.EnableSharedMemory;
            break;
        }

        _existingServerInstallationInstance = new LocalServerInstance(controller, null, port);
        _existingServerInstallationInstance.UserAccount.Password = RootPasswordTextBox.Text;
        _existingServerInstallationInstance.ConnectionProtocol = ProtocolComboBox.SelectedIndex == 0
                                                                 ? Data.MySqlClient.MySqlConnectionProtocol.Tcp
                                                                 : (ProtocolComboBox.SelectedIndex == 1
                                                                    ? Data.MySqlClient.MySqlConnectionProtocol.NamedPipe
                                                                    : Data.MySqlClient.MySqlConnectionProtocol.SharedMemory);
        _existingServerInstallationInstance.PipeOrSharedMemoryName = ProtocolComboBox.SelectedIndex > 0
                                                                     ? PipeOrSharedMemoryNameTextBox.Text.Trim()
                                                                     : null;
        
        var connectionResult = _existingServerInstallationInstance.CanConnectWithoutDedicatedProcess();
        _rootPasswordOk = connectionResult == ConnectionResultType.ConnectionSuccess;
        ExistingConfigFileBrowseButton.Enabled = _rootPasswordOk;
        UpdateExistingServerInstallationInstance(_rootPasswordOk ? _existingServerInstallationInstance : null, controller);
        providerProperties.ErrorMessage = connectionResult.GetDescription();
      }
      catch (Exception ex)
      {
        _rootPasswordOk = false;
        providerProperties.ErrorMessage = ex.Message;
      }
      finally
      {
        Cursor = Cursors.Default;
        ReplaceInstallationControlsPanel.Enabled = true;
        providerProperties.ErrorIcon = _rootPasswordOk
          ? Resources.Config_DoneIcon
          : Resources.Config_ErrorIcon;
        ConnectionErrorProvider.SetProperties(ConnectButton, providerProperties);
        ConnectButton.Enabled = !_rootPasswordOk;
        UpdateButtons();
      }
    }

    private void ExistingConfigFileBrowseButton_Click(object sender, EventArgs e)
    {
      if (!string.IsNullOrEmpty(ExistingConfigFilePathTextBox.Text))
      {
        ConfigFileDialog.InitialDirectory = Path.GetDirectoryName(ExistingConfigFilePathTextBox.Text);
      }
      
      if (ConfigFileDialog.ShowDialog() == DialogResult.OK)
      {
        ExistingConfigFilePathTextBox.Text = ConfigFileDialog.FileName;
      }
    }

    private void InstallationRadioButtonsCheckedChanged(object sender, EventArgs e)
    {
      ReplaceInstallationControlsPanel.Enabled = ReplaceServerInstallationRadioButton.Checked;
      ReplaceInstallationControlsPanel.Visible = ReplaceServerInstallationRadioButton.Checked;
      SideBySideInstallationControlsPanel.Enabled = SideBySideInstallationRadioButton.Checked;
      SideBySideInstallationControlsPanel.Visible = SideBySideInstallationRadioButton.Checked;

      ValidatedHandler(sender, e);
    }

    private void NewDataDirectoryBrowseButton_Click(object sender, EventArgs e)
    {
      DataDirectoryBrowserDialog.SelectedPath = NewDataDirectoryTextBox.Text;
      var dialogResult = DataDirectoryBrowserDialog.ShowDialog();
      if (dialogResult == DialogResult.OK)
      {
        NewDataDirectoryTextBox.Text = DataDirectoryBrowserDialog.SelectedPath;
      }
    }

    private void ProtocolComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      PortLabel.Visible = PortTextBox.Visible = ProtocolComboBox.SelectedIndex == 0;
      NameLabel.Visible = PipeOrSharedMemoryNameTextBox.Visible = ProtocolComboBox.SelectedIndex > 0;
      ResetConnectionTest();
    }

    #endregion Event Handlers
  }
}

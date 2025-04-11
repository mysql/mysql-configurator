/* Copyright (c) 2023, 2025, Oracle and/or its affiliates.

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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using MySql.Data.MySqlClient;
using MySql.Configurator.Base.Classes;
using MySql.Configurator.Base.Enums;
using MySql.Configurator.Properties;
using Shell32;
using System.Text.RegularExpressions;
using MySql.Configurator.Core.Ini;
using MySql.Configurator.Core.Logging;
using MySql.Configurator.Core.Controllers;
using MySql.Configurator.Core.Settings;
using MySql.Configurator.UI.Wizards.ServerConfigPages;
using MySql.Configurator.Core.MSI;
using MySql.Configurator.UI.Wizards;
using System.Threading.Tasks;
using System.Windows.Forms;
using TS = System.Threading.Tasks.TaskScheduler;

namespace MySql.Configurator.Core.Server
{
  public class ServerConfigurationController
  {
    #region Constants

    /// <summary>
    /// The name of the Server's admin tools executable filename.
    /// </summary>
    public const string ADMIN_TOOL_EXECUTABLE_FILENAME = "mysqladmin.exe";

    /// <summary>
    /// The binary directory name.
    /// </summary>
    public const string BINARY_DIRECTORY_NAME = "bin";

    /// <summary>
    /// The name of the Server's client executable filename.
    /// </summary>
    public const string CLIENT_EXECUTABLE_FILENAME = "mysql.exe";

    /// <summary>
    /// The default time a configuration step will take to produce a timeout.
    /// </summary>
    public const int DEFAULT_TIMEOUT_IN_SECONDS = 30;

    /// <summary>
    /// The base name to use for backup files.
    /// </summary>
    private const string DATABASE_BACKUP_BASE_FILE_NAME = @"mysql_dump";

    /// <summary>
    /// The name of the directory used to save the database/s backups.
    /// </summary>
    private const string DATABASE_BACKUP_DIRECTORY = @"Backup";

    /// <summary>
    /// The name of the Server's dump tool executable filename.
    /// </summary>
    public const string DUMP_TOOL_EXECUTABLE_FILENAME = "mysqldump.exe";

    /// <summary>
    /// The value used to indicate that all users should have full access to a named pipe.
    /// </summary>
    public const string NAMED_PIPE_FULL_ACCESS_TO_ALL_USERS = "*everyone*";

    /// <summary>
    /// The name of the Server's executable filename.
    /// </summary>
    public const string SERVER_EXECUTABLE_FILENAME = "mysqld.exe";

    /// <summary>
    /// The name of the Server's executable filename without its extension.
    /// </summary>
    public const string SERVER_EXECUTABLE_FILENAME_NO_EXTENSION = "mysqld";

    /// <summary>
    /// The name of the Server's upgrade tool executable filename.
    /// </summary>
    public const string UPGRADE_TOOL_EXECUTABLE_FILENAME = "mysql_upgrade.exe";

    #endregion

    #region General Fields

    /// <summary>
    /// Defines the assigned configuration timeout.
    /// </summary>
    private int _configurationTimeout;

    /// <summary>
    /// Defines the path to the data directory.
    /// </summary>
    private string _dataDirectory;

    /// <summary>
    /// Defines if the timer that checks for timeouts during a configuration step is enabled or not.
    /// </summary>
    private bool _inTimerCallback;

    /// <summary>
    /// Controller used to revert failed configuration steps.
    /// </summary>
    private ServerRevertController _revertController;

    /// <summary>
    /// The server settings.
    /// </summary>
    protected MySqlServerSettings _settings;

    /// <summary>
    /// The timer used to keep track of time specific operations.
    /// </summary>
    private System.Threading.Timer _timer;

    /// <summary>
    /// A flag to mark the usage of statuses in the log.
    /// </summary>
    private bool _useStatusesList;

    #endregion

    #region Configuration Step Fields

    /// <summary>
    /// The server backup configuration step.
    /// </summary>
    private ConfigurationStep _backupDatabaseStep;

    /// <summary>
    /// The list of base configuration steps.
    /// </summary>
    private List<ConfigurationStep> _classicConfigurationSteps;

    /// <summary>
    /// The configuration step used for creating or removing example databases. 
    /// </summary>
    private ConfigurationStep _createRemoveExampleDatabasesStep;

    /// <summary>
    /// The configuration step used for initializing the server database.
    /// </summary>
    private ConfigurationStep _initializeServerConfigurationStep;

    /// <summary>
    /// The configuration step used for removing the existing server installation.
    /// </summary>
    private ConfigurationStep _removeExistingServerInstallationStep;

    /// <summary>
    /// The configuration step used for renaming the data directory of the existing server installation.
    /// </summary>
    private ConfigurationStep _renameExistingDataDirectoryStep;

    /// <summary>
    /// The list of configuration steps used during a self-contained upgrade.
    /// </summary>
    private List<ConfigurationStep> _selfContainedUpgradeSteps;

    /// <summary>
    /// The configuration step used for starting and upgrading a server installation.
    /// </summary>
    private ConfigurationStep _startAndUpgradeServerConfigStep;

    /// <summary>
    /// The configuration step used for starting the server.
    /// </summary>
    private ConfigurationStep _startServerConfigurationStep;

    /// <summary>
    /// The configuration step used for stopping the server during an upgrade.
    /// </summary>
    private ConfigurationStep _stopExistingServerInstanceStep;

    /// <summary>
    /// The configuration step used for stopping the server.
    /// </summary>
    private ConfigurationStep _stopServerConfigurationStep;

    /// <summary>
    /// The configuration step used for updating access to the data directory.
    /// </summary>
    private ConfigurationStep _updateAccessPermissions;

    /// <summary>
    /// The configuration step used for updating the authentication plugin of the root user.
    /// </summary>
    private ConfigurationStep _updateAuthenticationPluginStep;

    /// <summary>
    /// The configuration step used for updating the enterprise firewall plugin.
    /// </summary>
    private ConfigurationStep _updateEnterpriseFirewallPluginConfigStep;

    /// <summary>
    /// The configuration step used for resetting the persisted variables.
    /// </summary>
    private ConfigurationStep _resetPersistedVariablesStep;

    /// <summary>
    /// The configuration step used for enabling the server to run as a process.
    /// </summary>
    private ConfigurationStep _updateProcessStep;

    /// <summary>
    /// The configuration step used for settting the access to the data directory.
    /// </summary>
    private ConfigurationStep _updateSecurityStep;

    /// <summary>
    /// The configuration step used for creating and updating the start menu links.
    /// </summary>
    private ConfigurationStep _updateStartMenuLinksStep;

    /// <summary>
    /// The configuration step used for creating custom MySQL users.
    /// </summary>
    private ConfigurationStep _updateUsersStep;

    /// <summary>
    /// The configuration step used for creating and updating the Windows firewall rules.
    /// </summary>
    private ConfigurationStep _updateWindowsFirewallRulesStep;

    /// <summary>
    /// The configuration step used for creating and updating the MySQL Windows service.
    /// </summary>
    private ConfigurationStep _updateWindowsServiceStep;

    /// <summary>
    /// The configuration step used for upgrading a server installation.
    /// </summary>
    private ConfigurationStep _upgradeStandAloneServerStep;

    /// <summary>
    /// The server installation that is being upgraded.
    /// </summary>
    private MySqlServerInstance _upgradingInstance;

    /// <summary>
    /// The configuration step used for creating and updating the server options file.
    /// </summary>
    private ConfigurationStep _writeConfigurationFileStep;

    #endregion

    #region Remove Step Fields

    /// <summary>
    /// Remove step used to delete the server configuration file.
    /// </summary>
    private RemoveStep _deleteConfigurationFileStep;

    /// <summary>
    /// Remove step used to delete the data directory.
    /// </summary>
    private RemoveStep _deleteDataDirectoryStep;

    /// <summary>
    /// Remove step used to delete the Windows service created for the server installation.
    /// </summary>
    private RemoveStep _deleteServiceStep;

    /// <summary>
    /// List of remove steps used for a classic/stand-alone server configuration.
    /// </summary>
    private List<RemoveStep> _classicServerRemoveSteps;

    /// <summary>
    /// The path where the Connector/C++ client is stored at.
    /// </summary>
    public string _cPPClientPath;

    /// <summary>
    /// List of firewall rules found on this computer.
    /// </summary>
    private List<string> _firewallRulesList;

    /// <summary>
    /// Remove step used to remove the firewall rules.
    /// </summary>
    private RemoveStep _removeFirewallRuleStep;

    /// <summary>
    /// Remove step used to stop the server.
    /// </summary>
    private RemoveStep _stopServerRemoveStep;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerConfigurationController"/> class.
    /// </summary>
    public ServerConfigurationController()
    {
      _configurationTimeout = DEFAULT_TIMEOUT_IN_SECONDS;
      _upgradingInstance = null;
      _firewallRulesList = new List<string>();
      ConfigurationSummaryText = null;
      FullControlDictionary = new Dictionary<SecurityIdentifier, string>();
      Pages = new List<ConfigWizardPage>();
      UpdateDataDirectoryPermissions = true;
      CurrentState = ConfigState.ConfigurationRequired;
      UseStatusesList = false;
      Logger.LogInformation("Product configuration controller created.");
      ShowAdvancedOptions = false;
      if (LoadRoles())
      {
        return;
      }

      RolesDefined = null;
      TemporaryServerUser = null;
      ExistingServerInstallationInstance = null;
      PersistedVariablesToReset = new List<string>();
    }

    #region Events

    public event EventHandler ConfigurationEnded;

    public event EventHandler ConfigurationStarted;

    public event ConfigurationStatusChangeEventHandler ConfigurationStatusChanged;

    public event EventHandler ConfigureTimedOut;

    #endregion Events

    #region Properties

    /// <summary>
    /// Gets the file name for a backup of the database.
    /// </summary>
    public static string BackupFileName => $"{DATABASE_BACKUP_BASE_FILE_NAME}-{DateTime.Now.ToString("s").Replace(":", ".")}.sql";

    /// <summary>
    /// Gets a <see cref="CancellationToken"/> to signal a user's cancellation.
    /// </summary>
    public CancellationToken CancellationToken => CancellationTokenSource.Token;

    /// <summary>
    /// Gets or sets the cancellation token source.
    /// </summary>
    public CancellationTokenSource CancellationTokenSource { get; private set; }

    /// <summary>
    /// Gets or sets the list of configuration steps.
    /// </summary>
    public List<ConfigurationStep> ConfigurationSteps { get; protected set; }

    /// <summary>
    /// Gets or sets the configuration summary text.
    /// </summary>
    public string ConfigurationSummaryText { get; set; }

    /// <summary>
    /// Gets or sets the server confiuration type.
    /// </summary>
    public ConfigurationType ConfigurationType { get; set; }

    /// <summary>
    /// Gets or sets the state of the current configuration.
    /// </summary>
    public ConfigState CurrentState { get; protected set; }

    /// <summary>
    /// Gets or sets the current step.
    /// </summary>
    public BaseStep CurrentStep { get; protected set; }

    /// <summary>
    /// Gets or sets the server data directory.
    /// </summary>
    public string DataDirectory
    {
      get => Settings.DataDirectory;
      set => Settings.DataDirectory = value;
    }

    /// <summary>
    /// Gets a value indicating if there was a change of the default authentication plugin during the configuration.
    /// </summary>
    //public bool DefaultAuthenticationPluginChanged => OldSettings != null
    //                                                  && OldSettings.DefaultAuthenticationPlugin != Settings.DefaultAuthenticationPlugin;
    public bool DefaultAuthenticationPluginChanged => false;

    /// <summary>
    /// Gets the file path for the server error log.
    /// </summary>
    public string ErrorLogFilePath => Path.Combine(Settings.DataDirectory, "Data", Settings.ErrorLogFileName);

    /// <summary>
    /// Gets or sets the installed state of the example databases.
    /// </summary>
    public Dictionary<string, string> ExampleDatabasesStatus { get; set; }

    /// <summary>
    /// Gets the path to the example databases scripts.
    /// </summary>
    public string ExampleDatabasesLocation { get; private set; }

    /// <summary>
    /// Gets or sets a MySQL Server instance corresponding to an existing server installation different to the one being configured.
    /// </summary>
    public MySqlServerInstance ExistingServerInstallationInstance { get; set; }

    /// <summary>
    /// Gets the list of firewall rules on this computer.
    /// </summary>
    public List<string> FirewallRulesList {
      get
      {
        if (_firewallRulesList.Count > 0)
        {
          return _firewallRulesList;
        }

        try
        {
          var firewallPolicyType = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
          var firewallPolicy = Activator.CreateInstance(firewallPolicyType) as dynamic;
          var rules = firewallPolicy.Rules as IEnumerable;
          _firewallRulesList = new List<string>();
          foreach (dynamic rule in rules)
          {
            _firewallRulesList.Add(rule.Name);
          }
        }
        catch (Exception ex)
        {
          Logger.LogWarning(ex.Message);
        }

        return _firewallRulesList;
      }
    }

    /// <summary>
    /// Gets or sets the list of local users or groups that will have full control over the data directory.
    /// </summary>
    public Dictionary<SecurityIdentifier, string> FullControlDictionary { get; set; }

    /// <summary>
    /// Gets or sets the server installation directory.
    /// </summary>
    public string InstallDirectory
    {
      get => Settings.InstallDirectory;
      set => Settings.InstallDirectory = value;
    }

    /// <summary>
    /// Gets a value indicating whether the database is backed up during an upgrade.
    /// </summary>
    public bool IsBackupDatabaseStepNeeded => Settings.BackupData;

    /// <summary>
    /// Gets or sets a value indicating whether the example databases are created.
    /// </summary>
    public bool IsCreateRemoveExamplesDatabasesStepNeeded { get; set; }

    /// <summary>
    /// Gets a value hinting whether the data directory has been initialized and configured.
    /// </summary>
    public bool IsDataDirectoryConfigured => IsThereServerDataFiles
                                             && ServerVersion.ServerSupportsDatabaseInitialization()
                                                || (!string.IsNullOrEmpty(DataDirectory)
                                                    && File.Exists(Path.Combine(DataDirectory, "data", "auto.cnf"))
                                                    && File.Exists(Path.Combine(DataDirectory, "data", Settings.ErrorLogFileName)));

    /// <summary>
    /// Gets or sets a value indicating whether the data directory needs to be renamed to reflect the server series.
    /// </summary>
    public bool IsDataDirectoryRenameNeeded { get; set; }

    /// <summary>
    /// Gets a value indicating if there are configuration files that need to be deleted.
    /// </summary>
    public bool IsDeleteConfigurationFileStepNeeded => (!string.IsNullOrEmpty(InstallDirectory) 
                                                        && File.Exists(Path.Combine(InstallDirectory, GeneralSettingsManager.CONFIGURATOR_SETTINGS_FILE_NAME)))
                                                       || File.Exists(Settings.FullConfigFilePath);

    /// <summary>
    /// Gets a value indicating whether the removal step that deletes the data directory needs to run.
    /// </summary>
    public bool IsDeleteDataDirectoryStepNeeded => IsThereServerDataFiles && RemoveDataDirectory;

    /// <summary>
    /// Gets a value indicating whether the removal step that deletes the Windows Service needs to run.
    /// </summary>
    public bool IsDeleteServiceStepNeeded => MySqlServiceControlManager.ServiceExists(Settings?.ServiceName);

    /// <summary>
    /// Gets a value indicating if there are steps that require to be executed for a server removal.
    /// </summary>
    public bool IsRemovalExecutionNeeded => IsDataDirectoryConfigured
      && (IsDeleteDataDirectoryStepNeeded
          || IsDeleteConfigurationFileStepNeeded
          || IsDeleteServiceStepNeeded
          || IsRemoveFirewallRuleStepNeeded
          || IsStopServerConfigurationStepNeeded);

    /// <summary>
    /// Gets a value indicating whether the removal step that deletes the firewall rules needs to run.
    /// </summary>
    public bool IsRemoveFirewallRuleStepNeeded => FirewallRulesList.Any(rule => rule.Equals($"Port {Settings.MySqlXPort}", StringComparison.InvariantCultureIgnoreCase)
                                                                        || rule.Equals($"Port {Settings.Port}", StringComparison.InvariantCultureIgnoreCase));

    /// <summary>
    /// Gets or sets a value indicating whether an existing MySQL Server installation is going to be removed at the end of the configuration.
    /// </summary>
    public bool IsRemoveExistingServerInstallationStepNeeded { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the upgrade is reusing the existing installation and data directories.
    /// </summary>
    public bool IsSameDirectoryUpgrade { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the service name needs to be renamed to reflect the server series.
    /// </summary>
    public bool IsServiceRenameNeeded { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether the start and upgrade step should be executed.
    /// </summary>
    public bool IsStartAndUpgradeConfigurationStepNeeded => ConfigurationType == ConfigurationType.Upgrade
                                                            && ServerVersion.ServerSupportsSelfContainedUpgrade()
                                                            && IsThereServerDataFiles;

    /// <summary>
    /// Gets a value indicating whether the configuration step that starts the server needs to run.
    /// </summary>
    public bool IsStartServerConfigurationStepNeeded => (((IsThereServerDataFiles
                                                          && !ServerVersion.ServerSupportsSelfContainedUpgrade())
                                                         || (IsWriteIniConfigurationFileNeeded))
                                                        || (!IsThereServerDataFiles
                                                            && IsInitializeServerConfigurationStepNeeded(false)
                                                            && ConfigurationType == ConfigurationType.Upgrade))
                                                        && ConfigurationType != ConfigurationType.Upgrade;

    /// <summary>
    /// Gets a value indicating whether the configuration step that stops the server needs to run.
    /// </summary>
    public bool IsStopServerConfigurationStepNeeded
    {
      get
      {
        var serverInstanceInfo = new MySqlServerInstance(this, ReportStatus);
        serverInstanceInfo.UseOldSettings = ConfigurationType == ConfigurationType.Reconfigure;
        return (ConfigurationType == ConfigurationType.Reconfigure
                && IsStartServerConfigurationStepNeeded
                && (IsStartAndUpgradeConfigurationStepNeeded || serverInstanceInfo.IsRunning));
      }
    }

    /// <summary>
    /// Gets a value indicating whether the extended XML configuration file exists.
    /// </summary>
    public bool IsThereConfigXmlFile => File.Exists(Path.Combine(Settings.IniDirectory, GeneralSettingsManager.CONFIGURATOR_SETTINGS_FILE_NAME));

    /// <summary>
    /// Gets a value indicating whether the data directory exists and contains files (i.e. the database has been initialized).
    /// </summary>
    public bool IsThereServerDataFiles => HasDataSubDirectoryWithFiles(DataDirectory);

    /// <summary>
    /// Gets a value indicating if the authentication plugin should be updated for the root user.
    /// </summary>
    public bool IsUpdateAuthenticationPluginStepNeeded => ConfigurationType == ConfigurationType.Upgrade
                                                          && RootUserAuthenticationPlugin == MySqlAuthenticationPluginType.MysqlNativePassword
                                                          && !ServerVersion.ServerSupportsMySqlNativePasswordAuthPlugin();

    /// <summary>
    /// Gets a value indicating whether the configuration step that updates the access permissions to the data folder needs to run.
    /// </summary>
    public bool IsUpdateServerFilesPermissionsStepNeeded => UpdateDataDirectoryPermissions
                                                            && (ConfigurationType == ConfigurationType.Configure
                                                                || (ConfigurationType == ConfigurationType.Reconfigure
                                                                    || ConfigurationType == ConfigurationType.Upgrade
                                                                    && IsThereServerDataFiles));

    /// <summary>
    /// Gets or sets a value indicating whether the configuration step that updates the Enterprise Firewall plugin needs to run.
    /// </summary>
    public bool IsUpdateEnterpriseFirewallPluginConfigurationStepNeeded => SupportsEnterpriseFirewallConfiguration
                                                                           && OldSettings != null
                                                                           && (Settings.Plugins.IsEnabled("mysql_firewall") && !OldSettings.Plugins.IsEnabled("mysql_firewall")
                                                                               || !Settings.Plugins.IsEnabled("mysql_firewall") && OldSettings.Plugins.IsEnabled("mysql_firewall"));

    /// <summary>
    /// Gets a value indicating whether the configuration step that updates the database security needs to run.
    /// </summary>
    public bool IsUpdateSecurityConfigurationStepNeeded => !IsDataDirectoryConfigured
                                                           || DefaultAuthenticationPluginChanged;

    /// <summary>
    /// Gets a value indicating if updating the Server ID is supported for the current configuration.
    /// </summary>
    public bool IsUpdateServerIdSupported => ConfigurationType == ConfigurationType.Configure;

    /// <summary>
    /// Gets a value indicating whether the configuration step that updates the Start menu links needs to run.
    /// </summary>
    public bool IsUpdateStartMenuLinksConfigurationStepNeeded => (ConfigurationType != ConfigurationType.Reconfigure
                                                                 || IsThereServerDataFiles)
                                                                 && Utilities.ExecutionIsFromMSI(ServerInstallation.Version);

    /// <summary>
    /// Gets a value indicating whether the configuration step that updates user accounts needs to run.
    /// </summary>
    public bool IsUpdateUsersConfigurationStepNeeded => Settings.NewServerUsers.Count > 0;

    /// <summary>
    /// Gets a value indicating whether the configuration step that updates the Windows service needs to run.
    /// </summary>
    public bool IsUpdateWindowsServiceConfigurationStepNeeded => (OldSettings != null
                                                                  && OldSettings.ServiceExists()
                                                                  && ConfigurationType != ConfigurationType.Configure
                                                                  && (!Settings.ConfigureAsService
                                                                      || OldSettings.ServiceName != Settings.ServiceName))
                                                                 || Settings.ConfigureAsService
                                                                    && ConfigurationType != ConfigurationType.Upgrade;

    /// <summary>
    /// Gets a value indicating wheter the configuration step to update settings for the MySQL process needs to run.
    /// </summary>
    public bool IsUpdateProcessConfigurationStepNeeded => !Settings.ConfigureAsService
                                                          && (!CurrentUserHasAccessToLogFile(Settings.ErrorLogFileName)
                                                              || !CurrentUserHasAccessToLogFile(Settings.GeneralQueryLogFileName)
                                                              || !CurrentUserHasAccessToLogFile(Settings.SlowQueryLogFileName)
                                                              || !CurrentUserHasAccessToLogFile(Settings.BinLogFileNameBase));

    /// <summary>
    /// Gets a value indicating whether the configuration step that writes the extended XML configuration file needs to run.
    /// </summary>
    public bool IsWriteExtendedConfigurationFileNeeded => !IsStartAndUpgradeConfigurationStepNeeded
                                                          && (!IsThereConfigXmlFile
                                                              || Settings.GeneralPropertiesChanged);

    /// <summary>
    /// Gets a value indicating whether the configuration step that writes the my.ini configuration file needs to run.
    /// </summary>
    public bool IsWriteIniConfigurationFileNeeded => (ConfigurationType != ConfigurationType.Upgrade
                                                      || DefaultAuthenticationPluginChanged
                                                      || (ConfigurationType == ConfigurationType.Upgrade
                                                          && (!IsThereServerDataFiles
                                                              || ExistingServerInstallationInstance != null)));

    /// <summary>
    /// Gets the settings of the existing server installation.
    /// </summary>
    public MySqlServerSettings OldSettings => Settings.OldSettings as MySqlServerSettings;

    /// <summary>
    /// Gets or sets the server installation associated to this controller.
    /// </summary>
    public ServerInstallation ServerInstallation { get; set; }

    /// <summary>
    /// Gets or sets the wizard configuration pages for this controller.
    /// </summary>
    public List<ConfigWizardPage> Pages { get; protected set; }

    /// <summary>
    /// Gets a list of server variables set with SET PERSIST that must be removed as part of the configuration.
    /// </summary>
    public List<string> PersistedVariablesToReset { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether a server reboot is needed.
    /// </summary>
    public bool RebootRequired { get; set; }

    /// <summary>
    /// Gets a value indicating if the data directory should be removed when uninstalling the product.
    /// </summary>
    public bool RemoveDataDirectory => !Settings.KeepDataDirectory;

    /// <summary>
    /// Gets or sets the list of removal steps.
    /// </summary>
    public List<RemoveStep> RemoveSteps { get; protected set; }

    /// <summary>
    /// Gets the list of reverted steps after a failed configuration.
    /// </summary>
    public List<string> RevertedSteps { get; private set; }

    /// <summary>
    /// Gets or sets an instance for controlling the roles assigned to custom MySQL users.
    /// </summary>
    public RoleDefinitions RolesDefined { get; private set; }

    /// <summary>
    /// Gets a value indicating if the credentials of the root user have been provided and validated.
    /// </summary>
    public bool RootUserCredentialsSet => !string.IsNullOrEmpty(Settings.ExistingRootPassword);

    /// <summary>
    /// Gets or sets the authentication plugin assigned to the root user.
    /// </summary>
    public MySqlAuthenticationPluginType RootUserAuthenticationPlugin { get; set; }

    /// <summary>
    /// Gets or sets the value of the password used for the root account.
    /// </summary>
    public string RootPassword { get; set; }

    /// <summary>
    /// Gets the path to the server executable mysqld.exe.
    /// </summary>
    public string ServerExecutableFilePath => Path.Combine(InstallDirectory, BINARY_DIRECTORY_NAME, SERVER_EXECUTABLE_FILENAME);

    /// <summary>
    /// Gets or sets the version of the server installation.
    /// </summary>
    public Version ServerVersion { get; set; }

    /// <summary>
    /// Gets the server configuration settings.
    /// </summary>
    public new MySqlServerSettings Settings => _settings;

    /// <summary>
    /// Gets or sets a flag indicating if the advanced options configuration page should be displayed.
    /// </summary>
    public bool ShowAdvancedOptions { get; set; }

    /// <summary>
    /// Gets or sets the statuses lists.
    /// </summary>
    public List<string> StatusesList { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the Server supports Enterprise Firewall configuration.
    /// </summary>
    public bool SupportsEnterpriseFirewallConfiguration => ServerInstallation.License == LicenseType.Commercial
                                                           && ServerVersion.ServerSupportsEnterpriseFirewall();

    /// <summary>
    /// Gets or sets the template that will be used during the server configuration.
    /// </summary>
    public IniTemplate Template { get; set; }

    /// <summary>
    /// Gets a temporary user account to be used instead of the root user account.
    /// </summary>
    public MySqlServerUser TemporaryServerUser { get; private set; }

    /// <summary>
    /// Gets a list of configuration steps that are related to a stand-alone server setup.
    /// </summary>
    private List<ConfigurationStep> StandAloneServerSteps => IsStartAndUpgradeConfigurationStepNeeded
                                                              ? _selfContainedUpgradeSteps
                                                              : _classicConfigurationSteps;

    /// <summary>
    /// Gets or sets a value indicating if the permissions to the data folder and related files should be updated.
    /// </summary>
    public bool UpdateDataDirectoryPermissions { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating if the status lists should be used.
    /// </summary>
    public bool UseStatusesList
    {
      get => _useStatusesList;
      set
      {
        _useStatusesList = value;
        StatusesList = value
          ? new List<string>()
          : null;
      }
    }

    #endregion Properties

    #region Delegates

    /// <summary>
    /// Delegate used for handling configuration status changes.
    /// </summary>
    /// <param name="sender">The sender object.</param>
    /// <param name="type">The configuration type.</param>
    /// <param name="msg">The message to report.</param>
    public delegate void ConfigurationStatusChangeEventHandler(object sender, ConfigurationEventType type, string msg);

    #endregion

    /// <summary>
    /// Adds the specified user to the list of users that will be created during the server configuration.
    /// </summary>
    /// <param name="values">The list of properties that will be assigned to the user.</param>
    /// <param name="msg">A string representing the error message that will be produced as output.</param>
    /// <returns><c>true</c> if the user was successfully added to the list; otherwise, <c>false</c>.</returns>
    public bool AddUser(ValueList values, out string msg)
    {
      var serverUser = new MySqlServerUser();
      if (!serverUser.SetValues(this, values, out msg))
      {
        return false;
      }

      if (!serverUser.IsValid(out msg))
      {
        return false;
      }

      if (Settings.NewServerUsers.Any(su => su != null
                                            && su.Username == serverUser.Username
                                            && su.Host == serverUser.Host))
      {
        return true;
      }

      Settings.NewServerUsers.Add(serverUser);
      return true;
    }

    /// <summary>
    /// Executes actions required after the configuration has completed.
    /// </summary>
    public void AfterConfigurationEnded()
    {
      // If any configuration step that persists the extended settings did not execute during an upgrade, persist them now.
      if (ConfigurationType == ConfigurationType.Upgrade
          && !_upgradeStandAloneServerStep.Execute
          && !_startAndUpgradeServerConfigStep.Execute
          && !_writeConfigurationFileStep.Execute)
      {
        Settings.PendingSystemTablesUpgrade = false;
        Settings.SaveGeneralSettings();
      }
    }

    /// <summary>
    /// Flags the configuration operation as cancelled
    /// </summary>
    public void CancelConfigure()
    {
      MySqlServiceControlManager.Cancel();
    }

    /// <summary>
    /// Checks whether the server is in a state to be configured.
    /// </summary>
    /// <param name="configurationType">The configuration type.</param>
    /// <returns><c>true</c> if the server can be configured; otherwise, <c>false</c>.</returns>
    public bool CanConfigure(ConfigurationType configurationType)
    {
      var configStates = ConfigState.ConfigurationRequired
                         | ConfigState.ConfigurationComplete
                         | ConfigState.ConfigurationCompleteWithWarnings
                         | ConfigState.ConfigurationCancelled;
      return configStates.HasFlag(CurrentState)
             && HasConfigStepsForType(configurationType);
    }

    /// <summary>
    /// Executes the configuration steps defined by the controller.
    /// </summary>
    public void Configure()
    {
      _revertController.Reset();
      _revertController.ReportStatusDelegate = ReportStatus;
      _revertController.ReportErrorDelegate = ReportError;
      if (ConfigurationType == ConfigurationType.Upgrade
          && ExistingServerInstallationInstance != null)
      {
        ExistingServerInstallationInstance.ResetRunningProcess();
        _revertController.ExistingServerInstallationInstance = ExistingServerInstallationInstance;
        ExistingServerInstallationInstance.ReportStatusDelegate = ReportStatus;
      }

      ResetCancellationToken();
      CurrentState = ConfigState.ConfigurationInProgress;
      ConfigurationStarted?.Invoke(this, EventArgs.Empty);
      Logger.LogInformation("Starting configuration of " + ServerInstallation.NameWithVersion);
      ResetTimer();

      var task = Task.Factory.StartNew(DoConfigure, CancellationTokenSource.Token, TaskCreationOptions.None, TS.Default)
         .ContinueWith(t => EndConfigure(), CancellationTokenSource.Token, TaskContinuationOptions.OnlyOnRanToCompletion,
         AppConfiguration.ConsoleMode ? TS.Default : TS.FromCurrentSynchronizationContext());
      if (AppConfiguration.ConsoleMode)
      {
        task.Wait();
      }
    }

    /// <summary>
    /// Extracts the example databases.
    /// </summary>
    public void ExtractExamplesDatabases()
    {
      if (!string.IsNullOrEmpty(ExampleDatabasesLocation)
          || Directory.Exists(ExampleDatabasesLocation))
      {
        return;
      }

      // Save zip file.
      var tempFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
      if (!Directory.Exists(tempFolder))
      {
        Directory.CreateDirectory(tempFolder);
      }

      var tempFilePath = Path.Combine(tempFolder, "example_databases.zip");
      var assembly = Assembly.GetExecutingAssembly();
      var resources = assembly.GetManifestResourceNames();
      using (var stream = assembly.GetManifestResourceStream($"MySql.Configurator.Resources.example_databases.zip"))
      {
        using (var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
        {
          stream.CopyTo(fileStream);
        }
      }

      // Extract contents.
      var exampleDatabasesLocation = Path.Combine(tempFolder, "Example Databases");
      ZipFile.ExtractToDirectory(tempFilePath, exampleDatabasesLocation);

      ExampleDatabasesLocation = exampleDatabasesLocation;
    }

    /// <summary>
    /// Assembles a connection options string for a MySQL command line program like the MySQL client.
    /// </summary>
    /// <param name="userAccount">The <see cref="MySqlServerUser"/> to be used for the connection, if <c>null</c> a new instance for the root user will be used.</param>
    /// <param name="includePassword">Flag indicating whether the password is to be included in the options.</param>
    /// <param name="useOldSettings">Flag indicating whether the old settings must be used instead of the new settings for the port, pipe name and shared memory.</param>
    /// <returns>A connection options string for a MySQL command line program like the MySQL client.</returns>
    public string GetCommandLineConnectionOptions(MySqlServerUser userAccount, bool includePassword, bool useOldSettings)
    {
      if (userAccount == null)
      {
        userAccount = MySqlServerUser.GetLocalRootUser(Settings.ExistingRootPassword, Settings.DefaultAuthenticationPlugin);
      }

      var builder = new StringBuilder("--user=");
      builder.Append(userAccount.Username);
      if (includePassword
          && !string.IsNullOrEmpty(userAccount.Password))
      {
        builder.Append(" --password=");
        builder.Append(userAccount.Password);
      }

      if (ServerVersion.ServerSupportsCachingSha2Authentication())
      {
        builder.Append(" --default-auth=");
        builder.Append(userAccount.AuthenticationPlugin.GetDescription());
      }

      if (Settings.EnableTcpIp)
      {
        builder.Append(" --host=");
        builder.Append(MySqlServerUser.LOCALHOST);
        builder.Append(" --port=");
        builder.Append(useOldSettings ? OldSettings?.Port ?? Settings.Port : Settings.Port);
      }
      else if (Settings.EnableNamedPipe)
      {
        builder.Append(" --pipe=");
        builder.Append(useOldSettings ? OldSettings?.PipeName ?? Settings.PipeName : Settings.PipeName);
      }
      else if (Settings.EnableSharedMemory)
      {
        builder.Append(" --shared-memory-base-name=");
        builder.Append(useOldSettings ? OldSettings?.SharedMemoryName ?? Settings.SharedMemoryName : Settings.SharedMemoryName);
      }

      return builder.ToString();
    }

    /// <summary>
    /// Gets the connection string.
    /// </summary>
    /// <param name="username">The user name.</param>
    /// <param name="password">The user password.</param>
    /// <param name="useOldSettings">A flag indicating if old settings object should be used.</param>
    /// <param name="schemaName">The name of the database to connect to.</param>
    /// <returns>The connection string.</returns>
    public string GetConnectionString(string username, string password, bool useOldSettings, string schemaName)
    {
      return GetConnectionStringBuilder(new MySqlServerUser(username, password, Settings.DefaultAuthenticationPlugin), useOldSettings, schemaName).ConnectionString;
    }

    /// <summary>
    /// Gets the connection string.
    /// </summary>
    /// <param name="serverUser">The server user.</param>
    /// <param name="useOldSettings">A flag indicating if old settings object should be used.</param>
    /// <param name="schemaName">The name of the database to connect to.</param>
    /// <returns>The connection string.</returns>
    public string GetConnectionString(MySqlServerUser serverUser, bool useOldSettings = false, string schemaName = null)
    {
      return GetConnectionStringBuilder(serverUser, useOldSettings, schemaName).ConnectionString;
    }

    /// <summary>
    /// Gets the connection string builder.
    /// </summary>
    /// <param name="serverUser">The server user.</param>
    /// <param name="useOldSettings">A flag indicating if old settings object should be used.</param>
    /// <param name="schemaName">The name of the database to connect to.</param>
    /// <returns>The connection string builder object..</returns>
    public MySqlConnectionStringBuilder GetConnectionStringBuilder(MySqlServerUser serverUser, bool useOldSettings, string schemaName)
    {
      if (serverUser == null)
      {
        throw new ArgumentNullException(nameof(serverUser));
      }

      var currentSettings = useOldSettings && OldSettings != null
        ? OldSettings
        : Settings;
      var builder = new MySqlConnectionStringBuilder
      {
        Server = string.IsNullOrEmpty(serverUser.Host) ? MySqlServerUser.LOCALHOST : serverUser.Host,
        DefaultCommandTimeout = 120,
        Pooling = false,
        UserID = serverUser.Username,
        Password = serverUser.Password
      };

      if (!string.IsNullOrEmpty(schemaName))
      {
        builder.Database = schemaName;
      }

      // If we are not doing TCP/IP then we assume named pipes.
      // TODO: assuming pipes if no TCP/IP is used is a bold decision. What about shared memory?
      if (currentSettings.EnableTcpIp)
      {
        builder.ConnectionProtocol = MySqlConnectionProtocol.Tcp;
        builder.Port = Convert.ToUInt32(currentSettings.Port);
      }
      else if (currentSettings.EnableNamedPipe)
      {
        builder.ConnectionProtocol = MySqlConnectionProtocol.Pipe;
        builder.PipeName = currentSettings.PipeName;
      }
      else
      {
        builder.ConnectionProtocol = MySqlConnectionProtocol.Memory;
        builder.SharedMemoryName = currentSettings.SharedMemoryName;
      }

      if (currentSettings.IsNamedPipeTheOnlyEnabledProtocol)
      {
        builder.AllowPublicKeyRetrieval = true;
        builder.SslMode = MySqlSslMode.Disabled;
      }

      return builder;
    }

    public virtual string GetMsiCommandLine()
    {
      return string.Format($" INSTALLDIR=\"{0}\" INSTALLLOCATION=\"{0}\" ARPINSTALLLOCATION=\"{0}\" INSTALL_ROOT=\"{0}\" DATADIR=\"{DataDirectory}\"", InstallDirectory);
    }

    public Folder GetShell32NameSpaceFolder(object folder)
    {
      var shellAppType = Type.GetTypeFromProgID("Shell.Application");
      var shell = Activator.CreateInstance(shellAppType);
      return (Folder)shellAppType.InvokeMember("NameSpace",
      BindingFlags.InvokeMethod, null, shell, new[] { folder });
    }

    public bool HasConfigStepsForType(ConfigurationType type)
    {
      return ConfigurationSteps != null
             && ConfigurationSteps.Any(step => step.ValidForConfigureType(type) && step.Execute);
    }

    /// <summary>
    /// Checks if a given directory path contains a Data subdirectory with contents in it.
    /// </summary>
    /// <param name="directoryPath">A full directory path.</param>
    /// <returns><c>true</c> if the given directory path contains a Data subdirectory with contents in it, <c>false</c> otherwise.</returns>
    public bool HasDataSubDirectoryWithFiles(string directoryPath)
    {
      if (string.IsNullOrEmpty(directoryPath))
      {
        return false;
      }

      string dataDirectory = Path.Combine(directoryPath, "Data");
      return Directory.Exists(dataDirectory) && Directory.EnumerateFiles(dataDirectory).Any();
    }

    public bool HasExistingDataDirectory()
    {
      return Directory.Exists(DataDirectory)
             && Directory.EnumerateFileSystemEntries(DataDirectory).Any();
    }

    public void Init()
    {
      CurrentState = ConfigState.ConfigurationRequired;
      ServerVersion = new Version(ServerInstallation.VersionString);
      _settings = new MySqlServerSettings(ServerInstallation);
      _revertController = new ServerRevertController();
      LoadConfigurationSteps();
      if (_settings == null)
      {
        Logger.LogInformation("Product Configuration Controller - Init - Creating settings");
        _settings = new MySqlServerSettings(ServerInstallation);
      }

      ResetCancellationToken();
    }

    public void Initialize(bool afterInstallation)
    {
      Logger.LogInformation("Product configuration controller initialization started.");

      string baseDirectory = string.Empty;
      if (ServerInstallation != null)
      {
        if (!string.IsNullOrEmpty(InstallDirectory))
        {
          Logger.LogInformation($"Product configuration controller found {ServerInstallation.DISPLAY_NAME} installed.");

          // Unreliable since both 32 bit and 64 bit servers use the same keys (for the same version).
          // Though, usually one can only install one of both server architectures.
          baseDirectory = InstallDirectory; // Owner.GetInstalledProductRegistryKey("Location");
          _dataDirectory = DataDirectory; // Owner.GetInstalledProductRegistryKey("DataLocation");

          string versionString = ServerInstallation.VersionString;
          if (!string.IsNullOrEmpty(versionString))
          {
            ServerVersion = new Version(versionString);
          }

          // Look for existing service.
          if (!string.IsNullOrEmpty(baseDirectory))
          {
            var serviceNames = MySqlServiceControlManager.FindServiceNamesWithBaseDirectory(baseDirectory);
            if (serviceNames.Length > 0)
            {
              Settings.ServiceName = serviceNames[0];
            }
          }

          // If no existing service, use default.
          if (string.IsNullOrEmpty(Settings.ServiceName) && ServerVersion != null)
          {
            Settings.ServiceName = $"MySQL{ServerVersion.Major}{ServerVersion.Minor}";
          }
        }
        else
        {
          ServerVersion = new Version(ServerInstallation.VersionString);

          //not suppose to occur anymore
          if (string.IsNullOrEmpty(_dataDirectory))
          {
            SetDefaultDataDir();
          }

          Settings.ServiceName = string.Concat("MySQL", ServerVersion.Major.ToString(), ServerVersion.Minor.ToString());
        }
      }

      if (!string.IsNullOrEmpty(baseDirectory) && !string.IsNullOrEmpty(_dataDirectory))
      {
        Logger.LogInformation("Product configuration controller creating new template instance.");
      }
      else
      {
        Logger.LogInformation("Product not currently installed.");
        Template = null;
      }
    }

    /// <summary>
    /// Gets a value indicating whether the configuration step that initializes the Server needs to run.
    /// </summary>
    public bool IsInitializeServerConfigurationStepNeeded(bool reportStatus)
    {
      if (!ServerVersion.ServerSupportsDatabaseInitialization())
      {
        if (reportStatus)
        {
          ReportStatus(string.Format(Resources.ServerConfigInitializeDatabaseIncompatibleVersionText, ServerVersion));
        }

        return false;
      }

      // If the data directory exists before install the server, no need for initialize the database
      if (IsThereServerDataFiles)
      {
        if (reportStatus)
        {
          ReportStatus(Resources.ServerConfigInitializeDatabaseDataDirExistsText);
        }

        return false;
      }

      return true;
    }

    public void LoadState()
    {
      Logger.LogInformation("Product Configuration Controller - Initializing controller");
      if (Settings == null)
      {
        Init();
      }

      Logger.LogInformation("Product Configuration Controller - Loading Settings state");
      Settings.LoadInstalled();
      if (ServerInstallation.License == LicenseType.Unknown)
      {
        Logger.LogInformation("Product Configuration Controller - Setting License from location");
        SetLicenseFromLocation();
      }
    }

    public bool PipeOrMemoryNameExists(string name, bool pipe)
    {
      // TODO: get installed packages
      //return ProductManager.InstalledPackages.Any(p => {
      //  var c = p.Controller as ServerConfigurationController;
      //  return p != Package
      //         && p.Product.Category.Type.Equals("Server", StringComparison.OrdinalIgnoreCase)
      //         && c != null
      //         && ((pipe
      //              && c.Settings.EnableNamedPipe
      //              && c.Settings.PipeName.Equals(name, StringComparison.OrdinalIgnoreCase))
      //             || (!pipe
      //                 && c.Settings.EnableSharedMemory
      //                 && c.Settings.SharedMemoryName.Equals(name, StringComparison.OrdinalIgnoreCase)));
      //});

      return false;
    }

    public void PostConfigure(ServerInstallationStatus status)
    {
    }

    public void PostInstall(ServerInstallationStatus status)
    {
      // We need to "re-find" the ini directory because the data directory may have changed
      Settings.IniDirectory = null;
      Settings.ConfigFile = null;
      Settings.LoadIniDefaults();
    }

    public void PostModify(ServerInstallationStatus status)
    {
    }

    public void PostRemove(ServerInstallationStatus status)
    {
      if (status == ServerInstallationStatus.Failed)
      {
        return;
      }
    }

    public void PostUpgrade(ServerInstallationStatus status)
    {
      if (status != ServerInstallationStatus.Complete)
      {
        return;
      }
    }

    public void PrepareForConfigure()
    {
      Settings.CloneToOldSettings();
      if (ConfigurationType == ConfigurationType.Reconfigure)
      {
        return;
      }

      Settings.PipeName = GetUniquePipeOrMemoryName(true);
      Settings.SharedMemoryName = GetUniquePipeOrMemoryName(false);
    }

    public bool PreConfigure()
    {
      return true;
    }

    public bool PreInstall()
    {
      return true;
    }

    public bool PreModify()
    {
      return true;
    }

    public bool PreRemove()
    {
      return true;
    }

    public bool PreUpgrade()
    {
      StopServerSafe(true);
      return true;
    }

    /// <summary>
    /// Asynchronously handles the uninstall operation of the product associated to this controller.
    /// </summary>
    public void Remove()
    {
      ResetCancellationToken();
      CurrentState = ConfigState.ConfigurationInProgress;
      ConfigurationStarted?.Invoke(this, EventArgs.Empty);
      Logger.LogInformation("Starting removal of " + ServerInstallation.NameWithVersion);
      ResetTimer();

      var task = Task.Factory.StartNew(DoRemove, CancellationTokenSource.Token, TaskCreationOptions.None, TS.Default)
         .ContinueWith(t => EndRemove(), CancellationTokenSource.Token, TaskContinuationOptions.OnlyOnRanToCompletion,
         AppConfiguration.ConsoleMode ? TS.Default : TS.FromCurrentSynchronizationContext());
      if (AppConfiguration.ConsoleMode)
      {
        task.Wait();
      }
    }

    /// <summary>
    /// Identifies if user input is required prior to executing an uninstall operation.
    /// </summary>
    /// <returns><c>true</c> if user input is required to configure prior to uninstalling the product; otherwise, <c>false</c>.</returns>
    public bool RequiresUninstallConfiguration()
    {
      return IsThereServerDataFiles;
    }

    /// <summary>
    /// Reports a waiting message to the log.
    /// </summary>
    /// <param name="token">The token to report.</param>
    public void ReportWaiting(string token = ".")
    {
      if (string.IsNullOrEmpty(token))
      {
        return;
      }

      ReportStatus(ConfigurationEventType.Waiting, token);
    }

    /// <summary>
    /// Sets the default state.
    /// </summary>
    public void ResetState()
    {
      CurrentState = ConfigState.ConfigurationRequired;
    }

    /// <summary>
    /// Restarts the server by stopping and starting it again.
    /// </summary>
    /// <param name="useOldSettings">Flag indicating whether the old settings must be used instead of the new settings to build the command line options.</param>
    public void RestartServer(bool useOldSettings)
    {
      CancellationToken.ThrowIfCancellationRequested();
      ReportStatus(Resources.ServerStopProcessStep);
      StopServerSafe(useOldSettings);
      CancellationToken.ThrowIfCancellationRequested();
      ReportStatus(Resources.ServerStartProcessStep);
      StartServer();
    }

    /// <summary>
    /// Set a property based on the provided value pair.
    /// </summary>
    /// <param name="pair">The value pair.</param>
    /// <param name="message">A string representing an error message if the assignment failed.</param>
    /// <returns><c>true</c> if the assignment was successful; otherwise, <c>false</c>.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public bool SetConfigurationValue(string[] pair, out string message)
    {
      if (Settings.OldSettings == null)
      {
        Settings.CloneToOldSettings();
      }

      if (Settings == null)
      {
        throw new InvalidOperationException("Settings object has not been initialized");
      }

      if (pair == null || pair.Length != 2)
      {
        throw new InvalidOperationException("Value pair is either null or doesn't have the right number of elements.");
      }

      if (string.IsNullOrEmpty(pair[0]) || string.IsNullOrEmpty(pair[1]))
      {
        throw new InvalidOperationException("Empty or blank settings are not allowed");
      }

      return Settings.SetValue(pair[0], pair[1], out message);
    }

    /// <summary>
    /// Sets up the wizard pages.
    /// </summary>
    public void SetPages()
    {
      Pages.Clear();

      // Remove pages.
      if (ConfigurationType == ConfigurationType.Remove)
      {
        if (RequiresUninstallConfiguration())
        {
          var serverRemovePage = new ServerRemovePage(this, ServerInstallation.VersionString);
          Pages.Add(serverRemovePage);
        }

        return;
      }

      Logger.LogInformation(string.Format(Resources.SettingUpControllerMessage, ConfigurationType.GetDescription()));

      // Configure configuration pages.
      if (ConfigurationType == ConfigurationType.Configure)
      {
        var fullInstallDir = Path.GetFullPath(Settings.InstallDirectory).TrimEnd('\\');
        var otherServersRunning = Utilities.GetRunningProcessses("mysqld").Where(p => string.Compare(Path.GetDirectoryName(p.MainModule.FileName).TrimEnd('\\'),
                                                                                                                  fullInstallDir,
                                                                                                                  StringComparison.InvariantCultureIgnoreCase) != 0);

        // If other instances are running, allow to perform an upgrade.
        if (otherServersRunning.Count() > 0)
        {
          Pages.Add(new ServerConfigServerInstallationsPage(this));
        }
        // Otherwise, let user set the data directory path.
        else
        {
          Pages.Add(new ServerConfigDataDirectoryPage(this) { PageVisible = ConfigurationType == ConfigurationType.Configure });
        }
      }

      // Upgrade pages.
      Pages.Add(new ServerConfigBackupPage(this) { PageVisible = ConfigurationType == ConfigurationType.Upgrade });
      if (ConfigurationType == ConfigurationType.Upgrade)
      {
        Pages.Add(new ServerConfigSecurityPage(this) { PageVisible = !ValidateServerFilesHaveRecommendedPermissions() });
        return;
      }

      // Configure configuration and reconfiguration pages.
      Pages.Add(new ServerConfigLocalMachinePage(this));
      Pages.Add(new ServerConfigNamedPipesPage(this) { PageVisible = Settings != null
                                                       && Settings.EnableNamedPipe});
      Pages.Add(new ServerConfigUserAccountsPage(this));
      Pages.Add(new ServerConfigServicePage(this));
      Pages.Add(new ServerConfigSecurityPage(this) { PageVisible = !ValidateServerFilesHaveRecommendedPermissions() });
      ConfigWizardPage loggingPage = new ServerConfigLoggingOptionsPage(this);
      loggingPage.PageVisible = ConfigurationType != ConfigurationType.Reconfigure;
      Pages.Add(loggingPage);
      ConfigWizardPage advancedPage = new ServerConfigAdvancedOptionsPage(this);
      advancedPage.PageVisible = ConfigurationType == ConfigurationType.Configure;
      Pages.Add(advancedPage);
      Pages.Add(new ServerExampleDatabasesPage(this));
    }

    /// <summary>
    /// Updates the configuration steps each time a configuration is run.
    /// </summary>
    public void UpdateConfigurationSteps()
    {
      RolesDefined.ServerVersion = ServerVersion;
      foreach (var step in ConfigurationSteps)
      {
        if (step == _writeConfigurationFileStep) _writeConfigurationFileStep.Execute = IsWriteIniConfigurationFileNeeded || IsWriteExtendedConfigurationFileNeeded;
        else if (step == _stopServerConfigurationStep) _stopServerConfigurationStep.Execute = IsStopServerConfigurationStepNeeded;
        else if (step == _startServerConfigurationStep) _startServerConfigurationStep.Execute = IsStartServerConfigurationStepNeeded;
        //else if (step == _prepareAuthenticationPluginChangeStep) _prepareAuthenticationPluginChangeStep.Execute = DefaultAuthenticationPluginChanged;
        else if (step == _updateSecurityStep) _updateSecurityStep.Execute = IsUpdateSecurityConfigurationStepNeeded;
        else if (step == _updateUsersStep) _updateUsersStep.Execute = IsUpdateUsersConfigurationStepNeeded;
        else if (step == _updateEnterpriseFirewallPluginConfigStep) _updateEnterpriseFirewallPluginConfigStep.Execute = IsUpdateEnterpriseFirewallPluginConfigurationStepNeeded;
        else if (step == _updateWindowsServiceStep) _updateWindowsServiceStep.Execute = IsUpdateWindowsServiceConfigurationStepNeeded;
        else if (step == _updateProcessStep) _updateProcessStep.Execute = IsUpdateProcessConfigurationStepNeeded;
        else if (step == _updateAccessPermissions) _updateAccessPermissions.Execute = IsUpdateServerFilesPermissionsStepNeeded;
        else if (step == _initializeServerConfigurationStep) _initializeServerConfigurationStep.Execute = IsInitializeServerConfigurationStepNeeded(true);
        else if (step == _updateStartMenuLinksStep) _updateStartMenuLinksStep.Execute = IsUpdateStartMenuLinksConfigurationStepNeeded;
        else if (step == _createRemoveExampleDatabasesStep) _createRemoveExampleDatabasesStep.Execute = IsCreateRemoveExamplesDatabasesStepNeeded;
      }

      if (ConfigurationType == ConfigurationType.Upgrade)
      {
        UpdateUpgradeConfigSteps();
      }

      // Not all products have configuration steps.
      if (ConfigurationSteps == null
          || ConfigurationSteps.Count == 0)
      {
        _configurationTimeout = 0;
        return;
      }

      _configurationTimeout = ConfigurationSteps.Sum(step => step.ValidForConfigureType(ConfigurationType) && step.Execute
                                              ? (step.NormalTime > 0 ? step.NormalTime : DEFAULT_TIMEOUT_IN_SECONDS)
                                              : 0);
    }

    /// <summary>
    /// Updates the configuration steps during a Server upgrade to reflect user selections.
    /// </summary>
    public void UpdateUpgradeConfigSteps()
    {
      _backupDatabaseStep.Execute = IsBackupDatabaseStepNeeded;
      _upgradeStandAloneServerStep.Execute = IsThereServerDataFiles
                                             && CurrentState == ConfigState.ConfigurationRequired;
      _removeExistingServerInstallationStep.Execute = IsRemoveExistingServerInstallationStepNeeded;
      _resetPersistedVariablesStep.Execute = PersistedVariablesToReset?.Count > 0;
      _renameExistingDataDirectoryStep.Execute = IsDataDirectoryRenameNeeded;
      _startAndUpgradeServerConfigStep.Execute = IsStartAndUpgradeConfigurationStepNeeded;
      _stopServerConfigurationStep.Execute = IsSameDirectoryUpgrade;
      _stopExistingServerInstanceStep.Execute = !IsSameDirectoryUpgrade;
      _updateAccessPermissions.Execute = IsUpdateServerFilesPermissionsStepNeeded;
      _updateAuthenticationPluginStep.Execute = IsUpdateAuthenticationPluginStepNeeded;
      ConfigurationSteps = StandAloneServerSteps;
    }

    /// <summary>
    /// Updates the remove steps each during an uninstall operation.
    /// </summary>
    public void UpdateRemoveSteps()
    {
      if (Settings == null)
      {
        return;
      }

      LoadRemovalConfigurationSteps();
      _deleteServiceStep.Execute = IsDeleteServiceStepNeeded;
      _deleteConfigurationFileStep.Execute = IsDeleteConfigurationFileStepNeeded;
      _deleteDataDirectoryStep.Execute = IsDeleteDataDirectoryStepNeeded;
      _removeFirewallRuleStep.Execute = IsRemoveFirewallRuleStepNeeded;

      if (RemoveSteps == null)
      {
        RemoveSteps = new List<RemoveStep>() { };
      }

      _configurationTimeout = RemoveSteps.Sum(step => step.Execute
                                               ? (step.NormalTime > 0 ? step.NormalTime : DEFAULT_TIMEOUT_IN_SECONDS)
                                               : 0);
    }

    /// <summary>
    /// Validates if the permissions to the data directory are already set up as recommended.
    /// </summary>
    /// <returns><c>true</c> if the permissions are already set up as recommended; otherwise, <c>false</c>.</returns>
    public bool ValidateServerFilesHaveRecommendedPermissions()
    {
      if (!IsThereServerDataFiles)
      {
        return false;
      }

      var dataDirectory = Path.Combine(DataDirectory, "Data");
      if (DirectoryServicesWrapper.DirectoryPermissionsAreInherited(dataDirectory) == true)
      {
        return false;
      }

      var usersGroupSid = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);
      var administratorsGroupSid = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null);
      SecurityIdentifier serviceAccountSid = null;
      if (Settings.ConfigureAsService)
      {
        if (string.IsNullOrEmpty(Settings.ServiceAccountUsername))
        {
          return false;
        }

        var serviceAccountUsername = Settings.ServiceAccountUsername.StartsWith(".")
                                       ? Settings.ServiceAccountUsername.Replace(".", Environment.MachineName)
                                       : Settings.ServiceAccountUsername;
        var account = new NTAccount(serviceAccountUsername);
        if (account == null)
        {
          Logger.LogError(Resources.ServerConfigConvertToNTAccountFailed);
          return false;
        }

        try
        {
          serviceAccountSid = account.Translate(typeof(SecurityIdentifier)) as SecurityIdentifier;
        }
        catch (Exception ex)
        {
          Logger.LogException(ex);
        }

        if (serviceAccountSid == null)
        {
          Logger.LogError(string.Format(Resources.ServerConfigCouldNotObtainSid, account.Value));
          return false;
        }
      }

      var serverFilesHaveRecommendedPermissions = !DirectoryServicesWrapper.HasAccessToDirectory(usersGroupSid, dataDirectory, null)
                                                  && DirectoryServicesWrapper.HasAccessToDirectory(administratorsGroupSid, dataDirectory, FileSystemRights.FullControl)
                                                  && Settings.ConfigureAsService
                                                       ? DirectoryServicesWrapper.HasAccessToDirectory(serviceAccountSid, dataDirectory, FileSystemRights.FullControl)
                                                       : OldSettings != null
                                                         && OldSettings.ConfigureAsService
                                                           ? !DirectoryServicesWrapper.HasAccessToDirectory(DirectoryServicesWrapper.GetSecurityIdentifier(OldSettings.ServiceAccountUsername), dataDirectory, FileSystemRights.FullControl)
                                                           : true;
      UpdateDataDirectoryPermissions = !serverFilesHaveRecommendedPermissions;
      if (ConfigurationType == ConfigurationType.Upgrade)
      {
        UpdateUpgradeConfigSteps();
      }
      else
      {
        UpdateConfigurationSteps();
      }

      return serverFilesHaveRecommendedPermissions;
    }

    /// <summary>
    /// Backs up the MySQL database using mysqldump.
    /// </summary>
    private void BackupDatabase()
    {
      if (!IsSameDirectoryUpgrade)
      {
        if (ExistingServerInstallationInstance == null)
        {
          throw new Exception(Resources.ExistingServerInstanceNotSetError);
        }

        if (!ExistingServerInstallationInstance.IsRunning)
        {
          throw new Exception(Resources.ExistingServerInstanceNotRunningError);
        }
      }

      var success = false;
      string errorMessage = null;
      CancellationToken.ThrowIfCancellationRequested();
      // Run mysqldump tool

      var iniDirectory = OldSettings.IniDirectory;
      var backupDirectoryPath = Path.Combine(Settings.ConfigurationFileExists.HasValue 
        ? iniDirectory 
        : Path.Combine(AppConfiguration.HomeDir, DATABASE_BACKUP_DIRECTORY));
      if (!Directory.Exists(backupDirectoryPath))
      {
        Directory.CreateDirectory(backupDirectoryPath);
      }

      var backupFile = Path.Combine(backupDirectoryPath, BackupFileName);
      if (!string.IsNullOrEmpty(backupFile))
      {
        ReportStatus(string.Format(Resources.ServerConfigBackupDatabaseDumpRunning, backupFile));
        var binDirectory = Path.Combine(IsSameDirectoryUpgrade 
          ? InstallDirectory
          : ExistingServerInstallationInstance.BaseDir, BINARY_DIRECTORY_NAME);
        var user = GetUserAccountToConnectBeforeUpdatingRootUser();
        var connectionOptions = string.Empty;
        bool sendingPasswordInCommandLine = false;
        string tempConfigFileWithPassword = null;
        if (!string.IsNullOrEmpty(user.Password))
        {
          tempConfigFileWithPassword = Base.Classes.Utilities.CreateTempConfigurationFile(IniFile.GetClientPasswordLines(user.Password));
          sendingPasswordInCommandLine = tempConfigFileWithPassword == null;
          connectionOptions = sendingPasswordInCommandLine
            ? $"--password={user.Password} "
            : $"--defaults-extra-file=\"{tempConfigFileWithPassword}\" ";
        }

        connectionOptions += GetCommandLineConnectionOptions(user, false, ConfigurationType == ConfigurationType.Upgrade);
        var arguments =
          $" {connectionOptions} --default-character-set=utf8 --routines --events --single-transaction=TRUE --all-databases --result-file=\"{backupFile}\"";
        var dumpToolProcessResult = Base.Classes.Utilities.RunProcess(
          Path.Combine(binDirectory, DUMP_TOOL_EXECUTABLE_FILENAME),
          arguments,
          binDirectory,
          ReportStatus,
          ReportStatus,
          true,
          sendingPasswordInCommandLine);
        Base.Classes.Utilities.DeleteFile(tempConfigFileWithPassword, 10, 500);
        success = dumpToolProcessResult.ExitCode == 0;
      }
      else
      {
        errorMessage = Resources.ServerConfigBackupDatabaseBackupDirectoryError;
      }

      ReportStatus(success
        ? Resources.ServerConfigBackupDatabaseDumpRunSuccess
        : errorMessage);
      CurrentStep.Status = success
        ? ConfigurationStepStatus.Finished
        : ConfigurationStepStatus.Error;
    }

    /// <summary>
    /// Checks if the account has access to the specified log file or its parent folder. If that is not the case, access is granted.
    /// </summary>
    /// <param name="logFilePath">The path to the log file.</param>
    /// <param name="accountName">The name of the account.</param>
    /// <param name="createFileIfNotExists">Flag to indicate if the specified log file should be created if it does not exist.</param>
    private void CheckLogFilePermissions(string logFilePath, string accountName, bool createFileIfNotExists)
    {
      if (string.IsNullOrEmpty(logFilePath))
      {
        throw new Exception(Resources.ServerConfigLogPathMissing);
      }

      if (string.IsNullOrEmpty(accountName))
      {
        throw new Exception(Resources.ServerConfigAccountNameMissing);
      }

      if (!Path.IsPathRooted(logFilePath))
      {
        // If path is not rooted it means the log file will be located in the data folder.
        // Granting access to the data folder is already handled elsewhere.
        return;
      }

      var fileInfo = new FileInfo(logFilePath);
      if (!fileInfo.Exists
          && createFileIfNotExists)
      {
        File.Create(logFilePath);
      }

      var sid = DirectoryServicesWrapper.GetSecurityIdentifier(accountName);
      if ((fileInfo.Exists
          && DirectoryServicesWrapper.HasAccessToFile(sid, logFilePath, FileSystemRights.FullControl))
          || (!fileInfo.Exists
              && DirectoryServicesWrapper.HasAccessToDirectory(sid, fileInfo.DirectoryName, FileSystemRights.FullControl)))
      {
        return;
      }

      if (fileInfo.Exists)
      {
        DirectoryServicesWrapper.GrantUserPermissionToFile(logFilePath, Settings.ServiceAccountUsername, FileSystemRights.FullControl, AccessControlType.Allow);
      }
      else
      {
        DirectoryServicesWrapper.GrantUserPermissionToDirectory(fileInfo.DirectoryName, Settings.ServiceAccountUsername, FileSystemRights.FullControl, AccessControlType.Allow);
      }

      ReportStatus(string.Format(Resources.ServerConfigGrantedPermissionsToLogFile, logFilePath));
    }

    /// <summary>
    /// Validates that the current user has access to the specified log file. If the file does not exist, the parent folder is used
    /// to determine if access will be granted when file is created during configuration.
    /// </summary>
    /// <param name="logPath">The path to the log file.</param>
    /// <returns><c>true</c> if the current user has access to the log file; otherwise, <c>false</c>.</returns>
    private bool CurrentUserHasAccessToLogFile(string logPath)
    {
      if (string.IsNullOrEmpty(logPath))
      {
        return false;
      }

      WindowsIdentity currentUser;
      SecurityIdentifier currentUserSid;
      try
      {
        currentUser = WindowsIdentity.GetCurrent();
        currentUserSid = DirectoryServicesWrapper.GetSecurityIdentifier(currentUser.Name);
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
        return false;
      }

      var fileInfo = new FileInfo(logPath);
      return fileInfo.Exists
             ? DirectoryServicesWrapper.HasAccessToFile(currentUserSid, logPath, FileSystemRights.FullControl)
             : DirectoryServicesWrapper.HasAccessToDirectory(currentUserSid, fileInfo.DirectoryName, FileSystemRights.FullControl);
    }

    /// <summary>
    /// Checks that the Windows service is able to access the data folder and server logs.
    /// </summary>
    private void CheckServicePermissions()
    {
      ReportStatus(string.Format(Resources.ServerConfigGrantingPermissionsToAccount, Settings.ServiceAccountUsername));
      try
      {
        // Validate access to data directory.
        DirectoryServicesWrapper.GrantUserPermissionToDirectory(Settings.DataDirectory, Settings.ServiceAccountUsername, FileSystemRights.FullControl, AccessControlType.Allow);
        ReportStatus(Resources.ServerConfigGrantedPermissionsToDataDirectory);

        // Validate the service account has access to log files.
        ConfigureFilePermissionsForServerLogs(Settings.ServiceAccountUsername);

        // Validate access to install dir.
        DirectoryServicesWrapper.GrantUserPermissionToDirectory(Settings.InstallDirectory, Settings.ServiceAccountUsername, FileSystemRights.ReadAndExecute, AccessControlType.Allow);
        ReportStatus("Granted permissions to the install directory.");
      }
      catch (Exception ex)
      {
        ReportError(ex.Message);
        throw;
      }
    }

    /// <summary>
    /// Configured access to the server logs if needed.
    /// </summary>
    /// <param name="accountName">The name of the account to grant access to.</param>
    private void ConfigureFilePermissionsForServerLogs(string accountName)
    {
      if (string.IsNullOrEmpty(accountName))
      {
        Logger.LogError(Resources.ServerConfigAccountNameMissing);
        return;
      }

      try
      {
        CheckLogFilePermissions(Settings.ErrorLogFileName, accountName, true);
        if (Settings.EnableGeneralLog)
        {
          CheckLogFilePermissions(Settings.GeneralQueryLogFileName, accountName, false);
        }

        if (Settings.EnableSlowQueryLog)
        {
          CheckLogFilePermissions(Settings.SlowQueryLogFileName, accountName, false);
        }

        if (Settings.EnableBinLog)
        {
          CheckLogFilePermissions(Settings.BinLogFileNameBase, accountName, false);
        }
      }
      catch (Exception ex)
      {
        ReportStatus(ex.Message);
        Logger.LogException(ex);
      }
    }

    /// Configures a Windows Firewall rule.
    /// </summary>
    /// <param name="action">The action to perform on the rule, such as "add", "delete".</param>
    /// <param name="port">The network port add an exception to the Windows Firewall for.</param>
    /// <param name="serviceName">The name of the Windows service if the Server runs as a service.</param>
    /// <returns><c>true</c> if the Windows Firewall rule could be configured successfullt, <c>false</c> otherwise.</returns>
    private bool ConfigureWindowsFirewallRule(string action, string port, string serviceName)
    {
      CancellationToken.ThrowIfCancellationRequested();
      var useNetshAdvanced = Win32.IsVistaOrHigher;
      var firewallCommandTemplate = useNetshAdvanced
        ? "advfirewall firewall {0} rule name=\"Port {1}\" protocol=TCP localport={1}"
        : "firewall {0} portopening protocol=TCP port={1} profile=ALL";
      var firewallCommandExtra = useNetshAdvanced
        ? " dir=in action=allow"
        : $" name={serviceName} mode=ENABLE scope=ALL";
      var arguments = string.Format(firewallCommandTemplate, action, port);
      if (action == "add")
      {
        arguments += firewallCommandExtra;
      }

      ReportStatus(string.Format(Resources.ServerConfigEventFirewallSettingNetshCmd, action, arguments));
      var success = Utilities.RunNetShellProcess(arguments, out var netShellProcessOutput, out var netShellProcessError);
      ReportStatus(netShellProcessOutput);
      if (!success)
      {
        ReportStatus(string.Format(Resources.ServerConfigEventFirewallSettingNetshCmdResults, action, !string.IsNullOrEmpty(netShellProcessError) ? netShellProcessError : Resources.ServerConfigEventFirewallSettingNetshCmdUnknownError));
        ReportStatus(string.Format(Resources.ServerConfigEventFirewallSettingsTip, action));
      }

      return success;
    }

    /// <summary>
    /// Converts the provided scripts to an unistall script used to remove any items created.
    /// </summary>
    /// <param name="databaseToUse">The name of the database where statements will be executed.
    /// Leave empty or <c>null</c> to use the default database.</param>
    /// <param name="preInstallScript">Array of scripts.</param>
    /// <returns>An uninstall SQL script.</returns>
    private string ConvertToUninstallScript(string databaseToUse, params string[] scripts)
    {
      if (scripts == null
          || scripts.Length == 0)
      {
        return null;
      }

      var builder = new StringBuilder();
      if (!string.IsNullOrEmpty(databaseToUse))
      {
        builder.AppendLine($"USE {databaseToUse};");
      }

      try
      {
        foreach (var script in scripts)
        {
          using (var stringReader = new StringReader(script))
          {
            string line;
            while ((line = stringReader.ReadLine()) != null)
            {
              line = line.Replace("(", " (");
              var tokens = line.Split(' ');
              if (tokens == null
                  || tokens.Length < 3)
              {
                continue;
              }

              if (line.StartsWith("CREATE TABLE"))
              {
                var tableName = tokens[2];
                if (line.Contains("IF NOT EXISTS")
                    && tokens.Length > 5)
                {
                  tableName = tokens[5];
                }
                else
                {
                  var command = new StringBuilder(line);
                  var newLine = string.Empty;
                  while(!newLine.EndsWith(";"))
                  {
                    newLine = stringReader.ReadLine();
                    command.Append($" {newLine}");
                  }

                  line = command.ToString().Replace("(", " (");
                  tokens = line.Split(' ');
                  tableName = tokens.Length > 5
                              ? tokens[5]
                              : null;
                }

                builder.AppendLine($"DROP TABLE IF EXISTS {tableName};");
              }
              else if (line.StartsWith("CREATE FUNCTION")
                       || line.StartsWith("CREATE AGGREGATE FUNCTION"))
              {
                var functionName = tokens[2];
                if (line.StartsWith("CREATE AGGREGATE FUNCTION"))
                {
                  functionName = tokens[3];
                }

                builder.AppendLine($"DROP FUNCTION IF EXISTS {functionName};");
              }
              else if (line.StartsWith("CREATE PROCEDURE"))
              {
                var procedureName = tokens[2];
                builder.AppendLine($"DROP PROCEDURE IF EXISTS {procedureName};");
              }
            }
          }
        }

        return builder.ToString();
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
        return null;
      }
    }

    /// <summary>
    /// Creates or removes the example databases.
    /// </summary>
    private void CreateRemoveExampleDatabases()
    {
      ReportStatus(string.Format(Resources.ConfigInfoCreatingRemovingExamplesData));

      ExtractExamplesDatabases();
      var databasesToInstall = ExampleDatabaseInfo.GetSampleDatabasesInfo(ExampleDatabasesLocation);
      if (databasesToInstall == null)
      {
        ReportStatus(Resources.ConfigInfoNoSampleDbsToConfigure);
        CurrentStep.Status = ConfigurationStepStatus.Error;
        return;
      }

      var serverInstance = new MySqlServerInstance(this, ReportErrLogLine)
      {
        UserAccount = GetUserAccountToConnectBeforeUpdatingRootUser()
      };

      foreach (var exampleDatabase in databasesToInstall)
      {
        var exampleDbInfo = ExampleDatabasesStatus.FirstOrDefault(item => item.Key.Equals(exampleDatabase.SchemaName));
        if (exampleDbInfo.Key == null)
        {
          continue;
        }

        if (exampleDbInfo.Value.Equals("create"))
        {
          exampleDatabase.Install(serverInstance);
        }
        else if (exampleDbInfo.Value.Equals("remove"))
        {
          exampleDatabase.Remove(exampleDatabase.SchemaName, serverInstance);
        }
      }

      // Remove temp folder for example databases.
      if (Directory.Exists(ExampleDatabasesLocation))
      {
        try
        {
          Directory.Delete(ExampleDatabasesLocation, true);
          ExampleDatabasesLocation = null;
        }
        catch (Exception ex)
        {
          Logger.LogException(ex);
        }
      }

      CurrentStep.Status = ConfigurationStepStatus.Finished;
    }

    /// <summary>
    /// Create a firewall rule for this server instance.
    /// </summary>
    private void CreateFirewallRule(uint port)
    {
      ReportStatus(string.Format(Resources.ServerConfigEventFirewallSettingInfo, Settings.ServiceName, port));
      ReportStatus(ConfigureWindowsFirewallRule("add", port.ToString(), Settings.ServiceName)
        ? Resources.ServerConfigEventFirewallSettingSucceeded
        : Resources.ServerConfigEventFirewallSettingFailed);
    }

    /// <summary>
    /// Creates or alter user accounts in the MySQL database.
    /// </summary>
    /// <param name="adminUser">The <see cref="MySqlServerUser"/> used to establish the connection.</param>
    /// <param name="affectedUsers">A list of <see cref="MySqlServerUser"/> accounts to create or alter.</param>
    /// <param name="grantRolePermissions">Flag indicating whether permissions need to be updated for the given accounts.</param>
    /// <param name="useOldSettings">Flag indicating whether the confuguration previous to current changes will be used instead of current.</param>
    /// <returns><c>true</c> if the user accounts were affected successfully, <c>false</c> otherwise.</returns>
    private bool CreateOrAlterUserAccounts(MySqlServerUser adminUser, List<MySqlServerUser> affectedUsers, bool grantRolePermissions, bool useOldSettings = false)
    {
      if (adminUser == null)
      {
        throw new ArgumentNullException(nameof(adminUser));
      }

      if (affectedUsers == null)
      {
        throw new ArgumentNullException(nameof(affectedUsers));
      }

      if (affectedUsers.Count == 0)
      {
        return true;
      }

      var success = true;
      CancellationToken.ThrowIfCancellationRequested();
      try
      {
        var connectionString = GetConnectionString(adminUser, useOldSettings);
        using (var c = new MySqlConnection(connectionString))
        {
          c.Open();
          var cmd = c.CreateCommand();
          foreach (var affectedUser in affectedUsers)
          {
            CancellationToken.ThrowIfCancellationRequested();
            cmd.CommandText = RolesDefined.GetCreateOrAlterUserSql(UserCrudOperationType.CreateUser, affectedUser);
            cmd.ExecuteNonQuery();
            if (!grantRolePermissions)
            {
              continue;
            }

            var grantSqlStatements = RolesDefined.GetUpdateUserSql(affectedUser);
            foreach (string grantSql in grantSqlStatements)
            {
              CancellationToken.ThrowIfCancellationRequested();
              cmd.CommandText = grantSql;
              cmd.ExecuteNonQuery();
            }

            ReportStatus(string.Format(Resources.ServerConfigCustomUserCreated, affectedUser.Username, affectedUser.Host));
          }
        }
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
        ReportStatus(string.Format(Resources.ServerConfigCustomUserCreationFailure, ex.Message));
        success = false;
      }

      return success;
    }

    /// <summary>
    /// Step to trigger the deletion of the server configuration file.
    /// </summary>
    private void DeleteConfigurationFileStep()
    {
      CancellationToken.ThrowIfCancellationRequested();
      ReportStatus(Resources.RemovingGeneralSettingsFileText);
      GeneralSettingsManager.DeleteGeneralSettingsFile(InstallDirectory);
      ReportStatus(Resources.RemovingConfigurationFileText);
      CurrentStep.Status = Settings.DeleteConfigFile(RemoveDataDirectory)
        ? ConfigurationStepStatus.Finished
        : ConfigurationStepStatus.Error;
    }

    /// <summary>
    /// Step to trigger the deletion of the server data directory.
    /// </summary>
    private void DeleteDataDirectoryStep()
    {
      CancellationToken.ThrowIfCancellationRequested();
      ReportStatus(Resources.RemovingDataDirectoryText);
      CurrentStep.Status = DeleteDataDirectory()
        ? ConfigurationStepStatus.Finished
        : ConfigurationStepStatus.Error;
    }

    /// Step to trigger the deletion of the service created for the current server installation.
    /// </summary>
    private void DeleteServiceStep()
    {
      CancellationToken.ThrowIfCancellationRequested();
      ReportStatus(Resources.RemovingWindowsServiceText);
      CurrentStep.Status = DeleteService()
        ? ConfigurationStepStatus.Finished
        : ConfigurationStepStatus.Error;
    }

    /// <summary>
    /// Deletes the user account associated with the given <see cref="MySqlServerUser"/>.
    /// </summary>
    /// <param name="adminUser">The <see cref="MySqlServerUser"/> used to establish the connection.</param>
    /// <param name="affectedUser">The <see cref="MySqlServerUser"/> accounts to delete.</param>
    /// <param name="useOldSettings">Flag indicating whether the configuration previous to current changes will be used instead of current.</param>
    /// <returns><c>true</c> if the user account was deleted successfully or if it did not exist, <c>false</c> otherwise.</returns>
    private bool DeleteServerUserAccount(MySqlServerUser adminUser, MySqlServerUser affectedUser, bool useOldSettings = false)
    {
      if (adminUser == null)
      {
        throw new ArgumentNullException(nameof(adminUser));
      }

      if (affectedUser == null)
      {
        return true;
      }

      var success = true;
      try
      {
        var connectionString = GetConnectionString(adminUser, useOldSettings, "mysql");
        using (var c = new MySqlConnection(connectionString))
        {
          c.Open();
          var sql = $"DROP USER IF EXISTS '{affectedUser.Username}'@'{affectedUser.Host}';";
          var cmd = new MySqlCommand(sql, c);
          cmd.ExecuteNonQuery();
        }
      }
      catch (Exception ex)
      {
        success = false;
        Logger.LogException(ex);
      }

      return success;
    }

    /// <summary>
    /// Deletes the service created for the current server installation.
    /// </summary>
    private bool DeleteService()
    {
      if (Settings == null)
      {
        return false;
      }

      if (!IsDeleteServiceStepNeeded)
      {
        ReportStatus(Resources.ServerServiceNotFoundText);
        return true;
      }

      ReportStatus(Resources.RemovingServiceText);
      var deleteOperationSucceeded = MySqlServiceControlManager.Delete(Settings.ServiceName);
      if (deleteOperationSucceeded != null)
      {
        RebootRequired = !Convert.ToBoolean(deleteOperationSucceeded);
        ReportStatus(RebootRequired
                      ? Resources.ServiceRemoveAfterRebootText
                      : Resources.ServiceRemovedText);
      }
      else
      {
        ReportStatus(Resources.ServiceManualRemoveRequiredText);
      }

      return deleteOperationSucceeded ?? false;
    }

    /// <summary>
    /// Deletes the temporary user account created when changing the default authentication method..
    /// </summary>
    /// <returns><c>true</c> if the user account was deleted successfully or if it did not exist, <c>false</c> otherwise.</returns>
    private bool DeleteTemporaryUserAccount()
    {
      ReportStatus(string.Format(Resources.ServerConfigDeletingTemporaryUser, TemporaryServerUser.Username));
      var adminUser = MySqlServerUser.GetLocalRootUser(Settings.RootPassword, Settings.DefaultAuthenticationPlugin);
      var success = DeleteServerUserAccount(adminUser, TemporaryServerUser);
      ReportStatus(success
        ? string.Format(Resources.ServerConfigDeletingTemporaryUserSuccess, TemporaryServerUser.Username)
        : Resources.ServerConfigDeletingTemporaryUserError);
      if (success)
      {
        TemporaryServerUser = null;
      }

      return success;
    }

    /// <summary>
    /// Disposes of the timer.
    /// </summary>
    private void DisposeTimer()
    {
      if (_timer == null)
      {
        return;
      }

      _timer.Dispose();
      _timer = null;
    }

    /// <summary>
    /// Executes the steps that perform the configuration operation.
    /// </summary>
    private void DoConfigure()
    {
      foreach (var step in ConfigurationSteps.Where(step => step.ValidForConfigureType(ConfigurationType) && step.Execute).TakeWhile(step => !CancellationTokenSource.IsCancellationRequested))
      {
        CurrentStep = step;
        // report starting
        ReportStatus(ConfigurationEventType.StepStarting, "Executing step: " + step.Description);
        step.Status = ConfigurationStepStatus.Started;

        // now do the configure step
        try
        {
          step.Status = ConfigurationStepStatus.Finished;
          step.StepMethod();
        }
        catch (Exception ex)
        {
          step.Status = ConfigurationStepStatus.Error;
          ReportError(ex.Message);
        }

        // report stop
        ReportStatus(ConfigurationEventType.StepFinished, "Completed execution of step: " + step.Description);
        if (step.Required && step.Status == ConfigurationStepStatus.Error)
        {
          break;
        }
      }
    }

    /// <summary>
    /// Executes the steps that performs the uninstall operation.
    /// </summary>
    private void DoRemove()
    {
      RemoveStep previousStep = null;
      foreach (var step in RemoveSteps.Where(step => step.Execute).TakeWhile(step => !CancellationTokenSource.IsCancellationRequested))
      {
        CurrentStep = step;
        // Report starting,
        ReportStatus(ConfigurationEventType.StepStarting, $"Completed execution of step: {step.Description}");
        step.Status = ConfigurationStepStatus.Started;

        // Now do the remove step.
        try
        {
          step.StepMethod();
        }
        catch (Exception ex)
        {
          step.Status = ConfigurationStepStatus.Error;
          ReportError(ex.Message);
        }

        // Report stop.
        ReportStatus(ConfigurationEventType.StepFinished, $"Ended remove step: {step.Description}");
        if (step.Required && step.Status == ConfigurationStepStatus.Error)
        {
          CurrentState = ConfigState.ConfigurationError;
          break;
        }

        previousStep = step;
      }
    }

    /// <summary>
    /// Marks the configuration as complete.
    /// </summary>
    private void EndConfigure()
    {
      DisposeTimer();
      FinalizeConfigState();
      Logger.LogInformation("Finished configuration of " + ServerInstallation.NameWithVersion + " with state " + CurrentState);
      ConfigurationEnded?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Reports the completion of the uninstall operation.
    /// </summary>
    private void EndRemove()
    {
      DisposeTimer();
      FinalizeRemoveState();
      Logger.LogInformation("Finished removal of " + ServerInstallation.NameWithVersion + " with state " + CurrentState);
      ConfigurationEnded?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Marks the config state as finalized.
    /// </summary>
    private void FinalizeConfigState()
    {
      CurrentState = ConfigState.ConfigurationComplete;
      //if (ConfigurationCancelled)
      if (CancellationTokenSource.IsCancellationRequested)
      {
        CurrentState = ConfigState.ConfigurationCancelled;
        ResetCancellationToken();
        return;
      }

      // Check if any of the steps generated an error.
      if (ConfigurationSteps.Any(step => step.ValidForConfigureType(ConfigurationType) && step.Execute && step.Status == ConfigurationStepStatus.Error))
      {
        CurrentState = ConfigState.ConfigurationError;
      }
    }

    /// <summary>
    /// Finalizes the overall uninstall operation.
    /// </summary>
    private void FinalizeRemoveState()
    {
      CurrentState = ConfigState.ConfigurationComplete;
      if (CancellationTokenSource.IsCancellationRequested)
      {
        CurrentState = ConfigState.ConfigurationCancelled;
        return;
      }

      // Check if any of the required steps generated an error.
      if (RemoveSteps.Any(step => step.Execute && step.Required && step.Status == ConfigurationStepStatus.Error))
      {
        CurrentState = ConfigState.ConfigurationError;
      }
    }

    /// <summary>
    /// Gets a unique pipe or shared memory name based on the existing pipe names.
    /// </summary>
    /// <param name="pipe">A flag indicating if a pipe or shared memory name should be obtained.<</param>
    /// <returns>A unique name for the pipe or shared memory name.</returns>
    private string GetUniquePipeOrMemoryName(bool pipe)
    {
      string name;
      int postfix = 0;
      while (true)
      {
        name = postfix == 0 ? "MYSQL" : "MYSQL_" + postfix;
        if (!PipeOrMemoryNameExists(name, pipe)) break;
        postfix++;
      }

      return name;
    }

    /// <summary>
    /// Gets a <see cref="MySqlServerUser"/> that can connect to the server before the root user account is updated.
    /// </summary>
    /// <returns>A <see cref="MySqlServerUser"/> that can connect to the server before the root user account is updated.</returns>
    private MySqlServerUser GetUserAccountToConnectBeforeUpdatingRootUser()
    {
      return DefaultAuthenticationPluginChanged
        && TemporaryServerUser != null
          ? TemporaryServerUser
          : MySqlServerUser.GetLocalRootUser(
            IsUpdateAuthenticationPluginStepNeeded
              ? ExistingServerInstallationInstance.ConfigurationRootPassword
              : Settings.ExistingRootPassword,
            IsUpdateAuthenticationPluginStepNeeded
              ? RootUserAuthenticationPlugin
              : Settings.DefaultAuthenticationPlugin);
    }

    /// <summary>
    /// Initializes the server.
    /// </summary>
    private void InitializeServer()
    {
      CancellationToken.ThrowIfCancellationRequested();

      // Check if the installation of the server include the data files, or if the user is retrying a failed configuration, if is true delete them first
      var dataSubDirectory = Path.Combine(DataDirectory, "data");
      if (Directory.Exists(dataSubDirectory)
          && Directory.EnumerateFileSystemEntries(dataSubDirectory).Any())
      {
        ReportStatus(Resources.ServerConfigInitializeDatabaseDeletingDataDirText);
        Directory.Delete(dataSubDirectory, true);

        // Needs to reset the existing password if retrying configuration
        Settings.ExistingRootPassword = string.Empty;
      }

      CancellationToken.ThrowIfCancellationRequested();
      ReportStatus(Resources.ServerConfigInitializeDatabaseRunningInitializeInsecureText);
      var localServerInstance = new MySqlServerInstance(this, ReportErrLogLine)
      {
        WaitUntilAcceptingConnections = false
      };
      var success = localServerInstance.StartInstanceAsProcess($"--initialize-insecure=on --lower-case-table-names={(int)Settings.LowerCaseTableNames} {Settings.Plugins.GetActivationStateCommandOptions()}");
      ReportStatus(success
        ? string.Format(Resources.ServerConfigInitializeDatabaseSuccessText, localServerInstance.NameWithVersion)
        : Resources.ServerConfigInitializeDatabaseFailedText);
      CurrentStep.Status = success
        ? ConfigurationStepStatus.Finished
        : ConfigurationStepStatus.Error;
    }

    /// <summary>
    /// Installs the enterprise firewall plugin by running the installation script against the server.
    /// </summary>
    private void InstallEnterpriseFirewallPlugin()
    {
      CancellationToken.ThrowIfCancellationRequested();
      ReportStatus(Resources.ServerConfigEventConfigureEnterpriseFirewallInfo);
      string connectionString = GetConnectionString(MySqlServerUser.ROOT_USERNAME, Settings.RootPassword, false, "mysql");
      try
      {
        // Read the install script from the share folder.
        string firewallScript;
        using (var reader = new StreamReader($"{InstallDirectory}\\share\\win_install_firewall.sql"))
        {
          firewallScript = reader.ReadToEnd();
        }

        if (string.IsNullOrEmpty(firewallScript))
        {
          ReportError(string.Format(Resources.ServerConfigEventConfigureEnterpriseFirewallError, Resources.InstallEnterpriseFirewallScriptNotFound));
          CurrentStep.Status = ConfigurationStepStatus.Error;
          return;
        }

        // Generate the uninstall script and execute it.
        using (var connection = new MySqlConnection(connectionString))
        {
          var uninstallScript = ConvertToUninstallScript("mysql", firewallScript);
          var script = new MySqlScript
          {
            Connection = connection,
            Query = uninstallScript
          };
          connection.Open();
          script.Execute();
          script.Query = Resources.UninstallEnterpriseFirewall;
          script.Execute();
        }

        // Execute the install scripts.
        CancellationToken.ThrowIfCancellationRequested();
        if (Settings.Plugins.IsEnabled("mysql_firewall"))
        {
          using (var connection = new MySqlConnection(connectionString))
          {
            var script = new MySqlScript
            {
              Connection = connection,
              Query = firewallScript
            };
            connection.Open();
            script.Execute();
            var command = new MySqlCommand
            {
              Connection = connection,
              CommandText = "sp_set_firewall_mode",
              CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@arg_userhost", "root@localhost");
            command.Parameters.AddWithValue("@arg_mode", "RECORDING");
            command.ExecuteNonQuery();

            foreach (var serverUser in Settings.NewServerUsers)
            {
              var cmdUser = new MySqlCommand
              {
                Connection = connection,
                CommandText = "sp_set_firewall_mode",
                CommandType = CommandType.StoredProcedure
              };
              cmdUser.Parameters.AddWithValue("@arg_userhost", $"{serverUser.Username}@{serverUser.Host}");
              cmdUser.Parameters.AddWithValue("@arg_mode", "RECORDING");
              cmdUser.ExecuteNonQuery();
            }
          }

          ReportStatus(Resources.ServerConfigEventConfigureEnterpriseFirewallSuccess);
          CurrentStep.Status = ConfigurationStepStatus.Finished;
        }
      }
      catch (Exception e)
      {
        ReportError(string.Format(Resources.ServerConfigEventConfigureEnterpriseFirewallError, e));
        CurrentStep.Status = ConfigurationStepStatus.Error;
      }
    }

    /// <summary>
    /// Loads the execution steps.
    /// </summary>
    private void LoadConfigurationSteps()
    {
      // Initialize configuration steps.
      _backupDatabaseStep = new ConfigurationStep(Resources.ServerConfigBackupDatabaseStep, 60, BackupDatabase, true, ConfigurationType.Upgrade);
      _createRemoveExampleDatabasesStep = new ConfigurationStep(Resources.ServerUpdateExampleDatabasesText, 10, CreateRemoveExampleDatabases, false, ConfigurationType.Configure | ConfigurationType.Reconfigure);
      _initializeServerConfigurationStep = new ConfigurationStep(Resources.ServerInitializeDatabaseStep, 900, InitializeServer, true, ConfigurationType.Configure | ConfigurationType.Reconfigure | ConfigurationType.Upgrade);
      //_prepareAuthenticationPluginChangeStep = new ConfigurationStep(Resources.ServerPrepareAuthenticationPluginChangeStep, 20, PrepareAuthenticationPluginChange, true, ConfigurationType.Reconfigure | ConfigurationType.Upgrade);
      _removeExistingServerInstallationStep = new ConfigurationStep(Resources.ServerRemoveOldInstallationStep, 60, RemoveExistingServerInstallationStep, false, ConfigurationType.Upgrade);
      _renameExistingDataDirectoryStep = new ConfigurationStep(Resources.RenameExistingDataDirectoryStep, 10, RenameExistingDataDirectoryStep, false, ConfigurationType.Upgrade);
      _resetPersistedVariablesStep = new ConfigurationStep(Resources.ServerResetPersistedVariablesStep, 20, ResetPersistedVariablesStep, false, ConfigurationType.Upgrade);
      _startAndUpgradeServerConfigStep = new ConfigurationStep(Resources.ServerStartAndUpgradeProcessStep, 3600, StartAndUpgradeServerStep, true, ConfigurationType.Upgrade);
      _startServerConfigurationStep = new ConfigurationStep(Resources.ServerStartProcessStep, 90, StartServerStep);
      _stopExistingServerInstanceStep = new ConfigurationStep(Resources.StoppingExistingServerInstanceStep, 40, StopExistingServerInstance, true, ConfigurationType.Upgrade);
      _stopServerConfigurationStep = new ConfigurationStep(Resources.ServerStopProcessStep, 40, StopServerSafe);
      _updateAccessPermissions = new ConfigurationStep(Resources.ServerUpdateServerFilePermissions, 10, UpdateServerFilesPermissions, false, ConfigurationType.Configure | ConfigurationType.Reconfigure | ConfigurationType.Upgrade);
      _updateAuthenticationPluginStep = new ConfigurationStep(Resources.ServerUpdateAuthenticationPluginStep, 10, UpdateAuthenticationPlugin, true, ConfigurationType.Upgrade);
      _updateEnterpriseFirewallPluginConfigStep = new ConfigurationStep(Resources.ServerEnableEnterpriseFirewallStep, 45, InstallEnterpriseFirewallPlugin, true, ConfigurationType.Configure | ConfigurationType.Reconfigure | ConfigurationType.Upgrade);
      _updateProcessStep = new ConfigurationStep(Resources.ServerAdjustProcessStep, 10, UpdateProcessSettings, true, ConfigurationType.Configure | ConfigurationType.Reconfigure | ConfigurationType.Upgrade);
      _updateStartMenuLinksStep = new ConfigurationStep(Resources.ServerUpdateStartMenuLinkStep, 20, UpdateStartMenuLink, false, ConfigurationType.Configure | ConfigurationType.Reconfigure | ConfigurationType.Upgrade);
      _updateSecurityStep = new ConfigurationStep(Resources.ServerApplySecurityStep, 20, UpdateSecurity, true, ConfigurationType.Configure | ConfigurationType.Reconfigure | ConfigurationType.Upgrade);
      _updateUsersStep = new ConfigurationStep(Resources.ServerCreateUsersStep, 20, UpdateUsers, true, ConfigurationType.Configure | ConfigurationType.Reconfigure | ConfigurationType.Upgrade);
      _updateWindowsFirewallRulesStep = new ConfigurationStep(Resources.ServerUpdateWindowsFirewallStep, 40, UpdateWindowsFirewall, false, ConfigurationType.Configure | ConfigurationType.Reconfigure);
      _updateWindowsServiceStep = new ConfigurationStep(Resources.ServerAdjustServiceStep, 25, UpdateServiceSettings, true, ConfigurationType.Configure | ConfigurationType.Reconfigure | ConfigurationType.Upgrade); ;
      _upgradeStandAloneServerStep = new ConfigurationStep(Resources.ServerUpgradeStep, 3600, UpgradeServer, true, ConfigurationType.Upgrade);
      _writeConfigurationFileStep = new ConfigurationStep(Resources.ServerWriteConfigFileStep, 20, WriteConfigurationFile);
      
      LoadServerConfigurationSteps();
      LoadSelfContainedUpgradeSteps();

      // Use any of the collections related to a stand-alone server setup.
      ConfigurationSteps = StandAloneServerSteps;
    }

    /// <summary>
    /// Loads the steps that are specific to a stand alone configuration.
    /// </summary>
    private void LoadServerConfigurationSteps()
    {
      _classicConfigurationSteps = new List<ConfigurationStep>
      {
        //_prepareAuthenticationPluginChangeStep,
        _stopServerConfigurationStep,
        _writeConfigurationFileStep,
        _updateWindowsFirewallRulesStep,
        _updateWindowsServiceStep,
        _updateProcessStep,
        _upgradeStandAloneServerStep,
        _initializeServerConfigurationStep,
        _updateAccessPermissions,
        _startServerConfigurationStep,
        _updateSecurityStep,
        _updateUsersStep,
        _updateEnterpriseFirewallPluginConfigStep,
        _updateStartMenuLinksStep,
        _createRemoveExampleDatabasesStep
      };
    }

    /// <summary>
    /// Loads the steps that are specific to self-contained upgrades.
    /// </summary>
    private void LoadSelfContainedUpgradeSteps()
    {
      _selfContainedUpgradeSteps = new List<ConfigurationStep>
      {
        _resetPersistedVariablesStep,
        _backupDatabaseStep,
        _updateAuthenticationPluginStep,
        _stopExistingServerInstanceStep,
        _renameExistingDataDirectoryStep,
        _writeConfigurationFileStep,
        _stopServerConfigurationStep,
        _updateAccessPermissions,
        _updateWindowsServiceStep,
        _startAndUpgradeServerConfigStep,
        _updateSecurityStep,
        _updateStartMenuLinksStep,
        _removeExistingServerInstallationStep
      };
    }

    /// <summary>
    /// Loads the steps that are specific to a production InnoDB Cluster configuration.
    /// </summary>
    private void LoadRemovalConfigurationSteps()
    {
      _deleteConfigurationFileStep = new RemoveStep(Resources.ServerDeleteConfigurationFileStep, 20, DeleteConfigurationFileStep, false);
      _deleteDataDirectoryStep = new RemoveStep(Resources.ServerDeleteDataDirectoryStep, 20, DeleteDataDirectoryStep, true);
      _deleteServiceStep = new RemoveStep(Resources.ServerDeleteServiceStep, 25, DeleteServiceStep, false);
      _removeFirewallRuleStep = new RemoveStep(Resources.ServerRemoveFirewallRulesStep, 40, RemoveFirewallRulesStep, false);
      _stopServerRemoveStep = new RemoveStep(Resources.ServerStopProcessStep, 40, StopServerSafe, false);
      var baseRemoveSteps = new List<RemoveStep>()
      {
        _stopServerRemoveStep,
        _deleteServiceStep,
        _removeFirewallRuleStep,
        _deleteConfigurationFileStep,
        _deleteDataDirectoryStep
      };
      _classicServerRemoveSteps = baseRemoveSteps;
      RemoveSteps = _classicServerRemoveSteps;
    }

    /// <summary>
    /// Loads the roles that are available for users.
    /// </summary>
    /// <returns><c>true</c> if the roles were loaded; otherwise, <c>false</c>.</returns>
    private bool LoadRoles()
    {
      bool loadedRoles = false;
      var xmlSerializer = new XmlSerializer(typeof(RoleDefinitions));
      var assembly = Assembly.GetExecutingAssembly();
      //var resources = assembly.GetManifestResourceNames();
      using (Stream stream = assembly.GetManifestResourceStream($"MySql.Configurator.Resources.{AppConfiguration.ROLE_DEFINITIONS_FILE}"))
      {
        try
        {
          RolesDefined = (RoleDefinitions)xmlSerializer.Deserialize(stream);
          loadedRoles = true;
        }
        catch (Exception e)
        {
          Logger.LogError($"Error during XML parsing of file {AppConfiguration.ROLE_DEFINITIONS_FILE}. The error was:\n\t{e}");
        }
        finally
        {
          stream.Close();
        }
      }

      return loadedRoles;
    }

    /// <summary>
    /// Loads the ini template associated for this server installation.
    /// </summary>
    /// <returns>The ini template.</returns>
    private IniTemplate LoadTemplate()
    {
      string templateBase = "my-template{0}.ini";
      var version = ServerInstallation.Version;
      string templateFile = null;
      if ((version.Major >= 8
          && version.Minor > 0)
          || version.Major == 9)
      {
        templateFile = string.Format(templateBase, $"-{version.Major}.x");
      }
      else
      {
        templateFile = string.Format(templateBase, $"-{version.Major}.{version.Minor}");
      }

      return new IniTemplate(InstallDirectory,
                             DataDirectory,
                             templateFile,
                             Settings.IniDirectory,
                             !string.IsNullOrEmpty(Settings.ConfigFile) ? Settings.ConfigFile : MySqlServerSettings.DEFAULT_CONFIG_FILE_NAME,
                             version,
                             Settings.ServerInstallationType,
                             _revertController);
    }

    /// <summary>
    /// Gets the next non whitespace char in a string.
    /// </summary>
    /// <param name="arguments">The value to check.</param>
    /// <param name="index">The starting index.</param>
    /// <returns>The next non whitespace char.</returns>
    private char NextNonWhitespaceChar(string arguments, ref int index)
    {
      char nextNonWhitespaceChar = char.MinValue;
      do
      {
        if (char.IsWhiteSpace(arguments, ++index))
        {
          continue;
        }

        nextNonWhitespaceChar = arguments[index];
        break;
      }
      while (index < arguments.Length);
      return nextNonWhitespaceChar;
    }

    /// <summary>
    /// Runs mysql_upgrade.exe after a server upgrade.
    /// </summary>
    /// <returns><c>true</c> on success, <c>false</c> otherwise.</returns>
    private bool ProcessMySqlUpgradeTool()
    {
      if (_upgradingInstance == null)
      {
        _upgradingInstance = new MySqlServerInstance(this, ReportStatus);
      }

      var success = _upgradingInstance.IsRunning;
      if (!success)
      {
        CancellationToken.ThrowIfCancellationRequested();
        // Step 1: Start Server (if not already running) in order to be able to run mysql_upgrade tool
        ReportStatus(Resources.ServerConfigMySqlUpgradeStartServer);
        success = _upgradingInstance.StartInstanceAsProcess();
      }

      string errorMessage = null;
      if (success)
      {
        CancellationToken.ThrowIfCancellationRequested();
        // Step 2: If Server started successfully, run mysql_upgrade tool
        ReportStatus(Resources.ServerConfigMySqlUpgradeRunning);
        var binDirectory = Path.Combine(InstallDirectory, BINARY_DIRECTORY_NAME);
        var user = GetUserAccountToConnectBeforeUpdatingRootUser();
        var connectionOptions = string.Empty;
        bool sendingPasswordInCommandLine = false;
        string tempConfigFileWithPassword = null;
        if (!string.IsNullOrEmpty(user.Password))
        {
          tempConfigFileWithPassword = Base.Classes.Utilities.CreateTempConfigurationFile(IniFile.GetClientPasswordLines(user.Password));
          sendingPasswordInCommandLine = tempConfigFileWithPassword == null;
          connectionOptions = sendingPasswordInCommandLine
            ? $"--password={user.Password} "
            : $"--defaults-extra-file=\"{tempConfigFileWithPassword}\" ";
        }

        CancellationToken.ThrowIfCancellationRequested();
        connectionOptions += GetCommandLineConnectionOptions(user, false, true);
        var arguments = $" {connectionOptions} --force --verbose";
        var upgradeToolProcessResult = Base.Classes.Utilities.RunProcess(
          Path.Combine(binDirectory, UPGRADE_TOOL_EXECUTABLE_FILENAME),
          arguments,
          binDirectory,
          ReportStatus,
          ReportStatus,
          true,
          sendingPasswordInCommandLine);
        Base.Classes.Utilities.DeleteFile(tempConfigFileWithPassword, 10, 500);
        success = upgradeToolProcessResult.ExitCode == 0;
        if (!success)
        {
          errorMessage = Resources.ServerConfigMySqlUpgradeRunError;
        }

        CancellationToken.ThrowIfCancellationRequested();
        // Step 3: Shutdown the Server so that any changes made to the system tables take effect in the next start
        ReportStatus(Resources.ServerConfigMySqlUpgradeShutdownServer);
        if (!_upgradingInstance.ShutdownInstance(true))
        {
          _upgradingInstance.KillInstanceProcess();
        }
      }
      else
      {
        errorMessage = Resources.ServerConfigMySqlUpgradeServerStartError;
      }

      ReportStatus(success
        ? Resources.ServerConfigMySqlUpgradeRunSuccess
        : errorMessage);
      return success;
    }

    /// <summary>
    /// Removes the Windows Firewall rule craeted during the server configuration.
    /// </summary>
    /// <param name="port">The port number.</param>
    private bool RemoveFirewallRule(uint port)
    {
      CancellationToken.ThrowIfCancellationRequested();
      return ConfigureWindowsFirewallRule("delete", port.ToString(), string.Empty);
    }

    /// <summary>
    /// Step to trigger the removal of the Windows Firewall rules created during the server configuration.
    /// </summary>
    private void RemoveFirewallRulesStep()
    {
      CancellationToken.ThrowIfCancellationRequested();
      _firewallRulesList.Clear();
      ReportStatus(string.Format(Resources.RemovingFirewallRuleText, Settings.Port));

      var removedXProtocolFirewallRule = true;
      if (Settings.OpenFirewallForXProtocol
          && Settings.MySqlXPort != 0)
      {
        removedXProtocolFirewallRule = RemoveFirewallRule(Settings.MySqlXPort);
      }

      CurrentStep.Status = RemoveFirewallRule(Settings.Port)
        && removedXProtocolFirewallRule
        ? ConfigurationStepStatus.Finished
        : ConfigurationStepStatus.Error;
    }

    /// <summary>
    /// Configuration step that removes an existing MySQL Server installation being replaced with the one being configured.
    /// </summary>
    private void RemoveExistingServerInstallationStep()
    {
      if (ConfigurationType != ConfigurationType.Upgrade)
      {
        throw new Exception(Resources.ExistingServerInstanceNotSetError);
      }

      // Remove settings file.
      GeneralSettingsManager.DeleteGeneralSettingsFile(ExistingServerInstallationInstance.BaseDir);

      ReportStatus(Resources.ServerConfigRemovingExistingInstance);
      // Determine if the server to remove was installed using MSI
      var serverProductCode = Base.Classes.Utilities.FindInstalledServerProductCode(ExistingServerInstallationInstance.ServerVersion,
                                                                                    ExistingServerInstallationInstance.BaseDir);
      if (!string.IsNullOrEmpty(serverProductCode))
      {
        // Uninstall the MSI
        MsiInterop.MsiSetInternalUI(InstallUILevel.None, IntPtr.Zero);
        var returnCode = MsiInterop.MsiConfigureProductEx(serverProductCode, InstallLevel.Default, InstallState.Default, $"REMOVE=ALL REBOOT=ReallySuppress MYSQL_INSTALLER=\"YES\"");
        if (returnCode == MsiEnumError.SuccessRebootRequired
            || returnCode == MsiEnumError.Success)
        {
          var restartRequiredText = returnCode == MsiEnumError.SuccessRebootRequired ? " but the computer requires a restart." : string.Empty;
          ReportStatus($"MySQL Server {ExistingServerInstallationInstance.ServerVersion} was successfully uninstalled{restartRequiredText}.");
          CurrentStep.Status = ConfigurationStepStatus.Finished;
        }
        else
        {
          throw new Exception($"Failed to uninstall MySQL Server {ExistingServerInstallationInstance.ServerVersion}.");
        }
      }
      else
      {
        // The existing server instance was not installed through MSI, so delete the directory.
        if (Base.Classes.Utilities.DeleteDirectory(ExistingServerInstallationInstance.BaseDir, 3, 1000, 100, out var errorMessage))
        {
          ReportStatus($"The MySQL Server {ExistingServerInstallationInstance.ServerVersion} installation directory was successfully deleted.");
          CurrentStep.Status = ConfigurationStepStatus.Finished;
        }
        else
        {
          throw new Exception($"Failed to delete MySQL Server {ExistingServerInstallationInstance.ServerVersion} installation directory: {errorMessage}");
        }
      }
    }

    /// <summary>
    /// Renames an existing data directory to the default one that reflects the MySQL Server version being configured.
    /// </summary>
    private void RenameExistingDataDirectoryStep()
    {
      var newDataDir = Regex.Replace(DataDirectory, MySqlServerInstance.DEFAULT_DATADIR_NAME_REGEX, $"MySQL Server {ServerVersion.ToString(2)}", RegexOptions.IgnoreCase);
      ReportStatus(string.Format(Resources.ServerConfigRenamingDataDirectory, DataDirectory, newDataDir));
      try
      {
        _revertController.OldDataDirPath = DataDirectory;
        _revertController.NewDataDirPath = newDataDir;
        Directory.Move(DataDirectory, newDataDir);
        ReportStatus(string.Format(Resources.ServerConfigDataDirectoryRenamed, DataDirectory, newDataDir));
        Settings.DataDirectory = newDataDir;
        Settings.IniDirectory = Settings.DataDirectory;
        Settings.SecureFilePrivFolder = Path.Combine(Settings.IniDirectory, MySqlServerSettings.SECURE_FILE_PRIV_DIRECTORY);
        _revertController.DataDirRenamed = true;
        CurrentStep.Status = ConfigurationStepStatus.Finished;
      }
      catch (Exception ex)
      {
        ReportError(string.Format(Resources.RenamingDataDirectoryError, newDataDir));
        Logger.LogException(ex);
        RevertedSteps = _revertController.Rollback(OldSettings);
        CurrentStep.Status = ConfigurationStepStatus.Error;
      }
    }

    /// <summary>
    /// Reports an error to the log.
    /// </summary>
    /// <param name="line">The message to report.</param>
    private void ReportErrLogLine(string line)
    {
      var errorLogLine = ServerErrorLogLine.Parse(line);
      ReportStatus(!errorLogLine.Parsed ? errorLogLine.UnparsedLine : errorLogLine.Message);
    }

    /// <summary>
    /// Reports a status to the selected output option.
    /// </summary>
    /// <param name="type">The type of message to report.</param>
    /// <param name="details">The message to report.</param>
    private void ReportStatus(ConfigurationEventType type, string details)
    {
      if (ConfigurationStatusChanged == null)
      {
        return;
      }

      if (!AppConfiguration.ConsoleMode
          && Application.OpenForms.Count > 0
          && Application.OpenForms[0].InvokeRequired)
      {
        Application.OpenForms[0].Invoke((MethodInvoker)delegate { ConfigurationStatusChanged(this, type, details); });
      }
      else
      {
        ConfigurationStatusChanged(this, type, details);
      }
    }

    /// <summary>
    /// Configuration step that removes deprecated server variables set using SET PERSIST.
    /// </summary>
    private void ResetPersistedVariablesStep()
    {
      if (PersistedVariablesToReset.Count == 0)
      {
        return;
      }
      
      ReportStatus(string.Format(Resources.RemovingPersistedServerVariables,
                                 PersistedVariablesToReset.Count.ToString(),
                                 string.Join(", ", PersistedVariablesToReset)));
      var removeVariablesScript = new StringBuilder();
      foreach (var deprecatedVariable in PersistedVariablesToReset)
      {
        removeVariablesScript.AppendLine($"RESET PERSIST IF EXISTS {deprecatedVariable};");
      }

      if (ExistingServerInstallationInstance.ExecuteScripts(true, removeVariablesScript.ToString()) == 1)
      {
        PersistedVariablesToReset.Clear();
        CurrentStep.Status = ConfigurationStepStatus.Finished;
      }
      else
      {
        ReportError(Resources.RemovingPersistedServerVariablesError);
        CurrentStep.Status = ConfigurationStepStatus.Error;
      }
    }

    /// <summary>
    /// Resets the timer if it has already been initialized.
    /// </summary>
    private void ResetTimer()
    {
      var value = _configurationTimeout * 1000;
      if (_timer != null)
      {
        _timer.Change(value, value);
      }

      _timer = new System.Threading.Timer(TimerElapsed, null, value, value);
      return;
    }

    /// <summary>
    /// Sets the default data directory.
    /// </summary>
    private void SetDefaultDataDir()
    {
      _dataDirectory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
        $@"MySQL\MySQL Server {ServerVersion.ToString(2)}\");
    }

    /// <summary>
    /// Sets the license based on the installation directory.
    /// </summary>
    private void SetLicenseFromLocation()
    {
      if (string.IsNullOrEmpty(InstallDirectory))
      {
        return;
      }

      //check if the directory exists, if not and is a 64bit OS try to find the install dir in program files x(86) if not, set the license as unknown
      //this fix a problem when the product is installed by 1.3 Installer
      if (!Directory.Exists(InstallDirectory))
      {
        var dirName = InstallDirectory.Split(new[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
        if (dirName.Length <= 0
            || dirName[1].ToLower() != "program files"
            || !Win32.Is64BitOs)
        {
          return;
        }

        string dir = InstallDirectory.Replace(dirName[1], dirName[1] + " (x86)");
        if (!Directory.Exists(dir))
        {
          return;
        }

        InstallDirectory = dir;
      }
    }

    /// <summary>
    /// Sets the super_read_only variable to OFF.
    /// </summary>
    /// <param name="serverInstance">A <see cref="MySqlServerInstance"/> instance used to execute the query on the local instance.</param>
    /// <param name="reportStatusAndError">Flag that indicates if the status and error message should be reported.</param>
    /// <returns><c>true</c> if the variable super_read_only was set to OFF; otherwise, <c>false</c>.</returns>
    private bool SetLocalInstanceAsWritable(MySqlServerInstance serverInstance, bool reportStatusAndError)
    {
      if (serverInstance == null)
      {
        return false;
      }

      var operationSuccessful = false;
      const string VARIABLE_NAME = "super_read_only";
      var variableValue = serverInstance.GetVariable(VARIABLE_NAME, true);
      string reportMessage = null;

      if (variableValue == null)
      {
        reportMessage = Resources.ServerConfigGetSuperReadOnlyVariableValueFailed;
      }
      else if (variableValue.ToString() == "OFF")
      {
        reportMessage = Resources.ServerConfigSetInstanceAsWritableNotRequired;
        operationSuccessful = true;
      }
      else
      {
        operationSuccessful = serverInstance.ResetPersistentVariable(VARIABLE_NAME, true)
          && serverInstance.SetVariable(VARIABLE_NAME, "OFF", true);
        reportMessage = operationSuccessful
          ? Resources.ServerConfigSetInstanceAsWritableSuccess
          : Resources.ServerConfigSetInstanceAsWritableFail;
      }

      if (reportStatusAndError
          && !string.IsNullOrEmpty(reportMessage))
      {
        ReportStatus(reportMessage);
      }

      return operationSuccessful;
    }

    /// <summary>
    /// Executes the start and upgrade configuration step.
    /// </summary>
    private void StartAndUpgradeServerStep()
    {
      CancellationToken.ThrowIfCancellationRequested();
      try
      {
        var startStatus = StartServer(true, false, "--upgrade=FORCE");
        if (startStatus.UpgradeStatus.UpgradeFailed
            || !startStatus.UpgradeStatus.UpgradeFinished)
        {
          Settings.PendingSystemTablesUpgrade = true;
          Settings.SaveGeneralSettings();
          throw new Exception(Resources.ServerConfigStartUpgradeFailed);
        }

        Settings.PendingSystemTablesUpgrade = false;
        Settings.SaveGeneralSettings();
        if (!startStatus.AcceptingConnections)
        {
          throw new Exception(Resources.ServerConfigStartUpgradeNotAcceptingConnections);
        }

        CurrentStep.Status = ConfigurationStepStatus.Finished;

        // Save the SystemTablesUpgraded back to the extended settings file
        Settings.SaveGeneralSettings();
      }
      catch (Exception ex)
      {
        RevertedSteps = _revertController.Rollback(OldSettings);
        CurrentStep.Status = ConfigurationStepStatus.Error;
        throw ex;
      }
    }

    /// <summary>
    /// Executes the start server configuration step.
    /// </summary>
    private ServerStartStatus StartServer(bool waitUntilAcceptingConnections = true, bool setStepStatus = true, string additionalOptions = null)
    {
      CancellationToken.ThrowIfCancellationRequested();
      var serverInstance = new MySqlServerInstance(this, ReportErrLogLine)
      {
        UserAccount = GetUserAccountToConnectBeforeUpdatingRootUser(),
        WaitUntilAcceptingConnections = waitUntilAcceptingConnections
      };

      var startedStatus = serverInstance.StartInstance(additionalOptions);
      var startedSuccessfully = startedStatus.Started
                                && (waitUntilAcceptingConnections && startedStatus.AcceptingConnections ||
                                    !waitUntilAcceptingConnections);
      if (setStepStatus
          && CurrentStep != null)
      {
        CurrentStep.Status = startedSuccessfully
          ? ConfigurationStepStatus.Finished
          : ConfigurationStepStatus.Error;
      }
      else if (!startedSuccessfully)
      {
        throw new Exception(Resources.StartingServerInstanceErrorText);
      }

      return startedStatus;
    }

    /// <summary>
    /// Executes the start server configuraiton step.
    /// </summary>
    private void StartServerStep()
    {
      // If configuration file was updated and server is running we need to restart it first to load the new configurations.
      // E.g. when the port is changed during a reconfiguration.
      if (IsWriteIniConfigurationFileNeeded
          && Settings.ConfigureAsService
          && MySqlServiceControlManager.GetServiceStatus(Settings.ServiceName) == System.ServiceProcess.ServiceControllerStatus.Running)
      {
        StopServerSafe();
      }

      StartServer();
    }

    /// <summary>
    /// Stops an existing MySQL Server instance that will be replaced with the server being configured.
    /// </summary>
    private void StopExistingServerInstance()
    {
      if (ConfigurationType != ConfigurationType.Upgrade)
      {
        throw new Exception(Resources.ExistingServerInstanceNotSetError);
      }

      // Try to find a running Windows service, otherwise shutdown instance.
      if (ExistingServerInstallationInstance.IsRunning)
      {
        ReportStatus(Resources.ServerConfigStoppingExistingInstance);
        CancellationToken.ThrowIfCancellationRequested();
        if (!ExistingServerInstallationInstance.ShutdownInstance())
        {
          ReportStatus(Resources.ServerConfigShutdownExistingInstanceError);
          RevertedSteps = _revertController.Rollback(OldSettings);
          CurrentStep.Status = ConfigurationStepStatus.Error;
        }

        ReportStatus(Resources.ServerConfigExistingInstanceStopped);
      }
      else
      {
        ReportStatus(Resources.ServerConfigExistingInstanceNotRunning);
      }

      _revertController.InstanceStopped = true;
      CurrentStep.Status = ConfigurationStepStatus.Finished;
    }

    /// <summary>
    /// Stops the server safe, finishing all pending transactions. (use MySQLAdmin)
    /// </summary>
    private void StopServerSafe()
    {
      StopServerSafe(true);
    }

    /// <summary>
    /// Stops the server safe, finishing all pending transactions. (use MySQLAdmin)
    /// </summary>
    /// <param name="useOldSettings">Flag indicating whether the old settings must be used instead of the new settings to build the command line options.</param>
    private void StopServerSafe(bool useOldSettings)
    {
      CancellationToken.ThrowIfCancellationRequested();
      var serverInstanceInfo = new MySqlServerInstance(this, ReportStatus);
      serverInstanceInfo.UseOldSettings = useOldSettings;

      // Set the data directory to allow the call to the ShutdownInstance method to correctly validate
      // if the server is actually running before attempting to stop it.
      // Other scenarios do not need this property to be set because the server is expected to be running
      // allowing the DataDir property getter to query the database for that value.
      if (ConfigurationType == ConfigurationType.Remove
          || (ConfigurationType == ConfigurationType.Upgrade
              && IsSameDirectoryUpgrade))
      {
        serverInstanceInfo.DataDir = DataDirectory;
      }

      var serverStopped = serverInstanceInfo.ShutdownInstance(useOldSettings);

      //CurrentStep does not exist for Remove
      if (CurrentStep != null)
      {
        CurrentStep.Status = serverStopped
          ? ConfigurationStepStatus.Finished
          : ConfigurationStepStatus.Error;
      }
    }

    /// <summary>
    /// Marks the timer as elapsed.
    /// </summary>
    /// <param name="state">The object state.</param>
    private void TimerElapsed(object state)
    {
      if (_inTimerCallback)
      {
        return;
      }

      _inTimerCallback = true;
      ConfigureTimedOut?.Invoke(this, EventArgs.Empty);
      if (!CancellationTokenSource.IsCancellationRequested)
      {
        _timer?.Change(_configurationTimeout * 1000, _configurationTimeout * 1000);
      }

      _inTimerCallback = false;
    }

    /// <summary>
    /// Updates the authentication plugin of the root user to the default authentication plugin.
    /// </summary>
    private void UpdateAuthenticationPlugin()
    {
      CancellationToken.ThrowIfCancellationRequested();
      ReportStatus(string.Format(Resources.ServerConfigUpdatingRootUserAuthPlugin, Settings.DefaultAuthenticationPlugin.GetDescription()));
      try
      {
        _revertController.OldRootUserAuthenticationPlugin = RootUserAuthenticationPlugin;
        var rootUser = MySqlServerUser.GetLocalRootUser(IsSameDirectoryUpgrade
          ? Settings.ExistingRootPassword
          : ExistingServerInstallationInstance.ConfigurationRootPassword,
          RootUserAuthenticationPlugin);
        var serverInstance = new MySqlServerInstance(Settings.Port, rootUser, ReportStatus);
        serverInstance.UpdateUserAuthenticationPlugin(
          MySqlServerUser.ROOT_USERNAME,
          rootUser.Password,
          Settings.DefaultAuthenticationPlugin);
        ReportStatus(Resources.ServerConfigUpdatedRootUserAuthPlugin);
        _revertController.AuthenticationPluginUpdated = true;
        CurrentStep.Status = ConfigurationStepStatus.Finished;
      }
      catch(Exception ex) 
      {
        ReportError(Resources.ServerConfigFailedToUpdateRootUserAuthPlugin);
        ReportError(ex.Message);
        RevertedSteps = _revertController.Rollback(OldSettings);
        CurrentStep.Status = ConfigurationStepStatus.Error;
      }
    }

    /// <summary>
    /// Sets the correct access permissions to the data directory and related files.
    /// </summary>
    private void UpdateServerFilesPermissions()
    {
      CancellationToken.ThrowIfCancellationRequested();
      if (FullControlDictionary.Count == 0)
      {
        try
        {
          var _administratorsGroup = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null);
          var _creatorOwnerUser = new SecurityIdentifier(WellKnownSidType.CreatorOwnerSid, null);
          var _systemAccountUser = new SecurityIdentifier(WellKnownSidType.LocalSystemSid, null);
          if (Settings.ConfigureAsService)
          {
            var serviceAccountUsername = Settings.ServiceAccountUsername.StartsWith(".")
                                       ? Settings.ServiceAccountUsername.Replace(".", Environment.MachineName)
                                       : Settings.ServiceAccountUsername;
            var sid = DirectoryServicesWrapper.GetSecurityIdentifier(serviceAccountUsername);
            if (sid == null)
            {
              Logger.LogError(string.Format(Resources.ServerConfigSidRetrievalFailure, Settings.ServiceAccountUsername));
            }
            else
            {
              FullControlDictionary.Add(sid, "User");
            }
          }

          FullControlDictionary.Add(_administratorsGroup, "Group");
          FullControlDictionary.Add(_creatorOwnerUser, "User");
          FullControlDictionary.Add(_systemAccountUser, "User");
        }
        catch (Exception ex)
        {
          Logger.LogException(ex);
        }
      }

      var success = true;
      ReportStatus(Resources.ServerConfigEventServerSecurityInfo);
      if (!IsThereServerDataFiles)
      {
        ReportStatus(Resources.ServerConfigDataDirectoryDoesNotExist);
        success = false;
      }
      else
      {
        // If permissions have already been updated, skip.
        if (ValidateServerFilesHaveRecommendedPermissions())
        {
          ReportStatus(Resources.ServerUpdateServerFilePermissionsNotNeeded);
          CurrentStep.Status = ConfigurationStepStatus.Finished;
          return;
        }
        
        var serverInstanceInfo = new MySqlServerInstance(this, ReportStatus);
        if (serverInstanceInfo.IsRunning)
        {
          ReportStatus(Resources.ServerConfigInstanceRunning);
          success = false;
        }
        else
        {
          var dataDirectory = Path.Combine(DataDirectory, "Data");
          var usersGroupSid = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);

          // First convert inherited permissions to explicit permissions.
          if (DirectoryServicesWrapper.DirectoryPermissionsAreInherited(dataDirectory) == true)
          {
            success = DirectoryServicesWrapper.ConvertInheritedPermissionsToExplicitPermissions(dataDirectory, true);
            ReportStatus(success
                           ? Resources.ServerConfigConvertToExplicitPermissionsSuccess
                           : Resources.ServerConfigConvertToExplicitPermissionsFailed);
          }

          // Next grant full control to selected users/groups.
          if (success)
          {
            foreach (var item in FullControlDictionary)
            {
              var accountName = DirectoryServicesWrapper.GetAccountName(item.Key);
              if (!DirectoryServicesWrapper.GrantPermissionsToDirectory(dataDirectory, accountName, FileSystemRights.FullControl, AccessControlType.Allow, true))
              {
                ReportStatus(string.Format(Resources.ServerConfigGrantedFullControlFailed, DirectoryServicesWrapper.GetAccountName(item.Key)));
                break;
              }

              ReportStatus(string.Format(Resources.ServerConfigGrantedFullControlSuccess, DirectoryServicesWrapper.GetAccountName(item.Key)));
            }
          }
          
          // Then remove access to all other users/groups.
          if (success)
          {
            // Remove all access to the users group.
            success = DirectoryServicesWrapper.RemoveGroupPermissions(dataDirectory, usersGroupSid, true);
            ReportStatus(success
                           ? string.Format(Resources.ServerConfigRemovedAccessSuccess, "users", "group")
                           : string.Format(Resources.ServerConfigRemovedAccessFailed, "users", "group"));

            // Get account names for logging purposes.
            var accountNames = new List<string>();
            foreach (var item in FullControlDictionary)
            {
              var accountName = DirectoryServicesWrapper.GetAccountName(item.Key);
              if (string.IsNullOrEmpty(accountName))
              {
                continue;
              }

              accountNames.Add(accountName);
            }

            // Remove all access to any other user/group not in the Full Control list.
            var rules = DirectoryServicesWrapper.GetAuthorizationRules(new DirectoryInfo(dataDirectory));
            foreach (FileSystemAccessRule rule in rules)
            {
              var ruleValue = rule.IdentityReference.Value;
              var account = new NTAccount(ruleValue.Contains("\\") ? ruleValue.Split('\\')[1] : ruleValue);
              if (accountNames.Contains(account.Value))
              {
                continue;
              }

              DirectoryServicesWrapper.RemoveGroupPermissions(dataDirectory, DirectoryServicesWrapper.GetSecurityIdentifier(account.Value), true);
            }
          }
        }
      }

      var message = success
        ? Resources.ServerConfigEventServerSecuritySuccess
        : Resources.ServerConfigEventServerSecurityFailed;
      ReportStatus(message);
      CurrentStep.Status = success
        ? ConfigurationStepStatus.Finished
        : ConfigurationStepStatus.Error;
    }

    /// <summary>
    /// Updates the configuration file based on the specified settings.
    /// </summary>
    /// <returns><c>true</c> if the configuration file was updated; otherwise, <c>false</c>.</returns>
    private bool UpdateConfigFile()
    {
      CancellationToken.ThrowIfCancellationRequested();
      ReportStatus(string.Format(Resources.SavingConfigurationFile, Settings.ConfigFile));
      try
      {
        if (!Directory.Exists(Settings.IniDirectory))
        {
          Directory.CreateDirectory(Settings.IniDirectory);
        }

        IniTemplate t = null;

        // If this is an upgrade, we want to load the existing ini template, this to ensure that the values
        // already configured by the user persist in the new ini file.
        if (ConfigurationType == ConfigurationType.Upgrade)
        {
          t = Settings.GetExistingIniFileTemplate();

          // Verify that the Windows service does exist
          if (OldSettings.ConfigureAsService
              && !MySqlServiceControlManager.ServiceExists(ExistingServerInstallationInstance.ServiceName)
              && IsThereServerDataFiles
              && !IsServiceRenameNeeded)
          {
            Settings.ConfigureAsService = false;
          }
        }

        CancellationToken.ThrowIfCancellationRequested();
        if (t == null)
        {
          t = LoadTemplate();
        }

        if (!Directory.Exists(Settings.SecureFilePrivFolder))
        {
          Directory.CreateDirectory(Settings.SecureFilePrivFolder);
        }

        var settings = ConfigurationType == ConfigurationType.Upgrade
          ? OldSettings
          : Settings;
        if (ConfigurationType == ConfigurationType.Upgrade
            && IsDataDirectoryRenameNeeded)
        {
          settings.SecureFilePrivFolder = Settings.SecureFilePrivFolder;
          settings.IniDirectory = Settings.IniDirectory;
          settings.InstallDirectory = Settings.InstallDirectory;
        }

        //Set the Query Cache settings if Enterprise Firewall is enabled.
        if (settings.Plugins.IsEnabled("mysql_firewall"))
        {
          settings.EnableQueryCacheType = false;
          settings.EnableQueryCacheSize = false;
        }

        CancellationToken.ThrowIfCancellationRequested();
        settings.Save(t);

        // If this is an upgrade, we need to run a second pass at updating the ini file but this time comparing
        // it against the corresponding template. This to determine if there are new sections, deprecated or new
        // variables that need to be added. During this second pass we ignore updating existing values since we
        // don't want to override the values defined by the user.
        if (ConfigurationType == ConfigurationType.Upgrade)
        {
          t = LoadTemplate();
          settings.Save(t, true, IsDataDirectoryRenameNeeded);
        }

        ReportStatus(string.Format(Resources.SavedConfigurationFile, settings.ConfigFile));
        if (!ServerVersion.ServerSupportsRegeneratingRedoLogFiles())
        {
          return true;
        }

        //we need to delete
        for (int i = 0; i < 2; i++)
        {
          CancellationToken.ThrowIfCancellationRequested();
          string datetime = DateTime.Now.GetDateTimeFormats('s')[0].Replace(':', '-');
          string logFile = Path.Combine(DataDirectory, "data", string.Format("ib_logfile{0}", i));
          string backupLogFile = Path.Combine(DataDirectory, string.Format("ib_logfile{0}_{1}", i, datetime));
          if (File.Exists(logFile))
          {
            File.Move(logFile, backupLogFile);
          }
        }

        return true;
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
        return false;
      }
    }

    /// <summary>
    /// Sets the correct permissions needed by the server to access the log files.
    /// </summary>
    private void UpdateProcessSettings()
    {
      CancellationToken.ThrowIfCancellationRequested();
      ReportStatus(Resources.ServerConfigCheckingAccessToLogs);
      try
      {
        var currentUser = WindowsIdentity.GetCurrent();
        ConfigureFilePermissionsForServerLogs(currentUser.Name);
        CurrentStep.Status = ConfigurationStepStatus.Finished;
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
        ReportStatus(Resources.ServerConfigCheckAccessToLogsFailed);
        CurrentStep.Status = ConfigurationStepStatus.Error;
      }
    }

    /// <summary>
    /// Updates the root user's account password and its authentication plugin.
    /// </summary>
    private void UpdateSecurity()
    {
      CancellationToken.ThrowIfCancellationRequested();
      var success = true;
      var message = Resources.ServerConfigEventSecuritySettingsSuccess;
      var adminUser = GetUserAccountToConnectBeforeUpdatingRootUser();
      var usingTemporaryUser = TemporaryServerUser != null && adminUser == TemporaryServerUser;
      ReportStatus(Resources.ServerConfigEventSecuritySettingsInfo);
      try
      {
        var sql = ServerInstallation.Version.ServerSupportsIdentifyClause()
                ? RolesDefined.GetCreateOrAlterUserSql(UserCrudOperationType.AlterUser, MySqlServerUser.GetLocalRootUser(Settings.RootPassword.Sanitize(), Settings.DefaultAuthenticationPlugin))
                : $"UPDATE mysql.user SET Password=Password('{Settings.RootPassword.Sanitize()}') WHERE User='{MySqlServerUser.ROOT_USERNAME}'";
        string connectionString = GetConnectionString(adminUser, usingTemporaryUser, "mysql");
        using (var c = new MySqlConnection(connectionString))
        {
          c.Open();
          // Use the old way if is 5.7.5 or older
          var cmd = new MySqlCommand(sql, c);
          cmd.ExecuteNonQuery();
          if (string.IsNullOrEmpty(Settings.ExistingRootPassword))
          {
            // Delete the user account with empty username that might have been created when the Server was initialized insecurely
            adminUser.Password = Settings.RootPassword;
            DeleteServerUserAccount(adminUser, new MySqlServerUser(), DefaultAuthenticationPluginChanged);
          }
        }
      }
      catch (Exception ex)
      {
        success = false;
        Logger.LogException(ex);
        message = string.Format(Resources.ServerConfigEventSecuritySettingsError, ex.Message);
      }
      finally
      {
        if (usingTemporaryUser)
        {
          DeleteTemporaryUserAccount();
        }
      }

      // If any step before UpdateSecurity fails and user try to apply again the root user will have the new password set and should be used instead of blank
      Settings.ExistingRootPassword = Settings.RootPassword;

      ReportStatus(message);
      CurrentStep.Status = success
        ? ConfigurationStepStatus.Finished
        : ConfigurationStepStatus.Error;
    }

    private void UpdateServiceSettings()
    {
      CancellationToken.ThrowIfCancellationRequested();

      // If server was previously configured as a service but now it will run as a process.
      bool existingService = (OldSettings != null
                              && OldSettings.ServiceExists());
      bool isNew = ConfigurationType == ConfigurationType.Configure;
      if (existingService
          && !isNew
          && ConfigurationType != ConfigurationType.Upgrade
          && (!Settings.ConfigureAsService
              || OldSettings.ServiceName != Settings.ServiceName))
      {
        // Delete and report as the service not existing.
        ReportStatus(Resources.ServerConfigDeletingExistingService);
        var deleteOperationSucceeded = MySqlServiceControlManager.Delete(OldSettings.ServiceName);
        if (deleteOperationSucceeded != null)
        {
          RebootRequired = !Convert.ToBoolean(deleteOperationSucceeded);
          ReportStatus(RebootRequired
                      ? Resources.ServiceRemoveAfterRebootText
                      : Resources.ServiceRemovedText);
        }
        else
        {
          ReportStatus(Resources.ServiceManualRemoveRequiredText);
        }

        existingService = false;
      }

      CancellationToken.ThrowIfCancellationRequested();
      if (Settings.ConfigureAsService)
      {
        CheckServicePermissions();
        var cmd = $"\"{Path.Combine(InstallDirectory, BINARY_DIRECTORY_NAME, SERVER_EXECUTABLE_FILENAME)}\" --defaults-file=\"{Path.Combine(Settings.IniDirectory, Settings.ConfigFile)}\" {Settings.ServiceName}";
        if (existingService 
          && !isNew)
        {
          var newServiceName = ConfigurationType == ConfigurationType.Upgrade
            && IsServiceRenameNeeded
              ? Settings.GetDefaultServiceName()
              : Settings.ServiceName;
          ReportStatus(Resources.ServerConfigUpdatingExistingService);
          if (!newServiceName.Equals(Settings.ServiceName))
          {
            _revertController.OldServiceName = ExistingServerInstallationInstance.ServiceName;
            _revertController.NewServiceName = newServiceName;
            _revertController.ServiceCommand = Regex.Replace(cmd, newServiceName, ExistingServerInstallationInstance.ServiceName, RegexOptions.IgnoreCase);
            _revertController.ServiceCommand = _revertController.ServiceCommand.Replace(ServerVersion.ToString(2), ExistingServerInstallationInstance.ServerVersion.ToString(2));
            _revertController.ServiceRenamed = true;
            ReportStatus(string.Format(Resources.ServerConfigUpdatingExistingServiceWithNewName, ExistingServerInstallationInstance.ServiceName, newServiceName));
            cmd = $"\"{Path.Combine(InstallDirectory, BINARY_DIRECTORY_NAME, SERVER_EXECUTABLE_FILENAME)}\" --defaults-file=\"{Path.Combine(Settings.IniDirectory, Settings.ConfigFile)}\" {newServiceName}";
            MySqlServiceControlManager.Delete(ExistingServerInstallationInstance.ServiceName);
            MySqlServiceControlManager.Add(newServiceName, newServiceName, cmd, Settings.ServiceAccountUsername, Settings.ServiceAccountPassword, Settings.ServiceStartAtStartup);
            Settings.ServiceName = newServiceName;

            // If new service does not exist fail step.
            if (!Settings.ServiceExists())
            {
              RevertedSteps = _revertController.Rollback(OldSettings);
              CurrentStep.Status = ConfigurationStepStatus.Error;
            }

            return;
          }
          else
          {
            MySqlServiceControlManager.Update(ConfigurationType != ConfigurationType.Upgrade
            ? Settings.ServiceName
            : OldSettings.ServiceName,
            Settings.ServiceName,
            Settings.ServiceName,
            cmd,
            Settings.ServiceAccountUsername,
            Settings.ServiceAccountPassword,
            Settings.ServiceStartAtStartup);
          }
          
          ReportStatus(Resources.ServerConfigServiceUpdated);
        }
        else
        {
          ReportStatus(Resources.ServerConfigAddingService);
          MySqlServiceControlManager.Add(Settings.ServiceName, Settings.ServiceName, cmd, Settings.ServiceAccountUsername, Settings.ServiceAccountPassword, Settings.ServiceStartAtStartup);
          ReportStatus(Resources.ServerConfigServiceAdded);
        }
      }

      CurrentStep.Status = ConfigurationStepStatus.Finished;
    }

    private void UpdateStartMenuLink()
    {
      CancellationToken.ThrowIfCancellationRequested();
      ReportStatus(Resources.ServerConfigEventShortcutInfo);

      try
      {
        string mysqlStartMenu = $"{((Folder2)GetShell32NameSpaceFolder(ShellSpecialFolderConstants.ssfCOMMONSTARTMENU)).Self.Path}\\Programs\\MySQL\\MySQL Server {ServerInstallation.Version.ToString(2)}\\";
        var folder = GetShell32NameSpaceFolder(mysqlStartMenu);
        if (folder == null)
        {
          ReportStatus(string.Format(Resources.PathDoesNotExist, mysqlStartMenu));
          CurrentStep.Status = ConfigurationStepStatus.Error;
          return;
        }

        var newConfigFilePath = Settings.FullConfigFilePath;
        foreach (FolderItem fi in folder.Items())
        {
          bool saveLink = false;
          CancellationToken.ThrowIfCancellationRequested();
          if (!fi.IsLink || !fi.Name.Contains("Command Line Client"))
          {
            continue;
          }

          var link = (ShellLinkObject)fi.GetLink;
          string arguments = link.Arguments;
          int index = arguments.IndexOf("defaults-file", StringComparison.Ordinal);
          if (index < 0)
          {
            continue;
          }

          index += "defaults-file".Length - 1;
          char next = NextNonWhitespaceChar(arguments, ref index);
          if (next != '=')
          {
            continue;
          }

          next = NextNonWhitespaceChar(arguments, ref index);
          int startIndex = index;
          if (next == '"')
          {
            startIndex = index + 1;
          }

          int endIndex = arguments.IndexOf("\"", startIndex, StringComparison.Ordinal);
          if (endIndex == -1)
          {
            endIndex = arguments.Length;
          }

          string defaultsFileArg = arguments.Substring(startIndex, endIndex - startIndex);
          if (!string.IsNullOrEmpty(newConfigFilePath)
              && !defaultsFileArg.Equals(newConfigFilePath, StringComparison.InvariantCulture))
          {
            arguments = arguments.Replace(defaultsFileArg, newConfigFilePath);
            saveLink = true;
          }

          var namedPipeString = " \"--protocol=Pipe\" \"--get-server-public-key\" \"--ssl-mode=DISABLED\"";
          if (Settings.IsNamedPipeTheOnlyEnabledProtocol)
          {
            arguments += namedPipeString;
            saveLink = true;
          }
          else if (arguments.Contains(namedPipeString))
          {
            arguments = arguments.Replace(namedPipeString, "");
            saveLink = true;
          }

          if (saveLink)
          {
            arguments = arguments.Replace("  ", " ").Trim();
            link.Arguments = arguments;
            link.Save(fi.Path);
          }

          ReportStatus(Resources.ServerConfigEventShortcutSucceeded);
        }

        CurrentStep.Status = ConfigurationStepStatus.Finished;
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
        ReportStatus(Resources.ServerConfigEventShortcutError);
        if (CurrentStep != null)
        {
          CurrentStep.Status = ConfigurationStepStatus.Error;
        }
      }
    }

    private void UpdateUsers()
    {
      if (Settings.NewServerUsers.Count <= 0)
      {
        return;
      }

      CancellationToken.ThrowIfCancellationRequested();
      ReportStatus(Resources.ServerConfigEventAddNewUsersInfo);
      var success = CreateOrAlterUserAccounts(MySqlServerUser.GetLocalRootUser(Settings.RootPassword, Settings.DefaultAuthenticationPlugin), Settings.NewServerUsers, true);
      ReportStatus(success
        ? Resources.ServerConfigEventAddNewUsersSuccess
        : Resources.ServerConfigEventAddNewUsersError);
      CurrentStep.Status = success
        ? ConfigurationStepStatus.Finished
        : ConfigurationStepStatus.Error;
    }

    private void UpdateWindowsFirewall()
    {
      CancellationToken.ThrowIfCancellationRequested();
      _firewallRulesList.Clear();
      bool isDataDirectoryConfigured = IsDataDirectoryConfigured;
      if ((ConfigurationType == ConfigurationType.Reconfigure
           || isDataDirectoryConfigured)
          && OldSettings != null
          && OldSettings.OpenFirewallForXProtocol
          && OldSettings.MySqlXPort != 0)
      {
        RemoveFirewallRule(OldSettings.Port);
      }

      CancellationToken.ThrowIfCancellationRequested();
      if (Settings.OpenFirewall)
      {
        CreateFirewallRule(Settings.Port);
      }

      if (!ServerVersion.ServerSupportsXProtocol())
      {
        return;
      }

      CancellationToken.ThrowIfCancellationRequested();
      if ((ConfigurationType == ConfigurationType.Reconfigure
           || isDataDirectoryConfigured)
          && OldSettings != null
          && OldSettings.OpenFirewallForXProtocol)
      {
        RemoveFirewallRule(OldSettings.MySqlXPort);
      }

      CancellationToken.ThrowIfCancellationRequested();
      if (Settings.OpenFirewallForXProtocol
          && Settings.MySqlXPort != 0)
      {
        CreateFirewallRule(Settings.MySqlXPort);
      }
    }

    private void UpgradeServer()
    {
      if (CurrentState == ConfigState.ConfigurationUnnecessary)
      {
        return;
      }

      CancellationToken.ThrowIfCancellationRequested();
      var success = false;
      if (!string.IsNullOrEmpty(Settings.RootPassword))
      {
        success = ProcessMySqlUpgradeTool();
      }

      Settings.PendingSystemTablesUpgrade = !success;
      Settings.SaveGeneralSettings();
      CurrentStep.Status = success
        ? ConfigurationStepStatus.Finished
        : ConfigurationStepStatus.Error;
    }

    /// <summary>
    /// Writes the configuration files to disk.
    /// </summary>
    private void WriteConfigurationFile()
    {
      CancellationToken.ThrowIfCancellationRequested();
      if (ConfigurationType == ConfigurationType.Upgrade
          && Settings.PendingSystemTablesUpgrade
          && (!_startAndUpgradeServerConfigStep.Execute
              || !_upgradeStandAloneServerStep.Execute))
      {
        // Turn off the pending system tables upgrade flag during an upgrade if no upgrade step is being executed
        Settings.PendingSystemTablesUpgrade = false;
      }

      // Create the my.ini configuration file.
      if (IsWriteIniConfigurationFileNeeded)
      {
        var fileUpdated = UpdateConfigFile();
        CurrentStep.Status = fileUpdated
          ? ConfigurationStepStatus.Finished
          : ConfigurationStepStatus.Error;

        if (!fileUpdated)
        {
          RevertedSteps = _revertController.Rollback(OldSettings);
        }

        return;
      }
      else
      {
        ReportStatus(string.Format(Resources.SavingConfigurationFile, GeneralSettingsManager.CONFIGURATOR_SETTINGS_FILE_NAME));
        Settings.SaveGeneralSettings();
        ReportStatus(string.Format(Resources.SavedConfigurationFile, GeneralSettingsManager.CONFIGURATOR_SETTINGS_FILE_NAME));
        CurrentStep.Status = ConfigurationStepStatus.Finished;
      }
    }

    /// <summary>
    /// Deletes the server data directory.
    /// </summary>
    /// <returns><c>true</c> if the data directory was deleted successfully; otherwise, <c>false</c>.</returns>
    protected bool DeleteDataDirectory()
    {
      if (!HasExistingDataDirectory())
      {
        return true;
      }

      return Utilities.DeleteDirectory(DataDirectory);
    }

    protected void ReportError(string errorMessage)
    {
      if (string.IsNullOrEmpty(errorMessage))
      {
        return;
      }

      CurrentState = ConfigState.ConfigurationError;
      ReportStatus(ConfigurationEventType.Error, errorMessage);
      Logger.LogError(errorMessage);
    }

    /// <summary>
    /// Reports a message to the specified output location.
    /// </summary>
    /// <param name="message">The message to report.</param>
    protected void ReportStatus(string message)
    {
      if (string.IsNullOrEmpty(message))
      {
        return;
      }

      if (UseStatusesList)
      {
        StatusesList?.Add(message);
      }

      ReportStatus(ConfigurationEventType.Info, message);
      Logger.LogInformation(message);
    }

    protected void ResetCancellationToken()
    {
      if (CancellationTokenSource != null
          && CancellationTokenSource.IsCancellationRequested)
      {
        // Ensure the configuration starts with a fresh cancellation token.
        CancellationTokenSource.Dispose();
        CancellationTokenSource = null;
      }

      if (CancellationTokenSource == null)
      {
        CancellationTokenSource = new CancellationTokenSource();
      }
    }
  }
}

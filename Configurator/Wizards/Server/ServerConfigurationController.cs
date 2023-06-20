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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Controllers;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Core.IniFile.Template;
using MySql.Configurator.Core.Wizard;
using MySql.Configurator.Wizards.Examples;
using IniFile = MySql.Configurator.Core.IniFile.IniFile;
using System.Reflection;
using System.IO.Compression;
using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Core.Classes.MySql;
using MySql.Configurator.Properties;
using Shell32;

namespace MySql.Configurator.Wizards.Server
{
  [ProductConfiguration("mysql-server", 1)]
  public class ServerConfigurationController : ServerProductConfigurationController
  {
    #region Constants

    /// <summary>
    /// The value used to indicate that all users should have full access to a named pipe.
    /// </summary>
    public const string NAMED_PIPE_FULL_ACCESS_TO_ALL_USERS = "*everyone*";

    #endregion

    #region General Fields

    /// <summary>
    /// Defines the path to the data directory.
    /// </summary>
    private string _dataDirectory;

    #endregion

    #region Configuration Step Fields

    private ConfigurationStep _backupDatabaseStep;
    private List<ConfigurationStep> _classicConfigurationSteps;
    private ConfigurationStep _createRemoveExampleDatabasesStep;
    private ConfigurationStep _initializeServerConfigurationStep;
    //private ConfigurationStep _prepareAuthenticationPluginChangeStep;
    private List<ConfigurationStep> _selfContainedUpgradeSteps;
    private ConfigurationStep _setLocalInstanceAsWritableStep;
    private ConfigurationStep _startAndUpgradeServerConfigStep;
    private ConfigurationStep _startServerConfigurationStep;
    private ConfigurationStep _stopServerConfigurationStep;
    private ConfigurationStep _updateAccessPermissions;
    private ConfigurationStep _updateEnterpriseFirewallPluginConfigStep;
    private ConfigurationStep _updateProcessStep;
    private ConfigurationStep _updateSecurityStep;
    private ConfigurationStep _updateStartMenuLinksStep;
    private ConfigurationStep _updateUsersStep;
    private ConfigurationStep _updateWindowsFirewallRulesStep;
    private ConfigurationStep _updateWindowsServiceStep;
    private ConfigurationStep _upgradeStandAloneServerStep;
    private LocalServerInstance _upgradingInstance;
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

    public ServerConfigurationController()
    {
      _upgradingInstance = null;
      _firewallRulesList = new List<string>();
      FullControlDictionary = new Dictionary<SecurityIdentifier, string>();
      IsBackupDatabaseStepNeeded = false;
      UpdateDataDirectoryPermissions = true;
      RemoveDataDirectory = true;
      CurrentState = ConfigState.ConfigurationRequired;
      Logger.LogInformation("Product configuration controller created.");
      ShowAdvancedOptions = false;
      if (LoadRoles())
      {
        return;
      }

      RolesDefined = null;
      TemporaryServerUser = null;
    }

    #region Properties

    public override bool CanAddUsers => Package != null
                                        && !(Settings.ConfigurationFileExists == true);

    /// <summary>
    /// Gets a <see cref="CancellationToken"/> to signal a user's cancellation.
    /// </summary>
    public CancellationToken CancellationToken => CancellationTokenSource.Token;

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
    /// Gets or sets a value indicating whether the database is backed up during an upgrade.
    /// </summary>
    public bool IsBackupDatabaseStepNeeded { get; set; }

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
    /// Gets a value indicating whether the removal step that deletes the data directory needs to run.
    /// </summary>
    public bool IsDeleteDataDirectoryStepNeeded => IsThereServerDataFiles && RemoveDataDirectory;

    /// <summary>
    /// Gets a value indicating whether the removal step that deletes the Windows Service needs to run.
    /// </summary>
    public bool IsDeleteServiceStepNeeded => MySqlServiceControlManager.ServiceExists(Settings?.ServiceName);

    /// <summary>
    /// Gets a value indicating whether the removal step that deletes the firewall rules needs to run.
    /// </summary>
    public bool IsRemoveFirewallRuleStepNeeded => FirewallRulesList.Any(rule => rule.Equals($"Port {Settings.MySqlXPort}", StringComparison.InvariantCultureIgnoreCase)
                                                                        || rule.Equals($"Port {Settings.Port}", StringComparison.InvariantCultureIgnoreCase));

    /// <summary>
    /// Gets a value indicating whether the step for setting the local instance as writable should be executed.
    /// </summary>
    public bool IsSetLocalInstanceAsWritableStepNeeded => Settings.InnoDbClusterType == ServerConfigurationType.AddToCluster;

    /// <summary>
    /// </summary>
    public bool IsStartAndUpgradeConfigurationStepNeeded => ConfigurationType == ConfigurationType.Upgrade
                                                            && ServerVersion.ServerSupportsSelfContainedUpgrade()
                                                            && IsThereServerDataFiles;

    /// <summary>
    /// Gets a value indicating whether the configuration step that starts the server needs to run.
    /// </summary>
    public bool IsStartServerConfigurationStepNeeded => ((IsThereServerDataFiles
                                                          && !ServerVersion.ServerSupportsSelfContainedUpgrade())
                                                         || IsWriteIniConfigurationFileNeeded)
                                                        || (!IsThereServerDataFiles
                                                            && IsInitializeServerConfigurationStepNeeded(false)
                                                            && ConfigurationType == ConfigurationType.Upgrade);

    /// <summary>
    /// Gets a value indicating whether the configuration step that stops the server needs to run.
    /// </summary>
    public bool IsStopServerConfigurationStepNeeded
    {
      get
      {
        var serverInstanceInfo = new LocalServerInstance(this, ReportStatus);
        return (ConfigurationType == ConfigurationType.Upgrade
                && serverInstanceInfo.IsRunning)
               || (IsStartServerConfigurationStepNeeded
                   && (IsStartAndUpgradeConfigurationStepNeeded || serverInstanceInfo.IsRunning));
      }
    }

    /// <summary>
    /// Gets a value indicating whether the extended XML configuration file exists.
    /// </summary>
    public bool IsThereConfigXmlFile => File.Exists(Path.Combine(Settings.IniDirectory, BaseServerSettings.EXTENDED_CONFIG_FILE_NAME));

    /// <summary>
    /// Gets a value indicating whether the configuration step that updates the access permissions to the data folder needs to run.
    /// </summary>
    public bool IsUpdateServerFilesPermissionsStepNeeded => UpdateDataDirectoryPermissions
                                                            && (ConfigurationType == ConfigurationType.New
                                                                || (ConfigurationType == ConfigurationType.Reconfiguration
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
    public bool IsUpdateServerIdSupported => ConfigurationType == ConfigurationType.New;

    /// <summary>
    /// Gets a value indicating whether the configuration step that updates the Start menu links needs to run.
    /// </summary>
    public bool IsUpdateStartMenuLinksConfigurationStepNeeded => (ConfigurationType != ConfigurationType.Reconfiguration
                                                                 || IsThereServerDataFiles)
                                                                 && Core.Classes.Utilities.ExecutionIsFromMSI(Package.NormalizedVersion);

    /// <summary>
    /// Gets a value indicating whether the configuration step that updates user accounts needs to run.
    /// </summary>
    public bool IsUpdateUsersConfigurationStepNeeded => Settings.NewServerUsers.Count > 0;

    /// <summary>
    /// Gets a value indicating whether the configuration step that updates the Windows service needs to run.
    /// </summary>
    public bool IsUpdateWindowsServiceConfigurationStepNeeded => (OldSettings != null
                                                                  && OldSettings.ServiceExists()
                                                                  && ConfigurationType != ConfigurationType.New
                                                                  && (!Settings.ConfigureAsService
                                                                      || OldSettings.ServiceName != Settings.ServiceName))
                                                                 || Settings.ConfigureAsService;

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
                                                              || Settings.ExtendedPropertiesChanged);

    /// <summary>
    /// Gets a value indicating whether the configuration step that writes the my.ini configuration file needs to run.
    /// </summary>
    public bool IsWriteIniConfigurationFileNeeded => (ConfigurationType != ConfigurationType.Upgrade
                                                      || DefaultAuthenticationPluginChanged
                                                      || (ConfigurationType == ConfigurationType.Upgrade
                                                          && (!IsThereServerDataFiles)));

    public MySqlServerSettings OldSettings => Settings.OldSettings as MySqlServerSettings;

    /// <summary>
    /// Gets or sets a value indicating if the data directory should be removed when uninstalling the product.
    /// </summary>
    public bool RemoveDataDirectory { get; set; }

    public RoleDefinitions RolesDefined { get; private set; }

    public Version ServerVersion { get; private set; }

    public MySqlServiceControlManager ServiceManager { get; set; }
    public new MySqlServerSettings Settings => settings as MySqlServerSettings;
    public bool ShowAdvancedOptions { get; set; }
    /// <summary>
    /// Gets a value indicating whether the Server supports Enterprise Firewall configuration.
    /// </summary>
    public bool SupportsEnterpriseFirewallConfiguration => Package.License == LicenseType.Commercial
                                                           && ServerVersion.ServerSupportsEnterpriseFirewall();

    public IniTemplate Template { get; set; }

    /// <summary>
    /// Gets a temporary user account to be used instead of the root user account.
    /// </summary>
    public ServerUser TemporaryServerUser { get; private set; }

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

    #endregion Properties

    public override bool AddUser(ValueList values, out string msg)
    {
      var serverUser = new ServerUser();
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

    public override void AfterConfigurationEnded()
    {
      // If any configuration step that persists the extended settings did not execute during an upgrade, persist them now.
      if (ConfigurationType == ConfigurationType.Upgrade
          && !_upgradeStandAloneServerStep.Execute
          && !_startAndUpgradeServerConfigStep.Execute
          && !_writeConfigurationFileStep.Execute)
      {
        Settings.PendingSystemTablesUpgrade = false;
        Settings.SaveExtendedSettings();
      }
    }

    /// <summary>
    /// Flags the configuration operation as cancelled
    /// </summary>
    public override void CancelConfigure()
    {
      base.CancelConfigure();
      MySqlServiceControlManager.Cancel();
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

    public override string GetConnectionString(string username, string password, bool useOldSettings, string schemaName)
    {
      return GetConnectionStringBuilder(new ServerUser(username, password, Settings.DefaultAuthenticationPlugin), useOldSettings, schemaName).ConnectionString;
    }

    public string GetConnectionString(MySqlServerUser serverUser, bool useOldSettings = false, string schemaName = null)
    {
      return GetConnectionStringBuilder(serverUser, useOldSettings, schemaName).ConnectionString;
    }

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

    /// <summary>
    /// Gets a <see cref="LocalServerInstance"/> objects containing information about the local Server instance.
    /// </summary>
    /// <returns>A <see cref="LocalServerInstance"/> object containing information about the installed Server instance.</returns>
    public LocalServerInstance GetInstalledLocalServerInstance()
    {
      var standAloneExists = Settings.ServerConfigurationType == ServerConfigurationType.StandAlone
                             && IsThereServerDataFiles;
      if (standAloneExists)
      {
        return new LocalServerInstance(this, ReportStatus);
      }

      return null;
    }

    public Folder GetShell32NameSpaceFolder(object folder)
    {
      var shellAppType = Type.GetTypeFromProgID("Shell.Application");
      var shell = Activator.CreateInstance(shellAppType);
      return (Folder)shellAppType.InvokeMember("NameSpace",
      System.Reflection.BindingFlags.InvokeMethod, null, shell, new[] { folder });
    }

    public override void Init()
    {
      CurrentState = ConfigState.ConfigurationRequired;
      ServerVersion = new Version(Package.Version);
      settings = new MySqlServerSettings(Package);
      LoadConfigurationSteps();
      base.Init();
    }

    public override void Initialize(bool afterInstallation)
    {
      Logger.LogInformation("Product configuration controller initialization started.");

      string baseDirectory = string.Empty;
      if (Package != null)
      {
        if (Package.IsInstalled)
        {
          Logger.LogInformation($"Product configuration controller found {Package.DisplayName} installed.");

          // Unreliable since both 32 bit and 64 bit servers use the same keys (for the same version).
          // Though, usually one can only install one of both server architectures.
          baseDirectory = InstallDirectory; // Owner.GetInstalledProductRegistryKey("Location");
          _dataDirectory = DataDirectory; // Owner.GetInstalledProductRegistryKey("DataLocation");

          string versionString = Package.Version;
          if (!string.IsNullOrEmpty(versionString))
          {
            ServerVersion = new Version(versionString);
          }

          // Look for existing service.
          if (!string.IsNullOrEmpty(baseDirectory))
          {
            Settings.ServiceName = ServiceManager.FindServiceName(baseDirectory);
          }

          // If no existing service, use default.
          if (string.IsNullOrEmpty(Settings.ServiceName) && ServerVersion != null)
          {
            Settings.ServiceName = $"MySQL{ServerVersion.Major}{ServerVersion.Minor}";
          }
        }
        else
        {
          ServerVersion = new Version(Package.Version);

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

    public override void PostInstall(PackageStatus status)
    {
      base.PostInstall(status);

      // we need to "re-find" the ini directory because the data directory may have changed
      Settings.IniDirectory = null;
      Settings.ConfigFile = null;
      Settings.LoadIniDefaults();
    }

    public override void PostRemove(PackageStatus status)
    {
      if (status == PackageStatus.Failed)
      {
        return;
      }

      base.PostRemove(status);
    }

    public override void PrepareForConfigure()
    {
      base.PrepareForConfigure();

      if (ConfigurationType == ConfigurationType.Reconfiguration)
      {
        return;
      }

      Settings.PipeName = GetUniquePipeOrMemoryName(true);
      Settings.SharedMemoryName = GetUniquePipeOrMemoryName(false);
    }

    public override bool PreUpgrade()
    {
      StopServerSafe(true);
      return base.PreUpgrade();
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
    /// Sets up the wizard pages.
    /// </summary>
    public override void SetPages()
    {
      Pages.Clear();
      if (ConfigurationType == ConfigurationType.Upgrade
          && IsThereServerDataFiles)
      {
        Logger.LogInformation(Resources.SettingUpUpgrade);
        if (ServerVersion.ServerSupportsCachingSha2Authentication()
            && Settings != null
            && Settings.DefaultAuthenticationPlugin != MySqlAuthenticationPluginType.CachingSha2Password)
        {
          //Pages.Add(new ServerConfigDefaultAuthenticationPage(this));
          var configUsersPage = new ServerConfigUserAccountsPage(this) { PageVisible = false };
          Pages.Add(configUsersPage);
        }

        Pages.Add(new ServerConfigUpgradePage(this));
        var securityPage = new ServerConfigSecurityPage(this) { PageVisible = !ValidateServerFilesHaveRecommendedPermissions() };
        Pages.Add(securityPage);
        return;
      }

      if (ConfigurationType == ConfigurationType.Remove)
      {
        if (RequiresUninstallConfiguration())
        {
          var serverRemovePage = new ServerRemovePage(this, Package.DisplayVersion);
          Pages.Add(serverRemovePage);
        }

        return;
      }

      Logger.LogInformation(ConfigurationType == ConfigurationType.Reconfiguration ? Resources.SettingUpReconfiguration : Resources.SettingUpNewInstallation);
      Pages.Add(new ServerConfigLocalMachinePage(this));
      Pages.Add(new ServerConfigNamedPipesPage(this) { PageVisible = Settings != null
                                                       && Settings.EnableNamedPipe});
      
      //if (ServerVersion.ServerSupportsCachingSha2Authentication())
      //{
      //  Pages.Add(new ServerConfigDefaultAuthenticationPage(this));
      //}

      Pages.Add(new ServerConfigUserAccountsPage(this));
      Pages.Add(new ServerConfigServicePage(this));
      Pages.Add(new ServerConfigSecurityPage(this) { PageVisible = !ValidateServerFilesHaveRecommendedPermissions() });
      ConfigWizardPage loggingPage = new ServerConfigLoggingOptionsPage(this);
      loggingPage.PageVisible = ConfigurationType != ConfigurationType.Reconfiguration;
      Pages.Add(loggingPage);
      ConfigWizardPage advancedPage = new ServerConfigAdvancedOptionsPage(this);
      advancedPage.PageVisible = ConfigurationType == ConfigurationType.New;
      Pages.Add(advancedPage);
      Pages.Add(new ServerExampleDatabasesPage(this));
    }

    /// <summary>
    /// Updates the <see cref="ServerProductConfigurationController.CurrentState"/> as required or unnecessary.
    /// </summary>
    /// <param name="required">Flag indicating whether the configuration is required or not.</param>
    public void UpdateConfigurationState(bool required)
    {
      CurrentState = required
        ? ConfigState.ConfigurationRequired
        : ConfigState.ConfigurationUnnecessary;
    }

    /// <summary>
    /// Updates the configuration steps each time a configuration is run.
    /// </summary>
    public override void UpdateConfigurationSteps()
    {
      RolesDefined.ServerVersion = ServerVersion;
      foreach (var step in ConfigurationSteps)
      {
        if (step == _writeConfigurationFileStep) _writeConfigurationFileStep.Execute = IsWriteIniConfigurationFileNeeded || IsWriteExtendedConfigurationFileNeeded;
        else if (step == _setLocalInstanceAsWritableStep) _setLocalInstanceAsWritableStep.Execute = IsSetLocalInstanceAsWritableStepNeeded;
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

      // Make sure the base always runs after all steps are updated to execute or not.
      base.UpdateConfigurationSteps();
    }

    /// <summary>
    /// Updates the configuration steps during a Server upgrade to reflect user selections.
    /// </summary>
    public void UpdateUpgradeConfigSteps()
    {
      _backupDatabaseStep.Execute = IsBackupDatabaseStepNeeded;
      _upgradeStandAloneServerStep.Execute = Settings.ServerConfigurationType == ServerConfigurationType.StandAlone
                                             && IsThereServerDataFiles
                                             && CurrentState == ConfigState.ConfigurationRequired;
      _updateAccessPermissions.Execute = IsUpdateServerFilesPermissionsStepNeeded;
      _startAndUpgradeServerConfigStep.Execute = IsStartAndUpgradeConfigurationStepNeeded;
      _startAndUpgradeServerConfigStep.ChangeDescription(Settings.SystemTablesUpgraded == SystemTablesUpgradedType.Yes
        ? Resources.ServerStartAndUpgradeProcessStep
        : Resources.ServerStartMinimalUpgradeStep);
      ConfigurationSteps = StandAloneServerSteps;
    }

    /// <summary>
    /// Updates the remove steps each during an uninstall operation.
    /// </summary>
    public override void UpdateRemoveSteps()
    {
      if (Settings == null)
      {
        return;
      }

      LoadRemovalConfigurationSteps();
      _deleteServiceStep.Execute = IsDeleteServiceStepNeeded;
      _deleteDataDirectoryStep.Execute = IsDeleteDataDirectoryStepNeeded;
      _removeFirewallRuleStep.Execute = IsRemoveFirewallRuleStepNeeded;
      
      // Make sure the base always runs after all steps are updated to execute or not.
      base.UpdateRemoveSteps();
    }

    private bool AddUserToSpecialUsersRegistryKey()
    {
      var greatSuccess = false;
      var specialAccountUserList = Registry.LocalMachine;

      try
      {
        specialAccountUserList = specialAccountUserList.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon\SpecialAccounts\UserList", true);
        if (specialAccountUserList != null)
        {
          specialAccountUserList.SetValue(Settings.ServiceAccountUsername, 0);
          specialAccountUserList.Close();
          greatSuccess = true;
        }
      }
      catch (Exception e)
      {
        Logger.LogException(e);
      }

      return greatSuccess;
    }

    /// <summary>
    /// Backs up the MySQL database using mysqldump.
    /// </summary>
    private void BackupDatabase()
    {
      CancellationToken.ThrowIfCancellationRequested();
      _upgradingInstance = new LocalServerInstance(this, ReportStatus);
      var success = _upgradingInstance.IsRunning;
      if (!success)
      {
        // Step 1: Start Server (if not already running) in order to be able to run mysqldump tool
        ReportStatus(Resources.ServerConfigMySqlUpgradeStartServer);
        success = _upgradingInstance.StartInstanceAsProcess();
      }

      string errorMessage = null;
      CancellationToken.ThrowIfCancellationRequested();
      if (success)
      {
        // Step 2: If Server started successfully, run mysqldump tool
        var backupFile = Settings.FullBackupFilePath;
        if (!string.IsNullOrEmpty(backupFile))
        {
          ReportStatus(string.Format(Resources.ServerConfigBackupDatabaseDumpRunning, backupFile));
          var binDirectory = Path.Combine(InstallDirectory, BINARY_DIRECTORY_NAME);
          var user = GetUserAccountToConnectBeforeUpdatingRootUser();
          var connectionOptions = string.Empty;
          bool sendingPasswordInCommandLine = false;
          string tempConfigFileWithPassword = null;
          if (!string.IsNullOrEmpty(user.Password))
          {
            tempConfigFileWithPassword = Core.Classes.Utilities.CreateTempConfigurationFile(IniFile.GetClientPasswordLines(user.Password));
            sendingPasswordInCommandLine = tempConfigFileWithPassword == null;
            connectionOptions = sendingPasswordInCommandLine
              ? $"--password={user.Password} "
              : $"--defaults-extra-file=\"{tempConfigFileWithPassword}\" ";
          }

          connectionOptions += GetCommandLineConnectionOptions(user, false, true);
          var arguments =
            $" {connectionOptions} --default-character-set=utf8 --routines --events --single-transaction=TRUE --all-databases --result-file=\"{backupFile}\"";
          var dumpToolProcessResult = Core.Classes.Utilities.RunProcess(
            Path.Combine(binDirectory, DUMP_TOOL_EXECUTABLE_FILENAME),
            arguments,
            binDirectory,
            ReportStatus,
            ReportStatus,
            true,
            sendingPasswordInCommandLine);
          Core.Classes.Utilities.DeleteFile(tempConfigFileWithPassword, 10, 500);
          success = dumpToolProcessResult.ExitCode == 0;
        }
        else
        {
          errorMessage = Resources.ServerConfigBackupDatabaseBackupDirectoryError;
        }
      }
      else
      {
        errorMessage = Resources.ServerConfigBackupDatabaseServerStartError;
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
      var success = Core.Classes.Utilities.RunNetShellProcess(arguments, out var netShellProcessOutput, out var netShellProcessError);
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

      var serverInstance = new LocalServerInstance(this, ReportErrLogLine)
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
    /// <param name="adminUser">The <see cref="ServerUser"/> used to establish the connection.</param>
    /// <param name="affectedUsers">A list of <see cref="ServerUser"/> accounts to create or alter.</param>
    /// <param name="grantRolePermissions">Flag indicating whether permissions need to be updated for the given accounts.</param>
    /// <param name="useOldSettings">Flag indicating whether the confuguration previous to current changes will be used instead of current.</param>
    /// <returns><c>true</c> if the user accounts were affected successfully, <c>false</c> otherwise.</returns>
    private bool CreateOrAlterUserAccounts(ServerUser adminUser, List<ServerUser> affectedUsers, bool grantRolePermissions, bool useOldSettings = false)
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

            if (grantSqlStatements.Count > 0)
            {
              cmd.FlushPrivileges();
            }
          }
        }
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
        success = false;
      }

      return success;
    }

    /// <summary>
    /// Creates a temporary MySQL user that will be able to connect with the new default authentication plugin.
    /// </summary>
    /// <remarks>
    /// Since after the configuration file is written to disk and Server is started, root will not be able to
    ///  connect. We do not want to change the root user before stopping the Server because steps might fail
    ///  and we would end up with an unusable root user, thus not being able to connect with it.
    /// </remarks>
    /// <param name="authenticationPlugin">The <see cref="MySqlAuthenticationPluginType"/> assigned to the temporary user account.</param>
    /// <returns><c>true</c> if the temporary user account was created successfully, <c>false</c> otherwise.</returns>
    private bool CreateTemporaryUserAccount(MySqlAuthenticationPluginType authenticationPlugin)
    {
      ReportStatus(Resources.ServerConfigCreatingTemporaryUser);
      var rootUser = ServerUser.GetLocalRootUser(Settings.ExistingRootPassword, Settings.DefaultAuthenticationPlugin);
      TemporaryServerUser = ServerUser.GetLocalTemporaryUser(authenticationPlugin, RolesDefined.GetRoleByType(UserRoleType.Root));
      var success = CreateOrAlterUserAccounts(rootUser, new List<ServerUser>(1) { TemporaryServerUser }, true, true);
      if (!success)
      {
        DeleteServerUserAccount(rootUser, TemporaryServerUser, true);
        TemporaryServerUser = null;
      }

      ReportStatus(success
        ? string.Format(Resources.ServerConfigCreatingTemporaryUserSuccess, TemporaryServerUser.Username)
        : Resources.ServerConfigCreatingTemporaryUserError);
      return success;
    }

    /// <summary>
    /// Step to trigger the deletion of the server configuration file.
    /// </summary>
    private void DeleteConfigurationFileStep()
    {
      CancellationToken.ThrowIfCancellationRequested();
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
    /// Deletes the user account associated with the given <see cref="ServerUser"/>.
    /// </summary>
    /// <param name="adminUser">The <see cref="ServerUser"/> used to establish the connection.</param>
    /// <param name="affectedUser">The <see cref="ServerUser"/> accounts to delete.</param>
    /// <param name="useOldSettings">Flag indicating whether the configuration previous to current changes will be used instead of current.</param>
    /// <param name="flushPrivileges">Flag indicating whether FLUSH PRIVILEGES is executed after deleting the user.</param>
    /// <returns><c>true</c> if the user account was deleted successfully or if it did not exist, <c>false</c> otherwise.</returns>
    private bool DeleteServerUserAccount(ServerUser adminUser, ServerUser affectedUser, bool useOldSettings = false, bool flushPrivileges = true)
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
          if (flushPrivileges)
          {
            cmd.FlushPrivileges();
          }
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
      var adminUser = ServerUser.GetLocalRootUser(Settings.RootPassword, Settings.DefaultAuthenticationPlugin);
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
    /// Gets a <see cref="ServerUser"/> that can connect to the server before the root user account is updated.
    /// </summary>
    /// <returns>A <see cref="ServerUser"/> that can connect to the server before the root user account is updated.</returns>
    private ServerUser GetUserAccountToConnectBeforeUpdatingRootUser()
    {
      return DefaultAuthenticationPluginChanged && TemporaryServerUser != null
        ? TemporaryServerUser
        : ServerUser.GetLocalRootUser(Settings.ExistingRootPassword, Settings.DefaultAuthenticationPlugin);
    }

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
      var localServerInstance = new LocalServerInstance(this, ReportErrLogLine)
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
      _createRemoveExampleDatabasesStep = new ConfigurationStep("Updating example databases", 10, CreateRemoveExampleDatabases, false, ConfigurationType.New | ConfigurationType.Reconfiguration);
      _initializeServerConfigurationStep = new ConfigurationStep(Resources.ServerInitializeDatabaseStep, 900, InitializeServer, true, ConfigurationType.New | ConfigurationType.Reconfiguration | ConfigurationType.Upgrade);
      //_prepareAuthenticationPluginChangeStep = new ConfigurationStep(Resources.ServerPrepareAuthenticationPluginChangeStep, 20, PrepareAuthenticationPluginChange, true, ConfigurationType.Reconfiguration | ConfigurationType.Upgrade);
      _setLocalInstanceAsWritableStep = new ConfigurationStep(Resources.SetLocalInstanceAsWritableStep, 10, SetLocalInstanceAsWritableStep);
      _startServerConfigurationStep = new ConfigurationStep(Resources.ServerStartProcessStep, 90, StartServerStep);
      _stopServerConfigurationStep = new ConfigurationStep(Resources.ServerStopProcessStep, 40, StopServerSafe);
      _updateEnterpriseFirewallPluginConfigStep = new ConfigurationStep(Resources.ServerEnableEnterpriseFirewallStep, 45, InstallEnterpriseFirewallPlugin, true, ConfigurationType.New | ConfigurationType.Reconfiguration | ConfigurationType.Upgrade);
      _updateStartMenuLinksStep = new ConfigurationStep(Resources.ServerUpdateStartMenuLinkStep, 20, UpdateStartMenuLink, false, ConfigurationType.New | ConfigurationType.Reconfiguration | ConfigurationType.Upgrade);
      _updateSecurityStep = new ConfigurationStep(Resources.ServerApplySecurityStep, 20, UpdateSecurity, true, ConfigurationType.New | ConfigurationType.Reconfiguration | ConfigurationType.Upgrade);
      _updateAccessPermissions = new ConfigurationStep(Resources.ServerUpdateServerFilePermissions, 10, UpdateServerFilesPermissions, false, ConfigurationType.New | ConfigurationType.Reconfiguration | ConfigurationType.Upgrade);
      _updateUsersStep = new ConfigurationStep(Resources.ServerCreateUsersStep, 20, UpdateUsers, true, ConfigurationType.New | ConfigurationType.Reconfiguration | ConfigurationType.Upgrade);
      _updateWindowsFirewallRulesStep = new ConfigurationStep(Resources.ServerUpdateWindowsFirewallStep, 40, UpdateWindowsFirewall, false, ConfigurationType.New | ConfigurationType.Reconfiguration);
      _updateWindowsServiceStep = new ConfigurationStep(Resources.ServerAdjustServiceStep, 25, UpdateServiceSettings, true, ConfigurationType.New | ConfigurationType.Reconfiguration | ConfigurationType.Upgrade);
      _updateProcessStep = new ConfigurationStep(Resources.ServerAdjustProcessStep, 10, UpdateProcessSettings, true, ConfigurationType.New | ConfigurationType.Reconfiguration | ConfigurationType.Upgrade);
      _upgradeStandAloneServerStep = new ConfigurationStep(Resources.ServerUpgradeStep, 3600, UpgradeServer, true, ConfigurationType.Upgrade);
      _writeConfigurationFileStep = new ConfigurationStep(Resources.ServerWriteConfigFileStep, 20, WriteConfigurationFile);
      _startAndUpgradeServerConfigStep = new ConfigurationStep(Resources.ServerStartAndUpgradeProcessStep, 3600, StartAndUpgradeServerStep, true, ConfigurationType.Upgrade);

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
        _backupDatabaseStep,
        _upgradeStandAloneServerStep,
        _initializeServerConfigurationStep,
        _updateAccessPermissions,
        _startServerConfigurationStep,
        _setLocalInstanceAsWritableStep,
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
        _stopServerConfigurationStep,
        _updateAccessPermissions,
        _updateWindowsServiceStep,
        _startAndUpgradeServerConfigStep,
        _setLocalInstanceAsWritableStep,
        //_prepareAuthenticationPluginChangeStep,
        _stopServerConfigurationStep,
        _writeConfigurationFileStep,
        _startServerConfigurationStep,
        _updateSecurityStep,
        _updateStartMenuLinksStep
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

    private bool LoadRoles()
    {
      bool loadedRoles = false;
      var xmlSerializer = new XmlSerializer(typeof(RoleDefinitions));
      var assembly = Assembly.GetExecutingAssembly();
      //var resources = assembly.GetManifestResourceNames();
      using (Stream stream = assembly.GetManifestResourceStream($"MySql.Configurator.Resources.{AppConfiguration.RoleDefinitions}"))
      {
        try
        {
          RolesDefined = (RoleDefinitions)xmlSerializer.Deserialize(stream);
          loadedRoles = true;
        }
        catch (Exception e)
        {
          Logger.LogError($"Error during XML parsing of file {AppConfiguration.RoleDefinitions}. The error was:\n\t{e}");
        }
        finally
        {
          stream.Close();
        }
      }

      return loadedRoles;
    }

    private IniTemplate LoadTemplate()
    {
      string templateBase = "my-template{0}.ini";
      var version = Package.NormalizedVersion;
      string templateFile = null;
      if (version.Major >= 8
          && version.Minor > 0)
      {
        templateFile = string.Format(templateBase, $"-{version.Major}.x");
      }
      else
      {
        templateFile = string.Format(templateBase, $"-{version.Major}.{version.Minor}");
      }

      return new IniTemplate(InstallDirectory, DataDirectory, templateFile, Settings.IniDirectory, BaseServerSettings.CONFIG_FILE_NAME, version, Settings.ServerInstallType);
    }

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
    /// Performs initial actions for a default authentication plugin change.
    /// </summary>
    private void PrepareAuthenticationPluginChange()
    {
      CancellationToken.ThrowIfCancellationRequested();
      var success = OldSettings != null
                    && CreateTemporaryUserAccount(OldSettings.DefaultAuthenticationPlugin);
      CurrentStep.Status = success
        ? ConfigurationStepStatus.Finished
        : ConfigurationStepStatus.Error;
    }

    /// <summary>
    /// Runs mysql_upgrade.exe after a server upgrade.
    /// </summary>
    /// <returns><c>true</c> on success, <c>false</c> otherwise.</returns>
    private bool ProcessMySqlUpgradeTool()
    {
      if (_upgradingInstance == null)
      {
        _upgradingInstance = new LocalServerInstance(this, ReportStatus);
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
          tempConfigFileWithPassword = Core.Classes.Utilities.CreateTempConfigurationFile(IniFile.GetClientPasswordLines(user.Password));
          sendingPasswordInCommandLine = tempConfigFileWithPassword == null;
          connectionOptions = sendingPasswordInCommandLine
            ? $"--password={user.Password} "
            : $"--defaults-extra-file=\"{tempConfigFileWithPassword}\" ";
        }

        CancellationToken.ThrowIfCancellationRequested();
        connectionOptions += GetCommandLineConnectionOptions(user, false, true);
        var arguments = $" {connectionOptions} --force --verbose";
        var upgradeToolProcessResult = Core.Classes.Utilities.RunProcess(
          Path.Combine(binDirectory, UPGRADE_TOOL_EXECUTABLE_FILENAME),
          arguments,
          binDirectory,
          ReportStatus,
          ReportStatus,
          true,
          sendingPasswordInCommandLine);
        Core.Classes.Utilities.DeleteFile(tempConfigFileWithPassword, 10, 500);
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
      if (Settings.OpenFirewallForXProtocol)
      {
        removedXProtocolFirewallRule = RemoveFirewallRule(Settings.MySqlXPort);
      }

      CurrentStep.Status = RemoveFirewallRule(Settings.Port)
        && removedXProtocolFirewallRule
        ? ConfigurationStepStatus.Finished
        : ConfigurationStepStatus.Error;
    }

    private void ReportErrLogLine(string line)
    {
      var errorLogLine = ServerErrorLogLine.Parse(line);
      ReportStatus(!errorLogLine.Parsed ? errorLogLine.UnparsedLine : errorLogLine.Message);
    }

    /// <summary>
    /// Identifies if user input is required prior to executing an uninstall operation.
    /// </summary>
    /// <returns><c>true</c> if user input is required to configure prior to uninstalling the product; otherwise, <c>false</c>.</returns>
    public override bool RequiresUninstallConfiguration()
    {
      return IsThereServerDataFiles;
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

    private void SetDefaultDataDir()
    {
      _dataDirectory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
        $@"MySQL\MySQL Server {ServerVersion.Major}.{ServerVersion.Minor}\");
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
    /// Sets the local instance as writable.
    /// </summary>
    private void SetLocalInstanceAsWritableStep()
    {
      CancellationToken.ThrowIfCancellationRequested();
      var rootUser = MySqlServerUser.GetLocalRootUser(Settings.ExistingRootPassword, Settings.DefaultAuthenticationPlugin);
      var serverInstance = new MySqlServerInstance(Settings.Port, rootUser, ReportStatus);
      CurrentStep.Status = SetLocalInstanceAsWritable(serverInstance, true)
        ? ConfigurationStepStatus.Finished
        : ConfigurationStepStatus.Error;
    }

    private void StartAndUpgradeServerStep()
    {
      CancellationToken.ThrowIfCancellationRequested();
      var upgradingSystemTables = Settings.SystemTablesUpgraded == SystemTablesUpgradedType.Yes;
      if (!upgradingSystemTables)
      {
        ReportStatus(string.Format(Resources.ServerStartingWithMinimalUpgradeWarning, Settings.ConfigureAsService ? "Windows service" : "command line"));
      }
      var startStatus = upgradingSystemTables
        ? StartServer(true, false, "--upgrade=FORCE")
        : StartServer(true, false, "--upgrade=MINIMAL");
      if (upgradingSystemTables
          && (startStatus.UpgradeStatus.UpgradeFailed
              || !startStatus.UpgradeStatus.UpgradeFinished))
      {
        Settings.PendingSystemTablesUpgrade = true;
        Settings.SystemTablesUpgraded = OldSettings.SystemTablesUpgraded;
        Settings.SaveExtendedSettings(true);
        throw new Exception(Resources.ServerConfigStartUpgradeFailed);
      }

      Settings.PendingSystemTablesUpgrade = false;
      Settings.SaveExtendedSettings();

      if (!startStatus.AcceptingConnections)
      {
        throw new Exception(Resources.ServerConfigStartUpgradeNotAcceptingConnections);
      }

      CurrentStep.Status = ConfigurationStepStatus.Finished;

      // Save the SystemTablesUpgraded back to the extended settings file
      Settings.SaveExtendedSettings();
    }

    private ServerStartStatus StartServer(bool waitUntilAcceptingConnections = true, bool setStepStatus = true, string additionalOptions = null)
    {
      CancellationToken.ThrowIfCancellationRequested();
      var serverInstance = new LocalServerInstance(this, ReportErrLogLine)
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

    private void StartServerStep()
    {
      StartServer();
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
      var serverInstanceInfo = new LocalServerInstance(this, ReportStatus);
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
        
        var serverInstanceInfo = new LocalServerInstance(this, ReportStatus);
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

    private void UpdateConfigFile()
    {
      CancellationToken.ThrowIfCancellationRequested();
      ReportStatus(string.Format(Resources.SavingConfigurationFile, BaseServerSettings.CONFIG_FILE_NAME));
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
        if (Settings.ConfigureAsService
            && !MySqlServiceControlManager.ServiceExists(Settings.ServiceName)
            && IsThereServerDataFiles)
        {
          Settings.ConfigureAsService = false;
        }
      }

      CancellationToken.ThrowIfCancellationRequested();
      if (t == null)
      {
        t = LoadTemplate();
      }

      //Set the Query Cache settings if Enterprise Firewall is enabled.
      if (Settings.Plugins.IsEnabled("mysql_firewall"))
      {
        Settings.EnableQueryCacheType = false;
        Settings.EnableQueryCacheSize = false;
      }

      if (!Directory.Exists(Settings.SecureFilePrivFolder))
      {
        Directory.CreateDirectory(Settings.SecureFilePrivFolder);
      }

      CancellationToken.ThrowIfCancellationRequested();
      Settings.Save(t);

      // If this is an upgrade, we need to run a second pass at updating the ini file but this time comparing
      // it against the corresponding template. This to determine if there are new sections, deprecated or new
      // variables that need to be added. During this second pass we ignore updating existing values since we
      // don't want to override the values defined by the user.
      if (ConfigurationType == ConfigurationType.Upgrade)
      {
        t = LoadTemplate();
        Settings.Save(t, true);
      }

      ReportStatus(string.Format(Resources.SavedConfigurationFile, BaseServerSettings.CONFIG_FILE_NAME));
      if (!ServerVersion.ServerSupportsRegeneratingRedoLogFiles())
      {
        return;
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
        var sql = Package.NormalizedVersion.ServerSupportsIdentifyClause()
                ? RolesDefined.GetCreateOrAlterUserSql(UserCrudOperationType.AlterUser, ServerUser.GetLocalRootUser(Settings.RootPassword.Sanitize(), Settings.DefaultAuthenticationPlugin))
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
            DeleteServerUserAccount(adminUser, new ServerUser(), DefaultAuthenticationPluginChanged, false);
          }

          cmd.FlushPrivileges();
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
      bool existingService = OldSettings != null
                             && OldSettings.ServiceExists();
      bool isNew = ConfigurationType == ConfigurationType.New;
      if (existingService
          && !isNew
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
        string cmd = $"\"{Path.Combine(InstallDirectory, BINARY_DIRECTORY_NAME, SERVER_EXECUTABLE_FILENAME)}\" --defaults-file=\"{Path.Combine(Settings.IniDirectory, Settings.ConfigFile)}\" {Settings.ServiceName}";
        if (existingService && !isNew)
        {
          ReportStatus(Resources.ServerConfigUpdatingExistingService);
          MySqlServiceControlManager.Update(OldSettings.ServiceName, Settings.ServiceName, Settings.ServiceName, cmd, Settings.ServiceAccountUsername, Settings.ServiceAccountPassword, Settings.ServiceStartAtStartup);
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
        string mysqlStartMenu = $"{((Folder2)GetShell32NameSpaceFolder(ShellSpecialFolderConstants.ssfCOMMONSTARTMENU)).Self.Path}\\Programs\\MySQL\\MySQL Server {Package.NormalizedVersion.ToString(2)}\\";
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
      var success = CreateOrAlterUserAccounts(ServerUser.GetLocalRootUser(Settings.RootPassword, Settings.DefaultAuthenticationPlugin), Settings.NewServerUsers, true);
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
      if ((ConfigurationType == ConfigurationType.Reconfiguration
           || isDataDirectoryConfigured)
          && OldSettings != null
          && OldSettings.OpenFirewall)
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
      if ((ConfigurationType == ConfigurationType.Reconfiguration
           || isDataDirectoryConfigured)
          && OldSettings != null
          && OldSettings.OpenFirewallForXProtocol)
      {
        RemoveFirewallRule(OldSettings.MySqlXPort);
      }

      CancellationToken.ThrowIfCancellationRequested();
      if (Settings.OpenFirewallForXProtocol)
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
      Settings.SaveExtendedSettings(!success);
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

      if (IsWriteIniConfigurationFileNeeded)
      {
        UpdateConfigFile();
      }
      else
      {
        ReportStatus(string.Format(Resources.SavingConfigurationFile, BaseServerSettings.EXTENDED_CONFIG_FILE_NAME));
        Settings.SaveExtendedSettings();
        ReportStatus(string.Format(Resources.SavedConfigurationFile, BaseServerSettings.EXTENDED_CONFIG_FILE_NAME));
      }

      CurrentStep.Status = ConfigurationStepStatus.Finished;
    }
  }
}
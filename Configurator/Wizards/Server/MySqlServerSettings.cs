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
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Win32;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Core.Controllers;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Core.Firewall;
using MySql.Configurator.Core.IniFile;
using MySql.Configurator.Core.IniFile.Template;
using MySql.Configurator.Core.Package;
using MySql.Configurator.Properties;

namespace MySql.Configurator.Wizards.Server
{
  [Serializable]
  public class MySqlServerSettings : BaseServerSettings
  {
    #region Constants

    /// <summary>
    /// The default value for the authentication policy server variable.
    /// </summary>
    public const string DEFAULT_AUTHENTICATION_POLICY = "*,,";

    public const int DEFAULT_SLOW_QUERY_LOG_TIME = 10;
    public const string DEFAULT_PIPE_OR_SHARED_MEMORY_NAME = "MYSQL";
    public const string ENTERPRISE_FIREWALL_ENABLE_SCRIPT = @"share\win_install_firewall.sql";
    public const string INNODB_LOG_FILE_NAME_PREFIX = @"ib_logfile";
    public const int X_PROTOCOL_DEFAULT_PORT = 33060;
    private const string DATABASE_BACKUP_DIRECTORY = @"Backup";
    private const string DATABASE_BACKUP_BASE_FILE_NAME = @"mysql_dump";
    private const string SECURE_FILE_PRIV_DIRECTORY = @"Uploads";

    #endregion Constants

    public MySqlServerSettings(Package p)
      : base(p)
    {
      NewServerUsers = new List<ServerUser>();
      Plugins = new PluginsList(p.NormalizedVersion);
    }

    #region Properties

    /// <summary>
    /// Gets the file name for a backup of the database.
    /// </summary>
    public static string BackupFileName => $"{DATABASE_BACKUP_BASE_FILE_NAME}-{DateTime.Now.ToString("s").Replace(":", ".")}.sql";

    /// <summary>
    /// Gets the default file name for the Binary Log.
    /// </summary>
    public static string BinaryLogDefaultFileName => $"{Environment.MachineName}-bin";

    /// <summary>
    /// Gets the default file name for the Error Log.
    /// </summary>
    public static string ErrorLogDefaultFileName => $"{Environment.MachineName}.err";

    /// <summary>
    /// Gets the default file name for the General Query Log.
    /// </summary>
    public static string GeneralQueryLogDefaultFileName => $"{Environment.MachineName}.log";

    /// <summary>
    /// Gets the default file name for the InnoDB Log 0.
    /// </summary>
    public static string InnoDbLog0DefaultFileName => $"{INNODB_LOG_FILE_NAME_PREFIX}0";

    /// <summary>
    /// Gets the default file name for the InnoDB Log 1.
    /// </summary>
    public static string InnoDbLog1DefaultFileName => $"{INNODB_LOG_FILE_NAME_PREFIX}1";

    /// <summary>
    /// Gets a value indicating if Named Pipe is the only protocol that is enabled.
    /// </summary>
    public bool IsNamedPipeTheOnlyEnabledProtocol => EnableNamedPipe
                                                      && !EnableTcpIp
                                                      && !EnableSharedMemory;

    /// <summary>
    /// Gets the default file name for the Slow Query Log.
    /// </summary>
    public static string SlowQueryLogDefaultFileName => $"{Environment.MachineName}-slow.log";

    /// <summary>
    /// Gets the full path to a file for a backup of the database.
    /// </summary>
    [XmlIgnore]
    public string FullBackupFilePath
    {
      get
      {
        var backupDirectoryPath = Path.Combine(ConfigurationFileExists.HasValue ? IniDirectory : Path.Combine(AppConfiguration.HomeDir, DATABASE_BACKUP_DIRECTORY));
        var success = true;
        if (!Directory.Exists(backupDirectoryPath))
        {
          try
          {
            Directory.CreateDirectory(backupDirectoryPath);
          }
          catch
          {
            success = false;
          }
        }

        return success
          ? Path.Combine(backupDirectoryPath, BackupFileName)
          : null;
      }
    }

    /// <summary>
    /// Administers multifactor authentication (MFA) capabilities. It applies to the authentication factor-related clauses of CREATE USER and ALTER USER statements
    /// used to manage MySQL account definitions. 
    /// </summary>
    public string AuthenticationPolicy { get; set; }

    [ControllerSetting("If configured as a Windows Service, this value sets the service to start automatically at system " +
      "startup. Ignored if as_windows_service is false.", "auto_start", "autostart")]
    [DefaultValue(true)]
    public bool ServiceStartAtStartup;

    [ControllerSetting("The binary log file name base. If omitted, the default value of host_name-bin.log is used. If it " +
      "contains a path, the log is saved in that location. If the bin_log argument is not present, it is assumed to be true.", "bin_log_file", "binlogname", true)]
    public string BinLogFileNameBase { get; set; }

    [ControllerSetting("The default authentication plugin.", "default_authentication_plugin,default_auth_plugin")]
    public MySqlAuthenticationPluginType DefaultAuthenticationPlugin { get; set; }

    [ControllerSetting("Enables binary logging. Binary logs are saved in the default location unless overridden by using " +
      "the bin_log_file argument.", "bin_log", "binlog")]
    [DefaultValue(false)]
    public bool EnableBinLog { get; set; }

    [ControllerSetting("Enables the general query log.", "general_log", "generallog")]
    [DefaultValue(false)]
    public bool EnableGeneralLog { get; set; }

    [ControllerSetting("Indicates whether the server permits connections over a named pipe.", "named_pipe", "enable_named_pipe")]
    public bool EnableNamedPipe { get; set; }

    public bool EnableQueryCacheSize { get; set; }

    public bool EnableQueryCacheType { get; set; }

    [ControllerSetting("Indicates whether the server permits shared-memory connections.", "shared_memory", "enable_shared_memory")]
    public bool EnableSharedMemory { get; set; }

    [ControllerSetting("Enables the slow query log.", "slow_query_log", "slowlog")]
    [DefaultValue(true)]
    public bool EnableSlowQueryLog { get; set; }

    public string EnterpriseFirewallSetupScript { get; set; }

    [ControllerSetting("The error log file name base. If omitted, the default value of host_name.err is used. If it contains " +
      "a path before the file name, the log is saved in that location. If only the file name is provided, the file is saved " +
      "in the data directory. If the bin_log argument is not present, it is assumed to be true.", "error_log_file", "errorlogname", true)]
    public string ErrorLogFileName { get; set; }

    [ControllerSetting("The general query log file name. If omitted the default value of host_name.log is used. If it " +
      "contains a path before the file name, the log is saved in that location. If only the file name is provided, the file " +
      "is saved in the data directory. If the general_log argument is not present, it is assumed to be true.", "general_log_file", "generallogname", true)]
    public string GeneralQueryLogFileName { get; set; }

    public string InnoDbLog0FileName { get; set; }

    public string InnoDbLog1FileName { get; set; }

    [ControllerSetting("If a query takes longer than this many seconds, the server increments the slow_queries status " +
      "variable. If the slow query log is enabled, the query is logged to the slow query log file.", "long_query_time,slow_query_time", "slowtime")]
    [DefaultValue(180)]
    public int LongQueryTime { get; set; }

    [ControllerSetting("Sets the lower_case_table_names server variable.", "lower_case_table_name")]
    [DefaultValue(1)]
    public LowerCaseTableNamesTypes LowerCaseTableNames { get; set; }

    [ControllerSetting("The network port that X Plugin uses for connections. The X Plugin equivalent of port.", "mysqlx_port", "loose_mysqlx_port", true, "CheckXPort")]
    [DefaultValue(33060)]
    public uint MySqlXPort { get; set; }

    public List<ServerUser> NewServerUsers { get; set; }

    /// <summary>
    /// Sets the Windows group which is granted full access to a named pipe.
    /// </summary>
    [ControllerSetting("Defines the users who will be granted full access to named-pipe connections as required by some " +
      "client software. A group name can be specified to select multiple users to be granted full access.", "named_pipe_full_access_group")]
    [DefaultValue("")]
    public string NamedPipeFullAccessGroup { get; set; }

    [ControllerSetting("Creates Windows Firewall rules for both port and mysqlx_port.", "open_windows_firewall,open_win_firewall", "openfirewall")]
    [DefaultValue(true)]
    public bool OpenFirewall { get; set; }

    [DefaultValue(false)]
    public bool OpenFirewallForXProtocol { get; set; }

    [ControllerSetting("The name of the named pipe used to communicate with the server. If the named_pipe argument is not " +
      "present, it is automatically set to true.", "named_pipe_name,pipe_name", "pipename")]
    [DefaultValue("MYSQL")]
    public string PipeName { get; set; }

    [XmlIgnore]
    public PluginsList Plugins { get; set; }

    [ControllerSetting("Sets the value of the secure_file_priv server variable that is used to limit the effect of " +
      "data import and export operations, such as those performed by the LOAD DATA and SELECT ... INTO OUTFILE statements " +
      "and the LOAD_FILE().", "secure_file_priv")]
    public string SecureFilePrivFolder { get; set; }

    [ControllerSetting("A unique numeric identifier used in a replication topology. If binary logging is enabled, a server " +
      "ID is suggested to be specified.", "server_id", "serverid")]
    [DefaultValue(1)]
    public uint? ServerId { get; set; }

    [ControllerSetting("Optimizes settings depending on the intended use of the server instance.", "server_type", "servertype")]
    [DefaultValue(ServerInstallationType.Developer)]
    public ServerInstallationType ServerInstallType { get; set; }

    [ControllerSetting("The password of the Windows User Account used to run the Windows Service. Ignored if " +
      "as_windows_service is false or if windows_service_user is not present.", "windows_service_password,win_service_pwd", "sapass")]
    public string ServiceAccountPassword { get; set; }

    [ControllerSetting("The name of a Windows User Account used to run the Windows service. Ignored if as_windows_service " +
      "is false.", "windows_service_user,win_service_user", "sauser")]
    public string ServiceAccountUsername { get; set; }

    [ControllerSetting("The name given to the Windows service used to run the MySQL server. Ignored if as_windows_service " +
      "is false.", "windows_service_name,win_service_name", "servicename", true, "CheckServiceName")]
    public string ServiceName { get; set; }

    [ControllerSetting("The name of the shared-memory connection used to communicate with the server. If the shared_memory " +
      "argument is not present, it is assumed to be true.", "shared_memory_name", "shared_mem_name")]
    [DefaultValue("MYSQL")]
    public string SharedMemoryName { get; set; }

    [ControllerSetting("The slow-query log file name. If omitted the default value of host_name-slow.log is used. If it " +
      "contains a path before the file name, the log is saved in that location. If only the file name is provided, the " +
      "file is saved to the data directory. If the slow_query_log argument is not present, it is assumed to be true.", "slow_log_file", "slowlogname", true)]
    public string SlowQueryLogFileName { get; set; }

    #endregion

    public void LoadIniDefaults()
    {
      Logger.LogInformation("Server Settings - Load Ini Defaults - setting initial ini values");
      FindConfigFile();

      ServiceName = $"MySQL{Package.NormalizedVersion.Major}{Package.NormalizedVersion.Minor}";

      LoadLogsDefault();
      ServerId = 1;
      LowerCaseTableNames = LowerCaseTableNamesTypes.LowerCaseStoredInsensitiveComparison;

      DefaultAuthenticationPlugin = Package.NormalizedVersion.GetDefaultServerAuthenticationPlugin();
      OpenFirewall = true;
      OpenFirewallForXProtocol = false;
      EnableTcpIp = true;
      Port = DEFAULT_PORT;
      MySqlXPort = X_PROTOCOL_DEFAULT_PORT;
      PipeName = DEFAULT_PIPE_OR_SHARED_MEMORY_NAME;
      SharedMemoryName = DEFAULT_PIPE_OR_SHARED_MEMORY_NAME;
      ConfigureAsService = true;
      ServiceStartAtStartup = true;
      ServiceAccountUsername = MySqlServiceControlManager.STANDARD_SERVICE_ACCOUNT;
      EnableQueryCacheSize = true;
      EnableQueryCacheType = true;
      NamedPipeFullAccessGroup = string.Empty;

      VerifySecureFilePrivFolder();
    }

    /// <summary>
    /// Saves the configuration file.
    /// </summary>
    /// <param name="template">The template containing the values to be saved into the configuration file.</param>
    /// <param name="skipExistingValues">If a configuration file already exists, indicates if the existing values
    /// should be replaced with the re-calculated ones.</param>
    public void Save(IniTemplate template, bool skipExistingValues = false)
    {
      if (!template.IsValid)
      {
        throw new Exception(Resources.InvalidServerTemplate);
      }

      template.ServerType = ServerInstallType;
      template.EnableNetworking = EnableTcpIp;
      template.Port = Port;
      template.EnableNamedPipe = EnableNamedPipe;
      template.PipeName = PipeName;
      template.EnableSharedMemory = EnableSharedMemory;
      template.MemoryName = SharedMemoryName;
      template.EnableQueryType = EnableQueryCacheType;
      template.EnableQueryCache = EnableQueryCacheSize;
      template.DefaultAuthenticationPlugin = DefaultAuthenticationPlugin;
      template.AuthenticationPolicy = string.IsNullOrEmpty(AuthenticationPolicy) 
        ? DefaultAuthenticationPlugin != MySqlAuthenticationPluginType.None
          && DefaultAuthenticationPlugin != MySqlAuthenticationPluginType.CachingSha2Password
          ? $"{DefaultAuthenticationPlugin.GetDescription()},,"
          : DEFAULT_AUTHENTICATION_POLICY
        : AuthenticationPolicy;
      template.LogError = string.IsNullOrEmpty(ErrorLogFileName) ? string.Empty : $"\"{ErrorLogFileName.Replace('\\', '/')}\"";
      template.BaseDir = template.BaseDir.Replace('\\', '/');
      template.DataDir = template.DataDir.Replace('\\', '/');
      template.GeneralLogFile = string.IsNullOrEmpty(GeneralQueryLogFileName) ? string.Empty : $"\"{GeneralQueryLogFileName.Replace('\\', '/')}\"";
      template.SlowQueryLogFile = string.IsNullOrEmpty(SlowQueryLogFileName) ? string.Empty : $"\"{SlowQueryLogFileName.Replace('\\', '/')}\"";
      template.LongQueryTime = LongQueryTime.ToString();
      template.LogOutput = (EnableGeneralLog || EnableSlowQueryLog) ? "FILE" : "NONE";
      template.GeneralLog = (EnableGeneralLog) ? "1" : "0";
      template.SlowQueryLog = (EnableSlowQueryLog) ? "1" : "0";
      template.LogBin = (EnableBinLog) ? string.IsNullOrEmpty(BinLogFileNameBase) ? string.Empty :
        $"\"{BinLogFileNameBase.Replace('\\', '/')}\""
        : string.Empty;
      template.ServerId = ServerId;
      template.LowerCaseTableNames = LowerCaseTableNames;
      template.SecureFilePriv = string.IsNullOrEmpty(SecureFilePrivFolder) ? string.Empty : $"\"{SecureFilePrivFolder.Replace('\\', '/')}\"";
      template.PluginLoad = string.IsNullOrEmpty(Plugins.ToString()) ? string.Empty : $"\"{Plugins}\"";
      template.LooseMySqlXPort = MySqlXPort == 0 ? X_PROTOCOL_DEFAULT_PORT : MySqlXPort;
      template.NamedPipeFullAccessGroup = NamedPipeFullAccessGroup;
      template.ProcessTemplate(false, true, skipExistingValues);
      SaveExtendedSettings();
    }

    public bool ServiceExists()
    {
      if (!ConfigureAsService
          || string.IsNullOrEmpty(ServiceName))
      {
        return false;
      }

      return MySqlServiceControlManager.ServiceExists(ServiceName);
    }

    protected override void LoadDefaultsForInstall()
    {
      base.LoadDefaultsForInstall();
      LoadIniDefaults();
    }

    protected override void LoadDefaultsForUpgrade()
    {
      Logger.LogInformation("Server Settings - Load Defaults for Upgrade - load Defaults");
      base.LoadDefaultsForUpgrade();
      var serverProductConfigurationController = (ServerProductConfigurationController) Package.UpgradeTarget.Controller;
      var mySqlServerSettings = (MySqlServerSettings) serverProductConfigurationController.Settings;
      IniDirectory = mySqlServerSettings.IniDirectory;
      ServiceName = mySqlServerSettings.ServiceName;
      VerifySecureFilePrivFolder();
    }

    protected override void LoadInstalled(RegistryKey key)
    {
      Logger.LogInformation("Server Settings - Load Installed - calling base");
      base.LoadInstalled(key);
      Logger.LogInformation("Server Settings - Load Installed - load service information");
      LoadServiceInformation();
      Logger.LogInformation("Server Settings - Load Installed - Load my Ini Settings");
      LoadIniSettings();
    }

    private string GetDefaultServiceName()
    {
      string baseName = $"MYSQL{Package.NormalizedVersion.Major}{Package.NormalizedVersion.Minor}";
      int i = 1;
      string name = baseName;
      while (MySqlServiceControlManager.ServiceExists(name))
      {
        name = $"{baseName}_{i++}";
      }

      return name;
    }

    /// <summary>
    /// Gets an <see cref="IniTemplate" /> object loading them from an existing configuration file.
    /// </summary>
    /// <returns>An <see cref="IniTemplate"/> that contains settings from an existing ini file, otherwise returns null</returns>
    public IniTemplate GetExistingIniFileTemplate()
    {
      bool? configFileExists = ConfigurationFileExists;

      IniTemplate t;

      if (configFileExists.HasValue && configFileExists.Value)
      {
        t = new IniTemplate(InstallDirectory, DataDirectory, FullConfigFilePath, Package.NormalizedVersion, ServerInstallType);
      }
      else
      {
        t = null;
      }

      return t;
    }

    private bool IsRuleEnabled(string port)
    {
      var firewallPolicy = new Policy();
      var isRuleEnabled = firewallPolicy.Rules.Any(r => r.LocalPorts != null && r.LocalPorts.Contains(port) && r.Enabled);
      return isRuleEnabled;
    }

    public override void LoadExtendedSettings()
    {
      base.LoadExtendedSettings();
      if (ExtendedSettings == null)
      {
        return;
      }

      Plugins.Enable("mysql_firewall", ExtendedSettings.EnterpriseFirewallEnabled);
    }

    private void LoadIniSettings()
    {
      Logger.LogInformation("Server Settings - Load Ini Settings - find existing config file");
      FindConfigFile();
      if (string.IsNullOrEmpty(IniDirectory))
      {
        return;
      }

      // Assign to a variable to avoid recomputing the full path over and over
      var fullIniPath = FullConfigFilePath;

      // TODO: Review this if, only affects after a new server product is installed, because there is no exists any inifile
      if (!File.Exists(fullIniPath))
      {
        Logger.LogInformation("Server Settings - Load Ini Settings - Loading default settings");
        LoadIniDefaults();
      }
      else
      {
        Logger.LogInformation("Server Settings - Load Ini Settings - loading IniFileEngine");
        var iniFile = new IniFileEngine(fullIniPath).Load();

        Logger.LogInformation("Server Settings - Load Ini Settings - IniTemplate Parsing");
        var t = new IniTemplate(Package.NormalizedVersion, ServerInstallType);
        t.ParseConfigurationFile(fullIniPath);

        Logger.LogInformation("Server Settings - Load Ini Settings - getting settings from IniTemplate");
        EnableTcpIp = t.EnableNetworking;
        Port = t.Port;
        DefaultAuthenticationPlugin = t.DefaultAuthenticationPlugin;
        EnableNamedPipe = t.EnableNamedPipe;
        PipeName = t.PipeName;
        EnableSharedMemory = t.EnableSharedMemory;
        SharedMemoryName = t.MemoryName;
        ServerId = t.ServerId;
        NamedPipeFullAccessGroup = t.NamedPipeFullAccessGroup;

        Logger.LogInformation("Server Settings - Load Ini Settings - getting settings from IniFileEngine");
        var serverType = iniFile.FindValue<int>("mysql", "server_type", true);
        ServerInstallType = serverType == 1
          ? ServerInstallationType.Dedicated
          : serverType == 2
            ? ServerInstallationType.Server
            : serverType == 3
              ? ServerInstallationType.Developer
              : serverType == 4
                ? ServerInstallationType.Manual
                : ServerInstallationType.Developer;
        OpenFirewall = EnableTcpIp && IsRuleEnabled(Port.ToString());

        ErrorLogFileName = iniFile.FindValue("mysqld", "log-error", false);
        EnableGeneralLog = iniFile.FindValue<bool>("mysqld", "general-log", false);
        GeneralQueryLogFileName = iniFile.FindValue("mysqld", "general_log_file", false);
        EnableSlowQueryLog = iniFile.FindValue<bool>("mysqld", "slow-query-log", false);
        SlowQueryLogFileName = iniFile.FindValue("mysqld", "slow_query_log_file", false);
        LongQueryTime = iniFile.FindValue<int>("mysqld", "long_query_time", false);
        BinLogFileNameBase = iniFile.FindValue("mysqld", "log-bin", false);
        EnableBinLog = !string.IsNullOrEmpty(BinLogFileNameBase);

        string serverIdValue = iniFile.FindValue<string>("mysqld", "server-id", false);
        bool convertResult = UInt32.TryParse(serverIdValue, out var serverId);
        ServerId = convertResult ? serverId : (uint?)null;
        LowerCaseTableNames = (LowerCaseTableNamesTypes) iniFile.FindValue<int>("mysqld", "lower_case_table_names", false);
        var queryCacheSizeConfigTuple = iniFile.FindValueWithState("mysqld", "query_cache_size");
        EnableQueryCacheSize = queryCacheSizeConfigTuple.Item1 != ConfigurationKeyType.NotPresent
                               && queryCacheSizeConfigTuple.Item1 == ConfigurationKeyType.NotCommented;

        var queryCacheTypeConfigTuple = iniFile.FindValueWithState("mysqld", "query_cache_type");
        EnableQueryCacheType = queryCacheTypeConfigTuple.Item1 != ConfigurationKeyType.NotPresent
                               && queryCacheTypeConfigTuple.Item1 == ConfigurationKeyType.NotCommented;

        string authenticationPluginText = string.Empty;
        if (Package.NormalizedVersion.ServerSupportsDefaultAuthenticationPluginVariable())
        {
          authenticationPluginText = iniFile.FindValue("mysqld", "default_authentication_plugin", false);
        }
        else
        {
          AuthenticationPolicy = iniFile.FindValue("mysqld", "authentication_policy", false);
          var firstFactorPlugin = AuthenticationPolicy.Split(',')[0];
          authenticationPluginText = string.IsNullOrEmpty(AuthenticationPolicy)
                                     || firstFactorPlugin.Equals("*", StringComparison.InvariantCulture)
                                       ? "caching_sha2_password"
                                       : firstFactorPlugin;
        }

        DefaultAuthenticationPlugin = ExtensionMethods.TryParseFromDescription(DefaultAuthenticationPlugin, authenticationPluginText, false, out var authenticationPlugin)
                                      && authenticationPlugin != MySqlAuthenticationPluginType.None
            ? authenticationPlugin
            : Package.NormalizedVersion.GetDefaultServerAuthenticationPlugin();

        SecureFilePrivFolder = iniFile.FindValue("mysqld", "secure-file-priv", false);

        // Ensure that if the current ini file doesn't have a value set, then use the default
        VerifySecureFilePrivFolder();
        MySqlXPort = iniFile.FindValue<uint>("mysqld", "loose_mysqlx_port", false);
        var pluginLoad = iniFile.FindValue("mysqld", "plugin_load", false);

        Plugins.LoadFromIniFile(pluginLoad);
        Plugins.Enable("mysql_firewall", EnterpriseFirewallEnabled);
        var serverConfigurationController = Package.Controller as ServerConfigurationController;
        OpenFirewallForXProtocol = serverConfigurationController != null
                                     ? serverConfigurationController.ServerVersion.ServerSupportsXProtocol()
                                     : false;
        var dataDirectory = iniFile.FindValue("mysqld", "datadir", false);
        if (!string.IsNullOrEmpty(dataDirectory)
            && Directory.Exists(dataDirectory))
        {
          var parentDirectory = Directory.GetParent(dataDirectory);
          DataDirectory = parentDirectory != null
            ? parentDirectory.FullName
            : dataDirectory;
        }
      }

      LoadExtendedSettings();
    }

    private void LoadLogsDefault()
    {
      ErrorLogFileName = ErrorLogDefaultFileName;
      EnableGeneralLog = false;
      GeneralQueryLogFileName = GeneralQueryLogDefaultFileName;
      EnableSlowQueryLog = true;
      SlowQueryLogFileName = SlowQueryLogDefaultFileName;
      EnableBinLog = Package.NormalizedVersion.ServerHasBinaryLogEnabledByDefault();
      BinLogFileNameBase = BinaryLogDefaultFileName;
      InnoDbLog0FileName = InnoDbLog0DefaultFileName;
      InnoDbLog1FileName = InnoDbLog1DefaultFileName;
      EnterpriseFirewallSetupScript = Path.Combine(InstallDirectory, ENTERPRISE_FIREWALL_ENABLE_SCRIPT);
      LongQueryTime = DEFAULT_SLOW_QUERY_LOG_TIME;
    }

    private void LoadServiceInformation()
    {
      ServiceName = GetDefaultServiceName();
      var sm = new MySqlServiceControlManager(InstallDirectory);
      if (string.IsNullOrEmpty(sm.ServiceName)
          || !MySqlServiceControlManager.ServiceExists(sm.ServiceName))
      {
        return;
      }

      ConfigureAsService = true;
      ServiceName = sm.ServiceName;
      Service s = sm.GetServiceDetails(ServiceName);
      ServiceAccountUsername = s.StartName;
      ServiceStartAtStartup = s.StartMode == "Auto";
    }

    private void VerifySecureFilePrivFolder()
    {
      if (string.IsNullOrEmpty(SecureFilePrivFolder))
      {
        SecureFilePrivFolder = Path.Combine(IniDirectory, SECURE_FILE_PRIV_DIRECTORY);
      }
      else
      {
        try
        {
          var ParentPathSecureFile = Directory.GetParent(SecureFilePrivFolder).ToString();
          if (!string.Equals(DataDirectory, ParentPathSecureFile))
          {
            SecureFilePrivFolder = Path.Combine(DataDirectory, SECURE_FILE_PRIV_DIRECTORY);
          }

        }
        catch (Exception e)
        {
          Logger.LogException(e);
        }

      }
    }
  }
}

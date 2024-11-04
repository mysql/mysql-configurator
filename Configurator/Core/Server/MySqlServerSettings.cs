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
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using MySql.Configurator.Base.Classes;
using MySql.Configurator.Base.Enums;
using MySql.Configurator.Core.Controllers;
using MySql.Configurator.Core.Firewall;
using MySql.Configurator.Core.Ini;
using MySql.Configurator.Core.Logging;
using MySql.Configurator.Core.MSI;
using MySql.Configurator.Core.Settings;
using MySql.Configurator.Properties;
using MySql.Configurator.UI.Dialogs;

namespace MySql.Configurator.Core.Server
{
  [Serializable]
  public class MySqlServerSettings
  {
    #region Constants

    /// <summary>
    /// An alternate name of the Server's configuration file.
    /// </summary>
    public const string ALTERNATE_CONFIG_FILE_NAME = "my.cnf";

    /// <summary>
    /// The default value for the authentication policy server variable.
    /// </summary>
    public const string DEFAULT_AUTHENTICATION_POLICY = "*,,";

    /// <summary>
    /// The default name of the Server's configuration file as used by the MySQL Configurator.
    /// </summary>
    public const string DEFAULT_CONFIG_FILE_NAME = "my.ini";

    /// <summary>
    /// The default MySQL Server port.
    /// </summary>
    public const int DEFAULT_PORT = 3306;

    public const int DEFAULT_SLOW_QUERY_LOG_TIME = 10;
    public const string DEFAULT_PIPE_OR_SHARED_MEMORY_NAME = "MYSQL";
    public const string ENTERPRISE_FIREWALL_ENABLE_SCRIPT = @"share\win_install_firewall.sql";
    public const string INNODB_LOG_FILE_NAME_PREFIX = @"ib_logfile";
    public const int X_PROTOCOL_DEFAULT_PORT = 33060;
    public const string SECURE_FILE_PRIV_DIRECTORY = @"Uploads";

    #endregion Constants

    #region Fields

    /// <summary>
    /// The default location of the data directory.
    /// </summary>
    private string _defaultDataDir;

    /// <summary>
    /// The general server configuration settings.
    /// </summary>
    private GeneralSettings _generalSettings;

    /// <summary>
    /// A flag indicating if the general settings file could be loaded.
    /// </summary>
    private bool _generalSettingsFileLoaded;

    #endregion

    public MySqlServerSettings(ServerInstallation serverInstallation)
    {
      ServerInstallation = serverInstallation;
      _generalSettings = null;
      _generalSettingsFileLoaded = false;
      _defaultDataDir = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
        $@"MySQL\MySQL Server {serverInstallation.Version.Major}.{serverInstallation.Version.Minor}\");
      NewServerUsers = new List<MySqlServerUser>();
      Plugins = new PluginsList(serverInstallation.Version);
      ServerInstallationType = ServerInstallationType.Developer;
    }

    #region Properties

    /// <summary>
    /// Administers multifactor authentication (MFA) capabilities. It applies to the authentication factor-related clauses of CREATE USER and ALTER USER statements
    /// used to manage MySQL account definitions. 
    /// </summary>
    public string AuthenticationPolicy { get; set; }

    /// <summary>
    /// Gets the default file name for the Binary Log.
    /// </summary>
    public static string BinaryLogDefaultFileName => $"{Environment.MachineName}-bin";

    [ControllerSetting("The binary log file name base. If omitted, the default value of host_name-bin.log is used. If it " +
     "contains a path, the log is saved in that location. If the bin_log argument is not present, it is assumed to be true.", "bin_log_file", "binlogname", true)]
    public string BinLogFileNameBase { get; set; }

    public string ConfigFile { get; set; }

    public bool? ConfigurationFileExists
    {
      get
      {
        FindConfigFile();
        if (IniDirectory == null)
        {
          return null;
        }

        return File.Exists(FullConfigFilePath);
      }
    }

    [ControllerSetting("Configures MySQL Server to run as a Windows service. By default the Windows service runs " +
      "using the Standard System Account (Network Service). If the Windows service is to be run using a different Windows " +
      "User account, the windows_service_user and windows_service_password arguments must be used.", "as_windows_service,as_win_service")]
    [DefaultValue(true)]
    public bool ConfigureAsService { get; set; }

    [ControllerSetting("Overrides the default directory where data files are stored.", "data_directory,data_dir,datadir")]
    public string DataDirectory { get; set; }

    [ControllerSetting("The default authentication plugin.", "default_authentication_plugin,default_auth_plugin")]
    public MySqlAuthenticationPluginType DefaultAuthenticationPlugin { get; set; }

    public string DefaultDataDirectory => _defaultDataDir;

    public string DefaultInstallDirectory { get; set; }

    /// <summary>
    /// Gets the default authentication plugin for the current server version.
    /// </summary>
    public static MySqlAuthenticationPluginType DefaultServerAuthenticationPlugin => MySqlAuthenticationPluginType.CachingSha2Password;

    [ControllerSetting("Enables binary logging. Binary logs are saved in the default location unless overridden by using " +
      "the bin_log_file argument.", "bin_log", "binlog")]
    [DefaultValue(false)]
    public bool EnableBinLog { get; set; }

    public string EnterpriseFirewallSetupScript { get; set; }

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

    [ControllerSetting("Allow Client/Server connections using the TCP/IP protocol. This argument is used along with a port number.", "tcp_ip", "enable_tcpip")]
    [DefaultValue(true)]
    public bool EnableTcpIp { get; set; }

    [ControllerSetting("Enable the MySQL Enterprise Firewall plugin.", "enterprise_firewall", "enable_firewall")]
    [DefaultValue(false)]
    public bool EnterpriseFirewallEnabled { get; set; }

    /// <summary>
    /// Gets the default file name for the Error Log.
    /// </summary>
    public static string ErrorLogDefaultFileName => $"{Environment.MachineName}.err";

    [ControllerSetting("The error log file name base. If omitted, the default value of host_name.err is used. If it contains " +
      "a path before the file name, the log is saved in that location. If only the file name is provided, the file is saved " +
      "in the data directory. If the bin_log argument is not present, it is assumed to be true.", "error_log_file", "errorlogname", true)]
    public string ErrorLogFileName { get; set; }

    [ControllerSetting("The password of the root user of the existing local MySQL server instance.", "existing_password,existing_root_password", "existingrootpasswd", false, "CheckPassword")]
    public string ExistingRootPassword { get; set; }

    [XmlIgnore]
    public string FullConfigFilePath => !string.IsNullOrEmpty(IniDirectory)
                                         ? Path.Combine(IniDirectory, ConfigFile)
                                         : null;

    /// <summary>
    /// Gets the default file name for the General Query Log.
    /// </summary>
    public static string GeneralQueryLogDefaultFileName => $"{Environment.MachineName}.log";

    [ControllerSetting("The general query log file name. If omitted the default value of host_name.log is used. If it " +
      "contains a path before the file name, the log is saved in that location. If only the file name is provided, the file " +
      "is saved in the data directory. If the general_log argument is not present, it is assumed to be true.", "general_log_file", "generallogname", true)]
    public string GeneralQueryLogFileName { get; set; }

    [XmlIgnore]
    public string GeneralSettingsFilePath => Path.Combine(InstallDirectory, GeneralSettingsManager.CONFIGURATOR_SETTINGS_FILE_NAME);

    /// <summary>
    /// Gets a value indicating if the general settings file exists.
    /// </summary>
    public bool GeneralSettingsFileExists => File.Exists(GeneralSettingsFilePath)
      && _generalSettingsFileLoaded;

    [XmlIgnore]
    public bool GeneralPropertiesChanged
    {
      get
      {
        if (_generalSettings == null)
        {
          return false;
        }

        return !_generalSettings.HasSamePropertyValues(this);
      }
    }

    public string IniDirectory { get; set; }

    /// <summary>
    /// Gets the default file name for the InnoDB Log 0.
    /// </summary>
    public static string InnoDbLog0DefaultFileName => $"{INNODB_LOG_FILE_NAME_PREFIX}0";

    public string InnoDbLog0FileName { get; set; }

    /// <summary>
    /// Gets the default file name for the InnoDB Log 1.
    /// </summary>
    public static string InnoDbLog1DefaultFileName => $"{INNODB_LOG_FILE_NAME_PREFIX}1";

    public string InnoDbLog1FileName { get; set; }

    [ControllerSetting("Overrides the default installation directory.", "install_dir,installdir")]
    public string InstallDirectory { get; set; }

    /// <summary>
    /// Gets a value indicating if Named Pipe is the only protocol that is enabled.
    /// </summary>
    public bool IsNamedPipeTheOnlyEnabledProtocol => EnableNamedPipe
                                                      && !EnableTcpIp
                                                      && !EnableSharedMemory;

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

    public List<MySqlServerUser> NewServerUsers { get; set; }

    /// <summary>
    /// Sets the Windows group which is granted full access to a named pipe.
    /// </summary>
    [ControllerSetting("Defines the users who will be granted full access to named-pipe connections as required by some " +
      "client software. A group name can be specified to select multiple users to be granted full access.", "named_pipe_full_access_group")]
    [DefaultValue("")]
    public string NamedPipeFullAccessGroup { get; set; }

    [NonSerialized]
    public MySqlServerSettings OldSettings;

    [ControllerSetting("Creates Windows Firewall rules for both port and mysqlx_port.", "open_windows_firewall,open_win_firewall", "openfirewall")]
    [DefaultValue(true)]
    public bool OpenFirewall { get; set; }

    [DefaultValue(false)]
    public bool OpenFirewallForXProtocol { get; set; }

    [NonSerialized]
    protected ServerInstallation ServerInstallation;

    /// <summary>
    /// Gets or sets a value indicating whether an upgrade to system tables is pending to be performed.
    /// </summary>
    [XmlIgnore]
    public bool PendingSystemTablesUpgrade { get; set; }

    [ControllerSetting("The name of the named pipe used to communicate with the server. If the named_pipe argument is not " +
      "present, it is automatically set to true.", "named_pipe_name,pipe_name", "pipename")]
    [DefaultValue("MYSQL")]
    public string PipeName { get; set; }

    [XmlIgnore]
    public PluginsList Plugins { get; set; }

    [ControllerSetting("The TCP/IP port number of the MySQL server instance that is used for Client/Server protocol " +
      "connections. This argument is used along with tcp_ip.", "port", null, true, "CheckPort")]
    [DefaultValue(DEFAULT_PORT)]
    public uint Port { get; set; }

    [ControllerSetting("The password that will be assigned to the root user during a new installation or reconfiguration.", "password,pwd,root_password", "passwd,rootpasswd", false, "CheckPassword")]
    public string RootPassword { get; set; }

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
    public ServerInstallationType ServerInstallationType { get; set; }

    [ControllerSetting("The password of the Windows User Account used to run the Windows Service. Ignored if " +
      "as_windows_service is false or if windows_service_user is not present.", "windows_service_password,win_service_pwd", "sapass")]
    public string ServiceAccountPassword { get; set; }

    [ControllerSetting("The name of a Windows User Account used to run the Windows service. Ignored if as_windows_service " +
      "is false.", "windows_service_user,win_service_user", "sauser")]
    public string ServiceAccountUsername { get; set; }

    [ControllerSetting("The name given to the Windows service used to run the MySQL server. Ignored if as_windows_service " +
      "is false.", "windows_service_name,win_service_name", "servicename", true, "CheckServiceName")]
    public string ServiceName { get; set; }

    [ControllerSetting("If configured as a Windows Service, this value sets the service to start automatically at system " +
      "startup. Ignored if as_windows_service is false.", "auto_start", "autostart")]
    [DefaultValue(true)]
    public bool ServiceStartAtStartup;

    [ControllerSetting("The name of the shared-memory connection used to communicate with the server. If the shared_memory " +
      "argument is not present, it is assumed to be true.", "shared_memory_name", "shared_mem_name")]
    [DefaultValue("MYSQL")]
    public string SharedMemoryName { get; set; }

    /// <summary>
    /// Gets the default file name for the Slow Query Log.
    /// </summary>
    public static string SlowQueryLogDefaultFileName => $"{Environment.MachineName}-slow.log";

    [ControllerSetting("The slow-query log file name. If omitted the default value of host_name-slow.log is used. If it " +
      "contains a path before the file name, the log is saved in that location. If only the file name is provided, the " +
      "file is saved to the data directory. If the slow_query_log argument is not present, it is assumed to be true.", "slow_log_file", "slowlogname", true)]
    public string SlowQueryLogFileName { get; set; }

    /// <summary>
    /// Gets the general settings associated to this server installation.
    /// </summary>
    [XmlIgnore]
    protected GeneralSettings GeneralSettings => _generalSettings;

    #endregion

    public MySqlServerSettings Clone()
    {
      var method = typeof(Utilities).GetMethod("DeepClone");
      var type = GetType();
      var generic = method?.MakeGenericMethod(type);
      return (MySqlServerSettings)generic?.Invoke(null, new object[] { this });
    }

    public void CloneToOldSettings()
    {
      OldSettings = Clone();
    }

    /// <summary>
    /// Deletes the server configuration file and the extended configuration file created by MySQL Configurator.
    /// </summary>
    /// <param name="removeGeneralSettingsFile">Indicates if the general settings file should be deleted.</param>
    /// <returns><c>true</c> if the operation completed successfully; otherwise, <c>false</c>.</returns>
    public bool DeleteConfigFile(bool removeGeneralSettingsFile)
    {
      if (string.IsNullOrEmpty(IniDirectory))
      {
        return false;
      }

      var configFilePath = FullConfigFilePath;
      try
      {
        if (File.Exists(configFilePath))
        {
          File.SetAttributes(configFilePath, FileAttributes.Normal);
          File.Delete(configFilePath);
        }

        _generalSettingsFileLoaded = false;
      }
      catch (Exception ex)
      {
        Logger.LogError(string.Format(Resources.ConfigFileDeleteError, configFilePath));
        Logger.LogException(ex);

        return false;
      }

      if (!removeGeneralSettingsFile)
      {
        return true;
      }

      var generalSettingsFilePath = Path.Combine(InstallDirectory, GeneralSettingsManager.CONFIGURATOR_SETTINGS_FILE_NAME);
      try
      {
        if (File.Exists(generalSettingsFilePath))
        {
          File.SetAttributes(generalSettingsFilePath, FileAttributes.Normal);
          File.Delete(generalSettingsFilePath);
        }
      }
      catch (Exception ex)
      {
        Logger.LogError(string.Format(Resources.ConfigFileDeleteError, configFilePath));
        Logger.LogException(ex);

        return false;
      }

      return true;
    }

    public void FindConfigFile()
    {
      //if data dir or install dir is null then is in an earlier stage of the loading phase of the installer
      if (DataDirectory == null
          || InstallDirectory == null
          || (IniDirectory != null && ConfigFile != null))
      {
        return;
      }

      var foundConfigFile = false;
      string[] configFileNames = new string[2] { DEFAULT_CONFIG_FILE_NAME, ALTERNATE_CONFIG_FILE_NAME };
      string[] possibleConfigFileLocations = new string[3] { DataDirectory, InstallDirectory, IniDirectory };
      foreach (var configFileName in configFileNames)
      {
        foreach (var directory in possibleConfigFileLocations)
        {
          if (string.IsNullOrEmpty(directory))
          {
            continue;
          }

          if (!File.Exists(Path.Combine(directory, configFileName)))
          {
            continue;
          }

          ConfigFile = configFileName;
          IniDirectory = directory;
          foundConfigFile = true;
          break;
        }

        if (foundConfigFile)
        {
          break;
        }
      }

      if (!foundConfigFile)
      {
        ConfigFile = MySqlServerSettings.DEFAULT_CONFIG_FILE_NAME;
        IniDirectory = DataDirectory;
      }
    }

    /// <summary>
    /// Gets the default service name based on the version of the package.
    /// </summary>
    /// <returns>A string representing the default service name.</returns>
    public string GetDefaultServiceName()
    {
      string baseName = $"MYSQL{ServerInstallation.Version.Major}{ServerInstallation.Version.Minor}";
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
        t = new IniTemplate(InstallDirectory, DataDirectory, FullConfigFilePath, ServerInstallation.Version, ServerInstallationType, null);
      }
      else
      {
        t = null;
      }

      return t;
    }

    public bool HasChanges()
    {
      if (OldSettings == null)
      {
        return false;
      }

      var props = GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(ControllerSettingAttribute)));
      foreach (var propertyInfo in props)
      {
        object oldValue = propertyInfo.GetValue(OldSettings, null);
        object newValue = propertyInfo.GetValue(this, null);
        if (oldValue == null && newValue != null
            || oldValue != null && newValue == null)
        {
          return true;
        }

        if (oldValue == null)
        {
          continue;
        }

        if (!oldValue.Equals(newValue))
        {
          return true;
        }
      }

      return false;
    }

    public void LoadDefaultsForInstall()
    {
      ConfigureAsService = true;
      string name = ServerInstallation.DISPLAY_NAME.Replace('/', '.');
      PendingSystemTablesUpgrade = false;
      DataDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\MySQL\\{name}";
      if (!StringEndsWithVersion(DataDirectory))
      {
        DataDirectory = $"{DataDirectory} {ServerInstallation.Version.Major}.{ServerInstallation.Version.Minor}";
      }

      Logger.LogInformation("Controller Settings - Load Defaults For Install - load and set default settings");
      string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
      InstallDirectory = $"{programFiles}\\MySQL\\{name}";
      if (!StringEndsWithVersion(InstallDirectory))
      {
        InstallDirectory = $"{InstallDirectory} {ServerInstallation.Version.Major}.{ServerInstallation.Version.Minor}";
      }

      DefaultInstallDirectory = InstallDirectory;
      LoadIniDefaults();
    }

    public void LoadDefaultsForUpgrade()
    {
      Logger.LogInformation("Server Settings - Load Defaults for Upgrade - load Defaults");
      Logger.LogInformation("Controller Settings - Load Defaults For Upgrade - load and set default settings for upgrade");
      LoadDefaultsForInstall();
      var c = ServerInstallation.Controller;
      DataDirectory = c.DataDirectory;
      InstallDirectory = c.InstallDirectory;
      PendingSystemTablesUpgrade = true;
      var serverProductConfigurationController = ServerInstallation.Controller;
      var mySqlServerSettings = (MySqlServerSettings)serverProductConfigurationController.Settings;
      IniDirectory = mySqlServerSettings.IniDirectory;
      ServiceName = mySqlServerSettings.ServiceName;
      VerifySecureFilePrivFolder();
    }

    /// <summary>
    /// Loads the general server settings.
    /// </summary>
    public void LoadGeneralSettings()
    {
      if (!File.Exists(GeneralSettingsFilePath))
      {
        return;
      }

      _generalSettings = GeneralSettingsManager.ReadSettings(InstallDirectory);
      _generalSettingsFileLoaded = _generalSettings != null;
      if (!_generalSettingsFileLoaded)
      {
        if (!GeneralSettingsManager.LoadWarningShown)
        {
          InfoDialog.ShowDialog(InfoDialogProperties.GetWarningDialogProperties(Resources.AppName,
          string.Format(Resources.SettingsFileReadError, InstallDirectory, GeneralSettingsManager.CONFIGURATOR_SETTINGS_FILE_NAME),
          Resources.ReferToLogMessage));
          GeneralSettingsManager.LoadWarningShown = true;
        }

        return;
      }

      if (!string.IsNullOrEmpty(_generalSettings.IniDirectory))
      {
        var iniFilePath = Path.Combine(_generalSettings.IniDirectory, MySqlServerSettings.DEFAULT_CONFIG_FILE_NAME);
        var alternateIniFilePath = Path.Combine(_generalSettings.IniDirectory, MySqlServerSettings.ALTERNATE_CONFIG_FILE_NAME);
        var iniFileExists = File.Exists(iniFilePath);
        var alternateIniFileExists = File.Exists(alternateIniFilePath);
        if (!iniFileExists
            && !alternateIniFileExists)
        {
          using (var configurationFileDialog = new ConfigurationFileDialog())
          {
            if (configurationFileDialog.ShowDialog() == DialogResult.OK)
            {
              _generalSettings.IniDirectory = configurationFileDialog.ConfigurationFilePath;
            }
            else
            {
              throw new ConfiguratorException(ConfiguratorError.ConfigurationFileNotFound, _generalSettings.IniDirectory);
            }
          }
        }

        // Load and parse ini file to get the data dir path.
        var path = iniFileExists
          ? iniFilePath
          : alternateIniFilePath;
        var iniFile = new IniFileEngine(path).Load();
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

      this.SetPropertyValuesFrom(_generalSettings);
      if (GeneralSettings == null)
      {
        return;
      }

      Plugins.Enable("mysql_firewall", GeneralSettings.EnterpriseFirewallEnabled);
    }

    public void LoadIniDefaults()
    {
      Logger.LogInformation("Server Settings - Load Ini Defaults - setting initial ini values");
      FindConfigFile();

      ServiceName = $"MySQL{ServerInstallation.Version.Major}{ServerInstallation.Version.Minor}";

      LoadLogsDefault();
      ServerId = 1;
      LowerCaseTableNames = LowerCaseTableNamesTypes.LowerCaseStoredInsensitiveComparison;

      DefaultAuthenticationPlugin = DefaultServerAuthenticationPlugin;
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

    public void LoadInstalled()
    {
      LoadGeneralSettings();
      if (!string.IsNullOrEmpty(DataDirectory))
      {
        return;
      }

      if (string.IsNullOrEmpty(DataDirectory))
      {
        DataDirectory = _defaultDataDir;
      }
      Logger.LogInformation("Server Settings - Load Installed - load service information");
      LoadServiceInformation();
      Logger.LogInformation("Server Settings - Load Installed - Load my Ini Settings");
      LoadIniSettings();
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

      template.ServerInstallationType = ServerInstallationType;
      template.EnableNetworking = EnableTcpIp;
      template.Port = Port;
      template.EnableNamedPipe = EnableNamedPipe;
      template.PipeName = PipeName;
      template.EnableSharedMemory = EnableSharedMemory;
      template.MemoryName = SharedMemoryName;
      template.EnableQueryType = EnableQueryCacheType;
      template.EnableQueryCache = EnableQueryCacheSize;
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
      template.MySqlXPort = MySqlXPort == 0 ? X_PROTOCOL_DEFAULT_PORT : MySqlXPort;
      template.NamedPipeFullAccessGroup = NamedPipeFullAccessGroup;
      template.ProcessTemplate(false, true, skipExistingValues);
      SaveGeneralSettings();
    }

    /// <summary>
    /// Saves the general server configuration settings.
    /// </summary>
    public void SaveGeneralSettings()
    {
      if (string.IsNullOrEmpty(IniDirectory))
      {
        return;
      }

      var generalSettings = new GeneralSettings();
      generalSettings.SetPropertyValuesFrom(this);
      try
      {
        // Not all server versions create the ini directory by default during installation.
        if (!Directory.Exists(IniDirectory))
        {
          Directory.CreateDirectory(IniDirectory);
        }

        GeneralSettingsManager.SaveSettings(GeneralSettingsFilePath, generalSettings);
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
      }
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

    public bool SetValue(string keyword, string value, out string message)
    {
      message = string.Empty;
      var type = GetType();
      var props = type.GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(ControllerSettingAttribute)));
      foreach (var propertyInfo in props)
      {
        var controllerSettingAttribute = propertyInfo.GetCustomAttributes(typeof(ControllerSettingAttribute), true).First() as ControllerSettingAttribute;
        if (controllerSettingAttribute == null
            || !controllerSettingAttribute.IsValidKeyword(keyword))
        {
          continue;
        }

        try
        {
          if (propertyInfo.PropertyType == typeof(bool))
          {
            SetBoolValue(propertyInfo, value);
          }
          else if (propertyInfo.PropertyType == typeof(int))
          {
            SetIntValue(propertyInfo, value);
          }
          else if (propertyInfo.PropertyType == typeof(uint))
          {
            SetUIntValue(propertyInfo, value);
          }
          else if (propertyInfo.PropertyType.IsEnum)
          {
            SetEnumValue(propertyInfo, value);
          }
          else if (propertyInfo.PropertyType == typeof(string))
          {
            propertyInfo.SetValue(this, value, null);
          }

          if (controllerSettingAttribute.CheckAction != null)
          {
            ExecuteCheckAction(controllerSettingAttribute.CheckAction, keyword, value, propertyInfo);
          }
        }
        catch (Exception ex)
        {
          message = ex.Message;
          return false;
        }
        return true;
      }

      message = string.Format(Resources.InvalidPropertyGiven, keyword);
      return false;
    }

    public bool StringEndsWithVersion(string text)
    {
      string ver = $"{ServerInstallation.Version.Major}.{ServerInstallation.Version.Minor}";
      return text.EndsWith(ver);
    }

    /// <summary>
    /// Executes validations for controller setting attributes.
    /// </summary>
    /// <param name="actionName">The name of the action to execute.</param>
    /// <param name="key">The name of the controller setting.</param>
    /// <param name="value">The value of the controller setting.</param>
    /// <param name="propertyInfo">The property information instance used to update the value of the controller setting.</param>
    private void ExecuteCheckAction(string actionName, string key, string value, PropertyInfo propertyInfo = null)
    {
      switch (actionName)
      {
        case "CheckPort":
          if (!uint.TryParse(value, out var port))
          {
            throw new InvalidOperationException(string.Format(Resources.NotProperValueForUInt, value));
          }

          if (!Utilities.PortIsAvailable(port))
          {
            throw new Exception(string.Format(Resources.PortNotAvailable, value));
          }

          break;
        case "CheckPassword":
          var errorMessage = MySqlServerInstance.ValidatePassword(value, true);
          if (!string.IsNullOrEmpty(errorMessage))
          {
            throw new Exception(string.Format(errorMessage));
          }

          break;
        case "CheckServiceName":
          if (Service.ExistsServiceInstance(value))
          {
            throw new Exception(string.Format(Resources.InvalidServiceName, value));
          }

          break;
        case "CheckXPort":
          if (!uint.TryParse(value, out var xPort))
          {
            throw new InvalidOperationException(string.Format(Resources.NotProperValueForUInt, value));
          }

          if (!Utilities.PortIsAvailable(xPort))
          {
            throw new Exception(string.Format(Resources.PortNotAvailable, value));
          }

          break;
        default:
          throw new NotSupportedException(string.Format(Resources.CheckMethodNotSupported, actionName));
      }
    }

    private bool IsRuleEnabled(string port)
    {
      var firewallPolicy = new Policy();
      var isRuleEnabled = firewallPolicy.Rules.Any(r => r.LocalPorts != null && r.LocalPorts.Contains(port) && r.Enabled);
      return isRuleEnabled;
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
        var t = new IniTemplate(ServerInstallation.Version, ServerInstallationType);
        t.ParseConfigurationFile(fullIniPath);

        Logger.LogInformation("Server Settings - Load Ini Settings - getting settings from IniTemplate");
        EnableTcpIp = t.EnableNetworking;
        Port = t.Port;
        DefaultAuthenticationPlugin = DefaultServerAuthenticationPlugin;
        EnableNamedPipe = t.EnableNamedPipe;
        PipeName = t.PipeName;
        EnableSharedMemory = t.EnableSharedMemory;
        SharedMemoryName = t.MemoryName;
        ServerId = t.ServerId;
        NamedPipeFullAccessGroup = t.NamedPipeFullAccessGroup;

        Logger.LogInformation("Server Settings - Load Ini Settings - getting settings from IniFileEngine");


        // Attempt to read the server installation type from the ini file for old configurations.
        // Value is ignored if the extended settings file already contains an entry for the server installation type.
        var serverType = iniFile.FindValue<int>("mysql", "server_type", true);
        if (serverType != 0)
        {
          ServerInstallationType = serverType == 1
          ? ServerInstallationType.Dedicated
          : serverType == 2
            ? ServerInstallationType.Server
            : serverType == 3
              ? ServerInstallationType.Developer
              : serverType == 4
                ? ServerInstallationType.Manual
                : ServerInstallationType.Developer;
        }

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

        AuthenticationPolicy = iniFile.FindValue("mysqld", "authentication_policy", false);
        DefaultAuthenticationPlugin = ParseFirstFactorAuthentication(AuthenticationPolicy);
        SecureFilePrivFolder = iniFile.FindValue("mysqld", "secure-file-priv", false);

        // Ensure that if the current ini file doesn't have a value set, then use the default
        VerifySecureFilePrivFolder();
        MySqlXPort = iniFile.FindValue<uint>("mysqld", "loose_mysqlx_port", false);
        var pluginLoad = iniFile.FindValue("mysqld", "plugin_load", false);

        Plugins.LoadFromIniFile(pluginLoad);
        Plugins.Enable("mysql_firewall", EnterpriseFirewallEnabled);
        var serverConfigurationController = ServerInstallation.Controller;
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

      LoadGeneralSettings();
    }

    private void LoadLogsDefault()
    {
      ErrorLogFileName = ErrorLogDefaultFileName;
      EnableGeneralLog = false;
      GeneralQueryLogFileName = GeneralQueryLogDefaultFileName;
      EnableSlowQueryLog = true;
      SlowQueryLogFileName = SlowQueryLogDefaultFileName;
      EnableBinLog = ServerInstallation.Version.ServerHasBinaryLogEnabledByDefault();
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
      if (sm.ServiceInfos.Length == 0)
      {
        return;
      }

      ConfigureAsService = true;
      var configFileDirectory = !string.IsNullOrEmpty(IniDirectory) ? IniDirectory : DataDirectory;
      ServiceName = sm.GetBestServiceNameMatchingConfigFileDirectory(configFileDirectory);
      Service s = MySqlServiceControlManager.GetServiceDetails(ServiceName);
      ServiceAccountUsername = s.StartName;
      ServiceStartAtStartup = s.StartMode == "Auto";
    }

    /// <summary>
    /// Obtains the default authentication plugin based on value of the authentication_policy server variable.
    /// </summary>
    /// <param name="authenticationPolicy">The value of the authentication_policy server variable.</param>
    /// <returns>An instance of the <see cref="MySqlAuthenticationPluginType"/> identifying the default authentication plugin.</returns>
    private MySqlAuthenticationPluginType ParseFirstFactorAuthentication(string authenticationPolicy)
    {
      string authenticationPluginText = string.Empty;
      string firstFactor = null;
      if (string.IsNullOrEmpty(authenticationPolicy))
      {
        return DefaultServerAuthenticationPlugin;
      }

      firstFactor = authenticationPolicy.Trim().Split(',')[0].Trim();
      if (firstFactor.IndexOf(':') != -1)
      {
        var elements = firstFactor.Split(':');
        firstFactor = elements.Length > 1
          ? elements[1]
          : firstFactor;
      }

      authenticationPluginText = firstFactor != null
                                 && firstFactor.Equals("*", StringComparison.InvariantCulture)
                                   ? DefaultServerAuthenticationPlugin.GetDescription()
                                   : firstFactor;
      ExtensionMethods.TryParseFromDescription(DefaultAuthenticationPlugin, authenticationPluginText, false, out var authenticationPlugin);
      if (authenticationPlugin == MySqlAuthenticationPluginType.None)
      {
        Logger.LogWarning(string.Format(Resources.AuthenticationPolicyParseError, firstFactor));
        return DefaultServerAuthenticationPlugin;
      }

      return authenticationPlugin;
    }

    private void SetBoolValue(PropertyInfo propertyInfo, string value)
    {
      if (propertyInfo == null)
      {
        return;
      }

      bool result;
      if (!bool.TryParse(value, out result))
      {
        throw new InvalidOperationException(string.Format(Resources.NotProperValueForBool, value));
      }

      propertyInfo.SetValue(this, result, null);
    }

    private void SetIntValue(PropertyInfo propertyInfo, string value)
    {
      if (propertyInfo == null)
      {
        return;
      }

      int result;
      if (!int.TryParse(value, out result))
      {
        throw new InvalidOperationException(string.Format(Resources.NotProperValueForInt, value));
      }

      propertyInfo.SetValue(this, result, null);
    }

    private void SetUIntValue(PropertyInfo propertyInfo, string value)
    {
      if (propertyInfo == null)
      {
        return;
      }

      uint result;
      if (!uint.TryParse(value, out result))
      {
        throw new InvalidOperationException(string.Format(Resources.NotProperValueForInt, value));
      }

      propertyInfo.SetValue(this, result, null);
    }

    private void SetEnumValue(PropertyInfo propertyInfo, string value)
    {
      if (propertyInfo == null)
      {
        return;
      }

      if (!Enum.IsDefined(propertyInfo.PropertyType, value))
      {
        throw new InvalidOperationException(string.Format(Resources.NotProperValueForType, value, propertyInfo.Name));
      }

      propertyInfo.SetValue(this, Enum.Parse(propertyInfo.PropertyType, value), null);
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

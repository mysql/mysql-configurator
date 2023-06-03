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
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Win32;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Properties;

namespace MySql.Configurator.Core.Controllers
{
  [Serializable]
  public class BaseServerSettings : ControllerSettings
  {
    #region Constants

    /// <summary>
    /// The name of the Server's configuration file as used by the MySQL Configurator.
    /// </summary>
    public const string CONFIG_FILE_NAME = "my.ini";

    /// <summary>
    /// The default MySQL Server port.
    /// </summary>
    public const int DEFAULT_PORT = 3306;

    /// <summary>
    /// The name of the extended Server's configuration file as used by the MySQL Configurator.
    /// </summary>
    public const string EXTENDED_CONFIG_FILE_NAME = "server_config.xml";

    #endregion Constants

    #region Fields

    /// <summary>
    /// The default location of the data directory.
    /// </summary>
    private string _defaultDataDir;

    /// <summary>
    /// Extended server settings saved in an XML file.
    /// </summary>
    private ExtendedServerSettings _extendedSettings;

    #endregion Fields

    public BaseServerSettings(Package.Package p) : base(p)
    {
      _extendedSettings = null;
      SystemTablesUpgraded = SystemTablesUpgradedType.None;
      _defaultDataDir = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
        $@"MySQL\MySQL Server {p.NormalizedVersion.Major}.{p.NormalizedVersion.Minor}\");
    }

    #region Properties

    public string ConfigFile { get; set; }

    [ControllerSetting("Configures MySQL Server to run as a Windows service. By default the Windows service runs " +
      "using the Standard System Account (Network Service). If the Windows service is to be run using a different Windows " +
      "User account, the windows_service_user and windows_service_password arguments must be used.", "as_windows_service,as_win_service")]
    [DefaultValue(true)]
    public bool ConfigureAsService { get; set; }

    [ControllerSetting("Overrides the default directory where data files are stored.", "data_directory,data_dir,datadir")]
    public string DataDirectory { get; set; }

    public string DefaultDataDirectory { get; set; }

    [ControllerSetting("Allow Client/Server connections using the TCP/IP protocol. This argument is used along with a port number.", "tcp_ip", "enable_tcpip")]
    [DefaultValue(true)]
    public bool EnableTcpIp { get; set; }

    [ControllerSetting("Enable the MySQL Enterprise Firewall plugin.", "enterprise_firewall", "enable_firewall")]
    [DefaultValue(false)]
    public bool EnterpriseFirewallEnabled { get; set; }

    [ControllerSetting("The password of the root user of the existing local MySQL server instance.", "existing_password,existing_root_password", "existingrootpasswd", false, "CheckPassword")]
    public string ExistingRootPassword { get; set; }

    [XmlIgnore]
    public string ExtendedSettingsFilePath => Path.Combine(IniDirectory, EXTENDED_CONFIG_FILE_NAME);

    [XmlIgnore]
    public bool ExtendedPropertiesChanged
    {
      get
      {
        if (_extendedSettings == null)
        {
          return false;
        }

        return !_extendedSettings.HasSamePropertyValues(this);
      }
    }

    [XmlIgnore]
    public string FullConfigFilePath => Path.Combine(IniDirectory, ConfigFile);

    public string IniDirectory { get; set; }

    public ServerConfigurationType InnoDbClusterType { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether an upgrade to system tables is pending to be performed.
    /// </summary>
    [XmlIgnore]
    public bool PendingSystemTablesUpgrade { get; set; }

    [ControllerSetting("The TCP/IP port number of the MySQL server instance that is used for Client/Server protocol " +
      "connections. This argument is used along with tcp_ip.", "port", null, true, "CheckPort")]
    [DefaultValue(DEFAULT_PORT)]
    public uint Port { get; set; }

    [ControllerSetting("The password that will be assigned to the root user during a new installation or reconfiguration.", "password,pwd,root_password", "passwd,rootpasswd", false, "CheckPassword")]
    public string RootPassword { get; set; }

    [ControllerSetting("Indicates how the local instance is configured (StandAlone is currently the only supported option).", "serverconfigurationtype", null, true)]
    [DefaultValue(ServerConfigurationType.StandAlone)]
    public ServerConfigurationType ServerConfigurationType { get; set; }

    public string ServerVersion { get; set; }

    [XmlIgnore]
    public SystemTablesUpgradedType SystemTablesUpgraded { get; set; }

    /// <summary>
    /// Gets the extended settings associated to this server installation.
    /// </summary>
    [XmlIgnore]
    protected ExtendedServerSettings ExtendedSettings => _extendedSettings;

    #endregion Properties

    /// <summary>
    /// Verifies if configuration file exists based on current settings.
    /// </summary>
    /// <returns><c>true</c> if the configuration file exists, <c>false</c> if configuration file doesn't exists, <c>null</c> if IniDirectory property is null  </returns>
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

    /// <summary>
    /// Deletes the server configuration file and the extended configuration file created by MySQL Configurator.
    /// </summary>
    /// <param name="removeExtendedSettingsFile">Indicates if the extended settings file should be deleted.</param>
    /// <returns><c>true</c> if the operation completed successfully; otherwise, <c>false</c>.</returns>
    public bool DeleteConfigFile(bool removeExtendedSettingsFile)
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
      }
      catch (Exception ex)
      {
        Logger.LogError(string.Format(Resources.ConfigFileDeleteError, configFilePath));
        Logger.LogException(ex);

        return false;
      }

      if (!removeExtendedSettingsFile)
      {
        return true;
      }

      var extSettingsFilePath = Path.Combine(IniDirectory, EXTENDED_CONFIG_FILE_NAME);
      try
      {
        if (File.Exists(extSettingsFilePath))
        {
          File.SetAttributes(extSettingsFilePath, FileAttributes.Normal);
          File.Delete(extSettingsFilePath);
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

      if (ConfigFile == null)
      {
        ConfigFile = CONFIG_FILE_NAME;
      }

      if (File.Exists(Path.Combine(DataDirectory, ConfigFile)))
      {
        IniDirectory = DataDirectory.Clone() as string;
      }
      else if (File.Exists(Path.Combine(InstallDirectory, ConfigFile)))
      {
        IniDirectory = InstallDirectory.Clone() as string;
      }
      else
      {
        IniDirectory = DataDirectory;
      }
    }

    /// <summary>
    /// Loads Server configuration values not stored in the Server's configuration file.
    /// </summary>
    public virtual void LoadExtendedSettings()
    {
      if (string.IsNullOrEmpty(IniDirectory))
      {
        return;
      }

      if (!File.Exists(ExtendedSettingsFilePath))
      {
        return;
      }

      _extendedSettings = ExtendedServerSettings.Deserialize(ExtendedSettingsFilePath);
      if (_extendedSettings == null)
      {
        return;
      }

      this.SetPropertyValuesFrom(_extendedSettings);
    }

    /// <summary>
    /// Saves Server configuration values not stored in the Server's configuration file.
    /// </summary>
    /// <param name="preserveInnoDbClusterSettings"><c>true</c> if the InnoDB settings should be preserved; otherwise, <c>false</c>.</param>
    public virtual void SaveExtendedSettings(bool preserveInnoDbClusterSettings = false)
    {
      if (string.IsNullOrEmpty(IniDirectory))
      {
        return;
      }

      ServerVersion = Package.Version;
      var extendedSettings = new ExtendedServerSettings();
      extendedSettings.SetPropertyValuesFrom(this);
      try
      {
        // Not all server versions create the ini directory by default during installation.
        if (!Directory.Exists(IniDirectory))
        {
          Directory.CreateDirectory(IniDirectory);
        }

        extendedSettings.Serialize(ExtendedSettingsFilePath, !preserveInnoDbClusterSettings);
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
      }
    }

    protected override void LoadDefaultsForInstall()
    {
      ConfigureAsService = true;
      string name = Package.Title.Replace('/', '.');
      ServerVersion = null;
      SystemTablesUpgraded = SystemTablesUpgradedType.None;
      PendingSystemTablesUpgrade = false;
      DataDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\MySQL\\{name}";
      if (!StringEndsWithVersion(DataDirectory))
      {
        DataDirectory = $"{DataDirectory} {Package.NormalizedVersion.Major}.{Package.NormalizedVersion.Minor}";
      }

      DefaultDataDirectory = DataDirectory;
      base.LoadDefaultsForInstall();
    }

    protected override void LoadDefaultsForUpgrade()
    {
      base.LoadDefaultsForUpgrade();
      var c = (ServerProductConfigurationController)Package.UpgradeTarget.Controller;
      DataDirectory = c.DataDirectory;
      InstallDirectory = c.InstallDirectory;
      ServerVersion = null;
      SystemTablesUpgraded = SystemTablesUpgradedType.None;
      PendingSystemTablesUpgrade = true;
    }

    protected override void LoadInstalled()
    {
      base.LoadInstalled();
      if (!string.IsNullOrEmpty(DataDirectory))
      {
        return;
      }

      if (string.IsNullOrEmpty(DataDirectory))
      {
        DataDirectory = _defaultDataDir;
      }
    }
  }
}

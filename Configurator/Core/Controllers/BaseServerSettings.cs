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
using System.Windows.Forms;
using System.Xml.Serialization;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.Forms;
using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Core.Common;
using MySql.Configurator.Core.IniFile;
using MySql.Configurator.Properties;
using static MySql.Configurator.Core.Classes.Forms.InfoDialog;

namespace MySql.Configurator.Core.Controllers
{
  [Serializable]
  public class BaseServerSettings : ControllerSettings
  {
    #region Constants

    /// <summary>
    /// An alternate name of the Server's configuration file.
    /// </summary>
    public const string ALTERNATE_CONFIG_FILE_NAME = "my.cnf";

    /// <summary>
    /// The default name of the Server's configuration file as used by the MySQL Configurator.
    /// </summary>
    public const string DEFAULT_CONFIG_FILE_NAME = "my.ini";

    /// <summary>
    /// The default MySQL Server port.
    /// </summary>
    public const int DEFAULT_PORT = 3306;

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

    #endregion Fields

    public BaseServerSettings(Package.Package p) : base(p)
    {
      _generalSettings = null;
      _generalSettingsFileLoaded = false;
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

    [XmlIgnore]
    public string FullConfigFilePath => Path.Combine(IniDirectory, ConfigFile);

    public string IniDirectory { get; set; }

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

    /// <summary>
    /// Gets the general settings associated to this server installation.
    /// </summary>
    [XmlIgnore]
    protected GeneralSettings GeneralSettings => _generalSettings;

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
      string[] possibleConfigFileLocations = new string[2] { DataDirectory, InstallDirectory };
      foreach (var configFileName in configFileNames)
      {
        foreach (var directory in possibleConfigFileLocations)
        {
          if (!File.Exists(Path.Combine(directory, configFileName)))
          {
            continue;
          }

          ConfigFile = configFileName;
          IniDirectory = directory;
          foundConfigFile = true;
        }

        if (foundConfigFile)
        {
          break;
        }
      }

      if (!foundConfigFile)
      {
        ConfigFile = BaseServerSettings.DEFAULT_CONFIG_FILE_NAME;
        IniDirectory = DataDirectory;
      }
    }

    /// <summary>
    /// Loads the general server configuration settings.
    /// </summary>
    public virtual void LoadGeneralSettings()
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
        var iniFilePath = Path.Combine(_generalSettings.IniDirectory, BaseServerSettings.DEFAULT_CONFIG_FILE_NAME);
        if (File.Exists(iniFilePath))
        {
          // Load and parse ini file to get the data dir path.
          var iniFile = new IniFileEngine(iniFilePath).Load();
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
      }

      this.SetPropertyValuesFrom(_generalSettings);
    }

    /// <summary>
    /// Saves the general server configuration settings.
    /// </summary>
    public virtual void SaveGeneralSettings()
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

    protected override void LoadDefaultsForInstall()
    {
      ConfigureAsService = true;
      string name = Package.Title.Replace('/', '.');
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

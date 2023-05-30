// Copyright (c) 2023, Oracle and/or its affiliates.
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License as
// published by the Free Software Foundation; version 2 of the
// License.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
// 02110-1301  USA

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Properties;
using MySql.Configurator.Core.Classes.Forms;

namespace MySql.Configurator.Core.Classes.MySqlWorkbench
{
  /// <summary>
  /// Defines methods that help working with MySQL Workbench from external programs.
  /// </summary>
  public static class MySqlWorkbench
  {
    #region Constants

    /// <summary>
    /// The relative path of the MySQL Workbench application data directory.
    /// </summary>
    public const string APP_DATA_DIRECTORY_RELATIVE_PATH = @"\MySQL\Workbench\";

    /// <summary>
    /// The MySQL Workbench application name.
    /// </summary>
    public const string APP_NAME = "Workbench";

    /// <summary>
    /// The name of the connections file.
    /// </summary>
    public const string CONNECTIONS_FILE_NAME = "connections.xml";

    /// <summary>
    /// The relative path of the connections file under the application data directory.
    /// </summary>
    public const string CONNECTIONS_FILE_RELATIVE_PATH = APP_DATA_DIRECTORY_RELATIVE_PATH + CONNECTIONS_FILE_NAME;

    /// <summary>
    /// The file name containing extra parameters not handled by MySQL Workbench.
    /// </summary>
    public const string EXTRA_PARAMETERS_FILE_NAME = "extra_parameters.xml";

    /// <summary>
    /// The full MySQL Workbench application name.
    /// </summary>
    public const string FULL_APP_NAME = @"MySQL " + APP_NAME;

    /// <summary>
    /// The full MySQL Utilities application name.
    /// </summary>
    public const string FULL_UTILITIES_APP_NAME = "MySQL Utilities";

    /// <summary>
    /// The MySQL Workbench process name.
    /// </summary>
    public const string PROCESS_NAME = "MySQLWorkbench.exe";

    /// <summary>
    /// The name of the server instances file.
    /// </summary>
    public const string SERVERS_FILE_NAME = "server_instances.xml";

    /// <summary>
    /// The relative path of the server instances file under the application data directory.
    /// </summary>
    public const string SERVERS_FILE_RELATIVE_PATH = APP_DATA_DIRECTORY_RELATIVE_PATH + SERVERS_FILE_NAME;

    #endregion Constants

    #region Fields

    /// <summary>
    /// Flag indicating whether the current MySQL Workbench version supports sharing connections with external applications.
    /// </summary>
    private static bool? _allowsExternalConnectionsManagement;

    /// <summary>
    /// Monitors changes to the MySQL Workbench connections XML file.
    /// </summary>
    private static FileSystemWatcher _connectionsFileWatcher;

    /// <summary>
    /// A <see cref="ConnectionsMigrationStatusType"/> value about external connections migration status to MySQL Workbench.
    /// </summary>
    private static ConnectionsMigrationStatusType _connectionsMigrationStatus;

    /// <summary>
    /// Gets a value indicating whether MySQL Workbench is installed.
    /// </summary>
    private static bool _isInstalled;

    /// <summary>
    /// Monitors changes to the MySQL Workbench servers XML file.
    /// </summary>
    private static FileSystemWatcher _serversFileWatcher;

    /// <summary>
    /// Flag indicating whether MySQL Workbench was installed before the last time <see cref="IsInstalled"/> checked.
    /// </summary>
    private static bool _wasInstalled;

    /// <summary>
    /// Monitors changes to the MySQL Workbench application data directory.
    /// </summary>
    private static FileSystemWatcher _workbenchAppDataDirWatcher;

    /// <summary>
    /// Flag indicating whether the MySQL Workbench application data directory exists.
    /// </summary>
    private static bool? _workbenchDataDirectoryExists;

    #endregion Fields

    /// <summary>
    /// Initializes the <see cref="MySqlWorkbench"/> class.
    /// </summary>
    static MySqlWorkbench()
    {
      _allowsExternalConnectionsManagement = null;
      _connectionsFileWatcher = null;
      _connectionsMigrationStatus = ConnectionsMigrationStatusType.NoMigrationNeededOrAlreadyMigrated;
      _isInstalled = false;
      _serversFileWatcher = null;
      _wasInstalled = false;
      _workbenchAppDataDirWatcher = null;
      _workbenchDataDirectoryExists = null;
      ChangeCurrentCursor = null;
      ConnectionsMigrationDelay = ConnectionsMigrationDelayType.None;
      WorkbenchConnections = new MySqlWorkbenchConnectionCollection(true);
      ExternalConnections = new MySqlWorkbenchConnectionCollection(false);
      Servers = new MySqlWorkbenchServerCollection();
      ExternalApplicationConnectionsFilePath = string.Empty;
      ExternalApplicationsConnectionsFileRetryLoadCount = 3;
      ExternalApplicationsConnectionsFileRetryLoadOrRecreate = false;
      ExternalApplicationName = string.Empty;
    }

    #region Enumerations

    /// <summary>
    /// Specifies migration delay values for external connections migration to Workbench.
    /// </summary>
    public enum ConnectionsMigrationDelayType
    {
      /// <summary>
      /// No delay has been set.
      /// </summary>
      [Description("No delay has been set.")]
      None,

      /// <summary>
      /// Delays the migraton for 1 hour.
      /// </summary>
      [Description("Delays the migraton for 1 hour.")]
      DelayOneHour,

      /// <summary>
      /// Delays the migraton for 1 day.
      /// </summary>
      [Description("Delays the migraton for 1 day.")]
      DelayOneDay,

      /// <summary>
      /// Delays the migraton for 1 week.
      /// </summary>
      [Description("Delays the migraton for 1 week.")]
      DelayOneWeek,

      /// <summary>
      /// Delays the migraton for 1 month.
      /// </summary>
      [Description("Delays the migraton for 1 month.")]
      DelayOneMonth,

      /// <summary>
      /// Delays the migraton indefinitely, until triggered manually.
      /// </summary>
      [Description("Delays the migraton indefinitely, until triggered manually.")]
      DelayIndefinitely
    }

    /// <summary>
    /// Specifies migration status values for external connections migration to Workbench.
    /// </summary>
    public enum ConnectionsMigrationStatusType
    {
      /// <summary>
      /// No migration is necessary or it already took place.
      /// </summary>
      [Description("No migration is necessary or it already took place.")]
      NoMigrationNeededOrAlreadyMigrated,

      /// <summary>
      /// Migration needs to happen but connections were not migrated.
      /// </summary>
      [Description("Migration needs to happen but connections were not migrated.")]
      MigrationNeededButNotMigrated,

      /// <summary>
      /// Migration needed and connections were migrated successfully.
      /// </summary>
      [Description("Migration needed and connections were migrated successfully.")]
      MigrationNeededAlreadyMigrated
    }

    #endregion Enumerations

    #region Delegates / Events

    /// <summary>
    /// Delegate to change the cursor in a consumer program.
    /// </summary>
    /// <param name="cursor"></param>
    public delegate void ChangeCurrentCursorAction(Cursor cursor);

    /// <summary>
    /// Event fired when the MySQL Workbench application data directory is modified, deleted or created.
    /// </summary>
    public static event FileSystemEventHandler AppDataDirectoryChanged;

    /// <summary>
    /// EVent fired when the MySQL Workbench connections XML file is modified, deleted or created.
    /// </summary>
    public static event FileSystemEventHandler ConnectionsFileChanged;

    /// <summary>
    /// EVent fired when the MySQL Workbench servers XML file is modified, deleted or created.
    /// </summary>
    public static event FileSystemEventHandler ServersFileChanged;

    #endregion Delegates / Events

    #region Properties

    /// <summary>
    /// Gets a value indicating whether the current MySQL Workbench version supports sharing connections with external applications.
    /// </summary>
    public static bool AllowsExternalConnectionsManagement
    {
      get
      {
        bool allowByInstalled = IsInstalled;
        if (allowByInstalled
            && _allowsExternalConnectionsManagement == null)
        {
          _allowsExternalConnectionsManagement = SupportsExternalConnectionsManagement;
        }

        return allowByInstalled
               && _allowsExternalConnectionsManagement.Value;
      }
    }

    /// <summary>
    /// Gets or sets a delegate method to change the current cursor in a consumer program.
    /// </summary>
    public static ChangeCurrentCursorAction ChangeCurrentCursor { get; set; }

    /// <summary>
    /// Gets the MySQL Workbench connections or the external one depending on the <see cref="AllowsExternalConnectionsManagement"/> property value.
    /// </summary>
    public static MySqlWorkbenchConnectionCollection Connections => UseWorkbenchConnections
      ? WorkbenchConnections
      : ExternalConnections;

    /// <summary>
    /// Gets a value indicating whether the MySQL Workbench connections file exists.
    /// </summary>
    public static bool ConnectionsFileExists
    {
      get
      {
        string connectionsFilePath = ConnectionsFilePath;
        return !string.IsNullOrEmpty(connectionsFilePath)
               && File.Exists(connectionsFilePath);
      }
    }

    /// <summary>
    /// Gets the MySQL Workbench connections file path.
    /// </summary>
    public static string ConnectionsFilePath => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + CONNECTIONS_FILE_RELATIVE_PATH;

    /// <summary>
    /// Gets or sets a <see cref="ConnectionsMigrationDelayType"/> value about external connections migration delay to MySQL Workbench.
    /// </summary>
    public static ConnectionsMigrationDelayType ConnectionsMigrationDelay { get; set; }

    /// <summary>
    /// Gets a <see cref="ConnectionsMigrationStatusType"/> value about external connections migration status to MySQL Workbench.
    /// </summary>
    public static ConnectionsMigrationStatusType ConnectionsMigrationStatus
    {
      get
      {
        if (_connectionsMigrationStatus == ConnectionsMigrationStatusType.NoMigrationNeededOrAlreadyMigrated)
        {
          // If MySQL Workbench does not allow sharing connections with external programs or the external connections file does not exist,
          // it means external connections were already migrated or there is no need to migrate anything.
          _connectionsMigrationStatus = AllowsExternalConnectionsManagement && ExternalApplicationConnectionsFileExists
            ? ConnectionsMigrationStatusType.MigrationNeededButNotMigrated
            : ConnectionsMigrationStatusType.NoMigrationNeededOrAlreadyMigrated;
        }

        return _connectionsMigrationStatus;
      }

      private set => _connectionsMigrationStatus = value;
    }

    /// <summary>
    /// Gets a value indicating whether the MySQL Workbench connections file exists.
    /// </summary>
    public static bool ExternalApplicationConnectionsFileExists
    {
      get
      {
        string externalApplicationConnectionsFilePath = ExternalApplicationConnectionsFilePath;
        return !string.IsNullOrEmpty(externalApplicationConnectionsFilePath)
               && File.Exists(externalApplicationConnectionsFilePath);

      }
    }

    /// <summary>
    /// Gets or sets an external's application's file path of the MySQL connections file to be used.
    /// </summary>
    public static string ExternalApplicationConnectionsFilePath
    {
      get => ExternalConnections.ConnectionsFilePath;
      set
      {
        ExternalConnections.ConnectionsFilePath = value;
        if (!string.IsNullOrEmpty(value))
        {
          var parentDirectory = Path.GetDirectoryName(value);
          if (!string.IsNullOrEmpty(parentDirectory))
          {
            string extraParametersFilePath = Path.Combine(parentDirectory, EXTRA_PARAMETERS_FILE_NAME);
            ExternalConnections.ExtraParametersFilePath = extraParametersFilePath;
            WorkbenchConnections.ExtraParametersFilePath = extraParametersFilePath;
          }
        }

        LoadExternalConnections();
      }
    }

    /// <summary>
    /// Gets or sets the number of times the load of external connections file is retried.
    /// </summary>
    public static byte ExternalApplicationsConnectionsFileRetryLoadCount { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the external connections file load is retried or the file recreated if the load fails.
    /// </summary>
    public static bool ExternalApplicationsConnectionsFileRetryLoadOrRecreate { get; set; }

    /// <summary>
    /// The consumer application name, displayed in the title bar of <see cref="InfoDialog"/> dialogs.
    /// </summary>
    public static string ExternalApplicationName { get; set; }

    /// <summary>
    /// Gets a list of connections in MySQL Workbench connections format but stored in an external file.
    /// </summary>
    public static MySqlWorkbenchConnectionCollection ExternalConnections { get; }

    /// <summary>
    /// Gets a value indicating whether MySQL Workbench is installed.
    /// </summary>
    public static bool IsInstalled
    {
      get
      {
        _wasInstalled = _isInstalled;
        _isInstalled = Utilities.IsProductInstalled(FULL_APP_NAME);
        if (_isInstalled != _wasInstalled)
        {
          _allowsExternalConnectionsManagement = null;
        }

        return _isInstalled;
      }
    }

    /// <summary>
    /// Gets the MySQL Workbench installation location.
    /// </summary>
    public static string InstallLocation => Utilities.GetProductInstallLocation(FULL_APP_NAME);

    /// <summary>
    /// Gets a value indicating whether MySQL Utilities is installed.
    /// </summary>
    /// <returns></returns>
    public static bool IsMySqlUtilitiesInstalled
    {
      get
      {
        //This code will detect if MySQL Utilities is installed for versions 6 or higher of Workbench.
        if (!string.IsNullOrEmpty(UtilitiesInstallLocation))
        {
          return true;
        }

        //This code will work with Workbench versions 5 and lower.
        var installationPath = InstallLocation;
        var utilitiesFullPath = !string.IsNullOrEmpty(installationPath)
          ? Path.Combine(installationPath, "utilities")
          : null;
        return !string.IsNullOrEmpty(utilitiesFullPath)
               && Directory.GetDirectories(installationPath, "*", SearchOption.TopDirectoryOnly).Any(s => string.Equals(s, utilitiesFullPath, StringComparison.OrdinalIgnoreCase));
      }
    }

    /// <summary>
    /// Gets a value indicating whether MySQL Workbench is running.
    /// </summary>
    public static bool IsRunning => Utilities.IsProcessRunning(PROCESS_NAME);

    /// <summary>
    /// Gets the MySQL Workbench product version.
    /// </summary>
    public static Version ProductVersion
    {
      get
      {
        try
        {
          var installationPath = InstallLocation;
          if (string.IsNullOrEmpty(installationPath))
          {
            return null;
          }

          var filePath = Path.Combine(installationPath, PROCESS_NAME);
          return Utilities.GetProductVersion(filePath);
        }
        catch (Exception ex)
        {
          Logger.LogException(ex);
          return null;
        }
      }
    }

    /// <summary>
    /// Gets or sets a list of MySQL Workbench servers.
    /// </summary>
    public static MySqlWorkbenchServerCollection Servers { get; set; }

    /// <summary>
    /// Gets a value indicating whether the MySQL Workbench server instances file exists.
    /// </summary>
    public static bool ServersFileExists
    {
      get
      {
        string serversFilePath = ServersFilePath;
        return !string.IsNullOrEmpty(serversFilePath)
               && File.Exists(serversFilePath);
      }
    }

    /// <summary>
    /// Gets a value indicating whether the installed MySQL Workbench version supports sharing connections with external applications.
    /// </summary>
    public static bool SupportsExternalConnectionsManagement
    {
      get
      {
        var currentVersion = ProductVersion;
        if (currentVersion == null)
        {
          return false;
        }

        return currentVersion >= new Version("5.2.42.0");
      }
    }

    /// <summary>
    /// Gets a value indicating whether the installed MySQL Workbench version supports single dashes for long argument names in its command line.
    /// </summary>
    public static bool SupportsSingleDashForLongArguments
    {
      get
      {
        var currentVersion = ProductVersion;
        if (currentVersion == null)
        {
          return false;
        }

        return currentVersion < new Version("6.3.9.0");
      }
    }

    /// <summary>
    /// Gets the MySQL Workbench server instances file path.
    /// </summary>
    public static string ServersFilePath => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + SERVERS_FILE_RELATIVE_PATH;

    /// <summary>
    /// Gets a value indicating whether the MySQL Workbench connections file should be used or the one for an external application.
    /// </summary>
    public static bool UseWorkbenchConnections => (ConnectionsFileExists
                                                   || AllowsExternalConnectionsManagement)
                                                  && ConnectionsMigrationStatus != ConnectionsMigrationStatusType.MigrationNeededButNotMigrated;

    /// <summary>
    /// Gets the MySQL Utilities installation location.
    /// </summary>
    public static string UtilitiesInstallLocation => Utilities.GetProductInstallLocation(FULL_UTILITIES_APP_NAME);

    /// <summary>
    /// Gets a list of connections defined in the MySQL Workbench connections file.
    /// </summary>
    public static MySqlWorkbenchConnectionCollection WorkbenchConnections { get; }

    /// <summary>
    /// Gets the Workbench's application data directory.
    /// </summary>
    public static string WorkbenchDataDirectory => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + APP_DATA_DIRECTORY_RELATIVE_PATH;

    /// <summary>
    /// Gets a value indicating whether the MySQL Workbench application data directory exists.
    /// </summary>
    public static bool WorkbenchDataDirectoryExists
    {
      get
      {
        if (!_workbenchDataDirectoryExists.HasValue)
        {
          string appDirectory = WorkbenchDataDirectory;
          _workbenchDataDirectoryExists = !string.IsNullOrEmpty(appDirectory)
                                          && Directory.Exists(appDirectory);
        }

        return _workbenchDataDirectoryExists.Value;
      }
    }

    #endregion Properties

    /// <summary>
    /// Creates custom elements in the XML file being created.
    /// </summary>
    /// <param name="doc">The <see cref="XmlDocument"/> that contains repository information.</param>
    /// <param name="ptrValue">The value to be converted to hexadecimal.</param>
    /// <param name="contentStructName">Tha value of the content-struct-name attribute.</param>
    public static void CreateRootXmlElements(ref XmlDocument doc, int ptrValue, string contentStructName)
    {
      var root = doc.CreateElement("data");
      root.SetAttribute("grt_format", "2.0");
      doc.AppendChild(root);
      var valueNode = doc.CreateElement("value");
      valueNode.SetAttribute("_ptr_", ptrValue.ToString("X16"));
      valueNode.SetAttribute("type", "list");
      valueNode.SetAttribute("content-type", "object");
      valueNode.SetAttribute("content-struct-name", contentStructName);
      valueNode.InnerText = string.Empty;
      root.AppendChild(valueNode);
    }

    /// <summary>
    /// Gets the text displayed to users about the next automatic connections migration in case it was delayed.
    /// </summary>
    public static string GetConnectionsMigrationDelayText(DateTime nextAutomaticConnectionsMigration, bool workbenchMigrationSucceeded)
    {
      return nextAutomaticConnectionsMigration.Equals(DateTime.MaxValue)
        ? Resources.ConnectionsMigrationIndefiniteText
        : nextAutomaticConnectionsMigration.Equals(DateTime.MinValue)
          ? workbenchMigrationSucceeded ? Resources.ConnectionsMigrationAlreadyText : Resources.ConnectionsMigrationNotNeededText
          : $"{nextAutomaticConnectionsMigration.ToLongDateString()} {nextAutomaticConnectionsMigration.ToShortTimeString()}";
    }

    /// <summary>
    /// Gets a <see cref="ConnectionsMigrationDelayType"/> value from a delay text.
    /// </summary>
    /// <param name="fromDelayText">A delay text.</param>
    /// <param name="removeSpaces">Flag indicating whether spaces are removed from the delay text.</param>
    /// <param name="prependDelay">Flag indicating whether the word "Delay" is prepended to the delay text.</param>
    /// <returns>A <see cref="ConnectionsMigrationDelayType"/> value from a delay text.</returns>
    public static ConnectionsMigrationDelayType GetConnectionsMigrationDelayType(string fromDelayText, bool removeSpaces, bool prependDelay = false)
    {
      var returnEnumValue = ConnectionsMigrationDelayType.None;
      if (string.IsNullOrEmpty(fromDelayText))
      {
        return returnEnumValue;
      }

      if (removeSpaces)
      {
        fromDelayText = fromDelayText.Replace(" ", string.Empty);
      }

      if (prependDelay)
      {
        fromDelayText = "Delay" + fromDelayText;
      }

      Enum.TryParse(fromDelayText, true, out returnEnumValue);
      return returnEnumValue;
    }

    /// <summary>
    /// Opens MySQL Workbench right on the Server Administration window of the given server instance.
    /// </summary>
    /// <param name="server">MySQL Workbench server instance to administrate.</param>
    /// <returns><c>true</c> if the MySQL Workbench process is launched successfully, <c>false</c> otherwise.</returns>
    public static bool LaunchConfigure(MySqlWorkbenchServer server)
    {
      if (server == null)
      {
        Logger.LogError(Resources.LaunchWorkbenchCancelledNoServerInstance);
        return false;
      }

      string installPath = InstallLocation;
      if (string.IsNullOrEmpty(installPath))
      {
        Logger.LogError(Resources.LaunchWorkbenchCancelledNoInstallLocation);
        return false;
      }

      var installFilePath = Path.Combine(installPath, PROCESS_NAME);
      if (!File.Exists(installFilePath))
      {
        Logger.LogError(string.Format(Resources.LaunchWorkbenchCancelledExeNotFound, installFilePath));
        return false;
      }

      var success = false;
      try
      {
        var adminArgument = SupportsSingleDashForLongArguments
          ? "-admin"
          : "--admin";
        var startInfo = new ProcessStartInfo(installFilePath, $"{adminArgument} \"{server.Name}\"")
        {
          UseShellExecute = false
        };

        Process.Start(startInfo);
        success = true;
      }
      catch (Exception ex)
      {
        Logger.LogException(ex, true, Resources.FailureToLaunchWorkbench);
      }

      return success;
    }

    /// <summary>
    /// Opens MySQL Workbench right on the SQL Editor window using the given connection.
    /// </summary>
    /// <param name="connectionName">Name of the connection to use to open the SQL Editor.</param>
    /// <returns><c>true</c> if the MySQL Workbench process is launched successfully, <c>false</c> otherwise.</returns>
    public static bool LaunchSqlEditor(string connectionName)
    {
      if (string.IsNullOrEmpty(connectionName))
      {
        Logger.LogError(Resources.LaunchWorkbenchCancelledNoConnectionName);
        return false;
      }

      string installPath = InstallLocation;
      if (string.IsNullOrEmpty(installPath))
      {
        Logger.LogError(Resources.LaunchWorkbenchCancelledNoInstallLocation);
        return false;
      }

      var installFilePath = Path.Combine(installPath, PROCESS_NAME);
      if (!File.Exists(installFilePath))
      {
        Logger.LogError(string.Format(Resources.LaunchWorkbenchCancelledExeNotFound, installFilePath));
        return false;
      }

      var success = false;
      try
      {
        var queryArgument = SupportsSingleDashForLongArguments
          ? "-query"
          : "--query";
        var startInfo = new ProcessStartInfo(installFilePath, $"{queryArgument} \"{connectionName}\"")
        {
          UseShellExecute = false
        };

        Process.Start(startInfo);
        success = true;
      }
      catch (Exception ex)
      {
        Logger.LogException(ex, true, Resources.FailureToLaunchWorkbench);
      }

      return success;
    }

    /// <summary>
    /// Opens the MySQL Utilities command shell installed with MySQL Workbench.
    /// </summary>
    public static void LaunchUtilitiesShell()
    {
      var installPath = UtilitiesInstallLocation;
      var arguments = @"echo The following utilities are available: && echo. &&  dir *.exe  /B /W";
      if (string.IsNullOrEmpty(installPath))
      {
        //This code will be executed if MySQL Workbench installed version is 5 or lower.
        installPath = InstallLocation;
        if (string.IsNullOrEmpty(installPath))
        {
          return;
        }

        arguments = arguments.Insert(0, @"/K cd utilities && ");
      }
      else
      {
        //This code will be executed if MySQL Workbench installed version is 6 or higher.
        arguments = arguments.Insert(0, @"/K ");
      }

      var startInfo = new ProcessStartInfo("cmd.exe")
      {
        Arguments = arguments,
        UseShellExecute = false,
        CreateNoWindow = false,
        WorkingDirectory = installPath
      };

      Process.Start(startInfo);
    }

    /// <summary>
    /// Loads both external and Workbench connections from their corresponding XML files.
    /// </summary>
    public static void LoadConnections()
    {
      WorkbenchConnections.Load(false);
      LoadExternalConnections();
    }

    /// <summary>
    /// Loads Workbench connections and servers and also external connections.
    /// </summary>
    public static void LoadData()
    {
      LoadConnections();
      LoadServers();
    }

    /// <summary>
    /// Loads external connections from the given XML file.
    /// </summary>
    public static void LoadExternalConnections()
    {
      if (!string.IsNullOrEmpty(ExternalApplicationConnectionsFilePath))
      {
        ExternalConnections.Load(ExternalApplicationsConnectionsFileRetryLoadOrRecreate && ExternalApplicationConnectionsFileExists, ExternalApplicationsConnectionsFileRetryLoadCount);
      }
    }

    /// <summary>
    /// Loads Workbench servers from the servers.xml file under the Workbench install directory.
    /// </summary>
    public static void LoadServers()
    {
      Servers.Load(false);
    }

    /// <summary>
    /// Migrates connections from a consumer application's connections file to the MySQL Workbench connections one.
    /// </summary>
    /// <param name="showDelayOptions">Flag indicating whether options to delay the migration are shown in case the user chooses not to migrate connections now.</param>
    public static void MigrateExternalConnectionsToWorkbench(bool showDelayOptions)
    {
      // If MySQL Workbench does not allow sharing connections with external programs or the external connections file
      //  does not exist it means we already migrated existing connections or they were never created, no need to migrate.
      if (!AllowsExternalConnectionsManagement
          || !ExternalApplicationConnectionsFileExists
          || !ConnectionsFileExists)
      {
        ConnectionsMigrationDelay = ConnectionsMigrationDelayType.None;
        ConnectionsMigrationStatus = ConnectionsMigrationStatusType.NoMigrationNeededOrAlreadyMigrated;
        return;
      }

      // Make sure we are working on the latest copy of connections.
      LoadData();

      // If the external connections file did not have actually any connection, no need to prompt anything to users,
      // we are safe to just delete the file and use the Workbench connections file from now on.
      if (ExternalConnections.Count == 0)
      {
        ConnectionsMigrationDelay = ConnectionsMigrationDelayType.None;
        if (ExternalApplicationConnectionsFileExists)
        {
          File.Delete(ExternalApplicationConnectionsFilePath);
          ConnectionsMigrationStatus = ConnectionsMigrationStatusType.MigrationNeededAlreadyMigrated;
        }

        return;
      }

      // Inform users we are about to migrate connections and ask them if they wish to migrate now or later.
      var infoProperties = InfoDialogProperties.GetYesNoDialogProperties(
        InfoDialog.InfoType.Info,
        Resources.MigrateConnectionsToWorkbenchInfoTitle,
        Resources.MigrateConnectionsToWorkbenchInfoDetail,
        Resources.MigrateConnectionsToWorkbenchInfoSubDetail,
        string.Format(Resources.MigrateConnectionsToWorkbenchInfoMoreInfo, ExternalApplicationName));
      infoProperties.WordWrapMoreInfo = true;
      infoProperties.CommandAreaProperties.DefaultButton = InfoDialog.DefaultButtonType.Button2;
      infoProperties.CommandAreaProperties.DefaultButtonTimeout = 30;
      infoProperties.CommandAreaProperties.LeftAreaControl = showDelayOptions
        ? CommandAreaProperties.LeftAreaControlType.InfoComboBox
        : CommandAreaProperties.LeftAreaControlType.MoreInfoButton;
      infoProperties.CommandAreaProperties.LeftAreaComboBoxDataSource = showDelayOptions
        ? ConnectionsMigrationDelayType.None.GetDescriptionsDictionary(true, true, true)
        : null;
      infoProperties.CommandAreaProperties.Button3IsMoreInfo = showDelayOptions;
      var infoResult = InfoDialog.ShowDialog(infoProperties);
      if (infoResult.DialogResult != DialogResult.Yes)
      {
        ConnectionsMigrationDelay = showDelayOptions
          ? GetConnectionsMigrationDelayType(infoResult.InfoComboBoxSelectedValue, true)
          : ConnectionsMigrationDelayType.None;
        ConnectionsMigrationStatus = ConnectionsMigrationStatusType.MigrationNeededButNotMigrated;
        return;
      }

      // If MySQL Workbench is running we won't be able to migrate since the file will be in use, issue an error and exit, attempt to migrate next time.
      if (IsRunning)
      {
        InfoDialog.ShowDialog(InfoDialogProperties.GetErrorDialogProperties(
          Resources.GenericErrorTitle,
          Resources.UnableToMergeConnectionsErrorDetail,
          null,
          string.Format(Resources.UnableToMergeConnectionsErrorMoreInfo, ExternalApplicationName)));
        ConnectionsMigrationDelay = ConnectionsMigrationDelayType.None;
        ConnectionsMigrationStatus = ConnectionsMigrationStatusType.MigrationNeededButNotMigrated;
        return;
      }

      // Turn off the watcher monitoring the application data directory and save its state.
      var workbenchAppDataDirWatcherRaisingEvents = false;
      if (_workbenchAppDataDirWatcher != null
          && _workbenchAppDataDirWatcher.EnableRaisingEvents)
      {
        workbenchAppDataDirWatcherRaisingEvents = _workbenchAppDataDirWatcher.EnableRaisingEvents;
        _workbenchAppDataDirWatcher.EnableRaisingEvents = false;
      }

      // Turn off the watcher monitoring the Workbench's connections.xml file and save its state.
      var connectionsFileWatcherRaisingEvents = false;
      if (_connectionsFileWatcher != null
          && _connectionsFileWatcher.EnableRaisingEvents)
      {
        connectionsFileWatcherRaisingEvents = _connectionsFileWatcher.EnableRaisingEvents;
        _connectionsFileWatcher.EnableRaisingEvents = false;
      }

      // Migrate the external connections
      var connectionsMigrationResult = MigrateExternalConnections();

      // Attempt to migrate connection passwords from the consumer application to MySQL Workbench.
      var passwordsMigrationResult = connectionsMigrationResult.Success
        ? MySqlWorkbenchPasswordVault.MigratePasswordsFromConsumerApplicationToWorkbench()
        : null;

      // Assemble the informational messages about the migration results and show it to users.
      ShowConnectionsMigrationResults(connectionsMigrationResult, passwordsMigrationResult);

      // Load MySQL Workbench connections again so they are ready for use.
      if (connectionsMigrationResult.Success)
      {
        ExternalConnections.Clear();
        LoadData();
      }

      ConnectionsMigrationDelay = ConnectionsMigrationDelayType.None;
      ConnectionsMigrationStatus = ConnectionsMigrationStatusType.MigrationNeededAlreadyMigrated;

      // Revert the status of the watcher monitoring the application data directory if needed.
      if (_workbenchAppDataDirWatcher != null
          && workbenchAppDataDirWatcherRaisingEvents)
      {
        _workbenchAppDataDirWatcher.EnableRaisingEvents = true;
      }

      // Revert the status of the watcher monitoring the Workbench's connections.xml file if needed.
      if (_connectionsFileWatcher != null
          && connectionsFileWatcherRaisingEvents)
      {
        _connectionsFileWatcher.EnableRaisingEvents = true;
      }
    }

    /// <summary>
    /// Opens MySQL Workbench right on the Manage Connections dialog.
    /// </summary>
    public static void OpenManageConnectionsDialog()
    {
      // Check if Workbench is installed, if not exit.
      var installPath = InstallLocation;
      if (string.IsNullOrEmpty(installPath))
      {
        return;
      }

      // Check if Workbench executable is found in the installation path, if not exit.
      var fullWorkbenchFilePath = Path.Combine(installPath, PROCESS_NAME);
      if (!File.Exists(fullWorkbenchFilePath))
      {
        return;
      }

      var startInfo = new ProcessStartInfo { FileName = fullWorkbenchFilePath };
      var version = new Version(FileVersionInfo.GetVersionInfo(startInfo.FileName).ProductVersion);
      if (version >= new Version(5, 2, 40))
      {
        const string PARAMETERS = "import grt;grt.modules.Workbench.showConnectionManager()";
        var commandLineParams = new StringBuilder();
        commandLineParams.AppendFormat("--run \"{0}\"", PARAMETERS);
        startInfo.Arguments = commandLineParams.ToString();
      }

      Process.Start(startInfo);
    }

    /// <summary>
    /// Initializes watchers to monitor changes on the application data directory and the connections and servers XML files.
    /// </summary>
    public static void StartFileSystemWatchers()
    {
      if (_workbenchAppDataDirWatcher == null)
      {
        // Monitor creation/deletion of the Workbench application data directory
        _workbenchAppDataDirWatcher = Utilities.StartWatcherForFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"MySQL"), OnAppDataDirectoryChanged);
      }

      if (_connectionsFileWatcher == null
          && _serversFileWatcher == null)
      {
        // Create watcher for Workbench servers.xml and connections.xml files
        var wbDirArgs = new FileSystemEventArgs(IsInstalled ? WatcherChangeTypes.Created : WatcherChangeTypes.Deleted, WorkbenchDataDirectory, string.Empty);
        OnAppDataDirectoryChanged(_workbenchAppDataDirWatcher, wbDirArgs);
      }
    }

    /// <summary>
    /// Migrates external connections to the Workbench connections file if identical ones are not present already.
    /// </summary>
    /// <returns>A <see cref="MigrationResult"/> instance with information about the migration.</returns>
    private static MigrationResult MigrateExternalConnections()
    {
      // Add the connections to the Workbench connections file
      foreach (var conn in ExternalConnections.Where(externalConn => externalConn != null))
      {
        // If a connection with the same name is already in the Workbench connections file, check if the connection is identical, if it is not then add it with a suffix.
        var existingConnection = WorkbenchConnections.FirstOrDefault(c => string.Equals(c.Name, conn.Name, StringComparison.InvariantCultureIgnoreCase));
        bool connectionAlreadyExists = existingConnection != null;
        if (connectionAlreadyExists && existingConnection.Equals(conn))
        {
          conn.MigrationStatus = MySqlWorkbenchConnection.MigrationStatusType.AlreadyExists;
          continue;
        }

        bool renamed = false;
        int suffix = 2;
        var newWorkbenchConnection = conn.Clone(true);
        newWorkbenchConnection.SavedStatus = MySqlWorkbenchConnection.SavedStatusType.New;
        while (connectionAlreadyExists && WorkbenchConnections.Any(c => c.Name == newWorkbenchConnection.Name))
        {
          newWorkbenchConnection.Name = $"{conn.Name} ({suffix++})";
          renamed = true;
        }

        conn.MigrationStatus = renamed
          ? MySqlWorkbenchConnection.MigrationStatusType.RenamedAndMigrated
          : MySqlWorkbenchConnection.MigrationStatusType.MigratedAsIs;
        WorkbenchConnections.Add(newWorkbenchConnection);
      }

      string connectionsMigrationError = null;
      bool connectionsSaved = false;
      try
      {
        // Attempt to rename external application's connections file, if we can rename it we proceed with saving the connections in Workbench connections file.
        File.Move(ExternalApplicationConnectionsFilePath, ExternalApplicationConnectionsFilePath + ".bak");

        // Attempt to save the connections in MySQL Workbench.
        connectionsSaved = WorkbenchConnections.Save(true);

        // If the connections file save was successful then delete the renamed connections file, otherwise revert it back.
        if (connectionsSaved)
        {
          File.Delete(ExternalApplicationConnectionsFilePath + ".bak");
          ExternalConnections.CreateFileUponLoadIfNotFound = false;
        }
        else
        {
          File.Move(ExternalApplicationConnectionsFilePath + ".bak", ExternalApplicationConnectionsFilePath);
        }
      }
      catch (Exception ex)
      {
        connectionsMigrationError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        Logger.LogException(ex);
      }

      return new MigrationResult(connectionsSaved, connectionsMigrationError);
    }

    /// <summary>
    /// Fires the <see cref="AppDataDirectoryChanged"/> event.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="args">Event arguments.</param>
    public static void OnAppDataDirectoryChanged(object sender, FileSystemEventArgs args)
    {
      _workbenchDataDirectoryExists = null;
      if (!string.Equals(args.FullPath, WorkbenchDataDirectory, StringComparison.OrdinalIgnoreCase)
          && !string.Equals(args.FullPath, WorkbenchDataDirectory.TrimEnd('\\'), StringComparison.OrdinalIgnoreCase))
      {
        return;
      }

      switch (args.ChangeType)
      {
        case WatcherChangeTypes.Created:
        case WatcherChangeTypes.Renamed:
          if (WorkbenchDataDirectoryExists
              && AllowsExternalConnectionsManagement)
          {
            if (_connectionsFileWatcher == null)
            {
              _connectionsFileWatcher = Utilities.StartWatcherForFile(ConnectionsFilePath, OnConnectionsFileChanged);
            }

            if (_serversFileWatcher == null)
            {
              _serversFileWatcher = Utilities.StartWatcherForFile(ServersFilePath, OnServersFileChanged);
            }
          }
          break;

        case WatcherChangeTypes.Deleted:
          if (_connectionsFileWatcher != null
              && _connectionsFileWatcher.Filter == ConnectionsFilePath)
          {
            _connectionsFileWatcher.Dispose();
            _connectionsFileWatcher = null;
          }

          if (_serversFileWatcher != null)
          {
            _serversFileWatcher.Dispose();
            _serversFileWatcher = null;
          }
          break;
      }

      AppDataDirectoryChanged?.Invoke(sender, args);
    }

    /// <summary>
    /// Fires the <see cref="ConnectionsFileChanged"/> event.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="args">Event arguments.</param>
    private static void OnConnectionsFileChanged(object sender, FileSystemEventArgs args)
    {
      ConnectionsFileChanged?.Invoke(sender, args);
    }

    /// <summary>
    /// Fires the <see cref="ServersFileChanged"/> event.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="args">Event arguments.</param>
    private static void OnServersFileChanged(object sender, FileSystemEventArgs args)
    {
      ServersFileChanged?.Invoke(sender, args);
    }

    /// <summary>
    /// Assembles and shows to the users information about the migration of connections and their related passwords.
    /// </summary>
    /// <param name="connectionsMigrationResult">A <see cref="MigrationResult"/> instance with information about the connections migration.</param>
    /// <param name="passwordsMigrationResult">A <see cref="MigrationResult"/> instance with information about the passwords migration.</param>
    private static void ShowConnectionsMigrationResults(MigrationResult connectionsMigrationResult, MigrationResult passwordsMigrationResult)
    {
      // Prepare the results of the migration to show it to users.
      string infoTitle;
      string infoDetail;
      var moreInfoTextBuilder = new StringBuilder();
      InfoDialog.InfoType infoType;
      bool passwordsMigrated = passwordsMigrationResult == null || passwordsMigrationResult.Success;
      if (connectionsMigrationResult.Success)
      {
        if (passwordsMigrated)
        {
          infoType = InfoDialog.InfoType.Success;
          infoTitle = Resources.ConnectionsMigrationSuccessTitle;
        }
        else
        {
          infoType = InfoDialog.InfoType.Warning;
          infoTitle = Resources.ConnectionsMigrationWarningTitle;
        }

        infoDetail = string.Format(Resources.ConnectionsMigratedDetail, ExternalConnections.Count);
        var migrationStatuses = new[]
        {
          MySqlWorkbenchConnection.MigrationStatusType.MigratedAsIs,
          MySqlWorkbenchConnection.MigrationStatusType.RenamedAndMigrated,
          MySqlWorkbenchConnection.MigrationStatusType.AlreadyExists
        };
        foreach (var migrationStatus in migrationStatuses)
        {
          var migratedConnections = ExternalConnections.Where(c => c.MigrationStatus == migrationStatus).ToList();
          if (migratedConnections.Count == 0)
          {
            continue;
          }

          if (moreInfoTextBuilder.Length > 0)
          {
            moreInfoTextBuilder.Append(Environment.NewLine);
            moreInfoTextBuilder.Append(Environment.NewLine);
          }

          switch (migrationStatus)
          {
            case MySqlWorkbenchConnection.MigrationStatusType.MigratedAsIs:
              moreInfoTextBuilder.AppendFormat(Resources.ConnectionsMigratedSuccessfullyMoreInfo, migratedConnections.Count);
              break;

            case MySqlWorkbenchConnection.MigrationStatusType.RenamedAndMigrated:
              moreInfoTextBuilder.AppendFormat(Resources.ConnectionsMigratedRenamedMoreInfo, migratedConnections.Count);
              break;

            case MySqlWorkbenchConnection.MigrationStatusType.AlreadyExists:
              moreInfoTextBuilder.AppendFormat(Resources.ConnectionsNotMigratedMoreInfo, migratedConnections.Count);
              break;
          }

          moreInfoTextBuilder.Append(Environment.NewLine);
          foreach (var migratedConnection in migratedConnections)
          {
            moreInfoTextBuilder.Append(Environment.NewLine);
            moreInfoTextBuilder.AppendFormat("   * {0}", migratedConnection.Name);
          }
        }

        // Assemble message about migrated passwords.
        if (moreInfoTextBuilder.Length > 0)
        {
          moreInfoTextBuilder.Append(Environment.NewLine);
          moreInfoTextBuilder.Append(Environment.NewLine);
        }

        if (passwordsMigrated)
        {
          moreInfoTextBuilder.Append(Resources.ConnectionPasswordsMigratedText);
        }
        else
        {
          moreInfoTextBuilder.AppendFormat(Resources.ConnectionPasswordsNotMigratedText);
          if (!string.IsNullOrEmpty(passwordsMigrationResult.ErrorMessage))
          {
            moreInfoTextBuilder.Append(Environment.NewLine);
            moreInfoTextBuilder.Append(Environment.NewLine);
            moreInfoTextBuilder.Append(passwordsMigrationResult.ErrorMessage);
          }
        }
      }
      else
      {
        infoType = InfoDialog.InfoType.Error;
        infoTitle = Resources.ConnectionsMigrationErrorTitle;
        infoDetail = Resources.ConnectionsMigrationErrorDetail;
        moreInfoTextBuilder.Append(Resources.ConnectionsMigrationErrorMoreInfo);
        if (!string.IsNullOrEmpty(connectionsMigrationResult.ErrorMessage))
        {
          moreInfoTextBuilder.Append(Environment.NewLine);
          moreInfoTextBuilder.Append(Environment.NewLine);
          moreInfoTextBuilder.Append(connectionsMigrationResult.ErrorMessage);
        }
      }

      // Inform users the results of the migration.
      InfoDialog.ShowDialog(InfoDialogProperties.GetOkDialogProperties(
        infoType,
        infoTitle,
        infoDetail,
        null,
        moreInfoTextBuilder.ToString()));
    }
  }
}
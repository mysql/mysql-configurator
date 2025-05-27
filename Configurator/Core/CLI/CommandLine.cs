/* Copyright (c) 2024, 2025, Oracle and/or its affiliates.

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
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using MySql.Configurator.Base.Classes;
using MySql.Configurator.Base.Enums;
using MySql.Configurator.Core.Ini;
using MySql.Configurator.Core.Logging;
using MySql.Configurator.Core.Server;
using MySql.Configurator.Core.Settings;
using MySql.Configurator.Properties;
using MySql.Configurator.UI.Wizards.ServerConfigPages;
using MySql.Data.MySqlClient;

namespace MySql.Configurator.Core.CLI
{
  // Central class used to control the CLI execution.
  public static class CommandLine
  {
    #region Constants

    /// <summary>
    /// The milliseconds that must pass before the spinner command line indicator is updated.
    /// </summary>
    public const int DEFAULT_SPINNER_TIMER_MILLISECONDS = 250;

    #endregion

    #region Fields

    /// <summary>
    /// The instance representing an existing installation to upgrade.
    /// </summary>
    private static MySqlServerInstance _existingServerInstallationInstance;

    /// <summary>
    /// The timer used to keep track of a running operation.
    /// </summary>
    private static Timer _stepTimer;

    /// <summary>
    /// The spinner command line object used to represent an on-going operation.
    /// </summary>
    private static ConsoleSpinner _spinner;

    #endregion

    /// <summary>
    /// Initializes the static class on its first use.
    /// </summary>
    static CommandLine()
    {
      _spinner = new ConsoleSpinner();
      _stepTimer = new Timer(DEFAULT_SPINNER_TIMER_MILLISECONDS);
      _stepTimer.Elapsed += Timer_Elapsed;
      ResetEvent = new System.Threading.ManualResetEvent(false);
    }

    #region Properties

    /// <summary>
    /// Event used to manually notify of a status change.
    /// </summary>
    public static System.Threading.ManualResetEvent ResetEvent { get; set; }

    #endregion

    /// <summary>
    /// Processes the command line options identified by the parser.
    /// </summary>
    /// <returns>A <see cref="CLIExitCode"/> instance representing the result of the processing of the command line options.</returns>
    public static CLIExitCode ProcessCommandLineOptions(ServerInstallation serverInstallation)
    {
      var showHelp = false;
      PrintStartingText(serverInstallation);

      // Identify if there is a help option.
      if (CommandLineParser.GetMatchingProvidedOption("help") != null)
      {
        showHelp = true;
      }

      // Validate if an action option was provided to know for which action to show the help
      // section or which action to execute.
      var action = CommandLineParser.GetMatchingProvidedOption("action");
      if (showHelp)
      {
        if (action == null)
        {
          PrintGeneralHelp();
        }
        else
        {
          PrintActionHelp(action.Value);
        }

        return new CLIExitCode(ExitCode.Success);
      }
      
      // Remove action option since it will no longer be needed.
      if (action != null)
      {
        CommandLineParser.ProvidedOptions.Remove(action);
      }

      // Check for invalid configuration type when server is not yet configured.
      if (serverInstallation.Controller.ConfigurationType == ConfigurationType.Reconfigure
          && !serverInstallation.Controller.Settings.GeneralSettingsFileExists)
      {
        return new CLIExitCode(ExitCode.ServerNotConfigured);
      }

      // Validate that all provided options are applicable to the specified action.
      var optionsForConfigurationType = GetOptionsForAction(serverInstallation.Controller.ConfigurationType);
      foreach (var option in CommandLineParser.ProvidedOptions)
      {
        if (!optionsForConfigurationType.Any(configTypeOption => configTypeOption.Name.Equals(option.Name, StringComparison.InvariantCultureIgnoreCase)))
        {
          return new CLIExitCode(ExitCode.InvalidOptionForAction, serverInstallation.Controller.ConfigurationType.ToString(), option.Name);
        }
      }

      // Process special options
      var result = ProcessSpecialCommandLineOptions(serverInstallation);
      if (result.ExitCode != ExitCode.Success)
      {
        return result;
      }

      // Next we look for missing required options.
      foreach (var option in optionsForConfigurationType)
      {
        if (option.Required
            && !CommandLineParser.ProvidedOptions.Any(providedOption => providedOption.Name.Equals(option.Name, StringComparison.InvariantCultureIgnoreCase)))
        {
          if (option.Name.Equals("password", StringComparison.InvariantCultureIgnoreCase)
              || option.Name.Equals("old-instance-password", StringComparison.InvariantCultureIgnoreCase))
          {
            // Skip already processed special options.
            continue;
          }

          return new CLIExitCode(ExitCode.MissingRequiredOption, option.Name);
        }
      }

      // Process special cases
      if (serverInstallation.Controller.ConfigurationType == ConfigurationType.Reconfigure)
      {
        if (CommandLineParser.ProvidedOptions.Count == 0)
        {
          return new CLIExitCode(ExitCode.MissingOptionToReconfigure);
        }
      }

      // Set the values in the controller.
      foreach (var option in CommandLineParser.ProvidedOptions)
      {
        // Try to assign the provided value to the option.
        if (!TryToSetValue(serverInstallation.Controller, option.Name, option.Value))
        {
          return new CLIExitCode(ExitCode.InvalidOptionValue, option.Value, option.Name);
        }
      }

      CommandLineParser.ProvidedOptions.Clear();

      // Final validations for specific configuration types.
      if (serverInstallation.Controller.ConfigurationType == ConfigurationType.Reconfigure)
      {
        // Perform a connection check.
        var connectionResult = MySqlServerInstance.CanConnect(serverInstallation.Controller, serverInstallation.Controller.ConfigurationType == ConfigurationType.Reconfigure
          ? serverInstallation.Controller.Settings.ExistingRootPassword
          : serverInstallation.Controller.Settings.RootPassword, true);
        if (connectionResult != ConnectionResultType.ConnectionSuccess)
        {
          return new CLIExitCode(ExitCode.FailedConnectionTestDuringReconfiguration, connectionResult.GetDescription());
        }

        Console.WriteLine(Resources.CLISuccessfulConnection);
      }
      else if (serverInstallation.Controller.ConfigurationType == ConfigurationType.Upgrade)
      {
        var upgradeViableResult = IsUpgradeViable(serverInstallation);
        if (upgradeViableResult.ExitCode != ExitCode.Success)
        {
          return upgradeViableResult;
        }
      }

      // Execute operation.
      if (serverInstallation.Controller.ConfigurationType == ConfigurationType.Remove)
      {
        serverInstallation.Controller.UpdateRemoveSteps();
        if (!serverInstallation.Controller.IsRemovalExecutionNeeded)
        {
          Console.WriteLine(Resources.CLINoConfigurationsFound);
          return new CLIExitCode(ExitCode.Success);
        };
      }
      else
      {
        serverInstallation.Controller.UpdateConfigurationSteps();
      }

      var configurationState = ExecuteOperation(serverInstallation);

      // Return result of operation execution.
      return configurationState == ConfigState.ConfigurationComplete
        || configurationState == ConfigState.ConfigurationCompleteWithWarnings
        ? new CLIExitCode(ExitCode.Success)
        : new CLIExitCode(ExitCode.FailedConfiguration);
    }

    /// <summary>
    /// Determines if there are persisted system variables that have been removed on any version up to the one being configured, and if so sets those variables for resetting.
    /// </summary>
    private static void DetermineExistingServerPersistedVariablesToReset(ServerInstallation serverInstallation)
    {
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
          var removedVariables = serverInstallation.Controller.ServerVersion.GetUnsupportedPersistedServerVariables();
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
          serverInstallation.Controller.PersistedVariablesToReset = persistedVariables;
        }
      }
    }

    private static ConfigState ExecuteOperation(ServerInstallation serverInstallation)
    {
      Console.WriteLine(string.Format(Resources.CLIBeginOperation, serverInstallation.Controller.ConfigurationType.ToString()));
      serverInstallation.Controller.ConfigurationStatusChanged += ConfigurationStatusChanged;
      serverInstallation.Controller.ConfigurationStarted += ConfigurationStarted;
      serverInstallation.Controller.ConfigurationEnded += ConfigurationEnded;
      ResetEvent.Reset();
      if (serverInstallation.Controller.ConfigurationType == ConfigurationType.Remove)
      {
        serverInstallation.Controller.Remove();
      }
      else
      {
        serverInstallation.Controller.Configure();
      }

      ResetEvent.WaitOne();
      serverInstallation.Controller.ConfigurationStatusChanged -= ConfigurationStatusChanged;
      serverInstallation.Controller.ConfigurationStarted -= ConfigurationStarted;
      serverInstallation.Controller.ConfigurationEnded -= ConfigurationEnded;
      Console.WriteLine(string.Format(Resources.CLIEndOperation, serverInstallation.Controller.ConfigurationType.ToString()));

      return serverInstallation.Controller.CurrentState;
    }

    /// <summary>
    /// Gets the valid options supported by the specified action.
    /// </summary>
    /// <param name="action">The action for which to obtain the supported options.</param>
    /// <returns>A list of valid options for the specified action.</returns>
    private static List<CommandLineOption> GetOptionsForAction(ConfigurationType action)
    {
      var options = CommandLineParser.GetServerConfigurableSettings(action);
      if (action == ConfigurationType.Configure)
      {
        options.Add(CommandLineParser.SupportedOptions.First(option => option.Name.Equals("add-user", StringComparison.InvariantCultureIgnoreCase)));
      }

      return options;
    }

    /// <summary>
    /// Validates that the upgrade is viable based on the provided info.
    /// </summary>
    /// <param name="serverInstallation">The server installation.</param>
    /// <returns></returns>
    private static CLIExitCode IsUpgradeViable(ServerInstallation serverInstallation)
    {
      // Check there are running instances.
      var fullInstallDir = Path.GetFullPath(serverInstallation.Controller.Settings.InstallDirectory).TrimEnd('\\');
      var otherServersRunning = Utilities.GetRunningProcessses("mysqld").Where(p => string.Compare(Path.GetDirectoryName(p.MainModule.FileName).TrimEnd('\\'),
                                                                                                                fullInstallDir,
                                                                                                                StringComparison.InvariantCultureIgnoreCase) != 0);

      // If other instances are running, allow to perform an upgrade.
      if (otherServersRunning.Count() == 0)
      {
        return new CLIExitCode(ExitCode.NoRunningInstancesFound);
      }

      // Build existing server installation instance and perform connection check.
      var controller = new ServerConfigurationController();
      controller.ServerInstallation = ServerInstallationManager.LoadGenericServerInstallation();
      controller.Init();
      controller.PrepareForConfigure();
      _existingServerInstallationInstance = new MySqlServerInstance(controller, null, serverInstallation.Controller.Settings.OldInstancePort);
      _existingServerInstallationInstance.UserAccount.Password = serverInstallation.Controller.Settings.OldInstancePassword;
      _existingServerInstallationInstance.Controller.Settings.ExistingRootPassword = _existingServerInstallationInstance.UserAccount.Password;
      _existingServerInstallationInstance.ConnectionProtocol = serverInstallation.Controller.Settings.OldInstanceProtocol;
      _existingServerInstallationInstance.Controller.Settings.Port = serverInstallation.Controller.Settings.OldInstancePort;
      _existingServerInstallationInstance.PipeOrSharedMemoryName = serverInstallation.Controller.Settings.OldInstanceProtocol == MySqlConnectionProtocol.Pipe
        || serverInstallation.Controller.Settings.OldInstanceProtocol == MySqlConnectionProtocol.NamedPipe
        ? serverInstallation.Controller.Settings.OldInstancePipeName
        : serverInstallation.Controller.Settings.OldInstanceProtocol == MySqlConnectionProtocol.Memory
          || serverInstallation.Controller.Settings.OldInstanceProtocol == MySqlConnectionProtocol.SharedMemory
          ? serverInstallation.Controller.Settings.OldInstanceMemoryName
          : null;

      switch (serverInstallation.Controller.Settings.OldInstanceProtocol)
      {
        case MySqlConnectionProtocol.Sockets:
          controller.Settings.Port = serverInstallation.Controller.Settings.OldInstancePort;
          controller.Settings.EnableTcpIp = true;
          break;
        case MySqlConnectionProtocol.Pipe:
          controller.Settings.PipeName = serverInstallation.Controller.Settings.OldInstancePipeName;
          controller.Settings.EnableNamedPipe = true;
          break;
        case MySqlConnectionProtocol.SharedMemory:
          controller.Settings.SharedMemoryName = serverInstallation.Controller.Settings.OldInstanceMemoryName;
          controller.Settings.EnableSharedMemory = true;
          break;
      }

      var connectionResult = _existingServerInstallationInstance.CanConnect();
      if (connectionResult != ConnectionResultType.ConnectionSuccess)
      {
        return new CLIExitCode(ExitCode.FailedConnectionTestDuringReconfiguration, connectionResult.GetDescription());
      }

      Console.WriteLine(Resources.CLISuccessfulConnection);

      // Determine if the upgrade is supported.
      string versionErrorMessage = null;
      var newVersion = serverInstallation.Controller.ServerInstallation.Version;
      var oldVersion = _existingServerInstallationInstance.ServerVersion;
      var upgradeViability = newVersion.ServerSupportsInPlaceUpgrades(oldVersion);
      switch (upgradeViability)
      {
        case UpgradeViability.UnsupportedWithWarning:
          versionErrorMessage = string.Format(Resources.UpgradeNotSupportedWithWarningError, oldVersion, newVersion);
          break;
        case UpgradeViability.Unsupported:
          if (oldVersion.Major < 8)
          {
            versionErrorMessage = Resources.UpgradeOldServerNotSupportedError;
          }
          else if (oldVersion == newVersion)
          {
            versionErrorMessage = Resources.SameVersionError;
          }
          else
          {
            versionErrorMessage = string.Format(Resources.UpgradeNotSupportedError, oldVersion, newVersion);
          }

          break;
      }

      if (!string.IsNullOrEmpty(versionErrorMessage))
      {
        return new CLIExitCode(ExitCode.UnsupportedUpgrade, versionErrorMessage);
      }

      // Validations.
      var configFileIsValid = false;
      var dataDirectory = new DirectoryInfo(_existingServerInstallationInstance.DataDir).Parent.FullName;
      var defaultConfigFile = Path.Combine(dataDirectory, MySqlServerSettings.DEFAULT_CONFIG_FILE_NAME);
      var alternateConfigFile = Path.Combine(dataDirectory, MySqlServerSettings.ALTERNATE_CONFIG_FILE_NAME);
      var configFile = File.Exists(defaultConfigFile)
        ? defaultConfigFile
        : File.Exists(alternateConfigFile)
          ? alternateConfigFile
          : null;
      if (configFile != null)
      {
        var template = new IniTemplate(_existingServerInstallationInstance.BaseDir,
          _existingServerInstallationInstance.DataDir,
          configFile,
          _existingServerInstallationInstance.ServerVersion,
          _existingServerInstallationInstance.Controller.Settings.ServerInstallationType,
          null);

        configFileIsValid = template.IsValid;
      }

      if (!configFileIsValid)
      {
        return new CLIExitCode(ExitCode.InvalidIniFile);
      }

      // Get authentication plugin.
      serverInstallation.Controller.RootUserAuthenticationPlugin = _existingServerInstallationInstance.GetUserAuthenticationPlugin(MySqlServerUser.ROOT_USERNAME);
      if (serverInstallation.Controller.RootUserAuthenticationPlugin == MySqlAuthenticationPluginType.MysqlNativePassword)
      {
        Console.WriteLine(Resources.ServerConfigInvalidAuthenticationPlugin);
      }

      // Set upgrade details.
      var existingServerInstallation = ServerInstallationManager.LoadServerInstallation(_existingServerInstallationInstance.ServerVersion.ToString(), _existingServerInstallationInstance.BaseDir);
      var oldController = existingServerInstallation.Controller;
      oldController.LoadState();
      serverInstallation.Controller.Settings.OldSettings = oldController.Settings;
      serverInstallation.Controller.ConfigurationType = ConfigurationType.Upgrade;
      serverInstallation.Controller.Settings.DataDirectory = dataDirectory;
      serverInstallation.Controller.Settings.IniDirectory = new FileInfo(configFile).DirectoryName;
      serverInstallation.Controller.Settings.ErrorLogFileName = oldController.Settings.ErrorLogFileName;
      _existingServerInstallationInstance.Controller.Settings.InstallDirectory = _existingServerInstallationInstance.BaseDir;
      serverInstallation.Controller.Settings.LoadGeneralSettings(_existingServerInstallationInstance.Controller.Settings.GeneralSettingsFilePath);

      // Check if the error log file is in the default path, and if so update the path to the new version.
      var pathToErrorFile = Path.GetDirectoryName(serverInstallation.Controller.Settings.ErrorLogFileName);
      if (_existingServerInstallationInstance.IsDataDirNameDefault())
      {
        serverInstallation.Controller.Settings.ErrorLogFileName = Regex.Replace(serverInstallation.Controller.Settings.ErrorLogFileName, MySqlServerInstance.DEFAULT_DATADIR_NAME_REGEX, $"MySQL Server {serverInstallation.Controller.ServerVersion.ToString(2)}", RegexOptions.IgnoreCase);
      }

      serverInstallation.Controller.IsRemoveExistingServerInstallationStepNeeded = true;
      serverInstallation.Controller.IsDataDirectoryRenameNeeded = _existingServerInstallationInstance.IsDataDirNameDefault(serverInstallation.Controller.ServerVersion);
      serverInstallation.Controller.ExistingServerInstallationInstance = _existingServerInstallationInstance;

      // Find if existing instance is configured as service.
      var serviceNames = MySqlServiceControlManager.FindServiceNamesWithBaseDirectory(_existingServerInstallationInstance.BaseDir);
      if (serviceNames != null
          && serviceNames.Length > 0)
      {
        _existingServerInstallationInstance.ServiceName = serviceNames[0];
      }

      serverInstallation.Controller.IsServiceRenameNeeded = _existingServerInstallationInstance.IsServiceNameDefault(
        _existingServerInstallationInstance.ServiceName,
        _existingServerInstallationInstance.ServerVersion);
      DetermineExistingServerPersistedVariablesToReset(serverInstallation);

      // Set existing instance relevant properties for rollback.
      _existingServerInstallationInstance.Controller.ServerVersion = _existingServerInstallationInstance.ServerVersion;
      _existingServerInstallationInstance.Controller.ServerInstallation.VersionString = _existingServerInstallationInstance.Controller.ServerVersion.ToString();
      if (!string.IsNullOrEmpty(_existingServerInstallationInstance.BaseDir))
      {
        if (serviceNames.Length > 0)
        {
          _existingServerInstallationInstance.Controller.Settings.ServiceName = serviceNames[0];
          _existingServerInstallationInstance.Controller.Settings.ConfigureAsService = true;
        }

        var iniFile = new IniFileEngine(configFile).Load();
        _existingServerInstallationInstance.Controller.Settings.DataDirectory = new DirectoryInfo(iniFile.FindValue("mysqld", "datadir", false)).Parent.FullName;
        var errorLogFileName = iniFile.FindValue("mysqld", "log-error", false);
        _existingServerInstallationInstance.Controller.Settings.ErrorLogFileName = string.IsNullOrEmpty(errorLogFileName)
          ? MySqlServerSettings.ErrorLogDefaultFileName
          : errorLogFileName;
      }

      return new CLIExitCode(ExitCode.Success);
    }

    /// <summary>
    /// Prints the help section for the specified action.
    /// </summary>
    /// <param name="actionName">The action for which to show the help section.</param>
    private static void PrintActionHelp(string actionName)
    {
      if (!Enum.TryParse(actionName, true, out ConfigurationType actionType))
      {
        return;
      }

      if (actionType != ConfigurationType.Configure
          && actionType != ConfigurationType.Reconfigure
          && actionType != ConfigurationType.Upgrade
          && actionType != ConfigurationType.Remove)
      {
        return;
      }

      var actionHelp = string.Empty;
      switch (actionType)
      {
        case ConfigurationType.Configure:
          actionHelp = Resources.CLIConfigureActionHelp;
          break;
        case ConfigurationType.Reconfigure:
          actionHelp = Resources.CLIReconfigureActionHelp;
          break;
        case ConfigurationType.Upgrade:
          actionHelp = Resources.CLIUpgradeActionHelp;
          break;
        case ConfigurationType.Remove:
          actionHelp = Resources.CLIRemoveActionHelp;
          break;
      }

      // Generate action options section.
      var builder = new StringBuilder();
      var options = GetOptionsForAction(actionType);
      foreach (var option in options)
      {
        string aliases = "N/A";
        string shortcut = "N/A";
        string supportedValues = "N/A";
        if (!string.IsNullOrEmpty(option.Shortcut))
        {
          shortcut = option.Shortcut;
        }

        if (option.Aliases != null
            && option.Aliases.Length > 0)
        {
          aliases = string.Join(",", option.Aliases);
        }

        if (option.SupportedValues != null
            && option.SupportedValues.Length > 0)
        {
          supportedValues = string.Join(",", option.SupportedValues);
        }

        builder.AppendLine($"- {option.Name}:{shortcut}:{aliases}:{supportedValues}:{option.Description}");
      }

      actionHelp = actionHelp.Replace("[options]", builder.ToString());
      
      // Print action help.
      Console.WriteLine(actionHelp);
    }

    /// <summary>
    /// Prints the general help section.
    /// </summary>
    private static void PrintGeneralHelp()
    {
      Console.WriteLine(Resources.CLIGeneralHelp);
    }

    /// <summary>
    /// Prints the CLI starting text.
    /// </summary>
    private static void PrintStartingText(ServerInstallation serverInstallation)
    {
      var serverIsConfigured = serverInstallation.Controller.Settings.GeneralSettingsFileExists;
      var builder = new StringBuilder();
      builder.AppendLine();
      builder.AppendLine();
      builder.AppendLine(Resources.CLICopyrightNotice);
      builder.AppendLine();
      builder.AppendLine("-------------------------------------------------------------------------------------");
      builder.AppendLine();
      builder.AppendLine($"MySQL Configurator {AppConfiguration.Version.ToString()}");
      builder.AppendLine($"License: {AppConfiguration.License.ToString()}");
      builder.AppendLine($"Execution mode: {AppConfiguration.ExecutionMode}");
      builder.AppendLine($"Configuration state: {(serverIsConfigured ? "Server is configured" : "Server is not configured")}");
      if (serverIsConfigured)
      {
        builder.AppendLine($"Ini file: {serverInstallation.Controller.Settings.FullConfigFilePath}");
        builder.AppendLine($"Data directory: {serverInstallation.Controller.DataDirectory}");
      }

      builder.AppendLine();
      builder.AppendLine("-------------------------------------------------------------------------------------");
      builder.AppendLine();
      Console.WriteLine(builder.ToString());
    }

    /// <summary>
    /// Processes an add user command line option.
    /// </summary>
    /// <param name="option">The option to process.</param>
    /// <returns>A <see cref="CLIExitCode"/> instance representing the result of the processing of the user option.</returns>
    private static CLIExitCode ProcessAddUserOption(CommandLineOption option, ServerInstallation serverInstallation)
    {
      var result = CommandLineParser.ParseAddUserOption(option.Value, out string[] serverUserItems);
      if (result.ExitCode != ExitCode.Success)
      {
        return result;
      }

      // Process user name.
      var userName = serverUserItems[0].Substring(1, serverUserItems[0].Length - 2);
      var message = MySqlServerInstance.ValidateUserName(userName, false);
      if (!string.IsNullOrEmpty(message))
      {
        return new CLIExitCode(ExitCode.InvalidCustomUserName, userName, CommandLineParser.ADD_USER_OPTION_NAME, message);
      }

      // Process password/Windows Security Token.
      bool mysqlAuthentication = serverUserItems[4].Equals("MYSQL", StringComparison.InvariantCultureIgnoreCase);
      var passwordOrSecurityToken = serverUserItems[1].Substring(1, serverUserItems[1].Length - 2);
      if (mysqlAuthentication)
      {
        message = MySqlServerInstance.ValidatePassword(passwordOrSecurityToken, true);
        if (!string.IsNullOrEmpty(message))
        {
          return new CLIExitCode(ExitCode.InvalidCustomUserPassword, CommandLineParser.ADD_USER_OPTION_NAME, message);
        }
      }
      else
      {
        char[] validSeparators = { ';', ' ', ',' };
        string[] winAuthTokens = passwordOrSecurityToken.Trim().Split(validSeparators);
        foreach (string possibleToken in winAuthTokens)
        {
          bool tokenExists;
          if (possibleToken == string.Empty)
          {
            continue;
          }

          try
          {
            tokenExists = DirectoryServicesWrapper.TokenExists(possibleToken);
            if (!tokenExists)
            {
              return new CLIExitCode(ExitCode.CustomUserSecurityTokenNotFound, possibleToken, CommandLineParser.ADD_USER_OPTION_NAME, message);
            }
          }
          catch (Exception ex)
          {
            tokenExists = false;
            // Attempting to query the Active Directory may raise an error with the "Unspecified error" message
            // which can indicate different issues, in this case a more user friendly error message is required
            var exceptionMessage = ex.Message == Resources.ServerConfigUnspecifiedError
              ? Resources.ServerConfigUserFriendlyUnspecifiedError
              : ex.Message;
            Logger.LogError($"- {possibleToken}: {exceptionMessage}");
          }
        }
      }
      
      // Process host.
      var host = serverUserItems[2].ToLower();
      if (userName.Equals(MySqlServerUser.ROOT_USERNAME, StringComparison.OrdinalIgnoreCase)
          && (host == MySqlServerUser.LOCALHOST
              || host == "::1"
              || host == "127.0.0.1"))
      {
        return new CLIExitCode(ExitCode.InvalidCustomUserRootUser, CommandLineParser.ADD_USER_OPTION_NAME);
      }

      // Process user role.
      var roleString = serverUserItems[3].Substring(1, serverUserItems[3].Length - 2);
      var role = serverInstallation.Controller.RolesDefined.Roles.Find(name => name.ID.Equals(roleString, StringComparison.InvariantCultureIgnoreCase)
                                                                               || name.Display.Equals(roleString, StringComparison.InvariantCultureIgnoreCase));
      if (role == null)
      {
        return new CLIExitCode(ExitCode.InvalidCustomUserRole, roleString, CommandLineParser.ADD_USER_OPTION_NAME);
      }

      // Add user instance to list.
      var user = new MySqlServerUser()
      {
        Username = userName,
        AuthenticationPlugin = serverInstallation.Controller.Settings.DefaultAuthenticationPlugin,
        Host = host,
        UserRole = role
      };
      if (mysqlAuthentication)
      {
        user.Password = passwordOrSecurityToken;
      }
      else
      {
        user.WindowsSecurityTokenList = passwordOrSecurityToken;
      }

      // Validate not repeated user.
      if (!serverInstallation.Controller.Settings.NewServerUsers.Any(existingUser => existingUser.Username.Equals(user.Username, StringComparison.InvariantCultureIgnoreCase)
                                                                                    && existingUser.Host.Equals(user.Host, StringComparison.InvariantCultureIgnoreCase)))
      {
        serverInstallation.Controller.Settings.NewServerUsers.Add(user);
      }
      else
      {
        var warningMessage = string.Format(Resources.CLIDuplicateUserWarning, user.Username, user.Host);
        Logger.LogWarning(warningMessage);
        Console.WriteLine(warningMessage);
      }
      
      return new CLIExitCode(ExitCode.Success);
    }

    /// <summary>
    /// Processes the password during a configuration operation by obtaining from supported options/locations.
    /// </summary>
    /// <param name="passwordOptionName">The name of the password option based on the configuration type.</param>
    /// <param name="passwordFileOptionName">The name of the option that specifies the file from where the password can be obtained.</param>
    /// <param name="environmentVariable">The name of the enviroment variable that contains the password if it couldn't be retrieved from anywhere else.</param>
    /// <param name="serverInstallation">The server installation for which the password will be retrieved.</param>
    /// <returns>A <see cref="CLIExitCode"/> instance representing the result of the processing of the password option.</returns>
    private static CLIExitCode ProcessPasswordOption(string passwordOptionName, string passwordFileOptionName, string environmentVariable, ServerInstallation serverInstallation)
    {
      var passwordOption = CommandLineParser.GetMatchingProvidedOption(passwordOptionName);
      var passwordFileOption = CommandLineParser.GetMatchingProvidedOption(passwordFileOptionName);
      if (passwordOption != null)
      {
        if (serverInstallation.Controller.ConfigurationType == ConfigurationType.Configure)
        {
          var errorMessage = MySqlServerInstance.ValidatePassword(passwordOption.Value, true);
          if (!string.IsNullOrEmpty(errorMessage))
          {
            return new CLIExitCode(ExitCode.RootPasswordInvalidFormat, errorMessage);
          }
        }

        if (!TryToSetValue(serverInstallation.Controller, passwordOption.Name, passwordOption.Value))
        {
          return new CLIExitCode(ExitCode.InvalidOptionValue, passwordOption.Value, passwordOption.Name);
        }

        if (serverInstallation.Controller.ConfigurationType == ConfigurationType.Reconfigure)
        {
          serverInstallation.Controller.Settings.ExistingRootPassword = passwordOption.Value;
        }
        
        serverInstallation.Controller.Settings.RootPassword = passwordOption.Value;
        CommandLineParser.ProvidedOptions.Remove(passwordOption);
      }
      else if (passwordFileOption != null)
      {
        // Read password from file.
        if (string.IsNullOrEmpty(passwordFileOption.Value))
        {
          return new CLIExitCode(ExitCode.OptionValueNotFound, passwordFileOption.Name);
        }

        // Validate the file exists.
        if (!File.Exists(passwordFileOption.Value))
        {
          return new CLIExitCode(ExitCode.PasswordFileDoesNotExist, passwordFileOption.Value);
        }

        // Read password from file.
        var passwordValuePair = Utilities.ReadTextFile(passwordFileOption.Value, out string errorMessage);
        if (!string.IsNullOrEmpty(errorMessage))
        {
          return new CLIExitCode(ExitCode.ErrorReadingPasswordFile, passwordFileOption.Value, errorMessage);
        }

        var items = passwordValuePair.Split('=');
        string password = null;
        if (items.Length != 2
            || !items[0].Equals("password", StringComparison.CurrentCultureIgnoreCase)
            || string.IsNullOrEmpty(items[1]))
        {
          return new CLIExitCode(ExitCode.PasswordFileInvalidContents, passwordFileOption.Value);
        }
        else
        {
          password = items[1];
        }

        // Set password.
        if (!TryToSetValue(serverInstallation.Controller, passwordOptionName, password))
        {
          return new CLIExitCode(ExitCode.InvalidOptionValue, passwordOptionName, password);
        }

        CommandLineParser.ProvidedOptions.Remove(passwordFileOption);
      }
      else
      {
        // Read from environment variable.
        string password = Environment.GetEnvironmentVariable(environmentVariable);

        if (!string.IsNullOrEmpty(password)
            && !TryToSetValue(serverInstallation.Controller, passwordOptionName, password))
        {
          return new CLIExitCode(ExitCode.InvalidOptionValue, passwordOptionName, password);
        }

        // If password could not be obtained raise error if it is a new configuration.
        if (string.IsNullOrEmpty(password)
            && serverInstallation.Controller.ConfigurationType == ConfigurationType.Configure)
        {
          return new CLIExitCode(ExitCode.OptionValueNotFound, passwordOptionName);
        }
      }

      return new CLIExitCode(ExitCode.Success);
    }

    /// <summary>
    /// Processes the command line options that require additional processing.
    /// </summary>
    /// <param name="serverInstallation">The server installation.</param>
    /// <returns>A <see cref="CLIExitCode"/> instance representing the result of the processing of the special command line options.</returns>
    private static CLIExitCode ProcessSpecialCommandLineOptions(ServerInstallation serverInstallation)
    {
      // Upgrade configuration type special options.
      if (serverInstallation.Controller.ConfigurationType == ConfigurationType.Upgrade)
      {
        // Old Instance Password and defaults extra file.
        var passwordOptionProcessingResult = ProcessPasswordOption("old-instance-password", "defaults-extra-file", "MYSQL_PWD", serverInstallation);
        if (passwordOptionProcessingResult.ExitCode != ExitCode.Success)
        {
          return passwordOptionProcessingResult;
        }

        // Existing instance related options.
        var oldInstanceProtocol = CommandLineParser.GetMatchingProvidedOption("old-instance-protocol");
        MySqlConnectionProtocol connectionProtocol;
        if (oldInstanceProtocol != null)
        {
          if (!Enum.TryParse(oldInstanceProtocol.Value, out connectionProtocol))
          {
            return new CLIExitCode(ExitCode.InvalidOptionValue, oldInstanceProtocol.Value, oldInstanceProtocol.Name);
          }

          TryToSetValue(serverInstallation.Controller, oldInstanceProtocol.Name, oldInstanceProtocol.Value);
          CommandLineParser.ProvidedOptions.Remove(oldInstanceProtocol);
          CommandLineOption requiredProtocolOption = null;
          switch (connectionProtocol)
          {
            case MySqlConnectionProtocol.Tcp:
              requiredProtocolOption = CommandLineParser.GetMatchingProvidedOption("old-instance-port");
              if (requiredProtocolOption == null)
              {
                requiredProtocolOption = CommandLineParser.GetMatchingSupportedOption("old-instance-port");
                requiredProtocolOption.Value = MySqlServerSettings.DEFAULT_PORT.ToString();
              }

              break;
            case MySqlConnectionProtocol.Pipe:
              requiredProtocolOption = CommandLineParser.GetMatchingProvidedOption("old-instance-pipe-name");
              if (requiredProtocolOption == null)
              {
                requiredProtocolOption = CommandLineParser.GetMatchingSupportedOption("old-instance-pipe-name");
                requiredProtocolOption.Value = MySqlServerSettings.DEFAULT_PIPE_OR_SHARED_MEMORY_NAME.ToString();
              }

              break;
            case MySqlConnectionProtocol.SharedMemory:
              requiredProtocolOption = CommandLineParser.GetMatchingProvidedOption("old-instance-memory-name");
              if (requiredProtocolOption == null)
              {
                requiredProtocolOption = CommandLineParser.GetMatchingSupportedOption("old-instance-memory-name");
                requiredProtocolOption.Value = MySqlServerSettings.DEFAULT_PIPE_OR_SHARED_MEMORY_NAME.ToString();
              }

              break;
          }

          if (requiredProtocolOption == null)
          {
            return new CLIExitCode(ExitCode.MissingRequiredOption, requiredProtocolOption.Name);
          }

          if (!TryToSetValue(serverInstallation.Controller, requiredProtocolOption.Name, requiredProtocolOption.Value))
          {
            return new CLIExitCode(ExitCode.InvalidOptionValue, requiredProtocolOption.Value, requiredProtocolOption.Name);
          }

          CommandLineParser.ProvidedOptions.Remove(requiredProtocolOption);
        }
        else
        {
          oldInstanceProtocol = CommandLineParser.GetMatchingSupportedOption("old-instance-protocol");
          oldInstanceProtocol.Value = MySqlConnectionProtocol.Socket.ToString();
          TryToSetValue(serverInstallation.Controller, oldInstanceProtocol.Name, oldInstanceProtocol.Value);
          var portOption = CommandLineParser.GetMatchingProvidedOption("old-instance-port");
          portOption = CommandLineParser.GetMatchingSupportedOption("old-instance-port");
          portOption.Value = MySqlServerSettings.DEFAULT_PORT.ToString();
          TryToSetValue(serverInstallation.Controller, portOption.Name, portOption.Value);
        }
      }

      // Configure and reconfigure configuration types special options.
      if (serverInstallation.Controller.ConfigurationType == ConfigurationType.Configure
          || serverInstallation.Controller.ConfigurationType == ConfigurationType.Reconfigure)
      {
        // Root password.
        var passwordOptionProcessingResult = ProcessPasswordOption("password", "defaults-extra-file", "MYSQL_PWD", serverInstallation);
        if (passwordOptionProcessingResult.ExitCode != ExitCode.Success)
        {
          return passwordOptionProcessingResult;
        }

        // Windows service account password
        var winAccountUserOption = CommandLineParser.GetMatchingProvidedOption("windows-service-user");
        if (winAccountUserOption != null)
        {
          var winPasswordOptionProcessingResult = ProcessPasswordOption("windows-service-password", "windows-service-account-password-file", "WIN_SERVICE_ACCOUNT_PWD", serverInstallation);
          if (winPasswordOptionProcessingResult.ExitCode != ExitCode.Success)
          {
            return winPasswordOptionProcessingResult;
          }
        }

        // Install and uninstall sample databases.
        serverInstallation.Controller.ExampleDatabasesStatus = new Dictionary<string, string>();
        var installSampleDbOption = CommandLineParser.GetMatchingProvidedOption("install-sample-database");
        if (installSampleDbOption != null)
        {
          if (!Enum.TryParse(installSampleDbOption.Value, out ExampleDatabase installStatus))
          {
            return new CLIExitCode(ExitCode.InvalidOptionValue, installSampleDbOption.Value, installSampleDbOption.Name);
          }

          serverInstallation.Controller.IsCreateRemoveExamplesDatabasesStepNeeded = true;
          switch (installStatus)
          {
            case ExampleDatabase.All:
              serverInstallation.Controller.ExampleDatabasesStatus.Add("sakila", "create");
              serverInstallation.Controller.ExampleDatabasesStatus.Add("world", "create");
              break;
            case ExampleDatabase.Sakila:
              serverInstallation.Controller.ExampleDatabasesStatus.Add("sakila", "create");
              break;
            case ExampleDatabase.World:
              serverInstallation.Controller.ExampleDatabasesStatus.Add("world", "create");
              break;
          }

          CommandLineParser.ProvidedOptions.Remove(installSampleDbOption);
        }

        var uninstallSampleDbOption = CommandLineParser.GetMatchingProvidedOption("uninstall-sample-database");
        if (uninstallSampleDbOption != null)
        {
          if (!Enum.TryParse(uninstallSampleDbOption.Value, out ExampleDatabase uninstallStatus))
          {
            return new CLIExitCode(ExitCode.InvalidOptionValue, uninstallSampleDbOption.Value, uninstallSampleDbOption.Name);
          }

          switch (uninstallStatus)
          {
            case ExampleDatabase.All:
              serverInstallation.Controller.ExampleDatabasesStatus.Add("sakila", "remove");
              serverInstallation.Controller.ExampleDatabasesStatus.Add("world", "remove");
              break;
            case ExampleDatabase.Sakila:
              serverInstallation.Controller.ExampleDatabasesStatus.Add("sakila", "remove");
              break;
            case ExampleDatabase.World:
              serverInstallation.Controller.ExampleDatabasesStatus.Add("world", "remove");
              break;
          }

          CommandLineParser.ProvidedOptions.Remove(uninstallSampleDbOption);
        }

        // Configure configuration type special options.
        if (serverInstallation.Controller.ConfigurationType == ConfigurationType.Configure)
        {
          serverInstallation.Controller.Settings.NewServerUsers.Clear();
          while (CommandLineParser.ProvidedOptions.Any(option => option.Name.Equals("add-user", StringComparison.InvariantCultureIgnoreCase)))
          {
            var addUserOption = CommandLineParser.GetMatchingProvidedOption("add-user");
            var result = ProcessAddUserOption(addUserOption, serverInstallation);
            if (result.ExitCode != ExitCode.Success)
            {
              return result;
            }

            CommandLineParser.ProvidedOptions.Remove(addUserOption);
          }

          // Enable windows authentication plugin if needed.
          if (serverInstallation.Controller.Settings.NewServerUsers.Any(user => user.AuthenticationPlugin == MySqlAuthenticationPluginType.Windows))
          {
            serverInstallation.Controller.Settings.Plugins.Enable("authentication_windows", true);
          }
        }
      }

      return new CLIExitCode(ExitCode.Success);
    }

    /// <summary>
    /// Attempts to set the property corresponding to the specified CLI option.
    /// </summary>
    /// <param name="controller">The server controller containing the property to set.</param>
    /// <param name="option">The option name.</param>
    /// <param name="value">The option value.</param>
    /// <returns><c>true</c> if the option value could be set; otherwise, <c>false</c>.</returns>
    private static bool TryToSetValue(ServerConfigurationController controller, string option, string value)
    {
      // TODO: Handle special cases with a switch statement.
      // E.g. properties which don't have a specific property but instead need some basic processing.

      if (controller.SetConfigurationValue(new[] { option, value }, out var message))
      {
        return true;
      }

      if (AppConfiguration.ConsoleMode)
      {
        Console.WriteLine($@"{message}");
      }

      Logger.LogError($@"{message}");
      return false;
    } 

    #region Event Delegates

    /// <summary>
    /// Event delegate used to notify of a status change.
    /// </summary>
    /// <param name="sender">The sender object.</param>
    /// <param name="type">The configuration type.</param>
    /// <param name="message">The message to notify.</param>
    private static void ConfigurationStatusChanged(object sender, ConfigurationEventType type, string message)
    {
      var controller = sender as ServerConfigurationController;
      if (controller == null)
      {
        return;
      }

      switch (type)
      {
        case ConfigurationEventType.Info:
        case ConfigurationEventType.Waiting:
          Console.WriteLine(message);
          break;

        case ConfigurationEventType.StepStarting:
          Console.WriteLine($"Step: {controller.CurrentStep.Description}");
          break;

        case ConfigurationEventType.StepFinished:
          Console.WriteLine(controller.CurrentStep.Status == ConfigurationStepStatus.Finished
            ? Resources.CLIConfigurationStepCompleted
            : Resources.CLIConfigurationStepFailed);
          Console.WriteLine();
          Console.WriteLine();
          break;
      }
    }

    /// <summary>
    /// Event delegate used to notify that the configuration operation has ended.
    /// </summary>
    /// <param name="sender">The sender object.</param>
    /// <param name="e">The event arguments.</param>
    private static void ConfigurationEnded(object sender, EventArgs e)
    {
      var controller = sender as ServerConfigurationController;
      if (controller == null)
      {
        return;
      }

      _stepTimer.Enabled = false;
      Console.Write(string.Format(Resources.CLIOperationCompleted, controller.ConfigurationType.ToString(), $"MySQL Server {controller.ServerVersion.ToString()}"));
      switch (controller.CurrentState)
      {
        case ConfigState.ConfigurationComplete:
          Console.WriteLine(Resources.CLIOperationCompletedSuccessfully);
          break;
        case ConfigState.ConfigurationCompleteWithWarnings:
          Console.WriteLine(Resources.CLIOperationCompletedWithWarnings);
          break;
        case ConfigState.ConfigurationError:
          Console.WriteLine(Resources.CLIOperationCompletedWithErrors);
          break;
      }

      ResetEvent.Set();
    }

    /// <summary>
    /// Event delegate used to notify that the configuration operation has started.
    /// </summary>
    /// <param name="sender">The sender object.</param>
    /// <param name="e">The event arguments.</param>
    private static void ConfigurationStarted(object sender, EventArgs e)
    {
      var controller = sender as ServerConfigurationController;
      if (controller == null)
      {
        return;
      }

      Console.WriteLine();
      Console.WriteLine(string.Format(Resources.CLIConfigurationStarting, $"MySQL Server {controller.ServerVersion.ToString()}"));
      Console.WriteLine();
      Console.WriteLine();
      _stepTimer.Enabled = true;
    }

    /// <summary>
    /// Event delegate used to notify that the timer has elapsed.
    /// </summary>
    /// <param name="sender">The sender object.</param>
    /// <param name="e">The event arguments.</param>
    private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
    {
      _spinner.Turn();
    }

    #endregion
  }
}

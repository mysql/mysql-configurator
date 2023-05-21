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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Controllers;
using MySql.Configurator.Core.Enums;
using IniFile = MySql.Configurator.Core.IniFile.IniFile;
using TimeoutException = System.TimeoutException;
using MySql.Configurator.Core.Classes.MySql;
using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Properties;

namespace MySql.Configurator.Wizards.Server
{
  /// <summary>
  /// Contains information about a MySQL Server instance running locally in the localhost.
  /// </summary>
  public class LocalServerInstance : MySqlServerInstance
  {
    #region Constants

    /// <summary>
    /// The default maximum number of retries for a connection attempt.
    /// </summary>
    public const int DEFAULT_MAX_CONNECTION_RETRIES = 10;

    #endregion Constants

    #region Fields

    /// <summary>
    /// The <see cref="ServerConfigurationController"/> related to this instance.
    /// </summary>
    private ServerConfigurationController _controller;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <seealso cref="LocalServerInstance"/> class.
    /// </summary>
    /// <param name="type">The <seealso cref="InnoDbClusterType"/> this instance was configured as.</param>
    /// <param name="controller">The <see cref="ServerConfigurationController"/> related to this instance.</param>
    /// <param name="reportStatusDelegate">An <seealso cref="System.Action"/> to output status messages.</param>
    /// <param name="port">The port where this instance listens for connections.</param>
    public LocalServerInstance(ServerConfigurationController controller, Action<string> reportStatusDelegate = null, uint port = MySqlServerInstance.DEFAULT_PORT)
      : base(port, reportStatusDelegate)
    {
      _controller = controller ?? throw new ArgumentNullException(nameof(controller));
      MaxConnectionRetries = DEFAULT_MAX_CONNECTION_RETRIES;
      Type = ServerConfigurationType.StandAlone;
      ParseErrorLogForAcceptingConnections = true;
      Port = _controller.Settings.Port;
      UserAccount = MySqlServerUser.GetLocalRootUser(_controller.Settings.RootPassword, _controller.Settings.DefaultAuthenticationPlugin);
      UseOldSettings = false;
      WaitUntilAcceptingConnections = true;
    }

    #region Properties

    /// <summary>
    /// Gets the architecture of this server instance.
    /// </summary>
    public PackageArchitecture Architecture => _controller?.Package.Architecture ?? PackageArchitecture.Unknown;

    /// <summary>
    /// Gets the password used to configure this instance.
    /// </summary>
    public string ConfigurationRootPassword => _controller.Settings.ExistingRootPassword;

    /// <summary>
    /// Gets a value indicating whether this instance is running and connections can be made.
    /// </summary>
    public bool IsRunning => RunningProcess != null;

    /// <summary>
    /// Gets or sets the maximum number of retries to perform with the connection.
    /// </summary>
    public int MaxConnectionRetries { get; set; }

    /// <summary>
    /// Gets the name of this instance containing its Server version.
    /// </summary>
    public override string NameWithVersion => _controller.Package.NameWithVersion;

    /// <summary>
    /// Gets or sets a value indicating whether the instance was started manually and needs to be stopped later.
    /// </summary>
    public bool NeedsToBeStopped { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to determine if the server is accepting connections after starting it by parsing its error log, or by attempting connecting to it.
    /// </summary>
    public bool ParseErrorLogForAcceptingConnections { get; set; }

    /// <summary>
    /// Gets the ID of the process associated with this Server instance.
    /// </summary>
    public int ProcessId => Core.Classes.Utilities.GetServerInstanceProcessId(_controller.DataDirectory);

    /// <summary>
    /// Gets a running <seealso cref="Process"/> associated with this Server instance.
    /// </summary>
    public Process RunningProcess
    {
      get
      {
        var processId = ProcessId;
        return processId > 0 ? Core.Classes.Utilities.GetRunningProcess(processId) : null;
      }
    }

    /// <summary>
    /// Gets the full file path for the Server configuration file.
    /// </summary>
    public string ServerConfigFilePath => _controller.Settings.FullConfigFilePath;

    /// <summary>
    /// Gets the full file path for the Server executable.
    /// </summary>
    public string ServerExecutableFilePath => _controller.ServerExecutableFilePath;

    /// <summary>
    /// Gets the Server ID of this instance.
    /// </summary>
    public override uint ServerId
    {
      get
      {
        return _controller.Settings.ServerId ?? 1;
      }
    }

    /// <summary>
    /// Gets the <seealso cref="ServerConfigurationType"/> this instance was configured as.
    /// </summary>
    public ServerConfigurationType Type { get; }

    /// <summary>
    /// Gets a description of the <seealso cref="Type"/>.
    /// </summary>
    public string TypeDescription
    {
      get
      {
        return Type.GetDescription();
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether settings before current configuration changes are used, otherwise the settings after configuration changes.
    /// </summary>
    public bool UseOldSettings { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the method keeps trying to connect until successful or the given maximum number of retries is reached.
    /// </summary>
    public bool WaitUntilAcceptingConnections { get; set; }

    #endregion Properties

    /// <summary>
    /// Checks if a connection to the server can be established.
    /// </summary>
    /// <param name="controller">The <see cref="ServerConfigurationController"/> related to this instance.</param>
    /// <param name="passwordOverride">A password to use instead of the one in the configuration settings.</param>
    /// <param name="useOldSettings">Flag indicating whether settings before current configuration changes are used, otherwise the settings after configuration changes.</param>
    /// <returns>A <see cref="ConnectionResultType"/> value.</returns>
    public static ConnectionResultType CanConnect(ServerConfigurationController controller, string passwordOverride = null, bool useOldSettings = false)
    {
      if (controller == null)
      {
        throw new ArgumentNullException(nameof(controller));
      }

      var serverInstance = new LocalServerInstance(controller)
      {
        UseOldSettings = useOldSettings
      };

      var settings = useOldSettings
                     ? controller.OldSettings
                     : controller.Settings;
      serverInstance.ConnectionProtocol = settings.EnableTcpIp
                                            ? MySqlConnectionProtocol.Tcp
                                            : settings.EnableSharedMemory
                                              ? MySqlConnectionProtocol.SharedMemory
                                              : MySqlConnectionProtocol.NamedPipe;
      serverInstance.AllowPublicKeyRetrieval = settings.IsNamedPipeTheOnlyEnabledProtocol;
      if (settings.IsNamedPipeTheOnlyEnabledProtocol)
      {
        serverInstance.PipeOrSharedMemoryName = settings.PipeName;
        serverInstance.SslMode = MySqlSslMode.Disabled;
      }

      if (passwordOverride != null)
      {
        serverInstance.UserAccount.Password = passwordOverride;
      }

      if (useOldSettings)
      {
        serverInstance.UserAccount.AuthenticationPlugin = controller.DefaultAuthenticationPluginChanged
        ? controller.OldSettings.DefaultAuthenticationPlugin
        : controller.Settings.DefaultAuthenticationPlugin;
      }

      var isInitiallyRunning = serverInstance.IsRunning;
      if (!isInitiallyRunning)
      {
        var additionalOptions = controller.IsStartAndUpgradeConfigurationStepNeeded
          ? "--upgrade=MINIMAL"
          : null;
        if (!serverInstance.StartInstanceAsProcess(additionalOptions))
        {
          return ConnectionResultType.HostNotRunning;
        }
      }

      var connectionResult = serverInstance.CanConnect();
      if (!isInitiallyRunning)
      {
        serverInstance.KillInstanceProcess();
      }

      return connectionResult;
    }

    /// <summary>
    /// Checks if a connection to the server can be established.
    /// </summary>
    /// <param name="controller">The <see cref="ServerConfigurationController"/> related to this instance.</param>
    /// <param name="errorMessage">A custom error message for specific cases.</param>
    /// <param name="passwordOverride">A password to use instead of the one in the configuration settings.</param>
    /// <param name="useOldSettings">Flag indicating whether settings before current configuration changes are used, otherwise the settings after configuration changes.</param>
    /// <param name="checkDataDirectoryInUse">Flag indicating whether to check that the error was caused by the data directory already being used by another process.</param>
    /// <returns>A <see cref="ConnectionResultType"/> value.</returns>
    public static ConnectionResultType CanConnect(ServerConfigurationController controller, out string errorMessage, string passwordOverride = null, bool useOldSettings = false, bool checkDataDirectoryInUse = false)
    {
      errorMessage = null;
      var mySqlErrorLog = new ServerErrorLog(controller.ErrorLogFilePath);
      var connectionResult = CanConnect(controller, passwordOverride, useOldSettings);
      if (!checkDataDirectoryInUse
          || connectionResult != ConnectionResultType.HostNotRunning)
      {
        return connectionResult;
      }

      errorMessage = GetDataDirectoryInUseErrorMessage(mySqlErrorLog, controller);
      return connectionResult;
    }

    /// <summary>
    /// Checks if a connection to this instance can be established with the credentials in <see cref="MySqlServerInstance.UserAccount"/>.
    /// </summary>
    /// <returns>A <see cref="ConnectionResultType"/> value.</returns>
    public override ConnectionResultType CanConnect()
    {
      return !IsRunning
        ? ConnectionResultType.HostNotRunning
        : base.CanConnect();
    }

    /// <summary>
    /// Executes the given SQL scripts connecting to this instance.
    /// </summary>
    /// <param name="outputScriptToStatus">Flag indicating whether feedback about the scripts being executed is sent to the output.</param>
    /// <param name="sqlScripts">An array of SQL scripts to execute.</param>
    /// <returns>The number of scripts that executed successfully.</returns>
    public override int ExecuteScripts(bool outputScriptToStatus, params string[] sqlScripts)
    {
      // We want to repeat these validations from the base method here, to avoid starting an instance below if these simple validations fail.
      if (sqlScripts.Length == 0
          || !IsUsernameValid)
      {
        return 0;
      }

      bool previouslyRunning = IsRunning;
      if (!previouslyRunning
          && !StartInstanceAsProcess())
      {
        return 0;
      }

      int successfulScriptsCount = base.ExecuteScripts(outputScriptToStatus, sqlScripts);
      if (!previouslyRunning)
      {
        KillInstanceProcess();
      }

      return successfulScriptsCount;
    }

    /// <summary>
    /// Gets the list of MySQL processes that potentially conflict with the current process.
    /// </summary>
    /// <param name="controller">The <see cref="ServerConfigurationController"/> related to this instance.</param>
    /// <returns>An array containing a list of conflicting processes.</returns>
    public static Process[] GetPotentialConflictingProcesses(ServerConfigurationController controller)
    {
      var processId = Core.Classes.Utilities.GetServerInstanceProcessId(controller.DataDirectory);
      Process[] processes = null;
      var processList = new List<Process>();
      Process currentProcess = null;
      try
      {
        processes = Process.GetProcessesByName("mysqld");
        currentProcess = Process.GetProcessById(processId);
        if (processes == null
          || processes.Length == 0)
        {
          return null;
        }

        foreach (var process in processes)
        {
          if (process.Id == processId)
          {
            continue;
          }

          processList.Add(process);
        }
      }
      catch (Exception)
      {
        Logger.LogError(Resources.FailedToRetrieveMySqldProcesses);
        return null;
      }

      return processList.ToArray();
    }

    /// <summary>
    /// Gets the error message associated to the host not running because the data directory is being used.
    /// </summary>
    /// <param name="mySqlErrorLog">An object representing the error log of the current server instance.</param>
    /// <param name="controller">The current server instance controller.</param>
    /// <returns>A string representing the error message found in the error log.</returns>
    public static string GetDataDirectoryInUseErrorMessage(ServerErrorLog mySqlErrorLog, ServerConfigurationController controller)
    {
      if (mySqlErrorLog == null)
      {
        return null;
      }

      string errorMessage = null;
      mySqlErrorLog.ReadNewLinesFromFile(true);
      var logLines = mySqlErrorLog.LogLines.Select(line => line.Message).ToList();
      if (logLines.Count > 0)
      {
        var latestFail = logLines.FindLast(o => o.Contains("must be writable"));
        if (!string.IsNullOrEmpty(latestFail))
        {
          var affectedFilesMessage = "The process(es) id(s) potentially using the affected files are: {0}.";
          var conflictingProcesses = GetPotentialConflictingProcesses(controller);
          if (conflictingProcesses == null)
          {
            return Resources.DataDirectoryInUse;
          }

          var builder = new StringBuilder();
          for (int i = 0; i < conflictingProcesses.Length; i++)
          {
            builder.Append(conflictingProcesses[i].Id);
            if (i < conflictingProcesses.Length - 1)
            {
              builder.Append(", ");
            }
          }

          errorMessage = $"{Resources.DataDirectoryInUse}.{Environment.NewLine}{string.Format(affectedFilesMessage, builder.ToString())}";
        }
      }

      return errorMessage;
    }

    /// <summary>
    /// Gets the connection string builder used to establish a connection to this instance.
    /// </summary>
    /// <param name="schemaName">The name of the default schema to work with.</param>
    /// <returns>The connection string builder used to establish a connection to this instance.</returns>
    public override MySqlConnectionStringBuilder GetConnectionStringBuilder(string schemaName = null)
    {
      var builder = _controller.GetConnectionStringBuilder(UserAccount, UseOldSettings, schemaName);
      return builder;
    }

    /// <summary>
    /// Validates if the instance is set to read-only mode.
    /// </summary>
    /// <returns><c>true</c> if the instance is read-only, <c>false</c> if it is not read-only and
    /// <c>null</c> if an error occurred when attempting to retrieve the value.</returns>
    public bool? IsReadOnly()
    {
      if (!(CanConnect() == ConnectionResultType.ConnectionSuccess))
      {
        return null;
      }

      object superReadOnlyVariableValue = null;
      if (ServerVersion.ServerSupportsSuperReadOnlyVariable())
      {
        superReadOnlyVariableValue = GetVariable("super_read_only", true, ServerVersion.ServerSupportsPerformanceSchemaSystemVariableTables());
        if (superReadOnlyVariableValue == null)
        {
          ReportStatus(string.Format(Resources.FailedToRetrieveVariable, "super_read_only"));
          return null;
        }
      }

      var readOnlyVariableValue = GetVariable("read_only", true, ServerVersion.ServerSupportsPerformanceSchemaSystemVariableTables());
      if (readOnlyVariableValue == null)
      {
        ReportStatus(string.Format(Resources.FailedToRetrieveVariable, "read_only"));
        return null;
      }

      return (superReadOnlyVariableValue != null
              && superReadOnlyVariableValue.ToString().Equals("ON", StringComparison.InvariantCultureIgnoreCase))
             || readOnlyVariableValue.ToString().Equals("ON", StringComparison.InvariantCultureIgnoreCase);
    }

    /// <summary>
    /// Kills this MySQL Server instance's related process.
    /// </summary>
    public void KillInstanceProcess()
    {
      var processToKill = RunningProcess;
      if (processToKill == null)
      {
        ReportStatus(string.Format(Resources.ProcessNotRunningText, NameWithVersion));
        return;
      }

      var processId = processToKill.Id;
      ReportStatus(string.Format(Resources.StoppingProcessText, NameWithVersion, processId));
      if (!processToKill.HasExited)
      {
        processToKill.Kill();
        processToKill.WaitForExit();
      }

      processToKill.Dispose();
      ReportStatus(string.Format(Resources.StoppedProcessText, NameWithVersion, processId));
    }

    /// <summary>
    /// Attempts to connect to the Server instance and do a graceful shutdown before stopping it.
    /// </summary>
    /// <param name="useOldSettings">Flag indicating whether the old settings must be used instead of the new settings to build the command line options.</param>
    /// <returns><c>true</c> if the Server is stopped (gracefully or not), <c>false</c> otherwise.</returns>
    public bool ShutdownInstance(bool useOldSettings)
    {
      if (_controller.Package.License == LicenseType.Commercial
          && _controller.ConfigurationType == ConfigurationType.New
          && !_controller.IsThereServerDataFiles
          && _controller.ServerVersion.ServerSupportsEnterpriseFirewall())
      {
        // If the user is retrying configuration the data will be recreated so the root user will have a blank password
        // Changes done for Bug #21085453 - FAILED CONFIGURATION STEPS FOR EFW ARE NOT PROPERLY INDICATED AFTER EXECUTION
        _controller.Settings.ExistingRootPassword = string.Empty;
      }

      if (!string.IsNullOrEmpty(_controller.Settings.ExistingRootPassword))
      {
        var tempConfigFileWithPassword = Core.Classes.Utilities.CreateTempConfigurationFile(IniFile.GetClientPasswordLines(_controller.Settings.ExistingRootPassword));
        var sendingPasswordInCommandLine = tempConfigFileWithPassword == null;
        var connectionOptions = sendingPasswordInCommandLine
          ? $"--password={_controller.Settings.ExistingRootPassword} "
          : $"--defaults-extra-file=\"{tempConfigFileWithPassword}\" ";
        connectionOptions += _controller.GetCommandLineConnectionOptions(null, false, useOldSettings);
        ReportStatus(Resources.ServerShutdownSettingInnoDbFastShutdown);
        var result = Core.Classes.Utilities.RunProcess(
          Path.Combine(_controller.InstallDirectory, ServerProductConfigurationController.BINARY_DIRECTORY_NAME, ServerProductConfigurationController.CLIENT_EXECUTABLE_FILENAME),
          $" {connectionOptions} -e\"SET GLOBAL innodb_fast_shutdown = 0\"",
          null,
          ReportStatus,
          ReportStatus,
          true);
        ReportStatus(result.ExitCode == 0
          ? Resources.ServerShutdownSettingInnoDbFastShutdownSuccess
          : Resources.ServerShutdownSettingInnoDbFastShutdownError);

        ReportStatus(Resources.ServerShutdownMySqlAdminShutDown);
        result = Core.Classes.Utilities.RunProcess(
          Path.Combine(_controller.InstallDirectory, ServerProductConfigurationController.BINARY_DIRECTORY_NAME, ServerProductConfigurationController.ADMIN_TOOL_EXECUTABLE_FILENAME),
          $" {connectionOptions} shutdown",
          null,
          ReportStatus,
          ReportStatus,
          true);
        ReportStatus(result.ExitCode == 0
          ? Resources.ServerShutdownMySqlAdminShutDownSuccess
          : Resources.ServerShutdownMySqlAdminShutDownError);
        Core.Classes.Utilities.DeleteFile(tempConfigFileWithPassword, 10, 500);
      }

      StopInstance();
      return WaitUntilNotRunning(1, 30);
    }

    /// <summary>
    /// Starts this Server instance as previously configured (Windows Service or process).
    /// </summary>
    /// <param name="additionalOptions">Additional options to pass to the server process.</param>
    /// <returns>A <see cref="ServerStartStatus"/> value.</returns>
    public ServerStartStatus StartInstance(string additionalOptions = null)
    {
      return _controller.Settings.ConfigureAsService
        ? StartInstanceAsServiceWithExtendedStatus(additionalOptions)
        : StartInstanceAsProcessWithExtendedStatus(additionalOptions, true);
    }

    /// <summary>
    /// Starts a new process for this MySQL Server instance.
    /// </summary>
    /// <param name="additionalOptions">Additional options to pass to the server process.</param>
    /// <returns><c>true</c> if the instance process was started successfully, <c>false</c> otherwise.</returns>
    public bool StartInstanceAsProcess(string additionalOptions = null)
    {
      return StartInstanceAsProcessWithExtendedStatus(additionalOptions).Started;
    }

    /// <summary>
    /// Stops this Server instance as previously configured (Windows Service or process).
    /// </summary>
    public void StopInstance()
    {
      if (!IsRunning)
      {
        return;
      }

      ReportStatus(Resources.StoppingServerInstanceText);
      try
      {
        string mysqlWindowsServiceName = null;
        if (_controller.OldSettings != null
            && MySqlServiceControlManager.ServiceExists(_controller.OldSettings.ServiceName)
            && MySqlServiceControlManager.GetServiceStatus(_controller.OldSettings.ServiceName) == ServiceControllerStatus.Running)
        {
          mysqlWindowsServiceName = _controller.OldSettings.ServiceName;
        }
        else if (_controller.Settings != null
                 && MySqlServiceControlManager.ServiceExists(_controller.Settings.ServiceName)
                 && MySqlServiceControlManager.GetServiceStatus(_controller.Settings.ServiceName) == ServiceControllerStatus.Running)
        {
          mysqlWindowsServiceName = _controller.Settings.ServiceName;
        }

        if (!string.IsNullOrEmpty(mysqlWindowsServiceName))
        {
          ReportStatus(Resources.ServerInstanceStoppingWindowsServiceText);
          MySqlServiceControlManager.Stop(mysqlWindowsServiceName, _controller.CancellationToken);
          ReportStatus(Resources.ServerInstanceStoppedWindowsServiceText);
        }
        else if (IsRunning)
        {
          KillInstanceProcess();
        }
      }
      catch (Exception ex)
      {
        ReportStatus(string.Format(Resources.StoppingServerInstanceErrorText, ex.Message));
      }
    }

    /// <summary>
    /// Keeps retrying to open a connection to this instance until it is successful.
    /// </summary>
    /// <param name="reportStatusChanges">Flag indicating whether status changes are output to configuration log.</param>
    /// <param name="maxRetries">The number of retries to attempt a connection. If <c>0</c> or lower, retry indefinitely.</param>
    /// <returns><c>true</c> if a connection could be opened, <c>false</c> otherwise.</returns>
    public bool WaitUntilConnectionSuccessful(bool reportStatusChanges, int maxRetries = DEFAULT_MAX_CONNECTION_RETRIES)
    {
      if (maxRetries < 0)
      {
        // Set a reasonable maximum, no need to have a never-ending loop.
        maxRetries = 100;
      }

      int currentRetry = 0;
      var currentUseOldSettings = UseOldSettings;
      if (reportStatusChanges)
      {
        ReportStatus(string.Format(Resources.ServerConfigWaitingForSuccesfulConnectionText, NameWithVersion, maxRetries));
      }

      var success = false;
      uint connectionTimeOut = 10;
      const int WAITING_TIME_BETWEEN_CONNECTIONS_IN_SECONDS = 5;
      while (!success && currentRetry < maxRetries)
      {
        var flipSettings = true;
        currentRetry++;
        if (currentRetry > 1)
        {
          if (reportStatusChanges)
          {
            ReportStatus(string.Format(Resources.ServerConfigWaitingForSuccesfulConnectionRetryWaitText, WAITING_TIME_BETWEEN_CONNECTIONS_IN_SECONDS));
          }

          Thread.Sleep(WAITING_TIME_BETWEEN_CONNECTIONS_IN_SECONDS * 1000);
        }

        try
        {
          var connStringBuilder = GetConnectionStringBuilder();
          connStringBuilder.ConnectionTimeout = connectionTimeOut;
          if (reportStatusChanges)
          {
            ReportStatus(string.Format(Resources.ServerConfigWaitingForSuccesfulConnectionRetryText, currentRetry, connStringBuilder.GetHostIdentifier(), connStringBuilder.UserID, string.IsNullOrEmpty(connStringBuilder.Password) ? "no" : "a"));
          }

          using (var c = new MySqlConnection(connStringBuilder.ConnectionString))
          {
            c.Open();
          }

          success = true;
        }
        catch (TimeoutException timeoutException)
        {
          // Increase the timeout, see what happens in the next retry.
          connectionTimeOut *= 2;
          Logger.LogException(timeoutException);
          if (reportStatusChanges)
          {
            ReportStatus($"Timeout error: {timeoutException.Message}");
            ReportStatus($"Increasing timeout to {connectionTimeOut} seconds and retrying.");
          }
        }
        catch (MySqlException mySqlException)
        {
          if (mySqlException.Message.IndexOf("access denied", StringComparison.InvariantCultureIgnoreCase) >= 0)
          {
            // This means the current root user can't connect with the current credentials but the Server is accepting connections
            success = true;
            break;
          }

          if (reportStatusChanges)
          {
            ReportStatus($"MySQL error {mySqlException.Number}: {mySqlException.Message}");
          }

          Logger.LogException(mySqlException);
          if (mySqlException.Message.IndexOf("hosts", StringComparison.InvariantCultureIgnoreCase) > 0)
          {
            if (UseOldSettings)
            {
              UseOldSettings = false;
              flipSettings = false;
            }
          }

          if (flipSettings)
          {
            // Try flipping the UseOldSettings value and reconnecting
            UseOldSettings = !UseOldSettings;
          }
        }
        catch (Exception ex)
        {
          Logger.LogException(ex);
          if (reportStatusChanges)
          {
            ReportStatus($"Unknown error: {ex.Message}");
          }
        }
      }

      if (reportStatusChanges)
      {
        ReportStatus(success
                      ? string.Format(Resources.ServerConfigWaitingForSuccesfulConnectionSuccessText, NameWithVersion)
                      : string.Format(Resources.ServerConfigWaitingForSuccesfulConnectionFailedText, NameWithVersion, currentRetry));
      }

      UseOldSettings = currentUseOldSettings;
      return success;
    }

    /// <summary>
    /// Reports any errors related to starting the server.
    /// </summary>
    /// <param name="logLines">A list of log lines to process.</param>
    private void ReportServerStartErrors(List<ServerErrorLogLine> logLines)
    {
      if (logLines == null
          || logLines.Count() == 0)
      {
        return;
      }

      var errorLines = logLines.Where(line => line.Type.Equals("Error", StringComparison.OrdinalIgnoreCase));
      if (errorLines.Count() > 0)
      {
        var errorLineMessages = errorLines.Select(line => line.Message).ToList();
        var builder = new StringBuilder();
        builder.AppendLine(Resources.ServerConfigStartServerFailedWithErrors);
        foreach (var errorLineMessage in errorLineMessages)
        {
          builder.AppendLine(errorLineMessage);
        }

        ReportStatus(builder.ToString());
      }
      else
      {
        ReportStatus(Resources.ServerConfigStartServerFailedWIthUnknownError);
      }
    }

    /// <summary>
    /// Starts a new process for this MySQL Server instance.
    /// </summary>
    /// <param name="additionalOptions">Additional options to pass to the server process.</param>
    /// <param name="connectionsWaitReportStatus">Flag indicating if messages are reported while testing connection attempts.</param>
    /// <returns>A <see cref="ServerStartStatus"/> instance.</returns>
    private ServerStartStatus StartInstanceAsProcessWithExtendedStatus(string additionalOptions = null, bool connectionsWaitReportStatus = false)
    {
      var startStatus = new ServerStartStatus(false);
      if (IsRunning)
      {
        startStatus.Started = true;
        return startStatus;
      }

      if (string.IsNullOrEmpty(ServerExecutableFilePath)
          || string.IsNullOrEmpty(ServerConfigFilePath))
      {
        startStatus.Started = false;
        return startStatus;
      }

      ReportStatus(string.Format(Resources.ServerConfigProcessStartingText, NameWithVersion));
      var isAdditionalOptionEmpty = string.IsNullOrEmpty(additionalOptions);
      var isSelfContainedUpgrade = !isAdditionalOptionEmpty
                                   && additionalOptions.IndexOf("--upgrade", StringComparison.InvariantCultureIgnoreCase) >= 0;
      var isInitializingDatabase = !isAdditionalOptionEmpty
                                   && additionalOptions.IndexOf("--initialize", StringComparison.InvariantCultureIgnoreCase) >= 0;
      var redirectOutputToConsole = !isSelfContainedUpgrade
                                    && ReportStatusDelegate != null;
      var coreOptionsBuilder = new StringBuilder();
      if (File.Exists(ServerConfigFilePath))
      {
        coreOptionsBuilder.Append("--defaults-file=\"");
        coreOptionsBuilder.Append(ServerConfigFilePath);
        coreOptionsBuilder.Append("\"");
      }
      else
      {
        coreOptionsBuilder.Append("--port=");
        coreOptionsBuilder.Append(Port);
        if (!string.IsNullOrEmpty(_controller.DataDirectory))
        {
          coreOptionsBuilder.Append(" --datadir=\"");
          coreOptionsBuilder.Append(Path.Combine(_controller.DataDirectory, "data"));
          coreOptionsBuilder.Append("\"");
        }
      }

      // Initialize the async task that will parse the error log in case of a self-contained upgrade or in case of parsing the error log file to determine if the server is accepting connections
      Task<ServerUpgradeStatus> parsingLogForUpgradeTask = null;
      Task<bool> parsingLogForAcceptingConnectionsTask = null;
      ServerErrorLog mySqlErrorLog = null;
      var parseErrorLog = isSelfContainedUpgrade
                          || ParseErrorLogForAcceptingConnections
                             && WaitUntilAcceptingConnections;
      if (parseErrorLog)
      {
        _controller.UseStatusesList = redirectOutputToConsole;
        mySqlErrorLog = new ServerErrorLog(_controller.ErrorLogFilePath, redirectOutputToConsole ? _controller.StatusesList : null)
        {
          ReportStatusDelegate = ReportStatus,
          ReportWaitingDelegate = _controller.ReportWaiting
        };
        if (isSelfContainedUpgrade)
        {
          parsingLogForUpgradeTask = Task.Factory.StartNew(() => mySqlErrorLog.ParseServerUpgradeMessages(_controller.ServerVersion), _controller.CancellationToken);
        }
        else
        {
          parsingLogForAcceptingConnectionsTask = Task.Factory.StartNew(() => mySqlErrorLog.ParseServerAcceptingConnectionMessage(_controller.ServerVersion, !redirectOutputToConsole), _controller.CancellationToken);
        }
      }

      var consoleOption = redirectOutputToConsole
        ? " --console"
        : string.Empty;
      if (!string.IsNullOrEmpty(additionalOptions))
      {
        additionalOptions = " " + additionalOptions;
      }

      var processResult = Core.Classes.Utilities.RunProcess(
        ServerExecutableFilePath,
        $"{coreOptionsBuilder}{consoleOption}{additionalOptions}",
        null,
        ReportStatus,
        ReportStatus,
        isInitializingDatabase);
      startStatus.Started = processResult != null 
                            && (isInitializingDatabase
                              ? processResult.ExitCode == 0
                              : processResult.RunProcess != null
                                && !processResult.RunProcess.HasExited);
      ReportStatus(string.Format(startStatus.Started ? Resources.ServerConfigProcessStartedSuccessfullyText : Resources.ServerConfigProcessStartFailedText, NameWithVersion));
      if (startStatus.Started
          && parseErrorLog)
      {
        if (isSelfContainedUpgrade)
        {
          parsingLogForUpgradeTask.Wait(_controller.CancellationToken);
          startStatus.UpgradeStatus = parsingLogForUpgradeTask.IsCompleted
            ? parsingLogForUpgradeTask.Result
            : new ServerUpgradeStatus();
          startStatus.AcceptingConnections = startStatus.UpgradeStatus.AcceptingConnections;
        }
        else
        {
          parsingLogForAcceptingConnectionsTask.Wait(_controller.CancellationToken);
          startStatus.AcceptingConnections = parsingLogForAcceptingConnectionsTask.IsCompleted
                                             && parsingLogForAcceptingConnectionsTask.Result;
        }
      }
      else if (parseErrorLog)
      {
        ReportServerStartErrors(mySqlErrorLog.LogLines);
      }

      if (WaitUntilAcceptingConnections
          && startStatus.Started
          && (!parseErrorLog
              || !startStatus.AcceptingConnections))
      {
        startStatus.AcceptingConnections = WaitUntilConnectionSuccessful(connectionsWaitReportStatus);
      }

      _controller.UseStatusesList = false;
      parsingLogForUpgradeTask?.Dispose();
      parsingLogForAcceptingConnectionsTask?.Dispose();
      return startStatus;
    }

    /// <summary>
    /// Starts the Windows service related to this MySQL Server instance.
    /// </summary>
    /// <param name="additionalOptions">Additional options to pass to the Windows service.</param>
    /// <returns>A <see cref="ServerStartStatus"/> instance.</returns>
    private ServerStartStatus StartInstanceAsServiceWithExtendedStatus(string additionalOptions = null)
    {
      var isSelfContainedUpgrade = !string.IsNullOrEmpty(additionalOptions)
                                   && additionalOptions.IndexOf("--upgrade", StringComparison.InvariantCultureIgnoreCase) >= 0;
      var startStatus = new ServerStartStatus(true);
      Task<ServerUpgradeStatus> parsingLogForUpgradeTask = null;
      Task<bool> parsingLogTask = null;
      var mySqlErrorLog = new ServerErrorLog(_controller.ErrorLogFilePath)
      {
        ReportStatusDelegate = ReportStatus,
        ReportWaitingDelegate = _controller.ReportWaiting
      };
      ReportStatus(string.Format(Resources.ServerConfigEventStartServiceInfo, _controller.Settings.ServiceName));

      // Initialize the async task that will parse the error log in case of a self-contained upgrade
      if (isSelfContainedUpgrade)
      {
        parsingLogForUpgradeTask = Task.Factory.StartNew(() => mySqlErrorLog.ParseServerUpgradeMessages(_controller.ServerVersion), _controller.CancellationToken);
        try
        {
          MySqlServiceControlManager.Start(_controller.Settings.ServiceName, _controller.CancellationToken, additionalOptions, 90);
          parsingLogForUpgradeTask.Wait(_controller.CancellationToken);
          if (parsingLogForUpgradeTask.IsCompleted
              && parsingLogForUpgradeTask.Result.AcceptingConnections)
          {
            startStatus.Started = true;
            ReportStatus(string.Format(Resources.ServerConfigEventStartServiceSuccess, _controller.Settings.ServiceName));
          }
        }
        catch (System.ServiceProcess.TimeoutException)
        {
          startStatus.Started = false;
          ReportServerStartErrors(mySqlErrorLog.LogLines);
        }
        catch
        {
          startStatus.Started = false;
          ReportStatus(string.Format(Resources.ServerConfigEventStartServiceError, _controller.Settings.ServiceName));
        }

        if (!startStatus.Started)
        {
          // One final check in case parsing log failed.
          try
          {
            using (var ssc = new ExpandedServiceController(_controller.Settings.ServiceName))
            {
              if (ssc.Status == ServiceControllerStatus.Running)
              {
                startStatus.Started = true;
                startStatus.AcceptingConnections = true;
              }
            }
          }
          catch (Exception ex)
          {
            Logger.LogException(ex);
            throw;
          }
        }

        // Await for the async task that parses the error log in case of a self-contained upgrade to check when the upgrade has finished
        if (startStatus.Started)
        {
          parsingLogForUpgradeTask.Wait(_controller.CancellationToken);
          startStatus.UpgradeStatus = parsingLogForUpgradeTask.IsCompleted
            ? parsingLogForUpgradeTask.Result
            : new ServerUpgradeStatus();
          startStatus.AcceptingConnections = startStatus.UpgradeStatus.AcceptingConnections;
        }

        parsingLogForUpgradeTask.Dispose();
      }
      else
      {
        parsingLogTask = Task.Factory.StartNew(() => mySqlErrorLog.ParseServerAcceptingConnectionMessage(_controller.ServerVersion, true, !isSelfContainedUpgrade, 90), _controller.CancellationToken);
        try
        {
          MySqlServiceControlManager.Start(_controller.Settings.ServiceName, _controller.CancellationToken, additionalOptions, 90);
          parsingLogTask.Wait(_controller.CancellationToken);
          if (parsingLogTask.IsCompleted
              && parsingLogTask.Result)
          {
            startStatus.Started = true;
            ReportStatus(string.Format(Resources.ServerConfigEventStartServiceSuccess, _controller.Settings.ServiceName));
          }
        }
        catch (System.ServiceProcess.TimeoutException)
        {
          startStatus.Started = false;
          ReportServerStartErrors(mySqlErrorLog.LogLines);
        }
        catch
        {
          startStatus.Started = false;
          ReportStatus(string.Format(Resources.ServerConfigEventStartServiceError, _controller.Settings.ServiceName));
        }
        finally
        {
          parsingLogTask.Dispose();
        }

        if (!startStatus.Started)
        {
          // One final check in case parsing log failed.
          try
          {
            using (var ssc = new ExpandedServiceController(_controller.Settings.ServiceName))
            {
              if (ssc.Status == ServiceControllerStatus.Running)
              {
                startStatus.Started = true;
                startStatus.AcceptingConnections = true;
              }
            }
          }
          catch (Exception ex)
          {
            Logger.LogException(ex);
            throw;
          }
        }
      }

      if (WaitUntilAcceptingConnections
          && startStatus.Started
          && (!startStatus.AcceptingConnections
              || !ParseErrorLogForAcceptingConnections))
      {
        startStatus.AcceptingConnections = WaitUntilConnectionSuccessful(true, MaxConnectionRetries);
      }

      return startStatus;
    }

    /// <summary>
    /// Determines whether the server is running, specifying time to wait and certain number of retries until is not running.
    /// </summary>
    /// <param name="waitingSeconds">The waiting time in seconds.</param>
    /// <param name="maxRetries">The maximum number of retries.</param>
    /// <returns><c>true</c> if the instance is not running, <c>false</c> if running even when exhausting the number of retries.</returns>
    private bool WaitUntilNotRunning(int waitingSeconds, int maxRetries)
    {
      for (int i = 0; i < maxRetries; i++)
      {
        if (!IsRunning)
        {
          return true;
        }

        Thread.Sleep(waitingSeconds * 1000);
        ReportStatus(string.Format(Resources.ServerInstanceStillRunningRetryText, i + 1));
      }

      return false;
    }
  }
}
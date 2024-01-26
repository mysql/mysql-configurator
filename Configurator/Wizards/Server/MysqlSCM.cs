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
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Properties;

namespace MySql.Configurator.Wizards.Server
{
  /// <summary>
  /// Contains information about a Windows service used to run a MySQL Server instance.
  /// </summary>
  public class MySqlServiceInfo
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlServiceInfo"/> class.
    /// </summary>
    /// <param name="serviceName">The name of the Windows service.</param>
    /// <param name="iniDirectory">The path of the directory containing the configuration file used by the Windows service.</param>
    /// <param name="configFile">The name of the configuration file used by the Windows service.</param>
    public MySqlServiceInfo(string serviceName, string iniDirectory, string configFile)
    {
      ConfigFile = configFile;
      IniDirectory = iniDirectory;
      ServiceName = serviceName;
    }

    #region Properties

    /// <summary>
    /// Gets the name of the configuration file used by the Windows service.
    /// </summary>
    public string ConfigFile { get; private set; }

    /// <summary>
    /// Gets the path of the directory containing the configuration file used by the Windows service.
    /// </summary>
    public string IniDirectory { get; private set; }

    /// <summary>
    /// Gets the name of the Windows service.
    /// </summary>
    public string ServiceName { get; private set; }

    #endregion Properties
  }

  public class MySqlServiceControlManager
  {
    #region Constants

    /// <summary>
    /// The time in millisencods to wait for the next retry operation.
    /// </summary>
    public const int DEFAULT_SERVICE_EXISTS_SLEEP_TIME = 1000;

    /// <summary>
    /// The number of times to perform a retry operation to check for the existence of a particular Windows service.
    /// </summary>
    public const int DEFAULT_SERVICE_EXISTS_RETRY_COUNT = 5;

    /// <summary>
    /// The default regular expression pattern to match a MySQL Server's global configuration file used in a Windows service.
    /// </summary>
    public const string DEFAULT_SERVICE_CONFIG_FILE_PATTERN = @" --defaults-file=""(?<iniLocation>.+)?"" ";

    public const string STANDARD_SERVICE_ACCOUNT = @"NT AUTHORITY\NetworkService";

    #endregion

    #region Fields

    private static CancellationToken _cancellationToken = new CancellationToken(false);

    private Regex _defaultsFileRegex;

    private List<MySqlServiceInfo> _serviceInfos;

    #endregion Fields

    /// <summary>
    /// Creates an instance of the <see cref="MySqlServiceControlManager"/> class.
    /// </summary>
    /// <param name="configFileDirectory">A directory path containing a MySQL Server's global configuration file.</param>
    public MySqlServiceControlManager(string configFileDirectory)
    {
      _serviceInfos = new List<MySqlServiceInfo>();
      foreach (var serviceName in FindServiceNamesWithBaseDirectory(configFileDirectory))
      {
        var match = DefaultsFileRegex.Match(GetBinaryPath(serviceName));
        if (match.Success)
        {
          var configFilePath = Path.GetFullPath(match.Groups["iniLocation"].Value);
          var configFileExists = File.Exists(configFilePath);
          var verifiedConfigFileDirectory = configFileExists ? Path.GetDirectoryName(configFilePath) : string.Empty;
          var verifiedConfigFileName = configFileExists ? Path.GetFileName(configFilePath) : string.Empty;
          _serviceInfos.Add(new MySqlServiceInfo(serviceName, verifiedConfigFileDirectory, verifiedConfigFileName));
        }
      }
    }

    #region Properties

    /// <summary>
    /// Gets an array of objects containing basic information about Windows services associated to this MySQL Server instance.
    /// </summary>
    public MySqlServiceInfo[] ServiceInfos => _serviceInfos.ToArray();

    /// <summary>
    /// A regular expression to match a MySQL Server's global configuration file used in a Windows service.
    /// </summary>
    private Regex DefaultsFileRegex
    {
      get
      {
        if ( _defaultsFileRegex == null)
        {
          _defaultsFileRegex = new Regex(DEFAULT_SERVICE_CONFIG_FILE_PATTERN);
        }

        return _defaultsFileRegex;
      }
    }

    #endregion Properties

    public static void Add(string serviceName, string displayName, string fileName, string userName, string password, bool startAtStarUp)
    {
      try
      {
        using (var ssc = new ExpandedServiceController(serviceName, displayName, fileName, userName, password, startAtStarUp))
        {
          ssc.Close();
        }
      }
      catch (Exception e)
      {
        Logger.LogException(e);
        //// Rethrow the exception so the caller can handle this error case properly.
        throw;
      }
    }

    public static void Cancel()
    {
      _cancellationToken = new CancellationToken(true);
    }

    /// <summary>
    /// Deletes the service that matches the specified name.
    /// </summary>
    /// <param name="serviceName">The name of the service to delete.</param>
    /// <param name="throwOnFail"><c>true</c> to throw an exception if the delete operation fails; otherwise, <c>false</c>.</param>
    /// <param name="waitForServiceDeletion">Wait for the service to be deleted by checking the service status.</param>
    /// <param name="serviceExistsRetryCount">The number of retries to check for the existence of the specified Windows service when
    /// waiting for the service to be deleted.</param>
    /// <param name="sleepTimeBetweenRetries">The time in milliseconds to wait before retrying to check for the existence of the
    /// specified Windows service.</param>
    /// <returns><c>true</c> if the service was deleted successfully, <c>false</c> if the service was marked for deletion without
    /// actually being deleted and <c>null</c> if an error occurred when attempting to delete the service.</returns>
    public static bool? Delete(string serviceName, bool throwOnFail = true, bool waitForServiceDeletion = true, int serviceExistsRetryCount = DEFAULT_SERVICE_EXISTS_RETRY_COUNT, int sleepTimeBetweenRetries = DEFAULT_SERVICE_EXISTS_SLEEP_TIME)
    {
      if (string.IsNullOrEmpty(serviceName))
      {
        return null;
      }

      try
      {
        using (var ssc = new ExpandedServiceController(serviceName))
        {
          ssc.Remove();
          ssc.Close();
        }

        if (!waitForServiceDeletion)
        {
          return true;
        }

        var retryCount = 0;
        while (retryCount < serviceExistsRetryCount)
        {
          if (!ServiceExists(serviceName))
          {
            return true;
          }

          Logger.LogVerbose(string.Format(Resources.WindowsServiceWaitingToBeDeleted, serviceName, retryCount++));
          Thread.Sleep(sleepTimeBetweenRetries);
        }

        Logger.LogVerbose(Resources.WindowsServiceNotYetDeleted);
        return false;
      }
      catch (Exception e)
      {
        Logger.LogException(e);
        if (throwOnFail)
        {
          throw;
        }

        return null;
      }
    }

    /// <summary>
    /// Finds Windows services that match the given base directory and returns their names.
    /// </summary>
    /// <param name="baseDirectory"A directory path.></param>
    /// <returns>An array of service names matching the given base directory.</returns>
    public static string[] FindServiceNamesWithBaseDirectory(string baseDirectory)
    {
      if (string.IsNullOrEmpty(baseDirectory))
      {
        return null;
      }

      var foundServiceNames = new List<string>();
      var scmServices = ServiceController.GetServices();
      foreach (var scmService in scmServices)
      {
        using (var superServiceController = new ExpandedServiceController(scmService))
        {
          string regexSeed = Path.GetFullPath(baseDirectory) + ".";
          regexSeed = regexSeed.Replace(@"\", @"[\/\\]");
          regexSeed = regexSeed.Replace(@"(", @"\(");
          regexSeed = regexSeed.Replace(@")", @"\)");

          var localTemplate = new Regex(regexSeed);
          var localMatch = localTemplate.Match(superServiceController.BinaryPath);

          if (localMatch.Success)
          {
            foundServiceNames.Add(superServiceController.ServiceName);
          }
        }

        scmService.Close();
      }

      return foundServiceNames.ToArray();
    }

    /// <summary>
    /// Gets the name of a Windows service running a MySQL Server that best matches the given configuration file directory.
    /// </summary>
    /// <param name="configFileDirectory">A directory containing a global configuration file for a MySQL Server.</param>
    /// <returns>The name of a Windows service running a MySQL Server that best matches the given configuration file directory, or an empty string if no match is found.</returns>
    public string GetBestServiceNameMatchingConfigFileDirectory(string configFileDirectory)
    {
      return _serviceInfos.Count == 0
             ? string.Empty
             : (_serviceInfos.Count == 1
                ? _serviceInfos[0].ServiceName
                : _serviceInfos.FirstOrDefault(si => Path.GetFullPath(si.IniDirectory).Equals(Path.GetFullPath(configFileDirectory)))?.ServiceName ?? _serviceInfos[0].ServiceName);
    }

    public static string GetBinaryPath(string serviceName)
    {
      if (string.IsNullOrEmpty(serviceName))
      {
        return string.Empty;
      }

      string binaryPath = string.Empty;
      try
      {
        using (var ssc = new ExpandedServiceController(serviceName))
        {
          binaryPath = ssc.BinaryPath;
          ssc.Close();
        }
      }
      catch (Exception e)
      {
        Logger.LogException(e);
      }

      return binaryPath;
    }

    public static Service GetServiceDetails(string serviceName)
    {
      using (var managementBaseObject = new ManagementObjectSearcher(new SelectQuery($"SELECT * FROM Win32_Service WHERE Name = '{serviceName}'")).Get())
      {
        var managementObject = managementBaseObject.Cast<ManagementObject>().FirstOrDefault();
        if (managementObject == null)
        {
          return null;
        }

        var service = new Service
        {
          AcceptPause = managementObject["AcceptPause"] != null && (bool)managementObject["AcceptPause"],
          AcceptStop = managementObject["AcceptStop"] != null && (bool)managementObject["AcceptStop"],
          Caption = managementObject["Caption"]?.ToString() ?? string.Empty,
          Description = managementObject["Description"]?.ToString() ?? string.Empty,
          DisplayName = managementObject["DisplayName"]?.ToString() ?? string.Empty,
          Name = managementObject["Name"]?.ToString() ?? string.Empty,
          PathName = managementObject["PathName"]?.ToString() ?? string.Empty,
          ProcessId = managementObject["ProcessId"] != null ? Convert.ToInt32(managementObject["ProcessId"]) : 0,
          ServiceType = managementObject["ServiceType"]?.ToString() ?? string.Empty,
          Started = managementObject["Started"] != null && (bool)managementObject["Started"],
          StartMode = managementObject["StartMode"]?.ToString() ?? string.Empty,
          StartName = managementObject["StartName"]?.ToString() ?? string.Empty,
          State = managementObject["State"]?.ToString() ?? string.Empty,
          Status = managementObject["Status"]?.ToString() ?? string.Empty,
        };
        return service;
      }
    }

    public static ServiceControllerStatus GetServiceStatus(string serviceName)
    {
      var currentStatus = ServiceControllerStatus.Stopped;
      if (string.IsNullOrEmpty(serviceName))
      {
        return currentStatus;
      }

      try
      {
        using (var ssc = new ExpandedServiceController(serviceName))
        {
          currentStatus = ssc.Status;
          ssc.Close();
        }
      }
      catch (Exception e)
      {
        Logger.LogException(e);
      }

      return currentStatus;
    }

    public static void Restart(string serviceName)
    {
      _cancellationToken = new CancellationToken(false);
      Restart(serviceName, _cancellationToken);
    }

    public static void Restart(string serviceName, CancellationToken cancellationToken)
    {
      if (string.IsNullOrEmpty(serviceName))
      {
        return;
      }

      try
      {
        using (var ssc = new ExpandedServiceController(serviceName))
        {
          if (ssc.Status == ServiceControllerStatus.Running)
          {
            Logger.LogVerbose($"{DateTime.Now} - Attempting to stop {serviceName} service.");
            ssc.Stop();
          }

          ssc.WaitForStatus(ServiceControllerStatus.Stopped, cancellationToken);
          Logger.LogVerbose($"{DateTime.Now} - Attempting to start {serviceName} service.");
          ssc.Start();
          ssc.WaitForStatus(ServiceControllerStatus.Running, cancellationToken);
          ssc.Close();
        }
      }
      catch (Exception e)
      {
        Logger.LogException(e);
        throw;
      }
    }

    /// <summary>
    /// Checks if the specified service exists.
    /// </summary>
    /// <param name="serviceName">The service name.</param>
    /// <returns><c>true</c> if the service exists; otherwise, <c>false</c>.</returns>
    public static bool ServiceExists(string serviceName)
    {
      var services = ServiceController.GetServices();
      var service = services.FirstOrDefault(s => s.ServiceName == serviceName);
      return service != null;
    }

    /// <summary>
    /// Checks if the given service name is not being already used by a Windows service.
    /// </summary>
    /// <param name="serviceName">The service name.</param>
    /// <returns><c>true</c> if the given service name is not being already used by a Windows service, <c>false</c> otherwise.</returns>
    public static bool ServiceNameIsAvailable(string serviceName)
    {
      return !ServiceController.GetServices().Any(service => service.ServiceName.Equals(serviceName, StringComparison.InvariantCultureIgnoreCase));
    }

    public static void Start(string serviceName)
    {
      _cancellationToken = new CancellationToken(false);
      Start(serviceName, _cancellationToken);
    }

    /// <summary>
    /// Starts the specified MySQL Windows Service.
    /// </summary>
    /// <param name="serviceName">The name of the service.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="additionalOptions">Options passed on to the service start request.</param>
    /// <param name="waitForStatusSeconds">The number of seconds to wait for the service to start before raising a timeout exception.</param>
    public static void Start(string serviceName, CancellationToken cancellationToken, string additionalOptions = null, int waitForStatusSeconds = 0)
    {
      if (string.IsNullOrEmpty(serviceName))
      {
        return;
      }

      try
      {
        using (var ssc = new ExpandedServiceController(serviceName))
        {
          string logMessage = null;
          var waitForStatus = true;
          var waitingStatus = ServiceControllerStatus.Running;
          var startService = true;
          switch (ssc.Status)
          {
            case ServiceControllerStatus.Running:
              startService = false;
              waitForStatus = false;
              logMessage = string.Format(Resources.WindowsServiceAlreadyRunning, serviceName);
              break;

            case ServiceControllerStatus.StartPending:
            case ServiceControllerStatus.ContinuePending:
              startService = false;
              logMessage = string.Format(Resources.WindowsServiceWaitingToStart, serviceName);
              break;

            case ServiceControllerStatus.PausePending:
              waitingStatus = ServiceControllerStatus.Paused;
              logMessage = string.Format(Resources.WindowsServiceWaitingToPause, serviceName);
              break;

            case ServiceControllerStatus.StopPending:
              waitingStatus = ServiceControllerStatus.Stopped;
              logMessage = string.Format(Resources.WindowsServiceWaitingToStop, serviceName);
              break;

            case ServiceControllerStatus.Paused:
            case ServiceControllerStatus.Stopped:
              waitForStatus = false;
              logMessage = string.Format(Resources.WindowsServiceStoppedOrPaused, serviceName);
              break;
          }

          if (!string.IsNullOrEmpty(logMessage))
          {
            Logger.LogVerbose($"{DateTime.Now} - {logMessage}");
          }

          if (waitForStatus)
          {
            ssc.WaitForStatus(waitingStatus, cancellationToken);
          }

          if (startService)
          {
            Logger.LogVerbose($"{DateTime.Now} - {string.Format(Resources.WindowsServiceStarting, serviceName)}");
            if (!string.IsNullOrEmpty(additionalOptions))
            {
              ssc.Start(additionalOptions.Split(' '));
            }
            else
            {
              ssc.Start();
            }

            if (waitForStatusSeconds == 0)
            {
              ssc.WaitForStatus(ServiceControllerStatus.Running, cancellationToken);
            }
            else
            {
              ssc.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, waitForStatusSeconds));
            }
            
            if (ssc.Status == ServiceControllerStatus.Running)
            {
              Logger.LogVerbose($"{DateTime.Now} - {string.Format(Resources.WindowsServiceStarted, serviceName)}");
            }
            else
            {
              Logger.LogError($"{DateTime.Now} - {string.Format(Resources.WindowsServiceStartFailed, serviceName)}");
            }
            
          }

          ssc.Close();
        }
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
        throw;
      }
    }

    public static void Stop(string serviceName)
    {
      _cancellationToken = new CancellationToken(false);
      Stop(serviceName, _cancellationToken);
    }

    public static void Stop(string serviceName, CancellationToken cancellationToken)
    {
      if (string.IsNullOrEmpty(serviceName))
      {
        return;
      }

      try
      {
        using (var ssc = new ExpandedServiceController(serviceName))
        {
          string logMessage;
          var waitForStatus = false;
          var waitingStatus = ServiceControllerStatus.Stopped;
          var stopService = false;
          switch (ssc.Status)
          {
            case ServiceControllerStatus.Stopped:
              logMessage = $"Service {serviceName} is stopped already.";
              break;

            case ServiceControllerStatus.StopPending:
              waitForStatus = true;
              waitingStatus = ServiceControllerStatus.Stopped;
              logMessage = $"Service {serviceName} is being stopped. Waiting until the status changes...";
              break;

            default:
              stopService = true;
              logMessage = $"Service {serviceName} can be stopped.";
              break;
          }

          if (!string.IsNullOrEmpty(logMessage))
          {
            Logger.LogVerbose($"{DateTime.Now} - {logMessage}");
          }

          if (waitForStatus)
          {
            ssc.WaitForStatus(waitingStatus, cancellationToken);
          }

          if (stopService)
          {
            Logger.LogVerbose($"{DateTime.Now} - Attempting to stop the {serviceName} service...");
            ssc.Stop();
            ssc.WaitForStatus(ServiceControllerStatus.Stopped, cancellationToken);
            Logger.LogVerbose($"{DateTime.Now} - {serviceName} service was stopped successfully.");
          }

          ssc.Close();
        }
      }
      catch (Exception e)
      {
        Logger.LogException(e);
        throw;
      }
    }

    public static void Update(string serviceName, string newName, string displayName, string cmdline, string account, string pwd, bool startAtStartup)
    {
      if (string.IsNullOrEmpty(serviceName))
      {
        return;
      }

      try
      {
        using (var ssc = new ExpandedServiceController(serviceName))
        {
          ssc.Update(newName, cmdline, account, pwd, startAtStartup);
          ssc.Close();

          //not working update directly the service, is updating the display name but internally still have the old name
          //instead we will remove the old service and then create a new one
          //Delete(serviceName);
          //Add(newName, displayName, cmdline, account, pwd, startAtStartup);
        }
      }
      catch (Exception e)
      {
        Logger.LogException(e);
        throw;
      }
    }

    /// <summary>
    /// Checks to verify a given username and password are:
    /// 1. Has proper local security policy rights to function as a service account
    /// 2. Has appropriate file permissions to the data directory.
    /// Note - If data directory doesn't exist, this function will search for the first existing parent since user should inherit rights from that container.
    /// </summary>
    /// <param name="username">Windows username in format DOMAIN\USER. If DOMAIN is omitted, local machine is assumed.</param>
    /// <param name="password">Windows user's password.</param>
    /// <param name="dataDirectory">The directory the Windows service will be writing information to.</param>
    /// <returns>A <see cref="ServiceAccountMissingRequirement"/> which indicates what if anything is missing for the given user.</returns>
    public static ServiceAccountMissingRequirement ValidateServiceAccountUser(string username, string password, string dataDirectory)
    {
      var missingRequirement = ServiceAccountMissingRequirement.Undefined;
      string[] domainUser = username.Contains("\\") ? username.Split('\\') : new[] { ".", username };
      try
      {
        if (Win32.LogonUser(domainUser[1], domainUser[0], password, Win32.LOGON32_LOGON_SERVICE, Win32.LOGON32_PROVIDER_DEFAULT, out var safeTokenHandle) == false)
        {
          throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        var impersonatedUserId = new WindowsIdentity(safeTokenHandle.DangerousGetHandle());
        var impersonatedUserContext = impersonatedUserId.Impersonate();

        // Check for file permissions. Rely on inheritance for directories that don't yet exist.
        while (!Directory.Exists(dataDirectory))
        {
          dataDirectory = dataDirectory.Substring(0, dataDirectory.LastIndexOf(Path.DirectorySeparatorChar));
        }

        string randomFileName = Path.Combine(dataDirectory, Path.GetRandomFileName());
        File.WriteAllText(randomFileName, string.Empty);
        File.Delete(randomFileName);

        impersonatedUserContext.Dispose();
        impersonatedUserId.Dispose();
        missingRequirement = ServiceAccountMissingRequirement.None;
      }
      catch (Exception ex)
      {
        switch (ex)
        {
          case Win32Exception win32Exception:
            switch (win32Exception.NativeErrorCode)
            {
              case (int)Win32.SystemErrorCodes.ERROR_LOGON_FAILURE:
                missingRequirement = ServiceAccountMissingRequirement.MatchedUsernamePassword;
                break;

              case (int)Win32.SystemErrorCodes.ERROR_LOGON_TYPE_NOT_GRANTED:
                missingRequirement = ServiceAccountMissingRequirement.LogonAsAServiceRight;
                break;
            }

            break;

          case UnauthorizedAccessException _:
            missingRequirement = ServiceAccountMissingRequirement.DirectoryPermissions;
            break;
        }

        if (missingRequirement == ServiceAccountMissingRequirement.Undefined)
        {
          Logger.LogException(ex);
        }
      }

      return missingRequirement;
    }

    /// <summary>
    /// Validates a given Windows service name.
    /// </summary>
    /// <param name="serviceName">The service name.</param>
    /// <returns>An error message if invalid, otherwise <c>null</c>.</returns>
    public static string ValidateServiceName(string serviceName)
    {
      if (string.IsNullOrWhiteSpace(serviceName))
      {
        return Resources.WindowsServiceNameNullOrEmptyError;
      }

      if (serviceName.Any(c => c == '/' || c == '\\'))
      {
        return Resources.WindowsServiceNameContainsSlashesError;
      }

      if (Encoding.ASCII.GetBytes(serviceName).Any(b => b < 32))
      {
        return Resources.WindowsServiceNameContainsInvalidCharactersError;
      }

      return !ServiceNameIsAvailable(serviceName)
        ? Resources.WindowsServiceNameInUseError
        : null;
    }
  }
}

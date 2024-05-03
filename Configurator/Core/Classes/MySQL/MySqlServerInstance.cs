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
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Text;
using System.Text.RegularExpressions;
using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Core.Controllers;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Properties;
using MySql.Configurator.Wizards.Server;
using MySql.Data.MySqlClient;
using MySql.Data.MySqlClient.Authentication;
using MySql.Configurator.Core.Common;

namespace MySql.Configurator.Core.Classes.MySql
{
  /// <summary>
  /// Contains information about a MySQL Server instance.
  /// </summary>
  public class MySqlServerInstance
  {
    #region Constants

    /// <summary>
    /// The default name assigned to data directories.
    /// </summary>
    public const string DEFAULT_DATADIR_NAME_REGEX = @"MySQL Server (?<Series>\d{1,3}\.\d{1,3})";

    /// <summary>
    /// The MySQL default port.
    /// </summary>
    public const int DEFAULT_PORT = 3306;

    /// <summary>
    /// The default name assigned to windows service names.
    /// </summary>
    public const string DEFAULT_SERVICE_NAME_REGEX = @"^MySQL(?<Series>\d\d)$";

    /// <summary>
    /// The regex string to validate host names.
    /// </summary>
    public const string HOSTNAME_REGEX_VALIDATION = @"^(([a-zA-Z]|[a-zA-Z][a-zA-Z0-9-]*[a-zA-Z0-9]).)*([A-Za-z]|[A-Za-z][A-Za-z0-9-]*[A-Za-z0-9])$";

    /// <summary>
    /// The maximum length allowed for MySQL schemas and tables.
    /// </summary>
    public const int MAX_MYSQL_SCHEMA_OR_TABLE_NAME_LENGTH = 64;

    /// <summary>
    /// The maximum port number allowed.
    /// </summary>
    public const uint MAX_PORT_NUMBER_ALLOWED = ushort.MaxValue;

    /// <summary>
    /// The maximum port number allowed.
    /// </summary>
    public const uint MIN_PORT_NUMBER_ALLOWED = 1;

    /// <summary>
    /// The minimum port number allowed for MySQL connections.
    /// </summary>
    public const uint MIN_MYSQL_PORT_NUMBER_ALLOWED = 80;

    /// <summary>
    /// The <see cref="MySqlException"/> number related to an expired password error.
    /// </summary>
    public const int MYSQL_EXCEPTION_NUMBER_EXPIRED_PASSWORD = 1820;

    /// <summary>
    /// The <see cref="MySqlException"/> number related to an unsuccessful connection to a MySQL Server instance.
    /// </summary>
    public const int MYSQL_EXCEPTION_NUMBER_SERVER_UNREACHABLE = 1042;

    /// <summary>
    /// The <see cref="MySqlException"/> number related to a wrong password error.
    /// </summary>
    public const int MYSQL_EXCEPTION_NUMBER_WRONG_PASSWORD = 0;

    /// <summary>
    /// The regex used to validate MySQL user and cluster names.
    /// </summary>
    public const string NAME_REGEX_VALIDATION = @"^(\w|\d|_|\s)+$";

    /// <summary>
    /// The minimum suggested length for a password.
    /// </summary>
    public const int PASSWORD_MIN_LENGTH = 4;

    /// <summary>
    /// The waiting time in milliseconds between connection attempts.
    /// </summary>
    public const int WAITING_TIME_BETWEEN_CONNECTIONS_IN_MILLISECONDS = 3000;

    #endregion Constants

    #region Fields

    /// <summary>
    /// The base directory containing the MySQL Server instance installation files.
    /// </summary>
    private string _baseDir;

    /// <summary>
    /// The directory where the MySQL Server instance stores the data files.
    /// </summary>
    private string _dataDir;

    /// <summary>
    /// The member role of this instance in a group replication cluster.
    /// </summary>
    private GroupReplicationMemberRoleType _groupReplicationMemberRole;

    /// <summary>
    /// The ID of the process associated with this MySQL Server instance.
    /// </summary>
    private int _processId;

    /// <summary>
    /// The running <seealso cref="Process"/> associated with this Server instance.
    /// </summary>
    private Process _runningProcess;

    /// The version number of the instance.
    /// </summary>
    private Version _serverVersion;

    /// <summary>
    /// The MySQL service control manager associated to Windows services related to this instance (if any).
    /// </summary>
    private MySqlServiceControlManager _serviceControlManager;

    /// <summary>
    /// A dictionary with versions and variables that have been removed on each one since version 8.0.0.
    /// </summary>
    private static Dictionary<Version, List<string>> _removedVariables;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlServerInstance"/> class.
    /// </summary>
    /// <param name="port">The port where this instance listens for connections.</param>
    /// <param name="reportStatusDelegate">An <seealso cref="System.Action"/> to output status messages.</param>
    public MySqlServerInstance(uint port, Action<string> reportStatusDelegate = null)
    {
      _groupReplicationMemberRole = GroupReplicationMemberRoleType.Unknown;
      ConnectionProtocol = MySqlConnectionProtocol.Tcp;
      DisableReportStatus = false;
      PipeOrSharedMemoryName = null;
      Port = port;
      ReportStatusDelegate = reportStatusDelegate;
      AllowPublicKeyRetrieval = false;
      SslMode = MySqlSslMode.Preferred;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlServerInstance"/> class.
    /// </summary>
    /// <param name="port">The port where this instance listens for connections.</param>
    /// <param name="userAccount">The <see cref="MySqlServerUser"/> to establish connections.</param>
    /// <param name="reportStatusDelegate">An <seealso cref="System.Action"/> to output status messages.</param>
    public MySqlServerInstance(uint port, MySqlServerUser userAccount, Action<string> reportStatusDelegate = null)
      : this(port, reportStatusDelegate)
    {
      UserAccount = userAccount;
    }

    #region Properties

    /// <summary>
    /// Gets a dictionary with versions and variables that have been removed on each one since version 8.0.0.
    /// </summary>
    public static Dictionary<Version, List<string>> RemovedVariables
    {
      get
      {
        if (_removedVariables == null)
        {
          _removedVariables = new Dictionary<Version, List<string>>
          {
            {
              new Version(8, 0, 0),
              new List<string>() { "bootstrap",
                                   "com_alter_db_upgrade",
                                   "ignore-db-dir",
                                   "ignore_db_dirs",
                                   "innodb_checksums",
                                   "innodb_disable_resize_buffer_pool_debug",
                                   "innodb_file_format",
                                   "innodb_file_format_check",
                                   "innodb_file_format_max",
                                   "innodb_large_prefix",
                                   "innodb_locks_unsafe_for_binlog",
                                   "innodb_stats_sample_pages",
                                   "innodb_support_xa",
                                   "partition",
                                   "skip-partition",
                                   "sync_frm" }
            },
            {
              new Version(8, 0, 1),
              new List<string>() { "show_compatibility_56",
                                   "slave_heartbeat_period",
                                   "slave_last_heartbeat",
                                   "slave_received_heartbeats",
                                   "slave_retried_transactions",
                                   "slave_running",
                                   "temp-pool" }
            },
            {
              new Version(8, 0, 2),
              new List<string>() { "innodb_available_undo_logs",
                                   "innodb_undo_logs" }
            },
            {
              new Version(8, 0, 3),
              new List<string>() { "date_format",
                                   "datetime_format",
                                   "have_crypt",
                                   "des-key-file",
                                   "ignore_builtin_innodb",
                                   "log-warnings",
                                   "max_tmp_tables",
                                   "multi_range_count",
                                   "qcache_free_blocks",
                                   "qcache_free_memory",
                                   "qcache_hits",
                                   "qcache_inserts",
                                   "qcache_lowmem_prunes",
                                   "qcache_not_cached",
                                   "qcache_queries_in_cache",
                                   "qcache_total_blocks",
                                   "query_cache_limit",
                                   "query_cache_min_res_unit",
                                   "query_cache_size",
                                   "query_cache_type",
                                   "query_cache_wlock_invalidate",
                                   "secure_auth",
                                   "time_format",
                                   "tx_isolation",
                                   "tx_read_only" }
            },
            {
              new Version(8, 0, 4),
              new List<string>() { "group_replication_allow_local_disjoint_gtids_join",
                                   "innodb_scan_directories",
                                   "log_error_filter_rules" }
            },
            {
              new Version(8, 0, 11),
              new List<string>() { "log_builtin_as_identified_by_password",
                                   "old_passwords" }
            },
            {
              new Version(8, 0, 13),
              new List<string>() { "log_syslog",
                                   "log_syslog_facility",
                                   "log_syslog_include_pid",
                                   "log_syslog_tag",
                                   "metadata_locks_cache_size",
                                   "metadata_locks_hash_instances" }
            },
            { 
              new Version(8, 0, 16),
              new List<string>() { "internal_tmp_disk_storage_engine" }
            },
            { new Version(8, 0, 30),
              new List<string>() { "myisam_repair_threads" }
            }
          };
        }

        return _removedVariables;
      }
    }

    /// <summary>
    /// Flag indicating that RSA public keys should be retrieved from the server.
    /// </summary>
    public bool AllowPublicKeyRetrieval { get; set; }

    /// <summary>
    /// Gets the base directory containing the MySQL Server instance installation files.
    /// </summary>
    public string BaseDir
    {
      get
      {
        if (string.IsNullOrEmpty(_baseDir))
        {
          var boxedBaseDir = ExecuteScalar("SELECT @@basedir;", out var error);
          if (string.IsNullOrEmpty(error))
          {
            _baseDir = boxedBaseDir.ToString();
          }
          else
          {
            Logger.LogError(Resources.ServerInstanceFailedToRetrieveBaseDir);
            Logger.LogError(error);
          }
        }

        return _baseDir;
      }
    }

    /// <summary>
    /// Gets or sets the <see cref="MySqlConnectionProtocol"/> for establishing connections.
    /// </summary>
    public MySqlConnectionProtocol ConnectionProtocol { get; set; }

    /// <summary>
    /// Gets the directory where the MySQL Server instance stores the data files.
    /// </summary>
    public string DataDir
    {
      get
      {
        if (string.IsNullOrEmpty(_dataDir))
        {
          var boxedDataDir = ExecuteScalar("SELECT @@datadir;", out var error);
          if (string.IsNullOrEmpty(error))
          {
            _dataDir = boxedDataDir.ToString();
          }
          else
          {
            Logger.LogError(Resources.ServerInstanceFailedToRetrieveDataDir);
            Logger.LogError(error);
          }
        }

        return _dataDir;
      }

      set { _dataDir = value; }
    }

    /// <summary>
    /// Flag indicating if any reporting of statuses is disabled even if the <seealso cref="ReportStatusDelegate"/> exists.
    /// </summary>
    protected bool DisableReportStatus { get; set; }

    /// <summary>
    /// Gets the member role of this instance in a group replication cluster.
    /// </summary>
    /// <remarks>Accesses the database the first time it is called.
    /// Otherwise, it returns the previously obtained value.</remarks>
    public GroupReplicationMemberRoleType GroupReplicationMemberRole
    {
      get
      {
        if (_groupReplicationMemberRole == GroupReplicationMemberRoleType.Unknown)
        {
          _groupReplicationMemberRole = GetGroupReplicationMemberRole();
        }

        return _groupReplicationMemberRole;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this instance is running and connections can be made.
    /// </summary>
    public bool IsRunning => RunningProcess != null
                             && !RunningProcess.HasExited;

    /// <summary>
    /// Gets a value indicating if the username is valid.
    /// </summary>
    public bool IsUsernameValid
    {
      get
      {
        var errorMessage = ValidateUserName(UserAccount.Username, true);
        if (string.IsNullOrEmpty(errorMessage))
        {
          return true;
        }

        Logger.LogError(errorMessage);
        return false;
      }
    }

    /// <summary>
    /// Gets the name of this instance containing its Server version.
    /// </summary>
    public virtual string NameWithVersion => $"{UserAccount.Host}:{Port}";

    /// <summary>
    /// Gets or sets the name of the Windows pipe or the shared memory to use when establishing connections.
    /// </summary>
    public string PipeOrSharedMemoryName { get; set; }

    /// <summary>
    /// Gets the port where this instance listens for connections.
    /// </summary>
    public uint Port { get; protected set; }

    /// <summary>
    /// Gets the ID of the process associated with this Server instance.
    /// </summary>
    public int ProcessId
    {
      get
      {
        if (_processId == 0)
        {
          _processId = Utilities.GetServerInstanceProcessId(DataDir);
        }

        return _processId;
      }
    }

    /// <summary>
    /// Gets an <seealso cref="System.Action"/> to output status messages.
    /// </summary>
    public Action<string> ReportStatusDelegate { get; set; }

    /// <summary>
    /// Gets a running <seealso cref="Process"/> associated with this Server instance.
    /// </summary>
    public Process RunningProcess
    {
      get
      {
        if (_runningProcess == null && ProcessId > 0)
        {
          _runningProcess = Utilities.GetRunningProcess(ProcessId);
        }

        return _runningProcess;
      }
    }

    /// <summary>
    /// Gets the Server ID of this instance.
    /// </summary>
    public virtual uint ServerId
    {
      get
      {
        const string SQL = "SELECT @@server_id";
        var id = ExecuteScalar(SQL, out var error);
        return string.IsNullOrEmpty(error)
          ? (uint)id
          : 0;
      }
    }

    /// <summary>
    /// Gets the server version number of this instance.
    /// </summary>
    public Version ServerVersion
    {
      get
      {
        if (_serverVersion == null)
        {
          _serverVersion = GetServerVersion();
        }

        return _serverVersion;
      }
    }

    /// <summary>
    /// Gets the MySQL service control manager associated to Windows services related to this instance (if any).
    /// </summary>
    public MySqlServiceControlManager ServiceControlManager
    {
      get
      {
        if (_serviceControlManager == null
            && !string.IsNullOrEmpty(BaseDir))
        {
          _serviceControlManager = new MySqlServiceControlManager(BaseDir);
        }

        return _serviceControlManager;
      }
    }

    /// <summary>
    /// Flag to specifiy the desired security state of the connection to the server.
    /// </summary>
    public MySqlSslMode SslMode { get; set; }

    /// <summary>
    /// Gets the <see cref="MySqlServerUser"/> to establish connections.
    /// </summary>
    public MySqlServerUser UserAccount { get; set; }

    #endregion Properties

    /// <summary>
    /// Checks if the name of the data directory matches the default name assigned by the configurator.
    /// </summary>
    /// <param name="checkForVersion">If not <c>null</c>, it also checks the first 2 digits of the given version are not used in the name.</param>
    /// <returns><c>true</c> if name of the data directory matches the default name assigned by the configurator, <c>false</c> otherwise.</returns>
    public bool IsDataDirNameDefault(Version differentToVersion = null)
    {
      if (string.IsNullOrEmpty(DataDir))
      {
        throw new Exception(Resources.ServerInstanceFailedToRetrieveDataDir);
      }

      var parentFolder = new DirectoryInfo(DataDir).Parent.Name;
      var match = Regex.Match(parentFolder, DEFAULT_DATADIR_NAME_REGEX, RegexOptions.IgnoreCase);
      return differentToVersion != null ? match.Success && match.Groups["Series"].Value != differentToVersion.ToString(2) : match.Success;
    }

    /// <summary>
    /// Verifies if a given host name represents a local connection.
    /// </summary>
    /// <param name="hostName">A host name.</param>
    /// <returns><c>true</c> if the given host name represents a local connection, <c>false</c> otherwise.</returns>
    public static bool IsHostLocal(string hostName)
    {
      if (string.IsNullOrEmpty(hostName))
      {
        return false;
      }

      var localHostEntry = Dns.GetHostEntry(Dns.GetHostName());
      try
      {
        var hostEntry = Dns.GetHostEntry(hostName);
        return hostEntry.AddressList.Any(ipAddress => IPAddress.IsLoopback(ipAddress)
                                                      || localHostEntry.AddressList.Any(ipAddress.Equals));
      }
      catch
      {
        return false;
      }
    }

    /// <summary>
    /// Determines whether a given executable path contains a call to a MySQL Server executable.
    /// </summary>
    /// <param name="executablePath">The path to the executable program.</param>
    /// <returns><c>true</c> if the given executable path contains a call to a MySQL Server executable, <c>false</c> otherwise.</returns>
    public static bool IsMySqlServerExecutable(string executablePath)
    {
      if (string.IsNullOrEmpty(executablePath))
      {
        return false;
      }

      var args = Utilities.SplitArgs(executablePath);
      if (args.Length <= 0)
      {
        return false;
      }

      var exeName = args[0];
      return exeName.EndsWith("mysqld.exe")
             || exeName.EndsWith("mysqld-nt.exe")
             || exeName.EndsWith("mysqld")
             || exeName.EndsWith("mysqld-nt");
    }

    /// <summary>
    /// Validates if the provided service name follows the default naming convention.
    /// </summary>
    /// <param name="serviceName">The service name.</param>
    /// <param name="differentToVersion">Flag to indicate if the comparison should validate that the service doesn't match the provided version.</param>
    /// <returns><c>true</c> if the service name follows the default naming convention; otherwise, <c>false</c>.</returns>
    public bool IsServiceNameDefault(string serviceName, Version differentToVersion = null)
    {
      if (string.IsNullOrEmpty(serviceName))
      {
        return false;
      }

      var match = Regex.Match(serviceName, DEFAULT_SERVICE_NAME_REGEX, RegexOptions.IgnoreCase);
      return differentToVersion != null ? match.Success && match.Groups["Series"].Value != differentToVersion.ToString(2) : match.Success;
    }

    /// <summary>
    /// Validates that the given host name or IP address is well formed.
    /// </summary>
    /// <param name="hostNameOrIpAddress">A host name or IP address.</param>
    /// <param name="validHostNameType">The type of hostnames to validate against (a combination of flags can be given).</param>
    /// <returns>An empty string if the host name or IP address is well formed, otherwise an error message.</returns>
    public static string ValidateHostNameOrIpAddress(string hostNameOrIpAddress, ValidHostNameType validHostNameType = ValidHostNameType.DNS | ValidHostNameType.IPv4)
    {
      if (string.IsNullOrWhiteSpace(hostNameOrIpAddress))
      {
        return Resources.MySqlServerInstanceRequiredHostOrIpError;
      }

      return !validHostNameType.HasFlag(Uri.CheckHostName(hostNameOrIpAddress).ToValidHostNameType())
        ? Resources.MySqlServerInstanceInvalidHostOrIpError
        : string.Empty;
    }

    /// <summary>
    /// Validates that the given user password meets requirements.
    /// </summary>
    /// <param name="password">A MySQL cluster name.</param>
    /// <param name="validateBlank"></param>
    /// <returns>An empty string if the password meets requirements, otherwise an error message.</returns>
    public static string ValidatePassword(string password, bool validateBlank)
    {
      if (validateBlank && string.IsNullOrWhiteSpace(password))
      {
        return Resources.MySqlServerPasswordRequired;
      }

      if (password.Length < PASSWORD_MIN_LENGTH)
      {
        return Resources.MySqlServerPasswordNotGoodEnough;
      }

      return string.Empty;
    }

    /// <summary>
    /// Validates a Windows pipe or shared memory stream.
    /// </summary>
    /// <param name="namedPipe">Flag indicating if the name is for a named pipe or shared memory.</param>
    /// <param name="name">A name for either a Windows pipe or shared memory stream.</param>
    /// <returns>An empty string if the pipe or shared memory name is valid, otherwise an error message.</returns>
    public static string ValidatePipeOrSharedMemoryName(bool namedPipe, string name)
    {
      var element = namedPipe ? "pipe" : "shared memory";
      var errorMessage = string.Empty;
      if (string.IsNullOrWhiteSpace(name))
      {
        errorMessage = Resources.PipeOrSharedMemoryNameRequiredError;
      }
      else if (name.Length > 256)
      {
        errorMessage = Resources.PipeOrSharedMemoryNameLengthError;
      }
      else if (name.Any(c => c == '\\'))
      {
        errorMessage = Resources.PipeOrSharedMemoryNameBackSlashesError;
      }

      return string.IsNullOrEmpty(errorMessage)
        ? errorMessage
        : string.Format(errorMessage, element);
    }

    /// <summary>
    /// Validates the given port number.
    /// </summary>
    /// <param name="port">A text representation of the port number.</param>
    /// <param name="validateNotInUse">Check if the port is not already being used.</param>
    /// <param name="oldPort">An optional port already configured, if validating not in use but the given port equals the old port no error message is returned.</param>
    /// <param name="validateMySqlPort">Flag indicating whether the given port is to be used with a MySQL Server or with any other TCP/IP port.</param>
    /// <returns>An empty string if the port is a number and within a valid range, otherwise an error message.</returns>
    public static string ValidatePortNumber(string port, bool validateNotInUse, uint? oldPort = null, bool validateMySqlPort = true)
    {
      if (string.IsNullOrWhiteSpace(port))
      {
        return Resources.MySqlServerPortNumberRequired;
      }

      var isValid = uint.TryParse(port, out var numericPort);
      if (!isValid)
      {
        return Resources.MySqlServerPortNumberInvalid;
      }

      if (validateMySqlPort ? numericPort < MIN_MYSQL_PORT_NUMBER_ALLOWED : numericPort < MIN_PORT_NUMBER_ALLOWED
          || numericPort > MAX_PORT_NUMBER_ALLOWED)
      {
        return string.Format(Resources.MySqlServerInvalidPortRange, MIN_MYSQL_PORT_NUMBER_ALLOWED, MAX_PORT_NUMBER_ALLOWED);
      }

      if (validateNotInUse
          && !Utilities.PortIsAvailable(numericPort)
          && oldPort.HasValue
          && oldPort.Value != numericPort)
      {
        return Resources.MySqlServerPortInUse;
      }

      return string.Empty;
    }

    /// <summary>
    /// Validates the given MySQL schema or table name.
    /// </summary>
    /// <param name="name">A MySQL schema or table name.</param>
    /// <returns>An empty string if the MySQL schema or table name is valid, otherwise an error message.</returns>
    public static string ValidateSchemaOrTableName(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        return Resources.MySqlSchemaTableNameEmptyOrWhiteSpaceError;
      }

      if (name.Length > MAX_MYSQL_SCHEMA_OR_TABLE_NAME_LENGTH)
      {
        return string.Format(Resources.MySqlSchemaTableNameExceedsMaxLengthError, MAX_MYSQL_SCHEMA_OR_TABLE_NAME_LENGTH);
      }

      if (name.EndsWith(" "))
      {
        return Resources.MySqlSchemaTableNameEndsWithWhiteSpaceError;
      }

      if (name.All(char.IsDigit))
      {
        return Resources.MySqlSchemaTableNameAllDigitsError;
      }

      return string.Empty;
    }

    /// <summary>
    /// Validates that the given MySQL user name is well formed.
    /// </summary>
    /// <param name="username">A MySQL user name.</param>
    /// <param name="allowRoot">Flag indicating if root is allowed or an error message is thrown.</param>
    /// <returns>An empty string if the user name is well formed, otherwise an error message.</returns>
    public static string ValidateUserName(string username, bool allowRoot)
    {
      return MySqlServerUser.ValidateUserName(username, allowRoot);
    }

    /// <summary>
    /// Checks if a connection to this instance can be established with the credentials in <see cref="UserAccount"/>.
    /// </summary>
    /// <param name="fallbackAuthenticationPlugin">Flag indicating if the connection must be retried with a different authentication plugin if it fails.</param>
    /// <returns>A <see cref="ConnectionResultType"/> value.</returns>
    public ConnectionResultType CanConnect(bool fallbackAuthenticationPlugin)
    {
      var connectionResult = CanConnect();
      if (connectionResult == ConnectionResultType.ConnectionError
          && fallbackAuthenticationPlugin
          && UserAccount.AuthenticationPlugin != MySqlAuthenticationPluginType.None
          && UserAccount.AuthenticationPlugin != MySqlAuthenticationPluginType.Windows)
      {
        var backupAccount = UserAccount;
        var fallbackAccount = UserAccount.Clone() as MySqlServerUser;
        if (fallbackAccount == null)
        {
          return connectionResult;
        }

        fallbackAccount.AuthenticationPlugin = UserAccount.AuthenticationPlugin == MySqlAuthenticationPluginType.CachingSha2Password
                                               || UserAccount.AuthenticationPlugin == MySqlAuthenticationPluginType.Sha256Password
          ? MySqlAuthenticationPluginType.MysqlNativePassword
          : MySqlAuthenticationPluginType.CachingSha2Password;
        UserAccount = fallbackAccount;
        connectionResult = CanConnect();
        UserAccount = backupAccount;
      }

      return connectionResult;
    }

    /// <summary>
    /// Checks if a connection to this instance can be established with the credentials in <see cref="UserAccount"/>.
    /// </summary>
    /// <returns>A <see cref="ConnectionResultType"/> value.</returns>
    public virtual ConnectionResultType CanConnect()
    {
      if (!IsUsernameValid)
      {
        return ConnectionResultType.InvalidUserName;
      }

      ConnectionResultType connectionResult;
      using (var connection = new MySqlConnection(GetConnectionStringBuilder().ConnectionString))
      {
        try
        {
          connection.Open();
          connectionResult = ConnectionResultType.ConnectionSuccess;
        }
        catch (MySqlException mySqlException)
        {
          switch (mySqlException.Number)
          {
            // Connection could not be made.
            case MYSQL_EXCEPTION_NUMBER_SERVER_UNREACHABLE:
              connectionResult = ConnectionResultType.HostUnreachable;
              break;

            // Wrong password.
            case MYSQL_EXCEPTION_NUMBER_WRONG_PASSWORD:
              connectionResult = ConnectionResultType.WrongPassword;
              break;

            // Password has expired so any statement can't be run before resetting the expired password.
            case MYSQL_EXCEPTION_NUMBER_EXPIRED_PASSWORD:
              connectionResult = ConnectionResultType.WrongPassword;
              break;

            // Any other code
            default:
              connectionResult = ConnectionResultType.ConnectionError;
              Logger.LogException(mySqlException);
              break;
          }
        }
        catch (Exception ex)
        {
          connectionResult = ConnectionResultType.ConnectionError;
          Logger.LogException(ex);
        }
      }

      return connectionResult;
    }

    /// <summary>
    /// Executes a query that returns a single value packed as an object.
    /// </summary>
    /// <param name="sqlQuery">A query that returns a single value.</param>
    /// <param name="error">An error message if an error occurred.</param>
    /// <returns>A single value packed as an object.</returns>
    public object ExecuteScalar(string sqlQuery, out string error)
    {
      error = null;
      object result = null;
      using (var connection = new MySqlConnection(GetConnectionStringBuilder().ConnectionString))
      {
        try
        {
          connection.Open();
          result = MySqlHelper.ExecuteScalar(connection, sqlQuery);
        }
        catch (Exception ex)
        {
          error = ex.Message;
        }
      }

      return result;
    }

    /// <summary>
    /// Executes a query that does not return any values.
    /// </summary>
    /// <param name="sqlQuery">A query that returns a single value.</param>
    /// <param name="error">An error message if an error occurred.</param>
    /// <returns>The number of affected records.</returns>
    public int ExecuteNonQuery(string sqlQuery, out string error)
    {
      error = null;
      int affectedRecordsCount = 0;
      using (var connection = new MySqlConnection(GetConnectionStringBuilder().ConnectionString))
      {
        try
        {
          connection.Open();
          affectedRecordsCount = MySqlHelper.ExecuteNonQuery(connection, sqlQuery);
        }
        catch (Exception ex)
        {
          error = ex.Message;
        }
      }

      return affectedRecordsCount;
    }

    /// <summary>
    /// Executes a query that returns a <see cref="DataTable"/>.
    /// </summary>
    /// <param name="sqlQuery">A query that returns a <see cref="DataTable"/>.</param>
    /// <param name="error">An error message if an error occurred.</param>
    /// <returns>A <see cref="DataTable"/> with the query results, or <c>null</c> if an error occurs.</returns>
    public virtual DataTable ExecuteQuery(string sqlQuery, out string error)
    {
      error = null;
      DataTable dataTable = null;
      using (var connection = new MySqlConnection(GetConnectionStringBuilder().ConnectionString))
      {
        try
        {
          connection.Open();
          dataTable = Utilities.GetTableFromQuery(connection, sqlQuery);
        }
        catch (Exception ex)
        {
          error = ex.Message;
        }
      }

      return dataTable;
    }

    /// <summary>
    /// Executes the given SQL scripts connecting to this instance.
    /// </summary>
    /// <param name="outputScriptToStatus">Flag indicating whether feedback about the scripts being executed is sent to the output.</param>
    /// <param name="sqlScripts">An array of SQL scripts to execute.</param>
    /// <returns>The number of scripts that executed successfully.</returns>
    public virtual int ExecuteScripts(bool outputScriptToStatus, params string[] sqlScripts)
    {
      if (sqlScripts.Length == 0
          || !IsUsernameValid)
      {
        return 0;
      }

      int successfulScriptsCount = 0;
      using (var connection = new MySqlConnection(GetConnectionStringBuilder().ConnectionString))
      {
        try
        {
          connection.Open();
        }
        catch
        {
          ReportStatus(string.Format(Resources.MySqlServerInstanceInfoExecuteScriptsCannotConnectError, NameWithVersion, Port));
          return 0;
        }

        var mySqlScript = new MySqlScript(connection);
        foreach (var sqlScript in sqlScripts.Where(sqlScript => !string.IsNullOrEmpty(sqlScript)))
        {
          try
          {
            if (outputScriptToStatus)
            {
              ReportStatus(string.Format("{0}{1}{2}{1}{1}", Resources.MySqlServerInstanceInfoExecutingScript, Environment.NewLine, sqlScript));
            }

            mySqlScript.Query = sqlScript;
            mySqlScript.Execute();
            successfulScriptsCount++;
            if (outputScriptToStatus)
            {
              ReportStatus(Resources.MySqlServerInstanceInfoExecuteScriptExecutionSuccess);
            }
          }
          catch (Exception e)
          {
            ReportStatus(string.Format(Resources.MySqlServerInstanceInfoExecuteScriptsExecutionError, e.Message, Environment.NewLine));
          }
        }
      }

      return successfulScriptsCount;
    }

    /// <summary>
    /// Gets the connection string builder used to establish a connection to this instance.
    /// </summary>
    /// <param name="schemaName">The name of the default schema to work with.</param>
    /// <returns>The connection string builder used to establish a connection to this instance.</returns>
    public virtual MySqlConnectionStringBuilder GetConnectionStringBuilder(string schemaName = null)
    {
      if (UserAccount == null)
      {
        return null;
      }

      var builder = new MySqlConnectionStringBuilder()
      {
        Server = string.IsNullOrEmpty(UserAccount.Host) ? MySqlServerUser.LOCALHOST : UserAccount.Host,
        DefaultCommandTimeout = 120,
        Pooling = false,
        UserID = UserAccount.Username,
        Password = UserAccount.Password,
        ConnectionProtocol = ConnectionProtocol,
        AllowPublicKeyRetrieval = AllowPublicKeyRetrieval,
        SslMode = SslMode
      };

      // Previous versions of Connector/NET had SslMode=None now SslMode=Required is the default value
      // and only works when using a SHA256 authentication plugin. For other plugins it needs to explicitly bet set to None.
      if (UserAccount.AuthenticationPlugin != MySqlAuthenticationPluginType.Sha256Password
          && UserAccount.AuthenticationPlugin != MySqlAuthenticationPluginType.CachingSha2Password)
      {
        builder.SslMode = MySqlSslMode.Disabled;
      }

      if (!string.IsNullOrEmpty(schemaName))
      {
        builder.Database = schemaName;
      }

      switch (ConnectionProtocol)
      {
        case MySqlConnectionProtocol.Tcp:
          builder.Port = Port;
          break;

        case MySqlConnectionProtocol.Pipe:
        case MySqlConnectionProtocol.SharedMemory:
          builder.PipeName = PipeOrSharedMemoryName;
          break;
      }

      return builder;
    }

    /// <summary>
    /// <summary>
    /// Gets a value indicating the member role of this instance in a group replication cluster.
    /// </summary>
    /// <returns>A value indicating the member role of this instance in a group replication cluster.</returns>
    /// <remarks>Forces accesing the database to get the member role.</remarks>
    public GroupReplicationMemberRoleType GetGroupReplicationMemberRole()
    {
      if (ServerVersion == null)
      {
        return GroupReplicationMemberRoleType.Unknown;
      }

      string error;
      if (ServerVersion.ServerSupportsMemberRoleColumn())
      {
        const string SQL = "SELECT `member_role` FROM `performance_schema`.`replication_group_members` AS `rgmems` WHERE `rgmems`.`member_id`= @@server_uuid";
        var status = ExecuteScalar(SQL, out error);
        if (string.IsNullOrEmpty(error))
        {
          // No entries found in the performance_schema.replication_group_members table.
          if (status == null)
          {
            return GroupReplicationMemberRoleType.None;
          }

          return Enum.TryParse(status.ToString(), true, out GroupReplicationMemberRoleType parsed)
            ? parsed
            : GroupReplicationMemberRoleType.Unknown;
        }
      }
      else
      {
        const string COUNT_SQL = "SELECT COUNT(*) FROM `performance_schema`.`replication_group_members` AS `rgmems` WHERE `rgmems`.`member_id`= @@server_uuid";
        if (int.TryParse(ExecuteScalar(COUNT_SQL, out error).ToString(), out var count)
            && string.IsNullOrEmpty(error))
        {
          // No entries found in the performance_schema.replication_group_members table.
          if (count != 1)
          {
            return GroupReplicationMemberRoleType.None;
          }

          // An instance having an entry in the performance_schema.replication_group_members table and set as read_only indicates it is a replica(secondary) instance.
          const string READ_ONLY_SQL = "SELECT @@read_only";
          if (int.TryParse(ExecuteScalar(READ_ONLY_SQL, out error).ToString(), out count)
              && string.IsNullOrEmpty(error))
          {
            return count == 1
              ? GroupReplicationMemberRoleType.Secondary
              : GroupReplicationMemberRoleType.Primary;
          }
        }
      }

      ReportStatus($"{Resources.ServerInstanceGetGroupReplicationMemberRoleError} {error}");
      return GroupReplicationMemberRoleType.Unknown;
    }

    /// <summary>
    /// Gets the server id of the instance.
    /// </summary>
    /// <returns>The server id if it could be successfully retrieved; otherwise <c>null</c>.</returns>
    public uint? GetServerId()
    {
      const string SQL = "SELECT @@server_id";
      var serverid = ExecuteScalar(SQL, out var error);
      if (string.IsNullOrEmpty(error))
      {
        uint.TryParse(serverid.ToString(), out uint parsed);
        return parsed;
      }

      ReportStatus($"{Resources.ServerInstanceGetServerIdError} {error}");
      return null;
    }

    /// <summary>
    /// Gets the value of the specified variable.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="isGlobal">Flag indicating if the variable is global.</param>
    /// <param name="usePerformanceSchema">Flag indicating if the value of the variable should be retrieved from the performance schema</param>
    /// <returns>A string representing the value of the specified variable.</returns>
    public object GetVariable(string name, bool isGlobal, bool usePerformanceSchema = true)
    {
      if (string.IsNullOrEmpty(name))
      {
        return null;
      }

      string sql;
      object value = null;
      string error = string.Empty;
      if (usePerformanceSchema)
      {
        sql = $"SELECT variable_value FROM performance_schema.{(isGlobal ? "global_variables" : "session_variables")} WHERE variable_name = '{name}'";
        value = ExecuteScalar(sql, out error);
      }
      else
      {
        sql = $"SHOW {(isGlobal ? "GLOBAL" : "SESSION")} VARIABLES LIKE '{name}'";
        using (var connection = new MySqlConnection(GetConnectionStringBuilder().ConnectionString))
        {
          try
          {
            connection.Open();
            var reader = MySqlHelper.ExecuteReader(connection, sql);
            reader.Read();
            value = reader["Value"];
          }
          catch (Exception ex)
          {
            error = ex.Message;
          }
        }
      }

      if (string.IsNullOrEmpty(error))
      {
        return value;
      }

      ReportStatus($"{string.Format(Resources.ServerInstanceGetVariableFail, name)} {error}");
      return null;
    }

    /// <summary>
    /// Gets the authentication plugin associated to the specified user.
    /// </summary>
    /// <param name="userName">The MySQL user name.</param>
    /// <returns></returns>
    public MySqlAuthenticationPluginType GetUserAuthenticationPlugin(string userName)
    {
      if (string.IsNullOrEmpty(userName))
      {
        throw new ArgumentNullException(nameof(userName));
      }

      using (var connection = new MySqlConnection(GetConnectionStringBuilder().ConnectionString))
      {
        try
        {
          connection.Open();
          var reader = MySqlHelper.ExecuteReader(connection, $"SELECT plugin FROM mysql.user WHERE User='{userName}'");
          reader.Read();
          var result = reader["plugin"].ToString();
          if (!string.IsNullOrEmpty(result))
          {
            var plugin = MySqlAuthenticationPluginType.None;
            plugin.TryParseFromDescription(result, true, out plugin);
            return plugin;
          }

          return MySqlAuthenticationPluginType.None;
        }
        catch (Exception ex)
        {
          Logger.LogException(ex);
          return MySqlAuthenticationPluginType.None;
        }
      }
    }

    /// <summary>
    /// Kills this MySQL Server instance's related process.
    /// </summary>
    public void KillInstanceProcess()
    {
      if (_runningProcess == null)
      {
        ReportStatus(string.Format(Resources.ProcessNotRunningText, NameWithVersion));
        return;
      }

      var processId = _runningProcess.Id;
      ReportStatus(string.Format(Resources.StoppingProcessText, NameWithVersion, processId));
      if (!_runningProcess.HasExited)
      {
        _runningProcess.Kill();
        _runningProcess.WaitForExit();
      }

      _runningProcess.Dispose();
      _runningProcess = null;
      ReportStatus(string.Format(Resources.StoppedProcessText, NameWithVersion, processId));
    }

    /// <summary>
    /// Executes a RESET PERSIST statement for the specified variable.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="ifExists">Flag indicating if the IF EXISTS string is concatenated to the RESET PERSIST statement.</param>
    /// <returns><c>true</c> if the operation completed successfully; otherwise, <c>false</c>.</returns>
    public bool ResetPersistentVariable(string name, bool ifExists)
    {
      if (string.IsNullOrEmpty(name))
      {
        return false;
      }

      string sql = $"RESET PERSIST{(ifExists ? " IF EXISTS" : string.Empty)} {name}";
      ExecuteNonQuery(sql, out var error);
      if (string.IsNullOrEmpty(error))
      {
        return true;
      }

      ReportStatus($"{string.Format(Resources.ServerInstanceResetPersistenceFail, name)} {error}");
      return false;
    }

    /// <summary>
    /// Resets the running process.
    /// </summary>
    public void ResetRunningProcess()
    {
      if (_runningProcess == null)
      {
        return;
      }

      _runningProcess = null;
      _processId = 0;
    }

    /// <summary>
    /// Sets the value of the specified variable.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">The value of the variable.</param>
    /// <param name="isGlobal">Flag indicating if the variable is global.</param>
    /// <returns><c>true</c> if setting the variable was successful; otherwise, <c>false</c>.</returns>
    public bool SetVariable(string name, object value, bool isGlobal)
    {
      if (string.IsNullOrEmpty(name)
          || value == null)
      {
        return false;
      }

      string sql = $"SET {(isGlobal ? "GLOBAL" : string.Empty)} {name}={value}";
      ExecuteNonQuery(sql, out var error);
      if (string.IsNullOrEmpty(error))
      {
        return true;
      }

      ReportStatus($"{string.Format(Resources.ServerInstanceSetVariableFail, name)} {error}");
      return false;
    }

    /// <summary>
    /// Attempts to connect to the Server instance and do a graceful shutdown before stopping it.
    /// </summary>
    /// <returns><c>true</c> if the Server is stopped (gracefully or not), <c>false</c> otherwise.</returns>
    public bool ShutdownInstance()
    {
      if (UserAccount != null && !string.IsNullOrEmpty(BaseDir))
      {
        var tempConfigFileWithPassword = Utilities.CreateTempConfigurationFile(IniFile.IniFile.GetClientPasswordLines(UserAccount.Password));
        var sendingPasswordInCommandLine = tempConfigFileWithPassword == null;
        var connectionOptions = sendingPasswordInCommandLine
          ? $"--password={UserAccount.Password} "
          : $"--defaults-extra-file=\"{tempConfigFileWithPassword}\" ";
        connectionOptions += GetCommandLineConnectionOptions(false);
        ReportStatus(Resources.ServerShutdownSettingInnoDbFastShutdown);
        var result = Utilities.RunProcess(
          Path.Combine(BaseDir, ServerProductConfigurationController.BINARY_DIRECTORY_NAME, ServerProductConfigurationController.CLIENT_EXECUTABLE_FILENAME),
          $" {connectionOptions} -e\"SET GLOBAL innodb_fast_shutdown = 0\"",
          null,
          ReportStatus,
          ReportStatus,
          true);
        ReportStatus(result.ExitCode == 0
          ? Resources.ServerShutdownSettingInnoDbFastShutdownSuccess
          : Resources.ServerShutdownSettingInnoDbFastShutdownError);

        ReportStatus(Resources.ServerShutdownMySqlAdminShutDown);
        result = Utilities.RunProcess(
          Path.Combine(BaseDir, ServerProductConfigurationController.BINARY_DIRECTORY_NAME, ServerProductConfigurationController.ADMIN_TOOL_EXECUTABLE_FILENAME),
          $" {connectionOptions} shutdown",
          null,
          ReportStatus,
          ReportStatus,
          true);
        ReportStatus(result.ExitCode == 0
          ? Resources.ServerShutdownMySqlAdminShutDownSuccess
          : Resources.ServerShutdownMySqlAdminShutDownError);
        Utilities.DeleteFile(tempConfigFileWithPassword, 10, 500);
      }

      StopInstance();
      return WaitUntilNotRunning(1, 30);
    }

    /// <summary>
    /// Stops this Server instance as previously configured (Windows Service or process).
    /// </summary>
    public virtual void StopInstance()
    {
      if (!IsRunning)
      {
        return;
      }

      ReportStatus(Resources.StoppingServerInstanceText);
      try
      {
        if (ServiceControlManager != null)
        {
          var serviceName = ServiceControlManager.GetBestServiceNameMatchingConfigFileDirectory(DataDir);
          if (!string.IsNullOrEmpty(serviceName))
          {
            ReportStatus(Resources.ServerInstanceStoppingWindowsServiceText);
            MySqlServiceControlManager.Stop(serviceName);
            ReportStatus(Resources.ServerInstanceStoppedWindowsServiceText);
          }
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
    /// Updates the authentication plugin of the specified user. 
    /// </summary>
    /// <param name="userName">The MySQL user name.</param>
    /// <param name="password">The password of the MySQL user.</param>
    /// <param name="authenticationPlugin">The authentication plugin currently assigned to the MySQL user.</param>
    public void UpdateUserAuthenticationPlugin(string userName, string password, MySqlAuthenticationPluginType authenticationPlugin)
    {
      if (string.IsNullOrEmpty(userName))
      {
        throw new ArgumentNullException(nameof(userName));
      }

      if (string.IsNullOrEmpty(password))
      {
        throw new ArgumentNullException(nameof(password));
      }

      var result = -1;
      var connectionString = GetConnectionStringBuilder().ConnectionString;
      using (var connection = new MySqlConnection(connectionString))
      {
        connection.Open();
        var sql = $"ALTER USER '{userName}'@'localhost' IDENTIFIED WITH {authenticationPlugin.GetDescription()} BY '{password}'";
        var cmd = new MySqlCommand(sql, connection);
        result = cmd.ExecuteNonQuery();
        cmd.FlushPrivileges();
      }
      
      if (result == -1)
      {
        throw new ConfiguratorException(ConfiguratorError.AuthenticationPluginUpdateFailed);
      }
    }

    /// <summary>
    /// Outputs a status message using the <seealso cref="ReportStatusDelegate"/>.
    /// </summary>
    /// <param name="statusMessage">The status message.</param>
    protected void ReportStatus(string statusMessage)
    {
      if (DisableReportStatus
          || ReportStatusDelegate == null
          || string.IsNullOrEmpty(statusMessage))
      {
        return;
      }

      ReportStatusDelegate(statusMessage);
    }

    /// <summary>
    /// Determines whether the server is running, specifying time to wait and certain number of retries until it is not accepting connections.
    /// </summary>
    /// <param name="waitingSeconds">The waiting time in seconds.</param>
    /// <param name="maxRetries">The maximum number of retries.</param>
    /// <returns><c>true</c> if the instance is not running, <c>false</c> if running even when exhausting the number of retries.</returns>
    protected bool WaitUntilNotRunning(int waitingSeconds, int maxRetries)
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

    /// <summary>
    /// Assembles a connection options string for a MySQL command line program like the MySQL client.
    /// </summary>
    /// <param name="includePassword">Flag indicating whether the password is to be included in the options.</param>
    /// <returns>A connection options string for a MySQL command line program like the MySQL client.</returns>
    private string GetCommandLineConnectionOptions(bool includePassword)
    {
      if (UserAccount == null)
      {
        return string.Empty;
      }

      var builder = new StringBuilder("--user=");
      builder.Append(UserAccount.Username);
      if (includePassword
          && !string.IsNullOrEmpty(UserAccount.Password))
      {
        builder.Append(" --password=");
        builder.Append(UserAccount.Password);
      }

      if (ServerVersion.ServerSupportsCachingSha2Authentication())
      {
        builder.Append(" --default-auth=");
        builder.Append(UserAccount.AuthenticationPlugin.GetDescription());
      }

      if (ConnectionProtocol == MySqlConnectionProtocol.Tcp && Port > 0)
      {
        builder.Append(" --host=");
        builder.Append(MySqlServerUser.LOCALHOST);
        builder.Append(" --port=");
        builder.Append(Port.ToString());
      }
      else if (ConnectionProtocol == MySqlConnectionProtocol.NamedPipe && !string.IsNullOrEmpty(PipeOrSharedMemoryName))
      {
        builder.Append(" --pipe=");
        builder.Append(PipeOrSharedMemoryName);
      }
      else if (ConnectionProtocol == MySqlConnectionProtocol.SharedMemory && !string.IsNullOrEmpty(PipeOrSharedMemoryName))
      {
        builder.Append(" --shared-memory-base-name=");
        builder.Append(PipeOrSharedMemoryName);
      }

      return builder.ToString();
    }

    /// <summary>
    /// Gets a value indicating the member count for the group replication cluster.
    /// </summary>
    /// <returns>A value indicating the member count for the group replication cluster.</returns>
    private int GetGroupReplicationMemberCount()
    {
      const string SQL = "SELECT COUNT(*) FROM `performance_schema`.`replication_group_members`";
      var count = ExecuteScalar(SQL, out var error);
      if (string.IsNullOrEmpty(error))
      {
        if (count == null)
        {
          return -1;
        }
        return Convert.ToInt32(count.ToString());
      }

      ReportStatus($"An error occurred when trying to retrieve the member count of the cluster: {error}");
      return -1;
    }

    /// <summary>
    /// Gets a value representing the version number of this instance.
    /// </summary>
    /// <returns>The version number of this instance.</returns>
    private Version GetServerVersion()
    {
      const string SQL = "SELECT VERSION()";
      var version = ExecuteScalar(SQL, out var error);
      if (string.IsNullOrEmpty(error))
      {
        var versionString = version.ToString();
        var index = versionString.IndexOf('-');

        return Version.TryParse(index == -1
                                  ? versionString
                                  : versionString.Substring(0, index),
                                out Version parsed)
          ? parsed
          : null;
      }

      ReportStatus($"{Resources.ServerInstanceGetServerVersionError} {error}");
      return null;
    }
  }
}

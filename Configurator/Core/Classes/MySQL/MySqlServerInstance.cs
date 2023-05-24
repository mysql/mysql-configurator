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
using System.Linq;
using System.Net;
using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Properties;
using MySql.Data.MySqlClient;

namespace MySql.Configurator.Core.Classes.MySql
{
  /// <summary>
  /// Contains information about a MySQL Server instance.
  /// </summary>
  public class MySqlServerInstance
  {
    #region Constants

    /// <summary>
    /// The MySQL default port.
    /// </summary>
    public const int DEFAULT_PORT = 3306;

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
    /// The member role of this instance in a group replication cluster.
    /// </summary>
    private GroupReplicationMemberRoleType _groupReplicationMemberRole;
    /// The version number of the instance.
    /// </summary>
    private Version _serverVersion;

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
    /// Flag indicating that RSA public keys should be retrieved from the server.
    /// </summary>
    public bool AllowPublicKeyRetrieval { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="MySqlConnectionProtocol"/> for establishing connections.
    /// </summary>
    public MySqlConnectionProtocol ConnectionProtocol { get; set; }

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
    /// Gets an <seealso cref="System.Action"/> to output status messages.
    /// </summary>
    public Action<string> ReportStatusDelegate { get; }

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
    /// Flag to specifiy the desired security state of the connection to the server.
    /// </summary>
    public MySqlSslMode SslMode { get; set; }

    /// <summary>
    /// Gets the <see cref="MySqlServerUser"/> to establish connections.
    /// </summary>
    public MySqlServerUser UserAccount { get; set; }

    #endregion Properties

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
      var mySqlConnection = new MySqlConnection(GetConnectionStringBuilder().ConnectionString);
      using (mySqlConnection)
      {
        try
        {
          mySqlConnection.Open();
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
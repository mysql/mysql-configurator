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
using System.Linq;
using System.Text.RegularExpressions;
using MySql.Configurator.Base.Classes;
using MySql.Configurator.Base.Enums;
using MySql.Configurator.Core.Controllers;
using MySql.Configurator.Core.Settings;
using MySql.Configurator.Properties;

namespace MySql.Configurator.Core.Server
{
  /// <summary>
  /// Defines MySQL Server users that support authentication mechanisms and roles.
  /// </summary>
  [Serializable]
  public class MySqlServerUser
  {
    #region Constants

    /// <summary>
    /// The commonly used local host.
    /// </summary>
    public const string LOCALHOST = "localhost";

    /// <summary>
    /// The commonly used administrator username.
    /// </summary>
    public const string ROOT_USERNAME = "root";

    /// <summary>
    /// A temporary username that might be used by the MySQL Installer for configuration.
    /// </summary>
    public const string TEMPORARY_USERNAME = "mysqltempinternal";

    /// <summary>
    /// The maximum length allowed for MySQL user names.
    /// </summary>
    public const int USERNAME_MAX_LENGTH = 32;

    #endregion

    #region Fields

    /// <summary>
    /// The user name.
    /// </summary>
    private string _username;

    /// <summary>
    /// The tokens used for Windows authentication.
    /// </summary>
    private string _windowsSecurityTokenList;

    #endregion Fields

    /// Initiliazes a new instance of the <see cref="MySqlServerUser"/> class.
    /// </summary>
    public MySqlServerUser()
    {
      _username = string.Empty;
      Password = string.Empty;
      AuthenticationPlugin = MySqlAuthenticationPluginType.None;
    }

    /// <summary>
    /// Initiliazes a new instance of the <see cref="MySqlServerUser"/> class.
    /// </summary>
    /// <param name="username">The user name for the account.</param>
    /// <param name="password">The password for the account.</param>
    /// <param name="authenticationPlugin">The <see cref="MySqlAuthenticationPluginType"/> used for the account.</param>
    /// <param name="host">The hostname for the account.</param>
    /// <param name="role">The <see cref="Role"/> for setting permissions on the account.</param>
    public MySqlServerUser(string username, string password, MySqlAuthenticationPluginType authenticationPlugin, string host = null, Role role = null)
    {
      _username = username;
      _windowsSecurityTokenList = string.Empty;
      AuthenticationPlugin = authenticationPlugin;
      Host = string.IsNullOrEmpty(host) ? LOCALHOST : host;
      Password = password;
      UserRole = role;
    }

    #region Properties

    /// <summary>
    /// Gets or sets the authentication plugin assigned to new users.
    /// </summary>
    [ControllerSetting("The authentication plugin used for a specific user when connecting to the server.", "authentication_plugin,auth_plugin")]
    public MySqlAuthenticationPluginType AuthenticationPlugin { get; set; }

    /// <summary>
    /// Gets or sets the host names allowed to connect. 
    /// </summary>
    [ControllerSetting("The access restriction to connections originated from a specific host. Host names, IPv4 and IPv6 " +
      "addresses are permitted. The \" % \" and \"_\" wildcards are permitted for matching zero or more characters or a single character " +
      "respectively.", "from_host,host")]
    public string Host { get; set; }

    /// <summary>
    /// Gets or sets the password used to authenticate the current user.
    /// </summary>
    [ControllerSetting("The password used to authenticate a specific user.", "password,pwd")]
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Role"/> that maps to granted permissions on database objects.
    /// </summary>
    [ControllerSetting("The administrative role that grants a set of privileges to the database user.", "role")]
    public Role UserRole { get; set; }

    /// <summary>
    /// Gets or sets the name of the user used to connect to the server.
    /// </summary>
    [ControllerSetting("The name of a user account to be created for the server instance.", "user,user_name")]
    public string Username
    {
      get
      {
        return _username.Sanitize();
      }

      set
      {
        _username = value;
      }
    }

    /// <summary>
    /// Gets or sets a comma separated list of Windows tokens allowed to authenticate to the server.
    /// </summary>
    [ControllerSetting("A comma-separated list of tokens representing Windows users or user groups that are permitted to " +
      "authenticate to the server as the specified MySQL user.", "windows_security_tokens,win_sec_tokens,tokens")]
    public string WindowsSecurityTokenList
    {
      get
      {
        return _windowsSecurityTokenList.Sanitize('\\');
      }

      set
      {
        _windowsSecurityTokenList = value;
      }
    }

    #endregion Properties

    /// <summary>
    /// Gets a new instance of the <see cref="MySqlServerUser"/> class for the local root user.
    /// </summary>
    /// <param name="password">The password for the account.</param>
    /// <param name="authenticationPlugin">The <see cref="MySqlAuthenticationPluginType"/> used for the account.</param>
    /// <param name="role">The <see cref="Role"/> for setting permissions on the account.</param>
    /// <returns>A new instance of the <see cref="MySqlServerUser"/> class for the local root user.</returns>
    public static MySqlServerUser GetLocalRootUser(string password, MySqlAuthenticationPluginType authenticationPlugin, Role role = null)
    {
      return new MySqlServerUser(ROOT_USERNAME, password, authenticationPlugin, null, role);
    }

    /// <summary>
    /// Gets a new instance of the <see cref="MySqlServerUser"/> class for a temporary user account used on configurations.
    /// </summary>
    /// <param name="authenticationPlugin">The <see cref="MySqlAuthenticationPluginType"/> used for the account.</param>
    /// <param name="role">The <see cref="Role"/> for setting permissions on the account.</param>
    /// <returns>A new instance of the <see cref="MySqlServerUser"/> class for a temporary user account used on configurations.</returns>
    public static MySqlServerUser GetLocalTemporaryUser(MySqlAuthenticationPluginType authenticationPlugin, Role role = null)
    {
      return new MySqlServerUser(TEMPORARY_USERNAME, new string(TEMPORARY_USERNAME.Reverse().ToArray()), authenticationPlugin, null, role);
    }

    /// <summary>
    /// Validates that all required elements for a user have been provided.
    /// </summary>
    /// <param name="msg">A string representing the error message that will be produced as output.</param>
    /// <returns><c>true</c> if the user is valid; otherwise, <c>false</c>.</returns>
    public bool IsValid(out string msg)
    {
      msg = string.Empty;
      if (AuthenticationPlugin == MySqlAuthenticationPluginType.None)
      {
        msg = Resources.ServerUserMissingAuthenticationPlugin;
        return false;
      }

      if (AuthenticationPlugin == MySqlAuthenticationPluginType.Windows && string.IsNullOrEmpty(WindowsSecurityTokenList))
      {
        msg = Resources.ServerUserMissingWindowsTokensList;
        return false;
      }

      if (string.IsNullOrWhiteSpace(Username))
      {
        msg = Resources.ServerUserMissingUserName;
        return false;
      }

      if (UserRole != null)
      {
        return true;
      }

      msg = Resources.ServerUserMissingUserRole;
      return false;
    }

    /// <summary>
    /// Sets the properties of a user based on the provided value list.
    /// </summary>
    /// <param name="controller">The server configuration controller.</param>
    /// <param name="values">The list of values to set.</param>
    /// <param name="msg">A string representing the error message that will be produced as output.</param>
    /// <returns><c>true</c> if the provided values have been set correctly; otherwise, <c>false</c>.</returns>
    public bool SetValues(ServerConfigurationController controller, ValueList values, out string msg)
    {
      msg = null;
      if (values.Keys == null)
      {
        return false;
      }

      foreach (string key in values.Keys)
      {
        string lowerKey = key.ToLower();
        switch (lowerKey)
        {
          case "user":
          case "username":
            Username = values[key].ToString();
            break;

          case "from_host":
          case "host":
            Host = values[key].ToString();
            break;

          case "role":
            string lowerValue = values[key].ToString().ToLower();
            UserRole = controller.RolesDefined.Roles.Find(name => name.ID.ToLower() == lowerValue);
            if (UserRole == null)
            {
              msg = Properties.Resources.ServerUserRoleNotFound + values[key];
              return false;
            }
            break;

          case "authentication_plugin":
          case "auth_plugin":
            var authenticationPluginText = values[key].ToString();
            MySqlAuthenticationPluginType authenticationPlugin;
            var parsedFromInput = AuthenticationPlugin.TryParseFromDescription(authenticationPluginText, false, out authenticationPlugin);
            if (parsedFromInput)
            {
              AuthenticationPlugin = authenticationPlugin;
            }

            if (AuthenticationPlugin == MySqlAuthenticationPluginType.None)
            {
              AuthenticationPlugin = controller.Settings.DefaultAuthenticationPlugin;
            }

            if (!parsedFromInput)
            {
              msg = Properties.Resources.ServerUserInvalidAuthenticationType + values[key];
              return false;
            }

            break;

          case "password":
          case "pwd":
            Password = values[key].ToString();
            break;

          case "windows_security_tokens":
          case "win_sec_tokens":
          case "tokens":
            WindowsSecurityTokenList = values[key].ToString();
            break;

          case "type":
            // Skip key type since it is only needed to identify a user block.
            break;

          default:
            msg = Properties.Resources.ServerUserInvalidUserBlock + key;
            return false;
        }
      }

      return true;
    }


    /// <summary>
    /// Gets a new instance of the <see cref="MySqlServerUser"/> class for the local root user.
    /// </summary>
    /// <param name="password">The password for the account.</param>
    /// <param name="authenticationPlugin">The <see cref="MySqlAuthenticationPluginType"/> used for the account.</param>
    /// <returns>A new instance of the <see cref="MySqlServerUser"/> class for the local root user.</returns>
    public static MySqlServerUser GetLocalRootUser(string password, MySqlAuthenticationPluginType authenticationPlugin)
    {
      return new MySqlServerUser(ROOT_USERNAME, password, authenticationPlugin);
    }

    /// <summary>
    /// Gets a new instance of the <see cref="MySqlServerUser"/> class for a temporary user account used on configurations.
    /// </summary>
    /// <param name="authenticationPlugin">The <see cref="MySqlAuthenticationPluginType"/> used for the account.</param>
    /// <returns>A new instance of the <see cref="MySqlServerUser"/> class for a temporary user account used on configurations.</returns>
    public static MySqlServerUser GetLocalTemporaryUser(MySqlAuthenticationPluginType authenticationPlugin)
    {
      return new MySqlServerUser(TEMPORARY_USERNAME, new string(TEMPORARY_USERNAME.Reverse().ToArray()), authenticationPlugin);
    }

    /// <summary>
    /// Validates that the given MySQL user name is well formed.
    /// </summary>
    /// <param name="username">A MySQL user name.</param>
    /// <param name="allowRoot">Flag indicating if root is allowed or an error message is thrown.</param>
    /// <returns>An empty string if the user name is well formed, otherwise an error message.</returns>
    public static string ValidateUserName(string username, bool allowRoot)
    {
      if (string.IsNullOrWhiteSpace(username))
      {
        return Resources.MySqlServerUsernameRequired;
      }

      var trimmedUserName = username.Trim();
      if (trimmedUserName.Length > USERNAME_MAX_LENGTH)
      {
        return Resources.MySqlServerUsernameMaxLengthExceeded;
      }

      if (!allowRoot
          && username.Equals(ROOT_USERNAME, StringComparison.OrdinalIgnoreCase))
      {
        return Resources.MySqlServerUserNameInvalidRoot;
      }

      var clusterNameRegex = new Regex(MySqlServerInstance.NAME_REGEX_VALIDATION);
      return clusterNameRegex.IsMatch(trimmedUserName)
        ? string.Empty
        : Resources.MySqlServerUsernameInvalid;
    }

    /// <summary>
    /// Returns a shallow clone of an instance of this class.
    /// </summary>
    /// <returns>A shallow clone of an instance of this class.</returns>
    public object Clone()
    {
      return MemberwiseClone();
    }
  }
}

/* Copyright (c) 2014, 2023, Oracle and/or its affiliates.

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
using System.Linq;
using MySql.Configurator.Core.Controllers;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.MySql;
using MySql.Configurator.Core.Enums;

namespace MySql.Configurator.Wizards.Server
{
  /// <summary>
  /// Defines MySQL Server users that support authentication mechanisms and roles.
  /// </summary>
  [Serializable]
  public class ServerUser : MySqlServerUser
  {
    /// Initiliazes a new instance of the <see cref="ServerUser"/> class.
    /// </summary>
    public ServerUser()
    {
    }

    /// <summary>
    /// Initiliazes a new instance of the <see cref="ServerUser"/> class.
    /// </summary>
    /// <param name="username">The user name for the account.</param>
    /// <param name="password">The password for the account.</param>
    /// <param name="authenticationPlugin">The <see cref="MySqlAuthenticationPluginType"/> used for the account.</param>
    /// <param name="host">The hostname for the account.</param>
    /// <param name="role">The <see cref="Role"/> for setting permissions on the account.</param>
    public ServerUser(string username, string password, MySqlAuthenticationPluginType authenticationPlugin, string host = null, Role role = null)
      : base(username, password, authenticationPlugin, host)
    {
      UserRole = role;
    }

    #region Properties

    /// <summary>
    /// Gets or sets the authentication plugin assigned to new users.
    /// </summary>
    [ControllerSetting("The authentication plugin used for a specific user when connecting to the server.", "authentication_plugin,auth_plugin")]
    public new MySqlAuthenticationPluginType AuthenticationPlugin
    {
      get => base.AuthenticationPlugin;
      set => base.AuthenticationPlugin = value;
    }

    /// <summary>
    /// Gets or sets the host names allowed to connect. 
    /// </summary>
    [ControllerSetting("The access restriction to connections originated from a specific host. Host names, IPv4 and IPv6 " +
      "addresses are permitted. The \" % \" and \"_\" wildcards are permitted for matching zero or more characters or a single character " +
      "respectively.", "from_host,host")]
    public new string Host
    {
      get => base.Host;
      set => base.Host = value;
    }

    /// <summary>
    /// Gets or sets the password used to authenticate the current user.
    /// </summary>
    [ControllerSetting("The password used to authenticate a specific user.", "password,pwd")]
    public new string Password
    {
      get => base.Password;
      set => base.Password = value;
    }

    /// <summary>
    /// Gets or sets the <see cref="Role"/> that maps to granted permissions on database objects.
    /// </summary>
    [ControllerSetting("The administrative role that grants a set of privileges to the database user.", "role")]
    public Role UserRole { get; set; }

    /// <summary>
    /// Gets or sets the name of the user used to connect to the server.
    /// </summary>
    [ControllerSetting("The name of a user account to be created for the server instance.", "user,user_name")]
    public new string Username
    {
      get => base.Username;
      set => base.Username = value;
    }

    /// <summary>
    /// Gets or sets a comma separated list of Windows tokens allowed to authenticate to the server.
    /// </summary>
    [ControllerSetting("A comma-separated list of tokens representing Windows users or user groups that are permitted to " +
      "authenticate to the server as the specified MySQL user.", "windows_security_tokens,win_sec_tokens,tokens")]
    public new string WindowsSecurityTokenList
    {
      get => base.WindowsSecurityTokenList;
      set => base.WindowsSecurityTokenList = value;
    }

    #endregion Properties

    /// <summary>
    /// Gets a new instance of the <see cref="ServerUser"/> class for the local root user.
    /// </summary>
    /// <param name="password">The password for the account.</param>
    /// <param name="authenticationPlugin">The <see cref="MySqlAuthenticationPluginType"/> used for the account.</param>
    /// <param name="role">The <see cref="Role"/> for setting permissions on the account.</param>
    /// <returns>A new instance of the <see cref="ServerUser"/> class for the local root user.</returns>
    public static ServerUser GetLocalRootUser(string password, MySqlAuthenticationPluginType authenticationPlugin, Role role = null)
    {
      return new ServerUser(ROOT_USERNAME, password, authenticationPlugin, null, role);
    }

    /// <summary>
    /// Gets a new instance of the <see cref="ServerUser"/> class for a temporary user account used on configurations.
    /// </summary>
    /// <param name="authenticationPlugin">The <see cref="MySqlAuthenticationPluginType"/> used for the account.</param>
    /// <param name="role">The <see cref="Role"/> for setting permissions on the account.</param>
    /// <returns>A new instance of the <see cref="ServerUser"/> class for a temporary user account used on configurations.</returns>
    public static ServerUser GetLocalTemporaryUser(MySqlAuthenticationPluginType authenticationPlugin, Role role = null)
    {
      return new ServerUser(TEMPORARY_USERNAME, new string(TEMPORARY_USERNAME.Reverse().ToArray()), authenticationPlugin, null, role);
    }

    public override bool IsValid(out string msg)
    {
      if (!base.IsValid(out msg))
      {
        return false;
      }

      if (UserRole != null)
      {
        return true;
      }

      msg = Properties.Resources.ServerUserMissingUserRole;
      return false;
    }

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
  }
}

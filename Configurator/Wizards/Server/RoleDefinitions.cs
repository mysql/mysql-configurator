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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Enums;

namespace MySql.Configurator.Wizards.Server
{
  [Serializable]
  public sealed class RoleDefinitions
  {
    #region Properties

    [XmlAttribute("format")]
    public int Format;

    [XmlArray()]
    public List<Privilege> Privileges;

    [XmlArray]
    public List<Role> Roles;

    [XmlAttribute("version")]
    public int Version;

    [XmlIgnore]
    public Version ServerVersion { get; set; }

    #endregion Properties

    /// <summary>
    /// Gets a <see cref="Role"/> that corresponds to the given <see cref="UserRoleType"/>.
    /// </summary>
    /// <param name="userRole">A <see cref="UserRoleType"/> value.</param>
    /// <returns>A <see cref="Role"/> that corresponds to the given <see cref="UserRoleType"/>.</returns>
    public Role GetRoleByType(UserRoleType userRole)
    {
      return userRole == UserRoleType.Root
        ? Role.GetRootRole()
        : Roles?.FirstOrDefault(r => r.ID == userRole.GetDescription());
    }

    /// <summary>
    /// Builds a SQL statement for a CREATE USER or ALTER USER operation.
    /// </summary>
    /// <param name="type">The <see cref="UserCrudOperationType"/>.</param>
    /// <param name="serverUser">The <see cref="ServerUser"/> to create or update.</param>
    /// <returns>A SQL statement for a CREATE USER or ALTER USER operation.</returns>
    public string GetCreateOrAlterUserSql(UserCrudOperationType type, ServerUser serverUser)
    {
      var builder = new StringBuilder();
      builder.Append(type.GetDescription());
      builder.Append(" '");
      builder.Append(serverUser.Username);
      builder.Append("'@'");
      builder.Append(string.IsNullOrEmpty(serverUser.Host) ? "%" : serverUser.Host);
      builder.Append("' IDENTIFIED ");
      if (serverUser.AuthenticationPlugin == MySqlAuthenticationPluginType.Windows)
      {
        builder.Append("WITH authentication_windows AS '");
        builder.Append(Core.Classes.Utilities.StandardizeToMySqlUserDomainSyntax(serverUser.WindowsSecurityTokenList));
        builder.Append("'");
      }
      else
      {
        if (ServerVersion.ServerSupportsCachingSha2Authentication())
        {
          builder.Append("WITH ");
          builder.Append(serverUser.AuthenticationPlugin.GetDescription());
          builder.Append(" ");
        }

        builder.Append("BY '");
        builder.Append(serverUser.Password.Sanitize());
        builder.Append("'");
      }

      builder.Append(";");
      return builder.ToString();
    }

    /// <summary>
    /// Returns a list of SQL statements to execute to modify privileges for the given <see cref="ServerUser"/>.
    /// </summary>
    /// <param name="serverUser">A <see cref="ServerUser"/>.</param>
    /// <returns>A list of SQL statements to execute to modify privileges for the given <see cref="ServerUser"/>.</returns>
    public List<string> GetUpdateUserSql(ServerUser serverUser)
    {
      var sqlList = new List<string>();
      if (serverUser.UserRole.RolePrivileges != null
          && serverUser.UserRole.RolePrivileges.Count > 0)
      {
        var mysqlUserAssignments = new StringBuilder();
        foreach (var rolePrivilege in serverUser.UserRole.RolePrivileges)
        {
          // Find the corresponding Privilege.Column, verify it's version requirements and add the column to the StringBuilder if applicable.
          var privileges = Privileges.FindAll(priv => priv.Name == rolePrivilege.Name);
          if (privileges.Count == 0)
          {
            continue;
          }

          foreach (var privilege in privileges)
          {
            Regex versionFormat = new Regex(@"^\d+(.\d+){1,3}$");
            if (!string.IsNullOrEmpty(privilege.MinVersion)
                && versionFormat.IsMatch(privilege.MinVersion)
                && (ServerVersion < new Version(privilege.MinVersion)))
            {
              continue;
            }

            if (!string.IsNullOrEmpty(privilege.MaxVersion)
                && versionFormat.IsMatch(privilege.MaxVersion)
                && (ServerVersion > new Version(privilege.MaxVersion)))
            {
              continue;
            }

            if (mysqlUserAssignments.Length != 0)
            {
              mysqlUserAssignments.Append(',');
            }

            mysqlUserAssignments.AppendFormat("{0}='Y'", privilege.Column);
          }
        }

        if (mysqlUserAssignments.Length > 0)
        {
          sqlList.Add($"UPDATE mysql.user SET {mysqlUserAssignments} WHERE User='{serverUser.Username}' AND Host='{serverUser.Host}';");
        }
      }

      if (serverUser.UserRole.GrantedPrivileges != null)
      {
        foreach (var grantedPrivilege in serverUser.UserRole.GrantedPrivileges)
        {
          if (!ServerVersion.ServerIncludesHostTable()
              && grantedPrivilege.Sql.Contains("mysql.host"))
          {
            continue;
          }

          // Starting MySQL 8.0, the mysql.proc and mysql.event tables no longer exists. Instead, the ROUTINES, PARAMETERS and EVENT tables exist in
          // the INFORMATION_SCHEMA db to which all users have access.
          // See more details at: https://dev.mysql.com/doc/refman/8.0/en/information-schema-introduction.html
          if (!ServerVersion.ServerIncludesProcAndEventsTables()
              && (grantedPrivilege.Sql.Contains("mysql.proc")
                  || grantedPrivilege.Sql.Contains("mysql.event")))
          {
            continue;
          }

          sqlList.Add(string.Format(grantedPrivilege.Sql, serverUser.Username, serverUser.Host));
        }
      }

      return sqlList;
    }
  }
}

/* Copyright (c) 2011, 2023, Oracle and/or its affiliates.

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
using System.Xml.Serialization;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Properties;

namespace MySql.Configurator.Wizards.Server
{
  [Serializable]
  public class Role
  {
    #region Constants

    /// <summary>
    /// The SQL statement to grant all privileges to a given user connecting from a given host.
    /// </summary>
    public const string GRANT_ALL_PRIVILEGES_SQL = "GRANT ALL PRIVILEGES ON *.* TO '{0}'@'{1}' WITH GRANT OPTION";

    #endregion Constants

    #region Properties

    [XmlAttribute("id")]
    public string ID;

    [XmlAttribute("display")]
    public string Display;

    [XmlAttribute("description")]
    public string Description;

    [XmlArray()]
    public List<RolePrivilege> RolePrivileges;

    [XmlArray]
    public List<GrantedPrivilege> GrantedPrivileges;

    #endregion Properties

    /// <summary>
    /// Gets a <see cref="Role"/> with all the privileges the root user has.
    /// </summary>
    /// <returns>A <see cref="Role"/> with all the privileges the root user has.</returns>
    public static Role GetRootRole()
    {
      var id = UserRoleType.Root.GetDescription();
      var rootRole = new Role
      {
        ID = id,
        Display = id,
        Description = Resources.RootRoleDescription,
        RolePrivileges = null,
        GrantedPrivileges = new List<GrantedPrivilege>()
      };
      var grantedPrivilege = new GrantedPrivilege
      {
        Sql = GRANT_ALL_PRIVILEGES_SQL
      };
      rootRole.GrantedPrivileges.Add(grantedPrivilege);
      return rootRole;
    }
  }
}

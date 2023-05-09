/* Copyright (c) 2018, Oracle and/or its affiliates.

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

using System.ComponentModel;

namespace MySql.Configurator.Core.Enums
{
  /// <summary>
  /// Specifies identifiers to indicate the type of user role as defined in user-roles.xml.
  /// </summary>
  public enum UserRoleType
  {
    /// <summary>
    /// Minimal rights needed to backup any database.
    /// </summary>
    [Description("BackupAdmin")]
    BackupAdmin,

    /// <summary>
    /// Grants the rights to perform all tasks.
    /// </summary>
    [Description("DBA")]
    DatabaseAdmin,

    /// <summary>
    /// Tights to create and reverse engineer any database schema.
    /// </summary>
    [Description("DBDesigner")]
    DatabaseDesigner,

    /// <summary>
    /// Grants full rights on all databases.
    /// </summary>
    [Description("DBManager")]
    DatabaseManager,

    /// <summary>
    /// Grants rights needed to maintain server.
    /// </summary>
    [Description("InstanceManager")]
    InstanceManager,

    /// <summary>
    /// Minimum set of rights needed to monitor server.
    /// </summary>
    [Description("MonitorAdmin")]
    MonitorAdmin,

    /// <summary>
    /// Rights needed to assess, monitor, and kill any user process running in server.
    /// </summary>
    [Description("ProcessAdmin")]
    ProcessAdmin,

    /// <summary>
    /// Rights needed to setup and manage replication.
    /// </summary>
    [Description("ReplicationAdmin")]
    ReplicationAdmin,

    /// <summary>
    /// All rights just the same as the root user.
    /// </summary>
    [Description("Root")]
    Root,

    /// <summary>
    /// Rights to manage logins and grant and revoke server and database level permission.
    /// </summary>
    [Description("SecurityAdmin")]
    SecurityAdmin,

    /// <summary>
    /// Grants rights to create users logins and reset passwords.
    /// </summary>
    [Description("UserAdmin")]
    UserAdmin
  }
}
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

namespace MySql.Configurator.Core.Enums
{
  /// <summary>
  /// Specifies identifiers to indicate the type of member role of a server instance in a group replication cluster.
  /// </summary>
  public enum GroupReplicationMemberRoleType
  {
    /// <summary>
    /// Cannot determine the type or not set.
    /// </summary>
    Unknown,

    /// <summary>
    /// Not a member of an InnoDB Cluster (stand-alone instance).
    /// </summary>
    None,

    /// <summary>
    /// Source (R/W) instance.
    /// </summary>
    Primary,

    /// <summary>
    /// Replica (R/O) instance.
    /// </summary>
    Secondary
  }
}

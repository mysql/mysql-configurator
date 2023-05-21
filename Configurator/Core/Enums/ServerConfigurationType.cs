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
using System.ComponentModel;

namespace MySql.Configurator.Core.Enums
{
  /// <summary>
  /// Specifies identifiers to indicate the type of configuration applied to a MySQL Server.
  /// </summary>
  public enum ServerConfigurationType
  {
    /// <summary>
    /// Classic configuration as a stand-alone server.
    /// </summary>
    [Description("Stand-alone Server")]
    StandAlone = 0,

    /// <summary>
    /// Sandbox InnoDB Cluster (for testing purposes) with local seed and replica instances.
    /// </summary>
    [Description("Sandbox InnoDB Cluster")]
    Sandbox = 1,

    /// <summary>
    /// Server configured as the source (seed) of a new InnoDB Cluster.
    /// </summary>
    [Description("InnoDB Cluster (Seed)")]
    NewCluster = 2,

    /// <summary>
    /// Server configured as the replica of an existing InnoDB Cluster.
    /// </summary>
    [Description("InnoDB Cluster (Replica)")]
    AddToCluster = 3
  }
}

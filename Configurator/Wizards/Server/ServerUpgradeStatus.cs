/* Copyright (c) 2019, Oracle and/or its affiliates.

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

namespace MySql.Configurator.Wizards.Server
{
  /// <summary>
  /// Represents a status after upgrading MySQL Server's system tables.
  /// </summary>
  public class ServerUpgradeStatus
  {
    /// <summary>
    /// Gets or sets a value indicating whether the server is accepting connections.
    /// </summary>
    public bool AcceptingConnections { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the upgrade of system tables failed.
    /// </summary>
    public bool UpgradeFailed { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the upgrade of system tables completed successfully.
    /// </summary>
    public bool UpgradeFinished { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the upgrade of system tables started successfully.
    /// </summary>
    public bool UpgradeStarted { get; set; }
  }
}

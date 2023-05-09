/* Copyright (c) 2019, 2023, Oracle and/or its affiliates.

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
  /// Represents a status after starting MySQL Server.
  /// </summary>
  public class ServerStartStatus
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ServerStartStatus"/> class.
    /// </summary>
    /// <param name="asAService">Flag indicating whether the server was started as a service or as a process.</param>
    public ServerStartStatus(bool asAService)
    {
      AsAService = asAService;
    }

    #region Properties

    /// <summary>
    /// Gets a value indicating whether the server is accepting connections.
    /// </summary>
    public bool AcceptingConnections { get; internal set; }

    /// <summary>
    /// Gets a value indicating whether the server was started as a service or as a process.
    /// </summary>
    public bool AsAService { get; }

    /// <summary>
    /// Gets a value indicating whether the server was started successfully.
    /// </summary>
    public bool Started { get; internal set; }

    /// <summary>
    /// Gets information status after upgrading MySQL Server's system tables.
    /// </summary>
    public ServerUpgradeStatus UpgradeStatus { get; internal set; }

    #endregion Properties
  }
}

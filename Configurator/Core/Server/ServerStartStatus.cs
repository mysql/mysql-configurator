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

namespace MySql.Configurator.Core.Server
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

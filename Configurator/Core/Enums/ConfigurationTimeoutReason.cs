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
  /// List of possible reasons for the product configuration timeout for all products
  /// </summary>
  public enum ConfigurationTimeOutReason
  {
    /// <summary>
    /// Product is MySQL Server and the timeout reason is that the InnoDB logs are still being written so the service start is delayed
    /// </summary>
    MySQLServerWritingToInnoDBLog,

    /// <summary>
    /// Product is MySQL Server and the timeout reason is unknown
    /// </summary>
    MySQLServerUnknown,

    /// <summary>
    /// Product is generic and the timeout reason is unknown
    /// </summary>
    GenericUnknown,

    /// <summary>
    /// Configuration timeout is not being monitored
    /// </summary>
    None
  }
}

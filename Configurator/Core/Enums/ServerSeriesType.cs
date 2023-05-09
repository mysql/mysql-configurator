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

namespace MySql.Configurator.Core.Enums
{
  /// <summary>
  /// Specifies identifiers to indicate the MySQL Server series.
  /// </summary>
  [Flags]
  public enum ServerSeriesType
  {
    /// <summary>
    /// Server 5.1
    /// </summary>
    S51 = 1 << 0,

    /// <summary>
    /// Server 5.5
    /// </summary>
    S55 = S51 << 1,

    /// <summary>
    /// Server 5.6
    /// </summary>
    S56 = S55 << 1,

    /// <summary>
    /// Server 5.7
    /// </summary>
    S57 = S56 << 1,

    /// <summary>
    /// Server 8.0
    /// </summary>
    S80 = S57 << 1,

    /// <summary>
    /// Server 8.x
    /// </summary>
    S8x = S80 << 1,

    /// <summary>
    /// All supported series
    /// </summary>
    All = S51 | S55 | S56 | S57 | S80 | S8x
  }
}

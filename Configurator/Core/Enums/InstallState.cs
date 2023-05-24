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
  /// Current Install State
  /// </summary>
  public enum InstallState
  {
    /// <summary>
    /// Component disabled.
    /// </summary>
    NotUsed = -7,

    /// <summary>
    /// Configuration data corrupt.
    /// </summary>
    BadConfig = -6,

    /// <summary>
    /// Installation suspended or in progress.
    /// </summary>
    Incomplete = -5,

    /// <summary>
    /// Run from source, source is unavailable.
    /// </summary>
    SourceAbsent = -4,

    /// <summary>
    /// Return buffer overflow.
    /// </summary>
    MoreData = -3,

    /// <summary>
    /// Invalid function argument.
    /// </summary>
    InvalidId = -2,

    /// <summary>
    /// Unrecognized product or feature.
    /// </summary>
    Unknown = -1,

    /// <summary>
    /// Broken.
    /// </summary>
    Broken = 0,

    /// <summary>
    /// Advertised feature.
    /// </summary>
    Advertised = 1,

    /// <summary>
    /// Component being removed (action state, not settable).
    /// </summary>
    Removed = 1,

    /// <summary>
    /// Uninstalled (or action state absent but clients remain).
    /// </summary>
    Absent = 2,

    /// <summary>
    /// Installed on local drive.
    /// </summary>
    Local = 3,

    /// <summary>
    /// Run from source, CD or net.
    /// </summary>
    Source = 4,

    /// <summary>
    /// Use default, local or source.
    /// </summary>
    Default = 5
  }
}
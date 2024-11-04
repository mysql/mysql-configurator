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

namespace MySql.Configurator.Base.Enums
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

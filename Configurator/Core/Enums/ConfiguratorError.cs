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

using System.ComponentModel;

namespace MySql.Configurator.Core.Enums
{
  /// <summary>
  /// Configurator general errors.
  /// </summary>
  public enum ConfiguratorError : uint
  {
    /// <summary>
    /// Registry key not found.
    /// </summary>
    [Description("Failed to find a MySQL Server installation.")]
    RegistryKeyNotFound = 0,

    /// <summary>
    /// Version not found.
    /// </summary>
    [Description("Failed to find a MySQL Server installation.")]
    VersionNotFound = 1,

    /// <summary>
    /// Version not found.
    /// </summary>
    [Description("The version provided does not match with the one installed.")]
    VersionMismatch = 2,

    /// <summary>
    /// Version not found.
    /// </summary>
    [Description("Failed to find a MySQL Server installation in the current directory.")]
    InstallDirKeyNotFound = 3,

    /// <summary>
    /// Version not found.
    /// </summary>
    [Description("Failed to find the installation directory.")]
    InstallDirNotFound = 4,

    /// <summary>
    /// Version not found.
    /// </summary>
    [Description("No files found in the installation directory.")]
    InstallDirFilesNotFound = 5,

    /// <summary>
    /// Incorrect number of arguments.
    /// </summary>
    [Description("Two arguments are expected, the version and data dir path.")]
    IncorrectArgumentCount = 6,

    /// <summary>
    /// Invalid version.
    /// </summary>
    [Description("Invalid version '{0}'.")]
    InvalidVersion = 7,

    /// <summary>
    /// Short version.
    /// </summary>
    [Description("Invalid version '{0}'. Version must contain at least three digits.")]
    ShortVersion = 8,

    /// <summary>
    /// Invalid option.
    /// </summary>
    [Description("Invalid option '{0}'.")]
    InvalidOption = 7,
  }
}

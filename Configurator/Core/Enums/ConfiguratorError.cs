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
    /// Version mismatch.
    /// </summary>
    [Description("Version for the mysql.exe and mysql_configurator.exe do not match.")]
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
    InvalidOption = 9,

    /// <summary>
    /// Invalid option start.
    /// </summary>
    [Description("Option '{0}' must start with '--'.")]
    InvalidOptionStart = 10,

    /// <summary>
    /// Invalid execution mode.
    /// </summary>
    [Description("An invalid execution mode was provided.")]
    InvalidExecutionMode = 11,

    /// <summary>
    /// Upgrade history elements not found in the server's upgrade history file.
    /// </summary>
    [Description("No upgrade history elements found in the mysql_upgrade_history file.")]
    UpgradeHistoryElementsNotFound = 12,

    /// <summary>
    /// The mysqld.exe was not found in the same directory as mysql_configurator.exe.
    /// </summary>
    [Description("mysqld.exe not found at: '{0}'.")]
    MysqldExeNotFound = 13,

    /// <summary>
    /// The server configuration file was not found.
    /// </summary>
    [Description("Configuration file not found at: '{0}'")]
    ConfigurationFileNotFound = 14,

    [Description("Failed to update the authentication plugin of the root user.")]
    AuthenticationPluginUpdateFailed = 15
  }
}

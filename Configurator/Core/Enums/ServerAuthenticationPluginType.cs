/* Copyright (c) 2018, 2024, Oracle and/or its affiliates.

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

namespace WexInstaller.Core.Enums
{
  /// <summary>
  /// Specifies identifiers to indicate the type of authentication plugin to use when establishing connections to the Server.
  /// </summary>
  public enum ServerAuthenticationPluginType
  {
    /// <summary>
    /// Not set or unknown.
    /// </summary>
    None,

    /// <summary>
    /// Use MySQL Native Pluggable Authentication.
    /// </summary>
    [Description("mysql_native_password")]
    MysqlNativePassword,

    /// <summary>
    /// Use SHA-256 Pluggable Authentication.
    /// </summary>
    [Description("sha256_password")]
    Sha256Password,

    /// <summary>
    /// Use Caching SHA-2 Pluggable Authentication.
    /// </summary>
    [Description("caching_sha2_password")]
    CachingSha2Password,

    /// <summary>
    /// Use Windows Pluggable Authentication (Commercial-only).
    /// </summary>
    [Description("authentication_windows")]
    Windows
  }
}

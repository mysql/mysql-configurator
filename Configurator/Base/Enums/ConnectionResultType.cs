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

namespace MySql.Configurator.Base.Enums
{
  /// <summary>
  /// Specifies identifiers to indicate the result of a connection test.
  /// </summary>
  public enum ConnectionResultType
  {
    /// <summary>
    /// No connection attempt was made.
    /// </summary>
    [Description("No connection attempt was made.")]
    None,

    /// <summary>
    /// An error was thrown by the server and was shown to the user.
    /// </summary>
    [Description("An error was thrown by the MySQL server (please see the logs).")]
    ConnectionError,

    /// <summary>
    /// Connection was successful.
    /// </summary>
    [Description("Connection successful.")]
    ConnectionSuccess,

    /// <summary>
    /// The local host is not running.
    /// </summary>
    [Description("MySQL server is not running, a connection cannot be established.")]
    HostNotRunning,

    /// <summary>
    /// Could not connect to the specified MySQL host.
    /// </summary>
    [Description("Could not connect to MySQL, the service is possibly down or the host is unreachable.")]
    HostUnreachable,

    /// <summary>
    /// User name cannot be empty or just contain whitespaces.
    /// </summary>
    [Description("User name cannot be empty or just contain whitespaces.")]
    InvalidUserName,

    /// <summary>
    /// The password of the current user has expired and must be reset.
    /// </summary>
    [Description("The password of the current user has expired and must be reset.")]
    PasswordExpired,

    /// <summary>
    /// The password of the current user has been reset.
    /// </summary>
    [Description("The password of the current user has been reset.")]
    PasswordReset,

    /// <summary>
    /// Could not connect to the MySQL host with the specified password for the current user.
    /// </summary>
    [Description("Could not connect to MySQL with the given password.")]
    WrongPassword
  }
}

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

using System;

namespace MySql.Configurator.Base.Enums
{
  /// <summary>
  /// The type of hostname that can be specified in host name fields.
  /// </summary>
  [Flags]
  public enum ValidHostNameType
  {
    /// <summary>
    /// No value set.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// A domain name system (DNS) style host name.
    /// </summary>
    DNS = 1 << 0,

    /// <summary>
    /// An Internet Protocol (IP) version 4 host address.
    /// </summary>
    IPv4 = 1 << 1,

    /// <summary>
    /// An Internet Protocol (IP) version 6 host address.
    /// </summary>
    IPv6 = 1 << 2,

    /// <summary>
    /// Any valid host name type (DNS. IPv4 or IPv6).
    /// </summary>
    Any = DNS | IPv4 | IPv6
  }
}

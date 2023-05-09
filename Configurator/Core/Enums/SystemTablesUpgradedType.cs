/* Copyright (c) 2019, Oracle and/or its affiliates.

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
  /// Specifies options that describe if a server upgrade of system tables was run previously.
  /// </summary>
  public enum SystemTablesUpgradedType
  {
    /// <summary>
    /// No upgrade has been done before.
    /// </summary>
    None,

    /// <summary>
    /// System tables were upgraded on the last server upgrade.
    /// </summary>
    Yes,

    /// <summary>
    /// System tables upgrade was skipped during the last server upgrade.
    /// </summary>
    No
  }
}

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
  /// Specifies the available options for the Server variable lower_case_table_names, this option is used in the Initialize database step.
  /// The option lower_case_table_names=0 is not present because this option is not suitable for Windows systems
  /// </summary>
  public enum LowerCaseTableNamesTypes
  {
    /// <summary>
    /// With this option the table names are stored in lower case and the comparisons are not case-sensitive. This is the default option for Windows systems.
    /// </summary>
    LowerCaseStoredInsensitiveComparison = 1,

    /// <summary>
    /// With this option the table names are stored in as they were specified in the DDL statement and the comparisons are not casesensitive.
    /// </summary>
    SpecifiedCaseStoredInsensitiveComparison = 2
  }
}

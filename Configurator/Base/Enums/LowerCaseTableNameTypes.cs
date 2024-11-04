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

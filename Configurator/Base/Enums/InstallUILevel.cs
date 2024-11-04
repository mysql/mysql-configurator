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
  /// The Install Level of an MSI
  /// </summary>
  [Flags]
  public enum InstallUILevel
  {
    /// <summary>
    /// UI level is unchanged
    /// </summary>
    NoChange = 0,

    /// <summary>
    /// default UI is used
    /// </summary>
    Default = 1,

    /// <summary>
    /// completely silent installation
    /// </summary>
    None = 2,

    /// <summary>
    /// simple progress and error handling
    /// </summary>
    Basic = 3,

    /// <summary>
    /// authored UI, wizard dialogs suppressed
    /// </summary>
    Reduced = 4,

    /// <summary>
    /// authored UI with wizards, progress, errors
    /// </summary>
    Full = 5,

    /// <summary>
    /// display success/failure dialog at end of install
    /// </summary>
    EndDialog = 0x80,

    /// <summary>
    /// display only progress dialog
    /// </summary>
    ProgressOnly = 0x40,

    /// <summary>
    /// do not display the cancel button in basic UI
    /// </summary>
    HideCancel = 0x20,

    /// <summary>
    /// force display of source resolution even if quiet
    /// </summary>
    SourceResOnly = 0x100,

    /// <summary>
    /// show UAC prompt even if quiet (only for MSI version > 5)
    /// </summary>
    UACOnly = 0x200
  }
}

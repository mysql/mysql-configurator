/* Copyright (c) 2014, 2018, Oracle and/or its affiliates.

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

using System;

namespace MySql.Configurator.Core.Enums
{
  [Flags]
  public enum PathWarningType
  {
    None = 0,
    InstallDirPathInvalid = 1 << 0,
    InstallDirPathExists = 1 << 1,
    InstallDirPathCurrentInUse = 1 << 2,
    DataDirPathInvalid = 1 << 3,
    DataDirPathExists = 1 << 4,
    DataDirPathCurrentInUse = 1 << 5,
  }
}

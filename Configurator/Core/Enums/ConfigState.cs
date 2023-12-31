﻿/* Copyright (c) 2023, Oracle and/or its affiliates.

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
  public enum ConfigState
  {
    None = 0,
    ConfigurationRequired = 1 << 0,
    ConfigurationInProgress = 1 << 1,
    ConfigurationComplete = 1 << 2,
    ConfigurationError = 1 << 3,
    ConfigurationUnnecessary = 1 << 4,
    ConfigurationCompleteWithWarnings = 1 << 5,
    ConfigurationCancelled = 1 << 6
  }
}

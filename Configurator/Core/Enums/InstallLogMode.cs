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

namespace MySql.Configurator.Core.Enums
{
  /// <summary>
  /// Installer database log mode
  /// </summary>
  [Flags]
  public enum InstallLogMode
  {
    /// <summary>
    /// Fatal Exit
    /// </summary>
    FatalExit = (1 << (InstallMessage.FatalExit >> 24)),

    /// <summary>
    /// Error Mode
    /// </summary>
    Error = (1 << (InstallMessage.Error >> 24)),

    /// <summary>
    /// Warning Mode
    /// </summary>
    Warning = (1 << (InstallMessage.Warning >> 24)),

    /// <summary>
    /// User Mode
    /// </summary>
    User = (1 << (InstallMessage.User >> 24)),

    /// <summary>
    /// Info Mode
    /// </summary>
    Info = (1 << (InstallMessage.Info >> 24)),

    /// <summary>
    /// Resolve Source
    /// </summary>
    ResolveSource = (1 << (InstallMessage.ResolveSource >> 24)),

    /// <summary>
    /// Out of disk space
    /// </summary>
    OutOfDiskSpace = (1 << (InstallMessage.OutOfDiskSpace >> 24)),

    /// <summary>
    /// Action Start
    /// </summary>
    ActionStart = (1 << (InstallMessage.ActionStart >> 24)),

    /// <summary>
    /// Action Data
    /// </summary>
    ActionData = (1 << (InstallMessage.ActionData >> 24)),

    /// <summary>
    /// Common Data
    /// </summary>
    CommonData = (1 << (InstallMessage.CommonData >> 24)),

    /// <summary>
    /// Property Dump
    /// </summary>
    PropertyDump = (1 << (InstallMessage.Progress >> 24)),

    /// <summary>
    /// Verbose Log
    /// </summary>
    Verbose = (1 << (InstallMessage.Initialize >> 24)),

    /// <summary>
    /// Extra Debug
    /// </summary>
    ExtraDebug = (1 << (InstallMessage.Terminate >> 24)),

    /// <summary>
    /// Only log error
    /// </summary>
    LogOnlyError = (1 << (InstallMessage.ShowDialog >> 24)),

    /// <summary>
    /// Only log performance
    /// </summary>
    LogPerformance = (1 << (InstallMessage.Performance >> 24)),

    /// <summary>
    /// External handler progress
    /// </summary>
    Progress = (1 << (InstallMessage.Progress >> 24)),

    /// <summary>
    /// External handler initialize
    /// </summary>
    Initialize = (1 << (InstallMessage.Initialize >> 24)),

    /// <summary>
    /// External handler terminate
    /// </summary>
    Terminate = (1 << (InstallMessage.Terminate >> 24)),

    /// <summary>
    /// External handler show dialog
    /// </summary>
    ShowDialog = (1 << (InstallMessage.ShowDialog >> 24)),

    /// <summary>
    /// External handler files in use
    /// </summary>
    FilesInUse = (1 << (InstallMessage.FilesInUse >> 24)),

    /// <summary>
    /// External handler remove files in use
    /// </summary>
    RmFilesInUse = (1 << (InstallMessage.RmFilesInUse >> 24)),

    /// <summary>
    /// External handler install start
    /// </summary>
    InstallStart = (1 << (InstallMessage.InstallStart >> 24)),

    /// <summary>
    /// External handler install end
    /// </summary>
    InstallEnd = (1 << (InstallMessage.InstallEnd >> 24)),
  }

}

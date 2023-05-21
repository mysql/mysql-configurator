/* Copyright (c) 2023 Oracle and/or its affiliates.

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
  /// Install message type for callback is a combination of the following:
  ///  A message box style:      MB_*, where MB_OK is the default
  ///  A message box icon type:  MB_ICON*, where no icon is the default
  ///  A default button:         MB_DEFBUTTON?, where MB_DEFBUTTON1 is the default
  ///  One of the following install message types, no default
  /// </summary>
  public enum InstallMessage
  {
    /// <summary>
    /// premature termination, possibly fatal OOM
    /// </summary>
    FatalExit = 0x00000000,

    /// <summary>
    /// formatted error message
    /// </summary>
    Error = 0x01000000,

    /// <summary>
    /// formatted warning message
    /// </summary>
    Warning = 0x02000000,

    /// <summary>
    /// user request message
    /// </summary>
    User = 0x03000000,

    /// <summary>
    /// informative message for log
    /// </summary>
    Info = 0x04000000,

    /// <summary>
    /// list of files in use that need to be replaced
    /// </summary>
    FilesInUse = 0x05000000,

    /// <summary>
    /// request to determine a valid source location
    /// </summary>
    ResolveSource = 0x06000000,

    /// <summary>
    /// insufficient disk space message
    /// </summary>
    OutOfDiskSpace = 0x07000000,

    /// <summary>
    /// start of action: action name and description
    /// </summary>
    ActionStart = 0x08000000,

    /// <summary>
    /// formatted data associated with individual action item
    /// </summary>
    ActionData = 0x09000000,

    /// <summary>
    /// progress gauge info: units so far, total
    /// </summary>
    Progress = 0x0A000000,

    /// <summary>
    /// product info for dialog: language Id, dialog caption
    /// </summary>
    CommonData = 0x0B000000,

    /// <summary>
    /// sent prior to UI initialization, no string data
    /// </summary>
    Initialize = 0x0C000000,

    /// <summary>
    /// sent after UI termination, no string data
    /// </summary>
    Terminate = 0x0D000000,

    /// <summary>
    /// sent prior to display or authored dialog or wizard
    /// </summary>
    ShowDialog = 0x0E000000,

    /// <summary>
    /// log only, to log performance number like action time
    /// </summary>
    Performance = 0x0F000000,

    /// <summary>
    /// the list of apps that the user can request Restart Manager to shut down and restart
    /// </summary>
    RmFilesInUse = 0x19000000,

    /// <summary>
    /// sent prior to server-side install of a product
    /// </summary>
    InstallStart = 0x1A000000,

    /// <summary>
    /// sent after server-side install
    /// </summary>
    InstallEnd = 0x1B000000,
  }
}

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
  /// The Plugin Activation State option used at the Server Initialization/Starting command option
  /// </summary>
  public enum PluginActivationStateType
  {
    /// <summary>
    /// This option indicates that is not needed to send a command option.
    /// </summary>
    NA,

    /// <summary>
    /// Indicates that the plugin will be enabled.
    /// The format of the option is --<plugin_name>=ON
    /// </summary>
    [Description("ON")]
    On,

    /// <summary>
    /// Indicates that the plugin will be disabled.
    /// The format of the option is --<plugin_name>=OFF
    /// </summary>
    [Description("OFF")]
    Off,

    /// <summary>
    /// Indicates that the plugin will be enabled, but if the plugin fails the server doesn't start.
    /// The format of the option is --<plugin_name>=FORCE
    /// </summary>
    [Description("FORCE")]
    Force,

    /// <summary>
    /// Similar to FORCE_OPTION but indicates that the plugin couldn't be disabled at runtime causing an error when trying to disable the plugin.
    /// The format of the option is --<plugin_name>=FORCE_PLUS_PERMANENT
    /// </summary>
    [Description("FORCE_PLUS_PERMANENT")]
    ForcePlusPermanent,

    /// <summary>
    /// It is a synonym for the ON option
    /// The format of the option is --enable-<plugin_name>
    /// </summary>
    [Description("enable")]
    Enable,

    /// <summary>
    /// It is a synonym for the OFF option
    /// The format of the option is --skip-<plugin_name>
    /// </summary>
    [Description("skip")]
    Skip
  }
}

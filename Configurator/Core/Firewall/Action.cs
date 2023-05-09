/* Copyright (c) 2015, 2018, Oracle and/or its affiliates.

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

using NetFwTypeLib;

namespace MySql.Configurator.Core.Firewall
{
  /// <summary>
  /// This enumerated type specifies the action for a rule or default setting.
  /// </summary>
  public enum Action
  {
    /// <summary>
    /// Block traffic.
    /// </summary>
    Block = NET_FW_ACTION_.NET_FW_ACTION_BLOCK,

    /// <summary>
    /// Allow traffic.
    /// </summary>
    Allow = NET_FW_ACTION_.NET_FW_ACTION_ALLOW,

    /// <summary>
    /// Maximum traffic.
    /// </summary>
    Maximum = NET_FW_ACTION_.NET_FW_ACTION_MAX
  }
}

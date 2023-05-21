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

using System;
using NetFwTypeLib;

namespace MySql.Configurator.Core.Firewall
{
  /// <summary>
  /// This enumerated type specifies which direction of traffic a rule applies to
  /// </summary>
  public enum RuleDirection
  {
    /// <summary>
    /// The rule applies to inbound traffic.
    /// </summary>
    In = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN,

    /// <summary>
    /// The rule applies to outbound traffic.
    /// </summary>
    Out = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT,

    /// <summary>
    /// This value is used for boundary checking only and is not valid for application programming.
    /// </summary>
    [Obsolete("This value is used for boundary checking only and is not valid for application programming.")]
    Max = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_MAX
  }
}

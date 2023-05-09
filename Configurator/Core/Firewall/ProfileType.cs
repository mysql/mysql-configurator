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

using System;
using NetFwTypeLib;

namespace MySql.Configurator.Core.Firewall
{
  /// <summary>
  /// This enumerated type specifies the type of profile.
  /// The types of Profiles are combinable.
  /// </summary>
  [Flags]
  public enum ProfileType
  {
    /// <summary>
    /// Profile type is domain.
    /// </summary>
    Domain = NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_DOMAIN,

    /// <summary>
    /// Profile type is private. This profile type is used for home and other private network types.
    /// </summary>
    Private = NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PRIVATE,

    /// <summary>
    /// Profile type is public. This profile type is used for public internet access points.
    /// </summary>
    Public = NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PUBLIC,

    /// <summary>
    /// All
    /// </summary>
    All = NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL
  }
}

/* Copyright (c) 2018, Oracle and/or its affiliates.

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
  /// Installer (MSI) Feature Attributes
  /// </summary>
  public enum MsiInstallFeatureAttribute : int
  {
    /// <summary>
    /// Hidden feature
    /// </summary>
    Hidden = 0,

    /// <summary>
    /// Feature favors local
    /// </summary>
    FavorLocal = 1,

    /// <summary>
    /// Feature favors source
    /// </summary>
    FavorSource = 2,

    /// <summary>
    /// Feature follow it's parent
    /// </summary>
    FollowParent = 4,

    /// <summary>
    /// Feature favors advertise
    /// </summary>
    FavorAdvertise = 8,

    /// <summary>
    /// Feature does not advertise
    /// </summary>
    DisallowAdvertise = 16,

    /// <summary>
    /// Feature Noun supported.
    /// </summary>
    NounSupportedAdvertise = 32,

    /// <summary>
    /// All feature types
    /// </summary>
    All = FavorLocal | FavorSource | FollowParent | FavorAdvertise | DisallowAdvertise,
  }
}

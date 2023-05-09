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

using System;

namespace MySql.Configurator.Core.Enums
{
  [Flags]
  public enum ListViewExtendedStyles
  {
    /// <summary>
    /// LVS_EX_GRIDLINES
    /// </summary>
    GridLines = 0x00000001,

    /// <summary>
    /// LVS_EX_SUBITEMIMAGES
    /// </summary>
    SubItemImages = 0x00000002,

    /// <summary>
    /// LVS_EX_CHECKBOXES
    /// </summary>
    CheckBoxes = 0x00000004,

    /// <summary>
    /// LVS_EX_TRACKSELECT
    /// </summary>
    TrackSelect = 0x00000008,

    /// <summary>
    /// LVS_EX_HEADERDRAGDROP
    /// </summary>
    HeaderDragDrop = 0x00000010,

    /// <summary>
    /// LVS_EX_FULLROWSELECT
    /// </summary>
    FullRowSelect = 0x00000020,

    /// <summary>
    /// LVS_EX_ONECLICKACTIVATE
    /// </summary>
    OneClickActivate = 0x00000040,

    /// <summary>
    /// LVS_EX_TWOCLICKACTIVATE
    /// </summary>
    TwoClickActivate = 0x00000080,

    /// <summary>
    /// LVS_EX_FLATSB
    /// </summary>
    FlatsB = 0x00000100,

    /// <summary>
    /// LVS_EX_REGIONAL
    /// </summary>
    Regional = 0x00000200,

    /// <summary>
    /// LVS_EX_INFOTIP
    /// </summary>
    InfoTip = 0x00000400,

    /// <summary>
    /// LVS_EX_UNDERLINEHOT
    /// </summary>
    UnderlineHot = 0x00000800,

    /// <summary>
    /// LVS_EX_UNDERLINECOLD
    /// </summary>
    UnderlineCold = 0x00001000,

    /// <summary>
    /// LVS_EX_MULTIWORKAREAS
    /// </summary>
    MultilWorkAreas = 0x00002000,

    /// <summary>
    /// LVS_EX_LABELTIP
    /// </summary>
    LabelTip = 0x00004000,

    /// <summary>
    /// LVS_EX_BORDERSELECT
    /// </summary>
    BorderSelect = 0x00008000,

    /// <summary>
    /// LVS_EX_DOUBLEBUFFER
    /// </summary>
    DoubleBuffer = 0x00010000,

    /// <summary>
    /// LVS_EX_HIDELABELS
    /// </summary>
    HideLabels = 0x00020000,

    /// <summary>
    /// LVS_EX_SINGLEROW
    /// </summary>
    SingleRow = 0x00040000,

    /// <summary>
    /// LVS_EX_SNAPTOGRID
    /// </summary>
    SnapToGrid = 0x00080000,

    /// <summary>
    /// LVS_EX_SIMPLESELECT
    /// </summary>
    SimpleSelect = 0x00100000
  }
}

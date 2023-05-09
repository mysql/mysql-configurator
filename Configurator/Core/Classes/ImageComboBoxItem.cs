﻿/* Copyright (c) 2018, 2023, Oracle and/or its affiliates.

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

using System.Drawing;

namespace MySql.Configurator.Core.Classes
{
  public class ImageComboBoxItem
  {
    public Image ItemImage { get; }
    public string ItemTitle { get; }
    public string ItemDescription { get; }
    public int ItemHeight { get; set; }
    public object Tag { get; set; }

    public ImageComboBoxItem(Image image, string title, string description, object tag)
    {
      ItemImage = image;
      ItemTitle = title;
      ItemDescription = description;
      ItemHeight = image.Height;
      Tag = tag;
    }

    public override string ToString()
    {
      return ItemTitle;
    }
  }
}
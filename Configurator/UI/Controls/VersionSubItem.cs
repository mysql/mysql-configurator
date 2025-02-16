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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WexInstaller.Core;
using WexInstaller.Properties;

namespace WexInstaller.Controls
{
  public class VersionSubItem : MyListViewSubItem
  {
    public VersionSubItem(ListViewItem item, Package p) : base(item, p.Version, Resources.MySQLInstallerUpdateAvailable, false, false)
    {
    }

    public override void Draw(DrawListViewSubItemEventArgs e)
    {
      Package p = e.Item.Tag as Package;
      SizeF size = DrawText(e.Bounds, e);
      int x = e.Bounds.Left + (int)size.Width + 3;
      int y = e.Bounds.Top;
      if (p != null && p.AvailableUpgrades.Count > 0)
        e.Graphics.DrawImage(Resources.MySQLInstallerUpdateAvailable, x, y);
    }
  }
}

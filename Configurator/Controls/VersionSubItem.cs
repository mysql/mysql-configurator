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

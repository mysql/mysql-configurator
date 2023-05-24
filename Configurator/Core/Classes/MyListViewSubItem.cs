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

using System.Drawing;
using System.Windows.Forms;
using MySql.Configurator.Core.Controls;

namespace MySql.Configurator.Core.Classes
{
  public class MyListViewSubItem : ListViewItem.ListViewSubItem
  {
    public MyListViewSubItem(ListViewItem parent, string text, Image image, bool isLink, bool imageBeforeText, bool showToolTip = false)
      : base(parent, text)
    {
      ImageBeforeText = imageBeforeText;
      Image = image;
      IsLink = isLink;
      Item = parent;
      ShowTooltip = showToolTip;
      if (Image == null || parent.ListView.SmallImageList != null)
      {
        return;
      }

      var iList = new ImageList
      {
        ImageSize = Image.Size
      };
      parent.ListView.SmallImageList = iList;
    }

    #region Properties

    public Image Image { get; set; }

    public bool ImageBeforeText { get; }

    public bool IsLink { get; set; }

    public ListViewItem Item { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the tool tip text for the whole item is shown when the mouse moves within this sub-item.
    /// </summary>
    public bool ShowTooltip { get; set; }

    /// <summary>
    /// Gets or sets the tool tip text to be shown on this sub-item.
    /// </summary>
    public string ToolTipText { get; set; }

    /// <summary>
    /// Gets or sets the tool tip title to be shown on this sub-item.
    /// </summary>
    public string ToolTipTitle { get; set; }

    #endregion Properties

    public void Draw(DrawListViewSubItemEventArgs e)
    {
      var bounds = e.Bounds;
      if (ImageBeforeText && Image != null)
      {
        DrawImage(Image, ref bounds, e.Graphics);
      }

      ((MyListView)Item.ListView).DrawText(e, ref bounds, IsLink);
      if (!ImageBeforeText && Image != null)
      {
        DrawImage(Image, ref bounds, e.Graphics);
      }
    }

    private void DrawBackground(DrawListViewSubItemEventArgs e)
    {
      if ((e.ItemState & ListViewItemStates.Selected) != 0)
      {
        e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
      }

      e.DrawFocusRectangle(e.Bounds);
    }

    private void DrawImage(Image image, ref Rectangle bounds, Graphics g)
    {
      var iconLocation = new Point(bounds.Left + 4, bounds.Top + (bounds.Height - image.Height) / 2);
      g.DrawImageUnscaled(image, iconLocation);
      bounds.X += image.Width + 9;
      bounds.Width -= image.Width + 9;
    }
  }
}

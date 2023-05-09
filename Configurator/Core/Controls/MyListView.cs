/* Copyright (c) 2014, 2023, Oracle and/or its affiliates.

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
using System.Drawing;
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;

namespace MySql.Configurator.Core.Controls
{
  public class MyListView : ListView
  {
    #region Constants

    /// <summary>
    /// The default duration (in milliseconds) for the custom tool tip.
    /// </summary>
    private const int DEFAULT_CUSTOM_TOOLTIP_DURATION = 5000;

    #endregion Constants

    #region Fields

    private readonly ToolTip _customToolTip;
    private Point _lastMousePosition;
    private ListViewItem.ListViewSubItem _lastPositionedSubItem;

    #endregion Fields

    public MyListView()
    {
      _lastMousePosition = Point.Empty;
      _lastPositionedSubItem = null;
      _customToolTip = new ToolTip();
      _customToolTip.SetToolTip(this, string.Empty);
      OwnerDraw = true;
      SetStyle(ControlStyles.DoubleBuffer, true);
      SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
      SetStyle(ControlStyles.AllPaintingInWmPaint, true);
      UpdateStyles();
    }

    public event EventHandler<LinkActivationEventArgs> LinkActivate;

    #region Properties

    public ProgressBar ProgressBar { get; private set; }

    public int ProgressBarCol { get; set; }

    public int ProgressBarRow { get; set; }

    #endregion Properties

    public void DrawText(DrawListViewSubItemEventArgs e, ref Rectangle bounds, bool link)
    {
      var format = new StringFormat(StringFormatFlags.NoWrap);
      switch (e.Header.TextAlign)
      {
        case HorizontalAlignment.Right:
          format.Alignment = StringAlignment.Far;
          break;

        case HorizontalAlignment.Center:
          format.Alignment = StringAlignment.Center;
          break;

        default:
          format.Alignment = StringAlignment.Near;
          break;
      }

      format.LineAlignment = StringAlignment.Center;
      format.Trimming = StringTrimming.EllipsisCharacter;
      var selected = (e.ItemState & ListViewItemStates.Selected) == ListViewItemStates.Selected;
      var textColor = link
        ? Color.Blue
        : (selected ? SystemColors.HighlightText : e.Item.ForeColor);
      using (var brush = new SolidBrush(textColor))
      {
        var font = e.SubItem.Font;
        if (link)
        {
          font = new Font(font, FontStyle.Underline);
        }

        var size = e.Graphics.MeasureString(e.SubItem.Text, font);
        e.Graphics.DrawString(e.SubItem.Text, font, brush, bounds, format);
        bounds.X += (int)size.Width;
        bounds.Width -= (int)size.Width;
      }
    }

    public int GetColumnIndex(string name)
    {
      var index = 0;
      foreach (ColumnHeader col in Columns)
      {
        if (col.Text == name)
        {
          return index;
        }

        index++;
      }

      return index;
    }

    public void RemoveColumn(string name)
    {
      var index = GetColumnIndex(name);
      if (index >= 0)
      {
        Columns.RemoveAt(index);
      }
    }

    public void ShowProgressBar(bool show)
    {
      if (ProgressBar == null)
      {
        AddProgressBar();
      }

      if (ProgressBar != null)
      {
        ProgressBar.Visible = show;
      }

      PositionProgressBar();
    }

    protected override void Dispose(bool disposing)
    {
      _customToolTip?.Dispose();
      base.Dispose(disposing);
    }

    protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
    {
      e.DrawDefault = true;
      base.OnDrawColumnHeader(e);
    }

    protected override void OnDrawItem(DrawListViewItemEventArgs e)
    {
      if ((e.State & ListViewItemStates.Selected) != 0)
      {
        e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
      }

      e.DrawFocusRectangle();
    }

    protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
    {
      if (e.ColumnIndex == 0
          && string.IsNullOrEmpty(e.SubItem.Text))
      {
        e.DrawDefault = true;
        return;
      }

      if (!(e.SubItem is MyListViewSubItem sub))
      {
        var bounds = e.Bounds;
        DrawText(e, ref bounds, false);
      }
      else
      {
        sub.Draw(e);
      }

      base.OnDrawSubItem(e);
    }

    protected override void OnMouseHover(EventArgs e)
    {
      base.OnMouseHover(e);
      if (_customToolTip == null
          || _lastPositionedSubItem == null
          || !(_lastPositionedSubItem is MyListViewSubItem mySubItem))
      {
        return;
      }

      if (mySubItem.ShowTooltip
          && !string.IsNullOrEmpty(mySubItem.ToolTipText))
      {
        _customToolTip.ToolTipTitle = mySubItem.ToolTipTitle;
        _customToolTip.Show(mySubItem.ToolTipText, this, _lastMousePosition, DEFAULT_CUSTOM_TOOLTIP_DURATION);
      }
      else
      {
        _customToolTip.Hide(this);
      }
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
      _lastMousePosition = e.Location;
      _lastPositionedSubItem = null;
      base.OnMouseMove(e);
      if (e.Location.X < 0
          || e.Location.Y < 0)
      {
        return;
      }

      var info = HitTest(e.Location);
      _lastPositionedSubItem = info.SubItem;
      if (info.SubItem == null
          || !(info.SubItem is MyListViewSubItem mySubItem))
      {
        Cursor = Cursors.Default;
        return;
      }

      Cursor = mySubItem.IsLink
        ? Cursors.Hand
        : Cursors.Default;
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
      if (e.Location.X >= 0
          && e.Location.Y >= 0)
      {
        var info = HitTest(e.Location);
        if (info.SubItem is MyListViewSubItem si && si.IsLink)
        {
          var args = new LinkActivationEventArgs
          {
            Item = info.Item,
            SubItem = info.SubItem
          };
          LinkActivate?.Invoke(this, args);
        }
      }

      base.OnMouseUp(e);
    }

    private void AddProgressBar()
    {
      ProgressBar = new ProgressBar
      {
        BackColor = BackColor,
        Height = 17,
        Visible = false,
        Style = ProgressBarStyle.Continuous
      };
      Controls.Add(ProgressBar);
    }

    private void PositionProgressBar()
    {
      if (Items.Count <= ProgressBarRow)
      {
        return;
      }

      var rowRect = GetItemRect(ProgressBarRow, ItemBoundsPortion.Entire);
      for (var x = 0; x < ProgressBarCol - 1; x++)
      {
        rowRect.X += Columns[x].Width;
      }

      rowRect.Width = Columns[ProgressBarCol].Width;
      rowRect.X += 2;
      rowRect.Y += (rowRect.Height - ProgressBar.Height) / 2;
      ProgressBar.Location = rowRect.Location;
    }
  }
}

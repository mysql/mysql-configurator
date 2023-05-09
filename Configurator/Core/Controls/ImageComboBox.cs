/* Copyright (c) 2010, 2023, Oracle and/or its affiliates.

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
using System.Linq;
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;

namespace MySql.Configurator.Core.Controls
{
  public sealed class ImageComboBox : ComboBox
  {
    #region Fields

    private SolidBrush _backBrush;

    private SolidBrush _descriptionForeBrush;

    private Color _descriptionForeColor;

    private IntPtr _dropDownWindow;

    private SolidBrush _foreBrush;

    private int _maxImageHeight;

    private int _maxImageWidth;

    private StringFormat _stringFormat;

    private Font _titleFont;

    private int _titleFontSize;

    private SolidBrush _titleForeBrush;

    private Color _titleForeColor;

    #endregion Fields

    public ImageComboBox()
    {
      Size = new Size(206, 21);
      DropDownStyle = ComboBoxStyle.DropDownList;
      DrawMode = DrawMode.OwnerDrawVariable;

      // Drop Down Handle used to compute proper Drop Down Height
      _dropDownWindow = IntPtr.Zero;

      _maxImageHeight = 0;
      _maxImageWidth = 0;

      //Default Font Formats and colors for the designer.
      ForeColor = SystemColors.WindowText;
      BackColor = SystemColors.Window;

      float size = 9F * (96F / Utilities.GetDPI());
      Font = new Font("Segoe UI", size, FontStyle.Regular);
      TitleFont = new Font(Font.FontFamily, size, FontStyle.Bold);
      DescriptionFont = new Font(Font.FontFamily, (size - 2));
      TitleForeColor = SystemColors.WindowText;
      DescriptionForeColor = Color.Gray;

      // Default Margins.
      TitleMargin = new Padding(5);
      DescriptionMargin = new Padding(5, 1, 3, 5);
      ImageMargin = new Padding(3);

      //String format to set description text to wrap
      _stringFormat = new StringFormat(StringFormatFlags.NoClip);
    }

    #region Properties

    public override Color BackColor
    {
      get
      {
        return base.BackColor;
      }

      set
      {
        base.BackColor = value;
        _backBrush = new SolidBrush(BackColor);
      }
    }

    public Font DescriptionFont { get; set; }

    public Color DescriptionForeColor
    {
      get
      {
        return _descriptionForeColor;
      }

      set
      {
        _descriptionForeColor = value;
        _descriptionForeBrush = new SolidBrush(_descriptionForeColor);
      }
    }

    public Padding DescriptionMargin { get; set; }

    public override Color ForeColor
    {
      get
      {
        return base.ForeColor;
      }

      set
      {
        base.ForeColor = value;
        _foreBrush = new SolidBrush(ForeColor);
      }
    }

    public Padding ImageMargin { get; set; }

    public Font TitleFont
    {
      get
      {
        return _titleFont;
      }

      set
      {
        _titleFont = value;
        _titleFontSize = Convert.ToInt32(Math.Ceiling(_titleFont.Size));
      }
    }

    public Color TitleForeColor
    {
      get
      {
        return _titleForeColor;
      }

      set
      {
        _titleForeColor = value;
        _titleForeBrush = new SolidBrush(_titleForeColor);
      }
    }

    public Padding TitleMargin { get; set; }

    #endregion Properties

    public void AddItem(Image image, string title, string description)
    {
      AddItem(image, title, description, null);
    }

    public void AddItem(Image image, string title, string description, object tag)
    {
      Items.Add(new ImageComboBoxItem(image, title, description, tag));

      // Calculate the largest possible image block size.
      foreach (ImageComboBoxItem imageComboBoxItem in Items)
      {
        int paddedImageHeight = imageComboBoxItem.ItemImage.Height + ImageMargin.Top + ImageMargin.Bottom;
        int paddedImageWidth = imageComboBoxItem.ItemImage.Width + ImageMargin.Left + ImageMargin.Right;
        _maxImageHeight = _maxImageHeight < paddedImageHeight ? paddedImageHeight : _maxImageHeight;
        _maxImageWidth = _maxImageWidth < paddedImageWidth ? paddedImageWidth : _maxImageWidth;
      }
    }

    public int Find(string roleTitle)
    {
      int selectedIndex = -1;
      for (int i = 0; i < Items.Count; i++)
      {
        var imageComboBoxItem = Items[i] as ImageComboBoxItem;
        if (imageComboBoxItem == null || imageComboBoxItem.ItemTitle != roleTitle)
        {
          continue;
        }

        selectedIndex = i;
        break;
      }

      return selectedIndex;
    }

    protected override void OnDrawItem(DrawItemEventArgs e)
    {
      if (e.Index < 0 || Items.Count == 0)
      {
        return;
      }

      e.DrawBackground();
      e.DrawFocusRectangle();
      //Get Current Item
      var currentItem = (ImageComboBoxItem)Items[e.Index];
      bool currentlySelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

      if (e.Bounds.Height > ItemHeight)
      {
        // Calculations of distances and margins for dropdown items
        int imageLeft = (_maxImageWidth - currentItem.ItemImage.Width) / 2;
        int imageTop = (e.Bounds.Height - currentItem.ItemImage.Height) / 2 + e.Bounds.Top;
        int titleTop = e.Bounds.Top + TitleMargin.Top;
        int titleLeft = _maxImageWidth + TitleMargin.Left;
        int descriptionTop = titleTop + _titleFontSize + TitleMargin.Bottom + DescriptionMargin.Top;
        int descriptionLeft = _maxImageWidth + DescriptionMargin.Left;
        int descriptionWidth = e.Bounds.Width - descriptionLeft;
        int descriptionHeight = e.Bounds.Height - descriptionTop - DescriptionMargin.Bottom;
        RectangleF descriptionRectangle = new Rectangle(descriptionLeft, descriptionTop, descriptionWidth, descriptionHeight);

        //Draw Title
        e.Graphics.DrawString(currentItem.ItemTitle,
                              TitleFont,
                              currentlySelected ? _backBrush : _titleForeBrush,
                              titleLeft,
                              titleTop);

        //Draw Description
        e.Graphics.DrawString(currentItem.ItemDescription, DescriptionFont, currentlySelected ? _backBrush : _titleForeBrush, descriptionRectangle, _stringFormat);

        //Draw Image
        e.Graphics.DrawImage(currentItem.ItemImage, imageLeft, imageTop, currentItem.ItemImage.Width, currentItem.ItemImage.Height);
      }
      else
      {
        e.Graphics.DrawString(currentItem.ItemTitle,
                              Font,
                              currentlySelected ? _backBrush : _foreBrush,
                              e.Bounds.Left,
                              e.Bounds.Top);
      }
    }

    protected override void OnDropDownClosed(EventArgs e)
    {
      _dropDownWindow = IntPtr.Zero;
      base.OnDropDownClosed(e);
    }

    protected override void OnMeasureItem(MeasureItemEventArgs e)
    {
      base.OnMeasureItem(e);

      //Resize each dropdown item without resizing the combobox. Calculate the total required text area and if that's greater than the image height, make adjustments.
      e.ItemWidth = DropDownWidth - SystemInformation.BorderSize.Width * 2;

      var currentItem = Items[e.Index] as ImageComboBoxItem;
      if (currentItem == null)
      {
        return;
      }

      int totalAvailableTextWidth = e.ItemWidth - DescriptionMargin.Left - DescriptionMargin.Right - ImageMargin.Left - ImageMargin.Right - _maxImageWidth;
      double requireDescriptionHeight = Math.Ceiling(e.Graphics.MeasureString(currentItem.ItemDescription, DescriptionFont, totalAvailableTextWidth).Height);
      int requiredTextHeight = _titleFontSize + TitleMargin.Top + TitleMargin.Bottom +
                               Convert.ToInt32(requireDescriptionHeight) + DescriptionMargin.Top + DescriptionMargin.Bottom;
      e.ItemHeight = (currentItem.ItemImage.Height > requiredTextHeight) ? currentItem.ItemImage.Height : requiredTextHeight;
      currentItem.ItemHeight = e.ItemHeight;
    }

    protected override void WndProc(ref Message message)
    {
      base.WndProc(ref message);
      if ((message.Msg != (int) WM.CTLCOLORLISTBOX) || (_dropDownWindow != IntPtr.Zero))
      {
        return;
      }

      _dropDownWindow = message.LParam;

      Win32.RECT r = new Win32.RECT();
      Win32.GetWindowRect(_dropDownWindow, ref r);

      // Calculate the required height for the DropDown box plus it's border.
      int newHeight = Items.Cast<object>().TakeWhile((t, i) => i < MaxDropDownItems).OfType<ImageComboBoxItem>().Sum(imageComboBoxItem => imageComboBoxItem.ItemHeight);
      newHeight += SystemInformation.BorderSize.Height * 2;
      var posFlags = Win32.SWP_FRAMECHANGED | Win32.SWP_NOACTIVATE | Win32.SWP_NOZORDER | Win32.SWP_NOOWNERZORDER;
      Win32.SetWindowPos(_dropDownWindow, IntPtr.Zero, r.Left, r.Top, DropDownWidth - SystemInformation.BorderSize.Width * 2, newHeight, posFlags);
    }
  }
}

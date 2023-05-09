using System;
using System.Drawing;
using System.Windows.Forms;
using WexInstaller.Core;


namespace WexInstaller
{
  public class ListViewWexSubItem : ListViewItem.ListViewSubItem
  {
    public ListViewWexSubItem(ListViewWex parent) : base()
    {
      Parent = parent;
    }

    public ListViewWexSubItem(ListViewWex parent, string text, Image image, bool isLink) : this(parent)
    {
      Text = text;
      Image = image;
      IsLink = isLink;
    }

    public bool IsLink { get; set; }

    public Image Image { get; set; }
    private Image GhostImage;

    private ListViewWex Parent;

    public virtual void Draw(DrawListViewSubItemEventArgs e)
    {
      if (ShouldShowGhostImage(e.ItemIndex) && GhostImage == null)
        GhostImage = GhostIcon(Image);
      Rectangle bounds = e.Bounds;
      if (Image != null)
        DrawImage(ref bounds, e);
      if (!String.IsNullOrEmpty(Text))
        DrawText(bounds, e);
    }

    private bool ShouldShowGhostImage(int itemIndex)
    {
      return Parent.GhostProductImages || Parent.ItemDisabled(itemIndex);
    }

    private void DrawImage(ref Rectangle bounds, DrawListViewSubItemEventArgs e)
    {
      if (Image == null) return;

      Image i = ShouldShowGhostImage(e.ItemIndex) ? GhostImage : Image;
      Point iconLocation = new Point(bounds.Left + 4,
        bounds.Top + (bounds.Height - i.Height) / 2);
      e.Graphics.DrawImage(i, iconLocation);
      bounds.X += i.Width + 9;
      bounds.Width -= i.Width + 9;
    }

    protected SizeF DrawText(Rectangle bounds, DrawListViewSubItemEventArgs e)
    {
      StringFormat format = new StringFormat(StringFormatFlags.NoWrap);
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
      Color textColor = IsLink ? Color.Blue : e.SubItem.ForeColor;

      using (SolidBrush brush = new SolidBrush(textColor))
      {
        Font font = e.SubItem.Font;
        if (IsLink)
          font = new Font(font, FontStyle.Underline);
        SizeF size = e.Graphics.MeasureString(e.SubItem.Text, font);
        e.Graphics.DrawString(e.SubItem.Text, font, brush, bounds, format);
        return size;
      }
    }

    private Image GhostIcon(Image image)
    {
      if (Image == null) return null;
      Bitmap b = new Bitmap(image);
      Utilities.GhostBitmap(b, 7);
      return b;
    }


  }
}

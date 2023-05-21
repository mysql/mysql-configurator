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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Wizard;
using MySql.Configurator.Properties;

namespace MySql.Configurator.Controls
{
  public partial class InstallWizardSideBarControl : UserControl
  {
    public InstallWizardSideBarControl()
    {
      TabTop = 120;
      HiliteBackground = Resources.WizardSelection;
      InitializeComponent();
      SetStyle(ControlStyles.DoubleBuffer, true);
      SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
      SetStyle(ControlStyles.AllPaintingInWmPaint, true);
      UpdateStyles();
      Tabs = new List<SideBarTab>();
    }

    protected override void OnLoad(EventArgs e)
    {
      Utilities.NormalizeFont(this);
      base.OnLoad(e);
    }

    public List<SideBarTab> Tabs { get; }

    public Image HiliteBackground;
    public int TabTop;

    public void ShowConfigPanel(string name)
    {
      ProductAndVersionPanel.Visible = true;
      ProductAndVersionLabel.Text = name;
    }

    public SideBarTab SelectedTab 
    {
      get
      {
        return Tabs.FirstOrDefault(tab => tab.Selected);
      }
    }

    public void SelectTab(WizardPage page)
    {
      var tab = Tabs.FirstOrDefault(t => t.Page.Equals(page));
      if (tab == null)
      {
        return;
      }

      // Ensure the caption has not changed
      if (!tab.Name.Equals(page.TabTitle, StringComparison.Ordinal))
      {
        tab.Name = page.TabTitle;
      }

      SelectTab(tab);
    }

    public void SelectTab(SideBarTab tab)
    {
      var selectedTab = SelectedTab;
      if (selectedTab != null)
      {
        selectedTab.Selected = false;
        Invalidate(GetTabRectangle(selectedTab));
      }

      tab.Selected = true;
      Invalidate(GetTabRectangle(tab));
    }

    private Rectangle GetTabRectangle(SideBarTab tab)
    {
      int tabHeight = HiliteBackground?.Height ?? 25;
      int tabWidth = HiliteBackground?.Width ?? Width / 2;
      int top = TabTop + Tabs.TakeWhile(t => t != tab).Where(t => t.Page.PageVisible).Sum(t => tabHeight);
      return new Rectangle(Width-tabWidth, top, tabWidth, tabHeight);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);

      Brush semiBlackBrush = new SolidBrush(Color.FromArgb(180, 180, 180));
      Brush greyBrush = new SolidBrush(Color.Gray);
      Brush whiteBrush = new SolidBrush(Color.White);

      foreach (var tab in Tabs)
      {
        if (!tab.Page.PageVisible)
        {
          continue;
        }

        var tabRect = GetTabRectangle(tab);
        if (tab.Selected && HiliteBackground != null)
        {
          e.Graphics.DrawImage(HiliteBackground, tabRect);
        }

        var format = new StringFormat
        {
          LineAlignment = StringAlignment.Center
        };
        tabRect.Offset(29, 0);
        e.Graphics.DrawString(tab.Name, Font, tab.Page.Enabled ? tab.Selected ? whiteBrush : semiBlackBrush : greyBrush, tabRect, format);
      }
    }
  }
}
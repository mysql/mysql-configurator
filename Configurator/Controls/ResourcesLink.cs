/* Copyright (c) 2010, 2018, Oracle and/or its affiliates.

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

using System.Windows.Forms;
using MySql.Configurator.Core.Classes;

namespace MySql.Configurator.Controls
{
  public partial class ResourcesLink : UserControl
  {
    public ResourcesLink()
    {
      InitializeComponent();
    }

    public string Title
    {
      get { return TitleLinkLabel.Text; }
      set { TitleLinkLabel.Text = value; }
    }

    public string Url { get; set; }

    public string Description
    {
      get { return DescriptionLabel.Text; }
      set { DescriptionLabel.Text = value; }
    }

    private void TitleLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Utilities.OpenBrowser(Url);
    }
  }
}

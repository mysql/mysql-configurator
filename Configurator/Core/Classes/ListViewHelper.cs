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

using System.Windows.Forms;
using MySql.Configurator.Core.Enums;

namespace MySql.Configurator.Core.Classes
{
  public class ListViewHelper
  {
    private ListViewHelper()
    {
    }

    public static void SetExtendedStyle(Control control, ListViewExtendedStyles exStyle)
    {
      var styles = (ListViewExtendedStyles) Win32.SendMessage(control.Handle, (int)ListViewMessages.GetExtendedStyle, 0, 0);
      styles |= exStyle;
      Win32.SendMessage(control.Handle, (int)ListViewMessages.SetExtendedStyle, 0, (uint)styles);
    }

    public static void EnableDoubleBuffer(Control control)
    {
      // read current style
      var styles = (ListViewExtendedStyles)Win32.SendMessage(control.Handle, (int)ListViewMessages.GetExtendedStyle, 0, 0);
      // enable double buffer and border select
      styles |= ListViewExtendedStyles.DoubleBuffer | ListViewExtendedStyles.BorderSelect;
      // write new style
      Win32.SendMessage(control.Handle, (int)ListViewMessages.SetExtendedStyle, 0, (uint)styles);
    }
    public static void DisableDoubleBuffer(Control control)
    {
      // read current style
      var styles = (ListViewExtendedStyles) Win32.SendMessage(control.Handle, (int)ListViewMessages.GetExtendedStyle, 0, 0);
      // disable double buffer and border select
      styles -= styles & ListViewExtendedStyles.DoubleBuffer;
      styles -= styles & ListViewExtendedStyles.BorderSelect;
      // write new style
      Win32.SendMessage(control.Handle, (int)ListViewMessages.SetExtendedStyle, 0, (uint)styles);
    }
  }
}

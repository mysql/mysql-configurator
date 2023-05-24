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

using System.Collections;
using System.Windows.Forms;

namespace MySql.Configurator.Wizards.Server
{
  public class ColumnSorter : IComparer
  {
    public SortOrder Order { get; set; }
    public int Column { get; set; }
    private readonly CaseInsensitiveComparer _comparator;

    public ColumnSorter()
    {
      Order = SortOrder.None;
      Column = 0;
      _comparator = new CaseInsensitiveComparer();
    }

    public int Compare(object left, object right)
    {
      int result = 0; // Indicates objects are equal

      if (Order != SortOrder.None)
      {
        ListViewItem leftListViewItem = left as ListViewItem;
        ListViewItem rightListViewItem = right as ListViewItem;
        result = _comparator.Compare(leftListViewItem.SubItems[Column].Text,
                                     rightListViewItem.SubItems[Column].Text);
        if (Order == SortOrder.Descending)
        {
          result = -result;
        }
      }

      return result;
    }
  }
}

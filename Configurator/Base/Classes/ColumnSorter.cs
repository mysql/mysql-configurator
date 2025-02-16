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

using System.Collections;
using System.Windows.Forms;

namespace MySql.Configurator.Base.Classes
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

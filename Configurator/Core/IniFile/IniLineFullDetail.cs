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

using MySql.Configurator.Core.Enums;

namespace MySql.Configurator.Core.IniFile
{
  public class IniLineFullDetail
  {
    public string Section { get; private set; }

    public string Key { get; set; }

    public string Value { get; set; }

    public IniLineType IniLineType { get; private set; }

    public bool IsCommented { get; set; }

    public IniLineFullDetail(IniLineType lineType, bool isCommented = false, string section = null, string key = null, string value = null)
    {
      Section = section;
      Key = key;
      Value = value;
      IniLineType = lineType;
      IsCommented = isCommented;
    }
  }
}
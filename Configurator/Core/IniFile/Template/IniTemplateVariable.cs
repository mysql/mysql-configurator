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

namespace MySql.Configurator.Core.IniFile.Template
{
  internal class IniTemplateVariable : IniTemplateStatic
  {
    public IniTemplateVariable(string input, string name, string form, string opt)
      : base(input)
    {
      VariableName = name;
      Formula = form;
      Options = opt;
      ReduceResult = opt.Contains("USE_BYTES");
      Disabled = false;
      Valid = true;
      SupportsEmptyResult = false;
    }

    public string DefaultValue { get; set; }
    public bool Disabled { get; set; }
    public string Formula { get; set; }
    public string Options { get; }
    public string OutputParameter { get; set; }
    public bool ReduceResult { get; set; }
    public bool SupportsEmptyResult { get; set; }
    public bool Valid { get; set; }
    public string VariableName { get; }

  }
}

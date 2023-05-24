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

using MySql.Configurator.Properties;
using System;
using System.Linq;

namespace MySql.Configurator.Core.Controllers
{
  public class ControllerSettingAttribute : Attribute
  {
    private string _keywordString;

    /// <summary>
    /// Initializes a configurable controller setting.
    /// </summary>
    /// <param name="desc">The description of the setting.</param>
    /// <param name="keywords">The permitted keywords separated by a comma.</param>
    /// <param name="deprecatedKeywords">The permitted keyword that have been deprectaded.</param>
    /// <param name="useValueAsDefault">Flag to indicate if the default value should be assigned if the setting is not specified.</param>
    /// <param name="checkAction">Indicates the name of the validation that should be made for this setting.</param>
    public ControllerSettingAttribute(string desc, string keywords, string deprecatedKeywords = null, bool useValueAsDefault = false, string checkAction = null)
    {
      if (deprecatedKeywords != null)
      {
        DeprecatedKeywords = deprecatedKeywords.Split(',');
        for (int i = 0; i < DeprecatedKeywords.Length; i++)
        {
          DeprecatedKeywords[i] = DeprecatedKeywords[i].Trim().ToLowerInvariant();
        }
      }
      
      Description = desc;
      KeywordList = $"{keywords},{deprecatedKeywords}";
      _keywordString = keywords;
      Keywords = _keywordString.Split(',');
      for (int i = 0; i < Keywords.Length; i++)
      {
        Keywords[i] = Keywords[i].Trim().ToLowerInvariant();
      }

      UseValueAsDefault = useValueAsDefault;
      CheckAction = checkAction;
    }

    public string[] DeprecatedKeywords { get; set; }
    public string Description { get; private set; }
    public string KeywordList { get; private set; }
    public string[] Keywords { get; private set; }
    public bool UseValueAsDefault { get; private set; }

    /// <summary>
    /// Gets or sets the action to perform to validate the value for this attribute.
    /// </summary>
    public string CheckAction { get; private set; }

    public bool IsValidKeyword(string keyword)
    {
      if (DeprecatedKeywords != null
          && DeprecatedKeywords.Any(k => k == keyword))
      {
        Console.WriteLine(string.Format(Resources.ConfigurationSettingDeprecatedKeyword, keyword, _keywordString));
        return true;
      }

      return Keywords.Any(k => k == keyword);
    }
  }
}

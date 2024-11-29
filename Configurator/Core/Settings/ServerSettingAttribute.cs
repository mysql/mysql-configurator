/* Copyright (c) 2024, Oracle and/or its affiliates.

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

using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Configurator.Base.Enums;

namespace MySql.Configurator.Core.Settings
{
  /// <summary>
  /// Defines a special attribute used to mark a server setting as configurable for a user.
  /// </summary>
  public class ServerSettingAttribute : Attribute
  {
    /// <summary>
    /// Initializes a configurable server setting.
    /// </summary>
    /// <param name="description">The description of the setting.</param>
    /// <param name="aliases">The permitted aliases.</param>
    /// <param name="required">Flag indicating if the setting is required.</param>
    /// <param name="deprecatedKeywords">The permitted aliases that have been deprectaded.</param>
    /// <param name="checkAction">Indicates the name of the validation that should be made for this setting.</param>
    public ServerSettingAttribute(string description,
      string name,
      string[] aliases,
      bool required = false,
      string shortcut = null,
      ConfigurationType supportedConfigurationTypes = ConfigurationType.Configure | ConfigurationType.Reconfigure,
      string[] supportedValues = null, 
      string checkAction = null,
      string[] deprecatedKeywords = null)
    {
      Name = name;
      Keywords = new List<string>();
      if (aliases != null)
      {
        Keywords.AddRange(aliases);
      }

      AllKeywords = new List<string>();
      AllKeywords.Add(name);
      if (aliases != null)
      {
        AllKeywords.AddRange(aliases);
      }

      DeprecatedKeywords = new List<string>();
      if (deprecatedKeywords != null)
      {
        DeprecatedKeywords.AddRange(deprecatedKeywords);
        AllKeywords.AddRange(DeprecatedKeywords);
      }

      CheckAction = checkAction;
      Description = description;
      Required = required;
      Shortcut = shortcut;
      if (!string.IsNullOrEmpty(shortcut))
      {
        AllKeywords.Add(shortcut);
      }

      SupportedConfigurationTypes = supportedConfigurationTypes;
      SupportedValues = new List<string>();
      if (supportedValues != null)
      {
        SupportedValues.AddRange(supportedValues);
      }
    }

    #region Properties

    /// <summary>
    /// The full list of supported aliases.
    /// </summary>
    public List<string> AllKeywords { get; private set; }

    /// <summary>
    /// Gets or sets the action to perform to validate the value for this attribute.
    /// </summary>
    public string CheckAction { get; private set; }

    /// <summary>
    /// Gets or sets the list of valid aliases that are deprecated.
    /// </summary>
    public List<string> DeprecatedKeywords { get; private set; }

    /// <summary>
    /// Gets or sets the description of the server setting.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Gets or sets the list of valid aliases that are not deprecated.
    /// </summary>
    public List<string> Keywords { get; private set; }

    /// <summary>
    /// Gets or sets the name of the server setting attribute.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets or sets a flag indicating if the server setting is required.
    /// </summary>
    public bool Required { get; private set; }

    /// <summary>
    /// Gets or set the shortcut used to refer to this server setting.
    /// </summary>
    public string Shortcut { get; private set; }

    /// <summary>
    /// Gets or sets the configuration types for which this server setting is valid.
    /// </summary>
    public ConfigurationType SupportedConfigurationTypes { get; private set; }

    /// <summary>
    /// Gets or sets the list of supported values.
    /// </summary>
    public List<string> SupportedValues { get; private set; }

    #endregion

    /// <summary>
    /// Validates if the specified keyword is supported for this server setting.
    /// </summary>
    /// <param name="keyword">The keyword to validate.</param>
    /// <returns><c>true</c> if the keyword is valid; otherwise, <c>false</c>.</returns>
    public bool IsValidKeyword(string keyword)
    {
      return AllKeywords.Any(k => k == keyword);
    }
  }
}

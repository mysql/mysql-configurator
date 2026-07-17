/* Copyright (c) 2024, 2025, Oracle and/or its affiliates.

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

using System.Collections.Generic;

namespace MySql.Configurator.Core.CLI
{
  public class CommandLineOption
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandLine"/> class.
    /// </summary>
    /// <param name="name">The name of the option.</param>
    /// <param name="description">The description of the option.</param>
    /// <param name="providedName">The name of the option provided by the user.</param>
    /// <param name="aliases">The supported aliases.</param>
    /// <param name="supportsValue">Flag indicating if the option supports a value.</param>
    /// <param name="supportsRepeat">Flag indicating if the option supports being repeated.</param>
    /// <param name="required">Flag indicating if the option is required.</param>
    /// <param name="shortcut">A string representing the shortcut name.</param>
    /// <param name="supportedValues">A list of supported values (if applicable).</param>
    /// <param name="checkAction">Indicates the name of the validation that should be made for this option.</param>
    /// <param name="deprecatedAliases">The permitted aliases that have been deprectaded.</param>
    /// <param name="conditions">The conditions that must be met before this option can be used.</param>
    public CommandLineOption(string name, string description, string providedName, string[] aliases = null, bool supportsValue = true, bool supportsRepeat = false, bool required = false, string shortcut = null, string[] supportedValues = null, string checkAction = null, string[] deprecatedAliases = null, List<KeyValuePair<string, string>> conditions = null)
    {
      Aliases = aliases;
      CheckAction = checkAction;
      Description = description;
      HasFixedValues = supportedValues != null
        && supportedValues.Length > 0;
      Name = name;
      Conditions = conditions;
      ProvidedName = providedName;
      Required = required;
      Shortcut = shortcut;
      SupportsRepeat = supportsRepeat;
      SupportsValue = supportsValue;
      SupportedValues = supportedValues;
      DeprecatedAliases = deprecatedAliases;
    }

    #region Properties

    /// <summary>
    /// Gets or sets an array of the aliases that can be used to refer to this option.
    /// </summary>
    public string[] Aliases { get; private set; }

    /// <summary>
    /// Gets or sets the type of validation to perform on the field.
    /// </summary>
    public string CheckAction { get; private set; }

    /// <summary>
    /// Gets or sets the list of aliases that are deprecated.
    /// </summary>
    public string[] DeprecatedAliases { get; private set; }

    /// <summary>
    /// Gets or sets the description of this option.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Gets or sets a flag indicating wether this option only supports certain values.
    /// </summary>
    public bool HasFixedValues { get; private set; }

    /// <summary>
    /// Gets or sets the name of this option.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets or sets a list of key value pairs indicating the conditions that 
    /// must be met to allow this setting to be used.
    /// </summary>
    public List<KeyValuePair<string, string>> Conditions { get; private set; }

    /// <summary>
    /// Gets or sets the name of the option provided by the user.
    /// </summary>
    public string ProvidedName { get; private set; }

    /// <summary>
    /// Gets or sets a flag indicating wether this option is required.
    /// </summary>
    public bool Required { get; private set; }

    /// <summary>
    /// Gets or sets a string representing an alternate shortcut name for this option.
    /// </summary>
    public string Shortcut { get; private set; }

    /// <summary>
    /// Gets or sets a flag indicating wether this option can be repeated in the command line.
    /// </summary>
    public bool SupportsRepeat { get; private set; }

    /// <summary>
    /// Gets or sets a flag indicating wether this options supports being assigned a value.
    /// </summary>
    public bool SupportsValue { get; private set; }

    /// <summary>
    /// Gets or sets an array represeting the supported values of this option.
    /// </summary>
    public string[] SupportedValues { get; private set; }

    /// <summary>
    /// Gets or sets the value of this option.
    /// </summary>
    public string Value { get; set; }

    #endregion
  }
}

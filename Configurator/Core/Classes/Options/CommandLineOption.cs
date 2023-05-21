// Copyright (c) 2023, Oracle and/or its affiliates.
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License as
// published by the Free Software Foundation; version 2 of the
// License.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
// 02110-1301  USA

using MySql.Configurator.Properties;
using System;
using System.Linq;

namespace MySql.Configurator.Core.Classes.Options
{
  /// <summary>
  /// Represents a single switch or command line option
  /// </summary>
  public class CommandLineOption
  {
    #region Fields

    /// <summary>
    /// The characters that can be used as separators for values.
    /// </summary>
    private static readonly char[] _assignmentCharacters = {'=', ':', ' '};

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandLineOption"/> class.
    /// </summary>
    /// <param name="keys">The possible keys of this option separated by pipes ('|').</param>
    /// <param name="parsingAction">An action to execute when arguments are being parsed.</param>
    /// <param name="requiresValue">Flag indicating whether the option requires a value.</param>
    /// <param name="validValues">A string with possible valid values separated by commas.</param>
    public CommandLineOption(string keys, Action<string> parsingAction = null, bool requiresValue = false, string validValues = null)
      : this(keys?.Split('|'), parsingAction, requiresValue, !string.IsNullOrEmpty(validValues) ? validValues.Split(',') : null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandLineOption"/> class.
    /// </summary>
    /// <param name="keys">The possible keys of this option.</param>
    /// <param name="parsingAction">An action to execute when arguments are being parsed.</param>
    /// <param name="requiresValue">Flag indicating whether the option requires a value.</param>
    /// <param name="validValues">An array of possible valid values.</param>
    private CommandLineOption(string[] keys, Action<string> parsingAction = null, bool requiresValue = false, string[] validValues = null)
    {
      if (keys == null
          || keys.Length == 0)
      {
        throw new ArgumentNullException(nameof(keys));
      }

      if (keys.Distinct().Count() != keys.Length)
      {
        throw new ArgumentException(Resources.CommandLineOptionDuplicateKeysError);
      }

      if (keys.Any(k => k.IndexOfAny(_assignmentCharacters) >= 0))
      {
        throw new ArgumentException(string.Format(Resources.CommandLineOptionInvalidCharactersError,_assignmentCharacters));
      }

      Keys = keys;
      ParsingAction = parsingAction;
      RequiresValue = requiresValue;
      ValidValues = validValues;
    }

    #region Properties

    /// <summary>
    /// Gets or sets the character used to assign a value.
    /// </summary>
    public char AssignmentCharacter { get; set; }

    /// <summary>
    /// Gets a value indicating whether this option is present on a given command line string.
    /// </summary>
    public bool IsPresent { get; set; }

    /// <summary>
    /// Gets the main key of this option.
    /// </summary>
    public string Key => Keys.Length > 0 ? Keys[0] : null;

    /// <summary>
    /// Gets the possible keys of this option.
    /// </summary>
    public string[] Keys { get; }

    /// <summary>
    /// Gets an action to execute when arguments are being parsed.
    /// </summary>
    public Action<string> ParsingAction { get; }

    /// <summary>
    /// Gets or sets the command prefix used for the option.
    /// </summary>
    public string Prefix { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the option requires a value.
    /// </summary>
    public bool RequiresValue { get; }

    /// <summary>
    /// Gets an array of possible valid values.
    /// </summary>
    public string[] ValidValues { get; }

    /// <summary>
    /// Gets or sets the value of this option.
    /// </summary>
    public string Value { get; set; }

    #endregion Properties

    /// <summary>
    /// Invokes the <see cref="ParsingAction"/>.
    /// </summary>
    /// <param name="invokeEvenIfNotPresent">Flag indicating whether the action is invoked even if the option is not present in a command line options string.</param>
    public void InvokeAction(bool invokeEvenIfNotPresent = false)
    {
      if (!IsPresent
          && !invokeEvenIfNotPresent)
      {
        return;
      }

      ParsingAction?.Invoke(Value);
    }

    /// <summary>
    /// Verifies if a given value is valid for this option.
    /// </summary>
    /// <param name="value">A value as a string.</param>
    /// <returns><c>true</c> if the value is valid for this option, <c>false</c> otherwise.</returns>
    public bool IsValidValue(string value)
    {
      return ValidValues == null
             || ValidValues.Any(pv => string.Equals(value, pv, StringComparison.CurrentCultureIgnoreCase));
    }

    /// <summary>
    /// Resets values for this option to the defaults before parsing.
    /// </summary>
    public void Reset()
    {
      Prefix = null;
      AssignmentCharacter = char.MinValue;
      IsPresent = false;
      Value = null;
    }

    /// <summary>
    /// Validates the value for this option.
    /// </summary>
    /// <param name="validateEvenIfNotPresent">Flag indicating whether the validation is performed even if the option is not present in a command line options string.</param>
    public void Validate(bool validateEvenIfNotPresent = false)
    {
      if (!IsPresent
          && !validateEvenIfNotPresent)
      {
        return;
      }

      if (RequiresValue
          && AssignmentCharacter == char.MinValue)
      {
        throw new ArgumentException(string.Format(Resources.CommandLineOptionRequiresValueError,
          string.Join("|", Keys)));
      }

      if (!IsValidValue(Value))
      {
        throw new ArgumentException(string.Format(Resources.CommandLineOptionInvalidValueError, Value,
          string.Join("|", Keys)));
      }
    }
  }
}

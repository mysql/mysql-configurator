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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace MySql.Configurator.Core.Classes.Options
{
  /// <summary>
  /// Represents a collection of <see cref="CommandLineOption"/> objects.
  /// </summary>
  public class CommandLineOptions : KeyedCollection<string, CommandLineOption>
  {
    #region Constants

    /// <summary>
    /// A regular expression string to parse a command line argument.
    /// </summary>
    public const string ARGUMENT_PARTS_REGEX = "^(?<prefix>--|-|/)(?<name>[^:=]+)((?<assignment>[:=])(?<value>.*))?$";

    #endregion Constants

    #region Fields

    /// <summary>
    /// A regular expression to parse a command line argument.
    /// </summary>
    private readonly Regex _argumentPartsRegex;

    /// <summary>
    /// The <see cref="CommandLineOption"/> parsed on the previous argument.
    /// </summary>
    private CommandLineOption _lastParsedOption;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandLineOptions"/> class.
    /// </summary>
    public CommandLineOptions()
    {
      _argumentPartsRegex = new Regex(ARGUMENT_PARTS_REGEX);
      _lastParsedOption = null;
      UnmatchedOptions = new List<CommandLineOption>();
    }

    #region Properties

    /// <summary>
    /// Gets a list of options that were parsed successfully but that do not match any command line option in the collection.
    /// </summary>
    public List<CommandLineOption> UnmatchedOptions { get; }

    #endregion Properties

    /// <summary>
    /// Parses the given arguments line to match command line options added to the collection.
    /// </summary>
    /// <param name="argumentsLine">A text line representing a command line arguments.</param>
    /// <param name="skipArguments">Optional number of arguments to skip.</param>
    public void Parse(string argumentsLine, int skipArguments)
    {
      if (string.IsNullOrEmpty(argumentsLine))
      {
        return;
      }

      var arguments = Utilities.SplitArgs(argumentsLine).Skip(skipArguments);
      Parse(arguments);
    }

    /// <summary>
    /// Parses the given arguments to match command line options added to the collection.
    /// </summary>
    /// <param name="arguments">A collection of arguments.</param>
    public void Parse(IEnumerable<string> arguments)
    {
      UnmatchedOptions.Clear();
      foreach (var option in this)
      {
        option.Reset();
      }

      foreach (var argument in arguments)
      {
        var wasParsed = Parse(argument);
        if (!wasParsed
            && _lastParsedOption != null
            && string.IsNullOrEmpty(_lastParsedOption.Value))
        {
          // The argument is really the value of the last processed argument which was a command line option
          _lastParsedOption.Value = argument;
          _lastParsedOption.AssignmentCharacter = ' ';
        }
      }

      foreach (var option in this)
      {
        option.Validate();
        option.InvokeAction();
      }
    }

    /// <summary>
    /// Extracts the key from the specified element.
    /// </summary>
    /// <param name="item">The element from which to extract the key.</param>
    /// <returns>The key for the specified element.</returns>
    protected override string GetKeyForItem(CommandLineOption item)
    {
      return item.Key;
    }

    /// <summary>
    /// Inserts an element into the collection at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index at which <seealso cref="item"/> should be inserted.</param>
    /// <param name="item">The object to insert.</param>
    protected override void InsertItem(int index, CommandLineOption item)
    {
      base.InsertItem(index, item);
      AddExtraKeys(item);
    }

    /// <summary>
    /// Parses a specific command line argument.
    /// </summary>
    /// <param name="argument">A command line argument.</param>
    /// <returns><c>true</c> if the argument was successfully parsed, <c>false</c> otherwise.</returns>
    protected virtual bool Parse(string argument)
    {
      var argumentParts = SplitArgument(argument);
      if (argumentParts == null)
      {
        return false;
      }

      _lastParsedOption = null;
      var prefix = argumentParts[0];
      var name = argumentParts[1];
      var assignment = argumentParts[2];
      var value = argumentParts[3];
      if (!Contains(name))
      {
        _lastParsedOption = new CommandLineOption(name)
        {
          IsPresent = true,
          Value = value
        };
        UnmatchedOptions.Add(_lastParsedOption);
        return true;
      }

      _lastParsedOption = this[name];
      _lastParsedOption.IsPresent = true;
      _lastParsedOption.Prefix = prefix;
      if (!string.IsNullOrEmpty(assignment))
      {
        _lastParsedOption.AssignmentCharacter = assignment[0];
        _lastParsedOption.Value = value;
      }

      return true;
    }

    /// <summary>
    /// Removes the element at the specified index of the collection.
    /// </summary>
    /// <param name="index">The index of the element to remove.</param>
    protected override void RemoveItem(int index)
    {
      var removingOption = this[index];
      base.RemoveItem(index);

      // Handle the removal of the extra keys.
      foreach (var key in removingOption.Keys.Skip(1))
      {
        Dictionary.Remove(key);
      }
    }

    /// <summary>
    /// Replaces the item at the specified index with the specified item.
    /// </summary>
    /// <param name="index">The zero-based index of the item to be replaced.</param>
    /// <param name="item">The new item.</param>
    protected override void SetItem(int index, CommandLineOption item)
    {
      base.SetItem(index, item);
      RemoveItem(index);
      AddExtraKeys(item);
    }

    /// <summary>
    /// Attempts to split the given argument in parts (prefix, name, assignment, value).
    /// </summary>
    /// <param name="argument">A command line argument.</param>
    /// <returns>An array with the following parts: prefix, name, assignment, value, or <c>null</c> if the argument did not match a well-formed argument.</returns>
    protected string[] SplitArgument(string argument)
    {
      if (string.IsNullOrEmpty(argument))
      {
        return null;
      }

      var parts = new string[4];
      var match = _argumentPartsRegex.Match(argument);
      if (!match.Success)
      {
        return null;
      }

      parts[0] = match.Groups["prefix"].Value;
      parts[1] = match.Groups["name"].Value;
      if (match.Groups["assignment"].Success
          && match.Groups["value"].Success)
      {
        parts[2] = match.Groups["assignment"].Value;
        parts[3] = match.Groups["value"].Value;
      }

      return parts;
    }

    /// <summary>
    /// Handles extra keys within the options (from the 2nd onwards).
    /// </summary>
    /// <param name="option">A <see cref="CommandLineOption"/> instance.</param>
    private void AddExtraKeys(CommandLineOption option)
    {
      if (option == null)
      {
        return;
      }

      foreach (var key in option.Keys.Skip(1))
      {
        Dictionary.Add(key, option);
      }
    }
  }
}

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Configurator.Base.Classes;
using MySql.Configurator.Base.Enums;
using MySql.Configurator.Core.Server;
using MySql.Configurator.Core.Settings;

namespace MySql.Configurator.Core.CLI
{
  /// <summary>
  /// Parses the arguments provided by the user in the command line.
  /// </summary>
  public class CommandLineParser
  {
    #region Constants

    /// <summary>
    /// The option name for adding a new custom user.
    /// </summary>
    public const string ADD_USER_OPTION_NAME = "--add-user";

    #endregion

    /// <summary>
    /// Initializes static elements of this class whenever it is first referenced.
    /// </summary>
    static CommandLineParser()
    {
      ProvidedOptions = new List<CommandLineOption>();
      SupportedOptions = new List<CommandLineOption>()
      {
        new CommandLineOption("console", null, null, null, false, false, false, "c"),
        new CommandLineOption("action", null, null, null, true, false, false, "a", new string[]{ "configure", "reconfigure", "upgrade", "remove", "removenoshow" }),
        new CommandLineOption("help", null, null, null, false, false, false,"h"),
        new CommandLineOption("add-user", null, null, null, true, true)
      };
      SupportedOptions.AddRange(GetServerConfigurableSettings());
    }

    #region Properties

    /// <summary>
    /// Gets or sets the list of user provided options.
    /// </summary>
    public static List<CommandLineOption> ProvidedOptions { get; set; }

    /// <summary>
    /// Gets or sets the command line general options.
    /// </summary>
    public static List<CommandLineOption> SupportedOptions { get; set; }

    #endregion

    /// <summary>
    /// Gets the matching command line option from the user provided options collection.
    /// </summary>
    /// <param name="optionName">The option name.</param>
    /// <returns>A matching <see cref="CommandLineOption"/>; otherwise, <c>null</c>.</returns>
    public static CommandLineOption GetMatchingProvidedOption(string optionName)
    {
      return GetMatchingOption(ProvidedOptions, optionName);
    }

    /// <summary>
    /// Gets the matching command line option from the supported options collection.
    /// </summary>
    /// <param name="optionName">The option name.</param>
    /// <returns>A matching <see cref="CommandLineOption"/>; otherwise, <c>null</c>.</returns>
    public static CommandLineOption GetMatchingSupportedOption(string optionName)
    {
      return GetMatchingOption(SupportedOptions, optionName);
    }

    /// <summary>
    /// Gets the server configurable properties.
    /// </summary>
    /// <param name="action">The action to filter the actions for.</param>
    /// <returns>A list of the server configurable properties.</returns>
    public static List<CommandLineOption> GetServerConfigurableSettings(ConfigurationType action = ConfigurationType.None)
    {
      var options = typeof(MySqlServerSettings).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(ServerSettingAttribute)))
        .Select(propertyInfo => propertyInfo.GetCustomAttributes(typeof(ServerSettingAttribute), true)
        .FirstOrDefault() as ServerSettingAttribute);

      if (action != ConfigurationType.None)
      {
        options = options.Where(attribute => attribute != null && attribute.SupportedConfigurationTypes.HasFlag(action));
      }

      return options.Select(attribute => new CommandLineOption(attribute.Name, attribute.Description, null, attribute.Keywords.ToArray(), true, false, attribute.Required, attribute.Shortcut, attribute.SupportedValues?.ToArray(), attribute.CheckAction, attribute.DeprecatedAliases?.ToArray())).ToList();
    }

    /// <summary>
    /// Parses the --add-user option.
    /// </summary>
    /// <param name="value">The value to parse.</param>
    /// <returns>A <see cref="CLIExitCode"/> instance representing the result of the parsing of the value.</returns>
    public static CLIExitCode ParseAddUserOption(string value, out string[] serverUserItems)
    {
      serverUserItems = null;
      if (string.IsNullOrEmpty(value))
      {
        return new CLIExitCode(ExitCode.EmptyUserBlock, ADD_USER_OPTION_NAME);
      }

      serverUserItems = new string[5];
      int itemIndex = 0;
      var builder = new StringBuilder();
      var characters = value.ToCharArray();
      var index = 0;
      bool readingSingleQuoteBlock = false;
      while (index < characters.Length)
      {
        var character = characters[index];
        if (character == '\'')
        {
          readingSingleQuoteBlock = !readingSingleQuoteBlock;
        }

        // Assign value if we will start reading the next element.
        if (!readingSingleQuoteBlock
            && character == ':')
        {
          serverUserItems[itemIndex] = builder.ToString();
          builder.Clear();
          itemIndex++;
          if (itemIndex > 4)
          {
            return new CLIExitCode(ExitCode.TooManyElementsInUserBlock, value, ADD_USER_OPTION_NAME);
          }
        }
        else
        {
          builder.Append(character);
        }

        index++;
      }

      if (builder.Length > 0)
      {
        serverUserItems[itemIndex] = builder.ToString();
      }

      // Ensure all quotes were closed.
      if (readingSingleQuoteBlock)
      {
        return new CLIExitCode(ExitCode.MissingCustomUserClosingQuote, ADD_USER_OPTION_NAME, builder.ToString());
      }

      // All elements are mandatory except for the windows security token list.
      for(int i = 0; i< serverUserItems.Length - 1; i++)
      {
        if (string.IsNullOrEmpty(serverUserItems[i]))
        {
          return new CLIExitCode(ExitCode.InvalidCustomUserEmptyValue, value, ADD_USER_OPTION_NAME);
        }
      }

      // User name is expected to be enclosed in single or double quotes.
      if (!serverUserItems[0].StartsWith("'")
           && !serverUserItems[0].EndsWith("'"))
      {
        return new CLIExitCode(ExitCode.InvalidCustomUserUserNameValue, serverUserItems[0], ADD_USER_OPTION_NAME);
      }

      // User password/token is expected to be enclosed in single or double quotes.
      if (!serverUserItems[1].StartsWith("'")
           && !serverUserItems[1].EndsWith("'"))
      {
        return new CLIExitCode(ExitCode.InvalidCustomUserPasswordValue, serverUserItems[1], ADD_USER_OPTION_NAME);
      }

      // Role is expected to be enclosed in single or double quotes.
      if (!serverUserItems[3].StartsWith("'")
           && !serverUserItems[3].EndsWith("'"))
      {
        return new CLIExitCode(ExitCode.InvalidCustomUserRoleValue, serverUserItems[3], ADD_USER_OPTION_NAME);
      }

      // Validate Windows security token is populated (if applicable).
      if (serverUserItems[4].Equals("Windows", StringComparison.InvariantCultureIgnoreCase)
          && string.IsNullOrEmpty(serverUserItems[5]))
      {
        return new CLIExitCode(ExitCode.InvalidCustomUserEmptyToken, value, ADD_USER_OPTION_NAME);
      }

      return new CLIExitCode(ExitCode.Success);
    }

    /// <summary>
    /// Parses the arguments passed on to the command line. 
    /// </summary>
    /// <param name="arguments">The list of arguments to parse.</param>
    /// <returns>A <see cref="CLIExitCode"/> representing the resulting of the parsing of the provided arguments.</returns>
    public static CLIExitCode ParseCommandLineArguments(string[] arguments)
    {
      if (arguments == null
          || arguments.Length == 0)
      {
        throw new ArgumentNullException(nameof(arguments));
      }

      ProvidedOptions.Clear();
      foreach (var argument in arguments)
      {
        var argumentParsingResult = ParseArgument(argument);
        if (argumentParsingResult.ExitCode != ExitCode.Success)
        {
          return argumentParsingResult;
        }
      }

      var consoleOption = GetMatchingProvidedOption("console");
      var actionOption = GetMatchingProvidedOption("action");
      var helpOption = GetMatchingProvidedOption("help");
      AppConfiguration.ConsoleMode = consoleOption != null;
      if (consoleOption != null)
      {
        ProvidedOptions.Remove(consoleOption);
      }

      if ((!AppConfiguration.ConsoleMode
           && (((actionOption != null
                 && !actionOption.Value.Equals("configure", StringComparison.InvariantCultureIgnoreCase)
                 && !actionOption.Value.Equals("reconfigure", StringComparison.InvariantCultureIgnoreCase)
                 && !actionOption.Value.Equals("remove", StringComparison.InvariantCultureIgnoreCase)
                 && !actionOption.Value.Equals("removenoshow", StringComparison.InvariantCultureIgnoreCase))
                || (actionOption == null 
                    && ProvidedOptions.Count > 0))))
         || (AppConfiguration.ConsoleMode
             && helpOption != null
             && ((actionOption == null
                  && ProvidedOptions.Count > 1)
                  || (actionOption != null
                      && ProvidedOptions.Count > 2))))
      {
        // If console option was not provided and action is different than configure, reconfigure or remove
        // then the combination is not supported.
        return new CLIExitCode(ExitCode.TooManyArguments);
      }

      return new CLIExitCode(ExitCode.Success);
    }

    /// <summary>
    /// Gets the matching command line option from the specified collection.
    /// </summary>
    /// <param name="collection">The colllection to check for the matching option.</param>
    /// <param name="optionName">The option name.</param>
    /// <returns>A matching <see cref="CommandLineOption"/>; otherwise, <c>null</c>.</returns>
    private static CommandLineOption GetMatchingOption(List<CommandLineOption> collection, string optionName)
    {
      return collection.FirstOrDefault(option => (option.Name.Equals(optionName, StringComparison.InvariantCultureIgnoreCase)
                                                  || (option.Aliases != null
                                                      && option.Aliases.Contains(optionName, StringComparer.InvariantCultureIgnoreCase))
                                                  || (option.DeprecatedAliases != null
                                                      && option.DeprecatedAliases.Contains(optionName, StringComparer.InvariantCultureIgnoreCase))
                                                  || (!string.IsNullOrEmpty(option.Shortcut)
                                                      && option.Shortcut.Equals(optionName, StringComparison.InvariantCulture))));

    }

    /// <summary>
    /// Checks that the provided option is valid.
    /// </summary>
    /// <param name="optionName">The option name.</param>
    /// <param name="optionValue">The option value (if needed).</param>
    /// <returns>A <see cref="CLIExitCode"/> representing the result of the validation.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    private static CLIExitCode IsValidOption(string optionName, string optionValue)
    {
      if (string.IsNullOrEmpty(optionName))
      {
        throw new ArgumentNullException(nameof(optionName));
      }

      var option = GetMatchingSupportedOption(optionName);
      CommandLineOption commandLineOption = null;
      if (option != null)
      {
        commandLineOption = new CommandLineOption(
          option.Name,
          option.Description,
          optionName,
          option.Aliases,
          option.SupportsValue,
          option.SupportsRepeat,
          option.Required,
          option.Shortcut,
          option.SupportedValues,
          option.CheckAction,
          option.DeprecatedAliases
        );
        commandLineOption.Value = optionValue;
      }
      
      if (commandLineOption == null)
      {
        return new CLIExitCode(ExitCode.InvalidOption, optionName);
      }
      else if (commandLineOption.SupportsValue
               && string.IsNullOrEmpty(optionValue))
      {
        return new CLIExitCode(ExitCode.OptionValueNotFound, optionName);
      }
      else if(!commandLineOption.SupportsValue
              && !string.IsNullOrEmpty(optionValue))
      {
        return new CLIExitCode(ExitCode.OptionDoesNotSupportValue, optionName);
      }
      else if (commandLineOption.HasFixedValues
               && commandLineOption.SupportedValues.FirstOrDefault(value => value.Equals(optionValue, StringComparison.InvariantCultureIgnoreCase)) == null)
      {
        return new CLIExitCode(ExitCode.InvalidOptionValue, optionValue, optionName);
      }
      else if (!commandLineOption.SupportsRepeat
               && (GetMatchingProvidedOption(commandLineOption.Name) != null))
      {
        return new CLIExitCode(ExitCode.RepeatedOption, optionName);
      }

      commandLineOption.Value = optionValue;
      ProvidedOptions.Add(commandLineOption);
      return new CLIExitCode(ExitCode.Success);
    }

    /// <summary>
    /// Parses the provided command line argument.
    /// </summary>
    /// <param name="argument">The argument to parse.</param>
    /// <returns>A <see cref="CLIExitCode"/> representing the result of the parsing of the argument.</returns>
    private static CLIExitCode ParseArgument(string argument)
    {
      if (string.IsNullOrEmpty(argument))
      {
        return new CLIExitCode(ExitCode.NoArgument);
      }

      // Special cases until we ask RE to update MSI to use new syntax.
      if (argument.Equals("--remove", StringComparison.InvariantCultureIgnoreCase))
      {
        argument = "--action=remove";
      }

      if (argument.Equals("--removenoshow", StringComparison.InvariantCultureIgnoreCase))
      {
        argument = "--action=removenoshow";
      }

      // Remove trailing - or --.
      if (argument.StartsWith("--"))
      {
        argument = argument.Substring(2);
      }
      else if (argument.StartsWith("-"))
      {
        argument = argument.Substring(1);
      }
      else
      {
        return new CLIExitCode(ExitCode.InvalidOptionSyntax, argument);
      }

      if (string.IsNullOrEmpty(argument))
      {
        return new CLIExitCode(ExitCode.InvalidGenericSyntax);
      }

      // Separate into key value pair (if applicable)
      string optionName = null;
      string optionValue = null;
      if (argument.Contains("="))
      {
        var items = argument.Split('=');
        if (items.Length != 2)
        {
          return new CLIExitCode(ExitCode.InvalidOptionSyntax, argument);
        }

        optionName = items[0];
        optionValue = items[1];
      }
      else
      {
        optionName = argument;
      }

      return IsValidOption(optionName, optionValue);
    }
  }
}

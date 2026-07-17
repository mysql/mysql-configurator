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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;
using MySql.Configurator.Core.CLI;
using MySql.Configurator.Base.Enums;
using MySql.Configurator.Core.Settings;

namespace MySQLConfigurator.Test
{
  [TestClass]
  public class CommandLineTests
  {
    [TestMethod]
    public void ValidateGetMatchingSupportedOption()
    {
      var parser = new CommandLineParser();
      var privateObject = new PrivateObject(parser);
      var methodName = "GetMatchingSupportedOption";
      var bindingFlags = BindingFlags.Public | BindingFlags.Static;
      var option = privateObject.Invoke(methodName, bindingFlags, new object[] { "datadir" });
      Assert.AreEqual(true, option != null);
      option = privateObject.Invoke(methodName, bindingFlags, new object[] { "mysqlx-port" });
      Assert.AreEqual(true, option != null);
      option = privateObject.Invoke(methodName, bindingFlags, new object[] { "xport" });
      Assert.AreEqual(true, option != null);
      var portOption = privateObject.Invoke(methodName, bindingFlags, new object[] { "P" }) as CommandLineOption;
      Assert.AreEqual(true, portOption != null);
      Assert.AreEqual(true, portOption.Name.Equals("port"));
      var passwordOption = privateObject.Invoke(methodName, bindingFlags, new object[] { "p" }) as CommandLineOption;
      Assert.AreEqual(true, passwordOption != null);
      Assert.AreEqual(true, passwordOption.Name.Equals("password"));
    }

    [TestMethod]
    public void ValidateIsValidOptionForServerSettings()
    {
      var parser = new CommandLineParser();
      var privateObject = new PrivateObject(parser);
      var methodName = "IsValidOption";
      var bindingFlags = BindingFlags.NonPublic | BindingFlags.Static;
      var result = privateObject.Invoke(methodName, bindingFlags, new object[] { "datadir", null }) as CLIExitCode;
      Assert.AreEqual(ExitCode.OptionValueNotFound, result.ExitCode);
      result = privateObject.Invoke(methodName, bindingFlags, new object[] { "datadir", "C:\\Mysql" }) as CLIExitCode;
      Assert.AreEqual(ExitCode.Success, result.ExitCode);
    }

    [TestMethod]
    public void ValidateIsValidOption()
    {
      var parser = new CommandLineParser();
      var privateObject = new PrivateObject(parser);
      var methodName = "IsValidOption";
      var bindingFlags = BindingFlags.NonPublic | BindingFlags.Static;
      Assert.ThrowsException<ArgumentNullException>(() => privateObject.Invoke(methodName, bindingFlags, new object[] { null, null }));
      var result = privateObject.Invoke(methodName, bindingFlags, new object[] { "--console", null }) as CLIExitCode;
      Assert.AreEqual(ExitCode.InvalidOption, result.ExitCode);
      result = privateObject.Invoke(methodName, bindingFlags, new object[] { "console", null }) as CLIExitCode;
      Assert.AreEqual(ExitCode.Success, result.ExitCode);
      result = privateObject.Invoke(methodName, bindingFlags, new object[] { "console", null }) as CLIExitCode;
      Assert.AreEqual(ExitCode.RepeatedOption, result.ExitCode);
      result = privateObject.Invoke(methodName, bindingFlags, new object[] { "invalidoption", null }) as CLIExitCode;
      Assert.AreEqual(ExitCode.InvalidOption, result.ExitCode);
      result = privateObject.Invoke(methodName, bindingFlags, new object[] { "action", null }) as CLIExitCode;
      Assert.AreEqual(ExitCode.OptionValueNotFound, result.ExitCode);
      result = privateObject.Invoke(methodName, bindingFlags, new object[] { "action", "redo" }) as CLIExitCode;
      Assert.AreEqual(ExitCode.InvalidOptionValue, result.ExitCode);
      result = privateObject.Invoke(methodName, bindingFlags, new object[] { "a", "configure" }) as CLIExitCode;
      Assert.AreEqual(ExitCode.Success, result.ExitCode);
      CommandLineParser.ProvidedOptions = new List<CommandLineOption>();
      result = privateObject.Invoke(methodName, bindingFlags, new object[] { "action", "configure" }) as CLIExitCode;
      Assert.AreEqual(ExitCode.Success, result.ExitCode);
      CommandLineParser.ProvidedOptions = new List<CommandLineOption>();
      result = privateObject.Invoke(methodName, bindingFlags, new object[] { "action", "REMOVE" }) as CLIExitCode;
      Assert.AreEqual(ExitCode.Success, result.ExitCode);
      result = privateObject.Invoke(methodName, bindingFlags, new object[] { "A", "configure" }) as CLIExitCode;
      Assert.AreEqual(ExitCode.InvalidOption, result.ExitCode);
      CommandLineParser.ProvidedOptions = new List<CommandLineOption>();
      result = privateObject.Invoke(methodName, bindingFlags, new object[] { "a", "upgrade" }) as CLIExitCode;
      Assert.AreEqual(ExitCode.Success, result.ExitCode);
    }

    [TestMethod]
    public void ValidateParseArgument()
    {
      var parser = new CommandLineParser();
      var privateObject = new PrivateObject(parser);
      var methodName = "ParseArgument";
      var bindingFlags = BindingFlags.NonPublic | BindingFlags.Static;
      var result = privateObject.Invoke(methodName, bindingFlags, new object[] { null }) as CLIExitCode;
      Assert.AreEqual(ExitCode.NoArgument, result.ExitCode);
      result = privateObject.Invoke(methodName, bindingFlags, new object[] { string.Empty }) as CLIExitCode;
      Assert.AreEqual(ExitCode.NoArgument, result.ExitCode);
      result = privateObject.Invoke(methodName, bindingFlags, new object[] { "" }) as CLIExitCode;
      Assert.AreEqual(ExitCode.NoArgument, result.ExitCode);
      result = privateObject.Invoke(methodName, bindingFlags, new object[] { "console" }) as CLIExitCode;
      Assert.AreEqual(ExitCode.InvalidOptionSyntax, result.ExitCode);
      result = privateObject.Invoke(methodName, bindingFlags, new object[] { "console=" }) as CLIExitCode;
      Assert.AreEqual(ExitCode.InvalidOptionSyntax, result.ExitCode);
      result = privateObject.Invoke(methodName, bindingFlags, new object[] { "console=true=false" }) as CLIExitCode;
      Assert.AreEqual(ExitCode.InvalidOptionSyntax, result.ExitCode);
      result = privateObject.Invoke(methodName, bindingFlags, new object[] { "--console" }) as CLIExitCode;
      Assert.AreEqual(ExitCode.Success, result.ExitCode);
    }

    /// <summary>
    /// Validates parsing of the command line options using the CLI parser.
    /// </summary>
    [TestMethod]
    public void ValidateParseCommandLineArguments()
    {
      var parser = new CommandLineParser();
      var privateObject = new PrivateObject(parser);
      var methodName = "ParseCommandLineArguments";
      var bindingFlags = BindingFlags.Public | BindingFlags.Static;
      Assert.ThrowsException<ArgumentNullException>(() => privateObject.Invoke(methodName, bindingFlags, new object[] { null }));
      var result = privateObject.Invoke(methodName, bindingFlags, new object[] { new string[] { "--console" } }) as CLIExitCode;
      Assert.AreEqual(true, AppConfiguration.ConsoleMode);
      Assert.AreEqual(ExitCode.Success, result.ExitCode);
      result = privateObject.Invoke(methodName, bindingFlags, new object[] { new string[] { "--action=remove" } }) as CLIExitCode;
      Assert.AreEqual(false, AppConfiguration.ConsoleMode);
      Assert.AreEqual(ExitCode.Success, result.ExitCode);
      result = privateObject.Invoke(methodName, bindingFlags, new object[] { new string[] { "--action=remove", "--port=3306" } }) as CLIExitCode;
      Assert.AreEqual(ExitCode.Success, result.ExitCode);
    }
  }
}

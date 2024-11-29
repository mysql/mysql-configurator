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
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MySql.Configurator;
using MySql.Configurator.Base.Classes;
using MySql.Configurator.Core.CLI;
using MySql.Configurator.Base.Enums;
using MySql.Configurator.Core.Server;
using MySql.Configurator.Core.Settings;

namespace MySQLConfigurator.Test
{
  [TestClass]
  public class CommandLineTests
  {
    /// <summary>
    /// Validates parsing of the command line options at the exe level. 
    /// </summary>
    //[TestMethod]
    //public void ValidateCommandLineParsing()
    //{
    //try
    //{
    //  privateObject.Invoke(methodName, bindingFlags, new object[] { new string[] { configuratorExeName, "configure" } });
    //}
    //catch (ConfiguratorException exception)
    //{
    //  Assert.AreEqual(ConfiguratorError.InvalidOptionStart, exception.ErrorCode);
    //}
    //}

    [TestMethod]
    public void ValidateGetMatchingSupportedOption()
    {
      var parser = new CommandLineParser();
      var privateObject = new PrivateObject(parser);
      var methodName = "GetMatchingSupportedOption";
      var bindingFlags = BindingFlags.NonPublic | BindingFlags.Static;
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
      Assert.AreEqual(ExitCode.OptionValueNotFound, privateObject.Invoke(methodName, bindingFlags, new object[] { "datadir", null }));
      Assert.AreEqual(ExitCode.Success, privateObject.Invoke(methodName, bindingFlags, new object[] { "datadir", "C:\\Mysql" }));
    }

    [TestMethod]
    public void ValidateIsValidOption()
    {
      var parser = new CommandLineParser();
      var privateObject = new PrivateObject(parser);
      var methodName = "IsValidOption";
      var bindingFlags = BindingFlags.NonPublic | BindingFlags.Static;
      Assert.ThrowsException<ArgumentNullException>(() => privateObject.Invoke(methodName, bindingFlags, new object[] { null, null }));
      Assert.AreEqual(ExitCode.InvalidOption, privateObject.Invoke(methodName, bindingFlags, new object[] { "--console", null }));
      Assert.AreEqual(ExitCode.Success, privateObject.Invoke(methodName, bindingFlags, new object[] { "console", null }));
      Assert.AreEqual(ExitCode.RepeatedOption, privateObject.Invoke(methodName, bindingFlags, new object[] { "console", null }));
      Assert.AreEqual(ExitCode.InvalidOption, privateObject.Invoke(methodName, bindingFlags, new object[] { "invalidoption", null }));
      Assert.AreEqual(ExitCode.OptionValueNotFound, privateObject.Invoke(methodName, bindingFlags, new object[] { "action", null }));
      Assert.AreEqual(ExitCode.InvalidOptionValue, privateObject.Invoke(methodName, bindingFlags, new object[] { "action", "redo" }));
      Assert.AreEqual(ExitCode.Success, privateObject.Invoke(methodName, bindingFlags, new object[] { "a", "configure" }));
      CommandLineParser.ProvidedOptions = new List<CommandLineOption>();
      Assert.AreEqual(ExitCode.Success, privateObject.Invoke(methodName, bindingFlags, new object[] { "action", "configure" }));
      CommandLineParser.ProvidedOptions = new List<CommandLineOption>();
      Assert.AreEqual(ExitCode.Success, privateObject.Invoke(methodName, bindingFlags, new object[] { "action", "REMOVE" }));
      Assert.AreEqual(ExitCode.InvalidOption, privateObject.Invoke(methodName, bindingFlags, new object[] { "A", "configure" }));
      CommandLineParser.ProvidedOptions = new List<CommandLineOption>();
      Assert.AreEqual(ExitCode.Success, privateObject.Invoke(methodName, bindingFlags, new object[] { "a", "upgrade" }));
    }

    [TestMethod]
    public void ValidateParseArgument()
    {
      var parser = new CommandLineParser();
      var privateObject = new PrivateObject(parser);
      var methodName = "ParseArgument";
      var bindingFlags = BindingFlags.NonPublic | BindingFlags.Static;
      Assert.AreEqual(ExitCode.NoArgument, privateObject.Invoke(methodName, bindingFlags, new object[] { null }));
      Assert.AreEqual(ExitCode.NoArgument, privateObject.Invoke(methodName, bindingFlags, new object[] { string.Empty }));
      Assert.AreEqual(ExitCode.NoArgument, privateObject.Invoke(methodName, bindingFlags, new object[] { "" }));
      Assert.AreEqual(ExitCode.InvalidOptionSyntax, privateObject.Invoke(methodName, bindingFlags, new object[] { "console" }));
      Assert.AreEqual(ExitCode.InvalidOptionSyntax, privateObject.Invoke(methodName, bindingFlags, new object[] { "console=" }));
      Assert.AreEqual(ExitCode.InvalidOptionSyntax, privateObject.Invoke(methodName, bindingFlags, new object[] { "console=true=false" }));
      Assert.AreEqual(ExitCode.Success, privateObject.Invoke(methodName, bindingFlags, new object[] { "--console" }));
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
      var exitCode = privateObject.Invoke(methodName, bindingFlags, new object[] { new string[] { "--console" } });
      Assert.AreEqual(true, AppConfiguration.ConsoleMode);
      Assert.AreEqual(ExitCode.Success, exitCode);
      exitCode = privateObject.Invoke(methodName, bindingFlags, new object[] { new string[] { "--action=remove" } });
      Assert.AreEqual(false, AppConfiguration.ConsoleMode);
      Assert.AreEqual(ExitCode.Success, exitCode);
      Assert.AreEqual(ExitCode.TooManyArguments, privateObject.Invoke(methodName, bindingFlags, new object[] { new string[] { "--action=remove", "--port=3306" } }));
    }
  }
}

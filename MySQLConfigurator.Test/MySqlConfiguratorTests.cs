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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Wizards.Server;
using MySql.Configurator.Core.Package;
using MySql.Configurator;
using System.Runtime;
using MySql.Configurator.Core.Common;
using System.Configuration;
using System.Runtime.InteropServices;

namespace MySQLConfigurator.Test
{
  [TestClass]
  public class MySQLConfiguratorTests
  {
    [TestMethod]
    public void ValidateSupportedUpgradeScenarios()
    {
      Assembly assembly = Assembly.GetExecutingAssembly();
      FileVersionInfo currentFileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
      var currentVersion = new Version(currentFileVersionInfo.FileVersion);

      // Unsupported scenarios.
      Assert.AreEqual(UpgradeViability.Unsupported, currentVersion.ServerSupportsInPlaceUpgrades(new Version(5, 6, 0)));
      Assert.AreEqual(UpgradeViability.Unsupported, currentVersion.ServerSupportsInPlaceUpgrades(new Version(5, 7, 0)));
      Assert.AreEqual(UpgradeViability.Unsupported, currentVersion.ServerSupportsInPlaceUpgrades(currentVersion));
      Assert.AreEqual(UpgradeViability.Unsupported, currentVersion.ServerSupportsInPlaceUpgrades(currentVersion));
      
      // Unsupported with warning scenarios.
      Assert.AreEqual(UpgradeViability.UnsupportedWithWarning, currentVersion.ServerSupportsInPlaceUpgrades(new Version(8, 0, 0)));
      Assert.AreEqual(UpgradeViability.UnsupportedWithWarning, currentVersion.ServerSupportsInPlaceUpgrades(new Version(8, 0, 34)));
      Assert.AreEqual(UpgradeViability.UnsupportedWithWarning, new Version(8, 4, 0).ServerSupportsInPlaceUpgrades(new Version(8, 2, 0)));
      Assert.AreEqual(UpgradeViability.UnsupportedWithWarning, new Version(8, 5, 0).ServerSupportsInPlaceUpgrades(new Version(8, 3, 0)));
      Assert.AreEqual(UpgradeViability.UnsupportedWithWarning, new Version(9, 0, 0).ServerSupportsInPlaceUpgrades(new Version(8, 3, 0)));
      Assert.AreEqual(UpgradeViability.UnsupportedWithWarning, new Version(8, 3, 1).ServerSupportsInPlaceUpgrades(new Version(8, 3, 0)));
      Assert.AreEqual(UpgradeViability.UnsupportedWithWarning, new Version(9, 1, 0).ServerSupportsInPlaceUpgrades(new Version(8, 4, 1)));
      Assert.AreEqual(UpgradeViability.UnsupportedWithWarning, new Version(8, 2, 0).ServerSupportsInPlaceUpgrades(new Version(8, 0, 35)));

      // Supported scenarios.
      Assert.AreEqual(UpgradeViability.Supported, new Version(8, 4, 0).ServerSupportsInPlaceUpgrades(new Version(8, 0, 35)));
      Assert.AreEqual(UpgradeViability.Supported, new Version(8, 1, 0).ServerSupportsInPlaceUpgrades(new Version(8, 0, 35)));
      Assert.AreEqual(UpgradeViability.Supported, new Version(8, 1, 0).ServerSupportsInPlaceUpgrades(new Version(8, 0, 35)));
      Assert.AreEqual(UpgradeViability.Supported, new Version(8, 3, 0).ServerSupportsInPlaceUpgrades(new Version(8, 2, 0)));
      Assert.AreEqual(UpgradeViability.Supported, new Version(8, 4, 0).ServerSupportsInPlaceUpgrades(new Version(8, 3, 0)));
      Assert.AreEqual(UpgradeViability.Supported, new Version(8, 4, 1).ServerSupportsInPlaceUpgrades(new Version(8, 4, 0)));
      Assert.AreEqual(UpgradeViability.Supported, new Version(8, 4, 3).ServerSupportsInPlaceUpgrades(new Version(8, 4, 1)));
      Assert.AreEqual(UpgradeViability.Supported, new Version(9, 0, 0).ServerSupportsInPlaceUpgrades(new Version(8, 4, 1)));
    }

    [TestMethod]
    public void ValidateVersionInfoData()
    {
      var configuratorExeName = "mysql_configurator.exe";
      var configurationType = "Debug";
#if !DEBUG
      configurationType = "Release";
#endif
      var configuratorExePath = $"..\\..\\..\\Configurator\\bin\\{configurationType}\\{configuratorExeName}";
      var configuratorExeFileInfo = new FileInfo(configuratorExePath);
      Assert.IsTrue(configuratorExeFileInfo.Exists);
      
      // Validate version.
      var versionInfo = FileVersionInfo.GetVersionInfo(configuratorExeFileInfo.FullName);
      Assert.IsTrue(!string.IsNullOrEmpty(versionInfo.ProductVersion));
      Assert.AreEqual(versionInfo.ProductVersion, versionInfo.FileVersion);
      Assembly assembly = Assembly.GetExecutingAssembly();
      FileVersionInfo currentFileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
      Assert.AreEqual(currentFileVersionInfo.FileVersion, versionInfo.FileVersion);

      // Validate product name.
      Assert.AreEqual("MySQL Configurator", versionInfo.ProductName);

      // Validate company name.
      Assert.AreEqual("Oracle Corporation", versionInfo.CompanyName);

      // Validate internal name.
      Assert.AreEqual(configuratorExeName, versionInfo.InternalName);

      // Validate field description.
      Assert.AreEqual("The MySQL Configurator is designed to allow the configuration and/or upgrade of the MySQL Server product.", versionInfo.FileDescription);

      // Validate copyright.
      Assert.AreEqual($"Copyright (c) 2023, {DateTime.Now.Year}, Oracle and/or its affiliates.", versionInfo.LegalCopyright);

      // Validate legal trademarks.
      Assert.AreEqual("Oracle®, Java, MySQL, and NetSuite are registered trademarks of Oracle and/or its affiliates.", versionInfo.LegalTrademarks);
    }

    [TestMethod]
    public void ValidateAuthenticationPolicyServerVariableParsing()
    {
      var package = new Package
      {
        VersionString = "8.0.4",
        Publisher = "MySQL AB",
        DisplayName = "MySQL Server",
      };
      package.Version = new Version(package.VersionString);
      var settings = new MySqlServerSettings(package);
      var privateObject = new PrivateObject(settings);
      var methodName = "ParseFirstFactorAuthentication";
      Assert.AreEqual(MySqlAuthenticationPluginType.CachingSha2Password, privateObject.Invoke(methodName, new object[] { null }));
      Assert.AreEqual(MySqlAuthenticationPluginType.CachingSha2Password, privateObject.Invoke(methodName, new object[] { string.Empty }));
      Assert.AreEqual(MySqlAuthenticationPluginType.CachingSha2Password, privateObject.Invoke(methodName, new object[] { "" }));
      Assert.AreEqual(MySqlAuthenticationPluginType.CachingSha2Password, privateObject.Invoke(methodName, new object[] { "caching_sha2_password,," }));
      Assert.AreEqual(MySqlAuthenticationPluginType.CachingSha2Password, privateObject.Invoke(methodName, new object[] { "*,," }));
      Assert.AreEqual(MySqlAuthenticationPluginType.CachingSha2Password, privateObject.Invoke(methodName, new object[] { "* , , " }));
      Assert.AreEqual(MySqlAuthenticationPluginType.CachingSha2Password, privateObject.Invoke(methodName, new object[] { "*:caching_sha2_password,," }));
      Assert.AreEqual(MySqlAuthenticationPluginType.CachingSha2Password, privateObject.Invoke(methodName, new object[] { "*:invalid,," }));
      Assert.AreEqual(MySqlAuthenticationPluginType.MysqlNativePassword, privateObject.Invoke(methodName, new object[] { "*:mysql_native_password,," }));
      Assert.AreEqual(MySqlAuthenticationPluginType.Sha256Password, privateObject.Invoke(methodName, new object[] { "*:sha256_password,," }));
    }

    /// <summary>
    /// Validates that the provided command line options are parsed correctly. 
    /// </summary>
    [TestMethod]
    public void ValidateCommandLineParsing()
    {
      var program = new Program();
      var privateObject = new PrivateObject(program);
      var configuratorExeName = "mysql_configurator.exe";
      var methodName = "ProcessCommandLineArguments";
      var bindingFlags = BindingFlags.NonPublic | BindingFlags.Static;
      Assert.ThrowsException<ArgumentNullException>(() => privateObject.Invoke(methodName, bindingFlags, new object[] { null }));
      Assert.ThrowsException<ConfiguratorException>(() => privateObject.Invoke(methodName, bindingFlags, new object[] { new string[] { configuratorExeName, "configure" } }));
      Assert.ThrowsException<ConfiguratorException>(() => privateObject.Invoke(methodName, bindingFlags, new object[] { new string[] { configuratorExeName, "--upgrade" } }));
      Assert.AreEqual(ExecutionMode.Configure, privateObject.Invoke(methodName, bindingFlags, new object[] { new string[] { configuratorExeName } }));
      Assert.AreEqual(ExecutionMode.Configure, privateObject.Invoke(methodName, bindingFlags, new object[] { new string[] { configuratorExeName, "--configure" } }));
      Assert.AreEqual(ExecutionMode.Configure, privateObject.Invoke(methodName, bindingFlags, new object[] { new string[] { configuratorExeName, "--CONFIGURE" } }));
      Assert.AreEqual(ExecutionMode.Configure, privateObject.Invoke(methodName, bindingFlags, new object[] { new string[] { configuratorExeName, "--CONfigure" } }));
      Assert.ThrowsException<ConfiguratorException>(() => privateObject.Invoke(methodName, bindingFlags, new object[] { new string[] { configuratorExeName, "--configure", "--other" } }));
      Assert.AreEqual(ExecutionMode.Remove, privateObject.Invoke(methodName, bindingFlags, new object[] { new string[] { configuratorExeName, "--remove" } }));
      Assert.AreEqual(ExecutionMode.RemoveNoShow, privateObject.Invoke(methodName, bindingFlags, new object[] { new string[] { configuratorExeName, "--removenoshow" } }));
      try
      {
        privateObject.Invoke(methodName, bindingFlags, new object[] { new string[] { configuratorExeName, "configure" } });
      }
      catch(ConfiguratorException exception)
      {
        Assert.AreEqual(ConfiguratorError.InvalidOptionStart, exception.ErrorCode);
      }

      try
      {
        privateObject.Invoke(methodName, bindingFlags, new object[] { new string[] { configuratorExeName, "--upgrade" } });
      }
      catch (ConfiguratorException exception)
      {
        Assert.AreEqual(ConfiguratorError.InvalidOption, exception.ErrorCode);
      }

      try
      {
        privateObject.Invoke(methodName, bindingFlags, new object[] { new string[] { configuratorExeName, "--configure", "--other" } });
      }
      catch (ConfiguratorException exception)
      {
        Assert.AreEqual(ConfiguratorError.InvalidOption, exception.ErrorCode);
      }

      try
      {
        privateObject.Invoke(methodName, bindingFlags, new object[] { new string[] { configuratorExeName, "--configure,", "--show-remove-warning" } });
      }
      catch (ConfiguratorException exception)
      {
        Assert.AreEqual(ConfiguratorError.InvalidOption, exception.ErrorCode);
      }
    }
  }
}

/* Copyright (c) 2024, 2026, Oracle and/or its affiliates.

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
using System.Security.AccessControl;
using System.Security.Principal;
using MySql.Configurator.Base.Classes;
using MySql.Configurator.Base.Enums;
using MySql.Configurator.Core.Server;
using MySql.Configurator.Core.Settings;

namespace MySQLConfigurator.Test
{
  [TestClass]
  public class MySQLConfiguratorTests
  {
    /// <summary>
    /// Bug#39248552 - Improper handling of directory links during MySQL data directory removal.
    /// Verifies that directory junctions are deleted without traversing their targets.
    /// </summary>
    [TestMethod]
    public void DeleteDirectoryDeletesJunctionWithoutDeletingTarget()
    {
      var testRoot = Path.Combine(Path.GetTempPath(), $"MySQLConfigurator-{Guid.NewGuid():N}");
      var deletionRoot = Path.Combine(testRoot, "delete-root");
      var junctionPath = Path.Combine(deletionRoot, "junction");
      var targetDirectory = Path.Combine(testRoot, "target");
      var targetFile = Path.Combine(targetDirectory, "must-remain.txt");

      try
      {
        Directory.CreateDirectory(deletionRoot);
        Directory.CreateDirectory(targetDirectory);
        File.WriteAllText(targetFile, "This file must not be deleted through the junction.");

        using (var process = Process.Start(new ProcessStartInfo
               {
                 FileName = Path.Combine(Environment.SystemDirectory, "cmd.exe"),
                 Arguments = $"/c mklink /J \"{junctionPath}\" \"{targetDirectory}\"",
                 CreateNoWindow = true,
                 UseShellExecute = false
               }))
        {
          process.WaitForExit();
          Assert.AreEqual(0, process.ExitCode, "Failed to create the test junction.");
        }

        Assert.IsTrue(Utilities.DeleteDirectory(deletionRoot));
        Assert.IsFalse(Directory.Exists(junctionPath));
        Assert.IsTrue(File.Exists(targetFile));
      }
      finally
      {
        if (Directory.Exists(testRoot))
        {
          Directory.Delete(testRoot, true);
        }
      }
    }

    /// <summary>
    /// Bug#39248552 - Improper handling of directory links during MySQL data directory removal.
    /// Verifies that a directory junction used as the deletion root is removed without traversing its target.
    /// </summary>
    [TestMethod]
    public void DeleteDirectoryDeletesJunctionRootWithoutDeletingTarget()
    {
      var testRoot = Path.Combine(Path.GetTempPath(), $"MySQLConfigurator-{Guid.NewGuid():N}");
      var junctionPath = Path.Combine(testRoot, "delete-root");
      var targetDirectory = Path.Combine(testRoot, "target");
      var targetFile = Path.Combine(targetDirectory, "must-remain.txt");

      try
      {
        Directory.CreateDirectory(testRoot);
        Directory.CreateDirectory(targetDirectory);
        File.WriteAllText(targetFile, "This file must not be deleted through the root junction.");

        using (var process = Process.Start(new ProcessStartInfo
               {
                 FileName = Path.Combine(Environment.SystemDirectory, "cmd.exe"),
                 Arguments = $"/c mklink /J \"{junctionPath}\" \"{targetDirectory}\"",
                 CreateNoWindow = true,
                 UseShellExecute = false
               }))
        {
          process.WaitForExit();
          Assert.AreEqual(0, process.ExitCode, "Failed to create the test junction.");
        }

        var targetPermissionsInherited = DirectoryServicesWrapper.DirectoryPermissionsAreInherited(targetDirectory);
        var usersGroupSid = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);
        Assert.IsFalse(DirectoryServicesWrapper.ConvertInheritedPermissionsToExplicitPermissions(junctionPath, false));
        Assert.IsFalse(DirectoryServicesWrapper.GrantPermissionsToDirectory(junctionPath, Environment.UserName, FileSystemRights.FullControl, AccessControlType.Allow, false));
        Assert.IsFalse(DirectoryServicesWrapper.RemoveGroupPermissions(junctionPath, usersGroupSid, false));
        Assert.AreEqual(targetPermissionsInherited, DirectoryServicesWrapper.DirectoryPermissionsAreInherited(targetDirectory));

        var serverInstallation = new ServerInstallation(LicenseType.Community);
        serverInstallation.Initialize("8.0.40", null);
        var controller = new PrivateObject(serverInstallation.Controller);
        Assert.IsFalse((bool)controller.Invoke("ConfigurationDirectoryHasRecommendedPermissions", junctionPath, null));
        Assert.IsFalse((bool)controller.Invoke("SecureConfigurationDirectory", junctionPath, null));

        Assert.IsTrue(Utilities.DeleteDirectory(junctionPath));
        Assert.IsFalse(Directory.Exists(junctionPath));
        Assert.IsTrue(File.Exists(targetFile));
      }
      finally
      {
        if (Directory.Exists(testRoot))
        {
          Directory.Delete(testRoot, true);
        }
      }
    }

    /// <summary>
    /// Bug#39248552 - Improper handling of directory links during MySQL data directory removal.
    /// Verifies that manual server-file permission management does not schedule an ACL update.
    /// </summary>
    [TestMethod]
    public void ManualServerFilePermissionsDoesNotScheduleUpdate()
    {
      var serverInstallation = new ServerInstallation(LicenseType.Community);
      serverInstallation.Initialize("8.0.40", null);
      var controller = serverInstallation.Controller;
      controller.ConfigurationType = ConfigurationType.Configure;
      controller.UpdateDataDirectoryPermissions = true;
      controller.Settings.ServerFilePermissionsAccess = ServerFilePermissionsAccess.Manual;

      Assert.IsFalse(controller.IsUpdateServerFilesPermissionsStepNeeded);
    }

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
      Assert.AreEqual(UpgradeViability.UnsupportedWithWarning, new Version(8, 5, 0).ServerSupportsInPlaceUpgrades(new Version(8, 3, 0)));
      Assert.AreEqual(UpgradeViability.UnsupportedWithWarning, new Version(9, 0, 0).ServerSupportsInPlaceUpgrades(new Version(8, 3, 0)));

      // Supported scenarios.
      Assert.AreEqual(UpgradeViability.Supported, new Version(8, 2, 0).ServerSupportsInPlaceUpgrades(new Version(8, 0, 35)));
      Assert.AreEqual(UpgradeViability.Supported, new Version(8, 3, 1).ServerSupportsInPlaceUpgrades(new Version(8, 3, 0)));
      Assert.AreEqual(UpgradeViability.Supported, new Version(8, 4, 0).ServerSupportsInPlaceUpgrades(new Version(8, 0, 35)));
      Assert.AreEqual(UpgradeViability.Supported, new Version(8, 1, 0).ServerSupportsInPlaceUpgrades(new Version(8, 0, 35)));
      Assert.AreEqual(UpgradeViability.Supported, new Version(8, 1, 0).ServerSupportsInPlaceUpgrades(new Version(8, 0, 35)));
      Assert.AreEqual(UpgradeViability.Supported, new Version(8, 3, 0).ServerSupportsInPlaceUpgrades(new Version(8, 2, 0)));
      Assert.AreEqual(UpgradeViability.Supported, new Version(8, 4, 0).ServerSupportsInPlaceUpgrades(new Version(8, 2, 0)));
      Assert.AreEqual(UpgradeViability.Supported, new Version(8, 4, 0).ServerSupportsInPlaceUpgrades(new Version(8, 3, 0)));
      Assert.AreEqual(UpgradeViability.Supported, new Version(8, 4, 1).ServerSupportsInPlaceUpgrades(new Version(8, 4, 0)));
      Assert.AreEqual(UpgradeViability.Supported, new Version(8, 4, 3).ServerSupportsInPlaceUpgrades(new Version(8, 4, 1)));
      Assert.AreEqual(UpgradeViability.Supported, new Version(9, 0, 0).ServerSupportsInPlaceUpgrades(new Version(8, 4, 1)));
      Assert.AreEqual(UpgradeViability.Supported, new Version(9, 1, 0).ServerSupportsInPlaceUpgrades(new Version(8, 4, 1)));
      Assert.AreEqual(UpgradeViability.Supported, new Version(26, 7, 0).ServerSupportsInPlaceUpgrades(new Version(9, 7, 5)));
      Assert.AreEqual(UpgradeViability.Supported, new Version(26, 10, 0).ServerSupportsInPlaceUpgrades(new Version(9, 7, 5)));
      Assert.AreEqual(UpgradeViability.Supported, new Version(27, 1, 0).ServerSupportsInPlaceUpgrades(new Version(26, 10, 0)));
      Assert.AreEqual(UpgradeViability.Supported, new Version(28, 1, 2).ServerSupportsInPlaceUpgrades(new Version(26, 7, 0)));
      Assert.AreEqual(UpgradeViability.Supported, new Version(28, 4, 0).ServerSupportsInPlaceUpgrades(new Version(26, 7, 0)));
      Assert.AreEqual(UpgradeViability.Supported, new Version(28, 4, 0).ServerSupportsInPlaceUpgrades(new Version(9, 7, 5)));
      Assert.AreEqual(UpgradeViability.Supported, new Version(28, 7, 0).ServerSupportsInPlaceUpgrades(new Version(28, 4, 0)));
      Assert.AreEqual(UpgradeViability.Supported, new Version(30, 4, 0).ServerSupportsInPlaceUpgrades(new Version(28, 7, 0)));
      Assert.AreEqual(UpgradeViability.Supported, new Version(30, 2, 0).ServerSupportsInPlaceUpgrades(new Version(30, 1, 0)));
      Assert.AreEqual(UpgradeViability.Supported, new Version(30, 4, 0).ServerSupportsInPlaceUpgrades(new Version(30, 1, 0)));
    }

    [TestMethod]
    public void ValidateServerMaturity()
    {
      Assert.AreEqual(ServerMaturity.Older, Utilities.GetServerMaturity(new Version(5, 7, 0)));
      Assert.AreEqual(ServerMaturity.LTS, Utilities.GetServerMaturity(new Version(8, 4, 15)));
      Assert.AreEqual(ServerMaturity.LTS, Utilities.GetServerMaturity(new Version(9, 7, 5)));
      Assert.AreEqual(ServerMaturity.Innovation, Utilities.GetServerMaturity(new Version(10, 7, 0)));
      Assert.AreEqual(ServerMaturity.Innovation, Utilities.GetServerMaturity(new Version(11, 7, 0)));
      Assert.AreEqual(ServerMaturity.Innovation, Utilities.GetServerMaturity(new Version(26, 7, 0)));
      Assert.AreEqual(ServerMaturity.LTS, Utilities.GetServerMaturity(new Version(28, 4, 0)));
      Assert.AreEqual(ServerMaturity.Innovation, Utilities.GetServerMaturity(new Version(28, 7, 0)));
      Assert.AreEqual(ServerMaturity.LTS, Utilities.GetServerMaturity(new Version(30, 4, 0)));
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
      var serverInstallation = new ServerInstallation(AppConfiguration.License);
      serverInstallation.Initialize("8.0.4", null);
      var settings = new MySqlServerSettings(serverInstallation);
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
  }
}

/* Copyright (c) 2024, Oracle and/or its affiliates.

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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace MySQLConfigurator.Test
{
  [TestClass]
  public class MySQLConfiguratorTests
  {
    [TestMethod]
    public void ValidateVersionInfoData()
    {
      var configuratorExeName = "mysql_configurator.exe";
      var configurationType = "Debug";
#if !DEBUG
      var configurationType = "Release";
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
  }
}

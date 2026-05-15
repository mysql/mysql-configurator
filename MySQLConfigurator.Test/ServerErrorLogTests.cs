/* Copyright (c) 2026, Oracle and/or its affiliates.

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
using MySql.Configurator.Base.Enums;
using MySql.Configurator.Core.Server;
using System;
using System.Collections.Generic;
using System.IO;

namespace MySQLConfigurator.Test
{
  [TestClass]
  public class ServerErrorLogTests
  {
    /// <summary>
    /// Verifies that the temporary root password is parsed from a MySQL Server initialization log line.
    /// </summary>
    [TestMethod]
    public void TryGetTemporaryRootPasswordParsesGeneratedPassword()
    {
      var logLines = new List<ServerErrorLogLine>
      {
        ServerErrorLogLine.Parse("2026-05-08T18:01:01.123456Z 6 [Note] [MY-010454] [Server] A temporary password is generated for root@localhost: aB1#xyZ,.- ")
      };

      Assert.IsTrue(ServerErrorLog.TryGetTemporaryRootPassword(logLines, out var password));
      Assert.AreEqual("aB1#xyZ,.-", password);
    }

    /// <summary>
    /// Verifies that temporary root passwords containing spaces are preserved when parsed.
    /// </summary>
    [TestMethod]
    public void TryGetTemporaryRootPasswordHandlesPasswordWithSpaces()
    {
      var logLines = new List<ServerErrorLogLine>
      {
        ServerErrorLogLine.Parse("2026-05-08T18:01:01.123456Z 6 [Note] [MY-010454] [Server] A temporary password is generated for root@localhost: ab C!1")
      };

      Assert.IsTrue(ServerErrorLog.TryGetTemporaryRootPassword(logLines, out var password));
      Assert.AreEqual("ab C!1", password);
    }

    /// <summary>
    /// Verifies that the latest temporary root password is returned when the log contains multiple generated passwords.
    /// </summary>
    [TestMethod]
    public void TryGetTemporaryRootPasswordReturnsLatestGeneratedPassword()
    {
      var logLines = new List<ServerErrorLogLine>
      {
        ServerErrorLogLine.Parse("2026-05-08T18:01:01.123456Z 6 [Note] [MY-010454] [Server] A temporary password is generated for root@localhost: firstPwd1!"),
        ServerErrorLogLine.Parse("2026-05-08T18:01:02.123456Z 6 [Note] [MY-010454] [Server] A temporary password is generated for root@localhost: secondPwd2!")
      };

      Assert.IsTrue(ServerErrorLog.TryGetTemporaryRootPassword(logLines, out var password));
      Assert.AreEqual("secondPwd2!", password);
    }

    /// <summary>
    /// Verifies that parsing fails without returning a password when no generated temporary root password exists in the log.
    /// </summary>
    [TestMethod]
    public void TryGetTemporaryRootPasswordReturnsFalseWhenPasswordIsMissing()
    {
      var logLines = new List<ServerErrorLogLine>
      {
        ServerErrorLogLine.Parse("2026-05-08T18:01:01.123456Z 6 [Note] [MY-010454] [Server] mysqld.exe: ready for connections.")
      };

      Assert.IsFalse(ServerErrorLog.TryGetTemporaryRootPassword(logLines, out var password));
      Assert.IsNull(password);
    }

    /// <summary>
    /// Verifies that generated temporary root passwords are masked before log lines are reported.
    /// </summary>
    [TestMethod]
    public void RedactTemporaryRootPasswordMasksGeneratedPassword()
    {
      var redacted = ServerErrorLog.RedactTemporaryRootPassword("A temporary password is generated for root@localhost: Secret1!");

      Assert.AreEqual("A temporary password is generated for root@localhost: ****", redacted);
    }

    /// <summary>
    /// Verifies that temporary root passwords read from the error log file are masked before being reported.
    /// </summary>
    [TestMethod]
    public void ReadNewLinesFromFileRedactsTemporaryRootPasswordBeforeReporting()
    {
      var tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
      var dataDirectory = Path.Combine(tempDirectory, "Data");
      var errorLogFileName = "mysql.err";
      Directory.CreateDirectory(dataDirectory);
      try
      {
        var serverInstallation = new ServerInstallation(LicenseType.Community);
        serverInstallation.Initialize("8.0.40", null);
        var controller = serverInstallation.Controller;
        controller.Settings.DataDirectory = tempDirectory;
        controller.Settings.ErrorLogFileName = errorLogFileName;
        var serverErrorLog = new ServerErrorLog(controller);
        var reportedLines = new List<string>();
        serverErrorLog.ReportStatusDelegate = reportedLines.Add;

        File.WriteAllText(
          Path.Combine(dataDirectory, errorLogFileName),
          "2026-05-08T18:01:01.123456Z 6 [Note] [MY-010454] [Server] A temporary password is generated for root@localhost: Secret1!" + Environment.NewLine);

        serverErrorLog.ReadNewLinesFromFile(true);

        Assert.AreEqual(1, reportedLines.Count);
        Assert.IsFalse(reportedLines[0].Contains("Secret1!"));
        StringAssert.Contains(reportedLines[0], "****");
        Assert.IsTrue(ServerErrorLog.TryGetTemporaryRootPassword(serverErrorLog.LogLines, out var password));
        Assert.AreEqual("Secret1!", password);
      }
      finally
      {
        if (Directory.Exists(tempDirectory))
        {
          Directory.Delete(tempDirectory, true);
        }
      }
    }

    /// <summary>
    /// Verifies that the controller stores the generated temporary root password from initialization console output.
    /// </summary>
    [TestMethod]
    public void ReportErrLogLineStoresTemporaryRootPasswordFromConsoleOutput()
    {
      var serverInstallation = new ServerInstallation(LicenseType.Community);
      serverInstallation.Initialize("8.0.40", null);
      var controller = serverInstallation.Controller;
      controller.ConfigurationType = ConfigurationType.Configure;
      controller.Settings.DefaultAuthenticationPlugin = MySqlAuthenticationPluginType.CachingSha2Password;
      var privateObject = new PrivateObject(controller);

      privateObject.Invoke("ReportErrLogLine", "A temporary password is generated for root@localhost: temporaryRoot1!");
      var user = privateObject.Invoke("GetUserAccountToConnectBeforeUpdatingRootUser") as MySqlServerUser;

      Assert.IsNotNull(user);
      Assert.AreEqual(MySqlServerUser.ROOT_USERNAME, user.Username);
      Assert.AreEqual(MySqlServerUser.LOCALHOST, user.Host);
      Assert.AreEqual("temporaryRoot1!", user.Password);
    }

    /// <summary>
    /// Verifies that the controller uses the generated temporary root password before updating the root account.
    /// </summary>
    [TestMethod]
    public void GetUserAccountToConnectBeforeUpdatingRootUserUsesTemporaryRootPassword()
    {
      var serverInstallation = new ServerInstallation(LicenseType.Community);
      serverInstallation.Initialize("8.0.40", null);
      var controller = serverInstallation.Controller;
      controller.ConfigurationType = ConfigurationType.Configure;
      controller.Settings.DefaultAuthenticationPlugin = MySqlAuthenticationPluginType.CachingSha2Password;
      var privateObject = new PrivateObject(controller);
      privateObject.SetField("_temporaryRootPassword", "temporaryRoot1!");

      var user = privateObject.Invoke("GetUserAccountToConnectBeforeUpdatingRootUser") as MySqlServerUser;

      Assert.IsNotNull(user);
      Assert.AreEqual(MySqlServerUser.ROOT_USERNAME, user.Username);
      Assert.AreEqual(MySqlServerUser.LOCALHOST, user.Host);
      Assert.AreEqual("temporaryRoot1!", user.Password);
    }
  }
}

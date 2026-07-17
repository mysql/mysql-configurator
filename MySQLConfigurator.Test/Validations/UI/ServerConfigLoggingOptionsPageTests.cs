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
using MySql.Configurator.Properties;
using MySql.Configurator.UI.Wizards.ServerConfigPages;
using System.Reflection;

namespace MySQLConfigurator.Test.Validations.UI
{
  [TestClass]
  public class ValidateSecondsNumberTests
  {
    private ServerConfigLoggingOptionsPage _sut;

    [TestInitialize]
    public void Setup()
    {
      _sut = new ServerConfigLoggingOptionsPage(null);
    }

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow("\t")]
    [DataRow("abc")]
    [DataRow("12.3")]
    [DataRow("1,000")]
    [DataRow("0x10")]
    public void ValidateSecondsNumber_Returns_NotProperValueForInt_WhenInputIsNotALong(string seconds)
    {
      var expected = string.Format(Resources.NotProperValueForInt, seconds);

      var actual = InvokeValidateSecondsNumber(seconds);

      Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DataRow("2147483648")]
    [DataRow("-2147483649")]
    [DataRow("9223372036854775807")]
    [DataRow("-9223372036854775808")]
    public void ValidateSecondsNumber_Returns_OutOfRangeValueForInt_WhenParsedLongIsOutsideIntRange(string seconds)
    {
      var expected = string.Format(Resources.OutOfRangeValueForInt, seconds);

      var actual = InvokeValidateSecondsNumber(seconds);

      Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DataRow("-1")]
    [DataRow("-10")]
    [DataRow("-2147483648")]
    public void ValidateSecondsNumber_Returns_MinValueRequired_WhenValueIsNegativeButWithinIntRange(string seconds)
    {
      var expected = string.Format(Resources.MinValueRequired, "0");

      var actual = InvokeValidateSecondsNumber(seconds);

      Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DataRow("0")]
    [DataRow("1")]
    [DataRow("10")]
    [DataRow("2147483647")]
    public void ValidateSecondsNumber_Returns_EmptyString_WhenValueIsValidNonNegativeInt(string seconds)
    {
      var actual = InvokeValidateSecondsNumber(seconds);

      Assert.AreEqual(string.Empty, actual);
    }

    [TestMethod]
    public void ValidateSecondsNumber_Returns_NotProperValueForInt_WhenValueExceedsLongRange()
    {
      var seconds = "92233720368547758070";
      var expected = string.Format(Resources.NotProperValueForInt, seconds);

      var actual = InvokeValidateSecondsNumber(seconds);

      Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DataRow(" 0 ")]
    [DataRow("\t123\n")]
    public void ValidateSecondsNumber_Returns_EmptyString_WhenWhitespaceWrappedValidNumber(string seconds)
    {
      var actual = InvokeValidateSecondsNumber(seconds);

      Assert.AreEqual(string.Empty, actual);
    }

    private string InvokeValidateSecondsNumber(string seconds)
    {
      var method = typeof(ServerConfigLoggingOptionsPage).GetMethod(
          "ValidateSecondsNumber",
          BindingFlags.Instance | BindingFlags.NonPublic);

      Assert.IsNotNull(method, "ValidateSecondsNumber method not found.");

      return (string)method.Invoke(_sut, new object[] { seconds });
    }
  }
}


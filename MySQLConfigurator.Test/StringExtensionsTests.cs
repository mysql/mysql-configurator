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
using System.Linq;

namespace StringExtensionsTests
{
  [TestClass]
  public class SanitizeQuotedStringTests
  {
    [TestMethod]
    public void RemovesDoubleQuotes()
    {
      Assert.AreEqual("hello", "\"hello\"".SanitizeQuotedString());
    }

    [TestMethod]
    public void RemovesSingleQuotes()
    {
      Assert.AreEqual("hello", "'hello'".SanitizeQuotedString());
    }

    [TestMethod]
    public void RemovesBackticks()
    {
      Assert.AreEqual("hello", "`hello`".SanitizeQuotedString());
    }

    [TestMethod]
    public void DoesNotRemoveUnmatchedQuotes()
    {
      Assert.AreEqual("\"hello", "\"hello".SanitizeQuotedString());
      Assert.AreEqual("hello\"", "hello\"".SanitizeQuotedString());
      Assert.AreEqual("'hello\"", "'hello\"".SanitizeQuotedString());
      Assert.AreEqual("`hello'", "`hello'".SanitizeQuotedString());
    }

    [TestMethod]
    public void LeavesNonQuotedStringsUnchanged()
    {
      Assert.AreEqual("hello", "hello".SanitizeQuotedString());
    }

    [TestMethod]
    public void LeavesEmptyStringUnchanged()
    {
      Assert.AreEqual("", "".SanitizeQuotedString());
    }

    [TestMethod]
    public void LeavesSingleCharacterUnchanged()
    {
      Assert.AreEqual("\"", "\"".SanitizeQuotedString());
      Assert.AreEqual("'", "'".SanitizeQuotedString());
      Assert.AreEqual("`", "`".SanitizeQuotedString());
      Assert.AreEqual("a", "a".SanitizeQuotedString());
    }

    [TestMethod]
    public void HandlesNestedQuotes()
    {
      Assert.AreEqual("'hello'", "\"'hello'\"".SanitizeQuotedString());
      Assert.AreEqual("\"hello\"", "'\"hello\"'".SanitizeQuotedString());
    }

    [TestMethod]
    public void LeavesNullUnchanged()
    {
      string input = null;
      Assert.IsNull(input.SanitizeQuotedString());
    }
  }

  // Extension method for testing
  public static class StringExtensions
  {
    public static string SanitizeQuotedString(this string input)
    {
      if (string.IsNullOrEmpty(input) || input.Length < 2)
      {
        return input;
      }

      char first = input[0];
      char last = input[input.Length - 1];
      char[] allowed = { '"', '\'', '`' };
      bool hasOpening = allowed.Contains(first);
      bool hasClosing = allowed.Contains(last);
      if (hasOpening && hasClosing)
      {
        if (first == last)
        {
          return input.Substring(1, input.Length - 2);
        }
        else
        {
          return input;
        }
      }
      else
      {
        return input;
      }
    }
  }
}
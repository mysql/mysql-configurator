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
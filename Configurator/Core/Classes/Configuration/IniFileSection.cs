// Copyright (c) 2023, Oracle and/or its affiliates.
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License as
// published by the Free Software Foundation; version 2 of the
// License.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
// 02110-1301  USA

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySql.Configurator.Core.Classes.Configuration
{
  public class IniFileSection
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="IniFileSection"/> class.
    /// </summary>
    /// <param name="name">The name of a section in the configuration file.</param>
    /// <param name="filePath">The physical path for the ini file.</param>
    internal IniFileSection(string name, string filePath)
    {
      FilePath = filePath;
      Name = name;
      LoadKeys();
    }

    #region Properties

    /// <summary>
    /// Gets a dictionary of keys and values contained within this section.
    /// </summary>
    public Dictionary<string, string> Keys { get; private set; }

    /// <summary>
    /// Gets the path where the ini file is saved into disk.
    /// </summary>
    public string FilePath { get; }

    /// <summary>
    /// Gets the name of a section in the configuration file.
    /// </summary>
    public string Name { get; }

    #endregion Properties

    /// <summary>
    /// Gets a key's value from the given section string.
    /// </summary>
    /// <param name="section">A section string.</param>
    /// <param name="key">The key or property name.</param>
    /// <returns>A key's value.</returns>
    private string GetValue(string section, string key)
    {
      if (string.IsNullOrEmpty(FilePath))
      {
        return null;
      }

      // Sets the maxsize buffer to 250, if the more is required then doubles the size each time. 
      for (var maxsize = 250; ; maxsize *= 2)
      {
        // Obtains the value information and uses a StringBuilder to and stores them in the maxsize buffers (result).
        var result = new StringBuilder(maxsize);
        var size = Utilities.GetPrivateProfileString(section, key, string.Empty, result, maxsize, FilePath);
        if (size < maxsize - 1)
        {
          // Returns the value gathered from the EntryKey
          return result.ToString();
        }
      }
    }

    /// <summary>
    /// Loads the keys from the section string.
    /// </summary>
    private void LoadKeys()
    {
      if (string.IsNullOrEmpty(Name))
      {
        return;
      }

      // Sets the maxsize buffer to 500, if the more is required then doubles the size each time. 
      for (var maxsize = 500; ; maxsize *= 2)
      {
        // Obtains the EntryKey information in bytes and stores them in the maxsize buffer (Bytes array).
        var bytes = new byte[maxsize];
        var size = Utilities.GetPrivateProfileString(Name, 0, string.Empty, bytes, maxsize, FilePath);

        // Check the information obtained is not bigger than the allocated maxsize buffer - 2 bytes.
        // If it is, then skip over the next section so that the maxsize buffer can be doubled.
        if (size >= maxsize - 2)
        {
          continue;
        }

        // Converts the bytes value into an ASCII char. This is one long string.
        var entries = Encoding.ASCII.GetString(bytes, 0, size - (size > 0 ? 1 : 0));

        // Splits the Long string into an array based on the "\0" or null (new line) value and returns the value(s) in an array
        Keys = entries.Split(new[] { '\0' }).Distinct().ToDictionary(key => key, key => GetValue(Name, key));
        break;
      }
    }
  }
}
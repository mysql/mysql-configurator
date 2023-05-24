/* Copyright (c) 2023, Oracle and/or its affiliates.

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

using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MySql.Configurator.Core.IniFile
{
  public class IniFileEngine
  {
    [DllImport("kernel32")]
    static extern int GetPrivateProfileString(string section, string key, string value, StringBuilder result, int size, string fileName);

    [DllImport("kernel32")]
    static extern int GetPrivateProfileString(string section, int key, string value, [MarshalAs(UnmanagedType.LPArray)] byte[] result, int size, string fileName);

    [DllImport("kernel32")]
    static extern int GetPrivateProfileString(int section, string key, string value, [MarshalAs(UnmanagedType.LPArray)] byte[] result, int size, string fileName);

    private string _path;

    public IniFileEngine(string iniPath)
    {
      _path = iniPath;
    }

    public IniFile Load()
    {
      var ini = new IniFile(_path)
      {
        Sections = GetSections().Select(s => new IniSection {Section = s, Keys = GetKeys(s).Distinct().ToDictionary(k => k, k => GetValue(s, k))}).ToList()
      };

      return ini;
    }

    protected List<string> GetSections()
    {
      //    Sets the maxsize buffer to 500, if the more
      //    is required then doubles the size each time.
      for (var maxsize = 500; ; maxsize *= 2)
      {
        //    Obtains the information in bytes and stores
        //    them in the maxsize buffer (Bytes array)
        var bytes = new byte[maxsize];
        var size = GetPrivateProfileString(0, "", "", bytes, maxsize, _path);

        // Check the information obtained is not bigger
        // than the allocated maxsize buffer - 2 bytes.
        // if it is, then skip over the next section
        // so that the maxsize buffer can be doubled.
        if (size >= maxsize - 2) continue;
        // Converts the bytes value into an ASCII char. This is one long string.
        var selected = Encoding.ASCII.GetString(bytes, 0, size - (size > 0 ? 1 : 0));
        // Splits the Long string into an array based on the "\0"
        // or null (Newline) value and returns the value(s) in an array
        return selected.Split(new[] { '\0' }).ToList();
      }
    }
    // The Function called to obtain the EntryKey's from the given
    // SectionHeader string passed and returns them in an Dynamic Array
    protected List<string> GetKeys(string section)
    {
      //    Sets the maxsize buffer to 500, if the more
      //    is required then doubles the size each time. 
      for (var maxsize = 500; ; maxsize *= 2)
      {
        //    Obtains the EntryKey information in bytes
        //    and stores them in the maxsize buffer (Bytes array).
        //    Note that the SectionHeader value has been passed.
        var bytes = new byte[maxsize];
        var size = GetPrivateProfileString(section, 0, "", bytes, maxsize, _path);

        // Check the information obtained is not bigger
        // than the allocated maxsize buffer - 2 bytes.
        // if it is, then skip over the next section
        // so that the maxsize buffer can be doubled.
        if (size >= maxsize - 2) continue;
        // Converts the bytes value into an ASCII char.
        // This is one long string.
        var entries = Encoding.ASCII.GetString(bytes, 0, size - (size > 0 ? 1 : 0));
        // Splits the Long string into an array based on the "\0"
        // or null (Newline) value and returns the value(s) in an array
        return entries.Split(new[] { '\0' }).ToList();
      }
    }

    // The Function called to obtain the EntryKey Value from
    // the given SectionHeader and EntryKey string passed, then returned
    protected string GetValue(string section, string entry)
    {
      //    Sets the maxsize buffer to 250, if the more
      //    is required then doubles the size each time. 
      for (var maxsize = 250; ; maxsize *= 2)
      {
        //    Obtains the EntryValue information and uses the StringBuilder
        //    Function to and stores them in the maxsize buffers (result).
        //    Note that the SectionHeader and EntryKey values has been passed.
        var result = new StringBuilder(maxsize);
        var size = GetPrivateProfileString(section, entry, "", result, maxsize, _path);
        if (size < maxsize - 1)
        {
          // Returns the value gathered from the EntryKey
          return result.ToString();
        }
      }
    }
  }
}

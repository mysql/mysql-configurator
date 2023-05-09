// Copyright (c) 2019, Oracle and/or its affiliates. All rights reserved.
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
using System.IO;
using System.Linq;
using System.Text;

namespace MySql.Configurator.Core.Classes.Configuration
{
  public class IniFile
  {
    #region Fields

    /// <summary>
    /// Stores the list of strings that compose the ini file for post processing and classification.
    /// </summary>
    private List<string> _allLinesText;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="IniFile"/> class. If provided with a path, the file gets pre-loaded into memory.
    /// </summary>
    /// <param name="filePath">The physical path for the ini file.</param>
    public IniFile(string filePath = null)
    {
      Lines = new List<IniFileLine>();
      Sections = new List<IniFileSection>();
      if (string.IsNullOrEmpty(filePath)
          || !File.Exists(filePath))
      {
        return;
      }

      FilePath = filePath;
      ParseAllTextLines();
    }

    #region Properties

    /// <summary>
    /// Gets the path where the ini file is saved into disk.
    /// </summary>
    public string FilePath { get; }

    /// <summary>
    /// Gets or sets the list of lines with classification attributes that compose the ini file.
    /// </summary>
    public List<IniFileLine> Lines { get; }

    /// <summary>
    /// Gets or sets the list of sections.
    /// </summary>
    public List<IniFileSection> Sections { get; }

    #endregion Properties

    /// <summary>
    /// Gets a <see cref="IniFile"/> from a given file path.
    /// </summary>
    /// <param name="filePath">The physical path for the ini file.</param>
    /// <param name="loadSections">Flag indicating whether sections are loaded.</param>
    /// <returns>An <see cref="IniFile"/> instance.</returns>
    public static IniFile FromFilePath(string filePath, bool loadSections)
    {
      var iniFile = new IniFile(filePath);
      if (loadSections)
      {
        iniFile.LoadSections();
      }

      return iniFile;
    }

    /// <summary>
    /// Gets the last index for section.
    /// </summary>
    /// <param name="section">The section.</param>
    /// <returns>The index of the last line belonging to a certain section within the file.</returns>
    public int GetLastIndexForSection(string section = null)
    {
      return Lines.FindLastIndex(l => l.Section == section) + 1;
    }

    /// <summary>
    /// Gets the name of the last section used within the file.
    /// </summary>
    /// <returns>The name of the last section used within the file.</returns>
    public string GetLastSection()
    {
      return Lines.FindLast(l => !string.IsNullOrEmpty(l.Section)).Section;
    }

    /// <summary>
    /// Loads the configuration file sections.
    /// </summary>
    public void LoadSections()
    {
      if (string.IsNullOrEmpty(FilePath))
      {
        return;
      }

      Sections.Clear();
      List<string> sections;
      // Sets the maxsize buffer to 500, if the more is required then doubles the size each time.
      for (var maxsize = 500; ; maxsize *= 2)
      {
        // Obtains the information in bytes and stores them in the maxsize buffer (bytes array)
        var bytes = new byte[maxsize];
        var size = Utilities.GetPrivateProfileString(0, string.Empty, string.Empty, bytes, maxsize, FilePath);

        // Check the information obtained is not bigger than the allocated maxsize buffer - 2 bytes.
        // If it is, then skip over the next section so that the maxsize buffer can be doubled.
        if (size >= maxsize - 2)
        {
          continue;
        }

        // Converts the bytes value into an ASCII char. This is one long string.
        var selected = Encoding.ASCII.GetString(bytes, 0, size - (size > 0 ? 1 : 0));

        // Splits the Long string into an array based on the "\0" or null (new line) value and returns the value(s) in a list
        sections = selected.Split(new[] { '\0' }).ToList();
        break;
      }

      Sections.AddRange(sections.Select(section => new IniFileSection(section, FilePath)));
    }

    /// <summary>
    /// Checks if certain section exists in the ini file.
    /// </summary>
    /// <param name="sectionName">Name of the section.</param>
    /// <returns>True if the section exists within the file.</returns>
    public bool SectionExists(string sectionName)
    {
      var section = Lines.FirstOrDefault(s => s.Section == sectionName);
      return section != null;
    }

    /// <summary>
    /// Reads the file, loads all lines that compose it into memory and fully classify them.
    /// </summary>
    private void ParseAllTextLines()
    {
      Lines.Clear();
      _allLinesText = File.ReadAllLines(FilePath).ToList();
      var currentSection = string.Empty;
      foreach (var line in _allLinesText)
      {
        var isCommented = line.StartsWith("#");
        var lineTrimmed = isCommented ? line.Substring(1).TrimStart() : line;

        // Empty line
        if (string.IsNullOrEmpty(lineTrimmed))
        {
          Lines.Add(new IniFileLine(IniFileLineType.EmptyLine, isCommented));
          continue;
        }

        // Section
        if (lineTrimmed.StartsWith("[") && lineTrimmed.EndsWith("]"))
        {
          currentSection = lineTrimmed.Substring(1, lineTrimmed.Length - 2);
          Lines.Add(new IniFileLine(IniFileLineType.Section, isCommented, currentSection));
          continue;
        }

        // Key-Value pairs
        if (!string.IsNullOrEmpty(currentSection) && !lineTrimmed.Contains(' '))
        {
          var keyPair = lineTrimmed.Split(new[] { '=' }, 2);

          // Search for keys with value
          if (keyPair.Length == 2)
          {
            Lines.Add(new IniFileLine(IniFileLineType.KeyValuePair, isCommented, currentSection, keyPair[0], keyPair[1]));
            continue;
          }

          // Key is not has no value (e.g. no-beep)
          if (lineTrimmed.Any(char.IsLetterOrDigit))
          {
            Lines.Add(new IniFileLine(IniFileLineType.KeyValuePair, isCommented, currentSection, keyPair[0]));
            continue;
          }
        }

        // Comment or separator.
        Lines.Add(new IniFileLine(IniFileLineType.Comment, isCommented, currentSection, lineTrimmed));
      }
    }
  }
}
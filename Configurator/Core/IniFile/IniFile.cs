/* Copyright (c) 2014, 2021, Oracle and/or its affiliates.

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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MySql.Configurator.Core.Enums;

namespace MySql.Configurator.Core.IniFile
{
  public class IniFile
  {
    #region Constants

    public const string CLIENT_SECTION = "[client]";
    public const string PASSWORD_LINE = "password={0}";

    #endregion Constants

    #region Fields

    /// <summary>
    /// Stores the list of strings that compose the ini file for post processing and classification.
    /// </summary>
    private List<string> _allLinesText;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="IniFile"/> class. If provided with a path, the file gets pre-loaded into memory.
    /// </summary>
    /// <param name="iniPath">The physical path for the ini file.</param>
    public IniFile(string iniPath = null)
    {
      Lines = new List<IniLineFullDetail>();
      if (string.IsNullOrEmpty(iniPath) || !File.Exists(iniPath))
        return;

      Path = iniPath;
      ParseAllTextLines();
    }

    #region Properties

    /// <summary>
    /// Gets or sets the list of lines with classification attributes that compose the ini file.
    /// </summary>
    public List<IniLineFullDetail> Lines { get; set; }

    /// <summary>
    /// Gets the path where the ini file is saved into disk.
    /// </summary>
    public string Path { get; }

    /// <summary>
    /// Gets or sets the list of sections.
    /// </summary>
    public List<IniSection> Sections { get; set; }

    #endregion Properties

    /// <summary>
    /// Returns the configuration file lines related to the client section password.
    /// </summary>
    /// <param name="password">The password value.</param>
    /// <returns>The configuration file lines related to the client section password.</returns>
    public static string GetClientPasswordLines(string password)
    {
      var passwordIniFileContentsBuilder = new StringBuilder(CLIENT_SECTION);
      passwordIniFileContentsBuilder.AppendLine();
      passwordIniFileContentsBuilder.AppendLine(string.Format(PASSWORD_LINE, password));
      return passwordIniFileContentsBuilder.ToString();
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
      _allLinesText = File.ReadAllLines(Path).ToList();

      var currentSection = String.Empty;
      foreach (var line in _allLinesText)
      {
        var isCommented = line.StartsWith("#");
        var lineTrimmed = isCommented ? line.Substring(1).TrimStart() : line;

        //Empty line
        if (string.IsNullOrEmpty(lineTrimmed))
        {
          Lines.Add(new IniLineFullDetail(IniLineType.EmptyLine, isCommented));
          continue;
        }

        //Section
        if (lineTrimmed.StartsWith("[") && lineTrimmed.EndsWith("]"))
        {
          currentSection = lineTrimmed.Substring(1, lineTrimmed.Length - 2);
          Lines.Add(new IniLineFullDetail(IniLineType.Section, isCommented, currentSection));
          continue;
        }

        //Key-Value pairs
        if (!string.IsNullOrEmpty(currentSection) && !lineTrimmed.Contains(' '))
        {
          var keyPair = lineTrimmed.Split(new[] { '=' }, 2);
          var lineType = IniLineType.KeyValuePair;
          // Search for keys with value
          if (keyPair.Length == 2)
          {
            Lines.Add(new IniLineFullDetail(lineType, isCommented, currentSection, keyPair[0], keyPair[1]));
            continue;
          }

          // Key has no value (E.g. no-beep)
          if (lineTrimmed.Any(char.IsLetterOrDigit))
          {
            lineType = IniLineType.Flag;
            Lines.Add(new IniLineFullDetail(lineType, isCommented, currentSection, keyPair[0]));
            continue;
          }
        }

        // Comment or separator.
        Lines.Add(new IniLineFullDetail(IniLineType.Comment, isCommented, currentSection, lineTrimmed));
      }
    }
  }
}
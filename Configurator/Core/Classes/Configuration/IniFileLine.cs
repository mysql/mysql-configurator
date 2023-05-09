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

namespace MySql.Configurator.Core.Classes.Configuration
{
  /// <summary>
  /// Represents a line in an INI configuration file.
  /// </summary>
  public class IniFileLine
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="IniFileLine"/> class.
    /// </summary>
    /// <param name="lineType">The type of the INI file line.</param>
    /// <param name="isCommented">Flag indicating whether the line is commented out.</param>
    /// <param name="section">The name of the section the configuration file line belongs to.</param>
    /// <param name="key">The name of a property (or key) declared in the configuration file line.</param>
    /// <param name="value">The value related to the property declared in the configuration file line.</param>
    public IniFileLine(IniFileLineType lineType, bool isCommented = false, string section = null, string key = null, string value = null)
    {
      Section = section;
      Key = key;
      Value = value;
      IniLineType = lineType;
      IsCommented = isCommented;
    }

    #region Properties

    /// <summary>
    /// Gets the type of the INI file line.
    /// </summary>
    public IniFileLineType IniLineType { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the line is commented out.
    /// </summary>
    public bool IsCommented { get; set; }

    /// <summary>
    /// Gets or sets the name of a property (or key) declared in the configuration file line.
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Gets the name of the section the configuration file line belongs to.
    /// </summary>
    public string Section { get; }

    /// <summary>
    /// Gets or sets the value related to the property declared in the configuration file line.
    /// </summary>
    public string Value { get; set; }

    #endregion Properties
  }
}
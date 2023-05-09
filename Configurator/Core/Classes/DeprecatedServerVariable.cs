/* Copyright (c) 2018, 2023, Oracle and/or its affiliates.

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
using MySql.Configurator.Core.Enums;

namespace MySql.Configurator.Core.Classes
{
  /// <summary>
  /// Represents a variable deprecated from the MySQL Server in one or more series.
  /// </summary>
  public class DeprecatedServerVariable
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="DeprecatedServerVariable"/> class.
    /// </summary>
    /// <param name="name">The name of the deprecated Server variable.</param>
    /// <param name="series">The Server series where the variable has been deprecated.</param>
    public DeprecatedServerVariable(string name, ServerSeriesType series)
    {
      if (string.IsNullOrEmpty(name))
      {
        throw new ArgumentNullException(nameof(name));
      }

      Name = name;
      Series = series;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeprecatedServerVariable"/> class.
    /// </summary>
    /// <param name="name">The name of the deprecated Server variable.</param>
    /// <param name="series">The Server series where the variable has been deprecated.</param>
    /// <param name="version">The Server version where the variable has been deprecated</param>
    public DeprecatedServerVariable(string name, ServerSeriesType series, Version version)
    {
      if (string.IsNullOrEmpty(name))
      {
        throw new ArgumentNullException(nameof(name));
      }

      Name = name;
      Series = series;
      Version = version;
    }

    #region Properties

    /// <summary>
    /// Gets the name of the deprecated Server variable.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets all Server series where the variable has been deprecated.
    /// </summary>
    public ServerSeriesType Series { get; }

    /// <summary>
    /// Gets the Server version where the variable has been deprecated.
    /// </summary>
    public Version Version { get; }

    #endregion
  }
}

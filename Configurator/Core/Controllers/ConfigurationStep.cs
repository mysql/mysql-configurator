/* Copyright (c) 2014, 2023, Oracle and/or its affiliates.

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

using MySql.Configurator.Core.Enums;

namespace MySql.Configurator.Core.Controllers
{
  /// <summary>
  /// Represents a single configuration step.
  /// </summary>
  public class ConfigurationStep : BaseStep
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationStep"/> class that executes on any configuration type.
    /// </summary>
    /// <param name="description">The description of the step shown to users.</param>
    /// <param name="time">The maximum execution time (in seconds) the step is estimated to take.</param>
    /// <param name="method">The <see cref="StepDelegate"/> that executes actions on this <see cref="ConfigurationStep"/>.</param>
    public ConfigurationStep(string description, int time, StepDelegate method) :
      this(description, time, method, true, ConfigurationType.All)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationStep"/> class.
    /// </summary>
    /// <param name="description">The description of the step shown to users.</param>
    /// <param name="time">The maximum execution time (in seconds) the step is estimated to take.</param>
    /// <param name="method">The <see cref="StepDelegate"/> that executes actions on this <see cref="ConfigurationStep"/>.</param>
    /// <param name="required">Flag indicating whether the whole configuration fails if this <see cref="ConfigurationStep"/> fails.</param>
    /// <param name="configurationType">The <see cref="ConfigurationType"/> this step applies to.</param>
    public ConfigurationStep(string description, int time, StepDelegate method, bool required, ConfigurationType configurationType) :
      base(description, time, method, true)
    {
      ConfigurationType = configurationType;
    }

    #region Properties

    /// <summary>
    /// Gets or sets the <see cref="ConfigurationType"/> this step applies to.
    /// </summary>
    public ConfigurationType ConfigurationType { get; set; }

    #endregion Properties

    /// <summary>
    /// Checks whether the current <see cref="ConfigurationStep"/> is valid for the given <see cref="ConfigurationType"/> value.
    /// </summary>
    /// <param name="type">A <see cref="ConfigurationType"/> value.</param>
    /// <returns><c>true</c> if the current <see cref="ConfigurationStep"/> is valid for the given <see cref="ConfigurationType"/> value, <c>false</c> otherwise.</returns>
    public bool ValidForConfigureType(ConfigurationType type)
    {
      return ConfigurationType.HasFlag(type);
    }
  }
}

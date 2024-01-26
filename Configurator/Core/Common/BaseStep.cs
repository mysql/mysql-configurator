/* Copyright (c) 2023, 2024, Oracle and/or its affiliates.

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

using MySql.Configurator.Core.Enums;

namespace MySql.Configurator.Core.Controllers
{
  public delegate void StepDelegate();

  /// <summary>
  /// Represents a single execution base step.
  /// </summary>
  public abstract class BaseStep
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseStep"/> class.
    /// </summary>
    /// <param name="description">The description of the step shown to users.</param>
    /// <param name="time">The maximum execution time (in seconds) the step is estimated to take.</param>
    /// <param name="method">The <see cref="StepDelegate"/> that executes actions on this <see cref="BaseStep"/>.</param>
    /// <param name="required">Flag indicating whether the whole configuration fails if this <see cref="BaseStep"/> fails.</param>
    public BaseStep(string description, int time, StepDelegate method, bool required)
    {
      Description = description;
      ElapsedTime = 0;
      Execute = true;
      NormalTime = time;
      StepMethod = method;
      Status = ConfigurationStepStatus.NotStarted;
      Required = required;
    }

    #region Properties

    /// <summary>
    /// Gets the description of the step shown to users.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Gets or sets the elapsed time (in seconds) of the configuration step.
    /// </summary>
    public int ElapsedTime { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the step will execute or will be skipped.
    /// </summary>
    public bool Execute { get; set; }

    /// <summary>
    /// Gets or sets the maximum execution time (in seconds) the step is estimated to take.
    /// </summary>
    public int NormalTime { get; set; }

    /// <summary>
    /// Gets a value indicating whether the whole configuration fails if this <see cref="BaseStep"/> fails.
    /// </summary>
    public bool Required { get; }

    /// <summary>
    /// Gets or sets a value indicating the status of this <see cref="BaseStep"/>.
    /// </summary>
    public ConfigurationStepStatus Status { get; set; }

    /// <summary>
    /// Gets the <see cref="StepDelegate"/> that executes actions on this <see cref="BaseStep"/>.
    /// </summary>
    public StepDelegate StepMethod { get; }

    #endregion Properties

    /// <summary>
    /// Allows to update the description outside of this class.
    /// </summary>
    /// <param name="newDescription">The new description.</param>
    public void ChangeDescription(string newDescription)
    {
      Description = newDescription;
    }
  }
}

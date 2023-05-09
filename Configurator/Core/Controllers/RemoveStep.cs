/* Copyright (c) 2019, 2023, Oracle and/or its affiliates.

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

namespace MySql.Configurator.Core.Controllers
{
  /// <summary>
  /// Represents a single removal step.
  /// </summary>
  public class RemoveStep : BaseStep
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationStep"/> class.
    /// </summary>
    /// <param name="description">The description of the step shown to users.</param>
    /// <param name="time">The maximum execution time (in seconds) the step is estimated to take.</param>
    /// <param name="method">The <see cref="StepDelegate"/> that executes actions on this <see cref="RemoveStep"/>.</param>
    /// <param name="required">Flag indicating whether the whole configuration fails if this <see cref="RemoveStep"/> fails.</param>
    public RemoveStep(string description, int time, StepDelegate method, bool required) :
      base(description, time, method, required)
    {
    }
  }
}
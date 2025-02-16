/* Copyright (c) 2024, Oracle and/or its affiliates.

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

using MySql.Configurator.Base.Classes;
using MySql.Configurator.Base.Enums;

namespace MySql.Configurator.Core.CLI
{
  /// <summary>
  /// Provides methods and properties to build a useful user message related to an exit code.
  /// </summary>
  public class CLIExitCode
  {
    #region Fields

    /// <summary>
    /// The arguments to include in the exit code message.
    /// </summary>
    private string[] _formatArguments;

    #endregion

    /// <summary>
    /// Initializes an instance of the <see cref="CLIExitCode"/> class.
    /// </summary>
    /// <param name="exitCode">The exit code associated to this instance.</param>
    /// <param name="formatArguments">The arguments to include in the descriptive message.</param>
    public CLIExitCode(ExitCode exitCode, params string[] formatArguments)
    {
      _formatArguments = formatArguments;
      ExitCode = exitCode;
    }

    #region Properties

    /// <summary>
    /// The exit code associated to this instance.
    /// </summary>
    public ExitCode ExitCode { get; private set; }

    #endregion

    /// <summary>
    /// Gets a formatted descriptive message for the assigned exit code.
    /// </summary>
    /// <returns>A string representing the description assigned to the exit code.</returns>
    public string GetExitCodeMessage()
    {
      var description = ExitCode.GetDescription();
      if (_formatArguments == null
          || _formatArguments.Length == 0)
      {
        return description;
      }

      return string.Format(description, _formatArguments);
    }
  }
}

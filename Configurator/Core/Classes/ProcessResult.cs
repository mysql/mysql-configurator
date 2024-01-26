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

using System.Diagnostics;

namespace MySql.Configurator.Core.Classes
{
  public class ProcessResult
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ProcessResult"/> class.
    /// </summary>
    /// <param name="runProcess">The <see cref="Process"/> that was run.</param>
    /// <param name="exitCode">The <see cref="Process.ExitCode"/> before the process exits.</param>
    /// <param name="standardOutput">The complete string of lines output to the <seealso cref="Process.StandardOutput"/>.</param>
    /// <param name="standardError">The complete string of lines output to the <seealso cref="Process.StandardError"/>.</param>
    public ProcessResult(Process runProcess, int? exitCode, string standardOutput, string standardError)
    {
      ExitCode = exitCode;
      RunProcess = runProcess;
      StandardError = standardError;
      StandardOutput = standardOutput;
    }

    #region Properties

    /// <summary>
    /// Gets the <see cref="Process.ExitCode"/> before the process exits, or <c>null</c> if the process has not exited.
    /// </summary>
    public int? ExitCode { get; }

    /// <summary>
    /// Gets the <see cref="Process"/> that was run, <c>null</c> if the process has already exited.
    /// </summary>
    public Process RunProcess { get; }

    /// <summary>
    /// Gets the complete string of lines output to the <seealso cref="Process.StandardError"/>.
    /// </summary>
    public string StandardError { get; }

    /// <summary>
    /// Gets the complete string of lines output to the <seealso cref="Process.StandardOutput"/>.
    /// </summary>
    public string StandardOutput { get; }

    #endregion Properties
  }
}

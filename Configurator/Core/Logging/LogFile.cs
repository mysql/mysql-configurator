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

using System;
using System.IO;
using System.Text;

namespace MySql.Configurator.Core.Logging
{
  /// <summary>
  /// Represents a log file used by any of the bundled products
  /// </summary>
  public class LogFile
  {
    #region Fields

    /// <summary>
    /// File information for the log file
    /// </summary>
    private FileInfo _logFileInfo;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the LogFile class
    /// </summary>
    /// <param name="filePath">Log file full path and name</param>
    /// <param name="encoding">File encoding</param>
    /// <param name="lineSeparator">Line separator used by the log file</param>
    public LogFile(string filePath, Encoding encoding, string lineSeparator)
    {
      if (string.IsNullOrEmpty(filePath))
      {
        throw new ArgumentNullException(nameof(filePath));
      }

      LogFileInfo = new FileInfo(filePath);
      FileEncoding = encoding;
      LineSeparator = lineSeparator;
    }

    #region Properties

    /// <summary>
    /// Gets a value indicating whether the log file exists on disk
    /// </summary>
    public bool Exists => _logFileInfo.Exists;

    /// <summary>
    /// Gets or sets the log file encoding
    /// </summary>
    public Encoding FileEncoding { get; set; }

    /// <summary>
    /// Gets the full file path and name of the log file
    /// </summary>
    public string FullName => _logFileInfo.FullName;

    /// <summary>
    /// Gets or sets the line separator used by the log file
    /// </summary>
    public string LineSeparator { get; set; }

    /// <summary>
    /// Gets the file information of the log file
    /// </summary>
    protected FileInfo LogFileInfo
    {
      get { return _logFileInfo; }
      private set { _logFileInfo = value; }
    }

    #endregion Properties
  }
}

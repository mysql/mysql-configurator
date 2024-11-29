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
using System.Reflection;
using System.Windows.Forms;
using MySql.Configurator.Base.Enums;

namespace MySql.Configurator.Core.Settings
{
  /// <summary>
  /// Represents the general configuration details of the MySQL Configurator.
  /// </summary>
  public sealed class AppConfiguration
  {
    #region Constants

    /// <summary>
    /// The name of the resource file that details the types of user roles.
    /// </summary>
    public const string ROLE_DEFINITIONS_FILE = "user-roles.xml";

    #endregion
    
    #region Fields

    /// <summary>
    /// The app data home directory of the application.
    /// </summary>
    private static string _homeDir;

    #endregion

    /// <summary>
    /// Intitializes a new instance of the <see cref="AppConfiguration"/> class.
    /// </summary>
    static AppConfiguration()
    {
      _homeDir = null;
    }

    #region Properties

    /// <summary>
    /// Gets or sets a flag indicating if the application is running in console mode.
    /// </summary>
    public static bool ConsoleMode { get; set; }

    /// <summary>
    /// Gets or sets the execution mode that the application will run with.
    /// </summary>
    public static ExecutionMode ExecutionMode { get; set; }

    /// <summary>
    /// Gets or sets the license type of the application.
    /// </summary>
    public static LicenseType License { get; set; }

    /// <summary>
    /// Gets the version number of the application.
    /// </summary>
    public static Version Version
    {
      get
      {
        var versionAsText = Application.ProductVersion;
        return !string.IsNullOrEmpty(versionAsText)
          ? Version.Parse(versionAsText)
          : Assembly.GetExecutingAssembly().GetName().Version;
      }
    }

    /// <summary>
    /// Gets or sets the home directory of this application.
    /// </summary>
    public static string HomeDir
    {
      get
      {
        if (string.IsNullOrEmpty(_homeDir))
        {
          _homeDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "MySQL", "MySQL Configurator");
        }

        return _homeDir;
      }
    }

    #endregion
  }
}

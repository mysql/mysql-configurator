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

using MySql.Configurator.Base.Enums;
using System;

namespace MySql.Configurator.Core.Server
{
  /// <summary>
  /// Defines a server installation identified by MySQL Configurator.
  /// </summary>
  public class ServerInstallation
  {
    #region Constants

    /// <summary>
    /// The architecture of the server installation.
    /// </summary>
    public const ServerInstallationArchitecture ARCHITECTURE = ServerInstallationArchitecture.X64;
    
    /// <summary>
    /// The user visible name of the MySQL Server product.
    /// </summary>
    public const string DISPLAY_NAME = "MySQL Server";

    #endregion

    /// <summary>
    /// Instantiates a new instance of the <see cref="ServerInstallation"/> class.
    /// </summary>
    /// <param name="license">The license of the server installation.</param>
    public ServerInstallation(LicenseType license)
    {
      License = license;
    }

    #region Properties

    /// <summary>
    /// Gets or sets the server configuration controller.
    /// </summary>
    public ServerConfigurationController Controller { get; private set; }

    /// <summary>
    /// Gets or sets the server installation license.
    /// </summary>
    public LicenseType License { get; private set; }

    /// <summary>
    /// Gets the product name along with the version number.
    /// </summary>
    public string NameWithVersion => $"{DISPLAY_NAME} {VersionString}";

    /// <summary>
    /// Gets or sets the version of the server installation.
    /// </summary>
    public Version Version { get; private set; }

    /// <summary>
    /// Gets or sets a string representation of the version of the server installation.
    /// </summary>
    public string VersionString { get; set; }

    #endregion

    /// <summary>
    /// Initializes the server installation instance.
    /// </summary>
    /// <param name="versionString">The version string associated to this server installation.</param>
    /// <param name="installationDirectory">The installation directory.</param>
    public void Initialize(string versionString, string installationDirectory)
    {
      VersionString = versionString;
      Version = new Version(versionString);
      if (Controller == null)
      {
        Controller = new ServerConfigurationController();
        Controller.ServerInstallation = this;
        Controller.Init();
      }

      if (!string.IsNullOrEmpty(installationDirectory))
      {
        var controller = Controller;
        controller.Settings.InstallDirectory = installationDirectory;
        Controller.LoadState();
      }
    }
  }
}

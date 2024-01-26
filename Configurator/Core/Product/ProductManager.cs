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
using System.Collections.Generic;
using System.Linq;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Core.Common;
using MySql.Configurator.Core.Enums;

namespace MySql.Configurator.Core.Product
{
  public delegate void ActionStartHandler(string action);

  public static class ProductManager
  {
    #region Properties

    public static List<Package.Package> InstalledPackages { get; private set; }

    #endregion Properties

    public static void SetPackageInstalled(Package.Package package, bool installed)
    {
      package.IsInstalled = installed;
      bool contains = InstalledPackages.Contains(package);
      if (!installed)
      {
        if (contains)
        {
          InstalledPackages.Remove(package);
        }
      }
      else
      {
        if (!contains)
        {
          InstalledPackages.Add(package);
        }
      }
    }

    /// <summary>
    /// Create a package based on the version and installation directory provided.
    /// </summary>
    /// <param name="version">The version to assign to the package.</param>
    /// <param name="installDir">The installation directoy to assing to the package.</param>
    /// <returns>An initialized MySQL Server package with the specified values.</returns>
    public static Package.Package LoadPackage(string version, string installDir)
    {
      var package = new Package.Package
      {
        Version = version,
        Publisher = "MySQL AB",
        DisplayName = "MySQL Server",
      };

      package.NormalizedVersion = Utilities.NormalVersion(package.Version);
      package.Initialize(installDir);
      package.Architecture = PackageArchitecture.X64;
      package.License = AppConfiguration.License;

      return package;
    }

    /// <summary>
    /// Creates a generic package.
    /// </summary>
    /// <returns>A non-initialized generic MySQL Server package.</returns>
    public static Package.Package LoadGenericPackage()
    {
      var package = new Package.Package
      {
        Version = "8.0.0",
        Publisher = "MySQL AB",
        DisplayName = "MySQL Server",
      };

      package.NormalizedVersion = Utilities.NormalVersion(package.Version);
      package.Architecture = PackageArchitecture.X64;
      package.License = AppConfiguration.License;

      return package;
    }
  }
}

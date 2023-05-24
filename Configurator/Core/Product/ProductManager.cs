/* Copyright (c) 2023, Oracle and/or its affiliates.

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
using System.Collections.Generic;
using System.Linq;
using MySql.Configurator.Core.Classes;

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

    //private static void FindInstalledPackagesWithoutUpgradeCodes(TwoKey<Version, Guid>[] packageCodes)
    //{
    //  foreach (TwoKey<Version, Guid> packageCode in packageCodes)
    //  {
    //    Package.Package package = Manifest.NoUpgradeCode[packageCode];
    //    InstallState state = MsiInterop.MsiQueryProductState(packageCode.Item2.ToString("B"));
    //    package.IsInstalled = state == InstallState.Default;
    //    if (!package.IsInstalled)
    //    {
    //      continue;
    //    }

    //    package.IsInstalled = true;
    //    package.Initialize();
    //    InstalledPackages.Add(package);
    //  }
    //}

    private static void FindInstalledPackagesWithUpgradeCodes(Guid upgradeCode)
    {
      var relatedProducts = MSI.Installer.GetRelatedProducts(upgradeCode);
      if (relatedProducts == null)
      {
        return;
      }

      foreach (var twoKey in relatedProducts)
      {
        Package.Package package = Package.Package.FromProductInfo(twoKey.Item2, upgradeCode);
        package.UpgradeCode = upgradeCode;
        package.IsInstalled = true;
        package.Initialize(null);
        InstalledPackages.Add(package);
      }
    }

    public static void LoadProducts(Guid upgradeCodeGuid)
    {
      InstalledPackages = new List<Package.Package>();
      FindInstalledPackagesWithUpgradeCodes(upgradeCodeGuid);
      //FindInstalledPackagesWithoutUpgradeCodes();
    }

    /// <summary>
    /// Gets the main product.
    /// </summary>
    /// <param name="productGuid"></param>
    /// <returns></returns>
    public static Package.Package GetMatchingProduct(Guid productGuid)
    {
      if (InstalledPackages == null
          || InstalledPackages.Count == 0)
      {
        return null;
      }

      return InstalledPackages.FirstOrDefault(package => package.Id == productGuid);
    }

    public static Package.Package LoadPackage(string version, string dataDir)
    {
      var package = new Package.Package
      {
        Version = version,
        Publisher = "MySQL AB",
        DisplayName = "MySQL Server",
      };

      package.NormalizedVersion = Utilities.NormalVersion(package.Version);
      package.Initialize(dataDir);

      // TODO: Ask RE to add this to registry.
      //package.License = LicenseType.Commercial;

      return package;
    }
  }
}
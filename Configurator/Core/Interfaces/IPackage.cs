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
using MySql.Configurator.Core.Controllers;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Core.Package;
using MySql.Configurator.Core.Product;

namespace MySql.Configurator.Core.Interfaces
{
  public interface IPackage
  {
    PackageArchitecture Architecture { get; }
    ProductConfigurationController Controller { get; }

    /// <summary>
    /// Gets or sets a flag indicating if the features for this package should be loaded.
    /// </summary>
    bool EnableFeaturesLoad { get; set; }

    int FeatureCount { get; }

    /// <summary>
    /// Gets or sets the list of features assigned to this package.
    /// </summary>
    List<PackageFeature> Features { get; set; }

    string FullPath { get; }
    Guid Id { get; }
    bool InCache { get; }
    LicenseType License { get; }
    string NameWithVersion { get; }
    bool PerMachine { get; }
    Product.Product Product { get; }
    string TempFileName { get; }
    string Version { get; }
    Guid UpgradeCode { get; }
    Package.Package UpgradeTarget { get; set; }

    void SetProposedInstall(bool shouldInstall);
  }
}

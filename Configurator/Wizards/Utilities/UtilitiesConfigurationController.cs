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
using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Core.Controllers;
using MySql.Configurator.Core.Package;

namespace MySql.Configurator.Wizards.Utilities
{
  [ProductConfiguration("utilities", 1)]
  public class UtilitiesConfigurationController : ProductConfigurationController
  {
    private readonly Version _lowerCombinedUtilitiesAndFabricPackage = new Version("1.5.0.0");
    private readonly Version _higherCombinedUtilitiesAndFabricPackage = new Version("1.6.2.0");


    public UtilitiesConfigurationController()
    {
      Logger.LogInformation("Product configuration controller created.");
    }

    public override bool PreUpgrade()
    {
      if (Package.UpgradeTarget == null)
      {
        return base.PreUpgrade();
      }

      return base.PreUpgrade();
    }

    public override bool PreInstall()
    {
      return base.PreInstall();
    }
  }
}

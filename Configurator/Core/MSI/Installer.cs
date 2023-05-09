/* Copyright (c) 2014, 2023, Oracle and/or its affiliates.

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
using System.Text;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Common;
using MySql.Configurator.Core.Enums;

namespace MySql.Configurator.Core.MSI
{
  public class Installer
  {
    public static IEnumerable<TwoKey<Version, Guid>> GetRelatedProducts(Guid upgradeCode)
    {
      string strUpgradeCode = upgradeCode.ToString("B");
      StringBuilder productCode = new StringBuilder(40);
      for (uint x = 0; ; x++)
      {
        var msiEnumError = MsiInterop.MsiEnumRelatedProducts(strUpgradeCode, 0, x, productCode);
        if (msiEnumError == MsiEnumError.NoMoreItems)
        {
          break;
        }

        if (msiEnumError != MsiEnumError.Success)
        {
          throw new ConfiguratorException(msiEnumError);
        }

        Guid productGuid;

        if (!Guid.TryParse(productCode.ToString(), out productGuid))
        {
          throw new ConfiguratorException("Error loading related products");
        }

        string ver = MsiInterop.GetProperty(productGuid, "VersionString");
        if (string.IsNullOrEmpty(ver)) continue;

        if (ver.Split('.').Length < 4)
        {
          ver += ".0"; //Review this
        }

        Version version;
        if (!Version.TryParse(ver, out version))
        {
          throw new ConfiguratorException("Error retrieving product version");
        }

        yield return new TwoKey<Version, Guid>(version, productGuid);
      }
    }

    private static void CheckResult(MsiEnumError ret)
    {
      if (ret == MsiEnumError.Success)
      {
        return;
      }

      throw new ConfiguratorException(ret);
    }
  }
}

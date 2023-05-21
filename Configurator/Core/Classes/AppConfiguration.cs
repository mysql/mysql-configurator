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
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using MySql.Configurator.Core.Enums;

namespace MySql.Configurator.Core.Classes
{
  public sealed class AppConfiguration
  {
    #region Fields

    private static string _homeDir;

    #endregion

    static AppConfiguration()
    {
      _homeDir = null;

      // Create a default instance.
      Instance = new AppConfigurationData();
    }

    #region Properties

    public static LicenseType License { get; set; }

    public static Version InstallerVersion
    {
      get
      {
        var versionAsText = Application.ProductVersion;
        return !string.IsNullOrEmpty(versionAsText)
          ? Version.Parse(versionAsText)
          : Assembly.GetExecutingAssembly().GetName().Version;
      }
    }

    public static Version VersionLaunched { get; set; }

    public static AppConfigurationData Instance { get; private set; }

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

    public static string RoleDefinitions =>"user-roles.xml";

    #endregion
  }
}

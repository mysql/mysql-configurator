/* Copyright (c) 2024, Oracle and/or its affiliates.

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

using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Core.Common;
using MySql.Configurator.Core.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;

namespace MySql.Configurator.Core.Classes
{
  /// <summary>
  /// Handles the reading the mysql_upgrade_history server json file.
  /// </summary>
  public static class MySqlUpgradeHistoryManager
  {
    #region Constants

    /// <summary>
    /// The name of the ugprade history file.
    /// </summary>
    public const string UPGRADE_HISTORY_FILE_NAME = "mysql_upgrade_history";

    #endregion

    /// <summary>
    /// Loads the specified upgrade history file.
    /// </summary>
    /// <param name="path">The directory path where the MySQL upgrade history file is located.</param>
    /// <returns>A tuple containing the version for the latest entry as well as a flag indicating
    /// if the version is appended with a metadata string.</returns>
    public static (Version, bool) GetUpgradeHistoryLatestVersion(string mysqlUpgradeHistoryFilePath)
    {
      if (string.IsNullOrEmpty(mysqlUpgradeHistoryFilePath))
      {
        throw new ArgumentNullException(nameof(mysqlUpgradeHistoryFilePath));
      }

      var filePath = Path.Combine(mysqlUpgradeHistoryFilePath, "Data", UPGRADE_HISTORY_FILE_NAME);
      if (!File.Exists(filePath))
      {
        Logger.LogException(new FileNotFoundException(filePath));
        return (null, false);
      }

      try
      {
        MySqlUpgradeHistory upgradeHistory;
        using (StreamReader reader = new StreamReader(filePath))
        {
          var jsonString = reader.ReadToEnd();
          var serializer = new JavaScriptSerializer();
          upgradeHistory = serializer.Deserialize<MySqlUpgradeHistory>(jsonString);
        }

        if (upgradeHistory.Upgrade_History.Count == 0)
        {
          throw new ConfiguratorException(ConfiguratorError.UpgradeHistoryElementsNotFound);
        }

        var latestEntry = upgradeHistory.Upgrade_History.OrderByDescending(o => DateTime.Parse(o.Date)).First();
        var versionItems = latestEntry.Version.Split('-');
        var version = new Version(versionItems[0]);
        return (version, versionItems.Length > 1);
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
        return (null, false);
      }
    }
  }

  /// <summary>
  /// The upgrade history of a MySQL Server instance.
  /// Represents the root element of the mysql_upgrade_history server file.
  /// </summary>
  public class MySqlUpgradeHistory
  {
    /// <summary>
    /// Gets or sets the file format version number.
    /// </summary>
    public string File_Format { get; set; }
    /// <summary>
    /// Gets or sets a list containing the upgrade history.
    /// </summary>
    public List<UpgradeHistory> Upgrade_History { get; set; }
  }

  /// <summary>
  /// Contains historical upgrade details for a MySQL Server instance.
  /// </summary>
  public class UpgradeHistory
  {
    /// <summary>
    /// Gets or sets a value indicating the date of the upgrade.
    /// </summary>
    public string Date { get; set; }
    /// <summary>
    /// Gets or sets a value indicating the version number of the server instance.
    /// </summary>
    public string Version { get; set; }
    /// <summary>
    /// Gets or sets a value indicating the maturity (LTS or innovation) of the server instance..
    /// </summary>
    public string Maturity { get; set; }
    /// <summary>
    /// Gets or sets a value indicating if the data directory was initialized.
    /// </summary>
    public bool Initialize { get; set; }
  }
}

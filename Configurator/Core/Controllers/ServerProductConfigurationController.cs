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

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Dialogs;
using MySql.Configurator.Core.Enums;

namespace MySql.Configurator.Core.Controllers
{
  public abstract class ServerProductConfigurationController : ProductConfigurationController
  {
    #region Constants

    /// <summary>
    /// The name of the Server's admin tools executable filename.
    /// </summary>
    public const string ADMIN_TOOL_EXECUTABLE_FILENAME = "mysqladmin.exe";

    /// <summary>
    /// The binary directory name.
    /// </summary>
    public const string BINARY_DIRECTORY_NAME = "bin";

    /// <summary>
    /// The name of the Server's client executable filename.
    /// </summary>
    public const string CLIENT_EXECUTABLE_FILENAME = "mysql.exe";

    /// <summary>
    /// The name of the Server's dump tool executable filename.
    /// </summary>
    public const string DUMP_TOOL_EXECUTABLE_FILENAME = "mysqldump.exe";

    /// <summary>
    /// The name of the Server's executable filename.
    /// </summary>
    public const string SERVER_EXECUTABLE_FILENAME = "mysqld.exe";

    /// <summary>
    /// The name of the Server's executable filename without its extension.
    /// </summary>
    public const string SERVER_EXECUTABLE_FILENAME_NO_EXTENSION = "mysqld";

    /// <summary>
    /// The name of the Server's upgrade tool executable filename.
    /// </summary>
    public const string UPGRADE_TOOL_EXECUTABLE_FILENAME = "mysql_upgrade.exe";

    #endregion Constants

    #region Properties

    public string DataDirectory
    {
      get => Settings.DataDirectory;
      set => Settings.DataDirectory = value;
    }

    /// <summary>
    /// Gets a value indicating whether the data directory exists and contains files (i.e. the database has been initialized).
    /// </summary>
    public bool IsThereServerDataFiles => HasDataSubDirectoryWithFiles(DataDirectory);

    public string ServerExecutableDirPath => Path.Combine(InstallDirectory, BINARY_DIRECTORY_NAME);

    public string ServerExecutableFilePath => Path.Combine(InstallDirectory, BINARY_DIRECTORY_NAME, SERVER_EXECUTABLE_FILENAME);

    public new BaseServerSettings Settings => settings as BaseServerSettings;

    #endregion Properties

    /// <summary>
    /// Checks if a given directory path contains a Data subdirectory with contents in it.
    /// </summary>
    /// <param name="directoryPath">A full directory path.</param>
    /// <returns><c>true</c> if the given directory path contains a Data subdirectory with contents in it, <c>false</c> otherwise.</returns>
    public bool HasDataSubDirectoryWithFiles(string directoryPath)
    {
      if (string.IsNullOrEmpty(directoryPath))
      {
        return false;
      }

      string dataDirectory = Path.Combine(directoryPath, "Data");
      return Directory.Exists(dataDirectory) && Directory.EnumerateFiles(dataDirectory).Any();
    }

    public string GetConnectionString(bool promptIfNecessary, bool useOldSettings)
    {
      string username = "root";
      string rootPwd = Settings.RootPassword;

      if (string.IsNullOrEmpty(rootPwd))
      {
        if (!promptIfNecessary)
        {
          return null;
        }

        using (var dlg = new RootPasswordPromptDialog(Package.NameWithVersion, Settings.Port, false))
        {
          if (dlg.ShowDialog() == DialogResult.Cancel)
          {
            return null;
          }

          username = dlg.Username;
          rootPwd = dlg.Password;
        }
      }

      string connStr = GetConnectionString(username, rootPwd, useOldSettings, string.Empty);
      return connStr;
    }

    public virtual string GetConnectionString(string username, string password, bool useOldSettings, string schemaName)
    {
      return string.Empty;
    }

    public override string GetMsiCommandLine()
    {
      return $"{base.GetMsiCommandLine()} DATADIR=\"{DataDirectory}\"";
    }

    public bool HasExistingDataDirectory()
    {
      return Directory.Exists(DataDirectory)
             && Directory.EnumerateFileSystemEntries(DataDirectory).Any();
    }

    protected PathWarningType CheckDataDirectory(string path, List<string> paths)
    {
      var warningType = PathWarningType.None;
      if (!Path.IsPathRooted(path))
      {
        warningType = PathWarningType.DataDirPathInvalid;
      }
      else if (Directory.Exists(path))
      {
        warningType = PathWarningType.DataDirPathExists;
      }
      else if (Utilities.PathAlreadyUsed(path, paths))
      {
        warningType = PathWarningType.DataDirPathCurrentInUse;
      }

      return warningType;
    }

    /// <summary>
    /// Deletes the server data directory.
    /// </summary>
    /// <returns><c>true</c> if the data directory was deleted successfully; otherwise, <c>false</c>.</returns>
    protected bool DeleteDataDirectory()
    {
      if (!HasExistingDataDirectory())
      {
        return true;
      }

      return Utilities.DeleteDirectory(DataDirectory);
    }
  }
}

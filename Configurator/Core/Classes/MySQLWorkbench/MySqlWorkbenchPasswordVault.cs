// Copyright (c) 2023, Oracle and/or its affiliates.
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License as
// published by the Free Software Foundation; version 2 of the
// License.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
// 02110-1301  USA

using MySql.Configurator.Core.Classes.Security;
using MySql.Utility.Classes;
using System;
using System.IO;
using System.Security.Cryptography;

namespace MySql.Configurator.Core.Classes.MySqlWorkbench
{
  /// <summary>
  /// Defines methods that help working with MySQL Workbench connection passwords.
  /// </summary>
  public static class MySqlWorkbenchPasswordVault
  {
    #region Constants

    /// <summary>
    /// Character used to separate the vault key elements, which are the host name and the user name.
    /// </summary>
    internal const char DOMAIN_SEPARATOR = (char)2;

    #endregion Constants

    #region Fields

    /// <summary>
    /// The consumer application's password vault.
    /// </summary>
    private static SecureVault _applicationPasswordsVault;

    /// <summary>
    /// The consumer application's file path of the password vault file to be used.
    /// </summary>
    private static string _applicationPasswordVaultFilePath;

    /// <summary>
    /// The MySQL Workbench password vault.
    /// </summary>
    private static SecureVault _workbenchPasswordsVault;

    #endregion Fields

    /// <summary>
    /// Initializes the <see cref="MySqlWorkbenchPasswordVault"/> class.
    /// </summary>
    static MySqlWorkbenchPasswordVault()
    {
      _applicationPasswordsVault = null;
      _applicationPasswordVaultFilePath = null;
      _workbenchPasswordsVault = new SecureVault(WorkbenchPasswordVaultFilePath, DataProtectionScope.CurrentUser);
    }

    #region Properties

    /// <summary>
    /// Gets or sets the consumer application's file path of the password vault file to be used.
    /// </summary>
    public static string ApplicationPasswordVaultFilePath
    {
      get
      {
        if (string.IsNullOrEmpty(_applicationPasswordVaultFilePath))
        {
          throw new Exception("Passwords vault file path must be defined.");
        }

        return _applicationPasswordVaultFilePath;
      }

      set
      {
        _applicationPasswordVaultFilePath = value;
        if (string.IsNullOrEmpty(_applicationPasswordVaultFilePath))
        {
          return;
        }

        _applicationPasswordsVault = new SecureVault(_applicationPasswordVaultFilePath, DataProtectionScope.CurrentUser);
      }
    }

    /// <summary>
    /// Gets the file path of the MySQL Workbench password vault file.
    /// </summary>
    public static string WorkbenchPasswordVaultFilePath => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MySQL\Workbench\workbench_user_data.dat";

    /// <summary>
    /// Gets the consumer application's password vault.
    /// </summary>
    private static SecureVault ApplicationPasswordsVault
    {
      get
      {
        if (_applicationPasswordsVault == null)
        {
          throw new Exception("Passwords vault file path must be defined.");
        }

        return _applicationPasswordsVault;
      }
    }

    #endregion Properties

    /// <summary>
    /// Deletes the password for the given host identifier and user name, if it exists.
    /// </summary>
    /// <param name="useWorkbenchPasswordsFile">Flag indicating if the passwords vault file used is the MySQL Workbench one.</param>
    /// <param name="hostIdentifier">Host identifier usually composed of the Database Driver name and Host name.</param>
    /// <param name="userName">User name.</param>
    public static void DeletePassword(bool useWorkbenchPasswordsFile, string hostIdentifier, string userName)
    {
      var pwdKey = hostIdentifier + DOMAIN_SEPARATOR + userName;
      GetPasswordsVault(useWorkbenchPasswordsFile).DeleteEntry(pwdKey);
    }

    /// <summary>
    /// Deletes the password for the given host identifier and user name, if it exists.
    /// </summary>
    /// <param name="hostIdentifier">Host identifier usually composed of the Database Driver name and Host name.</param>
    /// <param name="userName">User name.</param>
    public static void DeletePassword(string hostIdentifier, string userName)
    {
      DeletePassword(MySqlWorkbench.UseWorkbenchConnections, hostIdentifier, userName);
    }

    /// <summary>
    /// Returns the plain text password for the given host identifier and user name.
    /// </summary>
    /// <param name="useWorkbenchPasswordsFile">Flag indicating if the passwords vault file used is the MySQL Workbench one.</param>
    /// <param name="hostIdentifier">Host identifier usually composed of the Database Driver name and Host name.</param>
    /// <param name="userName">User name.</param>
    /// <returns>Password in plain text.</returns>
    public static string FindPassword(bool useWorkbenchPasswordsFile, string hostIdentifier, string userName)
    {
      var pwdKey = hostIdentifier + DOMAIN_SEPARATOR + userName;
      return GetPasswordsVault(useWorkbenchPasswordsFile).FindEntry(pwdKey);
    }

    /// <summary>
    /// Returns the plain text password for the given host identifier and user name.
    /// </summary>
    /// <param name="hostIdentifier">Host identifier usually composed of the Database Driver name and Host name.</param>
    /// <param name="userName">User name.</param>
    /// <returns>Password in plain text.</returns>
    public static string FindPassword(string hostIdentifier, string userName)
    {
      return FindPassword(MySqlWorkbench.UseWorkbenchConnections, hostIdentifier, userName);
    }

    /// <summary>
    /// Gets the <see cref="SecureVault"/> to use to fetch or store passwords.
    /// </summary>
    /// <param name="useWorkbenchPasswordsFile">Flag indicating if the passwords vault file used is the MySQL Workbench one.</param>
    /// <returns>The <see cref="SecureVault"/> to use to fetch or store passwords.</returns>
    public static SecureVault GetPasswordsVault(bool useWorkbenchPasswordsFile)
    {
      return useWorkbenchPasswordsFile
        ? _workbenchPasswordsVault
        : ApplicationPasswordsVault;
    }

    /// <summary>
    /// Loads encrypted passwords from disk. These are typically held only for a short moment.
    /// </summary>
    /// <param name="useWorkbenchPasswordsFile">Flag indicating if the passwords vault file used is the MySQL Workbench one.</param>
    /// <returns>Quantity of passwords loaded.</returns>
    public static int LoadPasswords(bool useWorkbenchPasswordsFile)
    {
      return GetPasswordsVault(useWorkbenchPasswordsFile).LoadEntries();
    }

    /// <summary>
    /// Loads encrypted passwords from disk. These are typically held only for a short moment.
    /// </summary>
    /// <returns>Quantity of passwords loaded.</returns>
    public static int LoadPasswords()
    {
      return LoadPasswords(MySqlWorkbench.UseWorkbenchConnections);
    }

    /// <summary>
    /// Migrates passwords from a consumer application's passwords vault file to the MySQL Workbench one.
    /// </summary>
    /// <returns>A <see cref="MigrationResult"/> instance with information about the migration.</returns>
    public static MigrationResult MigratePasswordsFromConsumerApplicationToWorkbench()
    {
      // If MySQL Workbench is not installed or the consumer application's connections file does not exist
      // it means we already migrated existing connections or they were never created, no need to migrate.
      if (!MySqlWorkbench.IsInstalled
          || !File.Exists(ApplicationPasswordVaultFilePath))
      {
        return null;
      }

      var consumerApplicationPasswordsVault = GetPasswordsVault(false);
      var workbenchPasswordsVault = GetPasswordsVault(true);
      return workbenchPasswordsVault.MigrateEntriesFrom(consumerApplicationPasswordsVault);
    }

    /// <summary>
    /// Stores the given password in an encrypted binary file along with its related host identifier and user name.
    /// </summary>
    /// <param name="useWorkbenchPasswordsFile">Flag indicating if the passwords vault file used is the MySQL Workbench one.</param>
    /// <param name="hostIdentifier">Host identifier usually composed of the Database Driver name and Host name.</param>
    /// <param name="userName">User name.</param>
    /// <param name="password">Password.</param>
    public static void StorePassword(bool useWorkbenchPasswordsFile, string hostIdentifier, string userName, string password)
    {
      if (string.IsNullOrEmpty(userName)
          || string.IsNullOrEmpty(password))
      {
        return;
      }

      var pwdKey = hostIdentifier + DOMAIN_SEPARATOR + userName;
      GetPasswordsVault(useWorkbenchPasswordsFile).StoreEntry(pwdKey, password);
    }

    /// <summary>
    /// Stores the given password in an encrypted binary file along with its related host identifier and user name.
    /// </summary>
    /// <param name="hostIdentifier">Host identifier usually composed of the Database Driver name and Host name.</param>
    /// <param name="userName">User name.</param>
    /// <param name="password">Password.</param>
    public static void StorePassword(string hostIdentifier, string userName, string password)
    {
      StorePassword(MySqlWorkbench.UseWorkbenchConnections, hostIdentifier, userName, password);
    }

    /// <summary>
    /// Clears the password cache so passwords aren't kept in memory any longer than necessary.
    /// </summary>
    /// <param name="useWorkbenchPasswordsFile">Flag indicating if the passwords vault file used is the MySQL Workbench one.</param>
    /// <param name="saveInFile">Flag indicating if password cache is saved to disk before clearing.</param>
    /// <returns><c>true</c> if passwords were successfully saved to disk, <c>false</c> otherwise.</returns>
    public static bool UnloadPasswords(bool useWorkbenchPasswordsFile, bool saveInFile)
    {
      return GetPasswordsVault(useWorkbenchPasswordsFile).UnloadEntries(saveInFile);
    }

    /// <summary>
    /// Clears the password cache so passwords aren't kept in memory any longer than necessary.
    /// </summary>
    /// <param name="saveInFile">Flag indicating if password cache is saved to disk before clearing.</param>
    /// <returns><c>true</c> if passwords were successfully saved to disk, <c>false</c> otherwise.</returns>
    public static bool UnloadPasswords(bool saveInFile)
    {
      return UnloadPasswords(MySqlWorkbench.UseWorkbenchConnections, saveInFile);
    }
  }
}

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

using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Properties;
using MySql.Utility.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MySql.Configurator.Core.Classes.Security
{
  /// <summary>
  /// Represents a secure vault to save string values into disk using encryption.
  /// </summary>
  public class SecureVault
  {
    #region Constants

    /// <summary>
    /// The default character used to separate the value from the vault key.
    /// </summary>
    public const char DEFAULT_KEY_VALUE_SEPARATOR = (char)3;

    #endregion Constants

    /// <summary>
    /// Initializes the <see cref="SecureVault"/> class.
    /// </summary>
    /// <param name="filePath">The consumer application's file path of the secure vault file to be used.</param>
    /// <param name="protectionScope">The <see cref="DataProtectionScope"/> used for the encrypted file.</param>
    public SecureVault(string filePath, DataProtectionScope protectionScope)
    {
      KeyValueSeparator = DEFAULT_KEY_VALUE_SEPARATOR;
      ProtectionScope = protectionScope;
      if (string.IsNullOrEmpty(filePath))
      {
        throw new Exception("Secure vault file path must be defined.");
      }

      FilePath = filePath;
    }

    #region Properties

    /// <summary>
    /// Gets the consumer application's file path of the secure vault file to be used.
    /// </summary>
    public string FilePath { get; }

    /// <summary>
    /// Gets or sets the character used to separate a key and its value on file.
    /// </summary>
    public char KeyValueSeparator { get; set; }

    /// <summary>
    /// Gets a dictionary of saved entries.
    /// </summary>
    public Dictionary<string, string> EntriesCache { get; private set; }

    /// <summary>
    /// Gets or sets the <see cref="DataProtectionScope"/> used for the encrypted file.
    /// </summary>
    public DataProtectionScope ProtectionScope { get; set; }

    #endregion Properties

    /// <summary>
    /// Deletes the entry for the given key, if it exists.
    /// </summary>
    /// <param name="key">The key of the entry to delete.</param>
    public virtual void DeleteEntry(string key)
    {
      if (string.IsNullOrEmpty(key))
      {
        return;
      }

      var entriesCount = LoadEntries();
      if (entriesCount > 0
          && EntriesCache.ContainsKey(key))
      {
        EntriesCache.Remove(key);
        UnloadEntries(true);
      }
      else
      {
        UnloadEntries(false);
      }
    }

    /// <summary>
    /// Returns the plain text value for the given key.
    /// </summary>
    /// <param name="key">The unique key to store the value.</param>
    /// <returns>The plain text value for the given key.</returns>
    public virtual string FindEntry(string key)
    {
      if (string.IsNullOrEmpty(key))
      {
        return null;
      }

      string value = null;
      int entriesCount = LoadEntries();
      if (entriesCount > 0)
      {
        if (EntriesCache.ContainsKey(key))
        {
          value = EntriesCache[key];
        }

        UnloadEntries(false);
      }

      return value;
    }

    /// <summary>
    /// Loads encrypted entries from disk. These are typically held only for a short moment.
    /// </summary>
    /// <returns>The count of entries loaded.</returns>
    public virtual int LoadEntries()
    {
      var entriesQuantity = 0;
      if (!File.Exists(FilePath))
      {
        return entriesQuantity;
      }

      try
      {
        var encryptedData = File.ReadAllBytes(FilePath);
        var decryptedData = ProtectedData.Unprotect(encryptedData, null, ProtectionScope);
        var decryptedText = Encoding.ASCII.GetString(decryptedData);
        var entriesLinesArray = decryptedText.Split('\n');
        if (EntriesCache == null)
        {
          EntriesCache = new Dictionary<string, string>(entriesLinesArray.Length);
        }
        else
        {
          EntriesCache.Clear();
        }

        for (var lineNumber = 0; lineNumber < entriesLinesArray.Length; lineNumber++)
        {
          var passwordLine = entriesLinesArray[lineNumber];
          if (passwordLine == "\0")
          {
            break;
          }

          var pieces = passwordLine.Split(KeyValueSeparator);
          EntriesCache.Add(pieces[0], pieces.Length > 1 ? pieces[1] : string.Empty);
          entriesLinesArray[lineNumber] = null;
        }

        entriesQuantity = EntriesCache.Count;
      }
      catch (Exception ex)
      {
        Logger.LogException(ex, true, Resources.PasswordsFileLoadError, Resources.GenericErrorTitle);
      }

      return entriesQuantity;
    }

    /// <summary>
    /// Migrates passwords from a consumer application's passwords vault file to the MySQL Workbench one.
    /// </summary>
    /// <param name="foreignVault">A <see cref="SecureVault"/> to migrate entries from.</param>
    /// <returns>A <see cref="MigrationResult"/> instance with information about the migration.</returns>
    public virtual MigrationResult MigrateEntriesFrom(SecureVault foreignVault)
    {
      string errorMessage = null;
      if (foreignVault == null)
      {
        return null;
      }

      // Load entries, from the foreign vault, that are going to be migrated.
      foreignVault.LoadEntries();

      // If there are no entries, then nothing to migrate
      if (foreignVault.EntriesCache == null
          || foreignVault.EntriesCache.Count == 0)
      {
        return null;
      }

      // Load this vault entries, if Workbench has not created its passwords file then create it.
      var thisEntriesQuantity = LoadEntries();
      if (thisEntriesQuantity == 0
          || EntriesCache == null)
      {
        EntriesCache = new Dictionary<string, string>(1);
      }

      // Migrate only the entries that do not exist in this vault, skip the ones already here.
      foreach (var entry in foreignVault.EntriesCache.Where(entry => !EntriesCache.ContainsKey(entry.Key)))
      {
        EntriesCache.Add(entry.Key, entry.Value);
      }

      // Clear the foreign entries cache for security purposes.
      foreignVault.EntriesCache.Clear();

      var saveSuccess = false;
      try
      {
        // Attempt to rename the foreign vault file, if we can rename it we proceed with saving the entries in this vault's file.
        var backupFilePath = $"{foreignVault.FilePath}.bak";
        File.Move(foreignVault.FilePath, backupFilePath);

        // Save this vault's entries in file.
        saveSuccess = UnloadEntries(true);

        // Delete the renamed foreign vault's file if saving was successful, otherwise we revert it back.
        if (saveSuccess)
        {
          File.Delete(backupFilePath);
        }
        else
        {
          File.Move(backupFilePath, foreignVault.FilePath);
        }
      }
      catch (Exception ex)
      {
        errorMessage = ex.InnerException != null
          ? ex.InnerException.Message
          : ex.Message;
      }

      return new MigrationResult(saveSuccess, errorMessage);
    }

    /// <summary>
    /// Stores the given value in an encrypted binary file along with its related key.
    /// </summary>
    /// <param name="key">The unique key to store the value.</param>
    /// <param name="value">The value to store.</param>
    public virtual void StoreEntry(string key, string value)
    {
      if (string.IsNullOrEmpty(key)
          || string.IsNullOrEmpty(value))
      {
        return;
      }

      int entriesQuantity = LoadEntries();
      if (entriesQuantity > 0
          && EntriesCache.ContainsKey(key))
      {
        EntriesCache[key] = value;
      }
      else
      {
        if (EntriesCache == null)
        {
          EntriesCache = new Dictionary<string, string>(1);
        }

        EntriesCache.Add(key, value);
      }

      UnloadEntries(true);
    }

    /// <summary>
    /// Clears the entries cache so entries aren't kept in memory any longer than necessary.
    /// </summary>
    /// <param name="saveInFile">Flag indicating if the entries cache is saved to disk before clearing.</param>
    /// <returns><c>true</c> if entries were successfully saved to disk, <c>false</c> otherwise.</returns>
    public virtual bool UnloadEntries(bool saveInFile)
    {
      var saveSuccess = true;
      if (EntriesCache == null
          || EntriesCache.Count == 0)
      {
        return false;
      }

      if (saveInFile)
      {
        try
        {
          var decryptedText = new StringBuilder(string.Empty);
          foreach (var passwordItem in EntriesCache)
          {
            decryptedText.Append(passwordItem.Key);
            decryptedText.Append(KeyValueSeparator);
            decryptedText.Append(passwordItem.Value);
            decryptedText.Append("\n");
          }

          decryptedText.Append("\0");
          var decryptedData = Encoding.ASCII.GetBytes(decryptedText.ToString());
          var encryptedData = ProtectedData.Protect(decryptedData, null, ProtectionScope);
          decryptedText.Clear();
          File.WriteAllBytes(FilePath, encryptedData);
        }
        catch (Exception ex)
        {
          Logger.LogException(ex, true, Resources.PasswordsFileSaveError, Resources.GenericErrorTitle);
          saveSuccess = false;
        }
      }

      EntriesCache.Clear();
      return saveInFile
             && saveSuccess;
    }
  }
}
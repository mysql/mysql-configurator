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
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Microsoft.Win32;
using MySql.Configurator.Core.Classes.Forms;
using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Core.Classes.MySql;
using MySql.Configurator.Core.Controls;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Properties;
using MySql.Data.MySqlClient;

namespace MySql.Configurator.Core.Classes
{
  // Define the Action delegate again. Otherwise we get build problems with the ABS.
  public delegate void Action();

  public static class Utilities
  {
    #region Constants

    /// <summary>
    /// The text used on default collations for a specific character set.
    /// </summary>
    public const string DEFAULT_COLLATION_TEXT = "default collation";

    public const int DELETE_DIRECTORY_ERROR_RETRY_WAIT_IN_MILLISECONDS = 200;

    /// <summary>
    /// The secret key in string format used to generate the hashing.
    /// </summary>
    private const string HASHING_KEY = "1e65340434a8e8c77ef56a4bc0955a0c";

    /// <summary>
    /// The regex used to validate IPv4 addresses.
    /// </summary>
    public const string IPV4_ADDRESS_REGEX_VALIDATION = @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$";

    /// <summary>
    /// A large prime number integer (to avoid hashing collisions) used as a base in a <see cref="object.GetHashCode"/> override implementation.
    /// </summary>
    public const int HASHING_BASE = unchecked((int)2166136261);

    /// <summary>
    /// A large prime number integer (to avoid hashing collisions) used as a multiplier for other hash values in a <see cref="object.GetHashCode"/> override implementation.
    /// </summary>
    public const int HASHING_MULTIPLIER = 16777619;

    /// <summary>
    /// The regex used to validate host names.
    /// </summary>
    public const string HOSTNAME_REGEX_VALIDATION = @"^(([a-zA-Z]|[a-zA-Z][a-zA-Z0-9-]*[a-zA-Z0-9]).)*([A-Za-z]|[A-Za-z][A-Za-z0-9-]*[A-Za-z0-9])$";

    /// <summary>
    /// The minimum port number allowed for MySQL connections.
    /// </summary>
    public const uint MIN_PORT_NUMBER_ALLOWED = 80;

    /// <summary>
    /// The maximum port number allowed for MySQL connections.
    /// </summary>
    public const uint MAX_PORT_NUMBER_ALLOWED = 65535;

    /// <summary>
    /// The regex used to validate MySQL user and cluster names.
    /// </summary>
    public const string NAME_REGEX_VALIDATION = @"^(\w|\d|_|\s)+$";

    /// <summary>
    /// The base string used to identify the location of a server registry key.
    /// </summary>
    private const string REGISTRY_KEY_TEMPLATE = "SOFTWARE\\MySQL AB\\MySQL Server {0}.{1}";

    /// <summary>
    /// The maximum length allowed for MySQL user names.
    /// </summary>
    public const int USERNAME_MAX_LENGTH = 32;

    /// <summary>
    /// The registry key name to check for installed (or MSIs to uninstall) products.
    /// </summary>
    private const string UNINSTALL_REGISTRY_KEY_NAME = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

    #endregion Constants

    public static int CurrentDPI
    {
      get
      {
        using (var control = new Control())
        {
          using (var graphics = control.CreateGraphics())
          {
            return (int) graphics.DpiX;
          }
        }
      }
    }

    public static PathWarningType CheckInstallDirectory(string path, List<string> paths)
    {
      var warningType = PathWarningType.None;
      if (!Path.IsPathRooted(path))
      {
        warningType = PathWarningType.InstallDirPathInvalid;
      }
      else if (Directory.Exists(path))
      {
        warningType = PathWarningType.InstallDirPathExists;
      }
      else if (PathAlreadyUsed(path, paths))
      {
        warningType = PathWarningType.InstallDirPathCurrentInUse;
      }

      return warningType;
    }

    /// <summary>
    /// Compares two strings containing numbers and optionally additional content lexically.
    /// </summary>
    /// <returns>Same as string.CompareTo.</returns>
    public static int CompareAsNumbers(string s1, string s2)
    {
      // Make strings comparable as numbers (they must have the same length to work properly).
      if (s1.Length < s2.Length)
      {
        s1 = s1.PadLeft(s2.Length, '0');
      }

      if (s2.Length < s1.Length)
      {
        s2 = s2.PadLeft(s1.Length, '0');
      }

      return string.Compare(s1, s2, StringComparison.Ordinal);
    }

    /// <summary>
    /// Attempts to create a directory with the given path.
    /// </summary>
    /// <param name="directoryPath">The directory path.</param>
    /// <param name="retryCount">The number of times the operation must be retried if an error occurs.</param>
    /// <param name="waitInMilliseconds">The time to wait between retries.</param>
    /// <param name="errorMessage">An error message if an error occurs.</param>
    /// <returns><c>true</c> if the directory is created successfully, <c>false</c> otherwise.</returns>
    public static bool CreateDirectory(string directoryPath, int retryCount, int waitInMilliseconds, out string errorMessage)
    {
      errorMessage = null;
      if (string.IsNullOrEmpty(directoryPath))
      {
        return true;
      }

      var currentRetry = 0;
      while (!Directory.Exists(directoryPath)
             && currentRetry < retryCount)
      {
        try
        {
          Directory.CreateDirectory(directoryPath);
          errorMessage = null;
          break;
        }
        catch (Exception ex)
        {
          currentRetry++;
          if (waitInMilliseconds > 0)
          {
            Thread.Sleep(waitInMilliseconds);
          }

          DeleteDirectory(directoryPath, retryCount, waitInMilliseconds, waitInMilliseconds, out errorMessage);
          errorMessage = ex.Message;
        }
      }

      return string.IsNullOrEmpty(errorMessage);
    }

    public static T DeepClone<T>(T obj)
    {
      using (var ms = new MemoryStream())
      {
        var formatter = new BinaryFormatter();
        formatter.Serialize(ms, obj);
        ms.Position = 0;

        return (T)formatter.Deserialize(ms);
      }
    }

    /// <summary>
    /// Helper method to recursively remove a folder with some additional handling not provided by <see cref="Directory.Delete(string)"/>.
    /// </summary>
    /// <param name="target">The folder to remove (including all its content).</param>
    /// <returns><c>true</c> if removal of directory and all its contents was successful, <c>false</c> otherwise.</returns>
    public static bool DeleteDirectory(string target)
    {
      if (string.IsNullOrEmpty(target)
          || !Directory.Exists(target))
      {
        return false;
      }

      bool success = true;
      string[] files = Directory.GetFiles(target);
      string[] dirs = Directory.GetDirectories(target);

      foreach (string file in files)
      {
        // In the case a file is set to read-only we would get an IO exception.
        try
        {
          File.SetAttributes(file, FileAttributes.Normal);
          File.Delete(file);
        }
        catch
        {
          // Cannot delete files in use.  Continue.
          success = false;
        }
      }

      success = dirs.Aggregate(success, (current, dir) => current && DeleteDirectory(dir));
      File.SetAttributes(target, FileAttributes.Normal);
      try
      {
        Directory.Delete(target, false);
      }
      catch (DirectoryNotFoundException)
      {
        // Ignore. Folder might be removed outside of the application already.
      }
      catch (IOException)
      {
        // Usually thrown if the folder is blocked.
        if (Directory.GetCurrentDirectory().Equals(target, StringComparison.InvariantCultureIgnoreCase))
        {
          Directory.SetCurrentDirectory("..");
        }

        Thread.Sleep(0);
        try
        {
          Directory.Delete(target, false);
        }
        catch (Exception)
        {
          // The directory is locked by the OS and can't be removed.  Likely this happens when the user has
          // the directory open in explorer.
          success = false;
        }
      }

      return success;
    }

    /// <summary>
    /// Attempts to delete the directory in the given path.
    /// </summary>
    /// <param name="directoryPath">The directory path.</param>
    /// <param name="retryCount">The number of times the operation must be retried if an error occurs.</param>
    /// <param name="retryWaitInMilliseconds">The time to wait between retries.</param>
    /// <param name="successWaitInMilliseconds">The time to wait if the deletion is successful (in case a creation of the same folder follows the deletion).</param>
    /// <returns><c>true</c> if the directory is deleted successfully, <c>false</c> otherwise.</returns>
    public static bool DeleteDirectory(string directoryPath, int retryCount, int retryWaitInMilliseconds = 100, int successWaitInMilliseconds = 0)
    {
      return DeleteDirectory(directoryPath, retryCount, retryWaitInMilliseconds, successWaitInMilliseconds, out _);
    }

    /// <summary>
    /// Attempts to delete the directory in the given path.
    /// </summary>
    /// <param name="directoryPath">The directory path.</param>
    /// <param name="retryCount">The number of times the operation must be retried if an error occurs.</param>
    /// <param name="retryWaitInMilliseconds">The time to wait between retries.</param>
    /// <param name="successWaitInMilliseconds">The time to wait if the deletion is successful (in case a creation of the same folder follows the deletion).</param>
    /// <param name="errorMessage">An error message if an error occurs.</param>
    /// <returns><c>true</c> if the directory is deleted successfully, <c>false</c> otherwise.</returns>
    public static bool DeleteDirectory(string directoryPath, int retryCount, int retryWaitInMilliseconds, int successWaitInMilliseconds, out string errorMessage)
    {
      errorMessage = null;
      if (string.IsNullOrEmpty(directoryPath))
      {
        // Nothing to delete+
        return true;
      }

      var currentRetry = 0;
      while (Directory.Exists(directoryPath)
             && currentRetry < retryCount)
      {
        try
        {
          Directory.Delete(directoryPath, true);
          errorMessage = null;
          if (successWaitInMilliseconds > 0)
          {
            Thread.Sleep(successWaitInMilliseconds);
          }

          break;
        }
        catch (Exception ex)
        {
          errorMessage = ex.Message;
          currentRetry++;
          if (retryWaitInMilliseconds > 0)
          {
            Thread.Sleep(retryWaitInMilliseconds);
          }
        }
      }

      return string.IsNullOrEmpty(errorMessage);
    }

    /// <summary>
    /// Attempts to delete the file in the given path.
    /// </summary>
    /// <param name="fullPath">The directory path.</param>
    /// <param name="retryCount">The number of times the operation must be retried if an error occurs.</param>
    /// <param name="retryWaitInMilliseconds">The time to wait between retries.</param>
    /// <param name="successWaitInMilliseconds">The time to wait if the deletion is successful (in case a creation of the same folder follows the deletion).</param>
    /// <returns><c>true</c> if the file is deleted successfully, <c>false</c> otherwise.</returns>
    public static bool DeleteFile(string fullPath, int retryCount = 3, int retryWaitInMilliseconds = 100, int successWaitInMilliseconds = 0)
    {
      return DeleteFile(fullPath, retryCount, retryWaitInMilliseconds, successWaitInMilliseconds, out _);
    }

    /// <summary>
    /// Attempts to delete the file in the given path.
    /// </summary>
    /// <param name="fullPath">The directory path.</param>
    /// <param name="retryCount">The number of times the operation must be retried if an error occurs.</param>
    /// <param name="retryWaitInMilliseconds">The time to wait between retries.</param>
    /// <param name="successWaitInMilliseconds">The time to wait if the deletion is successful (in case a creation of the same folder follows the deletion).</param>
    /// <param name="errorMessage">An error message if an error occurs.</param>
    /// <returns><c>true</c> if the file is deleted successfully, <c>false</c> otherwise.</returns>
    public static bool DeleteFile(string fullPath, int retryCount, int retryWaitInMilliseconds, int successWaitInMilliseconds, out string errorMessage)
    {
      errorMessage = null;
      if (string.IsNullOrEmpty(fullPath))
      {
        // Nothing to delete
        return true;
      }

      var currentRetry = 0;
      while (File.Exists(fullPath)
             && currentRetry < retryCount)
      {
        try
        {
          File.Delete(fullPath);
          errorMessage = null;
          if (successWaitInMilliseconds > 0)
          {
            Thread.Sleep(successWaitInMilliseconds);
          }

          break;
        }
        catch (Exception ex)
        {
          errorMessage = ex.Message;
          currentRetry++;
          if (retryWaitInMilliseconds > 0)
          {
            Thread.Sleep(retryWaitInMilliseconds);
          }
        }
      }

      return string.IsNullOrEmpty(errorMessage);
    }

    public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
    {
      // Get the subdirectories for the specified directory.
      DirectoryInfo dir = new DirectoryInfo(sourceDirName);
      DirectoryInfo[] dirs = dir.GetDirectories();

      if (!dir.Exists)
      {
        throw new DirectoryNotFoundException(
            "Source directory does not exist or could not be found: "
            + sourceDirName);
      }

      // If the destination directory doesn't exist, create it. 
      if (!Directory.Exists(destDirName))
      {
        Directory.CreateDirectory(destDirName);
      }

      // Get the files in the directory and copy them to the new location.
      FileInfo[] files = dir.GetFiles();
      foreach (FileInfo file in files)
      {
        string temppath = Path.Combine(destDirName, file.Name);
        file.CopyTo(temppath, true);
      }

      // If copying subdirectories, copy them and their contents to new location. 
      if (!copySubDirs)
      {
        return;
      }

      foreach (var subdir in dirs)
      {
        string temppath = Path.Combine(destDirName, subdir.Name);
        DirectoryCopy(subdir.FullName, temppath, true);
      }
    }

    public static bool Elevated()
    {
      // Determine if current user has administrative privilieges, otherwise functionality will fail.
      var currentUserId = WindowsIdentity.GetCurrent();
      var currentPrincial = new WindowsPrincipal(currentUserId);
      return currentPrincial.IsInRole(WindowsBuiltInRole.Administrator);
    }

    /// <summary>
    /// Gets a value indicating if the installation originates from an MSI.
    /// </summary>
    /// <returns><c>true</c> if the application executable originates from an MSI installation; otherwise, <c>true</c>.</returns>
    public static bool ExecutionIsFromMSI(Version version)
    {
      if (version == null)
      {
        return false;
      }

      var keyName = string.Format(REGISTRY_KEY_TEMPLATE, version.Major, version.Minor);
      RegistryKey key = null;
      try
      {
        // Get installation reg key.
        key = Registry.LocalMachine.OpenSubKey(keyName);
        if (key == null
            && Win32.Is64BitOs)
        {
          key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(keyName);
        }

        if (key == null)
        {
          return false;
        }

        // Get registry key install location.
        var installLocation = key?.GetValue<string>("Location");
        if (string.IsNullOrEmpty(installLocation))
        {
          return false;
        }

        // Verify install location exists and has files. It's been identified that the server reg key will sometimes
        // not be removed, hence it is better to also check for the path to exist.
        if (!Directory.Exists(installLocation)
            || Directory.GetFiles(installLocation).Length == 0)
        {
          return false;
        }

        // Identify if exe is running from the MSI install location.
        string assemblyInstallLocation = null;
#if DEBUG
        assemblyInstallLocation = ConfigurationManager.AppSettings["installationDirectory"];
#else
        var assembly = Assembly.GetExecutingAssembly();
        var assemblyLocation = new DirectoryInfo(assembly.Location);
        if (assemblyLocation.Parent == null
            || assemblyLocation.Parent.Parent == null)
        {
          return false;
        }

        assemblyInstallLocation = assemblyLocation.Parent.Parent.FullName;
#endif

        var installLocationDirInfo = new DirectoryInfo(installLocation);
        var assemblyInstallLocationDirInfo = new DirectoryInfo(assemblyInstallLocation);
        return string.Compare(installLocationDirInfo.FullName.TrimEnd('\\'), assemblyInstallLocationDirInfo.FullName.TrimEnd('\\'), StringComparison.InvariantCultureIgnoreCase) == 0;
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
      }

      return false;
    }

    /// <summary>
    /// Unzips a MSI inside the given <see cref="zipFile"/> to the given location.
    /// </summary>
    /// <param name="zipFile">A ZIP file with full path.</param>
    /// <param name="targetDirectory">The directory where the MSI file is extracted to.</param>
    /// <returns>The full path of the extracted MSI or <c>null</c> if not extracted.</returns>
    public static string ExtractMsiAndDeleteArchive(string zipFile, string targetDirectory)
    {
      var zipEntryNames = GetZipArchiveEntryNames(zipFile, false);
      var msiZipEntryName = zipEntryNames?.FirstOrDefault(entryName => entryName.EndsWith("msi", StringComparison.OrdinalIgnoreCase));
      if (string.IsNullOrEmpty(msiZipEntryName))
      {
        // The downloaded zip file does not contain the MSI file we expect, so error out and cancel
        throw new Exception($"{Path.GetFileName(targetDirectory)} not found in the {zipFile} zip file");
      }

      // Attempt to extract the MSI file
      var extracted = UnZipEntry(zipFile, msiZipEntryName, null, targetDirectory, true);
      if (extracted)
      {
        File.Delete(zipFile);
      }

      return extracted
        ? Path.Combine(targetDirectory, msiZipEntryName)
        : null;
    }

    public static bool FindFile(string filename, string dir, bool recurse)
    {
      string[] files = Directory.GetFiles(dir, filename, SearchOption.AllDirectories);
      return files.Length > 0;
    }

    public static int FindNextAvailablePort(int basePort)
    {
      int port = basePort;
      var endPoints = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners();
      var portsInUse = endPoints.Select(endPoint => endPoint.Port).ToList();
      while (true)
      {
        if (!portsInUse.Contains(port))
        {
          return port;
        }

        port++;
      }
    }

    /// <summary>
    /// Creates a configuration file in a temporary location.
    /// </summary>
    /// <param name="contents">The contents of the configuration file..</param>
    /// <returns>The full file path of the copied configuration file.</returns>
    public static string CreateTempConfigurationFile(string contents)
    {
      var tempDirectory = Path.GetTempPath();
      var tempConfigFileName = $"{Guid.NewGuid().ToString("D")}.ini";
      var tempConfigFilePath = Path.Combine(tempDirectory, tempConfigFileName);
      try
      {
        File.WriteAllText(tempConfigFilePath, contents);
      }
      catch
      {
        return null;
      }

      return tempConfigFilePath;
    }

    /// <summary>
    /// Executes the given query, establishing a connection using the given connection string and returning a <see cref="DataRow"/>.
    /// </summary>
    /// <param name="connectionString">A connection string to establish a DB connection.</param>
    /// <param name="sqlQuery">The SQL query to execute.</param>
    /// <param name="showErrors">Flag indicating whether errors are shown to the users if any or just logged.</param>
    /// <returns>A <see cref="DataRow"/> if the query is successful, or <c>null</c> otherwise.</returns>
    public static DataRow ExecuteDataRow(string connectionString, string sqlQuery, bool showErrors = true)
    {
      if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(sqlQuery))
      {
        return null;
      }

      DataRow dataRow = null;
      try
      {
        dataRow = MySqlHelper.ExecuteDataRow(connectionString, sqlQuery);
      }
      catch (Exception ex)
      {
        Logger.LogException(ex, showErrors);
      }

      return dataRow;
    }

    /// <summary>
    /// Executes the given query, establishing a connection using the given connection string.
    /// </summary>
    /// <param name="connectionString">A connection string to establish a DB connection.</param>
    /// <param name="sqlQuery">The SQL query to execute.</param>
    /// <param name="showErrors">Flag indicating whether errors are shown to the users if any or just logged.</param>
    public static void ExecuteNonQuery(string connectionString, string sqlQuery, bool showErrors = true)
    {
      if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(sqlQuery))
      {
        return;
      }

      try
      {
        MySqlHelper.ExecuteNonQuery(connectionString, sqlQuery);
      }
      catch (Exception ex)
      {
        Logger.LogException(ex, showErrors);
      }
    }

    /// <summary>
    /// Executes the given query, establishing a connection using the given connection string and returning a single boxed value.
    /// </summary>
    /// <param name="connectionString">A connection string to establish a DB connection.</param>
    /// <param name="sqlQuery">The SQL query to execute.</param>
    /// <param name="showErrors">Flag indicating whether errors are shown to the users if any or just logged.</param>
    /// <returns>A single boxed value if the query is successful, or <c>null</c> otherwise.</returns>
    public static object ExecuteScalar(string connectionString, string sqlQuery, bool showErrors = true)
    {
      if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(sqlQuery))
      {
        return null;
      }

      object retValue = null;
      try
      {
        retValue = MySqlHelper.ExecuteScalar(connectionString, sqlQuery);
      }
      catch (Exception ex)
      {
        Logger.LogException(ex, showErrors);
      }

      return retValue;
    }

    /// <summary>
    /// Gets the active <see cref="Form"/> attached to the calling thread's message queue.
    /// </summary>
    /// <returns>The active <see cref="Form"/> attached to the calling thread's message queue.</returns>
    public static Form GetActiveForm()
    {
      var activeForm = Form.ActiveForm;
      if (activeForm != null)
      {
        return activeForm;
      }

      activeForm = Application.OpenForms.Count > 0
        ? Application.OpenForms[0]
        : null;
      if (activeForm != null)
      {
        return activeForm;
      }

      var handle = GetActiveWindow();
      if (handle == IntPtr.Zero)
      {
        handle = GetForegroundWindow();
      }

      var control = Control.FromHandle(handle);
      return control as Form;
    }

    /// <summary>
    /// Gets a list of all MySQL character sets along with their available collations.
    /// </summary>
    /// <param name="connectionString">A connection string to establish a DB connection.</param>
    /// <param name="firstElement">A custom string for the first element of the dictioary.</param>
    /// <returns>A list of all MySQL character sets along with their available collations.</returns>
    public static Dictionary<string, string[]> GetCollationsDictionary(string connectionString,
      string firstElement = null)
    {
      var charSetsTable = GetSchemaInformation(connectionString, SchemaInformationType.Collations, true);
      if (charSetsTable == null)
      {
        return null;
      }

      var rowsByGroup = charSetsTable.Select(string.Empty, "Charset, Collation").GroupBy(r => r["Charset"].ToString());
      var collationsDictionary = new Dictionary<string, string[]>(270);
      if (!string.IsNullOrEmpty(firstElement))
      {
        collationsDictionary.Add(firstElement, new[] { string.Empty, string.Empty });
      }

      foreach (var rowsGroup in rowsByGroup)
      {
        var charset = rowsGroup.Key;
        collationsDictionary.Add($"{charset} - {DEFAULT_COLLATION_TEXT}",
          new[] { charset, string.Empty });
        foreach (var collation in rowsGroup.Select(row => row["Collation"].ToString()))
        {
          collationsDictionary.Add($"{charset} - {collation}", new[] { charset, collation });
        }
      }

      return collationsDictionary;
    }

    /// <summary>
    /// Gets the default collation corresponding to the given character set name.
    /// </summary>
    /// <param name="connectionString">A connection string to establish a DB connection.</param>
    /// <param name="charSet">A character set name.</param>
    /// <returns>The default collation corresponding to the given character set name.</returns>
    public static string GetDefaultCollationFromCharSet(string connectionString, string charSet)
    {
      if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(charSet))
      {
        return null;
      }

      var infoTable = GetSchemaInformation(connectionString, SchemaInformationType.CharacterSets, true);
      var charSetRows = infoTable?.Select($"Charset = '{charSet}'");
      return charSetRows?.Length > 0 ? charSetRows[0]["Default collation"].ToString() : null;
    }

    public static int GetDPI()
    {
      using (var g = Graphics.FromHwnd(IntPtr.Zero))
      {
        IntPtr desktop = g.GetHdc();
        return GetDeviceCaps(desktop, (int)DeviceCap.LogPixelsX);
      }
    }

    /// <summary>
    /// Gets the character set and its collation used by the connected MySQL server.
    /// </summary>
    /// <param name="connectionString">A connection string to establish a DB connection.</param>
    /// <returns>The character set and its collation used by the connected MySQL server.</returns>
    public static Tuple<string, string> GetMySqlServerCharSetAndCollation(string connectionString)
    {
      if (string.IsNullOrEmpty(connectionString))
      {
        return null;
      }

      const string SQL = "SELECT @@character_set_server, @@collation_server";
      var dataRow = ExecuteDataRow(connectionString, SQL);
      return dataRow == null || dataRow.ItemArray.Length < 2
        ? null
        : new Tuple<string, string>(dataRow.ItemArray[0].ToString(), dataRow.ItemArray[1].ToString());
    }

    /// <summary>
    /// Gets the value of the DEFAULT_STORAGE_ENGINE MySQL Server variable indicating the default DB engine used for new table creations.
    /// </summary>
    /// <param name="connectionString">A connection string to establish a DB connection.</param>
    /// <returns>The default DB engine used for new table creations.</returns>
    public static string GetMySqlServerDefaultEngine(string connectionString)
    {
      if (string.IsNullOrEmpty(connectionString))
      {
        return string.Empty;
      }

      const string SQL = "SELECT @@default_storage_engine";
      object objEngine = ExecuteScalar(connectionString, SQL);
      return objEngine?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// Gets the value of the global SQL_MODE MySQL Server variable.
    /// </summary>
    /// <param name="connectionString">A connection string to establish a DB connection.</param>
    /// <returns>The value of the global SQL_MODE system variable.</returns>
    public static string GetMySqlServerGlobalMode(string connectionString)
    {
      if (string.IsNullOrEmpty(connectionString))
      {
        return string.Empty;
      }

      const string SQL = "SELECT @@GLOBAL.sql_mode";
      var objSqlMode = ExecuteScalar(connectionString, SQL);
      return objSqlMode?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// Gets the value of the LOWER_CASE_TABLE_NAMES MySQL Server variable indicating the case sensitivity that table names are stored and compared.
    /// </summary>
    /// <param name="connectionString">A connection string to establish a DB connection.</param>
    /// <returns><c>true</c> if table names are stored in lowercase on disk and comparisons are not case sensitive, <c>false</c> if table names are stored as specified and comparisons are case sensitive.</returns>
    public static bool GetMySqlServerLowerCaseTableNames(string connectionString)
    {
      if (string.IsNullOrEmpty(connectionString))
      {
        return false;
      }

      const string SQL = "SELECT @@lower_case_table_names";
      object objCaseSensitivity = ExecuteScalar(connectionString, SQL);
      return objCaseSensitivity != null && objCaseSensitivity.ToString().Equals("1");
    }

    /// <summary>
    /// Gets the value of the MAX_ALLOWED_PACKET MySQL Server variable indicating the max size in bytes of the packet returned by a single query.
    /// </summary>
    /// <param name="connectionString">A connection string to establish a DB connection.</param>
    /// <returns>The max size in bytes of the packet returned by a single query.</returns>
    public static int GetMySqlServerMaxAllowedPacket(string connectionString)
    {
      if (string.IsNullOrEmpty(connectionString))
      {
        return 0;
      }

      const string SQL = "SELECT @@max_allowed_packet";
      object objCount = MySqlHelper.ExecuteScalar(connectionString, SQL);
      return objCount != null ? Convert.ToInt32(objCount) : 0;
    }

    /// <summary>
    /// Gets the version of the connected MySQL server.
    /// </summary>
    /// <param name="connectionString">A connection string to establish a DB connection.</param>
    /// <returns>The version of the connected MySQL server.</returns>
    public static string GetMySqlServerVersion(string connectionString)
    {
      if (string.IsNullOrEmpty(connectionString))
      {
        return string.Empty;
      }

      const string SQL = "SELECT @@version";
      object version = MySqlHelper.ExecuteScalar(connectionString, SQL);
      return version?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// Returns a <see cref="DataTable"/> with information for MySQL routines (stored procedures).
    /// </summary>
    /// <remarks>This is a temporary solution while C/NET fixes the problem retrieving stored procedures data in Server versions >= 8.0.</remarks>
    /// <param name="connection">The <see cref="MySqlConnection"/> used to query the database.</param>
    /// <param name="restrictions">Specific parameters that vary among database collections.</param>
    /// <returns>A <see cref="DataTable"/> with information for MySQL routines (stored procedures).</returns>
    private static DataTable GetRoutines(MySqlConnection connection, string[] restrictions)
    {
      string[] keys = new string[4];
      keys[0] = "ROUTINE_CATALOG";
      keys[1] = "ROUTINE_SCHEMA";
      keys[2] = "ROUTINE_NAME";
      keys[3] = "ROUTINE_TYPE";

      var query = new StringBuilder("SELECT * FROM INFORMATION_SCHEMA.ROUTINES");
      string whereClause = GetWhereClause(null, keys, restrictions);
      if (!string.IsNullOrEmpty(whereClause))
      {
        query.Append(" WHERE ");
        query.Append(whereClause);
      }

      var dt = GetTableFromQuery(connection, query.ToString());
      if (dt != null)
      {
        dt.TableName = "Routines";
      }

      return dt;
    }

    /// <summary>
    /// Gets the character set and its collation used by the currently selected schema.
    /// </summary>
    /// <param name="connectionString">A connection string to establish a DB connection.</param>
    /// <param name="schemaName">The name of a database schema where the table resides.</param>
    /// <returns>The character set and its collation used by the currently selected schema.</returns>
    public static Tuple<string, string> GetSchemaCharSetAndCollation(string connectionString, string schemaName)
    {
      if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(schemaName))
      {
        return null;
      }

      string sql = $"SHOW CREATE SCHEMA `{schemaName}`";
      var dataRow = ExecuteDataRow(connectionString, sql);
      if (dataRow == null || dataRow.ItemArray.Length < 2)
      {
        return null;
      }

      var createStatement = dataRow.ItemArray[1].ToString();
      if (string.IsNullOrEmpty(createStatement))
      {
        return null;
      }

      int charSetOptionIndex = createStatement.IndexOf("character set ", StringComparison.InvariantCultureIgnoreCase);
      if (charSetOptionIndex < 0)
      {
        return null;
      }

      string schemaCollation;
      int charSetIndex = charSetOptionIndex + 14;
      int spaceAfterCharSetIndex = createStatement.IndexOf(" ", charSetIndex,
        StringComparison.InvariantCultureIgnoreCase);
      var schemaCharSet = createStatement.Substring(charSetIndex, spaceAfterCharSetIndex - charSetIndex).Trim();
      int collationOptionIndex = createStatement.IndexOf("collate ", spaceAfterCharSetIndex,
        StringComparison.InvariantCultureIgnoreCase);
      if (collationOptionIndex < 0)
      {
        schemaCollation = GetDefaultCollationFromCharSet(connectionString, schemaCharSet);
      }
      else
      {
        int collationIndex = charSetOptionIndex + 8;
        int spaceAfterCollationIndex = createStatement.IndexOf(" ", collationIndex,
          StringComparison.InvariantCultureIgnoreCase);
        schemaCollation = createStatement.Substring(collationIndex, spaceAfterCollationIndex - collationIndex).Trim();
      }

      return new Tuple<string, string>(schemaCharSet, schemaCollation);
    }

    /// <summary>
    /// Gets the schema information ofr the given database collection.
    /// </summary>
    /// <param name="connectionString">A connection string to establish a DB connection.</param>
    /// <param name="schemaInformation">The type of schema information to query.</param>
    /// <param name="showErrors">Flag indicating whether errors are shown to the users if any or just logged.</param>
    /// <param name="restrictions">Specific parameters that vary among database collections.</param>
    /// <returns>Schema information within a data table.</returns>
    public static DataTable GetSchemaInformation(string connectionString, SchemaInformationType schemaInformation, bool showErrors, params string[] restrictions)
    {
      if (string.IsNullOrEmpty(connectionString))
      {
        return null;
      }

      DataTable dt = null;
      try
      {
        using (var baseConnection = new MySqlConnection(connectionString))
        {
          baseConnection.Open();
          MySqlDataAdapter mysqlAdapter;
          switch (schemaInformation)
          {
            case SchemaInformationType.ColumnsSimple:
              mysqlAdapter =
                new MySqlDataAdapter($"SHOW COLUMNS FROM `{restrictions[1]}`.`{restrictions[2]}`",
                  baseConnection);
              dt = new DataTable();
              mysqlAdapter.Fill(dt);
              break;

            case SchemaInformationType.Engines:
              mysqlAdapter = new MySqlDataAdapter("SELECT * FROM information_schema.engines ORDER BY engine",
                baseConnection);
              dt = new DataTable();
              mysqlAdapter.Fill(dt);
              break;

            case SchemaInformationType.Collations:
              string queryString;
              if (restrictions != null && restrictions.Length > 0 && !string.IsNullOrEmpty(restrictions[0]))
              {
                queryString = $"SHOW COLLATION WHERE charset = '{restrictions[0]}'";
              }
              else
              {
                queryString = "SHOW COLLATION";
              }

              mysqlAdapter = new MySqlDataAdapter(queryString, baseConnection);
              dt = new DataTable();
              mysqlAdapter.Fill(dt);
              break;

            case SchemaInformationType.CharacterSets:
              mysqlAdapter = new MySqlDataAdapter("SHOW CHARSET", baseConnection);
              dt = new DataTable();
              mysqlAdapter.Fill(dt);
              break;

            case SchemaInformationType.Routines:
              dt = GetRoutines(baseConnection, restrictions);
              break;

            default:
              dt = baseConnection.GetSchema(schemaInformation.ToCollection(), restrictions);
              break;
          }
        }
      }
      catch (Exception ex)
      {
        Logger.LogException(ex, showErrors);
        if (!showErrors)
        {
          throw;
        }
      }

      return dt;
    }

    /// <summary>
    /// Computes a SHA1 hash for the specified file.
    /// </summary>
    /// <param name="pathName">The file for which the hash will be computed.</param>
    /// <returns>A string representing the computed hash if the operation was successful, <c>null</c> if any error occurred.</returns>
    public static string GetSha1Hash(string pathName)
    {
      if (string.IsNullOrEmpty(pathName))
      {
        return null;
      }

      var hasher = new SHA1CryptoServiceProvider();
      try
      {
        Stream stream = new FileStream(pathName, FileMode.Open);
        byte[] hash = hasher.ComputeHash(stream);
        stream.Close();
        string stringHash = BitConverter.ToString(hash);
        stringHash = stringHash.Replace("-", "");
        return stringHash;
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
        return null;
      }
      finally
      {
        hasher.Dispose();
      }
    }

    /// <summary>
    /// Computes an SHA256 hash for the specified file.
    /// </summary>
    /// <param name="pathName">The file for which the hash will be computed.</param>
    /// <returns>A string representing the computed hash if the operation was successful, <c>null</c> if any error occurred.</returns>
    public static string GetSha256Hash(string pathName)
    {
      if (string.IsNullOrEmpty(pathName))
      {
        return null;
      }

      try
      {
        using (SHA256 mySHA256 = SHA256.Create())
        {
          var fInfo = new FileInfo(pathName);
          using (FileStream fileStream = fInfo.Open(FileMode.Open))
          {
            byte[] hashValue = mySHA256.ComputeHash(fileStream);
            return BitConverter.ToString(hashValue).Replace("-", "").ToLower();
          }
        }
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
        return null;
      }
    }

    /// <summary>
    /// Computes an SHA256 hash for the specified file using the defined hashing key.
    /// </summary>
    /// <param name="pathName">The file for which the hash will be computed.</param>
    /// <returns>A string representing the computed hash if the operation was successful, <c>null</c> if any error occurred.</returns>
    public static string GetSha256HashUsingHashingKey(string pathName)
    {
      if (string.IsNullOrEmpty(pathName))
      {
        return null;
      }

      try
      {
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(HASHING_KEY)))
        {
          using (var stream = new FileStream(pathName, FileMode.Open))
          {
            var hmacBytes = hmac.ComputeHash(stream);
            return BitConverter.ToString(hmacBytes).Replace("-", "").ToLower();
          }
        }
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
        return null;
      }
    }

    /// <summary>
    /// Gets the collation defined on a MySQL table with the given name.
    /// </summary>
    /// <param name="connectionString">A connection string to establish a DB connection.</param>
    /// <param name="schemaName">The name of a database schema where the table resides.</param>
    /// <param name="tableName">The name of the database table.</param>
    /// <param name="charSet">The character set that belongs to the table collation.</param>
    /// <returns>The collation defined on a MySQL table with the given name.</returns>
    public static string GetTableCollation(string connectionString, string schemaName, string tableName, out string charSet)
    {
      charSet = null;
      if (string.IsNullOrEmpty(connectionString)
          || string.IsNullOrEmpty(schemaName)
          || string.IsNullOrEmpty(tableName))
      {
        return null;
      }

      var infoTable = GetSchemaInformation(connectionString, SchemaInformationType.Tables, true, null, schemaName, tableName);
      var tableCollation = infoTable == null || infoTable.Rows.Count == 0 ? null : infoTable.Rows[0]["TABLE_COLLATION"].ToString();
      if (!string.IsNullOrEmpty(tableCollation))
      {
        charSet = tableCollation.Substring(0, tableCollation.IndexOf("_", StringComparison.InvariantCultureIgnoreCase));
      }

      return tableCollation;
    }

    /// <summary>
    /// Returns a <see cref="DataTable"/> filled with data using the given query.
    /// </summary>
    /// <param name="connection">The <see cref="MySqlConnection"/> used to query the database.</param>
    /// <param name="sql">A query.</param>
    /// <returns>A <see cref="DataTable"/> filled with data using the given query.</returns>
    private static DataTable GetTableFromQuery(MySqlConnection connection, string sql)
    {
      if (connection == null || string.IsNullOrEmpty(sql))
      {
        return null;
      }

      if (connection.State != ConnectionState.Open)
      {
        connection.Open();
      }

      var dt = new DataTable();
      var cmd = new MySqlCommand(sql, connection);
      var reader = cmd.ExecuteReader();

      // add columns
      for (int i = 0; i < reader.FieldCount; i++)
      {
        var columnName = reader.GetName(i);
        var columnType = reader.GetFieldType(i);
        dt.Columns.Add(columnName, columnType ?? typeof(string));
      }

      using (reader)
      {
        while (reader.Read())
        {
          var row = dt.NewRow();
          for (int i = 0; i < reader.FieldCount; i++)
          {
            row[i] = reader.GetValue(i);
          }

          dt.Rows.Add(row);
        }
      }

      return dt;
    }

    /// <summary>
    /// Gets the IP address corresponding to the given host name.
    /// </summary>
    /// <param name="hostName">A qualified host name.</param>
    /// <returns>The IP address corresponding to the given host name.</returns>
    public static string GetBaseIPv4ForHostName(string hostName)
    {
      if (string.IsNullOrEmpty(hostName))
      {
        return null;
      }

      string host = hostName == "."
        ? "localhost"
        : hostName;
      try
      {
        var ip = Dns.GetHostEntry(host);
        foreach (var ipAddress in ip.AddressList.Where(addr => addr.AddressFamily == AddressFamily.InterNetwork))
        {
          return ipAddress.ToString();
        }
      }
      catch
      {
        return null;
      }

      return null;
    }

    /// <summary>
    /// Gets the IPv4 if the provided host name is not an IPV4 address.
    /// </summary>
    /// <param name="hostName">The address or host name to validate.</param>
    /// <returns>An IPv4 address.</returns>
    public static string GetIPv4ForHostName(string hostName)
    {
      // TODO: Remove this method when bug #30174253 on Connector/NET has been fixed.
      if (string.IsNullOrEmpty(hostName))
      {
        return null;
      }

      return Uri.CheckHostName(hostName) != UriHostNameType.IPv4
               ? GetBaseIPv4ForHostName(hostName)
               : hostName;
    }

    /// <summary>
    /// Gets the install location for a product with the given display name.
    /// </summary>
    /// <param name="displayName">The display name of the product.</param>
    /// <returns>The install location for a product with the given display name.</returns>
    public static string GetProductInstallLocation(string displayName)
    {
      var userInstalledLocation = RegistryHive.CurrentUser.GetProductInstallLocation(displayName);
      return !string.IsNullOrEmpty(userInstalledLocation)
             ? userInstalledLocation
             : RegistryHive.LocalMachine.GetProductInstallLocation(displayName);
    }

    /// <summary>
    /// Gets the install location for a product with the given display name.
    /// </summary>
    /// <param name="registryHive">The <see cref="RegistryHive"/> where the registry search is started from.</param>
    /// <param name="displayName">The display name of the product.</param>
    /// <returns>The install location for a product with the given display name.</returns>
    private static string GetProductInstallLocation(this RegistryHive registryHive, string displayName)
    {
      string installLocation;
      if (Environment.Is64BitOperatingSystem)
      {
        installLocation = GetRegistrySubKeyAttributeValue(UNINSTALL_REGISTRY_KEY_NAME, registryHive, RegistryView.Registry64, "DisplayName", displayName, "InstallLocation");
        if (!string.IsNullOrEmpty(installLocation))
        {
          return installLocation;
        }

        installLocation = GetRegistrySubKeyAttributeValue(UNINSTALL_REGISTRY_KEY_NAME, registryHive, RegistryView.Registry32, "DisplayName", displayName, "InstallLocation");
        if (!string.IsNullOrEmpty(installLocation))
        {
          return installLocation;
        }
      }
      else
      {
        installLocation = GetRegistrySubKeyAttributeValue(UNINSTALL_REGISTRY_KEY_NAME, registryHive, RegistryView.Default, "DisplayName", displayName, "InstallLocation");
        if (!string.IsNullOrEmpty(installLocation))
        {
          return installLocation;
        }
      }

      return null;
    }

    /// <summary>
    /// Gets the product version of an assembly with the given path and name.
    /// </summary>
    /// <param name="assemblyFilePath">The file path of a DLL or EXE file.</param>
    /// <returns>The product version of an assembly with the given path and name.</returns>
    public static Version GetProductVersion(string assemblyFilePath)
    {
      if (string.IsNullOrEmpty(assemblyFilePath)
          || !File.Exists(assemblyFilePath))
      {
        return null;
      }

      try
      {
        var asmName = AssemblyName.GetAssemblyName(assemblyFilePath);
        return asmName?.Version;
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
      }

      return null;
    }

    /// <summary>
    /// Gets the value of a given Windows Registry sub-key attribute, that matches another attribute's name and value.
    /// </summary>
    /// <param name="subKeyName">The registry sub key name.</param>
    /// <param name="registryHive">The <see cref="RegistryHive"/> where the sub key is retrieved from.</param>
    /// <param name="registryView">The <see cref="RegistryView"/> to look the registry key in.</param>
    /// <param name="matchingAttributeName">The name of the attribute within the sub key to match with the given value.</param>
    /// <param name="matchingAttributeValue">The value to look for in the <seealso cref="matchingAttributeName"/>.</param>
    /// <param name="targetAttributeName">The name of the attribute we want to get the value from.</param>
    /// <returns>The value of a given Windows Registry sub-key attribute, that matches another attribute's name and value.</returns>
    public static string GetRegistrySubKeyAttributeValue(string subKeyName, RegistryHive registryHive, RegistryView registryView, string matchingAttributeName, string matchingAttributeValue, string targetAttributeName)
    {
      if (string.IsNullOrWhiteSpace(subKeyName)
          || string.IsNullOrWhiteSpace(matchingAttributeName)
          || string.IsNullOrWhiteSpace(matchingAttributeValue)
          || string.IsNullOrWhiteSpace(targetAttributeName))
      {
        return null;
      }

      using (var key = registryHive.OpenRegistryKey(registryView, subKeyName))
      {
        if (key == null)
        {
          return null;
        }

        foreach (string keyName in key.GetSubKeyNames())
        {
          RegistryKey subKey;
          using (subKey = key.OpenRegistrySubKey(keyName))
          {
            if (subKey == null
                || !subKey.GetValueNames().Any(name => string.Equals(name, matchingAttributeName, StringComparison.OrdinalIgnoreCase))
                || !subKey.GetValueNames().Any(name => string.Equals(name, targetAttributeName, StringComparison.OrdinalIgnoreCase)))
            {
              continue;
            }

            var attributeValue = subKey.GetValue(matchingAttributeName).ToString();
            if (attributeValue.Contains(matchingAttributeValue, StringComparison.OrdinalIgnoreCase))
            {
              return subKey.GetValue(targetAttributeName).ToString();
            }
          }
        }
      }

      return null;
    }

    /// <summary>
    /// Gets the first found process currently running with a given full or partial name and an executable file path.
    /// </summary>
    /// <param name="processPath">The path of the executable running as a process.</param>
    /// <param name="processName">The full or partial name of the process.</param>
    /// <returns>The first found process currently running with a given full or partial name and an executable file path.</returns>
    public static Process GetRunningProcess(string processPath, string processName)
    {
      return GetRunningProcessses(processPath, processName).FirstOrDefault();
    }

    /// <summary>
    /// Gets a process currently running with the given process ID.
    /// </summary>
    /// <param name="processId">The process ID.</param>
    /// <returns>A process currently running with the given process ID.</returns>
    public static Process GetRunningProcess(int processId)
    {
      return Process.GetProcesses().FirstOrDefault(p => p.Id == processId);
    }

    /// <summary>
    /// Gets an iterable enumerator of processes currently running with a given full or partial name and an executable file path.
    /// </summary>
    /// <param name="processName">The full or partial name of the process.</param>
    /// <returns>An iterable enumerator of processes currently running with a given full or partial name and an executable file path.</returns>
    public static IEnumerable<Process> GetRunningProcessses(string processName)
    {
      var lowerProcessName = processName.ToLowerInvariant();
      return
        Process.GetProcesses()
          .Where(
            p =>
              p.ProcessName.ToLowerInvariant().Contains(lowerProcessName));
    }

    /// <summary>
    /// Gets an iterable enumerator of processes currently running with a given full or partial name and an executable file path.
    /// </summary>
    /// <param name="processPath">The path of the executable running as a process.</param>
    /// <param name="processName">The full or partial name of the process.</param>
    /// <returns>An iterable enumerator of processes currently running with a given full or partial name and an executable file path.</returns>
    public static IEnumerable<Process> GetRunningProcessses(string processPath, string processName)
    {
      return GetRunningProcessses(processName).Where(p => p.MainModule.FileName.ToLowerInvariant().Equals(processPath, StringComparison.InvariantCultureIgnoreCase));
    }

    /// <summary>
    /// Gets the Windows process ID of a specific Server instance.
    /// </summary>
    /// <param name="dataDirectory">The path to the data directory of the Server instance.</param>
    /// <returns>The Windows process ID of a specific Server instance., or <c>0</c> if it cannot be found.</returns>
    public static int GetServerInstanceProcessId(string dataDirectory)
    {
      if (string.IsNullOrEmpty(dataDirectory) || !Directory.Exists(dataDirectory))
      {
        return 0;
      }

      var pidFiles = Directory.GetFiles(dataDirectory, "*.pid", SearchOption.AllDirectories);
      if (pidFiles.Length == 0)
      {
        return 0;
      }

      var pidFile = pidFiles.Length == 1
        ? pidFiles[0]
        : pidFiles.OrderByDescending(fp => new FileInfo(fp).LastWriteTime).FirstOrDefault();
      if (string.IsNullOrEmpty(pidFile))
      {
        return 0;
      }

      var readProcessIdText = File.ReadAllText(pidFile).Trim();
      int instanceProcessId;
      if (string.IsNullOrEmpty(readProcessIdText) || !int.TryParse(readProcessIdText, out instanceProcessId))
      {
        return 0;
      }

      return instanceProcessId;
    }

    /// <summary>
    /// Determines the version number of a given file in the OS' system folder.
    /// Returns a 0 version if the file doesn't exist.
    /// </summary>
    public static Version GetSystemFileVersion(string file)
    {
      string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), file);
      if (!File.Exists(path))
      {
        return new Version();
      }

      FileVersionInfo info = FileVersionInfo.GetVersionInfo(path);
      return new Version(info.FileVersion);
    }

    /// <summary>
    /// Assembles a WHERE clause from given arrays of column names and their corresponding values.
    /// </summary>
    /// <param name="initialWhere">Initial where clause.</param>
    /// <param name="columnNames">An array of column names.</param>
    /// <param name="columnValues">An array of values.</param>
    /// <returns>A WHERE clause.</returns>
    private static string GetWhereClause(string initialWhere, string[] columnNames, string[] columnValues)
    {
      if (columnNames == null || columnValues == null)
      {
        return string.Empty;
      }

      var where = new StringBuilder(initialWhere);
      for (int i = 0; i < columnNames.Length; i++)
      {
        if (i >= columnValues.Length)
        {
          break;
        }

        if (string.IsNullOrEmpty(columnValues[i]))
        {
          continue;
        }

        if (where.Length > 0)
        {
          where.Append(" AND ");
        }

        where.Append(columnNames[i]);
        where.Append(" LIKE '");
        where.Append(columnValues[i]);
        where.Append("'");
      }

      return where.ToString();
    }

    /// <summary>
    /// Gets the names of the entries contained in a ZIP archive.
    /// </summary>
    /// <param name="zipFile">The ZIP file full path.</param>
    /// <param name="fullNames">Flag indicating whether the full names (with relative path) are returned or just the simple file names.</param>
    /// <returns></returns>
    public static List<string> GetZipArchiveEntryNames(string zipFile, bool fullNames)
    {
      if (string.IsNullOrEmpty(zipFile)
          || !File.Exists(zipFile))
      {
        return null;
      }

      List<string> entries = null;
      try
      {
        using (var archive = ZipFile.OpenRead(zipFile))
        {
          entries = archive?.Entries.Select(zipArchiveEntry => fullNames ? zipArchiveEntry.FullName : zipArchiveEntry.Name).ToList();
        }
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
      }


      return entries;
    }

    public static void GhostBitmap(Bitmap b, int level)
    {
      for (int y = 0; y < b.Height; y++)
        for (int x = 0; x < b.Width; x++)
        {
          Color c = b.GetPixel(x, y);
          int red = c.R + (255 - c.R) * level / 10;
          int green = c.G + (255 - c.G) * level / 10;
          int blue = c.B + (255 - c.B) * level / 10;
          b.SetPixel(x, y, Color.FromArgb(red, green, blue));
        }
    }

    public static void GhostBitmapWithAlpha(Bitmap b, int level)
    {
      //Bitmap d = new Bitmap(c.Width, c.Height);

      for (int i = 0; i < b.Width; i++)
      {
        for (int x = 0; x < b.Height; x++)
        {
          Color oc = b.GetPixel(i, x);
          int grayScale = (int)((oc.R * 0.3) + (oc.G * 0.59) + (oc.B * 0.11));
          Color nc = Color.FromArgb(oc.A, grayScale, grayScale, grayScale);
          b.SetPixel(i, x, nc);
        }
      }
    }

    /// <summary>
    /// Gets a value indicating whether a directory is empty or not.
    /// </summary>
    /// <param name="path">The path to the directory.</param>
    /// <returns><c>true</c> if the directory is empty, <c>false</c> otherwise.</returns>
    public static bool IsDirectoryEmpty(string path)
    {
      return !Directory.EnumerateFileSystemEntries(path).Any();
    }

    /// <summary>
    /// Checks if a process with the given name is installed.
    /// </summary>
    /// <param name="uninstallDisplayName">The display name of the application used for uninstalling it.</param>
    /// <returns><c>true</c> if a process with the given name is installed, <c>false</c> otherwise.</returns>
    public static bool IsProductInstalled(string uninstallDisplayName)
    {
      var productInstallLocation = GetProductInstallLocation(uninstallDisplayName);
      return !string.IsNullOrEmpty(productInstallLocation);
    }

    /// <summary>
    /// Initializes the <see cref="Logger"/> to be used with this product.
    /// </summary>
    /// <param name="consoleMode">Flag indicating whether the logging is done in console or UI mode.</param>
    /// <param name="logToConsole">Flag indicating whether the logging output goes to the console or to the Installer log.
    /// This argument is only relevant when <paramref name="consoleMode"/> is set to <c>true</c>.</param>
    public static void InitializeLogger(bool consoleMode, bool logToConsole = false)
    {
      Logger.Initialize(AppConfiguration.HomeDir, "configurator", consoleMode, logToConsole, "mysql-configurator", true);
      Logger.PrependUserNameToLogFileName = true;
    }

    /// <summary>
    /// Checks if a process with the given name is currently running.
    /// </summary>
    /// <param name="processName">The name of a process.</param>
    /// <returns><c>true</c> if a process with the given name is currently running, <c>false</c> otherwise.</returns>
    public static bool IsProcessRunning(string processName)
    {
      return !string.IsNullOrEmpty(processName)
             && Process.GetProcesses().Any(p => p.ProcessName.Contains(processName, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Gets a value indicating whether a process currently running with a given full or partial name and an executable file path exists.
    /// </summary>
    /// <param name="processPath">The path of the executable running as a process.</param>
    /// <param name="processName">The full or partial name of the process.</param>
    /// <returns><c>true</c> if a process currently running with a given full or partial name and an executable file path exists, <c>false</c> otherwise.</returns>
    public static bool IsProcessRunning(string processPath, string processName)
    {
      return GetRunningProcess(processPath, processName) != null;
    }

    /// <summary>
    /// Gets a value indicating whether a process currently running with the given process ID exists.
    /// </summary>
    /// <param name="processId">The process ID.</param>
    /// <returns><c>true</c> if a process currently running with the given process ID exists, <c>false</c> otherwise.</returns>
    public static bool IsProcessRunning(int processId)
    {
      return GetRunningProcess(processId) != null;
    }

    /// <summary>
    /// Checks if the given string representing an IP address is a valid one.
    /// </summary>
    /// <param name="ipAddressString">A string representing an IP address.</param>
    /// <returns><c>true</c> if the given string representing an IP address is a valid one, <c>false</c> otherwise.</returns>
    public static bool IsValidIpAddress(string ipAddressString)
    {
      return IPAddress.TryParse(ipAddressString, out _);
    }

    /// <summary>
    /// In order to make the recursive font scaling work we have to make all fonts
    /// non-ambient (sigh), that is, no control must use the font setting of its parent.
    /// The reason is that it cannot be determined from outside if a control uses an own
    /// or ambient property, so changing such properties can lead to multiple applications of
    /// the scaling.
    /// </summary>
    /// <param name="control"></param>
    public static void MakeFontsNonAmbient(Control control)
    {
      control.Font = new Font(control.Font.FontFamily, control.Font.Size, control.Font.Style, control.Font.Unit);
      foreach (Control child in control.Controls)
      {
        MakeFontsNonAmbient(child);
      }
    }

    public static void NormalizeFont(ContainerControl container)
    {
      int dpi = GetDPI();
      float ratio = 96F / dpi;
      NormalizeFontForControls(container.Controls, ratio);
    }

    /// <summary>
    /// Searches a string version, converting characters to number equivalents; a or A = 1 and z or Z = 26. When a
    /// character is encountered, that value is added to the next version section. 1c = 1.3 and 5.5.25a = 5.5.25.1
    /// </summary>
    /// <param name="version">A string version.</param>
    /// <returns>A Version representation of the input if possible, otherwise null.</returns>
    public static Version NormalVersion(string version)
    {
      string[] versionParts = version.Split('.');
      int[] versionPartNums = new int[4];

      int currentPart = 0;
      double increment = 0;
      foreach (string versionPart in versionParts)
      {
        Regex regex = new Regex(@"(?<number>\d+)(?<alpha>[a-zA-Z]?)");
        Match foundVersionPart = regex.Match(versionPart);
        versionPartNums[currentPart] = 0;
        if (foundVersionPart.Success)
        {
          int.TryParse(foundVersionPart.Groups["number"].Value, out versionPartNums[currentPart]);
          versionPartNums[currentPart] += Convert.ToInt32(increment);
          increment = 0;

          string alphaVersion = foundVersionPart.Groups["alpha"].Value;
          if ((alphaVersion != string.Empty) && (alphaVersion.Length == 1))
          {
            char alphaVersionChar;
            if (char.TryParse(alphaVersion, out alphaVersionChar))
            {
              increment = char.ToLower(alphaVersionChar).CompareTo('z') + 26;
            }
          }
        }

        currentPart += 1;
      }

      // default all remaining version parts to zero.
      for (int i = currentPart; i < versionPartNums.Length; i++)
      {
        versionPartNums[i] = 0;
      }

      // deal with any remaining increment.
      if (increment != 0)
      {
        if (currentPart < 5)
        {
          versionPartNums[currentPart] = Convert.ToInt32(increment);
        }
        else
        {
          versionPartNums[3] += 1;
        }
      }

      // deal with any negative numbers.
      for (int i = 3; i > 0; i--)
      {
        if (versionPartNums[i] >= 0)
        {
          continue;
        }

        versionPartNums[i - 1] += versionPartNums[i];
        versionPartNums[i] = 0;
      }

      if (versionPartNums[0] < 0)
      {
        versionPartNums[0] = 0;
      }

      return new Version(versionPartNums[0], versionPartNums[1], versionPartNums[2], versionPartNums[3]);
    }

    public static void OpenBrowser(string url)
    {
      try
      {
        var startInfo = new ProcessStartInfo("explorer.exe", url);
        Process.Start(startInfo);
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
      }
    }

    /// <summary>
    /// Returns the registry key inside CurrentUser or LocalMachine that matches the key name.
    /// </summary>
    /// <param name="keyName">The name of the key to open.</param>
    /// <param name="writable">Flag indicating if the registry key is opened with write access or read-only one.</param>
    /// <returns>The matching registry key.</returns>
    public static RegistryKey OpenRegistryKey(string keyName, bool writable = false)
    {
      return RegistryHive.CurrentUser.OpenRegistryKey(keyName, writable) ?? RegistryHive.LocalMachine.OpenRegistryKey(keyName, writable);
    }

    /// <summary>
    /// Opens the registry key that matches the key name in the specified registry hive.
    /// </summary>
    /// <param name="registryHive">The registry hive.</param>
    /// <param name="keyName">The name of the key to open.</param>
    /// <param name="writable">Flag indicating if the registry key is opened with write access or read-only one.</param>
    /// <returns>The matching registry key.</returns>
    public static RegistryKey OpenRegistryKey(this RegistryHive registryHive, string keyName, bool writable = false)
    {
      if (string.IsNullOrEmpty(keyName))
      {
        return null;
      }

      RegistryKey key;
      if (Environment.Is64BitOperatingSystem)
      {
        key = registryHive.OpenRegistryKey(RegistryView.Registry64, keyName, writable);
        if (key != null)
        {
          return key;
        }

        key = registryHive.OpenRegistryKey(RegistryView.Registry32, keyName, writable);
        if (key != null)
        {
          return key;
        }
      }
      else
      {
        key = registryHive.OpenRegistryKey(RegistryView.Default, keyName, writable);
        if (key != null)
        {
          return key;
        }
      }

      return null;
    }

    /// <summary>
    /// Opens the registry key that matches the key name in the specified registry hive.
    /// </summary>
    /// <param name="registryHive">The registry hive.</param>
    /// <param name="registryView">The <see cref="RegistryView"/> to look the registry key in.</param>
    /// <param name="keyName">The name of the key to open.</param>
    /// <param name="writable">Flag indicating if the registry key is opened with write access or read-only one.</param>
    /// <returns>The matching registry key.</returns>
    public static RegistryKey OpenRegistryKey(this RegistryHive registryHive, RegistryView registryView, string keyName, bool writable = false)
    {
      if (string.IsNullOrEmpty(keyName))
      {
        return null;
      }

      try
      {
        var key = RegistryKey.OpenBaseKey(registryHive, registryView).OpenSubKey(keyName, writable);
        if (key != null)
        {
          return key;
        }
      }
      catch (Exception e)
      {
        Logger.LogException(e);
      }

      return null;
    }

    /// <summary>
    /// Opens the registry key that matches the sub-key name in the specified <see cref="RegistryKey"/>.
    /// </summary>
    /// <param name="parentKey">A <see cref="RegistryKey"/> where the search starts in.</param>
    /// <param name="subKeyName">The name of the child key to open.</param>
    /// <param name="writable">Flag indicating if the registry key is opened with write access or read-only one.</param>
    /// <returns>The matching registry key.</returns>
    public static RegistryKey OpenRegistrySubKey(this RegistryKey parentKey, string subKeyName, bool writable = false)
    {
      if (parentKey == null || string.IsNullOrEmpty(subKeyName))
      {
        return null;
      }

      try
      {
        return parentKey.OpenSubKey(subKeyName, writable);
      }
      catch (Exception e)
      {
        Logger.LogException(e);
      }

      return null;
    }
    public static bool PathAlreadyUsed(string path, List<string> paths)
    {
      if (paths == null
          || paths.Count == 0)
      {
        return false;
      }

      string fullPath = Path.GetFullPath(path);
      return paths.Select(Path.GetFullPath).Any(p => p == fullPath);
    }

    public static bool PipeExists(string name)
    {
      try
      {
        NamedPipeClientStream ncs = new NamedPipeClientStream(".", name, PipeDirection.InOut);
        using (ncs)
        {
          ncs.Connect(500);
          return true;
        }
      }
      catch (Exception)
      {
        return false;
      }
    }

    /// <summary>
    /// Like System.Console.ReadLine(), only with a mask.
    /// </summary>
    /// <param name="mask">a <c>char</c> representing your choice of console mask</param>
    /// <returns>the string the user typed in </returns>
    public static string ReadConsolePassword(char mask)
    {
      const string BACKSPACE_SPACE_BACKSPACE = "\b \b";
      const int ENTER = 13, BACKSP = 8, CTRLBACKSP = 127;
      int[] filtered = { 0, 27, 9, 10 /*, 32 space, if you care */ }; // const

      var pass = new Stack<char>();
      char chr;

      while ((chr = Console.ReadKey(true).KeyChar) != ENTER)
      {
        if (chr == BACKSP)
        {
          if (pass.Count > 0)
          {
            Console.Write(BACKSPACE_SPACE_BACKSPACE);
            pass.Pop();
          }
        }
        else if (chr == CTRLBACKSP)
        {
          while (pass.Count > 0)
          {
            Console.Write(BACKSPACE_SPACE_BACKSPACE);
            pass.Pop();
          }
        }
        else if (filtered.Count(x => chr == x) > 0) { }
        else
        {
          pass.Push(chr);
          Console.Write(mask);
        }
      }

      Console.WriteLine();
      return new string(pass.Reverse().ToArray());
    }

    /// <summary>
    /// Runs the NetShell in the background.
    /// </summary>
    /// <param name="arguments">The arguments passed to the NetShell.</param>
    /// <param name="standardOutput">The redirected standard output from the process.</param>
    /// <param name="standardError">The redirected standard error from the process.</param>
    /// <returns><c>true</c> if the NetShell process ran and completed without errors, <c>false</c> otherwise.</returns>
    public static bool RunNetShellProcess(string arguments, out string standardOutput, out string standardError)
    {
      standardOutput = null;
      standardError = null;
      bool success;
      try
      {
        var processStartInfo = new ProcessStartInfo("netsh.exe")
        {
          Arguments = arguments,
          RedirectStandardInput = true,
          RedirectStandardOutput = true,
          RedirectStandardError = true,
          UseShellExecute = false,
          CreateNoWindow = true,
          Verb = "runas"
        };
        using (var process = Process.Start(processStartInfo))
        {
          if (process == null)
          {
            return false;
          }

          process.WaitForExit();
          standardOutput = process.StandardOutput.ReadToEnd();
          standardError = process.StandardError.ReadToEnd();
          success = process.ExitCode == 0;
        }
      }
      catch (Exception ex)
      {
        success = false;
        if (standardError != null)
        {
          standardError += Environment.NewLine + ex.Message;
        }

        Logger.LogException(ex);
      }

      return success;
    }

    public static bool RunningOnConsole()
    {
      var p = Process.GetCurrentProcess();
      return p.ProcessName.ToLowerInvariant().Contains("console");
    }

    /// <summary>
    /// Checks whether your current code runs inside a System.Threading.Task
    /// </summary>
    /// <returns>True when code is running inside a task otherwise return false</returns>
    public static bool RunningOnTask()
    {
      int? taskId = Task.CurrentId;
      return taskId.HasValue;
    }

    /// <summary>
    /// Runs a process in the background.
    /// </summary>
    /// <param name="executableFilePath">The full path to the executable file to run.</param>
    /// <param name="arguments">The arguments passed to the executable file.</param>
    /// <param name="workingDirectory">The full path to the directory where the process must be started.</param>
    /// <param name="outputRedirectionDelegate">A delegate to redirect standard output messages to.</param>
    /// <param name="errorRedirectionDelegate">A delegate to redirect the error messages to.</param>
    /// <param name="waitForExit">Flag indicating whether to wait until the process exits before returning it or not.</param>
    /// <param name="hideProcessMessages">Avoids printing messages to the <seealso cref="outputRedirectionDelegate"/> with process information.</param>
    /// <param name="waitLongerWorkaround">A workaround needed for some rogue processes like MySQL Shell that when <seealso cref="waitForExit"/> is <c>true</c> it needs some time to quit.</param>
    /// <returns>A <seealso cref="ProcessResult"/> instance, or <c>null</c> if the process did not run at all.</returns>
    public static ProcessResult RunProcess(string executableFilePath, string arguments, string workingDirectory, Action<string> outputRedirectionDelegate, Action<string> errorRedirectionDelegate, bool waitForExit, bool hideProcessMessages = false, bool waitLongerWorkaround = false)
    {
      if (string.IsNullOrEmpty(executableFilePath)
          || !File.Exists(executableFilePath))
      {
        return null;
      }

      ProcessResult result = null;
      try
      {
        if (string.IsNullOrEmpty(workingDirectory)
            || !Directory.Exists(workingDirectory))
        {
          workingDirectory = Path.GetDirectoryName(executableFilePath);
        }

        var processStartInfo = new ProcessStartInfo(executableFilePath)
        {
          Arguments = arguments.Trim(),
          RedirectStandardInput = true,
          RedirectStandardOutput = true,
          RedirectStandardError = true,
          UseShellExecute = false,
          CreateNoWindow = true
        };

        // It might seems this validation is not needed, but Path.GetDirectoryName name above could still return null or empty string.
        if (!string.IsNullOrEmpty(workingDirectory))
        {
          processStartInfo.WorkingDirectory = workingDirectory;
        }

        if (!hideProcessMessages)
        {
          outputRedirectionDelegate?.Invoke(string.Format(Resources.StartingProcessDetailsText, processStartInfo.FileName, processStartInfo.Arguments));
        }

        var process = Process.Start(processStartInfo);
        if (process == null)
        {
          return null;
        }

        int pid = process.Id;
        var outputBuilder = new StringBuilder();
        var errorBuilder = new StringBuilder();
        var processName = process.ProcessName;
        process.OutputDataReceived += (sender, e) =>
        {
          outputRedirectionDelegate?.Invoke(e.Data);
          outputBuilder.AppendLine(e.Data);
        };
        process.ErrorDataReceived += (sender, e) =>
        {
          errorRedirectionDelegate?.Invoke(e.Data);
          errorBuilder.AppendLine(e.Data);
        };
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.StandardInput.Close();
        if (waitForExit)
        {
          if (waitLongerWorkaround)
          {
            while (!process.WaitForExit(3000))
            {
              if (GetRunningProcess(pid) != null)
              {
                // This is a dummy block, its intention is purely to act as a workaround for processes
                // that do not end gracefully when waiting for them indefinitely (like the MySQL Shell one).
              }
            }
          }
          else
          {
            // Wait indefinitely
            process.WaitForExit();
          }
        }

        result = new ProcessResult(
          process.HasExited ? null : process,
          process.HasExited ? (int?)process.ExitCode : null,
          outputBuilder.ToString(),
          errorBuilder.ToString());
        if (!hideProcessMessages)
        {
          outputRedirectionDelegate?.Invoke(!process.HasExited
          ? string.Format(Resources.StartedAndRunningProcessSuccessfullyText, processName, pid)
          : string.Format(Resources.StartedAndExitedProcessSuccessfullyText, processName, pid, process.ExitCode));
        }

        if (process.HasExited)
        {
          process.Dispose();
        }
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
        errorRedirectionDelegate?.Invoke(string.Format(Resources.StartedProcessErrorText, Path.GetFileNameWithoutExtension(executableFilePath), ex.Message));
      }

      return result;
    }

    /// <summary>
    /// Standardizes user\domain entries to use double backslashes as required by MySQL.
    /// </summary>
    /// <param name="entry">The raw user entry value.</param>
    /// <returns>A standardized entry value.</returns>
    public static string StandardizeToMySqlUserDomainSyntax(string entry)
    {
      if (string.IsNullOrEmpty(entry))
      {
        // Nothing to standardize
        return entry;
      }

      const string DOUBLE_BACKSLASH = "\\\\";
      if (!entry.Contains(DOUBLE_BACKSLASH))
      {
        entry = entry.Replace("\\", DOUBLE_BACKSLASH);
      }

      return entry;
    }

    public static void SaveObjectToXml<T>(T o, string file)
    {
      try
      {
        var serializer = new XmlSerializer(typeof(T));
        TextWriter writer = new StreamWriter(file);
        serializer.Serialize(writer, o);
        writer.Close();
      }
      catch (Exception ex)
      {
        Logger.LogError($"Error saving object to XML file {file}. The error was: {ex}");
        throw;
      }
    }

    /// <summary>
    /// Helper method to scale all font sizes for the given control and its children to match
    /// the size at a 96 dpi screen resolution. Needed to avoid text overflow for fixed-sized layouts.
    /// To avoid unnecessary recreation of fonts call this method only if the current resolution is not 96 DPI.
    /// XXX: this should really only be a temporary solution. Make the layout auto-sizing instead!
    /// </summary>
    /// <param name="control"></param>
    public static void ScaleControlFont(Control control)
    {
      // Some controls don't seem to automatically scale fonts. Don't scale them either here
      // or we end up with unreadable text.
      if (control is TextBoxBase || control is TabControl)
      {
        return;
      }

      control.Font = new Font(control.Font.FontFamily, ScaleTo(control.Font.Size, CurrentDPI), control.Font.Style, control.Font.Unit);
      foreach (Control child in control.Controls)
      {
        ScaleControlFont(child);
      }
    }

    /// <summary>
    /// Scales the given size (usually a font size) to match a 96 dpi resolution.
    /// This is necessary to compensate font size changes if a desktop runs at a different resolution.
    /// </summary>
    public static float ScaleTo(float originalSize, float dpi)
    {
      return originalSize * 96 / dpi;
    }

    /// <summary>
    /// Sets the net_write_timeout and net_read_timeout MySQL server variables to the given value for the duration of the current client session.
    /// </summary>
    /// <param name="connectionString">A connection string to establish a DB connection.</param>
    /// <param name="timeoutInSeconds">The number of seconds to wait for more data from a connection before aborting the read or for a block to be written to a connection before aborting the write.</param>
    public static void SetClientSessionReadWriteTimeouts(string connectionString, uint timeoutInSeconds)
    {
      if (string.IsNullOrEmpty(connectionString) || timeoutInSeconds < 1)
      {
        return;
      }

      string sql = string.Format("SET SESSION net_write_timeout = {0}, SESSION net_read_timeout = {0}", timeoutInSeconds);
      ExecuteNonQuery(connectionString, sql);
    }

    public static void SetFont(Control container, Font f)
    {
      var controls = FindAllControls(container);
      foreach (var c in controls)
      {
        c.Font = f;
      }
    }

    /// <summary>
    /// Shows a message to users on the GUI or CLI.
    /// </summary>
    /// <param name="title">The title used on the GUI window.</param>
    /// <param name="message">The message displayed on both the GUI and CLI.</param>
    /// <param name="infoType">The <see cref="InfoDialog.InfoType"/> used for the GUI <see cref="InfoDialog"/>.</param>
    public static void ShowMessage(string title, string message, InfoDialog.InfoType infoType = InfoDialog.InfoType.Info)
    {
      if (!RunningOnConsole())
      {
        Console.WriteLine(message);
      }
      else
      {
        InfoDialog.ShowDialog(InfoDialogProperties.GetOkDialogProperties(infoType, title, message));
      }
    }

    /// <summary>
    /// Splits the given string containing command line arguments into an array of arguments.
    /// </summary>
    /// <param name="commandLineText">A string containing command line arguments.</param>
    /// <returns>An array of arguments.</returns>
    public static string[] SplitArgs(string commandLineText)
    {
      IntPtr ptrToSplitArgs = CommandLineToArgvW(commandLineText, out var numberOfArgs);

      // CommandLineToArgvW returns NULL upon failure.
      if (ptrToSplitArgs == IntPtr.Zero)
      {
        throw new ArgumentException("Unable to split argument.", new Win32Exception());
      }

      // Make sure the memory ptrToSplitArgs to is freed, even upon failure.
      try
      {
        string[] splitArgs = new string[numberOfArgs];

        // ptrToSplitArgs is an array of pointers to null terminated Unicode strings.
        // Copy each of these strings into our split argument array.
        for (int i = 0; i < numberOfArgs; i++)
        {
          splitArgs[i] = Marshal.PtrToStringUni(Marshal.ReadIntPtr(ptrToSplitArgs, i * IntPtr.Size));
        }

        return splitArgs;
      }
      finally
      {
        // Free memory obtained by CommandLineToArgW.
        LocalFree(ptrToSplitArgs);
      }
    }

    /// <summary>
    /// Sets up and runs a watcher to monitor changes in a file or directory.
    /// </summary>
    /// <param name="filePath">The file (with a full path) to monitor, if a directory is to be monitored the last character must be a backslash.</param>
    /// <param name="method">The delegate method to associate with a change, deletion or creation.</param>
    /// <returns>A <see cref="FileSystemWatcher"/> object that monitors the file or directory.</returns>
    public static FileSystemWatcher StartWatcherForFile(string filePath, FileSystemEventHandler method)
    {
      if (string.IsNullOrEmpty(filePath))
      {
        return null;
      }

      var isDirectory = filePath.EndsWith(@"\");
      var monitorPath = Path.GetDirectoryName(isDirectory ? filePath.Substring(0, filePath.Length - 1) : filePath);
      if (string.IsNullOrEmpty(monitorPath))
      {
        return null;
      }

      var watcher = new FileSystemWatcher
      {
        Path = monitorPath,
        Filter = isDirectory ? "*.*" : Path.GetFileName(filePath),
        IncludeSubdirectories = isDirectory,
        NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName | NotifyFilters.DirectoryName
      };

      watcher.Changed += method;
      watcher.Deleted += method;
      watcher.Created += method;
      watcher.EnableRaisingEvents = true;
      return watcher;
    }

    /// <summary>
    /// Attempts to parse a string representation of a boolean value allowed in MySQL system variables.
    /// </summary>
    /// <param name="value">A string representation of a boolean value.</param>
    /// <param name="result">The equivalent bool value.</param>
    /// <returns><c>true</c> if the parse is successful, <c>false</c> otherwise.</returns>
    public static bool TryParseMySqlBoolean(string value, out bool result)
    {
      result = false;
      if (string.IsNullOrEmpty(value))
      {
        return false;
      }

      value = value.Trim();
      if (string.Equals(value, "true", StringComparison.OrdinalIgnoreCase)
          || string.Equals(value, "on", StringComparison.OrdinalIgnoreCase)
          || string.Equals(value, "yes", StringComparison.OrdinalIgnoreCase))
      {
        result = true;
        return true;
      }

      return string.Equals(value, "false", StringComparison.OrdinalIgnoreCase)
             || string.Equals(value, "off", StringComparison.OrdinalIgnoreCase)
             || string.Equals(value, "no", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Unescapes special XML characters.
    /// </summary>
    /// <returns></returns>
    public static string UnescapeSpecialXmlCharacters(string value)
    {
      value = value.Replace("&lt;", "<");
      value = value.Replace("&gt;", ">");
      value = value.Replace("&amp;", "&");
      value = value.Replace("&apos;", "'");
      value = value.Replace("&quot;", "\"");

      return value;
    }

    /// <summary>
    /// Asks a confirmation that requires a "Yes" or "No" answer on the GUI or CLI.
    /// </summary>
    /// <param name="title">The title used on the GUI window.</param>
    /// <param name="message">The message question displayed on both the GUI and CLI.</param>
    /// <param name="infoType">The <see cref="InfoDialog.InfoType"/> used for the GUI <see cref="InfoDialog"/>.</param>
    /// <returns><c>true</c> if the response is "Yes", <c>false</c> if the response is "No".</returns>
    public static bool YesNoConfirm(string title, string message, InfoDialog.InfoType infoType = InfoDialog.InfoType.Warning)
    {
      if (!RunningOnConsole())
      {
        return InfoDialog.ShowDialog(InfoDialogProperties.GetYesNoDialogProperties(InfoDialog.InfoType.Info, title, message)).DialogResult == DialogResult.Yes;
      }

      Console.WriteLine(message);
      while (true)
      {
        Console.Write(Resources.YesOrNoForConsoleText);
        string input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
        {
          continue;
        }

        input = input.Trim();
        switch (input)
        {
          case "Y":
          case "y":
            return true;

          case "N":
          case "n":
            return false;
        }
      }
    }

    private static List<Control> FindAllControls(Control container)
    {
      var controlList = new List<Control>();
      FindAllControls(container, controlList);
      return controlList;
    }

    private static void FindAllControls(Control container, ICollection<Control> ctrlList)
    {
      foreach (Control ctrl in container.Controls)
      {
        if (ctrl.Controls.Count == 0)
        {
          ctrlList.Add(ctrl);
        }
        else
        {
          FindAllControls(ctrl, ctrlList);
        }
      }
    }

    [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
    private static extern int GetDeviceCaps(IntPtr hDc, int nIndex);

    private static void NormalizeFontForControls(Control.ControlCollection controls, float ratio)
    {
      foreach (Control c in controls)
      {
        if (c is GroupBox || c is Panel)
        {
          NormalizeFontForControls(c.Controls, ratio);
        }
        else
        {
          c.Font = new Font(c.Font.FontFamily, c.Font.SizeInPoints * ratio);
        }
      }
    }

    /// <summary>
    /// Evaluate current system tcp connections to determine if the given port is available.
    /// This is the same information provided by the netstat command line application, 
    /// just in .Net strongly-typed object form.
    /// </summary>
    /// <param name="port">System port value to evaluate</param>
    /// <returns><c>true</c> if the given port is free, otherwise <c>false</c>.</returns>
    public static bool PortIsAvailable(uint port)
    {
      var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
      var endPoints = ipGlobalProperties.GetActiveTcpListeners();
      return endPoints.All(endPoint => endPoint.Port != port);
    }

    /// <summary>
    /// Extracts a specific entry in a ZIP file with a given name in the specified target directory.
    /// </summary>
    /// <param name="zipFile">The ZIP file full path.</param>
    /// /// <param name="entryName">The file name inside the ZIP file that is intended to be extracted.</param>
    /// <param name="targetEntryName">The file name given after the extraction, if null or empty the <seealso cref="entryName"/> is used.</param>
    /// <param name="targetDirectory">The target directory where the archived contents will be uncompressed to.</param>
    /// <param name="reThrowException">Flag indicating whether Exceptions should be re-thrown.</param>
    /// <returns><c>true</c> if the extraction completes successfully, <c>false</c> otherwise.</returns>
    public static bool UnZipEntry(string zipFile, string entryName, string targetEntryName, string targetDirectory, bool reThrowException = false)
    {
      if (string.IsNullOrEmpty(zipFile)
          || !File.Exists(zipFile)
          || string.IsNullOrEmpty(targetDirectory)
          || string.IsNullOrEmpty(entryName))
      {
        return false;
      }

      var error = ValidateFilePath(targetDirectory);
      if (!string.IsNullOrEmpty(error))
      {
        if (reThrowException)
        {
          throw new Exception(error);
        }

        return false;
      }

      var success = true;
      try
      {
        using (var archive = ZipFile.OpenRead(zipFile))
        {
          var msiZipEntry = archive.Entries.FirstOrDefault(entry => entry.Name.Equals(entryName, StringComparison.OrdinalIgnoreCase));
          if (msiZipEntry == null)
          {
            return false;
          }

          msiZipEntry.ExtractToFile(Path.Combine(targetDirectory, string.IsNullOrEmpty(targetEntryName) ? entryName : targetEntryName), true);
        }
      }
      catch (Exception ex)
      {
        success = false;
        Logger.LogException(ex);
        if (reThrowException)
        {
          throw;
        }
      }

      return success;
    }

    /// <summary>
    /// Validates a given file path.
    /// </summary>
    /// <param name="filePath">A file path.</param>
    /// <returns>An error message if the file path is invalid, or <c>null</c> if it's valid.</returns>
    public static string ValidateFilePath(string filePath)
    {
      string errorMessage = null;
      try
      {
        var directoryPath = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directoryPath)
            && !Path.IsPathRooted(filePath))
        {
          errorMessage = Resources.PathNotAbsoluteError;
        }

        if (string.IsNullOrEmpty(errorMessage))
        {
          var absolutePath = Path.GetFullPath(filePath);
          if (!string.IsNullOrEmpty(absolutePath))
          {
            var fileName = Path.GetFileName(absolutePath);
            if (string.IsNullOrEmpty(fileName))
            {
              errorMessage = Resources.PathContainsNoFileError;
            }

            if (string.IsNullOrEmpty(errorMessage)
                && !string.IsNullOrEmpty(directoryPath)
                && !Directory.Exists(directoryPath))
            {
              errorMessage = Resources.PathNonExistentDirectoryError;
            }
          }
        }
      }
      catch (ArgumentNullException)
      {
        errorMessage = Resources.PathIsNullError;
      }
      catch (ArgumentException)
      {
        errorMessage = Resources.PathContainsInvalidCharactersError;
      }
      catch (PathTooLongException)
      {
        errorMessage = Resources.PathTooLongError;
      }
      catch (NotSupportedException)
      {
        errorMessage = Resources.PathContainsColonError;
      }
      catch (Exception)
      {
        errorMessage = Resources.PathUnknownError;
      }

      return errorMessage;
    }

    /// <summary>
    /// Validates that the provided path is valid.
    /// </summary>
    /// <param name="path">The path to validate.</param>
    /// <returns>An empty strng if the path is valid; otherwise, an error message string.</returns>
    /// <remarks>This method does not check that path exists, it only validates that the format is valid.</remarks>
    public static string ValidateAbsoluteFilePath(string path)
    {
      // Check if the path has the required minimum length.
      if (string.IsNullOrEmpty(path)
          || path.Length < 3)
      {
        return Resources.PathInvalidMinimumLengthError;
      }

      // Check if the path contains invalid characters.
      var invalidFileNameChars = new string(Path.GetInvalidPathChars());
      invalidFileNameChars += @":/?*" + "\"";
      var containsBadCharacter = new Regex("[" + Regex.Escape(invalidFileNameChars) + "]");
      if (containsBadCharacter.IsMatch(path.Substring(3, path.Length - 3)))
      {
        return Resources.PathInvalidBadCharactersError;
      }

      // Check if the path is rooted.
      var directoryPath = Path.GetDirectoryName(path);
      if (!string.IsNullOrEmpty(directoryPath)
          && !Path.IsPathRooted(path))
      {
        return Resources.PathInvalidNotAbsoluteError;
      }

      // Check if the path starts with the expected format.
      var driveRegex = new Regex(@"^[a-zA-Z]:\\$");
      if (!driveRegex.IsMatch(path.Substring(0, 3)))
      {
        return Resources.PathInvalidFormatError;
      }

      // Check if the drive exists.
      try
      {
        var machineDrives = DriveInfo.GetDrives().Select(drive => drive.Name);
        if (!machineDrives.Contains(path.Substring(0, 3)))
        {
          return Resources.PathInvalidDriveNotFoundError;
        }
      }
      catch (Exception)
      {
        return Resources.PathInvalidUnableToGetDrivesError;
      }

      // Check that path does not end with a . char.
      if (path[path.Length - 1] == '.')
      {
        return Resources.PathInvalidEndsInDotError;
      }

      return string.Empty;
    }

    /// <summary>
    /// Checks a password and a second verification entry to see if they match.
    /// </summary>
    /// <param name="password">A password.</param>
    /// <param name="confirmationPassword">A verification password entry.</param>
    /// <param name="passwordStrengthLabel">An optional <see cref="PasswordStrengthLabel"/> that reflects the password strength.</param>
    /// <returns><c>null</c> if the passwords are valid, otherwise an error message.</returns>
    public static string ValidatePasswords(string password, string confirmationPassword, PasswordStrengthLabel passwordStrengthLabel = null)
    {
      var errorMessage = MySqlServerInstance.ValidatePassword(password, true);
      if (string.IsNullOrEmpty(errorMessage))
      {
        errorMessage = password.Equals(confirmationPassword, StringComparison.Ordinal)
          ? null
          : Resources.PasswordsDoNotMatch;
      }

      passwordStrengthLabel?.UpdatePasswordStrengthMessage(password, errorMessage == null);
      return errorMessage;
    }

    /// <summary>
    /// Parses a Unicode command line string and returns an array of pointers to the command line arguments, along with a count of such arguments, in a way that is similar to the standard C run-time argv and argc values.
    /// </summary>
    /// <param name="lpCmdLine">Pointer to a null-terminated Unicode string that contains the full command line. If this parameter is an empty string the function returns the path to the current executable file.</param>
    /// <param name="pNumArgs">Pointer to an int that receives the number of array elements returned, similar to argc.</param>
    /// <returns>A pointer to an array of LPWSTR values, similar to argv. If the method fails, the return value is NULL. To get extended error information, call GetLastError.</returns>
    [DllImport(DllImportConstants.SHELL32, SetLastError = true)]
    private static extern IntPtr CommandLineToArgvW([MarshalAs(UnmanagedType.LPWStr)] string lpCmdLine, out int pNumArgs);

    /// <summary>
    /// Retrieves the window handle to the active window attached to the calling thread's message queue.
    /// </summary>
    /// <returns>The handle to the active window attached to the calling thread's message queue.</returns>
    [DllImport(DllImportConstants.USER32, ExactSpelling = true, CharSet = CharSet.Auto)]
    [ResourceExposure(ResourceScope.Process)]
    private static extern IntPtr GetActiveWindow();

    /// <summary>
    /// Retrieves a handle to the foreground window (the window with which the user is currently working).
    /// </summary>
    /// <remarks>The system assigns a slightly higher priority to the thread that creates the foreground window than it does to other threads.</remarks>
    /// <returns>A handle to the foreground window.</returns>
    [DllImport(DllImportConstants.USER32, ExactSpelling = true, CharSet = CharSet.Auto)]
    [ResourceExposure(ResourceScope.Process)]
    private static extern IntPtr GetForegroundWindow();

    [DllImport(DllImportConstants.KERNEL32)]
    public static extern int GetPrivateProfileString(string section, string key, string value, StringBuilder result, int size, string fileName);

    [DllImport(DllImportConstants.KERNEL32)]
    public static extern int GetPrivateProfileString(string section, int key, string value, [MarshalAs(UnmanagedType.LPArray)] byte[] result, int size, string fileName);

    [DllImport(DllImportConstants.KERNEL32)]
    public static extern int GetPrivateProfileString(int section, string key, string value, [MarshalAs(UnmanagedType.LPArray)] byte[] result, int size, string fileName);

    /// <summary>
    /// Frees up the memory of the given pointer.
    /// </summary>
    /// <param name="hMem">A <see cref="IntPtr"/> pointer.</param>
    /// <returns><c>null</c> if the memory was freed successfully, otherwise the given pointer is returned.</returns>
    [DllImport(DllImportConstants.KERNEL32)]
    private static extern IntPtr LocalFree(IntPtr hMem);
  }
}

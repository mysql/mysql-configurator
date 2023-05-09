/* Copyright (c) 2011, 2023, Oracle and/or its affiliates.

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
using System.IO;
using System.Text;
using MySql.Configurator.Wizards.Server;

namespace MySql.Configurator.Wizards.Examples
{
  /// <summary>
  /// Contains information of a sample database.
  /// </summary>
  public class ExampleDatabaseInfo
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ExampleDatabaseInfo"/> class.
    /// </summary>
    /// <param name="schemaName">The name of the schema.</param>
    /// <param name="schemaFilePath">The name of the file with the script that creates the database schema.</param>
    /// <param name="dataFilePath">The name of the file with the script that creates the database data.</param>
    public ExampleDatabaseInfo(string schemaName, string schemaFilePath, string dataFilePath)
    {
      SchemaName = schemaName;
      SchemaFilePath = schemaFilePath;
      DataFilePath = dataFilePath;
      ScriptFilesEncoding = Encoding.UTF8;
    }

    #region Properties

    /// <summary>
    /// Gets the name of the schema.
    /// </summary>
    public string SchemaName { get; private set; }

    /// <summary>
    /// Gets the name of the file with the script that creates the database data.
    /// </summary>
    public string DataFilePath { get; private set; }

    /// <summary>
    /// Gets the name of the file with the script that creates the database schema.
    /// </summary>
    public string SchemaFilePath { get; private set; }

    /// <summary>
    /// Gets or sets the <see cref="Encoding"/> to use when reading the schema and data files.
    /// </summary>
    public Encoding ScriptFilesEncoding { get; set; }

    #endregion Properties

    /// <summary>
    /// Gets a list of <see cref="ExampleDatabaseInfo"/> with information of all sample databases contained in the Samples and Examples product.
    /// </summary>
    /// <param name="samplesDatabasesPath">The directory path where sample database scripts exist.</param>
    /// <returns>A list of <see cref="ExampleDatabaseInfo"/> with information of all sample databases contained in the Samples and Examples product.</returns>
    public static List<ExampleDatabaseInfo> GetSampleDatabasesInfo(string samplesDatabasesPath)
    {
      if (string.IsNullOrEmpty(samplesDatabasesPath)
          || !Directory.Exists(samplesDatabasesPath))
      {
        return null;
      }

      var sampleDatabasesInfo = new List<ExampleDatabaseInfo>();
      string[] sampleDirs = Directory.GetDirectories(samplesDatabasesPath);
      foreach (var dir in sampleDirs)
      {
        string schemaFilePath = null;
        string dataFilePath = null;
        var schemaName = dir.Substring(dir.LastIndexOf('\\') + 1).ToLowerInvariant();
        string[] files = Directory.GetFiles(dir);
        foreach (var file in files)
        {
          if (file.EndsWith("-schema.sql", StringComparison.InvariantCultureIgnoreCase))
          {
            schemaFilePath = file;
          }
          else if (file.EndsWith("-data.sql", StringComparison.InvariantCultureIgnoreCase))
          {
            dataFilePath = file;
          }
        }

        // Make sure at least a schema can be created
        if (!string.IsNullOrEmpty(schemaFilePath))
        {
          sampleDatabasesInfo.Add(new ExampleDatabaseInfo(schemaName, schemaFilePath, dataFilePath));
        }
      }

      return sampleDatabasesInfo;
    }

    /// <summary>
    /// Installs the sample database (schema and/or data) in the given MySQL Server instance.
    /// </summary>
    /// <param name="localInstance">The <seealso cref="LocalServerInstance"/> where the sample database will be installed.</param>
    /// <returns><c>true</c> if the sample database was installed successfully, <c>false</c> otherwise.</returns>
    public bool Install(LocalServerInstance localInstance)
    {
      if (localInstance == null
          || string.IsNullOrEmpty(SchemaFilePath)
          || !File.Exists(SchemaFilePath))
      {
        return false;
      }

      int scriptsCount = 1;
      string sqlData = null;
      var sr = new StreamReader(SchemaFilePath, ScriptFilesEncoding);
      var sqlSchema = sr.ReadToEnd();
      if (!string.IsNullOrEmpty(DataFilePath))
      {
        sr = new StreamReader(DataFilePath, ScriptFilesEncoding);
        sqlData = sr.ReadToEnd();
        scriptsCount++;
      }

      return localInstance.ExecuteScripts(false, sqlSchema, sqlData) == scriptsCount;
    }

    public bool Remove(string schemaName, LocalServerInstance localInstance)
    {
      if (localInstance == null
          || string.IsNullOrEmpty(schemaName))
      {
        return false;
      }

      localInstance.ExecuteNonQuery($"DROP DATABASE {schemaName}", out string error);
      return string.IsNullOrEmpty(error);
    }
  }
}
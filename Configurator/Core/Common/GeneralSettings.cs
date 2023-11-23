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

using Microsoft.Win32;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.Logging;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace MySql.Configurator.Core.Common
{
  /// <summary>
  /// Represents the configurator general settings.
  /// </summary>
  [Serializable]
  public class GeneralSettings
  {
    #region Properties

    /// <summary>
    /// Gets or sets a value indicating whether the Server is configured as a Service.
    /// </summary>
    public bool ConfigureAsService { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the Enterprise Firewall is enabled.
    /// </summary>
    public bool EnterpriseFirewallEnabled { get; set; }

    /// <summary>
    /// Gets or sets the path to the server configuration file.
    /// </summary>
    public string IniDirectory { get; set; }

    #endregion
  }

  public class GeneralSettingsManager
  {
    /// <summary>
    /// The name of the configurator general settings file.
    /// </summary>
    public const string CONFIGURATOR_SETTINGS_FILE_NAME = "configurator_settings.json";

    /// <summary>
    /// Deletes the general settings file from the specified path.
    /// </summary>
    /// <param name="path">The path where the file is located.</param>
    public static void DeleteGeneralSettingsFile(string path)
    {
      try
      {
        var settingsFileName = Path.Combine(path, CONFIGURATOR_SETTINGS_FILE_NAME);
        if (File.Exists(settingsFileName))
        {
          File.Delete(settingsFileName);
        }
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
      }
    }

    /// <summary>
    /// Saves the general settings to the specified path and based on the provided settings object.
    /// </summary>
    /// <param name="path">The path where the file will be saved to.</param>
    /// <param name="settings">The settings object that will be converted to the settings file.</param>
    /// <returns><c>true</c> if the file was saved successfully; otherwise, <c>false</c>.</returns>
    public static bool SaveSettings(string path, GeneralSettings settings)
    {
      if (settings == null
          || string.IsNullOrEmpty(path))
      {
        return false;
      }

      try
      {
        var jsonString = JsonSerializer.Serialize(settings);
        File.WriteAllText(path, jsonString);
        return true;
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
        return false;
      }
    }

    /// <summary>
    /// Reads the general settings file and converts it to a <see cref="GeneralSettings"/> object.
    /// </summary>
    /// <param name="path">The path to read the file from.</param>
    /// <returns>A settings object if the file was found and could be parsed; otherwise, <c>null</c>.</returns>
    public static GeneralSettings ReadSettings(string path)
    {
      if (string.IsNullOrEmpty(path))
      {
        return null;
      }

      try
      {
        var settingsFileName = Path.Combine(path, CONFIGURATOR_SETTINGS_FILE_NAME);
        if (!File.Exists(settingsFileName))
        {
          return null;
        }

        var settingsContents = File.ReadAllText(settingsFileName);
        var settings = JsonSerializer.Deserialize<GeneralSettings>(settingsContents);
        return settings;
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
        return null;
      }
    }
  }
}

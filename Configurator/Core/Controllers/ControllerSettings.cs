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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Win32;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Core.Classes.MySql;
using MySql.Configurator.Core.Common;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Core.MSI;
using MySql.Configurator.Properties;

namespace MySql.Configurator.Core.Controllers
{
  [Serializable]
  public class ControllerSettings
  {
    [NonSerialized]
    protected Package.Package Package;

    [NonSerialized]
    public ControllerSettings OldSettings;

    public ControllerSettings(Package.Package p)
    {
      Package = p;
    }

    public void LoadState()
    {
      // Load registry key.
      RegistryKey registryKey = OpenRegistryKey();
      if (registryKey.IsNull())
      {
        throw new ConfiguratorException(ConfiguratorError.RegistryKeyNotFound);
      }

      // Load version.
      var versionObject = registryKey?.GetValue<string>("Version");
      if (versionObject == null
          || !Version.TryParse(versionObject.ToString(), out Version version))
      {
        throw new ConfiguratorException(ConfiguratorError.VersionNotFound);
      }

      if (Package.NormalizedVersion.Major != version.Major
          || Package.NormalizedVersion.Minor != version.Minor
          || Package.NormalizedVersion.Build != version.Build)
      {
        throw new ConfiguratorException(ConfiguratorError.VersionMismatch);
      }

      // Load installation directory.
      var installPath = registryKey?.GetValue<string>("Location");
      if (string.IsNullOrEmpty(installPath))
      {
        throw new ConfiguratorException(ConfiguratorError.InstallDirKeyNotFound);
      }

      if (!Directory.Exists(installPath))
      {
        throw new ConfiguratorException(ConfiguratorError.InstallDirKeyNotFound);
      }

      if (Directory.EnumerateFiles(installPath).Count() == 0)
      {
        throw new ConfiguratorException(ConfiguratorError.InstallDirFilesNotFound);
      }

      Package.IsInstalled = true;

      // Load license.
      // TODO: Check with server team if this is valid.
      var license = registryKey?.GetValue<string>("Location");
      if (!string.IsNullOrEmpty(installPath))
      {
        Package.License = license.Equals("Commercial", StringComparison.InvariantCultureIgnoreCase)
          ? LicenseType.Commercial
          : license.Equals("Community", StringComparison.InvariantCultureIgnoreCase)
            ? LicenseType.Community
            : LicenseType.Unknown;
      }
      
      using (registryKey)
      {
        Logger.LogInformation("Controller Settings - Load State - Load Installed");
        LoadInstalled(registryKey);
      }

      if (!Package.IsInstalled)
      {
        if (Package.UpgradeTarget != null)
        {
          LoadDefaultsForUpgrade();
        }
        else
        {
          LoadDefaultsForInstall();
        }
      }
    }

    [ControllerSetting("Overrides the default installation directory.", "install_dir,installdir")]
    public string InstallDirectory { get; set; }

    public string DefaultInstallDirectory { get; set; }

    protected virtual void LoadDefaultsForInstall()
    {
      Logger.LogInformation("Controller Settings - Load Defaults For Install - load and set default settings");
      bool wowPathRequired = Win32.Is64BitOs && (Package.Architecture == PackageArchitecture.X86);
      var name = Package.Title.Replace('/', '.');
      string programFiles = wowPathRequired
        ? Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
        : Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

      InstallDirectory = $"{programFiles}\\MySQL\\{name}";
      if (!StringEndsWithVersion(InstallDirectory))
      {
        InstallDirectory = $"{InstallDirectory} {Package.NormalizedVersion.Major}.{Package.NormalizedVersion.Minor}";
      }

      DefaultInstallDirectory = InstallDirectory;
    }

    protected virtual void LoadInstalled(RegistryKey key)
    {
      Logger.LogInformation("Controller Settings - Load Installed - setting Install Dir from registry");
      InstallDirectory = key?.GetValue<string>("Location");
      Logger.LogInformation("Controller Settings - Load Installed - InstallDir " + InstallDirectory);
      if (string.IsNullOrEmpty(InstallDirectory))
      {
        InstallDirectory = MsiInterop.GetProperty(Package.Id, "InstallLocation");
      }

      if (string.IsNullOrEmpty(InstallDirectory))
      {
        SetInstallDirectoryFromComponents();
      }
    }

    protected virtual void LoadDefaultsForUpgrade()
    {
      Logger.LogInformation("Controller Settings - Load Defaults For Upgrade - load and set default settings for upgrade");
      LoadDefaultsForInstall();
    }

    private void SetInstallDirectoryFromComponents()
    {
      var paths = GetComponentPaths();
      if (paths.Count == 0)
      {
        return;
      }

      InstallDirectory = paths[0];
    }

    private List<string> GetComponentPaths()
    {
      var paths = new List<string>();
      using (var query = new MsiQuery(Package.Id))
      {
        if (!query.OpenQuery("SELECT Component, ComponentId, Directory_ FROM Component WHERE Directory_ = 'INSTALLDIR'"))
        {
          return paths;
        }

        try
        {
          while (query.NextRow())
          {
            StringBuilder path = new StringBuilder(500);
            uint pathLen = (uint)path.Capacity;
            string id = query.GetString(2);
            InstallState state = MsiInterop.MsiGetComponentPath(Package.Id.ToString("B"), id, path, ref pathLen);
            if (state != InstallState.Local)
            {
              continue;
            }

            string fullPath = path.ToString();
            if (File.Exists(fullPath))
            {
              fullPath = Path.GetDirectoryName(fullPath);
            }

            if (!Directory.Exists(fullPath))
            {
              continue;
            }

            if (!paths.Contains(fullPath))
            {
              paths.Add(fullPath);
            }
          }
        }
        catch(Exception ex)
        {
          Logger.LogException(ex);
        }
      }

      return paths;
    }

    public ControllerSettings Clone()
    {
      var method = typeof(Utilities).GetMethod("DeepClone");
      var type = GetType();
      var generic = method?.MakeGenericMethod(type);
      return (ControllerSettings) generic?.Invoke(null, new object[] { this });
    }

    public void CloneToOldSettings()
    {
      OldSettings = Clone();
    }

    public string GenerateHelpString(Type type = null)
    {
      if (type == null)
      {
        type = GetType();
      }

      var props = type.GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(ControllerSettingAttribute))).ToList();
      props.Reverse();
      var builder = new StringBuilder();
      string indent = "               ";
      foreach (var propertyInfo in props)
      {
        var controllerSettingAttribute = propertyInfo.GetCustomAttributes(typeof(ControllerSettingAttribute), true).First() as ControllerSettingAttribute;
        if (controllerSettingAttribute == null)
        {
          continue;
        }

        string possibleValues = string.Empty;
        if (propertyInfo.PropertyType == typeof (bool))
        {
          possibleValues = "[true|false]";
        }
        else if (propertyInfo.PropertyType.IsEnum)
        {
          possibleValues = GetValuesForEnum(propertyInfo);
        }

        string defaultValue = GetDefaultValue(propertyInfo, controllerSettingAttribute.UseValueAsDefault);
        string[] keywords = controllerSettingAttribute.KeywordList.Split(',');
        builder.AppendFormat("{0,-15} - ", keywords[0]);
        builder.Append(GetDescription(18, controllerSettingAttribute.Description));
        if (!string.IsNullOrEmpty(possibleValues))
        {
          builder.AppendFormat("{0}   Possible values include {1}\r\n", indent, possibleValues);
        }

        if (!string.IsNullOrEmpty(defaultValue))
        {
          builder.AppendFormat("{0}   Default value: {1}\r\n", indent, defaultValue);
        }

        if (keywords.Length <= 1)
        {
          continue;
        }

        builder.AppendFormat("{0}   Possible keywords include: ", indent);
        string delimiter = "";
        for (int x=1; x < keywords.Length; x++)
        {
          builder.AppendFormat("{0}{1}", delimiter, keywords[x]);
          delimiter = ", ";
        }

        builder.AppendLine(string.Empty);
      }

      return builder.ToString();
    }

    private string GetDescription(int indent, string desc)
    {
      int width = 80 - indent;
      if (desc.Length < width)
      {
        return desc + Environment.NewLine;
      }

      var builder = new StringBuilder();
      builder.AppendLine(desc.Substring(0, width));
      desc = desc.Substring(width);
      while (desc.Length > 0)
      {
        int size = desc.Length > width ? width : desc.Length;
        for (int x = 0; x < indent; x++)
        {
          builder.Append(" ");
        }

        string m = desc.Substring(0, size).Trim();
        builder.AppendLine(m);
        desc = desc.Substring(size);
      }

      return builder.ToString();
    }

    private string GetDefaultValue(PropertyInfo pi, bool useValue)
    {
      foreach (var defaultValueAttribute in pi.GetCustomAttributes(true).OfType<DefaultValueAttribute>().Select(attribute => attribute))
      {
        return defaultValueAttribute.Value.ToString();
      }

      return useValue
        ? pi.GetValue(this, null).ToString()
        : string.Empty;
    }

    private string GetValuesForEnum(PropertyInfo pi)
    {
      string delimiter = string.Empty;
      string values = "[";
      Array ar = Enum.GetValues(pi.PropertyType);
      foreach (var value in ar)
      {
        values += delimiter;
        values += value.ToString();
        delimiter = "|";
      }

      values += "]";
      return values;
    }

    public bool SetValue(string keyword, string value, out string message)
    {
      message = string.Empty;
      var type = GetType();
      var props = type.GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(ControllerSettingAttribute)));
      foreach (var propertyInfo in props)
      {
        var controllerSettingAttribute = propertyInfo.GetCustomAttributes(typeof(ControllerSettingAttribute), true).First() as ControllerSettingAttribute;
        if (controllerSettingAttribute == null
            || !controllerSettingAttribute.IsValidKeyword(keyword))
        {
          continue;
        }

        try
        {
          if (propertyInfo.PropertyType == typeof (bool))
          {
            SetBoolValue(propertyInfo, value);
          }
          else if (propertyInfo.PropertyType == typeof (int))
          {
            SetIntValue(propertyInfo, value);
          }
          else if (propertyInfo.PropertyType == typeof(uint))
          {
            SetUIntValue(propertyInfo, value);
          }
          else if (propertyInfo.PropertyType.IsEnum)
          {
            SetEnumValue(propertyInfo, value);
          }
          else if (propertyInfo.PropertyType == typeof (string))
          {
            propertyInfo.SetValue(this, value, null);
          }

          if (controllerSettingAttribute.CheckAction != null)
          {
            ExecuteCheckAction(controllerSettingAttribute.CheckAction, keyword, value, propertyInfo);
          }
        }
        catch (Exception ex)
        {
          message = ex.Message;
          return false;
        }
        return true;
      }

      message = string.Format(Resources.InvalidPropertyGiven, keyword);
      return false;
    }

    /// <summary>
    /// Executes validations for controller setting attributes.
    /// </summary>
    /// <param name="actionName">The name of the action to execute.</param>
    /// <param name="key">The name of the controller setting.</param>
    /// <param name="value">The value of the controller setting.</param>
    /// <param name="propertyInfo">The property information instance used to update the value of the controller setting.</param>
    private void ExecuteCheckAction(string actionName, string key, string value, PropertyInfo propertyInfo = null)
    {
      switch (actionName)
      {
        case "CheckPort":
          if (!uint.TryParse(value, out var port))
          {
            throw new InvalidOperationException(string.Format(Resources.NotProperValueForUInt, value));
          }

          if (!Utilities.PortIsAvailable(port))
          {
            throw new Exception(string.Format(Resources.PortNotAvailable, value));
          }

          break;
        case "CheckPassword":
          var errorMessage = MySqlServerInstance.ValidatePassword(value, true);
          if (!string.IsNullOrEmpty(errorMessage))
          {
            throw new Exception(string.Format(errorMessage));
          }

          break;
        case "CheckServiceName":
          if (Service.ExistsServiceInstance(value))
          {
            throw new Exception(string.Format(Resources.InvalidServiceName, value));
          }

          break;
        case "CheckXPort":
          if (!uint.TryParse(value, out var xPort))
          {
            throw new InvalidOperationException(string.Format(Resources.NotProperValueForUInt, value));
          }

          if (!Utilities.PortIsAvailable(xPort))
          {
            throw new Exception(string.Format(Resources.PortNotAvailable, value));
          }

          break;
        default:
          throw new NotSupportedException(string.Format(Resources.CheckMethodNotSupported, actionName));
      }
    }

    private void SetBoolValue(PropertyInfo propertyInfo, string value)
    {
      if (propertyInfo == null)
      {
        return;
      }

      bool result;
      if (!bool.TryParse(value, out result))
      {
        throw new InvalidOperationException(string.Format(Resources.NotProperValueForBool, value));
      }

      propertyInfo.SetValue(this, result, null);
    }

    private void SetIntValue(PropertyInfo propertyInfo, string value)
    {
      if (propertyInfo == null)
      {
        return;
      }

      int result;
      if (!int.TryParse(value, out result))
      {
        throw new InvalidOperationException(string.Format(Resources.NotProperValueForInt, value));
      }

      propertyInfo.SetValue(this, result, null);
    }

    private void SetUIntValue(PropertyInfo propertyInfo, string value)
    {
      if (propertyInfo == null)
      {
        return;
      }

      uint result;
      if (!uint.TryParse(value, out result))
      {
        throw new InvalidOperationException(string.Format(Resources.NotProperValueForInt, value));
      }

      propertyInfo.SetValue(this, result, null);
    }

    private void SetEnumValue(PropertyInfo propertyInfo, string value)
    {
      if (propertyInfo == null)
      {
        return;
      }

      if (!Enum.IsDefined(propertyInfo.PropertyType, value))
      {
        throw new InvalidOperationException(string.Format(Resources.NotProperValueForType, value, propertyInfo.Name));
      }

      propertyInfo.SetValue(this, Enum.Parse(propertyInfo.PropertyType, value), null);
    }

    protected bool StringEndsWithVersion(string text)
    {
      string ver = $"{Package.NormalizedVersion.Major}.{Package.NormalizedVersion.Minor}";
      return text.EndsWith(ver);
    }

    RegistryKey OpenRegistryKey(string keyname)
    {
      keyname = keyname.Replace("[major]", Package.NormalizedVersion.Major.ToString());
      keyname = keyname.Replace("[minor]", Package.NormalizedVersion.Minor.ToString());
      keyname = keyname.Replace("[build]", Package.NormalizedVersion.Revision.ToString());

      var key = Registry.LocalMachine.OpenSubKey(keyname);
      if (key == null && Win32.Is64BitOs)
      {
        // Check for a 32-bit install
        key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(keyname);
      }

      return key;
    }

    RegistryKey OpenRegistryKey()
    {
      string subKey = Package.RegistryKeyTemplate;
      if (!string.IsNullOrEmpty(subKey))
      {
        return OpenRegistryKey(subKey);
      }

      subKey = null;
      if (!string.IsNullOrEmpty(subKey))
      {
        return OpenRegistryKey(subKey);
      }

      subKey = Package.Product.RegistryKeyTemplate;
      if (!string.IsNullOrEmpty(subKey))
      {
        return OpenRegistryKey(subKey);
      }

      return GenerateDefaultRegistryKeyTemplates().Select(OpenRegistryKey).FirstOrDefault(key => key != null);
    }

    private IEnumerable<string> GenerateDefaultRegistryKeyTemplates()
    {
      string baseDisplayName = Package.BaseDisplayName;
      string baseTitle = Package.Product.BaseTitle;
      var myList = new List<string>
      {
        $"SOFTWARE\\{Package.Publisher}\\{(Package.DisplayName.Contains("MySQL") ? Package.DisplayName : string.Concat("MySQL", " ", Package.DisplayName))}",
        $"SOFTWARE\\{Package.Publisher}\\{(Package.NameWithVersion.Contains("MySQL") ? Package.NameWithVersion : string.Concat("MySQL", " ", Package.NameWithVersion))}",
        $"SOFTWARE\\MySQL AB\\{(Package.DisplayName.Contains("MySQL") ? Package.DisplayName : string.Concat("MySQL", " ", Package.DisplayName))}",
        $"SOFTWARE\\MySQL AB\\{(Package.NameWithVersion.Contains("MySQL") ? Package.NameWithVersion : string.Concat("MySQL", " ", Package.NameWithVersion))}",
        $"SOFTWARE\\{Package.Publisher}\\{(baseDisplayName.Contains("MySQL") ? baseDisplayName : string.Concat("MySQL", " ", baseDisplayName))}",
        $"SOFTWARE\\{Package.Publisher}\\{(baseTitle.Contains("MySQL") ? baseTitle : string.Concat("MySQL", " ", baseTitle))}",
        $"SOFTWARE\\MySQL AB\\{(baseDisplayName.Contains("MySQL") ? baseDisplayName : string.Concat("MySQL", " ", baseDisplayName))}",
        $"SOFTWARE\\MySQL AB\\{(baseTitle.Contains("MySQL") ? baseTitle : string.Concat("MySQL", " ", baseTitle))}",
        $"SOFTWARE\\MySQL AB\\{string.Concat(Package.DisplayName, " ", Package.NormalizedVersion.Major, ".", Package.NormalizedVersion.Minor)}"
      };

      return myList;
    }

    public bool HasChanges()
    {
      if (OldSettings == null)
      {
        return false;
      }

      var props = GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(ControllerSettingAttribute)));
      foreach (var propertyInfo in props)
      {
        object oldValue = propertyInfo.GetValue(OldSettings, null);
        object newValue = propertyInfo.GetValue(this, null);
        if (oldValue == null && newValue != null
            || oldValue != null && newValue == null)
        {
          return true;
        }

        if (oldValue == null)
        {
          continue;
        }

        if (!oldValue.Equals(newValue))
        {
          return true;
        }
      }

      return false;
    }
  }
}

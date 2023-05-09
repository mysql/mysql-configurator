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
using System.Data;
using System.Linq;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Core.IniFile;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.Common;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;
using MySql.Configurator.Properties;
using MySql.Configurator.Core.Classes.Attributes;
using MySql.Configurator.Core.Classes.MySqlWorkbench;
using System.Drawing.Imaging;
using MySql.Configurator.Core.Classes.VisualStyles;

namespace MySql.Configurator.Core.Classes
{
  public static class ExtensionMethods
  {
    /// <summary>
    /// The default icon for an <see cref="ErrorProvider"/>.
    /// </summary>
    private static Icon _errorProviderDefaultIcon;

    /// <summary>
    /// Gets The default icon for an <see cref="ErrorProvider"/>.
    /// </summary>
    internal static Icon ErrorProviderDefaultIcon => _errorProviderDefaultIcon ?? (_errorProviderDefaultIcon = new Icon(typeof(ErrorProvider), "Error.ico"));

    /// <summary>
    /// The <see cref="Control.Validating"/> event method.
    /// </summary>
    private static readonly MethodInfo _onValidating = typeof(Control).GetMethod("OnValidating", BindingFlags.Instance | BindingFlags.NonPublic);

    /// <summary>
    /// The <see cref="Control.Validated"/> event method.
    /// </summary>
    private static readonly MethodInfo _onValidated = typeof(Control).GetMethod("OnValidated", BindingFlags.Instance | BindingFlags.NonPublic);

    /// <summary>
    /// Creates a new bitmap based on a given bitmap with its colors changed using the given <see cref="ColorMatrix"/>.
    /// </summary>
    /// <param name="original">The bitmap to change.</param>
    /// <param name="colorMatrix">The <see cref="ColorMatrix"/> used to alter the colors in the original bitmap.</param>
    /// <param name="gammaFactor">Extra brightness correction to the bitmap. Must be greater than 0. 0 = total white, 1.0f = original, > 1.0f = darker.</param>
    /// <returns>A new bitmap based on a given bitmap with its colors changed using the given <see cref="ColorMatrix"/>.</returns>
    public static Bitmap ChangeColors(this Bitmap original, ColorMatrix colorMatrix, float gammaFactor = 1.0f)
    {
      if (original == null)
      {
        return null;
      }

      if (colorMatrix == null)
      {
        return original;
      }

      // Create a blank bitmap the same size as original
      var newBitmap = new Bitmap(original.Width, original.Height);

      // Get a graphics object from the new image
      using (var g = Graphics.FromImage(newBitmap))
      {
        // Create some image attributes
        var attributes = new ImageAttributes();

        // Set the color matrix attribute
        attributes.SetColorMatrix(colorMatrix);

        // Set the gamma.
        attributes.SetGamma(gammaFactor);

        // Draw the original image on the new image using the given color matrix
        g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
      }

      return newBitmap;
    }

    /// <summary>
    /// Returns a value indicating whether the specified <see cref="String"/> object occurs within the given source string.
    /// </summary>
    /// <param name="source">The source string.</param>
    /// <param name="value">The string to seek. </param>
    /// <param name="stringComparison">A <see cref="StringComparison"/> value.</param>
    /// <returns><c>true</c> if the value parameter occurs within this string, or if value is the empty string; otherwise, <c>false</c>.</returns>
    public static bool Contains(this string source, string value, StringComparison stringComparison)
    {
      return !string.IsNullOrEmpty(source) && (string.IsNullOrEmpty(value) || source.IndexOf(value, stringComparison) >= 0);
    }

    /// <summary>
    /// Compares two <see cref="DbConnectionStringBuilder"/> instances to see if they are similar by checking their core host parameters (server, port, user id and database).
    /// </summary>
    /// <param name="sourceConnectionStringBuilder">The source connection string builder.</param>
    /// <param name="targetConnectionStringBuilder">The target connection string builder.</param>
    /// <param name="compareDatabase">Flag indicating whether the database parameter is considered in the comparison.</param>
    /// <returns><c>true</c> if the two connection strings are similar in their core host parameters, <c>false</c> otherwise.</returns>
    public static bool CompareHostParameters(this DbConnectionStringBuilder sourceConnectionStringBuilder, DbConnectionStringBuilder targetConnectionStringBuilder, bool compareDatabase)
    {
      if (sourceConnectionStringBuilder == null && targetConnectionStringBuilder == null)
      {
        return true;
      }

      if (sourceConnectionStringBuilder == null || targetConnectionStringBuilder == null)
      {
        return false;
      }

      bool areSimilar = sourceConnectionStringBuilder.ContainsKey("server") && targetConnectionStringBuilder.ContainsKey("server");
      areSimilar = areSimilar && sourceConnectionStringBuilder["server"].ToString().Equals(targetConnectionStringBuilder["server"].ToString(), StringComparison.InvariantCultureIgnoreCase);
      areSimilar = areSimilar && sourceConnectionStringBuilder.ContainsKey("port") && targetConnectionStringBuilder.ContainsKey("port");
      areSimilar = areSimilar && sourceConnectionStringBuilder["port"].ToString().Equals(targetConnectionStringBuilder["port"].ToString(), StringComparison.InvariantCultureIgnoreCase);
      areSimilar = areSimilar && sourceConnectionStringBuilder.ContainsKey("user id") && targetConnectionStringBuilder.ContainsKey("user id");
      areSimilar = areSimilar && sourceConnectionStringBuilder["user id"].ToString().Equals(targetConnectionStringBuilder["user id"].ToString(), StringComparison.InvariantCultureIgnoreCase);
      if (compareDatabase)
      {
        areSimilar = areSimilar && sourceConnectionStringBuilder.ContainsKey("database") && targetConnectionStringBuilder.ContainsKey("database");
        areSimilar = areSimilar && sourceConnectionStringBuilder["database"].ToString().Equals(targetConnectionStringBuilder["database"].ToString(), StringComparison.InvariantCultureIgnoreCase);
      }

      return areSimilar;
    }

    /// <summary>
    /// Compares two connection strings to see if they are similar by checking their core host parameters (server, port, user id and database).
    /// </summary>
    /// <param name="sourceConnectionString">The source connection string.</param>
    /// <param name="targetConnectionString">The target connection string.</param>
    /// <param name="compareDatabase">Flag indicating whether the database parameter is considered in the comparison.</param>
    /// <returns><c>true</c> if the two connection strings are similar in their core host parameters, <c>false</c> otherwise.</returns>
    public static bool CompareHostParameters(this string sourceConnectionString, string targetConnectionString, bool compareDatabase)
    {
      if (string.IsNullOrEmpty(sourceConnectionString) && string.IsNullOrEmpty(targetConnectionString))
      {
        return true;
      }

      if (string.IsNullOrEmpty(sourceConnectionString) || string.IsNullOrEmpty(targetConnectionString))
      {
        return false;
      }

      var sourceConnectionSb = new DbConnectionStringBuilder { ConnectionString = sourceConnectionString };
      var targetConnectionSb = new DbConnectionStringBuilder { ConnectionString = targetConnectionString };
      return sourceConnectionSb.CompareHostParameters(targetConnectionSb, compareDatabase);
    }

    /// <summary>
    /// Gets recursively child controls of the given type.
    /// </summary>
    /// <param name="control">A <see cref="Control"/> instance.</param>
    /// <returns>Child controls of the given type.</returns>
    public static IEnumerable<T> GetChildControlsOfType<T>(this Control control)
    {
      if (control == null)
      {
        return null;
      }

      var controls = control.Controls.Cast<Control>();
      var enumerable = controls as IList<Control> ?? controls.ToList();
      return enumerable
          .OfType<T>()
          .Concat(enumerable.SelectMany(GetChildControlsOfType<T>));
    }

    /// <summary>
    /// Gets the text defined in the Description attribute of an enumeration value.
    /// </summary>
    /// <param name="value">An enumeration value.</param>
    /// <returns>The text defined in the Description attribute of an enumeration value, if not defined it returns the value converted to string.</returns>
    public static string GetDescription(this Enum value)
    {
      var field = value.GetType().GetField(value.ToString());
      var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
      return attribute == null ? value.ToString() : attribute.Description;
    }

    /// <summary>
    /// Gets a dictionary containing keys representing enumeration values, and values representing enumeration descriptions.
    /// </summary>
    /// <param name="value">An enumeration value.</param>
    /// <param name="splitEnumValuesByCaps">Flag indicating whether enumeration values are split with spaces before a capital letter is found.</param>
    /// <param name="repeatKeyInValue">Flag indicating whether the key text is prepended to the value text to produce something like "keyText - valueText".</param>
    /// <param name="skipGivenValue">Flag indicating whether the given enumeration value should not be included in the dictionary.</param>
    /// <param name="stripTextInValue">Text to strip from the enumeration value, if <c>null</c> the enumeration value is used as is.</param>
    /// <returns>A dictionary containing keys representing enumeration values, and values representing enumeration descriptions.</returns>
    public static Dictionary<string, string> GetDescriptionsDictionary(this Enum value, bool splitEnumValuesByCaps, bool repeatKeyInValue, bool skipGivenValue, string stripTextInValue = null)
    {
      var enumerationValues = Enum.GetValues(value.GetType());
      var dictionary = new Dictionary<string, string>(enumerationValues.Length);
      foreach (Enum enumValue in enumerationValues)
      {
        if (skipGivenValue && Equals(enumValue, value))
        {
          continue;
        }

        var keyText = enumValue.ToString();
        if (!string.IsNullOrEmpty(stripTextInValue))
        {
          keyText = keyText.Replace(stripTextInValue, string.Empty);
        }

        if (splitEnumValuesByCaps)
        {
          keyText = Regex.Replace(keyText, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ");
        }

        var valueText = repeatKeyInValue
          ? keyText + " - " + enumValue.GetDescription()
          : keyText;
        dictionary.Add(keyText, valueText);
      }

      return dictionary;
    }

    /// <summary>
    /// Checks if the given <see cref="CommandAreaProperties.ButtonsLayoutType"/> value corresponds to a layout with 2 buttons.
    /// </summary>
    /// <param name="layoutType">A CommandAreaProperties.ButtonsLayoutType value.</param>
    /// <returns><c>true</c> if the given <see cref="CommandAreaProperties.ButtonsLayoutType"/> value corresponds to a layout with 2 buttons, <c>false</c> otherwise.</returns>
    public static bool Is2Button(this CommandAreaProperties.ButtonsLayoutType layoutType)
    {
      return layoutType == CommandAreaProperties.ButtonsLayoutType.Generic2Buttons
                            || layoutType == CommandAreaProperties.ButtonsLayoutType.OkCancel
                            || layoutType == CommandAreaProperties.ButtonsLayoutType.YesNo;
    }

    /// <summary>
    /// Checks if the given <see cref="CommandAreaProperties.ButtonsLayoutType"/> value corresponds to a generic layout.
    /// </summary>
    /// <param name="layoutType">A CommandAreaProperties.ButtonsLayoutType value.</param>
    /// <returns><c>true</c> if the given <see cref="CommandAreaProperties.ButtonsLayoutType"/> value corresponds to a generic layout, <c>false</c> otherwise.</returns>
    public static bool IsGeneric(this CommandAreaProperties.ButtonsLayoutType layoutType)
    {
      return layoutType == CommandAreaProperties.ButtonsLayoutType.Generic1Button
                            || layoutType == CommandAreaProperties.ButtonsLayoutType.Generic2Buttons
                            || layoutType == CommandAreaProperties.ButtonsLayoutType.Generic3Buttons;
    }

    public static bool IsSameOrSubclass(this Type derivedType, Type baseType)
    {
      return derivedType.IsSubclassOf(baseType)
             || derivedType == baseType;
    }

    public static bool IsNull(this object obj)
    {
      return obj == null;
    }

    public static string FindValue(this IniSection section, string key, bool includeCommentedKeys)
    {
      if (section.IsNull()) return string.Empty;

      var emptyKey = new KeyValuePair<string, string>("EmptyKey", string.Empty);

      var result =
         section.Keys.Where(k => includeCommentedKeys ? k.Key.ToLowerInvariant().Contains(key.ToLowerInvariant()) : k.Key.ToLowerInvariant().Equals(key.ToLowerInvariant()))
           .OrderBy(k => k.Key)
           .DefaultIfEmpty(emptyKey).Last()
           .Value;

      return result ?? string.Empty;
    }

    public static Tuple<ConfigurationKeyType, string> FindValueWithState(this IniSection section, string key)
    {
      if (section.IsNull())
        return new Tuple<ConfigurationKeyType, string>(ConfigurationKeyType.NotPresent, string.Empty);

      var emptyKey = new KeyValuePair<string, string>("EmptyKey", string.Empty);

      var keyResult =
        section.Keys.Where(k => k.Key.ToLowerInvariant().Contains(key.ToLowerInvariant()))
          .OrderBy(k => k.Key)
          .DefaultIfEmpty(emptyKey).Last();

      var configurationKeyType = ConfigurationKeyType.NotCommented;

      if (keyResult.Key == "EmptyKey") configurationKeyType = ConfigurationKeyType.NotPresent;
      if (keyResult.Key.Trim().StartsWith("#")) configurationKeyType = ConfigurationKeyType.Commented;

      var result = new Tuple<ConfigurationKeyType, string>(configurationKeyType, keyResult.Value);

      return result;
    }

    public static string FindValue(this IniFile.IniFile iniFile, string section, string key, bool includeCommentedKeys)
    {
      if (iniFile.IsNull()) return string.Empty;
      if (!iniFile.Sections.Any(s => s.Section.ToLowerInvariant().Equals(section.ToLowerInvariant()))) return string.Empty;

      var result = iniFile.Sections.FirstOrDefault(s => s.Section.ToLower().Equals(section)).FindValue(key, includeCommentedKeys);

      return result;
    }

    public static Tuple<ConfigurationKeyType, string> FindValueWithState(this IniFile.IniFile iniFile, string section, string key)
    {
      if (iniFile.IsNull())
        return new Tuple<ConfigurationKeyType, string>(ConfigurationKeyType.NotPresent, string.Empty);
      if (!iniFile.Sections.Any(s => s.Section.ToLowerInvariant().Equals(section.ToLowerInvariant())))
        return new Tuple<ConfigurationKeyType, string>(ConfigurationKeyType.NotPresent, string.Empty);

      var result = iniFile.Sections.First(s => s.Section.ToLowerInvariant().Equals(section.ToLowerInvariant())).FindValueWithState(key);

      return result;
    }

    public static T FindValue<T>(this IniFile.IniFile iniFile, string section, string key, bool includeCommentedKeys)
    {
      T t = default(T);

      if (iniFile.IsNull())
      {
        return t;
      }

      var stringValue = FindValue(iniFile, section, key, includeCommentedKeys);

      if (string.IsNullOrEmpty(stringValue))
      {
        return t;
      }

      if (typeof(T) == typeof(bool))
      {
        int value = (int)Convert.ChangeType(stringValue, typeof(int));
        t = (T)Convert.ChangeType(value, typeof(T));
      }
      else
      {
        t = (T)Convert.ChangeType(stringValue, typeof(T));
      }

      return t;
    }

    public static Tuple<ConfigurationKeyType, T> FindValueWithState<T>(this IniFile.IniFile iniFile, string section, string key)
    {
      T t = default(T);
      Tuple<ConfigurationKeyType, T> result;

      if (iniFile.IsNull())
      {
        result = new Tuple<ConfigurationKeyType, T>(ConfigurationKeyType.NotPresent, t);
        return result;
      }

      var keyValue = FindValueWithState(iniFile, section, key);

      if (string.IsNullOrEmpty(keyValue.Item2))
      {
        result = new Tuple<ConfigurationKeyType, T>(keyValue.Item1, t);
        return result;
      }

      if (typeof(T) == typeof(bool))
      {
        int value = (int)Convert.ChangeType(keyValue.Item2, typeof(int));
        t = (T)Convert.ChangeType(value, typeof(T));
      }
      else
      {
        t = (T)Convert.ChangeType(keyValue.Item2, typeof(T));
      }

      result = new Tuple<ConfigurationKeyType, T>(keyValue.Item1, t);
      return result;
    }

    public static T FindValue<T>(this IniSection section, string key, bool includeCommentedKeys)
    {
      T t = default(T);

      if (section.IsNull())
      {
        return t;
      }

      var stringValue = FindValue(section, key, includeCommentedKeys);

      if (string.IsNullOrEmpty(stringValue))
      {
        return t;
      }

      if (typeof(T) == typeof(bool))
      {
        int value = (int)Convert.ChangeType(stringValue, typeof(int));
        t = (T)Convert.ChangeType(value, typeof(T));
      }
      else
      {
        t = (T)Convert.ChangeType(stringValue, typeof(T));
      }

      return t;
    }

    public static Tuple<ConfigurationKeyType, T> FindValueWithState<T>(this IniSection section, string key)
    {
      T t = default(T);
      Tuple<ConfigurationKeyType, T> result;

      if (section.IsNull())
      {
        result = new Tuple<ConfigurationKeyType, T>(ConfigurationKeyType.NotPresent, t);
        return result;
      }

      var keyValue = FindValueWithState(section, key);

      if (string.IsNullOrEmpty(keyValue.Item2))
      {
        result = new Tuple<ConfigurationKeyType, T>(keyValue.Item1, t);
        return result;
      }

      if (typeof(T) == typeof(bool))
      {
        int value = (int)Convert.ChangeType(keyValue.Item2, typeof(int));
        t = (T)Convert.ChangeType(value, typeof(T));
      }
      else
      {
        t = (T)Convert.ChangeType(keyValue.Item2, typeof(T));
      }

      result = new Tuple<ConfigurationKeyType, T>(keyValue.Item1, t);
      return result;
    }

    /// <summary>
    /// Executes a FLUSH PRIVILEGES statement.
    /// </summary>
    /// <param name="command">A <see cref="MySqlCommand"/> to execute.</param>
    public static void FlushPrivileges(this MySqlCommand command)
    {
      if (command == null
          || command.Connection.State != ConnectionState.Open)
      {
        return;
      }

      command.CommandText = "FLUSH PRIVILEGES;";
      command.ExecuteNonQuery();
    }

    /// <summary>
    /// Gets a property's alternate name (if the property is decorated by the <see cref="AlternateNameAttribute"/>).
    /// </summary>
    /// <param name="propertyInfo">A <see cref="PropertyInfo"/> instance.</param>
    /// <returns>An alternate name for the given property if applicable.</returns>
    public static string GetAlternateName(this PropertyInfo propertyInfo)
    {
      if (propertyInfo == null)
      {
        return null;
      }

      return propertyInfo.GetCustomAttributes(false).FirstOrDefault(a => a is AlternateNameAttribute) is AlternateNameAttribute alternateAttribute
        ? alternateAttribute.Name
        : null;
    }

    /// <summary>
    /// Gets a property's alternate name (if the property is decorated by the <see cref="AlternateNameAttribute"/>).
    /// </summary>
    /// <param name="instance">An object instance.</param>
    /// <param name="propertyName">A property name in the object's class.</param>
    /// <param name="bindingFlags">The <see cref="BindingFlags"/> to match a property.</param>
    /// <returns>An alternate name for the given property if applicable.</returns>
    public static string GetAlternateName(this object instance, string propertyName, BindingFlags bindingFlags)
    {
      if (instance == null
          || string.IsNullOrEmpty(propertyName))
      {
        return null;
      }

      var propertyInfo = instance.GetType().GetProperty(propertyName, bindingFlags);
      return propertyInfo.GetAlternateName();
    }

    /// <summary>
    /// Returns a proper <see cref="MySqlAuthenticationPluginType"/> value depending on the given Server version.
    /// </summary>
    /// <param name="serverVersion">The Server version.</param>
    /// <returns>A proper <see cref="MySqlAuthenticationPluginType"/> value depending on the given Server version.</returns>
    public static MySqlAuthenticationPluginType GetDefaultServerAuthenticationPlugin(this Version serverVersion)
    {
      return serverVersion.ServerSupportsCachingSha2Authentication()
        ? MySqlAuthenticationPluginType.CachingSha2Password
        : MySqlAuthenticationPluginType.MysqlNativePassword;
    }

    /// <summary>
    /// Builds the host identifier describing where the MySQL server instance can be reached at.
    /// </summary>
    /// <param name="stringBuilder">A <see cref="MySqlConnectionStringBuilder"/> instance.</param>
    /// <returns>The host identifier describing where the MySQL server instance can be reached at.</returns>
    public static string GetHostIdentifier(this MySqlConnectionStringBuilder stringBuilder)
    {
      if (stringBuilder == null)
      {
        return string.Empty;
      }

      switch (stringBuilder.ConnectionProtocol)
      {
        case MySqlConnectionProtocol.UnixSocket:
        case MySqlConnectionProtocol.NamedPipe:
          return $"{MySqlWorkbenchConnection.DEFAULT_DATABASE_DRIVER_NAME}@local:{stringBuilder.PipeName}";

        default:
          return $"{MySqlWorkbenchConnection.DEFAULT_DATABASE_DRIVER_NAME}@{stringBuilder.Server}:{stringBuilder.Port}";
      }
    }

    /// <summary>
    /// Gets the corresponding <see cref="ServerSeriesType"/> of a given MySQL Server version.
    /// </summary>
    /// <param name="serverVersion">The MySQL Server version.</param>
    /// <returns>The corresponding <see cref="ServerSeriesType"/> of a given MySQL Server version.</returns>
    public static ServerSeriesType GetServerSeries(this string serverVersion)
    {
      if (string.IsNullOrEmpty(serverVersion))
      {
        throw new ArgumentNullException(nameof(serverVersion));
      }

      return GetServerSeries(new Version(serverVersion));
    }

    /// <summary>
    /// Gets the corresponding <see cref="ServerSeriesType"/> of a given MySQL Server version.
    /// </summary>
    /// <param name="serverVersion">The MySQL Server version.</param>
    /// <returns>The corresponding <see cref="ServerSeriesType"/> of a given MySQL Server version.</returns>
    public static ServerSeriesType GetServerSeries(this Version serverVersion)
    {
      if (serverVersion.Major == 5)
      {
        switch (serverVersion.Minor)
        {
          case 1:
            return ServerSeriesType.S51;

          case 5:
            return ServerSeriesType.S55;

          case 6:
            return ServerSeriesType.S56;

          case 7:
            return ServerSeriesType.S57;
        }
      }

      if (serverVersion.Major == 8)
      {
        switch (serverVersion.Minor)
        {
          case 0:
            return ServerSeriesType.S80;

          default:
            return ServerSeriesType.S8x;
        }
      }
      
      throw new Exception("Series not supported");
    }

    public static T GetValue<T>(this RegistryKey key, string name)
    {
      if (key.IsNull()) return default(T);

      return key.GetValue<T>(name, default(T));
    }

    public static T GetValue<T>(this RegistryKey key, string name, T defaultValue)
    {
      if (key.IsNull())
      {
        return default(T);
      }

      var obj = key.GetValue(name, defaultValue);

      if (obj == null)
      {
        return defaultValue;
      }

      try
      {
        return (T)obj;
      }
      catch (InvalidCastException)
      {
      }

      if (typeof(T) == typeof(string))
      {
        obj = obj.ToString();
        return (T)obj;
      }

      var types = new[] { typeof(string), typeof(T).MakeByRefType() };

      var method = typeof(T).GetMethod("TryParse", types);
      if (method == null)
      {
        return defaultValue;
      }

      var valueString = obj.ToString();
      T value = defaultValue;
      var parameters = new object[] { valueString, value };

      method.Invoke(obj, parameters);
      return (T)parameters[1];
    }

    /// <summary>
    /// Gets the length, in pixels, of the longest key or value element among the specified dictionary.
    /// </summary>
    /// <param name="dictionary">A dictionary of string tuples.</param>
    /// <param name="useKey">Flag indicating whether the key (first) element of each tuple to calculate the lenght, or the value (second) element.</param>
    /// <param name="font">The <see cref="Font"/> used to draw the text.</param>
    /// <param name="addedPadding">Length, in pixels, of any padding to add to the computed length.</param>
    /// <returns>The length, in pixels, of the longest key or value element among the specified dictionary.</returns>
    public static int GetMaxElementLength(this Dictionary<string, string> dictionary, bool useKey, Font font, int addedPadding = 0)
    {
      if (dictionary == null)
      {
        return 0;
      }

      int longestLength = 0;
      foreach (var tuple in dictionary)
      {
        longestLength = Math.Max(longestLength, TextRenderer.MeasureText(useKey ? tuple.Key : tuple.Value, font).Width);
      }

      return longestLength + addedPadding;
    }

    /// <summary>
    /// Gets a list of tuples containing <see cref="PropertyInfo"/>s of properties with matching names or alternate names.
    /// </summary>
    /// <param name="object1">An object.</param>
    /// <param name="object2">Another object.</param>
    /// <param name="useAlternateNames">Flag indicating whether property names are also matched with alternate names given by the <see cref="AlternateNameAttribute"/> decorator.</param>
    /// <param name="exclude">Flag indicating if properties decorated with the <see cref="ExcludeAttribute"/> should be excluded.</param>
    /// <param name="caseSensitive">Flag indicating if property (or alternate property) names are compared case sensitive or insensitive.</param>
    /// <returns>A list of tuples containing <see cref="PropertyInfo"/>s of properties with matching names or alternate names.</returns>
    public static List<Tuple<PropertyInfo, PropertyInfo>> GetPropertyInfosMatching(this object object1, object object2, bool useAlternateNames = true, bool exclude = true, bool caseSensitive = true)
    {
      if (object1 == null)
      {
        throw new ArgumentNullException(nameof(object1));
      }

      if (object2 == null)
      {
        throw new ArgumentNullException(nameof(object2));
      }

      var object1PropertyInfos = object1.GetType().GetProperties();
      var object2PropertyInfos = object2.GetType().GetProperties();
      var stringComparison = caseSensitive
        ? StringComparison.Ordinal
        : StringComparison.OrdinalIgnoreCase;
      var retList = new List<Tuple<PropertyInfo, PropertyInfo>>(Math.Max(object1PropertyInfos.Length, object2PropertyInfos.Length));
      foreach (var object1PropertyInfo in object1PropertyInfos)
      {
        if (!object1PropertyInfo.CanRead
            || exclude && object1PropertyInfo.IsExcluded())
        {
          continue;
        }

        var object1AlternateName = useAlternateNames ? object1PropertyInfo.GetAlternateName() : null;
        var object2PropertyInfo = object2PropertyInfos.FirstOrDefault(o2Pi =>
        {
          var object2AlternateName = useAlternateNames ? o2Pi.GetAlternateName() : null;
          return o2Pi.Name.Equals(object1PropertyInfo.Name, stringComparison)
                 || !string.IsNullOrEmpty(object1AlternateName)
                    && o2Pi.Name.Equals(object1AlternateName, stringComparison)
                 || !string.IsNullOrEmpty(object2AlternateName)
                    && object2AlternateName.Equals(object1PropertyInfo.Name, stringComparison)
                 || !string.IsNullOrEmpty(object2AlternateName)
                    && !string.IsNullOrEmpty(object1AlternateName)
                    && object2AlternateName.Equals(object1AlternateName, stringComparison);
        });
        if (object2PropertyInfo == null
            || exclude && object2PropertyInfo.IsExcluded())
        {
          continue;
        }

        retList.Add(new Tuple<PropertyInfo, PropertyInfo>(object1PropertyInfo, object2PropertyInfo));
      }

      return retList;
    }

    /// <summary>
    /// Gets the width of text drawn on the given control that can fit within its drawing area by splitting the text in lines.
    /// </summary>
    /// <param name="control">The control where we want to draw the text, normally a label.</param>
    /// <param name="maxlinesOfText">The maximum lines in which the text can be split into.</param>
    /// <param name="overridingText">The text to be drawn in the control, if <c>null</c> the control's Text is used.</param>
    /// <returns>The width of the text split into lines.</returns>
    public static int GetWidthOfTextSplitInLines(this Control control, int maxlinesOfText, string overridingText = null)
    {
      int maxWidth = 0;
      if (control == null)
      {
        return maxWidth;
      }

      if (maxlinesOfText <= 0)
      {
        throw new ArgumentOutOfRangeException(nameof(maxlinesOfText));
      }

      if (overridingText == null)
      {
        overridingText = control.Text;
      }

      maxWidth = control.PreferredSize.Width;
      if (string.IsNullOrEmpty(overridingText) || maxWidth <= control.Width)
      {
        return control.Width;
      }

      int lines = 1;
      while (lines <= maxlinesOfText && maxWidth > control.Width)
      {
        maxWidth = control.PreferredSize.Width / lines++;
      }

      lines = Math.Min(lines, maxlinesOfText);
      int wordWrappedLines;
      int step = Math.Abs(maxWidth - control.Width);
      int stepMultiplier = 0;
      do
      {
        maxWidth += step * stepMultiplier;
        stepMultiplier = 1;
        wordWrappedLines = control.WordWrapText(overridingText, maxWidth).Count;
      }
      while (wordWrappedLines > lines);

      return maxWidth;
    }

    /// <summary>
    /// Checks if an <see cref="ErrorProvider"/> has set an error on any control within its parent container.
    /// </summary>
    /// <param name="errorProvider">An <see cref="ErrorProvider"/> instance.</param>
    /// <returns><c>true</c> if the <see cref="ErrorProvider"/> has set an error on any control within its parent container, <c>false</c> otherwise.</returns>
    public static bool HasErrors(this ErrorProvider errorProvider)
    {
      return errorProvider != null
             && errorProvider.ContainerControl.GetChildControlsOfType<Control>().Any(c => !string.IsNullOrEmpty(errorProvider.GetError(c)));
    }

    /// <summary>
    /// Checks if two objects have the same property values on their matching property names or alternate names.
    /// </summary>
    /// <param name="object1">An object.</param>
    /// <param name="object2">Another object.</param>
    /// <param name="useAlternateNames">Flag indicating whether property names are also matched with alternate names given by the <see cref="AlternateNameAttribute"/> decorator.</param>
    /// <param name="exclude">Flag indicating if properties decorated with the <see cref="ExcludeAttribute"/> should be excluded.</param>
    /// <param name="caseSensitive">Flag indicating if property (or alternate property) names are compared case sensitive or insensitive.</param>
    /// <returns><c>true</c> if the two objects have the same property values on their matching property names or alternate names, <c>false</c> otherwise.</returns>
    public static bool HasSamePropertyValues(this object object1, object object2, bool useAlternateNames = true, bool exclude = true, bool caseSensitive = true)
    {
      var propertyInfoTuples = object1.GetPropertyInfosMatching(object2, useAlternateNames, exclude, caseSensitive);
      return propertyInfoTuples.All(tuple => tuple.Item1.GetValue(object1) == tuple.Item2.GetValue(object2));
    }

    /// <summary>
    /// Checks if a property is excluded from operations (decorated by the <see cref="ExcludeAttribute"/>).
    /// </summary>
    /// <param name="propertyInfo">A <see cref="PropertyInfo"/> instance.</param>
    /// <returns><c>true</c> if a property is excluded from operations (decorated by the <see cref="ExcludeAttribute"/>), <c>false</c> otherwise.</returns>
    public static bool IsExcluded(this PropertyInfo propertyInfo)
    {
      if (propertyInfo == null)
      {
        throw new ArgumentNullException(nameof(propertyInfo));
      }

      return propertyInfo.GetCustomAttributes(false).Any(a => a is ExcludeAttribute);
    }

    /// <summary>
    /// Checks if a property is excluded from operations (decorated by the <see cref="ExcludeAttribute"/>).
    /// </summary>
    /// <param name="instance">An object instance.</param>
    /// <param name="propertyName">A property name in the object's class.</param>
    /// <param name="bindingFlags">The <see cref="BindingFlags"/> to match a property.</param>
    /// <returns><c>true</c> if a property is excluded from operations (decorated by the <see cref="ExcludeAttribute"/>), <c>false</c> otherwise.</returns>
    public static bool IsExcluded(this object instance, string propertyName, BindingFlags bindingFlags)
    {
      var propertyInfo = instance.GetType().GetProperty(propertyName, bindingFlags);
      return propertyInfo.IsExcluded();
    }

    /// <summary>
    /// Returns an estimated rough size of an <see cref="object"/> instance.
    /// </summary>
    /// <param name="instance">An <see cref="object"/> instance.</param>
    /// <returns>An estimated rough size of an <see cref="object"/> instance.</returns>
    public static int RoughSizeOf(this object instance)
    {
      if (instance == null)
      {
        return 0;
      }

      var typeHandle = instance.GetType().TypeHandle;
      return Marshal.ReadInt32(typeHandle.Value, 4);
    }

    /// <summary>
    /// Escapes characters that require it to fit as part of a MySQL command
    /// </summary>
    /// <param name="entry">Raw user entry value</param>
    /// <param name="exceptionCharacters">Characters to avoid escaping.</param>
    /// <returns>MySQL escaped user entry value</returns>
    /// <remarks>Characters will be escaped each time this method is called since it is
    /// not possible to accurately determine if the string has been previously 
    /// escaped.</remarks>
    public static string Sanitize(this string entry, params char[] exceptionCharacters)
    {
      if (string.IsNullOrEmpty(entry))
      {
        // Nothing to sanitize
        return entry;
      }

      // Exception flags
      var escapeTab = exceptionCharacters.Length == 0 || !exceptionCharacters.Contains('\t');
      var escapeCarriageReturn = exceptionCharacters.Length == 0 || !exceptionCharacters.Contains('\r');
      var escapeLineFeed = exceptionCharacters.Length == 0 || !exceptionCharacters.Contains('\n');
      var escapeZeroChar = exceptionCharacters.Length == 0 || !exceptionCharacters.Contains('\0');
      var escapeSingleQuote = exceptionCharacters.Length == 0 || !exceptionCharacters.Contains('\'');
      var escapeDoubleQuote = exceptionCharacters.Length == 0 || !exceptionCharacters.Contains('"');
      var escapeBackslash = exceptionCharacters.Length == 0 || !exceptionCharacters.Contains('\\');

      var sb = new StringBuilder();
      var entryCharArray = entry.ToCharArray();
      foreach (char c in entryCharArray)
      {
        if (c.Equals('\t')
            && escapeTab)
        {
          sb.Append(@"\t");
          continue;
        }

        if (c.Equals('\r')
            && escapeCarriageReturn)
        {
          sb.Append(@"\r");
          continue;
        }

        if (c.Equals('\n')
            && escapeLineFeed)
        {
          sb.Append(@"\n");
          continue;
        }

        if (c.Equals('\0')
            && escapeZeroChar)
        {
          sb.Append(@"\0");
          continue;
        }

        if ((c.ToString() == @"'" && escapeSingleQuote)
            || (c.ToString() == @"""" && escapeDoubleQuote)
            || (c.ToString() == @"\" && escapeBackslash))
        {
          sb.Append(@"\");
        }

        sb.Append(c);
      }

      return sb.ToString();
    }

    /// <summary>
    /// Gets a value indicating whether the MySQL Server skips the plugin-load and plugin-load-add options as stated in bug #29622406.
    /// </summary>
    /// <param name="serverVersion">The MySQL Server version.</param>
    /// <returns><c>true</c> if the MySQL Server automatically skips the specified plugin load options, <c>false</c> otherwise.</returns>
    public static bool ServerAutomaticallySkipsLoadForNonEarlyPlugins(this Version serverVersion)
    {
      return serverVersion >= new Version("8.0.18");
    }

    /// <summary>
    /// Gets a value indicating whether the MySQL Server includes an anonymous user in its initial installation.
    /// </summary>
    /// <param name="serverVersion">The Server version.</param>
    /// <returns><c>true</c> if the MySQL Server includes an anonymous user in its initial installation, <c>false</c> otherwise.</returns>
    public static bool ServerIncludesAnonymousUser(this Version serverVersion)
    {
      return serverVersion < new Version("5.7.0");
    }

    /// <summary>
    /// Gets a value indicating whether the MySQL Server includes the mysql.host table in its initial installation.
    /// </summary>
    /// <param name="serverVersion">The Server version.</param>
    /// <returns><c>true</c> if the MySQL Server includes the mysql.host table in its initial installation, <c>false</c> otherwise.</returns>
    public static bool ServerIncludesHostTable(this Version serverVersion)
    {
      return serverVersion < new Version("5.6.7");
    }

    /// <summary>
    /// Gets a value indicating whether the MySQL Server includes the mysql.proc and mysql.events table.
    /// </summary>
    /// <param name="serverVersion">The Server version.</param>
    /// <returns><c>true</c> if the MySQL Server includes the mysql.proc and mysql.events table, <c>false</c> otherwise.</returns>
    public static bool ServerIncludesProcAndEventsTables(this Version serverVersion)
    {
      return serverVersion < new Version("8.0.0");
    }

    /// <summary>
    /// Gets a value indicating whether the MySQL Server supports database initialization.
    /// </summary>
    /// <param name="serverVersion">The Server version.</param>
    /// <returns><c>true</c> if the MySQL Server supports database initialization, <c>false</c> otherwise.</returns>
    public static bool ServerSupportsDatabaseInitialization(this Version serverVersion)
    {
      return serverVersion >= new Version("5.7.7");
    }

    /// <summary>
    /// Gets a value indicating whether the default_authentication_plugin variable is still not deprecated or removed.
    /// </summary>
    /// <param name="serverVersion">The MySQL Server version.</param>
    /// <returns><c>true</c> if the default_authentication_plugin variable is still usable without a deprecation warning by the specified server version; otherwise, <c>false</c>.</returns>
    public static bool ServerSupportsDefaultAuthenticationPluginVariable(this Version serverVersion)
    {
      return serverVersion < new Version("8.0.27");
    }

    /// <summary>
    /// Gets a value indicating whether the MySQL Server supports Enterprise Firewall.
    /// </summary>
    /// <param name="serverVersion">The MySQL Server version.</param>
    /// <returns><c>true</c> if the MySQL Server supports Enterprise Firewall, <c>false</c> otherwise.</returns>
    public static bool ServerSupportsEnterpriseFirewall(this Version serverVersion)
    {
      return serverVersion >= new Version("5.6.24") && serverVersion < new Version("5.7.0")
             || serverVersion >= new Version("5.7.7");
    }

    /// <summary>
    /// Gets a value indicating whether the MySQL Server supports IDENTIFY [WITH] BY clause in CREATE USER and ALTER USER statements.
    /// </summary>
    /// <param name="serverVersion">The MySQL Server version.</param>
    /// <returns><c>true</c> if the MySQL Server supports IDENTIFY [WITH] BY clause in CREATE USER and ALTER USER statements, <c>false</c> otherwise.</returns>
    public static bool ServerSupportsIdentifyClause(this Version serverVersion)
    {
      return serverVersion >= new Version("5.7.6");
    }

    /// <summary>
    /// <summary>    /// Gets a value indicating whether the MySQL Server supports InnoDB Cluster configuration.
    /// </summary>
    /// <param name="serverVersion">The MySQL Server version.</param>
    /// <returns><c>true</c> if the MySQL Server supports InnoDB Cluster configuration, <c>false</c> otherwise.</returns>
    public static bool ServerSupportsLowerCaseTableNamesModification(this Version serverVersion)
    {
      return serverVersion < new Version("8.0.0");
    }

    /// <summary>
    /// Gets a value indicating whether the MySQL Server MSI supports the MYSQL_INSTALLER property.
    /// </summary>
    /// <param name="serverVersion">The MySQL Server version.</param>
    /// <returns><c>true</c> if the MySQL Server MSI supports the MYSQL_INSTALLER property, <c>false</c> otherwise.</returns>
    public static bool ServerSupportsMySqlInstallerProperty(this Version serverVersion)
    {
      return serverVersion >= new Version("5.6.0");
    }

    /// <summary>
    /// Gets a value indicating whether the system variable tables are supported by the performance schema.
    /// </summary>
    /// <param name="serverVersion">The MySQL Server version.</param>
    /// <returns><c>true</c> if the system variable tables are supported, <c>false</c> otherwise.</returns>
    public static bool ServerSupportsPerformanceSchemaSystemVariableTables(this Version serverVersion)
    {
      return serverVersion >= new Version("5.7.0");
    }

    /// <summary>
    /// Gets a value indicating whether the MySQL Server supports SET PERSIST feature that can modify some global variables without the need of restart the Server.
    /// </summary>
    /// <param name="serverVersion">The MySQL Server version.</param>
    /// <returns><c>true</c> if the MySQL Server supports SET PERSIST feature, <c>false</c> otherwise.</returns>
    public static bool ServerSupportsPersistSetGlobalSettings(this Version serverVersion)
    {
      return serverVersion >= new Version("8.0.11");
    }

    /// <summary>
    /// Gets a value indicating whether the MySQL Server supports regenerating the redo log files.
    /// </summary>
    /// <param name="serverVersion">The MySQL Server version.</param>
    /// <returns><c>true</c> if the MySQL Server supports regenerating the redo log files, <c>false</c> otherwise.</returns>
    public static bool ServerSupportsRegeneratingRedoLogFiles(this Version serverVersion)
    {
      return serverVersion < new Version("8.0.0");
    }

    /// Gets a value indicating whether the MySQL Server supports self contained upgrades that do not need running the mysql_upgrade client.
    /// </summary>
    /// <param name="serverVersion"></param>
    /// <returns></returns>
    public static bool ServerSupportsSelfContainedUpgrade(this Version serverVersion)
    {
      return serverVersion >= new Version("8.0.16");
    }

    /// <summary>
    /// Gets a value indicating whether the MySQL Server supports caching_sha2_password authentication plugin.
    /// </summary>
    /// <param name="serverVersion">The MySQL Server version.</param>
    /// <returns><c>true</c> if the MySQL Server supports caching_sha2_password authentication plugin, <c>false</c> otherwise.</returns>
    public static bool ServerSupportsCachingSha2Authentication(this Version serverVersion)
    {
      return serverVersion >= new Version("8.0.4");
    }

    /// <summary>
    /// Gets a value indicating whether the MySQL Server supports the MEMBER_ROLE column found in the performance_schema.replication_group_members table.
    /// </summary>
    /// <param name="serverVersion">The MySQL Server version.</param>
    /// <returns><c>true</c> if the MySQL Server supports the MEMBER_ROLE column, <c>false</c> otherwise.</returns>
    public static bool ServerSupportsMemberRoleColumn(this Version serverVersion)
    {
      return serverVersion >= new Version("8.0.2");
    }

    /// <summary>
    /// Gets a value indicating whether the MySQL Server supports the named_pipe_full_access_group variable.
    /// </summary>
    /// <param name="serverVersion">The MySQL Server version.</param>
    /// <returns><c>true</c> if the MySQL Server support the named_pipe_full_acess_group variable; otherwise, <c>false</c>.</returns>
    public static bool ServerSupportsNamedPipeFullAccessGroupVariable(this Version serverVersion)
    {
      return (serverVersion.Major == 8
              && serverVersion.Minor == 0
              && serverVersion.Build > 13)
             ||
             (serverVersion.Major == 5
              && serverVersion.Minor == 7
              && serverVersion.Build > 24)
             ||
             (serverVersion.Major == 5
              && serverVersion.Minor == 6
              && serverVersion.Build > 42);
    }

    /// <summary>
    /// Gets a value indicating whether the super_read_only variable is supported by the MySQL Server version.
    /// </summary>
    /// <param name="serverVersion">The MySQL Server version.</param>
    /// <returns><c>true</c> if the super_read_only variable is supported, <c>false</c> otherwise.</returns>
    public static bool ServerSupportsSuperReadOnlyVariable(this Version serverVersion)
    {
      return serverVersion >= new Version("5.7.8");
    }

    /// <summary>
    /// Gets a value indicating whether the MySQL Server supports Windows Authentication.
    /// </summary>
    /// <param name="serverVersion">The MySQL Server version.</param>
    /// <returns><c>true</c> if the MySQL Server supports Windows Authentication, <c>false</c> otherwise.</returns>
    public static bool ServerSupportsWindowsAuthentication(this Version serverVersion)
    {
      return serverVersion >= new Version("5.5.15");
    }

    /// <summary>
    /// Gets a value indicating whether the MySQL Server supports the X Protocol.
    /// </summary>
    /// <param name="serverVersion">The MySQL Server version.</param>
    /// <returns><c>true</c> if the MySQL Server supports the X Protocol, <c>false</c> otherwise.</returns>
    public static bool ServerSupportsXProtocol(this Version serverVersion)
    {
      return serverVersion >= new Version("8.0.10");
    }

    /// <summary>
    /// Gets a value indicating whether the binary log is enabled by default.
    /// </summary>
    /// <param name="serverVersion">The MySQL Server version.</param>
    /// <returns><c>true</c> if the binary log is enabled by default in the specified MySQL Server version, <c>false</c> otherwise.</returns>
    public static bool ServerHasBinaryLogEnabledByDefault(this Version serverVersion)
    {
      return serverVersion >= new Version("8.0.3");
    }

    /// <summary>
    /// Sets properties related to an error provider in a single call.
    /// </summary>
    /// <param name="errorProvider">An <see cref="ErrorProvider"/> instance (can't be <c>null</c>).</param>
    /// <param name="onControl">A <see cref="Control"/> instance (can't be <c>null</c>).</param>
    /// <param name="properties">The properties to apply to the <see cref="ErrorProvider"/>. If <c>null</c> the <see cref="ErrorProviderProperties.Empty"/> properties are used.</param>
    public static void SetProperties(this ErrorProvider errorProvider, Control onControl, ErrorProviderProperties properties)
    {
      if (errorProvider == null)
      {
        throw new ArgumentNullException(nameof(errorProvider));
      }

      if (properties == null)
      {
        properties = ErrorProviderProperties.Empty;
      }

      if (onControl == null)
      {
        throw new ArgumentNullException(nameof(onControl));
      }

      if (properties.Clear)
      {
        errorProvider.Clear();
      }

      var icon = properties.ErrorIcon ?? ErrorProviderDefaultIcon;
      if (errorProvider.Icon != icon)
      {
        errorProvider.Icon = icon;
      }

      errorProvider.SetError(onControl, properties.ErrorMessage);
      if (properties.IconPadding != 0
          && !string.IsNullOrEmpty(properties.ErrorMessage))
      {
        // The icon will  not be displayed with a null or empty text, so no need to set the padding.
        errorProvider.SetIconPadding(onControl, properties.IconPadding);
      }

      errorProvider.SetIconAlignment(onControl, properties.IconAlignment);
    }

    /// <summary>
    /// Sets property values from one object to another if they have the same property names or alternate names.
    /// </summary>
    /// <param name="toObject">The object that have its property values set.</param>
    /// <param name="fromObject">The object from which property values are copied.</param>
    /// <param name="useAlternateNames">Flag indicating whether property names are also matched with alternate names given by the <see cref="AlternateNameAttribute"/> decorator.</param>
    /// <param name="exclude">Flag indicating if properties decorated with the <see cref="ExcludeAttribute"/> should be excluded.</param>
    /// <param name="caseSensitive">Flag indicating if property (or alternate property) names are compared case sensitive or insensitive.</param>
    public static void SetPropertyValuesFrom(this object toObject, object fromObject, bool useAlternateNames = true, bool exclude = true, bool caseSensitive = true)
    {
      var propertyInfoTuples = toObject.GetPropertyInfosMatching(fromObject, useAlternateNames, exclude, caseSensitive);
      foreach (var tuple in propertyInfoTuples)
      {
        var fromObjectPropertyInfo = tuple.Item2;
        var toObjectPropertyInfo = tuple.Item1;
        if (!fromObjectPropertyInfo.CanRead
            || !toObjectPropertyInfo.CanWrite)
        {
          continue;
        }

        toObjectPropertyInfo.SetValue(toObject, fromObjectPropertyInfo.GetValue(fromObject));
      }
    }

    /// <summary>
    /// Returns a string representation of the given <see cref="SchemaInformationType"/> value.
    /// </summary>
    /// <param name="schemaInformation">A <see cref="SchemaInformationType"/> value.</param>
    /// <returns>A string representation of the given <see cref="SchemaInformationType"/> value.</returns>
    public static string ToCollection(this SchemaInformationType schemaInformation)
    {
      switch (schemaInformation)
      {
        case SchemaInformationType.ColumnsFull:
          return "COLUMNS";

        case SchemaInformationType.ForeignKeyColumns:
          return "FOREIGN KEY COLUMNS";

        case SchemaInformationType.ForeignKeys:
          return "FOREIGN KEYS";

        case SchemaInformationType.ProcedureParameters:
          return "PROCEDURE PARAMETERS";

        case SchemaInformationType.ProceduresWithParameters:
          return "PROCEDURES WITH PARAMETERS";

        default:
          return schemaInformation.ToString().ToUpperInvariant();
      }
    }

    /// <summary>
    /// Converts a given <see cref="UriHostNameType"/> value to a <see cref="ValidHostNameType"/> one.
    /// </summary>
    /// <param name="uriHostNameType">A <see cref="UriHostNameType"/> value.</param>
    /// <returns>A <see cref="ValidHostNameType"/> value.</returns>
    public static ValidHostNameType ToValidHostNameType(this UriHostNameType uriHostNameType)
    {
      switch (uriHostNameType)
      {
        case UriHostNameType.Dns:
          return ValidHostNameType.DNS;

        case UriHostNameType.IPv4:
          return ValidHostNameType.IPv4;

        case UriHostNameType.IPv6:
          return ValidHostNameType.IPv6;

        default:
          return ValidHostNameType.Unknown;
      }
    }

    /// <summary>
    /// Creates a new bitmap based on a given bitmap with its colors transformed by the given factors.
    /// </summary>
    /// <param name="original">The bitmap to change.</param>
    /// <param name="brightnessFactor">The amount of "sunlight" in the picture. Ranges between -1.0f to 1.0f, -1.0f = pitch-black, 0 = original, 1.0f = total white.</param>
    /// <param name="contrastFactor">The amount of difference between red, green, and blue colors. Must be greater than 0. 0 = complete gray, 1 = original, > 1.0f = glaring white.</param>
    /// <param name="saturationFactor">The amount of "grayscale-ness" in the picture. Must be greater than 0. 0 = grayscale, 1.0f = original colors, > 1.0f = very colorful.</param>
    /// <param name="gammaFactor">Extra brightness correction to the bitmap. Must be greater than 0. 0 = total white, 1.0f = original, > 1.0f = darker.</param>
    /// <returns>A new bitmap based on a given bitmap with its colors transformed by the given factors.</returns>
    public static Bitmap TransformColors(this Bitmap original, float brightnessFactor = 0, float contrastFactor = 1.0f, float saturationFactor = 1.0f, float gammaFactor = 1.0f)
    {
      return original.ChangeColors(StyleableHelper.CreateColorMatrix(brightnessFactor, contrastFactor, saturationFactor), gammaFactor);
    }

    /// <summary>
    /// Gets an enumeration value whose description matches the given one.
    /// </summary>
    /// <param name="enumType"></param>
    /// <param name="description">The description assigned to an enumeration value.</param>
    /// <param name="ignoreCase"><c>true</c> to ignore case; <c>false</c> to consider case.</param>
    /// <param name="result">
    /// Contains an object of type <see cref="TEnum"/> whose value's description matches <seealso cref="description"/>.
    /// If the parse operation fails, result contains the default value of the underlying type of <see cref="TEnum"/>.
    /// Note that this value need not be a member of the <see cref="TEnum"/> enumeration.
    /// This parameter is passed uninitialized.
    /// </param>
    /// <returns>An enumeration value of the given type if it matches the given description.</returns>
    public static bool TryParseFromDescription<TEnum>(this TEnum enumType, string description, bool ignoreCase, out TEnum result) where TEnum : struct
    {
      var theType = typeof(TEnum);
      if (!theType.IsEnum)
      {
        throw new InvalidOperationException(Resources.TEnumNotEnumTypeException);
      }

      var memberInfos = theType.GetMembers();
      result = default(TEnum);
      bool couldParse = false;
      foreach (var memberInfo in memberInfos)
      {
        var attributes = memberInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (attributes.Length == 0)
        {
          continue;
        }

        var descriptionAttribute = attributes[0] as DescriptionAttribute;
        if (descriptionAttribute == null
            || !descriptionAttribute.Description.Equals(description, StringComparison.Ordinal))
        {
          continue;
        }

        var fieldInfo = memberInfo as FieldInfo;
        if (fieldInfo == null)
        {
          break;
        }

        couldParse = true;
        result = (TEnum)fieldInfo.GetValue(null);
      }

      return couldParse;
    }

    /// <summary>
    /// Fires the <see cref="Control.Validating"/> and <see cref="Control.Validated"/> event methods.
    /// </summary>
    /// <param name="control">A <see cref="Control"/> instance.</param>
    /// <param name="runValidating">Flag indicating whether the "validating" delegate is run.</param>
    /// <param name="runValidated">Flag indicating whether the "validated" delegate is run.</param>
    /// <returns><c>true</c> if the <see cref="Control.Validating"/> did not cancel, <c>false</c> otherwise.</returns>
    public static bool Validate(this Control control, bool runValidating = true, bool runValidated = true)
    {
      if (control == null)
      {
        return true;
      }

      if (runValidating)
      {
        var e = new CancelEventArgs();
        _onValidating.Invoke(control, new object[] { e });
        if (e.Cancel)
        {
          return false;
        }
      }

      if (runValidated)
      {
        _onValidated.Invoke(control, new object[] { EventArgs.Empty });
      }

      return true;
    }

    /// <summary>
    /// Implements word wrapping for the label text.
    /// </summary>
    /// <param name="control">The control where we want to draw the text, normally a label.</param>
    /// <param name="proposedText">The proposed text to be drawn on the control.</param>
    /// <param name="overridingWidth">The width in which the text is to be fit, if the number is <c>0</c> or less the control's Width is used.</param>
    /// <returns>A list of strings containing the proposed text word-wrapped in several lines.</returns>
    public static List<string> WordWrapText(this Control control, string proposedText, int overridingWidth = 0)
    {
      if (control == null)
      {
        return null;
      }

      List<string> wordWrapLines = new List<string>();
      if (control.AutoSize)
      {
        wordWrapLines.Add(proposedText);
        return wordWrapLines;
      }

      if (overridingWidth <= 0)
      {
        overridingWidth = control.Width;
      }

      string remainingText = proposedText.Trim();
      do
      {
        SizeF stringSize = TextRenderer.MeasureText(remainingText, control.Font);
        double trimPercentage = overridingWidth / stringSize.Width;
        string textToDraw;
        if (trimPercentage < 1)
        {
          int lengthToCut = Convert.ToInt32(remainingText.Length * trimPercentage);
          lengthToCut = lengthToCut > 0 ? lengthToCut - 1 : 0;
          int spaceBeforePos = lengthToCut;
          int spaceAfterPos = remainingText.IndexOf(" ", lengthToCut, StringComparison.Ordinal);
          textToDraw = spaceAfterPos >= 0 ? remainingText.Substring(0, spaceAfterPos) : remainingText;
          while (spaceBeforePos > -1 && TextRenderer.MeasureText(textToDraw, control.Font).Width > overridingWidth)
          {
            spaceBeforePos = remainingText.LastIndexOf(" ", spaceBeforePos, StringComparison.Ordinal);
            textToDraw = spaceBeforePos >= 0 ? remainingText.Substring(0, spaceBeforePos) : textToDraw;
            spaceBeforePos--;
          }
        }
        else
        {
          textToDraw = remainingText;
        }

        textToDraw = textToDraw.Trim();
        if (textToDraw.Length > 0)
        {
          wordWrapLines.Add(textToDraw);
        }

        remainingText = textToDraw.Length < remainingText.Length ? remainingText.Substring(textToDraw.Length).Trim() : string.Empty;
      } while (remainingText.Length > 0);

      return wordWrapLines;
    }
  }
}

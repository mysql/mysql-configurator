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

using MySql.Configurator.Core.Classes.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MySql.Configurator.Core.Controllers
{
  public class PluginManager
  {
    private bool _loadedPlugins;
    private readonly Dictionary<string, ControllerType> _controllerTypes;

    public static PluginManager Instance = new PluginManager();

    public PluginManager()
    {
      _controllerTypes = new Dictionary<string, ControllerType>();
    }

    /// <summary>
    /// Retrieves the latest controller plugin (if any) associated with the given product name
    /// </summary>
    public static ProductConfigurationController GetController(string productName)
    {
      return Instance.GetControllerInternal(productName);
    }

    private ProductConfigurationController GetControllerInternal(string productName)
    {
      if (!_loadedPlugins)
      {
        LoadPlugins();
      }

      // now find the best match between the product name and our controller product names
      var longestKey = string.Empty;
      foreach (var key in _controllerTypes.Keys)
      {
        if (productName.StartsWith(key)
            && key.Length > longestKey.Length)
        {
          longestKey = key;
        }
      }

      if (!string.IsNullOrEmpty(longestKey))
      {
        Logger.LogInformation($"Creating configuration controller for: {productName}.");
        var cType = _controllerTypes[longestKey];
        var c = (ProductConfigurationController)cType.HostingAssembly.CreateInstance(cType.TypeName, false);
        if (c != null)
        {
          return c;
        }
      }

      return new ProductConfigurationController();
    }

    private void LoadControllersFromAssembly(Assembly assembly)
    {
      //_controllerTypes.Add("mysql-server", new ControllerType(assembly, "MySql.Configurator.Wizards.Server.ServerConfigurationController", 1));
      var interfaceType = typeof(ProductConfigurationController);
      var types = new Type[] { };
      try
      {
        types = assembly.GetTypes();
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
      }

      Logger.LogInformation($"Loading controllers from {assembly.Location}");
      foreach (var type in types)
      {
        if (type.IsAbstract || !interfaceType.IsAssignableFrom(type))
        {
          continue;
        }

        var attr = type.GetCustomAttributes(typeof(ProductConfigurationAttribute), false);

        // if the type doesn't have the right attribute, we ignore it
        if (attr.Length != 1)
        {
          continue;
        }

        var productConfigurationAttribute = (ProductConfigurationAttribute)attr[0];

        // if this is the first controller for this product name, then add it to our
        // dictionary
        if (!_controllerTypes.ContainsKey(productConfigurationAttribute.ProductName))
        {
          _controllerTypes[productConfigurationAttribute.ProductName] = new ControllerType(assembly, type.FullName, productConfigurationAttribute.Version);
        }
        else
        {
          var existingType = _controllerTypes[productConfigurationAttribute.ProductName];
          if (productConfigurationAttribute.Version > existingType.Version)
          {
            _controllerTypes[productConfigurationAttribute.ProductName] = new ControllerType(assembly, type.FullName, productConfigurationAttribute.Version);
          }
        }
      }
    }

    /// <summary>
    /// Loads all the plugins from the executing assembly and all assemblies in the same folder
    /// that end in -plugin.dll
    /// </summary>
    private void LoadPlugins()
    {
      if (_loadedPlugins)
      {
        return;
      }

      // first load the plugins that are baked in
      LoadControllersFromAssembly(Assembly.GetExecutingAssembly());

      var pluginsDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      var plugins = Directory.GetFiles(pluginsDir, "mysql_configurator.exe");
      foreach (var plugin in plugins)
      {
        var a = Assembly.LoadFrom(plugin);
        LoadControllersFromAssembly(a);
      }

      _loadedPlugins = true;
    }
  }
}
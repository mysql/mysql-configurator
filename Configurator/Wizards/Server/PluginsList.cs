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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MySql.Configurator.Core.Enums;

namespace MySql.Configurator.Wizards.Server
{

  /// <summary>
  /// Represents a list of <see cref="Plugin"/> instances.
  /// </summary>
  [Serializable]
  public class PluginsList
  {
    private List<Plugin> _plugins;

    /// <summary>
    /// The server version of the instance that will include the plugins initialized in this class.
    /// </summary>
    private Version _serverVersion;

    [XmlIgnore]
    protected List<Plugin> Plugins => _plugins ?? Initialize(_serverVersion);

    /// <summary>
    /// Loads the list of all the available plugins to be included at the plugin_load key in the ini file.
    /// </summary>
    /// <param name="serverVersion">The server version for which this list of plugins is being initialized for.</param>
    public List<Plugin> Initialize(Version serverVersion)
    {
      _serverVersion = serverVersion;

      return _plugins = new List<Plugin>
      {
        new Plugin("mysqlx"),
        new Plugin("mysql_firewall", "firewall.dll", _serverVersion, PluginActivationStateType.Skip, true),
        new Plugin("mysql_firewall_users", "firewall.dll", _serverVersion, PluginActivationStateType.Skip, true),
        new Plugin("mysql_firewall_whitelist", "firewall.dll", _serverVersion, PluginActivationStateType.Skip, true),
        new Plugin("keyring", "keyring.dll"),
        new Plugin("authentication_windows", "authentication_windows.dll")
      };
      //If you want to register more server plugins for the installer to be included in the list, you could do it in here.
      //Follow the examples from above.
      //You could manage how these are enabled / disabled from their respective UI's like Settings.Plugins.Enable("x").
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginsList"/> class.
    /// </summary>
    public PluginsList(Version serverVersion)
    {
      Initialize(serverVersion);
    }

    /// <summary>
    /// Load plugins configuration from the value for the plugin_load key at the ini file.
    /// </summary>
    /// <param name="pluginLoadValue">The value for the plugin_load key at the ini file.</param>
    public void LoadFromIniFile(string pluginLoadValue)
    {
      Plugins.Where(p => pluginLoadValue.Contains(p.Name)).ToList().ForEach(plugin => { plugin.Enabled = true; });
    }

    /// <summary>
    /// Returns a string with the plugins text used in a configuration file.
    /// </summary>
    /// <returns>A string with the plugins text used in a configuration file.</returns>
    public override string ToString()
    {
      var result = new StringBuilder();
      foreach (var plugin in Plugins)
      {
        if (!plugin.Enabled
            || plugin.RequiresInstall)
        {
          continue;
        }

        if (result.Length > 0)
        {
          result.Append(";");
        }

        result.Append(plugin);
      }

      return result.ToString();
    }

    /// <summary>
    /// Enables the plugin specified name.
    /// </summary>
    /// <param name="name">The plugin name.</param>
    /// <param name="enable">If set to <c>true</c> [enable].</param>
    public void Enable(string name, bool enable)
    {
      var firstOrDefault = Plugins.FirstOrDefault(p => p.Name == name);
      if (firstOrDefault != null)
        firstOrDefault.Enabled = enable;
    }

    public string GetActivationStateCommandOptions()
    {
      if(Plugins == null || Plugins.Count == 0)
      {
        return string.Empty;
      }

      return string.Join(" ", Plugins.Select(plugin => plugin.GetActivationStateCommandOption()));
    }

    /// <summary>
    /// Determines whether the specified plugin is enabled.
    /// </summary>
    /// <param name="name">The plugin name.</param>
    /// <returns>
    ///   <c>true</c> if the specified plugin is enabled; otherwise, <c>false</c>.
    /// </returns>
    public bool IsEnabled(string name)
    {
      var firstOrDefault = Plugins.FirstOrDefault(p => p.Name == name);
      return firstOrDefault != null && firstOrDefault.Enabled;
    }
  }
}

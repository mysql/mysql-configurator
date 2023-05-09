/* Copyright (c) 2016, 2023, Oracle and/or its affiliates.

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
using System.Text;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Enums;

namespace MySql.Configurator.Wizards.Server
{
  /// <summary>
  /// Represents a plugin that can be enabled or disabled by an installed product.
  /// </summary>
  [Serializable]

  public class Plugin
  {
    #region Fields

    /// <summary>
    /// The server version on which this plugin is used.
    /// </summary>
    private Version _serverVersion;

    #endregion

    /// <summary>
    /// Instantiates a new instance of the <see cref="Plugin"/> class.
    /// </summary>
    /// <param name="name">The name of the plugin.</param>
    public Plugin(string name)
    {
      if (string.IsNullOrEmpty(name))
      {
        throw new ArgumentNullException(nameof(name));
      }

      Name = name;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Plugin"/> class.
    /// </summary>
    /// <param name="name">The name of the plugin.</param>
    /// <param name="serverVersion">The server version on which this plugin is used.</param>
    /// <param name="libraryFile">The name of the library file associated with the plugin.</param>
    /// <param name="activationState">A <see cref="PluginActivationStateType"/> value.</param>
    /// <param name="enabled">if set to <c>true</c> [enabled].</param>
    /// <param name="isEarlyLoad">Indicates if the plugin is an early load plugin.</param>
    public Plugin(string name, string libraryFile = null, Version serverVersion = null, PluginActivationStateType activationState = PluginActivationStateType.NA, bool requiresInstall = false, bool enabled = false, bool isEarlyLoad = false)
        : this(name)
    {
      LibraryFile = libraryFile;
      Enabled = enabled;
      PluginActivationState = activationState;
      _serverVersion = serverVersion;
      IsEarlyLoad = isEarlyLoad;
      RequiresInstall = requiresInstall;
    }

    #region Properties
    /// <summary>
    /// Gets or sets a value indicating whether the plugin is enabled.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Gets the name of the library file associated with the plugin.
    /// </summary>
    public string LibraryFile { get; private set; }

    /// <summary>
    /// Gets the name of the plugin.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the Plugin Activation State option used at the Server Initialization/Starting command option
    /// </summary>
    public PluginActivationStateType PluginActivationState { get; private set; }

    /// <summary>
    /// Gets a value indicating if the plugin can be early loaded by the server.
    /// </summary>
    public bool IsEarlyLoad { get; private set; }

    /// <summary>
    /// Gets a value indicating if the plugin must be installed using the INSTALL command.
    /// </summary>
    public bool RequiresInstall { get; private set; }

    #endregion Properties

    /// <summary>
    /// Returns a string with the plugin text used in a configuration file.
    /// </summary>
    /// <returns>A string with the plugin text used in a configuration file.</returns>
    public override string ToString()
    {
      if (!Enabled
          || RequiresInstall)
      {
        return string.Empty;
      }

      var result = new StringBuilder(Name);
      if (!string.IsNullOrEmpty(LibraryFile))
      {
        result.AppendFormat("={0}", LibraryFile);
      }

      return result.ToString();
    }

    public string GetActivationStateCommandOption()
    {

      if (!Enabled)
      {
        return string.Empty;
      }


      string commandOption;

      switch (PluginActivationState)
      {
        case PluginActivationStateType.On:
        case PluginActivationStateType.Off:
        case PluginActivationStateType.Force:
        case PluginActivationStateType.ForcePlusPermanent:
          commandOption = $"--{Name}={PluginActivationState.GetDescription()}";
          break;
        case PluginActivationStateType.Enable:
        case PluginActivationStateType.Skip:
          commandOption = _serverVersion != null
                          && _serverVersion.ServerAutomaticallySkipsLoadForNonEarlyPlugins()
                            ? string.Empty
                            : $"--loose-{PluginActivationState.GetDescription()}-{Name}";
          break;
        default:
          commandOption = string.Empty;
          break;
      }

      return commandOption;
    }
  }
}

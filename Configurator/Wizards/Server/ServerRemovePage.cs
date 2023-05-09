/* Copyright (c) 2019, 2023, Oracle and/or its affiliates.

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
using System.Windows.Forms;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Core.Wizard;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Properties;

namespace MySql.Configurator.Wizards.Server
{
  /// <summary>
  /// Removal configuration page used when the server is being uninstalled.
  /// </summary>
  public partial class ServerRemovePage : ConfigWizardPage
  {
    #region Fields

    /// <summary>
    /// The <see cref="ServerConfigurationController"/> of the local Server instance.
    /// </summary>
    private readonly ServerConfigurationController _controller;

    /// <summary>
    /// Flag indicating whether a connection could be established with the given credentials.
    /// </summary>
    private bool _credsOk;

    /// <summary>
    /// The settings object being used based on the selected server configuration type.
    /// </summary>
    private MySqlServerSettings _settings;

    /// <summary>
    /// The password that has been validated by testing the connection.
    /// </summary>
    private string _validatedPassword;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerRemovePage"/> class.
    /// </summary>
    /// <param name="controller">The <see cref="ServerConfigurationController"/> of the local Server instance.</param>
    /// <param name="packageVersion">The version of the package being removed.</param>
    public ServerRemovePage(ServerConfigurationController controller, string packageVersion = null)
    {
      ValidConfigureTypes = ConfigurationType.New
        | ConfigurationType.Reconfiguration
        | ConfigurationType.Upgrade
        | ConfigurationType.Remove;
      InitializeComponent();
      _controller = controller;
      _credsOk = false;
      _validatedPassword = null;
      if (_controller.ConfigurationType != ConfigurationType.Remove)
      {
        captionLabel.Text = Resources.ServerRemovePageReconfigurationTitle;
      }
      else
      {
        captionLabel.Text = string.Format(Resources.ServerRemovePageRemoveTitle);
      }

      _settings = _controller.ConfigurationType == ConfigurationType.Remove
        || _controller.ConfigurationType == ConfigurationType.New
        ? _controller.Settings
        : _controller.ConfigurationType == ConfigurationType.Upgrade
          && _controller.OldSettings == null
            ? _controller.Settings
            : _controller.OldSettings;
      if (_settings == null)
      {
        return;
      }

      UpdatePageVisibility();
    }

    #region Properties

    /// <summary>
    /// Disables the Next button if not all required values are set.
    /// </summary>
    /// <returns><c>true</c> if the Next button should be enabled, <c>false</c> otherwise.</returns>
    public override bool NextOk => _settings.InnoDbClusterType != ServerConfigurationType.AddToCluster
                                   || ((_controller.ConfigurationType != ConfigurationType.Upgrade
                                        || _credsOk))
                                   && base.NextOk;

    #endregion Properties

    /// <summary>
    /// Executes actions performed when the Next button is clicked.
    /// </summary>
    /// <returns><c>true</c> if it the configuration should proceed to the next panel, <c>false</c> otherwise.</returns>
    public override bool Next()
    {
      // Set the controller properties.
      if (_controller.ConfigurationType == ConfigurationType.Remove)
      {
        _controller.RemoveDataDirectory = RemoveDataDirectoryPanel.Visible && RemoveDataDirectoryCheckBox.Checked;
      }

      if (_controller.ConfigurationType == ConfigurationType.Upgrade
          && _settings.InnoDbClusterType == ServerConfigurationType.AddToCluster)
      {
        _controller.Settings.ExistingRootPassword =
          _controller.Settings.RootPassword =
            _validatedPassword;
      }

      return base.Next();
    }

    /// <summary>
    /// Updates the state of the buttons on the wizard.
    /// </summary>
    protected override void UpdateButtons()
    {
      Wizard.NextButton.Enabled = NextOk;
      base.UpdateButtons();
    }

    /// <summary>
    /// Sets the visibility of the wizard page and the dynamic panels based on the server installation and configuration types.
    /// </summary>
    public void UpdatePageVisibility()
    {
      PageVisible = false;
      RemoveDataDirectoryPanel.Visible = false;
      if (_controller == null
          || _settings == null)
      {
        return;
      }

      // Removing the data directory is exclusive to a Remove/Uninstall operation.
      if (_controller.ConfigurationType == ConfigurationType.Remove
          && _controller.IsThereServerDataFiles)
      {
        RemoveDataDirectoryPanel.Visible = true;
        PageVisible = true;
        return;
      }

      if (_controller.ConfigurationType == ConfigurationType.New
          && _settings.InnoDbClusterType != ServerConfigurationType.Sandbox)
      {
        return;
      }

      PageVisible = true;
    }
  }
}

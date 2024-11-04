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

using MySql.Configurator.Base.Enums;
using MySql.Configurator.Properties;
using MySql.Configurator.Core.Server;

namespace MySql.Configurator.UI.Wizards.ServerConfigPages
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
    public override bool NextOk => (_controller.ConfigurationType != ConfigurationType.Upgrade
                                    || _credsOk)
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

      PageVisible = true;

      // Removing the data directory is exclusive to a Remove/Uninstall operation.
      if (_controller.ConfigurationType == ConfigurationType.Remove
          && _controller.IsThereServerDataFiles)
      {
        RemoveDataDirectoryPanel.Visible = true;
      }
    }
  }
}

/* Copyright (c) 2025, Oracle and/or its affiliates.

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
using MySql.Configurator.Core.Server;
using MySql.Configurator.Base.Classes;
using System.Windows.Forms;

namespace MySql.Configurator.UI.Wizards.ServerConfigPages
{
  /// <summary>
  /// Configuration page used to setup the MySQL Enterprise Firewall.
  /// </summary>
  public partial class ServerConfigEnterpriseFirewall : ConfigWizardPage
  {
    #region Fields

    /// <summary>
    /// The <see cref="ServerConfigurationController"/> of the local Server instance.
    /// </summary>
    private readonly ServerConfigurationController _controller;

    /// <summary>
    /// The server settings.
    /// </summary>
    private MySqlServerSettings _settings;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerConfigEnterpriseFirewall"/> class.
    /// </summary>
    /// <param name="controller">The <see cref="ServerConfigurationController"/> of the local Server instance.</param>
    public ServerConfigEnterpriseFirewall(ServerConfigurationController controller)
    {
      InitializeComponent();
      _controller = controller;
      _settings = _controller.Settings;
      UpdateControls();
    }

    #region Properties

    #endregion Properties

    /// <summary>
    /// Executes actions performed when the Next button is clicked.
    /// </summary>
    /// <returns><c>true</c> if it the configuration should proceed to the next panel, <c>false</c> otherwise.</returns>
    public override bool Next()
    {
      _settings.UpgradeEnterpriseFirewall = (_controller.ConfigurationType == ConfigurationType.Reconfigure
                                             && UpgradeEnterpriseFirewallPanel.Visible
                                             && UpgradeEnterpriseFirewallCheckBox.Checked)
                                            || (_controller.ConfigurationType == ConfigurationType.Upgrade
                                                && UpgradeEnterpriseFirewallCheckBox.Checked);

      _settings.EnableEnterpriseFirewall = (_controller.ConfigurationType == ConfigurationType.Reconfigure
                                            && ((UpgradeEnterpriseFirewallPanel.Visible
                                                 && UpgradeEnterpriseFirewallCheckBox.Checked)
                                                || EnableDisableEnterpriseFirewallCheckBox.Checked))
                                           || (_controller.ConfigurationType == ConfigurationType.Configure
                                               && EnableDisableEnterpriseFirewallCheckBox.Checked);

      var enablePlugins = _controller.ConfigurationType == ConfigurationType.Reconfigure
                          && _settings.EnterpriseFirewallEnabled
                          && !_settings.EnterpriseFirewallComponentEnabled;
      _settings.Plugins.Enable("mysql_firewall", enablePlugins);
      _settings.Plugins.Enable("mysql_firewall_users", enablePlugins);
      _settings.Plugins.Enable("mysql_firewall_whitelist", enablePlugins);
      _controller.UpdateConfigurationSteps();
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
    /// Sets the status and visibility of the dynamic panels and controls.
    /// </summary>
    public void UpdateControls()
    {
      switch (_controller.ConfigurationType)
      {
        case ConfigurationType.Reconfigure:
          if (_settings.GeneralSettings.EnterpriseFirewallComponentEnabled)
          {
            UpgradeEnterpriseFirewallPanel.Visible = false;
            EnableDisableEnterpriseFirewallPanel.Visible = true;
            EnableDisableEnterpriseFirewallCheckBox.Checked = true;
          }
          else if (_settings.GeneralSettings.EnterpriseFirewallEnabled)
          {
            EnableDisableEnterpriseFirewallPanel.Visible = true;
            EnableDisableEnterpriseFirewallCheckBox.Checked = true;
            UpgradeEnterpriseFirewallPanel.Visible = true;
            EnableDisableEnterpriseFirewallCheckBox.Checked = true;
          }
          else
          {
            UpgradeEnterpriseFirewallPanel.Visible = false;
            EnableDisableEnterpriseFirewallPanel.Visible = true;
            EnableDisableEnterpriseFirewallCheckBox.Checked = false;
          }

          break;

        case ConfigurationType.Upgrade:
          EnableDisableEnterpriseFirewallPanel.Visible = false;
          UpgradeEnterpriseFirewallPanel.Visible = true;
          EnableDisableEnterpriseFirewallCheckBox.Checked = false;
          break;

        case ConfigurationType.Configure:
        default:
          EnableDisableEnterpriseFirewallPanel.Visible = true;
          UpgradeEnterpriseFirewallPanel.Visible = false;
          EnableDisableEnterpriseFirewallCheckBox.Checked = false;
          break;
      }

      UpgradeEnterpriseFirewallCheckBox.Checked = false;
    }

    private void EnterpriseFirewallDocumentationLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Utilities.OpenBrowser(@"https://dev.mysql.com/doc/refman/en/firewall.html");
    }

    /// <summary>
    /// Handles the CheckedChanged event for the upgrade checkbox.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void UpgradeEnterpriseFirewallCheckBox_CheckedChanged(object sender, System.EventArgs e)
    {
      if (_controller.ConfigurationType != ConfigurationType.Reconfigure)
      {
        return;
      }

      EnableDisableEnterpriseFirewallPanel.Visible = !UpgradeEnterpriseFirewallCheckBox.Checked; 
    }
  }
}

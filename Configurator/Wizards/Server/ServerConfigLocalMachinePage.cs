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
using System.Linq;
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Core.Classes.MySql;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Core.Wizard;
using MySql.Configurator.Properties;

namespace MySql.Configurator.Wizards.Server
{
  public partial class ServerConfigLocalMachinePage : ConfigWizardPage
  {
    #region Fields

    private readonly ServerConfigurationController _controller;
    private MySqlServerSettings _oldSettings;
    private MySqlServerSettings _settings;
    #endregion Fields

    public ServerConfigLocalMachinePage(ServerConfigurationController controller)
    {
      InitializeComponent();
      _controller = controller;

      ConfigTypeComboBox.AddItem(Resources.developer_machine, Resources.ConfigMachineTypeDeveloper, Resources.ConfigMachineTypeDeveloperDescription);
      ConfigTypeComboBox.AddItem(Resources.server_machine, Resources.ConfigMachineTypeServer, Resources.ConfigMachineTypeServerDescription);
      ConfigTypeComboBox.AddItem(Resources.dedicated_machine, Resources.ConfigMachineTypeDedicated, Resources.ConfigMachineTypeDedicateDescription);
      ConfigTypeComboBox.AddItem(Resources.ServerCategoryIcon, Resources.ConfigMachineTypeManual, Resources.ConfigMachineTypeManualDescription);
    }

    #region Properties

    public override bool NextOk => (NamedPipeCheckBox.Checked
                                    || SharedMemoryCheckBox.Checked
                                    || TcpIpCheckBox.Checked)
                                   && base.NextOk;

    #endregion Properties

    public override void Activate()
    {
      PortTextBox.Text = _settings.Port.ToString();
      XProtocolPortTextBox.Text = _settings.MySqlXPort <= 0 ? MySqlServerSettings.X_PROTOCOL_DEFAULT_PORT.ToString() : _settings.MySqlXPort.ToString();
      TcpIpCheckBox.Checked = _settings.EnableTcpIp;
      TcpIpCheckBox_CheckedChanged(null, null);

      OpenWindowsFirewallCheckBox.Checked = _settings.OpenFirewall;

      PipeNameTextBox.Text = _settings.PipeName;
      NamedPipeCheckBox.Checked = _settings.EnableNamedPipe;
      NamedPipeCheckBox_CheckedChanged(null, null);

      SharedMemoryNameTextBox.Text = _settings.SharedMemoryName;
      SharedMemoryCheckBox.Checked = _settings.EnableSharedMemory;
      SharedMemoryCheckBox_CheckedChanged(null, null);

      ShowAdvancedLoggingOptionsCheckBox.Checked = _controller.ShowAdvancedOptions;
      HandleAdvancedOptions(_controller.ShowAdvancedOptions);

      if (_controller.ConfigurationType == ConfigurationType.New || _controller.ConfigurationType == ConfigurationType.Reconfiguration)
      {
        var supportsEnterpriseFirewallConfiguration = _controller.SupportsEnterpriseFirewallConfiguration;
        EnterpriseFirewallCheckBox.Visible = supportsEnterpriseFirewallConfiguration;
        EnterpriseFirewallCheckBox.Checked = EnterpriseFirewallCheckBox.Enabled
                                             && _settings.Plugins.IsEnabled("mysql_firewall");
        ToolTip.SetToolTip(EnterpriseFirewallCheckBox, EnterpriseFirewallCheckBox.Enabled ? string.Empty : Resources.ServerConfigEnterpriseFirewallNotSupportedWithInnoDbCluster);
        EnterpriseFirewallDescription.Visible = supportsEnterpriseFirewallConfiguration;
        EnterpriseFirewallTitleLabel.Visible = supportsEnterpriseFirewallConfiguration;
        EnterpriseFirewallLinkLabel.Visible = supportsEnterpriseFirewallConfiguration;
      }
      XProtocolPortPanel.Visible = _controller.ServerVersion.ServerSupportsXProtocol();
      OpenWindowsFirewallCheckBox.Text = XProtocolPortPanel.Visible ? Resources.OpenFirewallPorts : Resources.OpenFirewallPort;
      SetDefaultOptions();
      base.Activate();
    }

    public override bool Next()
    {
      switch (ConfigTypeComboBox.SelectedIndex)
      {
        case 0:
          _settings.ServerInstallType = ServerInstallationType.Developer;
          break;

        case 1:
          _settings.ServerInstallType = ServerInstallationType.Server;
          break;

        case 2:
          _settings.ServerInstallType = ServerInstallationType.Dedicated;
          break;

        case 3:
          _settings.ServerInstallType = ServerInstallationType.Manual;
          break;
      }

      _settings.OpenFirewall = OpenWindowsFirewallCheckBox.Checked;
      _settings.EnableTcpIp = TcpIpCheckBox.Checked;
      if (_settings.EnableTcpIp)
      {
        _settings.Port = uint.Parse(PortTextBox.Text.Trim());
      }

      if (XProtocolPortPanel.Visible)
      {
        _settings.OpenFirewallForXProtocol = OpenWindowsFirewallCheckBox.Checked;
        _settings.MySqlXPort = uint.Parse(XProtocolPortTextBox.Text.Trim());
      }

      _settings.EnableNamedPipe = NamedPipeCheckBox.Checked;
      _settings.PipeName = PipeNameTextBox.Text.Trim();
      _settings.EnableSharedMemory = SharedMemoryCheckBox.Checked;
      _settings.SharedMemoryName = SharedMemoryNameTextBox.Text.Trim();

      Logger.LogInformation($"Advancing to next step using:\n\tService Name: {_settings.ServiceName}\n\tEnable TCP/IP: {TcpIpCheckBox.Checked}\n\tPort: {_settings.Port}\n\tEnable pipe: {NamedPipeCheckBox.Checked}\n\tPipe name: {PipeNameTextBox.Text.Trim()}\n\tEnable shared memory: {SharedMemoryCheckBox.Checked}\n\tMemory name {SharedMemoryNameTextBox.Text.Trim()}");

      var enable = EnterpriseFirewallCheckBox.Checked;
      _settings.Plugins.Enable("mysql_firewall", enable);
      _settings.Plugins.Enable("mysql_firewall_users", enable);
      _settings.Plugins.Enable("mysql_firewall_whitelist", enable);
      _settings.EnterpriseFirewallEnabled = enable;

      // Enable de Named Pipe configuration page if required.
      if (_controller.ServerVersion.ServerSupportsNamedPipeFullAccessGroupVariable())
      {
        var namedPipesPage = _controller.Pages.FirstOrDefault(page => page is ServerConfigNamedPipesPage);
        if (namedPipesPage != null)
        {
          namedPipesPage.PageVisible = _settings.EnableNamedPipe;
          Parent.Refresh();
        }
      }

      return base.Next();
    }

    public override void WizardShowing()
    {
      base.WizardShowing();

      _settings = _controller.Settings;
      _oldSettings = _controller.Settings.OldSettings as MySqlServerSettings;

      ConfigTypeComboBox.SelectedIndex = 0;
      if (_controller.ConfigurationType == ConfigurationType.Reconfiguration)
      {
        switch(_settings.ServerInstallType)
        {
          case ServerInstallationType.Dedicated:
            ConfigTypeComboBox.SelectedIndex = 2;
            break;

          case ServerInstallationType.Server:
            ConfigTypeComboBox.SelectedIndex = 1;
            break;

          case ServerInstallationType.Developer:
            ConfigTypeComboBox.SelectedIndex = 0;
            break;

          case ServerInstallationType.Manual:
            ConfigTypeComboBox.SelectedIndex = 3;
            break;

          default:
            ConfigTypeComboBox.SelectedIndex = 0;
            break;
        }
      }
    }

    /// <summary>
    /// Handles the TextChanged event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    protected override void TextChangedHandler(object sender, EventArgs e)
    {
      // Looks like we could get rid of this empty override, but it is necessary to avoid an error of:
      // The method 'xxx' cannot be the method for an event because a class this class derives from already defines the method
      base.TextChangedHandler(sender, e);
    }

    /// <summary>
    /// Handles the TextValidated event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    /// <remarks>This event method is meant to be used with the <see cref="Control.Validated"/> event.</remarks>
    protected override void ValidatedHandler(object sender, EventArgs e)
    {
      // Looks like we could get rid of this empty override, but it is necessary to avoid an error of:
      // The method 'xxx' cannot be the method for an event because a class this class derives from already defines the method
      base.ValidatedHandler(sender, e);
    }

    /// <summary>
    /// Contains calls to methods that validate the given control's value.
    /// </summary>
    /// <returns>An error message or <c>null</c> / <see cref="string.Empty"/> if everything is valid.</returns>
    protected override string ValidateFields()
    {
      string errorMessage = base.ValidateFields();
      switch (ErrorProviderControl.Name)
      {
        case nameof(PortTextBox):
          errorMessage = ValidatePort(PortTextBox.Text, XProtocolPortPanel.Visible ? XProtocolPortTextBox.Text : null, _oldSettings.Port);
          break;

        case nameof(XProtocolPortTextBox):
          errorMessage = ValidatePort(XProtocolPortTextBox.Text, PortTextBox.Text, _oldSettings.MySqlXPort);
          break;

        case nameof(PipeNameTextBox):
          errorMessage = ValidatePipeAndSharedMemoryNames(true);
          break;

        case nameof(SharedMemoryNameTextBox):
          errorMessage = ValidatePipeAndSharedMemoryNames(false);
          break;
      }

      return errorMessage;
    }

    private void EnterpriseFirewallLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Core.Classes.Utilities.OpenBrowser(@"https://dev.mysql.com/doc/en/firewall.html ");
    }

    private void HandleAdvancedOptions(bool show)
    {
      var loggingOptions = _controller.Pages.OfType<ServerConfigLoggingOptionsPage>().FirstOrDefault();
      if (loggingOptions != null)
      {
        loggingOptions.PageVisible = show;
        _controller.ShowAdvancedOptions = show;
      }

      var advancedOptions = _controller.Pages.OfType<ServerConfigAdvancedOptionsPage>().FirstOrDefault();
      if (advancedOptions != null)
      {
        advancedOptions.PageVisible = show;
      }

      Parent.Refresh();
      _controller.ShowAdvancedOptions = show;
    }

    private void NamedPipeCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      PipeNameTextBox.Enabled = NamedPipeCheckBox.Checked;
      ValidatedHandler(PipeNameTextBox, EventArgs.Empty);
    }

    private void SharedMemoryCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      SharedMemoryNameTextBox.Enabled = SharedMemoryCheckBox.Checked;
      ValidatedHandler(SharedMemoryNameTextBox, EventArgs.Empty);
    }

    private void ShowAdvancedLoggingOptionsCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      HandleAdvancedOptions(ShowAdvancedLoggingOptionsCheckBox.Checked);
    }

    private void TcpIpCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      OpenWindowsFirewallCheckBox.Checked = TcpIpCheckBox.Checked;
      OpenWindowsFirewallCheckBox.Enabled = TcpIpCheckBox.Checked;
      PortTextBox.Enabled = TcpIpCheckBox.Checked;
      ValidatedHandler(PortTextBox, EventArgs.Empty);
    }

    private string ValidatePipeAndSharedMemoryNames(bool namedPipe)
    {
      var errorMessage = string.Empty;
      if (namedPipe && !NamedPipeCheckBox.Checked
          || !namedPipe && !SharedMemoryCheckBox.Checked)
      {
        return errorMessage;
      }

      var textBox = namedPipe ? PipeNameTextBox : SharedMemoryNameTextBox;
      string name = textBox.Text.Trim();
      errorMessage = MySqlServerInstance.ValidatePipeOrSharedMemoryName(namedPipe, name);
      if (!string.IsNullOrEmpty(errorMessage))
      {
        return errorMessage;
      }

      bool exists = _controller.PipeOrMemoryNameExists(name, namedPipe);
      if (_controller.ConfigurationType != ConfigurationType.New)
      {
        var oldName = namedPipe ? _oldSettings.PipeName : _oldSettings.SharedMemoryName;
        exists &= !oldName.Equals(name, StringComparison.OrdinalIgnoreCase);
      }

      return exists
        ? string.Format(namedPipe ? Resources.PipeAlreadyExists : Resources.MemoryAlreadyExists, name)
        : string.Empty;
    }

    private string ValidatePort(string portAsText, string otherPortAsText, uint oldPort)
    {
      var validationErrorMessage = MySqlServerInstance.ValidatePortNumber(portAsText, false);
      if (string.IsNullOrEmpty(validationErrorMessage))
      {
        validationErrorMessage = ValidatePortInUse(Convert.ToUInt32(portAsText), oldPort);
      }

      if (string.IsNullOrEmpty(validationErrorMessage)
          && !string.IsNullOrEmpty(otherPortAsText)
          && otherPortAsText.Equals(portAsText, StringComparison.Ordinal))
      {
        validationErrorMessage = Resources.SameCSPortXPort;
      }

      return validationErrorMessage;
    }

    private string ValidatePortInUse(uint port, uint oldPort)
    {
      if (Core.Classes.Utilities.PortIsAvailable(port))
      {
        return string.Empty;
      }

      if (_controller.ConfigurationType == ConfigurationType.New)
      {
        return Resources.ServerConfigPortInUse;
      }

      if ((_controller.ConfigurationType == ConfigurationType.Reconfiguration || _controller.ConfigurationType == ConfigurationType.Upgrade) && port != oldPort)
      {
        return Resources.ServerConfigPortInUse;
      }

      return string.Empty;
    }

    /// <summary>
    /// Sets the default options for this configuration page.
    /// </summary>
    private void SetDefaultOptions()
    {
      TcpIpCheckBox.Enabled = true;
      NamedPipeCheckBox.Enabled = true;
      PipeNameTextBox.Enabled = _settings.EnableNamedPipe;
      SharedMemoryCheckBox.Enabled = true;
      SharedMemoryNameTextBox.Enabled = _settings.EnableSharedMemory;
    }
  }
}
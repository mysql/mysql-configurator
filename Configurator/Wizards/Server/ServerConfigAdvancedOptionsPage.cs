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
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Core.Wizard;
using MySql.Configurator.Properties;

namespace MySql.Configurator.Wizards.Server
{
  public partial class ServerConfigAdvancedOptionsPage : ConfigWizardPage
  {
    #region Fields

    private readonly ServerConfigurationController _controller;

    #endregion Fields

    public ServerConfigAdvancedOptionsPage(ServerConfigurationController controller)
    {
      InitializeComponent();
      _controller = controller;
    }

    #region Properties

    public override bool NextOk => base.NextOk
                                   || ValidationsErrorProvider.GetError(ServerIdTextBox).Equals(Resources.ServerConfigSuggestServerId, StringComparison.OrdinalIgnoreCase);

    #endregion Properties

    public override void Activate()
    {
      ServerIdTextBox.Text = _controller.Settings.ServerId.HasValue ? _controller.Settings.ServerId.ToString() : string.Empty;
      if (_controller.Settings.LowerCaseTableNames == LowerCaseTableNamesTypes.LowerCaseStoredInsensitiveComparison)
      {
        LowerCaseRadioButton.Checked = true;
      }
      else
      {
        PreserveGivenCaseRadioButton.Checked = true;
      }

      UpdateServerIdReadOnlyStatus();
      UpdateTableNamesControlsEnabledStatus();
      base.Activate();
    }

    public override bool Next()
    {
      _controller.Settings.ServerId = string.IsNullOrEmpty(ServerIdTextBox.Text) ? (uint?)null : Convert.ToUInt32(ServerIdTextBox.Text);
      _controller.Settings.LowerCaseTableNames = LowerCaseRadioButton.Checked
        ? LowerCaseTableNamesTypes.LowerCaseStoredInsensitiveComparison
        : LowerCaseTableNamesTypes.SpecifiedCaseStoredInsensitiveComparison;
      return base.Next();
    }

    /// <summary>
    /// Updates the ReadOnly property for the Server ID textbox.
    /// </summary>
    private void UpdateServerIdReadOnlyStatus()
    {
      ServerIdTextBox.ReadOnly = !_controller.IsUpdateServerIdSupported;
      ToolTip.SetToolTip(ServerIdTextBox, !ServerIdTextBox.ReadOnly
        ? Resources.ServerConfigServerIdDuringInnoDbClusterReconfigurationSupported
        : Resources.ServerConfigServerIdDuringInnoDbClusterReconfigurationNotSupported);
    }

    /// <summary>
    /// Updates the Enabled property for the Tables Names related controls.
    /// </summary>
    private void UpdateTableNamesControlsEnabledStatus()
    {
      bool enabled = _controller.ServerVersion.ServerSupportsLowerCaseTableNamesModification()
                     || _controller.IsInitializeServerConfigurationStepNeeded(false);
      LowerCaseTableNamesLabel.Enabled = enabled;
      LowerCaseRadioButton.Enabled = enabled;
      LowerCaseDescriptionLabel.Enabled = enabled;
      PreserveGivenCaseRadioButton.Enabled = enabled;
      PreserveGivenCaseDescriptionLabel.Enabled = enabled;
      ToolTip.SetToolTip(LowerCaseRadioButton, LowerCaseRadioButton.Enabled
        ? Resources.ServerConfigLowerCaseTableNamesSupported
        : Resources.ServerConfigTableNamesNotSupported);
      ToolTip.SetToolTip(PreserveGivenCaseRadioButton, PreserveGivenCaseRadioButton.Enabled
        ? Resources.ServerConfigPreserveGivenCaseSupported
        : Resources.ServerConfigTableNamesNotSupported);
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
        case nameof(ServerIdTextBox):
          errorMessage = ValidateServerId(ErrorProviderControl.Text);
          break;
      }

      return errorMessage;
    }

    private string ValidateServerId(string text)
    {
      ErrorProperties.ErrorIcon = null;
      if (string.IsNullOrEmpty(text))
      {
        if (_controller.Settings.EnableGeneralLog
            || _controller.Settings.EnableSlowQueryLog
            || _controller.Settings.EnableBinLog)
        {
          ErrorProperties.ErrorIcon = Resources.warning_sign_icon;
          return Resources.ServerConfigSuggestServerId;
        }

        return string.Empty;
      }

      bool isValid = uint.TryParse(text, out var serverId);
      if (!isValid)
      {
        return Resources.ServerConfigServerIdError;
      }

      _controller.Settings.ServerId = serverId;
      return string.Empty;
    }
  }
}
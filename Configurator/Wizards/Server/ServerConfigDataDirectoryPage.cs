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
using System.IO;
using System.Windows.Forms;
using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Core.Wizard;
using MySql.Configurator.Dialogs;

namespace MySql.Configurator.Wizards.Server
{
  public partial class ServerConfigDataDirectoryPage : ConfigWizardPage
  {
    #region Fields

    /// <summary>
    /// A configuration controller for a MySQL Server configuration.
    /// </summary>
    private readonly ServerConfigurationController _controller;

    #endregion Fields

    public ServerConfigDataDirectoryPage(ServerConfigurationController controller)
    {
      InitializeComponent();
      _controller = controller;
      ErrorProperties.IconPadding += 65;
    }

    public override void Activate()
    {
      DataDirectoryTextBox.Text = _controller.Settings.DataDirectory;
      FireAllValidations();
      base.Activate();
    }

    public override bool Next()
    {
      _controller.Settings.DataDirectory = DataDirectoryTextBox.Text;
      _controller.Settings.IniDirectory = _controller.Settings.DataDirectory;
      _controller.Settings.SecureFilePrivFolder = Path.Combine(_controller.Settings.IniDirectory, MySqlServerSettings.SECURE_FILE_PRIV_DIRECTORY);

      var mainForm = FindForm() as MainForm;
      if (mainForm != null)
      {
        mainForm.DataDirectoryLabel.Text = $"Data Directory: {_controller.Settings.DataDirectory}";
        Logger.LogInformation($"Status: {mainForm.ConfigurationTypeLabel.Text};{mainForm.VersionLabel.Text};{mainForm.DataDirectoryLabel.Text}");
        mainForm.StatusStrip.Refresh();
      }

      return base.Next();
    }

    /// <summary>
    /// Contains calls to methods that validate the given control's value.
    /// </summary>
    /// <returns>An error message or <c>null</c> / <see cref="string.Empty"/> if everything is valid.</returns>
    protected override string ValidateFields()
    {
      string errorMessage = base.ValidateFields();
      if (ErrorProviderControl.Enabled)
      {
        ErrorProviderControl.Text = ErrorProviderControl.Text.Trim();
        switch (ErrorProviderControl.Name)
        {
          case nameof(DataDirectoryTextBox):
            errorMessage = Core.Classes.Utilities.ValidateAbsoluteFilePath(ErrorProviderControl.Text, true);
            break;
        }
      }

      return errorMessage;
    }

    #region Event Handlers

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

    private void DataDirectoryBrowseButton_Click(object sender, EventArgs e)
    {
      DataDirectoryBrowserDialog.SelectedPath = DataDirectoryTextBox.Text;
      if (DataDirectoryBrowserDialog.ShowDialog() == DialogResult.OK)
      {
        DataDirectoryTextBox.Text = DataDirectoryBrowserDialog.SelectedPath;
      }
    }

    private void DataDirectoryRevertButton_Click(object sender, EventArgs e)
    {
      DataDirectoryTextBox.Text = _controller.Settings.DefaultDataDirectory;
    }

    #endregion Event Handlers
  }
}

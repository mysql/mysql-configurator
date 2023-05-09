/* Copyright (c) 2022, 2023, Oracle and/or its affiliates.

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
using System.DirectoryServices;
using System.IO;
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Core.Wizard;
using MySql.Configurator.Properties;

namespace MySql.Configurator.Wizards.Server
{
  /// <summary>
  /// Wizard page used to configure access to the named pipes.
  /// </summary>
  public partial class ServerConfigNamedPipesPage : ConfigWizardPage
  {
    #region Fields

    /// <summary>
    /// The server configuration controller.
    /// </summary>
    private readonly ServerConfigurationController _controller;

    /// <summary>
    /// The settings for the current configuration.
    /// </summary>
    private MySqlServerSettings _settings;

    #endregion Fields

    public ServerConfigNamedPipesPage(ServerConfigurationController controller)
    {
      InitializeComponent();
      _controller = controller;
    }

    #region Properties

    public override bool NextOk => (MinimumNecessaryAccessControlRadioButton.Checked
                                   || FullAcessRadioButton.Checked
                                   || (LocalGroupRadioButton.Checked
                                       && LocalGroupNameComboBox.SelectedItem != null))
                                   && base.NextOk;

    #endregion Properties

    public override void Activate()
    {
      if (string.IsNullOrEmpty(_settings.NamedPipeFullAccessGroup))
      {
        MinimumNecessaryAccessControlRadioButton.Checked = true;
      }
      else if (_settings.NamedPipeFullAccessGroup.Equals(ServerConfigurationController.NAMED_PIPE_FULL_ACCESS_TO_ALL_USERS))
      {
        FullAcessRadioButton.Checked = true;
      }
      else
      {
        LocalGroupRadioButton.Checked = true;
      }

      base.Activate();
    }

    public override bool Next()
    {
      _settings.NamedPipeFullAccessGroup = LocalGroupRadioButton.Checked
                                             ? LocalGroupNameComboBox.SelectedItem.ToString()
                                             : FullAcessRadioButton.Checked
                                               ? ServerConfigurationController.NAMED_PIPE_FULL_ACCESS_TO_ALL_USERS
                                               : string.Empty;
      return base.Next();
    }

    /// <summary>
    /// Handles the WizardShowing event.
    /// </summary>
    public override void WizardShowing()
    {
      base.WizardShowing();
      _settings = _controller.Settings;
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
        case nameof(LocalGroupNameComboBox):
          var groupName = LocalGroupNameComboBox.SelectedItem.ToString();
          if (string.IsNullOrEmpty(groupName))
          {
            return string.Empty;
          }

          Cursor = Cursors.WaitCursor;
          bool groupExists = DirectoryServicesWrapper.GroupExists(groupName);
          Cursor = Cursors.Default;
          if (!groupExists)
          {
            errorMessage = Resources.ServerConfigLocalGroupDoesNotExist;
          }

          break;
      }

      return errorMessage;
    }


    /// <summary>
    /// Handles the SelectedIndexChanged event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void GroupNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      ValidatedHandler(LocalGroupNameComboBox, EventArgs.Empty);
    }

    /// <summary>
    /// Gets and loads the Windows groups in the local machine into the Group Name combobox.
    /// </summary>
    private void LoadWindowsGroups()
    {
      Cursor = Cursors.WaitCursor;
      LocalGroupNameComboBox.Items.Clear();
      try
      {
        using (DirectoryEntry computerEntry = new DirectoryEntry($"WinNT://{Environment.MachineName},computer"))
        {
          foreach (DirectoryEntry childEntry in computerEntry.Children)
          {
            if (!childEntry.SchemaClassName.Equals("Group"))
            {
              continue;
            }

            LocalGroupNameComboBox.Items.Add(childEntry.Name);
          }
        }
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
      }

      LocalGroupNameComboBox.Refresh();
      Cursor = Cursors.Default;
    }

    /// <summary>
    /// Handles the Click event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void RefreshButton_Click(object sender, EventArgs e)
    {
      LoadWindowsGroups();
    }

    private void MinimumNecessaryAccessControlRadioButton_CheckedChanged(object sender, EventArgs e)
    {
      var radioButton = sender as RadioButton;
      if (radioButton != null
          && !radioButton.Checked)
      {
        return;
      }

      MinimumNecessaryAccessDescriptionLabel.Enabled = true;
      LocalGroupDescriptionLabel.Enabled = false;
      LocalGroupPanel.Enabled = false;
      LocalGroupWarningPanel.Enabled = false;
      FullAccessWarningPanel.Enabled = false;
      Wizard.NextButton.Enabled = NextOk;
    }

    private void LocalGroupRadioButton_CheckedChanged(object sender, EventArgs e)
    {
      var radioButton = sender as RadioButton;
      if (radioButton != null
          && !radioButton.Checked)
      {
        return;
      }

      if (LocalGroupNameComboBox.Items == null
          || LocalGroupNameComboBox.Items.Count == 0)
      {
        LoadWindowsGroups();
        if (!string.IsNullOrEmpty(_settings.NamedPipeFullAccessGroup)
            && LocalGroupNameComboBox.Items.Contains(_settings.NamedPipeFullAccessGroup))
        {
          LocalGroupNameComboBox.SelectedItem = _settings.NamedPipeFullAccessGroup;
        }
      }

      MinimumNecessaryAccessDescriptionLabel.Enabled = false;
      LocalGroupDescriptionLabel.Enabled = true;
      LocalGroupPanel.Enabled = true;
      LocalGroupWarningPanel.Enabled = true;
      FullAccessWarningPanel.Enabled = false;
      Wizard.NextButton.Enabled = NextOk;
    }

    private void GrantFullAcessToEveryoneRadioButton_CheckedChanged(object sender, EventArgs e)
    {
      var radioButton = sender as RadioButton;
      if (radioButton != null
          && !radioButton.Checked)
      {
        return;
      }

      MinimumNecessaryAccessDescriptionLabel.Enabled = false;
      LocalGroupDescriptionLabel.Enabled = false;
      LocalGroupPanel.Enabled = false;
      LocalGroupWarningPanel.Enabled = false;
      FullAccessWarningPanel.Enabled = true;
      Wizard.NextButton.Enabled = NextOk;
    }

    private void NamedPipeFullAccessGroupDocumentationLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Core.Classes.Utilities.OpenBrowser("https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_named_pipe_full_access_group");
    }
  }
}
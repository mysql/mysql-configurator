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
using System.Linq;
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.Forms;
using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Core.Classes.MySql;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Core.Wizard;
using MySql.Configurator.Properties;

namespace MySql.Configurator.Wizards.Server
{
  public partial class ServerConfigUserAccountsPage : ConfigWizardPage
  {
    #region Fields

    private readonly ColumnSorter _addEditUsersListViewSorter;
    private ServerConfigurationController _controller;
    private bool _rootPasswordOk;
    private bool _dataDirectoryConfigured;
    private MySqlServerSettings _settings;
    private bool _showWinAuthOption;

    #endregion Fields

    public ServerConfigUserAccountsPage(ServerConfigurationController controller)
    {
      InitializeComponent();
      _controller = controller;
      _dataDirectoryConfigured = _controller.IsDataDirectoryConfigured;
      _settings = controller.Settings;
      _rootPasswordOk = false;
      _addEditUsersListViewSorter = new ColumnSorter();
      UserAccountsListView.ListViewItemSorter = _addEditUsersListViewSorter;

      // Verify application resources are available.
      if (_controller.RolesDefined != null)
      {
        return;
      }

      UserAccountsListView.Enabled = false;
      AddUserButton.Enabled = false;
      EditUserButton.Enabled = false;
      DeleteUserButton.Enabled = false;
      RolesDefinedErrorProvider.SetProperties(UserAccountsLabel, new ErrorProviderProperties(Resources.ServerConfigMissingResources));
    }

    #region Properties

    public override bool NextOk => (!_dataDirectoryConfigured
                                    || _rootPasswordOk)
                                   && base.NextOk;

    #endregion Properties

    public override void Activate()
    {
      RootPasswordTextBox.Focus();
      SetControlsVisibility();
      FireAllValidations();
      base.Activate();
    }

    public override bool Next()
    {
      var currentRootPassword = CurrentRootPasswordTextBox.Text;
      if (CurrentRootPasswordTextBox.Visible)
      {
        // Reassign password to settings.
        _settings.ExistingRootPassword = currentRootPassword;
      }

      // Reassign password to settings.
      _settings.RootPassword = RootPasswordTextBox.Visible
        ? RootPasswordTextBox.Text
        : currentRootPassword;

      _settings.NewServerUsers.Clear();
      if (UserAccountPanel.Visible)
      {
        bool hasWindowsAccounts = false;
        foreach (var su in from ListViewItem li in UserAccountsListView.Items select li.Tag as ServerUser)
        {
          if (su.AuthenticationPlugin != MySqlAuthenticationPluginType.Windows
              && su.AuthenticationPlugin != _settings.DefaultAuthenticationPlugin)
          {
            su.AuthenticationPlugin = _settings.DefaultAuthenticationPlugin;
          }

          _settings.NewServerUsers.Add(su);
          hasWindowsAccounts = hasWindowsAccounts || (su.AuthenticationPlugin == MySqlAuthenticationPluginType.Windows);
          Logger.LogInformation($"Adding User: {su.Username}@{su.Host} as {su.UserRole.Display}");
        }

        _settings.Plugins.Enable("authentication_windows", hasWindowsAccounts);
      }

      return base.Next();
    }

    private void PasswordCheckButton_Click(object sender, EventArgs e)
    {
      Cursor = Cursors.WaitCursor;
      _rootPasswordOk = false;
      var providerProperties = new ErrorProviderProperties(Resources.ConnectionTestingText, Resources.MySQLInstallerConfig_InProgress, true);
      ConnectionErrorProvider.SetProperties(PasswordCheckButton, providerProperties);
      var connectionResult = LocalServerInstance.CanConnect(_controller, CurrentRootPasswordTextBox.Text, _controller.ConfigurationType == ConfigurationType.Reconfiguration);
      _rootPasswordOk = connectionResult == ConnectionResultType.ConnectionSuccess;
      providerProperties.ErrorIcon = _rootPasswordOk
        ? Resources.MySQLInstallerConfig_DoneIcon
        : Resources.MySQLInstallerConfig_ErrorIcon;
      providerProperties.ErrorMessage = connectionResult.GetDescription();
      ConnectionErrorProvider.SetProperties(PasswordCheckButton, providerProperties);
      Cursor = Cursors.Default;
      UpdateButtons();
    }

    private string CheckPasswords()
    {
      return !_dataDirectoryConfigured
        ? Core.Classes.Utilities.ValidatePasswords(RootPasswordTextBox.Text, RepeatPasswordTextBox.Text, PasswordStrengthLabel)
        : null;
    }

    private void EditUserItem()
    {
      var item = UserAccountsListView.SelectedItems[0];
      using (var dialog = new DatabaseUserDialog(_showWinAuthOption, _controller.RolesDefined.Roles, _settings.DefaultAuthenticationPlugin, _controller.ServerVersion))
      {
        dialog.ServerUser = item.Tag as ServerUser;
        if (dialog.ShowDialog() == DialogResult.Cancel)
        {
          return;
        }

        // Check to make sure a duplicate user/host was not entered.
        if ((from ListViewItem li in UserAccountsListView.Items
          let su = li.Tag as ServerUser
          where su.Username == dialog.ServerUser.Username
          where (su.Host == "%" || su.Host == dialog.ServerUser.Host) && li != item
          select li).Any())
        {
          InfoDialog.ShowDialog(InfoDialogProperties.GetWarningDialogProperties(Resources.ServerConfigDuplicateTitle, Resources.ServerConfigEditedDuplicateUser));
          return;
        }

        item.Tag = dialog.ServerUser;
        item.StateImageIndex = dialog.ServerUser.AuthenticationPlugin == MySqlAuthenticationPluginType.Windows ? 1 : 0;
        item.SubItems[1].Text = dialog.ServerUser.Username;
        item.SubItems[2].Text = dialog.ServerUser.Host;
        item.SubItems[3].Text = dialog.ServerUser.UserRole.Display;
      }
    }

    private void SetControlsVisibility()
    {
      if (_dataDirectoryConfigured)
      {
        CurrentRootPasswordLabel.Visible = true;
        CurrentRootPasswordTextBox.Visible = true;
        CurrentRootPasswordTextBox.Focus();
      }

      // Windows Authentication was introduced in server version 5.5.15
      _showWinAuthOption = _controller.Package.NormalizedVersion.ServerSupportsWindowsAuthentication()
                          && _controller.Package.License == LicenseType.Commercial;

      // Assign to variable to avoid firing getter more than once
      RootPasswordLabel.Visible = !_dataDirectoryConfigured;
      RootPasswordTextBox.Visible = !_dataDirectoryConfigured;
      RepeatPasswordLabel.Visible = !_dataDirectoryConfigured;
      RepeatPasswordTextBox.Visible = !_dataDirectoryConfigured;
      UserAccountPanel.Visible = !_dataDirectoryConfigured;
      PasswordStrengthLabel.Visible = !_dataDirectoryConfigured;
      PasswordCheckButton.Visible = _dataDirectoryConfigured;
      PasswordsTableLayoutPanel.SetColumnSpan(CurrentRootPasswordTextBox, _dataDirectoryConfigured ? 1 : 2);
    }

    private void UserAccountsListView_ColumnClick(object sender, ColumnClickEventArgs e)
    {
      if (e.Column == _addEditUsersListViewSorter.Column)
      {
        // Reverse the sort order
        _addEditUsersListViewSorter.Order = _addEditUsersListViewSorter.Order == SortOrder.Ascending
          ? SortOrder.Descending
          : SortOrder.Ascending;
      }
      else
      {
        _addEditUsersListViewSorter.Column = e.Column;
        _addEditUsersListViewSorter.Order = SortOrder.Ascending;
      }

      UserAccountsListView.Sort();
    }

    private void UserAccountsListView_DoubleClick(object sender, EventArgs e)
    {
      if (UserAccountsListView.SelectedIndices.Count > 0)
      {
        EditUserItem();
      }
    }

    private void UserAccountsListView_SelectedIndexChanged(object sender, EventArgs e)
    {
      EditUserButton.Enabled = UserAccountsListView.SelectedItems.Count > 0;
      DeleteUserButton.Enabled = UserAccountsListView.SelectedItems.Count > 0;
    }

    private void AddUserButton_Click(object sender, EventArgs e)
    {
      using (var dialog = new DatabaseUserDialog(_showWinAuthOption, _controller.RolesDefined.Roles, _settings.DefaultAuthenticationPlugin, _controller.ServerVersion))
      {
        if (dialog.ShowDialog() == DialogResult.Cancel)
        {
          return;
        }

        // Check to make sure the user did not try to add the existing default root accounts.
        string serverName = dialog.ServerUser.Host.ToLower();
        if (dialog.ServerUser.Username.Equals(MySqlServerUser.ROOT_USERNAME, StringComparison.OrdinalIgnoreCase)
            && (serverName == MySqlServerUser.LOCALHOST
                || serverName == "::1"
                || serverName == "127.0.0.1"))
        {
          InfoDialog.ShowDialog(
            InfoDialogProperties.GetErrorDialogProperties(
              string.Format(Resources.ServerConfigAddedExistingRootUserTitle, dialog.ServerUser.Host),
              string.Format(Resources.ServerConfigAddedExistingRootUserText, dialog.ServerUser.Host)));
          return;
        }

        // Check to make sure a duplicate user/host was not entered.
        if (UserAccountsListView.Items.Cast<ListViewItem>().Select(li => li.Tag as ServerUser).Any(su => su != null && su.Username == dialog.ServerUser.Username && su.Host == dialog.ServerUser.Host))
        {
          InfoDialog.ShowDialog(InfoDialogProperties.GetWarningDialogProperties(Resources.ServerConfigDuplicateTitle, Resources.ServerConfigAddedDuplicateUser));
          return;
        }

        var item = new ListViewItem
        {
          Tag = dialog.ServerUser,
          StateImageIndex = dialog.ServerUser.AuthenticationPlugin == MySqlAuthenticationPluginType.Windows ? 1 : 0
        };
        item.SubItems.Add(dialog.ServerUser.Username);
        item.SubItems.Add(dialog.ServerUser.Host);
        item.SubItems.Add(dialog.ServerUser.UserRole.Display);
        UserAccountsListView.Items.Add(item);
      }
    }

    private void EditUserButton_Click(object sender, EventArgs e)
    {
      EditUserItem();
    }

    private void DeleteUserButton_Click(object sender, EventArgs e)
    {
      if (UserAccountsListView.SelectedItems.Count == 1)
      {
        // Multi-select is false and removeButton is enabled only when an item is selected.
        UserAccountsListView.Items.Remove(UserAccountsListView.SelectedItems[0]);
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
      ErrorProviderControl.Text = ErrorProviderControl.Text.Trim();
      var resetConnectionTest = false;
      switch (ErrorProviderControl.Name)
      {
        case nameof(RootPasswordTextBox):
        case nameof(RepeatPasswordTextBox):
          resetConnectionTest = true;
          errorMessage = CheckPasswords();
          break;
      }

      if (ErrorProviderControl == RepeatPasswordTextBox)
      {
        // If the control being validated is the RepeatPasswordTextBox, set the control to place the error provider to RootPasswordTextBox
        ErrorProviderControl = RootPasswordTextBox;
      }

      if (resetConnectionTest)
      {
        ResetConnectionTest();
      }

      return errorMessage;
    }

    /// <summary>
    /// Resets the status of the connection test so it has to be performed again.
    /// </summary>
    private void ResetConnectionTest()
    {
      ConnectionErrorProvider.Clear();
      _rootPasswordOk = false;
    }
  }
}

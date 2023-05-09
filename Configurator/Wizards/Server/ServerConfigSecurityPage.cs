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
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Core.Wizard;

namespace MySql.Configurator.Wizards.Server
{
  /// <summary>
  /// Configuration page to ask for credentials for a database upgrade.
  /// </summary>
  public partial class ServerConfigSecurityPage : ConfigWizardPage
  {
    #region Fields

    /// <summary>
    /// Security identifiers for the local administrators group.
    /// </summary>
    private SecurityIdentifier _administratorsGroup;

    /// <summary>
    /// The <seealso cref="ServerConfigurationController"/> used to perform actions.
    /// </summary>
    private readonly ServerConfigurationController _controller;

    /// <summary>
    /// Security identifier for the creator/owner default user.
    /// </summary>
    private SecurityIdentifier _creatorOwnerUser;

    /// <summary>
    /// The current Windows Service account that will be used.
    /// </summary>
    private string _currentServiceAccountUsername;

    /// <summary>
    /// The path to the data directory.
    /// </summary>
    private string _dataDirectory;

    /// <summary>
    /// Indicates if the controls for the "Yes Review Changes" option have been initialized.
    /// </summary>
    private bool _reviewControlsAreInitialized;

    /// <summary>
    /// Indicates if the server files permissions need to be updated.
    /// </summary>
    private bool _updateServerFilesPermissions;

    /// <summary>
    /// Security identifier for the general system account.
    /// </summary>
    private SecurityIdentifier _systemAccountUser;

    /// <summary>
    /// Security identifier for the local users group.
    /// </summary>
    private SecurityIdentifier _usersGroup;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerConfigUpgradePage"/> class.
    /// </summary>
    /// <param name="controller">The <seealso cref="ServerConfigurationController"/> used to perform actions.</param>
    public ServerConfigSecurityPage(ServerConfigurationController controller)
    {
      InitializeComponent();
      _controller = controller;
      _reviewControlsAreInitialized = false;
      _updateServerFilesPermissions = true;
      ReviewChangesPanel.Visible = false;

      if (!string.IsNullOrEmpty(_controller.DataDirectory))
      {
        _dataDirectory = Path.Combine(_controller.DataDirectory, "Data");
        DataDirectoryPathLabel.Text = _dataDirectory;
        InitializeKnownUsersAndGroups();
      }
    }

    /// <summary>
    /// Activates this instance.
    /// </summary>
    public override void Activate()
    {
      base.Activate();

      // Refresh the user running the Windows Service to be marked for full control on the data directory.
      if (_controller.Settings.ConfigureAsService)
      {
        if (!FullControlListView.Items.ContainsKey(_controller.Settings.ServiceAccountUsername))
        {
          AddItemToListView(FullControlListView, _controller.Settings.ServiceAccountUsername, false, true);
        }

        if (!_controller.Settings.ServiceAccountUsername.Equals(_currentServiceAccountUsername, StringComparison.InvariantCultureIgnoreCase))
        {
          FullControlListView.Items.RemoveByKey(_currentServiceAccountUsername);
        }

        _currentServiceAccountUsername = _controller.Settings.ServiceAccountUsername;
      }
      else if(FullControlListView.Items.ContainsKey(_controller.Settings.ServiceAccountUsername))
      {
        FullControlListView.Items.RemoveByKey(_controller.Settings.ServiceAccountUsername);
      }
    }

    public void InitializeKnownUsersAndGroups()
    {
      _administratorsGroup = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null);
      _creatorOwnerUser = new SecurityIdentifier(WellKnownSidType.CreatorOwnerSid, null);
      _systemAccountUser = new SecurityIdentifier(WellKnownSidType.LocalSystemSid, null);
      _usersGroup = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);
    }

    /// <summary>
    /// Executes actions performed when the Next button is clicked.
    /// </summary>
    /// <returns><c>true</c> if it the configuration should proceed to the next panel, <c>false</c> otherwise.</returns>
    public override bool Next()
    {
      _controller.UpdateDataDirectoryPermissions = _updateServerFilesPermissions;
      if (_controller.ConfigurationType == Core.Enums.ConfigurationType.Upgrade)
      {
        _controller.UpdateUpgradeConfigSteps();
      }
      else
      {
        _controller.UpdateConfigurationSteps();
      }
      
      if (_updateServerFilesPermissions)
      {
        var fullControlDictionary = new Dictionary<SecurityIdentifier, string>();
        if (YesRadioButton.Checked)
        {
          if (_controller.Settings.ConfigureAsService)
          {
            var serviceAccountUsername = _controller.Settings.ServiceAccountUsername.StartsWith(".")
                                       ? _controller.Settings.ServiceAccountUsername.Replace(".", Environment.MachineName)
                                       : _controller.Settings.ServiceAccountUsername;
            var sid = DirectoryServicesWrapper.GetSecurityIdentifier(serviceAccountUsername);
            if (sid == null)
            {
              Logger.LogError(string.Format(Properties.Resources.ServerConfigSidRetrievalFailure, _controller.Settings.ServiceAccountUsername));
            }
            else
            {
              fullControlDictionary.Add(sid, "User");
            }
          }

          fullControlDictionary.Add(_administratorsGroup, "Group");
          fullControlDictionary.Add(_creatorOwnerUser, "User");
          fullControlDictionary.Add(_systemAccountUser, "User");
        }
        else
        {
          Cursor = Cursors.WaitCursor;
          foreach (ListViewItem item in FullControlListView.Items)
          {
            var sid = DirectoryServicesWrapper.GetSecurityIdentifier(item.Name);
            if (sid == null)
            {
              Logger.LogError(string.Format(Properties.Resources.ServerConfigSidRetrievalFailure, _controller.Settings.ServiceAccountUsername));
            }
            else
            {
              fullControlDictionary.Add(sid, DirectoryServicesWrapper.IsGroup(item.Name) == true ? "Group" : "User");
            }
          }

          Cursor = Cursors.Default;
        }

        _controller.FullControlDictionary = fullControlDictionary;
      }

      return base.Next();
    }

    /// <summary>
    /// Loads the existing users and groups into the <see cref="ListView"/> controls.
    /// </summary>
    private void LoadUsersAndGroups()
    {
      if (string.IsNullOrEmpty(_dataDirectory))
      {
        Logger.LogError(Properties.Resources.ServerConfigNoValueAssignedToDataDirectory);
        return;
      }

      // If the data directory does not exist we go up one level until we find a folder from
      // where we can identify the default directory permissions.
      var directoryInfo = new DirectoryInfo(_dataDirectory);
      while(!directoryInfo.Exists)
      {
        if (directoryInfo.Parent == null)
        {
          break;
        }

        directoryInfo = directoryInfo.Parent;
      }

      if (!directoryInfo.Exists)
      {
        directoryInfo = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
        if (!directoryInfo.Exists)
        {
          Logger.LogError(Properties.Resources.ServerConfigNoValidBaseDirectoryFound);
          return;
        }
      }

      // Populate the List View controls with the default elements.
      var rules = DirectoryServicesWrapper.GetAuthorizationRules(directoryInfo);
      if (rules == null)
      {
        Logger.LogError(Properties.Resources.ServerConfigFailedToGetAuthorizationRules);
        return;
      }

      foreach (FileSystemAccessRule rule in rules)
      {
        var ruleValue = rule.IdentityReference.Value;
        if (string.IsNullOrEmpty(ruleValue))
        {
          Logger.LogWarning(Properties.Resources.ServerConfigNameMissingForRule);
          continue;
        }

        var account = new NTAccount(ruleValue.Contains("\\") ? ruleValue.Split('\\')[1] : ruleValue);
        if (account == null)
        {
          Logger.LogWarning(string.Format(Properties.Resources.ServerConfigConvertToNTAccountFailed, ruleValue));
          continue;
        }

        SecurityIdentifier securityIdentifier = null;
        try
        {
          securityIdentifier = account.Translate(typeof(SecurityIdentifier)) as SecurityIdentifier;
        }
        catch (Exception ex)
        {
          Logger.LogException(ex);
        }

        if (securityIdentifier == null)
        {
          Logger.LogWarning(string.Format(Properties.Resources.ServerConfigNameMissingForRule, account.Value));
          continue;
        }

        // Windows assigns full control to the CREATOR/OWNER and System accounts by default.
        // We will set them as editable in the Full Control list in case the user wants to remove them.
        if (securityIdentifier.Value == _creatorOwnerUser.Value
            || securityIdentifier.Value == _systemAccountUser.Value)
        {
          if (FullControlListView.Items.ContainsKey(account.Value))
          {
            continue;
          }

          AddItemToListView(FullControlListView, account.Value, false, false);
        }
        // Add the local Administrators group to the Full Control list as non-editable.
        else if (securityIdentifier.Value == _administratorsGroup.Value)
        {
          if (FullControlListView.Items.ContainsKey(account.Value))
          {
            continue;
          }

          AddItemToListView(FullControlListView, account.Value, true, true);
        }
        // Add the local Users group to the No-Access list as non-editable.
        else if (securityIdentifier.Value == _usersGroup.Value)
        {
          if (NoAccessListView.Items.ContainsKey(account.Value))
          {
            continue;
          }

          AddItemToListView(NoAccessListView, account.Value, true, true);
        }
        // Add any other inherited user/group to the No-Access list as editable.
        else
        {
          if (NoAccessListView.Items.ContainsKey(account.Value))
          {
            continue;
          }

          AddItemToListView(NoAccessListView, account.Value, DirectoryServicesWrapper.IsGroup(rule.IdentityReference.Value) == true, true);
        }
      }

      // Query for any other local groups and include them to the No-Access list as editable.
      var groups = DirectoryServicesWrapper.GetLocalGroups();
      if (groups != null)
      {
        foreach (var group in groups)
        {
          if (NoAccessListView.Items.ContainsKey(group)
              || FullControlListView.Items.ContainsKey(group))
          {
            continue;
          }

          AddItemToListView(NoAccessListView, group, true, false);
        }
      }
      else
      {
        Logger.LogError(string.Format(Properties.Resources.ServerConfigFailedToRetrieveLocalPrincipals, "groups"));
      }

      // Query for any other local users and include them to the No-Access list as editable.
      var users = DirectoryServicesWrapper.GetLocalUsers();
      if (users != null)
      {
        foreach (var user in users)
        {
          if (NoAccessListView.Items.ContainsKey(user)
              || FullControlListView.Items.ContainsKey(user))
          {
            continue;
          }

          AddItemToListView(NoAccessListView, user, false, false);
        }
      }
      else
      {
        Logger.LogError(string.Format(Properties.Resources.ServerConfigFailedToRetrieveLocalPrincipals, "users"));
      }
    }

    private void AddItemToListView(ListView listView, string principalName, bool isGroup, bool markAsDisabled)
    {
      if (listView == null
          || string.IsNullOrEmpty(principalName))
      {
        Logger.LogError(Properties.Resources.ServerConfigNullListView);
        return;
      }

      var item = new ListViewItem(new string[] { principalName, isGroup ? "Group" : "User" });
      item.Name = principalName;
      listView.Items.Add(item);

      if (!markAsDisabled)
      {
        return;
      }

      listView.Items[listView.Items.Count - 1].ForeColor = Color.Gray;
    }

    private void FullControlListView_SelectedIndexChanged(object sender, EventArgs e)
    {
      var listView = sender as ListView;
      if (listView == null
          || listView.SelectedItems.Count == 0)
      {
        return;
      }

      var item = listView.SelectedItems[0];
      MoveSelectedRightButton.Enabled = item.ForeColor != Color.Gray;
    }

    private void MoveSelectedLeftButton_Click(object sender, EventArgs e)
    {
      if (NoAccessListView.SelectedItems.Count == 0)
      {
        return;
      }

      foreach (ListViewItem item in NoAccessListView.SelectedItems)
      {
        if (item.ForeColor == Color.Gray)
        {
          continue;
        }

        NoAccessListView.Items.RemoveAt(item.Index);
        FullControlListView.Items.Add(item);
      }

      FullControlListView.Sort();
    }

    private void MoveSelectedRightButton_Click(object sender, EventArgs e)
    {
      if (FullControlListView.SelectedItems.Count == 0)
      {
        return;
      }

      foreach (ListViewItem item in FullControlListView.SelectedItems)
      {
        if (item.ForeColor == Color.Gray)
        {
          continue;
        }

        FullControlListView.Items.RemoveAt(item.Index);
        NoAccessListView.Items.Add(item);
      }

      NoAccessListView.Sort();
    }

    private void NoAccessListView_SelectedIndexChanged(object sender, EventArgs e)
    {
      var listView = sender as ListView;
      if (listView == null
          || listView.SelectedItems.Count == 0)
      {
        return;
      }
      
      var item = listView.SelectedItems[0];
      MoveSelectedLeftButton.Enabled = item.ForeColor != Color.Gray;
    }

    private void NoRadioButton_CheckedChanged(object sender, EventArgs e)
    {
      ReviewChangesPanel.Visible = false;
      _updateServerFilesPermissions = false;
    }

    private void YesRadioButton_CheckedChanged(object sender, EventArgs e)
    {
      ReviewChangesPanel.Visible = false;
      _updateServerFilesPermissions = true;
    }

    private void YesReviewRadioButton_CheckedChanged(object sender, EventArgs e)
    {
      var radioButton = sender as RadioButton;
      if (radioButton != null
          && radioButton.Checked
          && !_reviewControlsAreInitialized
          && !string.IsNullOrEmpty(_controller.DataDirectory))
      {
        Cursor = Cursors.WaitCursor;
        LoadUsersAndGroups();
        _reviewControlsAreInitialized = true;
        Cursor = Cursors.Default;
      }

      ReviewChangesPanel.Visible = true;
      _updateServerFilesPermissions = true;
    }
  }
}
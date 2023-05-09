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

using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Core.Wizard;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MySql.Configurator.Wizards.Server
{
  /// <summary>
  /// Represents a configuration panel where Server instances are selected to install the samples databases.
  /// </summary>
  public partial class ServerExampleDatabasesPage : ConfigWizardPage
  {
    #region Fields

    /// <summary>
    /// The <see cref="ServerConfigurationController"/> used for the configuration.
    /// </summary>
    private ServerConfigurationController _controller;

    /// <summary>
    /// The example databases that are currently installed.
    /// </summary>
    private List<string> _installedDatabases;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <seealso cref="ServerConfigurationController"/> class.
    /// </summary>
    /// <param name="controller">The <see cref="ServerConfigurationController"/> used for the configuration.</param>
    public ServerExampleDatabasesPage(ServerConfigurationController controller)
    {
      _controller = controller;
      InitializeComponent();
    }

    /// <summary>
    /// Executes actions performed when the panel is activated.
    /// </summary>
    public override void Activate()
    {
      base.Activate();
      _installedDatabases = new List<string>();
      UpdateExampleDatabasesCreationStatus();
    }

    /// <summary>
    /// Executes actions performed when the Next button is clicked.
    /// </summary>
    /// <returns><c>true</c> if it the configuration should proceed to the next panel, <c>false</c> otherwise.</returns>
    public override bool Next()
    {
      // Initialize dictionary.
      if (_controller.ExampleDatabasesStatus != null)
      {
        _controller.ExampleDatabasesStatus.Clear();
      }
      else
      {
        _controller.ExampleDatabasesStatus = new Dictionary<string, string>();
      }
      
      // Set install status of example databases.
      if (_installedDatabases.Contains("sakila")
          && CreateRemoveSakilaDatabaseCheckBox.Checked)
      {
        _controller.ExampleDatabasesStatus.Add("sakila", "remove");
      }
      else if (!_installedDatabases.Contains("sakila")
               && CreateRemoveSakilaDatabaseCheckBox.Checked)
      {
          _controller.ExampleDatabasesStatus.Add("sakila", "create");
      }

      if (_installedDatabases.Contains("world")
          && CreateRemoveWorldDatabaseCheckBox.Checked)
      {
        _controller.ExampleDatabasesStatus.Add("world", "remove");
      }
      else if (!_installedDatabases.Contains("world")
               && CreateRemoveWorldDatabaseCheckBox.Checked)
      {
        _controller.ExampleDatabasesStatus.Add("world", "create");
      }

      _controller.IsCreateRemoveExamplesDatabasesStepNeeded = CreateRemoveSakilaDatabaseCheckBox.Checked
                                                        || CreateRemoveWorldDatabaseCheckBox.Checked;
      return base.Next();
    }

    /// <summary>
    /// Identifies if example databases need to be created or removed.
    /// </summary>
    private void UpdateExampleDatabasesCreationStatus()
    {
      if (_controller.ConfigurationType == ConfigurationType.New)
      {
        return;
      }

      var query = new StringBuilder("SELECT schema_name FROM INFORMATION_SCHEMA.SCHEMATA WHERE ");

      // Find databases that can be installed.
      _controller.ExtractExamplesDatabases();
      var exampleDatabases = Directory.GetDirectories(_controller.ExampleDatabasesLocation);
      if (exampleDatabases.Length == 0)
      {
        return;
      }

      for (int index = 0; index < exampleDatabases.Length; index++)
      {
        query.Append($"schema_name = '{exampleDatabases[index].Substring(exampleDatabases[index].LastIndexOf('\\') + 1)}' ");
        if (index < exampleDatabases.Length -1)
        {
          query.Append("OR ");
        }
      }

      // Check if databases exist.
      _installedDatabases = new List<string>();
      try
      {
        var rootUser = ServerUser.GetLocalRootUser(_controller.Settings.ExistingRootPassword, _controller.Settings.DefaultAuthenticationPlugin);
        var connectionString = _controller.GetConnectionString(rootUser, _controller.ConfigurationType != ConfigurationType.New);
        using (var connection = new MySqlConnection(connectionString))
        {
          connection.Open();
          var command = new MySqlCommand(query.ToString(), connection);
          var reader = command.ExecuteReader();
          while (reader.Read())
          {
            _installedDatabases.Add(reader[0].ToString());
          }
        }
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
      }
      finally
      {
        // Ajust page.
        CreateRemoveSakilaDatabaseCheckBox.Text = $"{(_installedDatabases.Contains("sakila") ? "Remove" : "Create")} Sakila database";
        CreateRemoveWorldDatabaseCheckBox.Text = $"{(_installedDatabases.Contains("world") ? "Remove" : "Create")} World database";

        DynamicLabel.Text = _installedDatabases.Count == 2
          ? "Select the databases that should be removed:"
          : "Select the databases that should be created or removed:";
      }
    }
  }
}
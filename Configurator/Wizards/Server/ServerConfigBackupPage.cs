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
using MySql.Configurator.Core.Wizard;

namespace MySql.Configurator.Wizards.Server
{
  /// <summary>
  /// Configuration page to ask for credentials for a database upgrade.
  /// </summary>
  public partial class ServerConfigBackupPage : ConfigWizardPage
  {
    #region Fields

    /// <summary>
    /// The <seealso cref="ServerConfigurationController"/> used to perform actions.
    /// </summary>
    private readonly ServerConfigurationController _controller;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerConfigBackupPage"/> class.
    /// </summary>
    /// <param name="controller">The <seealso cref="ServerConfigurationController"/> used to perform actions.</param>
    public ServerConfigBackupPage(ServerConfigurationController controller)
    {
      BackupDatabase = true;
      InitializeComponent();
      _controller = controller;
    }

    #region Properties

    /// <summary>
    /// Gets a value indicating whether a backup of the database is done before upgrading the system tables.
    /// </summary>
    public bool BackupDatabase { get; private set; }

    #endregion Properties

    /// <summary>
    /// Activates this instance.
    /// </summary>
    public override void Activate()
    {
      base.Activate();
    }

    /// <summary>
    /// Executes actions performed when the Next button is clicked.
    /// </summary>
    /// <returns><c>true</c> if it the configuration should proceed to the next panel, <c>false</c> otherwise.</returns>
    public override bool Next()
    {
      _controller.IsBackupDatabaseStepNeeded = RunBackupRadioButton.Checked;
      _controller.UpdateUpgradeConfigSteps();
      return base.Next();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="BackupDatabaseCheckBox"/> checked property changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void BackupDatabaseCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      BackupDatabase = RunBackupRadioButton.Checked;
    }
  }
}

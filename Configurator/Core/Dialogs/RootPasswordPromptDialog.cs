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
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.MySql;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Core.Forms;
using MySql.Configurator.Properties;

namespace MySql.Configurator.Core.Dialogs
{
  public partial class RootPasswordPromptDialog : AutoStyleableBaseDialog
  {
    #region Fields

    private MySqlAuthenticationPluginType _authenticationPlugin;
    private bool _preventClosingIfNotSuccessful;

    #endregion Fields

    public RootPasswordPromptDialog(string server, uint port, bool preventClosingIfNotSuccessful)
    {
      InitializeComponent();
      _authenticationPlugin = server.IndexOf("5.7", StringComparison.OrdinalIgnoreCase) >= 0
                              || server.IndexOf("8.", StringComparison.OrdinalIgnoreCase) >= 0
        ? MySqlAuthenticationPluginType.CachingSha2Password
        : MySqlAuthenticationPluginType.MysqlNativePassword;
      _preventClosingIfNotSuccessful = preventClosingIfNotSuccessful;
      ConnectionResult = ConnectionResultType.None;
      LoginTitleLabel.Text = string.Format(Resources.PasswordDialogLargeTitle, server);
      Port = port;
    }

    #region Properties

    public ConnectionResultType ConnectionResult { get; private set; }

    public string Password => PasswordTextBox.Text.Trim();
    public uint Port { get; }

    public string Username => UsernameTextBox.Text.Trim();

    #endregion Properties

    private void RootPasswordPromptDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (DialogResult == DialogResult.Cancel)
      {
        return;
      }

      if (_preventClosingIfNotSuccessful && ConnectionResult != ConnectionResultType.ConnectionSuccess)
      {
        e.Cancel = true;
      }
    }

    private void TestConnectionButton_Click(object sender, EventArgs e)
    {
      Cursor = Cursors.WaitCursor;
      OkButton.Enabled = false;
      TestConnectionButton.Enabled = false;
      var instance = new MySqlServerInstance(Port, new MySqlServerUser(Username, Password, _authenticationPlugin));
      ConnectionResult = instance.CanConnect(true);
      ConnectionResultPictureBox.Image = ConnectionResult == ConnectionResultType.ConnectionSuccess
        ? Resources.ok_sign
        : Resources.error_sign;
      ConnectionResultPictureBox.Visible = true;
      ConnectionResultLabel.Text = ConnectionResult.GetDescription();
      ConnectionResultLabel.Visible = true;
      TestConnectionButton.Enabled = true;
      OkButton.Enabled = true;
      Cursor = Cursors.Default;
    }
  }
}

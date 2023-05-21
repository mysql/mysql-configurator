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

using System.Windows.Forms;

namespace MySql.Configurator.Core.Controls
{
  public partial class RootCredentialsCheckControl : UserControl
  {
    public RootCredentialsCheckControl()
    {
      InitializeComponent();
    }

    #region Properties

    public bool CanConnectWithCredentials { get; private set; }

    #endregion Properties

    private void CheckButton_Click(object sender, System.EventArgs e)
    {
      Cursor = Cursors.WaitCursor;
      CanConnectWithCredentials = false;
      ResultsPanel.Visible = false;
      // TODO: WIP
      //var connectionResult = ServerInstance.CanConnect(_controller, ExistingRootPasswordTextBox.Text, true);
      //CanConnectWithCredentials = connectionResult == ConnectionResultType.ConnectionSuccess;
      //ResultPictureBox.Image = _credsOk
      //  ? Resources.MySQLInstallerConfig_Done
      //  : Resources.MySQLInstallerConfig_Error;
      //ResultLabel.Text = connectionResult.GetDescription();
      ResultsPanel.Visible = true;
      Cursor = Cursors.Default;
    }
  }
}

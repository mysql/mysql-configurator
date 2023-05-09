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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MySql.Configurator.Core.Wizard;
using Utilities = MySql.Configurator.Core.Classes.Utilities;

namespace MySql.Configurator.Wizards.ConfigWizard
{
  public partial class ConfigCompletePage : WizardPage
  {
    public ConfigCompletePage()
    {
      InitializeComponent();
    }

    #region Properties

    public override bool CancelOk
    {
      get
      {
        Wizard.CancelButton.Visible = false;
        return base.CancelOk;
      }
    }

    public override bool BackOk
    {
      get
      {
        Wizard.BackButton.Visible = false;
        return base.BackOk;
      }
    }

    #endregion Properties

    public override void Activate()
    {
      base.Activate();
      WorkDone = true; // This is the last page, no need to ask for confirmation to close.
      if (!(Wizard is ConfigWizard configWizard))
      {
        return;
      }
    }

    public override bool Finish()
    {

      return base.Finish();
    }

    private void RebootTheSystem()
    {
      using (var process = new Process {StartInfo = {FileName = "shutdown", Arguments = "/r /t 0"}})
      {
        process.Start();
      }
    }

    #region Events

    private void ClusterOptionsBlogLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Core.Classes.Utilities.OpenBrowser("https://dev.mysql.com/blog-archive/mysql-innodb-cluster-changing-cluster-options-live/");
    }

    private void CopyLogToClipboardButton_Click(object sender, EventArgs e)
    {
      Clipboard.SetText(string.IsNullOrEmpty(Wizard.Log) ? " " : Wizard.Log);
    }

    private void MySQLServerDocumentationLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Core.Classes.Utilities.OpenBrowser("https://dev.mysql.com/doc/refman/8.0/en/");
    }

    #endregion
  }
}
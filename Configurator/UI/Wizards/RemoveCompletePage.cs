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
using System.Diagnostics;
using System.Windows.Forms;
using MySql.Configurator.Base.Classes;
using MySql.Configurator.Core.Logging;
using MySql.Configurator.Properties;
using MySql.Configurator.UI.Dialogs;

namespace MySql.Configurator.UI.Wizards
{
  public partial class RemoveCompletePage : WizardPage
  {
    public RemoveCompletePage()
    {
      InitializeComponent();
    }

    #region Properties

    public override bool BackOk
    {
      get
      {
        Wizard.BackButton.Visible = false;
        return base.BackOk;
      }
    }

    public override bool CancelOk
    {
      get
      {
        Wizard.CancelButton.Visible = false;
        return base.CancelOk;
      }
    }

    #endregion Properties

    public override void Activate()
    {
      RemovedServerInstallationsListView.Items.Clear();
      var removeProductsWizard = Wizard as RemoveProductsWizard;
      if (removeProductsWizard == null)
      {
        throw new Exception("Bad wizard");
      }

      foreach (var serverInstallation in removeProductsWizard.ProductsRemoved)
      {
        var item = RemovedServerInstallationsListView.Items.Add(string.Empty);
        item.Name = serverInstallation.NameWithVersion;
        item.Tag = serverInstallation;
        item.SubItems.Add(new MyListViewSubItem(item, serverInstallation.NameWithVersion, null, false, true));
        item.SubItems.Add(serverInstallation.VersionString);
        removeProductsWizard.RebootRequired |= serverInstallation.Controller.RebootRequired;
      }

      RebootComputerPanel.Visible |= removeProductsWizard.RebootRequired;

      if (removeProductsWizard.RebootRequired)
      {
        RebootComputerCheckBox.Checked = true;
      }

      if (!RebootComputerPanel.Visible)
      {
        RemovedServerInstallationsListView.Height += RebootComputerPanel.Height;
      }

      base.Activate();
    }

    public override bool Finish()
    {
      var process = new Process();
      if (RebootComputerCheckBox.Checked)
      {
        var result = InfoDialog.ShowDialog(InfoDialogProperties.GetOkCancelDialogProperties(InfoDialog.InfoType.Warning, Resources.AppName, Resources.ContinueRebootSystem)).DialogResult;
        if (result == DialogResult.Cancel)
        {
          return false;
        }
      }

      var removeProductsWizard = Wizard as RemoveProductsWizard;
      if (RebootComputerCheckBox.Checked
          && removeProductsWizard != null)
      {
        try
        {
          removeProductsWizard.CloseInstaller = true;
          var process1 = new Process
          {
            StartInfo = { FileName = "shutdown", Arguments = "/r /t 0" }
          };
          process1.Start();
        }
        catch (Exception ex)
        {
          Logger.LogException(ex);
        }
      }

      return base.Finish();
    }

    private void CopyLogToClipboardButton_Click(object sender, EventArgs e)
    {
      string completeProductLog = string.IsNullOrEmpty(Wizard.Log) ? " " : Wizard.Log;
      Clipboard.SetText(completeProductLog);
    }

    private void RebootComputerCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      if (!RebootComputerCheckBox.Checked)
      {
        InfoDialog.ShowDialog(InfoDialogProperties.GetWarningDialogProperties(Resources.AppName, Resources.RebootWarning));
      }
    }
  }
}

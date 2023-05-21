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
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.Forms;
using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Core.Product;
using MySql.Configurator.Core.Wizard;
using MySql.Configurator.Properties;

namespace MySql.Configurator.Wizards.RemoveWizard
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
      RemovedPackagesListView.Items.Clear();
      var removeProductsWizard = Wizard as RemoveProductsWizard;
      if (removeProductsWizard == null)
      {
        throw new Exception("Bad wizard");
      }

      foreach (var p in removeProductsWizard.ProductsRemoved)
      {
        var item = RemovedPackagesListView.Items.Add(string.Empty);
        item.Name = p.NameWithVersion;
        item.Tag = p;
        item.SubItems.Add(new MyListViewSubItem(item, p.NameWithVersion, p.Product.SmallIcon, false, true));
        item.SubItems.Add(p.Version);
        removeProductsWizard.RebootRequired |= p.Controller.RebootRequired;
      }

      RebootComputerPanel.Visible |= removeProductsWizard.RebootRequired;

      if (removeProductsWizard.RebootRequired)
      {
        RebootComputerCheckBox.Checked = true;
      }

      if (!RebootComputerPanel.Visible)
      {
        RemovedPackagesListView.Height += RebootComputerPanel.Height;
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

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

using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.Forms;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Core.Interfaces;
using MySql.Configurator.Core.Package;
using MySql.Configurator.Dialogs;
using MySql.Configurator.Properties;
using MySql.Configurator.Wizards.Server;

namespace MySql.Configurator.Wizards.RemoveWizard
{
  public partial class RemoveProductsWizard : Wizard
  {
    public RemoveProductsWizard()
    {
      InitializeComponent();
      ProductsRemoved = new List<IPackage>();
      ProductsToRemove = new List<Package>();
    }

    #region Properties

    /// <summary>
    /// Gets a value indicating if the wizard can be cancelled.
    /// </summary>
    public override bool CanCancel
    {
      get
      {
        var result = InfoDialog.ShowDialog(InfoDialogProperties.GetYesNoDialogProperties(InfoDialog.InfoType.Warning, Resources.CancelQuestionText, Resources.RemoveProductsWizardCancelConfirmationText)).DialogResult;
        return result != DialogResult.No;
      }
    }

    /// <summary>
    /// Gets or sets a flag to indicate if MySQL Installer should be closed.
    /// </summary>
    public bool CloseInstaller { get; set; }

    /// <summary>
    /// Get or sets the list that includes the products that have been successfully removed.
    /// </summary>
    public List<IPackage> ProductsRemoved { get; }

    /// <summary>
    /// Get or sets the List that includes the products that are marked to be removed.
    /// </summary>
    public List<Package> ProductsToRemove { get; set; }

    /// <summary>
    /// Gets or sets a flag to indicate if a computer reboot is required.
    /// </summary>
    public bool RebootRequired { get; set; }

    #endregion Properties

    /// <summary>
    /// Shows the wizard.
    /// </summary>
    /// <param name="parentMainForm">The parent form.</param>
    public void ShowWizard(Package package, MainForm parentMainForm)
    {
      WizardSideBar.ShowConfigPanel(package.NameWithVersion);
      ClearPages();
      //if (package.License != AppConfiguration.License)
      //{
      //  return;
      //}

      package.Controller.ConfigurationType = ConfigurationType.Remove;
      package.Controller.UpdateRemoveSteps();
      package.Controller.SetPages();
      var serverController = package.Controller as ServerConfigurationController;
      if (serverController != null)
      {
        if (!serverController.IsDeleteServiceStepNeeded
            && !serverController.IsRemoveFirewallRuleStepNeeded)
        {
          AddPage(new RemoveErrorPage());
          base.ShowWizard(parentMainForm);
          return;
        }
      }

      if (!serverController.IsDeleteServiceStepNeeded
          && !serverController.IsRemoveFirewallRuleStepNeeded
          && !serverController.IsDeleteDataDirectoryStepNeeded)
      {
        return;
      }

      ProductsToRemove.Add(package);
      foreach (var page in package.Controller.Pages.Where(page => page.ValidForType(package.Controller.ConfigurationType)))
      {
        AddPage(page);
      }

      AddPage(new RemoveApplyPage());
      AddPage(new RemoveCompletePage());
      base.ShowWizard(parentMainForm);
    }
  }
}
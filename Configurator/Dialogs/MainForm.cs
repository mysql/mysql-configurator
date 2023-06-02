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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.Forms;
using MySql.Configurator.Core.Common;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Core.Package;
using MySql.Configurator.Core.Product;
using MySql.Configurator.Wizards;
using MySql.Configurator.Wizards.ConfigWizard;
using MySql.Configurator.Wizards.RemoveWizard;
// using MySql.Configurator.Wizards.UpgradeWizard;

namespace MySql.Configurator.Dialogs
{
  public partial class MainForm : Form
  {
    #region Fields

    private string _action;

    private string _version;

    private string _dataDirPath;

    #endregion

    public MainForm(string version, string dataDirPath, string action)
    {
      InitializeComponent();
      _action = action;
      _version = version;
      _dataDirPath = dataDirPath;
      SetWindowPosition();
    }

    /// <summary>
    /// Handles the removal of a page from the form and disposes of it.
    /// </summary>
    /// <param name="containerPage">A page with more UI elements.</param>
    public void RemoveContainer(ContainerControl containerPage)
    {
      if (containerPage == null)
      {
        return;
      }

      if (Controls.Contains(containerPage))
      {
        Controls.Remove(containerPage);
      }

      containerPage.Dispose();
    }

    /// <summary>
    /// Handles the addition of a page to the form and the visibility of it in regard with other already added pages.
    /// </summary>
    /// <param name="containerPage">A page with more UI elements.</param>
    public void ShowContainer(ContainerControl containerPage)
    {
      if (containerPage == null)
      {
        return;
      }

      if (!Controls.Contains(containerPage))
      {
        Controls.Add(containerPage);
      }

      foreach (var otherContainer in Controls.OfType<ContainerControl>().Where(c => !c.Equals(containerPage)))
      {
        otherContainer.Visible = false;
      }

      containerPage.Visible = true;
    }

    private void SetWindowPosition()
    {
      // Set the form to a good default position. We have to compute that manually though
      // as the StartPosition property has no effect here.
      Screen currentScreen = Screen.PrimaryScreen;
      Rectangle workingArea = currentScreen.WorkingArea;
      var point = new Point((workingArea.Width - Width) / 2, (workingArea.Height - Height) / 2);

      SetDesktopLocation(point.X, point.Y);
    }

    private void TryToLaunchWizard(bool launchedFromMainIcon)
    {
      // Attempt to load package.
      Package package = null;
      try
      {
        package = ProductManager.LoadPackage(_version, _dataDirPath);
      }
      catch (ConfiguratorException ex)
      {
        InfoDialog.ShowDialog(InfoDialogProperties.GetErrorDialogProperties($"MySQL Server {_version} not found", ex.Message));
        Close();
      }

      if (package == null
          || !package.IsInstalled)
      {
        InfoDialog.ShowDialog(InfoDialogProperties.GetErrorDialogProperties($"MySQL Server {_version} not installed", $"No matching product was found."));
        Close();
        return;
      }

      ConfigurationType configurationType;
      switch (_action)
      {
        case "configure":
          configurationType = ConfigurationType.Reconfiguration;
          var configWizard = new ConfigWizard();
          Controls.Add(configWizard);
          configWizard.WizardCanceled += WizardClosed;
          configWizard.WizardClosed += WizardClosed;
          configWizard.ShowWizard(package, this, configurationType);
          break;

        case "remove":
          configurationType = ConfigurationType.Remove;
          var removeWizard = new RemoveProductsWizard();
          Controls.Add(removeWizard);
          removeWizard.WizardCanceled += WizardClosed;
          removeWizard.WizardClosed += WizardClosed;
          removeWizard.ShowWizard(package, this);
          break;

        case "upgrade":
          configurationType = ConfigurationType.Upgrade;
          var upgradeWizard = new ConfigWizard();
          Controls.Add(upgradeWizard);
          upgradeWizard.WizardCanceled += WizardClosed;
          upgradeWizard.WizardClosed += WizardClosed;
          upgradeWizard.ShowWizard(package, this, configurationType);
          break;

        default:
          break;
      }
      
    }

    /// <summary>
    /// Disposes of the wizard.
    /// </summary>
    private void DisposeWizard(Wizard wizard)
    {
      if (wizard == null)
      {
        return;
      }

      wizard.WizardCanceled -= WizardCanceled;
      wizard.WizardClosed -= WizardClosed;
      wizard.Dispose();
    }

    private void WizardClosed(object sender, EventArgs e)
    {
      RemoveContainer(sender as Wizard);
      DisposeWizard(sender as Wizard);
      Close();
    }

    private void WizardCanceled(object sender, EventArgs e)
    {
      bool isMsi = false;
      RemoveContainer(sender as Wizard);
      if (isMsi)
      {
        Close();
        return;
      }
    }

    /// <summary>
    /// Determine if the current action and ultimately the application
    /// can be stopped and closed.
    /// </summary>
    /// <returns>True if we can shut down, false otherwise.</returns>
    public bool CanClose()
    {
      var wizard = this.Controls.Cast<Wizard>().FirstOrDefault();
      if (wizard == null)
      {
        // This is unexpected, a Wizard should be already in the Controls collection, but if not found just let the form close.
        return true;
      }

      return wizard.CanCancel;
    }

    #region Event handling

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      // Cancel current actions if the user agrees.
      e.Cancel = !CanClose();
    }

    /// <summary>
    /// This method is attached to event shown of MainForm and is executed once the MainForm is already created
    /// </summary>
    private void MainForm_Shown(object sender, EventArgs e)
    {
      TryToLaunchWizard(true);
    }

    #endregion
  }
}
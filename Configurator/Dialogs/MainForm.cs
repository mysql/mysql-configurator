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
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.Forms;
using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Core.Common;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Core.Package;
using MySql.Configurator.Core.Product;
using MySql.Configurator.Properties;
using MySql.Configurator.Wizards;
using MySql.Configurator.Wizards.ConfigWizard;
using MySql.Configurator.Wizards.RemoveWizard;
using MySql.Configurator.Wizards.Server;
// using MySql.Configurator.Wizards.UpgradeWizard;

namespace MySql.Configurator.Dialogs
{
  public partial class MainForm : Form
  {
    #region Fields

    /// <summary>
    /// The application execution mode.
    /// </summary>
    private ExecutionMode _executionMode;

    /// <summary>
    /// The server package associated to the current installation.
    /// </summary>
    private Package _package;

    #endregion

    public MainForm(Package package, ExecutionMode executionMode)
    {
      InitializeComponent();
      _executionMode = executionMode;
      _package = package;
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
      ConfigurationType configurationType;
      switch (_executionMode)
      {
        case ExecutionMode.Configure:
          configurationType = ConfigurationType.Reconfiguration;
          var configWizard = new ConfigWizard();
          Controls.Add(configWizard);
          configWizard.WizardCanceled += WizardClosed;
          configWizard.WizardClosed += WizardClosed;
          configWizard.ShowWizard(_package, this, configurationType);
          break;

        case ExecutionMode.Remove:
        case ExecutionMode.RemoveNoShow:
          configurationType = ConfigurationType.Remove;
          var removeWizard = new RemoveProductsWizard();
          Controls.Add(removeWizard);
          removeWizard.WizardCanceled += WizardClosed;
          removeWizard.WizardClosed += WizardClosed;
          removeWizard.ShowWizard(_package, this);
          break;

        default:
          throw new ConfiguratorException(ConfiguratorError.InvalidExecutionMode);
      }

      StatusStrip.Visible = true;
      VersionLabel.Text = $"MySQL Server {_package.VersionString}";
      var controllerConfigurationType = _package.Controller.ConfigurationType;
      var stringConfigurationType = string.Empty;
      switch (controllerConfigurationType)
      {
        case (ConfigurationType.New):
          stringConfigurationType = $"{controllerConfigurationType} configuration";
          break;

        default:
          stringConfigurationType = controllerConfigurationType.ToString();
          break;
      }
      
      ConfigurationTypeLabel.Text = stringConfigurationType;
      if (controllerConfigurationType == ConfigurationType.Reconfiguration
          || controllerConfigurationType == ConfigurationType.Remove)
      {
        var serverController = _package.Controller as ServerConfigurationController;
        DataDirectoryLabel.Text = $"Data Directory: {serverController.DataDirectory}";
      }

      Logger.LogInformation($"Status: {ConfigurationTypeLabel.Text};{VersionLabel.Text};{DataDirectoryLabel.Text}");
      StatusStrip.Refresh();
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
      var wizard = Controls.OfType<Wizard>().FirstOrDefault();
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

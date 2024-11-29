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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MySql.Configurator.Base.Classes;
using MySql.Configurator.Base.Enums;
using MySql.Configurator.Core.Controllers;
using MySql.Configurator.Core.Logging;
using MySql.Configurator.Core.Server;
using MySql.Configurator.Core.Settings;
using MySql.Configurator.Properties;
using MySql.Configurator.UI.Dialogs;
using MySql.Configurator.UI.Wizards;

namespace MySql.Configurator.UI.Forms
{
  public partial class MainForm : Form
  {
    #region Fields

    /// <summary>
    /// The server installation associated to the current installation.
    /// </summary>
    private ServerInstallation _serverInstallation;

    #endregion

    public MainForm(ServerInstallation serverInstallation)
    {
      InitializeComponent();
      _serverInstallation = serverInstallation;
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
      // Update execution mode if current configuration needs to be upgraded.
      var controller = _serverInstallation.Controller;
      if (controller == null)
      {
        throw new ArgumentNullException(nameof(controller));
      }

      var upgradeHistoryFileExists = false;
      Version existingServerVersion = new Version();
      if (AppConfiguration.ExecutionMode == ExecutionMode.Configure)
      {
        var path = Path.Combine(controller.DataDirectory, "Data", MySqlUpgradeHistoryManager.UPGRADE_HISTORY_FILE_NAME);
        upgradeHistoryFileExists = File.Exists(path);

        // TODO: Enable upgrade history file regeneration when server bug is fixed.
        /*
        // If upgrade history file does not exist, stop and start server to regenerate it.
        if (!upgradeHistoryFileExists
            && controller.IsThereServerDataFiles
            && _package.Version.Major >= 8
            && _package.Version.Minor >= 4)
        {
          var serverInstance = new LocalServerInstance(controller);
          serverInstance.DataDir = controller.DataDirectory;
          if (serverInstance.IsRunning)
          {
            serverInstance.StopInstance();
          }

          serverInstance.StartInstanceAsProcess("--upgrade=MINIMAL");
          upgradeHistoryFileExists = File.Exists(path);
        }
        */

        if (controller.IsThereServerDataFiles
            && upgradeHistoryFileExists)
        {
          (Version, bool) result = MySqlUpgradeHistoryManager.GetUpgradeHistoryLatestVersion(controller.DataDirectory);
          existingServerVersion = result.Item1;
          bool hasMetadata = result.Item2;
          
          // If the version is lower or if version is the same but we have metadata it means that this is
          // an updated version of the current package and needs to be upgraded.
          if (existingServerVersion != null
              && (existingServerVersion < _serverInstallation.Version)
                  || (existingServerVersion == _serverInstallation.Version
                      && hasMetadata))
          {
            var validUpgrade = _serverInstallation.Version.ServerSupportsInPlaceUpgrades(existingServerVersion);
            if (validUpgrade == UpgradeViability.Supported
                && _serverInstallation.Version.Major == existingServerVersion.Major
                && _serverInstallation.Version.Minor == existingServerVersion.Minor)
            {
              Logger.LogInformation(Resources.ExecutionModeSwitch);
              AppConfiguration.ExecutionMode = ExecutionMode.Upgrade;
              controller.IsSameDirectoryUpgrade = true;
            }
          }
        }
      }
      
      // Show wizard matching the selected execution mode.
      ConfigurationType configurationType;
      switch (AppConfiguration.ExecutionMode)
      {
        case ExecutionMode.Configure:
          configurationType = ConfigurationType.Reconfigure;
          var configWizard = new ConfigWizard();
          Controls.Add(configWizard);
          configWizard.WizardCanceled += WizardClosed;
          configWizard.WizardClosed += WizardClosed;
          configWizard.ShowWizard(_serverInstallation, this, configurationType);
          break;

        case ExecutionMode.Remove:
        case ExecutionMode.RemoveNoShow:
          configurationType = ConfigurationType.Remove;
          var removeWizard = new RemoveProductsWizard();
          Controls.Add(removeWizard);
          removeWizard.WizardCanceled += WizardClosed;
          removeWizard.WizardClosed += WizardClosed;
          removeWizard.ShowWizard(_serverInstallation, this);
          break;

        case ExecutionMode.Upgrade:
          configurationType = ConfigurationType.Upgrade;
          configWizard = new ConfigWizard();
          Controls.Add(configWizard);
          configWizard.WizardCanceled += WizardClosed;
          configWizard.WizardClosed += WizardClosed;
          configWizard.ShowWizard(_serverInstallation, this, configurationType);
          break;

        default:
          throw new ConfiguratorException(ConfiguratorError.InvalidExecutionMode);
      }

      // Update status bar.
      StatusStrip.Visible = true;
      var controllerConfigurationType = _serverInstallation.Controller.ConfigurationType;
      VersionLabel.Text = controllerConfigurationType != ConfigurationType.Upgrade
        ? $"MySQL Server {_serverInstallation.Version.ToString()}"
        : $"MySQL Server {(upgradeHistoryFileExists ? existingServerVersion.ToString() : "Unknown")} -> {_serverInstallation.VersionString}";
      var stringConfigurationType = string.Empty;
      switch (controllerConfigurationType)
      {
        case (ConfigurationType.Configure):
          stringConfigurationType = $"{controllerConfigurationType} configuration";
          break;

        default:
          stringConfigurationType = controllerConfigurationType.ToString();
          break;
      }
      
      ConfigurationTypeLabel.Text = stringConfigurationType;
      if (controllerConfigurationType == ConfigurationType.Reconfigure
          || controllerConfigurationType == ConfigurationType.Remove
          || controllerConfigurationType == ConfigurationType.Upgrade)
      {
        var serverController = _serverInstallation.Controller as ServerConfigurationController;
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
        return true;
      }

      if (wizard.CurrentPage.OperationExecuting)
      {
        var result = InfoDialog.ShowDialog(InfoDialogProperties.GetYesNoDialogProperties(InfoDialog.InfoType.Warning, Resources.MainFormOnGoingOperationTitle, Resources.MainFormOnGoingOperationDescription, Resources.MainFormOnGoingOperationDetail));
        return result.DialogResult == DialogResult.OK
               || result.DialogResult == DialogResult.Yes;
      }

      return true;
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

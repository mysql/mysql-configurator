/* Copyright (c) 2019, Oracle and/or its affiliates.

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
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.Forms;
using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Core.Controllers;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Core.Wizard;
using MySql.Configurator.Properties;

namespace MySql.Configurator.Wizards.Common
{
  public partial class BaseConfigureRemoveApplyPage : WizardPage
  {
    #region Fields

    protected ProductConfigurationController CurrentController;
    protected bool Executing;
    protected Stopwatch StopWatch;

    #endregion Fields

    public BaseConfigureRemoveApplyPage()
    {
      InitializeComponent();
      StopWatch = new Stopwatch();
      Executing = false;
    }

    public BaseConfigureRemoveApplyPage(ProductConfigurationController controller)
      : this()
    {
      CurrentController = controller;
    }

    /// <summary>
    /// Thread-safe delegate used to set the enabled status of the specified control.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <param name="enabled">The value for the Enabled property.</param>
    protected delegate void SetControlEnabledStatusCallback(Control control, bool enabled);

    /// <summary>
    /// Thread-safe delegate used to set the visible status of the specified control.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <param name="visible">The value for the Visible property.</param>
    protected delegate void SetControlVisibleStatusCallback(Control control, bool visible);

    public override bool Finish()
    {
      DialogResult result;
      CurrentController.AfterConfigurationEnded();
      if (CurrentController.CurrentState == ConfigState.ConfigurationError)
      {
        string msg = string.Format(Resources.ConfirmFinishWithFailingConfig, CurrentController.Package.NameWithVersion);
        result = InfoDialog.ShowDialog(InfoDialogProperties.GetYesNoDialogProperties(InfoDialog.InfoType.Warning, Resources.AppName, msg)).DialogResult;
        bool shouldClose = result == DialogResult.Yes;
        if (shouldClose)
        {
          DetachEvents();
        }

        return shouldClose;
      }

      DetachEvents();
      if (!RebootWhenDoneCheckBox.Checked)
      {
        return base.Finish();
      }

      if (!RebootSystem())
      {
        return false;
      }

      try
      {
        var process = new Process { StartInfo = { FileName = "shutdown", Arguments = "/r /t 0" } };
        process.Start();
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
      }

      return base.Finish();
    }

    protected DialogResult CancelConfigurationAfterTimeout()
    {
      var dialogProperties = InfoDialogProperties.GetInformationDialogProperties(
          string.Format(Resources.ConfigureHasTimedOutTitle, CurrentController.Package.NameWithVersion),
          Resources.ConfigureHasTimedOutDetail,
          Resources.ConfigureHasTimedOutSubDetail);
      dialogProperties.CommandAreaProperties.ButtonsLayout = CommandAreaProperties.ButtonsLayoutType.Generic2Buttons;
      dialogProperties.CommandAreaProperties.Button1Text = "Cancel";
      dialogProperties.CommandAreaProperties.Button1DialogResult = DialogResult.Cancel;
      dialogProperties.CommandAreaProperties.Button2Text = "Wait";
      dialogProperties.CommandAreaProperties.Button2DialogResult = DialogResult.OK;
      return InfoDialog.ShowDialog(dialogProperties).DialogResult;
    }
    
    /// <summary>
    /// Prompts the user to select if the system should be rebooted.
    /// </summary>
    /// <returns></returns>
    protected bool RebootSystem()
    {
      var result = InfoDialog.ShowDialog(InfoDialogProperties.GetOkCancelDialogProperties(InfoDialog.InfoType.Warning, Resources.AppName, Resources.ContinueRebootSystem)).DialogResult;
      return result != DialogResult.Cancel;
    }

    #region Properties

    public override bool FinishOk
    {
      get
      {
        SetControlVisibleStatus(Wizard.FinishButton, !Wizard.ExecuteButton.Visible);
        SetControlVisibleStatus(Wizard.BackButton, Wizard.ExecuteButton.Visible);
        return !Executing;
      }
    }

    #endregion Properties

    protected virtual void DetachEvents()
    {
    }
    
    protected void RebootWhenDoneCheckBox_CheckedChanged_1(object sender, EventArgs e)
    {
      if (!RebootWhenDoneCheckBox.Checked)
      {
        InfoDialog.ShowDialog(InfoDialogProperties.GetWarningDialogProperties(Resources.AppName, Resources.RebootWarning));
      }
    }

    protected void RetryButton_Click(object sender, EventArgs e)
    {
      Execute();
    }

    /// <summary>
    /// Thread-safe delegate used to set the enabled status of the specified control.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <param name="enabled">The value for the Enabled property.</param>
    protected void SetControlEnabledStatus(Control control, bool enabled)
    {
      // InvokeRequired required compares the thread ID of the
      // calling thread to the thread ID of the creating thread.
      // If these threads are different, it returns true.
      if (control.InvokeRequired)
      {
        SetControlEnabledStatusCallback callbackMethod = SetControlEnabledStatus;
        Invoke(callbackMethod, control, enabled);
      }
      else
      {
        control.Enabled = enabled;
      }
    }

    /// <summary>
    /// Thread-safe delegate used to set the visible status of the specified control.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <param name="visible">The value for the Enabled property.</param>
    protected void SetControlVisibleStatus(Control control, bool visible)
    {
      // InvokeRequired required compares the thread ID of the
      // calling thread to the thread ID of the creating thread.
      // If these threads are different, it returns true.
      if (control.InvokeRequired)
      {
        SetControlVisibleStatusCallback callbackMethod = SetControlVisibleStatus;
        Invoke(callbackMethod, control, visible);
      }
      else
      {
        control.Visible = visible;
      }
    }
  }
}
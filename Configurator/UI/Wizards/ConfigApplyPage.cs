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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Configurator.Base.Classes;
using MySql.Configurator.Base.Enums;
using MySql.Configurator.Core.Controllers;
using MySql.Configurator.Core.Logging;
using MySql.Configurator.Core.Server;
using MySql.Configurator.Properties;
using MySql.Configurator.UI.Controls;
using MySql.Configurator.UI.Dialogs;
using Action = System.Action;

namespace MySql.Configurator.UI.Wizards
{
  public partial class ConfigApplyPage : BaseConfigureRemoveApplyPage
  {
    #region Fields

    private bool _callActivateOnExecute;
    private ConfigStepControl _currentStep;
    #endregion Fields

    public ConfigApplyPage(ServerConfigurationController controller)
    {
      _callActivateOnExecute = false;
      InitializeComponent();
      CurrentController = controller;
      StopWatch = new Stopwatch();
      ExecutionStepsTabPage.Text = Resources.ConfigurationSteps;
    }

    private delegate DialogResult CancelConfigurationDelegate();

    #region Properties

    public override bool NextOk {
      get
      {
        Wizard.FinishButton.Visible = false;
        return !OperationExecuting
               && !Wizard.ExecuteButton.Enabled;
      }
    }

    #endregion Properties

    public override void Activate()
    {
      ExecutionStepsTabPage.Controls.Clear();
      var pt = new Point(14, 4);
      if (!(Wizard is ConfigWizard configWizard))
      {
        return;
      }

      CurrentController.UpdateConfigurationSteps();
      var executingSteps = CurrentController.ConfigurationSteps.Where(step => step.ValidForConfigureType(configWizard.ConfigurationType) && step.Execute).ToList();
      if (executingSteps.Count == 0)
      {
        ConfigurationEnded(null, EventArgs.Empty);
        return;
      }

      foreach (var step in executingSteps)
      {
        var c = new ConfigStepControl
        {
          Label = step.Description,
          Step = step,
          Location = pt
        };
        pt.Offset(0, c.Height);
        ExecutionStepsTabPage.Controls.Add(c);
      }

      Wizard.ExecuteButton.Visible = true;
      base.Activate();
    }

    public override bool Cancel()
    {
      CurrentController.CancelConfigure();
      return base.Cancel();
    }

    public override bool Next()
    {
      DialogResult result;
      CurrentController.AfterConfigurationEnded();
      if (CurrentController.CurrentState == ConfigState.ConfigurationError)
      {
        string msg = string.Format(Resources.ConfirmFinishWithFailingConfig, CurrentController.ServerInstallation.NameWithVersion);
        result = InfoDialog.ShowDialog(InfoDialogProperties.GetYesNoDialogProperties(InfoDialog.InfoType.Warning, Resources.AppName, msg)).DialogResult;
        return result == DialogResult.Yes;
      }

      Wizard.Log = LogContentsTextBox.Text;
      if (!RebootWhenDoneCheckBox.Checked)
      {
        return base.Finish();
      }

      result = InfoDialog.ShowDialog(InfoDialogProperties.GetYesNoDialogProperties(InfoDialog.InfoType.Warning, Resources.AppName, Resources.ContinueRebootSystem)).DialogResult;
      if (result == DialogResult.No)
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

      return base.Next();
    }

    public override void Execute()
    {
      Action action;
      action = delegate
      {
        if (_callActivateOnExecute)
        {
          Activate();
        }

        LogContentsTextBox.Clear();
        Wizard.ExecuteButton.Enabled = false;
        Wizard.CancelButton.Visible = true;
        ConfigurationResultLabel.Text = string.Empty;
        RetryButton.Visible = false;

        foreach (var configStepControl in ExecutionStepsTabPage.Controls.OfType<ConfigStepControl>())
        {
          configStepControl.SetStatus(ConfigStepControl.OPEN);
        }

        subCaptionLabel.Text = Resources.ConfigStepsAreExecuting;
        if (Wizard is ConfigWizard configWizard)
        {
          CurrentController.ConfigurationType = configWizard.ConfigurationType;
        }

        CurrentController.Configure();
      };

      ExecuteLongRunningOperation(action, false, false);
    }

    /// <summary>
    /// Subscribes custom events relavant to this wizard page.
    /// </summary>
    public override void SubscribeEvents()
    {
      base.SubscribeEvents();
      if (CurrentController == null)
      {
        throw new ArgumentNullException(nameof(CurrentController));
      }

      CurrentController.ConfigurationStarted += ConfigurationStarted;
      CurrentController.ConfigurationEnded += ConfigurationEnded;
      CurrentController.ConfigureTimedOut += ConfigureTimedOut;
      CurrentController.ConfigurationStatusChanged += controller_ConfigurationStatusChanged;
    }

    /// <summary>
    /// Unsubscribes custom events relavant to this wizard page.
    /// </summary>
    public override void UnsubscribeEvents()
    {
      base.UnsubscribeEvents();
      if (CurrentController == null)
      {
        throw new ArgumentNullException(nameof(CurrentController));
      }

      CurrentController.ConfigurationStarted -= ConfigurationStarted;
      CurrentController.ConfigurationEnded -= ConfigurationEnded;
      CurrentController.ConfigureTimedOut -= ConfigureTimedOut;
      CurrentController.ConfigurationStatusChanged -= controller_ConfigurationStatusChanged;
    }

    private void ConfigurationEnded(object sender, EventArgs e)
    {
      try
      {
        subCaptionLabel.Text = Resources.ConfigStepsFinished;
        switch (CurrentController.CurrentState)
        {
          case ConfigState.ConfigurationError:
            _callActivateOnExecute = true;
            ConfigurationResultLabel.Text = string.Format(Resources.ConfigureFailed, CurrentController.ServerInstallation.NameWithVersion);
            RetryButton.Visible = true;
            Wizard.BackButton.Enabled = true;
            Wizard.BackButton.Visible = true;

            // Update step descriptions.
            var serverController = CurrentController;
            if (serverController != null)
            {
              var revertedSteps = serverController.RevertedSteps;
              if (revertedSteps == null)
              {
                break;
              }

              foreach (var revertedStep in revertedSteps)
              {
                var control = ExecutionStepsTabPage.Controls.OfType<ConfigStepControl>().FirstOrDefault(configStepControl => configStepControl.Step.Description.Equals(revertedStep));
                if (control == null)
                {
                  continue;
                }

                control.Label = $"{control.Step.Description} (REVERTED)";
              }
            }

            break;

          case ConfigState.ConfigurationCancelled:
            _callActivateOnExecute = true;
            ConfigurationResultLabel.Text = string.Format(Resources.ConfigureCancelled, CurrentController.ServerInstallation.NameWithVersion);
            RetryButton.Visible = true;
            break;

          default:
            _callActivateOnExecute = false;
            ConfigurationResultLabel.Text = string.Format(Resources.ConfigureSuccess, CurrentController.ServerInstallation.NameWithVersion);
            ShowConfigurationSummaryTab();
            Wizard.ExecuteButton.Visible = false;
            Wizard.CancelButton.Visible = false;
            Wizard.BackButton.Visible = false;
            break;
        }

        if (CurrentController.ConfigurationType == ConfigurationType.Reconfigure
            && CurrentController.RebootRequired)
        {
          ExecutionTabControl.Height -= RebootWhenDonePanel.Height;
          RebootWhenDonePanel.Visible = true;
          RebootWhenDoneCheckBox.Checked = true;
        }
      }
      finally
      {
        EndLongRunningOperation();
        UpdateButtons();
        Wizard.FinishButton.Visible = false;
      }
    }

    /// <summary>
    /// Event method triggered when the configuration operation starts.
    /// </summary>
    /// <param name="sender">The sender object.</param>
    /// <param name="e">The evenet arugments.</param>
    private void ConfigurationStarted(object sender, EventArgs e)
    {
      BeginLongRunningOperation();
      UpdateButtons();
      Wizard.BackButton.Visible = false;
      Wizard.ExecuteButton.Enabled = false;
      Wizard.CancelButton.Visible = true;
    }

    private void ConfigureTimedOut(object sender, EventArgs e)
    {
      DialogResult dialogResult;
      if (InvokeRequired)
      {
        CancelConfigurationDelegate cancelConfigDelegate = CancelConfigurationAfterTimeout;
        dialogResult = (DialogResult)Invoke(cancelConfigDelegate);
      }
      else
      {
        dialogResult = CancelConfigurationAfterTimeout();
      }

      if (dialogResult == DialogResult.Cancel)
      {
        CurrentController.CancelConfigure();
      }
    }

    private void controller_ConfigurationStatusChanged(object sender, ConfigurationEventType type, string msg)
    {
      if (ExecutionStepsTabPage.VerticalScroll.Visible)
      {
        var currStep = CurrentController.CurrentStep;
        var stepControl = ExecutionStepsTabPage.Controls.Cast<ConfigStepControl>().FirstOrDefault(c => c != null && c.Step == currStep);
        if (stepControl != null)
        {
          ExecutionStepsTabPage.ScrollControlIntoView(stepControl);
        }
      }

      if (LogContentsTextBox.IsDisposed)
      {
        return;
      }

      switch (type)
      {
        case ConfigurationEventType.StepStarting:
          StartStep(CurrentController.CurrentStep as ConfigurationStep, msg);
          break;

        case ConfigurationEventType.StepFinished:
          FinishStep(CurrentController.CurrentStep as ConfigurationStep, msg);
          break;

        default:
          if (type != ConfigurationEventType.Waiting
              && LogContentsTextBox.Text.Length != 0)
          {
            LogContentsTextBox.AppendText(Environment.NewLine);
          }

          LogContentsTextBox.AppendText(msg);
          break;
      }
    }

    private ConfigStepControl FindStepControl(ConfigurationStep step)
    {
      return ExecutionStepsTabPage.Controls.OfType<ConfigStepControl>().FirstOrDefault(configStepControl => configStepControl.Step == step);
    }

    private void FinishStep(ConfigurationStep step, string msg)
    {
      var configStepControl = FindStepControl(step);
      configStepControl.SetStatus(step.Status == ConfigurationStepStatus.Finished
        ? ConfigStepControl.SUCCESS
        : ConfigStepControl.ERROR);
      configStepControl.SetDots(0);
      ProgressUpdateTimer.Stop();
      ConfigurationResultLabel.Text = string.Empty;
      if (!string.IsNullOrEmpty(msg))
      {
        LogContentsTextBox.AppendText(string.Format("{0}{1}{0}{0}", Environment.NewLine, msg));
      }
    }

    private void ProgressUpdateTimer_Tick(object sender, EventArgs e)
    {
      long millis = StopWatch.ElapsedMilliseconds;
      _currentStep.Step.ElapsedTime = (int)millis / 1000;
      if (millis > _currentStep.Step.NormalTime * 1000)
      {
        ConfigurationResultLabel.Text = string.Format(Resources.ConfigStepTakingTooLong, _currentStep.Step.Description);
      }

      int dots = (int)(millis / 500) % 3;
      _currentStep.SetDots(dots + 1);
    }

    private void ShowConfigurationSummaryTab()
    {
      if (string.IsNullOrEmpty(CurrentController.ConfigurationSummaryText)
          || ExecutionTabControl.TabPages.Count > 2)
      {
        return;
      }

      var summaryTab = new TabPage("Summary")
      {
        BackColor = ExecutionStepsTabPage.BackColor
      };
      var summaryTextBox = new TextBox
      {
        Text = string.Format("{0}{0}{1}", Environment.NewLine, CurrentController.ConfigurationSummaryText),
        BackColor = LogContentsTextBox.BackColor,
        ReadOnly = true,
        ScrollBars = ScrollBars.Both,
        Multiline = true,
        Dock = DockStyle.Fill
      };
      summaryTab.Controls.Add(summaryTextBox);
      ExecutionTabControl.TabPages.Add(summaryTab);
      ExecutionTabControl.SelectedIndex = ExecutionTabControl.TabPages.Count - 1;
      summaryTab.Focus();
    }

    private void StartStep(ConfigurationStep step, string msg)
    {
      var configStepControl = FindStepControl(step);
      configStepControl.SetStatus(ConfigStepControl.CURRENT);
      _currentStep = configStepControl;
      StopWatch.Restart();
      ProgressUpdateTimer.Start();
      if (!string.IsNullOrEmpty(msg))
      {
        LogContentsTextBox.AppendText(msg + Environment.NewLine);
      }
    }
  }
}

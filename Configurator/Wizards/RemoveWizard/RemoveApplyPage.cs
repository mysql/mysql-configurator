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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Controllers;
using MySql.Configurator.Core.Controls;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Core.Interfaces;
using MySql.Configurator.Core.Package;
using MySql.Configurator.Core.Product;
using MySql.Configurator.Properties;
using MySql.Configurator.Wizards.Common;

namespace MySql.Configurator.Wizards.RemoveWizard
{
  public partial class RemoveApplyPage : BaseConfigureRemoveApplyPage
  {
    #region Constants

    /// <summary>
    /// Indicates the height reduction for controls once the uninstall operation completes.
    /// </summary>
    private const int HEIGHT_REDUCED_WHEN_UNINSTALL_COMPLETE = 60;

    #endregion

    #region Fields

    /// <summary>
    /// The step that is currently being executed. This could be a parent remove step if no substeps are found
    /// or a substep.
    /// </summary>
    private RemoveStepControl _childRemoveStepControl;

    /// <summary>
    /// Represents the percentage of completion for the product that is currently being uninstalled.
    /// </summary>
    private int _currentProgress;
    /// <summary>
    /// Indicates if the progress should be updated when the step completes its execution.
    /// </summary>
    private bool _increaseProgressWhenStepCompletes;

    /// <summary>
    /// Represents the parent remove step control corresponding to the product that is currently being uninstalled.
    /// </summary>
    private RemoveStepControl _parentRemoveStepControl;

    /// <summary>
    /// Represents the percentage to increment upon the completion of each step/subStep.
    /// </summary>
    private int _progressIncrement;
    #endregion Fields

    /// <summary>
    /// The main constructor.
    /// </summary>
    public RemoveApplyPage()
    {
      InitializeComponent();
      StopWatch = new Stopwatch();
      Initialize();
    }

    /// <summary>
    /// Thread-safe delegate used to append text to the log tab.
    /// </summary>
    /// <param name="message">The text to append to the log tab.</param>
    private delegate void AppendTextToLogTabCallback(string message);

    /// <summary>
    /// Delegate used to display a dialog when the user wants to cancel the uninstall operation.
    /// </summary>
    /// <returns>The user selection about cancelling the uninstall operation.</returns>
    private delegate DialogResult CancelConfigurationDelegate();

    /// <summary>
    /// Thread-safe delegate used to set Text property of the StatusLabel.
    /// </summary>
    /// <param name="text">The value to set for the Text property.</param>
    /// <param name="removeStepControl">The step on which to update the StatusLabel.</param>
    private delegate void SetStatusLabelTextCallback(RemoveStepControl removeStepControl ,string text);

    #region Properties

    /// <summary>
    /// Gets or a value indicating whether the Execute button is visible and enabled.
    /// </summary>
    public override bool ExecuteOk
    {
      get
      {
        SetControlVisibleStatus(Wizard.NextButton, false);
        return base.ExecuteOk;
      }
    }

    /// <summary>
    /// Gets or sets the package manager to handle uninstalling products.
    /// </summary>
    protected PackageManager PackageManager { get; private set; }

    #endregion Properties

    /// <summary>
    /// This method is executed when the wizard page becomes active.
    /// </summary>
    public override void Activate()
    {
      if (!(Wizard is RemoveProductsWizard removeWizard))
      {
        return;
      }

      Initialize();
      RemoveStepsFlowLayoutPanel.Controls.Clear();
      bool showExpandCollapseButtons = false;
      foreach (var package in removeWizard.ProductsToRemove)
      {
        if (package.License != AppConfiguration.License)
        {
          continue;
        }

        package.Controller.UpdateRemoveSteps();
        var executingSteps = package.Controller.RemoveSteps.Where(step => step.Execute).ToList();
        if (!showExpandCollapseButtons && executingSteps.Count > 1)
        {
          showExpandCollapseButtons = true;
        }

        var removeStepControl = new RemoveStepControl(executingSteps, package.Controller)
        {
          Label = $"{package.NameWithVersion}({package.Architecture.ToString().ToLower()})",
          Step = executingSteps[0]
        };
        RemoveStepsFlowLayoutPanel.Controls.Add(removeStepControl);
      }

      ShowExpandCollapseButtons(showExpandCollapseButtons);
      Wizard.ExecuteButton.Visible = true;
      RebootWhenDonePanel.Visible = false;
      RetryButton.Visible = false;
      base.Activate();
    }

    /// <summary>
    /// Cancels the uninstall operation.
    /// </summary>
    public override bool Cancel()
    {
      if (PackageManager != null
          && Executing)
      {
        PackageManager.Cancel();
      }

      CurrentController?.CancelConfigure();
      return base.Cancel();
    }

    /// <summary>
    /// Executes the uninstall operation for the selected products.
    /// </summary>
    public override void Execute()
    {
      subCaptionLabel.Visible = false;
      Wizard.BackButton.Enabled = false;
      Wizard.ExecuteButton.Enabled = false;

      // Reset failed steps.
      foreach (var removeStepControl in RemoveStepsFlowLayoutPanel.Controls.OfType<RemoveStepControl>().Where(removeStepControl => removeStepControl.Controller.CurrentState == ConfigState.ConfigurationError))
      {
        removeStepControl.Controller.ResetState();
        removeStepControl.Step.Status = ConfigurationStepStatus.NotStarted;
        removeStepControl.SetStatus(ConfigStepControl.OPEN);
        if (removeStepControl.SubSteps == null
            || removeStepControl.SubSteps.Count == 0)
        {
          continue;
        }

        foreach (var subStep in removeStepControl.SubSteps)
        {
          subStep.Step.Status = ConfigurationStepStatus.NotStarted;
          subStep.SetStatus(ConfigStepControl.OPEN);
        }
      }

      UninstallNextProduct();
    }

    /// <summary>
    /// Finalizes the remove process.
    /// </summary>
    /// <returns><c>true</c> if the user confirms to reboot the system; otherwise, <c>false</c>.</returns>
    public override bool Finish()
    {
      return base.Finish();
    }

    /// <summary>
    /// Handles the completion of a package being uninstalled.
    /// </summary>
    /// <param name="args">The <see cref="PackageStatusEventArgs"/> instance containing the event data.</param>
    public virtual void PackageFinished(PackageStatusEventArgs args)
    {
      switch (args.Status)
      {
        case PackageStatus.Canceled:
          SetStatus(Resources.Cancelled);
          _childRemoveStepControl.SetStatus(0);
          _childRemoveStepControl.Step.Status = ConfigurationStepStatus.Error;
          break;

        case PackageStatus.Failed:
          SetStatus(Resources.Failed);
          _childRemoveStepControl.SetStatus(3);
          _childRemoveStepControl.Step.Status = ConfigurationStepStatus.Error;
          break;

        default:
          SetStatus(Resources.Completed);
          _currentProgress += _progressIncrement;
          SetProgress(_currentProgress);
          _childRemoveStepControl.SetStatus(1);
          _childRemoveStepControl.Step.Status = ConfigurationStepStatus.Finished;
          break;
      }

      AddToLogTab(GetCurrentStepControl(args.Package).Item1, args.ToString());
    }

    /// <summary>
    /// Handles the start of the uninstall operation for a package.
    /// </summary>
    /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
    public virtual void PackageStarted(PackageStatusEventArgs args)
    {
      _increaseProgressWhenStepCompletes = false;
      UpdateStatus(_parentRemoveStepControl);
      _parentRemoveStepControl.SetStatus(2);
    }

    /// <summary>
    /// Handles the status change of a package being uninstalled.
    /// </summary>
    /// <param name="args">The <see cref="PackageStatusEventArgs"/> instance containing the event data.</param>
    public virtual void PackageStatusChanged(PackageStatusEventArgs args)
    {
      var tuple = GetCurrentStepControl(args.Package);
      if (tuple == null)
      {
        return;
      }

      if (!args.IsVerbose)
      {
        AddToLogTab(tuple.Item1, args.Message);
      }

      if (args.Status == PackageStatus.Progress)
      {
        var value = (int)(args.Progress * (_progressIncrement / 100.0));
        SetProgress(_currentProgress + value);
      }
    }

    /// <summary>
    /// Handles completion of the uninstall queue.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    public virtual void QueueFinished(object sender, EventArgs e)
    {
      Wizard.Log = PackageManager.GetLog();
      SetControlVisibleStatus(Wizard.ExecuteButton, false);
      UpdateButtons();
    }

    /// <summary>
    /// Unsubscribes events from the handlers.
    /// </summary>
    protected override void DetachEvents()
    {
      if (_parentRemoveStepControl == null)
      {
        return;
      }

      _parentRemoveStepControl.Controller.ConfigurationStarted -= ConfigurationStarted;
      _parentRemoveStepControl.Controller.ConfigurationEnded -= ConfigurationEnded;
      _parentRemoveStepControl.Controller.ConfigureTimedOut -= ConfigureTimedOut;
      _parentRemoveStepControl.Controller.ConfigurationStatusChanged -= controller_ConfigurationStatusChanged;
      _parentRemoveStepControl.Controller.PackageUninstall -= PackageUninstall;
    }

    /// <summary>
    /// Gets the <see cref="RemoveStepControl"/> matching the specified <see cref="IPackage"/>.
    /// </summary>
    /// <param name="selectedPackage">The package to look for in the remove step control list.</param>
    /// <returns>A tuple containing the index of the remove step control along with the remove step control instance.</returns>
    protected Tuple<int, RemoveStepControl> GetCurrentStepControl(IPackage selectedPackage)
    {
      if (!(Wizard is RemoveProductsWizard))
      {
        return null;
      }

      int removeStepControlIndex = 0;
      for (int index = 0; index < RemoveStepsFlowLayoutPanel.Controls.Count; index++)
      {
        if (!(RemoveStepsFlowLayoutPanel.Controls[index] is RemoveStepControl removeStepControl))
        {
          continue;
        }

        removeStepControlIndex++;
        if (removeStepControl.Controller.Package == selectedPackage)
        {
          return new Tuple<int, RemoveStepControl>(removeStepControlIndex - 1, removeStepControl);
        }
      }

      return null;
    }

    /// <summary>
    /// Appends the specified text to log tab. 
    /// </summary>
    /// <param name="index">The index of the product currently being uninstalled.</param>
    /// <param name="message">The message to append to the log.</param>
    private void AddToLogTab(int index, string message)
    {
      if (string.IsNullOrEmpty(message))
      {
        return;
      }

      AppendTextToLogTab($"{index + 1}: {message}{Environment.NewLine}");
      PackageManager.AppendLog($"{index + 1}: {message}");
    }

    /// <summary>
    /// Thread-safe delegate used to append text to the log tab.
    /// </summary>
    /// <param name="message">The text to append to the log tab.</param>
    private void AppendTextToLogTab(string message)
    {
      // InvokeRequired required compares the thread ID of the
      // calling thread to the thread ID of the creating thread.
      // If these threads are different, it returns true.
      if (_parentRemoveStepControl.StatusLabel.InvokeRequired)
      {
        AppendTextToLogTabCallback callbackMethod = AppendTextToLogTab;
        Invoke(callbackMethod, message);
      }
      else
      {
        LogContentsTextBox.AppendText(message);
      }
    }

    /// <summary>
    /// Handles the Click event for the Collapse button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void CollapseButton_Click(object sender, EventArgs e)
    {
      foreach (var control in RemoveStepsFlowLayoutPanel.Controls)
      {
        if (!(control is RemoveStepControl))
        {
          continue;
        }

        var removeStepControl = control as RemoveStepControl;
        removeStepControl.Collapse();
      }
    }

    /// <summary>
    /// Handles the completion of the uninstall operation.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void ConfigurationEnded(object sender, EventArgs e)
    {
      Executing = false;
      _currentProgress = 100;
      SetProgress(100);

      switch (_parentRemoveStepControl.Step.Status)
      {
        case ConfigurationStepStatus.Finished:
          SetStatus(_parentRemoveStepControl.HasFailedSubSteps(false) == true
                     ? Resources.CompletedWithErrors
                     : Resources.Completed);
          break;

        case ConfigurationStepStatus.Error:
          SetStatus(Resources.Failed);
          break;
      }

      if (!UninstallNextProduct())
      {
        TerminateUninstallOperation();
      }
    }

    /// <summary>
    /// Handles the start of the uninstall operation.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void ConfigurationStarted(object sender, EventArgs e)
    {
      if (_parentRemoveStepControl == null)
      {
        return;
      }

      _currentProgress = 0;
      var stepCount = _parentRemoveStepControl.SubSteps == null
                      || _parentRemoveStepControl.SubSteps.Count == 0
                        ? 1
                        : _parentRemoveStepControl.SubSteps.Count;
      _progressIncrement = 100 / stepCount;
    }

    /// <summary>
    /// Handles the uninstall operation timing out.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
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
        _parentRemoveStepControl.Controller.CancelConfigure();
      }
    }

    /// <summary>
    /// Handles changes in the state execution of the uninstall operation.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="type">A <see cref="ConfigurationEventType"/> value.</param>
    /// <param name="message">The message to send.</param>
    private void controller_ConfigurationStatusChanged(object sender, ConfigurationEventType type, string message)
    {
      if (RemoveStepsFlowLayoutPanel.VerticalScroll.Visible)
      {
        var currentStep = _parentRemoveStepControl.Controller.CurrentStep;
        var stepControl = RemoveStepsFlowLayoutPanel.Controls.Cast<RemoveStepControl>().FirstOrDefault(c => c != null && c.Step == currentStep);
        if (stepControl != null)
        {
          RemoveStepsFlowLayoutPanel.ScrollControlIntoView(stepControl);
        }
      }

      if (LogContentsTextBox.IsDisposed)
      {
        return;
      }

      switch (type)
      {
        case ConfigurationEventType.StepStarting:
          StartStep(_parentRemoveStepControl.Controller.CurrentStep, message);
          break;

        case ConfigurationEventType.StepFinished:
          FinishStep(_parentRemoveStepControl.Controller.CurrentStep, message);

          // Set the status of the parent step if all required steps have completed.
          bool? hasFailedSubSteps = _parentRemoveStepControl.HasFailedSubSteps(true);
          if (hasFailedSubSteps != null && hasFailedSubSteps == false)
          {
            _parentRemoveStepControl.SetStatus(ConfigStepControl.SUCCESS);
          }

          break;

        default:
          if (type != ConfigurationEventType.Waiting
              && LogContentsTextBox.Text.Length != 0)
          {
            LogContentsTextBox.AppendText(Environment.NewLine);
          }

          LogContentsTextBox.AppendText(message);
          break;
      }
    }

    /// <summary>
    /// Handles the Click event for the Expand button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void ExpandButton_Click(object sender, EventArgs e)
    {
      foreach (var control in RemoveStepsFlowLayoutPanel.Controls)
      {
        if (!(control is RemoveStepControl))
        {
          continue;
        }

        var removeStepControl = control as RemoveStepControl;
        removeStepControl.Expand();
      }
    }

    /// <summary>
    /// Gets the remove step control associated to the specified step.
    /// </summary>
    /// <param name="step">The step to search for.</param>
    /// <returns>A <see cref="RemoveStepControl"/> instance that is set as the parent of the specified step.</returns>
    private RemoveStepControl FindStepControl(BaseStep step)
    {
      foreach (var removeStepControl in RemoveStepsFlowLayoutPanel.Controls.OfType<RemoveStepControl>())
      {
        RemoveStepControl removeStep;

        // If there are no sub-steps then only the current step is needed.
        if (removeStepControl.SubSteps == null
            || removeStepControl.SubSteps.Count == 0)
        {
          if (removeStepControl.Step == step)
          {
            removeStep = removeStepControl;
          }
          else
          {
            continue;
          }
        }
        else
        {
          removeStep = removeStepControl.SubSteps.FirstOrDefault(subStep => subStep.Step == step);
          if (removeStep == null)
          {
            continue;
          }

          // A required sub-step has failed, hence we need to fail the parent step too.
          if (removeStep.Step.Status == ConfigurationStepStatus.Error && removeStep.Step.Required)
          {
            removeStepControl.SetStatus(ConfigStepControl.ERROR);
            removeStepControl.Step.Status = ConfigurationStepStatus.Error;
          }
        }

        return removeStep;
      }

      return null;
    }

    /// <summary>
    /// Finalizes the specified step.
    /// </summary>
    /// <param name="step">The step to finalize.</param>
    /// <param name="message">The message to display for the specified step.</param>
    private void FinishStep(BaseStep step, string message)
    {
      var removeStepControl = FindStepControl(step);
      removeStepControl.SetStatus(step.Status == ConfigurationStepStatus.Finished
        ? ConfigStepControl.SUCCESS
        : ConfigStepControl.ERROR);
      removeStepControl.SetDots(0);
      if (_increaseProgressWhenStepCompletes)
      {
        _currentProgress += _progressIncrement;
        SetProgress(_currentProgress);
      }
      else
      {
        _increaseProgressWhenStepCompletes = true;
      }

      ProgressUpdateTimer.Stop();
      if (!string.IsNullOrEmpty(message))
      {
        LogContentsTextBox.AppendText(string.Format("{0}{1}{0}{0}", Environment.NewLine, message));
      }
    }

    /// <summary>
    /// Builds the tooltip that should be set to the RebootWhenDoneCheckBox control.
    /// </summary>
    /// <returns>A string representing the a list of products that requested a reboot; if no products require a reboot <c>null</c> is returned.</returns>
    private string GetRebootWhenDoneCheckBoxToolTip()
    {
      if (!(Wizard is RemoveProductsWizard removeWizard))
      {
        return null;
      }

      var productsListBuilder = new StringBuilder();
      foreach (var package in removeWizard.ProductsToRemove)
      {
        if (package.Controller == null
            || !package.Controller.RebootRequired)
        {
          continue;
        }

        productsListBuilder.AppendLine($"- {package.NameWithVersion}");
      }

      var productsList = productsListBuilder.ToString();
      return !string.IsNullOrEmpty(productsList)
        ? $"The following products require a reboot:{Environment.NewLine}{productsList.TrimEnd(',')}"
        : null;
    }

    /// <summary>
    /// Initializes this wizard page.
    /// </summary>
    private void Initialize()
    {
      ExecutionStepsTabPage.Text = Resources.RemoveSteps;
      UninstallInstallerLabel.Text = string.Format(UninstallInstallerLabel.Text, AppConfiguration.License);
      _increaseProgressWhenStepCompletes = true;
      ShowExpandCollapseButtons(false);
      PackageManager = new PackageManager(true);
      PackageManager.QueueStarted += QueueStarted;
      PackageManager.QueueFinished += QueueFinished;
      PackageManager.PackageStarted += PackageStarted;
      PackageManager.PackageFinished += PackageFinished;
      PackageManager.StatusChanged += PackageStatusChanged;
    }

    /// <summary>
    /// Handles the removal of a product.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void PackageUninstall(object sender, EventArgs e)
    {
      UninstallProduct();
    }

    /// <summary>
    /// Handles the Tick event of the ProgressUpdateTimer object.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void ProgressUpdateTimer_Tick(object sender, EventArgs e)
    {
      long millis = StopWatch.ElapsedMilliseconds;
      _childRemoveStepControl.Step.ElapsedTime = (int)millis / 1000;
      if (millis > _childRemoveStepControl.Step.NormalTime * 1000)
      {
        ConfigurationResultLabel.Text = string.Format(Resources.ConfigStepTakingTooLong, _childRemoveStepControl.Step.Description);
      }

      int dots = (int)(millis / 500) % 3;
      _childRemoveStepControl.SetDots(dots + 1);
    }

    /// <summary>
    /// Handles the execution start of the uninstall queue..
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void QueueStarted(object sender, EventArgs e)
    {
      UpdateButtons();
      SetControlEnabledStatus(Wizard.ExecuteButton, false);
      SetControlEnabledStatus(Wizard.NextButton, false);
      SetControlEnabledStatus(Wizard.BackButton, false);
    }

    /// <summary>
    /// Adds the specified package to the the queue of packages marked for uninstall.
    /// </summary>
    /// <param name="package">The package to add.</param>
    private void SetPackage(IPackage package)
    {
      PackageManager.ResetPackageList();
      PackageManager.QueuePackage(package, PackageAction.Remove);
      UpdateStatus(GetCurrentStepControl(package).Item2);
    }

    /// <summary>
    /// Sets the percent progress of the remove step control matching the specified package.
    /// </summary>
    /// <param name="progress">The percentage to set as the current progress.</param>
    private void SetProgress(int progress)
    {
      if (_parentRemoveStepControl == null)
      {
        return;
      }

      SetStatusLabelText(_parentRemoveStepControl, progress == -1
        ? string.Empty
        : progress + "%");
    }

    /// <summary>
    /// Sets the status to the item currently being uninstalled.
    /// </summary>
    /// <param name="status"></param>
    private void SetStatus(string status)
    {
      SetStatusLabelText(_parentRemoveStepControl, status);
    }

    /// <summary>
    /// Thread-safe delegate used to set Text property of the StatusLabel.
    /// </summary>
    /// <param name="text">The value to set for the Text property.</param>
    /// <param name="removeStepControl">The step on which to update the StatusLabel.</param>
    private void SetStatusLabelText(RemoveStepControl removeStepControl, string text)
    {
      // InvokeRequired required compares the thread ID of the
      // calling thread to the thread ID of the creating thread.
      // If these threads are different, it returns true.
      if (_parentRemoveStepControl.StatusLabel.InvokeRequired)
      {
        SetStatusLabelTextCallback callbackMethod = SetStatusLabelText;
        Invoke(callbackMethod, removeStepControl, text);
      }
      else
      {
        removeStepControl.StatusLabel.Text = text;
      }
    }

    /// <summary>
    /// Shows/hides the Expand and Collapse buttons.
    /// </summary>
    /// <param name="visible">Flag to indicate the value of the Visible property for the Expand and Collapse buttons.</param>
    private void ShowExpandCollapseButtons(bool visible)
    {
      CollapseButton.Visible =
        ExpandButton.Visible = visible;
    }

    /// <summary>
    /// Starts the execution of the specified step.
    /// </summary>
    /// <param name="step">The step to start.</param>
    /// <param name="message">The message display for the specified step.</param>
    private void StartStep(BaseStep step, string message)
    {
      var removeStepControl = FindStepControl(step);
      removeStepControl.SetStatus(ConfigStepControl.CURRENT);
      _childRemoveStepControl = removeStepControl;
      StopWatch.Restart();
      ProgressUpdateTimer.Start();
      if (!string.IsNullOrEmpty(message))
      {
        LogContentsTextBox.AppendText($"{message}{Environment.NewLine}");
      }
    }

    /// <summary>
    /// Updates the UI once all packages have been uninstalled.
    /// </summary>
    private void TerminateUninstallOperation()
    {
      subCaptionLabel.Text = Resources.RemoveStepsFinished;
      if (RemoveStepsFlowLayoutPanel.Controls.OfType<RemoveStepControl>().Any(control =>
           control.Controller.CurrentState == ConfigState.ConfigurationCancelled
           || control.Controller.CurrentState == ConfigState.ConfigurationError))
      {
        ConfigurationResultLabel.Text = Resources.RemoveFailed;
        RetryButton.Visible = true;
        RebootWhenDonePanel.Visible = true;
        RebootWhenDoneCheckBox.Visible = false;
        RebootWhenDoneLabel.Visible = false;
        Wizard.BackButton.Enabled = true;
        Wizard.BackButton.Visible = true;
      }
      else
      {
        ConfigurationResultLabel.Text = RemoveStepsFlowLayoutPanel.Controls.OfType<RemoveStepControl>().Any(control =>
                                        control.HasFailedSubSteps(false) == true)
                                          ? Resources.RemoveSuccessWithErrors
                                          : Resources.RemoveSuccess;
        RetryButton.Visible = false;
        RebootWhenDonePanel.Visible = false;
        Wizard.ExecuteButton.Visible = false;
        Wizard.CancelButton.Visible = false;
        Wizard.BackButton.Visible = false;
        Wizard.FinishButton.Visible = true;
        RemoveStepsFlowLayoutPanel.Height -= HEIGHT_REDUCED_WHEN_UNINSTALL_COMPLETE;
      }

      if (UninstallInstallerPanel.Visible)
      {
        ExecutionTabControl.Height -= HEIGHT_REDUCED_WHEN_UNINSTALL_COMPLETE;
      }

      if (RemoveStepsFlowLayoutPanel.Controls.OfType<RemoveStepControl>().Any(control => control.Controller.RebootRequired))
      {
        RebootWhenDonePanel.Visible = true;
        RebootWhenDoneCheckBox.Visible = true;
        RebootWhenDoneLabel.Visible = true;
        RebootWhenDoneCheckBox.Checked = true;
        ToolTip.SetToolTip(RebootWhenDoneCheckBox, GetRebootWhenDoneCheckBoxToolTip());

        if (UninstallInstallerPanel.Visible)
        {
          ExecutionTabControl.Height -= HEIGHT_REDUCED_WHEN_UNINSTALL_COMPLETE;
          RebootWhenDonePanel.Location = new Point(RebootWhenDonePanel.Location.X, ExecutionTabControl.Location.Y + ExecutionTabControl.Height + 5);
        }
      }

      Wizard.FinishButton.Enabled = FinishOk;
    }

    /// <summary>
    /// Executes the uninstall of the next product in the queue. 
    /// </summary>
    /// <returns></returns>
    private bool UninstallNextProduct()
    {
      DetachEvents();
      var parentRemoveStepControl = RemoveStepsFlowLayoutPanel.Controls.OfType<RemoveStepControl>().FirstOrDefault(removeStepControl =>
        removeStepControl.Step.Status == ConfigurationStepStatus.NotStarted
        && removeStepControl.Controller.CurrentState != ConfigState.ConfigurationError);
      if (parentRemoveStepControl == null)
      {
        return false;
      }

      _parentRemoveStepControl = parentRemoveStepControl;
      CurrentController = _parentRemoveStepControl.Controller;
      Executing = true;
      CurrentController.ConfigurationStarted += ConfigurationStarted;
      CurrentController.ConfigurationEnded += ConfigurationEnded;
      CurrentController.ConfigureTimedOut += ConfigureTimedOut;
      CurrentController.ConfigurationStatusChanged += controller_ConfigurationStatusChanged;
      CurrentController.PackageUninstall += PackageUninstall;
      CurrentController.Remove();
      return true;
    }

    /// <summary>
    /// Uninstalls the product for the current item.
    /// </summary>
    private void UninstallProduct()
    {
      if (CurrentController == null)
      {
        return;
      }

      SetPackage(CurrentController.Package);
      PackageManager.EnablePackage(CurrentController.Package, true);
      PackageManager.Start();
      while (_childRemoveStepControl.Step.Status != ConfigurationStepStatus.Finished && _childRemoveStepControl.Step.Status != ConfigurationStepStatus.Error)
      {
        Thread.Sleep(1000);
      }
    }

    /// <summary>
    /// Updates the status of the specified remove step control based on the state reported by the package manager.
    /// </summary>
    /// <param name="removeStepControl">The <see cref="RemoveStepControl"/> instance to update the status for.</param>
    private void UpdateStatus(RemoveStepControl removeStepControl)
    {
      if (removeStepControl == null)
      {
        return;
      }

      var package = removeStepControl.Controller.Package;
      string status = string.Empty;
      if (PackageManager.Contains(package))
      {
        status = PackageManager.GetStatus(package);
      }

      SetStatusLabelText(_parentRemoveStepControl, status);
    }
  }
}

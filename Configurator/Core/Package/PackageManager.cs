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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Core.Controllers;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Core.Interfaces;
using MySql.Configurator.Core.MSI;
using MySql.Configurator.Properties;
using TS = System.Threading.Tasks.TaskScheduler;

namespace MySql.Configurator.Core.Package
{
  public delegate void PackageStatusEventHandler(PackageStatusEventArgs args);

  public class PackageManager
  {
    private CancellationTokenSource _cancellationTokenSource;
    private StringBuilder _installActionLog;
    private SynchronizationContext _consoleContext;
    private PackageOperation _current;
    private int _maxPos;
    private int _minPos;
    private int _position;
    private int _progress;
    private bool _running;

    /// <summary>
    /// Flag to identify if Installer is running in CLI mode (console) or with a UI.
    /// </summary>
    private bool _runningOnConsole;
    private int _step;

    /// <summary>
    /// Flag to indicate if it should run in the current context because it is already running on a different thread.
    /// </summary>
    private bool _useDefaultContext;

    public PackageManager(bool useDefaultContext = false)
    {
      _consoleContext = new SynchronizationContext();
      _installActionLog = new StringBuilder();
      _useDefaultContext = useDefaultContext;
      PackageList = new List<PackageOperation>();
      _runningOnConsole = Utilities.RunningOnConsole();
    }

    public event PackageStatusEventHandler PackageFinished;

    public event PackageStatusEventHandler PackageStarted;

    public event EventHandler QueueFinished;

    public event EventHandler QueueStarted;
    public event PackageStatusEventHandler StatusChanged;
    public bool DoingPackage { get; private set; }
    public List<PackageOperation> PackageList { get; }
    public bool RebootRequired { get; private set; }
    /// <summary>
    /// Gets or sets a value indicating if Installer is running in CLI mode(console).
    /// </summary>
    public bool RunningOnConsole => _runningOnConsole;

    public void Cancel()
    {
      _cancellationTokenSource?.Cancel();
    }

    public void CancelPackage(IPackage package)
    {
      var packageOperation = GetOp(package);
      if (packageOperation != null)
      {
        packageOperation.Status = PackageStatus.Canceled;
      }
    }

    public bool Contains(IPackage package)
    {
      return GetOp(package) != null;
    }

    public void EnablePackage(Package package, bool enable)
    {
      var packageOperation = GetOp(package);
      if (packageOperation != null)
      {
        packageOperation.Enabled = enable;
      }
    }

    public string GetLog()
    {
      return _installActionLog.ToString();
    }

    public void AppendLog(string msg)
    {
      if (string.IsNullOrEmpty(msg))
      {
        return;
      }

       _installActionLog.AppendLine(msg);
    }

    public string GetStatus(IPackage package)
    {
      var packageOperation = GetOp(package);
      return packageOperation?.GetStatus(packageOperation == _current);
    }

    public void QueuePackage(IPackage package, PackageAction action)
    {
      // make sure the package is not already in the list
      if (PackageList.Any(op => op.Package == package))
      {
        throw new InvalidOperationException(Resources.PackageAlreadyQueued);
      }

      var packageOperation = new PackageOperation(package, action, true, PackageStatus.NotStarted, false);

      //Check if we can proceed with a normal operation or we need to do a two step action
      if (package.UpgradeTarget != null)
      {
        if (package.UpgradeCode == Guid.Empty || package.UpgradeTarget.UpgradeCode == Guid.Empty)
        {
          //One of the packages doesn't have an upgrade code.
          packageOperation.TwoStepsAction = true;
        }
        else if (package.UpgradeCode != package.UpgradeTarget.UpgradeCode)
        {
          //The Upgrade Code is different.
          packageOperation.TwoStepsAction = true;
        }
        else if ((package.PerMachine && !package.UpgradeTarget.PerMachine)
                 || (!package.PerMachine && package.UpgradeTarget.PerMachine))
        {
          // The per-machine/per-user setting differs between the package and the upgrade target.
          packageOperation.TwoStepsAction = true;
        }
      }

      PackageList.Add(packageOperation);
    }

    public void Start()
    {
      _cancellationTokenSource = new CancellationTokenSource();
      if (_runningOnConsole)
      {
        SynchronizationContext.SetSynchronizationContext(_consoleContext);
      }

      _running = true;
      //If we restart any process for PackageManager and reboot required is true, doesn't mean that reboot flag should be reset, the systems still needs to reboot.
      //RebootRequired = false;
      QueueStarted?.Invoke(this, EventArgs.Empty);
      Next();
    }

    private void DoAction(StepType stepType)
    {
      if (_cancellationTokenSource.IsCancellationRequested)
      {
        _current.Status = PackageStatus.Canceled;
        return;
      }

      //var installer = new ChainedInstaller();
      IPackage currentPackage;
      PackageAction currentAction;

      switch (stepType)
      {
        default:
          currentPackage = _current.Package;
          currentAction = _current.Action;
          break;

        case StepType.PreStep:
          currentPackage = _current.Package.UpgradeTarget;
          currentAction = PackageAction.Remove;
          break;
      }

      var parameters = new PackageParameters(currentPackage, currentAction);
      Logger.LogInformation($"Beginning changes for {currentPackage.NameWithVersion} with options {parameters.CommandLine}");
      //installer.AddPackage(parameters);
      //installer.Updated += installer_Updated;
      //MsiEnumError[] returnCodes = installer.Install();
      //_current.RebootRequired = returnCodes[0] == MsiEnumError.SuccessRebootRequired;
      //_current.Status = returnCodes[0] == MsiEnumError.Success
      //                  || returnCodes[0] == MsiEnumError.SuccessRebootRequired
      //  ? PackageStatus.Complete
      //  : PackageStatus.Failed;
    }

    private void EndAction()
    {
      if (_current.RebootRequired)
      {
        Logger.LogInformation($"{_current.Package.NameWithVersion}'s change state request requires a reboot.");
        RebootRequired = true;
      }

      NotifyPackageComplete(_current.Package, _current.Action, _current.Status);
      _current = null;
      if (_cancellationTokenSource.IsCancellationRequested)
      {
        return;
      }

      Next();
    }

    private void Execute()
    {
      if (!_current.Enabled
          || !NotifyPackageStarted(_current.Package, _current.Action))
      {
        Next();
      }

      if (!_running)
      {
        return;
      }

      Task task;
      if (!_current.TwoStepsAction)
      {
        task = Task.Factory.StartNew(() => DoAction(StepType.MainStep), _cancellationTokenSource.Token, TaskCreationOptions.None, TS.Default);
        if (_runningOnConsole)
        {
          task.Wait();
        }
      }
      else
      {
        _current.Package.SetProposedInstall(true);
        _current.Action = PackageAction.Install;
        task = Task.Factory.StartNew(() => DoAction(StepType.PreStep), _cancellationTokenSource.Token, TaskCreationOptions.None, TS.Default)
          .ContinueWith(t => DoAction(StepType.MainStep), _cancellationTokenSource.Token, TaskContinuationOptions.OnlyOnRanToCompletion,
          TS.FromCurrentSynchronizationContext());

        if (_runningOnConsole)
        {
          task.Wait();
        }
      }

      //TS ts = Utilities.RunningOnConsole() ? TS.Default : TS.FromCurrentSynchronizationContext();
      if (!_runningOnConsole)
      {
        task.ContinueWith(t => EndAction(),
                          _cancellationTokenSource.Token,
                          TaskContinuationOptions.OnlyOnRanToCompletion,
                          _useDefaultContext
                            ? TS.Default
                            : TS.FromCurrentSynchronizationContext());
      }
      else
      {
        EndAction();
      }
    }

    private PackageOperation GetOp(IPackage package)
    {
      return PackageList.FirstOrDefault(op => op.Package == package);
    }

    private void installer_Updated(object sender, ChainedInstallerEventArgs args)
    {
      var status = new PackageStatusEventArgs(_current.Package, _current.Action, PackageStatus.Progress, args.GetMessage(), args.IsVerbose);
      UpdateStatus(args, status);
    }

    private void Next()
    {
      DoingPackage = false;
      if (_cancellationTokenSource != null
          && _cancellationTokenSource.IsCancellationRequested)
      {
        return;
      }

      var packageOperation = PackageList.FirstOrDefault(t => t.Enabled && t.Status == PackageStatus.NotStarted);
      _current = packageOperation;
      if (packageOperation == null)
      {
        _running = false;
        QueueFinished?.Invoke(this, EventArgs.Empty);
        return;
      }

      DoingPackage = true;
      Execute();
    }

    private void NotifyPackageComplete(IPackage package, PackageAction action, PackageStatus status)
    {
      Logger.LogInformation($"{package.NameWithVersion}'s change state request {(status == PackageStatus.Complete ? "passed" : "failed")}.");

      // let the controller do any post remove cleanup
      if (status != PackageStatus.Canceled)
      {
        if (_current.TwoStepsAction)
        {
          action = PackageAction.Upgrade;
        }

        PostAction(package, action, status);
      }

      // notify our listeners that this package is done
      var args = new PackageStatusEventArgs(package, action, status);
      RebootRequired |= package.Controller.RebootRequired;

      // We ensure that the controller for this packages is reset to its default values in
      // case the user is attempting to do multiple operations without closing installer.
      if (!_runningOnConsole
          && action == PackageAction.Install
          && status == PackageStatus.Complete)
      {
        package.Controller.Init();
      }

      PackageFinished?.Invoke(args);
    }

    private bool NotifyPackageStarted(IPackage package, PackageAction action)
    {
      var args = new PackageStatusEventArgs(package, action, PackageStatus.Started);
      PackageStarted?.Invoke(args);
      if (PreAction(package, action) && !args.Cancel)
      {
        return true;
      }

      CancelPackage(package);
      NotifyPackageComplete(package, action, PackageStatus.Canceled);
      return false;
    }

    private void PostAction(IPackage package, PackageAction action, PackageStatus status)
    {
      ProductConfigurationController c = package.Controller;
      switch (action)
      {
        case PackageAction.Install:
          c.PostInstall(status);
          break;

        case PackageAction.Remove:
          c.PostRemove(status);
          break;

        case PackageAction.Configure:
          c.PostConfigure(status);
          break;

        case PackageAction.Modify:
          c.PostModify(status);
          break;

        case PackageAction.Upgrade:
          c.PostUpgrade(status);
          break;
      }
    }

    private bool PreAction(IPackage package, PackageAction action)
    {
      var controller = package.Controller;
      switch (action)
      {
        case PackageAction.Install:
          return controller.PreInstall();

        case PackageAction.Remove:
          return controller.PreRemove();

        case PackageAction.Configure:
          return controller.PreConfigure();

        case PackageAction.Modify:
          return controller.PreModify();

        case PackageAction.Upgrade:
          return controller.PreUpgrade();
      }

      return true;
    }

    /// <summary>
    /// Resets the queued package list.
    /// </summary>
    public void ResetPackageList()
    {
      PackageList.Clear();
    }

    private void SetPosition(int newPos)
    {
      _position = newPos;
      float stagePos = (float)_position / (_maxPos - _minPos);
      int newProgress = (int)(50.0f * stagePos);
      newProgress /= _current.TwoStepsAction ? 2 : 1;
      if (_progress >= 50)
      {
        newProgress += 50;
      }

      _progress = Math.Max(newProgress, _progress);
    }

    private void UpdateStatus(ChainedInstallerEventArgs ciea, PackageStatusEventArgs status)
    {
      switch (ciea.Action)
      {
        case ChainedInstallerAction.StartInstallation:
          _minPos = _maxPos = _step = _position = _progress = 0;
          break;

        case ChainedInstallerAction.ProgressSetRange:
          _minPos = ciea.ProgressMin;
          _maxPos = ciea.ProgressMax;
          _step = 1;
          _position = 0;
          if (_progress > 0)
          {
            _progress = 50;
          }
          break;

        case ChainedInstallerAction.ProgressSetStep:
          _step = ciea.ProgressStep;
          break;

        case ChainedInstallerAction.ProgressSetPosition:
          if (ciea.ProgressPosition != _maxPos || _position != 0)
          {
            SetPosition(ciea.ProgressPosition);
          }
          break;

        case ChainedInstallerAction.ProgressSingleStep:
          SetPosition(_position + _step);
          break;
      }
      
      if (_progress < 0)
      {
        _progress = 0;
      }

      if (_progress > 100)
      {
        _progress = 100;
      }

      status.Progress = _progress;
      if (StatusChanged == null)
      {
        return;
      }

      if (!_runningOnConsole
          && Application.OpenForms[0].InvokeRequired)
      {
        Application.OpenForms[0].Invoke((MethodInvoker) (() => StatusChanged(status)));
      }
      else
      {
        StatusChanged(status);
      }
    }
  }
}

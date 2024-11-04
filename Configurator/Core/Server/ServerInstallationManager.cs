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

using MySql.Configurator.Base.Classes;
using MySql.Configurator.Base.Enums;
using MySql.Configurator.Core.Logging;
using MySql.Configurator.Core.MSI;
using MySql.Configurator.Core.Settings;
using MySql.Configurator.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TS = System.Threading.Tasks.TaskScheduler;

namespace MySql.Configurator.Core.Server
{
  /// <summary>
  /// Manages the server installation operations. Currently only used for removing an existing installation during an upgrade.
  /// </summary>
  public class ServerInstallationManager
  {
    #region Fields

    /// <summary>
    /// The cancellation token source.
    /// </summary>
    private CancellationTokenSource _cancellationTokenSource;

    /// <summary>
    /// The syncronization context of the on-going operation.
    /// </summary>
    private SynchronizationContext _consoleContext;

    /// <summary>
    /// The current operation that is executing.
    /// </summary>
    private ServerInstallationOperation _currentOperation;

    /// <summary>
    /// Defines the maximum position regarding the progress percentage of the on-going operation.
    /// </summary>
    private int _maxPos;

    /// <summary>
    /// Defines the minimum position regarding the progress percentage of the on-going operation.
    /// </summary>
    private int _minPos;

    /// <summary>
    /// The string builder containing the log for the on-going operation.
    /// </summary>
    private StringBuilder _operationActionLog;

    /// <summary>
    /// Defines the current position regarding the progress percentage of the on-going operation.
    /// </summary>
    private int _position;

    /// <summary>
    /// Defines the progress percentage of the on-going operation.
    /// </summary>
    private int _progress;

    /// <summary>
    /// Flag to indicate if an operation is running.
    /// </summary>
    private bool _running;

    /// <summary>
    /// Flag to identify if Installer is running in CLI mode (console) or with a UI.
    /// </summary>
    private bool _runningOnConsole;

    /// <summary>
    /// Defines the progress percentage of the on-going operation.
    /// </summary>
    private int _step;

    /// <summary>
    /// Flag to indicate if it should run in the current context because it is already running on a different thread.
    /// </summary>
    private bool _useDefaultContext;

    #endregion

    #region Delegates

    /// <summary>
    /// Delegate used to define custom actions for an on-going operation.
    /// </summary>
    /// <param name="args">The arguments of the operation.</param>
    public delegate void ServerInstallationStatusEventHandler(ServerInstallationStatusEventArgs args);

    #endregion

    /// <summary>
    /// Initializes the server installation manager instance.
    /// </summary>
    /// <param name="useDefaultContext">Flag indicating if the default synchronization contest should be used.</param>
    public ServerInstallationManager(bool useDefaultContext = false)
    {
      _consoleContext = new SynchronizationContext();
      _operationActionLog = new StringBuilder();
      _useDefaultContext = useDefaultContext;
      ServerInstallationList = new List<ServerInstallationOperation>();
      _runningOnConsole = Utilities.RunningOnConsole();
    }

    #region Events

    /// <summary>
    /// Event used to signal that the queue of operations have completed.
    /// </summary>
    public event EventHandler QueueFinished;

    /// <summary>
    /// Event used to signal that the queue of operations have started.
    /// </summary>
    public event EventHandler QueueStarted;

    /// <summary>
    /// Event to signal that the current server installation operation has finished.
    /// </summary>
    public event ServerInstallationStatusEventHandler ServerInstallationFinished;

    /// <summary>
    /// Event to signal that the current server installation operation has started.
    /// </summary>
    public event ServerInstallationStatusEventHandler ServerInstallationStarted;

    /// <summary>
    /// Event to signal that there has been a change in the progress of the current server installation operation.
    /// </summary>
    public event ServerInstallationStatusEventHandler StatusChanged;

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets a flag indicating if there is an on-going server installation operation.
    /// </summary>
    public bool DoingServerInstallation { get; private set; }

    /// <summary>
    /// Gets the list of server installations.
    /// </summary>
    public List<ServerInstallationOperation> ServerInstallationList { get; }

    /// <summary>
    /// Flag to indicate if a server installation requires that the computer is rebooted.
    /// </summary>
    public bool RebootRequired { get; private set; }

    #endregion

    /// <summary>
    /// Loads a server installation instance with the specified version and installation directory.
    /// </summary>
    /// <param name="versionString">The string representation of the version of the server installation.</param>
    /// <param name="installationDirectory">The installation directory of the server installation.</param>
    /// <returns>An instance of a server installation.</returns>
    public static ServerInstallation LoadServerInstallation(string versionString, string installationDirectory)
    {
      var serverInstallation = new ServerInstallation(AppConfiguration.License);
      serverInstallation.Initialize(versionString, installationDirectory);
      return serverInstallation;
    }

    /// <summary>
    /// Loads a generic server installation instance.
    /// </summary>
    /// <returns>An instance of a server installation with a default version and installation directory.</returns>
    public static ServerInstallation LoadGenericServerInstallation()
    {
      var serverInstallation = new ServerInstallation(AppConfiguration.License);
      serverInstallation.Initialize("0.0.0", null);
      return serverInstallation;
    }

    /// <summary>
    /// Cancels the on-going operation.
    /// </summary>
    public void Cancel()
    {
      _cancellationTokenSource?.Cancel();
    }

    /// <summary>
    /// Cancels the specified server installation.
    /// </summary>
    /// <param name="serverInstallation">The server installation to cancel.</param>
    public void CancelServerInstallation(ServerInstallation serverInstallation)
    {
      var serverInstallationOperation = GetServerInstallationOperation(serverInstallation);
      if (serverInstallationOperation != null)
      {
        serverInstallationOperation.Status = ServerInstallationStatus.Canceled;
      }
    }

    /// <summary>
    /// Validates that the specified server installation is in the list of operations.
    /// </summary>
    /// <param name="serverInstallation">The server installation to check for.</param>
    /// <returns><c>true</c> if the server installation was found in the queue; otherwise, <c>false</c>.</returns>
    public bool Contains(ServerInstallation serverInstallation)
    {
      return GetServerInstallationOperation(serverInstallation) != null;
    }

    /// <summary>
    /// Marks the specified server installation as valid or invalid for being executed.
    /// </summary>
    /// <param name="serverInstallation">The server installation to enable/disable.</param>
    /// <param name="enable">A flag indicating if the server installation should be enabled or disabled.</param>
    public void EnableServerInstallation(ServerInstallation serverInstallation, bool enable)
    {
      var serverInstallationOperation = GetServerInstallationOperation(serverInstallation);
      if (serverInstallationOperation != null)
      {
        serverInstallationOperation.Enabled = enable;
      }
    }

    /// <summary>
    /// Gets a string representing the log of all the operations executed.
    /// </summary>
    /// <returns>A string with the log of the operations executed.</returns>
    public string GetLog()
    {
      return _operationActionLog.ToString();
    }

    /// <summary>
    /// Appends the specified message to the log.
    /// </summary>
    /// <param name="msg">The meesage to append.</param>
    public void AppendLog(string msg)
    {
      if (string.IsNullOrEmpty(msg))
      {
        return;
      }

      _operationActionLog.AppendLine(msg);
    }

    /// <summary>
    /// Gets the operation status of the specified server installation operation.
    /// </summary>
    /// <param name="serverInstallation">The server installation for which to get the status.</param>
    /// <returns></returns>
    public string GetStatus(ServerInstallation serverInstallation)
    {
      var packageOperation = GetServerInstallationOperation(serverInstallation);
      return packageOperation?.GetStatus(packageOperation == _currentOperation);
    }

    /// <summary>
    /// Adds the specified server installation to the queue with the specified operation.
    /// </summary>
    /// <param name="serverInstallation">The server installation.</param>
    /// <param name="action">The operation to execute for the specified server installation.</param>
    public void QueueServerInstallation(ServerInstallation serverInstallation, ServerInstallationAction action)
    {
      // Make sure the server installation is not already in the list.
      if (ServerInstallationList.Any(op => op.ServerInstallation == serverInstallation))
      {
        throw new InvalidOperationException(Resources.ServerInstallationAlreadyQueued);
      }

      var serverInstallationeOperation = new ServerInstallationOperation(serverInstallation, action, true, ServerInstallationStatus.NotStarted, false);
      ServerInstallationList.Add(serverInstallationeOperation);
    }

    /// <summary>
    /// Initiates the server installation operations.
    /// </summary>
    public void Start()
    {
      _cancellationTokenSource = new CancellationTokenSource();
      if (_runningOnConsole)
      {
        SynchronizationContext.SetSynchronizationContext(_consoleContext);
      }

      _running = true;
      QueueStarted?.Invoke(this, EventArgs.Empty);
      Next();
    }

    /// <summary>
    /// Method used with a task object to initiate the server installation operations on a new thread.
    /// </summary>
    /// <param name="stepType">The type of step to execute.</param>
    private void DoAction(StepType stepType)
    {
      if (_cancellationTokenSource.IsCancellationRequested)
      {
        _currentOperation.Status = ServerInstallationStatus.Canceled;
        return;
      }

      ServerInstallation currentServerInstallation;
      ServerInstallationAction currentAction;

      switch (stepType)
      {
        default:
          currentServerInstallation = _currentOperation.ServerInstallation;
          currentAction = _currentOperation.Action;
          break;
      }
    }

    /// <summary>
    /// Method used with a task object to mark the server installation operations as completed.
    /// </summary>
    private void EndAction()
    {
      if (_currentOperation.RebootRequired)
      {
        Logger.LogInformation($"{_currentOperation.ServerInstallation.NameWithVersion}'s change state request requires a reboot.");
        RebootRequired = true;
      }

      NotifyServerInstallationComplete(_currentOperation.ServerInstallation, _currentOperation.Action, _currentOperation.Status);
      _currentOperation = null;
      if (_cancellationTokenSource.IsCancellationRequested)
      {
        return;
      }

      Next();
    }


    /// <summary>
    /// Method used to trigger the execution of the server installation operations in a new thread.
    /// </summary>
    private void Execute()
    {
      if (!_currentOperation.Enabled
          || !NotifyServerInstallationStarted(_currentOperation.ServerInstallation, _currentOperation.Action))
      {
        Next();
      }

      if (!_running)
      {
        return;
      }

      Task task;
      if (!_currentOperation.TwoStepsAction)
      {
        task = Task.Factory.StartNew(() => DoAction(StepType.MainStep), _cancellationTokenSource.Token, TaskCreationOptions.None, TS.Default);
        if (_runningOnConsole)
        {
          task.Wait();
        }
      }
      else
      {
        _currentOperation.Action = ServerInstallationAction.Install;
        task = Task.Factory.StartNew(() => DoAction(StepType.PreStep), _cancellationTokenSource.Token, TaskCreationOptions.None, TS.Default)
          .ContinueWith(t => DoAction(StepType.MainStep), _cancellationTokenSource.Token, TaskContinuationOptions.OnlyOnRanToCompletion,
          TS.FromCurrentSynchronizationContext());

        if (_runningOnConsole)
        {
          task.Wait();
        }
      }

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

    /// <summary>
    /// Gets the operation object associated to the specified server installation.
    /// </summary>
    /// <param name="serverInstallation">The server installation.</param>
    /// <returns>A server installation operation object.</returns>
    private ServerInstallationOperation GetServerInstallationOperation(ServerInstallation serverInstallation)
    {
      return ServerInstallationList.FirstOrDefault(op => op.ServerInstallation == serverInstallation);
    }

    /// <summary>
    /// Switches to the next server installation operation.
    /// </summary>
    private void Next()
    {
      DoingServerInstallation = false;
      if (_cancellationTokenSource != null
          && _cancellationTokenSource.IsCancellationRequested)
      {
        return;
      }

      var packageOperation = ServerInstallationList.FirstOrDefault(t => t.Enabled && t.Status == ServerInstallationStatus.NotStarted);
      _currentOperation = packageOperation;
      if (packageOperation == null)
      {
        _running = false;
        QueueFinished?.Invoke(this, EventArgs.Empty);
        return;
      }

      DoingServerInstallation = true;
      Execute();
    }

    /// <summary>
    /// Event method triggered to mark a server installation operation as complete.
    /// </summary>
    /// <param name="serverInstallation">The server installation.</param>
    /// <param name="action">The type of the server installation operation.</param>
    /// <param name="status">The status of the execution of the operation.</param>
    private void NotifyServerInstallationComplete(ServerInstallation serverInstallation, ServerInstallationAction action, ServerInstallationStatus status)
    {
      Logger.LogInformation($"{serverInstallation.NameWithVersion}'s change state request {(status == ServerInstallationStatus.Complete ? "passed" : "failed")}.");

      // let the controller do any post remove cleanup
      if (status != ServerInstallationStatus.Canceled)
      {
        if (_currentOperation.TwoStepsAction)
        {
          action = ServerInstallationAction.Upgrade;
        }

        PostAction(serverInstallation, action, status);
      }

      // notify our listeners that this package is done
      var args = new ServerInstallationStatusEventArgs(serverInstallation, action, status);
      RebootRequired |= serverInstallation.Controller.RebootRequired;

      // We ensure that the controller for this server installations is reset to its default values in
      // case the user is attempting to do multiple operations without closing installer.
      if (!_runningOnConsole
          && action == ServerInstallationAction.Install
          && status == ServerInstallationStatus.Complete)
      {
        serverInstallation.Controller.Init();
      }

      ServerInstallationFinished?.Invoke(args);
    }

    /// <summary>
    /// Event method triggered to mark a server installation operation as started.
    /// </summary>
    /// <param name="serverInstallation">The server installation.</param>
    /// <param name="action">The type of the server installation operation.</param>
    private bool NotifyServerInstallationStarted(ServerInstallation serverInstallation, ServerInstallationAction action)
    {
      var args = new ServerInstallationStatusEventArgs(serverInstallation, action, ServerInstallationStatus.Started);
      ServerInstallationStarted?.Invoke(args);
      if (PreAction(serverInstallation, action) && !args.Cancel)
      {
        return true;
      }

      CancelServerInstallation(serverInstallation);
      NotifyServerInstallationComplete(serverInstallation, action, ServerInstallationStatus.Canceled);
      return false;
    }

    /// <summary>
    /// Event method triggered after an operation has been marked as complete.
    /// </summary>
    /// <param name="serverInstallation">The server installation.</param>
    /// <param name="action">The type of the server installation operation.</param>
    /// <param name="status">The status of the execution of the operation.</param>
    private void PostAction(ServerInstallation serverInstallation, ServerInstallationAction action, ServerInstallationStatus status)
    {
      var c = serverInstallation.Controller;
      switch (action)
      {
        case ServerInstallationAction.Install:
          c.PostInstall(status);
          break;

        case ServerInstallationAction.Remove:
          c.PostRemove(status);
          break;

        case ServerInstallationAction.Configure:
          c.PostConfigure(status);
          break;

        case ServerInstallationAction.Modify:
          c.PostModify(status);
          break;

        case ServerInstallationAction.Upgrade:
          c.PostUpgrade(status);
          break;
      }
    }

    /// <summary>
    /// Event method triggered before an operation is started.
    /// </summary>
    /// <param name="serverInstallation">The server installation.</param>
    /// <param name="action">The type of the server installation operation.</param>
    private bool PreAction(ServerInstallation serverInstallation, ServerInstallationAction action)
    {
      var controller = serverInstallation.Controller;
      switch (action)
      {
        case ServerInstallationAction.Install:
          return controller.PreInstall();

        case ServerInstallationAction.Remove:
          return controller.PreRemove();

        case ServerInstallationAction.Configure:
          return controller.PreConfigure();

        case ServerInstallationAction.Modify:
          return controller.PreModify();

        case ServerInstallationAction.Upgrade:
          return controller.PreUpgrade();
      }

      return true;
    }

    /// <summary>
    /// Resets the queued server installation list.
    /// </summary>
    public void ResetServerInstallationList()
    {
      ServerInstallationList.Clear();
    }

    /// <summary>
    /// Sets the new position for the progress of the on-going server installation operation.
    /// </summary>
    /// <param name="newPos">The new position.</param>
    private void SetPosition(int newPos)
    {
      _position = newPos;
      float stagePos = (float)_position / (_maxPos - _minPos);
      int newProgress = (int)(50.0f * stagePos);
      newProgress /= _currentOperation.TwoStepsAction ? 2 : 1;
      if (_progress >= 50)
      {
        newProgress += 50;
      }

      _progress = Math.Max(newProgress, _progress);
    }

    /// <summary>
    /// Updates the reported progress of the on-going server installation operation.
    /// </summary>
    /// <param name="ciea">Event arguments.</param>
    /// <param name="status">The current status.</param>
    private void UpdateStatus(ChainedInstallerEventArgs ciea, ServerInstallationStatusEventArgs status)
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
        Application.OpenForms[0].Invoke((MethodInvoker)(() => StatusChanged(status)));
      }
      else
      {
        StatusChanged(status);
      }
    }
  }
}

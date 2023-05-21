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
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Core.Dialogs;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Core.Wizard;
using TS = System.Threading.Tasks.TaskScheduler;
using Utilities = MySql.Configurator.Core.Classes.Utilities;

namespace MySql.Configurator.Core.Controllers
{
  public delegate void ConfigurationStatusChangeEventHandler(object sender, ConfigurationEventType type, string msg);

  public class ProductConfigurationController
  {
    #region Constants

    public const int DEFAULT_TIMEOUT_IN_SECONDS = 30;

    #endregion Constants

    #region Fields

    private int _configurationTimeout;
    protected ControllerSettings settings;
    private bool _inTimerCallback;
    private System.Threading.Timer _timer;
    private bool _useStatusesList;

    #endregion Fields

    public ProductConfigurationController()
    {
      UseStatusesList = false;
      Pages = new List<ConfigWizardPage>();
      _configurationTimeout = DEFAULT_TIMEOUT_IN_SECONDS;
      ConfigurationSummaryText = null;
    }

    #region Events

    public event EventHandler ConfigurationEnded;

    public event EventHandler ConfigurationStarted;

    public event ConfigurationStatusChangeEventHandler ConfigurationStatusChanged;

    public event EventHandler ConfigureTimedOut;

    /// <summary>
    /// Event handler for performing an uninstall.
    /// </summary>
    public event EventHandler PackageUninstall;

    #endregion Events

    #region Properties

    public virtual bool CanAddUsers => false;

    public CancellationTokenSource CancellationTokenSource { get; private set; }

    /// <summary>
    /// Gets or sets the current step.
    /// </summary>
    public BaseStep CurrentStep { get; protected set; }

    /// <summary>
    /// Gets or sets the list of configuration steps.
    /// </summary>
    public List<ConfigurationStep> ConfigurationSteps { get; protected set; }

    public string ConfigurationSummaryText { get; set; }

    public ConfigurationType ConfigurationType { get; set; }

    public ConfigState CurrentState { get; protected set; }

    public string InstallDirectory
    {
      get => Settings.InstallDirectory;
      set => Settings.InstallDirectory = value;
    }

    /// <summary>
    /// Gets a value indicating whether the upgrade is being made from one series to another.
    /// </summary>
    public bool IsSeriesUpgrade { get; set; }

    public Package.Package Package { get; set; }

    public List<ConfigWizardPage> Pages { get; protected set; }

    public bool RebootRequired { get; set; }

    /// <summary>
    /// Gets or sets the list of removal steps.
    /// </summary>
    public List<RemoveStep> RemoveSteps { get; protected set; }

    public ControllerSettings Settings => settings;

    public List<string> StatusesList { get; private set; }

    public bool UseStatusesList
    {
      get => _useStatusesList;
      set
      {
        _useStatusesList = value;
        StatusesList = value
          ? new List<string>()
          : null;
      }
    }

    #endregion Properties

    public virtual bool AddUser(ValueList valueList, out string msg)
    {
      msg = "Unable to add new users in this mode";
      return false;
    }

    public virtual void AfterConfigurationEnded()
    {
    }

    /// <summary>
    /// Flags the configuration operation as cancelled
    /// </summary>
    public virtual void CancelConfigure()
    {
      CancellationTokenSource.Cancel();
      DisposeTimer();
      FinalizeConfigState();
    }

    public bool CanConfigure(ConfigurationType configurationType)
    {
      var configStates = ConfigState.ConfigurationRequired
                         | ConfigState.ConfigurationComplete
                         | ConfigState.ConfigurationCompleteWithWarnings
                         | ConfigState.ConfigurationCancelled;
      return configStates.HasFlag(CurrentState)
             && HasConfigStepsForType(configurationType);
    }

    public virtual void Configure()
    {
      ResetCancellationToken();
      CurrentState = ConfigState.ConfigurationInProgress;
      ConfigurationStarted?.Invoke(this, EventArgs.Empty);
      Logger.LogInformation("Starting configuration of " + Package.NameWithVersion);
      ResetTimer();

      bool console = Utilities.RunningOnConsole();
      var task = Task.Factory.StartNew(DoConfigure, CancellationTokenSource.Token, TaskCreationOptions.None, TS.Default)
         .ContinueWith(t => EndConfigure(), CancellationTokenSource.Token, TaskContinuationOptions.OnlyOnRanToCompletion,
         console ? TS.Default : TS.FromCurrentSynchronizationContext());
      if (Utilities.RunningOnConsole())
      {
        task.Wait();
      }
    }

    public virtual void GetAdvancedOptions()
    {
      using (var baseAdvancedOptionsDialog = new BaseAdvancedOptionsDialog(Package))
      {
        try
        {
          baseAdvancedOptionsDialog.ShowDialog();
        }
        catch (Exception ex)
        {
          Logger.LogException(ex);
        }
      }
    }

    public virtual string GetMsiCommandLine()
    {
      return string.Format(" INSTALLDIR=\"{0}\" INSTALLLOCATION=\"{0}\" ARPINSTALLLOCATION=\"{0}\" INSTALL_ROOT=\"{0}\"", InstallDirectory);
    }

    public bool HasConfigStepsForType(ConfigurationType type)
    {
      return ConfigurationSteps != null
             && ConfigurationSteps.Any(step => step.ValidForConfigureType(type) && step.Execute);
    }

    public bool HasConfigurationChanges()
    {
      return Settings.HasChanges();
    }

    public bool HasPageForConfigureType(ConfigurationType type)
    {
      return Pages.Any(page => page.ValidForType(type));
    }

    public virtual void Init()
    {
      if (settings == null)
      {
        Logger.LogInformation("Product Configuration Controller - Init - Creating settings");
        settings = new ControllerSettings(Package);
      }

      if (!Utilities.RunningOnConsole()
          && !Utilities.RunningOnTask())
      {
        Logger.LogInformation("Product Configuration Controller - Init - Setting pages");
        SetPages();
      }

      ResetCancellationToken();
    }

    public virtual void Initialize(bool afterInstallation)
    {
    }

    public void LoadState()
    {
      Logger.LogInformation("Product Configuration Controller - Initializing controller");
      if (Settings == null)
      {
        Init();
      }

      Logger.LogInformation("Product Configuration Controller - Loading Settings state");
      Settings.LoadState();
      if (Package.License == LicenseType.Unknown)
      {
        Logger.LogInformation("Product Configuration Controller - Setting License from location");
        SetLicenseFromLocation();
      }

      if (Package.Architecture == PackageArchitecture.Unknown)
      {
        Logger.LogInformation("Product Configuration Controller - Setting architecture from location");
        SetArchitectureFromLocation();
      }
    }

    public virtual void PrepareForConfigure()
    {
      Settings.CloneToOldSettings();
    }

    /// <summary>
    /// Asynchronously handles the uninstall operation of the product associated to this controller.
    /// </summary>
    public virtual void Remove()
    {
      ResetCancellationToken();
      CurrentState = ConfigState.ConfigurationInProgress;
      ConfigurationStarted?.Invoke(this, EventArgs.Empty);
      Logger.LogInformation("Starting removal of " + Package.NameWithVersion);
      ResetTimer();

      bool console = Utilities.RunningOnConsole();
      var task = Task.Factory.StartNew(DoRemove, CancellationTokenSource.Token, TaskCreationOptions.None, TS.Default)
         .ContinueWith(t => EndRemove(), CancellationTokenSource.Token, TaskContinuationOptions.OnlyOnRanToCompletion,
         console ? TS.Default : TS.FromCurrentSynchronizationContext());
      if (Utilities.RunningOnConsole())
      {
        task.Wait();
      }
    }

    /// <summary>
    /// Resets the timer if it has already been initialized.
    /// </summary>
    private void ResetTimer()
    {
      var value = _configurationTimeout * 1000;
      if (_timer != null)
      {
        _timer.Change(value, value);
      }

      _timer = new System.Threading.Timer(TimerElapsed, null, value, value);
      return;
    }

    public bool SetConfigurationValue(string[] pair, out string message)
    {
      if (Settings.OldSettings == null)
      {
        Settings.CloneToOldSettings();
      }

      if (Settings == null)
      {
        throw new InvalidOperationException("Settings object has not been initialized");
      }

      if (pair == null || pair.Length != 2)
      {
        throw new InvalidOperationException("Value pair is either null or doesn't have the right number of elements.");
      }

      if (string.IsNullOrEmpty(pair[0]) || string.IsNullOrEmpty(pair[1]))
      {
        throw new InvalidOperationException("Empty or blank settings are not allowed");
      }

      return Settings.SetValue(pair[0], pair[1], out message);
    }

    public virtual void SetPages() { }

    /// <summary>
    /// Updates the configuration steps each time a configuration is run.
    /// </summary>
    public virtual void UpdateConfigurationSteps()
    {
      // Not all products have configuration steps.
      if (ConfigurationSteps == null
          || ConfigurationSteps.Count == 0)
      {
        _configurationTimeout = 0;
        return;
      }

      _configurationTimeout = ConfigurationSteps.Sum(step => step.ValidForConfigureType(ConfigurationType) && step.Execute
                                              ? (step.NormalTime > 0 ? step.NormalTime : DEFAULT_TIMEOUT_IN_SECONDS)
                                              : 0);
    }

    /// <summary>
    /// Updates the remove steps each during an uninstall operation.
    /// </summary>
    public virtual void UpdateRemoveSteps()
    {
      if (RemoveSteps == null)
      {
        RemoveSteps = new List<RemoveStep>(){};
      }

      _configurationTimeout = RemoveSteps.Sum(step => step.Execute
                                               ? (step.NormalTime > 0 ? step.NormalTime : DEFAULT_TIMEOUT_IN_SECONDS)
                                               : 0);
    }

    protected void ReportError(string errorMessage)
    {
      if (string.IsNullOrEmpty(errorMessage))
      {
        return;
      }

      CurrentState = ConfigState.ConfigurationError;
      ReportStatus(ConfigurationEventType.Error, errorMessage);
      Logger.LogError(errorMessage);
    }

    protected void ReportStatus(string message)
    {
      if (string.IsNullOrEmpty(message))
      {
        return;
      }

      if (UseStatusesList)
      {
        StatusesList?.Add(message);
      }

      ReportStatus(ConfigurationEventType.Info, message);
      Logger.LogInformation(message);
    }

    public void ReportWaiting(string token = ".")
    {
      if (string.IsNullOrEmpty(token))
      {
        return;
      }

      ReportStatus(ConfigurationEventType.Waiting, token);
    }

    protected void ResetCancellationToken()
    {
      if (CancellationTokenSource != null
          && CancellationTokenSource.IsCancellationRequested)
      {
        // Ensure the configuration starts with a fresh cancellation token.
        CancellationTokenSource.Dispose();
        CancellationTokenSource = null;
      }

      if (CancellationTokenSource == null)
      {
        CancellationTokenSource = new CancellationTokenSource();
      }
    }

    private void DisposeTimer()
    {
      if (_timer == null)
      {
        return;
      }

      _timer.Dispose();
      _timer = null;
    }

    private void DoConfigure()
    {
      foreach (var step in ConfigurationSteps.Where(step => step.ValidForConfigureType(ConfigurationType) && step.Execute).TakeWhile(step => !CancellationTokenSource.IsCancellationRequested))
      {
        CurrentStep = step;
        // report starting
        ReportStatus(ConfigurationEventType.StepStarting, "Beginning configuration step: " + step.Description);
        step.Status = ConfigurationStepStatus.Started;

        // now do the configure step
        try
        {
          step.Status = ConfigurationStepStatus.Finished;
          step.StepMethod();
        }
        catch (Exception ex)
        {
          step.Status = ConfigurationStepStatus.Error;
          ReportError(ex.Message);
        }

        // report stop
        ReportStatus(ConfigurationEventType.StepFinished, "Ended configuration step: " + step.Description);
        if (step.Required && step.Status == ConfigurationStepStatus.Error)
        {
          break;
        }
      }
    }

    /// <summary>
    /// Executes the step that perform the uninstall operation.
    /// </summary>
    private void DoRemove()
    {
      RemoveStep previousStep = null;
      foreach (var step in RemoveSteps.Where(step => step.Execute).TakeWhile(step => !CancellationTokenSource.IsCancellationRequested))
      {
        CurrentStep = step;
        // Report starting,
        ReportStatus(ConfigurationEventType.StepStarting, $"Beginning remove step: {step.Description}");
        step.Status = ConfigurationStepStatus.Started;

        // Now do the remove step.
        try
        {
          step.StepMethod();
        }
        catch (Exception ex)
        {
          step.Status = ConfigurationStepStatus.Error;
          ReportError(ex.Message);
        }

        // Report stop.
        ReportStatus(ConfigurationEventType.StepFinished, $"Ended remove step: {step.Description}");
        if (step.Required && step.Status == ConfigurationStepStatus.Error)
        {
          CurrentState = ConfigState.ConfigurationError;
          break;
        }

        previousStep = step;
      }
    }

    private void EndConfigure()
    {
      DisposeTimer();
      FinalizeConfigState();
      Logger.LogInformation("Finished configuration of " + Package.NameWithVersion + " with state " + CurrentState);
      ConfigurationEnded?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Reports the completion of the uninstall operation.
    /// </summary>
    private void EndRemove()
    {
      DisposeTimer();
      FinalizeRemoveState();
      Logger.LogInformation("Finished removal of " + Package.NameWithVersion + " with state " + CurrentState);
      ConfigurationEnded?.Invoke(this, EventArgs.Empty);
    }

    private void FinalizeConfigState()
    {
      CurrentState = ConfigState.ConfigurationComplete;
      //if (ConfigurationCancelled)
      if (CancellationTokenSource.IsCancellationRequested)
      {
        CurrentState = ConfigState.ConfigurationCancelled;
        ResetCancellationToken();
        return;
      }

      // Check if any of the steps generated an error.
      if (ConfigurationSteps.Any(step => step.ValidForConfigureType(ConfigurationType) && step.Execute && step.Status == ConfigurationStepStatus.Error))
      {
        CurrentState = ConfigState.ConfigurationError;
      }
    }

    /// <summary>
    /// Finalizes the overall uninstall operation.
    /// </summary>
    private void FinalizeRemoveState()
    {
      CurrentState = ConfigState.ConfigurationComplete;
      if (CancellationTokenSource.IsCancellationRequested)
      {
        CurrentState = ConfigState.ConfigurationCancelled;
        return;
      }

      // Check if any of the required steps generated an error.
      if (RemoveSteps.Any(step => step.Execute && step.Required && step.Status == ConfigurationStepStatus.Error))
      {
        CurrentState = ConfigState.ConfigurationError;
      }
    }

    private void ReportStatus(ConfigurationEventType type, string details)
    {
      if (ConfigurationStatusChanged == null)
      {
        return;
      }

      if (!Utilities.RunningOnConsole()
          && Application.OpenForms[0].InvokeRequired)
      {
        Application.OpenForms[0].Invoke((MethodInvoker)delegate { ConfigurationStatusChanged(this, type, details); });
      }
      else
      {
        ConfigurationStatusChanged(this, type, details);
      }
    }

    /// <summary>
    /// Sets the default state.
    /// </summary>
    public void ResetState()
    {
      CurrentState = ConfigState.ConfigurationRequired;
    }

    private void SetArchitectureFromLocation()
    {
      if (string.IsNullOrEmpty(InstallDirectory))
      {
        return;
      }

      if (!Environment.Is64BitOperatingSystem)
      {
        Package.Architecture = PackageArchitecture.X86;
        return;
      }

      // Of course this is a best guess, only falling in this scenario if the installed version of a package is not in the products manifest
      var programFilesX86Info = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86));
      var programFilesX64Info = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));
      var installDirectoryInfo = new DirectoryInfo(InstallDirectory);
      Package.Architecture = installDirectoryInfo.FullName.Contains(programFilesX64Info.FullName)
        ? PackageArchitecture.X64
        : installDirectoryInfo.FullName.Contains(programFilesX86Info.FullName)
          ? PackageArchitecture.X86
          : PackageArchitecture.Unknown;
    }

    private void SetLicenseFromLocation()
    {
      if (string.IsNullOrEmpty(InstallDirectory))
      {
        return;
      }

      //check if the directory exists, if not and is a 64bit OS try to find the install dir in program files x(86) if not, set the license as unknown
      //this fix a problem when the product is installed by 1.3 Installer
      if (!Directory.Exists(InstallDirectory))
      {
        var dirName = InstallDirectory.Split(new[] {"\\"}, StringSplitOptions.RemoveEmptyEntries);
        if (dirName.Length <= 0
            || dirName[1].ToLower() != "program files"
            || !Win32.Is64BitOs)
        {
          return;
        }

        string dir = InstallDirectory.Replace(dirName[1], dirName[1] + " (x86)");
        if (!Directory.Exists(dir))
        {
          return;
        }

        InstallDirectory = dir;
      }
    }

    #region Lifecycle Events

    public virtual void PostConfigure(PackageStatus status)
    {
    }

    public virtual void PostInstall(PackageStatus status)
    {
    }

    public virtual void PostModify(PackageStatus status)
    {
      if (status == PackageStatus.Complete)
      {
        Package.SetFeatureStates();
      }
    }

    public virtual void PostRemove(PackageStatus status)
    {
    }

    public virtual void PostUpgrade(PackageStatus status)
    {
      if (status != PackageStatus.Complete)
      {
        return;
      }

      Package.UpgradeTarget = null;
    }

    /// <summary>
    /// Executes actions after a multi-package upgrade.
    /// </summary>
    /// <param name="package">The package to be affected.</param>
    /// <param name="status">The status of the operation.</param>
    public virtual void PostUpgrade(Package.Package package, PackageStatus status)
    {
      if (status != PackageStatus.Complete
          || package == null)
      {
        return;
      }

      package = null;
    }

    public virtual bool PreConfigure()
    {
      return true;
    }

    public virtual bool PreInstall()
    {
      return true;
    }

    public virtual bool PreModify()
    {
      return true;
    }

    public virtual bool PreRemove()
    {
      return true;
    }

    public virtual bool PreUpgrade()
    {
      return true;
    }

    public virtual bool RequiresUninstallConfiguration()
    {
      return false;
    }

    #endregion

    private void TimerElapsed(object state)
    {
      if (_inTimerCallback)
      {
        return;
      }

      _inTimerCallback = true;
      ConfigureTimedOut?.Invoke(this, EventArgs.Empty);
      if (!CancellationTokenSource.IsCancellationRequested)
      {
        _timer?.Change(_configurationTimeout * 1000, _configurationTimeout * 1000);
      }

      _inTimerCallback = false;
    }

    /// <summary>
    /// Uninstalls the product associated to this controller.
    /// </summary>
    /// <returns><c>true</c> if the product uninstalled sucessfully; otherwise, <c>false</c>.</returns>
    public void UninstallProduct()
    {
      if (Package == null)
      {
        return;
      }

      PackageUninstall?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Step to trigger the uninstall of the product associated to this controller.
    /// </summary>
    public void UninstallProductStep()
    {
      UninstallProduct();
    }
  }
}

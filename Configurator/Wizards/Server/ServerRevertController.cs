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

using MySql.Configurator.Core.Classes.MySql;
using MySql.Configurator.Core.Common;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Properties;
using System;
using System.Collections.Generic;
using System.IO;

namespace MySql.Configurator.Wizards.Server
{
  /// <summary>
  /// Provides properties and methods to revert to the previous configuration if the current server configuration failed.
  /// </summary>
  public class ServerRevertController
  {
    #region Fields

    /// <summary>
    /// The list of steps that have been reverted.
    /// </summary>
    public List<string> _revertedSteps;

    #endregion

    #region Properties

    /// <summary>
    /// Flag indicating if the authentication plugin for the root user was updated.
    /// </summary>
    public bool AuthenticationPluginUpdated { get; set; }

    /// <summary>
    /// Flag indicating if the data dir was renamed.
    /// </summary>
    public bool DataDirRenamed { get; set; }

    /// <summary>
    /// Gets or sets the existing server installation instance.
    /// </summary>
    public LocalServerInstance ExistingServerInstallationInstance { get; set; }

    /// <summary>
    /// Flag indicating if the ini file was renamed.
    /// </summary>
    public bool IniFileUpdated { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating if the existing instance was shutdown.
    /// </summary>
    public bool InstanceStopped { get; set; }

    /// <summary>
    /// Gets or sets the name assigned to the new data directory.
    /// </summary>
    public string NewDataDirPath { get; set; }

    /// <summary>
    /// Gets or sets the new service name.
    /// </summary>
    public string NewServiceName { get; set; }

    /// <summary>
    /// Gets or sets the name assigned to the old data directory.
    /// </summary>
    public string OldDataDirPath { get; set; }

    /// <summary>
    /// Gets or sets the name assigned to the old ini file created as backup.
    /// </summary>
    public string OldIniFileName { get; set; }

    /// <summary>
    /// Gets or sets the authentication plugin previously assigned to the root user.
    /// </summary>
    public MySqlAuthenticationPluginType OldRootUserAuthenticationPlugin { get; set; }

    /// <summary>
    /// Gets or sets the old service name.
    /// </summary>
    public string OldServiceName { get; set; }

    /// <summary>
    /// Delegate used to report errors back to the UI.
    /// </summary>
    public Action<string> ReportErrorDelegate { get; set; }

    /// <summary>
    /// Delegate used to report status back to the UI.
    /// </summary>
    public Action<string> ReportStatusDelegate { get; set; }

    /// <summary>
    /// Gets or sets the command assigned to the Windows service.
    /// </summary>
    public string ServiceCommand { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating if the existing instance serve was renamed.
    /// </summary>
    public bool ServiceRenamed { get; set; }

    #endregion

    /// <summary>
    /// Reports the specified message to the assigned delegate.
    /// </summary>
    /// <param name="message">The message to report.</param>
    protected void ReportStatus(string message)
    {
      if (string.IsNullOrEmpty(message))
      {
        return;
      }

      ReportStatusDelegate(message);
    }

    /// <summary>
    /// Reports the specified error message to the assigned delegate.
    /// </summary>
    /// <param name="message">The error message to report.</param>
    protected void ReportError(string message)
    {
      if (string.IsNullOrEmpty(message))
      {
        return;
      }

      ReportErrorDelegate(message);
    }

    /// <summary>
    /// Resets the controller.
    /// </summary>
    public void Reset()
    {
      _revertedSteps = new List<string>();
      AuthenticationPluginUpdated = false;
      DataDirRenamed = false;
      ExistingServerInstallationInstance = null;
      IniFileUpdated = false;
      InstanceStopped = false;
      NewDataDirPath = null;
      NewServiceName = null;
      OldDataDirPath = null;
      OldIniFileName = null;
      OldServiceName = null;
      ServiceCommand = null;
      ServiceRenamed = false;
    }

    /// <summary>
    /// Reverts the change of the authentication plugin for the root user.
    /// </summary>
    /// <param name="settings">The controller settings.</param>
    public void RevertAuthenticationPluginChange(MySqlServerSettings settings)
    {
      ReportStatus(Resources.ServerConfigRevertingAuthenticationPluginChanged);
      var rootUser = MySqlServerUser.GetLocalRootUser(ExistingServerInstallationInstance.ConfigurationRootPassword,
        settings.DefaultAuthenticationPlugin);
      var serverInstance = new MySqlServerInstance(settings.Port, rootUser, ReportStatus);
      try
      {
        serverInstance.UpdateUserAuthenticationPlugin(MySqlServerUser.ROOT_USERNAME,
          rootUser.Password,
          OldRootUserAuthenticationPlugin);
        ReportStatus(Resources.ServerConfigRevertedAuthenticationPluginChange);
        AuthenticationPluginUpdated = false;
      }
      catch (Exception ex)
      {
        ReportError(string.Format(Resources.ServerConfigFailedToRevertAuthenticationPluginChange, ex.Message));
      }
    }

    /// <summary>
    /// Reverts the data directory renaming.
    /// </summary>
    public void RevertDataDirRename(MySqlServerSettings settings)
    {
      ReportStatus(Resources.ServerConfigRevertingDataDirRename);
      try
      {
        Directory.Move(NewDataDirPath, OldDataDirPath);
        ReportStatus(string.Format(Resources.ServerConfigDataDirectoryRenamed, NewDataDirPath, OldDataDirPath));
        settings.DataDirectory = OldDataDirPath;
        settings.IniDirectory = settings.DataDirectory;
        settings.SecureFilePrivFolder = Path.Combine(settings.IniDirectory, MySqlServerSettings.SECURE_FILE_PRIV_DIRECTORY);
        _revertedSteps.Add(Resources.RenameExistingDataDirectoryStep);
        DataDirRenamed = false;
      }
      catch (Exception ex)
      {
        ReportError(string.Format(Resources.ServerConfigRevertDataDirRenameError, ex.Message));
      }

      ReportStatus(Resources.ServerConfigRevertedDataDirRename);
    }

    /// <summary>
    /// Reverts the settings files.
    /// </summary>
    public void RevertSettingsFiles(MySqlServerSettings settings)
    {
      ReportStatus(Resources.ServerConfigSettingsFilesReverting);
      try
      {
        // Revert ini file.
        ReportStatus(Resources.ServerConfigRevertingIniFile);
        File.Copy(OldIniFileName, settings.FullConfigFilePath, true);

        // Remove settings file.
        ReportStatus(Resources.ServerConfigRemovingSettingFile);
        GeneralSettingsManager.DeleteGeneralSettingsFile(settings.InstallDirectory);
        
        ReportStatus(Resources.ServerConfigSettingsFilesReverted);
        _revertedSteps.Add(Resources.ServerWriteConfigFileStep);

        IniFileUpdated = false;
      }
      catch (Exception ex)
      {
        ReportError(string.Format(Resources.ServerConfigSettingsFilesReverFailed, ex.Message));
      }
    }

    /// <summary>
    /// Reverts the service renaming.
    /// </summary>
    public void RevertServiceRename(MySqlServerSettings settings)
    {
      ReportStatus(Resources.ServerConfigServiceRenameReverting);
      try
      {
        ReportStatus(string.Format(Resources.ServerConfigUpdatingExistingServiceWithNewName, NewServiceName, OldServiceName));
        MySqlServiceControlManager.Delete(NewServiceName);
        MySqlServiceControlManager.Add(OldServiceName,
          OldServiceName,
          ServiceCommand,
          settings.ServiceAccountUsername,
          settings.ServiceAccountPassword,
          settings.ServiceStartAtStartup);
        settings.ServiceName = OldServiceName;
        ReportStatus(string.Format(Resources.ServerConfigServiceRenameReverted, OldServiceName));
        _revertedSteps.Add(Resources.ServerAdjustServiceStep);
        ServiceRenamed = false;
      }
      catch (Exception ex)
      {
        ReportError(string.Format(Resources.ServerConfigServiceRenameRevertFailed, ex.Message));
      }
    }

    /// <summary>
    /// Rolls back the required steps based on the value of the properties.
    /// </summary>
    /// <param name="settings">The settings to revert.</param>
    public List<string> Rollback(MySqlServerSettings settings)
    {
      ReportStatus(Resources.ServerConfigRollbackStarted);
      if (ServiceRenamed)
      {
        RevertServiceRename(settings);
      }

      if (IniFileUpdated)
      {
        RevertSettingsFiles(settings);
      }

      if (DataDirRenamed)
      {
        RevertDataDirRename(settings);
      }

      if (InstanceStopped
          || (ExistingServerInstallationInstance != null
              && !ExistingServerInstallationInstance.IsRunning))
      {
        StartExistingInstance();
      }

      if (AuthenticationPluginUpdated)
      {
        RevertAuthenticationPluginChange(settings);
      }

      var revertedSteps = _revertedSteps;
      Reset();
      ReportStatus(Resources.ServerConfigRollbackFinished);
      return revertedSteps;
    }

    /// <summary>
    /// Starts the existing server instance.
    /// </summary>
    public void StartExistingInstance()
    {
      ReportStatus(Resources.ServerConfigStartingExistingInstance);
      ExistingServerInstallationInstance.StartInstance();
      ReportStatus(Resources.ServerConfigExistingInstanceStarted);
      _revertedSteps.Add(Resources.StoppingExistingServerInstanceStep);
    }
  }
}

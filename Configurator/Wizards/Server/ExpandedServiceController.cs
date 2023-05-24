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
using System.ComponentModel;
using System.Security;
using System.Security.Permissions;
using System.ServiceProcess;
using System.Threading;
using Microsoft.Win32;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.Logging;

namespace MySql.Configurator.Wizards.Server
{
  //// Wrapper for the ServiceController object that adds ability to Add and remove services and use service BinaryPath 
  public class ExpandedServiceController : ServiceController
  {
    #region Private

    //// Parameters
    private string _binaryPath;

    //// Methods
    private static Win32Exception CreateSafeWin32Exception()
    {
      Win32Exception exception;
      new SecurityPermission(PermissionState.Unrestricted).Assert();
      try
      {
        exception = new Win32Exception();
      }
      finally
      {
        CodeAccessPermission.RevertAssert();
      }

      return exception;
    }

    private static IntPtr GetDataBaseHandleWithAllAccess(string machineName)
    {
      IntPtr zero;
      if (machineName.Equals(".") || (machineName.Length == 0))
      {
        zero = Win32.OpenSCManager(null, null, (Win32.SC_MANAGER_ALL | Win32.ACCESS_TYPE_DELETE));
      }
      else
      {
        zero = Win32.OpenSCManager(machineName, null, (Win32.SC_MANAGER_ALL | Win32.ACCESS_TYPE_DELETE));
      }

      if (zero == IntPtr.Zero)
      {
        Exception innerException = CreateSafeWin32Exception();
        throw new InvalidOperationException("Failed to open connection to service database.", innerException);
      }

      return zero;
    }

    private void GetBinaryPath()
    {
      var serviceEntry = Registry.LocalMachine;
      try
      {
        serviceEntry = serviceEntry.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\" + ServiceName, false);
        if (serviceEntry != null)
        {
          _binaryPath = serviceEntry.GetValue("ImagePath", "").ToString();
        }
      }
      catch (Exception e)
      {
        Logger.LogException(e);
        _binaryPath = string.Empty;
      }
      finally
      {
        serviceEntry?.Close();
      }
    }

    #endregion

    #region Public

    public const int DEFAULT_POLLING_DELAY_MILLISECONDS = 1000;

    //// WaitingForStatus polling delay to avoid pounding the CPU while waiting for status. 500 ms is the default value.
    public int PollingDelay { get; set; }

    //// Wrapper for existing constructors
    public ExpandedServiceController(string name)
      : base(name)
    {
      PollingDelay = DEFAULT_POLLING_DELAY_MILLISECONDS;
      GetBinaryPath();
    }

    //// New constructors used to create new services and cope an existing one.
    public ExpandedServiceController(string name, string displayName, string fileName, string username, string password, bool startAtStartUp)
    {
      PollingDelay = DEFAULT_POLLING_DELAY_MILLISECONDS;
      //// Create the new Service.
      IntPtr scm = GetDataBaseHandleWithAllAccess(MachineName);
      int startType = startAtStartUp ? Win32.START_TYPE_AUTO : Win32.START_TYPE_DEMAND;
      if (username != null)
      {
        username = username.Contains(@"\") ? username : @".\" + username;
      }

      try
      {
        IntPtr service = Win32.OpenService(scm, name, Win32.ACCESS_TYPE_ALL);
        if (service == IntPtr.Zero)
        {
          try
          {
            service = Win32.CreateService(scm, name, displayName, Win32.ACCESS_TYPE_ALL, Win32.SERVICE_TYPE_WIN32_OWN_PROCESS, startType, Win32.ERROR_CONTROL_NORMAL, fileName, null, IntPtr.Zero, null, username, password);
          }
          finally
          {
            Win32.CloseServiceHandle(service);
          }
        }

        if (service == IntPtr.Zero)
        {
          throw CreateSafeWin32Exception();
        }
      }
      finally
      {
        Win32.CloseServiceHandle(scm);
      }

      ServiceName = name;
      DisplayName = displayName;
      GetBinaryPath();
    }

    public ExpandedServiceController(ServiceController defaultBase)
      : base(defaultBase.ServiceName, defaultBase.MachineName)
    {
      PollingDelay = DEFAULT_POLLING_DELAY_MILLISECONDS;
      GetBinaryPath();
    }

    public void Update(string newname, string cmdline, string username, string password, bool startup)
    {
      IntPtr scm = GetDataBaseHandleWithAllAccess(MachineName);
      int startType = startup ? Win32.START_TYPE_AUTO : Win32.START_TYPE_DEMAND;
      if (username != null)
      {
        username = username.Contains(@"\") ? username : @".\" + username;
      }

      try
      {
        IntPtr service = Win32.OpenService(scm, ServiceName, Win32.ACCESS_TYPE_ALL);
        if (service == IntPtr.Zero)
        {
          return;
        }

        try
        {
          bool b = Win32.ChangeServiceConfig(service, Win32.SERVICE_TYPE_WIN32_OWN_PROCESS, (uint)startType, Win32.ERROR_CONTROL_NORMAL, cmdline, null, IntPtr.Zero, null, username, password, newname);
          if (!b)
          {
            throw CreateSafeWin32Exception();
          }
        }
        finally
        {
          Win32.CloseServiceHandle(service);
        }
      }
      finally
      {
        Win32.CloseServiceHandle(scm);
      }
    }

    public void Remove()
    {
      string serviceName = ServiceName;
      string machineName = MachineName;
      if (Status == ServiceControllerStatus.Running)
      {
        Stop();
      }

      WaitForStatus(ServiceControllerStatus.Stopped);
      Close();
      IntPtr scm = GetDataBaseHandleWithAllAccess(machineName);

      try
      {
        IntPtr service = Win32.OpenService(scm, serviceName, Win32.ACCESS_TYPE_ALL | Win32.ACCESS_TYPE_DELETE);
        if (service == IntPtr.Zero)
        {
          throw CreateSafeWin32Exception();
        }

        try
        {
          if (!Win32.DeleteService(service))
          {
            throw CreateSafeWin32Exception();
          }
        }
        finally
        {
          Win32.CloseServiceHandle(service);
        }
      }
      finally
      {
        Win32.CloseServiceHandle(scm);
      }
    }

    public string BinaryPath
    {
      get
      {
        return _binaryPath;
      }

      set
      {
        _binaryPath = value;
        RegistryKey serviceEntry = Registry.LocalMachine;
        try
        {
          serviceEntry = serviceEntry.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\" + ServiceName, true);
          serviceEntry?.SetValue("ImagePath", _binaryPath);
        }
        catch (Exception e)
        {
          Logger.LogException(e);
        }
        finally
        {
          serviceEntry?.Close();
        }
      }
    }

    public void WaitForStatus(ServiceControllerStatus statusToWaitFor, CancellationToken cancellationToken)
    {
      Logger.LogVerbose($"{DateTime.Now} - Waiting for service status change to {statusToWaitFor}.");
      while (!cancellationToken.IsCancellationRequested)
      {
        Refresh();
        if (Status == statusToWaitFor)
        {
          return;
        }

        //// Delay added to avoid pounding CPU while waiting for status.
        Thread.Sleep(PollingDelay);
      }

      //// Cancel Requested
      throw new OperationCanceledException(cancellationToken);
    }

    #endregion
  }
}
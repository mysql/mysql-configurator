// Copyright (c) 2023, Oracle and/or its affiliates.
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License as
// published by the Free Software Foundation; version 2 of the
// License.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
// 02110-1301  USA

using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Text.RegularExpressions;
using MySql.Configurator.Core.Classes.MySql;

namespace MySql.Configurator.Core.Classes
{
  public class Service
  {
    /// <summary>
    /// Gets Windows services with a path name that match an optional filter, or all.
    /// </summary>
    /// <param name="pathNameRegexFilter">An optional regex filter for the path name value.</param>
    /// <returns>A list of <see cref="ManagementObject"/> instances representing Windows services.</returns>
    public static List<ManagementObject> GetInstances(string pathNameRegexFilter)
    {
      var list = new List<ManagementObject>();
      Regex regex = null;
      if (!string.IsNullOrEmpty(pathNameRegexFilter))
      {
        regex = new Regex(pathNameRegexFilter);
      }

      var mc = new ManagementClass("Win32_Service");
      var instances = mc.GetInstances().Cast<ManagementObject>().ToList();
      foreach (var instance in instances)
      {
        if (regex == null)
        {
          list.Add(instance);
        }
        else
        {
          var path = instance.GetPropertyValue("PathName");
          if (path == null)
          {
            continue;
          }

          if (regex.Match(path.ToString()).Success)
          {
            list.Add(instance);
          }
        }
      }

      return list;
    }

    /// <summary>
    /// Checks if a Windows service with the given name exists.
    /// </summary>
    /// <param name="serviceName">A Windows service name.</param>
    /// <returns><c>true</c> if a Windows service with the given name exists, <c>false</c> otherwise.</returns>
    public static bool ExistsServiceInstance(string serviceName)
    {
      if (string.IsNullOrWhiteSpace(serviceName))
      {
        return false;
      }

      var services = ServiceController.GetServices();
      return services.Any(s => string.Equals(s.ServiceName, serviceName, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Gets the image path value used by a Windows service with the given name.
    /// </summary>
    /// <param name="serviceName">A Windows service name.</param>
    /// <returns>The image path value used by a Windows service.</returns>
    public static string GetServiceImagePath(string serviceName)
    {
      string imagePath;
      using (var key = RegistryHive.LocalMachine.OpenRegistryKey($@"SYSTEM\CurrentControlSet\Services\{serviceName}"))
      {
        if (key == null)
        {
          return null;
        }

        imagePath = key.GetValue("ImagePath", null).ToString();
      }

      return imagePath;
    }

    /// <summary>
    /// Determines whether the instance is a real MySQL service or not.
    /// </summary>
    /// <param name="serviceName">Name of the service.</param>
    /// <param name="onlyServer">Flag indicating if only MySQL Server executables are detected, or others as well, like MySQL router.</param>
    /// <returns><c>true</c> if the instance is a real MySQL service, <c>false</c> otherwise.</returns>
    public static bool IsRealMySqlService(string serviceName, bool onlyServer)
    {
      return IsRealMySqlService(serviceName, onlyServer, out _);
    }

    /// <summary>
    /// Determines whether the instance is a real MySQL service or not.
    /// </summary>
    /// <param name="serviceName">Name of the service.</param>
    /// <param name="onlyServer">Flag indicating if only MySQL Server executables are detected, or others as well, like MySQL router.</param>
    /// <param name="imagePath">ImagePath property of the registry key from the provided service name.</param>
    /// <returns><c>true</c> if the instance is a real MySQL service, <c>false otherwise.</c>.</returns>
    public static bool IsRealMySqlService(string serviceName, bool onlyServer, out string imagePath)
    {
      imagePath = GetServiceImagePath(serviceName);
      return MySqlServerInstance.IsMySqlServerExecutable(imagePath);
    }
  }
}
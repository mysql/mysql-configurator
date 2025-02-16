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
using Microsoft.Win32;
using System.Linq;
using System.ServiceProcess;

namespace MySql.Configurator.Base.Classes
{
  public class Service
  {
    public bool AcceptPause { get; set; }
    public bool AcceptStop { get; set; }
    public string Caption { get; set; }
    public string Description { get; set; }
    public string DisplayName { get; set; }
    public string Name { get; set; }
    public string PathName { get; set; }
    public int ProcessId { get; set; }
    public string ServiceType { get; set; }
    public bool Started { get; set; }
    public string StartMode { get; set; }
    public string StartName { get; set; }
    public string State { get; set; }
    public string Status { get; set; }

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
  }
}

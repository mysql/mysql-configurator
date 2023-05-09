// Copyright (c) 2013, 2021, Oracle and/or its affiliates.
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

using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Core.Classes.MySqlWorkbench;
using MySql.Configurator.Core.Classes.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;

namespace MySql.Configurator.Core.Classes.MySql
{
  /// <summary>
  /// Contains connection parameters extracted from a Windows service of a MySQL Server instance.
  /// </summary>
  public class MySqlServerStartupParameters : BaseStartupParameters
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlServerStartupParameters"/> class.
    /// </summary>
    protected MySqlServerStartupParameters()
    {
      IsForMySql = true;
      IsForMySqlServer = true;
      NamedPipesEnabled = false;
      PipeName = null;
      Port = 0;
    }

    #region Properties

    /// <summary>
    /// Gets a value indicating if names pipes are enabled for the MySQL Server connection.
    /// </summary>
    public bool NamedPipesEnabled { get; private set; }

    /// <summary>
    /// Gets the name of the pipe used by the connection.
    /// </summary>
    public string PipeName { get; private set; }

    /// <summary>
    /// Gets the connection port of the MySQL Server instance.
    /// </summary>
    public uint Port { get; private set; }

    #endregion Properties

    /// <summary>
    /// Gets the connection properties for a existing MySQL Servers instance installed as a Windows service.
    /// </summary>
    /// <param name="serviceName">The name that identifies the service to the system. This can also be the display name for the service.</param>
    /// <param name="computerName">The computer on which the service resides. The default value specifies the local computer.</param>
    /// <returns>A <see cref="MySqlServerStartupParameters"/> object with the connection properties.</returns>
    public static MySqlServerStartupParameters GetStartupParameters(string serviceName, string computerName = ".")
    {
      if (!Service.ExistsServiceInstance(serviceName))
      {
        return null;
      }

      var serviceController = new ServiceController(serviceName, computerName);
      return GetStartupParameters(serviceController);
    }

    /// <summary>
    /// Gets the connection properties for a existing MySQL Servers instance installed as a Windows service.
    /// </summary>
    /// <param name="winService">Windows service for a MySQL Server instance.</param>
    /// <returns>A <see cref="MySqlServerStartupParameters"/> object with the connection properties.</returns>
    public static MySqlServerStartupParameters GetStartupParameters(ServiceController winService)
    {
      try
      {
        if (winService == null
          || !Service.ExistsServiceInstance(winService.ServiceName)
          || !Service.IsRealMySqlService(winService.ServiceName, true, out var imagePath))
        {
          return null;
        }

        var parameters = new MySqlServerStartupParameters
        {
          HostName = winService.MachineName == "."
            ? MySqlWorkbenchConnection.DEFAULT_HOSTNAME
            : winService.MachineName
        };

        // Get our host information
        if (!parameters.IsForMySqlServer)
        {
          return parameters;
        }

        parameters.PipeName = "mysql";

        // Parse the command line arguments
        uint port = 0;
        var options = new CommandLineOptions
      {
        new CommandLineOption("defaults-file", arg => parameters.ConfigurationFilePath = arg),
        new CommandLineOption("port|P", arg => uint.TryParse(arg, out port)),
        new CommandLineOption("enable-named-pipe", arg => parameters.NamedPipesEnabled = true),
        new CommandLineOption("socket", arg => parameters.PipeName = arg)
      };
        options.Parse(imagePath, 1);
        parameters.Port = port;
        if (string.IsNullOrEmpty(parameters.ConfigurationFilePath))
        {
          return parameters;
        }

        // We have a valid defaults file
        var iniFile = new Configuration.IniFile(parameters.ConfigurationFilePath);
        iniFile.LoadSections();
        var mysqldSection = iniFile.Sections.FirstOrDefault(section => section.Name.Equals("mysqld", StringComparison.OrdinalIgnoreCase));
        uint.TryParse(mysqldSection?.Keys.FirstOrDefault(kvp => kvp.Key.Equals("port", StringComparison.OrdinalIgnoreCase)).Value, out port);
        parameters.Port = port;
        parameters.PipeName = mysqldSection?.Keys.FirstOrDefault(kvp => kvp.Key.Equals("socket", StringComparison.OrdinalIgnoreCase)).Value;
        if (mysqldSection != null)
        {
          parameters.NamedPipesEnabled = parameters.NamedPipesEnabled
                                       || mysqldSection.Keys.ContainsKey("enable-named-pipe");
        }

        return parameters;
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
        return null;
      }
    }

    /// <summary>
    /// Gets the related Workbench connections that connect to the MySQL service related to this startup parameters instance.
    /// </summary>
    public override List<MySqlWorkbenchConnection> GetRelatedWorkbenchConnections()
    {
      if (string.IsNullOrEmpty(HostName)
          || !IsForMySqlServer)
      {
        return null;
      }

      var workbenchConnections = new List<MySqlWorkbenchConnection>();
      var filteredConnections = MySqlWorkbench.MySqlWorkbench.WorkbenchConnections.Where(connection => !string.IsNullOrEmpty(connection.Name) && connection.Port == Port).ToList();
      foreach (var connection in filteredConnections)
      {
        switch (connection.ConnectionMethod)
        {
          case MySqlWorkbenchConnection.ConnectionMethodType.LocalUnixSocketOrWindowsPipe:
            if (!NamedPipesEnabled
                || string.Equals(connection.UnixSocketOrWindowsPipe, PipeName, StringComparison.OrdinalIgnoreCase))
            {
              continue;
            }
            break;

          case MySqlWorkbenchConnection.ConnectionMethodType.Tcp:
          case MySqlWorkbenchConnection.ConnectionMethodType.XProtocol:
            if (connection.Port != Port)
            {
              continue;
            }
            break;

          case MySqlWorkbenchConnection.ConnectionMethodType.Unknown:
            continue;
        }

        if (!Utilities.IsValidIpAddress(connection.Host))
        {
          if (Utilities.GetIPv4ForHostName(connection.Host) != HostIPv4)
          {
            continue;
          }
        }
        else
        {
          if (connection.Host != HostIPv4)
          {
            continue;
          }
        }

        workbenchConnections.Add(connection);
      }

      return workbenchConnections;
    }
  }
}
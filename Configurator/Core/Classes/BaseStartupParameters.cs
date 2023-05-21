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

using MySql.Configurator.Core.Classes.MySqlWorkbench;
using System.Collections.Generic;

namespace MySql.Configurator.Core.Classes
{
  public class BaseStartupParameters
  {
    #region Fields

    /// <summary>
    /// Gets the connection IP address.
    /// </summary>
    private string _hostIPv4;

    /// <summary>
    /// The connection host name.
    /// </summary>
    private string _hostName;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseStartupParameters"/> class.
    /// </summary>
    public BaseStartupParameters()
    {
      _hostIPv4 = null;
      _hostName = null;
      ConfigurationFilePath = null;
      IsForMySql = false;
      IsForMySqlServer = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseStartupParameters"/> class.
    /// </summary>
    /// <param name="hostName">The connection host name.</param>
    public BaseStartupParameters(string hostName) : this()
    {
      HostName = hostName;
    }

    #region Properties

    /// <summary>
    /// Gets the file path to the configuration file containing initialization parameters and values.
    /// </summary>
    public string ConfigurationFilePath { get; protected set; }

    /// <summary>
    /// Gets the connection IP address.
    /// </summary>
    public string HostIPv4
    {
      get
      {
        if (string.IsNullOrEmpty(_hostIPv4))
        {
          _hostIPv4 = Utilities.GetIPv4ForHostName(_hostName);
        }

        return _hostIPv4;
      }
    }

    /// <summary>
    /// Gets the connection host name.
    /// </summary>
    public string HostName
    {
      get => _hostName;
      set
      {
        if (!string.Equals(_hostName, value))
        {
          _hostIPv4 = null;
        }

        _hostName = value;
      }
    }

    /// <summary>
    /// Gets a value indicating if this instance is bound to a MySQL Server product.
    /// </summary>
    public bool IsForMySqlServer { get; protected set; }

    /// <summary>
    /// Gets a value indicating if this instance is bound to any MySQL product.
    /// </summary>
    public bool IsForMySql { get; protected set; }

    #endregion Properties

    /// <summary>
    /// Gets the related Workbench connections that connect to the MySQL service related to this startup parameters instance.
    /// </summary>
    public virtual List<MySqlWorkbenchConnection> GetRelatedWorkbenchConnections()
    {
      return null;
    }

  }
}

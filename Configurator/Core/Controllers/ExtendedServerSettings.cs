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
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Core.Enums;

namespace MySql.Configurator.Core.Controllers
{
  /// <summary>
  /// Represents Server configuration values not stored in the Server's configuration file.
  /// </summary>
  [Serializable]
  public class ExtendedServerSettings
  {
    #region Properties

    /// <summary>
    /// Gets or sets a value indicating whether the Server is configured as a Service.
    /// </summary>
    public bool ConfigureAsService { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the Enterprise Firewall is enabled.
    /// </summary>
    public bool EnterpriseFirewallEnabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether an upgrade to system tables is pending to be performed.
    /// </summary>
    public bool PendingSystemTablesUpgrade { get; set; }

    /// <summary>
    /// Gets or sets the server installation type selected during a Server's configuration
    /// </summary>
    public ServerInstallationType ServerInstallationType { get; set; }

    #endregion Properties

    /// <summary>
    /// Deserializes the <see cref="ExtendedServerSettings"/> class.
    /// </summary>
    /// <param name="filePath">The location where the serialized file is located.</param>
    public static ExtendedServerSettings Deserialize(string filePath)
    {
      ExtendedServerSettings extendedSettings = null;
      try
      {
        var serializer = new XmlSerializer(typeof(ExtendedServerSettings));
        using (var stream = new FileStream(filePath, FileMode.Open))
        {
          extendedSettings = (ExtendedServerSettings)serializer.Deserialize(stream);
        }
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
      }

      return extendedSettings;
    }

    /// <summary>
    /// Serializes the <see cref="ExtendedServerSettings"/> class.
    /// </summary>
    /// <param name="filePath">The location where the serialized file will be output.</param>
    /// <returns><c>true</c> if the serialization was done successfully, <c>false</c> otherwise.</returns>
    public bool Serialize(string filePath)
    {
      bool success = true;
      try
      {
        var serializer = new XmlSerializer(typeof(ExtendedServerSettings));
        using (var myWriter = new StreamWriter(filePath, false))
        {
          serializer.Serialize(myWriter, this);
          myWriter.Close();
        }
      }
      catch (Exception ex)
      {
        success = false;
        Logger.LogException(ex);
      }

      return success;
    }
  }
}

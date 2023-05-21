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
    /// <summary>
    /// Initializes a new instance of the <see cref="ExtendedServerSettings"/> class.
    /// </summary>
    public ExtendedServerSettings()
    {
      SystemTablesUpgraded = SystemTablesUpgradedType.None;
    }

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
    /// Gets or sets the <see cref="ServerConfigurationType"/> used during a Server's configuration.
    /// </summary>
    [InnoDbCluster]
    public ServerConfigurationType InnoDbClusterType { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether an upgrade to system tables is pending to be performed.
    /// </summary>
    public bool PendingSystemTablesUpgrade { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="ServerConfigurationType"/> used during a Server's configuration.
    /// </summary>
    public ServerConfigurationType ServerConfigurationType { get; set; }

    /// <summary>
    /// Gets the server version that is currently installed.
    /// </summary>
    public String ServerVersion { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if system tables were upgrades during the last server upgrade.
    /// </summary>
    public SystemTablesUpgradedType SystemTablesUpgraded { get; set; }

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
    /// <param name="useNonInnoDbClusterSerializer">Flag to indicate if the serializer that ignores InnoDB setitngs is used.</param>
    /// <returns><c>true</c> if the serialization was done successfully, <c>false</c> otherwise.</returns>
    public bool Serialize(string filePath, bool useNonInnoDbClusterSerializer)
    {
      bool success = true;
      try
      {
        var serializer = useNonInnoDbClusterSerializer
          ? GetNonInnoDbClusterSerializer()
          : new XmlSerializer(typeof(ExtendedServerSettings));
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

    /// <summary>
    /// Gets a <see cref="XmlSerializer"/> that ignores properties flagged with the <see cref="InnoDbClusterAttribute"/>.
    /// </summary>
    /// <returns>A <see cref="XmlSerializer"/> that ignores properties flagged with the <see cref="InnoDbClusterAttribute"/>.</returns>
    private XmlSerializer GetNonInnoDbClusterSerializer()
    {
      var xOver = new XmlAttributeOverrides();
      var propertyInfos = typeof(ExtendedServerSettings).GetProperties();
      foreach (var propertyInfo in propertyInfos)
      {
        var customAttributes = propertyInfo.GetCustomAttributes(false);
        if (!customAttributes.Any(a => a is InnoDbClusterAttribute))
        {
          continue;
        }

        var attrs = new XmlAttributes { XmlIgnore = true };
        xOver.Add(typeof(ExtendedServerSettings), propertyInfo.Name, attrs);
      }

      return new XmlSerializer(typeof(ExtendedServerSettings), xOver);
    }
  }
}

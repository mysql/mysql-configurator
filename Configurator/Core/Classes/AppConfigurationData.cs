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

using MySql.Configurator.Core.Classes.Logging;
using System;
using System.IO;
using System.Xml.Serialization;

namespace MySql.Configurator.Core.Classes
{
  [XmlRoot("Configuration")]
  public class AppConfigurationData
  {
    public AppConfigurationData()
    {
    }

    public static AppConfigurationData LoadFromFile(string file)
    {
      try
      {
        var serializer = new XmlSerializer(typeof(AppConfigurationData));
        TextReader reader = new StreamReader(file);
        var configurationData = (AppConfigurationData)serializer.Deserialize(reader);
        reader.Close();
        return configurationData;
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
        return null;
      }
    }
  }
}
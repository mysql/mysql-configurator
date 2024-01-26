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

using System.Xml;

namespace MySql.Configurator.Core.Classes.MySqlWorkbench
{
  public class MySqlWorkbenchServer
  {
    #region Fields

    private MySqlWorkbenchConnection _relatedConnection;

    #endregion Fields

    internal MySqlWorkbenchServer(XmlElement el)
    {
      Id = el.Attributes["id"].Value;
      foreach (XmlElement childEl in el.ChildNodes)
      {
        ProcessElement(childEl);
      }
    }

    #region Properties

    public string ConfigPath { get; private set; }

    public string ConfigSection { get; private set; }

    public MySqlWorkbenchConnection Connection
    {
      get
      {
        if (_relatedConnection == null)
        {
          GetRelatedConnection();
        }

        return _relatedConnection;
      }
    }

    public string ConnectionId { get; private set; }

    public string Id { get; private set; }

    public string Name { get; private set; }

    public string ServiceName { get; private set; }

    public string System { get; private set; }

    #endregion Properties

    private void GetRelatedConnection()
    {
      _relatedConnection = MySqlWorkbench.Connections.GetConnectionForId(ConnectionId);
    }

    private void ProcessElement(XmlElement el)
    {
      string type = el.Attributes["type"].Value;
      if (type == "dict")
      {
        foreach (XmlElement childEl in el.ChildNodes)
        {
          ProcessElement(childEl);
        }
          
        return;
      }

      string key = el.Attributes["key"].Value;
      switch (key)
      {
        case "sys.config.path":
          ConfigPath = el.InnerText;
          break;

        case "sys.config.section":
          ConfigSection = el.InnerText;
          break;

        case "sys.mysqld.service_name":
          ServiceName = el.InnerText;
          break;

        case "sys.system":
          System = el.InnerText;
          break;

        case "name":
          Name = el.InnerText;
          break;

        case "connection":
          ConnectionId = el.InnerText; GetRelatedConnection();
          break;
      }
    }
  }
}

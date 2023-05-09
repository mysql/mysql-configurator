/* Copyright (c) 2014, 2023, Oracle and/or its affiliates.

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

namespace MySql.Configurator.Wizards.Server
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
  }
}

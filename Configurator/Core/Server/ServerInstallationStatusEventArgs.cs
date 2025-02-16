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

using MySql.Configurator.Base.Enums;
using MySql.Configurator.Properties;

namespace MySql.Configurator.Core.Server
{
  public class ServerInstallationStatusEventArgs
  {
    public ServerInstallationStatusEventArgs(ServerInstallation serverInstallation, ServerInstallationAction action, ServerInstallationStatus status)
    {
      Cancel = false;
      Message = null;
      IsVerbose = false;
      ServerInstallation = serverInstallation;
      Action = action;
      Status = status;
    }

    public ServerInstallationStatusEventArgs(ServerInstallation serverInstallation, ServerInstallationAction action, ServerInstallationStatus status, string message, bool isVerbose)
    : this (serverInstallation, action, status)
    {
      Message = message;
      IsVerbose = isVerbose;
    }

    #region Properties

    public ServerInstallation ServerInstallation { get; }
    public ServerInstallationAction Action { get; }
    public ServerInstallationStatus Status { get; }
    public string Message { get; }
    public int Progress { get; set; }
    public bool IsVerbose { get; }
    public bool Cancel { get; set; }

    #endregion Properties

    public override string ToString()
    {
      switch (Status)
      {
        case ServerInstallationStatus.Canceled:
          return string.Format(Resources.ActionCancelledText, ServerInstallation.NameWithVersion, Action);

        case ServerInstallationStatus.Complete:
          return string.Format(Resources.ActionSucceededText, ServerInstallation.NameWithVersion, Action);

        case ServerInstallationStatus.Failed:
          return string.Format(Resources.ActionFailedText, ServerInstallation.NameWithVersion, Action);
      }

      string msg = "[" + Action + "]  ";
      msg += ServerInstallationDisplay(ServerInstallation, true);
      return msg;
    }

    private string ServerInstallationDisplay(ServerInstallation serverInstallation, bool includeName)
    {
      var display = includeName
        ? serverInstallation.NameWithVersion
        : serverInstallation.VersionString;
      display += $" {ServerInstallation.ARCHITECTURE}";
      return display;
    }
  }
}

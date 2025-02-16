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

namespace MySql.Configurator.Core.Server
{
  public class ServerInstallationOperation
  {
    public ServerInstallationOperation(ServerInstallation serverInstallation)
    {
      ServerInstallation = serverInstallation;
    }

    public ServerInstallationOperation(ServerInstallation serverInstallation, ServerInstallationAction action, bool enabled, ServerInstallationStatus status, bool twoStepAction)
    : this(serverInstallation)
    {
      Action = action;
      Enabled = enabled;
      Status = status;
      TwoStepsAction = twoStepAction;
    }

    #region Properties

    public ServerInstallation ServerInstallation { get; }
    public ServerInstallationAction Action { get; set; }
    public bool RebootRequired { get; set; }
    public bool Enabled { get; set; }
    public bool TwoStepsAction { get; set; }
    public ServerInstallationStatus Status { get; set; }

    #endregion Properties

    public string GetStatus(bool current)
    {
      if (!current)
      {
        return Enabled ? $"Ready to {Action}" : string.Empty;
      }

      switch (Action)
      {
        case ServerInstallationAction.Install:
          return "Installing";

        case ServerInstallationAction.Modify:
          return "Modifying";

        case ServerInstallationAction.Remove:
          return "Removing";

        case ServerInstallationAction.Upgrade:
          return "Upgrading";

        case ServerInstallationAction.Configure:
          return "Configuring";
      }

      return null;
    }
  }
}

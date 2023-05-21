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

using MySql.Configurator.Core.Enums;
using MySql.Configurator.Core.Interfaces;

namespace MySql.Configurator.Core.Package
{
  public class PackageOperation
  {
    public PackageOperation(IPackage package)
    {
      Package = package;
    }

    public PackageOperation(IPackage package, PackageAction action, bool enabled, PackageStatus status, bool twoStepAction)
    : this(package)
    {
      Action = action;
      Enabled = enabled;
      Status = status;
      TwoStepsAction = twoStepAction;
    }

    #region Properties

    public IPackage Package { get; }
    public PackageAction Action { get; set; }
    public bool RebootRequired { get; set; }
    public bool Enabled { get; set; }
    public bool TwoStepsAction { get; set; }
    public PackageStatus Status { get; set; }

    #endregion Properties

    public string GetStatus(bool current)
    {
      if (!current)
      {
        return Enabled ? $"Ready to {Action}" : string.Empty;
      }

      switch (Action)
      {
        case PackageAction.Install:
          return "Installing";

        case PackageAction.Modify:
          return "Modifying";

        case PackageAction.Remove:
          return "Removing";

        case PackageAction.Upgrade:
          return "Upgrading";

        case PackageAction.Configure:
          return "Configuring";
      }

      return null;
    }
  }
}

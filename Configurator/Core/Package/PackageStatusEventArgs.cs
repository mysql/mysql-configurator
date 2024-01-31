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

using MySql.Configurator.Core.Enums;
using MySql.Configurator.Core.Interfaces;
using MySql.Configurator.Properties;

namespace MySql.Configurator.Core.Package
{
  public class PackageStatusEventArgs
  {
    public PackageStatusEventArgs(IPackage package, PackageAction action, PackageStatus status)
    {
      Cancel = false;
      Message = null;
      IsVerbose = false;
      Package = package;
      Action = action;
      Status = status;
    }

    public PackageStatusEventArgs(IPackage package, PackageAction action, PackageStatus status, string message, bool isVerbose)
    : this (package, action, status)
    {
      Message = message;
      IsVerbose = isVerbose;
    }

    #region Properties

    public IPackage Package { get; }
    public PackageAction Action { get; }
    public PackageStatus Status { get; }
    public string Message { get; }
    public int Progress { get; set; }
    public bool IsVerbose { get; }
    public bool Cancel { get; set; }

    #endregion Properties

    public override string ToString()
    {
      switch (Status)
      {
        case PackageStatus.Canceled:
          return string.Format(Resources.ActionCancelledText, Package.NameWithVersion, Action);

        case PackageStatus.Complete:
          return string.Format(Resources.ActionSucceededText, Package.NameWithVersion, Action);

        case PackageStatus.Failed:
          return string.Format(Resources.ActionFailedText, Package.NameWithVersion, Action);
      }

      string msg = "[" + Action + "]  ";
      if (Action == PackageAction.Upgrade)
      {
        msg += $"{Package.Product.Name} {PackageDisplay(Package.UpgradeTarget, false)} -> {PackageDisplay(Package, false)}";
      }
      else
      {
        msg += PackageDisplay(Package, true);
      }

      return msg;
    }

    private string PackageDisplay(IPackage package, bool includeName)
    {
      var display = includeName
        ? package.NameWithVersion
        : package.VersionString;
      if (package.Architecture != PackageArchitecture.Any
          && package.Architecture != PackageArchitecture.Unknown)
      {
        display += $" {package.Architecture}";
      }

      return display;
    }
  }
}

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

using System;
using System.Collections.Generic;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Core.Interfaces;
using MySql.Configurator.Core.Package;

namespace MySql.Configurator.Core.MSI
{
  public class PackageParameters
  {
    /// <summary> Low level UI handler event, if set, no other events are executed </summary>
    public InstallUIHandler UIHandler;

    private readonly List<string> _toAdd;
    private readonly List<string> _toRemove;

    public PackageParameters(IPackage package, PackageAction action)
    {
      _toAdd = new List<string>();
      _toRemove = new List<string>();
      Package = package;
      Action = action;
      var pkg = package as Package.Package;
      if (pkg != null)
      {
        pkg.LoadPropertiesForUninstalledPackage();
      }
      
      GetCommandLine();
    }

    public PackageParameters(string path, InstallAction action)
    {
      // TODO: Analyze this constructor, it is being used but it's doing nothing
      //UseProductCode = false;
      //Path = path;
      //switch (installAction)
      //{
      //  case InstallAction.Install:
      //    CommandLine = "ADDLOCAL=ALL";
      //    break;
      //  case InstallAction.Uninstall:
      //    CommandLine = "REMOVE=ALL";
      //    break;
      //  case InstallAction.Reinstall:
      //    CommandLine = "REINSTALL=ALL REINSTALLMODE=a";
      //    break;
      //  case InstallAction.Repair:
      //    CommandLine = "REINSTALL=ALL REINSTALLMODE=omus";
      //    break;
      //  //case InstallAction.Modify:
      //  //  CommandLine = GetFeatureList();
      //}
    }

    #region Properties

    public PackageAction Action { get; }

    /// <summary> command line, in form PROPERTY1=VALUE1 PROPERTY2=VALUE2 where PROPERTY are public Msi properties </summary>
    public string CommandLine { get; set; }

    public IPackage Package { get; }

    /// <summary> Path to msi package </summary>
    public string Path { get; set; }

    /// <summary> When true, the ChainedInstaller will use the MsiConfigureProductEx function else MsiInstallProduct</summary>
    public bool UseProductCode { get; set; }

    #endregion Properties

    private void GetCommandLine()
    {
      Path = Package.FullPath;
      UseProductCode = false;
      bool perMachine = Package.PerMachine;
      const string REBOOT_OPTION = "REBOOT=ReallySuppress";
      switch (Action)
      {
        case PackageAction.Install:
          CommandLine = $"{REBOOT_OPTION} {GetFeatureList()}{GetPathProperties()}";
          break;

        case PackageAction.Remove:
          CommandLine = $"REMOVE=ALL {REBOOT_OPTION}";
          Version.TryParse(Package.Version, out var serverVersion);
          CommandLine += " MYSQL_INSTALLER=\"YES\"";
          Path = Package.Id.ToString("B");
          UseProductCode = true;
          break;

        case PackageAction.Reinstall:
          CommandLine = $"REINSTALL=ALL REINSTALLMODE=a {REBOOT_OPTION}{GetPathProperties()}";
          perMachine = Package.UpgradeTarget.PerMachine;
          break;

        case PackageAction.Upgrade:
          CommandLine = $"{REBOOT_OPTION}{GetPathProperties()}";
          break;

        case PackageAction.Modify:
          Path = Package.Id.ToString("B");
          UseProductCode = true;
          CommandLine = $"{GetFeatureList()} {REBOOT_OPTION}";
          break;
      }

      if (perMachine
          && (Action == PackageAction.Install 
              || Action == PackageAction.Upgrade))
      {
        CommandLine += " ALLUSERS=1";
      }
      else
      {
        CommandLine += " ALLUSERS=\"\"";
      }
    }

    /// <summary>
    /// Get the a command line string with the list of features that should be installed or removed.
    /// </summary>
    /// <returns>A string containing a command line representation of the features to be installed or removed.</returns>
    private string GetFeatureList()
    {
      // Load features if no features have been loaded.
      if (Package.EnableFeaturesLoad
          && (Package.Features == null
              || Package.Features.Count == 0))
      {
        Package.Features = PackageFeature.GetFeaturesForProduct(Package.FullPath);
      }

      ProcessFeatureCollection(Package.Features);
      return $" {ProcessList("ADDLOCAL", _toAdd)} {ProcessList("REMOVE", _toRemove)}";
    }

    private string GetPathProperties()
    {
      return Package.Controller.GetMsiCommandLine();
    }

    private void ProcessFeatureCollection(List<PackageFeature> features)
    {
      if (features == null || features.Count == 0)
      {
        return;
      }

      foreach (PackageFeature f in features)
      {
        if (f.Installed && !f.ProposedInstalled)
        {
          _toRemove.Add(f.Name);
        }
        else if (!f.Installed && f.ProposedInstalled)
        {
          _toAdd.Add(f.Name);
        }

        ProcessFeatureCollection(f.Features);
      }
    }

    private string ProcessList(string key, List<string> list)
    {
      if (list == null)
      {
        return string.Empty;
      }

      if (Action == PackageAction.Install
          && !Package.EnableFeaturesLoad
          && key.Equals("ADDLOCAL", StringComparison.InvariantCulture)
          && list.Count == 0)
      {
        return " ADDLOCAL=ALL";
      }

      if (list.Count == 0)
      {
        return string.Empty;
      }

      if (list.Count == Package.FeatureCount)
      {
        return key + "=ALL";
      }

      string val = key + "=";
      string delimiter = "";
      foreach (string feature in list)
      {
        val += delimiter;
        val += feature;
        delimiter = ",";
      }

      return val;
    }
  }

}
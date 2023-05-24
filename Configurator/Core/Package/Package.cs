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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Core.Controllers;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Core.Interfaces;
using MySql.Configurator.Core.MSI;

namespace MySql.Configurator.Core.Package
{
  [Serializable]
  public class Package : IPackage, IComparable
  {
    #region Fields

    private string _key;
    #endregion Fields

    public Package()
    {
      Product = new Product.Product();
      Product.Name = "mysql-server";
      Product.Title = "MySQL Server";
      Features = new List<PackageFeature>();
    }

    #region Properties

    [XmlAttribute("arch")]
    public PackageArchitecture Architecture { get; set; }

    /// <summary>
    /// Gets or sets archive from manifest
    /// </summary>
    [XmlAttribute("archive")]
    public string Archive { get; set; }

    [XmlIgnore]
    public List<Package> AvailableUpgrades { get; private set; }

    [XmlIgnore]
    public string BaseDisplayName => DisplayName?.TrimEnd('0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.', ' ', '-');

    [XmlAttribute("title")]
    public string BaseTitle { get; set; }

    /// <summary>
    /// Gets or sets the SHA256 digest corresponding to the MSI.
    /// </summary>
    [XmlAttribute("checksum")]
    public string Checksum { get; set; }

    [XmlIgnore]
    public ProductConfigurationController Controller { get; private set; }

    /// <summary>
    /// Gets or sets the SHA256 digest corresponding to a commercial zip file.
    /// </summary>
    [XmlAttribute("digest")]
    public string Digest { get; set; }

    [XmlIgnore]
    public string DisplayName { get; set; }

    [XmlIgnore]
    public string DisplayVersion
    {
      get
      {
        string version = Version;
        if (Maturity != PackageMaturity.GA && Maturity != PackageMaturity.Unknown)
        {
          version += " " + Maturity;
        }

        return version;
      }
    }

    [XmlIgnore]
    public long EstimatedSelectedFeaturesSize => GetFeatureSize(Features, true);

    [XmlIgnore]
    public long EstimatedSize => GetFeatureSize(Features, false);

    /// <summary>
    /// Gets or sets a flag indicating if the features for this package should be loaded.
    /// </summary>
    [XmlIgnore]
    public bool EnableFeaturesLoad { get; set; }

    /// <summary>
    /// Gets total number of features.
    /// </summary>
    [XmlIgnore]
    public int FeatureCount
    {
      get
      {
        int count = 0;
        foreach (var packageFeature in Features)
        {
          count++; // TODO: Does the package itself count as feature?
          count += packageFeature.FeatureCount;
        }

        return count;
      }
    }

    /// <summary>
    /// Gets or sets the list of features assigned to this package.
    /// </summary>
    [XmlIgnore]
    public List<PackageFeature> Features { get; set; }

    [XmlAttribute("filename")]
    public string FileName { get; set; }

    /// <summary>
    /// Gets full path to the MSI
    /// </summary>
    [XmlIgnore]
    public string FullPath { get; set; }

    [XmlIgnore]
    public bool HasFeatureChanges => Features.Aggregate(false, (current, feature) => current || feature.HasChanges());

    [XmlAttribute("id")]
    public Guid Id { get; set; }

    [XmlIgnore]
    public bool InCache
    {
      get
      {
        // if is null == product not in the manifest or imposter
        if (string.IsNullOrEmpty(FileName))
        {
          return false;
        }

        string file = Path.Combine(FullPath, FileName);
        return File.Exists(file);
      }
    }

    [XmlIgnore]
    public DateTime InstallDate { get; private set; }

    [XmlAttribute("download")]
    public bool IsDownloadable { get; set; }

    [XmlIgnore]
    public bool IsInstalled { get; set; }

    [XmlIgnore]
    public string Key
    {
      get
      {
        if (_key == null)
        {
          CreateKey();
        }

        return _key;
      }
    }

    [XmlAttribute("license")]
    public LicenseType License { get; set; }

    [XmlIgnore]
    public bool MatchesCurrentArchitecture => Architecture != PackageArchitecture.X64 || Win32.Is64BitOs;

    [XmlAttribute("maturity")]
    public PackageMaturity Maturity { get; set; }

    [XmlIgnore]
    public string NameWithVersion => $"{Title.TrimEnd('0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.', ' ', '-')} {Version}";

    [XmlIgnore]
    public Version NormalizedVersion { get; set; }

    [XmlIgnore]
    public int OrderNumber
    {
      get
      {
        switch (Architecture)
        {
          case PackageArchitecture.Any:
            return 1;
          case PackageArchitecture.X64:
            return 2;
          case PackageArchitecture.X86:
            return 3;
          default:
            return 99;
        }
      }
    }

    /// <summary>
    /// Gets or sets number from manifest
    /// </summary>
    [XmlAttribute("number")]
    public string PatchNumber { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the install type for this package is per machine
    /// </summary>
    [XmlIgnore]
    public bool PerMachine { get; private set; }

    [XmlIgnore]
    public Product.Product Product { get; set; }

    [XmlIgnore]
    public DateTime Published
    {
      get
      {
        if (string.IsNullOrEmpty(PublishedDate))
        {
          return new DateTime();
        }

        bool result = DateTime.TryParseExact(PublishedDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date);
        return result ? date : new DateTime();
      }
    }

    [XmlAttribute("published")]
    public string PublishedDate { get; set; }

    [XmlIgnore]
    public string Publisher { get; set; }

    [XmlAttribute("regKeyTemplate")]
    public string RegistryKeyTemplate { get; set; }

    [XmlIgnore]
    public Version ServerMax { get; private set; }

    [XmlAttribute("serverMax")]
    public string ServerMaxText
    {
      get => ServerMax?.ToString() ?? string.Empty;
      set
      {
        if (!string.IsNullOrEmpty(value))
        {
          ServerMax = Utilities.NormalVersion(value);
        }
      }
    }

    [XmlIgnore]
    public Version ServerMin { get; private set; }

    [XmlAttribute("serverMin")]
    public string ServerMinText
    {
      get => ServerMin?.ToString() ?? string.Empty;
      set
      {
        if (!string.IsNullOrEmpty(value))
        {
          ServerMin = Utilities.NormalVersion(value);
        }
      }
    }

    /// <summary>
    /// Gets a temporary file name for downloading
    /// </summary>
    [XmlIgnore]
    public string TempFileName => $"{FullPath}.temp";

    [XmlIgnore]
    public string Title
    {
      get
      {
        string myTitle;

        if (!string.IsNullOrEmpty(BaseTitle))
        {
          myTitle = BaseTitle;
        }
        else
        {
          myTitle = Product.BaseTitle;
        }

        return myTitle.Replace("[version]", Version);
      }
    }

    [XmlAttribute("type")]
    public PackageType Type { get; set; }

    [XmlIgnore]
    public Guid UpgradeCode
    {
      get => string.IsNullOrEmpty(UpgradeCodeText)
          ? Guid.Empty
          : Guid.Parse(UpgradeCodeText);

      set => UpgradeCodeText = value == Guid.Empty
          ? string.Empty
          : value.ToString();
    }

    [XmlAttribute("upgradeCode")]
    public string UpgradeCodeText { get; set; }

    [XmlIgnore]
    public Package UpgradeTarget { get; set; }

    /// <summary>
    /// Gets or sets the package version from manifest
    /// </summary>
    [XmlAttribute("version")]
    public string Version { get; set; }

    #endregion

    public static bool CheckIfInstalled(Guid productCode)
    {
      return MsiInterop.MsiQueryProductState(productCode.ToString("B")) >= InstallState.Local;
    }

    public static bool CheckIfInstalled(string productCode)
    {
      return Guid.TryParse(productCode, out var code) && CheckIfInstalled(code);
    }

    public static Package FromProductInfo(Guid productCode, Guid upgradeCode)
    {
      var installedId = ValidateProductCode(productCode, upgradeCode);
      if (installedId == Guid.Empty)
      {
        return null;
      }

      var package = new Package
      {
        Id = installedId,
        Version = MsiInterop.GetProperty(productCode, "VersionString"),
        Publisher = MsiInterop.GetProperty(productCode, "Publisher"),
        DisplayName = MsiInterop.GetProperty(productCode, "ProductName")
      };
      package.NormalizedVersion = Utilities.NormalVersion(package.Version);
      return package;
    }

    public static string GetInstalledProductCode(Guid upgradeCode, out MsiEnumError error)
    {
      var productBuf = new StringBuilder(40);
      error = MsiInterop.MsiEnumRelatedProducts(upgradeCode.ToString("B"), 0, 0, productBuf);
      return error == MsiEnumError.Success || error == MsiEnumError.NoMoreItems
        ? productBuf.ToString()
        : null;
    }

    public static string GetInstalledProductCode(Guid upgradeCode)
    {
      return GetInstalledProductCode(upgradeCode, out _);
    }

    public static Guid ValidateProductCode(Guid productCode, Guid upgradeCode)
    {
      if (CheckIfInstalled(productCode))
      {
        // The product code matches an installed product.
        return productCode;
      }

      // Attempt to get an installed product code from the upgrade code.
      var installedProductCode = GetInstalledProductCode(upgradeCode);
      if (!string.IsNullOrEmpty(installedProductCode)
          && Guid.TryParse(installedProductCode, out var correctProductCode))
      {
        return correctProductCode;
      }

      return Guid.Empty;
    }

    public bool CanUpgradeTo(Package otherPackage)
    {
      return otherPackage.NormalizedVersion > NormalizedVersion
             || otherPackage.NormalizedVersion.Major > NormalizedVersion.Major
             || otherPackage.NormalizedVersion.Minor > NormalizedVersion.Minor;
    }

    public int CompareTo(object obj)
    {
      var package = obj as Package;
      return package?.NormalizedVersion.CompareTo(NormalizedVersion) ?? -1;
    }

    public List<PackageFeature> GetFeatures(string feature, bool matchTitle = false)
    {
      string lowerFeature = feature.ToLower();
      var list = new List<PackageFeature>();
      FindFeaturesInCollection(list, Features, lowerFeature, matchTitle);
      return list;
    }

    public void Initialize(string dataDir)
    {
      if (Controller == null)
      {
        Logger.LogInformation("Package - setting up controller");
        Controller = PluginManager.GetController(Product.Name);
        Controller.Package = this;
        Logger.LogInformation("Package - Initializing controller");
        Controller.Init();
      }

      if (IsInstalled)
      {
        Logger.LogInformation("Package - Installed - loading properties");
        LoadProperties();
        Logger.LogInformation("Package - Installed - Set Features states");
        SetFeatureStates();
        //if (UpgradeCode != new Guid())
        Logger.LogInformation("Package - Installed - Finding upgrades");
      }

      Logger.LogInformation("Package - Installed - Loading controller state");
      if (!string.IsNullOrEmpty(dataDir))
      {
        var controller = (ServerProductConfigurationController)Controller;
        controller.Settings.DataDirectory = dataDir;
      }

      Controller.LoadState();
    }

    public bool IsWithinServerMinMaxVersionRange(Package serverPackage)
    {
      if (serverPackage == null) return false;
      if (ServerMin == null) return true;
      if (serverPackage.NormalizedVersion < ServerMin) return false;
      if (ServerMax == null) return true;
      return serverPackage.NormalizedVersion <= ServerMax;
    }

    public void SetFeatureStates()
    {
      Logger.LogInformation("Package - SetFeatureStates - setting states");
      SetFeatureStates(Features);
    }

    public void SetProposedInstall(bool shouldInstall)
    {
      foreach (var feature in Features)
      {
        feature.SetProposedInstalled(shouldInstall);
      }
    }

    private void CreateKey()
    {
      string name = $"{Product.Name}-{Version}-{Architecture.ToString()}{(License == LicenseType.Community ? "" : "-com")}";
      _key = name.ToLowerInvariant();
    }

    private void FindFeaturesInCollection(ICollection<PackageFeature> list, IEnumerable<PackageFeature> features, string feature, bool matchTitle = false)
    {
      foreach (var pf in features)
      {
        if (pf.Name.ToLower().Contains(feature))
        {
          list.Add(pf);
        }
        else if (matchTitle && !string.IsNullOrEmpty(pf.Title) && pf.Title.ToLower().Contains(feature))
        {
          list.Add(pf);
        }

        FindFeaturesInCollection(list, pf.Features, feature);
      }
    }

    private long GetFeatureSize(IEnumerable<PackageFeature> features, bool onlySelectedFeatures)
    {
      long size = 0;

      foreach (var feature in features)
      {
        if (!onlySelectedFeatures || feature.ProposedInstalled)
        {
          size += feature.Size;
        }

        size += GetFeatureSize(feature.Features, onlySelectedFeatures);
      }

      return size;
    }

    private void LoadProperties()
    {
      Logger.LogInformation("Package - Load Properties - Loading msi properties");
      var installedId = ValidateProductCode(Id, UpgradeCode);
      if (installedId == Guid.Empty)
      {
        // There is no installed package with the current product code (Id)
        //  so properties can't be retrieved.
        return;
      }

      if (Id != installedId)
      {
        Id = installedId;
      }

      Publisher = MsiInterop.GetProperty(Id, "Publisher");
      DisplayName = MsiInterop.GetProperty(Id, "ProductName");
      string date = MsiInterop.GetProperty(Id, "InstallDate");
      PerMachine = MsiInterop.GetProperty(Id, "AssignmentType") == "1";
      if (date == null)
      {
        return;
      }

      DateTime dt;
      if (DateTime.TryParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
      {
        InstallDate = dt;
      }
    }

    /// <summary>
    /// Loads relevant properties from the MSI when package is not yet installed.
    /// </summary>
    public void LoadPropertiesForUninstalledPackage()
    {
      if (IsInstalled)
      {
        return;
      }

      PerMachine = MsiInterop.GetPropertyInfo(FullPath, "ALLUSERS") == "1";
    }

    /// <summary>
    /// Process the feature list
    /// </summary>
    /// <param name="toAdd">list of features to add</param>
    /// <param name="toRemove">list of features to remove</param>
    private void ProcessFeatures(List<PackageFeature> toAdd, List<PackageFeature> toRemove)
    {
      foreach (var packageFeature in Features)
      {
        if (packageFeature.Installed != packageFeature.ProposedInstalled)
        {
          if (!packageFeature.Installed)
          {
            toAdd.Add(packageFeature);
          }
          else
          {
            toRemove.Add(packageFeature);
          }
        }

        packageFeature.ProcessFeatures(toAdd, toRemove);
      }
    }

    private void SetFeatureStates(List<PackageFeature> features)
    {
      if (features == null)
      {
        return;
      }

      foreach (var feature in features)
      {
        Logger.LogInformation("Package - SetFeatureStates - setting feature " + feature.Name);
        feature.HasComponents = true;
        InstallState state = MsiInterop.MsiQueryFeatureState(Id.ToString("B"), feature.Name);
        feature.ProposedInstalled = feature.Installed = state == InstallState.Local;
        Logger.LogInformation("Package - SetFeatureStates - setting features for feature " + feature.Name);
        SetFeatureStates(feature.Features);
      }
    }
  }
}

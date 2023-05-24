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

using System.Drawing;
using System.Xml.Serialization;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Properties;

namespace MySql.Configurator.Core.Product
{
  /// <summary>
  /// The Product object
  /// </summary>
  public class Product 
  {
    #region Fields

    private string _baseTitle;

    #endregion Fields

    public Product()
    {
      CurrentState = ProductState.Unknown;
    }

    #region Properties

    [XmlIgnore]
    public string BaseTitle => _baseTitle ?? (_baseTitle = Title.TrimEnd('0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.', ' ', '-'));

    /// <summary>
    /// Gets the current state of the Product
    /// </summary>
    [XmlIgnore]
    public ProductState CurrentState { get; private set; }

    [XmlIgnore]
    public Package.Package LatestGA { get; set; }

    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlAttribute("regKeyTemplate")]
    public string RegistryKeyTemplate { get; set; }

    [XmlAttribute("setupTypeFlags")]
    public int SetupTypeFlags { get; set; }

    [XmlIgnore]
    public Image SmallIcon => Resources.ResourceManager.GetObject("ServerCategoryIcon24", Resources.Culture) as Image;

    [XmlAttribute("title")]
    public string Title { get; set; }

    [XmlAttribute("upgradeId")]
    public string UpgradeId { get; set; }

    [XmlAttribute("upgradeType")]
    public UpgradeType UpgradeType { get; set; }

    [XmlAttribute("urlBaseDir")]
    public string UrlBase { get; set; }

    #endregion Properties

    /// <summary>
    /// Look up function.  Converts ProductState enumeration to a text string.
    /// </summary>
    /// <param name="s">ProductState to covert</param>
    /// <returns>Text representing product state</returns>
    public static string StateAsString(ProductState s)
    {
      string result;
      switch (s)
      {
        case ProductState.Unknown:
          result = string.Empty;
          break;
        case ProductState.WebRemote:
          result = "To be downloaded";
          break;
        case ProductState.FoundLocal:
          result = "To be installed";
          break;
        case ProductState.WillPerformUpgrade:
          result = "To be upgraded";
          break;
        case ProductState.CurrentlyInstalled:
          result = "Changes pending";
          break;
        case ProductState.NewerInstalled:
          result = "Newer installed";
          break;
        case ProductState.DownloadStarted:
          result = "Download started";
          break;
        case ProductState.DownloadInProgress:
          result = "Downloading";
          break;
        case ProductState.DownloadSuccess:
          result = "Download success";
          break;
        case ProductState.DownloadError:
          result = "Download failed";
          break;
        case ProductState.DownloadBadArchive:
          result = "Downloaded ZIP file is corrupt";
          break;
        case ProductState.DownloadNoRemotePath:
          result = "No Remote Path Found";
          break;
        case ProductState.InstallStarted:
          result = "Install started";
          break;
        case ProductState.InstallInProgress:
          result = "Installing";
          break;
        case ProductState.InstallSuccess:
          result = "Install success";
          break;
        case ProductState.InstallError:
          result = "Install error";
          break;
        case ProductState.RemoveStarted:
          result = "Remove started";
          break;
        case ProductState.RemoveInProgress:
          result = "Removing";
          break;
        case ProductState.RemoveSuccess:
          result = "Successfully removed";
          break;
        case ProductState.RemoveFailed:
          result = "Failed to remove";
          break;
        case ProductState.UpdateStarted:
          result = "Update started";
          break;
        case ProductState.UpdateInProgress:
          result = "Updating";
          break;
        case ProductState.UpdateSuccess:
          result = "Update success";
          break;
        case ProductState.UpdateFailed:
          result = "Update failed";
          break;
        case ProductState.DownloadCancelled:
          result = "Download canceled";
          break;
        case ProductState.RebootRequired:
          result = "A reboot is required.";
          break;
        default:
          result = "Wrong product state";
          break;
      }

      return result;
    }

    /// <summary>
    /// Gets an estimate of the installation size
    /// </summary>
    /// <returns>The estimated on disk size of the install</returns>
    public long GetInstallationSizeEstimate()
    {
      // return GetPackage().GetInstallationSizeEstimate();
      long result = 0;
      //TODO
      //foreach (PackageFeature feature in GetPackage().Features)
      //{
      //  result += feature.GetInstallationSizeEstimate();
      //}

      return result;
    }

    /// <summary>
    /// Used to fetch the value of one of this product's registry keys
    /// </summary>
    /// <param name="keyName">The name of the registry key to retrieve</param>
    /// <returns>The value of the key</returns>
    public string GetInstalledProductRegistryKey(string keyName)
    {
      string keyValue = string.Empty;
      //TODO
      //Package pack = GetPackage();
      //pack.RegistryEntries.TryGetValue(keyName, out keyValue);
      return keyValue;
    }

    /// <summary>
    /// Initialization that must take place after the XML is parsed.
    /// </summary>
    /// <param name="initalizePackage">True to initialize the package too.</param>
    public void PostInitialize(bool initalizePackage)
    {
      //Package p = GetPackage();
      //if (initalizePackage)
      //{
      //  p.PostInitialize();
      //}

      //p.UrlBase = UrlBase;
      //p.IsCommercial = IsCommercialProduct;

      //if (!string.IsNullOrEmpty(p.Id))
      //{
      //  // Valid Package.
      //  // Is the package in our cache?
      //  CurrentState = p.FoundLocal ? ProductState.FoundLocal : ProductState.WebRemote;

      //  // Is the package an upgrade for another currently installed product?
      //  InstalledProductState state = FindRelatedInstalledProducts();
      //  switch (state)
      //  {
      //    case InstalledProductState.Older:
      //      IsUpgrade = true;
      //      CurrentState = ProductState.WillPerformUpgrade;
      //      break;

      //    case InstalledProductState.Newer:
      //      IsUpgrade = false;
      //      CurrentState = ProductState.NewerInstalled;
      //      break;

      //    case InstalledProductState.None:
      //      IsUpgrade = false;
      //      break;
      //  }

      //  // Is the package installed?
      //  _proposedInstalled = p.Installed;
      //  if (p.Installed)
      //  {
      //    CurrentState = ProductState.CurrentlyInstalled;
      //    string path = GetInstalledProductRegistryKey("Location");
      //    if (string.IsNullOrEmpty(path))
      //    {
      //      path = GetInstalledProductRegistryKey("InstallLocation");
      //    }

      //    if (!string.IsNullOrEmpty(path) && (path.EndsWith("\\") || path.EndsWith("/")))
      //    {
      //      path = path.Remove(path.Length - 1);
      //    }

      //    if (!string.IsNullOrEmpty(path))
      //    {
      //      InstallationPath = Path.GetDirectoryName(path);
      //      ProductSubPath = Path.GetFileName(path); // This works also for sub folders.
      //    }
      //  }

      //  // For new installations this path is empty at this point. However if we cannot find
      //  // the location of an already installed product then we can go the same route and do a good guess.
      //  if (string.IsNullOrEmpty(InstallationPath))
      //  {
      //    // Initialize installation path for the product, depending on product and OS architecture.
      //    bool wowPathRequired = Win32.Is64BitOS() && (Architecture == PackageArchitecture.X86);
      //    if (wowPathRequired)
      //    {
      //      InstallationPath = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)"), @"MySQL\");
      //    }
      //    else
      //    {
      //      InstallationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"MySQL\");
      //    }
      //  }

      //  if (string.IsNullOrEmpty(ProductSubPath))
      //  {
      //    // TODO: This sub path processing should not be hard coded!
      //    // Move it to the manifest.
      //    ProductSubPath = TitleWithVersion.Replace('/', ' ');
      //    if (Name == "Connector-odbc" || Name.Contains("mysql-server"))
      //    {
      //      ProductSubPath = ProductSubPath.Remove(ProductSubPath.LastIndexOf('.'));
      //    }

      //    if (Name == "Workbench")
      //    {
      //      ProductSubPath = ProductSubPath.Remove(ProductSubPath.LastIndexOf('.')) + " CE";
      //    }

      //    if (Name.Contains("memagent"))
      //    {
      //      ProductSubPath = "Enterprise";
      //    }

      //    ProductSubPath += "\\";
      //  }

      //  // Dereference mutual exclusions.
      //  // Note: currently only 1:1 exclusions are supported, however the implementation can easily
      //  //       be extended to support 1:n exclusions if needed (e.g. comma separated product ids in xml
      //  //       and a list of products in MutualExcludedProduct(s) here).
      //  MutualExcludedProduct = null;
      //  foreach (Product product in ProductManager.Products)
      //  {
      //    if (!string.IsNullOrEmpty(MutualExclusive) && (MutualExclusive == product.Name))
      //    {
      //      MutualExcludedProduct = product;
      //      if (Installed && product.Installed)
      //      {
      //        Logger.LogWarning(
      //          string.Format("Found products installed which should never be installed together:\n\t{0}\n\t{1}", TitleWithVersionAndArchitecture, product.TitleWithVersionAndArchitecture));
      //      }

      //      break;
      //    }
      //  }
      //}
    }

    /*
    //private InstalledProductState FindRelatedInstalledProducts()
    //{
    //  InstalledProductState result = InstalledProductState.None;

    //  if (UpgradeId.Length > 0)
    //  {
    //    StringBuilder productCode = new StringBuilder(39);
    //    uint currentIndex = 0;
    //    MSIEnumError rc = MSIEnumError.Success;

    //    while (rc == MSIEnumError.Success)
    //    {
    //      // Setup and query MSI database.
    //      productCode.Clear();
    //      try
    //      {
    //        rc = (MSIEnumError)MsiInterop.MsiEnumRelatedProducts(UpgradeId, 0, currentIndex++, productCode);
    //      }
    //      catch (Exception e)
    //      {
    //        _installActionLog.AppendLine(e.Message);
    //      }

    //      // Failed to retrieve another package.
    //      if (productCode.Length <= 0)
    //      {
    //        continue;
    //      }

    //      Package p = GetPackage();
    //      string relatedProductCode = productCode.ToString();

    //      // Make sure the packages aren't the same.
    //      if (relatedProductCode == p.Id)
    //      {
    //        continue;
    //      }

    //      // No need to search again if we have already 2 packages.
    //      if (Packages.Count > 1)
    //      {
    //        result = InstalledProductState.Older;
    //        continue;
    //      }

    //      Package relatedPackage = new Package();
    //      relatedPackage.Id = relatedProductCode;
    //      relatedPackage.UpdateOptionalParameters();

    //      // Make sure the pacakges aren't the same version.
    //      if (p.ThisVersion == relatedPackage.ThisVersion)
    //      {
    //        continue;
    //      }

    //      Version relatedPackageVersion = Utilities.NormalVersion(relatedPackage.ThisVersion);
    //      Version packageVersion = Utilities.NormalVersion(p.ThisVersion);

    //      if ((IsServerProduct || IsExamplesProduct || IsDocumentsProduct) &&
    //          ((packageVersion.Major != relatedPackageVersion.Major) || (packageVersion.Minor != relatedPackageVersion.Minor)))
    //      {
    //        continue;
    //      }

    //      bool haveNewer = packageVersion > relatedPackageVersion; // packageVersion == relatedPackageVersion is not possible here.

    //      if (!PopulateRelatedPackageFeatures(relatedPackage))
    //      {
    //        continue;
    //      }

    //      if ((p.Architecture == relatedPackage.Architecture) || (p.Architecture == PackageArchitecture.Any) || (relatedPackage.Architecture == PackageArchitecture.Any))
    //      {
    //        relatedPackage.PostInitialize();
    //        relatedPackage.UrlBase = p.UrlBase;
    //        if (!haveNewer)
    //        {
    //          // The machine has a package installed that's newer than our manifest.
    //          // Our manifest's package is outdated so toss it.
    //          Packages.Clear();
    //          result = InstalledProductState.Newer;
    //        }
    //        else
    //        {
    //          result = InstalledProductState.Older;
    //        }

    //        Packages.Add(relatedPackage);
    //      }
    //      else
    //      {
    //        result = InstalledProductState.None;
    //      }
    //    }

    //    if (rc != MSIEnumError.NoMoreItems)
    //    {
    //      // Record the real error.
    //      _installActionLog.AppendLine(string.Format("Found error: {0}", rc.ToString()));
    //    }
    //  }
    //  else
    //  {
    //    // Do a funky upgrade.
    //    // No need to search again if we already have 2 packages.
    //    if (Packages.Count < 2)
    //    {
    //      Package thisPackage = GetPackage();
    //      Package possibleOlderPackage = new Package();
    //      possibleOlderPackage.Id = thisPackage.Id;
    //      if (possibleOlderPackage.Installed && possibleOlderPackage.ThisVersion != thisPackage.ThisVersion)
    //      {
    //        bool foundProducts = PopulateRelatedPackageFeatures(possibleOlderPackage);
    //        if (foundProducts)
    //        {
    //          possibleOlderPackage.PostInitialize();
    //          possibleOlderPackage.UrlBase = thisPackage.UrlBase;
    //          Packages.Add(possibleOlderPackage);
    //          result = InstalledProductState.Older; // We assume we do an upgrade of the installed product.
    //          // TODO: shouldn't we add the same version and architecture checks here as we have above?
    //        }
    //      }
    //    }
    //    else
    //    {
    //      // With 2 packages already listed we have an upgrade operation.
    //      result = InstalledProductState.Older;
    //    }
    //  }

    //  return result;
    //}

    #region MSI Actions in a background worker thread.

    /// <summary>
    /// Helper function for generating MSI path property string.
    /// </summary>
    /// <returns>A string of MSI path properities suitable for command-line actions (Install/Update)</returns>
    //private string GetPathProperties()
    //{
    //  // WiX will write the ARPINSTALLLOCATION value to the following registry key:
    //  // HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{PackageID}\InstallLocation
    //  // On 64-bit systems where 32-bit MSIs are installed, the value will be written to this registry key instead:
    //  // HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{PackageID}\InstallLocation

    //  string oldPath = string.Empty;
    //  foreach (Package pack in Packages.Where(pack => pack.IsInstalled))
    //  {
    //    pack.RegistryEntries.TryGetValue("Location", out oldPath);
    //  }

    //  Version version = new Version(Version);

    //  string properties = !string.IsNullOrEmpty(oldPath) && IsServerProduct && (version.Major.ToString() + version.Minor.ToString()) == "57"
    //    ? string.Format("INSTALLDIR=\"{0}\" INSTALLLOCATION=\"{0}\" ARPINSTALLLOCATION=\"{0}\" ", oldPath)
    //    : string.Format("INSTALLDIR=\"{0}\" INSTALLLOCATION=\"{0}\" ARPINSTALLLOCATION=\"{0}\" ", Path.Combine(InstallationPath, ProductSubPath));

    //  if (Controller is ServerProductConfigurationController)
    //  {
    //    //TODO
    //    //string dataDir = (Controller as ServerProductConfigurationController).ConfigData.DataDir;
    //    //properties += string.Format(" DATADIR=\"{0}\" ", dataDir);
    //  }

    //  return properties;
    //}

    #endregion

    /// <summary>
    /// Stops the current download or installation process if there is one running.
    /// </summary>
    //public void CancelActions()
    //{
    //  switch (CurrentState)
    //  {
    //    case ProductState.DownloadStarted:
    //    case ProductState.DownloadInProgress:
    //      //_webClient.CancelAsync();
    //      break;

    //    case ProductState.InstallStarted:
    //    case ProductState.InstallInProgress:
    //      if (_backgroundWorker.WorkerSupportsCancellation)
    //      {
    //        _backgroundWorker.CancelAsync();
    //      }

    //      break;
    //  }
    //}
    */
  }
}

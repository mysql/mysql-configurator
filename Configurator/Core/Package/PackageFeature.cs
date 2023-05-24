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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MySql.Configurator.Core.MSI;

namespace MySql.Configurator.Core.Package
{
  public class PackageFeature
  {
    #region Constants

    /// <summary>
    /// SQL query used to identify the features that have associated components.
    /// </summary>
    private const string GET_FEATURES_COMPONENTS_QUERY = "SELECT DISTINCT FeatureComponents.Feature_ FROM FeatureComponents";

    /// <summary>
    /// SQL query used to retrieve the list of features from the database.
    /// </summary>
    private const string GET_FEATURES_QUERY = "SELECT Feature.Feature, Feature.Feature_Parent, Feature.Title, Feature.Description, Feature.Display, Feature.Attributes FROM Feature ORDER BY Feature.Display, Feature.Feature_Parent";

    #endregion Constants

    #region Fields

    /// <summary>
    /// Flag indicating if the feature is proposed to be installed.
    /// </summary>
    private bool _proposedInstalled;

    #endregion Fields

    public PackageFeature()
    {
      Features = new List<PackageFeature>();
      Default = true;
    }

    public PackageFeature(string name) : this()
    {
      Name = name;
    }

    #region Properties

    /// <summary>
    /// Returns all child and grand child features.
    /// </summary>
    public List<PackageFeature> AllFeatures
    {
      get
      {
        var result = new List<PackageFeature> { this };
        foreach (var feature in Features)
        {
          result.AddRange(feature.AllFeatures);
        }

        return result;
      }
    }

    public bool Default { get; set; }

    public string Description { get; set; }

    public string Display { get; set; }

    public int FeatureCount
    {
      get
      {
        int count = 0;
        foreach (var packageFeature in Features)
        {
          count++;
          count += packageFeature.FeatureCount;
        }

        return count;
      }
    }

    public List<PackageFeature> Features { get; set; }

    public bool HasComponents { get; set; }

    public bool Installed { get; set; }

    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the parent feature (if any).
    /// </summary>
    public PackageFeature Parent { get; set; }

    public bool ProposedInstalled
    {
      get { return _proposedInstalled; }
      set { SetProposedInstalled(value); }
    }

    /// <summary>
    /// Flag to indicate if the feature is required for installation.
    /// Required features can't be unselected when configuring the package.
    /// </summary>
    public bool Required { get; private set; }

    public long Size { get; set; }

    public string Title { get; set; }

    #endregion Properties

    /// <summary>
    /// Gets the features of a product by retrieving them from the provided MSI file.
    /// </summary>
    /// <param name="msiPath">The path to the MSI file.</param>
    /// <returns>A list of features found in the MSI file.</returns>
    public static List<PackageFeature> GetFeaturesForProduct(string msiPath)
    {
      using (var query = new MsiQuery(msiPath))
      {
        return GetFeaturesForProduct(query, true);
      }
    }

    /// <summary>
    /// Gets the features of an installed product by searching for it with the provided product code.
    /// </summary>
    /// <param name="productCode">The product code.</param>
    /// <returns>A list of features associated to the installed producted.</returns>
    public static List<PackageFeature> GetFeaturesForProduct(Guid productCode)
    {
      using (var query = new MsiQuery(productCode))
      {
        return GetFeaturesForProduct(query, false);
      }
    }

    /// <summary>
    /// Gets a list of features by using the provided SQL query.
    /// </summary>
    /// <param name="query">The SQL query used to extract the features.</param>
    /// <param name="enablePropossedInstalled">Flag used to set the propossedIntalled property of each feature.</param>
    /// <returns>A list of features.</returns>
    private static List<PackageFeature> GetFeaturesForProduct(MsiQuery query, bool enablePropossedInstalled)
    {
      if (query == null)
      {
        return null;
      }

      var featureMap = new Dictionary<string, PackageFeature>();
      var features = new List<PackageFeature>();

      // Get list of features.
      if (!query.OpenQuery(GET_FEATURES_QUERY))
      {
        return null;
      }

      while (query.NextRow())
      {
        var feature = new PackageFeature();
        string name = query.GetString(1);
        if (featureMap.ContainsKey(name))
        {
          feature = featureMap[name];
        }

        feature.Name = query.GetString(1);
        feature.Title = query.GetString(3);
        feature.Description = query.GetString(4);
        feature.Display = query.GetString(5);
        feature.Required = GetRequiredAttribute(query.GetString(6));
        feature.SetProposedInstalled(enablePropossedInstalled);
        var parent = query.GetString(2);

        if (!featureMap.ContainsKey(name))
        {
          featureMap.Add(feature.Name, feature);
        }

        if (!string.IsNullOrEmpty(parent))
        {
          if (!featureMap.ContainsKey(parent))
          {
            featureMap.Add(parent, new PackageFeature(parent));
          }

          var parentFeature = featureMap[parent];
          parentFeature.Features.Add(feature);
          feature.Parent = parentFeature;
        }
        else
        {
          features.Add(feature);
        }
      }

      query.Close();
      if (!query.OpenQuery(GET_FEATURES_COMPONENTS_QUERY))
      {
        return null;
      }

      // Get features with associated components.
      while (query.NextRow())
      {
        var componentFeatureName = query.GetString(1);
        foreach (var keyValuePair in featureMap)
        {
          var feature = keyValuePair.Value as PackageFeature;
          SetFeatureComponents(feature, componentFeatureName);
          if (!feature.HasComponents)
          {
            feature.HasComponents = feature.AllFeatures.Any(o => o.HasComponents);
          }
        }
      }

      query.Close();
      return features;
    }

    public virtual long GetInstallationSizeEstimate()
    {
      long sizeEst = Features.Sum(f => f.Size);
      sizeEst += Size;
      return sizeEst;
    }

    /// <summary>
    /// Reads the attributes value to identify the state of the required flag.
    /// </summary>
    /// <param name="attributesString">The string representation of the attributes value.</param>
    /// <returns><c>true</c> if the feature is required; otherwise, <c>false</c>.</returns>
    private static bool GetRequiredAttribute(string attributesString)
    {
      if (!int.TryParse(attributesString, out int attributes))
      {
        return false;
      }

      var byteArray = new BitArray(new int[] { attributes });
      var bits = new bool[byteArray.Count];
      byteArray.CopyTo(bits, 0);
      if (byteArray.Length < 5)
      {
        return false;
      }

      return byteArray[4];
    }

    public bool HasChanges()
    {
      var hasChanges = Installed != ProposedInstalled;
      return Features.Aggregate(hasChanges, (current, feature) => current || feature.HasChanges());
    }

    public virtual void SetProposedInstalled(bool shouldInstall)
    {
      if (shouldInstall == _proposedInstalled)
      {
        return;
      }

      _proposedInstalled = shouldInstall;
      foreach (var packageFeature in Features)
      {
        packageFeature.SetProposedInstalled(shouldInstall);
      }
    }

    internal void ProcessFeatures(List<PackageFeature> toAdd, List<PackageFeature> toRemove)
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

    /// <summary>
    /// Sets the HasComponents flag to <c>true</c> for the feature or child feature that matches the provided feature name.
    /// </summary>
    /// <param name="feature">The feature to review.</param>
    /// <param name="componentFeatureName">The feature name that will be modified.</param>
    private static void SetFeatureComponents(PackageFeature feature, string componentFeatureName)
    {
      if (feature.Name == componentFeatureName)
      {
        feature.HasComponents = true;
        return;
      }


      if (feature.Features == null)
      {
        return;
      }

      foreach (var childFeature in feature.Features)
      {
        SetFeatureComponents(childFeature, componentFeatureName);
      }
    }
  }
}
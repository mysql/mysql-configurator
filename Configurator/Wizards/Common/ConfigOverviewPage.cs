/* Copyright (c) 2011, 2023, Oracle and/or its affiliates.

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
using System.Linq;
using System.Windows.Forms;
using MySql.Configurator.Core.Controllers;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Core.Package;
using MySql.Configurator.Core.Wizard;
using MySql.Configurator.Dialogs;
using MySql.Configurator.Properties;

namespace MySql.Configurator.Wizards.Common
{
  public partial class ConfigOverviewPage : WizardPage
  {
    #region Fields

    private readonly ConfigurationType _configurationType;
    private readonly Wizard _parentWizard;
    private ListViewItem _configuringItem;
    private ConfigWizard.ConfigWizard _configWizard;
    private int _index = -1;

    #endregion Fields

    public ConfigOverviewPage(Wizard parentWizard, List<Package> packageList, ConfigurationType configType)
    {
      _configWizard = null;
      _parentWizard = parentWizard;
      InitializeComponent();
      PackageList = packageList;
      PageVisible = PackageList.Count != 0;
      _configurationType = configType;
    }

    #region Properties

    public override bool BackOk
    {
      get
      {
        Wizard.BackButton.Visible = false;
        return false;
      }
    }

    public MainForm MainForm => (MainForm)Application.OpenForms[0];

    public List<Package> PackageList { get; set; }

    #endregion Properties

    public override void Activate()
    {
      foreach (var package in PackageList)
      {
        var item = package.Product.Name.ToLower().Contains("server")
          ? ProductsListView.Items.Insert(0, string.Empty)
          : ProductsListView.Items.Add(string.Empty);
        item.Name = package.NameWithVersion;
        item.Tag = package;
        item.SubItems.Add(package.NameWithVersion);
        item.SubItems.Add("Ready to configure");
      }

      base.Activate();
    }

    public override bool Next()
    {
      _index++;
      if (_index >= ProductsListView.Items.Count)
      {
        return true;
      }

      _configuringItem = ProductsListView.Items[_index];
      _configuringItem.SubItems[2].Text = Resources.ConfigOverviewConfiguringText;

      var package = _configuringItem.Tag as Package;
      _configWizard = new ConfigWizard.ConfigWizard();
      _configWizard.WizardCanceled += ConfigWizardCanceled;
      _configWizard.WizardClosed += ConfigWizardClosed;
      _configWizard.ShowWizard(package, MainForm, _configurationType);
      return false;
    }

    public void RefreshConfiguringPackages()
    {
      var configuringPackages = new List<Package>();
      foreach (var package in PackageList.Where(p => p.IsInstalled))
      {
        package.Initialize(((ServerProductConfigurationController)package.Controller).Settings.DataDirectory);
        if (!package.Controller.CanConfigure(_configurationType))
        {
          continue;
        }

        configuringPackages.Add(package);
      }

      PageVisible = configuringPackages.Count > 0;
      _parentWizard.Refresh();
      PackageList = configuringPackages;
    }

    private void ConfigWizardCanceled(object sender, EventArgs e)
    {
      _configuringItem.SubItems[2].Text = Resources.ConfigOverviewConfigurationCancelledText;
      DisposeConfigWizard();
    }

    private void ConfigWizardClosed(object sender, EventArgs e)
    {
      if (_configuringItem?.Tag is Package package)
      {
        _configuringItem.SubItems[2].Text = GetProductStateString(package.Controller.CurrentState);
      }

      DisposeConfigWizard();
    }

    /// <summary>
    /// Disposes of the configuration wizard.
    /// </summary>
    private void DisposeConfigWizard()
    {
      _configWizard.WizardCanceled -= ConfigWizardCanceled;
      _configWizard.WizardClosed -= ConfigWizardClosed;
      _configWizard.Dispose();
      _configWizard = null;
    }

    private string GetProductStateString(ConfigState configurationState)
    {
      string textStatus = string.Empty;
      switch (configurationState)
      {
        case ConfigState.ConfigurationRequired:
          textStatus = "Initial configuration.";
          break;

        case ConfigState.ConfigurationInProgress:
          textStatus = "Configuring product.";
          break;

        case ConfigState.ConfigurationComplete:
          textStatus = "Configuration complete.";
          break;

        case ConfigState.ConfigurationError:
          textStatus = "Configuration failed.";
          break;

        case ConfigState.ConfigurationUnnecessary:
          textStatus = "Configuration not needed.";
          break;
      }

      return textStatus;
    }
  }
}
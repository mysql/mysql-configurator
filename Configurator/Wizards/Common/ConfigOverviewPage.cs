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

    public ConfigOverviewPage(Wizard parentWizard, Package package, ConfigurationType configType)
    {
      _configWizard = null;
      _parentWizard = parentWizard;
      InitializeComponent();
      Package = package;
      PageVisible = package != null;
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

    public Package Package { get; set; }

    #endregion Properties

    public override void Activate()
    {
      var item = Package.Product.Name.ToLower().Contains("server")
        ? ProductsListView.Items.Insert(0, string.Empty)
        : ProductsListView.Items.Add(string.Empty);
      item.Name = Package.NameWithVersion;
      item.Tag = Package;
      item.SubItems.Add(Package.NameWithVersion);
      item.SubItems.Add("Ready to configure");

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

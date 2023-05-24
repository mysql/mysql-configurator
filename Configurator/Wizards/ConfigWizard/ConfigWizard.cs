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

using System.Linq;
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.Forms;
using MySql.Configurator.Core.Controllers;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Core.Package;
using MySql.Configurator.Dialogs;
using MySql.Configurator.Properties;
using MySql.Configurator.Wizards.Common;

namespace MySql.Configurator.Wizards.ConfigWizard
{
  public partial class ConfigWizard : Wizard
  {
    public ConfigWizard()
    {
      InitializeComponent();
    }

    #region Properties

    public override bool CanCancel
    {
      get
      {
        var result = InfoDialog.ShowDialog(InfoDialogProperties.GetYesNoDialogProperties(InfoDialog.InfoType.Warning, Resources.CancelQuestionText, Resources.CancelConfigurationQuestion)).DialogResult;
        return result != DialogResult.No;
      }
    }

    public ConfigurationType ConfigurationType { get; set; }

    #endregion Properties

    public void ShowWizard(Package package, MainForm parentMainForm, ConfigurationType configurationType)
    {
      ClearPages();
      var configurationController = package.Controller;

      // Need to assign correctly for some operations how will behave the configuration UI and backend operations
      if (configurationType == ConfigurationType.Reconfiguration
          && configurationController.GetType().IsSameOrSubclass(typeof(ServerProductConfigurationController)))
        {
          var serverController = (ServerProductConfigurationController)configurationController;
          if (serverController.Settings.ConfigurationFileExists == false)
          {
            configurationType = ConfigurationType.New;
            AddPage(new WelcomePage());
          }
          else if (serverController.Settings.PendingSystemTablesUpgrade)
          {
            configurationType = ConfigurationType.Upgrade;
          }
        }

      configurationController.ConfigurationType = configurationType;
      ConfigurationType = configurationType;
      configurationController.PrepareForConfigure();
      WizardSideBar.ShowConfigPanel(package.NameWithVersion);
      configurationController.SetPages();
      foreach (var page in configurationController.Pages.Where(page => page.ValidForType(configurationType)))
      {
        AddPage(page);
      }

      var configApply = new ConfigApplyPage(configurationController);
      AddPage(configApply);
      AddPage(new ConfigCompletePage());
      base.ShowWizard(parentMainForm);
    }
  }
}
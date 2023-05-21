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
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.Forms;
using MySql.Configurator.Core.Controllers;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Core.Package;
using MySql.Configurator.Core.Wizard;
using MySql.Configurator.Properties;

namespace MySql.Configurator.Wizards.Common
{
  public partial class PathsConflictPage : WizardPage
  {
    private bool _hasWarnings;

    public List<Package> ConflictPackages { get; set; }

    public PathsConflictPage()
    {
      _hasWarnings = false;
      InitializeComponent();
      PageVisible = false;
    }

    private void CheckDirectory(Package package)
    {
      //var warning = package.Controller.GetDirectoryWarning();
      //CheckWarningType(warning);
      //var warnings = ConflictPackages.SelectMany(s => s.Controller.GetDirectoryWarning().GetFlags())
      //    .Cast<PathWarningType>()
      //    .Where(w => w != PathWarningType.None).ToList();
      //WarningsResultLabel.Text = $"{warnings.Count} Warnings";
      //_hasWarnings = warnings.Count > 0;

      UpdateButtons();
    }

    protected void CheckWarningType(PathWarningType warningType)
    {
      InstallPathWarningLabel.Text = DataPathWarningLabel.Text = string.Empty;
      InstallDirWarningPictureBox.Visible =
        InstallDirErrorPictureBox.Visible = 
        DataDirWarningPictureBox.Visible = 
        DataDirErrorPictureBox.Visible = false;

      if (warningType.HasFlag(PathWarningType.InstallDirPathInvalid))
      {
        InstallPathWarningLabel.Text = Resources.PathInvalidError;
        InstallDirErrorPictureBox.Visible = true;
      }
      else if (warningType.HasFlag(PathWarningType.InstallDirPathExists))
      {
        InstallPathWarningLabel.Text = Resources.PathExistsWarning;
        InstallDirWarningPictureBox.Visible = true;
      }
      else if (warningType.HasFlag(PathWarningType.InstallDirPathCurrentInUse))
      {
        InstallPathWarningLabel.Text = Resources.PathAlreadyUsed;
        InstallDirWarningPictureBox.Visible = true;
      }

      InstallPathWarningLabel.Visible = !String.IsNullOrEmpty(InstallPathWarningLabel.Text);

      if (warningType.HasFlag(PathWarningType.DataDirPathInvalid))
      {
        DataPathWarningLabel.Text = Resources.PathInvalidError;
        DataDirErrorPictureBox.Visible = true;
      }
      else if (warningType.HasFlag(PathWarningType.DataDirPathExists))
      {
        DataPathWarningLabel.Text = Resources.PathExistsWarning;
        DataDirWarningPictureBox.Visible = true;
      }
      else if (warningType.HasFlag(PathWarningType.DataDirPathCurrentInUse))
      {
        DataPathWarningLabel.Text = Resources.PathAlreadyUsed;
        DataDirWarningPictureBox.Visible = true;
      }

      DataPathWarningLabel.Visible = !String.IsNullOrEmpty(DataPathWarningLabel.Text);
    }

    public override void Activate()
    {
      ProductListView.Items.Clear();
      if (ConflictPackages == null)
      {
        return;
      }

      foreach (var package in ConflictPackages)
      {
        ListViewItem lvi = ProductListView.Items.Add(package.NameWithVersion);
        lvi.Tag = package;
        lvi.SubItems.Add(package.Architecture.ToString());
      }

      if (ProductListView.Items.Count > 0)
      {
        ProductListView.Items[0].Selected = true;
      }

      base.Activate();
    }

    public override bool Next()
    {
      if (!_hasWarnings)
      {
        return base.Next();
      }

      var result = InfoDialog.ShowDialog(InfoDialogProperties.GetYesNoDialogProperties(InfoDialog.InfoType.Warning, Resources.WarningText, Resources.HasPathWarnings)).DialogResult;
      return result == DialogResult.Yes;
    }

    private void ProductListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
    {
      DataDirPanel.Visible = false;
      InstallDirPanel.Visible = false;
      if (!e.IsSelected
          || !(e.Item.Tag is Package))
      {
        return;
      }

      var package = (Package)e.Item.Tag;
      DataDirPanel.Visible = false;
      InstallDirPanel.Visible = true;
      var controller = package.Controller as ServerProductConfigurationController;
      if (controller != null)
      {
        // The current package is a server package
        DataDirPanel.Visible = true;
        InstallDirTextBox.Text = controller.InstallDirectory;
        DataDirTextBox.Text = controller.DataDirectory;
      }
      else
      {
        // The package is standard 
        InstallDirTextBox.Text = package.Controller.InstallDirectory;
      }

      CheckDirectory(package);
    }

    protected void Browse(TextBox textBox)
    {
      if (ProductListView.SelectedItems.Count != 1
          || !(ProductListView.SelectedItems[0].Tag is Package))
      {
        return;
      }

      var d = new FolderBrowserDialog
      {
        SelectedPath = textBox.Text
      };
      var package = (Package)ProductListView.SelectedItems[0].Tag;
      if (d.ShowDialog() == DialogResult.Cancel)
      {
        return;
      }

      textBox.Text = d.SelectedPath;
      package.Controller.Settings.InstallDirectory = InstallDirTextBox.Text.Trim();
      CheckDirectory(package);
    }

    private void InstallDirResetButton_Click(object sender, EventArgs e)
    {
      if (ProductListView.SelectedItems.Count != 1
          || !(ProductListView.SelectedItems[0].Tag is Package))
      {
        return;
      }
      
      var package = (Package)ProductListView.SelectedItems[0].Tag;
      package.Controller.Settings.InstallDirectory = package.Controller.Settings.DefaultInstallDirectory;
      InstallDirTextBox.Text = package.Controller.Settings.DefaultInstallDirectory;
      CheckDirectory(package);
    }

    private void DataDirResetButton_Click(object sender, EventArgs e)
    {
      if (ProductListView.SelectedItems.Count != 1
          || !(ProductListView.SelectedItems[0].Tag is Package))
      {
        return;
      }

      var package = (Package)ProductListView.SelectedItems[0].Tag;
      var controller = package.Controller as ServerProductConfigurationController;
      controller.DataDirectory = controller.Settings.DefaultDataDirectory;
      DataDirTextBox.Text = controller.Settings.DefaultDataDirectory;
      CheckDirectory(package);
    }

    private void InstallDirBrowseButton_Click(object sender, EventArgs e)
    {
      Browse(InstallDirTextBox);
    }

    private void DataDirBrowseButton_Click(object sender, EventArgs e)
    {
      Browse(DataDirTextBox);
    }

    private void Textbox_Leave(object sender, EventArgs e)
    {
      if (!(sender is TextBox)
          || ProductListView.SelectedItems.Count != 1
          || !(ProductListView.SelectedItems[0].Tag is Package))
      {
        return;
      }

      var textbox = (TextBox)sender;
      if (textbox.Tag == null
          || (textbox.Tag.ToString() != "InstallDir"
              && textbox.Tag.ToString() != "DataDir"))
      {
        return;
      }

      var package = (Package)ProductListView.SelectedItems[0].Tag;
      if (textbox.Tag.ToString() == "InstallDir")
      {
        package.Controller.Settings.InstallDirectory = textbox.Text.Trim();
      }

      if (textbox.Tag.ToString() == "DataDir" && package.Controller is ServerProductConfigurationController)
      {
        ((ServerProductConfigurationController)package.Controller).Settings.DataDirectory = textbox.Text.Trim();
      }
      
      CheckDirectory(package);
    }
  }
}

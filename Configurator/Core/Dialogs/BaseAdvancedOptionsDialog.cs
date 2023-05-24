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
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.Forms;
using MySql.Configurator.Core.Forms;
using MySql.Configurator.Properties;
using Utilities = MySql.Configurator.Core.Classes.Utilities;

namespace MySql.Configurator.Core.Dialogs
{
  public partial class BaseAdvancedOptionsDialog : AutoStyleableBaseDialog
  {
    public BaseAdvancedOptionsDialog()
    {
      InitializeComponent();
    }

    public BaseAdvancedOptionsDialog(Package.Package package)
    {
      Package = package;
      Text += @" for " + package.NameWithVersion;
      InitializeComponent();
      InstallDirTextBox.Text = Package.Controller.InstallDirectory;
      CheckDirectory();
    }

    #region Properties

    public bool HasWarnings { get; protected set; }

    public Package.Package Package { get; protected set; }

    public sealed override string Text
    {
      get { return base.Text; }
      set { base.Text = value; }
    }

    #endregion Properties

    protected void Browse(TextBox textBox)
    {
      using (var folderBrowserDialog = new FolderBrowserDialog())
      {
        folderBrowserDialog.SelectedPath = textBox.Text;
        if (folderBrowserDialog.ShowDialog() == DialogResult.Cancel)
        {
          return;
        }

        textBox.Text = folderBrowserDialog.SelectedPath;
      }

      CheckDirectory();
    }

    protected void CheckDirectory(string path, List<string> paths, PictureBox warningIcon, PictureBox errorIcon, Label label)
    {
      label.Text = string.Empty;
      warningIcon.Visible = errorIcon.Visible = false;
      if (path.IndexOfAny(Path.GetInvalidPathChars()) >= 0 || !Path.IsPathRooted(path))
      {
        label.Text = Resources.PathInvalidError;
        errorIcon.Visible = true;
      }
      else if (Directory.Exists(path))
      {
        label.Text = Resources.PathExistsWarning;
        warningIcon.Visible = true;
      }
      else if (PathAlreadyUsed(path, paths))
      {
        label.Text = Resources.PathAlreadyUsed;
        warningIcon.Visible = true;
      }

      var errorMessage = Utilities.ValidateFilePath(path);
      if (!string.IsNullOrEmpty(errorMessage))
      {
        label.Text = errorMessage;
        errorIcon.Visible = true;
      }

      label.Visible = !string.IsNullOrEmpty(label.Text);
    }

    protected override void OnLoad(EventArgs e)
    {
      Utilities.NormalizeFont(this);
      base.OnLoad(e);
    }

    protected virtual void Save()
    {
      Package.Controller.Settings.InstallDirectory = InstallDirTextBox.Text.Trim();
    }

    protected virtual void UpdateOkButton()
    {
      OkButton.Enabled = !InstallDirErrorPictureBox.Visible;
      HasWarnings = InstallDirWarningPictureBox.Visible;
    }

    private void BaseAdvancedOptionsDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (DialogResult == DialogResult.Cancel)
      {
        return;
      }

      if (HasWarnings)
      {
        var dr = InfoDialog.ShowDialog(InfoDialogProperties.GetOkCancelDialogProperties(InfoDialog.InfoType.Warning, Resources.WarningText, Resources.HasOptionWarnings)).DialogResult;
        if (dr == DialogResult.Cancel)
        {
          e.Cancel = true;
          return;
        }
      }

      Save();
    }

    private void CheckDirectory()
    {
      var paths = new List<string>();
      CheckDirectory(InstallDirTextBox.Text.Trim(), paths, InstallDirWarningPictureBox, InstallDirErrorPictureBox, InstallDirWarningLabel);
      UpdateOkButton();
    }

    private void InstallDirBrowseButton_Click(object sender, EventArgs e)
    {
      Browse(InstallDirTextBox);
    }

    private void InstallDirRevertButton_Click(object sender, EventArgs e)
    {
      Package.Controller.Settings.InstallDirectory = Package.Controller.Settings.DefaultInstallDirectory;
      InstallDirTextBox.Text = Package.Controller.Settings.DefaultInstallDirectory;
      CheckDirectory();
    }

    private void InstallDirTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
    {
      CheckDirectory();
    }

    /// <summary>
    /// Validates if the provided path is contained in the list of provided paths. 
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="paths">The list of paths.</param>
    /// <returns></returns>
    private bool PathAlreadyUsed(string path, List<string> paths)
    {
      if (paths == null
          || paths.Count == 0)
      {
        return false;
      }

      string fullPath = Path.GetFullPath(path);
      return paths.Select(Path.GetFullPath).Any(storedFullPath => storedFullPath.Equals(fullPath, StringComparison.OrdinalIgnoreCase));
    }
  }
}

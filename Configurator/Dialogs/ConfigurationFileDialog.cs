/* Copyright (c) 2024, Oracle and/or its affiliates.

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
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Forms;
using Utilities = MySql.Configurator.Core.Classes.Utilities;
using MySql.Configurator.Properties;
using MySql.Configurator.Core.Controllers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MySql.Configurator.Dialogs
{
  /// <summary>
  /// Dialog that enables to provide an updated path for the server configuration file.
  /// </summary>
  public partial class ConfigurationFileDialog : AutoStyleableBaseDialog
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationFileDialog"/> class.
    /// </summary>
    public ConfigurationFileDialog()
    {
      InitializeComponent();
    }

    #region Properties

    /// <summary>
    /// The path containing the my.ini or my.cnf server configuration file.
    /// </summary>
    public string ConfigurationFilePath { get; private set; }

    #endregion Properties

    /// <summary>
    /// Checks that the path in the Configuration File textbox is valid and contains a server 
    /// configuration file.
    /// </summary>
    protected void CheckDirectory()
    {
      var path = ConfigurationFileTextBox.Text.Trim();
      ConfigurationFileErrorLabel.Text = string.Empty;
      ConfigurationFileErrorPictureBox.Visible = false;
      if (string.IsNullOrEmpty(path))
      {
        OkButton.Enabled = false;
        return;
      }

      var iniFilePath = Path.Combine(path, BaseServerSettings.DEFAULT_CONFIG_FILE_NAME);
      var alternateIniFilePath = Path.Combine(path, BaseServerSettings.ALTERNATE_CONFIG_FILE_NAME);
      if (path.IndexOfAny(Path.GetInvalidPathChars()) >= 0 || !Path.IsPathRooted(path))
      {
        ConfigurationFileErrorLabel.Text = Resources.PathInvalidError;
        ConfigurationFileErrorPictureBox.Visible = true;
      }

      var errorMessage = Utilities.ValidateFilePath(path);
      if (!string.IsNullOrEmpty(errorMessage))
      {
        ConfigurationFileErrorLabel.Text = errorMessage;
        ConfigurationFileErrorPictureBox.Visible = true;
      }

      if (!File.Exists(iniFilePath)
          && !File.Exists(alternateIniFilePath))
      {
        ConfigurationFileErrorLabel.Text = Resources.ConfigurationFileNotFound;
        ConfigurationFileErrorPictureBox.Visible = true;
      }

      ConfigurationFileErrorLabel.Visible = !string.IsNullOrEmpty(ConfigurationFileErrorLabel.Text);
      OkButton.Enabled = !ConfigurationFileErrorPictureBox.Visible;
    }

    /// <summary>
    /// Handles the OnLoad event.
    /// </summary>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    protected override void OnLoad(EventArgs e)
    {
      Utilities.NormalizeFont(this);
      base.OnLoad(e);
    }

    /// <summary>
    /// Handles the FormClosing event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
    private void ConfigurationFileDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (DialogResult == DialogResult.Cancel)
      {
        return;
      }

      ConfigurationFilePath = ConfigurationFileTextBox.Text.Trim();
    }

    /// <summary>
    /// Handles the Click event for the Browse button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void ConfigurationFileBrowseButton_Click(object sender, EventArgs e)
    {
      using (var folderBrowserDialog = new FolderBrowserDialog())
      {
        folderBrowserDialog.SelectedPath = ConfigurationFileTextBox.Text;
        if (folderBrowserDialog.ShowDialog() == DialogResult.Cancel)
        {
          return;
        }

        ConfigurationFileTextBox.Text = folderBrowserDialog.SelectedPath;
      }

      CheckDirectory();
    }

    /// <summary>
    /// Handles the Click event for the Revert button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void ConfigurationFileRevertButton_Click(object sender, EventArgs e)
    {
      ConfigurationFileTextBox.Clear();
      CheckDirectory();
    }

    /// <summary>
    /// Resets the validations timer.
    /// </summary>
    private void ResetValidationsTimer()
    {
      ValidationsTimer.Stop();
      ValidationsTimer.Start();
    }

    /// <summary>
    /// Handles the TextValidated event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    /// <remarks>This event method is meant to be used with the <see cref="Control.Validated"/> event.</remarks>
    private void ConfigurationFileTextBox_Validated(object sender, EventArgs e)
    {
      CheckDirectory();
    }

    /// <summary>
    /// Handles the TextChanged event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void ConfigurationFileTextBox_TextChangedHandler(object sender, EventArgs e)
    {
      ResetValidationsTimer();
    }

    /// <summary>
    /// Handles the Tick event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void ValidationsTimer_Tick(object sender, EventArgs e)
    {
      var focusedTextBox = this.GetChildControlsOfType<TextBoxBase>().FirstOrDefault(control => control.Focused);
      if (focusedTextBox != null)
      {
        ConfigurationFileTextBox_Validated(focusedTextBox, EventArgs.Empty);
      }
    }
  }
}

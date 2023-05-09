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
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Configurator.Core.Wizard;
using MySql.Configurator.Properties;

namespace MySql.Configurator.Wizards.Server
{
  public partial class ServerConfigLoggingOptionsPage : ConfigWizardPage
  {
    public ServerConfigLoggingOptionsPage(ServerConfigurationController controller)
    {
      InitializeComponent();
      _controller = controller;
      _enabledForeColor = SystemColors.ControlText;
      _disabledForeColor = Color.FromArgb(102, 102, 102);
      ErrorProperties.IconPadding = -20;
    }

    private readonly ServerConfigurationController _controller;
    private MySqlServerSettings Settings => _controller.Settings;
    private readonly Color _enabledForeColor;
    private readonly Color _disabledForeColor;

    public override void Activate()
    {
      ErrorLogFilePathTextBox.Text = Settings.ErrorLogFileName;
      GeneralLogFilePathTextBox.Text = Settings.GeneralQueryLogFileName;
      SlowQueryLogFilePathTextBox.Text = Settings.SlowQueryLogFileName;
      SlowQueryLogSecondsTextBox.Text = Settings.LongQueryTime.ToString();
      BinLogFilePathTextBox.Text = Settings.BinLogFileNameBase;
      GeneralLogCheckBox.Checked = Settings.EnableGeneralLog;
      SlowQueryLogCheckBox.Checked = Settings.EnableSlowQueryLog;
      BinLogCheckBox.Checked = Settings.EnableBinLog;
      SetDefaultOptions();
      FireAllValidations();
      base.Activate();
    }

    public override bool Next()
    {
      Settings.ErrorLogFileName = ErrorLogFilePathTextBox.Text;
      Settings.EnableGeneralLog = GeneralLogCheckBox.Checked;
      if (GeneralLogCheckBox.Checked)
      {
        Settings.GeneralQueryLogFileName = GeneralLogFilePathTextBox.Text;
      }

      Settings.EnableSlowQueryLog = SlowQueryLogCheckBox.Checked;
      if (SlowQueryLogCheckBox.Checked)
      {
        Settings.SlowQueryLogFileName = SlowQueryLogFilePathTextBox.Text;
        Settings.LongQueryTime = int.Parse(SlowQueryLogSecondsTextBox.Text);
      }

      Settings.EnableBinLog = BinLogCheckBox.Checked;
      if (BinLogCheckBox.Checked)
      {
        Settings.BinLogFileNameBase = BinLogFilePathTextBox.Text;
      }

      return base.Next();
    }

    private void GeneralLogCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      var enableGeneralLog = GeneralLogCheckBox.Checked;
      GeneralLogFilePathLabel.ForeColor = enableGeneralLog ? _enabledForeColor : _disabledForeColor;
      GeneralLogFilePathTextBox.Enabled = enableGeneralLog;
      GeneralLogFilePathBrowseButton.Enabled = enableGeneralLog;
      GeneralLogFilePathRevertButton.Enabled = enableGeneralLog;
      ValidatedHandler(GeneralLogFilePathTextBox, EventArgs.Empty);
    }

    private void SlowQueryLogCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      var enableSlowLog = SlowQueryLogCheckBox.Checked;
      SlowQueryLogFilePathLabel.ForeColor = enableSlowLog ? _enabledForeColor : _disabledForeColor;
      SlowQueryLogFilePathTextBox.Enabled = enableSlowLog;
      SlowQueryLogFilePathBrowseButton.Enabled = enableSlowLog;
      SlowQueryLogSecondsLabel.ForeColor = enableSlowLog ? _enabledForeColor : _disabledForeColor;
      SlowQueryLogSecondsTextBox.Enabled = enableSlowLog;
      SlowQueryLogFilePathRevertButton.Enabled = enableSlowLog;
      ValidatedHandler(SlowQueryLogFilePathTextBox, EventArgs.Empty);
      ValidatedHandler(SlowQueryLogFilePathTextBox, EventArgs.Empty);
    }

    private void BinLogCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      var enableBinLog = BinLogCheckBox.Checked;
      BinLogFilePathLabel.ForeColor = enableBinLog ? _enabledForeColor : _disabledForeColor;
      BinLogFilePathTextBox.Enabled = enableBinLog;
      BinLogFilePathBrowseButton.Enabled = enableBinLog;
      BinLogFilePathRevertButton.Enabled = enableBinLog;
      ValidatedHandler(BinLogFilePathTextBox, EventArgs.Empty);
    }

    private void ErrorLogFilePathBrowseButton_Click(object sender, EventArgs e)
    {
      LogsSaveFileDialog.Filter = @"Error Log Files (*.err)|*.err|All files (*.*)|*.*";
      LogsSaveFileDialog.FilterIndex = 1;
      LogsSaveFileDialog.RestoreDirectory = true;
      LogsSaveFileDialog.Title = @"Choose an error log file...";
      if (LogsSaveFileDialog.ShowDialog() == DialogResult.OK)
      {
        ErrorLogFilePathTextBox.Text = LogsSaveFileDialog.FileName;
      }
    }

    private void GeneralLogFilePathBrowseButton_Click(object sender, EventArgs e)
    {
      LogsSaveFileDialog.Filter = @"Log Files (*.log)|*.log|All files (*.*)|*.*";
      LogsSaveFileDialog.FilterIndex = 1;
      LogsSaveFileDialog.RestoreDirectory = true;
      LogsSaveFileDialog.Title = @"Choose a general log file...";
      if (LogsSaveFileDialog.ShowDialog() == DialogResult.OK)
      {
        GeneralLogFilePathTextBox.Text = LogsSaveFileDialog.FileName;
      }
    }

    private void SlowQueryLogFilePathBrowseButton_Click(object sender, EventArgs e)
    {
      LogsSaveFileDialog.Filter = @"Log Files (*.log)|*.log|All files (*.*)|*.*";
      LogsSaveFileDialog.FilterIndex = 1;
      LogsSaveFileDialog.RestoreDirectory = true;
      LogsSaveFileDialog.Title = @"Choose a slow query log file...";
      if (LogsSaveFileDialog.ShowDialog() == DialogResult.OK)
      {
        SlowQueryLogFilePathTextBox.Text = LogsSaveFileDialog.FileName;
      }
    }

    private void BinLogFilePathBrowseButton_Click(object sender, EventArgs e)
    {
      LogsSaveFileDialog.Filter = @"Bin Log Base (*)|*";
      LogsSaveFileDialog.FilterIndex = 1;
      LogsSaveFileDialog.RestoreDirectory = true;
      LogsSaveFileDialog.Title = @"Choose a bin log file...";
      if (LogsSaveFileDialog.ShowDialog() == DialogResult.OK)
      {
        BinLogFilePathTextBox.Text = LogsSaveFileDialog.FileName;
      }
    }

    /// <summary>
    /// Handles the TextChanged event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    protected override void TextChangedHandler(object sender, EventArgs e)
    {
      // Looks like we could get rid of this empty override, but it is necessary to avoid an error of:
      // The method 'xxx' cannot be the method for an event because a class this class derives from already defines the method
      base.TextChangedHandler(sender, e);
    }

    /// <summary>
    /// Handles the TextValidated event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    /// <remarks>This event method is meant to be used with the <see cref="Control.Validated"/> event.</remarks>
    protected override void ValidatedHandler(object sender, EventArgs e)
    {
      // Looks like we could get rid of this empty override, but it is necessary to avoid an error of:
      // The method 'xxx' cannot be the method for an event because a class this class derives from already defines the method
      base.ValidatedHandler(sender, e);
    }

    /// <summary>
    /// Contains calls to methods that validate the given control's value.
    /// </summary>
    /// <returns>An error message or <c>null</c> / <see cref="string.Empty"/> if everything is valid.</returns>
    protected override string ValidateFields()
    {
      string errorMessage = base.ValidateFields();
      if (ErrorProviderControl.Enabled)
      {
        ErrorProviderControl.Text = ErrorProviderControl.Text.Trim();
        switch (ErrorProviderControl.Name)
        {
          case nameof(ErrorLogFilePathTextBox):
          case nameof(GeneralLogFilePathTextBox):
          case nameof(SlowQueryLogFilePathTextBox):
          case nameof(BinLogFilePathTextBox):
            errorMessage = Core.Classes.Utilities.ValidateFilePath(ErrorProviderControl.Text);
            if (string.IsNullOrEmpty(errorMessage)
                && ErrorProviderControl.Name == BinLogFilePathTextBox.Name
                && (Path.HasExtension(ErrorProviderControl.Text)
                    || ErrorProviderControl.Text.EndsWith(".")))
            {
              errorMessage = Resources.BinLogPathExtensionError;
            }

            break;

          case nameof(SlowQueryLogSecondsTextBox):
            errorMessage = ValidateSecondsNumber(SlowQueryLogSecondsTextBox.Text);
            break;
        }
      }

      return errorMessage;
    }

    private string ValidateSecondsNumber(string seconds)
    {
      var messageResult = string.Empty;
      if (!int.TryParse(seconds, out var slowQueryTime))
      {
        messageResult = string.Format(Resources.NotProperValueForInt, seconds);
      }
      else if (slowQueryTime < 0)
      {
        messageResult = string.Format(Resources.MinValueRequired, "0");
      }

      return messageResult;
    }

    private void RevertFileNameButtonClick(object sender, EventArgs e)
    {
      if (!(sender is Button button))
      {
        return;
      }

      switch (button.Name)
      {
        case nameof(ErrorLogFilePathRevertButton):
          ErrorLogFilePathTextBox.Text = MySqlServerSettings.ErrorLogDefaultFileName;
          break;

        case nameof(GeneralLogFilePathRevertButton):
          GeneralLogFilePathTextBox.Text = MySqlServerSettings.GeneralQueryLogDefaultFileName;
          break;

        case nameof(SlowQueryLogFilePathRevertButton):
          SlowQueryLogFilePathTextBox.Text = MySqlServerSettings.SlowQueryLogDefaultFileName;
          break;

        case nameof(BinLogFilePathRevertButton):
          BinLogFilePathTextBox.Text = MySqlServerSettings.BinaryLogDefaultFileName;
          break;
      }
    }

    /// <summary>
    /// Sets the default options for this configuration page.
    /// </summary>
    private void SetDefaultOptions()
    {
      BinLogCheckBox.Enabled = true;
      BinLogCheckBox.Checked = Settings.EnableBinLog;
      BinLogFilePathTextBox.Enabled = Settings.EnableBinLog;
      BinLogFilePathBrowseButton.Enabled = Settings.EnableBinLog;
    }
  }
}
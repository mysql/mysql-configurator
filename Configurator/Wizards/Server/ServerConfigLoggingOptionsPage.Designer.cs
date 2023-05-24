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

namespace MySql.Configurator.Wizards.Server
{
  partial class ServerConfigLoggingOptionsPage
  {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }

      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerConfigLoggingOptionsPage));
      this.LoggingOptionsDescriptionLabel = new System.Windows.Forms.Label();
      this.GeneralLogCheckBox = new System.Windows.Forms.CheckBox();
      this.SlowQueryLogCheckBox = new System.Windows.Forms.CheckBox();
      this.BinLogCheckBox = new System.Windows.Forms.CheckBox();
      this.SlowQueryLogFilePathTextBox = new System.Windows.Forms.TextBox();
      this.GeneralLogFilePathTextBox = new System.Windows.Forms.TextBox();
      this.ErrorLogFilePathTextBox = new System.Windows.Forms.TextBox();
      this.BinLogFilePathTextBox = new System.Windows.Forms.TextBox();
      this.GeneralLogFilePathBrowseButton = new System.Windows.Forms.Button();
      this.SlowQueryLogFilePathBrowseButton = new System.Windows.Forms.Button();
      this.ErrorLogFilePathBrowseButton = new System.Windows.Forms.Button();
      this.BinLogFilePathBrowseButton = new System.Windows.Forms.Button();
      this.ErrorLogLabel = new System.Windows.Forms.Label();
      this.GeneralLogDescriptionLabel = new System.Windows.Forms.Label();
      this.SlowQueryLogDescriptionLabel = new System.Windows.Forms.Label();
      this.BinLogDescriptionLabel = new System.Windows.Forms.Label();
      this.GeneralLogFilePathLabel = new System.Windows.Forms.Label();
      this.SlowQueryLogFilePathLabel = new System.Windows.Forms.Label();
      this.BinLogFilePathLabel = new System.Windows.Forms.Label();
      this.SlowQueryLogSecondsLabel = new System.Windows.Forms.Label();
      this.SlowQueryLogSecondsTextBox = new System.Windows.Forms.TextBox();
      this.LogsSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
      this.ErrorLogFilePathRevertButton = new System.Windows.Forms.Button();
      this.GeneralLogFilePathRevertButton = new System.Windows.Forms.Button();
      this.SlowQueryLogFilePathRevertButton = new System.Windows.Forms.Button();
      this.BinLogFilePathRevertButton = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // subCaptionLabel
      // 
      this.subCaptionLabel.Location = new System.Drawing.Point(38, 74);
      this.subCaptionLabel.Size = new System.Drawing.Size(440, 15);
      this.subCaptionLabel.Text = "Please use this dialog to specify advanced configuration options.";
      this.subCaptionLabel.Visible = false;
      // 
      // captionLabel
      // 
      this.captionLabel.Size = new System.Drawing.Size(106, 17);
      this.captionLabel.Text = "Logging Options";
      // 
      // LoggingOptionsDescriptionLabel
      // 
      this.LoggingOptionsDescriptionLabel.AccessibleDescription = "A label displaying an explanatory text about all logging options";
      this.LoggingOptionsDescriptionLabel.AccessibleName = "Logging Options Description";
      this.LoggingOptionsDescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.LoggingOptionsDescriptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.LoggingOptionsDescriptionLabel.Location = new System.Drawing.Point(27, 63);
      this.LoggingOptionsDescriptionLabel.Name = "LoggingOptionsDescriptionLabel";
      this.LoggingOptionsDescriptionLabel.Size = new System.Drawing.Size(520, 81);
      this.LoggingOptionsDescriptionLabel.TabIndex = 2;
      this.LoggingOptionsDescriptionLabel.Text = resources.GetString("LoggingOptionsDescriptionLabel.Text");
      // 
      // GeneralLogCheckBox
      // 
      this.GeneralLogCheckBox.AccessibleDescription = "A check box to enable the general query log";
      this.GeneralLogCheckBox.AccessibleName = "Enable General Log";
      this.GeneralLogCheckBox.AutoSize = true;
      this.GeneralLogCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.GeneralLogCheckBox.Location = new System.Drawing.Point(33, 196);
      this.GeneralLogCheckBox.Name = "GeneralLogCheckBox";
      this.GeneralLogCheckBox.Size = new System.Drawing.Size(93, 19);
      this.GeneralLogCheckBox.TabIndex = 7;
      this.GeneralLogCheckBox.Text = "General Log";
      this.GeneralLogCheckBox.UseVisualStyleBackColor = true;
      this.GeneralLogCheckBox.CheckedChanged += new System.EventHandler(this.GeneralLogCheckBox_CheckedChanged);
      // 
      // SlowQueryLogCheckBox
      // 
      this.SlowQueryLogCheckBox.AccessibleDescription = "A check box to enable the slow query log";
      this.SlowQueryLogCheckBox.AccessibleName = "Enable Slow Query Log";
      this.SlowQueryLogCheckBox.AutoSize = true;
      this.SlowQueryLogCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.SlowQueryLogCheckBox.Location = new System.Drawing.Point(33, 294);
      this.SlowQueryLogCheckBox.Name = "SlowQueryLogCheckBox";
      this.SlowQueryLogCheckBox.Size = new System.Drawing.Size(113, 19);
      this.SlowQueryLogCheckBox.TabIndex = 13;
      this.SlowQueryLogCheckBox.Text = "Slow Query Log";
      this.SlowQueryLogCheckBox.UseVisualStyleBackColor = true;
      this.SlowQueryLogCheckBox.CheckedChanged += new System.EventHandler(this.SlowQueryLogCheckBox_CheckedChanged);
      // 
      // BinLogCheckBox
      // 
      this.BinLogCheckBox.AccessibleDescription = "A check box to enable the binary log";
      this.BinLogCheckBox.AccessibleName = "Enable Binary Log";
      this.BinLogCheckBox.AutoSize = true;
      this.BinLogCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.BinLogCheckBox.Location = new System.Drawing.Point(33, 393);
      this.BinLogCheckBox.Name = "BinLogCheckBox";
      this.BinLogCheckBox.Size = new System.Drawing.Size(130, 29);
      this.BinLogCheckBox.TabIndex = 21;
      this.BinLogCheckBox.Text = "Binary Log";
      this.BinLogCheckBox.UseVisualStyleBackColor = true;
      this.BinLogCheckBox.CheckedChanged += new System.EventHandler(this.BinLogCheckBox_CheckedChanged);
      // 
      // SlowQueryLogFilePathTextBox
      // 
      this.SlowQueryLogFilePathTextBox.AccessibleDescription = "A text box to input the slow query log file path";
      this.SlowQueryLogFilePathTextBox.AccessibleName = "Slow Query Log File Path";
      this.SlowQueryLogFilePathTextBox.Enabled = false;
      this.SlowQueryLogFilePathTextBox.Location = new System.Drawing.Point(97, 355);
      this.SlowQueryLogFilePathTextBox.Name = "SlowQueryLogFilePathTextBox";
      this.SlowQueryLogFilePathTextBox.Size = new System.Drawing.Size(284, 23);
      this.SlowQueryLogFilePathTextBox.TabIndex = 16;
      this.SlowQueryLogFilePathTextBox.TextChanged += new System.EventHandler(this.TextChangedHandler);
      this.SlowQueryLogFilePathTextBox.Validated += new System.EventHandler(this.ValidatedHandler);
      // 
      // GeneralLogFilePathTextBox
      // 
      this.GeneralLogFilePathTextBox.AccessibleDescription = "A text box to input the general log file path";
      this.GeneralLogFilePathTextBox.AccessibleName = "General Log File Path";
      this.GeneralLogFilePathTextBox.Enabled = false;
      this.GeneralLogFilePathTextBox.Location = new System.Drawing.Point(97, 256);
      this.GeneralLogFilePathTextBox.Name = "GeneralLogFilePathTextBox";
      this.GeneralLogFilePathTextBox.Size = new System.Drawing.Size(396, 23);
      this.GeneralLogFilePathTextBox.TabIndex = 10;
      this.GeneralLogFilePathTextBox.TextChanged += new System.EventHandler(this.TextChangedHandler);
      this.GeneralLogFilePathTextBox.Validated += new System.EventHandler(this.ValidatedHandler);
      // 
      // ErrorLogFilePathTextBox
      // 
      this.ErrorLogFilePathTextBox.AccessibleDescription = "A text box to input the error log file path";
      this.ErrorLogFilePathTextBox.AccessibleName = "Error Log File Path";
      this.ErrorLogFilePathTextBox.Location = new System.Drawing.Point(97, 158);
      this.ErrorLogFilePathTextBox.Name = "ErrorLogFilePathTextBox";
      this.ErrorLogFilePathTextBox.Size = new System.Drawing.Size(397, 23);
      this.ErrorLogFilePathTextBox.TabIndex = 4;
      this.ErrorLogFilePathTextBox.TextChanged += new System.EventHandler(this.TextChangedHandler);
      this.ErrorLogFilePathTextBox.Validated += new System.EventHandler(this.ValidatedHandler);
      // 
      // BinLogFilePathTextBox
      // 
      this.BinLogFilePathTextBox.AccessibleDescription = "A text box to input the binary log file path";
      this.BinLogFilePathTextBox.AccessibleName = "Binary Log File Path";
      this.BinLogFilePathTextBox.Enabled = false;
      this.BinLogFilePathTextBox.Location = new System.Drawing.Point(97, 469);
      this.BinLogFilePathTextBox.Name = "BinLogFilePathTextBox";
      this.BinLogFilePathTextBox.Size = new System.Drawing.Size(396, 23);
      this.BinLogFilePathTextBox.TabIndex = 24;
      this.BinLogFilePathTextBox.TextChanged += new System.EventHandler(this.TextChangedHandler);
      this.BinLogFilePathTextBox.Validated += new System.EventHandler(this.ValidatedHandler);
      // 
      // GeneralLogFilePathBrowseButton
      // 
      this.GeneralLogFilePathBrowseButton.AccessibleDescription = "A button  to browse the file system for a log file";
      this.GeneralLogFilePathBrowseButton.AccessibleName = "General Log File Path Browse";
      this.GeneralLogFilePathBrowseButton.Enabled = false;
      this.GeneralLogFilePathBrowseButton.Location = new System.Drawing.Point(520, 254);
      this.GeneralLogFilePathBrowseButton.Name = "GeneralLogFilePathBrowseButton";
      this.GeneralLogFilePathBrowseButton.Size = new System.Drawing.Size(27, 22);
      this.GeneralLogFilePathBrowseButton.TabIndex = 12;
      this.GeneralLogFilePathBrowseButton.Text = "...";
      this.GeneralLogFilePathBrowseButton.UseVisualStyleBackColor = true;
      this.GeneralLogFilePathBrowseButton.Click += new System.EventHandler(this.GeneralLogFilePathBrowseButton_Click);
      // 
      // SlowQueryLogFilePathBrowseButton
      // 
      this.SlowQueryLogFilePathBrowseButton.AccessibleDescription = "A button  to browse the file system for a log file";
      this.SlowQueryLogFilePathBrowseButton.AccessibleName = "Slow Query Log File Path Browse";
      this.SlowQueryLogFilePathBrowseButton.Enabled = false;
      this.SlowQueryLogFilePathBrowseButton.Location = new System.Drawing.Point(408, 355);
      this.SlowQueryLogFilePathBrowseButton.Name = "SlowQueryLogFilePathBrowseButton";
      this.SlowQueryLogFilePathBrowseButton.Size = new System.Drawing.Size(27, 22);
      this.SlowQueryLogFilePathBrowseButton.TabIndex = 18;
      this.SlowQueryLogFilePathBrowseButton.Text = "...";
      this.SlowQueryLogFilePathBrowseButton.UseVisualStyleBackColor = true;
      this.SlowQueryLogFilePathBrowseButton.Click += new System.EventHandler(this.SlowQueryLogFilePathBrowseButton_Click);
      // 
      // ErrorLogFilePathBrowseButton
      // 
      this.ErrorLogFilePathBrowseButton.AccessibleDescription = "A button  to browse the file system for a log file";
      this.ErrorLogFilePathBrowseButton.AccessibleName = "Error Log File Path Browse";
      this.ErrorLogFilePathBrowseButton.Location = new System.Drawing.Point(520, 157);
      this.ErrorLogFilePathBrowseButton.Name = "ErrorLogFilePathBrowseButton";
      this.ErrorLogFilePathBrowseButton.Size = new System.Drawing.Size(27, 22);
      this.ErrorLogFilePathBrowseButton.TabIndex = 6;
      this.ErrorLogFilePathBrowseButton.Text = "...";
      this.ErrorLogFilePathBrowseButton.UseVisualStyleBackColor = true;
      this.ErrorLogFilePathBrowseButton.Click += new System.EventHandler(this.ErrorLogFilePathBrowseButton_Click);
      // 
      // BinLogFilePathBrowseButton
      // 
      this.BinLogFilePathBrowseButton.AccessibleDescription = "A button  to browse the file system for a log file";
      this.BinLogFilePathBrowseButton.AccessibleName = "Binary Log File Path Browse";
      this.BinLogFilePathBrowseButton.Enabled = false;
      this.BinLogFilePathBrowseButton.Location = new System.Drawing.Point(520, 468);
      this.BinLogFilePathBrowseButton.Name = "BinLogFilePathBrowseButton";
      this.BinLogFilePathBrowseButton.Size = new System.Drawing.Size(27, 22);
      this.BinLogFilePathBrowseButton.TabIndex = 26;
      this.BinLogFilePathBrowseButton.Text = "...";
      this.BinLogFilePathBrowseButton.UseVisualStyleBackColor = true;
      this.BinLogFilePathBrowseButton.Click += new System.EventHandler(this.BinLogFilePathBrowseButton_Click);
      // 
      // ErrorLogLabel
      // 
      this.ErrorLogLabel.AccessibleDescription = "A label displaying the text error log";
      this.ErrorLogLabel.AccessibleName = "Error Log Text";
      this.ErrorLogLabel.AutoSize = true;
      this.ErrorLogLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.ErrorLogLabel.Location = new System.Drawing.Point(30, 162);
      this.ErrorLogLabel.Name = "ErrorLogLabel";
      this.ErrorLogLabel.Size = new System.Drawing.Size(61, 15);
      this.ErrorLogLabel.TabIndex = 3;
      this.ErrorLogLabel.Text = "Error Log:";
      // 
      // GeneralLogDescriptionLabel
      // 
      this.GeneralLogDescriptionLabel.AccessibleDescription = "A label displaying an explanatory text about the general log option";
      this.GeneralLogDescriptionLabel.AccessibleName = "General Log Description";
      this.GeneralLogDescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.GeneralLogDescriptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.GeneralLogDescriptionLabel.Location = new System.Drawing.Point(30, 218);
      this.GeneralLogDescriptionLabel.Name = "GeneralLogDescriptionLabel";
      this.GeneralLogDescriptionLabel.Size = new System.Drawing.Size(496, 33);
      this.GeneralLogDescriptionLabel.TabIndex = 8;
      this.GeneralLogDescriptionLabel.Text = "The general query log is a general record of what the MySQL Server is doing. \r\nIt" +
    " should only be used to track down issues.";
      // 
      // SlowQueryLogDescriptionLabel
      // 
      this.SlowQueryLogDescriptionLabel.AccessibleDescription = "A label displaying an explanatory text about the slow query log option";
      this.SlowQueryLogDescriptionLabel.AccessibleName = "Slow Query Log Description";
      this.SlowQueryLogDescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.SlowQueryLogDescriptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.SlowQueryLogDescriptionLabel.Location = new System.Drawing.Point(30, 316);
      this.SlowQueryLogDescriptionLabel.Name = "SlowQueryLogDescriptionLabel";
      this.SlowQueryLogDescriptionLabel.Size = new System.Drawing.Size(496, 33);
      this.SlowQueryLogDescriptionLabel.TabIndex = 14;
      this.SlowQueryLogDescriptionLabel.Text = "The slow query log consists of SQL statements that took more than the given value" +
    " of seconds to execute. It is recommended to turn this log on.";
      // 
      // BinLogDescriptionLabel
      // 
      this.BinLogDescriptionLabel.AccessibleDescription = "A label displaying an explanatory text about the binary log option";
      this.BinLogDescriptionLabel.AccessibleName = "Binary Log Description";
      this.BinLogDescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.BinLogDescriptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.BinLogDescriptionLabel.Location = new System.Drawing.Point(31, 415);
      this.BinLogDescriptionLabel.Name = "BinLogDescriptionLabel";
      this.BinLogDescriptionLabel.Size = new System.Drawing.Size(496, 48);
      this.BinLogDescriptionLabel.TabIndex = 22;
      this.BinLogDescriptionLabel.Text = resources.GetString("BinLogDescriptionLabel.Text");
      // 
      // GeneralLogFilePathLabel
      // 
      this.GeneralLogFilePathLabel.AccessibleDescription = "A label displaying the text file path";
      this.GeneralLogFilePathLabel.AccessibleName = "General Log File Path Text";
      this.GeneralLogFilePathLabel.AutoSize = true;
      this.GeneralLogFilePathLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.GeneralLogFilePathLabel.Location = new System.Drawing.Point(29, 261);
      this.GeneralLogFilePathLabel.Name = "GeneralLogFilePathLabel";
      this.GeneralLogFilePathLabel.Size = new System.Drawing.Size(55, 15);
      this.GeneralLogFilePathLabel.TabIndex = 9;
      this.GeneralLogFilePathLabel.Text = "File Path:";
      // 
      // SlowQueryLogFilePathLabel
      // 
      this.SlowQueryLogFilePathLabel.AccessibleDescription = "A label displaying the text file path";
      this.SlowQueryLogFilePathLabel.AccessibleName = "Slow Query Log File Path Text";
      this.SlowQueryLogFilePathLabel.AutoSize = true;
      this.SlowQueryLogFilePathLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.SlowQueryLogFilePathLabel.Location = new System.Drawing.Point(29, 360);
      this.SlowQueryLogFilePathLabel.Name = "SlowQueryLogFilePathLabel";
      this.SlowQueryLogFilePathLabel.Size = new System.Drawing.Size(55, 15);
      this.SlowQueryLogFilePathLabel.TabIndex = 15;
      this.SlowQueryLogFilePathLabel.Text = "File Path:";
      // 
      // BinLogFilePathLabel
      // 
      this.BinLogFilePathLabel.AccessibleDescription = "A label displaying the text file path";
      this.BinLogFilePathLabel.AccessibleName = "Binary Log File Path Text";
      this.BinLogFilePathLabel.AutoSize = true;
      this.BinLogFilePathLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.BinLogFilePathLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.BinLogFilePathLabel.Location = new System.Drawing.Point(30, 472);
      this.BinLogFilePathLabel.Name = "BinLogFilePathLabel";
      this.BinLogFilePathLabel.Size = new System.Drawing.Size(55, 15);
      this.BinLogFilePathLabel.TabIndex = 23;
      this.BinLogFilePathLabel.Text = "File Path:";
      // 
      // SlowQueryLogSecondsLabel
      // 
      this.SlowQueryLogSecondsLabel.AccessibleDescription = "A label displaying the text seconds";
      this.SlowQueryLogSecondsLabel.AccessibleName = "Slow Query Seconds Text";
      this.SlowQueryLogSecondsLabel.AutoSize = true;
      this.SlowQueryLogSecondsLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.SlowQueryLogSecondsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.SlowQueryLogSecondsLabel.Location = new System.Drawing.Point(441, 360);
      this.SlowQueryLogSecondsLabel.Name = "SlowQueryLogSecondsLabel";
      this.SlowQueryLogSecondsLabel.Size = new System.Drawing.Size(54, 15);
      this.SlowQueryLogSecondsLabel.TabIndex = 19;
      this.SlowQueryLogSecondsLabel.Text = "Seconds:";
      // 
      // SlowQueryLogSecondsTextBox
      // 
      this.SlowQueryLogSecondsTextBox.AccessibleDescription = "A text box to input the number of seconds before triggering a slow query entry";
      this.SlowQueryLogSecondsTextBox.AccessibleName = "Slow Query Seconds";
      this.SlowQueryLogSecondsTextBox.Enabled = false;
      this.SlowQueryLogSecondsTextBox.Location = new System.Drawing.Point(500, 355);
      this.SlowQueryLogSecondsTextBox.Name = "SlowQueryLogSecondsTextBox";
      this.SlowQueryLogSecondsTextBox.Size = new System.Drawing.Size(47, 23);
      this.SlowQueryLogSecondsTextBox.TabIndex = 20;
      this.SlowQueryLogSecondsTextBox.TextChanged += new System.EventHandler(this.TextChangedHandler);
      this.SlowQueryLogSecondsTextBox.Validated += new System.EventHandler(this.ValidatedHandler);
      // 
      // ErrorLogFilePathRevertButton
      // 
      this.ErrorLogFilePathRevertButton.AccessibleDescription = "A button to revert the value of the error log file path to its default value";
      this.ErrorLogFilePathRevertButton.AccessibleName = "Error Log File Path Revert";
      this.ErrorLogFilePathRevertButton.BackColor = System.Drawing.Color.Transparent;
      this.ErrorLogFilePathRevertButton.BackgroundImage = global::MySql.Configurator.Properties.Resources.Revert;
      this.ErrorLogFilePathRevertButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
      this.ErrorLogFilePathRevertButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.ErrorLogFilePathRevertButton.ForeColor = System.Drawing.Color.Transparent;
      this.ErrorLogFilePathRevertButton.Location = new System.Drawing.Point(500, 159);
      this.ErrorLogFilePathRevertButton.Name = "ErrorLogFilePathRevertButton";
      this.ErrorLogFilePathRevertButton.Size = new System.Drawing.Size(16, 22);
      this.ErrorLogFilePathRevertButton.TabIndex = 5;
      this.ErrorLogFilePathRevertButton.UseVisualStyleBackColor = false;
      this.ErrorLogFilePathRevertButton.Click += new System.EventHandler(this.RevertFileNameButtonClick);
      // 
      // GeneralLogFilePathRevertButton
      // 
      this.GeneralLogFilePathRevertButton.AccessibleDescription = "A button to revert the value of the general log file path to its default value";
      this.GeneralLogFilePathRevertButton.AccessibleName = "General Log File Path Revert";
      this.GeneralLogFilePathRevertButton.BackColor = System.Drawing.Color.Transparent;
      this.GeneralLogFilePathRevertButton.BackgroundImage = global::MySql.Configurator.Properties.Resources.Revert;
      this.GeneralLogFilePathRevertButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
      this.GeneralLogFilePathRevertButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.GeneralLogFilePathRevertButton.ForeColor = System.Drawing.Color.Transparent;
      this.GeneralLogFilePathRevertButton.Location = new System.Drawing.Point(499, 256);
      this.GeneralLogFilePathRevertButton.Name = "GeneralLogFilePathRevertButton";
      this.GeneralLogFilePathRevertButton.Size = new System.Drawing.Size(16, 22);
      this.GeneralLogFilePathRevertButton.TabIndex = 11;
      this.GeneralLogFilePathRevertButton.UseVisualStyleBackColor = false;
      this.GeneralLogFilePathRevertButton.Click += new System.EventHandler(this.RevertFileNameButtonClick);
      // 
      // SlowQueryLogFilePathRevertButton
      // 
      this.SlowQueryLogFilePathRevertButton.AccessibleDescription = "A button to revert the value of the slow query log file path to its default value" +
    "";
      this.SlowQueryLogFilePathRevertButton.AccessibleName = "Slow Query Log File Path Revert";
      this.SlowQueryLogFilePathRevertButton.BackColor = System.Drawing.Color.Transparent;
      this.SlowQueryLogFilePathRevertButton.BackgroundImage = global::MySql.Configurator.Properties.Resources.Revert;
      this.SlowQueryLogFilePathRevertButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
      this.SlowQueryLogFilePathRevertButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.SlowQueryLogFilePathRevertButton.ForeColor = System.Drawing.Color.Transparent;
      this.SlowQueryLogFilePathRevertButton.Location = new System.Drawing.Point(387, 356);
      this.SlowQueryLogFilePathRevertButton.Name = "SlowQueryLogFilePathRevertButton";
      this.SlowQueryLogFilePathRevertButton.Size = new System.Drawing.Size(16, 22);
      this.SlowQueryLogFilePathRevertButton.TabIndex = 17;
      this.SlowQueryLogFilePathRevertButton.UseVisualStyleBackColor = false;
      this.SlowQueryLogFilePathRevertButton.Click += new System.EventHandler(this.RevertFileNameButtonClick);
      // 
      // BinLogFilePathRevertButton
      // 
      this.BinLogFilePathRevertButton.AccessibleDescription = "A button to revert the value of the binary log file path to its default value";
      this.BinLogFilePathRevertButton.AccessibleName = "Binary Log File Path Revert";
      this.BinLogFilePathRevertButton.BackColor = System.Drawing.Color.Transparent;
      this.BinLogFilePathRevertButton.BackgroundImage = global::MySql.Configurator.Properties.Resources.Revert;
      this.BinLogFilePathRevertButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
      this.BinLogFilePathRevertButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.BinLogFilePathRevertButton.ForeColor = System.Drawing.Color.Transparent;
      this.BinLogFilePathRevertButton.Location = new System.Drawing.Point(499, 470);
      this.BinLogFilePathRevertButton.Name = "BinLogFilePathRevertButton";
      this.BinLogFilePathRevertButton.Size = new System.Drawing.Size(16, 22);
      this.BinLogFilePathRevertButton.TabIndex = 25;
      this.BinLogFilePathRevertButton.UseVisualStyleBackColor = false;
      this.BinLogFilePathRevertButton.Click += new System.EventHandler(this.RevertFileNameButtonClick);
      // 
      // ServerConfigLoggingOptionsPage
      // 
      this.AccessibleDescription = "A configuration wizard page with logging options";
      this.AccessibleName = "Logging Options Page";
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
      this.Caption = "Logging Options";
      this.Controls.Add(this.BinLogFilePathRevertButton);
      this.Controls.Add(this.SlowQueryLogFilePathRevertButton);
      this.Controls.Add(this.GeneralLogFilePathRevertButton);
      this.Controls.Add(this.ErrorLogFilePathRevertButton);
      this.Controls.Add(this.LoggingOptionsDescriptionLabel);
      this.Controls.Add(this.SlowQueryLogSecondsTextBox);
      this.Controls.Add(this.SlowQueryLogSecondsLabel);
      this.Controls.Add(this.BinLogFilePathLabel);
      this.Controls.Add(this.SlowQueryLogFilePathLabel);
      this.Controls.Add(this.GeneralLogFilePathLabel);
      this.Controls.Add(this.BinLogDescriptionLabel);
      this.Controls.Add(this.SlowQueryLogDescriptionLabel);
      this.Controls.Add(this.GeneralLogDescriptionLabel);
      this.Controls.Add(this.ErrorLogLabel);
      this.Controls.Add(this.BinLogFilePathBrowseButton);
      this.Controls.Add(this.ErrorLogFilePathBrowseButton);
      this.Controls.Add(this.SlowQueryLogFilePathBrowseButton);
      this.Controls.Add(this.GeneralLogFilePathBrowseButton);
      this.Controls.Add(this.GeneralLogCheckBox);
      this.Controls.Add(this.SlowQueryLogCheckBox);
      this.Controls.Add(this.GeneralLogFilePathTextBox);
      this.Controls.Add(this.BinLogCheckBox);
      this.Controls.Add(this.ErrorLogFilePathTextBox);
      this.Controls.Add(this.SlowQueryLogFilePathTextBox);
      this.Controls.Add(this.BinLogFilePathTextBox);
      this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "ServerConfigLoggingOptionsPage";
      this.SubCaption = "Please use this dialog to specify advanced configuration options.";
      this.Controls.SetChildIndex(this.BinLogFilePathTextBox, 0);
      this.Controls.SetChildIndex(this.SlowQueryLogFilePathTextBox, 0);
      this.Controls.SetChildIndex(this.ErrorLogFilePathTextBox, 0);
      this.Controls.SetChildIndex(this.BinLogCheckBox, 0);
      this.Controls.SetChildIndex(this.GeneralLogFilePathTextBox, 0);
      this.Controls.SetChildIndex(this.SlowQueryLogCheckBox, 0);
      this.Controls.SetChildIndex(this.GeneralLogCheckBox, 0);
      this.Controls.SetChildIndex(this.GeneralLogFilePathBrowseButton, 0);
      this.Controls.SetChildIndex(this.captionLabel, 0);
      this.Controls.SetChildIndex(this.subCaptionLabel, 0);
      this.Controls.SetChildIndex(this.SlowQueryLogFilePathBrowseButton, 0);
      this.Controls.SetChildIndex(this.ErrorLogFilePathBrowseButton, 0);
      this.Controls.SetChildIndex(this.BinLogFilePathBrowseButton, 0);
      this.Controls.SetChildIndex(this.ErrorLogLabel, 0);
      this.Controls.SetChildIndex(this.GeneralLogDescriptionLabel, 0);
      this.Controls.SetChildIndex(this.SlowQueryLogDescriptionLabel, 0);
      this.Controls.SetChildIndex(this.BinLogDescriptionLabel, 0);
      this.Controls.SetChildIndex(this.GeneralLogFilePathLabel, 0);
      this.Controls.SetChildIndex(this.SlowQueryLogFilePathLabel, 0);
      this.Controls.SetChildIndex(this.BinLogFilePathLabel, 0);
      this.Controls.SetChildIndex(this.SlowQueryLogSecondsLabel, 0);
      this.Controls.SetChildIndex(this.SlowQueryLogSecondsTextBox, 0);
      this.Controls.SetChildIndex(this.LoggingOptionsDescriptionLabel, 0);
      this.Controls.SetChildIndex(this.ErrorLogFilePathRevertButton, 0);
      this.Controls.SetChildIndex(this.GeneralLogFilePathRevertButton, 0);
      this.Controls.SetChildIndex(this.SlowQueryLogFilePathRevertButton, 0);
      this.Controls.SetChildIndex(this.BinLogFilePathRevertButton, 0);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label LoggingOptionsDescriptionLabel;
    private System.Windows.Forms.CheckBox GeneralLogCheckBox;
    private System.Windows.Forms.CheckBox SlowQueryLogCheckBox;
    private System.Windows.Forms.CheckBox BinLogCheckBox;
    private System.Windows.Forms.TextBox SlowQueryLogFilePathTextBox;
    private System.Windows.Forms.TextBox GeneralLogFilePathTextBox;
    private System.Windows.Forms.TextBox ErrorLogFilePathTextBox;
    private System.Windows.Forms.TextBox BinLogFilePathTextBox;
    private System.Windows.Forms.Button GeneralLogFilePathBrowseButton;
    private System.Windows.Forms.Button SlowQueryLogFilePathBrowseButton;
    private System.Windows.Forms.Button ErrorLogFilePathBrowseButton;
    private System.Windows.Forms.Button BinLogFilePathBrowseButton;
    private System.Windows.Forms.Label ErrorLogLabel;
    private System.Windows.Forms.Label GeneralLogDescriptionLabel;
    private System.Windows.Forms.Label SlowQueryLogDescriptionLabel;
    private System.Windows.Forms.Label BinLogDescriptionLabel;
    private System.Windows.Forms.Label GeneralLogFilePathLabel;
    private System.Windows.Forms.Label SlowQueryLogFilePathLabel;
    private System.Windows.Forms.Label BinLogFilePathLabel;
    private System.Windows.Forms.Label SlowQueryLogSecondsLabel;
    private System.Windows.Forms.TextBox SlowQueryLogSecondsTextBox;
    private System.Windows.Forms.SaveFileDialog LogsSaveFileDialog;
    private System.Windows.Forms.Button ErrorLogFilePathRevertButton;
    private System.Windows.Forms.Button BinLogFilePathRevertButton;
    private System.Windows.Forms.Button SlowQueryLogFilePathRevertButton;
    private System.Windows.Forms.Button GeneralLogFilePathRevertButton;
    }
}

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

namespace MySql.Configurator.Dialogs
{
  partial class ConfigurationFileDialog
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
      this.components = new System.ComponentModel.Container();
      this.DialogCancelButton = new System.Windows.Forms.Button();
      this.OkButton = new System.Windows.Forms.Button();
      this.ConfigurationFileErrorPictureBox = new System.Windows.Forms.PictureBox();
      this.ConfigurationFileErrorLabel = new System.Windows.Forms.Label();
      this.ConfigurationFileRevertButton = new System.Windows.Forms.Button();
      this.ConfigurationFileBrowseButton = new System.Windows.Forms.Button();
      this.ConfigurationFilePathLabel = new System.Windows.Forms.Label();
      this.ConfigurationFileTextBox = new System.Windows.Forms.TextBox();
      this.ConfigurationFilePathDescriptionLabel = new System.Windows.Forms.Label();
      this.LogoPictureBox = new System.Windows.Forms.PictureBox();
      this.TitleLabel = new System.Windows.Forms.Label();
      this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
      this.ValidationsErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
      this.ValidationsTimer = new System.Windows.Forms.Timer(this.components);
      this.ContentAreaPanel.SuspendLayout();
      this.CommandAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ConfigurationFileErrorPictureBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).BeginInit();
      this.SuspendLayout();
      // 
      // FootnoteAreaPanel
      // 
      this.FootnoteAreaPanel.Location = new System.Drawing.Point(0, 449);
      this.FootnoteAreaPanel.Size = new System.Drawing.Size(951, 0);
      // 
      // ContentAreaPanel
      // 
      this.ContentAreaPanel.Controls.Add(this.LogoPictureBox);
      this.ContentAreaPanel.Controls.Add(this.TitleLabel);
      this.ContentAreaPanel.Controls.Add(this.ConfigurationFilePathDescriptionLabel);
      this.ContentAreaPanel.Controls.Add(this.ConfigurationFileErrorPictureBox);
      this.ContentAreaPanel.Controls.Add(this.ConfigurationFileErrorLabel);
      this.ContentAreaPanel.Controls.Add(this.ConfigurationFileRevertButton);
      this.ContentAreaPanel.Controls.Add(this.ConfigurationFileBrowseButton);
      this.ContentAreaPanel.Controls.Add(this.ConfigurationFilePathLabel);
      this.ContentAreaPanel.Controls.Add(this.ConfigurationFileTextBox);
      this.ContentAreaPanel.Size = new System.Drawing.Size(505, 220);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.Controls.Add(this.OkButton);
      this.CommandAreaPanel.Controls.Add(this.DialogCancelButton);
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 175);
      this.CommandAreaPanel.Size = new System.Drawing.Size(505, 45);
      // 
      // DialogCancelButton
      // 
      this.DialogCancelButton.AccessibleDescription = "A button to dismiss the dialog";
      this.DialogCancelButton.AccessibleName = "Cancel";
      this.DialogCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.DialogCancelButton.Location = new System.Drawing.Point(418, 10);
      this.DialogCancelButton.Name = "DialogCancelButton";
      this.DialogCancelButton.Size = new System.Drawing.Size(75, 23);
      this.DialogCancelButton.TabIndex = 1;
      this.DialogCancelButton.Text = "Cancel";
      this.DialogCancelButton.UseVisualStyleBackColor = true;
      // 
      // OkButton
      // 
      this.OkButton.AccessibleDescription = "A button to apply the changes done to the advanced install options";
      this.OkButton.AccessibleName = "OK";
      this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.OkButton.Enabled = false;
      this.OkButton.Location = new System.Drawing.Point(337, 10);
      this.OkButton.Name = "OkButton";
      this.OkButton.Size = new System.Drawing.Size(75, 23);
      this.OkButton.TabIndex = 0;
      this.OkButton.Text = "OK";
      this.OkButton.UseVisualStyleBackColor = true;
      // 
      // ConfigurationFileErrorPictureBox
      // 
      this.ConfigurationFileErrorPictureBox.AccessibleDescription = "A picture box displaying an error icon for an invalid configuration file path";
      this.ConfigurationFileErrorPictureBox.AccessibleName = "Configuration File Error Icon";
      this.ConfigurationFileErrorPictureBox.Image = global::MySql.Configurator.Properties.Resources.error_sign;
      this.ConfigurationFileErrorPictureBox.Location = new System.Drawing.Point(15, 145);
      this.ConfigurationFileErrorPictureBox.Name = "ConfigurationFileErrorPictureBox";
      this.ConfigurationFileErrorPictureBox.Size = new System.Drawing.Size(16, 16);
      this.ConfigurationFileErrorPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.ConfigurationFileErrorPictureBox.TabIndex = 57;
      this.ConfigurationFileErrorPictureBox.TabStop = false;
      this.ConfigurationFileErrorPictureBox.Visible = false;
      // 
      // ConfigurationFileErrorLabel
      // 
      this.ConfigurationFileErrorLabel.AccessibleDescription = "A label displaying a message about a warning or error in the configuration file p" +
    "ath validation";
      this.ConfigurationFileErrorLabel.AccessibleName = "Configuration File Validation Text";
      this.ConfigurationFileErrorLabel.AutoSize = true;
      this.ConfigurationFileErrorLabel.Location = new System.Drawing.Point(34, 146);
      this.ConfigurationFileErrorLabel.Name = "ConfigurationFileErrorLabel";
      this.ConfigurationFileErrorLabel.Size = new System.Drawing.Size(0, 25);
      this.ConfigurationFileErrorLabel.TabIndex = 4;
      this.ConfigurationFileErrorLabel.Visible = false;
      // 
      // ConfigurationFileRevertButton
      // 
      this.ConfigurationFileRevertButton.AccessibleDescription = "A button to revert the value of the configuration file path";
      this.ConfigurationFileRevertButton.AccessibleName = "Configuration File Revert";
      this.ConfigurationFileRevertButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.ConfigurationFileRevertButton.BackgroundImage = global::MySql.Configurator.Properties.Resources.Revert;
      this.ConfigurationFileRevertButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
      this.ConfigurationFileRevertButton.FlatAppearance.BorderSize = 0;
      this.ConfigurationFileRevertButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.ConfigurationFileRevertButton.Location = new System.Drawing.Point(460, 99);
      this.ConfigurationFileRevertButton.Margin = new System.Windows.Forms.Padding(0);
      this.ConfigurationFileRevertButton.Name = "ConfigurationFileRevertButton";
      this.ConfigurationFileRevertButton.Size = new System.Drawing.Size(29, 20);
      this.ConfigurationFileRevertButton.TabIndex = 1;
      this.ConfigurationFileRevertButton.UseVisualStyleBackColor = true;
      this.ConfigurationFileRevertButton.Click += new System.EventHandler(this.ConfigurationFileRevertButton_Click);
      // 
      // ConfigurationFileBrowseButton
      // 
      this.ConfigurationFileBrowseButton.AccessibleDescription = "A button to open a dialog to browse through the file system and select the path t" +
    "o the configuration file";
      this.ConfigurationFileBrowseButton.AccessibleName = "Configuration File Browse";
      this.ConfigurationFileBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.ConfigurationFileBrowseButton.Location = new System.Drawing.Point(461, 116);
      this.ConfigurationFileBrowseButton.Name = "ConfigurationFileBrowseButton";
      this.ConfigurationFileBrowseButton.Size = new System.Drawing.Size(29, 22);
      this.ConfigurationFileBrowseButton.TabIndex = 3;
      this.ConfigurationFileBrowseButton.Text = "...";
      this.ConfigurationFileBrowseButton.UseVisualStyleBackColor = true;
      this.ConfigurationFileBrowseButton.Click += new System.EventHandler(this.ConfigurationFileBrowseButton_Click);
      // 
      // ConfigurationFilePathLabel
      // 
      this.ConfigurationFilePathLabel.AccessibleDescription = "A label displaying the text configuration file path";
      this.ConfigurationFilePathLabel.AccessibleName = "Configuration File Path Text";
      this.ConfigurationFilePathLabel.AutoSize = true;
      this.ConfigurationFilePathLabel.Location = new System.Drawing.Point(12, 95);
      this.ConfigurationFilePathLabel.Name = "ConfigurationFilePathLabel";
      this.ConfigurationFilePathLabel.Size = new System.Drawing.Size(195, 25);
      this.ConfigurationFilePathLabel.TabIndex = 0;
      this.ConfigurationFilePathLabel.Text = "Configuration File Path:";
      // 
      // ConfigurationFileTextBox
      // 
      this.ConfigurationFileTextBox.AccessibleDescription = "A text box to set the full absolute path of the server configuration file";
      this.ConfigurationFileTextBox.AccessibleName = "Configuration File Path";
      this.ConfigurationFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.ConfigurationFileTextBox.Location = new System.Drawing.Point(12, 115);
      this.ConfigurationFileTextBox.Name = "ConfigurationFileTextBox";
      this.ConfigurationFileTextBox.Size = new System.Drawing.Size(443, 31);
      this.ConfigurationFileTextBox.TabIndex = 2;
      this.ConfigurationFileTextBox.TextChanged += new System.EventHandler(this.ConfigurationFileTextBox_TextChangedHandler);
      this.ConfigurationFileTextBox.Validated += new System.EventHandler(this.ConfigurationFileTextBox_Validated);
      // 
      // ConfigurationFilePathDescriptionLabel
      // 
      this.ConfigurationFilePathDescriptionLabel.AccessibleDescription = "A label displaying a description about the need to provide the path to the server" +
    " configuration file";
      this.ConfigurationFilePathDescriptionLabel.AccessibleName = "Configuration file path description";
      this.ConfigurationFilePathDescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.ConfigurationFilePathDescriptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.ConfigurationFilePathDescriptionLabel.Location = new System.Drawing.Point(90, 46);
      this.ConfigurationFilePathDescriptionLabel.Name = "ConfigurationFilePathDescriptionLabel";
      this.ConfigurationFilePathDescriptionLabel.Size = new System.Drawing.Size(401, 44);
      this.ConfigurationFilePathDescriptionLabel.TabIndex = 58;
      this.ConfigurationFilePathDescriptionLabel.Text = "The server configuration file (my.ini or my.cnf) was not found. Please specify th" +
    "e location of the file to continue.";
      // 
      // LogoPictureBox
      // 
      this.LogoPictureBox.AccessibleDescription = "A picture box displaying an icon related to the consumer application and the dial" +
    "og\'s purpose";
      this.LogoPictureBox.AccessibleName = "Icon";
      this.LogoPictureBox.Image = global::MySql.Configurator.Properties.Resources.MainLogo;
      this.LogoPictureBox.Location = new System.Drawing.Point(12, 15);
      this.LogoPictureBox.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
      this.LogoPictureBox.Name = "LogoPictureBox";
      this.LogoPictureBox.Size = new System.Drawing.Size(66, 65);
      this.LogoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.LogoPictureBox.TabIndex = 60;
      this.LogoPictureBox.TabStop = false;
      // 
      // TitleLabel
      // 
      this.TitleLabel.AccessibleDescription = "A generic title label";
      this.TitleLabel.AccessibleName = "Title";
      this.TitleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.TitleLabel.AutoEllipsis = true;
      this.TitleLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TitleLabel.ForeColor = System.Drawing.Color.Navy;
      this.TitleLabel.Location = new System.Drawing.Point(91, 15);
      this.TitleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.TitleLabel.Name = "TitleLabel";
      this.TitleLabel.Size = new System.Drawing.Size(402, 31);
      this.TitleLabel.TabIndex = 59;
      this.TitleLabel.Text = "Server Configuration File Not Found";
      // 
      // ValidationsErrorProvider
      // 
      this.ValidationsErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
      this.ValidationsErrorProvider.ContainerControl = this;
      // 
      // ValidationsTimer
      // 
      this.ValidationsTimer.Interval = 800;
      this.ValidationsTimer.Tick += new System.EventHandler(this.ValidationsTimer_Tick);
      // 
      // ConfigurationFileDialog
      // 
      this.AcceptButton = this.OkButton;
      this.AccessibleDescription = "A modal dialog showing a textbox to select the path of the server configuraiton f" +
    "ile";
      this.AccessibleName = "Configuration File Dialog";
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.CancelButton = this.DialogCancelButton;
      this.ClientSize = new System.Drawing.Size(505, 220);
      this.CommandAreaHeight = 45;
      this.CommandAreaVisible = true;
      this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FootnoteAreaHeight = 0;
      this.Name = "ConfigurationFileDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "MySQL Configurator";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConfigurationFileDialog_FormClosing);
      this.ContentAreaPanel.ResumeLayout(false);
      this.ContentAreaPanel.PerformLayout();
      this.CommandAreaPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.ConfigurationFileErrorPictureBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion
    private System.Windows.Forms.Button DialogCancelButton;
    private System.Windows.Forms.PictureBox ConfigurationFileErrorPictureBox;
    private System.Windows.Forms.Label ConfigurationFileErrorLabel;
    private System.Windows.Forms.Button ConfigurationFileRevertButton;
    private System.Windows.Forms.Button ConfigurationFileBrowseButton;
    private System.Windows.Forms.Label ConfigurationFilePathLabel;
    private System.Windows.Forms.TextBox ConfigurationFileTextBox;
    protected System.Windows.Forms.Button OkButton;
    private System.Windows.Forms.Label ConfigurationFilePathDescriptionLabel;
    private System.Windows.Forms.PictureBox LogoPictureBox;
    private System.Windows.Forms.Label TitleLabel;
    protected System.Windows.Forms.ToolTip ToolTip;
    protected System.Windows.Forms.ErrorProvider ValidationsErrorProvider;
    protected System.Windows.Forms.Timer ValidationsTimer;
  }
}

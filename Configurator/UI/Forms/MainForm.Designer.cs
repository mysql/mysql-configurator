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

namespace MySql.Configurator.UI.Forms
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
      this.StatusStrip = new System.Windows.Forms.StatusStrip();
      this.ConfigurationTypeLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.VersionLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.DataDirectoryLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.StatusStrip.SuspendLayout();
      this.SuspendLayout();
      // 
      // StatusStrip
      // 
      this.StatusStrip.AccessibleDescription = "A status bar used to show the current operation";
      this.StatusStrip.AccessibleName = "Status";
      this.StatusStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
      this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ConfigurationTypeLabel,
            this.VersionLabel,
            this.DataDirectoryLabel});
      this.StatusStrip.Location = new System.Drawing.Point(0, 556);
      this.StatusStrip.Name = "StatusStrip";
      this.StatusStrip.Size = new System.Drawing.Size(806, 28);
      this.StatusStrip.TabIndex = 1;
      // 
      // ConfigurationTypeLabel
      // 
      this.ConfigurationTypeLabel.AccessibleDescription = "A label containing the current configuration type";
      this.ConfigurationTypeLabel.AccessibleName = "Configuration Type";
      this.ConfigurationTypeLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ConfigurationTypeLabel.Margin = new System.Windows.Forms.Padding(28, 4, 10, 3);
      this.ConfigurationTypeLabel.Name = "ConfigurationTypeLabel";
      this.ConfigurationTypeLabel.Size = new System.Drawing.Size(0, 21);
      // 
      // VersionLabel
      // 
      this.VersionLabel.AccessibleDescription = "A label containing the version of the server being configured";
      this.VersionLabel.AccessibleName = "Version";
      this.VersionLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.VersionLabel.Margin = new System.Windows.Forms.Padding(10, 4, 10, 3);
      this.VersionLabel.Name = "VersionLabel";
      this.VersionLabel.Size = new System.Drawing.Size(0, 21);
      // 
      // DataDirectoryLabel
      // 
      this.DataDirectoryLabel.AccessibleDescription = "A label containing the path to the current data directory";
      this.DataDirectoryLabel.AccessibleName = "Data Directory";
      this.DataDirectoryLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DataDirectoryLabel.Margin = new System.Windows.Forms.Padding(10, 4, 10, 3);
      this.DataDirectoryLabel.Name = "DataDirectoryLabel";
      this.DataDirectoryLabel.Size = new System.Drawing.Size(0, 21);
      // 
      // MainForm
      // 
      this.AccessibleDescription = "The main application form";
      this.AccessibleName = "MySQL Configurator";
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.ClientSize = new System.Drawing.Size(786, 584);
      this.Controls.Add(this.StatusStrip);
      this.DoubleBuffered = true;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Icon = global::MySql.Configurator.Properties.Resources.mysql_server;
      this.MaximizeBox = false;
      this.Name = "MainForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "MySQL Configurator";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
      this.Shown += new System.EventHandler(this.MainForm_Shown);
      this.StatusStrip.ResumeLayout(false);
      this.StatusStrip.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

        }

    #endregion

    public System.Windows.Forms.StatusStrip StatusStrip;
    public System.Windows.Forms.ToolStripStatusLabel ConfigurationTypeLabel;
    public System.Windows.Forms.ToolStripStatusLabel VersionLabel;
    public System.Windows.Forms.ToolStripStatusLabel DataDirectoryLabel;
  }
}

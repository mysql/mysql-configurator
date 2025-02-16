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

namespace MySql.Configurator.UI.Controls
{
  partial class InstallWizardSideBarControl
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
      this.ProductAndVersionPanel = new System.Windows.Forms.Panel();
      this.ProductAndVersionLabel = new System.Windows.Forms.Label();
      this.LogoPictureBox = new System.Windows.Forms.PictureBox();
      this.ProductAndVersionPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // ProductAndVersionPanel
      // 
      this.ProductAndVersionPanel.AccessibleDescription = "A panel containing a label showing the current product in the wizard and its vers" +
    "ion";
      this.ProductAndVersionPanel.AccessibleName = "Current Product And Version";
      this.ProductAndVersionPanel.BackColor = System.Drawing.Color.Transparent;
      this.ProductAndVersionPanel.Controls.Add(this.ProductAndVersionLabel);
      this.ProductAndVersionPanel.Location = new System.Drawing.Point(3, 73);
      this.ProductAndVersionPanel.Name = "ProductAndVersionPanel";
      this.ProductAndVersionPanel.Size = new System.Drawing.Size(213, 31);
      this.ProductAndVersionPanel.TabIndex = 2;
      this.ProductAndVersionPanel.Visible = false;
      // 
      // ProductAndVersionLabel
      // 
      this.ProductAndVersionLabel.AccessibleDescription = "A label showing the current product in the wizard and its version";
      this.ProductAndVersionLabel.AccessibleName = "Current Product And Version";
      this.ProductAndVersionLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ProductAndVersionLabel.ForeColor = System.Drawing.Color.White;
      this.ProductAndVersionLabel.Location = new System.Drawing.Point(0, -2);
      this.ProductAndVersionLabel.Name = "ProductAndVersionLabel";
      this.ProductAndVersionLabel.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
      this.ProductAndVersionLabel.Size = new System.Drawing.Size(213, 25);
      this.ProductAndVersionLabel.TabIndex = 1;
      this.ProductAndVersionLabel.Text = "MySQL Server 5.6.14";
      this.ProductAndVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // LogoPictureBox
      // 
      this.LogoPictureBox.Image = global::MySql.Configurator.Properties.Resources.TransparentSakilaLogo;
      this.LogoPictureBox.InitialImage = null;
      this.LogoPictureBox.Location = new System.Drawing.Point(29, 8);
      this.LogoPictureBox.Name = "LogoPictureBox";
      this.LogoPictureBox.Size = new System.Drawing.Size(50, 48);
      this.LogoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
      this.LogoPictureBox.TabIndex = 3;
      this.LogoPictureBox.TabStop = false;
      // 
      // InstallWizardSideBarControl
      // 
      this.AccessibleDescription = "A side bar at the left of a wizard that shows information about a current product" +
    " and the flow of wizard pages";
      this.AccessibleName = "Wizard Side Bar";
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
      this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
      this.Controls.Add(this.LogoPictureBox);
      this.Controls.Add(this.ProductAndVersionPanel);
      this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "InstallWizardSideBarControl";
      this.Size = new System.Drawing.Size(219, 562);
      this.ProductAndVersionPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel ProductAndVersionPanel;
    private System.Windows.Forms.Label ProductAndVersionLabel;
    private System.Windows.Forms.PictureBox LogoPictureBox;
  }
}

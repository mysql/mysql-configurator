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
  partial class ConfigStepControl
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
      this.StepIconPictureBox = new System.Windows.Forms.PictureBox();
      this.StepCaption = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.StepIconPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // StepIconPictureBox
      // 
      this.StepIconPictureBox.AccessibleDescription = "A picture box with a bullet icon";
      this.StepIconPictureBox.AccessibleName = "Step Bullet";
      this.StepIconPictureBox.Image = global::MySql.Configurator.Properties.Resources.ActionOpen;
      this.StepIconPictureBox.Location = new System.Drawing.Point(15, 6);
      this.StepIconPictureBox.Name = "StepIconPictureBox";
      this.StepIconPictureBox.Size = new System.Drawing.Size(16, 16);
      this.StepIconPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.StepIconPictureBox.TabIndex = 0;
      this.StepIconPictureBox.TabStop = false;
      // 
      // StepCaption
      // 
      this.StepCaption.AccessibleDescription = "A label displaying the configuration step caption text";
      this.StepCaption.AccessibleName = "Step Caption";
      this.StepCaption.AutoSize = true;
      this.StepCaption.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.StepCaption.Location = new System.Drawing.Point(37, 7);
      this.StepCaption.Name = "StepCaption";
      this.StepCaption.Size = new System.Drawing.Size(38, 15);
      this.StepCaption.TabIndex = 1;
      this.StepCaption.Text = "label1";
      // 
      // ConfigStepControl
      // 
      this.AccessibleDescription = "A user control representing a configuration step";
      this.AccessibleName = "Configuration Step Control";
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.Controls.Add(this.StepCaption);
      this.Controls.Add(this.StepIconPictureBox);
      this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Margin = new System.Windows.Forms.Padding(0);
      this.Name = "ConfigStepControl";
      this.Size = new System.Drawing.Size(444, 28);
      ((System.ComponentModel.ISupportInitialize)(this.StepIconPictureBox)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    public System.Windows.Forms.PictureBox StepIconPictureBox;
    public System.Windows.Forms.Label StepCaption;
  }
}

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

namespace MySql.Configurator.Wizards.ConfigureWizard
{
  partial class ConfigureErrorPage
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigureErrorPage));
      this.label1 = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).BeginInit();
      this.SuspendLayout();
      // 
      // subCaptionLabel
      // 
      this.subCaptionLabel.Location = new System.Drawing.Point(30, 54);
      this.subCaptionLabel.Text = "";
      // 
      // captionLabel
      // 
      this.captionLabel.Size = new System.Drawing.Size(154, 25);
      this.captionLabel.Text = "Configure Server";
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(23, 99);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(519, 209);
      this.label1.TabIndex = 2;
      this.label1.Text = resources.GetString("label1.Text");
      // 
      // ConfigureErrorPage
      // 
      this.Caption = "Configure Server";
      this.Controls.Add(this.label1);
      this.Name = "ConfigureErrorPage";
      this.SubCaption = "";
      this.Controls.SetChildIndex(this.captionLabel, 0);
      this.Controls.SetChildIndex(this.subCaptionLabel, 0);
      this.Controls.SetChildIndex(this.label1, 0);
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
  }
}

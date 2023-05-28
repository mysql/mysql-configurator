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

namespace MySql.Configurator.Wizards.RemoveWizard
{
  partial class RemoveErrorPage
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
      this.captionLabel.Size = new System.Drawing.Size(136, 25);
      this.captionLabel.Text = "Remove Server";
      // 
      // label1
      // 
      this.label1.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.label1.Location = new System.Drawing.Point(23, 99);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(527, 108);
      this.label1.TabIndex = 2;
      this.label1.Text = "Oops! Looks like there is nothing to remove.\r\n\r\nMySQL Configurator was not able t" +
    "o find any configurations for the specified MySQL Server installation.";
      // 
      // RemoveErrorPage
      // 
      this.Caption = "Remove Server";
      this.Controls.Add(this.label1);
      this.Name = "RemoveErrorPage";
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

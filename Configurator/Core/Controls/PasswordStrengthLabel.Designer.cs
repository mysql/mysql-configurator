/* Copyright (c) 2014, 2023, Oracle and/or its affiliates.

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

namespace MySql.Configurator.Core.Controls
{
  partial class PasswordStrengthLabel
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
      this.TitleLabel = new System.Windows.Forms.Label();
      this.ValueLabel = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // TitleLabel
      // 
      this.TitleLabel.AutoSize = true;
      this.TitleLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TitleLabel.Location = new System.Drawing.Point(0, 0);
      this.TitleLabel.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
      this.TitleLabel.Name = "TitleLabel";
      this.TitleLabel.Size = new System.Drawing.Size(106, 13);
      this.TitleLabel.TabIndex = 29;
      this.TitleLabel.Text = "Password strength:";
      // 
      // ValueLabel
      // 
      this.ValueLabel.AutoSize = true;
      this.ValueLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ValueLabel.Location = new System.Drawing.Point(62, 0);
      this.ValueLabel.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
      this.ValueLabel.Name = "ValueLabel";
      this.ValueLabel.Size = new System.Drawing.Size(36, 13);
      this.ValueLabel.TabIndex = 30;
      this.ValueLabel.Text = "Max";
      // 
      // PasswordStrengthLabel
      // 
      this.Controls.Add(this.TitleLabel);
      this.Controls.Add(this.ValueLabel);
      this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Size = new System.Drawing.Size(200, 13);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label TitleLabel;
    private System.Windows.Forms.Label ValueLabel;
  }
}

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

namespace MySql.Configurator.Wizards.Common
{
  partial class WelcomePage
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
      this.WelcomeWizardLabel = new System.Windows.Forms.Label();
      this.ClickNextLabel = new System.Windows.Forms.Label();
      this.WelcomeBackPictureBox = new System.Windows.Forms.PictureBox();
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.WelcomeBackPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // subCaptionLabel
      // 
      this.subCaptionLabel.Location = new System.Drawing.Point(23, 80);
      this.subCaptionLabel.Text = "";
      // 
      // captionLabel
      // 
      this.captionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
      this.captionLabel.Location = new System.Drawing.Point(136, 29);
      this.captionLabel.Size = new System.Drawing.Size(266, 25);
      this.captionLabel.Text = "Welcome to the MySQL Server";
      this.captionLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // WelcomeWizardLabel
      // 
      this.WelcomeWizardLabel.AccessibleDescription = "A label displaying the purpose of the wizard page";
      this.WelcomeWizardLabel.AccessibleName = "Welcome Help Text";
      this.WelcomeWizardLabel.AutoSize = true;
      this.WelcomeWizardLabel.Location = new System.Drawing.Point(59, 279);
      this.WelcomeWizardLabel.Name = "WelcomeWizardLabel";
      this.WelcomeWizardLabel.Size = new System.Drawing.Size(447, 15);
      this.WelcomeWizardLabel.TabIndex = 2;
      this.WelcomeWizardLabel.Text = "With this wizard you will be able to configure your recent MySQL Server installat" +
    "ion.";
      // 
      // ClickNextLabel
      // 
      this.ClickNextLabel.AccessibleDescription = "A label displaying text instructing to click next";
      this.ClickNextLabel.AccessibleName = "Click Next Text";
      this.ClickNextLabel.AutoSize = true;
      this.ClickNextLabel.Location = new System.Drawing.Point(59, 303);
      this.ClickNextLabel.Name = "ClickNextLabel";
      this.ClickNextLabel.Size = new System.Drawing.Size(267, 15);
      this.ClickNextLabel.TabIndex = 3;
      this.ClickNextLabel.Text = "Simply click Next when you are ready to proceed.";
      // 
      // WelcomeBackPictureBox
      // 
      this.WelcomeBackPictureBox.AccessibleDescription = "A picture box displaying an image for the wizard page";
      this.WelcomeBackPictureBox.AccessibleName = "Welcome Back Image";
      this.WelcomeBackPictureBox.Image = global::MySql.Configurator.Properties.Resources.logo;
      this.WelcomeBackPictureBox.Location = new System.Drawing.Point(173, 80);
      this.WelcomeBackPictureBox.Name = "WelcomeBackPictureBox";
      this.WelcomeBackPictureBox.Size = new System.Drawing.Size(204, 165);
      this.WelcomeBackPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.WelcomeBackPictureBox.TabIndex = 6;
      this.WelcomeBackPictureBox.TabStop = false;
      // 
      // WelcomePage
      // 
      this.AccessibleDescription = "A wizard page serving as an introduction for the upgrade process";
      this.AccessibleName = "Welcome Back Page";
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
      this.Caption = "Welcome to the MySQL Server";
      this.Controls.Add(this.WelcomeBackPictureBox);
      this.Controls.Add(this.ClickNextLabel);
      this.Controls.Add(this.WelcomeWizardLabel);
      this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "WelcomePage";
      this.SubCaption = "";
      this.Controls.SetChildIndex(this.captionLabel, 0);
      this.Controls.SetChildIndex(this.subCaptionLabel, 0);
      this.Controls.SetChildIndex(this.WelcomeWizardLabel, 0);
      this.Controls.SetChildIndex(this.ClickNextLabel, 0);
      this.Controls.SetChildIndex(this.WelcomeBackPictureBox, 0);
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.WelcomeBackPictureBox)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label WelcomeWizardLabel;
    private System.Windows.Forms.Label ClickNextLabel;
    private System.Windows.Forms.PictureBox WelcomeBackPictureBox;
  }
}

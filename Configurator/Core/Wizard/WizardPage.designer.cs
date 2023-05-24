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

namespace MySql.Configurator.Core.Wizard
{
  partial class WizardPage
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WizardPage));
      this.captionLabel = new System.Windows.Forms.Label();
      this.subCaptionLabel = new System.Windows.Forms.Label();
      this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
      this.ValidationsTimer = new System.Windows.Forms.Timer(this.components);
      this.ValidationsErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).BeginInit();
      this.SuspendLayout();
      // 
      // captionLabel
      // 
      resources.ApplyResources(this.captionLabel, "captionLabel");
      this.captionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(95)))), ((int)(((byte)(140)))));
      this.captionLabel.Name = "captionLabel";
      // 
      // subCaptionLabel
      // 
      resources.ApplyResources(this.subCaptionLabel, "subCaptionLabel");
      this.subCaptionLabel.Name = "subCaptionLabel";
      // 
      // ValidationsTimer
      // 
      this.ValidationsTimer.Interval = 800;
      this.ValidationsTimer.Tick += new System.EventHandler(this.ValidationsTimer_Tick);
      // 
      // ValidationsErrorProvider
      // 
      this.ValidationsErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
      this.ValidationsErrorProvider.ContainerControl = this;
      // 
      // WizardPage
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
      this.BackColor = System.Drawing.SystemColors.Window;
      this.Controls.Add(this.subCaptionLabel);
      this.Controls.Add(this.captionLabel);
      resources.ApplyResources(this, "$this");
      this.Name = "WizardPage";
      this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.WizardPage_MouseMove);
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    public System.Windows.Forms.Label subCaptionLabel;
    public System.Windows.Forms.Label captionLabel;
    protected System.Windows.Forms.ToolTip ToolTip;
    protected System.Windows.Forms.Timer ValidationsTimer;
    protected System.Windows.Forms.ErrorProvider ValidationsErrorProvider;
  }
}

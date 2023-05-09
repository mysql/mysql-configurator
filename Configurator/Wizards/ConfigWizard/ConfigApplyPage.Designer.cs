/* Copyright (c) 2014, 2020, Oracle and/or its affiliates.

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

namespace MySql.Configurator.Wizards.ConfigWizard
{
  partial class ConfigApplyPage
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
      this.ProgressUpdateTimer = new System.Windows.Forms.Timer(this.components);
      this.LogTabPage = new System.Windows.Forms.TabPage();
      this.ExecutionTabControl.SuspendLayout();
      this.RebootWhenDonePanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).BeginInit();
      this.SuspendLayout();
      // 
      // ConfigurationResultLabel
      // 
      this.ConfigurationResultLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.ConfigurationResultLabel.Location = new System.Drawing.Point(30, 448);
      this.ConfigurationResultLabel.Size = new System.Drawing.Size(426, 53);
      // 
      // ExecutionTabControl
      // 
      this.ExecutionTabControl.Font = new System.Drawing.Font("Segoe UI", 9F);
      // 
      // RetryButton
      // 
      this.RetryButton.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.RetryButton.Location = new System.Drawing.Point(432, 26);
      // 
      // RebootWhenDoneCheckBox
      // 
      this.RebootWhenDoneCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      // 
      // RebootWhenDoneLabel
      // 
      this.RebootWhenDoneLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.RebootWhenDoneLabel.Text = "A change in the configuration requires a reboot to complete.\r\nDo you want to rebo" +
    "ot your computer when done?";
      // 
      // RebootWhenDonePanel
      // 
      this.RebootWhenDonePanel.Size = new System.Drawing.Size(510, 56);
      // 
      // subCaptionLabel
      // 
      this.subCaptionLabel.Location = new System.Drawing.Point(30, 55);
      this.subCaptionLabel.Size = new System.Drawing.Size(508, 16);
      this.subCaptionLabel.Text = "Click [Execute] to apply the changes";
      // 
      // captionLabel
      // 
      this.captionLabel.Size = new System.Drawing.Size(182, 25);
      this.captionLabel.Text = "Apply Configuration";
      // 
      // ProgressUpdateTimer
      // 
      this.ProgressUpdateTimer.Interval = 500;
      this.ProgressUpdateTimer.Tick += new System.EventHandler(this.ProgressUpdateTimer_Tick);
      // 
      // LogTabPage
      // 
      this.LogTabPage.AccessibleDescription = "A tab page containing a detailed log of operations performed during the configura" +
    "tion";
      this.LogTabPage.AccessibleName = "Log";
      this.LogTabPage.Location = new System.Drawing.Point(4, 22);
      this.LogTabPage.Name = "LogTabPage";
      this.LogTabPage.Padding = new System.Windows.Forms.Padding(3);
      this.LogTabPage.Size = new System.Drawing.Size(504, 342);
      this.LogTabPage.TabIndex = 1;
      this.LogTabPage.Text = "Log";
      // 
      // ConfigApplyPage
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
      this.Caption = "Apply Configuration";
      this.Name = "ConfigApplyPage";
      this.SubCaption = "Click [Execute] to apply the changes";
      this.ExecutionTabControl.ResumeLayout(false);
      this.RebootWhenDonePanel.ResumeLayout(false);
      this.RebootWhenDonePanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Timer ProgressUpdateTimer;
    private System.Windows.Forms.TabPage LogTabPage;
  }
}

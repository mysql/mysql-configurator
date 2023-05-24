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

namespace MySql.Configurator.Wizards.ConfigWizard
{
  partial class ConfigCompletePage
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
      this.ComponentsFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
      this.DisclaimerPanel = new System.Windows.Forms.Panel();
      this.MySQLServerDocumentationLinkLabel = new System.Windows.Forms.LinkLabel();
      this.DisclaimerLabel = new System.Windows.Forms.Label();
      this.CopyLogToClipboardButton = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).BeginInit();
      this.ComponentsFlowLayoutPanel.SuspendLayout();
      this.DisclaimerPanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // subCaptionLabel
      // 
      this.subCaptionLabel.Size = new System.Drawing.Size(298, 14);
      this.subCaptionLabel.Text = "The configuration procedure has been completed.";
      // 
      // captionLabel
      // 
      this.captionLabel.Size = new System.Drawing.Size(215, 25);
      this.captionLabel.Text = "Configuration Complete";
      // 
      // ComponentsFlowLayoutPanel
      // 
      this.ComponentsFlowLayoutPanel.Controls.Add(this.DisclaimerPanel);
      this.ComponentsFlowLayoutPanel.Location = new System.Drawing.Point(28, 178);
      this.ComponentsFlowLayoutPanel.Name = "ComponentsFlowLayoutPanel";
      this.ComponentsFlowLayoutPanel.Size = new System.Drawing.Size(495, 486);
      this.ComponentsFlowLayoutPanel.TabIndex = 10;
      // 
      // DisclaimerPanel
      // 
      this.DisclaimerPanel.AccessibleDescription = "A panel containing a MySQL Server disclaimer.";
      this.DisclaimerPanel.AccessibleName = "Disclaimer";
      this.DisclaimerPanel.Controls.Add(this.MySQLServerDocumentationLinkLabel);
      this.DisclaimerPanel.Controls.Add(this.DisclaimerLabel);
      this.DisclaimerPanel.Location = new System.Drawing.Point(4, 5);
      this.DisclaimerPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.DisclaimerPanel.Name = "DisclaimerPanel";
      this.DisclaimerPanel.Size = new System.Drawing.Size(489, 401);
      this.DisclaimerPanel.TabIndex = 17;
      // 
      // MySQLServerDocumentationLinkLabel
      // 
      this.MySQLServerDocumentationLinkLabel.AccessibleDescription = "A link label pointing to the official documentation for MySQL Server";
      this.MySQLServerDocumentationLinkLabel.AccessibleName = "MySQL Server Documentation";
      this.MySQLServerDocumentationLinkLabel.AutoSize = true;
      this.MySQLServerDocumentationLinkLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.MySQLServerDocumentationLinkLabel.Location = new System.Drawing.Point(-3, 48);
      this.MySQLServerDocumentationLinkLabel.Name = "MySQLServerDocumentationLinkLabel";
      this.MySQLServerDocumentationLinkLabel.Size = new System.Drawing.Size(213, 25);
      this.MySQLServerDocumentationLinkLabel.TabIndex = 13;
      this.MySQLServerDocumentationLinkLabel.TabStop = true;
      this.MySQLServerDocumentationLinkLabel.Text = "MySQL Reference Manual";
      this.MySQLServerDocumentationLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.MySQLServerDocumentationLinkLabel_LinkClicked);
      // 
      // DisclaimerLabel
      // 
      this.DisclaimerLabel.AccessibleDescription = "A label displaying a text prmoting the use of MySQL Server";
      this.DisclaimerLabel.AccessibleName = "Disclaimer";
      this.DisclaimerLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.DisclaimerLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.DisclaimerLabel.Location = new System.Drawing.Point(-3, 2);
      this.DisclaimerLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.DisclaimerLabel.Name = "DisclaimerLabel";
      this.DisclaimerLabel.Size = new System.Drawing.Size(489, 106);
      this.DisclaimerLabel.TabIndex = 6;
      this.DisclaimerLabel.Text = "Refer to the following documentation to get the most out of your MySQL Server ins" +
    "tallation:";
      // 
      // CopyLogToClipboardButton
      // 
      this.CopyLogToClipboardButton.AccessibleDescription = "A button that copies the installation log to the clipboard";
      this.CopyLogToClipboardButton.AccessibleName = "Copy Log To Clipboard";
      this.CopyLogToClipboardButton.AutoSize = true;
      this.CopyLogToClipboardButton.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.CopyLogToClipboardButton.Location = new System.Drawing.Point(28, 110);
      this.CopyLogToClipboardButton.Name = "CopyLogToClipboardButton";
      this.CopyLogToClipboardButton.Size = new System.Drawing.Size(204, 35);
      this.CopyLogToClipboardButton.TabIndex = 1;
      this.CopyLogToClipboardButton.Text = "C&opy Log to Clipboard";
      this.CopyLogToClipboardButton.UseVisualStyleBackColor = true;
      this.CopyLogToClipboardButton.Click += new System.EventHandler(this.CopyLogToClipboardButton_Click);
      // 
      // ConfigCompletePage
      // 
      this.AccessibleDescription = "A wizard page shown when the selected products finished configuring";
      this.AccessibleName = "Configuration Complete Page";
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
      this.AutoSize = true;
      this.Caption = "Configuration Complete";
      this.Controls.Add(this.CopyLogToClipboardButton);
      this.Controls.Add(this.ComponentsFlowLayoutPanel);
      this.DoubleBuffered = true;
      this.Name = "ConfigCompletePage";
      this.Size = new System.Drawing.Size(566, 675);
      this.SubCaption = "The configuration procedure has been completed.";
      this.Controls.SetChildIndex(this.ComponentsFlowLayoutPanel, 0);
      this.Controls.SetChildIndex(this.subCaptionLabel, 0);
      this.Controls.SetChildIndex(this.captionLabel, 0);
      this.Controls.SetChildIndex(this.CopyLogToClipboardButton, 0);
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).EndInit();
      this.ComponentsFlowLayoutPanel.ResumeLayout(false);
      this.DisclaimerPanel.ResumeLayout(false);
      this.DisclaimerPanel.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.FlowLayoutPanel ComponentsFlowLayoutPanel;
    private System.Windows.Forms.Panel DisclaimerPanel;
    private System.Windows.Forms.Label DisclaimerLabel;
    private System.Windows.Forms.Button CopyLogToClipboardButton;
    private System.Windows.Forms.LinkLabel MySQLServerDocumentationLinkLabel;
  }
}

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
  partial class BaseConfigureRemoveApplyPage
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
      this.ConfigurationResultLabel = new System.Windows.Forms.Label();
      this.LogTabPage = new System.Windows.Forms.TabPage();
      this.LogContentsTextBox = new System.Windows.Forms.TextBox();
      this.ExecutionStepsTabPage = new System.Windows.Forms.TabPage();
      this.ExecutionTabControl = new System.Windows.Forms.TabControl();
      this.RebootWhenDonePanel = new System.Windows.Forms.Panel();
      this.RebootWhenDoneLabel = new System.Windows.Forms.Label();
      this.RebootWhenDoneCheckBox = new System.Windows.Forms.CheckBox();
      this.RetryButton = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).BeginInit();
      this.LogTabPage.SuspendLayout();
      this.ExecutionTabControl.SuspendLayout();
      this.RebootWhenDonePanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // subCaptionLabel
      // 
      this.subCaptionLabel.Location = new System.Drawing.Point(30, 52);
      this.subCaptionLabel.Size = new System.Drawing.Size(508, 19);
      this.subCaptionLabel.Text = "Click [Execute] to ...";
      // 
      // captionLabel
      // 
      this.captionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
      this.captionLabel.Size = new System.Drawing.Size(48, 25);
      this.captionLabel.Text = "Title";
      // 
      // ConfigurationResultLabel
      // 
      this.ConfigurationResultLabel.AccessibleDescription = "A label showing a summary of the result of the applied configuration";
      this.ConfigurationResultLabel.AccessibleName = "Configuration Results";
      this.ConfigurationResultLabel.AutoEllipsis = true;
      this.ConfigurationResultLabel.Location = new System.Drawing.Point(30, 448);
      this.ConfigurationResultLabel.Name = "ConfigurationResultLabel";
      this.ConfigurationResultLabel.Size = new System.Drawing.Size(427, 53);
      this.ConfigurationResultLabel.TabIndex = 14;
      // 
      // LogTabPage
      // 
      this.LogTabPage.AccessibleDescription = "A tab page containing a detailed log of operations performed during the configura" +
    "tion";
      this.LogTabPage.AccessibleName = "Log";
      this.LogTabPage.Controls.Add(this.LogContentsTextBox);
      this.LogTabPage.Location = new System.Drawing.Point(4, 32);
      this.LogTabPage.Name = "LogTabPage";
      this.LogTabPage.Padding = new System.Windows.Forms.Padding(3);
      this.LogTabPage.Size = new System.Drawing.Size(504, 332);
      this.LogTabPage.TabIndex = 1;
      this.LogTabPage.Text = "Log";
      // 
      // LogContentsTextBox
      // 
      this.LogContentsTextBox.AccessibleDescription = "A text box containing a detailed log of operations performed during the configura" +
    "tion";
      this.LogContentsTextBox.AccessibleName = "Detailed Log";
      this.LogContentsTextBox.BackColor = System.Drawing.SystemColors.Window;
      this.LogContentsTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.LogContentsTextBox.Location = new System.Drawing.Point(3, 3);
      this.LogContentsTextBox.Multiline = true;
      this.LogContentsTextBox.Name = "LogContentsTextBox";
      this.LogContentsTextBox.ReadOnly = true;
      this.LogContentsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.LogContentsTextBox.Size = new System.Drawing.Size(498, 326);
      this.LogContentsTextBox.TabIndex = 0;
      // 
      // ExecutionStepsTabPage
      // 
      this.ExecutionStepsTabPage.AccessibleDescription = "A tab page containing controls showing the steps to be performed";
      this.ExecutionStepsTabPage.AccessibleName = "Execution Steps";
      this.ExecutionStepsTabPage.AutoScroll = true;
      this.ExecutionStepsTabPage.Location = new System.Drawing.Point(4, 34);
      this.ExecutionStepsTabPage.Name = "ExecutionStepsTabPage";
      this.ExecutionStepsTabPage.Padding = new System.Windows.Forms.Padding(3);
      this.ExecutionStepsTabPage.Size = new System.Drawing.Size(504, 330);
      this.ExecutionStepsTabPage.TabIndex = 0;
      this.ExecutionStepsTabPage.Text = "Execution Steps";
      this.ExecutionStepsTabPage.UseVisualStyleBackColor = true;
      // 
      // ExecutionTabControl
      // 
      this.ExecutionTabControl.AccessibleDescription = "A control showing tabs related to the execution process";
      this.ExecutionTabControl.AccessibleName = "Execution Tabs";
      this.ExecutionTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.ExecutionTabControl.Controls.Add(this.ExecutionStepsTabPage);
      this.ExecutionTabControl.Controls.Add(this.LogTabPage);
      this.ExecutionTabControl.Location = new System.Drawing.Point(30, 74);
      this.ExecutionTabControl.Name = "ExecutionTabControl";
      this.ExecutionTabControl.SelectedIndex = 0;
      this.ExecutionTabControl.Size = new System.Drawing.Size(512, 368);
      this.ExecutionTabControl.TabIndex = 2;
      // 
      // RebootWhenDonePanel
      // 
      this.RebootWhenDonePanel.AccessibleDescription = "A panel containing controls about rebooting the computer when the configuration c" +
    "hanges require a reboot";
      this.RebootWhenDonePanel.AccessibleName = "Reboot Computer Group";
      this.RebootWhenDonePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.RebootWhenDonePanel.Controls.Add(this.RebootWhenDoneLabel);
      this.RebootWhenDonePanel.Controls.Add(this.RebootWhenDoneCheckBox);
      this.RebootWhenDonePanel.Controls.Add(this.RetryButton);
      this.RebootWhenDonePanel.Location = new System.Drawing.Point(29, 445);
      this.RebootWhenDonePanel.Name = "RebootWhenDonePanel";
      this.RebootWhenDonePanel.Size = new System.Drawing.Size(508, 56);
      this.RebootWhenDonePanel.TabIndex = 15;
      this.RebootWhenDonePanel.Visible = false;
      // 
      // RebootWhenDoneLabel
      // 
      this.RebootWhenDoneLabel.AccessibleDescription = "A label displaying a message about rebooting the computer when the configuration " +
    "changes require a reboot";
      this.RebootWhenDoneLabel.AccessibleName = "Reboot Computer Text";
      this.RebootWhenDoneLabel.Location = new System.Drawing.Point(3, 1);
      this.RebootWhenDoneLabel.Name = "RebootWhenDoneLabel";
      this.RebootWhenDoneLabel.Size = new System.Drawing.Size(424, 33);
      this.RebootWhenDoneLabel.TabIndex = 0;
      this.RebootWhenDoneLabel.Text = "The execued operation requires a reboot to complete.\r\nDo you want to reboot your " +
    "computer when done?";
      // 
      // RebootWhenDoneCheckBox
      // 
      this.RebootWhenDoneCheckBox.AccessibleDescription = "A check box to reboot the computer when the closing the configuration wizard";
      this.RebootWhenDoneCheckBox.AccessibleName = "Reboot When Done";
      this.RebootWhenDoneCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.RebootWhenDoneCheckBox.AutoSize = true;
      this.RebootWhenDoneCheckBox.Location = new System.Drawing.Point(6, 24);
      this.RebootWhenDoneCheckBox.Name = "RebootWhenDoneCheckBox";
      this.RebootWhenDoneCheckBox.Size = new System.Drawing.Size(219, 29);
      this.RebootWhenDoneCheckBox.TabIndex = 1;
      this.RebootWhenDoneCheckBox.Text = "Yes, reboot when done";
      this.RebootWhenDoneCheckBox.UseVisualStyleBackColor = true;
      // 
      // RetryButton
      // 
      this.RetryButton.AccessibleDescription = "A button to retry the configuration steps in case the configuration fails";
      this.RetryButton.AccessibleName = "Retry";
      this.RetryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.RetryButton.Location = new System.Drawing.Point(434, 7);
      this.RetryButton.Name = "RetryButton";
      this.RetryButton.Size = new System.Drawing.Size(75, 27);
      this.RetryButton.TabIndex = 2;
      this.RetryButton.Text = "Retry";
      this.RetryButton.UseVisualStyleBackColor = true;
      this.RetryButton.Visible = false;
      this.RetryButton.Click += new System.EventHandler(this.RetryButton_Click);
      // 
      // BaseConfigureRemoveApplyPage
      // 
      this.AccessibleDescription = "A wizard page showing configuration steps to be performed and a detailed configur" +
    "ation log";
      this.AccessibleName = "Apply Configuration Page";
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
      this.Caption = "Title";
      this.Controls.Add(this.RebootWhenDonePanel);
      this.Controls.Add(this.ConfigurationResultLabel);
      this.Controls.Add(this.ExecutionTabControl);
      this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "BaseConfigureRemoveApplyPage";
      this.SubCaption = "Click [Execute] to ...";
      this.Controls.SetChildIndex(this.ExecutionTabControl, 0);
      this.Controls.SetChildIndex(this.ConfigurationResultLabel, 0);
      this.Controls.SetChildIndex(this.subCaptionLabel, 0);
      this.Controls.SetChildIndex(this.captionLabel, 0);
      this.Controls.SetChildIndex(this.RebootWhenDonePanel, 0);
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).EndInit();
      this.LogTabPage.ResumeLayout(false);
      this.LogTabPage.PerformLayout();
      this.ExecutionTabControl.ResumeLayout(false);
      this.RebootWhenDonePanel.ResumeLayout(false);
      this.RebootWhenDonePanel.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    protected System.Windows.Forms.Label ConfigurationResultLabel;
    private System.Windows.Forms.TabPage LogTabPage;
    protected System.Windows.Forms.TextBox LogContentsTextBox;
    protected System.Windows.Forms.TabPage ExecutionStepsTabPage;
    protected System.Windows.Forms.TabControl ExecutionTabControl;
    protected System.Windows.Forms.Panel RebootWhenDonePanel;
    protected System.Windows.Forms.Label RebootWhenDoneLabel;
    protected System.Windows.Forms.CheckBox RebootWhenDoneCheckBox;
    protected System.Windows.Forms.Button RetryButton;
  }
}

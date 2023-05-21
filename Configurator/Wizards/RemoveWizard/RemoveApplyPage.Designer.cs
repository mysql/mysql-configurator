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
  partial class RemoveApplyPage
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
      this.RemoveStepsTabPage = new System.Windows.Forms.TabPage();
      this.LogTabPage = new System.Windows.Forms.TabPage();
      this.RemoveStepsFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
      this.ControlsPanel = new System.Windows.Forms.Panel();
      this.CollapseButton = new System.Windows.Forms.Button();
      this.ExpandButton = new System.Windows.Forms.Button();
      this.StatusColumnLabel = new System.Windows.Forms.Label();
      this.StepsColumnLabel = new System.Windows.Forms.Label();
      this.UninstallInstallerPanel = new System.Windows.Forms.Panel();
      this.UninstallInstallerLabel = new System.Windows.Forms.Label();
      this.UninstallInstallerCheckBox = new System.Windows.Forms.CheckBox();
      this.ExecutionStepsTabPage.SuspendLayout();
      this.ExecutionTabControl.SuspendLayout();
      this.RebootWhenDonePanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).BeginInit();
      this.ControlsPanel.SuspendLayout();
      this.UninstallInstallerPanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // ConfigurationResultLabel
      // 
      this.ConfigurationResultLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      // 
      // LogContentsTextBox
      // 
      this.LogContentsTextBox.Size = new System.Drawing.Size(498, 324);
      // 
      // ExecutionStepsTabPage
      // 
      this.ExecutionStepsTabPage.AutoScroll = false;
      this.ExecutionStepsTabPage.Controls.Add(this.ControlsPanel);
      this.ExecutionStepsTabPage.Controls.Add(this.RemoveStepsFlowLayoutPanel);
      // 
      // ExecutionTabControl
      // 
      this.ExecutionTabControl.Font = new System.Drawing.Font("Segoe UI", 9F);
      // 
      // RebootWhenDonePanel
      // 
      this.RebootWhenDonePanel.Size = new System.Drawing.Size(510, 56);
      // 
      // RebootWhenDoneLabel
      // 
      this.RebootWhenDoneLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.RebootWhenDoneLabel.Text = "A change in the configuration requires a reboot to complete.\r\nDo you want to rebo" +
    "ot your computer when done?";
      // 
      // RebootWhenDoneCheckBox
      // 
      this.RebootWhenDoneCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      // 
      // RetryButton
      // 
      this.RetryButton.Font = new System.Drawing.Font("Segoe UI", 9F);
      // 
      // subCaptionLabel
      // 
      this.subCaptionLabel.Location = new System.Drawing.Point(30, 54);
      this.subCaptionLabel.Text = "Click [Execute] to remove the server configurations";
      // 
      // captionLabel
      // 
      this.captionLabel.Size = new System.Drawing.Size(233, 25);
      this.captionLabel.Text = "Remove Steps";
      // 
      // ProgressUpdateTimer
      // 
      this.ProgressUpdateTimer.Interval = 500;
      this.ProgressUpdateTimer.Tick += new System.EventHandler(this.ProgressUpdateTimer_Tick);
      // 
      // RemoveStepsTabPage
      // 
      this.RemoveStepsTabPage.AccessibleDescription = "A tab page containing controls showing the removal steps to be performed";
      this.RemoveStepsTabPage.AccessibleName = "Remove Steps";
      this.RemoveStepsTabPage.AutoScroll = true;
      this.RemoveStepsTabPage.Location = new System.Drawing.Point(4, 22);
      this.RemoveStepsTabPage.Name = "RemoveStepsTabPage";
      this.RemoveStepsTabPage.Padding = new System.Windows.Forms.Padding(3);
      this.RemoveStepsTabPage.Size = new System.Drawing.Size(504, 336);
      this.RemoveStepsTabPage.TabIndex = 0;
      this.RemoveStepsTabPage.Text = "Remove Steps";
      this.RemoveStepsTabPage.UseVisualStyleBackColor = true;
      // 
      // LogTabPage
      // 
      this.LogTabPage.AccessibleDescription = "A tab page containing a detailed log of operations performed during the configura" +
    "tion";
      this.LogTabPage.AccessibleName = "Log";
      this.LogTabPage.Location = new System.Drawing.Point(4, 22);
      this.LogTabPage.Name = "LogTabPage";
      this.LogTabPage.Padding = new System.Windows.Forms.Padding(3);
      this.LogTabPage.Size = new System.Drawing.Size(504, 336);
      this.LogTabPage.TabIndex = 1;
      this.LogTabPage.Text = "Log";
      // 
      // RemoveStepsFlowLayoutPanel
      // 
      this.RemoveStepsFlowLayoutPanel.AccessibleDescription = "A flow layout panel containing all the steps to be executed";
      this.RemoveStepsFlowLayoutPanel.AccessibleName = "Remove Steps Layout";
      this.RemoveStepsFlowLayoutPanel.AutoScroll = true;
      this.RemoveStepsFlowLayoutPanel.Location = new System.Drawing.Point(0, 23);
      this.RemoveStepsFlowLayoutPanel.Name = "RemoveStepsFlowLayoutPanel";
      this.RemoveStepsFlowLayoutPanel.Size = new System.Drawing.Size(501, 312);
      this.RemoveStepsFlowLayoutPanel.TabIndex = 0;
      // 
      // ControlsPanel
      // 
      this.ControlsPanel.AccessibleDescription = "A panel containing the controls to handle the uninstallation of products and step" +
    "s to be executed";
      this.ControlsPanel.AccessibleName = "Controls";
      this.ControlsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
      this.ControlsPanel.Controls.Add(this.CollapseButton);
      this.ControlsPanel.Controls.Add(this.ExpandButton);
      this.ControlsPanel.Controls.Add(this.StatusColumnLabel);
      this.ControlsPanel.Controls.Add(this.StepsColumnLabel);
      this.ControlsPanel.Location = new System.Drawing.Point(0, 0);
      this.ControlsPanel.Name = "ControlsPanel";
      this.ControlsPanel.Size = new System.Drawing.Size(501, 24);
      this.ControlsPanel.TabIndex = 8;
      // 
      // CollapseButton
      // 
      this.CollapseButton.AccessibleDescription = "A button to collapse all the substeps";
      this.CollapseButton.AccessibleName = "Collapse All";
      this.CollapseButton.FlatAppearance.BorderSize = 0;
      this.CollapseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.CollapseButton.Image = global::MySql.Configurator.Properties.Resources.minus_sign;
      this.CollapseButton.Location = new System.Drawing.Point(479, 0);
      this.CollapseButton.Name = "CollapseButton";
      this.CollapseButton.Size = new System.Drawing.Size(22, 22);
      this.CollapseButton.TabIndex = 13;
      this.ToolTip.SetToolTip(this.CollapseButton, "Collapse all");
      this.CollapseButton.UseVisualStyleBackColor = true;
      this.CollapseButton.Click += new System.EventHandler(this.CollapseButton_Click);
      // 
      // ExpandButton
      // 
      this.ExpandButton.AccessibleDescription = "A button to expand all substeps";
      this.ExpandButton.AccessibleName = "Expand All";
      this.ExpandButton.FlatAppearance.BorderSize = 0;
      this.ExpandButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.ExpandButton.Image = global::MySql.Configurator.Properties.Resources.plus_sign;
      this.ExpandButton.Location = new System.Drawing.Point(456, 0);
      this.ExpandButton.Name = "ExpandButton";
      this.ExpandButton.Size = new System.Drawing.Size(22, 22);
      this.ExpandButton.TabIndex = 12;
      this.ToolTip.SetToolTip(this.ExpandButton, "Expand all");
      this.ExpandButton.UseVisualStyleBackColor = true;
      this.ExpandButton.Click += new System.EventHandler(this.ExpandButton_Click);
      // 
      // StatusColumnLabel
      // 
      this.StatusColumnLabel.AccessibleDescription = "A label to display the column Status";
      this.StatusColumnLabel.AccessibleName = "Status Column";
      this.StatusColumnLabel.AutoSize = true;
      this.StatusColumnLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.StatusColumnLabel.Location = new System.Drawing.Point(313, 5);
      this.StatusColumnLabel.Name = "StatusColumnLabel";
      this.StatusColumnLabel.Size = new System.Drawing.Size(60, 25);
      this.StatusColumnLabel.TabIndex = 11;
      this.StatusColumnLabel.Text = "Status";
      // 
      // StepsColumnLabel
      // 
      this.StepsColumnLabel.AccessibleDescription = "A label to display the column Steps";
      this.StepsColumnLabel.AccessibleName = "Steps Column";
      this.StepsColumnLabel.AutoSize = true;
      this.StepsColumnLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.StepsColumnLabel.Location = new System.Drawing.Point(15, 4);
      this.StepsColumnLabel.Name = "StepsColumnLabel";
      this.StepsColumnLabel.Size = new System.Drawing.Size(66, 30);
      this.StepsColumnLabel.TabIndex = 10;
      this.StepsColumnLabel.Text = "Step";
      // 
      // UninstallInstallerPanel
      // 
      this.UninstallInstallerPanel.AccessibleDescription = "A panel containing controls related to removing MySQL Installer";
      this.UninstallInstallerPanel.AccessibleName = "Uninstall MySQL Installer Group";
      this.UninstallInstallerPanel.Controls.Add(this.UninstallInstallerLabel);
      this.UninstallInstallerPanel.Controls.Add(this.UninstallInstallerCheckBox);
      this.UninstallInstallerPanel.Location = new System.Drawing.Point(29, 386);
      this.UninstallInstallerPanel.Name = "UninstallInstallerPanel";
      this.UninstallInstallerPanel.Size = new System.Drawing.Size(509, 54);
      this.UninstallInstallerPanel.TabIndex = 15;
      this.UninstallInstallerPanel.Visible = false;
      // 
      // UninstallInstallerLabel
      // 
      this.UninstallInstallerLabel.AccessibleDescription = "A label displaying a question about uninstalling MySQL Installer";
      this.UninstallInstallerLabel.AccessibleName = "Uninstall MySQL Installer Text";
      this.UninstallInstallerLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.UninstallInstallerLabel.Location = new System.Drawing.Point(3, 3);
      this.UninstallInstallerLabel.Name = "UninstallInstallerLabel";
      this.UninstallInstallerLabel.Size = new System.Drawing.Size(507, 24);
      this.UninstallInstallerLabel.TabIndex = 0;
      this.UninstallInstallerLabel.Text = "All {0} MySQL products have been removed. Uninstall MySQL Installer too?";
      // 
      // UninstallInstallerCheckBox
      // 
      this.UninstallInstallerCheckBox.AccessibleDescription = "A check box shown when all products have been removed meant to remove MySQL Insta" +
    "ller as well when closing the wizard";
      this.UninstallInstallerCheckBox.AccessibleName = "Uninstall MySQL Installer";
      this.UninstallInstallerCheckBox.AutoSize = true;
      this.UninstallInstallerCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.UninstallInstallerCheckBox.Location = new System.Drawing.Point(5, 30);
      this.UninstallInstallerCheckBox.Name = "UninstallInstallerCheckBox";
      this.UninstallInstallerCheckBox.Size = new System.Drawing.Size(265, 29);
      this.UninstallInstallerCheckBox.TabIndex = 1;
      this.UninstallInstallerCheckBox.Text = "Yes, uninstall MySQL Installer";
      this.UninstallInstallerCheckBox.UseVisualStyleBackColor = true;
      // 
      // RemoveApplyPage
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
      this.Caption = "Remove Steps";
      this.Controls.Add(this.UninstallInstallerPanel);
      this.Name = "RemoveApplyPage";
      this.SubCaption = "Click [Execute] to remove the server configurations";
      this.Controls.SetChildIndex(this.UninstallInstallerPanel, 0);
      this.Controls.SetChildIndex(this.ExecutionTabControl, 0);
      this.Controls.SetChildIndex(this.RebootWhenDonePanel, 0);
      this.Controls.SetChildIndex(this.subCaptionLabel, 0);
      this.Controls.SetChildIndex(this.captionLabel, 0);
      this.Controls.SetChildIndex(this.ConfigurationResultLabel, 0);
      this.ExecutionStepsTabPage.ResumeLayout(false);
      this.ExecutionTabControl.ResumeLayout(false);
      this.RebootWhenDonePanel.ResumeLayout(false);
      this.RebootWhenDonePanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).EndInit();
      this.ControlsPanel.ResumeLayout(false);
      this.ControlsPanel.PerformLayout();
      this.UninstallInstallerPanel.ResumeLayout(false);
      this.UninstallInstallerPanel.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Timer ProgressUpdateTimer;
    private System.Windows.Forms.TabPage RemoveStepsTabPage;
    private System.Windows.Forms.TabPage LogTabPage;
    private System.Windows.Forms.FlowLayoutPanel RemoveStepsFlowLayoutPanel;
    private System.Windows.Forms.Panel ControlsPanel;
    private System.Windows.Forms.Button CollapseButton;
    private System.Windows.Forms.Button ExpandButton;
    private System.Windows.Forms.Label StatusColumnLabel;
    private System.Windows.Forms.Label StepsColumnLabel;
    private System.Windows.Forms.Panel UninstallInstallerPanel;
    private System.Windows.Forms.Label UninstallInstallerLabel;
    private System.Windows.Forms.CheckBox UninstallInstallerCheckBox;
  }
}

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

using MySql.Configurator.Controls;

namespace MySql.Configurator.Wizards
{
  partial class Wizard
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Wizard));
      this.WizardBackButton = new System.Windows.Forms.Button();
      this.WizardCancelButton = new System.Windows.Forms.Button();
      this.WizardNextButton = new System.Windows.Forms.Button();
      this.FooterAreaFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
      this.WizardHelpButton = new System.Windows.Forms.Button();
      this.WizardFinishButton = new System.Windows.Forms.Button();
      this.WizardExecuteButton = new System.Windows.Forms.Button();
      this.SeparatorLinePanel = new System.Windows.Forms.Panel();
      this.WizardSideBar = new MySql.Configurator.Controls.InstallWizardSideBarControl();
      this.FooterAreaFlowLayoutPanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // WizardBackButton
      // 
      this.WizardBackButton.AccessibleDescription = "A button to navigate to the previous wizard page";
      this.WizardBackButton.AccessibleName = "Back";
      this.WizardBackButton.BackColor = System.Drawing.SystemColors.Control;
      this.WizardBackButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDark;
      this.WizardBackButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.WizardBackButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.WizardBackButton.Location = new System.Drawing.Point(7, 11);
      this.WizardBackButton.Name = "WizardBackButton";
      this.WizardBackButton.Size = new System.Drawing.Size(86, 26);
      this.WizardBackButton.TabIndex = 0;
      this.WizardBackButton.Text = global::MySql.Configurator.Properties.Resources.BackButtonDefaultText;
      this.WizardBackButton.UseVisualStyleBackColor = false;
      this.WizardBackButton.Click += new System.EventHandler(this.WizardBackButton_Click);
      // 
      // WizardCancelButton
      // 
      this.WizardCancelButton.AccessibleDescription = "A button to cancel the wizard";
      this.WizardCancelButton.AccessibleName = "Cancel";
      this.WizardCancelButton.BackColor = System.Drawing.SystemColors.Control;
      this.WizardCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.WizardCancelButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDark;
      this.WizardCancelButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.WizardCancelButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.WizardCancelButton.Location = new System.Drawing.Point(375, 11);
      this.WizardCancelButton.Name = "WizardCancelButton";
      this.WizardCancelButton.Size = new System.Drawing.Size(86, 26);
      this.WizardCancelButton.TabIndex = 4;
      this.WizardCancelButton.Text = global::MySql.Configurator.Properties.Resources.CancelButtonDefaultText;
      this.WizardCancelButton.UseVisualStyleBackColor = false;
      this.WizardCancelButton.Click += new System.EventHandler(this.WizardCancelButton_Click);
      // 
      // WizardNextButton
      // 
      this.WizardNextButton.AccessibleDescription = "A button to navigate to the next wizard page";
      this.WizardNextButton.AccessibleName = "Next";
      this.WizardNextButton.BackColor = System.Drawing.SystemColors.Control;
      this.WizardNextButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDark;
      this.WizardNextButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.WizardNextButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.WizardNextButton.Location = new System.Drawing.Point(191, 11);
      this.WizardNextButton.Name = "WizardNextButton";
      this.WizardNextButton.Size = new System.Drawing.Size(86, 26);
      this.WizardNextButton.TabIndex = 2;
      this.WizardNextButton.Text = global::MySql.Configurator.Properties.Resources.NextButtonDefaultText;
      this.WizardNextButton.UseVisualStyleBackColor = false;
      this.WizardNextButton.Click += new System.EventHandler(this.WizardNextButton_Click);
      // 
      // FooterAreaFlowLayoutPanel
      // 
      this.FooterAreaFlowLayoutPanel.AccessibleDescription = "A panel in the lower section of a wizard denoting a footer area";
      this.FooterAreaFlowLayoutPanel.AccessibleName = "Footer Area";
      this.FooterAreaFlowLayoutPanel.BackColor = System.Drawing.SystemColors.Control;
      this.FooterAreaFlowLayoutPanel.Controls.Add(this.WizardHelpButton);
      this.FooterAreaFlowLayoutPanel.Controls.Add(this.WizardCancelButton);
      this.FooterAreaFlowLayoutPanel.Controls.Add(this.WizardFinishButton);
      this.FooterAreaFlowLayoutPanel.Controls.Add(this.WizardNextButton);
      this.FooterAreaFlowLayoutPanel.Controls.Add(this.WizardExecuteButton);
      this.FooterAreaFlowLayoutPanel.Controls.Add(this.WizardBackButton);
      this.FooterAreaFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.FooterAreaFlowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
      this.FooterAreaFlowLayoutPanel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FooterAreaFlowLayoutPanel.Location = new System.Drawing.Point(220, 513);
      this.FooterAreaFlowLayoutPanel.Name = "FooterAreaFlowLayoutPanel";
      this.FooterAreaFlowLayoutPanel.Padding = new System.Windows.Forms.Padding(0, 8, 8, 0);
      this.FooterAreaFlowLayoutPanel.Size = new System.Drawing.Size(564, 49);
      this.FooterAreaFlowLayoutPanel.TabIndex = 2;
      this.FooterAreaFlowLayoutPanel.WrapContents = false;
      // 
      // WizardHelpButton
      // 
      this.WizardHelpButton.AccessibleDescription = "A button to show contextual help about the current wizard page";
      this.WizardHelpButton.AccessibleName = "Help";
      this.WizardHelpButton.BackColor = System.Drawing.SystemColors.Control;
      this.WizardHelpButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDark;
      this.WizardHelpButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.WizardHelpButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.WizardHelpButton.Location = new System.Drawing.Point(467, 11);
      this.WizardHelpButton.Name = "WizardHelpButton";
      this.WizardHelpButton.Size = new System.Drawing.Size(86, 26);
      this.WizardHelpButton.TabIndex = 5;
      this.WizardHelpButton.Text = "&Help";
      this.WizardHelpButton.UseVisualStyleBackColor = false;
      this.WizardHelpButton.Visible = false;
      this.WizardHelpButton.Click += new System.EventHandler(this.WizardHelpButton_Click);
      // 
      // WizardFinishButton
      // 
      this.WizardFinishButton.AccessibleDescription = "A button to close the wizard after it has finished all operations";
      this.WizardFinishButton.AccessibleName = "Finish";
      this.WizardFinishButton.BackColor = System.Drawing.SystemColors.Control;
      this.WizardFinishButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDark;
      this.WizardFinishButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.WizardFinishButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.WizardFinishButton.Location = new System.Drawing.Point(283, 11);
      this.WizardFinishButton.Name = "WizardFinishButton";
      this.WizardFinishButton.Size = new System.Drawing.Size(86, 26);
      this.WizardFinishButton.TabIndex = 3;
      this.WizardFinishButton.Text = "&Finish";
      this.WizardFinishButton.UseVisualStyleBackColor = false;
      this.WizardFinishButton.Click += new System.EventHandler(this.WizardFinishButton_Click);
      // 
      // WizardExecuteButton
      // 
      this.WizardExecuteButton.AccessibleDescription = "A button to execute the wizard operation";
      this.WizardExecuteButton.AccessibleName = "Execute";
      this.WizardExecuteButton.BackColor = System.Drawing.SystemColors.Control;
      this.WizardExecuteButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDark;
      this.WizardExecuteButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.WizardExecuteButton.Location = new System.Drawing.Point(99, 11);
      this.WizardExecuteButton.Name = "WizardExecuteButton";
      this.WizardExecuteButton.Size = new System.Drawing.Size(86, 26);
      this.WizardExecuteButton.TabIndex = 1;
      this.WizardExecuteButton.Text = "E&xecute";
      this.WizardExecuteButton.UseVisualStyleBackColor = false;
      this.WizardExecuteButton.Visible = false;
      this.WizardExecuteButton.Click += new System.EventHandler(this.WizardExecuteButton_Click);
      // 
      // SeparatorLinePanel
      // 
      this.SeparatorLinePanel.AccessibleDescription = "A panel resized in a way that it looks like a separator line between the footer a" +
    "rea and the wizard pages area";
      this.SeparatorLinePanel.AccessibleName = "Separator Line";
      this.SeparatorLinePanel.BackColor = System.Drawing.SystemColors.ActiveBorder;
      this.SeparatorLinePanel.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.SeparatorLinePanel.Location = new System.Drawing.Point(220, 512);
      this.SeparatorLinePanel.Name = "SeparatorLinePanel";
      this.SeparatorLinePanel.Size = new System.Drawing.Size(564, 1);
      this.SeparatorLinePanel.TabIndex = 1;
      // 
      // WizardSideBar
      // 
      this.WizardSideBar.AccessibleDescription = "A side bar at the left of the wizard that shows information about a current produ" +
    "ct and the flow of wizard pages";
      this.WizardSideBar.AccessibleName = "Wizard Side Bar";
      this.WizardSideBar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("WizardSideBar.BackgroundImage")));
      this.WizardSideBar.Dock = System.Windows.Forms.DockStyle.Left;
      this.WizardSideBar.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.WizardSideBar.Location = new System.Drawing.Point(0, 0);
      this.WizardSideBar.Name = "WizardSideBar";
      this.WizardSideBar.Size = new System.Drawing.Size(220, 562);
      this.WizardSideBar.TabIndex = 0;
      this.WizardSideBar.TabStop = false;
      // 
      // Wizard
      // 
      this.AccessibleDescription = "A wizard control containing pages and navigation buttons";
      this.AccessibleName = "Wizard";
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
      this.BackColor = System.Drawing.Color.White;
      this.Controls.Add(this.SeparatorLinePanel);
      this.Controls.Add(this.FooterAreaFlowLayoutPanel);
      this.Controls.Add(this.WizardSideBar);
      this.DoubleBuffered = true;
      this.Margin = new System.Windows.Forms.Padding(0);
      this.Name = "Wizard";
      this.Size = new System.Drawing.Size(784, 562);
      this.FooterAreaFlowLayoutPanel.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    protected InstallWizardSideBarControl WizardSideBar;
    private System.Windows.Forms.Button WizardBackButton;
    private System.Windows.Forms.Button WizardCancelButton;
    private System.Windows.Forms.Button WizardNextButton;
    private System.Windows.Forms.FlowLayoutPanel FooterAreaFlowLayoutPanel;
    private System.Windows.Forms.Button WizardHelpButton;
    private System.Windows.Forms.Button WizardExecuteButton;
    private System.Windows.Forms.Button WizardFinishButton;
    private System.Windows.Forms.Panel SeparatorLinePanel;
  }
}

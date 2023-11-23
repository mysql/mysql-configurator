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

namespace MySql.Configurator.Wizards.Server
{
  partial class ServerRemovePage
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
      this.ServerRemovalFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
      this.RemoveDataDirectoryPanel = new System.Windows.Forms.Panel();
      this.RemoveDataDirectoryCheckBox = new System.Windows.Forms.CheckBox();
      this.RemoveDataDirectoryLabel = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).BeginInit();
      this.ServerRemovalFlowLayoutPanel.SuspendLayout();
      this.RemoveDataDirectoryPanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // subCaptionLabel
      // 
      this.subCaptionLabel.Location = new System.Drawing.Point(27, 37);
      this.subCaptionLabel.Visible = false;
      // 
      // captionLabel
      // 
      this.captionLabel.Size = new System.Drawing.Size(38, 25);
      this.captionLabel.Text = "<>";
      // 
      // ServerRemovalFlowLayoutPanel
      // 
      this.ServerRemovalFlowLayoutPanel.AccessibleDescription = "A panel containing inner panels with options appearing depending on the MySQL Ser" +
    "ver installation type.";
      this.ServerRemovalFlowLayoutPanel.AccessibleName = "Server Removal Flow Panel";
      this.ServerRemovalFlowLayoutPanel.Controls.Add(this.RemoveDataDirectoryPanel);
      this.ServerRemovalFlowLayoutPanel.Location = new System.Drawing.Point(0, 142);
      this.ServerRemovalFlowLayoutPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.ServerRemovalFlowLayoutPanel.Name = "ServerRemovalFlowLayoutPanel";
      this.ServerRemovalFlowLayoutPanel.Size = new System.Drawing.Size(849, 1613);
      this.ServerRemovalFlowLayoutPanel.TabIndex = 2;
      // 
      // RemoveDataDirectoryPanel
      // 
      this.RemoveDataDirectoryPanel.AccessibleDescription = "A panel containing controls for removing the data directory.";
      this.RemoveDataDirectoryPanel.AccessibleName = "Remove Data Directory";
      this.RemoveDataDirectoryPanel.Controls.Add(this.RemoveDataDirectoryCheckBox);
      this.RemoveDataDirectoryPanel.Controls.Add(this.RemoveDataDirectoryLabel);
      this.RemoveDataDirectoryPanel.Location = new System.Drawing.Point(4, 5);
      this.RemoveDataDirectoryPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.RemoveDataDirectoryPanel.Name = "RemoveDataDirectoryPanel";
      this.RemoveDataDirectoryPanel.Size = new System.Drawing.Size(840, 147);
      this.RemoveDataDirectoryPanel.TabIndex = 8;
      // 
      // RemoveDataDirectoryCheckBox
      // 
      this.RemoveDataDirectoryCheckBox.AccessibleDescription = "A check box to confirm the removal of the data directory";
      this.RemoveDataDirectoryCheckBox.AccessibleName = "Remove Data Directory";
      this.RemoveDataDirectoryCheckBox.AutoSize = true;
      this.RemoveDataDirectoryCheckBox.Checked = true;
      this.RemoveDataDirectoryCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
      this.RemoveDataDirectoryCheckBox.Location = new System.Drawing.Point(36, 108);
      this.RemoveDataDirectoryCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.RemoveDataDirectoryCheckBox.Name = "RemoveDataDirectoryCheckBox";
      this.RemoveDataDirectoryCheckBox.Size = new System.Drawing.Size(238, 27);
      this.RemoveDataDirectoryCheckBox.TabIndex = 2;
      this.RemoveDataDirectoryCheckBox.Text = "Remove the data directory";
      this.RemoveDataDirectoryCheckBox.UseVisualStyleBackColor = true;
      // 
      // RemoveDataDirectoryLabel
      // 
      this.RemoveDataDirectoryLabel.AccessibleDescription = "A label displaying a description asking the user if the data directory should be " +
    "removed";
      this.RemoveDataDirectoryLabel.AccessibleName = "Remove Data Directory Description";
      this.RemoveDataDirectoryLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.RemoveDataDirectoryLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.RemoveDataDirectoryLabel.Location = new System.Drawing.Point(28, 0);
      this.RemoveDataDirectoryLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.RemoveDataDirectoryLabel.Name = "RemoveDataDirectoryLabel";
      this.RemoveDataDirectoryLabel.Size = new System.Drawing.Size(770, 90);
      this.RemoveDataDirectoryLabel.TabIndex = 0;
      this.RemoveDataDirectoryLabel.Text = "Do you want to remove the data directory?\r\nWhen the data directory is left untouc" +
    "hed, all data stored in the databases will persist and can be reused on a differ" +
    "ent installation. ";
      // 
      // ServerRemovePage
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 23F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Caption = "<>";
      this.Controls.Add(this.ServerRemovalFlowLayoutPanel);
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.Name = "ServerRemovePage";
      this.Size = new System.Drawing.Size(849, 1760);
      this.Controls.SetChildIndex(this.ServerRemovalFlowLayoutPanel, 0);
      this.Controls.SetChildIndex(this.captionLabel, 0);
      this.Controls.SetChildIndex(this.subCaptionLabel, 0);
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).EndInit();
      this.ServerRemovalFlowLayoutPanel.ResumeLayout(false);
      this.RemoveDataDirectoryPanel.ResumeLayout(false);
      this.RemoveDataDirectoryPanel.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.FlowLayoutPanel ServerRemovalFlowLayoutPanel;
    private System.Windows.Forms.Panel RemoveDataDirectoryPanel;
    private System.Windows.Forms.Label RemoveDataDirectoryLabel;
    private System.Windows.Forms.CheckBox RemoveDataDirectoryCheckBox;
  }
}

/* Copyright (c) 2019, Oracle and/or its affiliates. All rights reserved.

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
  partial class RemoveStepControl
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
      this.StatusLabel = new System.Windows.Forms.Label();
      this.ExpandCollapseButton = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.StepIconPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // StepIconPictureBox
      // 
      this.StepIconPictureBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.StepIconPictureBox.Location = new System.Drawing.Point(25, 7);
      // 
      // StepCaption
      // 
      this.StepCaption.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.StepCaption.Location = new System.Drawing.Point(47, 8);
      // 
      // StatusLabel
      // 
      this.StatusLabel.AccessibleDescription = "A label to display the status of this step";
      this.StatusLabel.AccessibleName = "Status";
      this.StatusLabel.AutoSize = true;
      this.StatusLabel.Location = new System.Drawing.Point(313, 7);
      this.StatusLabel.Name = "StatusLabel";
      this.StatusLabel.Size = new System.Drawing.Size(0, 15);
      this.StatusLabel.TabIndex = 3;
      // 
      // ExpandCollapseButton
      // 
      this.ExpandCollapseButton.AccessibleDescription = "A button to expand or collapse all substeps";
      this.ExpandCollapseButton.AccessibleName = "Expand Or Collapse All";
      this.ExpandCollapseButton.FlatAppearance.BorderSize = 0;
      this.ExpandCollapseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.ExpandCollapseButton.Location = new System.Drawing.Point(3, 3);
      this.ExpandCollapseButton.Name = "ExpandCollapseButton";
      this.ExpandCollapseButton.Size = new System.Drawing.Size(22, 22);
      this.ExpandCollapseButton.TabIndex = 14;
      this.ExpandCollapseButton.UseVisualStyleBackColor = true;
      this.ExpandCollapseButton.Click += new System.EventHandler(this.ExpandCollapseButton_Click);
      // 
      // RemoveStepControl
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.BackColor = System.Drawing.SystemColors.Window;
      this.Controls.Add(this.ExpandCollapseButton);
      this.Controls.Add(this.StatusLabel);
      this.Name = "RemoveStepControl";
      this.Size = new System.Drawing.Size(445, 28);
      this.Controls.SetChildIndex(this.StepIconPictureBox, 0);
      this.Controls.SetChildIndex(this.StepCaption, 0);
      this.Controls.SetChildIndex(this.StatusLabel, 0);
      this.Controls.SetChildIndex(this.ExpandCollapseButton, 0);
      ((System.ComponentModel.ISupportInitialize)(this.StepIconPictureBox)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    public System.Windows.Forms.Label StatusLabel;
    private System.Windows.Forms.Button ExpandCollapseButton;
  }
}

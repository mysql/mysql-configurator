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
  partial class ServerExampleDatabasesPage
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerExampleDatabasesPage));
      this.InstructionsLabel = new System.Windows.Forms.Label();
      this.CreateRemoveSakilaDatabaseCheckBox = new System.Windows.Forms.CheckBox();
      this.CreateRemoveWorldDatabaseCheckBox = new System.Windows.Forms.CheckBox();
      this.DynamicLabel = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).BeginInit();
      this.SuspendLayout();
      // 
      // subCaptionLabel
      // 
      this.subCaptionLabel.Location = new System.Drawing.Point(25, 66);
      this.subCaptionLabel.Size = new System.Drawing.Size(521, 18);
      this.subCaptionLabel.Text = "Configure example databases.";
      this.subCaptionLabel.Visible = false;
      // 
      // captionLabel
      // 
      this.captionLabel.Size = new System.Drawing.Size(165, 25);
      this.captionLabel.Text = "Sample Databases";
      // 
      // InstructionsLabel
      // 
      this.InstructionsLabel.AccessibleDescription = "A label displaying instructions about the wizard page";
      this.InstructionsLabel.AccessibleName = "Instructions Text";
      this.InstructionsLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.InstructionsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.InstructionsLabel.Location = new System.Drawing.Point(27, 63);
      this.InstructionsLabel.Name = "InstructionsLabel";
      this.InstructionsLabel.Size = new System.Drawing.Size(520, 51);
      this.InstructionsLabel.TabIndex = 1;
      this.InstructionsLabel.Text = "MySQL databases samples can be installed or removed as part of this configuration" +
    ". These samples provide test data to use when developing applications for the My" +
    "SQL ecosystem.";
      // 
      // CreateRemoveSakilaDatabaseCheckBox
      // 
      this.CreateRemoveSakilaDatabaseCheckBox.AccessibleDescription = "A check box to enable the creation or removal of the world sakila database";
      this.CreateRemoveSakilaDatabaseCheckBox.AccessibleName = "Create Remove Sakila Database";
      this.CreateRemoveSakilaDatabaseCheckBox.AutoSize = true;
      this.CreateRemoveSakilaDatabaseCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.CreateRemoveSakilaDatabaseCheckBox.Location = new System.Drawing.Point(33, 150);
      this.CreateRemoveSakilaDatabaseCheckBox.Name = "CreateRemoveSakilaDatabaseCheckBox";
      this.CreateRemoveSakilaDatabaseCheckBox.Size = new System.Drawing.Size(143, 19);
      this.CreateRemoveSakilaDatabaseCheckBox.TabIndex = 3;
      this.CreateRemoveSakilaDatabaseCheckBox.Text = "Create Sakila database";
      this.CreateRemoveSakilaDatabaseCheckBox.UseVisualStyleBackColor = true;
      // 
      // CreateRemoveWorldDatabaseCheckBox
      // 
      this.CreateRemoveWorldDatabaseCheckBox.AccessibleDescription = "A check box to enable the creation or removal of the world example database";
      this.CreateRemoveWorldDatabaseCheckBox.AccessibleName = "Create Remove World Database";
      this.CreateRemoveWorldDatabaseCheckBox.AutoSize = true;
      this.CreateRemoveWorldDatabaseCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.CreateRemoveWorldDatabaseCheckBox.Location = new System.Drawing.Point(33, 182);
      this.CreateRemoveWorldDatabaseCheckBox.Name = "CreateRemoveWorldDatabaseCheckBox";
      this.CreateRemoveWorldDatabaseCheckBox.Size = new System.Drawing.Size(145, 19);
      this.CreateRemoveWorldDatabaseCheckBox.TabIndex = 4;
      this.CreateRemoveWorldDatabaseCheckBox.Text = "Create World database";
      this.CreateRemoveWorldDatabaseCheckBox.UseVisualStyleBackColor = true;
      // 
      // DynamicLabel
      // 
      this.DynamicLabel.AccessibleDescription = "A label displaying dynamic instructions about the wizard page";
      this.DynamicLabel.AccessibleName = "Instructions Dynamic Text";
      this.DynamicLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.DynamicLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.DynamicLabel.Location = new System.Drawing.Point(28, 118);
      this.DynamicLabel.Name = "DynamicLabel";
      this.DynamicLabel.Size = new System.Drawing.Size(520, 33);
      this.DynamicLabel.TabIndex = 5;
      this.DynamicLabel.Text = "Select the databases that should be created:";
      // 
      // ServerExampleDatabasesPage
      // 
      this.AccessibleName = "Server Access Page";
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
      this.Caption = "Sample Databases";
      this.Controls.Add(this.DynamicLabel);
      this.Controls.Add(this.CreateRemoveWorldDatabaseCheckBox);
      this.Controls.Add(this.CreateRemoveSakilaDatabaseCheckBox);
      this.Controls.Add(this.InstructionsLabel);
      this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "ServerExampleDatabasesPage";
      this.SubCaption = "Configure example databases.";
      this.Controls.SetChildIndex(this.captionLabel, 0);
      this.Controls.SetChildIndex(this.subCaptionLabel, 0);
      this.Controls.SetChildIndex(this.InstructionsLabel, 0);
      this.Controls.SetChildIndex(this.CreateRemoveSakilaDatabaseCheckBox, 0);
      this.Controls.SetChildIndex(this.CreateRemoveWorldDatabaseCheckBox, 0);
      this.Controls.SetChildIndex(this.DynamicLabel, 0);
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.Label InstructionsLabel;
    private System.Windows.Forms.CheckBox CreateRemoveSakilaDatabaseCheckBox;
    private System.Windows.Forms.CheckBox CreateRemoveWorldDatabaseCheckBox;
    private System.Windows.Forms.Label DynamicLabel;
  }
}

/* Copyright (c) 2010, 2018, Oracle and/or its affiliates.

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

namespace MySql.Configurator.Controls
{
  partial class ResourcesLink
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
      this.DescriptionLabel = new System.Windows.Forms.Label();
      this.BulletPictureBox = new System.Windows.Forms.PictureBox();
      this.TitleLinkLabel = new System.Windows.Forms.LinkLabel();
      ((System.ComponentModel.ISupportInitialize)(this.BulletPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // DescriptionLabel
      // 
      this.DescriptionLabel.AccessibleDescription = "A label showing a description about a resource";
      this.DescriptionLabel.AccessibleName = "Resource Description";
      this.DescriptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.DescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.DescriptionLabel.Location = new System.Drawing.Point(26, 30);
      this.DescriptionLabel.Name = "DescriptionLabel";
      this.DescriptionLabel.Size = new System.Drawing.Size(369, 37);
      this.DescriptionLabel.TabIndex = 1;
      this.DescriptionLabel.Text = "label2";
      // 
      // BulletPictureBox
      // 
      this.BulletPictureBox.AccessibleDescription = "A picture denoting a list bullet";
      this.BulletPictureBox.AccessibleName = "Resource Bullet";
      this.BulletPictureBox.Image = global::MySql.Configurator.Properties.Resources.link_arrow;
      this.BulletPictureBox.Location = new System.Drawing.Point(6, 8);
      this.BulletPictureBox.Name = "BulletPictureBox";
      this.BulletPictureBox.Size = new System.Drawing.Size(9, 9);
      this.BulletPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.BulletPictureBox.TabIndex = 7;
      this.BulletPictureBox.TabStop = false;
      // 
      // TitleLinkLabel
      // 
      this.TitleLinkLabel.AccessibleDescription = "A label showing a resource title";
      this.TitleLinkLabel.AccessibleName = "Resource Title";
      this.TitleLinkLabel.AutoSize = true;
      this.TitleLinkLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
      this.TitleLinkLabel.LinkColor = System.Drawing.Color.SteelBlue;
      this.TitleLinkLabel.Location = new System.Drawing.Point(26, 3);
      this.TitleLinkLabel.Name = "TitleLinkLabel";
      this.TitleLinkLabel.Size = new System.Drawing.Size(89, 19);
      this.TitleLinkLabel.TabIndex = 0;
      this.TitleLinkLabel.TabStop = true;
      this.TitleLinkLabel.Text = "MySQL.com";
      this.TitleLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.TitleLinkLabel_LinkClicked);
      // 
      // ResourcesLink
      // 
      this.AccessibleDescription = "A custom control to display a help resource link and description";
      this.AccessibleName = "Resource Link";
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
      this.BackColor = System.Drawing.Color.White;
      this.Controls.Add(this.DescriptionLabel);
      this.Controls.Add(this.BulletPictureBox);
      this.Controls.Add(this.TitleLinkLabel);
      this.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.Name = "ResourcesLink";
      this.Size = new System.Drawing.Size(398, 75);
      ((System.ComponentModel.ISupportInitialize)(this.BulletPictureBox)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label DescriptionLabel;
    private System.Windows.Forms.PictureBox BulletPictureBox;
    private System.Windows.Forms.LinkLabel TitleLinkLabel;
  }
}

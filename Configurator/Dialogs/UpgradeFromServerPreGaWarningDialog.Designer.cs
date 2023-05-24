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

namespace MySql.Configurator.Dialogs
{
  partial class UpgradeFromServerPreGaWarningDialog
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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpgradeFromServerPreGaWarningDialog));
      this.OkButton = new System.Windows.Forms.Button();
      this.LogoPictureBox = new System.Windows.Forms.PictureBox();
      this.DescriptionLabel = new System.Windows.Forms.Label();
      this.TitleLabel = new System.Windows.Forms.Label();
      this.ReleaseNotesLinkLabel = new System.Windows.Forms.LinkLabel();
      this.DisclaimerLabel = new System.Windows.Forms.Label();
      this.ContentAreaPanel.SuspendLayout();
      this.CommandAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // FootnoteAreaPanel
      // 
      this.FootnoteAreaPanel.Location = new System.Drawing.Point(0, 292);
      this.FootnoteAreaPanel.Size = new System.Drawing.Size(634, 0);
      // 
      // ContentAreaPanel
      // 
      this.ContentAreaPanel.Controls.Add(this.DisclaimerLabel);
      this.ContentAreaPanel.Controls.Add(this.ReleaseNotesLinkLabel);
      this.ContentAreaPanel.Controls.Add(this.TitleLabel);
      this.ContentAreaPanel.Controls.Add(this.DescriptionLabel);
      this.ContentAreaPanel.Controls.Add(this.LogoPictureBox);
      this.ContentAreaPanel.Size = new System.Drawing.Size(707, 422);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.Controls.Add(this.OkButton);
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 377);
      this.CommandAreaPanel.Size = new System.Drawing.Size(707, 45);
      // 
      // OkButton
      // 
      this.OkButton.AccessibleDescription = "A button to acknowledge the information and close the dialog";
      this.OkButton.AccessibleName = "OK";
      this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.OkButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.OkButton.Location = new System.Drawing.Point(620, 11);
      this.OkButton.Name = "OkButton";
      this.OkButton.Size = new System.Drawing.Size(75, 23);
      this.OkButton.TabIndex = 0;
      this.OkButton.Text = "OK";
      this.OkButton.UseVisualStyleBackColor = true;
      // 
      // LogoPictureBox
      // 
      this.LogoPictureBox.AccessibleDescription = "A picture box displaying the MySQL Configurator logo";
      this.LogoPictureBox.AccessibleName = "Logo";
      this.LogoPictureBox.Image = global::MySql.Configurator.Properties.Resources.MainLogo_Warn;
      this.LogoPictureBox.Location = new System.Drawing.Point(22, 25);
      this.LogoPictureBox.Name = "LogoPictureBox";
      this.LogoPictureBox.Size = new System.Drawing.Size(100, 100);
      this.LogoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.LogoPictureBox.TabIndex = 2;
      this.LogoPictureBox.TabStop = false;
      // 
      // DescriptionLabel
      // 
      this.DescriptionLabel.AccessibleDescription = "A label displaying a description about the dialog\'s purpose";
      this.DescriptionLabel.AccessibleName = "Description Text";
      this.DescriptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.DescriptionLabel.Location = new System.Drawing.Point(141, 59);
      this.DescriptionLabel.Name = "DescriptionLabel";
      this.DescriptionLabel.Size = new System.Drawing.Size(554, 226);
      this.DescriptionLabel.TabIndex = 3;
      this.DescriptionLabel.Text = resources.GetString("DescriptionLabel.Text");
      // 
      // TitleLabel
      // 
      this.TitleLabel.AccessibleDescription = "A label displaying a title text for the dialog";
      this.TitleLabel.AccessibleName = "Title";
      this.TitleLabel.AutoSize = true;
      this.TitleLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TitleLabel.ForeColor = System.Drawing.Color.Navy;
      this.TitleLabel.Location = new System.Drawing.Point(140, 25);
      this.TitleLabel.Name = "TitleLabel";
      this.TitleLabel.Size = new System.Drawing.Size(368, 20);
      this.TitleLabel.TabIndex = 4;
      this.TitleLabel.Text = "Upgrading the MySQL Server from a milestone release";
      // 
      // ReleaseNotesLinkLabel
      // 
      this.ReleaseNotesLinkLabel.AccessibleDescription = "A link label to open a web page with the release notes of the newer MySQL Server " +
    "version";
      this.ReleaseNotesLinkLabel.AccessibleName = "Release Notes";
      this.ReleaseNotesLinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.ReleaseNotesLinkLabel.AutoSize = true;
      this.ReleaseNotesLinkLabel.Location = new System.Drawing.Point(141, 287);
      this.ReleaseNotesLinkLabel.Name = "ReleaseNotesLinkLabel";
      this.ReleaseNotesLinkLabel.Size = new System.Drawing.Size(181, 15);
      this.ReleaseNotesLinkLabel.TabIndex = 5;
      this.ReleaseNotesLinkLabel.TabStop = true;
      this.ReleaseNotesLinkLabel.Text = "MySQL Server x.y.z Release Notes";
      this.ReleaseNotesLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ReleaseNotesLinkLabel_LinkClicked);
      // 
      // DisclaimerLabel
      // 
      this.DisclaimerLabel.AccessibleDescription = "A label displaying a disclaimer about upgrading at your own risk";
      this.DisclaimerLabel.AccessibleName = "Disclaimer Text";
      this.DisclaimerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.DisclaimerLabel.Location = new System.Drawing.Point(141, 312);
      this.DisclaimerLabel.Name = "DisclaimerLabel";
      this.DisclaimerLabel.Size = new System.Drawing.Size(554, 35);
      this.DisclaimerLabel.TabIndex = 6;
      this.DisclaimerLabel.Text = "The MySQL Server upgrade will be deselected now. To procede with the upgrade plea" +
    "se select it again knowing you will be doing so at your own risk.";
      // 
      // UpgradeFromServerPreGaWarningDialog
      // 
      this.AcceptButton = this.OkButton;
      this.AccessibleDescription = "A modal dialog shown when upgrading MySQL Server from a milestone release";
      this.AccessibleName = "Upgrading from MySQL Server milestone release";
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(707, 422);
      this.CommandAreaVisible = true;
      this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FootnoteAreaHeight = 0;
      this.Name = "UpgradeFromServerPreGaWarningDialog";
      this.Text = "Upgrading from MySQL Server milestone release";
      this.ContentAreaPanel.ResumeLayout(false);
      this.ContentAreaPanel.PerformLayout();
      this.CommandAreaPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button OkButton;
    private System.Windows.Forms.PictureBox LogoPictureBox;
    private System.Windows.Forms.Label DescriptionLabel;
    private System.Windows.Forms.Label TitleLabel;
    private System.Windows.Forms.Label DisclaimerLabel;
    private System.Windows.Forms.LinkLabel ReleaseNotesLinkLabel;
  }
}
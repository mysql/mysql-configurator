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

namespace MySql.Configurator.Core.Controls
{
  partial class RootCredentialsCheckControl
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
      this.ResultsPanel = new System.Windows.Forms.Panel();
      this.ResultLabel = new System.Windows.Forms.Label();
      this.ResultPictureBox = new System.Windows.Forms.PictureBox();
      this.CheckButton = new System.Windows.Forms.Button();
      this.UpgradePasswordLabel = new System.Windows.Forms.Label();
      this.UpgradeUserNameLabel = new System.Windows.Forms.Label();
      this.ExistingRootPasswordTextBox = new System.Windows.Forms.TextBox();
      this.UpgradeUserNameValueLabel = new System.Windows.Forms.Label();
      this.ResultsPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ResultPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // ResultsPanel
      // 
      this.ResultsPanel.Controls.Add(this.ResultLabel);
      this.ResultsPanel.Controls.Add(this.ResultPictureBox);
      this.ResultsPanel.Location = new System.Drawing.Point(153, 60);
      this.ResultsPanel.Name = "ResultsPanel";
      this.ResultsPanel.Size = new System.Drawing.Size(371, 23);
      this.ResultsPanel.TabIndex = 5;
      this.ResultsPanel.Visible = false;
      // 
      // ResultLabel
      // 
      this.ResultLabel.AutoSize = true;
      this.ResultLabel.Location = new System.Drawing.Point(25, 5);
      this.ResultLabel.Name = "ResultLabel";
      this.ResultLabel.Size = new System.Drawing.Size(38, 13);
      this.ResultLabel.TabIndex = 0;
      this.ResultLabel.Text = "label1";
      // 
      // ResultPictureBox
      // 
      this.ResultPictureBox.Location = new System.Drawing.Point(4, 2);
      this.ResultPictureBox.Name = "ResultPictureBox";
      this.ResultPictureBox.Size = new System.Drawing.Size(16, 16);
      this.ResultPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.ResultPictureBox.TabIndex = 17;
      this.ResultPictureBox.TabStop = false;
      // 
      // CheckButton
      // 
      this.CheckButton.Location = new System.Drawing.Point(72, 60);
      this.CheckButton.Name = "CheckButton";
      this.CheckButton.Size = new System.Drawing.Size(75, 23);
      this.CheckButton.TabIndex = 4;
      this.CheckButton.Text = "Check";
      this.CheckButton.UseVisualStyleBackColor = true;
      this.CheckButton.Click += new System.EventHandler(this.CheckButton_Click);
      // 
      // UpgradePasswordLabel
      // 
      this.UpgradePasswordLabel.AutoSize = true;
      this.UpgradePasswordLabel.Location = new System.Drawing.Point(7, 37);
      this.UpgradePasswordLabel.Name = "UpgradePasswordLabel";
      this.UpgradePasswordLabel.Size = new System.Drawing.Size(59, 13);
      this.UpgradePasswordLabel.TabIndex = 2;
      this.UpgradePasswordLabel.Text = "Password:";
      // 
      // UpgradeUserNameLabel
      // 
      this.UpgradeUserNameLabel.AutoSize = true;
      this.UpgradeUserNameLabel.Location = new System.Drawing.Point(33, 12);
      this.UpgradeUserNameLabel.Name = "UpgradeUserNameLabel";
      this.UpgradeUserNameLabel.Size = new System.Drawing.Size(33, 13);
      this.UpgradeUserNameLabel.TabIndex = 0;
      this.UpgradeUserNameLabel.Text = "User:";
      // 
      // ExistingRootPasswordTextBox
      // 
      this.ExistingRootPasswordTextBox.Location = new System.Drawing.Point(72, 32);
      this.ExistingRootPasswordTextBox.Name = "ExistingRootPasswordTextBox";
      this.ExistingRootPasswordTextBox.PasswordChar = '*';
      this.ExistingRootPasswordTextBox.Size = new System.Drawing.Size(217, 22);
      this.ExistingRootPasswordTextBox.TabIndex = 3;
      this.ExistingRootPasswordTextBox.UseSystemPasswordChar = true;
      // 
      // UpgradeUserNameValueLabel
      // 
      this.UpgradeUserNameValueLabel.AutoSize = true;
      this.UpgradeUserNameValueLabel.Location = new System.Drawing.Point(72, 12);
      this.UpgradeUserNameValueLabel.Name = "UpgradeUserNameValueLabel";
      this.UpgradeUserNameValueLabel.Size = new System.Drawing.Size(87, 13);
      this.UpgradeUserNameValueLabel.TabIndex = 1;
      this.UpgradeUserNameValueLabel.Text = "root@localhost";
      // 
      // RootCredentialsCheckControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.Transparent;
      this.Controls.Add(this.ResultsPanel);
      this.Controls.Add(this.CheckButton);
      this.Controls.Add(this.UpgradePasswordLabel);
      this.Controls.Add(this.UpgradeUserNameLabel);
      this.Controls.Add(this.ExistingRootPasswordTextBox);
      this.Controls.Add(this.UpgradeUserNameValueLabel);
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "RootCredentialsCheckControl";
      this.Size = new System.Drawing.Size(534, 92);
      this.ResultsPanel.ResumeLayout(false);
      this.ResultsPanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ResultPictureBox)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Panel ResultsPanel;
    private System.Windows.Forms.Label ResultLabel;
    private System.Windows.Forms.PictureBox ResultPictureBox;
    private System.Windows.Forms.Button CheckButton;
    private System.Windows.Forms.Label UpgradePasswordLabel;
    private System.Windows.Forms.Label UpgradeUserNameLabel;
    private System.Windows.Forms.TextBox ExistingRootPasswordTextBox;
    private System.Windows.Forms.Label UpgradeUserNameValueLabel;
  }
}

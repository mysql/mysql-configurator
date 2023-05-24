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

namespace MySql.Configurator.Core.Dialogs
{
  partial class BaseAdvancedOptionsDialog
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
      this.DialogCancelButton = new System.Windows.Forms.Button();
      this.OkButton = new System.Windows.Forms.Button();
      this.InstallDirErrorPictureBox = new System.Windows.Forms.PictureBox();
      this.InstallDirWarningLabel = new System.Windows.Forms.Label();
      this.InstallDirWarningPictureBox = new System.Windows.Forms.PictureBox();
      this.InstallDirRevertButton = new System.Windows.Forms.Button();
      this.InstallDirBrowseButton = new System.Windows.Forms.Button();
      this.InstallDirLabel = new System.Windows.Forms.Label();
      this.InstallDirTextBox = new System.Windows.Forms.TextBox();
      this.ContentAreaPanel.SuspendLayout();
      this.CommandAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.InstallDirErrorPictureBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.InstallDirWarningPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // FootnoteAreaPanel
      // 
      this.FootnoteAreaPanel.Location = new System.Drawing.Point(0, 449);
      this.FootnoteAreaPanel.Size = new System.Drawing.Size(951, 0);
      // 
      // ContentAreaPanel
      // 
      this.ContentAreaPanel.Controls.Add(this.InstallDirErrorPictureBox);
      this.ContentAreaPanel.Controls.Add(this.InstallDirWarningLabel);
      this.ContentAreaPanel.Controls.Add(this.InstallDirWarningPictureBox);
      this.ContentAreaPanel.Controls.Add(this.InstallDirRevertButton);
      this.ContentAreaPanel.Controls.Add(this.InstallDirBrowseButton);
      this.ContentAreaPanel.Controls.Add(this.InstallDirLabel);
      this.ContentAreaPanel.Controls.Add(this.InstallDirTextBox);
      this.ContentAreaPanel.Size = new System.Drawing.Size(502, 172);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.Controls.Add(this.OkButton);
      this.CommandAreaPanel.Controls.Add(this.DialogCancelButton);
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 127);
      this.CommandAreaPanel.Size = new System.Drawing.Size(502, 45);
      // 
      // DialogCancelButton
      // 
      this.DialogCancelButton.AccessibleDescription = "A button to dismiss the dialog";
      this.DialogCancelButton.AccessibleName = "Cancel";
      this.DialogCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.DialogCancelButton.Location = new System.Drawing.Point(415, 10);
      this.DialogCancelButton.Name = "DialogCancelButton";
      this.DialogCancelButton.Size = new System.Drawing.Size(75, 23);
      this.DialogCancelButton.TabIndex = 1;
      this.DialogCancelButton.Text = "Cancel";
      this.DialogCancelButton.UseVisualStyleBackColor = true;
      // 
      // OkButton
      // 
      this.OkButton.AccessibleDescription = "A button to apply the changes done to the advanced install options";
      this.OkButton.AccessibleName = "OK";
      this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.OkButton.Location = new System.Drawing.Point(334, 10);
      this.OkButton.Name = "OkButton";
      this.OkButton.Size = new System.Drawing.Size(75, 23);
      this.OkButton.TabIndex = 0;
      this.OkButton.Text = "OK";
      this.OkButton.UseVisualStyleBackColor = true;
      // 
      // InstallDirErrorPictureBox
      // 
      this.InstallDirErrorPictureBox.AccessibleDescription = "A picture box displaying an error icon for an invalid install directory";
      this.InstallDirErrorPictureBox.AccessibleName = "Install Directory Error Icon";
      this.InstallDirErrorPictureBox.Image = global::MySql.Configurator.Properties.Resources.error_sign;
      this.InstallDirErrorPictureBox.Location = new System.Drawing.Point(15, 77);
      this.InstallDirErrorPictureBox.Name = "InstallDirErrorPictureBox";
      this.InstallDirErrorPictureBox.Size = new System.Drawing.Size(16, 16);
      this.InstallDirErrorPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.InstallDirErrorPictureBox.TabIndex = 57;
      this.InstallDirErrorPictureBox.TabStop = false;
      this.InstallDirErrorPictureBox.Visible = false;
      // 
      // InstallDirWarningLabel
      // 
      this.InstallDirWarningLabel.AccessibleDescription = "A label displaying a message about a warning or error in the install directory va" +
    "lidation";
      this.InstallDirWarningLabel.AccessibleName = "Install Directory Validation Text";
      this.InstallDirWarningLabel.AutoSize = true;
      this.InstallDirWarningLabel.Location = new System.Drawing.Point(34, 79);
      this.InstallDirWarningLabel.Name = "InstallDirWarningLabel";
      this.InstallDirWarningLabel.Size = new System.Drawing.Size(0, 25);
      this.InstallDirWarningLabel.TabIndex = 4;
      this.InstallDirWarningLabel.Visible = false;
      // 
      // InstallDirWarningPictureBox
      // 
      this.InstallDirWarningPictureBox.AccessibleDescription = "A picture box displaying a warning icon for the install directory";
      this.InstallDirWarningPictureBox.AccessibleName = "Install Directory Warning Icon";
      this.InstallDirWarningPictureBox.Image = global::MySql.Configurator.Properties.Resources.warning_sign;
      this.InstallDirWarningPictureBox.Location = new System.Drawing.Point(15, 77);
      this.InstallDirWarningPictureBox.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
      this.InstallDirWarningPictureBox.Name = "InstallDirWarningPictureBox";
      this.InstallDirWarningPictureBox.Size = new System.Drawing.Size(16, 16);
      this.InstallDirWarningPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.InstallDirWarningPictureBox.TabIndex = 56;
      this.InstallDirWarningPictureBox.TabStop = false;
      this.InstallDirWarningPictureBox.Visible = false;
      // 
      // InstallDirRevertButton
      // 
      this.InstallDirRevertButton.AccessibleDescription = "A button to revert the value of the install directory";
      this.InstallDirRevertButton.AccessibleName = "Install Directory Revert";
      this.InstallDirRevertButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.InstallDirRevertButton.BackgroundImage = global::MySql.Configurator.Properties.Resources.Revert;
      this.InstallDirRevertButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
      this.InstallDirRevertButton.FlatAppearance.BorderSize = 0;
      this.InstallDirRevertButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.InstallDirRevertButton.Location = new System.Drawing.Point(461, 25);
      this.InstallDirRevertButton.Margin = new System.Windows.Forms.Padding(0);
      this.InstallDirRevertButton.Name = "InstallDirRevertButton";
      this.InstallDirRevertButton.Size = new System.Drawing.Size(29, 20);
      this.InstallDirRevertButton.TabIndex = 1;
      this.InstallDirRevertButton.UseVisualStyleBackColor = true;
      this.InstallDirRevertButton.Click += new System.EventHandler(this.InstallDirRevertButton_Click);
      // 
      // InstallDirBrowseButton
      // 
      this.InstallDirBrowseButton.AccessibleDescription = "A button to open a dialog to browse through the file system and select a director" +
    "y";
      this.InstallDirBrowseButton.AccessibleName = "Install Directory Browse";
      this.InstallDirBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.InstallDirBrowseButton.Location = new System.Drawing.Point(461, 47);
      this.InstallDirBrowseButton.Name = "InstallDirBrowseButton";
      this.InstallDirBrowseButton.Size = new System.Drawing.Size(29, 22);
      this.InstallDirBrowseButton.TabIndex = 3;
      this.InstallDirBrowseButton.Text = "...";
      this.InstallDirBrowseButton.UseVisualStyleBackColor = true;
      this.InstallDirBrowseButton.Click += new System.EventHandler(this.InstallDirBrowseButton_Click);
      // 
      // InstallDirLabel
      // 
      this.InstallDirLabel.AccessibleDescription = "A label displaying the text install directory";
      this.InstallDirLabel.AccessibleName = "Install Directory Text";
      this.InstallDirLabel.AutoSize = true;
      this.InstallDirLabel.Location = new System.Drawing.Point(12, 28);
      this.InstallDirLabel.Name = "InstallDirLabel";
      this.InstallDirLabel.Size = new System.Drawing.Size(139, 25);
      this.InstallDirLabel.TabIndex = 0;
      this.InstallDirLabel.Text = "Install Directory:";
      // 
      // InstallDirTextBox
      // 
      this.InstallDirTextBox.AccessibleDescription = "A text box to set the full absolute path of the install directory";
      this.InstallDirTextBox.AccessibleName = "Install Directory";
      this.InstallDirTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.InstallDirTextBox.Location = new System.Drawing.Point(12, 48);
      this.InstallDirTextBox.Name = "InstallDirTextBox";
      this.InstallDirTextBox.Size = new System.Drawing.Size(443, 31);
      this.InstallDirTextBox.TabIndex = 2;
      this.InstallDirTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.InstallDirTextBox_Validating);
      // 
      // BaseAdvancedOptionsDialog
      // 
      this.AcceptButton = this.OkButton;
      this.AccessibleDescription = "A modal dialog showing advanced install options";
      this.AccessibleName = "Advanced Install Options Dialog";
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.CancelButton = this.DialogCancelButton;
      this.ClientSize = new System.Drawing.Size(502, 172);
      this.CommandAreaHeight = 45;
      this.CommandAreaVisible = true;
      this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FootnoteAreaHeight = 0;
      this.Name = "BaseAdvancedOptionsDialog";
      this.Text = "Advanced Options";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BaseAdvancedOptionsDialog_FormClosing);
      this.ContentAreaPanel.ResumeLayout(false);
      this.ContentAreaPanel.PerformLayout();
      this.CommandAreaPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.InstallDirErrorPictureBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.InstallDirWarningPictureBox)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion
    private System.Windows.Forms.Button DialogCancelButton;
    private System.Windows.Forms.PictureBox InstallDirErrorPictureBox;
    private System.Windows.Forms.Label InstallDirWarningLabel;
    private System.Windows.Forms.PictureBox InstallDirWarningPictureBox;
    private System.Windows.Forms.Button InstallDirRevertButton;
    private System.Windows.Forms.Button InstallDirBrowseButton;
    private System.Windows.Forms.Label InstallDirLabel;
    private System.Windows.Forms.TextBox InstallDirTextBox;
    protected System.Windows.Forms.Button OkButton;
  }
}

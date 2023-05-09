/* Copyright (c) 2014, 2023, Oracle and/or its affiliates.

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
  partial class NewUserControl
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewUserControl));
      this.MainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
      this.PasswordStrengthPanel = new System.Windows.Forms.Panel();
      this.PasswordStrengthValueLabel = new System.Windows.Forms.Label();
      this.PasswordStrengthLabel = new System.Windows.Forms.Label();
      this.PasswordMinLengthLabel = new System.Windows.Forms.Label();
      this.UsernameTextBox = new System.Windows.Forms.TextBox();
      this.UsernameInfoPictureBox = new System.Windows.Forms.PictureBox();
      this.PasswordLabel = new System.Windows.Forms.Label();
      this.PasswordTextBox = new System.Windows.Forms.TextBox();
      this.PasswordErrorPictureBox = new System.Windows.Forms.PictureBox();
      this.RepeatPasswordTextBox = new System.Windows.Forms.TextBox();
      this.RepeatPasswordLabel = new System.Windows.Forms.Label();
      this.UsernameLabel = new System.Windows.Forms.Label();
      this.PasswordFeedbackToolTip = new System.Windows.Forms.ToolTip(this.components);
      this.UsernameFeedbackToolTip = new System.Windows.Forms.ToolTip(this.components);
      this.MainTableLayoutPanel.SuspendLayout();
      this.PasswordStrengthPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.UsernameInfoPictureBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.PasswordErrorPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // MainTableLayoutPanel
      // 
      this.MainTableLayoutPanel.AccessibleDescription = "A table layout panel containing all controls";
      this.MainTableLayoutPanel.AccessibleName = "Main Area";
      this.MainTableLayoutPanel.ColumnCount = 3;
      this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
      this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
      this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28F));
      this.MainTableLayoutPanel.Controls.Add(this.PasswordStrengthPanel, 1, 3);
      this.MainTableLayoutPanel.Controls.Add(this.UsernameTextBox, 1, 0);
      this.MainTableLayoutPanel.Controls.Add(this.UsernameInfoPictureBox, 2, 0);
      this.MainTableLayoutPanel.Controls.Add(this.PasswordLabel, 0, 1);
      this.MainTableLayoutPanel.Controls.Add(this.PasswordTextBox, 1, 1);
      this.MainTableLayoutPanel.Controls.Add(this.PasswordErrorPictureBox, 2, 1);
      this.MainTableLayoutPanel.Controls.Add(this.RepeatPasswordTextBox, 1, 2);
      this.MainTableLayoutPanel.Controls.Add(this.RepeatPasswordLabel, 0, 2);
      this.MainTableLayoutPanel.Controls.Add(this.UsernameLabel, 0, 0);
      this.MainTableLayoutPanel.Location = new System.Drawing.Point(3, 3);
      this.MainTableLayoutPanel.Name = "MainTableLayoutPanel";
      this.MainTableLayoutPanel.RowCount = 4;
      this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
      this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
      this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
      this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
      this.MainTableLayoutPanel.Size = new System.Drawing.Size(328, 92);
      this.MainTableLayoutPanel.TabIndex = 0;
      // 
      // PasswordStrengthPanel
      // 
      this.PasswordStrengthPanel.AccessibleDescription = "A panel containing controls related to the password\'s strength";
      this.PasswordStrengthPanel.AccessibleName = "Password Strength Group";
      this.PasswordStrengthPanel.Controls.Add(this.PasswordStrengthValueLabel);
      this.PasswordStrengthPanel.Controls.Add(this.PasswordStrengthLabel);
      this.PasswordStrengthPanel.Controls.Add(this.PasswordMinLengthLabel);
      this.PasswordStrengthPanel.Location = new System.Drawing.Point(123, 69);
      this.PasswordStrengthPanel.Name = "PasswordStrengthPanel";
      this.PasswordStrengthPanel.Size = new System.Drawing.Size(174, 20);
      this.PasswordStrengthPanel.TabIndex = 52;
      // 
      // PasswordStrengthValueLabel
      // 
      this.PasswordStrengthValueLabel.AccessibleDescription = "A label displaying the estimated password\'s strength";
      this.PasswordStrengthValueLabel.AccessibleName = "Password Strength";
      this.PasswordStrengthValueLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.PasswordStrengthValueLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PasswordStrengthValueLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.PasswordStrengthValueLabel.Location = new System.Drawing.Point(102, 1);
      this.PasswordStrengthValueLabel.Margin = new System.Windows.Forms.Padding(0);
      this.PasswordStrengthValueLabel.Name = "PasswordStrengthValueLabel";
      this.PasswordStrengthValueLabel.Size = new System.Drawing.Size(72, 22);
      this.PasswordStrengthValueLabel.TabIndex = 0;
      this.PasswordStrengthValueLabel.Text = "Blank";
      this.PasswordStrengthValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.PasswordStrengthValueLabel.Visible = false;
      // 
      // PasswordStrengthLabel
      // 
      this.PasswordStrengthLabel.AccessibleDescription = "A label displaying the text password strength";
      this.PasswordStrengthLabel.AccessibleName = "Password Strength Text";
      this.PasswordStrengthLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.PasswordStrengthLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.PasswordStrengthLabel.Location = new System.Drawing.Point(0, 1);
      this.PasswordStrengthLabel.Margin = new System.Windows.Forms.Padding(0);
      this.PasswordStrengthLabel.Name = "PasswordStrengthLabel";
      this.PasswordStrengthLabel.Size = new System.Drawing.Size(102, 22);
      this.PasswordStrengthLabel.TabIndex = 1;
      this.PasswordStrengthLabel.Text = "Password Strength:";
      this.PasswordStrengthLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.PasswordStrengthLabel.Visible = false;
      // 
      // PasswordMinLengthLabel
      // 
      this.PasswordMinLengthLabel.AccessibleDescription = "A label displaying text about the password\'s minimum length";
      this.PasswordMinLengthLabel.AccessibleName = "Password Minimum Length";
      this.PasswordMinLengthLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PasswordMinLengthLabel.Location = new System.Drawing.Point(2, 1);
      this.PasswordMinLengthLabel.Name = "PasswordMinLengthLabel";
      this.PasswordMinLengthLabel.Size = new System.Drawing.Size(142, 22);
      this.PasswordMinLengthLabel.TabIndex = 2;
      this.PasswordMinLengthLabel.Text = "Password minimum length: 4";
      this.PasswordMinLengthLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.PasswordMinLengthLabel.Visible = false;
      // 
      // UsernameTextBox
      // 
      this.UsernameTextBox.AccessibleDescription = "A text box to input the user name";
      this.UsernameTextBox.AccessibleName = "User Name";
      this.UsernameTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.UsernameTextBox.Location = new System.Drawing.Point(123, 3);
      this.UsernameTextBox.Name = "UsernameTextBox";
      this.UsernameTextBox.Size = new System.Drawing.Size(144, 23);
      this.UsernameTextBox.TabIndex = 1;
      this.UsernameTextBox.TextChanged += new System.EventHandler(this.usernameTextBox_TextChanged);
      // 
      // UsernameInfoPictureBox
      // 
      this.UsernameInfoPictureBox.AccessibleDescription = "A picture box displaying an information icon related to the user name";
      this.UsernameInfoPictureBox.AccessibleName = "User Name Info Icon";
      this.UsernameInfoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("UsernameInfoPictureBox.Image")));
      this.UsernameInfoPictureBox.Location = new System.Drawing.Point(303, 3);
      this.UsernameInfoPictureBox.Name = "UsernameInfoPictureBox";
      this.UsernameInfoPictureBox.Size = new System.Drawing.Size(16, 16);
      this.UsernameInfoPictureBox.TabIndex = 33;
      this.UsernameInfoPictureBox.TabStop = false;
      this.UsernameInfoPictureBox.Visible = false;
      // 
      // PasswordLabel
      // 
      this.PasswordLabel.AccessibleDescription = "A label displaying the text password";
      this.PasswordLabel.AccessibleName = "Password Text";
      this.PasswordLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.PasswordLabel.Location = new System.Drawing.Point(3, 22);
      this.PasswordLabel.Name = "PasswordLabel";
      this.PasswordLabel.Size = new System.Drawing.Size(114, 22);
      this.PasswordLabel.TabIndex = 2;
      this.PasswordLabel.Text = "Password:";
      this.PasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // PasswordTextBox
      // 
      this.PasswordTextBox.AccessibleDescription = "A text box to input the password";
      this.PasswordTextBox.AccessibleName = "Password";
      this.PasswordTextBox.Enabled = false;
      this.PasswordTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PasswordTextBox.Location = new System.Drawing.Point(123, 25);
      this.PasswordTextBox.Name = "PasswordTextBox";
      this.PasswordTextBox.Size = new System.Drawing.Size(144, 23);
      this.PasswordTextBox.TabIndex = 3;
      this.PasswordTextBox.UseSystemPasswordChar = true;
      this.PasswordTextBox.TextChanged += new System.EventHandler(this.RepeatPasswordTextBox_TextChanged);
      // 
      // PasswordErrorPictureBox
      // 
      this.PasswordErrorPictureBox.AccessibleDescription = "A picture box displaying a warning icon to signal a problem with the password";
      this.PasswordErrorPictureBox.AccessibleName = "Password Warning Icon";
      this.PasswordErrorPictureBox.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.PasswordErrorPictureBox.Image = global::MySql.Configurator.Properties.Resources.warning_sign;
      this.PasswordErrorPictureBox.Location = new System.Drawing.Point(306, 25);
      this.PasswordErrorPictureBox.Name = "PasswordErrorPictureBox";
      this.PasswordErrorPictureBox.Size = new System.Drawing.Size(16, 16);
      this.PasswordErrorPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.PasswordErrorPictureBox.TabIndex = 31;
      this.PasswordErrorPictureBox.TabStop = false;
      this.PasswordErrorPictureBox.Visible = false;
      // 
      // RepeatPasswordTextBox
      // 
      this.RepeatPasswordTextBox.AccessibleDescription = "A text box to input a password confirmation";
      this.RepeatPasswordTextBox.AccessibleName = "Password Confirmation";
      this.RepeatPasswordTextBox.Enabled = false;
      this.RepeatPasswordTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.RepeatPasswordTextBox.Location = new System.Drawing.Point(123, 47);
      this.RepeatPasswordTextBox.Name = "RepeatPasswordTextBox";
      this.RepeatPasswordTextBox.Size = new System.Drawing.Size(144, 23);
      this.RepeatPasswordTextBox.TabIndex = 5;
      this.RepeatPasswordTextBox.UseSystemPasswordChar = true;
      this.RepeatPasswordTextBox.TextChanged += new System.EventHandler(this.RepeatPasswordTextBox_TextChanged);
      // 
      // RepeatPasswordLabel
      // 
      this.RepeatPasswordLabel.AccessibleDescription = "A label displaying the text repeat password";
      this.RepeatPasswordLabel.AccessibleName = "RepeatPasswordLabel";
      this.RepeatPasswordLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.RepeatPasswordLabel.Location = new System.Drawing.Point(3, 44);
      this.RepeatPasswordLabel.Name = "RepeatPasswordLabel";
      this.RepeatPasswordLabel.Size = new System.Drawing.Size(114, 22);
      this.RepeatPasswordLabel.TabIndex = 4;
      this.RepeatPasswordLabel.Text = "Repeat Password:";
      this.RepeatPasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // UsernameLabel
      // 
      this.UsernameLabel.AccessibleDescription = "A label displaying the text user name";
      this.UsernameLabel.AccessibleName = "User Name Text";
      this.UsernameLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.UsernameLabel.Location = new System.Drawing.Point(3, 0);
      this.UsernameLabel.Name = "UsernameLabel";
      this.UsernameLabel.Size = new System.Drawing.Size(114, 22);
      this.UsernameLabel.TabIndex = 0;
      this.UsernameLabel.Text = "User Name:";
      this.UsernameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // NewUserControl
      // 
      this.AccessibleDescription = "A control to setup a new MySQL user";
      this.AccessibleName = "New User Control";
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.BackColor = System.Drawing.Color.Transparent;
      this.Controls.Add(this.MainTableLayoutPanel);
      this.Name = "NewUserControl";
      this.Size = new System.Drawing.Size(334, 98);
      this.MainTableLayoutPanel.ResumeLayout(false);
      this.MainTableLayoutPanel.PerformLayout();
      this.PasswordStrengthPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.UsernameInfoPictureBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.PasswordErrorPictureBox)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;
    private System.Windows.Forms.Panel PasswordStrengthPanel;
    private System.Windows.Forms.Label PasswordMinLengthLabel;
    private System.Windows.Forms.Label PasswordStrengthLabel;
    private System.Windows.Forms.Label PasswordStrengthValueLabel;
    private System.Windows.Forms.TextBox UsernameTextBox;
    private System.Windows.Forms.PictureBox UsernameInfoPictureBox;
    private System.Windows.Forms.ToolTip PasswordFeedbackToolTip;
    private System.Windows.Forms.Label PasswordLabel;
    private System.Windows.Forms.TextBox PasswordTextBox;
    private System.Windows.Forms.PictureBox PasswordErrorPictureBox;
    private System.Windows.Forms.TextBox RepeatPasswordTextBox;
    private System.Windows.Forms.Label RepeatPasswordLabel;
    private System.Windows.Forms.Label UsernameLabel;
    private System.Windows.Forms.ToolTip UsernameFeedbackToolTip;


  }
}

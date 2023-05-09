/* Copyright (c) 2011, 2019, Oracle and/or its affiliates.

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

namespace MySql.Configurator.Wizards.Server{
  partial class ServerConfigAdvancedOptionsPage
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
      this.ServerIdLabel = new System.Windows.Forms.Label();
      this.ServerIdTextBox = new System.Windows.Forms.TextBox();
      this.LowerCaseTableNamesLabel = new System.Windows.Forms.Label();
      this.LowerCaseRadioButton = new System.Windows.Forms.RadioButton();
      this.PreserveGivenCaseRadioButton = new System.Windows.Forms.RadioButton();
      this.LowerCaseDescriptionLabel = new System.Windows.Forms.Label();
      this.PreserveGivenCaseDescriptionLabel = new System.Windows.Forms.Label();
      this.ServerIdDescriptionLabel = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).BeginInit();
      this.SuspendLayout();
      // 
      // subCaptionLabel
      // 
      this.subCaptionLabel.Location = new System.Drawing.Point(27, 67);
      this.subCaptionLabel.Size = new System.Drawing.Size(495, 10);
      this.subCaptionLabel.Text = "";
      this.subCaptionLabel.Visible = false;
      // 
      // captionLabel
      // 
      this.captionLabel.Size = new System.Drawing.Size(166, 25);
      this.captionLabel.Text = "Advanced Options";
      // 
      // ServerIdLabel
      // 
      this.ServerIdLabel.AccessibleDescription = "A label displaying the text server ID";
      this.ServerIdLabel.AccessibleName = "Server ID Text";
      this.ServerIdLabel.AutoSize = true;
      this.ServerIdLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ServerIdLabel.Location = new System.Drawing.Point(27, 91);
      this.ServerIdLabel.Name = "ServerIdLabel";
      this.ServerIdLabel.Size = new System.Drawing.Size(56, 15);
      this.ServerIdLabel.TabIndex = 2;
      this.ServerIdLabel.Text = "Server ID:";
      // 
      // ServerIdTextBox
      // 
      this.ServerIdTextBox.AccessibleDescription = "A text box to input the server ID";
      this.ServerIdTextBox.AccessibleName = "Server ID";
      this.ServerIdTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ServerIdTextBox.Location = new System.Drawing.Point(89, 88);
      this.ServerIdTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 6, 3);
      this.ServerIdTextBox.Name = "ServerIdTextBox";
      this.ServerIdTextBox.Size = new System.Drawing.Size(74, 23);
      this.ServerIdTextBox.TabIndex = 3;
      this.ServerIdTextBox.TextChanged += new System.EventHandler(this.TextChangedHandler);
      this.ServerIdTextBox.Validated += new System.EventHandler(this.ValidatedHandler);
      // 
      // LowerCaseTableNamesLabel
      // 
      this.LowerCaseTableNamesLabel.AccessibleDescription = "A label displaying the text table names case";
      this.LowerCaseTableNamesLabel.AccessibleName = "Table Names Case Text";
      this.LowerCaseTableNamesLabel.AutoSize = true;
      this.LowerCaseTableNamesLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.LowerCaseTableNamesLabel.Location = new System.Drawing.Point(27, 167);
      this.LowerCaseTableNamesLabel.Name = "LowerCaseTableNamesLabel";
      this.LowerCaseTableNamesLabel.Size = new System.Drawing.Size(109, 15);
      this.LowerCaseTableNamesLabel.TabIndex = 5;
      this.LowerCaseTableNamesLabel.Text = "Table Names Case:";
      // 
      // LowerCaseRadioButton
      // 
      this.LowerCaseRadioButton.AccessibleDescription = "An option to select the lower case table names option";
      this.LowerCaseRadioButton.AccessibleName = "Lower Case Table Names";
      this.LowerCaseRadioButton.AutoSize = true;
      this.LowerCaseRadioButton.Checked = true;
      this.LowerCaseRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.LowerCaseRadioButton.ForeColor = System.Drawing.SystemColors.ControlText;
      this.LowerCaseRadioButton.Location = new System.Drawing.Point(30, 194);
      this.LowerCaseRadioButton.Name = "LowerCaseRadioButton";
      this.LowerCaseRadioButton.Size = new System.Drawing.Size(136, 19);
      this.LowerCaseRadioButton.TabIndex = 6;
      this.LowerCaseRadioButton.TabStop = true;
      this.LowerCaseRadioButton.Text = "Lower Case (default):";
      this.LowerCaseRadioButton.UseVisualStyleBackColor = true;
      // 
      // PreserveGivenCaseRadioButton
      // 
      this.PreserveGivenCaseRadioButton.AccessibleDescription = "An option to select the preserve given table names case option";
      this.PreserveGivenCaseRadioButton.AccessibleName = "Preserve Given Table Names Case";
      this.PreserveGivenCaseRadioButton.AutoSize = true;
      this.PreserveGivenCaseRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.PreserveGivenCaseRadioButton.ForeColor = System.Drawing.SystemColors.ControlText;
      this.PreserveGivenCaseRadioButton.Location = new System.Drawing.Point(30, 233);
      this.PreserveGivenCaseRadioButton.Name = "PreserveGivenCaseRadioButton";
      this.PreserveGivenCaseRadioButton.Size = new System.Drawing.Size(133, 19);
      this.PreserveGivenCaseRadioButton.TabIndex = 8;
      this.PreserveGivenCaseRadioButton.Text = "Preserve Given Case:";
      this.PreserveGivenCaseRadioButton.UseVisualStyleBackColor = true;
      // 
      // LowerCaseDescriptionLabel
      // 
      this.LowerCaseDescriptionLabel.AccessibleDescription = "A label displaying text describing the lower case table names option";
      this.LowerCaseDescriptionLabel.AccessibleName = "Lower Case Table Names Description";
      this.LowerCaseDescriptionLabel.AutoSize = true;
      this.LowerCaseDescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.LowerCaseDescriptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.LowerCaseDescriptionLabel.Location = new System.Drawing.Point(48, 213);
      this.LowerCaseDescriptionLabel.Name = "LowerCaseDescriptionLabel";
      this.LowerCaseDescriptionLabel.Size = new System.Drawing.Size(383, 15);
      this.LowerCaseDescriptionLabel.TabIndex = 7;
      this.LowerCaseDescriptionLabel.Text = "This option sets the configuration variable lower_case_table_names = 1.";
      // 
      // PreserveGivenCaseDescriptionLabel
      // 
      this.PreserveGivenCaseDescriptionLabel.AccessibleDescription = "A label displaying text describing the preserve given table names case option";
      this.PreserveGivenCaseDescriptionLabel.AccessibleName = "Preserve Given Table Names Case Description";
      this.PreserveGivenCaseDescriptionLabel.AutoSize = true;
      this.PreserveGivenCaseDescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PreserveGivenCaseDescriptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.PreserveGivenCaseDescriptionLabel.Location = new System.Drawing.Point(48, 252);
      this.PreserveGivenCaseDescriptionLabel.Name = "PreserveGivenCaseDescriptionLabel";
      this.PreserveGivenCaseDescriptionLabel.Size = new System.Drawing.Size(383, 15);
      this.PreserveGivenCaseDescriptionLabel.TabIndex = 9;
      this.PreserveGivenCaseDescriptionLabel.Text = "This option sets the configuration variable lower_case_table_names = 2.";
      // 
      // ServerIdDescriptionLabel
      // 
      this.ServerIdDescriptionLabel.AccessibleDescription = "A label displaying text describing the server ID option";
      this.ServerIdDescriptionLabel.AccessibleName = "Server ID Description";
      this.ServerIdDescriptionLabel.AutoSize = true;
      this.ServerIdDescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ServerIdDescriptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.ServerIdDescriptionLabel.Location = new System.Drawing.Point(48, 114);
      this.ServerIdDescriptionLabel.Name = "ServerIdDescriptionLabel";
      this.ServerIdDescriptionLabel.Size = new System.Drawing.Size(315, 30);
      this.ServerIdDescriptionLabel.TabIndex = 4;
      this.ServerIdDescriptionLabel.Text = "A unique numeric identifier used in a replication topology.\r\nIf binary logging is" +
    " enabled, a Server ID must be specified.";
      // 
      // ServerConfigAdvancedOptionsPage
      // 
      this.AccessibleDescription = "A configuration wizard page showing advanced options";
      this.AccessibleName = "Advanced Options Page";
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.Caption = "Advanced Options";
      this.Controls.Add(this.ServerIdDescriptionLabel);
      this.Controls.Add(this.PreserveGivenCaseDescriptionLabel);
      this.Controls.Add(this.LowerCaseDescriptionLabel);
      this.Controls.Add(this.PreserveGivenCaseRadioButton);
      this.Controls.Add(this.LowerCaseRadioButton);
      this.Controls.Add(this.LowerCaseTableNamesLabel);
      this.Controls.Add(this.ServerIdLabel);
      this.Controls.Add(this.ServerIdTextBox);
      this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "ServerConfigAdvancedOptionsPage";
      this.SubCaption = "";
      this.Controls.SetChildIndex(this.captionLabel, 0);
      this.Controls.SetChildIndex(this.subCaptionLabel, 0);
      this.Controls.SetChildIndex(this.ServerIdTextBox, 0);
      this.Controls.SetChildIndex(this.ServerIdLabel, 0);
      this.Controls.SetChildIndex(this.LowerCaseTableNamesLabel, 0);
      this.Controls.SetChildIndex(this.LowerCaseRadioButton, 0);
      this.Controls.SetChildIndex(this.PreserveGivenCaseRadioButton, 0);
      this.Controls.SetChildIndex(this.LowerCaseDescriptionLabel, 0);
      this.Controls.SetChildIndex(this.PreserveGivenCaseDescriptionLabel, 0);
      this.Controls.SetChildIndex(this.ServerIdDescriptionLabel, 0);
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label ServerIdLabel;
    private System.Windows.Forms.TextBox ServerIdTextBox;
    private System.Windows.Forms.Label LowerCaseTableNamesLabel;
    private System.Windows.Forms.RadioButton LowerCaseRadioButton;
    private System.Windows.Forms.RadioButton PreserveGivenCaseRadioButton;
    private System.Windows.Forms.Label LowerCaseDescriptionLabel;
    private System.Windows.Forms.Label PreserveGivenCaseDescriptionLabel;
    private System.Windows.Forms.Label ServerIdDescriptionLabel;
  }
}
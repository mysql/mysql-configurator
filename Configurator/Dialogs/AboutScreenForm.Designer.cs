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
  partial class AboutScreenForm
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
      this.LoadingActionLabel = new System.Windows.Forms.Label();
      this.CopyrightLabel = new System.Windows.Forms.Label();
      this.RegisteredTrademarkLabel = new System.Windows.Forms.Label();
      this.OtherNamesLabel = new System.Windows.Forms.Label();
      this.CopyrightSymbolLabel = new System.Windows.Forms.Label();
      this.VersionLicenseLabel = new System.Windows.Forms.Label();
      this.ConfiguratorLabel = new System.Windows.Forms.Label();
      this.MySqlLabel = new System.Windows.Forms.Label();
      this.OkButton = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // LoadingActionLabel
      // 
      this.LoadingActionLabel.AccessibleDescription = "Label displaying the current action during initial loading";
      this.LoadingActionLabel.AccessibleName = "Initial Loading Action";
      this.LoadingActionLabel.AutoSize = true;
      this.LoadingActionLabel.BackColor = System.Drawing.Color.Transparent;
      this.LoadingActionLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.LoadingActionLabel.ForeColor = System.Drawing.Color.White;
      this.LoadingActionLabel.Location = new System.Drawing.Point(108, 148);
      this.LoadingActionLabel.Name = "LoadingActionLabel";
      this.LoadingActionLabel.Size = new System.Drawing.Size(0, 21);
      this.LoadingActionLabel.TabIndex = 0;
      // 
      // CopyrightLabel
      // 
      this.CopyrightLabel.AccessibleDescription = "Label displaying the copyright years";
      this.CopyrightLabel.AccessibleName = "Copyright Years";
      this.CopyrightLabel.AutoSize = true;
      this.CopyrightLabel.BackColor = System.Drawing.Color.Transparent;
      this.CopyrightLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.CopyrightLabel.ForeColor = System.Drawing.Color.Gray;
      this.CopyrightLabel.Location = new System.Drawing.Point(102, 200);
      this.CopyrightLabel.Name = "CopyrightLabel";
      this.CopyrightLabel.Size = new System.Drawing.Size(241, 13);
      this.CopyrightLabel.TabIndex = 1;
      this.CopyrightLabel.Text = "Copyright (c) 2023, Oracle and/or its affiliates.";
      this.CopyrightLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // RegisteredTrademarkLabel
      // 
      this.RegisteredTrademarkLabel.AccessibleDescription = "Label displaying a trademark disclaimer about Oracle";
      this.RegisteredTrademarkLabel.AccessibleName = "Oracle Trademark";
      this.RegisteredTrademarkLabel.AutoSize = true;
      this.RegisteredTrademarkLabel.BackColor = System.Drawing.Color.Transparent;
      this.RegisteredTrademarkLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.RegisteredTrademarkLabel.ForeColor = System.Drawing.Color.Gray;
      this.RegisteredTrademarkLabel.Location = new System.Drawing.Point(102, 218);
      this.RegisteredTrademarkLabel.Name = "RegisteredTrademarkLabel";
      this.RegisteredTrademarkLabel.Size = new System.Drawing.Size(417, 13);
      this.RegisteredTrademarkLabel.TabIndex = 2;
      this.RegisteredTrademarkLabel.Text = "Oracle, Java, and MySQL are registered trademarks of Oracle and/or its affiliates" +
    ".";
      this.RegisteredTrademarkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // OtherNamesLabel
      // 
      this.OtherNamesLabel.AccessibleDescription = "Label displaying a disclaimer about other names being trademark of their respecti" +
    "ve owners";
      this.OtherNamesLabel.AccessibleName = "Other names trademark";
      this.OtherNamesLabel.AutoSize = true;
      this.OtherNamesLabel.BackColor = System.Drawing.Color.Transparent;
      this.OtherNamesLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.OtherNamesLabel.ForeColor = System.Drawing.Color.Gray;
      this.OtherNamesLabel.Location = new System.Drawing.Point(102, 237);
      this.OtherNamesLabel.Name = "OtherNamesLabel";
      this.OtherNamesLabel.Size = new System.Drawing.Size(311, 13);
      this.OtherNamesLabel.TabIndex = 3;
      this.OtherNamesLabel.Text = "Other names may be trademarks of their respective owners.";
      this.OtherNamesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // CopyrightSymbolLabel
      // 
      this.CopyrightSymbolLabel.AccessibleDescription = "Label displaying the copyright symbol";
      this.CopyrightSymbolLabel.AccessibleName = "Copyright Symbol";
      this.CopyrightSymbolLabel.AutoSize = true;
      this.CopyrightSymbolLabel.BackColor = System.Drawing.Color.Transparent;
      this.CopyrightSymbolLabel.Font = new System.Drawing.Font("Segoe UI Black", 10F, System.Drawing.FontStyle.Bold);
      this.CopyrightSymbolLabel.ForeColor = System.Drawing.Color.White;
      this.CopyrightSymbolLabel.Location = new System.Drawing.Point(184, 41);
      this.CopyrightSymbolLabel.Name = "CopyrightSymbolLabel";
      this.CopyrightSymbolLabel.Size = new System.Drawing.Size(21, 19);
      this.CopyrightSymbolLabel.TabIndex = 2;
      this.CopyrightSymbolLabel.Text = "©";
      this.CopyrightSymbolLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // VersionLicenseLabel
      // 
      this.VersionLicenseLabel.AccessibleDescription = "Label displaying the version and license of the application";
      this.VersionLicenseLabel.AccessibleName = "Version And License";
      this.VersionLicenseLabel.AutoSize = true;
      this.VersionLicenseLabel.BackColor = System.Drawing.Color.Transparent;
      this.VersionLicenseLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.VersionLicenseLabel.ForeColor = System.Drawing.Color.White;
      this.VersionLicenseLabel.Location = new System.Drawing.Point(103, 110);
      this.VersionLicenseLabel.Name = "VersionLicenseLabel";
      this.VersionLicenseLabel.Size = new System.Drawing.Size(147, 15);
      this.VersionLicenseLabel.TabIndex = 4;
      this.VersionLicenseLabel.Text = "Version 8.1.0 (Community)";
      this.VersionLicenseLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // ConfiguratorLabel
      // 
      this.ConfiguratorLabel.AccessibleDescription = "Label displaying the text configurator";
      this.ConfiguratorLabel.AccessibleName = "Configurator";
      this.ConfiguratorLabel.AutoSize = true;
      this.ConfiguratorLabel.BackColor = System.Drawing.Color.Transparent;
      this.ConfiguratorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold);
      this.ConfiguratorLabel.ForeColor = System.Drawing.Color.White;
      this.ConfiguratorLabel.Location = new System.Drawing.Point(96, 63);
      this.ConfiguratorLabel.Name = "ConfiguratorLabel";
      this.ConfiguratorLabel.Size = new System.Drawing.Size(207, 37);
      this.ConfiguratorLabel.TabIndex = 3;
      this.ConfiguratorLabel.Text = "Configurator";
      this.ConfiguratorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // MySqlLabel
      // 
      this.MySqlLabel.AccessibleDescription = "Label displaying the text MySQL";
      this.MySqlLabel.AccessibleName = "MySQL";
      this.MySqlLabel.AutoSize = true;
      this.MySqlLabel.BackColor = System.Drawing.Color.Transparent;
      this.MySqlLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
      this.MySqlLabel.ForeColor = System.Drawing.Color.White;
      this.MySqlLabel.Location = new System.Drawing.Point(99, 33);
      this.MySqlLabel.Name = "MySqlLabel";
      this.MySqlLabel.Size = new System.Drawing.Size(97, 29);
      this.MySqlLabel.TabIndex = 1;
      this.MySqlLabel.Text = "MySQL";
      this.MySqlLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // OkButton
      // 
      this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.OkButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.OkButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.OkButton.Location = new System.Drawing.Point(487, 262);
      this.OkButton.Name = "OkButton";
      this.OkButton.Size = new System.Drawing.Size(75, 23);
      this.OkButton.TabIndex = 5;
      this.OkButton.Text = "OK";
      this.OkButton.UseVisualStyleBackColor = true;
      // 
      // AboutScreenForm
      // 
      this.AcceptButton = this.OkButton;
      this.AccessibleDescription = "The about screen shown to display information about the application";
      this.AccessibleName = "About Screen";
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.BackgroundImage = global::MySql.Configurator.Properties.Resources.SplashScreen;
      this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.CancelButton = this.OkButton;
      this.ClientSize = new System.Drawing.Size(574, 321);
      this.Controls.Add(this.OkButton);
      this.Controls.Add(this.VersionLicenseLabel);
      this.Controls.Add(this.ConfiguratorLabel);
      this.Controls.Add(this.CopyrightSymbolLabel);
      this.Controls.Add(this.MySqlLabel);
      this.Controls.Add(this.OtherNamesLabel);
      this.Controls.Add(this.RegisteredTrademarkLabel);
      this.Controls.Add(this.CopyrightLabel);
      this.Controls.Add(this.LoadingActionLabel);
      this.DoubleBuffered = true;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "AboutScreenForm";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "About MySQL Configurator";
      this.Deactivate += new System.EventHandler(this.AboutScreenForm_Deactivate);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label LoadingActionLabel;
    private System.Windows.Forms.Label CopyrightLabel;
    private System.Windows.Forms.Label RegisteredTrademarkLabel;
    private System.Windows.Forms.Label OtherNamesLabel;
    private System.Windows.Forms.Label CopyrightSymbolLabel;
    private System.Windows.Forms.Label VersionLicenseLabel;
    private System.Windows.Forms.Label ConfiguratorLabel;
    private System.Windows.Forms.Label MySqlLabel;
    private System.Windows.Forms.Button OkButton;
  }
}
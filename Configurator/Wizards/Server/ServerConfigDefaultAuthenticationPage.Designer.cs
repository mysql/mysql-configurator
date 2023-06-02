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
  partial class ServerConfigDefaultAuthenticationPage
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerConfigDefaultAuthenticationPage));
      this.UseSha256AuthenticationRadioButton = new System.Windows.Forms.RadioButton();
      this.UseNativeAuthenticationRadioButton = new System.Windows.Forms.RadioButton();
      this.UseSha256AuthenticationDescription1Label = new System.Windows.Forms.Label();
      this.UseSha256AuthenticationDescription3Label = new System.Windows.Forms.Label();
      this.UseNativeAuthenticationDescriptionLabel = new System.Windows.Forms.Label();
      this.UseSha256AuthenticationDescription2Label = new System.Windows.Forms.Label();
      this.WarningPictureBox = new System.Windows.Forms.PictureBox();
      ((System.ComponentModel.ISupportInitialize)(this.WarningPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // subCaptionLabel
      // 
      this.subCaptionLabel.Location = new System.Drawing.Point(507, 15);
      this.subCaptionLabel.Visible = false;
      // 
      // captionLabel
      // 
      this.captionLabel.Size = new System.Drawing.Size(207, 25);
      this.captionLabel.Text = "Authentication Method";
      // 
      // UseSha256AuthenticationRadioButton
      // 
      this.UseSha256AuthenticationRadioButton.AccessibleDescription = "An option to use strong password encryption for authentication";
      this.UseSha256AuthenticationRadioButton.AccessibleName = "Use Strong Encryption";
      this.UseSha256AuthenticationRadioButton.AutoSize = true;
      this.UseSha256AuthenticationRadioButton.Checked = true;
      this.UseSha256AuthenticationRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.UseSha256AuthenticationRadioButton.Location = new System.Drawing.Point(30, 70);
      this.UseSha256AuthenticationRadioButton.Name = "UseSha256AuthenticationRadioButton";
      this.UseSha256AuthenticationRadioButton.Size = new System.Drawing.Size(412, 19);
      this.UseSha256AuthenticationRadioButton.TabIndex = 1;
      this.UseSha256AuthenticationRadioButton.TabStop = true;
      this.UseSha256AuthenticationRadioButton.Text = "Use Strong Password Encryption for Authentication (RECOMMENDED)";
      this.UseSha256AuthenticationRadioButton.UseVisualStyleBackColor = true;
      // 
      // UseNativeAuthenticationRadioButton
      // 
      this.UseNativeAuthenticationRadioButton.AccessibleDescription = "An option to use the legacy authentication method";
      this.UseNativeAuthenticationRadioButton.AccessibleName = "Use Legacy Authentication";
      this.UseNativeAuthenticationRadioButton.AutoSize = true;
      this.UseNativeAuthenticationRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.UseNativeAuthenticationRadioButton.Location = new System.Drawing.Point(30, 300);
      this.UseNativeAuthenticationRadioButton.Name = "UseNativeAuthenticationRadioButton";
      this.UseNativeAuthenticationRadioButton.Size = new System.Drawing.Size(405, 19);
      this.UseNativeAuthenticationRadioButton.TabIndex = 5;
      this.UseNativeAuthenticationRadioButton.Text = "Use Legacy Authentication Method (Retain MySQL 5.x Compatibility)";
      this.UseNativeAuthenticationRadioButton.UseVisualStyleBackColor = true;
      // 
      // UseSha256AuthenticationDescription1Label
      // 
      this.UseSha256AuthenticationDescription1Label.AccessibleDescription = "A label displaying an explanatory text about using strong encryption for the auth" +
    "entication";
      this.UseSha256AuthenticationDescription1Label.AccessibleName = "Use Strong Encryption Description";
      this.UseSha256AuthenticationDescription1Label.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.UseSha256AuthenticationDescription1Label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.UseSha256AuthenticationDescription1Label.Location = new System.Drawing.Point(27, 92);
      this.UseSha256AuthenticationDescription1Label.Name = "UseSha256AuthenticationDescription1Label";
      this.UseSha256AuthenticationDescription1Label.Size = new System.Drawing.Size(513, 47);
      this.UseSha256AuthenticationDescription1Label.TabIndex = 2;
      this.UseSha256AuthenticationDescription1Label.Text = "MySQL supports an authentication method based on improved stronger SHA256-based p" +
    "assword methods. It is recommended that all new MySQL Server installations use t" +
    "his method going forward.";
      // 
      // UseSha256AuthenticationDescription3Label
      // 
      this.UseSha256AuthenticationDescription3Label.AccessibleDescription = "A label displaying an explanatory text about using strong encryption for the auth" +
    "entication";
      this.UseSha256AuthenticationDescription3Label.AccessibleName = "Use Strong Encryption Description 3";
      this.UseSha256AuthenticationDescription3Label.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.UseSha256AuthenticationDescription3Label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.UseSha256AuthenticationDescription3Label.Location = new System.Drawing.Point(27, 210);
      this.UseSha256AuthenticationDescription3Label.Name = "UseSha256AuthenticationDescription3Label";
      this.UseSha256AuthenticationDescription3Label.Size = new System.Drawing.Size(513, 61);
      this.UseSha256AuthenticationDescription3Label.TabIndex = 4;
      this.UseSha256AuthenticationDescription3Label.Text = resources.GetString("UseSha256AuthenticationDescription3Label.Text");
      // 
      // UseNativeAuthenticationDescriptionLabel
      // 
      this.UseNativeAuthenticationDescriptionLabel.AccessibleDescription = "A label displaying an explanatory text about using the legacy authentication meth" +
    "od";
      this.UseNativeAuthenticationDescriptionLabel.AccessibleName = "Use Legacy Authentication Description";
      this.UseNativeAuthenticationDescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.UseNativeAuthenticationDescriptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.UseNativeAuthenticationDescriptionLabel.Location = new System.Drawing.Point(27, 322);
      this.UseNativeAuthenticationDescriptionLabel.Name = "UseNativeAuthenticationDescriptionLabel";
      this.UseNativeAuthenticationDescriptionLabel.Size = new System.Drawing.Size(513, 152);
      this.UseNativeAuthenticationDescriptionLabel.TabIndex = 6;
      this.UseNativeAuthenticationDescriptionLabel.Text = resources.GetString("UseNativeAuthenticationDescriptionLabel.Text");
      // 
      // UseSha256AuthenticationDescription2Label
      // 
      this.UseSha256AuthenticationDescription2Label.AccessibleDescription = "A label displaying an explanatory text about using strong encryption for the auth" +
    "entication";
      this.UseSha256AuthenticationDescription2Label.AccessibleName = "Use Strong Encryption Description 2";
      this.UseSha256AuthenticationDescription2Label.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.UseSha256AuthenticationDescription2Label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.UseSha256AuthenticationDescription2Label.Location = new System.Drawing.Point(74, 151);
      this.UseSha256AuthenticationDescription2Label.Name = "UseSha256AuthenticationDescription2Label";
      this.UseSha256AuthenticationDescription2Label.Size = new System.Drawing.Size(451, 46);
      this.UseSha256AuthenticationDescription2Label.TabIndex = 3;
      this.UseSha256AuthenticationDescription2Label.Text = "Attention: This authentication plugin on the server side requires new versions of" +
    " connectors and clients that add support for this default authentication (cachin" +
    "g_sha2_password authentication).";
      // 
      // WarningPictureBox
      // 
      this.WarningPictureBox.AccessibleDescription = "A picture box displaying a big warning icon";
      this.WarningPictureBox.AccessibleName = "Big Warning Icon";
      this.WarningPictureBox.Image = global::MySql.Configurator.Properties.Resources.BigWarning;
      this.WarningPictureBox.Location = new System.Drawing.Point(30, 156);
      this.WarningPictureBox.Name = "WarningPictureBox";
      this.WarningPictureBox.Size = new System.Drawing.Size(38, 38);
      this.WarningPictureBox.TabIndex = 10;
      this.WarningPictureBox.TabStop = false;
      // 
      // ServerConfigDefaultAuthenticationPage
      // 
      this.AccessibleDescription = "A configuration wizard page to select the default authentication method";
      this.AccessibleName = "Default Authentication Method Page";
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
      this.Caption = "Authentication Method";
      this.Controls.Add(this.WarningPictureBox);
      this.Controls.Add(this.UseSha256AuthenticationDescription2Label);
      this.Controls.Add(this.UseNativeAuthenticationDescriptionLabel);
      this.Controls.Add(this.UseSha256AuthenticationDescription3Label);
      this.Controls.Add(this.UseSha256AuthenticationDescription1Label);
      this.Controls.Add(this.UseNativeAuthenticationRadioButton);
      this.Controls.Add(this.UseSha256AuthenticationRadioButton);
      this.Name = "ServerConfigDefaultAuthenticationPage";
      this.Controls.SetChildIndex(this.captionLabel, 0);
      this.Controls.SetChildIndex(this.subCaptionLabel, 0);
      this.Controls.SetChildIndex(this.UseSha256AuthenticationRadioButton, 0);
      this.Controls.SetChildIndex(this.UseNativeAuthenticationRadioButton, 0);
      this.Controls.SetChildIndex(this.UseSha256AuthenticationDescription1Label, 0);
      this.Controls.SetChildIndex(this.UseSha256AuthenticationDescription3Label, 0);
      this.Controls.SetChildIndex(this.UseNativeAuthenticationDescriptionLabel, 0);
      this.Controls.SetChildIndex(this.UseSha256AuthenticationDescription2Label, 0);
      this.Controls.SetChildIndex(this.WarningPictureBox, 0);
      ((System.ComponentModel.ISupportInitialize)(this.WarningPictureBox)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.RadioButton UseSha256AuthenticationRadioButton;
    private System.Windows.Forms.RadioButton UseNativeAuthenticationRadioButton;
    private System.Windows.Forms.Label UseSha256AuthenticationDescription1Label;
    private System.Windows.Forms.Label UseSha256AuthenticationDescription3Label;
    private System.Windows.Forms.Label UseNativeAuthenticationDescriptionLabel;
    private System.Windows.Forms.Label UseSha256AuthenticationDescription2Label;
    private System.Windows.Forms.PictureBox WarningPictureBox;
  }
}

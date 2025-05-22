/* Copyright (c) 2025, Oracle and/or its affiliates.

  This program is free software; you can redistribute it and/or modify 
  it under the terms of the GNU General Public License, version 2.0, as 
  published by the Free Software Foundation.

  This program is designed to work with certain software (including
  but not limited to OpenSSL) that is licensed under separate terms, as
  designated in a particular file or component or in included license
  documentation. The authors of MySQL hereby grant you an additional
  permission to link the program and your derivative works with the
  separately licensed software that they have either included with
  the program or referenced in the documentation.

  This program is distributed in the hope that it will be useful, but 
  WITHOUT ANY WARRANTY; without even the implied warranty of 
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
  See the GNU General Public License, version 2.0, for more details. 

  You should have received a copy of the GNU General Public License 
  along with this program; if not, write to the Free Software Foundation, Inc., 
  51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA */

namespace MySql.Configurator.UI.Wizards.ServerConfigPages
{
  partial class ServerConfigEnterpriseFirewall
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerConfigEnterpriseFirewall));
      this.EnterpriseFirewallFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
      this.UpgradeEnterpriseFirewallPanel = new System.Windows.Forms.Panel();
      this.UpgradeEnterpriseFirewallCheckBox = new System.Windows.Forms.CheckBox();
      this.UpgradeEnterpriseFirewallDescriptionLabel = new System.Windows.Forms.Label();
      this.EnableDisableEnterpriseFirewallPanel = new System.Windows.Forms.Panel();
      this.EnterpriseFirewallDocumentationLinkLabel = new System.Windows.Forms.LinkLabel();
      this.EnableDisableEnterpriseFirewallCheckBox = new System.Windows.Forms.CheckBox();
      this.EnableDisableEnterpriseFirewallDescriptionLabel = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).BeginInit();
      this.EnterpriseFirewallFlowLayoutPanel.SuspendLayout();
      this.UpgradeEnterpriseFirewallPanel.SuspendLayout();
      this.EnableDisableEnterpriseFirewallPanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // subCaptionLabel
      // 
      this.subCaptionLabel.Location = new System.Drawing.Point(25, 66);
      this.subCaptionLabel.Size = new System.Drawing.Size(522, 10);
      this.subCaptionLabel.Visible = false;
      // 
      // captionLabel
      // 
      this.captionLabel.Location = new System.Drawing.Point(34, 51);
      this.captionLabel.Size = new System.Drawing.Size(231, 25);
      this.captionLabel.Text = "MySQL Enterprise Firewall";
      // 
      // EnterpriseFirewallFlowLayoutPanel
      // 
      this.EnterpriseFirewallFlowLayoutPanel.AccessibleDescription = "A panel containing inner panels with options related to Enterprise Firewall.";
      this.EnterpriseFirewallFlowLayoutPanel.AccessibleName = "Enterprise Firewall Flow Panel";
      this.EnterpriseFirewallFlowLayoutPanel.Controls.Add(this.UpgradeEnterpriseFirewallPanel);
      this.EnterpriseFirewallFlowLayoutPanel.Controls.Add(this.EnableDisableEnterpriseFirewallPanel);
      this.EnterpriseFirewallFlowLayoutPanel.Location = new System.Drawing.Point(0, 142);
      this.EnterpriseFirewallFlowLayoutPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.EnterpriseFirewallFlowLayoutPanel.Name = "EnterpriseFirewallFlowLayoutPanel";
      this.EnterpriseFirewallFlowLayoutPanel.Size = new System.Drawing.Size(849, 1613);
      this.EnterpriseFirewallFlowLayoutPanel.TabIndex = 2;
      // 
      // UpgradeEnterpriseFirewallPanel
      // 
      this.UpgradeEnterpriseFirewallPanel.AccessibleDescription = "A panel containing controls upgrading enterprise firewall.";
      this.UpgradeEnterpriseFirewallPanel.AccessibleName = "Upgrade Component";
      this.UpgradeEnterpriseFirewallPanel.Controls.Add(this.UpgradeEnterpriseFirewallCheckBox);
      this.UpgradeEnterpriseFirewallPanel.Controls.Add(this.UpgradeEnterpriseFirewallDescriptionLabel);
      this.UpgradeEnterpriseFirewallPanel.Location = new System.Drawing.Point(4, 5);
      this.UpgradeEnterpriseFirewallPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.UpgradeEnterpriseFirewallPanel.Name = "UpgradeEnterpriseFirewallPanel";
      this.UpgradeEnterpriseFirewallPanel.Size = new System.Drawing.Size(840, 172);
      this.UpgradeEnterpriseFirewallPanel.TabIndex = 8;
      this.UpgradeEnterpriseFirewallPanel.Visible = false;
      // 
      // UpgradeEnterpriseFirewallCheckBox
      // 
      this.UpgradeEnterpriseFirewallCheckBox.AccessibleDescription = "A check box to upgrade enterprise firewall";
      this.UpgradeEnterpriseFirewallCheckBox.AccessibleName = "Upgrade Enterprise Firewall";
      this.UpgradeEnterpriseFirewallCheckBox.AutoSize = true;
      this.UpgradeEnterpriseFirewallCheckBox.Location = new System.Drawing.Point(39, 98);
      this.UpgradeEnterpriseFirewallCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.UpgradeEnterpriseFirewallCheckBox.Name = "UpgradeEnterpriseFirewallCheckBox";
      this.UpgradeEnterpriseFirewallCheckBox.Size = new System.Drawing.Size(302, 27);
      this.UpgradeEnterpriseFirewallCheckBox.TabIndex = 2;
      this.UpgradeEnterpriseFirewallCheckBox.Text = "Upgrade MySQL Enterprise Firewall";
      this.UpgradeEnterpriseFirewallCheckBox.UseVisualStyleBackColor = true;
      this.UpgradeEnterpriseFirewallCheckBox.CheckedChanged += new System.EventHandler(this.UpgradeEnterpriseFirewallCheckBox_CheckedChanged);
      // 
      // UpgradeEnterpriseFirewallDescriptionLabel
      // 
      this.UpgradeEnterpriseFirewallDescriptionLabel.AccessibleDescription = "A label displaying a description about upgrading Enterprise Firewall";
      this.UpgradeEnterpriseFirewallDescriptionLabel.AccessibleName = "Enable Disable Enterprise Firewall Description";
      this.UpgradeEnterpriseFirewallDescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.UpgradeEnterpriseFirewallDescriptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.UpgradeEnterpriseFirewallDescriptionLabel.Location = new System.Drawing.Point(33, 0);
      this.UpgradeEnterpriseFirewallDescriptionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.UpgradeEnterpriseFirewallDescriptionLabel.Name = "UpgradeEnterpriseFirewallDescriptionLabel";
      this.UpgradeEnterpriseFirewallDescriptionLabel.Size = new System.Drawing.Size(765, 103);
      this.UpgradeEnterpriseFirewallDescriptionLabel.TabIndex = 0;
      this.UpgradeEnterpriseFirewallDescriptionLabel.Text = resources.GetString("UpgradeEnterpriseFirewallDescriptionLabel.Text");
      // 
      // EnableDisableEnterpriseFirewallPanel
      // 
      this.EnableDisableEnterpriseFirewallPanel.AccessibleDescription = "A panel containing controls for enabling or disabling enterprise firewall.";
      this.EnableDisableEnterpriseFirewallPanel.AccessibleName = "Enable Disable Component";
      this.EnableDisableEnterpriseFirewallPanel.Controls.Add(this.EnterpriseFirewallDocumentationLinkLabel);
      this.EnableDisableEnterpriseFirewallPanel.Controls.Add(this.EnableDisableEnterpriseFirewallCheckBox);
      this.EnableDisableEnterpriseFirewallPanel.Controls.Add(this.EnableDisableEnterpriseFirewallDescriptionLabel);
      this.EnableDisableEnterpriseFirewallPanel.Location = new System.Drawing.Point(4, 187);
      this.EnableDisableEnterpriseFirewallPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.EnableDisableEnterpriseFirewallPanel.Name = "EnableDisableEnterpriseFirewallPanel";
      this.EnableDisableEnterpriseFirewallPanel.Size = new System.Drawing.Size(840, 134);
      this.EnableDisableEnterpriseFirewallPanel.TabIndex = 9;
      this.EnableDisableEnterpriseFirewallPanel.Visible = false;
      // 
      // EnterpriseFirewallDocumentationLinkLabel
      // 
      this.EnterpriseFirewallDocumentationLinkLabel.AccessibleDescription = "A link label to open a web page with documentation about MySQL Enterprise Firewal" +
    "l";
      this.EnterpriseFirewallDocumentationLinkLabel.AccessibleName = "Enterprise Firewall Documentation";
      this.EnterpriseFirewallDocumentationLinkLabel.AutoSize = true;
      this.EnterpriseFirewallDocumentationLinkLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.EnterpriseFirewallDocumentationLinkLabel.Location = new System.Drawing.Point(421, 73);
      this.EnterpriseFirewallDocumentationLinkLabel.Name = "EnterpriseFirewallDocumentationLinkLabel";
      this.EnterpriseFirewallDocumentationLinkLabel.Size = new System.Drawing.Size(358, 25);
      this.EnterpriseFirewallDocumentationLinkLabel.TabIndex = 24;
      this.EnterpriseFirewallDocumentationLinkLabel.TabStop = true;
      this.EnterpriseFirewallDocumentationLinkLabel.Text = "Click here to view the online documentation";
      this.EnterpriseFirewallDocumentationLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.EnterpriseFirewallDocumentationLinkLabel_LinkClicked);
      // 
      // EnableDisableEnterpriseFirewallCheckBox
      // 
      this.EnableDisableEnterpriseFirewallCheckBox.AccessibleDescription = "A check box to enable or disable enterprise firewall";
      this.EnableDisableEnterpriseFirewallCheckBox.AccessibleName = "Enable Disable Enterprise Firewall";
      this.EnableDisableEnterpriseFirewallCheckBox.AutoSize = true;
      this.EnableDisableEnterpriseFirewallCheckBox.Location = new System.Drawing.Point(39, 73);
      this.EnableDisableEnterpriseFirewallCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.EnableDisableEnterpriseFirewallCheckBox.Name = "EnableDisableEnterpriseFirewallCheckBox";
      this.EnableDisableEnterpriseFirewallCheckBox.Size = new System.Drawing.Size(287, 27);
      this.EnableDisableEnterpriseFirewallCheckBox.TabIndex = 2;
      this.EnableDisableEnterpriseFirewallCheckBox.Text = "Enable MySQL Enterprise Firewall";
      this.EnableDisableEnterpriseFirewallCheckBox.UseVisualStyleBackColor = true;
      // 
      // EnableDisableEnterpriseFirewallDescriptionLabel
      // 
      this.EnableDisableEnterpriseFirewallDescriptionLabel.AccessibleDescription = "A label displaying a description for enterprise firewall and how to enable or dis" +
    "able it";
      this.EnableDisableEnterpriseFirewallDescriptionLabel.AccessibleName = "Enable Disable Enterprise Firewall Description";
      this.EnableDisableEnterpriseFirewallDescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.EnableDisableEnterpriseFirewallDescriptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.EnableDisableEnterpriseFirewallDescriptionLabel.Location = new System.Drawing.Point(33, 0);
      this.EnableDisableEnterpriseFirewallDescriptionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.EnableDisableEnterpriseFirewallDescriptionLabel.Name = "EnableDisableEnterpriseFirewallDescriptionLabel";
      this.EnableDisableEnterpriseFirewallDescriptionLabel.Size = new System.Drawing.Size(765, 90);
      this.EnableDisableEnterpriseFirewallDescriptionLabel.TabIndex = 0;
      this.EnableDisableEnterpriseFirewallDescriptionLabel.Text = "Select the checkbox below to enable MySQL Enterprise Firewall, a security whiteli" +
    "st that offers\r\nprotection from cyber attacks. Additional post installation conf" +
    "iguration is necessary.";
      // 
      // ServerConfigEnterpriseFirewall
      // 
      this.AccessibleDescription = "A configuration wizard page used to enable or disable the Enterprise Firewall com" +
    "ponent";
      this.AccessibleName = "Enterprise Firewall Page";
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 23F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Caption = "MySQL Enterprise Firewall";
      this.Controls.Add(this.EnterpriseFirewallFlowLayoutPanel);
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.Name = "ServerConfigEnterpriseFirewall";
      this.Size = new System.Drawing.Size(849, 1760);
      this.Controls.SetChildIndex(this.EnterpriseFirewallFlowLayoutPanel, 0);
      this.Controls.SetChildIndex(this.captionLabel, 0);
      this.Controls.SetChildIndex(this.subCaptionLabel, 0);
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).EndInit();
      this.EnterpriseFirewallFlowLayoutPanel.ResumeLayout(false);
      this.UpgradeEnterpriseFirewallPanel.ResumeLayout(false);
      this.UpgradeEnterpriseFirewallPanel.PerformLayout();
      this.EnableDisableEnterpriseFirewallPanel.ResumeLayout(false);
      this.EnableDisableEnterpriseFirewallPanel.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.FlowLayoutPanel EnterpriseFirewallFlowLayoutPanel;
    private System.Windows.Forms.Panel UpgradeEnterpriseFirewallPanel;
    private System.Windows.Forms.Label UpgradeEnterpriseFirewallDescriptionLabel;
    private System.Windows.Forms.CheckBox UpgradeEnterpriseFirewallCheckBox;
    private System.Windows.Forms.Panel EnableDisableEnterpriseFirewallPanel;
    private System.Windows.Forms.CheckBox EnableDisableEnterpriseFirewallCheckBox;
    private System.Windows.Forms.Label EnableDisableEnterpriseFirewallDescriptionLabel;
    private System.Windows.Forms.LinkLabel EnterpriseFirewallDocumentationLinkLabel;
  }
}

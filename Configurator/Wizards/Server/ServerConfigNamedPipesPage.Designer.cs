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

using System.Windows.Forms;

namespace MySql.Configurator.Wizards.Server
{
  partial class ServerConfigNamedPipesPage
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerConfigNamedPipesPage));
      this.DescriptionPanel = new System.Windows.Forms.Panel();
      this.DocumentationLinkLabel = new System.Windows.Forms.LinkLabel();
      this.AdditionalDetailsLabel = new System.Windows.Forms.Label();
      this.DescriptionLabel = new System.Windows.Forms.Label();
      this.ConnectionErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
      this.NamedPipeConfigurationFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
      this.MinimumNecessaryAccessControlRadioButton = new System.Windows.Forms.RadioButton();
      this.panel1 = new System.Windows.Forms.Panel();
      this.MinimumNecessaryAccessDescriptionLabel = new System.Windows.Forms.Label();
      this.LocalGroupRadioButton = new System.Windows.Forms.RadioButton();
      this.LocalGroupDescriptionLabel = new System.Windows.Forms.Label();
      this.LocalGroupPanel = new System.Windows.Forms.Panel();
      this.LocalGroupNameLabel = new System.Windows.Forms.Label();
      this.LocalGroupNameComboBox = new System.Windows.Forms.ComboBox();
      this.RefreshButton = new System.Windows.Forms.Button();
      this.LocalGroupWarningPanel = new System.Windows.Forms.Panel();
      this.LocalGroupWarningPictureBox = new System.Windows.Forms.PictureBox();
      this.LocalGroupWarningLabel = new System.Windows.Forms.Label();
      this.FullAcessRadioButton = new System.Windows.Forms.RadioButton();
      this.FullAccessWarningPanel = new System.Windows.Forms.Panel();
      this.FullAccessWarningPictureBox = new System.Windows.Forms.PictureBox();
      this.FullAccessWarningLabel = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).BeginInit();
      this.DescriptionPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ConnectionErrorProvider)).BeginInit();
      this.NamedPipeConfigurationFlowLayoutPanel.SuspendLayout();
      this.panel1.SuspendLayout();
      this.LocalGroupPanel.SuspendLayout();
      this.LocalGroupWarningPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.LocalGroupWarningPictureBox)).BeginInit();
      this.FullAccessWarningPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.FullAccessWarningPictureBox)).BeginInit();
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
      this.captionLabel.Size = new System.Drawing.Size(115, 25);
      this.captionLabel.Text = "Named Pipe";
      // 
      // DescriptionPanel
      // 
      this.DescriptionPanel.AccessibleDescription = "A panel containing general information about the configuration of access to a nam" +
    "ed pipe";
      this.DescriptionPanel.AccessibleName = "Description";
      this.DescriptionPanel.Controls.Add(this.DocumentationLinkLabel);
      this.DescriptionPanel.Controls.Add(this.AdditionalDetailsLabel);
      this.DescriptionPanel.Controls.Add(this.DescriptionLabel);
      this.DescriptionPanel.Location = new System.Drawing.Point(27, 60);
      this.DescriptionPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.DescriptionPanel.Name = "DescriptionPanel";
      this.DescriptionPanel.Size = new System.Drawing.Size(536, 85);
      this.DescriptionPanel.TabIndex = 0;
      // 
      // DocumentationLinkLabel
      // 
      this.DocumentationLinkLabel.AccessibleDescription = "A link label pointing to the official documentation related to named pipe configu" +
    "ration";
      this.DocumentationLinkLabel.AccessibleName = "Documentation Link";
      this.DocumentationLinkLabel.AutoSize = true;
      this.DocumentationLinkLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.DocumentationLinkLabel.Location = new System.Drawing.Point(206, 35);
      this.DocumentationLinkLabel.Name = "DocumentationLinkLabel";
      this.DocumentationLinkLabel.Size = new System.Drawing.Size(199, 25);
      this.DocumentationLinkLabel.TabIndex = 54;
      this.DocumentationLinkLabel.TabStop = true;
      this.DocumentationLinkLabel.Text = "MySQL documentation.";
      this.DocumentationLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.NamedPipeFullAccessGroupDocumentationLinkLabel_LinkClicked);
      // 
      // AdditionalDetailsLabel
      // 
      this.AdditionalDetailsLabel.AccessibleDescription = "A label displaying complimentary text about access to a named pipe";
      this.AdditionalDetailsLabel.AccessibleName = "Additional Details";
      this.AdditionalDetailsLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.AdditionalDetailsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.AdditionalDetailsLabel.Location = new System.Drawing.Point(0, 65);
      this.AdditionalDetailsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.AdditionalDetailsLabel.Name = "AdditionalDetailsLabel";
      this.AdditionalDetailsLabel.Size = new System.Drawing.Size(350, 28);
      this.AdditionalDetailsLabel.TabIndex = 51;
      this.AdditionalDetailsLabel.Text = "Select the level of access that best fits your requirements:";
      // 
      // DescriptionLabel
      // 
      this.DescriptionLabel.AccessibleDescription = "A label displaying an explanatory text about access to a named pipe";
      this.DescriptionLabel.AccessibleName = "Description";
      this.DescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.DescriptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.DescriptionLabel.Location = new System.Drawing.Point(0, 5);
      this.DescriptionLabel.Name = "DescriptionLabel";
      this.DescriptionLabel.Size = new System.Drawing.Size(510, 65);
      this.DescriptionLabel.TabIndex = 2;
      this.DescriptionLabel.Text = resources.GetString("DescriptionLabel.Text");
      // 
      // ConnectionErrorProvider
      // 
      this.ConnectionErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
      this.ConnectionErrorProvider.ContainerControl = this;
      // 
      // NamedPipeConfigurationFlowLayoutPanel
      // 
      this.NamedPipeConfigurationFlowLayoutPanel.AccessibleDescription = "A panel containing controls to configre access to a named pipe";
      this.NamedPipeConfigurationFlowLayoutPanel.AccessibleName = "Named Pipe Configuration Flow Layout";
      this.NamedPipeConfigurationFlowLayoutPanel.Controls.Add(this.MinimumNecessaryAccessControlRadioButton);
      this.NamedPipeConfigurationFlowLayoutPanel.Controls.Add(this.panel1);
      this.NamedPipeConfigurationFlowLayoutPanel.Controls.Add(this.LocalGroupRadioButton);
      this.NamedPipeConfigurationFlowLayoutPanel.Controls.Add(this.LocalGroupDescriptionLabel);
      this.NamedPipeConfigurationFlowLayoutPanel.Controls.Add(this.LocalGroupPanel);
      this.NamedPipeConfigurationFlowLayoutPanel.Controls.Add(this.LocalGroupWarningPanel);
      this.NamedPipeConfigurationFlowLayoutPanel.Controls.Add(this.FullAcessRadioButton);
      this.NamedPipeConfigurationFlowLayoutPanel.Controls.Add(this.FullAccessWarningPanel);
      this.NamedPipeConfigurationFlowLayoutPanel.Location = new System.Drawing.Point(27, 147);
      this.NamedPipeConfigurationFlowLayoutPanel.Name = "NamedPipeConfigurationFlowLayoutPanel";
      this.NamedPipeConfigurationFlowLayoutPanel.Size = new System.Drawing.Size(520, 470);
      this.NamedPipeConfigurationFlowLayoutPanel.TabIndex = 20;
      // 
      // MinimumNecessaryAccessControlRadioButton
      // 
      this.MinimumNecessaryAccessControlRadioButton.AccessibleDescription = "An option to set minimum access control to a named pipe";
      this.MinimumNecessaryAccessControlRadioButton.AccessibleName = "Minimum Necessary Access Control";
      this.MinimumNecessaryAccessControlRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.MinimumNecessaryAccessControlRadioButton.Location = new System.Drawing.Point(3, 3);
      this.MinimumNecessaryAccessControlRadioButton.Name = "MinimumNecessaryAccessControlRadioButton";
      this.MinimumNecessaryAccessControlRadioButton.Size = new System.Drawing.Size(527, 29);
      this.MinimumNecessaryAccessControlRadioButton.TabIndex = 49;
      this.MinimumNecessaryAccessControlRadioButton.Text = "Minimum access to all users (RECOMMENDED)";
      this.MinimumNecessaryAccessControlRadioButton.UseVisualStyleBackColor = true;
      this.MinimumNecessaryAccessControlRadioButton.CheckedChanged += new System.EventHandler(this.MinimumNecessaryAccessControlRadioButton_CheckedChanged);
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.MinimumNecessaryAccessDescriptionLabel);
      this.panel1.Location = new System.Drawing.Point(0, 35);
      this.panel1.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(527, 55);
      this.panel1.TabIndex = 57;
      // 
      // MinimumNecessaryAccessDescriptionLabel
      // 
      this.MinimumNecessaryAccessDescriptionLabel.AccessibleDescription = "A label displaying important info about the current selection";
      this.MinimumNecessaryAccessDescriptionLabel.AccessibleName = "Minimum Necessary Access Description";
      this.MinimumNecessaryAccessDescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.MinimumNecessaryAccessDescriptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.MinimumNecessaryAccessDescriptionLabel.Location = new System.Drawing.Point(0, 0);
      this.MinimumNecessaryAccessDescriptionLabel.Name = "MinimumNecessaryAccessDescriptionLabel";
      this.MinimumNecessaryAccessDescriptionLabel.Size = new System.Drawing.Size(527, 45);
      this.MinimumNecessaryAccessDescriptionLabel.TabIndex = 41;
      this.MinimumNecessaryAccessDescriptionLabel.Text = resources.GetString("MinimumNecessaryAccessDescriptionLabel.Text");
      // 
      // LocalGroupRadioButton
      // 
      this.LocalGroupRadioButton.AccessibleDescription = "An option to select a group that will be granted full access to a named pipe";
      this.LocalGroupRadioButton.AccessibleName = "Local Group";
      this.LocalGroupRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.LocalGroupRadioButton.Location = new System.Drawing.Point(3, 96);
      this.LocalGroupRadioButton.Name = "LocalGroupRadioButton";
      this.LocalGroupRadioButton.Size = new System.Drawing.Size(527, 29);
      this.LocalGroupRadioButton.TabIndex = 58;
      this.LocalGroupRadioButton.Text = "Full access to members of an existing local group";
      this.LocalGroupRadioButton.UseVisualStyleBackColor = true;
      this.LocalGroupRadioButton.CheckedChanged += new System.EventHandler(this.LocalGroupRadioButton_CheckedChanged);
      // 
      // LocalGroupDescriptionLabel
      // 
      this.LocalGroupDescriptionLabel.AccessibleDescription = "A label displaying important info about the current selection";
      this.LocalGroupDescriptionLabel.AccessibleName = "Local Group Description";
      this.LocalGroupDescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.LocalGroupDescriptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.LocalGroupDescriptionLabel.Location = new System.Drawing.Point(0, 128);
      this.LocalGroupDescriptionLabel.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
      this.LocalGroupDescriptionLabel.Name = "LocalGroupDescriptionLabel";
      this.LocalGroupDescriptionLabel.Size = new System.Drawing.Size(520, 45);
      this.LocalGroupDescriptionLabel.TabIndex = 59;
      this.LocalGroupDescriptionLabel.Text = resources.GetString("LocalGroupDescriptionLabel.Text");
      // 
      // LocalGroupPanel
      // 
      this.LocalGroupPanel.Controls.Add(this.LocalGroupNameLabel);
      this.LocalGroupPanel.Controls.Add(this.LocalGroupNameComboBox);
      this.LocalGroupPanel.Controls.Add(this.RefreshButton);
      this.LocalGroupPanel.Location = new System.Drawing.Point(0, 176);
      this.LocalGroupPanel.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
      this.LocalGroupPanel.Name = "LocalGroupPanel";
      this.LocalGroupPanel.Size = new System.Drawing.Size(524, 35);
      this.LocalGroupPanel.TabIndex = 61;
      // 
      // LocalGroupNameLabel
      // 
      this.LocalGroupNameLabel.AccessibleDescription = "A label displaying the text local group name";
      this.LocalGroupNameLabel.AccessibleName = "Local Group Name";
      this.LocalGroupNameLabel.AutoSize = true;
      this.LocalGroupNameLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.LocalGroupNameLabel.Location = new System.Drawing.Point(0, 11);
      this.LocalGroupNameLabel.Name = "LocalGroupNameLabel";
      this.LocalGroupNameLabel.Size = new System.Drawing.Size(163, 25);
      this.LocalGroupNameLabel.TabIndex = 32;
      this.LocalGroupNameLabel.Text = "Local Group Name:";
      // 
      // LocalGroupNameComboBox
      // 
      this.LocalGroupNameComboBox.AccessibleDescription = "A combo box to select an existing windows group for named pipe access";
      this.LocalGroupNameComboBox.AccessibleName = "Local Group Name";
      this.LocalGroupNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.LocalGroupNameComboBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.LocalGroupNameComboBox.FormattingEnabled = true;
      this.LocalGroupNameComboBox.IntegralHeight = false;
      this.LocalGroupNameComboBox.Location = new System.Drawing.Point(117, 6);
      this.LocalGroupNameComboBox.Name = "LocalGroupNameComboBox";
      this.LocalGroupNameComboBox.Size = new System.Drawing.Size(200, 33);
      this.LocalGroupNameComboBox.TabIndex = 33;
      this.LocalGroupNameComboBox.SelectedIndexChanged += new System.EventHandler(this.GroupNameComboBox_SelectedIndexChanged);
      // 
      // RefreshButton
      // 
      this.RefreshButton.AccessibleDescription = "A button to refresh the list of local groups.";
      this.RefreshButton.AccessibleName = "Refresh Group Names List";
      this.RefreshButton.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.RefreshButton.Location = new System.Drawing.Point(350, 5);
      this.RefreshButton.Name = "RefreshButton";
      this.RefreshButton.Size = new System.Drawing.Size(72, 26);
      this.RefreshButton.TabIndex = 34;
      this.RefreshButton.Text = "Refresh";
      this.RefreshButton.UseVisualStyleBackColor = true;
      this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
      // 
      // LocalGroupWarningPanel
      // 
      this.LocalGroupWarningPanel.AccessibleDescription = "A panel use to display information related to selecting a local group for access " +
    "on named pipe connections";
      this.LocalGroupWarningPanel.AccessibleName = "Local Group Warning";
      this.LocalGroupWarningPanel.Controls.Add(this.LocalGroupWarningPictureBox);
      this.LocalGroupWarningPanel.Controls.Add(this.LocalGroupWarningLabel);
      this.LocalGroupWarningPanel.Location = new System.Drawing.Point(0, 217);
      this.LocalGroupWarningPanel.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
      this.LocalGroupWarningPanel.Name = "LocalGroupWarningPanel";
      this.LocalGroupWarningPanel.Size = new System.Drawing.Size(524, 55);
      this.LocalGroupWarningPanel.TabIndex = 65;
      // 
      // LocalGroupWarningPictureBox
      // 
      this.LocalGroupWarningPictureBox.AccessibleDescription = "A picture box displaying a big warning icon";
      this.LocalGroupWarningPictureBox.AccessibleName = "Big Warning Icon";
      this.LocalGroupWarningPictureBox.Image = global::MySql.Configurator.Properties.Resources.BigWarning;
      this.LocalGroupWarningPictureBox.Location = new System.Drawing.Point(0, 3);
      this.LocalGroupWarningPictureBox.Name = "LocalGroupWarningPictureBox";
      this.LocalGroupWarningPictureBox.Size = new System.Drawing.Size(38, 38);
      this.LocalGroupWarningPictureBox.TabIndex = 42;
      this.LocalGroupWarningPictureBox.TabStop = false;
      // 
      // LocalGroupWarningLabel
      // 
      this.LocalGroupWarningLabel.AccessibleDescription = "A label displaying important info about the current selection.";
      this.LocalGroupWarningLabel.AccessibleName = "Warning";
      this.LocalGroupWarningLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.LocalGroupWarningLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.LocalGroupWarningLabel.Location = new System.Drawing.Point(45, 0);
      this.LocalGroupWarningLabel.Name = "LocalGroupWarningLabel";
      this.LocalGroupWarningLabel.Size = new System.Drawing.Size(460, 56);
      this.LocalGroupWarningLabel.TabIndex = 41;
      this.LocalGroupWarningLabel.Text = "If a new local group is created, limit the membership to as few users as possible" +
    ". Additionally, newly added users must log out and log in again to join the grou" +
    "p (required by Windows).";
      // 
      // FullAcessRadioButton
      // 
      this.FullAcessRadioButton.AccessibleDescription = "An option to grant full access to all users on a named pipe";
      this.FullAcessRadioButton.AccessibleName = "Full Access";
      this.FullAcessRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FullAcessRadioButton.Location = new System.Drawing.Point(3, 278);
      this.FullAcessRadioButton.Name = "FullAcessRadioButton";
      this.FullAcessRadioButton.Size = new System.Drawing.Size(527, 29);
      this.FullAcessRadioButton.TabIndex = 67;
      this.FullAcessRadioButton.Text = "Full access to all users (NOT RECOMMENDED)";
      this.FullAcessRadioButton.UseVisualStyleBackColor = true;
      this.FullAcessRadioButton.CheckedChanged += new System.EventHandler(this.GrantFullAcessToEveryoneRadioButton_CheckedChanged);
      // 
      // FullAccessWarningPanel
      // 
      this.FullAccessWarningPanel.AccessibleDescription = "A panel use to display information related to granting full access to users on na" +
    "med pipe connections";
      this.FullAccessWarningPanel.AccessibleName = "Full Access Warning";
      this.FullAccessWarningPanel.Controls.Add(this.FullAccessWarningPictureBox);
      this.FullAccessWarningPanel.Controls.Add(this.FullAccessWarningLabel);
      this.FullAccessWarningPanel.Location = new System.Drawing.Point(0, 313);
      this.FullAccessWarningPanel.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
      this.FullAccessWarningPanel.Name = "FullAccessWarningPanel";
      this.FullAccessWarningPanel.Size = new System.Drawing.Size(524, 40);
      this.FullAccessWarningPanel.TabIndex = 68;
      // 
      // FullAccessWarningPictureBox
      // 
      this.FullAccessWarningPictureBox.AccessibleDescription = "A picture box displaying a big warning icon";
      this.FullAccessWarningPictureBox.AccessibleName = "Big Warning Icon";
      this.FullAccessWarningPictureBox.Image = global::MySql.Configurator.Properties.Resources.BigWarning;
      this.FullAccessWarningPictureBox.Location = new System.Drawing.Point(0, -2);
      this.FullAccessWarningPictureBox.Name = "FullAccessWarningPictureBox";
      this.FullAccessWarningPictureBox.Size = new System.Drawing.Size(38, 38);
      this.FullAccessWarningPictureBox.TabIndex = 42;
      this.FullAccessWarningPictureBox.TabStop = false;
      // 
      // FullAccessWarningLabel
      // 
      this.FullAccessWarningLabel.AccessibleDescription = "A label displaying important info about the current selection.";
      this.FullAccessWarningLabel.AccessibleName = "Warning";
      this.FullAccessWarningLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.FullAccessWarningLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.FullAccessWarningLabel.Location = new System.Drawing.Point(45, 0);
      this.FullAccessWarningLabel.Name = "FullAccessWarningLabel";
      this.FullAccessWarningLabel.Size = new System.Drawing.Size(460, 40);
      this.FullAccessWarningLabel.TabIndex = 41;
      this.FullAccessWarningLabel.Text = "For better security, consider the creation of a local group instead of providing " +
    "full access to all users.";
      // 
      // ServerConfigNamedPipesPage
      // 
      this.AccessibleDescription = "A configuration wizard page to configure the access control granted to named pipe" +
    " connections";
      this.AccessibleName = "Named Pipe Page";
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
      this.Caption = "Named Pipe";
      this.Controls.Add(this.NamedPipeConfigurationFlowLayoutPanel);
      this.Controls.Add(this.DescriptionPanel);
      this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "ServerConfigNamedPipesPage";
      this.Size = new System.Drawing.Size(566, 603);
      this.SubCaption = "";
      this.Controls.SetChildIndex(this.DescriptionPanel, 0);
      this.Controls.SetChildIndex(this.NamedPipeConfigurationFlowLayoutPanel, 0);
      this.Controls.SetChildIndex(this.subCaptionLabel, 0);
      this.Controls.SetChildIndex(this.captionLabel, 0);
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).EndInit();
      this.DescriptionPanel.ResumeLayout(false);
      this.DescriptionPanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ConnectionErrorProvider)).EndInit();
      this.NamedPipeConfigurationFlowLayoutPanel.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.LocalGroupPanel.ResumeLayout(false);
      this.LocalGroupPanel.PerformLayout();
      this.LocalGroupWarningPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.LocalGroupWarningPictureBox)).EndInit();
      this.FullAccessWarningPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.FullAccessWarningPictureBox)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private Panel DescriptionPanel;
    private ErrorProvider ConnectionErrorProvider;
    private Label DescriptionLabel;
    private FlowLayoutPanel NamedPipeConfigurationFlowLayoutPanel;
    private Label AdditionalDetailsLabel;
    private LinkLabel DocumentationLinkLabel;
    private RadioButton MinimumNecessaryAccessControlRadioButton;
    private Panel panel1;
    private Label MinimumNecessaryAccessDescriptionLabel;
    private RadioButton LocalGroupRadioButton;
    private Label LocalGroupDescriptionLabel;
    private Panel LocalGroupPanel;
    private Label LocalGroupNameLabel;
    private ComboBox LocalGroupNameComboBox;
    private Button RefreshButton;
    private Panel LocalGroupWarningPanel;
    private PictureBox LocalGroupWarningPictureBox;
    private Label LocalGroupWarningLabel;
    private RadioButton FullAcessRadioButton;
    private Panel FullAccessWarningPanel;
    private PictureBox FullAccessWarningPictureBox;
    private Label FullAccessWarningLabel;
  }
}

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

using MySql.Configurator.Core.Controls;

namespace MySql.Configurator.Wizards.Server
{
  partial class ServerConfigLocalMachinePage
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
      this.ServerConfigurationTypeLabel = new System.Windows.Forms.Label();
      this.ServerConfigurationTypeDescriptionLabel = new System.Windows.Forms.Label();
      this.ConfigTypeLabel = new System.Windows.Forms.Label();
      this.PortLabel = new System.Windows.Forms.Label();
      this.ConnectivityDescriptionLabel = new System.Windows.Forms.Label();
      this.PortTextBox = new System.Windows.Forms.TextBox();
      this.ConfigTypeComboBox = new MySql.Configurator.Core.Controls.ImageComboBox();
      this.ShowAdvancedLoggingOptionsCheckBox = new System.Windows.Forms.CheckBox();
      this.AdvancedConfigurationDescriptionLabel = new System.Windows.Forms.Label();
      this.AdvancedConfigurationLabel = new System.Windows.Forms.Label();
      this.OpenWindowsFirewallCheckBox = new System.Windows.Forms.CheckBox();
      this.ConnectivityLabel = new System.Windows.Forms.Label();
      this.TcpIpCheckBox = new System.Windows.Forms.CheckBox();
      this.NamedPipeCheckBox = new System.Windows.Forms.CheckBox();
      this.SharedMemoryCheckBox = new System.Windows.Forms.CheckBox();
      this.PipeNameLabel = new System.Windows.Forms.Label();
      this.PipeNameTextBox = new System.Windows.Forms.TextBox();
      this.SharedMemoryNameLabel = new System.Windows.Forms.Label();
      this.SharedMemoryNameTextBox = new System.Windows.Forms.TextBox();
      this.EnterpriseFirewallCheckBox = new System.Windows.Forms.CheckBox();
      this.EnterpriseFirewallDescription = new System.Windows.Forms.Label();
      this.EnterpriseFirewallTitleLabel = new System.Windows.Forms.Label();
      this.EnterpriseFirewallLinkLabel = new System.Windows.Forms.LinkLabel();
      this.PortPanel = new System.Windows.Forms.FlowLayoutPanel();
      this.PipeNamePanel = new System.Windows.Forms.FlowLayoutPanel();
      this.MemoryNamePanel = new System.Windows.Forms.FlowLayoutPanel();
      this.XProtocolPortPanel = new System.Windows.Forms.FlowLayoutPanel();
      this.XProtocolPortLabel = new System.Windows.Forms.Label();
      this.XProtocolPortTextBox = new System.Windows.Forms.TextBox();
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).BeginInit();
      this.PortPanel.SuspendLayout();
      this.PipeNamePanel.SuspendLayout();
      this.MemoryNamePanel.SuspendLayout();
      this.XProtocolPortPanel.SuspendLayout();
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
      this.captionLabel.Size = new System.Drawing.Size(191, 25);
      this.captionLabel.Text = "Type and Networking";
      // 
      // ServerConfigurationTypeLabel
      // 
      this.ServerConfigurationTypeLabel.AccessibleDescription = "A label displaying the text server configuration type";
      this.ServerConfigurationTypeLabel.AccessibleName = "Server Type Text";
      this.ServerConfigurationTypeLabel.AutoSize = true;
      this.ServerConfigurationTypeLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.ServerConfigurationTypeLabel.Location = new System.Drawing.Point(27, 66);
      this.ServerConfigurationTypeLabel.Name = "ServerConfigurationTypeLabel";
      this.ServerConfigurationTypeLabel.Size = new System.Drawing.Size(153, 15);
      this.ServerConfigurationTypeLabel.TabIndex = 2;
      this.ServerConfigurationTypeLabel.Text = "Server Configuration Type";
      // 
      // ServerConfigurationTypeDescriptionLabel
      // 
      this.ServerConfigurationTypeDescriptionLabel.AccessibleDescription = "A label displaying an explanatory text about the server type option";
      this.ServerConfigurationTypeDescriptionLabel.AccessibleName = "Server Type Description";
      this.ServerConfigurationTypeDescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.ServerConfigurationTypeDescriptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.ServerConfigurationTypeDescriptionLabel.Location = new System.Drawing.Point(27, 88);
      this.ServerConfigurationTypeDescriptionLabel.Name = "ServerConfigurationTypeDescriptionLabel";
      this.ServerConfigurationTypeDescriptionLabel.Size = new System.Drawing.Size(520, 35);
      this.ServerConfigurationTypeDescriptionLabel.TabIndex = 3;
      this.ServerConfigurationTypeDescriptionLabel.Text = "Choose the correct server configuration type for this MySQL Server installation. " +
    "This setting will define how much system resources are assigned to the MySQL Ser" +
    "ver instance.";
      // 
      // ConfigTypeLabel
      // 
      this.ConfigTypeLabel.AccessibleDescription = "A label displaying the text configuration type";
      this.ConfigTypeLabel.AccessibleName = "Configuration Type Text";
      this.ConfigTypeLabel.AutoSize = true;
      this.ConfigTypeLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.ConfigTypeLabel.Location = new System.Drawing.Point(27, 129);
      this.ConfigTypeLabel.Name = "ConfigTypeLabel";
      this.ConfigTypeLabel.Size = new System.Drawing.Size(74, 15);
      this.ConfigTypeLabel.TabIndex = 4;
      this.ConfigTypeLabel.Text = "Config Type:";
      // 
      // PortLabel
      // 
      this.PortLabel.AccessibleDescription = "A label displaying the text port";
      this.PortLabel.AccessibleName = "Port Text";
      this.PortLabel.AutoSize = true;
      this.PortLabel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.PortLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.PortLabel.Location = new System.Drawing.Point(3, 0);
      this.PortLabel.Name = "PortLabel";
      this.PortLabel.Size = new System.Drawing.Size(32, 29);
      this.PortLabel.TabIndex = 0;
      this.PortLabel.Text = "Port:";
      this.PortLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.ToolTip.SetToolTip(this.PortLabel, "MySQL Client/Server Protocol Port");
      // 
      // ConnectivityDescriptionLabel
      // 
      this.ConnectivityDescriptionLabel.AccessibleDescription = "A label displaying an explanatory text about connectivity options";
      this.ConnectivityDescriptionLabel.AccessibleName = "Connectivity Description";
      this.ConnectivityDescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.ConnectivityDescriptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.ConnectivityDescriptionLabel.Location = new System.Drawing.Point(27, 185);
      this.ConnectivityDescriptionLabel.Name = "ConnectivityDescriptionLabel";
      this.ConnectivityDescriptionLabel.Size = new System.Drawing.Size(512, 21);
      this.ConnectivityDescriptionLabel.TabIndex = 7;
      this.ConnectivityDescriptionLabel.Text = "Use the following controls to select how you would like to connect to this server" +
    ".";
      // 
      // PortTextBox
      // 
      this.PortTextBox.AccessibleDescription = "A text box to input the TCP IP port number";
      this.PortTextBox.AccessibleName = "Port Number";
      this.PortTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.PortTextBox.Location = new System.Drawing.Point(41, 3);
      this.PortTextBox.Name = "PortTextBox";
      this.PortTextBox.Size = new System.Drawing.Size(44, 23);
      this.PortTextBox.TabIndex = 1;
      this.PortTextBox.Text = "3306";
      this.ToolTip.SetToolTip(this.PortTextBox, "MySQL Client/Server Protocol Port");
      this.PortTextBox.WordWrap = false;
      this.PortTextBox.TextChanged += new System.EventHandler(this.TextChangedHandler);
      this.PortTextBox.Validated += new System.EventHandler(this.ValidatedHandler);
      // 
      // ConfigTypeComboBox
      // 
      this.ConfigTypeComboBox.AccessibleDescription = "A combo box to select one of the predefined configuration types for the server";
      this.ConfigTypeComboBox.AccessibleName = "Configuration Type";
      this.ConfigTypeComboBox.BackColor = System.Drawing.SystemColors.Window;
      this.ConfigTypeComboBox.DescriptionFont = new System.Drawing.Font("Segoe UI", 9F);
      this.ConfigTypeComboBox.DescriptionForeColor = System.Drawing.Color.Gray;
      this.ConfigTypeComboBox.DescriptionMargin = new System.Windows.Forms.Padding(5, 1, 3, 5);
      this.ConfigTypeComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
      this.ConfigTypeComboBox.DropDownHeight = 900;
      this.ConfigTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.ConfigTypeComboBox.DropDownWidth = 400;
      this.ConfigTypeComboBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.ConfigTypeComboBox.ForeColor = System.Drawing.SystemColors.WindowText;
      this.ConfigTypeComboBox.FormattingEnabled = true;
      this.ConfigTypeComboBox.ImageMargin = new System.Windows.Forms.Padding(3);
      this.ConfigTypeComboBox.IntegralHeight = false;
      this.ConfigTypeComboBox.Location = new System.Drawing.Point(106, 124);
      this.ConfigTypeComboBox.MaxDropDownItems = 3;
      this.ConfigTypeComboBox.Name = "ConfigTypeComboBox";
      this.ConfigTypeComboBox.Size = new System.Drawing.Size(418, 24);
      this.ConfigTypeComboBox.TabIndex = 5;
      this.ConfigTypeComboBox.TitleFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.ConfigTypeComboBox.TitleForeColor = System.Drawing.SystemColors.WindowText;
      this.ConfigTypeComboBox.TitleMargin = new System.Windows.Forms.Padding(5);
      // 
      // ShowAdvancedLoggingOptionsCheckBox
      // 
      this.ShowAdvancedLoggingOptionsCheckBox.AccessibleDescription = "A check box to show the advanced and logging options pages at the end of the conf" +
    "iguration";
      this.ShowAdvancedLoggingOptionsCheckBox.AccessibleName = "Show Advanced And Logging Options";
      this.ShowAdvancedLoggingOptionsCheckBox.AutoSize = true;
      this.ShowAdvancedLoggingOptionsCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.ShowAdvancedLoggingOptionsCheckBox.Location = new System.Drawing.Point(56, 379);
      this.ShowAdvancedLoggingOptionsCheckBox.Name = "ShowAdvancedLoggingOptionsCheckBox";
      this.ShowAdvancedLoggingOptionsCheckBox.Size = new System.Drawing.Size(226, 19);
      this.ShowAdvancedLoggingOptionsCheckBox.TabIndex = 18;
      this.ShowAdvancedLoggingOptionsCheckBox.Text = "Show Advanced and Logging Options";
      this.ShowAdvancedLoggingOptionsCheckBox.UseVisualStyleBackColor = true;
      this.ShowAdvancedLoggingOptionsCheckBox.CheckedChanged += new System.EventHandler(this.ShowAdvancedLoggingOptionsCheckBox_CheckedChanged);
      // 
      // AdvancedConfigurationDescriptionLabel
      // 
      this.AdvancedConfigurationDescriptionLabel.AccessibleDescription = "A label displaying an explanatory text about showing the advanced and logging con" +
    "figuration pages";
      this.AdvancedConfigurationDescriptionLabel.AccessibleName = "Show Advanced And Logging Options Description";
      this.AdvancedConfigurationDescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.AdvancedConfigurationDescriptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.AdvancedConfigurationDescriptionLabel.Location = new System.Drawing.Point(27, 342);
      this.AdvancedConfigurationDescriptionLabel.Name = "AdvancedConfigurationDescriptionLabel";
      this.AdvancedConfigurationDescriptionLabel.Size = new System.Drawing.Size(506, 34);
      this.AdvancedConfigurationDescriptionLabel.TabIndex = 17;
      this.AdvancedConfigurationDescriptionLabel.Text = "Select the check box below to get additional configuration pages where you can se" +
    "t advanced and logging options for this server instance.";
      // 
      // AdvancedConfigurationLabel
      // 
      this.AdvancedConfigurationLabel.AccessibleDescription = "A label displaying the text advanced configuration";
      this.AdvancedConfigurationLabel.AccessibleName = "Advanced Configuration Text";
      this.AdvancedConfigurationLabel.AutoSize = true;
      this.AdvancedConfigurationLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.AdvancedConfigurationLabel.Location = new System.Drawing.Point(27, 319);
      this.AdvancedConfigurationLabel.Name = "AdvancedConfigurationLabel";
      this.AdvancedConfigurationLabel.Size = new System.Drawing.Size(141, 15);
      this.AdvancedConfigurationLabel.TabIndex = 16;
      this.AdvancedConfigurationLabel.Text = "Advanced Configuration";
      // 
      // OpenWindowsFirewallCheckBox
      // 
      this.OpenWindowsFirewallCheckBox.AccessibleDescription = "A check box to open the specified ports in Windows Firewall";
      this.OpenWindowsFirewallCheckBox.AccessibleName = "Open Windows Firewall Ports";
      this.OpenWindowsFirewallCheckBox.AutoSize = true;
      this.OpenWindowsFirewallCheckBox.Checked = true;
      this.OpenWindowsFirewallCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
      this.OpenWindowsFirewallCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.OpenWindowsFirewallCheckBox.Location = new System.Drawing.Point(76, 234);
      this.OpenWindowsFirewallCheckBox.Name = "OpenWindowsFirewallCheckBox";
      this.OpenWindowsFirewallCheckBox.Size = new System.Drawing.Size(281, 19);
      this.OpenWindowsFirewallCheckBox.TabIndex = 11;
      this.OpenWindowsFirewallCheckBox.Text = "Open Windows Firewall ports for network access";
      this.OpenWindowsFirewallCheckBox.UseVisualStyleBackColor = true;
      // 
      // ConnectivityLabel
      // 
      this.ConnectivityLabel.AccessibleDescription = "A label displaying the text connectivity";
      this.ConnectivityLabel.AccessibleName = "Connectivity Text";
      this.ConnectivityLabel.AutoSize = true;
      this.ConnectivityLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.ConnectivityLabel.Location = new System.Drawing.Point(27, 162);
      this.ConnectivityLabel.Name = "ConnectivityLabel";
      this.ConnectivityLabel.Size = new System.Drawing.Size(77, 15);
      this.ConnectivityLabel.TabIndex = 6;
      this.ConnectivityLabel.Text = "Connectivity";
      // 
      // TcpIpCheckBox
      // 
      this.TcpIpCheckBox.AccessibleDescription = "A check box to enable the use of TCP IP connections";
      this.TcpIpCheckBox.AccessibleName = "Use TCP IP";
      this.TcpIpCheckBox.AutoSize = true;
      this.TcpIpCheckBox.Checked = true;
      this.TcpIpCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
      this.TcpIpCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.TcpIpCheckBox.Location = new System.Drawing.Point(56, 209);
      this.TcpIpCheckBox.Name = "TcpIpCheckBox";
      this.TcpIpCheckBox.Size = new System.Drawing.Size(62, 19);
      this.TcpIpCheckBox.TabIndex = 8;
      this.TcpIpCheckBox.Text = "TCP/IP";
      this.TcpIpCheckBox.UseVisualStyleBackColor = true;
      this.TcpIpCheckBox.CheckedChanged += new System.EventHandler(this.TcpIpCheckBox_CheckedChanged);
      // 
      // NamedPipeCheckBox
      // 
      this.NamedPipeCheckBox.AccessibleDescription = "A check box to enable the use of a named pipe to establish connections";
      this.NamedPipeCheckBox.AccessibleName = "Use Named Pipe";
      this.NamedPipeCheckBox.AutoSize = true;
      this.NamedPipeCheckBox.Checked = true;
      this.NamedPipeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
      this.NamedPipeCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.NamedPipeCheckBox.Location = new System.Drawing.Point(56, 259);
      this.NamedPipeCheckBox.Name = "NamedPipeCheckBox";
      this.NamedPipeCheckBox.Size = new System.Drawing.Size(91, 19);
      this.NamedPipeCheckBox.TabIndex = 12;
      this.NamedPipeCheckBox.Text = "Named Pipe";
      this.NamedPipeCheckBox.UseVisualStyleBackColor = true;
      this.NamedPipeCheckBox.CheckedChanged += new System.EventHandler(this.NamedPipeCheckBox_CheckedChanged);
      // 
      // SharedMemoryCheckBox
      // 
      this.SharedMemoryCheckBox.AccessibleDescription = "A check box to enable the use of shared memory to establish connections";
      this.SharedMemoryCheckBox.AccessibleName = "Use Shared Memory";
      this.SharedMemoryCheckBox.AutoSize = true;
      this.SharedMemoryCheckBox.Checked = true;
      this.SharedMemoryCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
      this.SharedMemoryCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.SharedMemoryCheckBox.Location = new System.Drawing.Point(56, 288);
      this.SharedMemoryCheckBox.Name = "SharedMemoryCheckBox";
      this.SharedMemoryCheckBox.Size = new System.Drawing.Size(110, 19);
      this.SharedMemoryCheckBox.TabIndex = 14;
      this.SharedMemoryCheckBox.Text = "Shared Memory";
      this.SharedMemoryCheckBox.UseVisualStyleBackColor = true;
      this.SharedMemoryCheckBox.CheckedChanged += new System.EventHandler(this.SharedMemoryCheckBox_CheckedChanged);
      // 
      // PipeNameLabel
      // 
      this.PipeNameLabel.AccessibleDescription = "A label displaying the text pipe name";
      this.PipeNameLabel.AccessibleName = "Pipe Name Text";
      this.PipeNameLabel.AutoSize = true;
      this.PipeNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.PipeNameLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.PipeNameLabel.Location = new System.Drawing.Point(3, 0);
      this.PipeNameLabel.Name = "PipeNameLabel";
      this.PipeNameLabel.Size = new System.Drawing.Size(68, 29);
      this.PipeNameLabel.TabIndex = 0;
      this.PipeNameLabel.Text = "Pipe Name:";
      this.PipeNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // PipeNameTextBox
      // 
      this.PipeNameTextBox.AccessibleDescription = "A text box to input the pipe name";
      this.PipeNameTextBox.AccessibleName = "Pipe Name";
      this.PipeNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.PipeNameTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.PipeNameTextBox.Location = new System.Drawing.Point(77, 3);
      this.PipeNameTextBox.Name = "PipeNameTextBox";
      this.PipeNameTextBox.Size = new System.Drawing.Size(241, 23);
      this.PipeNameTextBox.TabIndex = 1;
      this.PipeNameTextBox.WordWrap = false;
      this.PipeNameTextBox.TextChanged += new System.EventHandler(this.TextChangedHandler);
      this.PipeNameTextBox.Validated += new System.EventHandler(this.ValidatedHandler);
      // 
      // SharedMemoryNameLabel
      // 
      this.SharedMemoryNameLabel.AccessibleDescription = "A label displaying the text memory name";
      this.SharedMemoryNameLabel.AccessibleName = "Shared Memory Name Text";
      this.SharedMemoryNameLabel.AutoSize = true;
      this.SharedMemoryNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.SharedMemoryNameLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.SharedMemoryNameLabel.Location = new System.Drawing.Point(3, 0);
      this.SharedMemoryNameLabel.Name = "SharedMemoryNameLabel";
      this.SharedMemoryNameLabel.Size = new System.Drawing.Size(90, 29);
      this.SharedMemoryNameLabel.TabIndex = 0;
      this.SharedMemoryNameLabel.Text = "Memory Name:";
      this.SharedMemoryNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // SharedMemoryNameTextBox
      // 
      this.SharedMemoryNameTextBox.AccessibleDescription = "A text box to input the shared memory name";
      this.SharedMemoryNameTextBox.AccessibleName = "Shared Memory Name";
      this.SharedMemoryNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.SharedMemoryNameTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.SharedMemoryNameTextBox.Location = new System.Drawing.Point(99, 3);
      this.SharedMemoryNameTextBox.Name = "SharedMemoryNameTextBox";
      this.SharedMemoryNameTextBox.Size = new System.Drawing.Size(241, 23);
      this.SharedMemoryNameTextBox.TabIndex = 1;
      this.SharedMemoryNameTextBox.WordWrap = false;
      this.SharedMemoryNameTextBox.TextChanged += new System.EventHandler(this.TextChangedHandler);
      this.SharedMemoryNameTextBox.Validated += new System.EventHandler(this.ValidatedHandler);
      // 
      // EnterpriseFirewallCheckBox
      // 
      this.EnterpriseFirewallCheckBox.AccessibleDescription = "A check box to enable MySQL Enterprise Firewall";
      this.EnterpriseFirewallCheckBox.AccessibleName = "Enable Enterprise Firewall";
      this.EnterpriseFirewallCheckBox.AutoSize = true;
      this.EnterpriseFirewallCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.EnterpriseFirewallCheckBox.Location = new System.Drawing.Point(56, 470);
      this.EnterpriseFirewallCheckBox.Name = "EnterpriseFirewallCheckBox";
      this.EnterpriseFirewallCheckBox.Size = new System.Drawing.Size(200, 19);
      this.EnterpriseFirewallCheckBox.TabIndex = 21;
      this.EnterpriseFirewallCheckBox.Text = "Enable MySQL Enterprise Firewall";
      this.EnterpriseFirewallCheckBox.UseVisualStyleBackColor = true;
      this.EnterpriseFirewallCheckBox.Visible = false;
      // 
      // EnterpriseFirewallDescription
      // 
      this.EnterpriseFirewallDescription.AccessibleDescription = "A label displaying an explanatory text about the MySQL Enterprise Firewall";
      this.EnterpriseFirewallDescription.AccessibleName = "Enterprise Firewall Description";
      this.EnterpriseFirewallDescription.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.EnterpriseFirewallDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.EnterpriseFirewallDescription.Location = new System.Drawing.Point(27, 432);
      this.EnterpriseFirewallDescription.Name = "EnterpriseFirewallDescription";
      this.EnterpriseFirewallDescription.Size = new System.Drawing.Size(506, 34);
      this.EnterpriseFirewallDescription.TabIndex = 20;
      this.EnterpriseFirewallDescription.Text = "Select the check box below to enable MySQL Enterprise Firewall, a security whitel" +
    "ist that offers protection from cyber attacks. Additional post installation conf" +
    "iguration is necessary.";
      this.EnterpriseFirewallDescription.Visible = false;
      // 
      // EnterpriseFirewallTitleLabel
      // 
      this.EnterpriseFirewallTitleLabel.AccessibleDescription = "A label displaying the text MySQL Enterprise Firewall";
      this.EnterpriseFirewallTitleLabel.AccessibleName = "Enterprise Firewall Text";
      this.EnterpriseFirewallTitleLabel.AutoSize = true;
      this.EnterpriseFirewallTitleLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.EnterpriseFirewallTitleLabel.Location = new System.Drawing.Point(27, 409);
      this.EnterpriseFirewallTitleLabel.Name = "EnterpriseFirewallTitleLabel";
      this.EnterpriseFirewallTitleLabel.Size = new System.Drawing.Size(152, 15);
      this.EnterpriseFirewallTitleLabel.TabIndex = 19;
      this.EnterpriseFirewallTitleLabel.Text = "MySQL Enterprise Firewall";
      this.EnterpriseFirewallTitleLabel.Visible = false;
      // 
      // EnterpriseFirewallLinkLabel
      // 
      this.EnterpriseFirewallLinkLabel.AccessibleDescription = "A link label to open a web page with documentation about MySQL Enterprise Firewal" +
    "l";
      this.EnterpriseFirewallLinkLabel.AccessibleName = "Enterprise Firewall Documentation";
      this.EnterpriseFirewallLinkLabel.AutoSize = true;
      this.EnterpriseFirewallLinkLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.EnterpriseFirewallLinkLabel.Location = new System.Drawing.Point(262, 471);
      this.EnterpriseFirewallLinkLabel.Name = "EnterpriseFirewallLinkLabel";
      this.EnterpriseFirewallLinkLabel.Size = new System.Drawing.Size(244, 15);
      this.EnterpriseFirewallLinkLabel.TabIndex = 22;
      this.EnterpriseFirewallLinkLabel.TabStop = true;
      this.EnterpriseFirewallLinkLabel.Text = "Click here to view the online documentation.";
      this.EnterpriseFirewallLinkLabel.Visible = false;
      this.EnterpriseFirewallLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.EnterpriseFirewallLinkLabel_LinkClicked);
      // 
      // PortPanel
      // 
      this.PortPanel.AccessibleDescription = "A panel containing controls related to the TCP IP port";
      this.PortPanel.AccessibleName = "Port Group";
      this.PortPanel.AutoSize = true;
      this.PortPanel.Controls.Add(this.PortLabel);
      this.PortPanel.Controls.Add(this.PortTextBox);
      this.PortPanel.Location = new System.Drawing.Point(242, 203);
      this.PortPanel.Name = "PortPanel";
      this.PortPanel.Size = new System.Drawing.Size(110, 35);
      this.PortPanel.TabIndex = 9;
      // 
      // PipeNamePanel
      // 
      this.PipeNamePanel.AccessibleDescription = "A panel containing controls related to the pipe name option";
      this.PipeNamePanel.AccessibleName = "Pipe Name Group";
      this.PipeNamePanel.AutoSize = true;
      this.PipeNamePanel.Controls.Add(this.PipeNameLabel);
      this.PipeNamePanel.Controls.Add(this.PipeNameTextBox);
      this.PipeNamePanel.Location = new System.Drawing.Point(206, 253);
      this.PipeNamePanel.Name = "PipeNamePanel";
      this.PipeNamePanel.Size = new System.Drawing.Size(343, 29);
      this.PipeNamePanel.TabIndex = 13;
      // 
      // MemoryNamePanel
      // 
      this.MemoryNamePanel.AccessibleDescription = "A panel containing controls related to the memory name option";
      this.MemoryNamePanel.AccessibleName = "Memory Name Group";
      this.MemoryNamePanel.AutoSize = true;
      this.MemoryNamePanel.BackColor = System.Drawing.Color.Transparent;
      this.MemoryNamePanel.Controls.Add(this.SharedMemoryNameLabel);
      this.MemoryNamePanel.Controls.Add(this.SharedMemoryNameTextBox);
      this.MemoryNamePanel.Location = new System.Drawing.Point(184, 282);
      this.MemoryNamePanel.Name = "MemoryNamePanel";
      this.MemoryNamePanel.Size = new System.Drawing.Size(365, 29);
      this.MemoryNamePanel.TabIndex = 15;
      // 
      // XProtocolPortPanel
      // 
      this.XProtocolPortPanel.AccessibleDescription = "A panel containing controls related to the X Protocol port";
      this.XProtocolPortPanel.AccessibleName = "X Protocol Port Group";
      this.XProtocolPortPanel.AutoSize = true;
      this.XProtocolPortPanel.Controls.Add(this.XProtocolPortLabel);
      this.XProtocolPortPanel.Controls.Add(this.XProtocolPortTextBox);
      this.XProtocolPortPanel.Location = new System.Drawing.Point(381, 203);
      this.XProtocolPortPanel.Name = "XProtocolPortPanel";
      this.XProtocolPortPanel.Size = new System.Drawing.Size(168, 35);
      this.XProtocolPortPanel.TabIndex = 10;
      // 
      // XProtocolPortLabel
      // 
      this.XProtocolPortLabel.AccessibleDescription = "A label displaying the text X Protocol port";
      this.XProtocolPortLabel.AccessibleName = "X Protocol Port Text";
      this.XProtocolPortLabel.AutoSize = true;
      this.XProtocolPortLabel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.XProtocolPortLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.XProtocolPortLabel.Location = new System.Drawing.Point(3, 0);
      this.XProtocolPortLabel.Name = "XProtocolPortLabel";
      this.XProtocolPortLabel.Size = new System.Drawing.Size(90, 29);
      this.XProtocolPortLabel.TabIndex = 0;
      this.XProtocolPortLabel.Text = "X Protocol Port:";
      this.XProtocolPortLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.ToolTip.SetToolTip(this.XProtocolPortLabel, "X Protocol Port");
      // 
      // XProtocolPortTextBox
      // 
      this.XProtocolPortTextBox.AccessibleDescription = "A text box to input the X Protocol port number";
      this.XProtocolPortTextBox.AccessibleName = "X Protocol Port Number";
      this.XProtocolPortTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.XProtocolPortTextBox.Location = new System.Drawing.Point(99, 3);
      this.XProtocolPortTextBox.Name = "XProtocolPortTextBox";
      this.XProtocolPortTextBox.Size = new System.Drawing.Size(44, 23);
      this.XProtocolPortTextBox.TabIndex = 1;
      this.XProtocolPortTextBox.Text = "33060";
      this.ToolTip.SetToolTip(this.XProtocolPortTextBox, "X Protocol Port");
      this.XProtocolPortTextBox.WordWrap = false;
      this.XProtocolPortTextBox.TextChanged += new System.EventHandler(this.TextChangedHandler);
      this.XProtocolPortTextBox.Validated += new System.EventHandler(this.ValidatedHandler);
      // 
      // ServerConfigLocalMachinePage
      // 
      this.AccessibleDescription = "A configuration wizard page to configure the server type and networking options";
      this.AccessibleName = "Server Type And Networking Page";
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
      this.Caption = "Type and Networking";
      this.Controls.Add(this.PipeNamePanel);
      this.Controls.Add(this.PortPanel);
      this.Controls.Add(this.XProtocolPortPanel);
      this.Controls.Add(this.EnterpriseFirewallLinkLabel);
      this.Controls.Add(this.EnterpriseFirewallCheckBox);
      this.Controls.Add(this.EnterpriseFirewallDescription);
      this.Controls.Add(this.EnterpriseFirewallTitleLabel);
      this.Controls.Add(this.MemoryNamePanel);
      this.Controls.Add(this.SharedMemoryCheckBox);
      this.Controls.Add(this.NamedPipeCheckBox);
      this.Controls.Add(this.TcpIpCheckBox);
      this.Controls.Add(this.ConnectivityLabel);
      this.Controls.Add(this.OpenWindowsFirewallCheckBox);
      this.Controls.Add(this.ShowAdvancedLoggingOptionsCheckBox);
      this.Controls.Add(this.AdvancedConfigurationDescriptionLabel);
      this.Controls.Add(this.AdvancedConfigurationLabel);
      this.Controls.Add(this.ConfigTypeComboBox);
      this.Controls.Add(this.ConfigTypeLabel);
      this.Controls.Add(this.ServerConfigurationTypeDescriptionLabel);
      this.Controls.Add(this.ServerConfigurationTypeLabel);
      this.Controls.Add(this.ConnectivityDescriptionLabel);
      this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "ServerConfigLocalMachinePage";
      this.Controls.SetChildIndex(this.ConnectivityDescriptionLabel, 0);
      this.Controls.SetChildIndex(this.ServerConfigurationTypeLabel, 0);
      this.Controls.SetChildIndex(this.ServerConfigurationTypeDescriptionLabel, 0);
      this.Controls.SetChildIndex(this.ConfigTypeLabel, 0);
      this.Controls.SetChildIndex(this.ConfigTypeComboBox, 0);
      this.Controls.SetChildIndex(this.AdvancedConfigurationLabel, 0);
      this.Controls.SetChildIndex(this.AdvancedConfigurationDescriptionLabel, 0);
      this.Controls.SetChildIndex(this.ShowAdvancedLoggingOptionsCheckBox, 0);
      this.Controls.SetChildIndex(this.OpenWindowsFirewallCheckBox, 0);
      this.Controls.SetChildIndex(this.ConnectivityLabel, 0);
      this.Controls.SetChildIndex(this.TcpIpCheckBox, 0);
      this.Controls.SetChildIndex(this.NamedPipeCheckBox, 0);
      this.Controls.SetChildIndex(this.SharedMemoryCheckBox, 0);
      this.Controls.SetChildIndex(this.MemoryNamePanel, 0);
      this.Controls.SetChildIndex(this.EnterpriseFirewallTitleLabel, 0);
      this.Controls.SetChildIndex(this.EnterpriseFirewallDescription, 0);
      this.Controls.SetChildIndex(this.EnterpriseFirewallCheckBox, 0);
      this.Controls.SetChildIndex(this.EnterpriseFirewallLinkLabel, 0);
      this.Controls.SetChildIndex(this.subCaptionLabel, 0);
      this.Controls.SetChildIndex(this.captionLabel, 0);
      this.Controls.SetChildIndex(this.XProtocolPortPanel, 0);
      this.Controls.SetChildIndex(this.PortPanel, 0);
      this.Controls.SetChildIndex(this.PipeNamePanel, 0);
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).EndInit();
      this.PortPanel.ResumeLayout(false);
      this.PortPanel.PerformLayout();
      this.PipeNamePanel.ResumeLayout(false);
      this.PipeNamePanel.PerformLayout();
      this.MemoryNamePanel.ResumeLayout(false);
      this.MemoryNamePanel.PerformLayout();
      this.XProtocolPortPanel.ResumeLayout(false);
      this.XProtocolPortPanel.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label ServerConfigurationTypeLabel;
    private System.Windows.Forms.Label ServerConfigurationTypeDescriptionLabel;
    private System.Windows.Forms.Label ConfigTypeLabel;
    private System.Windows.Forms.Label PortLabel;
    private System.Windows.Forms.Label ConnectivityDescriptionLabel;
    private System.Windows.Forms.TextBox PortTextBox;
    private ImageComboBox ConfigTypeComboBox;
    private System.Windows.Forms.CheckBox ShowAdvancedLoggingOptionsCheckBox;
    private System.Windows.Forms.Label AdvancedConfigurationDescriptionLabel;
    private System.Windows.Forms.Label AdvancedConfigurationLabel;
    private System.Windows.Forms.CheckBox OpenWindowsFirewallCheckBox;
    private System.Windows.Forms.Label ConnectivityLabel;
    private System.Windows.Forms.CheckBox TcpIpCheckBox;
    private System.Windows.Forms.CheckBox NamedPipeCheckBox;
    private System.Windows.Forms.CheckBox SharedMemoryCheckBox;
    private System.Windows.Forms.Label PipeNameLabel;
    private System.Windows.Forms.TextBox PipeNameTextBox;
    private System.Windows.Forms.Label SharedMemoryNameLabel;
    private System.Windows.Forms.TextBox SharedMemoryNameTextBox;
    private System.Windows.Forms.CheckBox EnterpriseFirewallCheckBox;
    private System.Windows.Forms.Label EnterpriseFirewallDescription;
    private System.Windows.Forms.Label EnterpriseFirewallTitleLabel;
    private System.Windows.Forms.LinkLabel EnterpriseFirewallLinkLabel;
    private System.Windows.Forms.FlowLayoutPanel PortPanel;
    private System.Windows.Forms.FlowLayoutPanel PipeNamePanel;
    private System.Windows.Forms.FlowLayoutPanel MemoryNamePanel;
    private System.Windows.Forms.FlowLayoutPanel XProtocolPortPanel;
    private System.Windows.Forms.Label XProtocolPortLabel;
    private System.Windows.Forms.TextBox XProtocolPortTextBox;
  }
}

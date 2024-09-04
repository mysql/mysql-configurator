/* Copyright (c) 2023, 2024, Oracle and/or its affiliates.

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

namespace MySql.Configurator.Wizards.Server
{
  partial class ServerConfigServerInstallationsPage
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
      this.ServerInstallationsTitleLabel = new System.Windows.Forms.Label();
      this.ConnectionErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
      this.DataDirectoryBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
      this.InstallationTypeFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
      this.ReplaceServerInstallationRadioButton = new System.Windows.Forms.RadioButton();
      this.ReplaceInstallationOptionPanel = new System.Windows.Forms.Panel();
      this.ReplaceServerInstallationDescriptionLabel = new System.Windows.Forms.Label();
      this.ReplaceInstallationControlsPanel = new System.Windows.Forms.Panel();
      this.ExistingConfigFileBrowseButton = new System.Windows.Forms.Button();
      this.ExistingConfigFilePathLabel = new System.Windows.Forms.Label();
      this.ExistingConfigFilePathTextBox = new System.Windows.Forms.TextBox();
      this.InstallDirectoryTextBox = new System.Windows.Forms.TextBox();
      this.InstallDirectoryLabel = new System.Windows.Forms.Label();
      this.ExistingDataDirectoryLabel = new System.Windows.Forms.Label();
      this.VersionTextBox = new System.Windows.Forms.TextBox();
      this.VersionLabel = new System.Windows.Forms.Label();
      this.NameLabel = new System.Windows.Forms.Label();
      this.PipeOrSharedMemoryNameTextBox = new System.Windows.Forms.TextBox();
      this.PortLabel = new System.Windows.Forms.Label();
      this.PortTextBox = new System.Windows.Forms.TextBox();
      this.RootPasswordLabel = new System.Windows.Forms.Label();
      this.ProtocolLabel = new System.Windows.Forms.Label();
      this.ProtocolComboBox = new System.Windows.Forms.ComboBox();
      this.ExistingDataDirectoryTextBox = new System.Windows.Forms.TextBox();
      this.ReviewExistingServerInstallationDataLabel = new System.Windows.Forms.Label();
      this.ConnectButton = new System.Windows.Forms.Button();
      this.RootPasswordTextBox = new System.Windows.Forms.TextBox();
      this.ExistingServerConnectionLabel = new System.Windows.Forms.Label();
      this.SideBySideInstallationRadioButton = new System.Windows.Forms.RadioButton();
      this.SideBySideInstallationOptionPanel = new System.Windows.Forms.Panel();
      this.SideBySideInstallationDescriptionLabel = new System.Windows.Forms.Label();
      this.SideBySideInstallationControlsPanel = new System.Windows.Forms.Panel();
      this.NewDataDirectoryBrowseButton = new System.Windows.Forms.Button();
      this.ConfirmDataDirectoryLabel = new System.Windows.Forms.Label();
      this.NewDataDirectoryTextBox = new System.Windows.Forms.TextBox();
      this.ConfigFileDialog = new System.Windows.Forms.OpenFileDialog();
      this.DataDirectoryRenameWarningProvider = new System.Windows.Forms.ErrorProvider(this.components);
      this.VersionErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
      this.VersionWarningProvider = new System.Windows.Forms.ErrorProvider(this.components);
      this.UserWarningProvider = new System.Windows.Forms.ErrorProvider(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.ConnectionErrorProvider)).BeginInit();
      this.InstallationTypeFlowLayoutPanel.SuspendLayout();
      this.ReplaceInstallationOptionPanel.SuspendLayout();
      this.ReplaceInstallationControlsPanel.SuspendLayout();
      this.SideBySideInstallationOptionPanel.SuspendLayout();
      this.SideBySideInstallationControlsPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.DataDirectoryRenameWarningProvider)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.VersionErrorProvider)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.VersionWarningProvider)).BeginInit();
      this.SuspendLayout();
      // 
      // subCaptionLabel
      // 
      this.subCaptionLabel.Font = new System.Drawing.Font("Segoe UI", 4F);
      this.subCaptionLabel.Location = new System.Drawing.Point(40, 119);
      this.subCaptionLabel.Size = new System.Drawing.Size(742, 23);
      // 
      // captionLabel
      // 
      this.captionLabel.AccessibleDescription = "A title label for the configuration page about MySQL Server installations.";
      this.captionLabel.AccessibleName = "MySQL Server Installations";
      this.captionLabel.Location = new System.Drawing.Point(35, 30);
      this.captionLabel.Size = new System.Drawing.Size(235, 25);
      this.captionLabel.Text = "MySQL Server Installations";
      // 
      // ServerInstallationsTitleLabel
      // 
      this.ServerInstallationsTitleLabel.AccessibleDescription = "A label displaying a text explaining that existing MySQL Server installations wer" +
    "e found on the system.";
      this.ServerInstallationsTitleLabel.AccessibleName = "MySQL Server installations description text";
      this.ServerInstallationsTitleLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ServerInstallationsTitleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.ServerInstallationsTitleLabel.Location = new System.Drawing.Point(39, 107);
      this.ServerInstallationsTitleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.ServerInstallationsTitleLabel.Name = "ServerInstallationsTitleLabel";
      this.ServerInstallationsTitleLabel.Size = new System.Drawing.Size(804, 53);
      this.ServerInstallationsTitleLabel.TabIndex = 1;
      this.ServerInstallationsTitleLabel.Text = "An existing MySQL Server installation was found on your system.\r\nYou can either u" +
    "pgrade in-place or install side-by-side.";
      // 
      // ConnectionErrorProvider
      // 
      this.ConnectionErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
      this.ConnectionErrorProvider.ContainerControl = this;
      // 
      // DataDirectoryBrowserDialog
      // 
      this.DataDirectoryBrowserDialog.Description = "MySQL Server\'s Data Directory";
      this.DataDirectoryBrowserDialog.RootFolder = System.Environment.SpecialFolder.CommonPrograms;
      // 
      // InstallationTypeFlowLayoutPanel
      // 
      this.InstallationTypeFlowLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.InstallationTypeFlowLayoutPanel.Controls.Add(this.ReplaceServerInstallationRadioButton);
      this.InstallationTypeFlowLayoutPanel.Controls.Add(this.ReplaceInstallationOptionPanel);
      this.InstallationTypeFlowLayoutPanel.Controls.Add(this.ReplaceInstallationControlsPanel);
      this.InstallationTypeFlowLayoutPanel.Controls.Add(this.SideBySideInstallationRadioButton);
      this.InstallationTypeFlowLayoutPanel.Controls.Add(this.SideBySideInstallationOptionPanel);
      this.InstallationTypeFlowLayoutPanel.Controls.Add(this.SideBySideInstallationControlsPanel);
      this.InstallationTypeFlowLayoutPanel.Location = new System.Drawing.Point(41, 167);
      this.InstallationTypeFlowLayoutPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.InstallationTypeFlowLayoutPanel.Name = "InstallationTypeFlowLayoutPanel";
      this.InstallationTypeFlowLayoutPanel.Size = new System.Drawing.Size(804, 1158);
      this.InstallationTypeFlowLayoutPanel.TabIndex = 2;
      // 
      // ReplaceServerInstallationRadioButton
      // 
      this.ReplaceServerInstallationRadioButton.AccessibleDescription = "An option to replace a selected MySQL Server installation, meaning its data direc" +
    "tory will be used for the installation being configured.";
      this.ReplaceServerInstallationRadioButton.AccessibleName = "Replace MySQL Server installation option";
      this.ReplaceServerInstallationRadioButton.AutoSize = true;
      this.ReplaceServerInstallationRadioButton.Checked = true;
      this.ReplaceServerInstallationRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ReplaceServerInstallationRadioButton.Location = new System.Drawing.Point(4, 5);
      this.ReplaceServerInstallationRadioButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.ReplaceServerInstallationRadioButton.Name = "ReplaceServerInstallationRadioButton";
      this.ReplaceServerInstallationRadioButton.Size = new System.Drawing.Size(802, 29);
      this.ReplaceServerInstallationRadioButton.TabIndex = 4;
      this.ReplaceServerInstallationRadioButton.TabStop = true;
      this.ReplaceServerInstallationRadioButton.Text = "Perform an in-place upgrade of the existing MySQL Server installation (it must be " +
    "running)";
      this.ReplaceServerInstallationRadioButton.UseVisualStyleBackColor = true;
      this.ReplaceServerInstallationRadioButton.CheckedChanged += new System.EventHandler(this.InstallationRadioButtonsCheckedChanged);
      // 
      // ReplaceInstallationOptionPanel
      // 
      this.ReplaceInstallationOptionPanel.AccessibleDescription = "A panel containing the option for replacing an existing MySQL Server installation" +
    ".";
      this.ReplaceInstallationOptionPanel.AccessibleName = "Replace MySQL Server installation option";
      this.ReplaceInstallationOptionPanel.Controls.Add(this.ReplaceServerInstallationDescriptionLabel);
      this.ReplaceInstallationOptionPanel.Location = new System.Drawing.Point(4, 44);
      this.ReplaceInstallationOptionPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.ReplaceInstallationOptionPanel.Name = "ReplaceInstallationOptionPanel";
      this.ReplaceInstallationOptionPanel.Size = new System.Drawing.Size(796, 102);
      this.ReplaceInstallationOptionPanel.TabIndex = 5;
      // 
      // ReplaceServerInstallationDescriptionLabel
      // 
      this.ReplaceServerInstallationDescriptionLabel.AccessibleDescription = "A label displaying a text explaining that the chosen existing server installation" +
    " will be replaced and its data upgraded to be used with the new server installat" +
    "ion being configured.";
      this.ReplaceServerInstallationDescriptionLabel.AccessibleName = "Replace existing MySQL Server installation description text";
      this.ReplaceServerInstallationDescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ReplaceServerInstallationDescriptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.ReplaceServerInstallationDescriptionLabel.Location = new System.Drawing.Point(0, 10);
      this.ReplaceServerInstallationDescriptionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.ReplaceServerInstallationDescriptionLabel.Name = "ReplaceServerInstallationDescriptionLabel";
      this.ReplaceServerInstallationDescriptionLabel.Size = new System.Drawing.Size(770, 91);
      this.ReplaceServerInstallationDescriptionLabel.TabIndex = 1;
      this.ReplaceServerInstallationDescriptionLabel.Text = "The existing server installation will be replaced as part of the upgrade process." +
    " In certain cases the data will also be ugraded.\r\nUpon success, the previous ver" +
    "sion will be removed from the system.";
      // 
      // ReplaceInstallationControlsPanel
      // 
      this.ReplaceInstallationControlsPanel.AccessibleDescription = "A panel containing all controls related to replacing an existing MySQL Server ins" +
    "tallation.";
      this.ReplaceInstallationControlsPanel.AccessibleName = "Replace MySQL Server installation steps";
      this.ReplaceInstallationControlsPanel.BackColor = System.Drawing.Color.Transparent;
      this.ReplaceInstallationControlsPanel.Controls.Add(this.ExistingConfigFileBrowseButton);
      this.ReplaceInstallationControlsPanel.Controls.Add(this.ExistingConfigFilePathLabel);
      this.ReplaceInstallationControlsPanel.Controls.Add(this.ExistingConfigFilePathTextBox);
      this.ReplaceInstallationControlsPanel.Controls.Add(this.InstallDirectoryTextBox);
      this.ReplaceInstallationControlsPanel.Controls.Add(this.InstallDirectoryLabel);
      this.ReplaceInstallationControlsPanel.Controls.Add(this.ExistingDataDirectoryLabel);
      this.ReplaceInstallationControlsPanel.Controls.Add(this.VersionTextBox);
      this.ReplaceInstallationControlsPanel.Controls.Add(this.VersionLabel);
      this.ReplaceInstallationControlsPanel.Controls.Add(this.NameLabel);
      this.ReplaceInstallationControlsPanel.Controls.Add(this.PipeOrSharedMemoryNameTextBox);
      this.ReplaceInstallationControlsPanel.Controls.Add(this.PortLabel);
      this.ReplaceInstallationControlsPanel.Controls.Add(this.PortTextBox);
      this.ReplaceInstallationControlsPanel.Controls.Add(this.RootPasswordLabel);
      this.ReplaceInstallationControlsPanel.Controls.Add(this.ProtocolLabel);
      this.ReplaceInstallationControlsPanel.Controls.Add(this.ProtocolComboBox);
      this.ReplaceInstallationControlsPanel.Controls.Add(this.ExistingDataDirectoryTextBox);
      this.ReplaceInstallationControlsPanel.Controls.Add(this.ReviewExistingServerInstallationDataLabel);
      this.ReplaceInstallationControlsPanel.Controls.Add(this.ConnectButton);
      this.ReplaceInstallationControlsPanel.Controls.Add(this.RootPasswordTextBox);
      this.ReplaceInstallationControlsPanel.Controls.Add(this.ExistingServerConnectionLabel);
      this.ReplaceInstallationControlsPanel.Location = new System.Drawing.Point(4, 156);
      this.ReplaceInstallationControlsPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.ReplaceInstallationControlsPanel.Name = "ReplaceInstallationControlsPanel";
      this.ReplaceInstallationControlsPanel.Size = new System.Drawing.Size(796, 419);
      this.ReplaceInstallationControlsPanel.TabIndex = 6;
      // 
      // ExistingConfigFileBrowseButton
      // 
      this.ExistingConfigFileBrowseButton.AccessibleDescription = "A button to open a dialog to browse the file system and select a global configura" +
    "tion file.";
      this.ExistingConfigFileBrowseButton.AccessibleName = "Configuration file browse button";
      this.ExistingConfigFileBrowseButton.Enabled = false;
      this.ExistingConfigFileBrowseButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ExistingConfigFileBrowseButton.Location = new System.Drawing.Point(621, 359);
      this.ExistingConfigFileBrowseButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.ExistingConfigFileBrowseButton.Name = "ExistingConfigFileBrowseButton";
      this.ExistingConfigFileBrowseButton.Size = new System.Drawing.Size(111, 41);
      this.ExistingConfigFileBrowseButton.TabIndex = 19;
      this.ExistingConfigFileBrowseButton.Text = "Browse...";
      this.ExistingConfigFileBrowseButton.UseVisualStyleBackColor = true;
      this.ExistingConfigFileBrowseButton.Click += new System.EventHandler(this.ExistingConfigFileBrowseButton_Click);
      // 
      // ExistingConfigFilePathLabel
      // 
      this.ExistingConfigFilePathLabel.AccessibleDescription = "A text to ask for the selected MySQL Server port.";
      this.ExistingConfigFilePathLabel.AccessibleName = "Selected MySQL server port number label";
      this.ExistingConfigFilePathLabel.AutoSize = true;
      this.ExistingConfigFilePathLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ExistingConfigFilePathLabel.Location = new System.Drawing.Point(34, 364);
      this.ExistingConfigFilePathLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.ExistingConfigFilePathLabel.Name = "ExistingConfigFilePathLabel";
      this.ExistingConfigFilePathLabel.Size = new System.Drawing.Size(156, 25);
      this.ExistingConfigFilePathLabel.TabIndex = 17;
      this.ExistingConfigFilePathLabel.Text = "Configuration File:";
      // 
      // ExistingConfigFilePathTextBox
      // 
      this.ExistingConfigFilePathTextBox.AccessibleDescription = "The path of the existing MySQL Server\'s global configuration file.";
      this.ExistingConfigFilePathTextBox.AccessibleName = "Existing global configuration file path";
      this.ExistingConfigFilePathTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ExistingConfigFilePathTextBox.Location = new System.Drawing.Point(201, 359);
      this.ExistingConfigFilePathTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.ExistingConfigFilePathTextBox.Name = "ExistingConfigFilePathTextBox";
      this.ExistingConfigFilePathTextBox.ReadOnly = true;
      this.ExistingConfigFilePathTextBox.Size = new System.Drawing.Size(412, 31);
      this.ExistingConfigFilePathTextBox.TabIndex = 18;
      this.ToolTip.SetToolTip(this.ExistingConfigFilePathTextBox, "The full path of the data directory used by the chosen MySQL Server installation " +
        "to replace.\r\nThis data directory will be re-used by the MySQL Server being confi" +
        "gured and its data upgraded.");
      // 
      // InstallDirectoryTextBox
      // 
      this.InstallDirectoryTextBox.AccessibleDescription = "The data directory path of the existing MySQL Server installation to replace.";
      this.InstallDirectoryTextBox.AccessibleName = "Existing installation directory path";
      this.InstallDirectoryTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.InstallDirectoryTextBox.Location = new System.Drawing.Point(201, 257);
      this.InstallDirectoryTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.InstallDirectoryTextBox.Name = "InstallDirectoryTextBox";
      this.InstallDirectoryTextBox.ReadOnly = true;
      this.InstallDirectoryTextBox.Size = new System.Drawing.Size(529, 31);
      this.InstallDirectoryTextBox.TabIndex = 14;
      this.ToolTip.SetToolTip(this.InstallDirectoryTextBox, "The directory where the MySQL Server binaries are located.\r\nThis directory will b" +
        "e deleted if the option to remove the server installation is selected.");
      // 
      // InstallDirectoryLabel
      // 
      this.InstallDirectoryLabel.AccessibleDescription = "Text about the existing MySQL Server\'s install directory.";
      this.InstallDirectoryLabel.AccessibleName = "Existing MySQL server install directory label";
      this.InstallDirectoryLabel.AutoSize = true;
      this.InstallDirectoryLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.InstallDirectoryLabel.Location = new System.Drawing.Point(54, 262);
      this.InstallDirectoryLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.InstallDirectoryLabel.Name = "InstallDirectoryLabel";
      this.InstallDirectoryLabel.Size = new System.Drawing.Size(139, 25);
      this.InstallDirectoryLabel.TabIndex = 13;
      this.InstallDirectoryLabel.Text = "Install Directory:";
      // 
      // ExistingDataDirectoryLabel
      // 
      this.ExistingDataDirectoryLabel.AccessibleDescription = "Text about the existing MySQL Server\'s data directory.";
      this.ExistingDataDirectoryLabel.AccessibleName = "Existing MySQL Server data directory label";
      this.ExistingDataDirectoryLabel.AutoSize = true;
      this.ExistingDataDirectoryLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ExistingDataDirectoryLabel.Location = new System.Drawing.Point(64, 313);
      this.ExistingDataDirectoryLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.ExistingDataDirectoryLabel.Name = "ExistingDataDirectoryLabel";
      this.ExistingDataDirectoryLabel.Size = new System.Drawing.Size(130, 25);
      this.ExistingDataDirectoryLabel.TabIndex = 15;
      this.ExistingDataDirectoryLabel.Text = "Data Directory:";
      // 
      // VersionTextBox
      // 
      this.VersionTextBox.AccessibleDescription = "The existing MySQL Server version.";
      this.VersionTextBox.AccessibleName = "Existing MySQL Server version";
      this.VersionTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.VersionTextBox.Location = new System.Drawing.Point(201, 205);
      this.VersionTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.VersionTextBox.Name = "VersionTextBox";
      this.VersionTextBox.ReadOnly = true;
      this.VersionTextBox.Size = new System.Drawing.Size(186, 31);
      this.VersionTextBox.TabIndex = 12;
      this.ToolTip.SetToolTip(this.VersionTextBox, "The full path to the data directory used by the selected MySQL Server installatio" +
        "n.\r\nThis data directory will be used by the MySQL Server being configured.");
      // 
      // VersionLabel
      // 
      this.VersionLabel.AccessibleDescription = "Text about the existing MySQL Server version.";
      this.VersionLabel.AccessibleName = "Existing MySQL Server version label";
      this.VersionLabel.AutoSize = true;
      this.VersionLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.VersionLabel.Location = new System.Drawing.Point(120, 211);
      this.VersionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.VersionLabel.Name = "VersionLabel";
      this.VersionLabel.Size = new System.Drawing.Size(74, 25);
      this.VersionLabel.TabIndex = 11;
      this.VersionLabel.Text = "Version:";
      // 
      // NameLabel
      // 
      this.NameLabel.AccessibleDescription = "A text to ask for the selected MySQL Server named pipe or shared memory name.";
      this.NameLabel.AccessibleName = "Selected MySQL server named pipe or shared memory label";
      this.NameLabel.AutoSize = true;
      this.NameLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.NameLabel.Location = new System.Drawing.Point(422, 52);
      this.NameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.NameLabel.Name = "NameLabel";
      this.NameLabel.Size = new System.Drawing.Size(63, 25);
      this.NameLabel.TabIndex = 3;
      this.NameLabel.Text = "Name:";
      // 
      // PipeOrSharedMemoryNameTextBox
      // 
      this.PipeOrSharedMemoryNameTextBox.AccessibleDescription = "A text box to input the named pipe or shared memory name of the existing MySQL Se" +
    "rver instance.";
      this.PipeOrSharedMemoryNameTextBox.AccessibleName = "Pipe shared memory name";
      this.PipeOrSharedMemoryNameTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PipeOrSharedMemoryNameTextBox.Location = new System.Drawing.Point(493, 46);
      this.PipeOrSharedMemoryNameTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.PipeOrSharedMemoryNameTextBox.Name = "PipeOrSharedMemoryNameTextBox";
      this.PipeOrSharedMemoryNameTextBox.Size = new System.Drawing.Size(251, 31);
      this.PipeOrSharedMemoryNameTextBox.TabIndex = 6;
      this.ToolTip.SetToolTip(this.PipeOrSharedMemoryNameTextBox, "The name of the pipe or shared memory used by the MySQL Server to replace.");
      this.PipeOrSharedMemoryNameTextBox.TextChanged += new System.EventHandler(this.TextChangedHandler);
      this.PipeOrSharedMemoryNameTextBox.Validated += new System.EventHandler(this.ValidatedHandler);
      // 
      // PortLabel
      // 
      this.PortLabel.AccessibleDescription = "A text to ask for the existing MySQL Server\'s port.";
      this.PortLabel.AccessibleName = "Existing MySQL Server port number label";
      this.PortLabel.AutoSize = true;
      this.PortLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PortLabel.Location = new System.Drawing.Point(415, 52);
      this.PortLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.PortLabel.Name = "PortLabel";
      this.PortLabel.Size = new System.Drawing.Size(48, 25);
      this.PortLabel.TabIndex = 4;
      this.PortLabel.Text = "Port:";
      // 
      // PortTextBox
      // 
      this.PortTextBox.AccessibleDescription = "A text box to input the TCP IP port number of the existing MySQL Server instance." +
    "";
      this.PortTextBox.AccessibleName = "Existing MySQL server port number";
      this.PortTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PortTextBox.Location = new System.Drawing.Point(479, 46);
      this.PortTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.PortTextBox.Name = "PortTextBox";
      this.PortTextBox.Size = new System.Drawing.Size(74, 31);
      this.PortTextBox.TabIndex = 4;
      this.PortTextBox.Text = "3306";
      this.ToolTip.SetToolTip(this.PortTextBox, "The TCP/IP port of the MySQL Server installation to replace.");
      this.PortTextBox.TextChanged += new System.EventHandler(this.TextChangedHandler);
      this.PortTextBox.Validated += new System.EventHandler(this.ValidatedHandler);
      // 
      // RootPasswordLabel
      // 
      this.RootPasswordLabel.AccessibleDescription = "A text to ask for the root user\'s password.";
      this.RootPasswordLabel.AccessibleName = "Root password label";
      this.RootPasswordLabel.AutoSize = true;
      this.RootPasswordLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.RootPasswordLabel.Location = new System.Drawing.Point(54, 103);
      this.RootPasswordLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.RootPasswordLabel.Name = "RootPasswordLabel";
      this.RootPasswordLabel.Size = new System.Drawing.Size(136, 25);
      this.RootPasswordLabel.TabIndex = 7;
      this.RootPasswordLabel.Text = "Root password:";
      // 
      // ProtocolLabel
      // 
      this.ProtocolLabel.AccessibleDescription = "A text to ask for the selected MySQL Server port.";
      this.ProtocolLabel.AccessibleName = "Selected MySQL server port number label";
      this.ProtocolLabel.AutoSize = true;
      this.ProtocolLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ProtocolLabel.Location = new System.Drawing.Point(54, 50);
      this.ProtocolLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.ProtocolLabel.Name = "ProtocolLabel";
      this.ProtocolLabel.Size = new System.Drawing.Size(83, 25);
      this.ProtocolLabel.TabIndex = 1;
      this.ProtocolLabel.Text = "Protocol:";
      // 
      // ProtocolComboBox
      // 
      this.ProtocolComboBox.AccessibleDescription = "A drop-down list with connection protocols of an existing MySQL Server instance.";
      this.ProtocolComboBox.AccessibleName = "Existing MySQL Server instance connection protocol";
      this.ProtocolComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.ProtocolComboBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ProtocolComboBox.FormattingEnabled = true;
      this.ProtocolComboBox.Items.AddRange(new object[] {
            "TCP/IP",
            "Named Pipe",
            "Shared Memory"});
      this.ProtocolComboBox.Location = new System.Drawing.Point(195, 44);
      this.ProtocolComboBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.ProtocolComboBox.Name = "ProtocolComboBox";
      this.ProtocolComboBox.Size = new System.Drawing.Size(192, 33);
      this.ProtocolComboBox.TabIndex = 2;
      this.ToolTip.SetToolTip(this.ProtocolComboBox, "The protocol used by the MySQL Server installation to replace to accept connectio" +
        "ns.");
      this.ProtocolComboBox.SelectedIndexChanged += new System.EventHandler(this.ProtocolComboBox_SelectedIndexChanged);
      // 
      // ExistingDataDirectoryTextBox
      // 
      this.ExistingDataDirectoryTextBox.AccessibleDescription = "The data directory path of the existing MySQL Server installation to replace.";
      this.ExistingDataDirectoryTextBox.AccessibleName = "Existing data directory path";
      this.ExistingDataDirectoryTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ExistingDataDirectoryTextBox.Location = new System.Drawing.Point(201, 308);
      this.ExistingDataDirectoryTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.ExistingDataDirectoryTextBox.Name = "ExistingDataDirectoryTextBox";
      this.ExistingDataDirectoryTextBox.ReadOnly = true;
      this.ExistingDataDirectoryTextBox.Size = new System.Drawing.Size(529, 31);
      this.ExistingDataDirectoryTextBox.TabIndex = 16;
      this.ToolTip.SetToolTip(this.ExistingDataDirectoryTextBox, "The full path of the data directory used by the chosen MySQL Server installation " +
        "to replace.\r\nThis data directory will be re-used by the MySQL Server being confi" +
        "gured and its data upgraded.");
      // 
      // ReviewExistingServerInstallationDataLabel
      // 
      this.ReviewExistingServerInstallationDataLabel.AccessibleDescription = "The second step is to review the information of the MySQL Server installation tha" +
    "t will be replaced.";
      this.ReviewExistingServerInstallationDataLabel.AccessibleName = "Review the information of the MySQL Server to replace";
      this.ReviewExistingServerInstallationDataLabel.AutoSize = true;
      this.ReviewExistingServerInstallationDataLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ReviewExistingServerInstallationDataLabel.Location = new System.Drawing.Point(10, 168);
      this.ReviewExistingServerInstallationDataLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.ReviewExistingServerInstallationDataLabel.Name = "ReviewExistingServerInstallationDataLabel";
      this.ReviewExistingServerInstallationDataLabel.Size = new System.Drawing.Size(558, 25);
      this.ReviewExistingServerInstallationDataLabel.TabIndex = 10;
      this.ReviewExistingServerInstallationDataLabel.Text = "2. Review the information of the MySQL Server installation to replace:";
      // 
      // ConnectButton
      // 
      this.ConnectButton.AccessibleDescription = "A button to connect to the existing MySQL Server instance.";
      this.ConnectButton.AccessibleName = "Connect";
      this.ConnectButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ConnectButton.Location = new System.Drawing.Point(445, 97);
      this.ConnectButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.ConnectButton.Name = "ConnectButton";
      this.ConnectButton.Size = new System.Drawing.Size(111, 41);
      this.ConnectButton.TabIndex = 9;
      this.ConnectButton.Text = "Connect";
      this.ToolTip.SetToolTip(this.ConnectButton, "Tests the connection to the MySQL Server installation to replace.");
      this.ConnectButton.UseVisualStyleBackColor = true;
      this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
      // 
      // RootPasswordTextBox
      // 
      this.RootPasswordTextBox.AccessibleDescription = "A field to input the root user\'s password.";
      this.RootPasswordTextBox.AccessibleName = "Root password";
      this.RootPasswordTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.RootPasswordTextBox.Location = new System.Drawing.Point(195, 97);
      this.RootPasswordTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.RootPasswordTextBox.Name = "RootPasswordTextBox";
      this.RootPasswordTextBox.PasswordChar = '*';
      this.RootPasswordTextBox.Size = new System.Drawing.Size(192, 31);
      this.RootPasswordTextBox.TabIndex = 8;
      this.ToolTip.SetToolTip(this.RootPasswordTextBox, "The password of the root account of the MySQL Server installation to replace.");
      this.RootPasswordTextBox.UseSystemPasswordChar = true;
      this.RootPasswordTextBox.TextChanged += new System.EventHandler(this.TextChangedHandler);
      this.RootPasswordTextBox.Validated += new System.EventHandler(this.ValidatedHandler);
      // 
      // ExistingServerConnectionLabel
      // 
      this.ExistingServerConnectionLabel.AccessibleDescription = "The first step is to connect to an existing MySQL Server that will be replaced.";
      this.ExistingServerConnectionLabel.AccessibleName = "Connect to the existing MySQL Server to replace";
      this.ExistingServerConnectionLabel.AutoSize = true;
      this.ExistingServerConnectionLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ExistingServerConnectionLabel.Location = new System.Drawing.Point(10, 10);
      this.ExistingServerConnectionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.ExistingServerConnectionLabel.Name = "ExistingServerConnectionLabel";
      this.ExistingServerConnectionLabel.Size = new System.Drawing.Size(552, 25);
      this.ExistingServerConnectionLabel.TabIndex = 0;
      this.ExistingServerConnectionLabel.Text = "1. Connect to the existing MySQL Server installation as the root user:";
      // 
      // SideBySideInstallationRadioButton
      // 
      this.SideBySideInstallationRadioButton.AccessibleDescription = "An option to configure this MySQL Server as a side-by-side installation with othe" +
    "r existing MySQL Server installations.";
      this.SideBySideInstallationRadioButton.AccessibleName = "Configure server side-by-side with existing installations option";
      this.SideBySideInstallationRadioButton.AutoSize = true;
      this.SideBySideInstallationRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.SideBySideInstallationRadioButton.Location = new System.Drawing.Point(4, 585);
      this.SideBySideInstallationRadioButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.SideBySideInstallationRadioButton.Name = "SideBySideInstallationRadioButton";
      this.SideBySideInstallationRadioButton.Size = new System.Drawing.Size(537, 29);
      this.SideBySideInstallationRadioButton.TabIndex = 7;
      this.SideBySideInstallationRadioButton.Text = "Configure this server instance as a side-by-side installation";
      this.SideBySideInstallationRadioButton.UseVisualStyleBackColor = true;
      this.SideBySideInstallationRadioButton.CheckedChanged += new System.EventHandler(this.InstallationRadioButtonsCheckedChanged);
      // 
      // SideBySideInstallationOptionPanel
      // 
      this.SideBySideInstallationOptionPanel.AccessibleDescription = "A panel containing the option for installing the MySQL Server side by side with o" +
    "ther existing server installations..";
      this.SideBySideInstallationOptionPanel.AccessibleName = "Configure server side by side option";
      this.SideBySideInstallationOptionPanel.Controls.Add(this.SideBySideInstallationDescriptionLabel);
      this.SideBySideInstallationOptionPanel.Location = new System.Drawing.Point(4, 624);
      this.SideBySideInstallationOptionPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.SideBySideInstallationOptionPanel.Name = "SideBySideInstallationOptionPanel";
      this.SideBySideInstallationOptionPanel.Size = new System.Drawing.Size(796, 80);
      this.SideBySideInstallationOptionPanel.TabIndex = 8;
      // 
      // SideBySideInstallationDescriptionLabel
      // 
      this.SideBySideInstallationDescriptionLabel.AccessibleDescription = "A label displaying a text explaining that this server installation will be config" +
    "ured side by side with other existing MySQL Server installations.";
      this.SideBySideInstallationDescriptionLabel.AccessibleName = "Syde by side installation description text";
      this.SideBySideInstallationDescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.SideBySideInstallationDescriptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.SideBySideInstallationDescriptionLabel.Location = new System.Drawing.Point(0, 15);
      this.SideBySideInstallationDescriptionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.SideBySideInstallationDescriptionLabel.Name = "SideBySideInstallationDescriptionLabel";
      this.SideBySideInstallationDescriptionLabel.Size = new System.Drawing.Size(782, 55);
      this.SideBySideInstallationDescriptionLabel.TabIndex = 1;
      this.SideBySideInstallationDescriptionLabel.Text = "This server installation will be configured independently and will reside side-by" +
    "-side with any other server installation currently existing on this system.";
      // 
      // SideBySideInstallationControlsPanel
      // 
      this.SideBySideInstallationControlsPanel.AccessibleDescription = "A panel containing all controls related to configuring this MySQL Server instance" +
    " side by side with other existing server installations.";
      this.SideBySideInstallationControlsPanel.AccessibleName = "Configure server side by side controls";
      this.SideBySideInstallationControlsPanel.Controls.Add(this.NewDataDirectoryBrowseButton);
      this.SideBySideInstallationControlsPanel.Controls.Add(this.ConfirmDataDirectoryLabel);
      this.SideBySideInstallationControlsPanel.Controls.Add(this.NewDataDirectoryTextBox);
      this.SideBySideInstallationControlsPanel.Location = new System.Drawing.Point(4, 714);
      this.SideBySideInstallationControlsPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.SideBySideInstallationControlsPanel.Name = "SideBySideInstallationControlsPanel";
      this.SideBySideInstallationControlsPanel.Size = new System.Drawing.Size(796, 85);
      this.SideBySideInstallationControlsPanel.TabIndex = 9;
      this.SideBySideInstallationControlsPanel.Visible = false;
      // 
      // NewDataDirectoryBrowseButton
      // 
      this.NewDataDirectoryBrowseButton.AccessibleDescription = "A button to open a dialog to browse the file system and select a data directory.";
      this.NewDataDirectoryBrowseButton.AccessibleName = "Data directory browse button";
      this.NewDataDirectoryBrowseButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.NewDataDirectoryBrowseButton.Location = new System.Drawing.Point(614, 37);
      this.NewDataDirectoryBrowseButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.NewDataDirectoryBrowseButton.Name = "NewDataDirectoryBrowseButton";
      this.NewDataDirectoryBrowseButton.Size = new System.Drawing.Size(111, 41);
      this.NewDataDirectoryBrowseButton.TabIndex = 2;
      this.NewDataDirectoryBrowseButton.Text = "Browse...";
      this.NewDataDirectoryBrowseButton.UseVisualStyleBackColor = true;
      this.NewDataDirectoryBrowseButton.Click += new System.EventHandler(this.NewDataDirectoryBrowseButton_Click);
      // 
      // ConfirmDataDirectoryLabel
      // 
      this.ConfirmDataDirectoryLabel.AccessibleDescription = "A text describing that this MySQL Server installation will be configured independ" +
    "ently and will exist side by side with other existing MySQL Server installations" +
    ".";
      this.ConfirmDataDirectoryLabel.AccessibleName = "Confirm data directory label";
      this.ConfirmDataDirectoryLabel.AutoSize = true;
      this.ConfirmDataDirectoryLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ConfirmDataDirectoryLabel.Location = new System.Drawing.Point(3, 7);
      this.ConfirmDataDirectoryLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.ConfirmDataDirectoryLabel.Name = "ConfirmDataDirectoryLabel";
      this.ConfirmDataDirectoryLabel.Size = new System.Drawing.Size(597, 25);
      this.ConfirmDataDirectoryLabel.TabIndex = 0;
      this.ConfirmDataDirectoryLabel.Text = "Confirm the data directory path to use with this MySQL Server installation:";
      // 
      // NewDataDirectoryTextBox
      // 
      this.NewDataDirectoryTextBox.AccessibleDescription = "The data directory path to use with this MySQL Server installation being configur" +
    "ed.";
      this.NewDataDirectoryTextBox.AccessibleName = "New data directory path";
      this.NewDataDirectoryTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.NewDataDirectoryTextBox.Location = new System.Drawing.Point(8, 39);
      this.NewDataDirectoryTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.NewDataDirectoryTextBox.Name = "NewDataDirectoryTextBox";
      this.NewDataDirectoryTextBox.Size = new System.Drawing.Size(566, 31);
      this.NewDataDirectoryTextBox.TabIndex = 1;
      this.ToolTip.SetToolTip(this.NewDataDirectoryTextBox, "The full path to the data directory that will be used by the MySQL Server being c" +
        "onfigured.");
      this.NewDataDirectoryTextBox.TextChanged += new System.EventHandler(this.TextChangedHandler);
      this.NewDataDirectoryTextBox.Validated += new System.EventHandler(this.ValidatedHandler);
      // 
      // ConfigFileDialog
      // 
      this.ConfigFileDialog.Filter = "MySQL Server configuration files (*.ini, *.cnf)|*.ini;*.cnf";
      this.ConfigFileDialog.Title = "MySQL Server\'s global configuration file";
      // 
      // DataDirectoryRenameWarningProvider
      // 
      this.DataDirectoryRenameWarningProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
      this.DataDirectoryRenameWarningProvider.ContainerControl = this;
      // 
      // VersionErrorProvider
      // 
      this.VersionErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
      this.VersionErrorProvider.ContainerControl = this;
      // 
      // VersionWarningProvider
      // 
      this.VersionWarningProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
      this.VersionWarningProvider.ContainerControl = this;
      // 
      // UserWarningProvider
      // 
      this.UserWarningProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
      this.UserWarningProvider.ContainerControl = this;
      // 
      // ServerConfigServerInstallationsPage
      // 
      this.AccessibleDescription = "A configuration wizard page used to select the type of server installation";
      this.AccessibleName = "MySQL Server Installations Page";
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 23F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Caption = "MySQL Server Installations";
      this.Controls.Add(this.InstallationTypeFlowLayoutPanel);
      this.Controls.Add(this.ServerInstallationsTitleLabel);
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.Name = "ServerConfigServerInstallationsPage";
      this.Size = new System.Drawing.Size(849, 1044);
      this.Controls.SetChildIndex(this.subCaptionLabel, 0);
      this.Controls.SetChildIndex(this.ServerInstallationsTitleLabel, 0);
      this.Controls.SetChildIndex(this.InstallationTypeFlowLayoutPanel, 0);
      this.Controls.SetChildIndex(this.captionLabel, 0);
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.ConnectionErrorProvider)).EndInit();
      this.InstallationTypeFlowLayoutPanel.ResumeLayout(false);
      this.InstallationTypeFlowLayoutPanel.PerformLayout();
      this.ReplaceInstallationOptionPanel.ResumeLayout(false);
      this.ReplaceInstallationControlsPanel.ResumeLayout(false);
      this.ReplaceInstallationControlsPanel.PerformLayout();
      this.SideBySideInstallationOptionPanel.ResumeLayout(false);
      this.SideBySideInstallationControlsPanel.ResumeLayout(false);
      this.SideBySideInstallationControlsPanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.DataDirectoryRenameWarningProvider)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.VersionErrorProvider)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.VersionWarningProvider)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.UserWarningProvider)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label ServerInstallationsTitleLabel;
    private System.Windows.Forms.ErrorProvider ConnectionErrorProvider;
    private System.Windows.Forms.FolderBrowserDialog DataDirectoryBrowserDialog;
    private System.Windows.Forms.FlowLayoutPanel InstallationTypeFlowLayoutPanel;
    private System.Windows.Forms.OpenFileDialog ConfigFileDialog;
    private System.Windows.Forms.RadioButton ReplaceServerInstallationRadioButton;
    private System.Windows.Forms.Panel ReplaceInstallationOptionPanel;
    private System.Windows.Forms.Label ReplaceServerInstallationDescriptionLabel;
    private System.Windows.Forms.Panel ReplaceInstallationControlsPanel;
    private System.Windows.Forms.Button ExistingConfigFileBrowseButton;
    private System.Windows.Forms.Label ExistingConfigFilePathLabel;
    private System.Windows.Forms.TextBox ExistingConfigFilePathTextBox;
    private System.Windows.Forms.TextBox InstallDirectoryTextBox;
    private System.Windows.Forms.Label InstallDirectoryLabel;
    private System.Windows.Forms.Label ExistingDataDirectoryLabel;
    private System.Windows.Forms.TextBox VersionTextBox;
    private System.Windows.Forms.Label VersionLabel;
    private System.Windows.Forms.Label NameLabel;
    private System.Windows.Forms.TextBox PipeOrSharedMemoryNameTextBox;
    private System.Windows.Forms.Label PortLabel;
    private System.Windows.Forms.TextBox PortTextBox;
    private System.Windows.Forms.Label RootPasswordLabel;
    private System.Windows.Forms.Label ProtocolLabel;
    private System.Windows.Forms.ComboBox ProtocolComboBox;
    private System.Windows.Forms.TextBox ExistingDataDirectoryTextBox;
    private System.Windows.Forms.Label ReviewExistingServerInstallationDataLabel;
    private System.Windows.Forms.Button ConnectButton;
    private System.Windows.Forms.TextBox RootPasswordTextBox;
    private System.Windows.Forms.Label ExistingServerConnectionLabel;
    private System.Windows.Forms.RadioButton SideBySideInstallationRadioButton;
    private System.Windows.Forms.Panel SideBySideInstallationOptionPanel;
    private System.Windows.Forms.Label SideBySideInstallationDescriptionLabel;
    private System.Windows.Forms.Panel SideBySideInstallationControlsPanel;
    private System.Windows.Forms.Button NewDataDirectoryBrowseButton;
    private System.Windows.Forms.Label ConfirmDataDirectoryLabel;
    private System.Windows.Forms.TextBox NewDataDirectoryTextBox;
    private System.Windows.Forms.ErrorProvider DataDirectoryRenameWarningProvider;
    private System.Windows.Forms.ErrorProvider VersionErrorProvider;
    private System.Windows.Forms.ErrorProvider VersionWarningProvider;
    private System.Windows.Forms.ErrorProvider UserWarningProvider;
  }
}

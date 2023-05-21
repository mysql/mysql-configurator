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

namespace MySql.Configurator.Wizards.Common
{
  partial class PathsConflictPage
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
      this.ProductListView = new System.Windows.Forms.ListView();
      this.ProductColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.ArchitectureColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.PathConflictsHelpLabel = new System.Windows.Forms.Label();
      this.PathsOverrideHelpLabel = new System.Windows.Forms.Label();
      this.InstallDirResultsPanel = new System.Windows.Forms.Panel();
      this.InstallPathWarningLabel = new System.Windows.Forms.Label();
      this.InstallDirErrorPictureBox = new System.Windows.Forms.PictureBox();
      this.InstallDirWarningPictureBox = new System.Windows.Forms.PictureBox();
      this.InstallDirBrowseButton = new System.Windows.Forms.Button();
      this.InstallDirLabel = new System.Windows.Forms.Label();
      this.InstallDirTextBox = new System.Windows.Forms.TextBox();
      this.InstallDirPanel = new System.Windows.Forms.TableLayoutPanel();
      this.InstallDirResetButton = new System.Windows.Forms.Button();
      this.PathsOverrideTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
      this.DataDirPanel = new System.Windows.Forms.TableLayoutPanel();
      this.DataDirBrowseButton = new System.Windows.Forms.Button();
      this.DataDirLabel = new System.Windows.Forms.Label();
      this.DataDirTextBox = new System.Windows.Forms.TextBox();
      this.DataDirResultsPanel = new System.Windows.Forms.Panel();
      this.DataPathWarningLabel = new System.Windows.Forms.Label();
      this.DataDirErrorPictureBox = new System.Windows.Forms.PictureBox();
      this.DataDirWarningPictureBox = new System.Windows.Forms.PictureBox();
      this.DataDirResetButton = new System.Windows.Forms.Button();
      this.WarningsResultLabel = new System.Windows.Forms.Label();
      this.InstallDirResultsPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.InstallDirErrorPictureBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.InstallDirWarningPictureBox)).BeginInit();
      this.InstallDirPanel.SuspendLayout();
      this.PathsOverrideTableLayoutPanel.SuspendLayout();
      this.DataDirPanel.SuspendLayout();
      this.DataDirResultsPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.DataDirErrorPictureBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.DataDirWarningPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // subCaptionLabel
      // 
      this.subCaptionLabel.Location = new System.Drawing.Point(25, 66);
      this.subCaptionLabel.Size = new System.Drawing.Size(521, 18);
      this.subCaptionLabel.Text = "Some products has path conflicts";
      // 
      // captionLabel
      // 
      this.captionLabel.Size = new System.Drawing.Size(127, 25);
      this.captionLabel.Text = "Path Conflicts";
      // 
      // ProductListView
      // 
      this.ProductListView.AccessibleDescription = "A list of products with conflicting paths";
      this.ProductListView.AccessibleName = "Products List";
      this.ProductListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.ProductListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ProductColumnHeader,
            this.ArchitectureColumnHeader});
      this.ProductListView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ProductListView.FullRowSelect = true;
      this.ProductListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.ProductListView.HideSelection = false;
      this.ProductListView.Location = new System.Drawing.Point(27, 135);
      this.ProductListView.MultiSelect = false;
      this.ProductListView.Name = "ProductListView";
      this.ProductListView.ShowGroups = false;
      this.ProductListView.Size = new System.Drawing.Size(518, 140);
      this.ProductListView.TabIndex = 3;
      this.ProductListView.UseCompatibleStateImageBehavior = false;
      this.ProductListView.View = System.Windows.Forms.View.Details;
      this.ProductListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ProductListView_ItemSelectionChanged);
      // 
      // ProductColumnHeader
      // 
      this.ProductColumnHeader.Text = "Product";
      this.ProductColumnHeader.Width = 390;
      // 
      // ArchitectureColumnHeader
      // 
      this.ArchitectureColumnHeader.Text = "Architecture";
      this.ArchitectureColumnHeader.Width = 100;
      // 
      // PathConflictsHelpLabel
      // 
      this.PathConflictsHelpLabel.AccessibleDescription = "A label displaying instructions about conflicting paths";
      this.PathConflictsHelpLabel.AccessibleName = "Paths Conflict Text";
      this.PathConflictsHelpLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.PathConflictsHelpLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.PathConflictsHelpLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.PathConflictsHelpLabel.Location = new System.Drawing.Point(27, 100);
      this.PathConflictsHelpLabel.Name = "PathConflictsHelpLabel";
      this.PathConflictsHelpLabel.Size = new System.Drawing.Size(518, 36);
      this.PathConflictsHelpLabel.TabIndex = 2;
      this.PathConflictsHelpLabel.Text = "Here are the list of the products that has path conflicts, please navigate betwee" +
    "n them and if is necesary change the path or paths below.";
      // 
      // PathsOverrideHelpLabel
      // 
      this.PathsOverrideHelpLabel.AccessibleDescription = "A label displaying instructions about overriding data and install directory paths" +
    "";
      this.PathsOverrideHelpLabel.AccessibleName = "Path Overrides Text";
      this.PathsOverrideHelpLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.PathsOverrideHelpLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.PathsOverrideHelpLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.PathsOverrideHelpLabel.Location = new System.Drawing.Point(27, 303);
      this.PathsOverrideHelpLabel.Name = "PathsOverrideHelpLabel";
      this.PathsOverrideHelpLabel.Size = new System.Drawing.Size(514, 38);
      this.PathsOverrideHelpLabel.TabIndex = 5;
      this.PathsOverrideHelpLabel.Text = "You can use the same folder or change it to a new one, take in mind that the inst" +
    "all process can overwrite the folder if already exists.";
      // 
      // InstallDirResultsPanel
      // 
      this.InstallDirResultsPanel.AccessibleDescription = "A panel containing controls to show the result of the validation for the install " +
    "directory";
      this.InstallDirResultsPanel.AccessibleName = "Install Directory Validation Group";
      this.InstallDirResultsPanel.Controls.Add(this.InstallPathWarningLabel);
      this.InstallDirResultsPanel.Controls.Add(this.InstallDirErrorPictureBox);
      this.InstallDirResultsPanel.Controls.Add(this.InstallDirWarningPictureBox);
      this.InstallDirResultsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.InstallDirResultsPanel.Location = new System.Drawing.Point(3, 49);
      this.InstallDirResultsPanel.Name = "InstallDirResultsPanel";
      this.InstallDirResultsPanel.Size = new System.Drawing.Size(473, 23);
      this.InstallDirResultsPanel.TabIndex = 4;
      // 
      // InstallPathWarningLabel
      // 
      this.InstallPathWarningLabel.AccessibleDescription = "A label displaying a message about a warning or error in the install directory va" +
    "lidation";
      this.InstallPathWarningLabel.AccessibleName = "Install Directory Validation Text";
      this.InstallPathWarningLabel.Location = new System.Drawing.Point(19, 1);
      this.InstallPathWarningLabel.Name = "InstallPathWarningLabel";
      this.InstallPathWarningLabel.Size = new System.Drawing.Size(442, 18);
      this.InstallPathWarningLabel.TabIndex = 0;
      this.InstallPathWarningLabel.Visible = false;
      // 
      // InstallDirErrorPictureBox
      // 
      this.InstallDirErrorPictureBox.AccessibleDescription = "A picture box displaying an error icon for an invalid install directory";
      this.InstallDirErrorPictureBox.AccessibleName = "Install Directory Error Icon";
      this.InstallDirErrorPictureBox.Image = global::MySql.Configurator.Properties.Resources.error_sign;
      this.InstallDirErrorPictureBox.Location = new System.Drawing.Point(0, 0);
      this.InstallDirErrorPictureBox.Name = "InstallDirErrorPictureBox";
      this.InstallDirErrorPictureBox.Size = new System.Drawing.Size(16, 16);
      this.InstallDirErrorPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.InstallDirErrorPictureBox.TabIndex = 57;
      this.InstallDirErrorPictureBox.TabStop = false;
      this.InstallDirErrorPictureBox.Visible = false;
      // 
      // InstallDirWarningPictureBox
      // 
      this.InstallDirWarningPictureBox.AccessibleDescription = "A picture box displaying a warning icon for the install directory";
      this.InstallDirWarningPictureBox.AccessibleName = "Install Directory Warning Icon";
      this.InstallDirWarningPictureBox.Image = global::MySql.Configurator.Properties.Resources.warning_sign;
      this.InstallDirWarningPictureBox.Location = new System.Drawing.Point(0, 0);
      this.InstallDirWarningPictureBox.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
      this.InstallDirWarningPictureBox.Name = "InstallDirWarningPictureBox";
      this.InstallDirWarningPictureBox.Size = new System.Drawing.Size(16, 16);
      this.InstallDirWarningPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.InstallDirWarningPictureBox.TabIndex = 55;
      this.InstallDirWarningPictureBox.TabStop = false;
      this.InstallDirWarningPictureBox.Visible = false;
      // 
      // InstallDirBrowseButton
      // 
      this.InstallDirBrowseButton.AccessibleDescription = "A button to open a dialog box to browse through the file system and select a dire" +
    "ctory";
      this.InstallDirBrowseButton.AccessibleName = "Install Directory Browse";
      this.InstallDirBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.InstallDirBrowseButton.Location = new System.Drawing.Point(482, 20);
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
      this.InstallDirLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.InstallDirLabel.Location = new System.Drawing.Point(3, 0);
      this.InstallDirLabel.Name = "InstallDirLabel";
      this.InstallDirLabel.Size = new System.Drawing.Size(99, 15);
      this.InstallDirLabel.TabIndex = 0;
      this.InstallDirLabel.Text = "Install Directory:";
      // 
      // InstallDirTextBox
      // 
      this.InstallDirTextBox.AccessibleDescription = "A text box to set the full absolute path of the install directory";
      this.InstallDirTextBox.AccessibleName = "Install Directory";
      this.InstallDirTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.InstallDirTextBox.Location = new System.Drawing.Point(3, 20);
      this.InstallDirTextBox.Name = "InstallDirTextBox";
      this.InstallDirTextBox.Size = new System.Drawing.Size(473, 23);
      this.InstallDirTextBox.TabIndex = 2;
      this.InstallDirTextBox.Tag = "InstallDir";
      this.InstallDirTextBox.Leave += new System.EventHandler(this.Textbox_Leave);
      // 
      // InstallDirPanel
      // 
      this.InstallDirPanel.AccessibleDescription = "A panel containing options to set a value for the install directory";
      this.InstallDirPanel.AccessibleName = "Install Directory Group";
      this.InstallDirPanel.ColumnCount = 2;
      this.InstallDirPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.InstallDirPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.InstallDirPanel.Controls.Add(this.InstallDirBrowseButton, 1, 1);
      this.InstallDirPanel.Controls.Add(this.InstallDirLabel, 0, 0);
      this.InstallDirPanel.Controls.Add(this.InstallDirTextBox, 0, 1);
      this.InstallDirPanel.Controls.Add(this.InstallDirResultsPanel, 0, 3);
      this.InstallDirPanel.Controls.Add(this.InstallDirResetButton, 1, 0);
      this.InstallDirPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.InstallDirPanel.Location = new System.Drawing.Point(3, 3);
      this.InstallDirPanel.Name = "InstallDirPanel";
      this.InstallDirPanel.RowCount = 3;
      this.InstallDirPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.InstallDirPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.InstallDirPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.InstallDirPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
      this.InstallDirPanel.Size = new System.Drawing.Size(514, 75);
      this.InstallDirPanel.TabIndex = 0;
      // 
      // InstallDirResetButton
      // 
      this.InstallDirResetButton.AccessibleDescription = "A button to revert the value of the install directory";
      this.InstallDirResetButton.AccessibleName = "Install Directory Revert";
      this.InstallDirResetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.InstallDirResetButton.BackgroundImage = global::MySql.Configurator.Properties.Resources.Revert;
      this.InstallDirResetButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
      this.InstallDirResetButton.FlatAppearance.BorderSize = 0;
      this.InstallDirResetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.InstallDirResetButton.Location = new System.Drawing.Point(479, 0);
      this.InstallDirResetButton.Margin = new System.Windows.Forms.Padding(0);
      this.InstallDirResetButton.Name = "InstallDirResetButton";
      this.InstallDirResetButton.Size = new System.Drawing.Size(35, 17);
      this.InstallDirResetButton.TabIndex = 1;
      this.InstallDirResetButton.UseVisualStyleBackColor = true;
      this.InstallDirResetButton.Click += new System.EventHandler(this.InstallDirResetButton_Click);
      // 
      // PathsOverrideTableLayoutPanel
      // 
      this.PathsOverrideTableLayoutPanel.AccessibleDescription = "A panel containing controls to override the data and install directory paths";
      this.PathsOverrideTableLayoutPanel.AccessibleName = "Path Overrides Group";
      this.PathsOverrideTableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.PathsOverrideTableLayoutPanel.ColumnCount = 1;
      this.PathsOverrideTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.PathsOverrideTableLayoutPanel.Controls.Add(this.DataDirPanel, 0, 1);
      this.PathsOverrideTableLayoutPanel.Controls.Add(this.InstallDirPanel, 0, 0);
      this.PathsOverrideTableLayoutPanel.Location = new System.Drawing.Point(25, 339);
      this.PathsOverrideTableLayoutPanel.Name = "PathsOverrideTableLayoutPanel";
      this.PathsOverrideTableLayoutPanel.RowCount = 2;
      this.PathsOverrideTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.PathsOverrideTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.PathsOverrideTableLayoutPanel.Size = new System.Drawing.Size(520, 162);
      this.PathsOverrideTableLayoutPanel.TabIndex = 6;
      // 
      // DataDirPanel
      // 
      this.DataDirPanel.AccessibleDescription = "A panel containing options to set a value for the data directory";
      this.DataDirPanel.AccessibleName = "Data Directory Group";
      this.DataDirPanel.ColumnCount = 2;
      this.DataDirPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.DataDirPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.DataDirPanel.Controls.Add(this.DataDirBrowseButton, 1, 1);
      this.DataDirPanel.Controls.Add(this.DataDirLabel, 0, 0);
      this.DataDirPanel.Controls.Add(this.DataDirTextBox, 0, 1);
      this.DataDirPanel.Controls.Add(this.DataDirResultsPanel, 0, 2);
      this.DataDirPanel.Controls.Add(this.DataDirResetButton, 1, 0);
      this.DataDirPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.DataDirPanel.Location = new System.Drawing.Point(3, 84);
      this.DataDirPanel.Name = "DataDirPanel";
      this.DataDirPanel.RowCount = 3;
      this.DataDirPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.DataDirPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.DataDirPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.DataDirPanel.Size = new System.Drawing.Size(514, 75);
      this.DataDirPanel.TabIndex = 1;
      // 
      // DataDirBrowseButton
      // 
      this.DataDirBrowseButton.AccessibleDescription = "A button to open a dialog box to browse through the file system and select a dire" +
    "ctory";
      this.DataDirBrowseButton.AccessibleName = "Data Directory Browse";
      this.DataDirBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.DataDirBrowseButton.Location = new System.Drawing.Point(482, 20);
      this.DataDirBrowseButton.Name = "DataDirBrowseButton";
      this.DataDirBrowseButton.Size = new System.Drawing.Size(29, 22);
      this.DataDirBrowseButton.TabIndex = 3;
      this.DataDirBrowseButton.Text = "...";
      this.DataDirBrowseButton.UseVisualStyleBackColor = true;
      this.DataDirBrowseButton.Click += new System.EventHandler(this.DataDirBrowseButton_Click);
      // 
      // DataDirLabel
      // 
      this.DataDirLabel.AccessibleDescription = "A label displaying the text data directory";
      this.DataDirLabel.AccessibleName = "Data Directory Text";
      this.DataDirLabel.AutoSize = true;
      this.DataDirLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.DataDirLabel.Location = new System.Drawing.Point(3, 0);
      this.DataDirLabel.Name = "DataDirLabel";
      this.DataDirLabel.Size = new System.Drawing.Size(92, 15);
      this.DataDirLabel.TabIndex = 0;
      this.DataDirLabel.Text = "Data Directory:";
      // 
      // DataDirTextBox
      // 
      this.DataDirTextBox.AccessibleDescription = "A text box to set the full absolute path of the data directory";
      this.DataDirTextBox.AccessibleName = "Data Directory";
      this.DataDirTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.DataDirTextBox.Location = new System.Drawing.Point(3, 20);
      this.DataDirTextBox.Name = "DataDirTextBox";
      this.DataDirTextBox.Size = new System.Drawing.Size(473, 23);
      this.DataDirTextBox.TabIndex = 2;
      this.DataDirTextBox.Tag = "DataDir";
      this.DataDirTextBox.Leave += new System.EventHandler(this.Textbox_Leave);
      // 
      // DataDirResultsPanel
      // 
      this.DataDirResultsPanel.AccessibleDescription = "A panel containing controls to show the result of the validation for the data dir" +
    "ectory";
      this.DataDirResultsPanel.AccessibleName = "Data Directory Validation Group";
      this.DataDirResultsPanel.Controls.Add(this.DataPathWarningLabel);
      this.DataDirResultsPanel.Controls.Add(this.DataDirErrorPictureBox);
      this.DataDirResultsPanel.Controls.Add(this.DataDirWarningPictureBox);
      this.DataDirResultsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.DataDirResultsPanel.Location = new System.Drawing.Point(3, 49);
      this.DataDirResultsPanel.Name = "DataDirResultsPanel";
      this.DataDirResultsPanel.Size = new System.Drawing.Size(473, 24);
      this.DataDirResultsPanel.TabIndex = 4;
      // 
      // DataPathWarningLabel
      // 
      this.DataPathWarningLabel.AccessibleDescription = "A label displaying a message about a warning or error in the data directory valid" +
    "ation";
      this.DataPathWarningLabel.AccessibleName = "Data Directory Validation Text";
      this.DataPathWarningLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.DataPathWarningLabel.Location = new System.Drawing.Point(16, 1);
      this.DataPathWarningLabel.Name = "DataPathWarningLabel";
      this.DataPathWarningLabel.Size = new System.Drawing.Size(442, 20);
      this.DataPathWarningLabel.TabIndex = 0;
      this.DataPathWarningLabel.Visible = false;
      // 
      // DataDirErrorPictureBox
      // 
      this.DataDirErrorPictureBox.AccessibleDescription = "A picture box displaying an error icon for an invalid data directory";
      this.DataDirErrorPictureBox.AccessibleName = "Data Directory Error Icon";
      this.DataDirErrorPictureBox.Image = global::MySql.Configurator.Properties.Resources.error_sign;
      this.DataDirErrorPictureBox.Location = new System.Drawing.Point(0, 0);
      this.DataDirErrorPictureBox.Name = "DataDirErrorPictureBox";
      this.DataDirErrorPictureBox.Size = new System.Drawing.Size(16, 16);
      this.DataDirErrorPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.DataDirErrorPictureBox.TabIndex = 57;
      this.DataDirErrorPictureBox.TabStop = false;
      this.DataDirErrorPictureBox.Visible = false;
      // 
      // DataDirWarningPictureBox
      // 
      this.DataDirWarningPictureBox.AccessibleDescription = "A picture box displaying a warning icon for the data directory";
      this.DataDirWarningPictureBox.AccessibleName = "Data Directory Warning Icon";
      this.DataDirWarningPictureBox.Image = global::MySql.Configurator.Properties.Resources.warning_sign;
      this.DataDirWarningPictureBox.Location = new System.Drawing.Point(0, 0);
      this.DataDirWarningPictureBox.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
      this.DataDirWarningPictureBox.Name = "DataDirWarningPictureBox";
      this.DataDirWarningPictureBox.Size = new System.Drawing.Size(16, 16);
      this.DataDirWarningPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.DataDirWarningPictureBox.TabIndex = 55;
      this.DataDirWarningPictureBox.TabStop = false;
      this.DataDirWarningPictureBox.Visible = false;
      // 
      // DataDirResetButton
      // 
      this.DataDirResetButton.AccessibleDescription = "A button to revert the value of the data directory";
      this.DataDirResetButton.AccessibleName = "Data Directory Revert";
      this.DataDirResetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.DataDirResetButton.BackgroundImage = global::MySql.Configurator.Properties.Resources.Revert;
      this.DataDirResetButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
      this.DataDirResetButton.FlatAppearance.BorderSize = 0;
      this.DataDirResetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.DataDirResetButton.Location = new System.Drawing.Point(479, 0);
      this.DataDirResetButton.Margin = new System.Windows.Forms.Padding(0);
      this.DataDirResetButton.Name = "DataDirResetButton";
      this.DataDirResetButton.Size = new System.Drawing.Size(35, 17);
      this.DataDirResetButton.TabIndex = 1;
      this.DataDirResetButton.UseVisualStyleBackColor = true;
      this.DataDirResetButton.Click += new System.EventHandler(this.DataDirResetButton_Click);
      // 
      // WarningsResultLabel
      // 
      this.WarningsResultLabel.AccessibleDescription = "A label displaying the warnings count for conflicting paths";
      this.WarningsResultLabel.AccessibleName = "Warnings Count";
      this.WarningsResultLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.WarningsResultLabel.Location = new System.Drawing.Point(345, 278);
      this.WarningsResultLabel.Name = "WarningsResultLabel";
      this.WarningsResultLabel.Size = new System.Drawing.Size(194, 17);
      this.WarningsResultLabel.TabIndex = 4;
      this.WarningsResultLabel.Text = "0 Warnings";
      this.WarningsResultLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // PathsConflictPage
      // 
      this.AccessibleDescription = "A wizard page containing options to resolve path conflicts with the data and inst" +
    "all directories";
      this.AccessibleName = "Path Conflicts Page";
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
      this.Caption = "Path Conflicts";
      this.Controls.Add(this.WarningsResultLabel);
      this.Controls.Add(this.PathsOverrideTableLayoutPanel);
      this.Controls.Add(this.PathsOverrideHelpLabel);
      this.Controls.Add(this.ProductListView);
      this.Controls.Add(this.PathConflictsHelpLabel);
      this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "PathsConflictPage";
      this.SubCaption = "Some products has path conflicts";
      this.Controls.SetChildIndex(this.PathConflictsHelpLabel, 0);
      this.Controls.SetChildIndex(this.ProductListView, 0);
      this.Controls.SetChildIndex(this.PathsOverrideHelpLabel, 0);
      this.Controls.SetChildIndex(this.captionLabel, 0);
      this.Controls.SetChildIndex(this.subCaptionLabel, 0);
      this.Controls.SetChildIndex(this.PathsOverrideTableLayoutPanel, 0);
      this.Controls.SetChildIndex(this.WarningsResultLabel, 0);
      this.InstallDirResultsPanel.ResumeLayout(false);
      this.InstallDirResultsPanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.InstallDirErrorPictureBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.InstallDirWarningPictureBox)).EndInit();
      this.InstallDirPanel.ResumeLayout(false);
      this.InstallDirPanel.PerformLayout();
      this.PathsOverrideTableLayoutPanel.ResumeLayout(false);
      this.DataDirPanel.ResumeLayout(false);
      this.DataDirPanel.PerformLayout();
      this.DataDirResultsPanel.ResumeLayout(false);
      this.DataDirResultsPanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.DataDirErrorPictureBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.DataDirWarningPictureBox)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ListView ProductListView;
    private System.Windows.Forms.ColumnHeader ProductColumnHeader;
    private System.Windows.Forms.ColumnHeader ArchitectureColumnHeader;
    private System.Windows.Forms.Label PathConflictsHelpLabel;
    private System.Windows.Forms.Label PathsOverrideHelpLabel;
    private System.Windows.Forms.Panel InstallDirResultsPanel;
    private System.Windows.Forms.PictureBox InstallDirErrorPictureBox;
    private System.Windows.Forms.PictureBox InstallDirWarningPictureBox;
    private System.Windows.Forms.Button InstallDirBrowseButton;
    private System.Windows.Forms.Label InstallDirLabel;
    private System.Windows.Forms.TextBox InstallDirTextBox;
    private System.Windows.Forms.TableLayoutPanel InstallDirPanel;
    private System.Windows.Forms.TableLayoutPanel PathsOverrideTableLayoutPanel;
    private System.Windows.Forms.TableLayoutPanel DataDirPanel;
    private System.Windows.Forms.Button DataDirBrowseButton;
    private System.Windows.Forms.Label DataDirLabel;
    private System.Windows.Forms.TextBox DataDirTextBox;
    private System.Windows.Forms.Panel DataDirResultsPanel;
    private System.Windows.Forms.PictureBox DataDirErrorPictureBox;
    private System.Windows.Forms.PictureBox DataDirWarningPictureBox;
    private System.Windows.Forms.Label InstallPathWarningLabel;
    private System.Windows.Forms.Label DataPathWarningLabel;
    private System.Windows.Forms.Button InstallDirResetButton;
    private System.Windows.Forms.Button DataDirResetButton;
    private System.Windows.Forms.Label WarningsResultLabel;
  }
}

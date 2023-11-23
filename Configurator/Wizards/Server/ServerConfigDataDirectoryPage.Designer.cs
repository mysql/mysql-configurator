namespace MySql.Configurator.Wizards.Server
{
  partial class ServerConfigDataDirectoryPage
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerConfigDataDirectoryPage));
      this.DataDirectoryDescriptionLabel = new System.Windows.Forms.Label();
      this.DataDirectoryBrowseButton = new System.Windows.Forms.Button();
      this.DataDirectoryTextBox = new System.Windows.Forms.TextBox();
      this.DataDirectoryRevertButton = new System.Windows.Forms.Button();
      this.DataDirectoryBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).BeginInit();
      this.SuspendLayout();
      // 
      // captionLabel
      // 
      this.captionLabel.AccessibleDescription = "A title text for the configuration page about the data directory.";
      this.captionLabel.AccessibleName = "Data Directory Title";
      this.captionLabel.Size = new System.Drawing.Size(133, 25);
      this.captionLabel.Text = "Data Directory";
      // 
      // DataDirectoryDescriptionLabel
      // 
      this.DataDirectoryDescriptionLabel.AccessibleDescription = "A label displaying a text explaining that existing MySQL Server installations wer" +
    "e found on the system.";
      this.DataDirectoryDescriptionLabel.AccessibleName = "MySQL Server installations description text";
      this.DataDirectoryDescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DataDirectoryDescriptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.DataDirectoryDescriptionLabel.Location = new System.Drawing.Point(27, 77);
      this.DataDirectoryDescriptionLabel.Name = "DataDirectoryDescriptionLabel";
      this.DataDirectoryDescriptionLabel.Size = new System.Drawing.Size(512, 64);
      this.DataDirectoryDescriptionLabel.TabIndex = 1;
      this.DataDirectoryDescriptionLabel.Text = resources.GetString("DataDirectoryDescriptionLabel.Text");
      // 
      // DataDirectoryBrowseButton
      // 
      this.DataDirectoryBrowseButton.AccessibleDescription = "A button to open a dialog to browse the file system and select a data directory.";
      this.DataDirectoryBrowseButton.AccessibleName = "Data directory browse button";
      this.DataDirectoryBrowseButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DataDirectoryBrowseButton.Location = new System.Drawing.Point(482, 142);
      this.DataDirectoryBrowseButton.Name = "DataDirectoryBrowseButton";
      this.DataDirectoryBrowseButton.Size = new System.Drawing.Size(37, 23);
      this.DataDirectoryBrowseButton.TabIndex = 4;
      this.DataDirectoryBrowseButton.Text = "...";
      this.DataDirectoryBrowseButton.UseVisualStyleBackColor = true;
      this.DataDirectoryBrowseButton.Click += new System.EventHandler(this.DataDirectoryBrowseButton_Click);
      // 
      // DataDirectoryTextBox
      // 
      this.DataDirectoryTextBox.AccessibleDescription = "The data directory path to use with this MySQL Server installation being configur" +
    "ed.";
      this.DataDirectoryTextBox.AccessibleName = "Data directory path";
      this.DataDirectoryTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DataDirectoryTextBox.Location = new System.Drawing.Point(29, 144);
      this.DataDirectoryTextBox.Name = "DataDirectoryTextBox";
      this.DataDirectoryTextBox.Size = new System.Drawing.Size(424, 23);
      this.DataDirectoryTextBox.TabIndex = 2;
      this.ToolTip.SetToolTip(this.DataDirectoryTextBox, "The full path to the data directory that will be used by the MySQL Server being c" +
        "onfigured.");
      this.DataDirectoryTextBox.TextChanged += new System.EventHandler(this.TextChangedHandler);
      this.DataDirectoryTextBox.Validated += new System.EventHandler(this.ValidatedHandler);
      // 
      // DataDirectoryRevertButton
      // 
      this.DataDirectoryRevertButton.AccessibleDescription = "A button to revert the value of the data directory path to its default value.";
      this.DataDirectoryRevertButton.AccessibleName = "Data Directory Path Revert";
      this.DataDirectoryRevertButton.BackColor = System.Drawing.Color.Transparent;
      this.DataDirectoryRevertButton.BackgroundImage = global::MySql.Configurator.Properties.Resources.Revert;
      this.DataDirectoryRevertButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
      this.DataDirectoryRevertButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.DataDirectoryRevertButton.ForeColor = System.Drawing.Color.Transparent;
      this.DataDirectoryRevertButton.Location = new System.Drawing.Point(460, 143);
      this.DataDirectoryRevertButton.Name = "DataDirectoryRevertButton";
      this.DataDirectoryRevertButton.Size = new System.Drawing.Size(16, 22);
      this.DataDirectoryRevertButton.TabIndex = 3;
      this.ToolTip.SetToolTip(this.DataDirectoryRevertButton, "Revert the data directory path to its suggested default value.");
      this.DataDirectoryRevertButton.UseVisualStyleBackColor = false;
      this.DataDirectoryRevertButton.Click += new System.EventHandler(this.DataDirectoryRevertButton_Click);
      // 
      // DataDirectoryBrowserDialog
      // 
      this.DataDirectoryBrowserDialog.Description = "Select the MySQL Server data directory";
      this.DataDirectoryBrowserDialog.RootFolder = System.Environment.SpecialFolder.CommonApplicationData;
      // 
      // ServerConfigDataDirectoryPage
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Caption = "Data Directory";
      this.Controls.Add(this.DataDirectoryRevertButton);
      this.Controls.Add(this.DataDirectoryBrowseButton);
      this.Controls.Add(this.DataDirectoryTextBox);
      this.Controls.Add(this.DataDirectoryDescriptionLabel);
      this.Name = "ServerConfigDataDirectoryPage";
      this.Controls.SetChildIndex(this.captionLabel, 0);
      this.Controls.SetChildIndex(this.subCaptionLabel, 0);
      this.Controls.SetChildIndex(this.DataDirectoryDescriptionLabel, 0);
      this.Controls.SetChildIndex(this.DataDirectoryTextBox, 0);
      this.Controls.SetChildIndex(this.DataDirectoryBrowseButton, 0);
      this.Controls.SetChildIndex(this.DataDirectoryRevertButton, 0);
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label DataDirectoryDescriptionLabel;
    private System.Windows.Forms.Button DataDirectoryBrowseButton;
    private System.Windows.Forms.TextBox DataDirectoryTextBox;
    private System.Windows.Forms.Button DataDirectoryRevertButton;
    private System.Windows.Forms.FolderBrowserDialog DataDirectoryBrowserDialog;
  }
}

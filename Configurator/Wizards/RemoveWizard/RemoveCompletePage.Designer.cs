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

using MySql.Configurator.Core.Controls;

namespace MySql.Configurator.Wizards.RemoveWizard
{
  partial class RemoveCompletePage
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
      this.RebootComputerPanel = new System.Windows.Forms.Panel();
      this.RebootComputerLabel = new System.Windows.Forms.Label();
      this.RebootComputerCheckBox = new System.Windows.Forms.CheckBox();
      this.CopyLogToClipboardButton = new System.Windows.Forms.Button();
      this.RemoveCompleteFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
      this.RemovedPackagesListView = new MySql.Configurator.Core.Controls.MyListView();
      this.ProductIconColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.ProductNameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.ProductVersionColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).BeginInit();
      this.RebootComputerPanel.SuspendLayout();
      this.RemoveCompleteFlowLayoutPanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // subCaptionLabel
      // 
      this.subCaptionLabel.Font = new System.Drawing.Font("Segoe UI", 6F);
      this.subCaptionLabel.Location = new System.Drawing.Point(26, 75);
      this.subCaptionLabel.Size = new System.Drawing.Size(522, 21);
      this.subCaptionLabel.Text = "You successfully removed the following MySQL products.";
      // 
      // captionLabel
      // 
      this.captionLabel.Font = new System.Drawing.Font("Segoe UI", 9.5F);
      this.captionLabel.Location = new System.Drawing.Point(23, 30);
      this.captionLabel.Size = new System.Drawing.Size(169, 25);
      this.captionLabel.Text = "Removal Complete";
      // 
      // RebootComputerPanel
      // 
      this.RebootComputerPanel.AccessibleDescription = "A panel containing controls about rebooting the computer when the configuration c" +
    "hanges require a reboot";
      this.RebootComputerPanel.AccessibleName = "Reboot Computer Group";
      this.RebootComputerPanel.Controls.Add(this.RebootComputerLabel);
      this.RebootComputerPanel.Controls.Add(this.RebootComputerCheckBox);
      this.RebootComputerPanel.Location = new System.Drawing.Point(3, 251);
      this.RebootComputerPanel.Name = "RebootComputerPanel";
      this.RebootComputerPanel.Size = new System.Drawing.Size(505, 73);
      this.RebootComputerPanel.TabIndex = 3;
      this.RebootComputerPanel.Visible = false;
      // 
      // RebootComputerLabel
      // 
      this.RebootComputerLabel.AccessibleDescription = "A label displaying a message about rebooting the computer when the configuration " +
    "changes require a reboot";
      this.RebootComputerLabel.AccessibleName = "Reboot Computer Text";
      this.RebootComputerLabel.Location = new System.Drawing.Point(0, 9);
      this.RebootComputerLabel.Name = "RebootComputerLabel";
      this.RebootComputerLabel.Size = new System.Drawing.Size(471, 33);
      this.RebootComputerLabel.TabIndex = 0;
      this.RebootComputerLabel.Text = "One or more of the products removed require a reboot to complete.\r\nDo you want to" +
    " reboot your computer when done?";
      // 
      // RebootComputerCheckBox
      // 
      this.RebootComputerCheckBox.AccessibleDescription = "A check box to reboot the computer when the closing the wizard";
      this.RebootComputerCheckBox.AccessibleName = "Reboot When Done";
      this.RebootComputerCheckBox.AutoSize = true;
      this.RebootComputerCheckBox.Location = new System.Drawing.Point(4, 48);
      this.RebootComputerCheckBox.Name = "RebootComputerCheckBox";
      this.RebootComputerCheckBox.Size = new System.Drawing.Size(219, 29);
      this.RebootComputerCheckBox.TabIndex = 1;
      this.RebootComputerCheckBox.Text = "Yes, reboot when done";
      this.RebootComputerCheckBox.UseVisualStyleBackColor = true;
      this.RebootComputerCheckBox.CheckedChanged += new System.EventHandler(this.RebootComputerCheckBox_CheckedChanged);
      // 
      // CopyLogToClipboardButton
      // 
      this.CopyLogToClipboardButton.AccessibleDescription = "A button to copy the contents of the removal log to the clipboard";
      this.CopyLogToClipboardButton.AccessibleName = "Copy Log to Clipboard";
      this.CopyLogToClipboardButton.AutoSize = true;
      this.CopyLogToClipboardButton.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.CopyLogToClipboardButton.Location = new System.Drawing.Point(3, 210);
      this.CopyLogToClipboardButton.Name = "CopyLogToClipboardButton";
      this.CopyLogToClipboardButton.Size = new System.Drawing.Size(204, 35);
      this.CopyLogToClipboardButton.TabIndex = 1;
      this.CopyLogToClipboardButton.Text = "C&opy Log to Clipboard";
      this.CopyLogToClipboardButton.UseVisualStyleBackColor = true;
      this.CopyLogToClipboardButton.Click += new System.EventHandler(this.CopyLogToClipboardButton_Click);
      // 
      // RemoveCompleteFlowLayoutPanel
      // 
      this.RemoveCompleteFlowLayoutPanel.AccessibleDescription = "A panel giving a vertical flow to all controls related to removing products";
      this.RemoveCompleteFlowLayoutPanel.AccessibleName = "Remove Complete Group";
      this.RemoveCompleteFlowLayoutPanel.Controls.Add(this.RemovedPackagesListView);
      this.RemoveCompleteFlowLayoutPanel.Controls.Add(this.CopyLogToClipboardButton);
      this.RemoveCompleteFlowLayoutPanel.Controls.Add(this.RebootComputerPanel);
      this.RemoveCompleteFlowLayoutPanel.Location = new System.Drawing.Point(25, 99);
      this.RemoveCompleteFlowLayoutPanel.Name = "RemoveCompleteFlowLayoutPanel";
      this.RemoveCompleteFlowLayoutPanel.Size = new System.Drawing.Size(519, 399);
      this.RemoveCompleteFlowLayoutPanel.TabIndex = 2;
      // 
      // RemovedPackagesListView
      // 
      this.RemovedPackagesListView.AccessibleDescription = "A list view containing selected products that have been removed";
      this.RemovedPackagesListView.AccessibleName = "Removed Packages List";
      this.RemovedPackagesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ProductIconColumnHeader,
            this.ProductNameColumnHeader,
            this.ProductVersionColumnHeader});
      this.RemovedPackagesListView.HideSelection = false;
      this.RemovedPackagesListView.Location = new System.Drawing.Point(3, 3);
      this.RemovedPackagesListView.Name = "RemovedPackagesListView";
      this.RemovedPackagesListView.OwnerDraw = true;
      this.RemovedPackagesListView.ProgressBarCol = 0;
      this.RemovedPackagesListView.ProgressBarRow = 0;
      this.RemovedPackagesListView.ShowItemToolTips = true;
      this.RemovedPackagesListView.Size = new System.Drawing.Size(505, 201);
      this.RemovedPackagesListView.TabIndex = 0;
      this.RemovedPackagesListView.UseCompatibleStateImageBehavior = false;
      this.RemovedPackagesListView.View = System.Windows.Forms.View.Details;
      // 
      // ProductIconColumnHeader
      // 
      this.ProductIconColumnHeader.Text = "";
      this.ProductIconColumnHeader.Width = 20;
      // 
      // ProductNameColumnHeader
      // 
      this.ProductNameColumnHeader.Text = "Product";
      this.ProductNameColumnHeader.Width = 254;
      // 
      // ProductVersionColumnHeader
      // 
      this.ProductVersionColumnHeader.Text = "Version";
      // 
      // RemoveCompletePage
      // 
      this.AccessibleDescription = "A wizard page containing a summary of selected products that have been removed";
      this.AccessibleName = "Remove Complete Page";
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
      this.Caption = "Removal Complete";
      this.Controls.Add(this.RemoveCompleteFlowLayoutPanel);
      this.DoubleBuffered = true;
      this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "RemoveCompletePage";
      this.SubCaption = "You successfully removed the following MySQL products.";
      this.Controls.SetChildIndex(this.captionLabel, 0);
      this.Controls.SetChildIndex(this.subCaptionLabel, 0);
      this.Controls.SetChildIndex(this.RemoveCompleteFlowLayoutPanel, 0);
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).EndInit();
      this.RebootComputerPanel.ResumeLayout(false);
      this.RebootComputerPanel.PerformLayout();
      this.RemoveCompleteFlowLayoutPanel.ResumeLayout(false);
      this.RemoveCompleteFlowLayoutPanel.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion
        private MyListView RemovedPackagesListView;
        private System.Windows.Forms.ColumnHeader ProductIconColumnHeader;
        private System.Windows.Forms.ColumnHeader ProductNameColumnHeader;
        private System.Windows.Forms.ColumnHeader ProductVersionColumnHeader;
        private System.Windows.Forms.Panel RebootComputerPanel;
        private System.Windows.Forms.Label RebootComputerLabel;
        private System.Windows.Forms.CheckBox RebootComputerCheckBox;
        private System.Windows.Forms.Button CopyLogToClipboardButton;
        private System.Windows.Forms.FlowLayoutPanel RemoveCompleteFlowLayoutPanel;
    }
}

/* Copyright (c) 2011, 2018, Oracle and/or its affiliates.

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

namespace MySql.Configurator.Wizards.Common
{
  partial class ConfigOverviewPage
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigOverviewPage));
      this.ConfigurationStateImageList = new System.Windows.Forms.ImageList(this.components);
      this.LastErrorDescriptionLabel = new System.Windows.Forms.Label();
      this.ProductsListView = new MyListView();
      this.StatusImageColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.ProductNameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.StatusDescriptionColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.SuspendLayout();
      // 
      // subCaptionLabel
      // 
      this.subCaptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.subCaptionLabel.Size = new System.Drawing.Size(503, 70);
      this.subCaptionLabel.Text = "We\'ll now walk through a configuration wizard for each of the following products." +
    "\r\n\r\nYou can cancel at any point if you wish to leave this wizard without configu" +
    "ring all the products.";
      // 
      // captionLabel
      // 
      this.captionLabel.Size = new System.Drawing.Size(200, 25);
      this.captionLabel.Text = "Product Configuration";
      // 
      // ConfigurationStateImageList
      // 
      this.ConfigurationStateImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ConfigurationStateImageList.ImageStream")));
      this.ConfigurationStateImageList.TransparentColor = System.Drawing.Color.Transparent;
      this.ConfigurationStateImageList.Images.SetKeyName(0, "MySQLInstallerConfig_Current.png");
      this.ConfigurationStateImageList.Images.SetKeyName(1, "MySQLInstallerConfig_Done.png");
      this.ConfigurationStateImageList.Images.SetKeyName(2, "MySQLInstallerConfig_Error.png");
      this.ConfigurationStateImageList.Images.SetKeyName(3, "MySQLInstallerConfig_Warning.png");
      // 
      // LastErrorDescriptionLabel
      // 
      this.LastErrorDescriptionLabel.AccessibleDescription = "A label displaying text about the last configuration error";
      this.LastErrorDescriptionLabel.AccessibleName = "Last Error Description";
      this.LastErrorDescriptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.LastErrorDescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.LastErrorDescriptionLabel.ForeColor = System.Drawing.Color.Red;
      this.LastErrorDescriptionLabel.Location = new System.Drawing.Point(30, 465);
      this.LastErrorDescriptionLabel.Name = "LastErrorDescriptionLabel";
      this.LastErrorDescriptionLabel.Size = new System.Drawing.Size(500, 34);
      this.LastErrorDescriptionLabel.TabIndex = 3;
      // 
      // ProductsListView
      // 
      this.ProductsListView.AccessibleDescription = "A list of products to be configured";
      this.ProductsListView.AccessibleName = "Products List";
      this.ProductsListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.ProductsListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.ProductsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.StatusImageColumnHeader,
            this.ProductNameColumnHeader,
            this.StatusDescriptionColumnHeader});
      this.ProductsListView.Location = new System.Drawing.Point(30, 153);
      this.ProductsListView.Name = "ProductsListView";
      this.ProductsListView.OwnerDraw = true;
      this.ProductsListView.ProgressBarCol = 0;
      this.ProductsListView.ProgressBarRow = 0;
      this.ProductsListView.Scrollable = false;
      this.ProductsListView.ShowItemToolTips = true;
      this.ProductsListView.Size = new System.Drawing.Size(500, 309);
      this.ProductsListView.StateImageList = this.ConfigurationStateImageList;
      this.ProductsListView.TabIndex = 2;
      this.ProductsListView.UseCompatibleStateImageBehavior = false;
      this.ProductsListView.View = System.Windows.Forms.View.Details;
      // 
      // StatusImageColumnHeader
      // 
      this.StatusImageColumnHeader.Text = "";
      this.StatusImageColumnHeader.Width = 22;
      // 
      // ProductNameColumnHeader
      // 
      this.ProductNameColumnHeader.Text = "Product";
      this.ProductNameColumnHeader.Width = 274;
      // 
      // StatusDescriptionColumnHeader
      // 
      this.StatusDescriptionColumnHeader.Text = "Status";
      this.StatusDescriptionColumnHeader.Width = 216;
      // 
      // ConfigOverviewPage
      // 
      this.AccessibleDescription = "A wizard page showing installed products that can be subsequently configured";
      this.AccessibleName = "Configuration Overview Page";
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
      this.Caption = "Product Configuration";
      this.Controls.Add(this.ProductsListView);
      this.Controls.Add(this.LastErrorDescriptionLabel);
      this.Name = "ConfigOverviewPage";
      this.SubCaption = "We\'ll now walk through a configuration wizard for each of the following products." +
    "\r\n\r\nYou can cancel at any point if you wish to leave this wizard without configu" +
    "ring all the products.";
      this.Controls.SetChildIndex(this.captionLabel, 0);
      this.Controls.SetChildIndex(this.subCaptionLabel, 0);
      this.Controls.SetChildIndex(this.LastErrorDescriptionLabel, 0);
      this.Controls.SetChildIndex(this.ProductsListView, 0);
      this.ResumeLayout(false);
      this.PerformLayout();

        }


        #endregion

        private MyListView ProductsListView;
        private System.Windows.Forms.ColumnHeader StatusImageColumnHeader;
        private System.Windows.Forms.ColumnHeader ProductNameColumnHeader;
        private System.Windows.Forms.ColumnHeader StatusDescriptionColumnHeader;
        private System.Windows.Forms.ImageList ConfigurationStateImageList;
        private System.Windows.Forms.Label LastErrorDescriptionLabel;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WexInstaller.Properties;
using WexInstaller.Core;
using System.Threading;

namespace WexInstaller
{
    public partial class RemoveAllPage : UserControl
    {
        SolidBrush darkGrayBrush = new SolidBrush(Color.DarkGray);
        Font titleFont = System.Drawing.SystemFonts.CaptionFont;

        ListViewItem removingItem;

        private AutoResetEvent clearedToProceed;

        public RemoveAllPage()
        {
            InitializeComponent();

            productList.Items.Clear();
            foreach (ProductCategory pc in ProductManager.ProductCategories)
            {
                foreach (Product p in pc.Products)
                {
                    if (p.Installed)
                    {
                        ListViewItem item = new ListViewItem(p.TitleWithVersion);
                        item.Name = p.Title;
                        item.Tag = p;
                        item.SubItems.Add(String.Empty);
                        item.SubItems.Add(String.Empty);
                        item.Checked = true;
                        productList.Items.Add(item);
                    }
                }
            }

            clearedToProceed = new AutoResetEvent(true);
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            (this.ParentForm as MainForm).GoBack();
        }

        private void ProductInstallProgressChanged(object sender, ProductMSIActionProgressEventArgs pe)
        {
          detailedLog.AppendText(String.Format("Beginning removal of {0}.{1}", (sender as Product).Title, Environment.NewLine));
          detailedLog.AppendText(String.Format("{0}{1}", pe.Message, Environment.NewLine));
          removingItem.SubItems[1].Text = String.Format(Resources.StatusPercentage, pe.ProgressPercentage);
        }

        private void ProductInstallCompleted(object sender, ProductMSIActionCompletedEventArgs pe)
        {
          if (pe.ExitCode != 0)
          {
            detailedLog.AppendText(String.Format("The product {0} failed to remove successfully.{1}", (sender as Product).Title, Environment.NewLine));
          }
          else
          {
            detailedLog.AppendText(String.Format("The product {0} was successfully removed.{1}", (sender as Product).Title, Environment.NewLine));
          }
          detailedLog.AppendText(Environment.NewLine);

          clearedToProceed.Set();

          if (removingItem.Index == productList.Items.Count - 1)
          {
            closeButton.Enabled = true;
            closeButton.Visible = true;

            cancelBtn.Enabled = false;
            cancelBtn.Visible = false;
          }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            enableDetails.Visible = true;

            if (productList.CheckedItems.Count > 0)
            {
                DialogResult result = MessageBox.Show("Are you sure you wish to remove all listed products?", "Confirm Removal", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (result == DialogResult.Yes)
                {
                    startButton.Enabled = false;

                    int totalRemaining = productList.CheckedItems.Count;
                    while (totalRemaining > 0)
                    {
                        if (clearedToProceed.WaitOne(1))
                        {
                            totalRemaining = productList.CheckedItems.Count;
                            foreach (ListViewItem item in productList.CheckedItems)
                            {
                                Product p = (item.Tag as Product);
                                ProductState state = p.CurrentState;
                                if (p.CurrentState == ProductState.RemoveStarted ||
                                    p.CurrentState == ProductState.RemoveInProgress)
                                {
                                    continue;
                                }

                                if (p.CurrentState == ProductState.InstallSuccess ||
                                   p.CurrentState == ProductState.CurrentlyInstalled )
                                {
                                    clearedToProceed.Reset();
                                    removingItem = item;
                                    p.ProductMSIActionProgressChanged +=new ProductMSIActionProgressHandler(ProductInstallProgressChanged);
                                    p.ProductMSIActionCompleted += new ProductMSIActionCompleteHandler(ProductInstallCompleted);
                                    p.Remove();
                                    totalRemaining--;
                                    break;
                                }
                                else
                                {
                                    totalRemaining--;
                                }
                            }
                        }
                        Application.DoEvents();
                        Thread.Sleep(0);
                    }
                }
            }
            else
            {
                startButton.Enabled = false;
                MessageBox.Show("No products selected for removal at this time.", "Nothing to do", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                startButton.Enabled = true;
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void productList_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            switch (e.Column)
            {
                case 0:
                    ListView l = sender as ListView;
                    foreach (ListViewItem li in l.Items)
                    {
                        li.Checked = !li.Checked;
                    }
                    break;
                default:
                    break;
            }
        }

        private void enableDetails_Click(object sender, EventArgs e)
        {
            enableDetails.Visible = false;
            detailedLog.Visible = true;
            detailedLog.BringToFront();
        }

    }
}

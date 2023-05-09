namespace WexInstaller.Dialogs
{
  partial class OptionalUpdateDialog
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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionalUpdateDialog));
      this.buttonOK = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.buttonRemindMeLater = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      this.SuspendLayout();
      // 
      // buttonOK
      // 
      this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.buttonOK.Location = new System.Drawing.Point(18, 155);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(111, 33);
      this.buttonOK.TabIndex = 8;
      this.buttonOK.Text = "Install Now";
      this.buttonOK.UseVisualStyleBackColor = true;
      // 
      // label2
      // 
      this.label2.Location = new System.Drawing.Point(15, 100);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(463, 52);
      this.label2.TabIndex = 7;
      this.label2.Text = resources.GetString("label2.Text");
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(243, 43);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(235, 33);
      this.label1.TabIndex = 6;
      this.label1.Text = "Optional Update";
      // 
      // pictureBox1
      // 
      this.pictureBox1.Image = global::WexInstaller.Properties.Resources.MySQLInstallerLogo;
      this.pictureBox1.Location = new System.Drawing.Point(12, 12);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(216, 64);
      this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 5;
      this.pictureBox1.TabStop = false;
      // 
      // buttonRemindMeLater
      // 
      this.buttonRemindMeLater.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.buttonRemindMeLater.Location = new System.Drawing.Point(152, 155);
      this.buttonRemindMeLater.Name = "buttonRemindMeLater";
      this.buttonRemindMeLater.Size = new System.Drawing.Size(111, 33);
      this.buttonRemindMeLater.TabIndex = 9;
      this.buttonRemindMeLater.Text = "Remind Me Later";
      this.buttonRemindMeLater.UseVisualStyleBackColor = true;
      // 
      // OptionalUpdateDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(249)))), ((int)(((byte)(249)))));
      this.ClientSize = new System.Drawing.Size(496, 213);
      this.Controls.Add(this.buttonRemindMeLater);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.pictureBox1);
      this.Name = "OptionalUpdateDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Optional Update Available";
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.Button buttonRemindMeLater;
  }
}
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

namespace MySql.Configurator.Core.Forms
{
  partial class ValidatingBaseDialog
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
      this.ValidationsErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
      this.ValidationsTimer = new System.Windows.Forms.Timer(this.components);
      this.ValidationsBackgroundWorker = new System.ComponentModel.BackgroundWorker();
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).BeginInit();
      this.SuspendLayout();
      // 
      // ValidationsErrorProvider
      // 
      this.ValidationsErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
      this.ValidationsErrorProvider.ContainerControl = this;
      // 
      // ValidationsTimer
      // 
      this.ValidationsTimer.Interval = 800;
      this.ValidationsTimer.Tick += new System.EventHandler(this.ValidationsTimerTick);
      // 
      // ValidationsBackgroundWorker
      // 
      this.ValidationsBackgroundWorker.WorkerSupportsCancellation = true;
      this.ValidationsBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ValidationsBackgroundWorker_DoWork);
      // 
      // ValidatingBaseDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(634, 292);
      this.CommandAreaVisible = true;
      this.FootnoteAreaVisible = true;
      this.Name = "ValidatingBaseDialog";
      this.Text = "ValidatingBaseDialog";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ValidatingBaseDialog_FormClosing);
      ((System.ComponentModel.ISupportInitialize)(this.ValidationsErrorProvider)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    protected System.Windows.Forms.ErrorProvider ValidationsErrorProvider;
    protected System.Windows.Forms.Timer ValidationsTimer;
    private System.ComponentModel.BackgroundWorker ValidationsBackgroundWorker;
  }
}

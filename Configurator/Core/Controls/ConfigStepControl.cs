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

using System;
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Controllers;
using MySql.Configurator.Properties;

namespace MySql.Configurator.Core.Controls
{
  public partial class ConfigStepControl : UserControl
  {
    public const int SUCCESS = 1;
    public const int OPEN = 0;
    public const int CURRENT = 2;
    public const int ERROR = 3;

    private string _baseLabel;

    /// <summary>
    /// Delegate to allow setting the image of the StepIconPictureBox control.
    /// </summary>
    /// <param name="status">An integer value representing the status defined by the SUCCESS, OPEN, CURRENT and ERROR constants.</param>
    public delegate void SetStatusCallback(int status);

    /// <summary>
    /// Delegate to allow displaying dots alongside the step's name to notify the user that the step is currently executing.
    /// </summary>
    /// <param name="dots">The number of dots to display</param>
    public delegate void SetDotsCallback(int dots);

    public ConfigStepControl()
    {
      InitializeComponent();
      TabStop = false;
    }

    protected override void OnLoad(EventArgs e)
    {
      Utilities.NormalizeFont(this);
      base.OnLoad(e);
    }

    public string Label
    {
      get
      {
        return _baseLabel;
      }

      set
      {
        _baseLabel = value;
        StepCaption.Text = value;
        StepCaption.AccessibleName = string.Format(Resources.ConfigStepCaptionAccessibleName, value);
        StepIconPictureBox.AccessibleName = string.Format(Resources.ConfigStepBulletAccessibleName, value);
      }
    }

    public ConfigurationStep Step;

    /// <summary>
    /// Delegete to allow displaying dots alongside the step's name to notify the user that the step is currently executing.
    /// </summary>
    /// <param name="dots">The number of dots to display</param>
    public void SetDots(int dots)
    {
      // InvokeRequired required compares the thread ID of the
      // calling thread to the thread ID of the creating thread.
      // If these threads are different, it returns true.
      if (StepIconPictureBox.InvokeRequired)
      {
        SetStatusCallback callbackMethod = new SetStatusCallback(SetDots);
        Invoke(callbackMethod, new object[] { dots });
      }
      else
      {
        SetDotsValue(dots);
      }
    }

    public void SetDotsValue(int dots)
    {
      StepCaption.Text = Label;
      if (dots == 0)
      {
        return;
      }

      StepCaption.Text += @"  ";
      for (int x = 0; x < dots; x++)
      {
        StepCaption.Text += @".";
      }

      StepCaption.Refresh();
    }

    /// <summary>
    /// Delegate to allow setting the image of the StepIconPictureBox control.
    /// </summary>
    /// <param name="status">An integer value representing the status defined by the SUCCESS, OPEN, CURRENT and ERROR constants.</param>
    public void SetStatus(int status)
    {
      // InvokeRequired required compares the thread ID of the
      // calling thread to the thread ID of the creating thread.
      // If these threads are different, it returns true.
      if (StepIconPictureBox.InvokeRequired)
      {
        SetStatusCallback callbackMethod = new SetStatusCallback(SetStatus);
        Invoke(callbackMethod, new object[] { status });
      }
      else
      {
        SetStatusValue(status);
      }
    }

    public void SetStatusValue(int status)
    {
      switch (status)
      {
        case OPEN:
          StepIconPictureBox.Image = Resources.ActionOpen;
          break;

        case CURRENT:
          StepIconPictureBox.Image = Resources.ActionCurrent;
          break;

        case ERROR:
          StepIconPictureBox.Image = Resources.ActionError;
          break;

        case SUCCESS:
          StepIconPictureBox.Image = Resources.ActionDone;
          break;
      }
    }
  }
}

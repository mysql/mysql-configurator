/* Copyright (c) 2014, 2023, Oracle and/or its affiliates.

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
using System.Diagnostics;
using System.Windows.Forms;
using MySql.Configurator.Core.Controllers;
using MySql.Configurator.Core.Enums;

namespace MySql.Configurator.Core.Controls
{
  public class StepFlowLayoutPanel : FlowLayoutPanel
  {
    private ProductConfigurationController _controller;
    private Stopwatch _stopWatch;
    private Timer _timer;
    private ConfigStepControl _currentStep;

    public StepFlowLayoutPanel()
    {
      _stopWatch = new Stopwatch();
      _timer = new Timer { Interval = 500 };
      _timer.Tick += timer_Tick;
    }

    public void SetController(ProductConfigurationController c, ConfigurationType configType)
    {
      Controls.Clear();
      _controller = c;
      foreach (var step in _controller.ConfigurationSteps)
      {
        if (!step.ValidForConfigureType(configType)
            || !step.Execute)
        {
          continue;
        }

        var cStep = new ConfigStepControl
        {
          Label = step.Description,
          Step = step
        };
        Controls.Add(cStep);
      }
    }

    private void timer_Tick(object sender, EventArgs e)
    {
      long millis = _stopWatch.ElapsedMilliseconds;
      int dots = (int)(millis / 500) % 3;
      _currentStep.SetDots(dots);
    }

    private void ConfigurationStepChanged(object sender, ConfigurationStep step)
    {
      foreach (var control in Controls)
      {
        var stepControl = control as ConfigStepControl;
        if (stepControl == null
            || stepControl.Step != step)
        {
          continue;
        }

        switch (step.Status)
        {
          case ConfigurationStepStatus.Started:
            stepControl.SetStatus(ConfigStepControl.CURRENT);
            _currentStep = stepControl;
            _stopWatch.Restart();
            _timer.Start();
            break;

          case ConfigurationStepStatus.Finished:
            stepControl.SetStatus(ConfigStepControl.SUCCESS);
            stepControl.SetDots(0);
            _timer.Stop();
            break;

          case ConfigurationStepStatus.Error:
            stepControl.SetStatus(ConfigStepControl.ERROR);
            stepControl.SetDots(0);
            _timer.Stop();
            break;

          case ConfigurationStepStatus.NotStarted: stepControl.SetStatus(ConfigStepControl.OPEN);
            break;
        }
      }
    }
  }
}

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

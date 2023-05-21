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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MySql.Configurator.Core.Controllers;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Properties;

namespace MySql.Configurator.Core.Controls
{
  /// <summary>
  /// Control used to display the status of an uninstall step and any substeps.
  /// </summary>
  public partial class RemoveStepControl : ConfigStepControl
  {
    #region Fields

    /// <summary>
    /// Defines the initial width and height of this control.
    /// </summary>
    private Size _initialSize;

    /// <summary>
    /// Indicates if the substeps are currently expanded.
    /// </summary>
    private bool _expanded;

    #endregion

    public RemoveStepControl(List<RemoveStep> subSteps, ProductConfigurationController controller) : base()
    {
      InitializeComponent();
      _initialSize = new Size(445, 28);
      Size = _initialSize;
      TabStop = false;
      Controller = controller;
      if (subSteps != null && subSteps.Count > 1)
      {
        SubSteps = new List<RemoveStepControl>();
        var point = new Point(25, _initialSize.Height);
        foreach (var step in subSteps)
        {
          var stepControl = new RemoveStepControl(null, null)
          {
            Label = step.Description,
            Step = step,
            Location = point
          };
          point.Offset(0, stepControl.Height);
          SubSteps.Add(stepControl);
          Controls.Add(stepControl);
          Height += stepControl.Height;
        }

        ExpandCollapseButton.Image = Resources.minus_sign;
        _expanded = true;
      }
      else
      {
        ExpandCollapseButton.Visible = false;
      }
    }

    #region Properties

    /// <summary>
    /// The <see cref="ProductConfigurationController"/> associated with this control.
    /// </summary>
    public ProductConfigurationController Controller { get; private set; }

    public new RemoveStep Step { get; set; }

    /// <summary>
    /// Gets or sets the list of sub steps to execute as part of the uninstall operation.
    /// </summary>
    public List<RemoveStepControl> SubSteps { get; set; }

    #endregion

    /// <summary>
    /// Collapses substeps into a single parent step.
    /// </summary>
    public void Collapse()
    {
      _expanded = false;
      ExpandCollapseButton.Image = Resources.plus_sign;
      Size = _initialSize;
    }

    /// <summary>
    /// Expands control to display all steps.
    /// </summary>
    public void Expand()
    {
      if (SubSteps == null || SubSteps.Count == 0)
      {
        return;
      }

      _expanded = true;
      ExpandCollapseButton.Image = Resources.minus_sign;
      Size = new Size(Size.Width, _initialSize.Height + (SubSteps.Count * _initialSize.Height));
    }

    /// <summary>
    /// Handles the Click event for the Expand button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    /// <remarks>Expands and collapses all substeps.</remarks>
    private void ExpandCollapseButton_Click(object sender, EventArgs e)
    {
      if (_expanded)
      {
        Collapse();
      }
      else
      {
        Expand();
      }
    }

    /// <summary>
    /// For parent step controls, checks if any of the steps failed to execute successfuly.
    /// </summary>
    /// <param name="checkRequiredStepsOnly">Flag indicating if only steps that are marked as required should be checked.</param>
    /// <returns><c>null</c> if the step has no child steps, <c>true</c> if any of the child steps failed to execute successfully;
    /// otherwise, <c>false</c>.</returns>
    public bool? HasFailedSubSteps(bool checkRequiredStepsOnly)
    {
      // Step has no child steps.
      if (SubSteps == null)
      {
        return null;
      }

      return SubSteps.Any(removeStepControl => ((checkRequiredStepsOnly && removeStepControl.Step.Required)
                                                || !checkRequiredStepsOnly)
                                               && removeStepControl.Step.Status != ConfigurationStepStatus.Finished);
    }
  }
}

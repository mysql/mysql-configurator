/* Copyright (c) 2010, 2019, Oracle and/or its affiliates.

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
using System.Linq;
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Core.Interfaces;
using Utilities = MySql.Configurator.Core.Classes.Utilities;

namespace MySql.Configurator.Core.Wizard
{
  public partial class WizardPage : UserControl
  {
    #region Fields

    /// <summary>
    /// Flag indicating whether the <see cref="TextChangedHandler"/> logic must not execute.
    /// </summary>
    private bool _skipTextChanged;

    #endregion Fields

    public WizardPage()
    {
      InitializeComponent();
      ErrorProperties = ErrorProviderProperties.Empty;
      ErrorLabel = null;
      WorkDone = false;
      PageVisible = true;
      DisabledControlShowingTooltip = null;
      SkipUpdateButtons = false;
      SkipValidations = false;
      IsUserInput = true;
    }

    #region Properties

    public virtual bool BackOk => Wizard.CanGoBack;

    public virtual bool CancelOk => true;

    public string Caption
    {
      get => captionLabel.Text;
      set => captionLabel.Text = value;
    }

    public virtual bool ExecuteOk => true;

    public virtual bool FinishOk => true;

    /// <summary>
    /// Gets or sets a value indicating whether when text changes on an input control was due user input or programmatic.
    /// </summary>
    protected bool IsUserInput;

    /// <summary>
    /// Gets a value indicating whether to show or hide the Next button.
    /// </summary>
    public virtual bool NextOk => Wizard.CanGoNext
                                  && !ValidationsErrorProvider.HasErrors();

    public bool PageVisible { get; set; }

    public string SubCaption
    {
      get => subCaptionLabel.Text;
      set => subCaptionLabel.Text = value;
    }

    public virtual string TabTitle => captionLabel.Text;

    public IWizard Wizard { get; set; }

    /// <summary>
    /// Property that can be used to signal that a panel has done all its work and there is no
    /// need to ask the user if he agrees when closing the application.
    /// </summary>
    public bool WorkDone { get; set; }

    /// <summary>
    /// Gets or sets <see cref="Utility.Classes.ErrorProviderProperties"/> to use with the <see cref="ValidationsErrorProvider"/>.
    /// </summary>
    protected ErrorProviderProperties ErrorProperties { get; }

    /// <summary>
    /// Gets or sets the <see cref="Control"/> that the <see cref="ValidationsErrorProvider"/> is attached to.
    /// </summary>
    protected Control ErrorProviderControl { get; set; }

    /// <summary>
    /// Gets or sets an optional <see cref="Label"/> shown next to an <see cref="ErrorProvider"/>.
    /// </summary>
    protected Label ErrorLabel { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="WizardPage.UpdateButtons"/> method is called.
    /// </summary>
    protected bool SkipUpdateButtons { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the validations within <see cref="ValidatedHandler"/> are skipped.
    /// </summary>
    /// <remarks>Normally used when single validations are needed to be skipped when programmatically setting values and then <see cref="FireAllValidations"/> is called at the end.</remarks>
    protected bool SkipValidations { get; set; }

    /// <summary>
    /// Gets or sets a disabled <see cref="Control"/> applying a workaround to show its tooltip.
    /// </summary>
    private Control DisabledControlShowingTooltip { get; set; }

    #endregion Properties

    public virtual void Activate()
    {
      Logger.LogInformation($"Beginning {Name}.");
    }

    public virtual bool Back()
    {
      return true;
    }

    public virtual bool Cancel()
    {
      return true;
    }

    public virtual void Deactivate()
    {
      Logger.LogInformation($"Finished {Name}.");
    }

    public virtual void Execute()
    {
    }

    public virtual bool Finish()
    {
      return true;
    }

    public virtual bool Next()
    {
      return true;
    }

    public virtual void WizardShowing()
    {
    }

    /// <summary>
    /// Fire validations for fields with no data, just for the sake of displaying their related error providers.
    /// </summary>
    /// <param name="validateInvisible">Flag indicating whether invisible controls are validated.</param>
    /// <param name="validateDisabled">Flag indicating whether disabled controls are validated.</param>
    protected virtual void FireAllValidations(bool validateInvisible = false, bool validateDisabled = false)
    {
      if (!IsUserInput)
      {
        return;
      }

      SkipUpdateButtons = true;
      foreach (var control in this.GetChildControlsOfType<Control>())
      {
        var controlEnabled = control.Enabled
                             && (!(control is TextBox textBox) || !textBox.ReadOnly);
        if (!control.Visible && !validateInvisible
            || !controlEnabled && !validateDisabled)
        {
          continue;
        }

        if (control.InvokeRequired)
        {
          control.Invoke(new MethodInvoker(() => control.Validate(false)));
        }
        else
        {
          control.Validate(false);
        }
      }

      UpdateButtons();
      SkipUpdateButtons = false;
    }

    protected override void OnLoad(EventArgs e)
    {
      Utilities.NormalizeFont(this);
      base.OnLoad(e);
    }

    /// <summary>
    /// Resets the validations timer.
    /// </summary>
    protected void ResetValidationsTimer()
    {
      ValidationsTimer.Stop();
      ValidationsTimer.Start();
    }

    /// <summary>
    /// Sets the text property value of the given control.
    /// </summary>
    /// <param name="control">Any object inheriting from <see cref="Control"/>.</param>
    /// <param name="textValue">Text to assign to the control's Text property.</param>
    protected virtual void SetControlTextSkippingValidation(Control control, string textValue)
    {
      if (control == null)
      {
        return;
      }

      if (InvokeRequired)
      {
        Invoke(new MethodInvoker(() => SetControlTextSkippingValidation(control, textValue)));
        return;
      }

      if (control.Text == textValue)
      {
        return;
      }

      _skipTextChanged = true;
      control.Text = textValue;
      _skipTextChanged = false;
    }

    /// <summary>
    /// Handles the TextChanged event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    /// <remarks>This event method is meant to be used with the <see cref="Control.TextChanged"/> event.</remarks>
    protected virtual void TextChangedHandler(object sender, EventArgs e)
    {
      if (_skipTextChanged)
      {
        return;
      }

      ResetValidationsTimer();
    }

    protected virtual void UpdateButtons()
    {
      Wizard?.UpdateButtons();
    }

    /// <summary>
    /// Handles the TextValidated event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    /// <remarks>This event method is meant to be used with the <see cref="Control.Validated"/> event.</remarks>
    protected virtual void ValidatedHandler(object sender, EventArgs e)
    {
      if (InvokeRequired)
      {
        Invoke(new MethodInvoker(() => ValidatedHandler(sender, e)));
        return;
      }

      ValidationsTimer.Stop();
      if (!IsUserInput)
      {
        return;
      }

      ErrorProviderControl = sender as Control;
      if (SkipValidations
          || ErrorProviderControl == null)
      {
        return;
      }

      ErrorLabel = null;
      ErrorProperties.ErrorMessage = ValidateFields();

      // This single error provider should be used for the majority of validations, unless other providers are needed, which can be handled in this same method.
      ValidationsErrorProvider.SetProperties(ErrorProviderControl, ErrorProperties);

      if (ErrorLabel != null)
      {
        ErrorLabel.Text = ErrorProperties.ErrorMessage;
      }

      if (!SkipUpdateButtons)
      {
        UpdateButtons();
      }
    }

    /// <summary>
    /// Contains calls to methods that validate the given control's value.
    /// </summary>
    /// <returns>An error message or <c>null</c> / <see cref="string.Empty"/> if everything is valid.</returns>
    /// <remarks>This event method is meant to ALWAYS been overridden to specify actions on the switch to validate different text boxes.</remarks>
    protected virtual string ValidateFields()
    {
      if (InvokeRequired)
      {
        return (string)Invoke(new Func<string>(ValidateFields));
      }

      if (ErrorProviderControl == null)
      {
        return null;
      }

      string errorMessage = null;
      switch (ErrorProviderControl.Name)
      {
        case "SomeControl":
          ErrorProviderControl = null; // Here we may override the control to attach the error provider to a different one.
          ErrorLabel = null; // Here we may set the ErrorLabel to a specific label control used along with SomeControl.
          errorMessage = "Add here a call to a method that validates the control's value and returns an error message or null/empty if everything is valid";
          break;
      }

      return errorMessage;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ValidationsTimer"/> timer's elapses.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    protected virtual void ValidationsTimer_Tick(object sender, EventArgs e)
    {
      var focusedControl = this.GetChildControlsOfType<Control>().FirstOrDefault(control => control.Focused);
      if (focusedControl != null)
      {
        focusedControl.Validate(false);
      }
      else
      {
        // In case no control has focus no validation is needed, just stop the timer.
        ValidationsTimer.Stop();
      }
    }

    private void WizardPage_MouseMove(object sender, MouseEventArgs e)
    {
      // Workaround to show a tooltip on a disabled control
      var control = GetChildAtPoint(e.Location);
      if (control != null)
      {
        if (control.Enabled || DisabledControlShowingTooltip != null)
        {
          return;
        }

        var tipString = ToolTip.GetToolTip(control);
        if (string.IsNullOrEmpty(tipString))
        {
          return;
        }

        DisabledControlShowingTooltip = control;
        var x = e.Location.X - DisabledControlShowingTooltip.Location.X;
        var y = e.Location.Y - DisabledControlShowingTooltip.Location.Y;
        ToolTip.Show(tipString, DisabledControlShowingTooltip, x, y);
      }
      else if (DisabledControlShowingTooltip != null)
      {
        ToolTip.Hide(DisabledControlShowingTooltip);
        DisabledControlShowingTooltip = null;
      }
    }
  }
}
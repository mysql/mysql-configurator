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
using System.Drawing;
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Properties;

namespace MySql.Configurator.Core.Controls
{
  /// <summary>
  /// Event handler for the OnChanged event.
  /// </summary>
  /// <param name="sender">The sender</param>
  /// <param name="e">Event arguments</param>
  public delegate void OnChangeHandler(object sender, EventArgs e);

  /// <summary>
  /// A self contained user control that provides text boxes for username, password, and verify password.
  /// When a user enters a set of matching passwords, the relative strength of the password is display along
  /// with a tooltip that describes the password complexity.
  /// </summary>
  public partial class NewUserControl : UserControl
  {
    /// <summary>
    /// Holds the text string for the username tooltip. string.Empty hides the username info icon.
    /// </summary>
    private string _usernameToolTipText;

    /// <summary>
    /// Minimum password length magic number.
    /// </summary>
    private int _minimumPasswordLength;

    /// <summary>
    /// Initializes a new instance of the <see cref="NewUserControl" /> class.
    /// </summary>
    public NewUserControl()
    {
      _usernameToolTipText = string.Empty;
      _minimumPasswordLength = 4;
      InitializeComponent();
    }

    /// <summary>
    /// Event is fired when the username, password, or confirm password textboxes are changed.
    /// </summary>
    public event OnChangeHandler OnChanged;

    /// <summary>
    /// Gets a value indicating whether the password and confirm password match and meet minimum complexity requirements.
    /// </summary>
    public bool PasswordRequirementsMet => !PasswordErrorPictureBox.Visible;

    /// <summary>
    /// Gets username if password meets the minimum requirements, otherwise string.Empty.
    /// </summary>
    public string Username => PasswordRequirementsMet ? UsernameTextBox.Text : string.Empty;

    /// <summary>
    /// Gets password if password meets the minimum requirements, otherwise string.Empty.
    /// </summary>
    public string Password => PasswordRequirementsMet ? PasswordTextBox.Text : string.Empty;

    /// <summary>
    /// Gets or sets the tool tip text for the username information icon.
    /// </summary>
    public string UsernameToolTipText
    {
      get
      {
        return _usernameToolTipText;
      }

      set
      {
        _usernameToolTipText = value;
        UsernameInfoPictureBox.Visible = !string.IsNullOrEmpty(_usernameToolTipText);
        UsernameFeedbackToolTip.SetToolTip(UsernameInfoPictureBox, _usernameToolTipText);
      }
    }

    /// <summary>
    /// Checks the password complexity and updates the UI controls.
    /// </summary>
    private void CheckPasswords()
    {
      if (!PasswordTextBox.Enabled)
      {
        return;
      }

      PasswordMinLengthLabel.Visible = PasswordTextBox.Text.Length < _minimumPasswordLength;
      if (PasswordMinLengthLabel.Visible)
      {
        PasswordErrorPictureBox.Visible = true;
        PasswordFeedbackToolTip.SetToolTip(PasswordErrorPictureBox, Resources.UsernamePasswordConfirmationPasswordNotGoodEnough);
      }
      else
      {
        PasswordErrorPictureBox.Visible = PasswordTextBox.Text != RepeatPasswordTextBox.Text;
        if (PasswordErrorPictureBox.Visible)
        {
          PasswordFeedbackToolTip.SetToolTip(PasswordErrorPictureBox, Resources.UsernamePasswordConfirmationPasswordDoNotMatch);
        }
      }

      UpdatePasswordStrengthMessage();
      PasswordStrengthValueLabel.Visible = !PasswordErrorPictureBox.Visible;
      PasswordStrengthLabel.Visible = PasswordStrengthValueLabel.Visible;
    }

    /// <summary>
    /// Updates the password strength message text.
    /// </summary>
    private void UpdatePasswordStrengthMessage()
    {
      PasswordStrengthType passwordStrength = PasswordStrengthVerifier.CheckPasswordStrength(PasswordTextBox.Text);
      PasswordStrengthValueLabel.Text = passwordStrength.ToString();
      switch (passwordStrength)
      {
        case PasswordStrengthType.Blank:
          PasswordStrengthValueLabel.ForeColor = ParentForm?.BackColor ?? BackColor;
          break;
        case PasswordStrengthType.Medium:
          PasswordStrengthValueLabel.ForeColor = Color.Gold;
          break;
        case PasswordStrengthType.Strong:
          PasswordStrengthValueLabel.ForeColor = Color.Green;
          break;
        default:
          PasswordStrengthValueLabel.ForeColor = Color.Red;
          break;
      }

      PasswordStrengthValueLabel.Top = PasswordStrengthLabel.Top;
    }

    /// <summary>
    /// Username TextBox changed event handler
    /// </summary>
    /// <param name="sender">username textbox control</param>
    /// <param name="e">event parameter</param>
    private void usernameTextBox_TextChanged(object sender, EventArgs e)
    {
      bool hasUsername = !string.IsNullOrEmpty(UsernameTextBox.Text);
      PasswordTextBox.Enabled = hasUsername;
      PasswordErrorPictureBox.Visible = hasUsername;
      RepeatPasswordTextBox.Enabled = hasUsername;
      PasswordStrengthPanel.Visible = hasUsername;
      PasswordStrengthValueLabel.Visible = hasUsername;
      PasswordStrengthLabel.Visible = hasUsername;

      if (hasUsername)
      {
        CheckPasswords();
      }
      else
      {
        PasswordTextBox.Text = string.Empty;
        RepeatPasswordTextBox.Text = string.Empty;
      }

      OnChanged?.Invoke(this, e);
    }

    /// <summary>
    /// Password and ConfirmPassword TextBoxes changed event handler
    /// </summary>
    /// <param name="sender">password or confirmPassword textbox</param>
    /// <param name="e">event parameter</param>
    private void RepeatPasswordTextBox_TextChanged(object sender, EventArgs e)
    {
      CheckPasswords();
      OnChanged?.Invoke(this, e);
    }
  }
}

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

using System.Drawing;
using System.Windows.Forms;
using MySql.Configurator.Base.Classes;
using MySql.Configurator.Base.Enums;

namespace MySql.Configurator.UI.Controls
{
  /// <inheritdoc />
  /// <summary>
  /// Represents a text to show the password strength where passwords are entered.
  /// </summary>
  public partial class PasswordStrengthLabel : FlowLayoutPanel
  {
    public PasswordStrengthLabel()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Updates the text and color of the password strength value label.
    /// </summary>
    /// <param name="password">The password to check for its strength.</param>
    /// <param name="visible">Flag indicating whether the control is visible or not.</param>
    public void UpdatePasswordStrengthMessage(string password, bool visible = true)
    {
      Visible = visible;
      if (!visible)
      {
        return;
      }

      var passwordStrength = PasswordStrengthVerifier.CheckPasswordStrength(password);
      ValueLabel.Text = passwordStrength.ToString();
      switch (passwordStrength)
      {
        case PasswordStrengthType.Blank:
          ValueLabel.ForeColor = Parent?.BackColor ?? SystemColors.Control;
          break;

        case PasswordStrengthType.Medium:
          ValueLabel.ForeColor = Color.Gold;
          break;

        case PasswordStrengthType.Strong:
          ValueLabel.ForeColor = Color.Green;
          break;

        default:
          ValueLabel.ForeColor = Color.Red;
          break;
      }
    }
  }
}

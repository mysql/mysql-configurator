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

using System.Drawing;
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Enums;

namespace MySql.Configurator.Core.Controls
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

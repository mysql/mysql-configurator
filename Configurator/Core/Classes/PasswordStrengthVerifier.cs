/* Copyright (c) 2012, 2023, Oracle and/or its affiliates.

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

using System.Text.RegularExpressions;
using MySql.Configurator.Core.Enums;

namespace MySql.Configurator.Core.Classes
{
  public static class PasswordStrengthVerifier
  {

    public static PasswordStrengthType CheckPasswordStrength(string password)
    {
      int passwordStrenghtLevel = 0;

      if (password.Length > 0)
      {
        passwordStrenghtLevel++;

        if (password.Length >= 8)
        {
          if (password.Length >= 12)
            passwordStrenghtLevel++;
          if (Regex.IsMatch(password, "[0-9]")) // Use of digits.
            passwordStrenghtLevel++;
          if (Regex.IsMatch(password, "(?=.*[a-z])(?=.*[A-Z])")) // Use of upper and lower case characters.
            passwordStrenghtLevel++;
          if (Regex.IsMatch(password, @"[!@#=$%^&*()_+|~=`{}\[\]:"";'<>?,.\/ \-]")) // Use of special characters.
            passwordStrenghtLevel++;
        }
      }
      switch (passwordStrenghtLevel)
      {
        case 1:
        case 2:
        case 3:
          return PasswordStrengthType.Weak;
        case 4:
          return PasswordStrengthType.Medium;
        case 5:
          return PasswordStrengthType.Strong;
        default:
          return PasswordStrengthType.Blank;
      }
    }
  }
}

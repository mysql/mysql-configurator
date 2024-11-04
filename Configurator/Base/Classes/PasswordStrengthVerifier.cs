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

using System.Text.RegularExpressions;
using MySql.Configurator.Base.Enums;

namespace MySql.Configurator.Base.Classes
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

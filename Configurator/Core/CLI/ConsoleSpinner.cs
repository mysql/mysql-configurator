/* Copyright (c) 2024, Oracle and/or its affiliates.

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

namespace MySql.Configurator.Core.CLI
{
  /// <summary>
  /// Basic class used to show a spinning effect in the command line.
  /// </summary>
  public class ConsoleSpinner
  {
    /// <summary>
    /// A counter used to keep track of the character to display.
    /// </summary>
    private int _counter;

    /// <summary>
    /// Initalizes a new instace of the <see cref="ConsoleSpinner"/> class.
    /// </summary>
    public ConsoleSpinner()
    {
      _counter = 0;
    }

    /// <summary>
    /// Controls how the cursor is displayed to create the effect of a spinning line.
    /// </summary>
    public void Turn()
    {
      _counter++;
      switch (_counter % 4)
      {
        case 0:
          Console.Write("/");
          break;
        case 1:
          Console.Write("-");
          break;
        case 2:
          Console.Write("\\");
          break;
        case 3: 
          Console.Write("|");
          break;
      }

      Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
    }
  }
}

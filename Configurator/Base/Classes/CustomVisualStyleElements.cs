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

using System.Windows.Forms.VisualStyles;

namespace MySql.Configurator.Base.Classes
{
  /// <summary>
  /// Represents visual style elements that render in an Aero style for windows and dialogs.
  /// </summary>
  public static class CustomVisualStyleElements
  {
    /// <summary>
    /// Represents a standard Task Dialog that as a standard has a content area and a command area as panels.
    /// </summary>
    public static class TaskDialog
    {
      /// <summary>
      /// Class name used to create <see cref="System.Windows.Forms.VisualStyles.VisualStyleElement"/> objects.
      /// </summary>
      private const string CLASS_NAME = "TASKDIALOG";

      /// <summary>
      /// Main contents panel for most of the controls displayed in the dialog.
      /// </summary>
      private static VisualStyleElement _primaryPanel;

      /// <summary>
      /// Panel containing command buttons to apply actions for the dialog.
      /// </summary>
      private static VisualStyleElement _secondaryPanel;

      /// <summary>
      /// Gets a panel representing the main contents area for most of the controls displayed in the dialog.
      /// </summary>
      public static VisualStyleElement PrimaryPanel => _primaryPanel ?? (_primaryPanel = VisualStyleElement.CreateElement(CLASS_NAME, 1, 0));

      /// <summary>
      /// Gets a panel containing command buttons to apply actions for the dialog.
      /// </summary>
      public static VisualStyleElement SecondaryPanel => _secondaryPanel ?? (_secondaryPanel = VisualStyleElement.CreateElement(CLASS_NAME, 8, 0));
    }

    /// <summary>
    /// Represents a standard Text Style element to draw title text on a Vista-style Task Dialog.
    /// </summary>
    public static class TextStyle
    {
      /// <summary>
      /// Class name used to create <see cref="System.Windows.Forms.VisualStyles.VisualStyleElement"/> objects.
      /// </summary>
      private const string CLASS_NAME = "TEXTSTYLE";

      /// <summary>
      /// Visual style element corresponding to the text of the main contents (the body) of the Task Dialog.
      /// </summary>
      private static VisualStyleElement _bodyText;

      /// <summary>
      /// Visual style element corresponding to the text of the title of the Task Dialog.
      /// </summary>
      private static VisualStyleElement _mainInstruction;

      /// <summary>
      /// Gets the visual style element corresponding to the text of the main contents (the body) of the Task Dialog.
      /// </summary>
      public static VisualStyleElement BodyText => _bodyText ?? (_bodyText = VisualStyleElement.CreateElement(CLASS_NAME, 4, 0));

      /// <summary>
      /// Gets the visual style element corresponding to the text of the title of the Task Dialog.
      /// </summary>
      public static VisualStyleElement MainInstruction => _mainInstruction ?? (_mainInstruction = VisualStyleElement.CreateElement(CLASS_NAME, 1, 0));
    }
  }
}

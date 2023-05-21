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
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MySql.Configurator.Core.Classes
{
  /// <summary>
  /// Class for debugging purposes meant to identify controls that receive focus by capturing Windows messages.
  /// </summary>
  public class LastFocusedControlFilter : IMessageFilter
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="LastFocusedControlFilter"/> class.
    /// </summary>
    public LastFocusedControlFilter()
    {
      FocusedControlHandle = IntPtr.Zero;
      FocusedControl = null;
      PrintDebugMessages = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LastFocusedControlFilter"/> class.
    /// </summary>
    /// <param name="printDebugMessages">Flag indicating whether messages are printed out to the debug console.</param>
    /// <param name="ignoredControls">A list of controls to ignore while filtering messages.</param>
    public LastFocusedControlFilter(bool printDebugMessages, List<Control> ignoredControls = null ) : this()
    {
      IgnoredControls = ignoredControls;
      PrintDebugMessages = printDebugMessages;
    }

    #region Properties

    /// <summary>
    /// Gets the the last control with focus.
    /// </summary>
    public Control FocusedControl { get; private set; }

    /// <summary>
    /// Gets the handle pointer of last control with focus.
    /// </summary>
    public IntPtr FocusedControlHandle { get; private set; }

    /// <summary>
    /// Gets or sets a list of controls to ignore while filtering messages.
    /// </summary>
    public List<Control> IgnoredControls { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether messages are printed out to the debug console.
    /// </summary>
    public bool PrintDebugMessages { get; set; }

    #endregion Properties

    public bool PreFilterMessage(ref Message m)
    {
      IntPtr handle = GetFocus();
      var control = Control.FromHandle(handle);
      if (control == null
          || (IgnoredControls != null
              && IgnoredControls.Any(c => c.IsHandleCreated && !c.Disposing && !c.IsDisposed && c.Handle == handle)))
      {
        return false;
      }

      FocusedControlHandle = handle;
      FocusedControl = control;
      if (PrintDebugMessages)
      {
        Debug.WriteLine($"Control {control.Name} has focus.");
      }

      return false;
    }

    /// <summary>
    /// Gets the handle of a control that currently has focus.
    /// </summary>
    /// <returns></returns>
    [DllImport("user32")]
    private static extern IntPtr GetFocus();
  }
}

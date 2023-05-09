/* Copyright (c) 2011, 2023, Oracle and/or its affiliates.

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

using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;

namespace MySql.Configurator.Core.Classes
{
  #region Constants

  /// <summary>
  /// Windows Messages
  /// Defined in winuser.h from Windows SDK v6.1
  /// Documentation pulled from MSDN.
  /// </summary>
  public enum WM : uint
  {
    /// <summary>
    /// The WM_NULL message performs no operation. An application sends the WM_NULL message if it wants to post a message that the recipient window will ignore.
    /// </summary>
    NULL = 0x0000,

    /// <summary>
    /// The WM_CREATE message is sent when an application requests that a window be created by calling the CreateWindowEx or CreateWindow function. (The message is sent before the function returns.) The window procedure of the new window receives this message after the window is created, but before the window becomes visible.
    /// </summary>
    CREATE = 0x0001,

    /// <summary>
    /// The WM_DESTROY message is sent when a window is being destroyed. It is sent to the window procedure of the window being destroyed after the window is removed from the screen.
    /// This message is sent first to the window being destroyed and then to the child windows (if any) as they are destroyed. During the processing of the message, it can be assumed that all child windows still exist.
    /// /// </summary>
    DESTROY = 0x0002,

    /// <summary>
    /// The WM_MOVE message is sent after a window has been moved.
    /// </summary>
    MOVE = 0x0003,

    /// <summary>
    /// The WM_SIZE message is sent to a window after its size has changed.
    /// </summary>
    SIZE = 0x0005,

    /// <summary>
    /// The WM_ACTIVATE message is sent to both the window being activated and the window being deactivated. If the windows use the same input queue, the message is sent synchronously, first to the window procedure of the top-level window being deactivated, then to the window procedure of the top-level window being activated. If the windows use different input queues, the message is sent asynchronously, so the window is activated immediately.
    /// </summary>
    ACTIVATE = 0x0006,

    /// <summary>
    /// The WM_SETFOCUS message is sent to a window after it has gained the keyboard focus.
    /// </summary>
    SETFOCUS = 0x0007,

    /// <summary>
    /// The WM_KILLFOCUS message is sent to a window immediately before it loses the keyboard focus.
    /// </summary>
    KILLFOCUS = 0x0008,

    /// <summary>
    /// The WM_ENABLE message is sent when an application changes the enabled state of a window. It is sent to the window whose enabled state is changing. This message is sent before the EnableWindow function returns, but after the enabled state (WS_DISABLED style bit) of the window has changed.
    /// </summary>
    ENABLE = 0x000A,

    /// <summary>
    /// An application sends the WM_SETREDRAW message to a window to allow changes in that window to be redrawn or to prevent changes in that window from being redrawn.
    /// </summary>
    SETREDRAW = 0x000B,

    /// <summary>
    /// An application sends a WM_SETTEXT message to set the text of a window.
    /// </summary>
    SETTEXT = 0x000C,

    /// <summary>
    /// An application sends a WM_GETTEXT message to copy the text that corresponds to a window into a buffer provided by the caller.
    /// </summary>
    GETTEXT = 0x000D,

    /// <summary>
    /// An application sends a WM_GETTEXTLENGTH message to determine the length, in characters, of the text associated with a window.
    /// </summary>
    GETTEXTLENGTH = 0x000E,

    /// <summary>
    /// The WM_PAINT message is sent when the system or another application makes a request to paint a portion of an application's window. The message is sent when the UpdateWindow or RedrawWindow function is called, or by the DispatchMessage function when the application obtains a WM_PAINT message by using the GetMessage or PeekMessage function.
    /// </summary>
    PAINT = 0x000F,

    /// <summary>
    /// The WM_CLOSE message is sent as a signal that a window or an application should terminate.
    /// </summary>
    CLOSE = 0x0010,

    /// <summary>
    /// The WM_QUERYENDSESSION message is sent when the user chooses to end the session or when an application calls one of the system shutdown functions. If any application returns zero, the session is not ended. The system stops sending WM_QUERYENDSESSION messages as soon as one application returns zero.
    /// After processing this message, the system sends the WM_ENDSESSION message with the wParam parameter set to the results of the WM_QUERYENDSESSION message.
    /// </summary>
    QUERYENDSESSION = 0x0011,

    /// <summary>
    /// The WM_QUERYOPEN message is sent to an icon when the user requests that the window be restored to its previous size and position.
    /// </summary>
    QUERYOPEN = 0x0013,

    /// <summary>
    /// The WM_ENDSESSION message is sent to an application after the system processes the results of the WM_QUERYENDSESSION message. The WM_ENDSESSION message informs the application whether the session is ending.
    /// </summary>
    ENDSESSION = 0x0016,

    /// <summary>
    /// The WM_QUIT message indicates a request to terminate an application and is generated when the application calls the PostQuitMessage function. It causes the GetMessage function to return zero.
    /// </summary>
    QUIT = 0x0012,

    /// <summary>
    /// The WM_ERASEBKGND message is sent when the window background must be erased (for example, when a window is resized). The message is sent to prepare an invalidated portion of a window for painting.
    /// </summary>
    ERASEBKGND = 0x0014,

    /// <summary>
    /// This message is sent to all top-level windows when a change is made to a system color setting.
    /// </summary>
    SYSCOLORCHANGE = 0x0015,

    /// <summary>
    /// The WM_SHOWWINDOW message is sent to a window when the window is about to be hidden or shown.
    /// </summary>
    SHOWWINDOW = 0x0018,

    /// <summary>
    /// An application sends the WM_WININICHANGE message to all top-level windows after making a change to the WIN.INI file. The SystemParametersInfo function sends this message after an application uses the function to change a setting in WIN.INI.
    /// Note  The WM_WININICHANGE message is provided only for compatibility with earlier versions of the system. Applications should use the WM_SETTINGCHANGE message.
    /// </summary>
    WININICHANGE = 0x001A,

    /// <summary>
    /// An application sends the WM_WININICHANGE message to all top-level windows after making a change to the WIN.INI file. The SystemParametersInfo function sends this message after an application uses the function to change a setting in WIN.INI.
    /// Note  The WM_WININICHANGE message is provided only for compatibility with earlier versions of the system. Applications should use the WM_SETTINGCHANGE message.
    /// </summary>
    SETTINGCHANGE = WM.WININICHANGE,

    /// <summary>
    /// The WM_DEVMODECHANGE message is sent to all top-level windows whenever the user changes device-mode settings.
    /// </summary>
    DEVMODECHANGE = 0x001B,

    /// <summary>
    /// The WM_ACTIVATEAPP message is sent when a window belonging to a different application than the active window is about to be activated. The message is sent to the application whose window is being activated and to the application whose window is being deactivated.
    /// </summary>
    ACTIVATEAPP = 0x001C,

    /// <summary>
    /// An application sends the WM_FONTCHANGE message to all top-level windows in the system after changing the pool of font resources.
    /// </summary>
    FONTCHANGE = 0x001D,

    /// <summary>
    /// A message that is sent whenever there is a change in the system time.
    /// </summary>
    TIMECHANGE = 0x001E,

    /// <summary>
    /// The WM_CANCELMODE message is sent to cancel certain modes, such as mouse capture. For example, the system sends this message to the active window when a dialog box or message box is displayed. Certain functions also send this message explicitly to the specified window regardless of whether it is the active window. For example, the EnableWindow function sends this message when disabling the specified window.
    /// </summary>
    CANCELMODE = 0x001F,

    /// <summary>
    /// The WM_SETCURSOR message is sent to a window if the mouse causes the cursor to move within a window and mouse input is not captured.
    /// </summary>
    SETCURSOR = 0x0020,

    /// <summary>
    /// The WM_MOUSEACTIVATE message is sent when the cursor is in an inactive window and the user presses a mouse button. The parent window receives this message only if the child window passes it to the DefWindowProc function.
    /// </summary>
    MOUSEACTIVATE = 0x0021,

    /// <summary>
    /// The WM_CHILDACTIVATE message is sent to a child window when the user clicks the window's title bar or when the window is activated, moved, or sized.
    /// </summary>
    CHILDACTIVATE = 0x0022,

    /// <summary>
    /// The WM_QUEUESYNC message is sent by a computer-based training (CBT) application to separate user-input messages from other messages sent through the WH_JOURNALPLAYBACK Hook procedure.
    /// </summary>
    QUEUESYNC = 0x0023,

    /// <summary>
    /// The WM_GETMINMAXINFO message is sent to a window when the size or position of the window is about to change. An application can use this message to override the window's default maximized size and position, or its default minimum or maximum tracking size.
    /// </summary>
    GETMINMAXINFO = 0x0024,

    /// <summary>
    /// Windows NT 3.51 and earlier: The WM_PAINTICON message is sent to a minimized window when the icon is to be painted. This message is not sent by newer versions of Microsoft Windows, except in unusual circumstances explained in the Remarks.
    /// </summary>
    PAINTICON = 0x0026,

    /// <summary>
    /// Windows NT 3.51 and earlier: The WM_ICONERASEBKGND message is sent to a minimized window when the background of the icon must be filled before painting the icon. A window receives this message only if a class icon is defined for the window; otherwise, WM_ERASEBKGND is sent. This message is not sent by newer versions of Windows.
    /// </summary>
    ICONERASEBKGND = 0x0027,

    /// <summary>
    /// The WM_NEXTDLGCTL message is sent to a dialog box procedure to set the keyboard focus to a different control in the dialog box.
    /// </summary>
    NEXTDLGCTL = 0x0028,

    /// <summary>
    /// The WM_SPOOLERSTATUS message is sent from Print Manager whenever a job is added to or removed from the Print Manager queue.
    /// </summary>
    SPOOLERSTATUS = 0x002A,

    /// <summary>
    /// The WM_DRAWITEM message is sent to the parent window of an owner-drawn button, combo box, list box, or menu when a visual aspect of the button, combo box, list box, or menu has changed.
    /// </summary>
    DRAWITEM = 0x002B,

    /// <summary>
    /// The WM_MEASUREITEM message is sent to the owner window of a combo box, list box, list view control, or menu item when the control or menu is created.
    /// </summary>
    MEASUREITEM = 0x002C,

    /// <summary>
    /// Sent to the owner of a list box or combo box when the list box or combo box is destroyed or when items are removed by the LB_DELETESTRING, LB_RESETCONTENT, CB_DELETESTRING, or CB_RESETCONTENT message. The system sends a WM_DELETEITEM message for each deleted item. The system sends the WM_DELETEITEM message for any deleted list box or combo box item with nonzero item data.
    /// </summary>
    DELETEITEM = 0x002D,

    /// <summary>
    /// Sent by a list box with the LBS_WANTKEYBOARDINPUT style to its owner in response to a WM_KEYDOWN message.
    /// </summary>
    VKEYTOITEM = 0x002E,

    /// <summary>
    /// Sent by a list box with the LBS_WANTKEYBOARDINPUT style to its owner in response to a WM_CHAR message.
    /// </summary>
    CHARTOITEM = 0x002F,

    /// <summary>
    /// An application sends a WM_SETFONT message to specify the font that a control is to use when drawing text.
    /// </summary>
    SETFONT = 0x0030,

    /// <summary>
    /// An application sends a WM_GETFONT message to a control to retrieve the font with which the control is currently drawing its text.
    /// </summary>
    GETFONT = 0x0031,

    /// <summary>
    /// An application sends a WM_SETHOTKEY message to a window to associate a hot key with the window. When the user presses the hot key, the system activates the window.
    /// </summary>
    SETHOTKEY = 0x0032,

    /// <summary>
    /// An application sends a WM_GETHOTKEY message to determine the hot key associated with a window.
    /// </summary>
    GETHOTKEY = 0x0033,

    /// <summary>
    /// The WM_QUERYDRAGICON message is sent to a minimized (iconic) window. The window is about to be dragged by the user but does not have an icon defined for its class. An application can return a handle to an icon or cursor. The system displays this cursor or icon while the user drags the icon.
    /// </summary>
    QUERYDRAGICON = 0x0037,

    /// <summary>
    /// The system sends the WM_COMPAREITEM message to determine the relative position of a new item in the sorted list of an owner-drawn combo box or list box. Whenever the application adds a new item, the system sends this message to the owner of a combo box or list box created with the CBS_SORT or LBS_SORT style.
    /// </summary>
    COMPAREITEM = 0x0039,

    /// <summary>
    /// Active Accessibility sends the WM_GETOBJECT message to obtain information about an accessible object contained in a server application.
    /// Applications never send this message directly. It is sent only by Active Accessibility in response to calls to AccessibleObjectFromPoint, AccessibleObjectFromEvent, or AccessibleObjectFromWindow. However, server applications handle this message.
    /// </summary>
    GETOBJECT = 0x003D,

    /// <summary>
    /// The WM_COMPACTING message is sent to all top-level windows when the system detects more than 12.5 percent of system time over a 30- to 60-second interval is being spent compacting memory. This indicates that system memory is low.
    /// </summary>
    COMPACTING = 0x0041,

    /// <summary>
    /// WM_COMMNOTIFY is Obsolete for Win32-Based Applications
    /// </summary>
    [Obsolete]
    COMMNOTIFY = 0x0044,

    /// <summary>
    /// The WM_WINDOWPOSCHANGING message is sent to a window whose size, position, or place in the Z order is about to change as a result of a call to the SetWindowPos function or another window-management function.
    /// </summary>
    WINDOWPOSCHANGING = 0x0046,

    /// <summary>
    /// The WM_WINDOWPOSCHANGED message is sent to a window whose size, position, or place in the Z order has changed as a result of a call to the SetWindowPos function or another window-management function.
    /// </summary>
    WINDOWPOSCHANGED = 0x0047,

    /// <summary>
    /// Notifies applications that the system, typically a battery-powered personal computer, is about to enter a suspended mode.
    /// Use: POWERBROADCAST
    /// </summary>
    [Obsolete]
    POWER = 0x0048,

    /// <summary>
    /// An application sends the WM_COPYDATA message to pass data to another application.
    /// </summary>
    COPYDATA = 0x004A,

    /// <summary>
    /// The WM_CANCELJOURNAL message is posted to an application when a user cancels the application's journaling activities. The message is posted with a NULL window handle.
    /// </summary>
    CANCELJOURNAL = 0x004B,

    /// <summary>
    /// Sent by a common control to its parent window when an event has occurred or the control requires some information.
    /// </summary>
    NOTIFY = 0x004E,

    /// <summary>
    /// The WM_INPUTLANGCHANGEREQUEST message is posted to the window with the focus when the user chooses a new input language, either with the hotkey (specified in the Keyboard control panel application) or from the indicator on the system taskbar. An application can accept the change by passing the message to the DefWindowProc function or reject the change (and prevent it from taking place) by returning immediately.
    /// </summary>
    INPUTLANGCHANGEREQUEST = 0x0050,

    /// <summary>
    /// The WM_INPUTLANGCHANGE message is sent to the topmost affected window after an application's input language has been changed. You should make any application-specific settings and pass the message to the DefWindowProc function, which passes the message to all first-level child windows. These child windows can pass the message to DefWindowProc to have it pass the message to their child windows, and so on.
    /// </summary>
    INPUTLANGCHANGE = 0x0051,

    /// <summary>
    /// Sent to an application that has initiated a training card with Microsoft Windows Help. The message informs the application when the user clicks an authorable button. An application initiates a training card by specifying the HELP_TCARD command in a call to the WinHelp function.
    /// </summary>
    TCARD = 0x0052,

    /// <summary>
    /// Indicates that the user pressed the F1 key. If a menu is active when F1 is pressed, WM_HELP is sent to the window associated with the menu; otherwise, WM_HELP is sent to the window that has the keyboard focus. If no window has the keyboard focus, WM_HELP is sent to the currently active window.
    /// </summary>
    HELP = 0x0053,

    /// <summary>
    /// The WM_USERCHANGED message is sent to all windows after the user has logged on or off. When the user logs on or off, the system updates the user-specific settings. The system sends this message immediately after updating the settings.
    /// </summary>
    USERCHANGED = 0x0054,

    /// <summary>
    /// Determines if a window accepts ANSI or Unicode structures in the WM_NOTIFY notification message. WM_NOTIFYFORMAT messages are sent from a common control to its parent window and from the parent window to the common control.
    /// </summary>
    NOTIFYFORMAT = 0x0055,

    /// <summary>
    /// The WM_CONTEXTMENU message notifies a window that the user clicked the right mouse button (right-clicked) in the window.
    /// </summary>
    CONTEXTMENU = 0x007B,

    /// <summary>
    /// The WM_STYLECHANGING message is sent to a window when the SetWindowLong function is about to change one or more of the window's styles.
    /// </summary>
    STYLECHANGING = 0x007C,

    /// <summary>
    /// The WM_STYLECHANGED message is sent to a window after the SetWindowLong function has changed one or more of the window's styles
    /// </summary>
    STYLECHANGED = 0x007D,

    /// <summary>
    /// The WM_DISPLAYCHANGE message is sent to all windows when the display resolution has changed.
    /// </summary>
    DISPLAYCHANGE = 0x007E,

    /// <summary>
    /// The WM_GETICON message is sent to a window to retrieve a handle to the large or small icon associated with a window. The system displays the large icon in the ALT+TAB dialog, and the small icon in the window caption.
    /// </summary>
    GETICON = 0x007F,

    /// <summary>
    /// An application sends the WM_SETICON message to associate a new large or small icon with a window. The system displays the large icon in the ALT+TAB dialog box, and the small icon in the window caption.
    /// </summary>
    SETICON = 0x0080,

    /// <summary>
    /// The WM_NCCREATE message is sent prior to the WM_CREATE message when a window is first created.
    /// </summary>
    NCCREATE = 0x0081,

    /// <summary>
    /// The WM_NCDESTROY message informs a window that its nonclient area is being destroyed. The DestroyWindow function sends the WM_NCDESTROY message to the window following the WM_DESTROY message. WM_DESTROY is used to free the allocated memory object associated with the window.
    /// The WM_NCDESTROY message is sent after the child windows have been destroyed. In contrast, WM_DESTROY is sent before the child windows are destroyed.
    /// </summary>
    NCDESTROY = 0x0082,

    /// <summary>
    /// The WM_NCCALCSIZE message is sent when the size and position of a window's client area must be calculated. By processing this message, an application can control the content of the window's client area when the size or position of the window changes.
    /// </summary>
    NCCALCSIZE = 0x0083,

    /// <summary>
    /// The WM_NCHITTEST message is sent to a window when the cursor moves, or when a mouse button is pressed or released. If the mouse is not captured, the message is sent to the window beneath the cursor. Otherwise, the message is sent to the window that has captured the mouse.
    /// </summary>
    NCHITTEST = 0x0084,

    /// <summary>
    /// The WM_NCPAINT message is sent to a window when its frame must be painted.
    /// </summary>
    NCPAINT = 0x0085,

    /// <summary>
    /// The WM_NCACTIVATE message is sent to a window when its nonclient area needs to be changed to indicate an active or inactive state.
    /// </summary>
    NCACTIVATE = 0x0086,

    /// <summary>
    /// The WM_GETDLGCODE message is sent to the window procedure associated with a control. By default, the system handles all keyboard input to the control; the system interprets certain types of keyboard input as dialog box navigation keys. To override this default behavior, the control can respond to the WM_GETDLGCODE message to indicate the types of input it wants to process itself.
    /// </summary>
    GETDLGCODE = 0x0087,

    /// <summary>
    /// The WM_SYNCPAINT message is used to synchronize painting while avoiding linking independent GUI threads.
    /// </summary>
    SYNCPAINT = 0x0088,

    /// <summary>
    /// The WM_NCMOUSEMOVE message is posted to a window when the cursor is moved within the nonclient area of the window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
    /// </summary>
    NCMOUSEMOVE = 0x00A0,

    /// <summary>
    /// The WM_NCLBUTTONDOWN message is posted when the user presses the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
    /// </summary>
    NCLBUTTONDOWN = 0x00A1,

    /// <summary>
    /// The WM_NCLBUTTONUP message is posted when the user releases the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
    /// </summary>
    NCLBUTTONUP = 0x00A2,

    /// <summary>
    /// The WM_NCLBUTTONDBLCLK message is posted when the user double-clicks the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
    /// </summary>
    NCLBUTTONDBLCLK = 0x00A3,

    /// <summary>
    /// The WM_NCRBUTTONDOWN message is posted when the user presses the right mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
    /// </summary>
    NCRBUTTONDOWN = 0x00A4,

    /// <summary>
    /// The WM_NCRBUTTONUP message is posted when the user releases the right mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
    /// </summary>
    NCRBUTTONUP = 0x00A5,

    /// <summary>
    /// The WM_NCRBUTTONDBLCLK message is posted when the user double-clicks the right mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
    /// </summary>
    NCRBUTTONDBLCLK = 0x00A6,

    /// <summary>
    /// The WM_NCMBUTTONDOWN message is posted when the user presses the middle mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
    /// </summary>
    NCMBUTTONDOWN = 0x00A7,

    /// <summary>
    /// The WM_NCMBUTTONUP message is posted when the user releases the middle mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
    /// </summary>
    NCMBUTTONUP = 0x00A8,

    /// <summary>
    /// The WM_NCMBUTTONDBLCLK message is posted when the user double-clicks the middle mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
    /// </summary>
    NCMBUTTONDBLCLK = 0x00A9,

    /// <summary>
    /// The WM_NCXBUTTONDOWN message is posted when the user presses the first or second X button while the cursor is in the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
    /// </summary>
    NCXBUTTONDOWN = 0x00AB,

    /// <summary>
    /// The WM_NCXBUTTONUP message is posted when the user releases the first or second X button while the cursor is in the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
    /// </summary>
    NCXBUTTONUP = 0x00AC,

    /// <summary>
    /// The WM_NCXBUTTONDBLCLK message is posted when the user double-clicks the first or second X button while the cursor is in the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
    /// </summary>
    NCXBUTTONDBLCLK = 0x00AD,

    /// <summary>
    /// The WM_INPUT_DEVICE_CHANGE message is sent to the window that registered to receive raw input. A window receives this message through its WindowProc function.
    /// </summary>
    INPUT_DEVICE_CHANGE = 0x00FE,

    /// <summary>
    /// The WM_INPUT message is sent to the window that is getting raw input.
    /// </summary>
    INPUT = 0x00FF,

    /// <summary>
    /// This message filters for keyboard messages.
    /// </summary>
    KEYFIRST = 0x0100,

    /// <summary>
    /// The WM_KEYDOWN message is posted to the window with the keyboard focus when a nonsystem key is pressed. A nonsystem key is a key that is pressed when the ALT key is not pressed.
    /// </summary>
    KEYDOWN = 0x0100,

    /// <summary>
    /// The WM_KEYUP message is posted to the window with the keyboard focus when a nonsystem key is released. A nonsystem key is a key that is pressed when the ALT key is not pressed, or a keyboard key that is pressed when a window has the keyboard focus.
    /// </summary>
    KEYUP = 0x0101,

    /// <summary>
    /// The WM_CHAR message is posted to the window with the keyboard focus when a WM_KEYDOWN message is translated by the TranslateMessage function. The WM_CHAR message contains the character code of the key that was pressed.
    /// </summary>
    CHAR = 0x0102,

    /// <summary>
    /// The WM_DEADCHAR message is posted to the window with the keyboard focus when a WM_KEYUP message is translated by the TranslateMessage function. WM_DEADCHAR specifies a character code generated by a dead key. A dead key is a key that generates a character, such as the umlaut (double-dot), that is combined with another character to form a composite character. For example, the umlaut-O character (Ö) is generated by typing the dead key for the umlaut character, and then typing the O key.
    /// </summary>
    DEADCHAR = 0x0103,

    /// <summary>
    /// The WM_SYSKEYDOWN message is posted to the window with the keyboard focus when the user presses the F10 key (which activates the menu bar) or holds down the ALT key and then presses another key. It also occurs when no window currently has the keyboard focus; in this case, the WM_SYSKEYDOWN message is sent to the active window. The window that receives the message can distinguish between these two contexts by checking the context code in the lParam parameter.
    /// </summary>
    SYSKEYDOWN = 0x0104,

    /// <summary>
    /// The WM_SYSKEYUP message is posted to the window with the keyboard focus when the user releases a key that was pressed while the ALT key was held down. It also occurs when no window currently has the keyboard focus; in this case, the WM_SYSKEYUP message is sent to the active window. The window that receives the message can distinguish between these two contexts by checking the context code in the lParam parameter.
    /// </summary>
    SYSKEYUP = 0x0105,

    /// <summary>
    /// The WM_SYSCHAR message is posted to the window with the keyboard focus when a WM_SYSKEYDOWN message is translated by the TranslateMessage function. It specifies the character code of a system character key — that is, a character key that is pressed while the ALT key is down.
    /// </summary>
    SYSCHAR = 0x0106,

    /// <summary>
    /// The WM_SYSDEADCHAR message is sent to the window with the keyboard focus when a WM_SYSKEYDOWN message is translated by the TranslateMessage function. WM_SYSDEADCHAR specifies the character code of a system dead key — that is, a dead key that is pressed while holding down the ALT key.
    /// </summary>
    SYSDEADCHAR = 0x0107,

    /// <summary>
    /// The WM_UNICHAR message is posted to the window with the keyboard focus when a WM_KEYDOWN message is translated by the TranslateMessage function. The WM_UNICHAR message contains the character code of the key that was pressed.
    /// The WM_UNICHAR message is equivalent to WM_CHAR, but it uses Unicode Transformation Format (UTF)-32, whereas WM_CHAR uses UTF-16. It is designed to send or post Unicode characters to ANSI windows and it can can handle Unicode Supplementary Plane characters.
    /// </summary>
    UNICHAR = 0x0109,

    /// <summary>
    /// This message filters for keyboard messages.
    /// </summary>
    KEYLAST = 0x0109,

    /// <summary>
    /// Sent immediately before the IME generates the composition string as a result of a keystroke. A window receives this message through its WindowProc function.
    /// </summary>
    IME_STARTCOMPOSITION = 0x010D,

    /// <summary>
    /// Sent to an application when the IME ends composition. A window receives this message through its WindowProc function.
    /// </summary>
    IME_ENDCOMPOSITION = 0x010E,

    /// <summary>
    /// Sent to an application when the IME changes composition status as a result of a keystroke. A window receives this message through its WindowProc function.
    /// </summary>
    IME_COMPOSITION = 0x010F,

    IME_KEYLAST = 0x010F,

    /// <summary>
    /// The WM_INITDIALOG message is sent to the dialog box procedure immediately before a dialog box is displayed. Dialog box procedures typically use this message to initialize controls and carry out any other initialization tasks that affect the appearance of the dialog box.
    /// </summary>
    INITDIALOG = 0x0110,

    /// <summary>
    /// The WM_COMMAND message is sent when the user selects a command item from a menu, when a control sends a notification message to its parent window, or when an accelerator keystroke is translated.
    /// </summary>
    COMMAND = 0x0111,

    /// <summary>
    /// A window receives this message when the user chooses a command from the Window menu, clicks the maximize button, minimize button, restore button, close button, or moves the form. You can stop the form from moving by filtering this out.
    /// </summary>
    SYSCOMMAND = 0x0112,

    /// <summary>
    /// The WM_TIMER message is posted to the installing thread's message queue when a timer expires. The message is posted by the GetMessage or PeekMessage function.
    /// </summary>
    TIMER = 0x0113,

    /// <summary>
    /// The WM_HSCROLL message is sent to a window when a scroll event occurs in the window's standard horizontal scroll bar. This message is also sent to the owner of a horizontal scroll bar control when a scroll event occurs in the control.
    /// </summary>
    HSCROLL = 0x0114,

    /// <summary>
    /// The WM_VSCROLL message is sent to a window when a scroll event occurs in the window's standard vertical scroll bar. This message is also sent to the owner of a vertical scroll bar control when a scroll event occurs in the control.
    /// </summary>
    VSCROLL = 0x0115,

    /// <summary>
    /// The WM_INITMENU message is sent when a menu is about to become active. It occurs when the user clicks an item on the menu bar or presses a menu key. This allows the application to modify the menu before it is displayed.
    /// </summary>
    INITMENU = 0x0116,

    /// <summary>
    /// The WM_INITMENUPOPUP message is sent when a drop-down menu or submenu is about to become active. This allows an application to modify the menu before it is displayed, without changing the entire menu.
    /// </summary>
    INITMENUPOPUP = 0x0117,

    /// <summary>
    /// The WM_MENUSELECT message is sent to a menu's owner window when the user selects a menu item.
    /// </summary>
    MENUSELECT = 0x011F,

    /// <summary>
    /// The WM_MENUCHAR message is sent when a menu is active and the user presses a key that does not correspond to any mnemonic or accelerator key. This message is sent to the window that owns the menu.
    /// </summary>
    MENUCHAR = 0x0120,

    /// <summary>
    /// The WM_ENTERIDLE message is sent to the owner window of a modal dialog box or menu that is entering an idle state. A modal dialog box or menu enters an idle state when no messages are waiting in its queue after it has processed one or more previous messages.
    /// </summary>
    ENTERIDLE = 0x0121,

    /// <summary>
    /// The WM_MENURBUTTONUP message is sent when the user releases the right mouse button while the cursor is on a menu item.
    /// </summary>
    MENURBUTTONUP = 0x0122,

    /// <summary>
    /// The WM_MENUDRAG message is sent to the owner of a drag-and-drop menu when the user drags a menu item.
    /// </summary>
    MENUDRAG = 0x0123,

    /// <summary>
    /// The WM_MENUGETOBJECT message is sent to the owner of a drag-and-drop menu when the mouse cursor enters a menu item or moves from the center of the item to the top or bottom of the item.
    /// </summary>
    MENUGETOBJECT = 0x0124,

    /// <summary>
    /// The WM_UNINITMENUPOPUP message is sent when a drop-down menu or submenu has been destroyed.
    /// </summary>
    UNINITMENUPOPUP = 0x0125,

    /// <summary>
    /// The WM_MENUCOMMAND message is sent when the user makes a selection from a menu.
    /// </summary>
    MENUCOMMAND = 0x0126,

    /// <summary>
    /// An application sends the WM_CHANGEUISTATE message to indicate that the user interface (UI) state should be changed.
    /// </summary>
    CHANGEUISTATE = 0x0127,

    /// <summary>
    /// An application sends the WM_UPDATEUISTATE message to change the user interface (UI) state for the specified window and all its child windows.
    /// </summary>
    UPDATEUISTATE = 0x0128,

    /// <summary>
    /// An application sends the WM_QUERYUISTATE message to retrieve the user interface (UI) state for a window.
    /// </summary>
    QUERYUISTATE = 0x0129,

    /// <summary>
    /// The WM_CTLCOLORMSGBOX message is sent to the owner window of a message box before Windows draws the message box. By responding to this message, the owner window can set the text and background colors of the message box by using the given display device context handle.
    /// </summary>
    CTLCOLORMSGBOX = 0x0132,

    /// <summary>
    /// An edit control that is not read-only or disabled sends the WM_CTLCOLOREDIT message to its parent window when the control is about to be drawn. By responding to this message, the parent window can use the specified device context handle to set the text and background colors of the edit control.
    /// </summary>
    CTLCOLOREDIT = 0x0133,

    /// <summary>
    /// Sent to the parent window of a list box before the system draws the list box. By responding to this message, the parent window can set the text and background colors of the list box by using the specified display device context handle.
    /// </summary>
    CTLCOLORLISTBOX = 0x0134,

    /// <summary>
    /// The WM_CTLCOLORBTN message is sent to the parent window of a button before drawing the button. The parent window can change the button's text and background colors. However, only owner-drawn buttons respond to the parent window processing this message.
    /// </summary>
    CTLCOLORBTN = 0x0135,

    /// <summary>
    /// The WM_CTLCOLORDLG message is sent to a dialog box before the system draws the dialog box. By responding to this message, the dialog box can set its text and background colors using the specified display device context handle.
    /// </summary>
    CTLCOLORDLG = 0x0136,

    /// <summary>
    /// The WM_CTLCOLORSCROLLBAR message is sent to the parent window of a scroll bar control when the control is about to be drawn. By responding to this message, the parent window can use the display context handle to set the background color of the scroll bar control.
    /// </summary>
    CTLCOLORSCROLLBAR = 0x0137,

    /// <summary>
    /// A static control, or an edit control that is read-only or disabled, sends the WM_CTLCOLORSTATIC message to its parent window when the control is about to be drawn. By responding to this message, the parent window can use the specified device context handle to set the text and background colors of the static control.
    /// </summary>
    CTLCOLORSTATIC = 0x0138,

    /// <summary>
    /// Use WM_MOUSEFIRST to specify the first mouse message. Use the PeekMessage() Function.
    /// </summary>
    MOUSEFIRST = 0x0200,

    /// <summary>
    /// The WM_MOUSEMOVE message is posted to a window when the cursor moves. If the mouse is not captured, the message is posted to the window that contains the cursor. Otherwise, the message is posted to the window that has captured the mouse.
    /// </summary>
    MOUSEMOVE = 0x0200,

    /// <summary>
    /// The WM_LBUTTONDOWN message is posted when the user presses the left mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
    /// </summary>
    LBUTTONDOWN = 0x0201,

    /// <summary>
    /// The WM_LBUTTONUP message is posted when the user releases the left mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
    /// </summary>
    LBUTTONUP = 0x0202,

    /// <summary>
    /// The WM_LBUTTONDBLCLK message is posted when the user double-clicks the left mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
    /// </summary>
    LBUTTONDBLCLK = 0x0203,

    /// <summary>
    /// The WM_RBUTTONDOWN message is posted when the user presses the right mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
    /// </summary>
    RBUTTONDOWN = 0x0204,

    /// <summary>
    /// The WM_RBUTTONUP message is posted when the user releases the right mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
    /// </summary>
    RBUTTONUP = 0x0205,

    /// <summary>
    /// The WM_RBUTTONDBLCLK message is posted when the user double-clicks the right mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
    /// </summary>
    RBUTTONDBLCLK = 0x0206,

    /// <summary>
    /// The WM_MBUTTONDOWN message is posted when the user presses the middle mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
    /// </summary>
    MBUTTONDOWN = 0x0207,

    /// <summary>
    /// The WM_MBUTTONUP message is posted when the user releases the middle mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
    /// </summary>
    MBUTTONUP = 0x0208,

    /// <summary>
    /// The WM_MBUTTONDBLCLK message is posted when the user double-clicks the middle mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
    /// </summary>
    MBUTTONDBLCLK = 0x0209,

    /// <summary>
    /// The WM_MOUSEWHEEL message is sent to the focus window when the mouse wheel is rotated. The DefWindowProc function propagates the message to the window's parent. There should be no internal forwarding of the message, since DefWindowProc propagates it up the parent chain until it finds a window that processes it.
    /// </summary>
    MOUSEWHEEL = 0x020A,

    /// <summary>
    /// The WM_XBUTTONDOWN message is posted when the user presses the first or second X button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
    /// </summary>
    XBUTTONDOWN = 0x020B,

    /// <summary>
    /// The WM_XBUTTONUP message is posted when the user releases the first or second X button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
    /// </summary>
    XBUTTONUP = 0x020C,

    /// <summary>
    /// The WM_XBUTTONDBLCLK message is posted when the user double-clicks the first or second X button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
    /// </summary>
    XBUTTONDBLCLK = 0x020D,

    /// <summary>
    /// The WM_MOUSEHWHEEL message is sent to the focus window when the mouse's horizontal scroll wheel is tilted or rotated. The DefWindowProc function propagates the message to the window's parent. There should be no internal forwarding of the message, since DefWindowProc propagates it up the parent chain until it finds a window that processes it.
    /// </summary>
    MOUSEHWHEEL = 0x020E,

    /// <summary>
    /// Use WM_MOUSELAST to specify the last mouse message. Used with PeekMessage() Function.
    /// </summary>
    MOUSELAST = 0x020E,

    /// <summary>
    /// The WM_PARENTNOTIFY message is sent to the parent of a child window when the child window is created or destroyed, or when the user clicks a mouse button while the cursor is over the child window. When the child window is being created, the system sends WM_PARENTNOTIFY just before the CreateWindow or CreateWindowEx function that creates the window returns. When the child window is being destroyed, the system sends the message before any processing to destroy the window takes place.
    /// </summary>
    PARENTNOTIFY = 0x0210,

    /// <summary>
    /// The WM_ENTERMENULOOP message informs an application's main window procedure that a menu modal loop has been entered.
    /// </summary>
    ENTERMENULOOP = 0x0211,

    /// <summary>
    /// The WM_EXITMENULOOP message informs an application's main window procedure that a menu modal loop has been exited.
    /// </summary>
    EXITMENULOOP = 0x0212,

    /// <summary>
    /// The WM_NEXTMENU message is sent to an application when the right or left arrow key is used to switch between the menu bar and the system menu.
    /// </summary>
    NEXTMENU = 0x0213,

    /// <summary>
    /// The WM_SIZING message is sent to a window that the user is resizing. By processing this message, an application can monitor the size and position of the drag rectangle and, if needed, change its size or position.
    /// </summary>
    SIZING = 0x0214,

    /// <summary>
    /// The WM_CAPTURECHANGED message is sent to the window that is losing the mouse capture.
    /// </summary>
    CAPTURECHANGED = 0x0215,

    /// <summary>
    /// The WM_MOVING message is sent to a window that the user is moving. By processing this message, an application can monitor the position of the drag rectangle and, if needed, change its position.
    /// </summary>
    MOVING = 0x0216,

    /// <summary>
    /// Notifies applications that a power-management event has occurred.
    /// </summary>
    POWERBROADCAST = 0x0218,

    /// <summary>
    /// Notifies an application of a change to the hardware configuration of a device or the computer.
    /// </summary>
    DEVICECHANGE = 0x0219,

    /// <summary>
    /// An application sends the WM_MDICREATE message to a multiple-document interface (MDI) client window to create an MDI child window.
    /// </summary>
    MDICREATE = 0x0220,

    /// <summary>
    /// An application sends the WM_MDIDESTROY message to a multiple-document interface (MDI) client window to close an MDI child window.
    /// </summary>
    MDIDESTROY = 0x0221,

    /// <summary>
    /// An application sends the WM_MDIACTIVATE message to a multiple-document interface (MDI) client window to instruct the client window to activate a different MDI child window.
    /// </summary>
    MDIACTIVATE = 0x0222,

    /// <summary>
    /// An application sends the WM_MDIRESTORE message to a multiple-document interface (MDI) client window to restore an MDI child window from maximized or minimized size.
    /// </summary>
    MDIRESTORE = 0x0223,

    /// <summary>
    /// An application sends the WM_MDINEXT message to a multiple-document interface (MDI) client window to activate the next or previous child window.
    /// </summary>
    MDINEXT = 0x0224,

    /// <summary>
    /// An application sends the WM_MDIMAXIMIZE message to a multiple-document interface (MDI) client window to maximize an MDI child window. The system resizes the child window to make its client area fill the client window. The system places the child window's window menu icon in the rightmost position of the frame window's menu bar, and places the child window's restore icon in the leftmost position. The system also appends the title bar text of the child window to that of the frame window.
    /// </summary>
    MDIMAXIMIZE = 0x0225,

    /// <summary>
    /// An application sends the WM_MDITILE message to a multiple-document interface (MDI) client window to arrange all of its MDI child windows in a tile format.
    /// </summary>
    MDITILE = 0x0226,

    /// <summary>
    /// An application sends the WM_MDICASCADE message to a multiple-document interface (MDI) client window to arrange all its child windows in a cascade format.
    /// </summary>
    MDICASCADE = 0x0227,

    /// <summary>
    /// An application sends the WM_MDIICONARRANGE message to a multiple-document interface (MDI) client window to arrange all minimized MDI child windows. It does not affect child windows that are not minimized.
    /// </summary>
    MDIICONARRANGE = 0x0228,

    /// <summary>
    /// An application sends the WM_MDIGETACTIVE message to a multiple-document interface (MDI) client window to retrieve the handle to the active MDI child window.
    /// </summary>
    MDIGETACTIVE = 0x0229,

    /// <summary>
    /// An application sends the WM_MDISETMENU message to a multiple-document interface (MDI) client window to replace the entire menu of an MDI frame window, to replace the window menu of the frame window, or both.
    /// </summary>
    MDISETMENU = 0x0230,

    /// <summary>
    /// The WM_ENTERSIZEMOVE message is sent one time to a window after it enters the moving or sizing modal loop. The window enters the moving or sizing modal loop when the user clicks the window's title bar or sizing border, or when the window passes the WM_SYSCOMMAND message to the DefWindowProc function and the wParam parameter of the message specifies the SC_MOVE or SC_SIZE value. The operation is complete when DefWindowProc returns.
    /// The system sends the WM_ENTERSIZEMOVE message regardless of whether the dragging of full windows is enabled.
    /// </summary>
    ENTERSIZEMOVE = 0x0231,

    /// <summary>
    /// The WM_EXITSIZEMOVE message is sent one time to a window, after it has exited the moving or sizing modal loop. The window enters the moving or sizing modal loop when the user clicks the window's title bar or sizing border, or when the window passes the WM_SYSCOMMAND message to the DefWindowProc function and the wParam parameter of the message specifies the SC_MOVE or SC_SIZE value. The operation is complete when DefWindowProc returns.
    /// </summary>
    EXITSIZEMOVE = 0x0232,

    /// <summary>
    /// Sent when the user drops a file on the window of an application that has registered itself as a recipient of dropped files.
    /// </summary>
    DROPFILES = 0x0233,

    /// <summary>
    /// An application sends the WM_MDIREFRESHMENU message to a multiple-document interface (MDI) client window to refresh the window menu of the MDI frame window.
    /// </summary>
    MDIREFRESHMENU = 0x0234,

    /// <summary>
    /// Sent to an application when a window is activated. A window receives this message through its WindowProc function.
    /// </summary>
    IME_SETCONTEXT = 0x0281,

    /// <summary>
    /// Sent to an application to notify it of changes to the IME window. A window receives this message through its WindowProc function.
    /// </summary>
    IME_NOTIFY = 0x0282,

    /// <summary>
    /// Sent by an application to direct the IME window to carry out the requested command. The application uses this message to control the IME window that it has created. To send this message, the application calls the SendMessage function with the following parameters.
    /// </summary>
    IME_CONTROL = 0x0283,

    /// <summary>
    /// Sent to an application when the IME window finds no space to extend the area for the composition window. A window receives this message through its WindowProc function.
    /// </summary>
    IME_COMPOSITIONFULL = 0x0284,

    /// <summary>
    /// Sent to an application when the operating system is about to change the current IME. A window receives this message through its WindowProc function.
    /// </summary>
    IME_SELECT = 0x0285,

    /// <summary>
    /// Sent to an application when the IME gets a character of the conversion result. A window receives this message through its WindowProc function.
    /// </summary>
    IME_CHAR = 0x0286,

    /// <summary>
    /// Sent to an application to provide commands and request information. A window receives this message through its WindowProc function.
    /// </summary>
    IME_REQUEST = 0x0288,

    /// <summary>
    /// Sent to an application by the IME to notify the application of a key press and to keep message order. A window receives this message through its WindowProc function.
    /// </summary>
    IME_KEYDOWN = 0x0290,

    /// <summary>
    /// Sent to an application by the IME to notify the application of a key release and to keep message order. A window receives this message through its WindowProc function.
    /// </summary>
    IME_KEYUP = 0x0291,

    /// <summary>
    /// The WM_MOUSEHOVER message is posted to a window when the cursor hovers over the client area of the window for the period of time specified in a prior call to TrackMouseEvent.
    /// </summary>
    MOUSEHOVER = 0x02A1,

    /// <summary>
    /// The WM_MOUSELEAVE message is posted to a window when the cursor leaves the client area of the window specified in a prior call to TrackMouseEvent.
    /// </summary>
    MOUSELEAVE = 0x02A3,

    /// <summary>
    /// The WM_NCMOUSEHOVER message is posted to a window when the cursor hovers over the nonclient area of the window for the period of time specified in a prior call to TrackMouseEvent.
    /// </summary>
    NCMOUSEHOVER = 0x02A0,

    /// <summary>
    /// The WM_NCMOUSELEAVE message is posted to a window when the cursor leaves the nonclient area of the window specified in a prior call to TrackMouseEvent.
    /// </summary>
    NCMOUSELEAVE = 0x02A2,

    /// <summary>
    /// The WM_WTSSESSION_CHANGE message notifies applications of changes in session state.
    /// </summary>
    WTSSESSION_CHANGE = 0x02B1,

    TABLET_FIRST = 0x02c0,
    TABLET_LAST = 0x02df,

    /// <summary>
    /// An application sends a WM_CUT message to an edit control or combo box to delete (cut) the current selection, if any, in the edit control and copy the deleted text to the clipboard in CF_TEXT format.
    /// </summary>
    CUT = 0x0300,

    /// <summary>
    /// An application sends the WM_COPY message to an edit control or combo box to copy the current selection to the clipboard in CF_TEXT format.
    /// </summary>
    COPY = 0x0301,

    /// <summary>
    /// An application sends a WM_PASTE message to an edit control or combo box to copy the current content of the clipboard to the edit control at the current caret position. Data is inserted only if the clipboard contains data in CF_TEXT format.
    /// </summary>
    PASTE = 0x0302,

    /// <summary>
    /// An application sends a WM_CLEAR message to an edit control or combo box to delete (clear) the current selection, if any, from the edit control.
    /// </summary>
    CLEAR = 0x0303,

    /// <summary>
    /// An application sends a WM_UNDO message to an edit control to undo the last operation. When this message is sent to an edit control, the previously deleted text is restored or the previously added text is deleted.
    /// </summary>
    UNDO = 0x0304,

    /// <summary>
    /// The WM_RENDERFORMAT message is sent to the clipboard owner if it has delayed rendering a specific clipboard format and if an application has requested data in that format. The clipboard owner must render data in the specified format and place it on the clipboard by calling the SetClipboardData function.
    /// </summary>
    RENDERFORMAT = 0x0305,

    /// <summary>
    /// The WM_RENDERALLFORMATS message is sent to the clipboard owner before it is destroyed, if the clipboard owner has delayed rendering one or more clipboard formats. For the content of the clipboard to remain available to other applications, the clipboard owner must render data in all the formats it is capable of generating, and place the data on the clipboard by calling the SetClipboardData function.
    /// </summary>
    RENDERALLFORMATS = 0x0306,

    /// <summary>
    /// The WM_DESTROYCLIPBOARD message is sent to the clipboard owner when a call to the EmptyClipboard function empties the clipboard.
    /// </summary>
    DESTROYCLIPBOARD = 0x0307,

    /// <summary>
    /// The WM_DRAWCLIPBOARD message is sent to the first window in the clipboard viewer chain when the content of the clipboard changes. This enables a clipboard viewer window to display the new content of the clipboard.
    /// </summary>
    DRAWCLIPBOARD = 0x0308,

    /// <summary>
    /// The WM_PAINTCLIPBOARD message is sent to the clipboard owner by a clipboard viewer window when the clipboard contains data in the CF_OWNERDISPLAY format and the clipboard viewer's client area needs repainting.
    /// </summary>
    PAINTCLIPBOARD = 0x0309,

    /// <summary>
    /// The WM_VSCROLLCLIPBOARD message is sent to the clipboard owner by a clipboard viewer window when the clipboard contains data in the CF_OWNERDISPLAY format and an event occurs in the clipboard viewer's vertical scroll bar. The owner should scroll the clipboard image and update the scroll bar values.
    /// </summary>
    VSCROLLCLIPBOARD = 0x030A,

    /// <summary>
    /// The WM_SIZECLIPBOARD message is sent to the clipboard owner by a clipboard viewer window when the clipboard contains data in the CF_OWNERDISPLAY format and the clipboard viewer's client area has changed size.
    /// </summary>
    SIZECLIPBOARD = 0x030B,

    /// <summary>
    /// The WM_ASKCBFORMATNAME message is sent to the clipboard owner by a clipboard viewer window to request the name of a CF_OWNERDISPLAY clipboard format.
    /// </summary>
    ASKCBFORMATNAME = 0x030C,

    /// <summary>
    /// The WM_CHANGECBCHAIN message is sent to the first window in the clipboard viewer chain when a window is being removed from the chain.
    /// </summary>
    CHANGECBCHAIN = 0x030D,

    /// <summary>
    /// The WM_HSCROLLCLIPBOARD message is sent to the clipboard owner by a clipboard viewer window. This occurs when the clipboard contains data in the CF_OWNERDISPLAY format and an event occurs in the clipboard viewer's horizontal scroll bar. The owner should scroll the clipboard image and update the scroll bar values.
    /// </summary>
    HSCROLLCLIPBOARD = 0x030E,

    /// <summary>
    /// This message informs a window that it is about to receive the keyboard focus, giving the window the opportunity to realize its logical palette when it receives the focus.
    /// </summary>
    QUERYNEWPALETTE = 0x030F,

    /// <summary>
    /// The WM_PALETTEISCHANGING message informs applications that an application is going to realize its logical palette.
    /// </summary>
    PALETTEISCHANGING = 0x0310,

    /// <summary>
    /// This message is sent by the OS to all top-level and overlapped windows after the window with the keyboard focus realizes its logical palette.
    /// This message enables windows that do not have the keyboard focus to realize their logical palettes and update their client areas.
    /// </summary>
    PALETTECHANGED = 0x0311,

    /// <summary>
    /// The WM_HOTKEY message is posted when the user presses a hot key registered by the RegisterHotKey function. The message is placed at the top of the message queue associated with the thread that registered the hot key.
    /// </summary>
    HOTKEY = 0x0312,

    /// <summary>
    /// The WM_PRINT message is sent to a window to request that it draw itself in the specified device context, most commonly in a printer device context.
    /// </summary>
    PRINT = 0x0317,

    /// <summary>
    /// The WM_PRINTCLIENT message is sent to a window to request that it draw its client area in the specified device context, most commonly in a printer device context.
    /// </summary>
    PRINTCLIENT = 0x0318,

    /// <summary>
    /// The WM_APPCOMMAND message notifies a window that the user generated an application command event, for example, by clicking an application command button using the mouse or typing an application command key on the keyboard.
    /// </summary>
    APPCOMMAND = 0x0319,

    /// <summary>
    /// The WM_THEMECHANGED message is broadcast to every window following a theme change event. Examples of theme change events are the activation of a theme, the deactivation of a theme, or a transition from one theme to another.
    /// </summary>
    THEMECHANGED = 0x031A,

    /// <summary>
    /// Sent when the contents of the clipboard have changed.
    /// </summary>
    CLIPBOARDUPDATE = 0x031D,

    /// <summary>
    /// The system will send a window the WM_DWMCOMPOSITIONCHANGED message to indicate that the availability of desktop composition has changed.
    /// </summary>
    DWMCOMPOSITIONCHANGED = 0x031E,

    /// <summary>
    /// WM_DWMNCRENDERINGCHANGED is called when the non-client area rendering status of a window has changed. Only windows that have set the flag DWM_BLURBEHIND.fTransitionOnMaximized to true will get this message.
    /// </summary>
    DWMNCRENDERINGCHANGED = 0x031F,

    /// <summary>
    /// Sent to all top-level windows when the colorization color has changed.
    /// </summary>
    DWMCOLORIZATIONCOLORCHANGED = 0x0320,

    /// <summary>
    /// WM_DWMWINDOWMAXIMIZEDCHANGE will let you know when a DWM composed window is maximized. You also have to register for this message as well. You'd have other windowd go opaque when this message is sent.
    /// </summary>
    DWMWINDOWMAXIMIZEDCHANGE = 0x0321,

    /// <summary>
    /// Sent to request extended title bar information. A window receives this message through its WindowProc function.
    /// </summary>
    GETTITLEBARINFOEX = 0x033F,

    HANDHELDFIRST = 0x0358,
    HANDHELDLAST = 0x035F,
    AFXFIRST = 0x0360,
    AFXLAST = 0x037F,
    PENWINFIRST = 0x0380,
    PENWINLAST = 0x038F,

    /// <summary>
    /// The WM_APP constant is used by applications to help define private messages, usually of the form WM_APP+X, where X is an integer value.
    /// </summary>
    APP = 0x8000,

    /// <summary>
    /// The WM_USER constant is used by applications to help define private messages for use by private window classes, usually of the form WM_USER+X, where X is an integer value.
    /// </summary>
    USER = 0x0400,

    /// <summary>
    /// An application sends the WM_CPL_LAUNCH message to Windows Control Panel to request that a Control Panel application be started.
    /// </summary>
    CPL_LAUNCH = USER + 0x1000,

    /// <summary>
    /// The WM_CPL_LAUNCHED message is sent when a Control Panel application, started by the WM_CPL_LAUNCH message, has closed. The WM_CPL_LAUNCHED message is sent to the window identified by the wParam parameter of the WM_CPL_LAUNCH message that started the application.
    /// </summary>
    CPL_LAUNCHED = USER + 0x1001,

    /// <summary>
    /// WM_SYSTIMER is a well-known yet still undocumented message. Windows uses WM_SYSTIMER for internal actions like scrolling.
    /// </summary>
    SYSTIMER = 0x118,
  }

  /// <summary>
  ///  System-wide parameter - Used in SystemParametersInfo function
  /// </summary>
  public enum SPI : uint
  {
    /// <summary>
    /// Determines whether the warning beeper is on.
    /// The pvParam parameter must point to a BOOL variable that receives TRUE if the beeper is on, or FALSE if it is off.
    /// </summary>
    GETBEEP = 0x0001,

    /// <summary>
    /// Turns the warning beeper on or off. The uiParam parameter specifies TRUE for on, or FALSE for off.
    /// </summary>
    SETBEEP = 0x0002,

    /// <summary>
    /// Retrieves the two mouse threshold values and the mouse speed.
    /// </summary>
    GETMOUSE = 0x0003,

    /// <summary>
    /// Sets the two mouse threshold values and the mouse speed.
    /// </summary>
    SETMOUSE = 0x0004,

    /// <summary>
    /// Retrieves the border multiplier factor that determines the width of a window's sizing border.
    /// The pvParam parameter must point to an integer variable that receives this value.
    /// </summary>
    GETBORDER = 0x0005,

    /// <summary>
    /// Sets the border multiplier factor that determines the width of a window's sizing border.
    /// The uiParam parameter specifies the new value.
    /// </summary>
    SETBORDER = 0x0006,

    /// <summary>
    /// Retrieves the keyboard repeat-speed setting, which is a value in the range from 0 (approximately 2.5 repetitions per second)
    /// through 31 (approximately 30 repetitions per second). The actual repeat rates are hardware-dependent and may vary from
    /// a linear scale by as much as 20%. The pvParam parameter must point to a DWORD variable that receives the setting
    /// </summary>
    GETKEYBOARDSPEED = 0x000A,

    /// <summary>
    /// Sets the keyboard repeat-speed setting. The uiParam parameter must specify a value in the range from 0
    /// (approximately 2.5 repetitions per second) through 31 (approximately 30 repetitions per second).
    /// The actual repeat rates are hardware-dependent and may vary from a linear scale by as much as 20%.
    /// If uiParam is greater than 31, the parameter is set to 31.
    /// </summary>
    SETKEYBOARDSPEED = 0x000B,

    /// <summary>
    /// Not implemented.
    /// </summary>
    LANGDRIVER = 0x000C,

    /// <summary>
    /// Sets or retrieves the width, in pixels, of an icon cell. The system uses this rectangle to arrange icons in large icon view.
    /// To set this value, set uiParam to the new value and set pvParam to null. You cannot set this value to less than SM_CXICON.
    /// To retrieve this value, pvParam must point to an integer that receives the current value.
    /// </summary>
    ICONHORIZONTALSPACING = 0x000D,

    /// <summary>
    /// Retrieves the screen saver time-out value, in seconds. The pvParam parameter must point to an integer variable that receives the value.
    /// </summary>
    GETSCREENSAVETIMEOUT = 0x000E,

    /// <summary>
    /// Sets the screen saver time-out value to the value of the uiParam parameter. This value is the amount of time, in seconds,
    /// that the system must be idle before the screen saver activates.
    /// </summary>
    SETSCREENSAVETIMEOUT = 0x000F,

    /// <summary>
    /// Determines whether screen saving is enabled. The pvParam parameter must point to a bool variable that receives TRUE
    /// if screen saving is enabled, or FALSE otherwise.
    /// </summary>
    GETSCREENSAVEACTIVE = 0x0010,

    /// <summary>
    /// Sets the state of the screen saver. The uiParam parameter specifies TRUE to activate screen saving, or FALSE to deactivate it.
    /// </summary>
    SETSCREENSAVEACTIVE = 0x0011,

    /// <summary>
    /// Retrieves the current granularity value of the desktop sizing grid. The pvParam parameter must point to an integer variable
    /// that receives the granularity.
    /// </summary>
    GETGRIDGRANULARITY = 0x0012,

    /// <summary>
    /// Sets the granularity of the desktop sizing grid to the value of the uiParam parameter.
    /// </summary>
    SETGRIDGRANULARITY = 0x0013,

    /// <summary>
    /// Sets the desktop wallpaper. The value of the pvParam parameter determines the new wallpaper. To specify a wallpaper bitmap,
    /// set pvParam to point to a null-terminated string containing the name of a bitmap file. Setting pvParam to "" removes the wallpaper.
    /// Setting pvParam to SETWALLPAPER_DEFAULT or null reverts to the default wallpaper.
    /// </summary>
    SETDESKWALLPAPER = 0x0014,

    /// <summary>
    /// Sets the current desktop pattern by causing Windows to read the Pattern= setting from the WIN.INI file.
    /// </summary>
    SETDESKPATTERN = 0x0015,

    /// <summary>
    /// Retrieves the keyboard repeat-delay setting, which is a value in the range from 0 (approximately 250 ms delay) through 3
    /// (approximately 1 second delay). The actual delay associated with each value may vary depending on the hardware. The pvParam parameter must point to an integer variable that receives the setting.
    /// </summary>
    GETKEYBOARDDELAY = 0x0016,

    /// <summary>
    /// Sets the keyboard repeat-delay setting. The uiParam parameter must specify 0, 1, 2, or 3, where zero sets the shortest delay
    /// (approximately 250 ms) and 3 sets the longest delay (approximately 1 second). The actual delay associated with each value may
    /// vary depending on the hardware.
    /// </summary>
    SETKEYBOARDDELAY = 0x0017,

    /// <summary>
    /// Sets or retrieves the height, in pixels, of an icon cell.
    /// To set this value, set uiParam to the new value and set pvParam to null. You cannot set this value to less than SM_CYICON.
    /// To retrieve this value, pvParam must point to an integer that receives the current value.
    /// </summary>
    ICONVERTICALSPACING = 0x0018,

    /// <summary>
    /// Determines whether icon-title wrapping is enabled. The pvParam parameter must point to a bool variable that receives TRUE
    /// if enabled, or FALSE otherwise.
    /// </summary>
    GETICONTITLEWRAP = 0x0019,

    /// <summary>
    /// Turns icon-title wrapping on or off. The uiParam parameter specifies TRUE for on, or FALSE for off.
    /// </summary>
    SETICONTITLEWRAP = 0x001A,

    /// <summary>
    /// Determines whether pop-up menus are left-aligned or right-aligned, relative to the corresponding menu-bar item.
    /// The pvParam parameter must point to a bool variable that receives TRUE if left-aligned, or FALSE otherwise.
    /// </summary>
    GETMENUDROPALIGNMENT = 0x001B,

    /// <summary>
    /// Sets the alignment value of pop-up menus. The uiParam parameter specifies TRUE for right alignment, or FALSE for left alignment.
    /// </summary>
    SETMENUDROPALIGNMENT = 0x001C,

    /// <summary>
    /// Sets the width of the double-click rectangle to the value of the uiParam parameter.
    /// The double-click rectangle is the rectangle within which the second click of a double-click must fall for it to be registered
    /// as a double-click.
    /// To retrieve the width of the double-click rectangle, call GetSystemMetrics with the SM_CXDOUBLECLK flag.
    /// </summary>
    SETDOUBLECLKWIDTH = 0x001D,

    /// <summary>
    /// Sets the height of the double-click rectangle to the value of the uiParam parameter.
    /// The double-click rectangle is the rectangle within which the second click of a double-click must fall for it to be registered
    /// as a double-click.
    /// To retrieve the height of the double-click rectangle, call GetSystemMetrics with the SM_CYDOUBLECLK flag.
    /// </summary>
    SETDOUBLECLKHEIGHT = 0x001E,

    /// <summary>
    /// Retrieves the logical font information for the current icon-title font. The uiParam parameter specifies the size of a LOGFONT structure,
    /// and the pvParam parameter must point to the LOGFONT structure to fill in.
    /// </summary>
    GETICONTITLELOGFONT = 0x001F,

    /// <summary>
    /// Sets the double-click time for the mouse to the value of the uiParam parameter. The double-click time is the maximum number
    /// of milliseconds that can occur between the first and second clicks of a double-click. You can also call the SetDoubleClickTime
    /// function to set the double-click time. To get the current double-click time, call the GetDoubleClickTime function.
    /// </summary>
    SETDOUBLECLICKTIME = 0x0020,

    /// <summary>
    /// Swaps or restores the meaning of the left and right mouse buttons. The uiParam parameter specifies TRUE to swap the meanings
    /// of the buttons, or FALSE to restore their original meanings.
    /// </summary>
    SETMOUSEBUTTONSWAP = 0x0021,

    /// <summary>
    /// Sets the font that is used for icon titles. The uiParam parameter specifies the size of a LOGFONT structure,
    /// and the pvParam parameter must point to a LOGFONT structure.
    /// </summary>
    SETICONTITLELOGFONT = 0x0022,

    /// <summary>
    /// This flag is obsolete. Previous versions of the system use this flag to determine whether ALT+TAB fast task switching is enabled.
    /// For Windows 95, Windows 98, and Windows NT version 4.0 and later, fast task switching is always enabled.
    /// </summary>
    GETFASTTASKSWITCH = 0x0023,

    /// <summary>
    /// This flag is obsolete. Previous versions of the system use this flag to enable or disable ALT+TAB fast task switching.
    /// For Windows 95, Windows 98, and Windows NT version 4.0 and later, fast task switching is always enabled.
    /// </summary>
    SETFASTTASKSWITCH = 0x0024,

    //#if(WINVER >= 0x0400)
    /// <summary>
    /// Sets dragging of full windows either on or off. The uiParam parameter specifies TRUE for on, or FALSE for off.
    /// Windows 95:  This flag is supported only if Windows Plus! is installed. See GETWINDOWSEXTENSION.
    /// </summary>
    SETDRAGFULLWINDOWS = 0x0025,

    /// <summary>
    /// Determines whether dragging of full windows is enabled. The pvParam parameter must point to a BOOL variable that receives TRUE
    /// if enabled, or FALSE otherwise.
    /// Windows 95:  This flag is supported only if Windows Plus! is installed. See GETWINDOWSEXTENSION.
    /// </summary>
    GETDRAGFULLWINDOWS = 0x0026,

    /// <summary>
    /// Retrieves the metrics associated with the nonclient area of nonminimized windows. The pvParam parameter must point
    /// to a NONCLIENTMETRICS structure that receives the information. Set the cbSize member of this structure and the uiParam parameter
    /// to sizeof(NONCLIENTMETRICS).
    /// </summary>
    GETNONCLIENTMETRICS = 0x0029,

    /// <summary>
    /// Sets the metrics associated with the nonclient area of nonminimized windows. The pvParam parameter must point
    /// to a NONCLIENTMETRICS structure that contains the new parameters. Set the cbSize member of this structure
    /// and the uiParam parameter to sizeof(NONCLIENTMETRICS). Also, the lfHeight member of the LOGFONT structure must be a negative value.
    /// </summary>
    SETNONCLIENTMETRICS = 0x002A,

    /// <summary>
    /// Retrieves the metrics associated with minimized windows. The pvParam parameter must point to a MINIMIZEDMETRICS structure
    /// that receives the information. Set the cbSize member of this structure and the uiParam parameter to sizeof(MINIMIZEDMETRICS).
    /// </summary>
    GETMINIMIZEDMETRICS = 0x002B,

    /// <summary>
    /// Sets the metrics associated with minimized windows. The pvParam parameter must point to a MINIMIZEDMETRICS structure
    /// that contains the new parameters. Set the cbSize member of this structure and the uiParam parameter to sizeof(MINIMIZEDMETRICS).
    /// </summary>
    SETMINIMIZEDMETRICS = 0x002C,

    /// <summary>
    /// Retrieves the metrics associated with icons. The pvParam parameter must point to an ICONMETRICS structure that receives
    /// the information. Set the cbSize member of this structure and the uiParam parameter to sizeof(ICONMETRICS).
    /// </summary>
    GETICONMETRICS = 0x002D,

    /// <summary>
    /// Sets the metrics associated with icons. The pvParam parameter must point to an ICONMETRICS structure that contains
    /// the new parameters. Set the cbSize member of this structure and the uiParam parameter to sizeof(ICONMETRICS).
    /// </summary>
    SETICONMETRICS = 0x002E,

    /// <summary>
    /// Sets the size of the work area. The work area is the portion of the screen not obscured by the system taskbar
    /// or by application desktop toolbars. The pvParam parameter is a pointer to a RECT structure that specifies the new work area rectangle,
    /// expressed in virtual screen coordinates. In a system with multiple display monitors, the function sets the work area
    /// of the monitor that contains the specified rectangle.
    /// </summary>
    SETWORKAREA = 0x002F,

    /// <summary>
    /// Retrieves the size of the work area on the primary display monitor. The work area is the portion of the screen not obscured
    /// by the system taskbar or by application desktop toolbars. The pvParam parameter must point to a RECT structure that receives
    /// the coordinates of the work area, expressed in virtual screen coordinates.
    /// To get the work area of a monitor other than the primary display monitor, call the GetMonitorInfo function.
    /// </summary>
    GETWORKAREA = 0x0030,

    /// <summary>
    /// Windows Me/98/95:  Pen windows is being loaded or unloaded. The uiParam parameter is TRUE when loading and FALSE
    /// when unloading pen windows. The pvParam parameter is null.
    /// </summary>
    SETPENWINDOWS = 0x0031,

    /// <summary>
    /// Retrieves information about the HighContrast accessibility feature. The pvParam parameter must point to a HIGHCONTRAST structure
    /// that receives the information. Set the cbSize member of this structure and the uiParam parameter to sizeof(HIGHCONTRAST).
    /// For a general discussion, see remarks.
    /// Windows NT:  This value is not supported.
    /// </summary>
    /// <remarks>
    /// There is a difference between the High Contrast color scheme and the High Contrast Mode. The High Contrast color scheme changes
    /// the system colors to colors that have obvious contrast; you switch to this color scheme by using the Display Options in the control panel.
    /// The High Contrast Mode, which uses GETHIGHCONTRAST and SETHIGHCONTRAST, advises applications to modify their appearance
    /// for visually-impaired users. It involves such things as audible warning to users and customized color scheme
    /// (using the Accessibility Options in the control panel). For more information, see HIGHCONTRAST on MSDN.
    /// For more information on general accessibility features, see Accessibility on MSDN.
    /// </remarks>
    GETHIGHCONTRAST = 0x0042,

    /// <summary>
    /// Sets the parameters of the HighContrast accessibility feature. The pvParam parameter must point to a HIGHCONTRAST structure
    /// that contains the new parameters. Set the cbSize member of this structure and the uiParam parameter to sizeof(HIGHCONTRAST).
    /// Windows NT:  This value is not supported.
    /// </summary>
    SETHIGHCONTRAST = 0x0043,

    /// <summary>
    /// Determines whether the user relies on the keyboard instead of the mouse, and wants applications to display keyboard interfaces
    /// that would otherwise be hidden. The pvParam parameter must point to a BOOL variable that receives TRUE
    /// if the user relies on the keyboard; or FALSE otherwise.
    /// Windows NT:  This value is not supported.
    /// </summary>
    GETKEYBOARDPREF = 0x0044,

    /// <summary>
    /// Sets the keyboard preference. The uiParam parameter specifies TRUE if the user relies on the keyboard instead of the mouse,
    /// and wants applications to display keyboard interfaces that would otherwise be hidden; uiParam is FALSE otherwise.
    /// Windows NT:  This value is not supported.
    /// </summary>
    SETKEYBOARDPREF = 0x0045,

    /// <summary>
    /// Determines whether a screen reviewer utility is running. A screen reviewer utility directs textual information to an output device,
    /// such as a speech synthesizer or Braille display. When this flag is set, an application should provide textual information
    /// in situations where it would otherwise present the information graphically.
    /// The pvParam parameter is a pointer to a BOOL variable that receives TRUE if a screen reviewer utility is running, or FALSE otherwise.
    /// Windows NT:  This value is not supported.
    /// </summary>
    GETSCREENREADER = 0x0046,

    /// <summary>
    /// Determines whether a screen review utility is running. The uiParam parameter specifies TRUE for on, or FALSE for off.
    /// Windows NT:  This value is not supported.
    /// </summary>
    SETSCREENREADER = 0x0047,

    /// <summary>
    /// Retrieves the animation effects associated with user actions. The pvParam parameter must point to an ANIMATIONINFO structure
    /// that receives the information. Set the cbSize member of this structure and the uiParam parameter to sizeof(ANIMATIONINFO).
    /// </summary>
    GETANIMATION = 0x0048,

    /// <summary>
    /// Sets the animation effects associated with user actions. The pvParam parameter must point to an ANIMATIONINFO structure
    /// that contains the new parameters. Set the cbSize member of this structure and the uiParam parameter to sizeof(ANIMATIONINFO).
    /// </summary>
    SETANIMATION = 0x0049,

    /// <summary>
    /// Determines whether the font smoothing feature is enabled. This feature uses font antialiasing to make font curves appear smoother
    /// by painting pixels at different gray levels.
    /// The pvParam parameter must point to a BOOL variable that receives TRUE if the feature is enabled, or FALSE if it is not.
    /// Windows 95:  This flag is supported only if Windows Plus! is installed. See GETWINDOWSEXTENSION.
    /// </summary>
    GETFONTSMOOTHING = 0x004A,

    /// <summary>
    /// Enables or disables the font smoothing feature, which uses font antialiasing to make font curves appear smoother
    /// by painting pixels at different gray levels.
    /// To enable the feature, set the uiParam parameter to TRUE. To disable the feature, set uiParam to FALSE.
    /// Windows 95:  This flag is supported only if Windows Plus! is installed. See GETWINDOWSEXTENSION.
    /// </summary>
    SETFONTSMOOTHING = 0x004B,

    /// <summary>
    /// Sets the width, in pixels, of the rectangle used to detect the start of a drag operation. Set uiParam to the new value.
    /// To retrieve the drag width, call GetSystemMetrics with the SM_CXDRAG flag.
    /// </summary>
    SETDRAGWIDTH = 0x004C,

    /// <summary>
    /// Sets the height, in pixels, of the rectangle used to detect the start of a drag operation. Set uiParam to the new value.
    /// To retrieve the drag height, call GetSystemMetrics with the SM_CYDRAG flag.
    /// </summary>
    SETDRAGHEIGHT = 0x004D,

    /// <summary>
    /// Used internally; applications should not use this value.
    /// </summary>
    SETHANDHELD = 0x004E,

    /// <summary>
    /// Retrieves the time-out value for the low-power phase of screen saving. The pvParam parameter must point to an integer variable
    /// that receives the value. This flag is supported for 32-bit applications only.
    /// Windows NT, Windows Me/98:  This flag is supported for 16-bit and 32-bit applications.
    /// Windows 95:  This flag is supported for 16-bit applications only.
    /// </summary>
    GETLOWPOWERTIMEOUT = 0x004F,

    /// <summary>
    /// Retrieves the time-out value for the power-off phase of screen saving. The pvParam parameter must point to an integer variable
    /// that receives the value. This flag is supported for 32-bit applications only.
    /// Windows NT, Windows Me/98:  This flag is supported for 16-bit and 32-bit applications.
    /// Windows 95:  This flag is supported for 16-bit applications only.
    /// </summary>
    GETPOWEROFFTIMEOUT = 0x0050,

    /// <summary>
    /// Sets the time-out value, in seconds, for the low-power phase of screen saving. The uiParam parameter specifies the new value.
    /// The pvParam parameter must be null. This flag is supported for 32-bit applications only.
    /// Windows NT, Windows Me/98:  This flag is supported for 16-bit and 32-bit applications.
    /// Windows 95:  This flag is supported for 16-bit applications only.
    /// </summary>
    SETLOWPOWERTIMEOUT = 0x0051,

    /// <summary>
    /// Sets the time-out value, in seconds, for the power-off phase of screen saving. The uiParam parameter specifies the new value.
    /// The pvParam parameter must be null. This flag is supported for 32-bit applications only.
    /// Windows NT, Windows Me/98:  This flag is supported for 16-bit and 32-bit applications.
    /// Windows 95:  This flag is supported for 16-bit applications only.
    /// </summary>
    SETPOWEROFFTIMEOUT = 0x0052,

    /// <summary>
    /// Determines whether the low-power phase of screen saving is enabled. The pvParam parameter must point to a BOOL variable
    /// that receives TRUE if enabled, or FALSE if disabled. This flag is supported for 32-bit applications only.
    /// Windows NT, Windows Me/98:  This flag is supported for 16-bit and 32-bit applications.
    /// Windows 95:  This flag is supported for 16-bit applications only.
    /// </summary>
    GETLOWPOWERACTIVE = 0x0053,

    /// <summary>
    /// Determines whether the power-off phase of screen saving is enabled. The pvParam parameter must point to a BOOL variable
    /// that receives TRUE if enabled, or FALSE if disabled. This flag is supported for 32-bit applications only.
    /// Windows NT, Windows Me/98:  This flag is supported for 16-bit and 32-bit applications.
    /// Windows 95:  This flag is supported for 16-bit applications only.
    /// </summary>
    GETPOWEROFFACTIVE = 0x0054,

    /// <summary>
    /// Activates or deactivates the low-power phase of screen saving. Set uiParam to 1 to activate, or zero to deactivate.
    /// The pvParam parameter must be null. This flag is supported for 32-bit applications only.
    /// Windows NT, Windows Me/98:  This flag is supported for 16-bit and 32-bit applications.
    /// Windows 95:  This flag is supported for 16-bit applications only.
    /// </summary>
    SETLOWPOWERACTIVE = 0x0055,

    /// <summary>
    /// Activates or deactivates the power-off phase of screen saving. Set uiParam to 1 to activate, or zero to deactivate.
    /// The pvParam parameter must be null. This flag is supported for 32-bit applications only.
    /// Windows NT, Windows Me/98:  This flag is supported for 16-bit and 32-bit applications.
    /// Windows 95:  This flag is supported for 16-bit applications only.
    /// </summary>
    SETPOWEROFFACTIVE = 0x0056,

    /// <summary>
    /// Reloads the system cursors. Set the uiParam parameter to zero and the pvParam parameter to null.
    /// </summary>
    SETCURSORS = 0x0057,

    /// <summary>
    /// Reloads the system icons. Set the uiParam parameter to zero and the pvParam parameter to null.
    /// </summary>
    SETICONS = 0x0058,

    /// <summary>
    /// Retrieves the input locale identifier for the system default input language. The pvParam parameter must point
    /// to an HKL variable that receives this value. For more information, see Languages, Locales, and Keyboard Layouts on MSDN.
    /// </summary>
    GETDEFAULTINPUTLANG = 0x0059,

    /// <summary>
    /// Sets the default input language for the system shell and applications. The specified language must be displayable
    /// using the current system character set. The pvParam parameter must point to an HKL variable that contains
    /// the input locale identifier for the default language. For more information, see Languages, Locales, and Keyboard Layouts on MSDN.
    /// </summary>
    SETDEFAULTINPUTLANG = 0x005A,

    /// <summary>
    /// Sets the hot key set for switching between input languages. The uiParam and pvParam parameters are not used.
    /// The value sets the shortcut keys in the keyboard property sheets by reading the registry again. The registry must be set before this flag is used. the path in the registry is \HKEY_CURRENT_USER\keyboard layout\toggle. Valid values are "1" = ALT+SHIFT, "2" = CTRL+SHIFT, and "3" = none.
    /// </summary>
    SETLANGTOGGLE = 0x005B,

    /// <summary>
    /// Windows 95:  Determines whether the Windows extension, Windows Plus!, is installed. Set the uiParam parameter to 1.
    /// The pvParam parameter is not used. The function returns TRUE if the extension is installed, or FALSE if it is not.
    /// </summary>
    GETWINDOWSEXTENSION = 0x005C,

    /// <summary>
    /// Enables or disables the Mouse Trails feature, which improves the visibility of mouse cursor movements by briefly showing
    /// a trail of cursors and quickly erasing them.
    /// To disable the feature, set the uiParam parameter to zero or 1. To enable the feature, set uiParam to a value greater than 1
    /// to indicate the number of cursors drawn in the trail.
    /// Windows 2000/NT:  This value is not supported.
    /// </summary>
    SETMOUSETRAILS = 0x005D,

    /// <summary>
    /// Determines whether the Mouse Trails feature is enabled. This feature improves the visibility of mouse cursor movements
    /// by briefly showing a trail of cursors and quickly erasing them.
    /// The pvParam parameter must point to an integer variable that receives a value. If the value is zero or 1, the feature is disabled.
    /// If the value is greater than 1, the feature is enabled and the value indicates the number of cursors drawn in the trail.
    /// The uiParam parameter is not used.
    /// Windows 2000/NT:  This value is not supported.
    /// </summary>
    GETMOUSETRAILS = 0x005E,

    /// <summary>
    /// Windows Me/98:  Used internally; applications should not use this flag.
    /// </summary>
    SETSCREENSAVERRUNNING = 0x0061,

    /// <summary>
    /// Same as SETSCREENSAVERRUNNING.
    /// </summary>
    SCREENSAVERRUNNING = SETSCREENSAVERRUNNING,

    //#endif /* WINVER >= 0x0400 */

    /// <summary>
    /// Retrieves information about the FilterKeys accessibility feature. The pvParam parameter must point to a FILTERKEYS structure
    /// that receives the information. Set the cbSize member of this structure and the uiParam parameter to sizeof(FILTERKEYS).
    /// </summary>
    GETFILTERKEYS = 0x0032,

    /// <summary>
    /// Sets the parameters of the FilterKeys accessibility feature. The pvParam parameter must point to a FILTERKEYS structure
    /// that contains the new parameters. Set the cbSize member of this structure and the uiParam parameter to sizeof(FILTERKEYS).
    /// </summary>
    SETFILTERKEYS = 0x0033,

    /// <summary>
    /// Retrieves information about the ToggleKeys accessibility feature. The pvParam parameter must point to a TOGGLEKEYS structure
    /// that receives the information. Set the cbSize member of this structure and the uiParam parameter to sizeof(TOGGLEKEYS).
    /// </summary>
    GETTOGGLEKEYS = 0x0034,

    /// <summary>
    /// Sets the parameters of the ToggleKeys accessibility feature. The pvParam parameter must point to a TOGGLEKEYS structure
    /// that contains the new parameters. Set the cbSize member of this structure and the uiParam parameter to sizeof(TOGGLEKEYS).
    /// </summary>
    SETTOGGLEKEYS = 0x0035,

    /// <summary>
    /// Retrieves information about the MouseKeys accessibility feature. The pvParam parameter must point to a MOUSEKEYS structure
    /// that receives the information. Set the cbSize member of this structure and the uiParam parameter to sizeof(MOUSEKEYS).
    /// </summary>
    GETMOUSEKEYS = 0x0036,

    /// <summary>
    /// Sets the parameters of the MouseKeys accessibility feature. The pvParam parameter must point to a MOUSEKEYS structure
    /// that contains the new parameters. Set the cbSize member of this structure and the uiParam parameter to sizeof(MOUSEKEYS).
    /// </summary>
    SETMOUSEKEYS = 0x0037,

    /// <summary>
    /// Determines whether the Show Sounds accessibility flag is on or off. If it is on, the user requires an application
    /// to present information visually in situations where it would otherwise present the information only in audible form.
    /// The pvParam parameter must point to a BOOL variable that receives TRUE if the feature is on, or FALSE if it is off.
    /// Using this value is equivalent to calling GetSystemMetrics (SM_SHOWSOUNDS). That is the recommended call.
    /// </summary>
    GETSHOWSOUNDS = 0x0038,

    /// <summary>
    /// Sets the parameters of the SoundSentry accessibility feature. The pvParam parameter must point to a SOUNDSENTRY structure
    /// that contains the new parameters. Set the cbSize member of this structure and the uiParam parameter to sizeof(SOUNDSENTRY).
    /// </summary>
    SETSHOWSOUNDS = 0x0039,

    /// <summary>
    /// Retrieves information about the StickyKeys accessibility feature. The pvParam parameter must point to a STICKYKEYS structure
    /// that receives the information. Set the cbSize member of this structure and the uiParam parameter to sizeof(STICKYKEYS).
    /// </summary>
    GETSTICKYKEYS = 0x003A,

    /// <summary>
    /// Sets the parameters of the StickyKeys accessibility feature. The pvParam parameter must point to a STICKYKEYS structure
    /// that contains the new parameters. Set the cbSize member of this structure and the uiParam parameter to sizeof(STICKYKEYS).
    /// </summary>
    SETSTICKYKEYS = 0x003B,

    /// <summary>
    /// Retrieves information about the time-out period associated with the accessibility features. The pvParam parameter must point
    /// to an ACCESSTIMEOUT structure that receives the information. Set the cbSize member of this structure and the uiParam parameter
    /// to sizeof(ACCESSTIMEOUT).
    /// </summary>
    GETACCESSTIMEOUT = 0x003C,

    /// <summary>
    /// Sets the time-out period associated with the accessibility features. The pvParam parameter must point to an ACCESSTIMEOUT
    /// structure that contains the new parameters. Set the cbSize member of this structure and the uiParam parameter to sizeof(ACCESSTIMEOUT).
    /// </summary>
    SETACCESSTIMEOUT = 0x003D,

    //#if(WINVER >= 0x0400)
    /// <summary>
    /// Windows Me/98/95:  Retrieves information about the SerialKeys accessibility feature. The pvParam parameter must point
    /// to a SERIALKEYS structure that receives the information. Set the cbSize member of this structure and the uiParam parameter
    /// to sizeof(SERIALKEYS).
    /// Windows Server 2003, Windows XP/2000/NT:  Not supported. The user controls this feature through the control panel.
    /// </summary>
    GETSERIALKEYS = 0x003E,

    /// <summary>
    /// Windows Me/98/95:  Sets the parameters of the SerialKeys accessibility feature. The pvParam parameter must point
    /// to a SERIALKEYS structure that contains the new parameters. Set the cbSize member of this structure and the uiParam parameter
    /// to sizeof(SERIALKEYS).
    /// Windows Server 2003, Windows XP/2000/NT:  Not supported. The user controls this feature through the control panel.
    /// </summary>
    SETSERIALKEYS = 0x003F,

    //#endif /* WINVER >= 0x0400 */

    /// <summary>
    /// Retrieves information about the SoundSentry accessibility feature. The pvParam parameter must point to a SOUNDSENTRY structure
    /// that receives the information. Set the cbSize member of this structure and the uiParam parameter to sizeof(SOUNDSENTRY).
    /// </summary>
    GETSOUNDSENTRY = 0x0040,

    /// <summary>
    /// Sets the parameters of the SoundSentry accessibility feature. The pvParam parameter must point to a SOUNDSENTRY structure
    /// that contains the new parameters. Set the cbSize member of this structure and the uiParam parameter to sizeof(SOUNDSENTRY).
    /// </summary>
    SETSOUNDSENTRY = 0x0041,

    //#if(_WIN32_WINNT >= 0x0400)
    /// <summary>
    /// Determines whether the snap-to-default-button feature is enabled. If enabled, the mouse cursor automatically moves
    /// to the default button, such as OK or Apply, of a dialog box. The pvParam parameter must point to a BOOL variable
    /// that receives TRUE if the feature is on, or FALSE if it is off.
    /// Windows 95:  Not supported.
    /// </summary>
    GETSNAPTODEFBUTTON = 0x005F,

    /// <summary>
    /// Enables or disables the snap-to-default-button feature. If enabled, the mouse cursor automatically moves to the default button,
    /// such as OK or Apply, of a dialog box. Set the uiParam parameter to TRUE to enable the feature, or FALSE to disable it.
    /// Applications should use the ShowWindow function when displaying a dialog box so the dialog manager can position the mouse cursor.
    /// Windows 95:  Not supported.
    /// </summary>
    SETSNAPTODEFBUTTON = 0x0060,

    //#endif /* _WIN32_WINNT >= 0x0400 */

    //#if (_WIN32_WINNT >= 0x0400) || (_WIN32_WINDOWS > 0x0400)
    /// <summary>
    /// Retrieves the width, in pixels, of the rectangle within which the mouse pointer has to stay for TrackMouseEvent
    /// to generate a WM_MOUSEHOVER message. The pvParam parameter must point to a UINT variable that receives the width.
    /// Windows 95:  Not supported.
    /// </summary>
    GETMOUSEHOVERWIDTH = 0x0062,

    /// <summary>
    /// Retrieves the width, in pixels, of the rectangle within which the mouse pointer has to stay for TrackMouseEvent
    /// to generate a WM_MOUSEHOVER message. The pvParam parameter must point to a UINT variable that receives the width.
    /// Windows 95:  Not supported.
    /// </summary>
    SETMOUSEHOVERWIDTH = 0x0063,

    /// <summary>
    /// Retrieves the height, in pixels, of the rectangle within which the mouse pointer has to stay for TrackMouseEvent
    /// to generate a WM_MOUSEHOVER message. The pvParam parameter must point to a UINT variable that receives the height.
    /// Windows 95:  Not supported.
    /// </summary>
    GETMOUSEHOVERHEIGHT = 0x0064,

    /// <summary>
    /// Sets the height, in pixels, of the rectangle within which the mouse pointer has to stay for TrackMouseEvent
    /// to generate a WM_MOUSEHOVER message. Set the uiParam parameter to the new height.
    /// Windows 95:  Not supported.
    /// </summary>
    SETMOUSEHOVERHEIGHT = 0x0065,

    /// <summary>
    /// Retrieves the time, in milliseconds, that the mouse pointer has to stay in the hover rectangle for TrackMouseEvent
    /// to generate a WM_MOUSEHOVER message. The pvParam parameter must point to a UINT variable that receives the time.
    /// Windows 95:  Not supported.
    /// </summary>
    GETMOUSEHOVERTIME = 0x0066,

    /// <summary>
    /// Sets the time, in milliseconds, that the mouse pointer has to stay in the hover rectangle for TrackMouseEvent
    /// to generate a WM_MOUSEHOVER message. This is used only if you pass HOVER_DEFAULT in the dwHoverTime parameter in the call to TrackMouseEvent. Set the uiParam parameter to the new time.
    /// Windows 95:  Not supported.
    /// </summary>
    SETMOUSEHOVERTIME = 0x0067,

    /// <summary>
    /// Retrieves the number of lines to scroll when the mouse wheel is rotated. The pvParam parameter must point
    /// to a UINT variable that receives the number of lines. The default value is 3.
    /// Windows 95:  Not supported.
    /// </summary>
    GETWHEELSCROLLLINES = 0x0068,

    /// <summary>
    /// Sets the number of lines to scroll when the mouse wheel is rotated. The number of lines is set from the uiParam parameter.
    /// The number of lines is the suggested number of lines to scroll when the mouse wheel is rolled without using modifier keys.
    /// If the number is 0, then no scrolling should occur. If the number of lines to scroll is greater than the number of lines viewable,
    /// and in particular if it is WHEEL_PAGESCROLL (#defined as UINT_MAX), the scroll operation should be interpreted
    /// as clicking once in the page down or page up regions of the scroll bar.
    /// Windows 95:  Not supported.
    /// </summary>
    SETWHEELSCROLLLINES = 0x0069,

    /// <summary>
    /// Retrieves the time, in milliseconds, that the system waits before displaying a shortcut menu when the mouse cursor is
    /// over a submenu item. The pvParam parameter must point to a DWORD variable that receives the time of the delay.
    /// Windows 95:  Not supported.
    /// </summary>
    GETMENUSHOWDELAY = 0x006A,

    /// <summary>
    /// Sets uiParam to the time, in milliseconds, that the system waits before displaying a shortcut menu when the mouse cursor is
    /// over a submenu item.
    /// Windows 95:  Not supported.
    /// </summary>
    SETMENUSHOWDELAY = 0x006B,

    /// <summary>
    /// Determines whether the IME status window is visible (on a per-user basis). The pvParam parameter must point to a BOOL variable
    /// that receives TRUE if the status window is visible, or FALSE if it is not.
    /// Windows NT, Windows 95:  This value is not supported.
    /// </summary>
    GETSHOWIMEUI = 0x006E,

    /// <summary>
    /// Sets whether the IME status window is visible or not on a per-user basis. The uiParam parameter specifies TRUE for on or FALSE for off.
    /// Windows NT, Windows 95:  This value is not supported.
    /// </summary>
    SETSHOWIMEUI = 0x006F,

    //#endif

    //#if(WINVER >= 0x0500)
    /// <summary>
    /// Retrieves the current mouse speed. The mouse speed determines how far the pointer will move based on the distance the mouse moves.
    /// The pvParam parameter must point to an integer that receives a value which ranges between 1 (slowest) and 20 (fastest).
    /// A value of 10 is the default. The value can be set by an end user using the mouse control panel application or
    /// by an application using SETMOUSESPEED.
    /// Windows NT, Windows 95:  This value is not supported.
    /// </summary>
    GETMOUSESPEED = 0x0070,

    /// <summary>
    /// Sets the current mouse speed. The pvParam parameter is an integer between 1 (slowest) and 20 (fastest). A value of 10 is the default.
    /// This value is typically set using the mouse control panel application.
    /// Windows NT, Windows 95:  This value is not supported.
    /// </summary>
    SETMOUSESPEED = 0x0071,

    /// <summary>
    /// Determines whether a screen saver is currently running on the window station of the calling process.
    /// The pvParam parameter must point to a BOOL variable that receives TRUE if a screen saver is currently running, or FALSE otherwise.
    /// Note that only the interactive window station, "WinSta0", can have a screen saver running.
    /// Windows NT, Windows 95:  This value is not supported.
    /// </summary>
    GETSCREENSAVERRUNNING = 0x0072,

    /// <summary>
    /// Retrieves the full path of the bitmap file for the desktop wallpaper. The pvParam parameter must point to a buffer
    /// that receives a null-terminated path string. Set the uiParam parameter to the size, in characters, of the pvParam buffer. The returned string will not exceed MAX_PATH characters. If there is no desktop wallpaper, the returned string is empty.
    /// Windows NT, Windows Me/98/95:  This value is not supported.
    /// </summary>
    GETDESKWALLPAPER = 0x0073,

    //#endif /* WINVER >= 0x0500 */

    //#if(WINVER >= 0x0500)
    /// <summary>
    /// Determines whether active window tracking (activating the window the mouse is on) is on or off. The pvParam parameter must point
    /// to a BOOL variable that receives TRUE for on, or FALSE for off.
    /// Windows NT, Windows 95:  This value is not supported.
    /// </summary>
    GETACTIVEWINDOWTRACKING = 0x1000,

    /// <summary>
    /// Sets active window tracking (activating the window the mouse is on) either on or off. Set pvParam to TRUE for on or FALSE for off.
    /// Windows NT, Windows 95:  This value is not supported.
    /// </summary>
    SETACTIVEWINDOWTRACKING = 0x1001,

    /// <summary>
    /// Determines whether the menu animation feature is enabled. This global switch must be on to enable menu animation effects.
    /// The pvParam parameter must point to a BOOL variable that receives TRUE if animation is enabled and FALSE if it is disabled.
    /// If animation is enabled, GETMENUFADE indicates whether menus use fade or slide animation.
    /// Windows NT, Windows 95:  This value is not supported.
    /// </summary>
    GETMENUANIMATION = 0x1002,

    /// <summary>
    /// Enables or disables menu animation. This global switch must be on for any menu animation to occur.
    /// The pvParam parameter is a BOOL variable; set pvParam to TRUE to enable animation and FALSE to disable animation.
    /// If animation is enabled, GETMENUFADE indicates whether menus use fade or slide animation.
    /// Windows NT, Windows 95:  This value is not supported.
    /// </summary>
    SETMENUANIMATION = 0x1003,

    /// <summary>
    /// Determines whether the slide-open effect for combo boxes is enabled. The pvParam parameter must point to a BOOL variable
    /// that receives TRUE for enabled, or FALSE for disabled.
    /// Windows NT, Windows 95:  This value is not supported.
    /// </summary>
    GETCOMBOBOXANIMATION = 0x1004,

    /// <summary>
    /// Enables or disables the slide-open effect for combo boxes. Set the pvParam parameter to TRUE to enable the gradient effect,
    /// or FALSE to disable it.
    /// Windows NT, Windows 95:  This value is not supported.
    /// </summary>
    SETCOMBOBOXANIMATION = 0x1005,

    /// <summary>
    /// Determines whether the smooth-scrolling effect for list boxes is enabled. The pvParam parameter must point to a BOOL variable
    /// that receives TRUE for enabled, or FALSE for disabled.
    /// Windows NT, Windows 95:  This value is not supported.
    /// </summary>
    GETLISTBOXSMOOTHSCROLLING = 0x1006,

    /// <summary>
    /// Enables or disables the smooth-scrolling effect for list boxes. Set the pvParam parameter to TRUE to enable the smooth-scrolling effect,
    /// or FALSE to disable it.
    /// Windows NT, Windows 95:  This value is not supported.
    /// </summary>
    SETLISTBOXSMOOTHSCROLLING = 0x1007,

    /// <summary>
    /// Determines whether the gradient effect for window title bars is enabled. The pvParam parameter must point to a BOOL variable
    /// that receives TRUE for enabled, or FALSE for disabled. For more information about the gradient effect, see the GetSysColor function.
    /// Windows NT, Windows 95:  This value is not supported.
    /// </summary>
    GETGRADIENTCAPTIONS = 0x1008,

    /// <summary>
    /// Enables or disables the gradient effect for window title bars. Set the pvParam parameter to TRUE to enable it, or FALSE to disable it.
    /// The gradient effect is possible only if the system has a color depth of more than 256 colors. For more information about
    /// the gradient effect, see the GetSysColor function.
    /// Windows NT, Windows 95:  This value is not supported.
    /// </summary>
    SETGRADIENTCAPTIONS = 0x1009,

    /// <summary>
    /// Determines whether menu access keys are always underlined. The pvParam parameter must point to a BOOL variable that receives TRUE
    /// if menu access keys are always underlined, and FALSE if they are underlined only when the menu is activated by the keyboard.
    /// Windows NT, Windows 95:  This value is not supported.
    /// </summary>
    GETKEYBOARDCUES = 0x100A,

    /// <summary>
    /// Sets the underlining of menu access key letters. The pvParam parameter is a BOOL variable. Set pvParam to TRUE to always underline menu
    /// access keys, or FALSE to underline menu access keys only when the menu is activated from the keyboard.
    /// Windows NT, Windows 95:  This value is not supported.
    /// </summary>
    SETKEYBOARDCUES = 0x100B,

    /// <summary>
    /// Same as GETKEYBOARDCUES.
    /// </summary>
    GETMENUUNDERLINES = GETKEYBOARDCUES,

    /// <summary>
    /// Same as SETKEYBOARDCUES.
    /// </summary>
    SETMENUUNDERLINES = SETKEYBOARDCUES,

    /// <summary>
    /// Determines whether windows activated through active window tracking will be brought to the top. The pvParam parameter must point
    /// to a BOOL variable that receives TRUE for on, or FALSE for off.
    /// Windows NT, Windows 95:  This value is not supported.
    /// </summary>
    GETACTIVEWNDTRKZORDER = 0x100C,

    /// <summary>
    /// Determines whether or not windows activated through active window tracking should be brought to the top. Set pvParam to TRUE
    /// for on or FALSE for off.
    /// Windows NT, Windows 95:  This value is not supported.
    /// </summary>
    SETACTIVEWNDTRKZORDER = 0x100D,

    /// <summary>
    /// Determines whether hot tracking of user-interface elements, such as menu names on menu bars, is enabled. The pvParam parameter
    /// must point to a BOOL variable that receives TRUE for enabled, or FALSE for disabled.
    /// Hot tracking means that when the cursor moves over an item, it is highlighted but not selected. You can query this value to decide
    /// whether to use hot tracking in the user interface of your application.
    /// Windows NT, Windows 95:  This value is not supported.
    /// </summary>
    GETHOTTRACKING = 0x100E,

    /// <summary>
    /// Enables or disables hot tracking of user-interface elements such as menu names on menu bars. Set the pvParam parameter to TRUE
    /// to enable it, or FALSE to disable it.
    /// Hot-tracking means that when the cursor moves over an item, it is highlighted but not selected.
    /// Windows NT, Windows 95:  This value is not supported.
    /// </summary>
    SETHOTTRACKING = 0x100F,

    /// <summary>
    /// Determines whether menu fade animation is enabled. The pvParam parameter must point to a BOOL variable that receives TRUE
    /// when fade animation is enabled and FALSE when it is disabled. If fade animation is disabled, menus use slide animation.
    /// This flag is ignored unless menu animation is enabled, which you can do using the SETMENUANIMATION flag.
    /// For more information, see AnimateWindow.
    /// Windows NT, Windows Me/98/95:  This value is not supported.
    /// </summary>
    GETMENUFADE = 0x1012,

    /// <summary>
    /// Enables or disables menu fade animation. Set pvParam to TRUE to enable the menu fade effect or FALSE to disable it.
    /// If fade animation is disabled, menus use slide animation. he The menu fade effect is possible only if the system
    /// has a color depth of more than 256 colors. This flag is ignored unless MENUANIMATION is also set. For more information,
    /// see AnimateWindow.
    /// Windows NT, Windows Me/98/95:  This value is not supported.
    /// </summary>
    SETMENUFADE = 0x1013,

    /// <summary>
    /// Determines whether the selection fade effect is enabled. The pvParam parameter must point to a BOOL variable that receives TRUE
    /// if enabled or FALSE if disabled.
    /// The selection fade effect causes the menu item selected by the user to remain on the screen briefly while fading out
    /// after the menu is dismissed.
    /// Windows NT, Windows Me/98/95:  This value is not supported.
    /// </summary>
    GETSELECTIONFADE = 0x1014,

    /// <summary>
    /// Set pvParam to TRUE to enable the selection fade effect or FALSE to disable it.
    /// The selection fade effect causes the menu item selected by the user to remain on the screen briefly while fading out
    /// after the menu is dismissed. The selection fade effect is possible only if the system has a color depth of more than 256 colors.
    /// Windows NT, Windows Me/98/95:  This value is not supported.
    /// </summary>
    SETSELECTIONFADE = 0x1015,

    /// <summary>
    /// Determines whether ToolTip animation is enabled. The pvParam parameter must point to a BOOL variable that receives TRUE
    /// if enabled or FALSE if disabled. If ToolTip animation is enabled, GETTOOLTIPFADE indicates whether ToolTips use fade or slide animation.
    /// Windows NT, Windows Me/98/95:  This value is not supported.
    /// </summary>
    GETTOOLTIPANIMATION = 0x1016,

    /// <summary>
    /// Set pvParam to TRUE to enable ToolTip animation or FALSE to disable it. If enabled, you can use SETTOOLTIPFADE
    /// to specify fade or slide animation.
    /// Windows NT, Windows Me/98/95:  This value is not supported.
    /// </summary>
    SETTOOLTIPANIMATION = 0x1017,

    /// <summary>
    /// If SETTOOLTIPANIMATION is enabled, GETTOOLTIPFADE indicates whether ToolTip animation uses a fade effect or a slide effect.
    ///  The pvParam parameter must point to a BOOL variable that receives TRUE for fade animation or FALSE for slide animation.
    ///  For more information on slide and fade effects, see AnimateWindow.
    /// Windows NT, Windows Me/98/95:  This value is not supported.
    /// </summary>
    GETTOOLTIPFADE = 0x1018,

    /// <summary>
    /// If the SETTOOLTIPANIMATION flag is enabled, use SETTOOLTIPFADE to indicate whether ToolTip animation uses a fade effect
    /// or a slide effect. Set pvParam to TRUE for fade animation or FALSE for slide animation. The tooltip fade effect is possible only
    /// if the system has a color depth of more than 256 colors. For more information on the slide and fade effects,
    /// see the AnimateWindow function.
    /// Windows NT, Windows Me/98/95:  This value is not supported.
    /// </summary>
    SETTOOLTIPFADE = 0x1019,

    /// <summary>
    /// Determines whether the cursor has a shadow around it. The pvParam parameter must point to a BOOL variable that receives TRUE
    /// if the shadow is enabled, FALSE if it is disabled. This effect appears only if the system has a color depth of more than 256 colors.
    /// Windows NT, Windows Me/98/95:  This value is not supported.
    /// </summary>
    GETCURSORSHADOW = 0x101A,

    /// <summary>
    /// Enables or disables a shadow around the cursor. The pvParam parameter is a BOOL variable. Set pvParam to TRUE to enable the shadow
    /// or FALSE to disable the shadow. This effect appears only if the system has a color depth of more than 256 colors.
    /// Windows NT, Windows Me/98/95:  This value is not supported.
    /// </summary>
    SETCURSORSHADOW = 0x101B,

    //#if(_WIN32_WINNT >= 0x0501)
    /// <summary>
    /// Retrieves the state of the Mouse Sonar feature. The pvParam parameter must point to a BOOL variable that receives TRUE
    /// if enabled or FALSE otherwise. For more information, see About Mouse Input on MSDN.
    /// Windows 2000/NT, Windows 98/95:  This value is not supported.
    /// </summary>
    GETMOUSESONAR = 0x101C,

    /// <summary>
    /// Turns the Sonar accessibility feature on or off. This feature briefly shows several concentric circles around the mouse pointer
    /// when the user presses and releases the CTRL key. The pvParam parameter specifies TRUE for on and FALSE for off. The default is off.
    /// For more information, see About Mouse Input.
    /// Windows 2000/NT, Windows 98/95:  This value is not supported.
    /// </summary>
    SETMOUSESONAR = 0x101D,

    /// <summary>
    /// Retrieves the state of the Mouse ClickLock feature. The pvParam parameter must point to a BOOL variable that receives TRUE
    /// if enabled, or FALSE otherwise. For more information, see About Mouse Input.
    /// Windows 2000/NT, Windows 98/95:  This value is not supported.
    /// </summary>
    GETMOUSECLICKLOCK = 0x101E,

    /// <summary>
    /// Turns the Mouse ClickLock accessibility feature on or off. This feature temporarily locks down the primary mouse button
    /// when that button is clicked and held down for the time specified by SETMOUSECLICKLOCKTIME. The uiParam parameter specifies
    /// TRUE for on,
    /// or FALSE for off. The default is off. For more information, see Remarks and About Mouse Input on MSDN.
    /// Windows 2000/NT, Windows 98/95:  This value is not supported.
    /// </summary>
    SETMOUSECLICKLOCK = 0x101F,

    /// <summary>
    /// Retrieves the state of the Mouse Vanish feature. The pvParam parameter must point to a BOOL variable that receives TRUE
    /// if enabled or FALSE otherwise. For more information, see About Mouse Input on MSDN.
    /// Windows 2000/NT, Windows 98/95:  This value is not supported.
    /// </summary>
    GETMOUSEVANISH = 0x1020,

    /// <summary>
    /// Turns the Vanish feature on or off. This feature hides the mouse pointer when the user types; the pointer reappears
    /// when the user moves the mouse. The pvParam parameter specifies TRUE for on and FALSE for off. The default is off.
    /// For more information, see About Mouse Input on MSDN.
    /// Windows 2000/NT, Windows 98/95:  This value is not supported.
    /// </summary>
    SETMOUSEVANISH = 0x1021,

    /// <summary>
    /// Determines whether native User menus have flat menu appearance. The pvParam parameter must point to a BOOL variable
    /// that returns TRUE if the flat menu appearance is set, or FALSE otherwise.
    /// Windows 2000/NT, Windows Me/98/95:  This value is not supported.
    /// </summary>
    GETFLATMENU = 0x1022,

    /// <summary>
    /// Enables or disables flat menu appearance for native User menus. Set pvParam to TRUE to enable flat menu appearance
    /// or FALSE to disable it.
    /// When enabled, the menu bar uses COLOR_MENUBAR for the menubar background, COLOR_MENU for the menu-popup background, COLOR_MENUHILIGHT
    /// for the fill of the current menu selection, and COLOR_HILIGHT for the outline of the current menu selection.
    /// If disabled, menus are drawn using the same metrics and colors as in Windows 2000 and earlier.
    /// Windows 2000/NT, Windows Me/98/95:  This value is not supported.
    /// </summary>
    SETFLATMENU = 0x1023,

    /// <summary>
    /// Determines whether the drop shadow effect is enabled. The pvParam parameter must point to a BOOL variable that returns TRUE
    /// if enabled or FALSE if disabled.
    /// Windows 2000/NT, Windows Me/98/95:  This value is not supported.
    /// </summary>
    GETDROPSHADOW = 0x1024,

    /// <summary>
    /// Enables or disables the drop shadow effect. Set pvParam to TRUE to enable the drop shadow effect or FALSE to disable it.
    /// You must also have CS_DROPSHADOW in the window class style.
    /// Windows 2000/NT, Windows Me/98/95:  This value is not supported.
    /// </summary>
    SETDROPSHADOW = 0x1025,

    /// <summary>
    /// Retrieves a BOOL indicating whether an application can reset the screensaver's timer by calling the SendInput function
    /// to simulate keyboard or mouse input. The pvParam parameter must point to a BOOL variable that receives TRUE
    /// if the simulated input will be blocked, or FALSE otherwise.
    /// </summary>
    GETBLOCKSENDINPUTRESETS = 0x1026,

    /// <summary>
    /// Determines whether an application can reset the screensaver's timer by calling the SendInput function to simulate keyboard
    /// or mouse input. The uiParam parameter specifies TRUE if the screensaver will not be deactivated by simulated input,
    /// or FALSE if the screensaver will be deactivated by simulated input.
    /// </summary>
    SETBLOCKSENDINPUTRESETS = 0x1027,

    //#endif /* _WIN32_WINNT >= 0x0501 */

    /// <summary>
    /// Determines whether UI effects are enabled or disabled. The pvParam parameter must point to a BOOL variable that receives TRUE
    /// if all UI effects are enabled, or FALSE if they are disabled.
    /// Windows NT, Windows Me/98/95:  This value is not supported.
    /// </summary>
    GETUIEFFECTS = 0x103E,

    /// <summary>
    /// Enables or disables UI effects. Set the pvParam parameter to TRUE to enable all UI effects or FALSE to disable all UI effects.
    /// Windows NT, Windows Me/98/95:  This value is not supported.
    /// </summary>
    SETUIEFFECTS = 0x103F,

    /// <summary>
    /// Retrieves the amount of time following user input, in milliseconds, during which the system will not allow applications
    /// to force themselves into the foreground. The pvParam parameter must point to a DWORD variable that receives the time.
    /// Windows NT, Windows 95:  This value is not supported.
    /// </summary>
    GETFOREGROUNDLOCKTIMEOUT = 0x2000,

    /// <summary>
    /// Sets the amount of time following user input, in milliseconds, during which the system does not allow applications
    /// to force themselves into the foreground. Set pvParam to the new timeout value.
    /// The calling thread must be able to change the foreground window, otherwise the call fails.
    /// Windows NT, Windows 95:  This value is not supported.
    /// </summary>
    SETFOREGROUNDLOCKTIMEOUT = 0x2001,

    /// <summary>
    /// Retrieves the active window tracking delay, in milliseconds. The pvParam parameter must point to a DWORD variable
    /// that receives the time.
    /// Windows NT, Windows 95:  This value is not supported.
    /// </summary>
    GETACTIVEWNDTRKTIMEOUT = 0x2002,

    /// <summary>
    /// Sets the active window tracking delay. Set pvParam to the number of milliseconds to delay before activating the window
    /// under the mouse pointer.
    /// Windows NT, Windows 95:  This value is not supported.
    /// </summary>
    SETACTIVEWNDTRKTIMEOUT = 0x2003,

    /// <summary>
    /// Retrieves the number of times SetForegroundWindow will flash the taskbar button when rejecting a foreground switch request.
    /// The pvParam parameter must point to a DWORD variable that receives the value.
    /// Windows NT, Windows 95:  This value is not supported.
    /// </summary>
    GETFOREGROUNDFLASHCOUNT = 0x2004,

    /// <summary>
    /// Sets the number of times SetForegroundWindow will flash the taskbar button when rejecting a foreground switch request.
    /// Set pvParam to the number of times to flash.
    /// Windows NT, Windows 95:  This value is not supported.
    /// </summary>
    SETFOREGROUNDFLASHCOUNT = 0x2005,

    /// <summary>
    /// Retrieves the caret width in edit controls, in pixels. The pvParam parameter must point to a DWORD that receives this value.
    /// Windows NT, Windows Me/98/95:  This value is not supported.
    /// </summary>
    GETCARETWIDTH = 0x2006,

    /// <summary>
    /// Sets the caret width in edit controls. Set pvParam to the desired width, in pixels. The default and minimum value is 1.
    /// Windows NT, Windows Me/98/95:  This value is not supported.
    /// </summary>
    SETCARETWIDTH = 0x2007,

    //#if(_WIN32_WINNT >= 0x0501)
    /// <summary>
    /// Retrieves the time delay before the primary mouse button is locked. The pvParam parameter must point to DWORD that receives
    /// the time delay. This is only enabled if SETMOUSECLICKLOCK is set to TRUE. For more information, see About Mouse Input on MSDN.
    /// Windows 2000/NT, Windows 98/95:  This value is not supported.
    /// </summary>
    GETMOUSECLICKLOCKTIME = 0x2008,

    /// <summary>
    /// Turns the Mouse ClickLock accessibility feature on or off. This feature temporarily locks down the primary mouse button
    /// when that button is clicked and held down for the time specified by SETMOUSECLICKLOCKTIME. The uiParam parameter
    /// specifies TRUE for on, or FALSE for off. The default is off. For more information, see Remarks and About Mouse Input on MSDN.
    /// Windows 2000/NT, Windows 98/95:  This value is not supported.
    /// </summary>
    SETMOUSECLICKLOCKTIME = 0x2009,

    /// <summary>
    /// Retrieves the type of font smoothing. The pvParam parameter must point to a UINT that receives the information.
    /// Windows 2000/NT, Windows Me/98/95:  This value is not supported.
    /// </summary>
    GETFONTSMOOTHINGTYPE = 0x200A,

    /// <summary>
    /// Sets the font smoothing type. The pvParam parameter points to a UINT that contains either FE_FONTSMOOTHINGSTANDARD,
    /// if standard anti-aliasing is used, or FE_FONTSMOOTHINGCLEARTYPE, if ClearType is used. The default is FE_FONTSMOOTHINGSTANDARD.
    /// When using this option, the fWinIni parameter must be set to SPIF_SENDWININICHANGE | SPIF_UPDATEINIFILE; otherwise,
    /// SystemParametersInfo fails.
    /// </summary>
    SETFONTSMOOTHINGTYPE = 0x200B,

    /// <summary>
    /// Retrieves a contrast value that is used in ClearType™ smoothing. The pvParam parameter must point to a UINT
    /// that receives the information.
    /// Windows 2000/NT, Windows Me/98/95:  This value is not supported.
    /// </summary>
    GETFONTSMOOTHINGCONTRAST = 0x200C,

    /// <summary>
    /// Sets the contrast value used in ClearType smoothing. The pvParam parameter points to a UINT that holds the contrast value.
    /// Valid contrast values are from 1000 to 2200. The default value is 1400.
    /// When using this option, the fWinIni parameter must be set to SPIF_SENDWININICHANGE | SPIF_UPDATEINIFILE; otherwise,
    /// SystemParametersInfo fails.
    /// SETFONTSMOOTHINGTYPE must also be set to FE_FONTSMOOTHINGCLEARTYPE.
    /// Windows 2000/NT, Windows Me/98/95:  This value is not supported.
    /// </summary>
    SETFONTSMOOTHINGCONTRAST = 0x200D,

    /// <summary>
    /// Retrieves the width, in pixels, of the left and right edges of the focus rectangle drawn with DrawFocusRect.
    /// The pvParam parameter must point to a UINT.
    /// Windows 2000/NT, Windows Me/98/95:  This value is not supported.
    /// </summary>
    GETFOCUSBORDERWIDTH = 0x200E,

    /// <summary>
    /// Sets the height of the left and right edges of the focus rectangle drawn with DrawFocusRect to the value of the pvParam parameter.
    /// Windows 2000/NT, Windows Me/98/95:  This value is not supported.
    /// </summary>
    SETFOCUSBORDERWIDTH = 0x200F,

    /// <summary>
    /// Retrieves the height, in pixels, of the top and bottom edges of the focus rectangle drawn with DrawFocusRect.
    /// The pvParam parameter must point to a UINT.
    /// Windows 2000/NT, Windows Me/98/95:  This value is not supported.
    /// </summary>
    GETFOCUSBORDERHEIGHT = 0x2010,

    /// <summary>
    /// Sets the height of the top and bottom edges of the focus rectangle drawn with DrawFocusRect to the value of the pvParam parameter.
    /// Windows 2000/NT, Windows Me/98/95:  This value is not supported.
    /// </summary>
    SETFOCUSBORDERHEIGHT = 0x2011,

    /// <summary>
    /// Not implemented.
    /// </summary>
    GETFONTSMOOTHINGORIENTATION = 0x2012,

    /// <summary>
    /// Not implemented.
    /// </summary>
    SETFONTSMOOTHINGORIENTATION = 0x2013,
  }

  public enum TCM : uint
  {
    FIRST = 0x1300,
    GETIMAGELIST = (FIRST + 2),
    SETIMAGELIST = (FIRST + 3),
    GETITEMCOUNT = (FIRST + 4),
    GETITEMA = (FIRST + 5),
    GETITEMW = (FIRST + 60),
    SETITEMA = (FIRST + 6),
    SETITEMW = (FIRST + 61),
    INSERTITEMA = (FIRST + 7),
    INSERTITEMW = (FIRST + 62),
    DELETEITEM = (FIRST + 8),
    DELETEALLITEMS = (FIRST + 9),
    GETITEMRECT = (FIRST + 10),
    GETCURSEL = (FIRST + 11),
    SETCURSEL = (FIRST + 12),
    HITTEST = (FIRST + 13),
    SETITEMEXTRA = (FIRST + 14),
    ADJUSTRECT = (FIRST + 40),
    SETITEMSIZE = (FIRST + 41),
    REMOVEIMAGE = (FIRST + 42),
    SETPADDING = (FIRST + 43),
    GETROWCOUNT = (FIRST + 44),
    GETCURFOCUS = (FIRST + 47),
    SETCURFOCUS = (FIRST + 48),
    SETMINTABWIDTH = (FIRST + 49),
    DESELECTALL = (FIRST + 50),
    HIGHLIGHTITEM = (FIRST + 51),
    SETEXTENDEDSTYLE = (FIRST + 52), // optional wParam == mask
    GETEXTENDEDSTYLE = (FIRST + 53),
  }

  // ShowWindow flags.
  public enum SW : uint
  {
    HIDE = 0,
    SHOWNORMAL = 1,
    NORMAL = 1,
    SHOWMINIMIZED = 2,
    SHOWMAXIMIZED = 3,
    MAXIMIZE = 3,
    SHOWNOACTIVATE = 4,
    SHOW = 5,
    MINIMIZE = 6,
    SHOWMINNOACTIVE = 7,
    SHOWNA = 8,
    RESTORE = 9,
    SHOWDEFAULT = 10,
    FORCEMINIMIZE = 11,
    MAX = 11,
  }

  // Scrollbar message values.
  public enum SB : ushort
  {
    LINEUP = 0,
    LINELEFT = 0,
    LINEDOWN = 1,
    LINERIGHT = 1,
    PAGEUP = 2,
    PAGELEFT = 2,
    PAGEDOWN = 3,
    PAGERIGHT = 3,
    THUMBPOSITION = 4,
    THUMBTRACK = 5,
    TOP = 6,
    LEFT = 6,
    BOTTOM = 7,
    RIGHT = 7,
    ENDSCROLL = 8
  }

  // UpDown window class message values.
  public enum UDM : uint
  {
    SETRANGE = WM.USER + 101,
    GETRANGE = WM.USER + 102,
    SETPOS = WM.USER + 103,
    GETPOS = WM.USER + 104,
    SETBUDDY = WM.USER + 105,
    GETBUDDY = WM.USER + 106,
    SETACCEL = WM.USER + 107,
    GETACCEL = WM.USER + 108,
    SETBASE = WM.USER + 109,
    GETBASE = WM.USER + 110,
    SETRANGE32 = WM.USER + 111,
    GETRANGE32 = WM.USER + 112, // wParam & lParam are LPINT

    //SETUNICODEFORMAT    CCM_SETUNICODEFORMAT
    //GETUNICODEFORMAT    CCM_GETUNICODEFORMAT
    SETPOS32 = WM.USER + 113,

    GETPOS32 = WM.USER + 114,
  }

  // Window styles
  public enum WS : uint
  {
    POPUP = 0x80000000,
    BORDER = 0x00800000,

    EX_TOPMOST = 0x00000008,
    EX_LAYERED = 0x00080000,
    EX_NOACTIVATE = 0x08000000,
  }

  // Header item.
  public enum HDI : uint
  {
    WIDTH = 0x0001,
    HEIGHT = WIDTH,
    TEXT = 0x0002,
    FORMAT = 0x0004,
    LPARAM = 0x0008,
    BITMAP = 0x0010,
    IMAGE = 0x0020,
    DI_SETITEM = 0x0040,
    ORDER = 0x0080,
    FILTER = 0x0100
  }

  // Header format.
  public enum HDF : uint
  {
    LEFT = 0x0000,
    RIGHT = 0x0001,
    CENTER = 0x0002,
    JUSTIFYMASK = 0x0003,
    RTLREADING = 0x0004,
    OWNERDRAW = 0x8000,
    STRING = 0x4000,
    BITMAP = 0x2000,
    BITMAP_ON_RIGHT = 0x1000,
    IMAGE = 0x0800
  }

  // Notification messages.
  public enum NM : uint
  {
    FIRST = 0,
    CUSTOMDRAW = unchecked(FIRST + (uint)-12)
  }

  // Custom draw return flags.
  public enum CDRF : uint
  {
    DODEFAULT = 0x00000000,
    NEWFONT = 0x00000002,
    SKIPDEFAULT = 0x00000004,
    DOERASE = 0x00000008,
    NOTIFYPOSTPAINT = 0x00000010,
    NOTIFYITEMDRAW = 0x00000020,
    NOTIFYSUBITEMDRAW = NOTIFYITEMDRAW
  }

  // Custom draw draw state.
  public enum CDDS : uint
  {
    PREPAINT = 0x00000001,
    POSTPAINT = 0x00000002,
    PREERASE = 0x00000003,
    POSTERASE = 0x00000004,
    ITEM = 0x00010000,
    ITEMPREPAINT = (ITEM | PREPAINT),
    ITEMPOSTPAINT = (ITEM | POSTPAINT),
    ITEMPREERASE = (ITEM | PREERASE),
    ITEMPOSTERASE = (ITEM | POSTERASE),
    SUBITEM = 0x00020000
  }

  // Custom draw item state.
  public enum CDIS : uint
  {
    SELECTED = 0x0201,
    DEFAULT = 0x0200
  }

  // Reflected messages.
  public enum OCM : uint
  {
    BASE = WM.USER + 0x1c00,
    COMMAND = BASE + 0x0111,
    CTLCOLORBTN = BASE + 0x0135,
    CTLCOLOREDIT = BASE + 0x0133,
    CTLCOLORDLG = BASE + 0x0136,
    CTLCOLORLISTBOX = BASE + 0x0134,
    CTLCOLORMSGBOX = BASE + 0x0132,
    CTLCOLORSCROLLBAR = BASE + 0x0137,
    CTLCOLORSTATIC = BASE + 0x0138,
    CTLCOLOR = BASE + 0x0019,
    DRAWITEM = BASE + 0x002B,
    MEASUREITEM = BASE + 0x002C,
    DELETEITEM = BASE + 0x002D,
    VKEYTOITEM = BASE + 0x002E,
    CHARTOITEM = BASE + 0x002F,
    COMPAREITEM = BASE + 0x0039,
    HSCROLL = BASE + 0x0114,
    VSCROLL = BASE + 0x0115,
    PARENTNOTIFY = BASE + 0x0210,
    NOTIFY = BASE + 0x004E
  }

  #endregion Constants

  /// <summary>
  /// Helper class for invoking Windows functionality directly.
  /// </summary>
  [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
  public class Win32
  {
    #region Constants

    public const int MAX_PATH = 260;

    // GetWindow() constants
    public const uint GW_HWNDFIRST = 0;

    public const uint GW_HWNDLAST = 1;
    public const uint GW_HWNDNEXT = 2;
    public const uint GW_HWNDPREV = 3;
    public const uint GW_OWNER = 4;
    public const uint GW_CHILD = 5;

    public const uint HC_ACTION = 0;
    public const uint WH_CALLWNDPROC = 4;

    public const uint GWL_WNDPROC = unchecked((uint)-4);
    public const uint GWL_EXSTYLE = unchecked((uint)-20);

    // Mouse message constants.
    public const uint MA_ACTIVATE = 1;

    public const uint MA_ACTIVATEANDEAT = 2;
    public const uint MA_NOACTIVATE = 3;
    public const uint MA_NOACTIVATEANDEAT = 4;

    // Listview message constants.
    public const uint LVM_FIRST = 0x1000;

    public const uint LVM_GETITEMRECT = (LVM_FIRST + 14);
    public const uint LVM_GETHEADER = (LVM_FIRST + 31);
    public const uint LVM_APPROXIMATEVIEWRECT = (LVM_FIRST + 64);

    public const uint LVIR_BOUNDS = 0;
    public const uint LVIR_ICON = 1;
    public const uint LVIR_LABEL = 2;

    private const Int32 HDF_SORTUP = 0x400;
    private const Int32 HDF_SORTDOWN = 0x200;

    private const Int32 HDM_GETITEM = 0x120b;
    private const Int32 HDM_SETITEM = 0x120c;

    // SetWindowPos constants.
    public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

    public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
    public static readonly IntPtr HWND_TOP = new IntPtr(0);

    public const uint SWP_NOSIZE = 0x0001;
    public const uint SWP_NOMOVE = 0x0002;
    public const uint SWP_NOZORDER = 0x0004;
    public const uint SWP_NOREDRAW = 0x0008;
    public const uint SWP_NOACTIVATE = 0x0010;
    public const uint SWP_FRAMECHANGED = 0x0020;  /* The frame changed: send WM_NCCALCSIZE */
    public const uint SWP_SHOWWINDOW = 0x0040;
    public const uint SWP_HIDEWINDOW = 0x0080;
    public const uint SWP_NOCOPYBITS = 0x0100;
    public const uint SWP_NOOWNERZORDER = 0x0200;  /* Don't do owner Z ordering */
    public const uint SWP_NOSENDCHANGING = 0x0400;  /* Don't send WM_WINDOWPOSCHANGING */

    public const Int32 ULW_COLORKEY = 0x00000001;
    public const Int32 ULW_ALPHA = 0x00000002;
    public const Int32 ULW_OPAQUE = 0x00000004;

    public const Int32 LWA_COLORKEY = 0x1;
    public const Int32 LWA_ALPHA = 0x2;

    public const byte AC_SRC_OVER = 0x00;
    public const byte AC_SRC_ALPHA = 0x01;

    public const uint RDW_INVALIDATE = 0x0001;
    public const uint RDW_INTERNALPAINT = 0x0002;
    public const uint RDW_ERASE = 0x0004;
    public const uint RDW_VALIDATE = 0x0008;
    public const uint RDW_NOINTERNALPAINT = 0x0010;
    public const uint RDW_NOERASE = 0x0020;
    public const uint RDW_NOCHILDREN = 0x0040;
    public const uint RDW_ALLCHILDREN = 0x0080;
    public const uint RDW_UPDATENOW = 0x0100;
    public const uint RDW_ERASENOW = 0x0200;
    public const uint RDW_FRAME = 0x0400;
    public const uint RDW_NOFRAME = 0x0800;

    // WM_SIZE constants
    public const uint SIZE_RESTORED = 0;

    public const uint SIZE_MINIMIZED = 1;
    public const uint SIZE_MAXIMIZED = 2;
    public const uint SIZE_MAXSHOW = 3;
    public const uint SIZE_MAXHIDE = 4;

    // Process and console related
    public const uint ATTACH_PARENT_PROCESS = 0xFFFFFFFF;

    public const uint STD_INPUT_HANDLE = 0xFFFFFFF6;
    public const uint STD_OUTPUT_HANDLE = 0xFFFFFFF5;
    public const uint STD_ERROR_HANDLE = 0xFFFFFFF4;
    public const uint DUPLICATE_SAME_ACCESS = 2;

    // Aero rendering stuff.
    public const uint DWM_TNP_RECTDESTINATION = 0x00000001;

    public const uint DWM_TNP_RECTSOURCE = 0x00000002;
    public const uint DWM_TNP_OPACITY = 0x00000004;
    public const uint DWM_TNP_VISIBLE = 0x00000008;
    public const uint DWM_TNP_SOURCECLIENTAREAONLY = 0x00000010;

    public const uint DWM_BB_ENABLE = 0x00000001;
    public const uint DWM_BB_BLURREGION = 0x00000002;
    public const uint DWM_BB_TRANSITIONONMAXIMIZED = 0x00000004;

    // Service Control stuff
    public const int ACCESS_TYPE_ALL = 0xf01ff;

    public const int ACCESS_TYPE_CHANGE_CONFIG = 2;
    public const int ACCESS_TYPE_ENUMERATE_DEPENDENTS = 8;
    public const int ACCESS_TYPE_INTERROGATE = 0x80;
    public const int ACCESS_TYPE_PAUSE_CONTINUE = 0x40;
    public const int ACCESS_TYPE_QUERY_CONFIG = 1;
    public const int ACCESS_TYPE_QUERY_STATUS = 4;
    public const int ACCESS_TYPE_START = 0x10;
    public const int ACCESS_TYPE_STOP = 0x20;
    public const int ACCESS_TYPE_DELETE = 0x00010000;
    public const int ACCESS_TYPE_USER_DEFINED_CONTROL = 0x100;
    public const int ERROR_CONTROL_CRITICAL = 3;
    public const int ERROR_CONTROL_IGNORE = 0;
    public const int ERROR_CONTROL_NORMAL = 1;
    public const int ERROR_CONTROL_SEVERE = 2;
    public const int SC_MANAGER_ALL = 0xf003f;
    public const int SC_MANAGER_CONNECT = 1;
    public const int SC_MANAGER_CREATE_SERVICE = 2;
    public const int SC_MANAGER_ENUMERATE_SERVICE = 4;
    public const int SC_MANAGER_LOCK = 8;
    public const int SC_MANAGER_MODIFY_BOOT_CONFIG = 0x20;
    public const int SC_MANAGER_QUERY_LOCK_STATUS = 0x10;
    public const int START_TYPE_AUTO = 2;
    public const int START_TYPE_BOOT = 0;
    public const int START_TYPE_DEMAND = 3;
    public const int START_TYPE_DISABLED = 4;
    public const int START_TYPE_SYSTEM = 1;
    public const int SERVICE_TYPE_ADAPTER = 4;
    public const int SERVICE_TYPE_ALL = 0x13f;
    public const int SERVICE_TYPE_DRIVER = 11;
    public const int SERVICE_TYPE_FILE_SYSTEM_DRIVER = 2;
    public const int SERVICE_TYPE_INTERACTIVE_PROCESS = 0x100;
    public const int SERVICE_TYPE_KERNEL_DRIVER = 1;
    public const int SERVICE_TYPE_RECOGNIZER_DRIVER = 8;
    public const int SERVICE_TYPE_WIN32 = 0x30;
    public const int SERVICE_TYPE_WIN32_OWN_PROCESS = 0x10;
    public const int SERVICE_TYPE_WIN32_SHARE_PROCESS = 0x20;

    public const uint STANDARD_RIGHTS_REQUIRED = 0x000F0000;
    public const uint SYNCHRONIZE = 0x00100000;
    public const uint EVENT_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0x3);
    public const uint EVENT_MODIFY_STATE = 0x0002;
    public const long ERROR_FILE_NOT_FOUND = 2L;

    public const int POLICY_ALL_ACCESS = 0x00F0FFF;

    // User impersonation
    public const int LOGON32_PROVIDER_DEFAULT = 0;

    public const int LOGON32_LOGON_INTERACTIVE = 2;
    public const int LOGON32_LOGON_SERVICE = 5;

    #endregion Constants

    public static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

    public static bool IsVistaOrHigher => Environment.OSVersion.Version >= new Version("6.0.0")
                                          && Environment.OSVersion.Platform == PlatformID.Win32NT;

    /// <summary>
    /// Flag indicating whether the architecture of the OS is 64-bit.
    /// </summary>
    private static bool? _is64BitOs;

    /// <summary>
    /// Gets a value indicating whether the architecture of the OS is 64-bit.
    /// </summary>
    public static bool Is64BitOs
    {
      get
      {
        if (!_is64BitOs.HasValue)
        {
          _is64BitOs = Environment.Is64BitOperatingSystem;
        }

        return _is64BitOs.Value;
      }
    }

    static Win32()
    {
      _is64BitOs = null;
    }

    #region Structures and data types

    public enum HRESULT : long
    {
      S_OK = 0x00000000,
      S_FALSE = 0x00000001,
      E_PENDING = 0x8000000A,
      E_FAIL = 0x80004005,
      E_OUTOFMEMORY = 0x8007000E,
      E_INVALIDARG = 0x80070057
    }

    [Flags]
    public enum FileMapProtection : uint
    {
      PageNone = 0,
      PageReadonly = 0x02,
      PageReadWrite = 0x04,
      PageWriteCopy = 0x08,
      PageExecuteRead = 0x20,
      PageExecuteReadWrite = 0x40,
      SectionCommit = 0x8000000,
      SectionImage = 0x1000000,
      SectionNoCache = 0x10000000,
      SectionReserve = 0x4000000,
    }

    public enum FileMapAccess : uint
    {
      FileMapCopy = 0x0001,
      FileMapWrite = 0x0002,
      FileMapRead = 0x0004,
      FileMapAllAccess = 0x001f,
      fileMapExecute = 0x0020,
    }

    /// <summary>
    /// Security enumeration from:
    /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dllproc/base/synchronization_object_security_and_access_rights.asp
    /// </summary>
    [Flags]
    public enum SyncObjectAccess : uint
    {
      DELETE = 0x00010000,
      READ_CONTROL = 0x00020000,
      WRITE_DAC = 0x00040000,
      WRITE_OWNER = 0x00080000,
      SYNCHRONIZE = 0x00100000,
      EVENT_ALL_ACCESS = 0x001F0003,
      EVENT_MODIFY_STATE = 0x00000002,
      MUTEX_ALL_ACCESS = 0x001F0001,
      MUTEX_MODIFY_STATE = 0x00000001,
      SEMAPHORE_ALL_ACCESS = 0x001F0003,
      SEMAPHORE_MODIFY_STATE = 0x00000002,
      TIMER_ALL_ACCESS = 0x001F0003,
      TIMER_MODIFY_STATE = 0x00000002,
      TIMER_QUERY_STATE = 0x00000001
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MINMAXINFO
    {
      public POINT ptReserved;
      public POINT ptMaxSize;       // Maximum size of the window.
      public POINT ptMaxPosition;   // Location of the window when maximized.
      public POINT ptMinTrackSize;
      public POINT ptMaxTrackSize;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct UDACCEL
    {
      public uint nSec;
      public uint nInc;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
      public int Left;
      public int Top;
      public int Right;
      public int Bottom;

      public int Width
      {
        get { return Right - Left; }
      }

      public int Height
      {
        get { return Bottom - Top; }
      }

      public System.Drawing.Point Location
      {
        get { return new System.Drawing.Point(Left, Top); }
      }

      public System.Drawing.Size Size
      {
        get { return new System.Drawing.Size(Right - Left, Bottom - Top); }
      }

      public void fillFromRectangle(System.Drawing.Rectangle rectangle)
      {
        Left = rectangle.Left;
        Top = rectangle.Top;
        Right = rectangle.Right;
        Bottom = rectangle.Bottom;
      }

      public RECT(int x, int y, int r, int b)
      {
        Left = x;
        Top = y;
        Right = r;
        Bottom = b;
      }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWPOS
    {
      public IntPtr hwnd;
      public IntPtr hwndAfter;
      public int x;
      public int y;
      public int cx;
      public int cy;
      public uint flags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NCCALCSIZE_PARAMS
    {
      public RECT rgc1;
      public RECT rgc2;
      public RECT rgc3;
      public IntPtr wndpos; // WINDOWPOS
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
      public Int32 x;
      public Int32 y;

      public POINT(Int32 x, Int32 y)
      {
        this.x = x; this.y = y;
      }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SIZE
    {
      public Int32 cx;
      public Int32 cy;

      public SIZE(Int32 cx, Int32 cy)
      {
        this.cx = cx; this.cy = cy;
      }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ARGB
    {
      public byte Blue;
      public byte Green;
      public byte Red;
      public byte Alpha;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BLENDFUNCTION
    {
      public byte BlendOp;
      public byte BlendFlags;
      public byte SourceConstantAlpha;
      public byte AlphaFormat;

      public BLENDFUNCTION(byte op, byte flags, byte constantAlpha, byte format)
      {
        BlendOp = op;
        BlendFlags = flags;
        SourceConstantAlpha = constantAlpha;
        AlphaFormat = format;
      }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LVCOLUMN
    {
      public Int32 mask;
      public Int32 cx;

      [MarshalAs(UnmanagedType.LPTStr)]
      public string pszText;

      public IntPtr hbm;
      public Int32 cchTextMax;
      public Int32 fmt;
      public Int32 iSubItem;
      public Int32 iImage;
      public Int32 iOrder;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BY_HANDLE_FILE_INFORMATION
    {
      public UInt32 FileAttributes;
      public System.Runtime.InteropServices.ComTypes.FILETIME CreationTime;
      public System.Runtime.InteropServices.ComTypes.FILETIME LastAccessTime;
      public System.Runtime.InteropServices.ComTypes.FILETIME LastWriteTime;
      public UInt32 VolumeSerialNumber;
      public UInt32 FileSizeHigh;
      public UInt32 FileSizeLow;
      public UInt32 NumberOfLinks;
      public UInt32 FileIndexHigh;
      public UInt32 FileIndexLow;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class DWM_THUMBNAIL_PROPERTIES
    {
      public uint dwFlags;
      public RECT rcDestination;
      public RECT rcSource;
      public byte opacity;

      [MarshalAs(UnmanagedType.Bool)]
      public bool fVisible;

      [MarshalAs(UnmanagedType.Bool)]
      public bool fSourceClientAreaOnly;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class MARGINS
    {
      public int cxLeftWidth, cxRightWidth,
                 cyTopHeight, cyBottomHeight;

      public MARGINS(int left, int top, int right, int bottom)
      {
        cxLeftWidth = left;
        cyTopHeight = top;
        cxRightWidth = right;
        cyBottomHeight = bottom;
      }
    }

    [StructLayout(LayoutKind.Sequential)]
    public class DWM_BLURBEHIND
    {
      public uint dwFlags;

      [MarshalAs(UnmanagedType.Bool)]
      public bool fEnable;

      public IntPtr hRegionBlur;

      [MarshalAs(UnmanagedType.Bool)]
      public bool fTransitionOnMaximized;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NMHDR
    {
      public IntPtr hwndFrom;
      public IntPtr idFrom;
      public uint code;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NMCUSTOMDRAW
    {
      public NMHDR hdr;
      public int dwDrawStage;
      public IntPtr hdc;
      public RECT rc;
      public IntPtr dwItemSpec;
      public int uItemState;
      public int lItemlParam;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NMLVCUSTOMDRAW
    {
      public NMCUSTOMDRAW nmcd;
      public uint clrText;
      public uint clrTextBk;
      public int iSubItem;
      public uint dwItemType;
      public uint clrFace;
      public int iIconEffect;
      public int iIconPhase;
      public int iPartId;
      public int iStateId;
      public RECT rcText;
      public uint uAlign;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NMHEADER
    {
      public NMHDR nhdr;
      public int iItem;
      public int iButton;
      public IntPtr pHDITEM;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PAINTSTRUCT
    {
      public IntPtr hdc;
      public int fErase;
      public RECT rcPaint;
      public int fRestore;
      public int fIncUpdate;
      public int Reserved1;
      public int Reserved2;
      public int Reserved3;
      public int Reserved4;
      public int Reserved5;
      public int Reserved6;
      public int Reserved7;
      public int Reserved8;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HDHITTESTINFO
    {
      public Point pt;
      public uint flags;
      public int iItem;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HDLAYOUT
    {
      public IntPtr prc; // RECT*
      public IntPtr pwpos; // WINDOWPOS*
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct HDITEM
    {
      public int mask;
      public int cxy;
      public string pszText;
      public IntPtr hbm;
      public int cchTextMax;
      public int fmt;
      public int lParam;
      public int iImage;
      public int iOrder;
      public uint type;
      public IntPtr pvFilter;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct DRAWITEMSTRUCT
    {
      public int ctrlType;
      public int ctrlID;
      public int itemID;
      public int itemAction;
      public int itemState;
      public IntPtr hwnd;
      public IntPtr hdc;
      public RECT rcItem;
      public IntPtr itemData;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SYSTEM_INFO
    {
      internal _PROCESSOR_INFO_UNION ProcessorInfo;
      public uint PageSize;
      public IntPtr MinimumApplicationAddress;
      public IntPtr MaximumApplicationAddress;
      public IntPtr ActiveProcessorMask;
      public uint NumberOfProcessors;
      public uint ProcessorType;
      public uint AllocationGranularity;
      public ushort ProcessorLevel;
      public ushort ProcessorRevision;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct _PROCESSOR_INFO_UNION
    {
      [FieldOffset(0)]
      internal uint OemId;

      [FieldOffset(0)]
      internal ushort ProcessorArchitecture;

      [FieldOffset(2)]
      internal ushort Reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LSA_OBJECT_ATTRIBUTES
    {
      internal int Length;
      internal IntPtr RootDirectory;
      internal IntPtr ObjectName;
      internal int Attributes;
      internal IntPtr SecurityDescriptor;
      internal IntPtr SecurityQualityOfService;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LSA_UNICODE_STRING
    {
      public UInt16 Length;
      public UInt16 MaximumLength;

      [MarshalAs(UnmanagedType.LPWStr)]
      internal string Buffer;
    }

    public enum UserAccountControlFlags
    {
      SCRIPT = 0x0001,
      ACCOUNTDISABLE = 0x0002,
      HOMEDIR_REQUIRED = 0x0008,
      LOCKOUT = 0x0010,
      PASSWD_NOTREQD = 0x0020,
      PASSWD_CANT_CHANGE = 0x0040,
      ENCRYPTED_TEXT_PWD_ALLOWED = 0x0080,
      TEMP_DUPLICATE_ACCOUNT = 0x0100,
      NORMAL_ACCOUNT = 0x0200,
      INTERDOMAIN_TRUST_ACCOUNT = 0x0800,
      WORKSTATION_TRUST_ACCOUNT = 0x1000,
      SERVER_TRUST_ACCOUNT = 0x2000,
      DONT_EXPIRE_PASSWORD = 0x10000,
      MNS_LOGON_ACCOUNT = 0x20000,
      SMARTCARD_REQUIRED = 0x40000,
      TRUSTED_FOR_DELEGATION = 0x80000,
      NOT_DELEGATED = 0x100000,
      USE_DES_KEY_ONLY = 0x200000,
      DONT_REQ_PREAUTH = 0x400000,
      PASSWORD_EXPIRED = 0x800000,
      TRUSTED_TO_AUTH_FOR_DELEGATION = 0x1000000
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct UserRights
    {
      public static string SeNetworkLogonRight = "SeNetworkLogonRight";
      public static string SeInteractiveLogonRight = "SeInteractiveLogonRight";
      public static string SeBatchLogonRight = "SeBatchLogonRight";
      public static string LogOnAsAService = "SeServiceLogonRight";
      public static string SeDenyNetworkLogonRight = "SeDenyNetworkLogonRight";
      public static string SeDenyInteractiveLogonRight = "SeDenyInteractiveLogonRight";
      public static string SeDenyBatchLogonRight = "SeDenyBatchLogonRight";
      public static string SeDenyServiceLogonRight = "SeDenyServiceLogonRight";
      public static string SeCreateGlobalPrivilege = "SeCreateGlobalPrivilege";
      public static string SeDebugPrivilege = "SeDebugPrivilege";
      public static string SeDenyRemoteInteractiveLogonRight = "SeDenyRemoteInteractiveLogonRight";
      public static string SeEnableDelegationPrivilege = "SeEnableDelegationPrivilege";
      public static string SeImpersonatePrivilege = "SeImpersonatePrivilege";
      public static string SeManageVolumePrivilege = "SeManageVolumePrivilege";
      public static string SeRemoteInteractiveLogonRight = "SeRemoteInteractiveLogonRight";
      public static string SeSyncAgentPrivilege = "SeSyncAgentPrivilege";
      public static string SeUndockPrivilege = "SeUndockPrivilege";
    }

    public enum NET_API_STATUS
    {
      /// <summary>
      /// The workstation driver is not installed.
      /// </summary>
      NERR_NetNotStarted = 2102,

      /// <summary>
      /// The server could not be located.
      /// </summary>
      NERR_UnknownServer = 2103,

      /// <summary>
      /// An internal error occurred. The network cannot access a shared memory segment.
      /// </summary>
      NERR_ShareMem = 2104,

      /// <summary>
      /// A network resource shortage occurred.
      /// </summary>
      NERR_NoNetworkResource = 2105,

      /// <summary>
      /// This operation is not supported on workstations.
      /// </summary>
      NERR_RemoteOnly = 2106,

      /// <summary>
      /// The device is not connected.
      /// </summary>
      NERR_DevNotRedirected = 2107,

      /// <summary>
      /// The Server service is not started.
      /// </summary>
      NERR_ServerNotStarted = 2114,

      /// <summary>
      /// The queue is empty.
      /// </summary>
      NERR_ItemNotFound = 2115,

      /// <summary>
      /// The device or directory does not exist.
      /// </summary>
      NERR_UnknownDevDir = 2116,

      /// <summary>
      /// The operation is invalid on a redirected resource.
      /// </summary>
      NERR_RedirectedPath = 2117,

      /// <summary>
      /// The name has already been shared.
      /// </summary>
      NERR_DuplicateShare = 2118,

      /// <summary>
      /// The server is currently out of the requested resource.
      /// </summary>
      NERR_NoRoom = 2119,

      /// <summary>
      /// Requested addition of items exceeds the maximum allowed.
      /// </summary>
      NERR_TooManyItems = 2121,

      /// <summary>
      /// The Peer service supports only two simultaneous users.
      /// </summary>
      NERR_InvalidMaxUsers = 2122,

      /// <summary>
      /// The API return buffer is too small.
      /// </summary>
      NERR_BufTooSmall = 2123,

      /// <summary>
      /// A remote API error occurred.
      /// </summary>
      NERR_RemoteErr = 2127,

      /// <summary>
      /// An error occurred when opening or reading the configuration file.
      /// </summary>
      NERR_LanmanIniError = 2131,

      /// <summary>
      /// A general network error occurred.
      /// </summary>
      NERR_NetworkError = 2136,

      /// <summary>
      /// The Workstation service is in an inconsistent state. Restart
      /// the computer before restarting the Workstation service.
      /// </summary>
      NERR_WkstaInconsistentState = 2137,

      /// <summary>
      /// The Workstation service has not been started.
      /// </summary>
      NERR_WkstaNotStarted = 2138,

      /// <summary>
      /// The requested information is not available.
      /// </summary>
      NERR_BrowserNotStarted = 2139,

      /// <summary>
      /// An internal error occurred.
      /// </summary>
      NERR_InternalError = 2140,

      /// <summary>
      /// The server is not configured for transactions.
      /// </summary>
      NERR_BadTransactConfig = 2141,

      /// <summary>
      /// The requested API is not supported on the remote server.
      /// </summary>
      NERR_InvalidAPI = 2142,

      /// <summary>
      /// The event name is invalid.
      /// </summary>
      NERR_BadEventName = 2143,

      /// <summary>
      /// The computer name already exists on the network. Change it and restart the computer.
      /// </summary>
      NERR_DupNameReboot = 2144,

      /// <summary>
      /// The specified component could not be found in the configuration information.
      /// </summary>
      NERR_CfgCompNotFound = 2146,

      /// <summary>
      /// The specified parameter could not be found in the configuration information.
      /// </summary>
      NERR_CfgParamNotFound = 2147,

      /// <summary>
      /// A line in the configuration file is too long.
      /// </summary>
      NERR_LineTooLong = 2149,

      /// <summary>
      /// The printer does not exist.
      /// </summary>
      NERR_QNotFound = 2150,

      /// <summary>
      /// The print job does not exist.
      /// </summary>
      NERR_JobNotFound = 2151,

      /// <summary>
      /// The printer destination cannot be found.
      /// </summary>
      NERR_DestNotFound = 2152,

      /// <summary>
      /// The printer destination already exists.
      /// </summary>
      NERR_DestExists = 2153,

      /// <summary>
      /// The printer queue already exists.
      /// </summary>
      NERR_QExists = 2154,

      /// <summary>
      /// No more printers can be added.
      /// </summary>
      NERR_QNoRoom = 2155,

      /// <summary>
      /// No more print jobs can be added.
      /// </summary>
      NERR_JobNoRoom = 2156,

      /// <summary>
      /// No more printer destinations can be added.
      /// </summary>
      NERR_DestNoRoom = 2157,

      /// <summary>
      /// This printer destination is idle and cannot accept control operations.
      /// </summary>
      NERR_DestIdle = 2158,

      /// <summary>
      /// This printer destination request contains an invalid control function.
      /// </summary>
      NERR_DestInvalidOp = 2159,

      /// <summary>
      /// The print processor is not responding.
      /// </summary>
      NERR_ProcNoRespond = 2160,

      /// <summary>
      /// The spooler is not running.
      /// </summary>
      NERR_SpoolerNotLoaded = 2161,

      /// <summary>
      /// This operation cannot be performed on the print destination in its current state.
      /// </summary>
      NERR_DestInvalidState = 2162,

      /// <summary>
      /// This operation cannot be performed on the printer queue in its current state.
      /// </summary>
      NERR_QinvalidState = 2163,

      /// <summary>
      /// This operation cannot be performed on the print job in its current state.
      /// </summary>
      NERR_JobInvalidState = 2164,

      /// <summary>
      /// A spooler memory allocation failure occurred.
      /// </summary>
      NERR_SpoolNoMemory = 2165,

      /// <summary>
      /// The device driver does not exist.
      /// </summary>
      NERR_DriverNotFound = 2166,

      /// <summary>
      /// The data type is not supported by the print processor.
      /// </summary>
      NERR_DataTypeInvalid = 2167,

      /// <summary>
      /// The print processor is not installed.
      /// </summary>
      NERR_ProcNotFound = 2168,

      /// <summary>
      /// The service database is locked.
      /// </summary>
      NERR_ServiceTableLocked = 2180,

      /// <summary>
      /// The service table is full.
      /// </summary>
      NERR_ServiceTableFull = 2181,

      /// <summary>
      /// The requested service has already been started.
      /// </summary>
      NERR_ServiceInstalled = 2182,

      /// <summary>
      /// The service does not respond to control actions.
      /// </summary>
      NERR_ServiceEntryLocked = 2183,

      /// <summary>
      /// The service has not been started.
      /// </summary>
      NERR_ServiceNotInstalled = 2184,

      /// <summary>
      /// The service name is invalid.
      /// </summary>
      NERR_BadServiceName = 2185,

      /// <summary>
      /// The service is not responding to the control function.
      /// </summary>
      NERR_ServiceCtlTimeout = 2186,

      /// <summary>
      /// The service control is busy.
      /// </summary>
      NERR_ServiceCtlBusy = 2187,

      /// <summary>
      /// The configuration file contains an invalid service program name.
      /// </summary>
      NERR_BadServiceProgName = 2188,

      /// <summary>
      /// The service could not be controlled in its present state.
      /// </summary>
      NERR_ServiceNotCtrl = 2189,

      /// <summary>
      /// The service ended abnormally.
      /// </summary>
      NERR_ServiceKillProc = 2190,

      /// <summary>
      /// The requested pause or stop is not valid for this service.
      /// </summary>
      NERR_ServiceCtlNotValid = 2191,

      /// <summary>
      /// The service control dispatcher could not find the service name in the dispatch table.
      /// </summary>
      NERR_NotInDispatchTbl = 2192,

      /// <summary>
      /// The service control dispatcher pipe read failed.
      /// </summary>
      NERR_BadControlRecv = 2193,

      /// <summary>
      /// A thread for the new service could not be created.
      /// </summary>
      NERR_ServiceNotStarting = 2194,

      /// <summary>
      /// This workstation is already logged on to the local-area network.
      /// </summary>
      NERR_AlreadyLoggedOn = 2200,

      /// <summary>
      /// The workstation is not logged on to the local-area network.
      /// </summary>
      NERR_NotLoggedOn = 2201,

      /// <summary>
      /// The user name or group name parameter is invalid.
      /// </summary>
      NERR_BadUsername = 2202,

      /// <summary>
      /// The password parameter is invalid.
      /// </summary>
      NERR_BadPassword = 2203,

      /// <summary>
      /// The logon processor did not add the message alias.
      /// </summary>
      NERR_UnableToAddName_W = 2204,

      /// <summary>
      /// The logon processor did not add the message alias.
      /// </summary>
      NERR_UnableToAddName_F = 2205,

      /// <summary>
      /// The logoff processor did not delete the message alias.
      /// </summary>
      NERR_UnableToDelName_W = 2206,

      /// <summary>
      /// The logoff processor did not delete the message alias.
      /// </summary>
      NERR_UnableToDelName_F = 2207,

      /// <summary>
      /// Network logons are paused.
      /// </summary>
      NERR_LogonsPaused = 2209,

      /// <summary>
      /// A centralized logon-server conflict occurred.
      /// </summary>
      NERR_LogonServerConflict = 2210,

      /// <summary>
      /// The server is configured without a valid user path.
      /// </summary>
      NERR_LogonNoUserPath = 2211,

      /// <summary>
      /// An error occurred while loading or running the logon script.
      /// </summary>
      NERR_LogonScriptError = 2212,

      /// <summary>
      /// The logon server was not specified. Your computer will be logged on as STANDALONE.
      /// </summary>
      NERR_StandaloneLogon = 2214,

      /// <summary>
      /// The logon server could not be found.
      /// </summary>
      NERR_LogonServerNotFound = 2215,

      /// <summary>
      /// There is already a logon domain for this computer.
      /// </summary>
      NERR_LogonDomainExists = 2216,

      /// <summary>
      /// The logon server could not validate the logon.
      /// </summary>
      NERR_NonValidatedLogon = 2217,

      /// <summary>
      /// The security database could not be found.
      /// </summary>
      NERR_ACFNotFound = 2219,

      /// <summary>
      /// The group name could not be found.
      /// </summary>
      NERR_GroupNotFound = 2220,

      /// <summary>
      /// The user name could not be found.
      /// </summary>
      NERR_UserNotFound = 2221,

      /// <summary>
      /// The resource name could not be found.
      /// </summary>
      NERR_ResourceNotFound = 2222,

      /// <summary>
      /// The group already exists.
      /// </summary>
      NERR_GroupExists = 2223,

      /// <summary>
      /// The user account already exists.
      /// </summary>
      NERR_UserExists = 2224,

      /// <summary>
      /// The resource permission list already exists.
      /// </summary>
      NERR_ResourceExists = 2225,

      /// <summary>
      /// This operation is only allowed on the primary domain controller of the domain.
      /// </summary>
      NERR_NotPrimary = 2226,

      /// <summary>
      /// The security database has not been started.
      /// </summary>
      NERR_ACFNotLoaded = 2227,

      /// <summary>
      /// There are too many names in the user accounts database.
      /// </summary>
      NERR_ACFNoRoom = 2228,

      /// <summary>
      /// A disk I/O failure occurred.
      /// </summary>
      NERR_ACFFileIOFail = 2229,

      /// <summary>
      /// The limit of 64 entries per resource was exceeded.
      /// </summary>
      NERR_ACFTooManyLists = 2230,

      /// <summary>
      /// Deleting a user with a session is not allowed.
      /// </summary>
      NERR_UserLogon = 2231,

      /// <summary>
      /// The parent directory could not be located.
      /// </summary>
      NERR_ACFNoParent = 2232,

      /// <summary>
      /// Unable to add to the security database session cache segment.
      /// </summary>
      NERR_CanNotGrowSegment = 2233,

      /// <summary>
      /// This operation is not allowed on this special group.
      /// </summary>
      NERR_SpeGroupOp = 2234,

      /// <summary>
      /// This user is not cached in user accounts database session cache.
      /// </summary>
      NERR_NotInCache = 2235,

      /// <summary>
      /// The user already belongs to this group.
      /// </summary>
      NERR_UserInGroup = 2236,

      /// <summary>
      /// The user does not belong to this group.
      /// </summary>
      NERR_UserNotInGroup = 2237,

      /// <summary>
      /// This user account is undefined.
      /// </summary>
      NERR_AccountUndefined = 2238,

      /// <summary>
      /// This user account has expired.
      /// </summary>
      NERR_AccountExpired = 2239,

      /// <summary>
      /// The user is not allowed to log on from this workstation.
      /// </summary>
      NERR_InvalidWorkstation = 2240,

      /// <summary>
      /// The user is not allowed to log on at this time.
      /// </summary>
      NERR_InvalidLogonHours = 2241,

      /// <summary>
      /// The password of this user has expired.
      /// </summary>
      NERR_PasswordExpired = 2242,

      /// <summary>
      /// The password of this user cannot change.
      /// </summary>
      NERR_PasswordCantChange = 2243,

      /// <summary>
      /// This password cannot be used now.
      /// </summary>
      NERR_PasswordHistConflict = 2244,

      /// <summary>
      /// The password does not meet the password policy requirements. Check the minimum password length, password complexity and password history requirements.
      /// </summary>
      NERR_PasswordTooShort = 2245,

      /// <summary>
      /// The password of this user is too recent to change.
      /// </summary>
      NERR_PasswordTooRecent = 2246,

      /// <summary>
      /// The security database is corrupted.
      /// </summary>
      NERR_InvalidDatabase = 2247,

      /// <summary>
      /// No updates are necessary to this replicant network/local security database.
      /// </summary>
      NERR_DatabaseUpToDate = 2248,

      /// <summary>
      /// This replicant database is outdated; synchronization is required.
      /// </summary>
      NERR_SyncRequired = 2249,

      /// <summary>
      /// The network connection could not be found.
      /// </summary>
      NERR_UseNotFound = 2250,

      /// <summary>
      /// This asg_type is invalid.
      /// </summary>
      NERR_BadAsgType = 2251,

      /// <summary>
      /// This device is currently being shared.
      /// </summary>
      NERR_DeviceIsShared = 2252,

      /// <summary>
      /// The computer name could not be added as a message alias. The name may already exist on the network.
      /// </summary>
      NERR_NoComputerName = 2270,

      /// <summary>
      /// The Messenger service is already started.
      /// </summary>
      NERR_MsgAlreadyStarted = 2271,

      /// <summary>
      /// The Messenger service failed to start.
      /// </summary>
      NERR_MsgInitFailed = 2272,

      /// <summary>
      /// The message alias could not be found on the network.
      /// </summary>
      NERR_NameNotFound = 2273,

      /// <summary>
      /// This message alias has already been forwarded.
      /// </summary>
      NERR_AlreadyForwarded = 2274,

      /// <summary>
      /// This message alias has been added but is still forwarded.
      /// </summary>
      NERR_AddForwarded = 2275,

      /// <summary>
      /// This message alias already exists locally.
      /// </summary>
      NERR_AlreadyExists = 2276,

      /// <summary>
      /// The maximum number of added message aliases has been exceeded.
      /// </summary>
      NERR_TooManyNames = 2277,

      /// <summary>
      /// The computer name could not be deleted.
      /// </summary>
      NERR_DelComputerName = 2278,

      /// <summary>
      /// Messages cannot be forwarded back to the same workstation.
      /// </summary>
      NERR_LocalForward = 2279,

      /// <summary>
      /// An error occurred in the domain message processor.
      /// </summary>
      NERR_GrpMsgProcessor = 2280,

      /// <summary>
      /// The message was sent, but the recipient has paused the Messenger service.
      /// </summary>
      NERR_PausedRemote = 2281,

      /// <summary>
      /// The message was sent but not received.
      /// </summary>
      NERR_BadReceive = 2282,

      /// <summary>
      /// The message alias is currently in use. Try again later.
      /// </summary>
      NERR_NameInUse = 2283,

      /// <summary>
      /// The Messenger service has not been started.
      /// </summary>
      NERR_MsgNotStarted = 2284,

      /// <summary>
      /// The name is not on the local computer.
      /// </summary>
      NERR_NotLocalName = 2285,

      /// <summary>
      /// The forwarded message alias could not be found on the network.
      /// </summary>
      NERR_NoForwardName = 2286,

      /// <summary>
      /// The message alias table on the remote station is full.
      /// </summary>
      NERR_RemoteFull = 2287,

      /// <summary>
      /// Messages for this alias are not currently being forwarded.
      /// </summary>
      NERR_NameNotForwarded = 2288,

      /// <summary>
      /// The broadcast message was truncated.
      /// </summary>
      NERR_TruncatedBroadcast = 2289,

      /// <summary>
      /// This is an invalid device name.
      /// </summary>
      NERR_InvalidDevice = 2294,

      /// <summary>
      /// A write fault occurred.
      /// </summary>
      NERR_WriteFault = 2295,

      /// <summary>
      /// A duplicate message alias exists on the network.
      /// </summary>
      NERR_DuplicateName = 2297,

      /// <summary>
      /// This message alias will be deleted later.
      /// </summary>
      NERR_DeleteLater = 2298,

      /// <summary>
      /// The message alias was not successfully deleted from all networks.
      /// </summary>
      NERR_IncompleteDel = 2299,

      /// <summary>
      /// This operation is not supported on computers with multiple networks.
      /// </summary>
      NERR_MultipleNets = 2300,

      /// <summary>
      /// This shared resource does not exist.
      /// </summary>
      NERR_NetNameNotFound = 2310,

      /// <summary>
      /// This device is not shared.
      /// </summary>
      NERR_DeviceNotShared = 2311,

      /// <summary>
      /// A session does not exist with that computer name.
      /// </summary>
      NERR_ClientNameNotFound = 2312,

      /// <summary>
      /// There is not an open file with that identification number.
      /// </summary>
      NERR_FileIdNotFound = 2314,

      /// <summary>
      /// A failure occurred when executing a remote administration command.
      /// </summary>
      NERR_ExecFailure = 2315,

      /// <summary>
      /// A failure occurred when opening a remote temporary file.
      /// </summary>
      NERR_TmpFile = 2316,

      /// <summary>
      /// The data returned from a remote administration command has been truncated to 64K.
      /// </summary>
      NERR_TooMuchData = 2317,

      /// <summary>
      /// This device cannot be shared as both a spooled and a non-spooled resource.
      /// </summary>
      NERR_DeviceShareConflict = 2318,

      /// <summary>
      /// The information in the list of servers may be incorrect.
      /// </summary>
      NERR_BrowserTableIncomplete = 2319,

      /// <summary>
      /// The computer is not active in this domain.
      /// </summary>
      NERR_NotLocalDomain = 2320,

      /// <summary>
      /// The share must be removed from the Distributed File System before it can be deleted.
      /// </summary>
      NERR_IsDfsShare = 2321,

      /// <summary>
      /// The operation is invalid for this device.
      /// </summary>
      NERR_DevInvalidOpCode = 2331,

      /// <summary>
      /// This device cannot be shared.
      /// </summary>
      NERR_DevNotFound = 2332,

      /// <summary>
      /// This device was not open.
      /// </summary>
      NERR_DevNotOpen = 2333,

      /// <summary>
      /// This device name list is invalid.
      /// </summary>
      NERR_BadQueueDevString = 2334,

      /// <summary>
      /// The queue priority is invalid.
      /// </summary>
      NERR_BadQueuePriority = 2335,

      /// <summary>
      /// There are no shared communication devices.
      /// </summary>
      NERR_NoCommDevs = 2337,

      /// <summary>
      /// The queue you specified does not exist.
      /// </summary>
      NERR_QueueNotFound = 2338,

      /// <summary>
      /// This list of devices is invalid.
      /// </summary>
      NERR_BadDevString = 2340,

      /// <summary>
      /// The requested device is invalid.
      /// </summary>
      NERR_BadDev = 2341,

      /// <summary>
      /// This device is already in use by the spooler.
      /// </summary>
      NERR_InUseBySpooler = 2342,

      /// <summary>
      /// This device is already in use as a communication device.
      /// </summary>
      NERR_CommDevInUse = 2343,

      /// <summary>
      /// This computer name is invalid.
      /// </summary>
      NERR_InvalidComputer = 2351,

      /// <summary>
      /// The string and prefix specified are too long.
      /// </summary>
      NERR_MaxLenExceeded = 2354,

      /// <summary>
      /// This path component is invalid.
      /// </summary>
      NERR_BadComponent = 2356,

      /// <summary>
      /// Could not determine the type of input.
      /// </summary>
      NERR_CantType = 2357,

      /// <summary>
      /// The buffer for types is not big enough.
      /// </summary>
      NERR_TooManyEntries = 2362,

      /// <summary>
      /// Profile files cannot exceed 64K.
      /// </summary>
      NERR_ProfileFileTooBig = 2370,

      /// <summary>
      /// The start offset is out of range.
      /// </summary>
      NERR_ProfileOffset = 2371,

      /// <summary>
      /// The system cannot delete current connections to network resources.
      /// </summary>
      NERR_ProfileCleanup = 2372,

      /// <summary>
      /// The system was unable to parse the command line in this file.
      /// </summary>
      NERR_ProfileUnknownCmd = 2373,

      /// <summary>
      /// An error occurred while loading the profile file.
      /// </summary>
      NERR_ProfileLoadErr = 2374,

      /// <summary>
      /// @W Errors occurred while saving the profile file. The profile was partially saved.
      /// </summary>
      NERR_ProfileSaveErr = 2375,

      /// <summary>
      /// Log file %1 is full.
      /// </summary>
      NERR_LogOverflow = 2377,

      /// <summary>
      /// This log file has changed between reads.
      /// </summary>
      NERR_LogFileChanged = 2378,

      /// <summary>
      /// Log file %1 is corrupt.
      /// </summary>
      NERR_LogFileCorrupt = 2379,

      /// <summary>
      /// The source path cannot be a directory.
      /// </summary>
      NERR_SourceIsDir = 2380,

      /// <summary>
      /// The source path is illegal.
      /// </summary>
      NERR_BadSource = 2381,

      /// <summary>
      /// The destination path is illegal.
      /// </summary>
      NERR_BadDest = 2382,

      /// <summary>
      /// The source and destination paths are on different servers.
      /// </summary>
      NERR_DifferentServers = 2383,

      /// <summary>
      /// The Run server you requested is paused.
      /// </summary>
      NERR_RunSrvPaused = 2385,

      /// <summary>
      /// An error occurred when communicating with a Run server.
      /// </summary>
      NERR_ErrCommRunSrv = 2389,

      /// <summary>
      /// An error occurred when starting a background process.
      /// </summary>
      NERR_ErrorExecingGhost = 2391,

      /// <summary>
      /// The shared resource you are connected to could not be found.
      /// </summary>
      NERR_ShareNotFound = 2392,

      /// <summary>
      /// The LAN adapter number is invalid.
      /// </summary>
      NERR_InvalidLana = 2400,

      /// <summary>
      /// There are open files on the connection.
      /// </summary>
      NERR_OpenFiles = 2401,

      /// <summary>
      /// Active connections still exist.
      /// </summary>
      NERR_ActiveConns = 2402,

      /// <summary>
      /// This share name or password is invalid.
      /// </summary>
      NERR_BadPasswordCore = 2403,

      /// <summary>
      /// The device is being accessed by an active process.
      /// </summary>
      NERR_DevInUse = 2404,

      /// <summary>
      /// The drive letter is in use locally.
      /// </summary>
      NERR_LocalDrive = 2405,

      /// <summary>
      /// The specified client is already registered for the specified event.
      /// </summary>
      NERR_AlertExists = 2430,

      /// <summary>
      /// The alert table is full.
      /// </summary>
      NERR_TooManyAlerts = 2431,

      /// <summary>
      /// An invalid or nonexistent alert name was raised.
      /// </summary>
      NERR_NoSuchAlert = 2432,

      /// <summary>
      /// The alert recipient is invalid.
      /// </summary>
      NERR_BadRecipient = 2433,

      /// <summary>
      /// A user's session with this server has been deleted
      /// </summary>
      NERR_AcctLimitExceeded = 2434,

      /// <summary>
      /// The log file does not contain the requested record number.
      /// </summary>
      NERR_InvalidLogSeek = 2440,

      /// <summary>
      /// The user accounts database is not configured correctly.
      /// </summary>
      NERR_BadUasConfig = 2450,

      /// <summary>
      /// This operation is not permitted when the Netlogon service is running.
      /// </summary>
      NERR_InvalidUASOp = 2451,

      /// <summary>
      /// This operation is not allowed on the last administrative account.
      /// </summary>
      NERR_LastAdmin = 2452,

      /// <summary>
      /// Could not find domain controller for this domain.
      /// </summary>
      NERR_DCNotFound = 2453,

      /// <summary>
      /// Could not set logon information for this user.
      /// </summary>
      NERR_LogonTrackingError = 2454,

      /// <summary>
      /// The Netlogon service has not been started.
      /// </summary>
      NERR_NetlogonNotStarted = 2455,

      /// <summary>
      /// Unable to add to the user accounts database.
      /// </summary>
      NERR_CanNotGrowUASFile = 2456,

      /// <summary>
      /// This server's clock is not synchronized with the primary domain controller's clock.
      /// </summary>
      NERR_TimeDiffAtDC = 2457,

      /// <summary>
      /// A password mismatch has been detected.
      /// </summary>
      NERR_PasswordMismatch = 2458,

      /// <summary>
      /// The server identification does not specify a valid server.
      /// </summary>
      NERR_NoSuchServer = 2460,

      /// <summary>
      /// The session identification does not specify a valid session.
      /// </summary>
      NERR_NoSuchSession = 2461,

      /// <summary>
      /// The connection identification does not specify a valid connection.
      /// </summary>
      NERR_NoSuchConnection = 2462,

      /// <summary>
      /// There is no space for another entry in the table of available servers.
      /// </summary>
      NERR_TooManyServers = 2463,

      /// <summary>
      /// The server has reached the maximum number of sessions it supports.
      /// </summary>
      NERR_TooManySessions = 2464,

      /// <summary>
      /// The server has reached the maximum number of connections it supports.
      /// </summary>
      NERR_TooManyConnections = 2465,

      /// <summary>
      /// The server cannot open more files because it has reached its maximum number.
      /// </summary>
      NERR_TooManyFiles = 2466,

      /// <summary>
      /// There are no alternate servers registered on this server.
      /// </summary>
      NERR_NoAlternateServers = 2467,

      /// <summary>
      /// Try down-level (remote admin protocol) version of API instead.
      /// </summary>
      NERR_TryDownLevel = 2470,

      /// <summary>
      /// The UPS driver could not be accessed by the UPS service.
      /// </summary>
      NERR_UPSDriverNotStarted = 2480,

      /// <summary>
      /// The UPS service is not configured correctly.
      /// </summary>
      NERR_UPSInvalidConfig = 2481,

      /// <summary>
      /// The UPS service could not access the specified Comm Port.
      /// </summary>
      NERR_UPSInvalidCommPort = 2482,

      /// <summary>
      /// The UPS indicated a line fail or low battery situation. Service not started.
      /// </summary>
      NERR_UPSSignalAsserted = 2483,

      /// <summary>
      /// The UPS service failed to perform a system shut down.
      /// </summary>
      NERR_UPSShutdownFailed = 2484,

      /// <summary>
      /// The program below returned an MS-DOS error code:
      /// </summary>
      NERR_BadDosRetCode = 2500,

      /// <summary>
      /// The program below needs more memory:
      /// </summary>
      NERR_ProgNeedsExtraMem = 2501,

      /// <summary>
      /// The program below called an unsupported MS-DOS function:
      /// </summary>
      NERR_BadDosFunction = 2502,

      /// <summary>
      /// The workstation failed to boot.
      /// </summary>
      NERR_RemoteBootFailed = 2503,

      /// <summary>
      /// The file below is corrupt.
      /// </summary>
      NERR_BadFileCheckSum = 2504,

      /// <summary>
      /// No loader is specified in the boot-block definition file.
      /// </summary>
      NERR_NoRplBootSystem = 2505,

      /// <summary>
      /// NetBIOS returned an error: The NCB and SMB are dumped above.
      /// </summary>
      NERR_RplLoadrNetBiosErr = 2506,

      /// <summary>
      /// A disk I/O error occurred.
      /// </summary>
      NERR_RplLoadrDiskErr = 2507,

      /// <summary>
      /// Image parameter substitution failed.
      /// </summary>
      NERR_ImageParamErr = 2508,

      /// <summary>
      /// Too many image parameters cross disk sector boundaries.
      /// </summary>
      NERR_TooManyImageParams = 2509,

      /// <summary>
      /// The image was not generated from an MS-DOS diskette formatted with /S.
      /// </summary>
      NERR_NonDosFloppyUsed = 2510,

      /// <summary>
      /// Remote boot will be restarted later.
      /// </summary>
      NERR_RplBootRestart = 2511,

      /// <summary>
      /// The call to the Remoteboot server failed.
      /// </summary>
      NERR_RplSrvrCallFailed = 2512,

      /// <summary>
      /// Cannot connect to the Remoteboot server.
      /// </summary>
      NERR_CantConnectRplSrvr = 2513,

      /// <summary>
      /// Cannot open image file on the Remoteboot server.
      /// </summary>
      NERR_CantOpenImageFile = 2514,

      /// <summary>
      /// Connecting to the Remoteboot server...
      /// </summary>
      NERR_CallingRplSrvr = 2515,

      /// <summary>
      /// Connecting to the Remoteboot server...
      /// </summary>
      NERR_StartingRplBoot = 2516,

      /// <summary>
      /// Remote boot service was stopped; check the error log for the cause of the problem.
      /// </summary>
      NERR_RplBootServiceTerm = 2517,

      /// <summary>
      /// Remote boot startup failed; check the error log for the cause of the problem.
      /// </summary>
      NERR_RplBootStartFailed = 2518,

      /// <summary>
      /// A second connection to a Remoteboot resource is not allowed.
      /// </summary>
      NERR_RPL_CONNECTED = 2519,

      /// <summary>
      /// The browser service was configured with MaintainServerList=No.
      /// </summary>
      NERR_BrowserConfiguredToNotRun = 2550,

      /// <summary>
      /// Service failed to start since none of the network adapters started with this service.
      /// </summary>
      NERR_RplNoAdaptersStarted = 2610,

      /// <summary>
      /// Service failed to start due to bad startup information in the registry.
      /// </summary>
      NERR_RplBadRegistry = 2611,

      /// <summary>
      /// Service failed to start because its database is absent or corrupt.
      /// </summary>
      NERR_RplBadDatabase = 2612,

      /// <summary>
      /// Service failed to start because RPLFILES share is absent.
      /// </summary>
      NERR_RplRplfilesShare = 2613,

      /// <summary>
      /// Service failed to start because RPLUSER group is absent.
      /// </summary>
      NERR_RplNotRplServer = 2614,

      /// <summary>
      /// Cannot enumerate service records.
      /// </summary>
      NERR_RplCannotEnum = 2615,

      /// <summary>
      /// Workstation record information has been corrupted.
      /// </summary>
      NERR_RplWkstaInfoCorrupted = 2616,

      /// <summary>
      /// Workstation record was not found.
      /// </summary>
      NERR_RplWkstaNotFound = 2617,

      /// <summary>
      /// Workstation name is in use by some other workstation.
      /// </summary>
      NERR_RplWkstaNameUnavailable = 2618,

      /// <summary>
      /// Profile record information has been corrupted.
      /// </summary>
      NERR_RplProfileInfoCorrupted = 2619,

      /// <summary>
      /// Profile record was not found.
      /// </summary>
      NERR_RplProfileNotFound = 2620,

      /// <summary>
      /// Profile name is in use by some other profile.
      /// </summary>
      NERR_RplProfileNameUnavailable = 2621,

      /// <summary>
      /// There are workstations using this profile.
      /// </summary>
      NERR_RplProfileNotEmpty = 2622,

      /// <summary>
      /// Configuration record information has been corrupted.
      /// </summary>
      NERR_RplConfigInfoCorrupted = 2623,

      /// <summary>
      /// Configuration record was not found.
      /// </summary>
      NERR_RplConfigNotFound = 2624,

      /// <summary>
      /// Adapter ID record information has been corrupted.
      /// </summary>
      NERR_RplAdapterInfoCorrupted = 2625,

      /// <summary>
      /// An internal service error has occurred.
      /// </summary>
      NERR_RplInternal = 2626,

      /// <summary>
      /// Vendor ID record information has been corrupted.
      /// </summary>
      NERR_RplVendorInfoCorrupted = 2627,

      /// <summary>
      /// Boot block record information has been corrupted.
      /// </summary>
      NERR_RplBootInfoCorrupted = 2628,

      /// <summary>
      /// The user account for this workstation record is missing.
      /// </summary>
      NERR_RplWkstaNeedsUserAcct = 2629,

      /// <summary>
      /// The RPLUSER local group could not be found.
      /// </summary>
      NERR_RplNeedsRPLUSERAcct = 2630,

      /// <summary>
      /// Boot block record was not found.
      /// </summary>
      NERR_RplBootNotFound = 2631,

      /// <summary>
      /// Chosen profile is incompatible with this workstation.
      /// </summary>
      NERR_RplIncompatibleProfile = 2632,

      /// <summary>
      /// Chosen network adapter ID is in use by some other workstation.
      /// </summary>
      NERR_RplAdapterNameUnavailable = 2633,

      /// <summary>
      /// There are profiles using this configuration.
      /// </summary>
      NERR_RplConfigNotEmpty = 2634,

      /// <summary>
      /// There are workstations, profiles, or configurations using this boot block.
      /// </summary>
      NERR_RplBootInUse = 2635,

      /// <summary>
      /// Service failed to backup Remoteboot database.
      /// </summary>
      NERR_RplBackupDatabase = 2636,

      /// <summary>
      /// Adapter record was not found.
      /// </summary>
      NERR_RplAdapterNotFound = 2637,

      /// <summary>
      /// Vendor record was not found.
      /// </summary>
      NERR_RplVendorNotFound = 2638,

      /// <summary>
      /// Vendor name is in use by some other vendor record.
      /// </summary>
      NERR_RplVendorNameUnavailable = 2639,

      /// <summary>
      /// (boot name, vendor ID) is in use by some other boot block record.
      /// </summary>
      NERR_RplBootNameUnavailable = 2640,

      /// <summary>
      /// Configuration name is in use by some other configuration.
      /// </summary>
      NERR_RplConfigNameUnavailable = 2641,

      /// <summary>
      /// The internal database maintained by the Dfs service is corrupt.
      /// </summary>
      NERR_DfsInternalCorruption = 2660,

      /// <summary>
      /// One of the records in the internal Dfs database is corrupt.
      /// </summary>
      NERR_DfsVolumeDataCorrupt = 2661,

      /// <summary>
      /// There is no DFS name whose entry path matches the input Entry Path.
      /// </summary>
      NERR_DfsNoSuchVolume = 2662,

      /// <summary>
      /// A root or link with the given name already exists.
      /// </summary>
      NERR_DfsVolumeAlreadyExists = 2663,

      /// <summary>
      /// The server share specified is already shared in the Dfs.
      /// </summary>
      NERR_DfsAlreadyShared = 2664,

      /// <summary>
      /// The indicated server share does not support the indicated DFS namespace.
      /// </summary>
      NERR_DfsNoSuchShare = 2665,

      /// <summary>
      /// The operation is not valid on this portion of the namespace.
      /// </summary>
      NERR_DfsNotALeafVolume = 2666,

      /// <summary>
      /// The operation is not valid on this portion of the namespace.
      /// </summary>
      NERR_DfsLeafVolume = 2667,

      /// <summary>
      /// The operation is ambiguous because the link has multiple servers.
      /// </summary>
      NERR_DfsVolumeHasMultipleServers = 2668,

      /// <summary>
      /// Unable to create a link.
      /// </summary>
      NERR_DfsCantCreateJunctionPoint = 2669,

      /// <summary>
      /// The server is not Dfs Aware.
      /// </summary>
      NERR_DfsServerNotDfsAware = 2670,

      /// <summary>
      /// The specified rename target path is invalid.
      /// </summary>
      NERR_DfsBadRenamePath = 2671,

      /// <summary>
      /// The specified DFS link is offline.
      /// </summary>
      NERR_DfsVolumeIsOffline = 2672,

      /// <summary>
      /// The specified server is not a server for this link.
      /// </summary>
      NERR_DfsNoSuchServer = 2673,

      /// <summary>
      /// A cycle in the Dfs name was detected.
      /// </summary>
      NERR_DfsCyclicalName = 2674,

      /// <summary>
      /// The operation is not supported on a server-based Dfs.
      /// </summary>
      NERR_DfsNotSupportedInServerDfs = 2675,

      /// <summary>
      /// This link is already supported by the specified server-share.
      /// </summary>
      NERR_DfsDuplicateService = 2676,

      /// <summary>
      /// Can't remove the last server-share supporting this root or link.
      /// </summary>
      NERR_DfsCantRemoveLastServerShare = 2677,

      /// <summary>
      /// The operation is not supported for an Inter-DFS link.
      /// </summary>
      NERR_DfsVolumeIsInterDfs = 2678,

      /// <summary>
      /// The internal state of the Dfs Service has become inconsistent.
      /// </summary>
      NERR_DfsInconsistent = 2679,

      /// <summary>
      /// The Dfs Service has been installed on the specified server.
      /// </summary>
      NERR_DfsServerUpgraded = 2680,

      /// <summary>
      /// The Dfs data being reconciled is identical.
      /// </summary>
      NERR_DfsDataIsIdentical = 2681,

      /// <summary>
      /// The DFS root cannot be deleted. Uninstall DFS if required.
      /// </summary>
      NERR_DfsCantRemoveDfsRoot = 2682,

      /// <summary>
      /// A child or parent directory of the share is already in a Dfs.
      /// </summary>
      NERR_DfsChildOrParentInDfs = 2683,

      /// <summary>
      /// Dfs internal error.
      /// </summary>
      NERR_DfsInternalError = 2690,

      /// <summary>
      /// This computer is already joined to a domain.
      /// </summary>
      NERR_SetupAlreadyJoined = 2691,

      /// <summary>
      /// This computer is not currently joined to a domain.
      /// </summary>
      NERR_SetupNotJoined = 2692,

      /// <summary>
      /// This computer is a domain controller and cannot be unjoined from a domain.
      /// </summary>
      NERR_SetupDomainController = 2693,

      /// <summary>
      /// The destination domain controller does not support creating machine accounts in OUs.
      /// </summary>
      NERR_DefaultJoinRequired = 2694,

      /// <summary>
      /// The specified workgroup name is invalid.
      /// </summary>
      NERR_InvalidWorkgroupName = 2695,

      /// <summary>
      /// The specified computer name is incompatible with the default language used on the domain controller.
      /// </summary>
      NERR_NameUsesIncompatibleCodePage = 2696,

      /// <summary>
      /// The specified computer account could not be found.
      /// </summary>
      NERR_ComputerAccountNotFound = 2697,

      /// <summary>
      /// This version of Windows cannot be joined to a domain.
      /// </summary>
      NERR_PersonalSku = 2698,

      /// <summary>
      /// The password must change at the next logon.
      /// </summary>
      NERR_PasswordMustChange = 2701,

      /// <summary>
      /// The account is locked out.
      /// </summary>
      NERR_AccountLockedOut = 2702,

      /// <summary>
      /// The password is too long.
      /// </summary>
      NERR_PasswordTooLong = 2703,

      /// <summary>
      /// The password does not meet the complexity policy.
      /// </summary>
      NERR_PasswordNotComplexEnough = 2704,

      /// <summary>
      /// The password does not meet the requirements of the password filter DLLs.
      /// </summary>
      NERR_PasswordFilterError = 2705,

      /// <summary>
      /// The offline join completion information was not found.
      /// </summary>
      NERR_NoOfflineJoinInfo = 2709,

      /// <summary>
      /// The offline join completion information was bad.
      /// </summary>
      NERR_BadOfflineJoinInfo = 2710,

      /// <summary>
      /// Unable to create offline join information. Please ensure you have access to the specified path location and permissions to modify its contents.
      /// Running as an elevated administrator may be required.
      /// </summary>
      NERR_CantCreateJoinInfo = 2711,

      /// <summary>
      /// The domain join info being saved was incomplete or bad.
      /// </summary>
      NERR_BadDomainJoinInfo = 2712,

      /// <summary>
      /// Offline join operation successfully completed but a restart is needed.
      /// </summary>
      NERR_JoinPerformedMustRestart = 2713,

      /// <summary>
      /// There was no offline join operation pending.
      /// </summary>
      NERR_NoJoinPending = 2714,

      /// <summary>
      /// Unable to set one or more requested machine or domain name values on the local computer.
      /// </summary>
      NERR_ValuesNotSet = 2715,

      /// <summary>
      /// Could not verify the current machine's hostname against the saved value in the join completion information.
      /// </summary>
      NERR_CantVerifyHostname = 2716,

      /// <summary>
      /// Unable to load the specified offline registry hive.
      /// Please ensure you have access to the specified path location and permissions to modify its contents.
      /// Running as an elevated administrator may be required.
      /// </summary>
      NERR_CantLoadOfflineHive = 2717,

      /// <summary>
      /// The minimum session security requirements for this operation were not met.
      /// </summary>
      NERR_ConnectionInsecure = 2718,

      /// <summary>
      /// Computer account provisioning blob version is not supported.
      /// </summary>
      NERR_ProvisioningBlobUnsupported = 2719
    }

    public enum SystemErrorCodes
    {
      /// <summary>
      /// Not all privileges or groups referenced are assigned to the caller.
      /// </summary>
      ERROR_NOT_ALL_ASSIGNED = 1300,

      /// <summary>
      /// Some mapping between account names and security IDs was not done.
      /// </summary>
      ERROR_SOME_NOT_MAPPED = 1301,

      /// <summary>
      /// No system quota limits are specifically set for this account.
      /// </summary>
      ERROR_NO_QUOTAS_FOR_ACCOUNT = 1302,

      /// <summary>
      /// No encryption key is available. A well-known encryption key was returned.
      /// </summary>
      ERROR_LOCAL_USER_SESSION_KEY = 1303,

      /// <summary>
      /// The password is too complex to be converted to a LAN Manager password. The LAN Manager password returned is a NULL string.
      /// </summary>
      ERROR_NULL_LM_PASSWORD = 1304,

      /// <summary>
      /// The revision level is unknown.
      /// </summary>
      ERROR_UNKNOWN_REVISION = 1305,

      /// <summary>
      /// Indicates two revision levels are incompatible.
      /// </summary>
      ERROR_REVISION_MISMATCH = 1306,

      /// <summary>
      /// This security ID may not be assigned as the owner of this object.
      /// </summary>
      ERROR_INVALID_OWNER = 1307,

      /// <summary>
      /// This security ID may not be assigned as the primary group of an object.
      /// </summary>
      ERROR_INVALID_PRIMARY_GROUP = 1308,

      /// <summary>
      /// An attempt has been made to operate on an impersonation token by a thread that is not currently impersonating a client.
      /// </summary>
      ERROR_NO_IMPERSONATION_TOKEN = 1309,

      /// <summary>
      /// The group may not be disabled.
      /// </summary>
      ERROR_CANT_DISABLE_MANDATORY = 1310,

      /// <summary>
      /// There are currently no logon servers available to service the logon request.
      /// </summary>
      ERROR_NO_LOGON_SERVERS = 1311,

      /// <summary>
      /// A specified logon session does not exist. It may already have been terminated.
      /// </summary>
      ERROR_NO_SUCH_LOGON_SESSION = 1312,

      /// <summary>
      /// A specified privilege does not exist.
      /// </summary>
      ERROR_NO_SUCH_PRIVILEGE = 1313,

      /// <summary>
      /// A required privilege is not held by the client.
      /// </summary>
      ERROR_PRIVILEGE_NOT_HELD = 1314,

      /// <summary>
      /// The name provided is not a properly formed account name.
      /// </summary>
      ERROR_INVALID_ACCOUNT_NAME = 1315,

      /// <summary>
      /// The specified account already exists.
      /// </summary>
      ERROR_USER_EXISTS = 1316,

      /// <summary>
      /// The specified account does not exist.
      /// </summary>
      ERROR_NO_SUCH_USER = 1317,

      /// <summary>
      /// The specified group already exists.
      /// </summary>
      ERROR_GROUP_EXISTS = 1318,

      /// <summary>
      /// The specified group does not exist.
      /// </summary>
      ERROR_NO_SUCH_GROUP = 1319,

      /// <summary>
      /// Either the specified user account is already a member of the specified group, or the specified group cannot be deleted because it contains a member.
      /// </summary>
      ERROR_MEMBER_IN_GROUP = 1320,

      /// <summary>
      /// The specified user account is not a member of the specified group account.
      /// </summary>
      ERROR_MEMBER_NOT_IN_GROUP = 1321,

      /// <summary>
      /// The last remaining administration account cannot be disabled or deleted.
      /// </summary>
      ERROR_LAST_ADMIN = 1322,

      /// <summary>
      /// Unable to update the password. The value provided as the current password is incorrect.
      /// </summary>
      ERROR_WRONG_PASSWORD = 1323,

      /// <summary>
      /// Unable to update the password. The value provided for the new password contains values that are not allowed in passwords.
      /// </summary>
      ERROR_ILL_FORMED_PASSWORD = 1324,

      /// <summary>
      /// Unable to update the password. The value provided for the new password does not meet the length, complexity, or history requirements of the domain.
      /// </summary>
      ERROR_PASSWORD_RESTRICTION = 1325,

      /// <summary>
      /// Incorrect user name or password.
      /// </summary>
      ERROR_LOGON_FAILURE = 1326,

      /// <summary>
      /// Account restrictions are preventing this user from signing in. For example: blank passwords aren't allowed, sign-in times are limited, or a policy restriction has been enforced.
      /// </summary>
      ERROR_ACCOUNT_RESTRICTION = 1327,

      /// <summary>
      /// Your account has time restrictions that keep you from signing in right now.
      /// </summary>
      ERROR_INVALID_LOGON_HOURS = 1328,

      /// <summary>
      /// This user isn't allowed to sign in to this computer.
      /// </summary>
      ERROR_INVALID_WORKSTATION = 1329,

      /// <summary>
      /// The password for this account has expired.
      /// </summary>
      ERROR_PASSWORD_EXPIRED = 1330,

      /// <summary>
      /// This user can't sign in because this account is currently disabled.
      /// </summary>
      ERROR_ACCOUNT_DISABLED = 1331,

      /// <summary>
      /// No mapping between account names and security IDs was done.
      /// </summary>
      ERROR_NONE_MAPPED = 1332,

      /// <summary>
      /// Too many local user identifiers (LUIDs) were requested at one time.
      /// </summary>
      ERROR_TOO_MANY_LUIDS_REQUESTED = 1333,

      /// <summary>
      /// No more local user identifiers (LUIDs) are available.
      /// </summary>
      ERROR_LUIDS_EXHAUSTED = 1334,

      /// <summary>
      /// The subauthority part of a security ID is invalid for this particular use.
      /// </summary>
      ERROR_INVALID_SUB_AUTHORITY = 1335,

      /// <summary>
      /// The access control list (ACL) structure is invalid.
      /// </summary>
      ERROR_INVALID_ACL = 1336,

      /// <summary>
      /// The security ID structure is invalid.
      /// </summary>
      ERROR_INVALID_SID = 1337,

      /// <summary>
      /// The security descriptor structure is invalid.
      /// </summary>
      ERROR_INVALID_SECURITY_DESCR = 1338,

      /// <summary>
      /// The inherited access control list (ACL) or access control entry (ACE) could not be built.
      /// </summary>
      ERROR_BAD_INHERITANCE_ACL = 1340,

      /// <summary>
      /// The server is currently disabled.
      /// </summary>
      ERROR_SERVER_DISABLED = 1341,

      /// <summary>
      /// The server is currently enabled.
      /// </summary>
      ERROR_SERVER_NOT_DISABLED = 1342,

      /// <summary>
      /// The value provided was an invalid value for an identifier authority.
      /// </summary>
      ERROR_INVALID_ID_AUTHORITY = 1343,

      /// <summary>
      /// No more memory is available for security information updates.
      /// </summary>
      ERROR_ALLOTTED_SPACE_EXCEEDED = 1344,

      /// <summary>
      /// The specified attributes are invalid, or incompatible with the attributes for the group as a whole.
      /// </summary>
      ERROR_INVALID_GROUP_ATTRIBUTES = 1345,

      /// <summary>
      /// Either a required impersonation level was not provided, or the provided impersonation level is invalid.
      /// </summary>
      ERROR_BAD_IMPERSONATION_LEVEL = 1346,

      /// <summary>
      /// Cannot open an anonymous level security token.
      /// </summary>
      ERROR_CANT_OPEN_ANONYMOUS = 1347,

      /// <summary>
      /// The validation information class requested was invalid.
      /// </summary>
      ERROR_BAD_VALIDATION_CLASS = 1348,

      /// <summary>
      /// The type of the token is inappropriate for its attempted use.
      /// </summary>
      ERROR_BAD_TOKEN_TYPE = 1349,

      /// <summary>
      /// Unable to perform a security operation on an object that has no associated security.
      /// </summary>
      ERROR_NO_SECURITY_ON_OBJECT = 1350,

      /// <summary>
      /// Configuration information could not be read from the domain controller, either because the machine is unavailable, or access has been denied.
      /// </summary>
      ERROR_CANT_ACCESS_DOMAIN_INFO = 1351,

      /// <summary>
      /// The security account manager (SAM) or local security authority (LSA) server was in the wrong state to perform the security operation.
      /// </summary>
      ERROR_INVALID_SERVER_STATE = 1352,

      /// <summary>
      /// The domain was in the wrong state to perform the security operation.
      /// </summary>
      ERROR_INVALID_DOMAIN_STATE = 1353,

      /// <summary>
      /// This operation is only allowed for the Primary Domain Controller of the domain.
      /// </summary>
      ERROR_INVALID_DOMAIN_ROLE = 1354,

      /// <summary>
      /// The specified domain either does not exist or could not be contacted.
      /// </summary>
      ERROR_NO_SUCH_DOMAIN = 1355,

      /// <summary>
      /// The specified domain already exists.
      /// </summary>
      ERROR_DOMAIN_EXISTS = 1356,

      /// <summary>
      /// An attempt was made to exceed the limit on the number of domains per server.
      /// </summary>
      ERROR_DOMAIN_LIMIT_EXCEEDED = 1357,

      /// <summary>
      /// Unable to complete the requested operation because of either a catastrophic media failure or a data structure corruption on the disk.
      /// </summary>
      ERROR_INTERNAL_DB_CORRUPTION = 1358,

      /// <summary>
      /// An internal error occurred.
      /// </summary>
      ERROR_INTERNAL_ERROR = 1359,

      /// <summary>
      /// Generic access types were contained in an access mask which should already be mapped to nongeneric types.
      /// </summary>
      ERROR_GENERIC_NOT_MAPPED = 1360,

      /// <summary>
      /// A security descriptor is not in the right format (absolute or self-relative).
      /// </summary>
      ERROR_BAD_DESCRIPTOR_FORMAT = 1361,

      /// <summary>
      /// The requested action is restricted for use by logon processes only. The calling process has not registered as a logon process.
      /// </summary>
      ERROR_NOT_LOGON_PROCESS = 1362,

      /// <summary>
      /// Cannot start a new logon session with an ID that is already in use.
      /// </summary>
      ERROR_LOGON_SESSION_EXISTS = 1363,

      /// <summary>
      /// A specified authentication package is unknown.
      /// </summary>
      ERROR_NO_SUCH_PACKAGE = 1364,

      /// <summary>
      /// The logon session is not in a state that is consistent with the requested operation.
      /// </summary>
      ERROR_BAD_LOGON_SESSION_STATE = 1365,

      /// <summary>
      /// The logon session ID is already in use.
      /// </summary>
      ERROR_LOGON_SESSION_COLLISION = 1366,

      /// <summary>
      /// A logon request contained an invalid logon type value.
      /// </summary>
      ERROR_INVALID_LOGON_TYPE = 1367,

      /// <summary>
      /// Unable to impersonate using a named pipe until data has been read from that pipe.
      /// </summary>
      ERROR_CANNOT_IMPERSONATE = 1368,

      /// <summary>
      /// The transaction state of a registry subtree is incompatible with the requested operation.
      /// </summary>
      ERROR_RXACT_INVALID_STATE = 1369,

      /// <summary>
      /// An internal security database corruption has been encountered.
      /// </summary>
      ERROR_RXACT_COMMIT_FAILURE = 1370,

      /// <summary>
      /// Cannot perform this operation on built-in accounts.
      /// </summary>
      ERROR_SPECIAL_ACCOUNT = 1371,

      /// <summary>
      /// Cannot perform this operation on this built-in special group.
      /// </summary>
      ERROR_SPECIAL_GROUP = 1372,

      /// <summary>
      /// Cannot perform this operation on this built-in special user.
      /// </summary>
      ERROR_SPECIAL_USER = 1373,

      /// <summary>
      /// The user cannot be removed from a group because the group is currently the user's primary group.
      /// </summary>
      ERROR_MEMBERS_PRIMARY_GROUP = 1374,

      /// <summary>
      /// The token is already in use as a primary token.
      /// </summary>
      ERROR_TOKEN_ALREADY_IN_USE = 1375,

      /// <summary>
      /// The specified local group does not exist.
      /// </summary>
      ERROR_NO_SUCH_ALIAS = 1376,

      /// <summary>
      /// The specified account name is not a member of the group.
      /// </summary>
      ERROR_MEMBER_NOT_IN_ALIAS = 1377,

      /// <summary>
      /// The specified account name is already a member of the group.
      /// </summary>
      ERROR_MEMBER_IN_ALIAS = 1378,

      /// <summary>
      /// The specified local group already exists.
      /// </summary>
      ERROR_ALIAS_EXISTS = 1379,

      /// <summary>
      /// Logon failure: the user has not been granted the requested logon type at this computer.
      /// </summary>
      ERROR_LOGON_NOT_GRANTED = 1380,

      /// <summary>
      /// The maximum number of secrets that may be stored in a single system has been exceeded.
      /// </summary>
      ERROR_TOO_MANY_SECRETS = 1381,

      /// <summary>
      /// The length of a secret exceeds the maximum length allowed.
      /// </summary>
      ERROR_SECRET_TOO_LONG = 1382,

      /// <summary>
      /// The local security authority database contains an internal inconsistency.
      /// </summary>
      ERROR_INTERNAL_DB_ERROR = 1383,

      /// <summary>
      /// During a logon attempt, the user's security context accumulated too many security IDs.
      /// </summary>
      ERROR_TOO_MANY_CONTEXT_IDS = 1384,

      /// <summary>
      /// Logon failure: the user has not been granted the requested logon type at this computer.
      /// </summary>
      ERROR_LOGON_TYPE_NOT_GRANTED = 1385,

      /// <summary>
      /// A cross-encrypted password is necessary to change a user password.
      /// </summary>
      ERROR_NT_CROSS_ENCRYPTION_REQUIRED = 1386,

      /// <summary>
      /// A member could not be added to or removed from the local group because the member does not exist.
      /// </summary>
      ERROR_NO_SUCH_MEMBER = 1387,

      /// <summary>
      /// A new member could not be added to a local group because the member has the wrong account type.
      /// </summary>
      ERROR_INVALID_MEMBER = 1388,

      /// <summary>
      /// Too many security IDs have been specified.
      /// </summary>
      ERROR_TOO_MANY_SIDS = 1389,

      /// <summary>
      /// A cross-encrypted password is necessary to change this user password.
      /// </summary>
      ERROR_LM_CROSS_ENCRYPTION_REQUIRED = 1390,

      /// <summary>
      /// Indicates an ACL contains no inheritable components.
      /// </summary>
      ERROR_NO_INHERITANCE = 1391,

      /// <summary>
      /// The file or directory is corrupted and unreadable.
      /// </summary>
      ERROR_FILE_CORRUPT = 1392,

      /// <summary>
      /// The disk structure is corrupted and unreadable.
      /// </summary>
      ERROR_DISK_CORRUPT = 1393,

      /// <summary>
      /// There is no user session key for the specified logon session.
      /// </summary>
      ERROR_NO_USER_SESSION_KEY = 1394,

      /// <summary>
      /// The service being accessed is licensed for a particular number of connections. No more connections can be made to the service at this time because there are already as many connections as the service can accept.
      /// </summary>
      ERROR_LICENSE_QUOTA_EXCEEDED = 1395,

      /// <summary>
      /// The target account name is incorrect.
      /// </summary>
      ERROR_WRONG_TARGET_NAME = 1396,

      /// <summary>
      /// Mutual Authentication failed. The server's password is out of date at the domain controller.
      /// </summary>
      ERROR_MUTUAL_AUTH_FAILED = 1397,

      /// <summary>
      /// There is a time and/or date difference between the client and server.
      /// </summary>
      ERROR_TIME_SKEW = 1398,

      /// <summary>
      /// This operation cannot be performed on the current domain.
      /// </summary>
      ERROR_CURRENT_DOMAIN_NOT_ALLOWED = 1399,

      /// <summary>
      /// Invalid window handle.
      /// </summary>
      ERROR_INVALID_WINDOW_HANDLE = 1400,

      /// <summary>
      /// Invalid menu handle.
      /// </summary>
      ERROR_INVALID_MENU_HANDLE = 1401,

      /// <summary>
      /// Invalid cursor handle.
      /// </summary>
      ERROR_INVALID_CURSOR_HANDLE = 1402,

      /// <summary>
      /// Invalid accelerator table handle.
      /// </summary>
      ERROR_INVALID_ACCEL_HANDLE = 1403,

      /// <summary>
      /// Invalid hook handle.
      /// </summary>
      ERROR_INVALID_HOOK_HANDLE = 1404,

      /// <summary>
      /// Invalid handle to a multiple-window position structure.
      /// </summary>
      ERROR_INVALID_DWP_HANDLE = 1405,

      /// <summary>
      /// Cannot create a top-level child window.
      /// </summary>
      ERROR_TLW_WITH_WSCHILD = 1406,

      /// <summary>
      /// Cannot find window class.
      /// </summary>
      ERROR_CANNOT_FIND_WND_CLASS = 1407,

      /// <summary>
      /// Invalid window; it belongs to other thread.
      /// </summary>
      ERROR_WINDOW_OF_OTHER_THREAD = 1408,

      /// <summary>
      /// Hot key is already registered.
      /// </summary>
      ERROR_HOTKEY_ALREADY_REGISTERED = 1409,

      /// <summary>
      /// Class already exists.
      /// </summary>
      ERROR_CLASS_ALREADY_EXISTS = 1410,

      /// <summary>
      /// Class does not exist.
      /// </summary>
      ERROR_CLASS_DOES_NOT_EXIST = 1411,

      /// <summary>
      /// Class still has open windows.
      /// </summary>
      ERROR_CLASS_HAS_WINDOWS = 1412,

      /// <summary>
      /// Invalid index.
      /// </summary>
      ERROR_INVALID_INDEX = 1413,

      /// <summary>
      /// Invalid icon handle.
      /// </summary>
      ERROR_INVALID_ICON_HANDLE = 1414,

      /// <summary>
      /// Using private DIALOG window words.
      /// </summary>
      ERROR_PRIVATE_DIALOG_INDEX = 1415,

      /// <summary>
      /// The list box identifier was not found.
      /// </summary>
      ERROR_LISTBOX_ID_NOT_FOUND = 1416,

      /// <summary>
      /// No wildcards were found.
      /// </summary>
      ERROR_NO_WILDCARD_CHARACTERS = 1417,

      /// <summary>
      /// Thread does not have a clipboard open.
      /// </summary>
      ERROR_CLIPBOARD_NOT_OPEN = 1418,

      /// <summary>
      /// Hot key is not registered.
      /// </summary>
      ERROR_HOTKEY_NOT_REGISTERED = 1419,

      /// <summary>
      /// The window is not a valid dialog window.
      /// </summary>
      ERROR_WINDOW_NOT_DIALOG = 1420,

      /// <summary>
      /// Control ID not found.
      /// </summary>
      ERROR_CONTROL_ID_NOT_FOUND = 1421,

      /// <summary>
      /// Invalid message for a combo box because it does not have an edit control.
      /// </summary>
      ERROR_INVALID_COMBOBOX_MESSAGE = 1422,

      /// <summary>
      /// The window is not a combo box.
      /// </summary>
      ERROR_WINDOW_NOT_COMBOBOX = 1423,

      /// <summary>
      /// Height must be less than 256.
      /// </summary>
      ERROR_INVALID_EDIT_HEIGHT = 1424,

      /// <summary>
      /// Invalid device context (DC) handle.
      /// </summary>
      ERROR_DC_NOT_FOUND = 1425,

      /// <summary>
      /// Invalid hook procedure type.
      /// </summary>
      ERROR_INVALID_HOOK_FILTER = 1426,

      /// <summary>
      /// Invalid hook procedure.
      /// </summary>
      ERROR_INVALID_FILTER_PROC = 1427,

      /// <summary>
      /// Cannot set nonlocal hook without a module handle.
      /// </summary>
      ERROR_HOOK_NEEDS_HMOD = 1428,

      /// <summary>
      /// This hook procedure can only be set globally.
      /// </summary>
      ERROR_GLOBAL_ONLY_HOOK = 1429,

      /// <summary>
      /// The journal hook procedure is already installed.
      /// </summary>
      ERROR_JOURNAL_HOOK_SET = 1430,

      /// <summary>
      /// The hook procedure is not installed.
      /// </summary>
      ERROR_HOOK_NOT_INSTALLED = 1431,

      /// <summary>
      /// Invalid message for single-selection list box.
      /// </summary>
      ERROR_INVALID_LB_MESSAGE = 1432,

      /// <summary>
      /// LB_SETCOUNT sent to non-lazy list box.
      /// </summary>
      ERROR_SETCOUNT_ON_BAD_LB = 1433,

      /// <summary>
      /// This list box does not support tab stops.
      /// </summary>
      ERROR_LB_WITHOUT_TABSTOPS = 1434,

      /// <summary>
      /// Cannot destroy object created by another thread.
      /// </summary>
      ERROR_DESTROY_OBJECT_OF_OTHER_THREAD = 1435,

      /// <summary>
      /// Child windows cannot have menus.
      /// </summary>
      ERROR_CHILD_WINDOW_MENU = 1436,

      /// <summary>
      /// The window does not have a system menu.
      /// </summary>
      ERROR_NO_SYSTEM_MENU = 1437,

      /// <summary>
      /// Invalid message box style.
      /// </summary>
      ERROR_INVALID_MSGBOX_STYLE = 1438,

      /// <summary>
      /// Invalid system-wide (SPI_*) parameter.
      /// </summary>
      ERROR_INVALID_SPI_VALUE = 1439,

      /// <summary>
      /// Screen already locked.
      /// </summary>
      ERROR_SCREEN_ALREADY_LOCKED = 1440,

      /// <summary>
      /// All handles to windows in a multiple-window position structure must have the same parent.
      /// </summary>
      ERROR_HWNDS_HAVE_DIFF_PARENT = 1441,

      /// <summary>
      /// The window is not a child window.
      /// </summary>
      ERROR_NOT_CHILD_WINDOW = 1442,

      /// <summary>
      /// Invalid GW_* command.
      /// </summary>
      ERROR_INVALID_GW_COMMAND = 1443,

      /// <summary>
      /// Invalid thread identifier.
      /// </summary>
      ERROR_INVALID_THREAD_ID = 1444,

      /// <summary>
      /// Cannot process a message from a window that is not a multiple document interface (MDI) window.
      /// </summary>
      ERROR_NON_MDICHILD_WINDOW = 1445,

      /// <summary>
      /// Popup menu already active.
      /// </summary>
      ERROR_POPUP_ALREADY_ACTIVE = 1446,

      /// <summary>
      /// The window does not have scroll bars.
      /// </summary>
      ERROR_NO_SCROLLBARS = 1447,

      /// <summary>
      /// Scroll bar range cannot be greater than MAXLONG.
      /// </summary>
      ERROR_INVALID_SCROLLBAR_RANGE = 1448,

      /// <summary>
      /// Cannot show or remove the window in the way specified.
      /// </summary>
      ERROR_INVALID_SHOWWIN_COMMAND = 1449,

      /// <summary>
      /// Insufficient system resources exist to complete the requested service.
      /// </summary>
      ERROR_NO_SYSTEM_RESOURCES = 1450,

      /// <summary>
      /// Insufficient system resources exist to complete the requested service.
      /// </summary>
      ERROR_NONPAGED_SYSTEM_RESOURCES = 1451,

      /// <summary>
      /// Insufficient system resources exist to complete the requested service.
      /// </summary>
      ERROR_PAGED_SYSTEM_RESOURCES = 1452,

      /// <summary>
      /// Insufficient quota to complete the requested service.
      /// </summary>
      ERROR_WORKING_SET_QUOTA = 1453,

      /// <summary>
      /// Insufficient quota to complete the requested service.
      /// </summary>
      ERROR_PAGEFILE_QUOTA = 1454,

      /// <summary>
      /// The paging file is too small for this operation to complete.
      /// </summary>
      ERROR_COMMITMENT_LIMIT = 1455,

      /// <summary>
      /// A menu item was not found.
      /// </summary>
      ERROR_MENU_ITEM_NOT_FOUND = 1456,

      /// <summary>
      /// Invalid keyboard layout handle.
      /// </summary>
      ERROR_INVALID_KEYBOARD_HANDLE = 1457,

      /// <summary>
      /// Hook type not allowed.
      /// </summary>
      ERROR_HOOK_TYPE_NOT_ALLOWED = 1458,

      /// <summary>
      /// This operation requires an interactive window station.
      /// </summary>
      ERROR_REQUIRES_INTERACTIVE_WINDOWSTATION = 1459,

      /// <summary>
      /// This operation returned because the timeout period expired.
      /// </summary>
      ERROR_TIMEOUT = 1460,

      /// <summary>
      /// Invalid monitor handle.
      /// </summary>
      ERROR_INVALID_MONITOR_HANDLE = 1461,

      /// <summary>
      /// Incorrect size argument.
      /// </summary>
      ERROR_INCORRECT_SIZE = 1462,

      /// <summary>
      /// The symbolic link cannot be followed because its type is disabled.
      /// </summary>
      ERROR_SYMLINK_CLASS_DISABLED = 1463,

      /// <summary>
      /// This application does not support the current operation on symbolic links.
      /// </summary>
      ERROR_SYMLINK_NOT_SUPPORTED = 1464,

      /// <summary>
      /// Windows was unable to parse the requested XML data.
      /// </summary>
      ERROR_XML_PARSE_ERROR = 1465,

      /// <summary>
      /// An error was encountered while processing an XML digital signature.
      /// </summary>
      ERROR_XMLDSIG_ERROR = 1466,

      /// <summary>
      /// This application must be restarted.
      /// </summary>
      ERROR_RESTART_APPLICATION = 1467,

      /// <summary>
      /// The caller made the connection request in the wrong routing compartment.
      /// </summary>
      ERROR_WRONG_COMPARTMENT = 1468,

      /// <summary>
      /// There was an AuthIP failure when attempting to connect to the remote host.
      /// </summary>
      ERROR_AUTHIP_FAILURE = 1469,

      /// <summary>
      /// Insufficient NVRAM resources exist to complete the requested service. A reboot might be required.
      /// </summary>
      ERROR_NO_NVRAM_RESOURCES = 1470,

      /// <summary>
      /// Unable to finish the requested operation because the specified process is not a GUI process.
      /// </summary>
      ERROR_NOT_GUI_PROCESS = 1471,

      /// <summary>
      /// The event log file is corrupted.
      /// </summary>
      ERROR_EVENTLOG_FILE_CORRUPT = 1500,

      /// <summary>
      /// No event log file could be opened, so the event logging service did not start.
      /// </summary>
      ERROR_EVENTLOG_CANT_START = 1501,

      /// <summary>
      /// The event log file is full.
      /// </summary>
      ERROR_LOG_FILE_FULL = 1502,

      /// <summary>
      /// The event log file has changed between read operations.
      /// </summary>
      ERROR_EVENTLOG_FILE_CHANGED = 1503,

      /// <summary>
      /// The specified task name is invalid.
      /// </summary>
      ERROR_INVALID_TASK_NAME = 1550,

      /// <summary>
      /// The specified task index is invalid.
      /// </summary>
      ERROR_INVALID_TASK_INDEX = 1551,

      /// <summary>
      /// The specified thread is already joining a task.
      /// </summary>
      ERROR_THREAD_ALREADY_IN_TASK = 1552,

      /// <summary>
      /// The Windows Installer Service could not be accessed. This can occur if the Windows Installer is not correctly installed. Contact your support personnel for assistance.
      /// </summary>
      ERROR_INSTALL_SERVICE_FAILURE = 1601,

      /// <summary>
      /// User cancelled installation.
      /// </summary>
      ERROR_INSTALL_USEREXIT = 1602,

      /// <summary>
      /// Fatal error during installation.
      /// </summary>
      ERROR_INSTALL_FAILURE = 1603,

      /// <summary>
      /// Installation suspended, incomplete.
      /// </summary>
      ERROR_INSTALL_SUSPEND = 1604,

      /// <summary>
      /// This action is only valid for products that are currently installed.
      /// </summary>
      ERROR_UNKNOWN_PRODUCT = 1605,

      /// <summary>
      /// Feature ID not registered.
      /// </summary>
      ERROR_UNKNOWN_FEATURE = 1606,

      /// <summary>
      /// Component ID not registered.
      /// </summary>
      ERROR_UNKNOWN_COMPONENT = 1607,

      /// <summary>
      /// Unknown property.
      /// </summary>
      ERROR_UNKNOWN_PROPERTY = 1608,

      /// <summary>
      /// Handle is in an invalid state.
      /// </summary>
      ERROR_INVALID_HANDLE_STATE = 1609,

      /// <summary>
      /// The configuration data for this product is corrupt. Contact your support personnel.
      /// </summary>
      ERROR_BAD_CONFIGURATION = 1610,

      /// <summary>
      /// Component qualifier not present.
      /// </summary>
      ERROR_INDEX_ABSENT = 1611,

      /// <summary>
      /// The installation source for this product is not available. Verify that the source exists and that you can access it.
      /// </summary>
      ERROR_INSTALL_SOURCE_ABSENT = 1612,

      /// <summary>
      /// This installation package cannot be installed by the Windows Installer service. You must install a Windows service pack that contains a newer version of the Windows Installer service.
      /// </summary>
      ERROR_INSTALL_PACKAGE_VERSION = 1613,

      /// <summary>
      /// Product is uninstalled.
      /// </summary>
      ERROR_PRODUCT_UNINSTALLED = 1614,

      /// <summary>
      /// SQL query syntax invalid or unsupported.
      /// </summary>
      ERROR_BAD_QUERY_SYNTAX = 1615,

      /// <summary>
      /// Record field does not exist.
      /// </summary>
      ERROR_INVALID_FIELD = 1616,

      /// <summary>
      /// The device has been removed.
      /// </summary>
      ERROR_DEVICE_REMOVED = 1617,

      /// <summary>
      /// Another installation is already in progress. Complete that installation before proceeding with this install.
      /// </summary>
      ERROR_INSTALL_ALREADY_RUNNING = 1618,

      /// <summary>
      /// This installation package could not be opened. Verify that the package exists and that you can access it, or contact the application vendor to verify that this is a valid Windows Installer package.
      /// </summary>
      ERROR_INSTALL_PACKAGE_OPEN_FAILED = 1619,

      /// <summary>
      /// This installation package could not be opened. Contact the application vendor to verify that this is a valid Windows Installer package.
      /// </summary>
      ERROR_INSTALL_PACKAGE_INVALID = 1620,

      /// <summary>
      /// There was an error starting the Windows Installer service user interface. Contact your support personnel.
      /// </summary>
      ERROR_INSTALL_UI_FAILURE = 1621,

      /// <summary>
      /// Error opening installation log file. Verify that the specified log file location exists and that you can write to it.
      /// </summary>
      ERROR_INSTALL_LOG_FAILURE = 1622,

      /// <summary>
      /// The language of this installation package is not supported by your system.
      /// </summary>
      ERROR_INSTALL_LANGUAGE_UNSUPPORTED = 1623,

      /// <summary>
      /// Error applying transforms. Verify that the specified transform paths are valid.
      /// </summary>
      ERROR_INSTALL_TRANSFORM_FAILURE = 1624,

      /// <summary>
      /// This installation is forbidden by system policy. Contact your system administrator.
      /// </summary>
      ERROR_INSTALL_PACKAGE_REJECTED = 1625,

      /// <summary>
      /// Function could not be executed.
      /// </summary>
      ERROR_FUNCTION_NOT_CALLED = 1626,

      /// <summary>
      /// Function failed during execution.
      /// </summary>
      ERROR_FUNCTION_FAILED = 1627,

      /// <summary>
      /// Invalid or unknown table specified.
      /// </summary>
      ERROR_INVALID_TABLE = 1628,

      /// <summary>
      /// Data supplied is of wrong type.
      /// </summary>
      ERROR_DATATYPE_MISMATCH = 1629,

      /// <summary>
      /// Data of this type is not supported.
      /// </summary>
      ERROR_UNSUPPORTED_TYPE = 1630,

      /// <summary>
      /// The Windows Installer service failed to start. Contact your support personnel.
      /// </summary>
      ERROR_CREATE_FAILED = 1631,

      /// <summary>
      /// The Temp folder is on a drive that is full or is inaccessible. Free up space on the drive or verify that you have write permission on the Temp folder.
      /// </summary>
      ERROR_INSTALL_TEMP_UNWRITABLE = 1632,

      /// <summary>
      /// This installation package is not supported by this processor type. Contact your product vendor.
      /// </summary>
      ERROR_INSTALL_PLATFORM_UNSUPPORTED = 1633,

      /// <summary>
      /// Component not used on this computer.
      /// </summary>
      ERROR_INSTALL_NOTUSED = 1634,

      /// <summary>
      /// This update package could not be opened. Verify that the update package exists and that you can access it, or contact the application vendor to verify that this is a valid Windows Installer update package.
      /// </summary>
      ERROR_PATCH_PACKAGE_OPEN_FAILED = 1635,

      /// <summary>
      /// This update package could not be opened. Contact the application vendor to verify that this is a valid Windows Installer update package.
      /// </summary>
      ERROR_PATCH_PACKAGE_INVALID = 1636,

      /// <summary>
      /// This update package cannot be processed by the Windows Installer service. You must install a Windows service pack that contains a newer version of the Windows Installer service.
      /// </summary>
      ERROR_PATCH_PACKAGE_UNSUPPORTED = 1637,

      /// <summary>
      /// Another version of this product is already installed. Installation of this version cannot continue. To configure or remove the existing version of this product, use Add/Remove Programs on the Control Panel.
      /// </summary>
      ERROR_PRODUCT_VERSION = 1638,

      /// <summary>
      /// Invalid command line argument. Consult the Windows Installer SDK for detailed command line help.
      /// </summary>
      ERROR_INVALID_COMMAND_LINE = 1639,

      /// <summary>
      /// Only administrators have permission to add, remove, or configure server software during a Terminal services remote session. If you want to install or configure software on the server, contact your network administrator.
      /// </summary>
      ERROR_INSTALL_REMOTE_DISALLOWED = 1640,

      /// <summary>
      /// The requested operation completed successfully. The system will be restarted so the changes can take effect.
      /// </summary>
      ERROR_SUCCESS_REBOOT_INITIATED = 1641,

      /// <summary>
      /// The upgrade cannot be installed by the Windows Installer service because the program to be upgraded may be missing, or the upgrade may update a different version of the program. Verify that the program to be upgraded exists on your computer and that you have the correct upgrade.
      /// </summary>
      ERROR_PATCH_TARGET_NOT_FOUND = 1642,

      /// <summary>
      /// The update package is not permitted by software restriction policy.
      /// </summary>
      ERROR_PATCH_PACKAGE_REJECTED = 1643,

      /// <summary>
      /// One or more customizations are not permitted by software restriction policy.
      /// </summary>
      ERROR_INSTALL_TRANSFORM_REJECTED = 1644,

      /// <summary>
      /// The Windows Installer does not permit installation from a Remote Desktop Connection.
      /// </summary>
      ERROR_INSTALL_REMOTE_PROHIBITED = 1645,

      /// <summary>
      /// Uninstallation of the update package is not supported.
      /// </summary>
      ERROR_PATCH_REMOVAL_UNSUPPORTED = 1646,

      /// <summary>
      /// The update is not applied to this product.
      /// </summary>
      ERROR_UNKNOWN_PATCH = 1647,

      /// <summary>
      /// No valid sequence could be found for the set of updates.
      /// </summary>
      ERROR_PATCH_NO_SEQUENCE = 1648,

      /// <summary>
      /// Update removal was disallowed by policy.
      /// </summary>
      ERROR_PATCH_REMOVAL_DISALLOWED = 1649,

      /// <summary>
      /// The XML update data is invalid.
      /// </summary>
      ERROR_INVALID_PATCH_XML = 1650,

      /// <summary>
      /// Windows Installer does not permit updating of managed advertised products. At least one feature of the product must be installed before applying the update.
      /// </summary>
      ERROR_PATCH_MANAGED_ADVERTISED_PRODUCT = 1651,

      /// <summary>
      /// The Windows Installer service is not accessible in Safe Mode. Please try again when your computer is not in Safe Mode or you can use System Restore to return your machine to a previous good state.
      /// </summary>
      ERROR_INSTALL_SERVICE_SAFEBOOT = 1652,

      /// <summary>
      /// A fail fast exception occurred. Exception handlers will not be invoked and the process will be terminated immediately.
      /// </summary>
      ERROR_FAIL_FAST_EXCEPTION = 1653,
    }

    public sealed class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
      private SafeTokenHandle() : base(true)
      {
      }

      protected override bool ReleaseHandle()
      {
        return Win32.CloseHandle(handle);
      }
    }

    #endregion Structures and data types

    #region Imports

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern uint SendMessage(IntPtr hWnd, uint msg, uint wParam, uint lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern uint SendMessage(IntPtr hWnd, uint msg, uint wParam, int[] lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern uint SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern uint SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, ref LVCOLUMN lParam);

    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr GetWindowDC(IntPtr handle);

    [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
    public static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr ReleaseDC(IntPtr handle, IntPtr hDC);

    [DllImport("Gdi32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

    [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DeleteDC(IntPtr hdc);

    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    public static extern int GetClassName(IntPtr hwnd, char[] className, int maxCount);

    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr GetWindow(IntPtr hwnd, uint uCmd);

    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsWindowVisible(IntPtr hwnd);

    [DllImport("user32", CharSet = CharSet.Auto)]
    public static extern int GetClientRect(IntPtr hwnd, [In, Out] ref RECT rect);

    [DllImport("user32", CharSet = CharSet.Auto)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool MoveWindow(IntPtr hwnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

    [DllImport("user32", CharSet = CharSet.Auto)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool UpdateWindow(IntPtr hwnd);

    [DllImport("user32", CharSet = CharSet.Auto)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ShowWindow(IntPtr hwnd, uint nCmdShow);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool RedrawWindow(IntPtr hWnd, [In] ref RECT lprcUpdate, IntPtr hrgnUpdate, uint flags);

    [DllImport("user32", CharSet = CharSet.Auto)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool InvalidateRect(IntPtr hwnd, ref RECT rect, bool bErase);

    [DllImport("user32", CharSet = CharSet.Auto)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ValidateRect(IntPtr hwnd, ref RECT rect);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetWindowRect(IntPtr hWnd, [In, Out] ref RECT rect);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetCursorPos(ref POINT lpPoint);

    [DllImport("user32.dll")]
    public static extern IntPtr WindowFromPoint(POINT lpPoint);

    [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref POINT pptDst,
      ref SIZE psize, IntPtr hdcSrc, ref POINT pprSrc, Int32 crKey, ref BLENDFUNCTION blendFunction, Int32 dwFlags);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, Int32 crKey, byte bAlpha, Int32 dwFlags);

    [DllImport("gdi32.dll", EntryPoint = "GdiAlphaBlend")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool AlphaBlend(IntPtr hdcDest, int nXOriginDest, int nYOriginDest,
        int nWidthDest, int nHeightDest,
        IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc,
        BLENDFUNCTION blendFunction);

    [DllImport("gdi32.dll", ExactSpelling = true)]
    public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

    [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DeleteObject(IntPtr hObject);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern uint RegisterWindowMessage(string lpString);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ReleaseCapture();

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool AttachConsole(UInt32 dwProcessId);

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool FreeConsole();

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool AllocConsole();

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetFileInformationByHandle(IntPtr hFile,
      out BY_HANDLE_FILE_INFORMATION lpFileInformation);

    [DllImport("kernel32.dll")]
    public static extern IntPtr GetStdHandle(UInt32 nStdHandle);

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetStdHandle(UInt32 nStdHandle, IntPtr hHandle);

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DuplicateHandle(IntPtr hSourceProcessHandle, IntPtr hSourceHandle,
      IntPtr hTargetProcessHandle, out IntPtr lpTargetHandle, UInt32 dwDesiredAccess,
      Boolean bInheritHandle, UInt32 dwOptions);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool CloseHandle(IntPtr hHandle);

    [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
    public static extern bool IsWow64Process([In] IntPtr hProcess, [Out] out bool lsSystemInfo);

    [DllImport("dwmapi.dll", PreserveSig = false)]
    public static extern void DwmEnableBlurBehindWindow(IntPtr hWnd, DWM_BLURBEHIND pBlurBehind);

    [DllImport("dwmapi.dll", PreserveSig = false)]
    public static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, MARGINS pMargins);

    [DllImport("dwmapi.dll", PreserveSig = false)]
    public static extern bool DwmIsCompositionEnabled();

    [DllImport("dwmapi.dll", PreserveSig = false)]
    public static extern void DwmEnableComposition(bool bEnable);

    [DllImport("dwmapi.dll", PreserveSig = false)]
    public static extern void DwmGetColorizationColor(out int pcrColorization,
      [MarshalAs(UnmanagedType.Bool)] out bool pfOpaqueBlend);

    [DllImport("dwmapi.dll", PreserveSig = false)]
    public static extern IntPtr DwmRegisterThumbnail(IntPtr dest, IntPtr source);

    [DllImport("dwmapi.dll", PreserveSig = false)]
    public static extern void DwmUnregisterThumbnail(IntPtr hThumbnail);

    [DllImport("dwmapi.dll", PreserveSig = false)]
    public static extern void DwmUpdateThumbnailProperties(IntPtr hThumbnail,
      DWM_THUMBNAIL_PROPERTIES props);

    [DllImport("dwmapi.dll", PreserveSig = false)]
    public static extern void DwmQueryThumbnailSourceSize(IntPtr hThumbnail, out Size size);

    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success), DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern bool CloseServiceHandle(IntPtr handle);

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern IntPtr OpenSCManager(string machineName, string databaseName, int access);

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern IntPtr CreateService(IntPtr databaseHandle, string serviceName,
      string displayName, int access, int serviceType, int startType, int errorControl,
      string binaryPath, string loadOrderGroup, IntPtr pTagId, string dependencies,
      string servicesStartName, string password);

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern bool ChangeServiceConfig(IntPtr serviceHandle, UInt32 nServiceType, UInt32 nStartType, UInt32 nErrorControl,
      string lpBinaryPathName, string lpLoadOrderGroup, IntPtr lpdwTagId, string lpDependencies, string lpServiceStartName, string lpPassword, string lpDisplayName);

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern bool DeleteService(IntPtr serviceHandle);

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern IntPtr OpenService(IntPtr databaseHandle, string serviceName, int access);

    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true, PreserveSig = true)]
    public static extern bool LookupAccountName(string lpSystemName, string lpAccountName,
      IntPtr psid, ref int cbsid, StringBuilder domainName, ref int cbdomainLength, ref int use);

    [DllImport("advapi32.dll", PreserveSig = true)]
    public static extern UInt32 LsaOpenPolicy(ref LSA_UNICODE_STRING SystemName,
      ref LSA_OBJECT_ATTRIBUTES ObjectAttributes, Int32 DesiredAccess, out IntPtr PolicyHandle);

    [DllImport("advapi32.dll", SetLastError = true, PreserveSig = true)]
    public static extern int LsaAddAccountRights(IntPtr PolicyHandle, IntPtr AccountSid,
      LSA_UNICODE_STRING[] UserRights, int CountOfRights);

    [DllImport("advapi32.dll")]
    public static extern int LsaNtStatusToWinError(int status);

    [DllImport("advapi32")]
    public static extern void FreeSid(IntPtr pSid);

    [DllImport("advapi32.dll")]
    public static extern int LsaClose(IntPtr ObjectHandle);

    [DllImport("advapi32.dll", SetLastError = true, PreserveSig = true)]
    public static extern int LsaEnumerateAccountRights(IntPtr PolicyHandle, IntPtr AccountSid, out IntPtr UserRights, out int CountOfRights);

    [DllImport("Kernel32", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr CreateFileMapping(IntPtr hFile, IntPtr lpAttributes,
      FileMapProtection flProtect, Int32 dwMaxSizeHi, Int32 dwMaxSizeLow, string lpName);

    [DllImport("Kernel32", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr MapViewOfFile(IntPtr hFileMapping, FileMapAccess dwDesiredAccess,
      Int32 dwFileOffsetHigh, Int32 dwFileOffsetLow, Int32 dwNumberOfBytesToMap);

    [DllImport("kernel32.dll")]
    public static extern bool FlushViewOfFile(IntPtr lpBaseAddress, Int32 dwNumberOfBytesToFlush);

    [DllImport("kernel32")]
    public static extern bool UnmapViewOfFile(IntPtr lpBaseAddress);

    [DllImport("kernel32.dll")]
    public static extern IntPtr CreateEvent(IntPtr lpEventAttributes, bool bManualReset, bool bInitialState, string lpName);

    [DllImport("Kernel32.dll", SetLastError = true)]
    public static extern IntPtr OpenEvent(uint dwDesiredAccess, bool bInheritHandle, string lpName);

    [DllImport("kernel32")]
    private static extern void GetSystemInfo(ref SYSTEM_INFO pSI);

    [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
        int dwLogonType, int dwLogonProvider, out SafeTokenHandle phToken);

    #endregion Imports

    #region Helper methods

    public static ushort HiWord(uint Number)
    {
      return (ushort)((Number >> 16) & 0xFFFF);
    }

    public static ushort HiWord(Int32 Number)
    {
      return (ushort)(((uint)Number >> 16) & 0xFFFF);
    }

    public static ushort LoWord(uint Number)
    {
      return (ushort)(Number & 0xFFFF);
    }

    public static ushort LoWord(Int32 Number)
    {
      return (ushort)(Number & 0xFFFF);
    }

    public static uint MakeLong(ushort LoWord, ushort HiWord)
    {
      return (uint)((HiWord << 16) | LoWord);
    }

    public static uint MakeLParam(ushort LoWord, ushort HiWord)
    {
      return (((uint)HiWord << 16) | LoWord);
    }

    public static POINT PointFromLParam(uint LParam)
    {
      return new POINT(LoWord(LParam), HiWord(LParam));
    }

    static public byte GetRValue(uint color)
    {
      return (byte)color;
    }

    static public byte GetGValue(uint color)
    {
      return ((byte)(((short)(color)) >> 8));
    }

    static public byte GetBValue(uint color)
    {
      return ((byte)((color) >> 16));
    }

    static public uint RGB(int r, int g, int b)
    {
      return ((uint)(((byte)(r) | ((short)((byte)(g)) << 8)) | (((short)(byte)(b)) << 16)));
    }

    public static void SetSortIcon(ListView listview, int column, SortOrder order)
    {
      if (listview.View != View.Details)
        return;

      IntPtr ColumnHeader = (IntPtr)SendMessage(listview.Handle, LVM_GETHEADER, 0, 0);

      for (int currentColumn = 0; currentColumn <= listview.Columns.Count - 1; currentColumn++)
      {
        IntPtr ColumnPtr = new IntPtr(currentColumn);
        LVCOLUMN lvColumn = new LVCOLUMN();
        lvColumn.mask = (Int32)HDI.FORMAT;
        SendMessage(ColumnHeader, HDM_GETITEM, ColumnPtr, ref lvColumn);

        if ((order != System.Windows.Forms.SortOrder.None) && (currentColumn == column))
        {
          switch (order)
          {
            case System.Windows.Forms.SortOrder.Ascending:
              lvColumn.fmt &= ~HDF_SORTDOWN;
              lvColumn.fmt |= HDF_SORTUP;
              break;

            case System.Windows.Forms.SortOrder.Descending:
              lvColumn.fmt &= ~HDF_SORTUP;
              lvColumn.fmt |= HDF_SORTDOWN;
              break;
          }
        }
        else
        {
          lvColumn.fmt &= ~HDF_SORTDOWN & ~HDF_SORTUP;
        }

        SendMessage(ColumnHeader, HDM_SETITEM, ColumnPtr, ref lvColumn);
      }
    }

    /// <summary>
    /// Initializes an LSA_UNICODE_STRING structure from a string
    /// </summary>
    public static LSA_UNICODE_STRING InitLsaString(string text)
    {
      // Unicode strings max. 32KB
      if (text.Length > 0x7ffe)
        throw new ArgumentException("String too long");
      LSA_UNICODE_STRING lus = new LSA_UNICODE_STRING();
      lus.Buffer = text;
      lus.Length = (ushort)(text.Length * sizeof(char));
      lus.MaximumLength = (ushort)(lus.Length + sizeof(char));
      return lus;
    }

    /// <summary>
    /// Gets the user SID
    /// </summary>
    public static IntPtr GetSIDInformation(string accountName)
    {
      IntPtr sid = IntPtr.Zero;
      int sidSize = 0;
      StringBuilder domainName = new StringBuilder();
      int nameSize = 0;
      int accountType = 0;

      LookupAccountName(String.Empty, accountName, sid, ref sidSize, domainName, ref nameSize, ref accountType);

      domainName = new StringBuilder(nameSize);
      sid = Marshal.AllocHGlobal(sidSize);

      bool result = LookupAccountName(String.Empty, accountName, sid, ref sidSize, domainName, ref nameSize, ref accountType);

      return sid;
    }

    public static LSA_UNICODE_STRING[] GetLsaStringArray(IntPtr pointer, int countOfItems)
    {
      LSA_UNICODE_STRING[] result = new LSA_UNICODE_STRING[countOfItems];
      try
      {
        IntPtr ptr = pointer;
        for (Int32 i = 0; i < countOfItems; i++)
        {
          result[i] = (LSA_UNICODE_STRING)Marshal.PtrToStructure(ptr, typeof(LSA_UNICODE_STRING));
          ptr = (IntPtr)(((long)ptr) + Marshal.SizeOf(typeof(LSA_UNICODE_STRING)));
        }
        return result;
      }
      catch
      {
        return null;
      }
    }

    #endregion Helper methods

    #region Console handling

    private static IntPtr hStdIn, hStdOut, hStdErr, hStdOutDup, hStdErrDup, hStdInDup;
    private static BY_HANDLE_FILE_INFORMATION bhfi;

    public static void RedirectConsole()
    {
      hStdIn = GetStdHandle(STD_INPUT_HANDLE);
      hStdOut = GetStdHandle(STD_OUTPUT_HANDLE);
      hStdErr = GetStdHandle(STD_ERROR_HANDLE);

      // Get current process handle
      IntPtr hProcess = Process.GetCurrentProcess().Handle;

      // Duplicate Stdout handle to save initial value
      DuplicateHandle(hProcess, hStdIn, hProcess, out hStdInDup, 0, true, DUPLICATE_SAME_ACCESS);

      // Duplicate Stdout handle to save initial value
      DuplicateHandle(hProcess, hStdOut, hProcess, out hStdOutDup, 0, true, DUPLICATE_SAME_ACCESS);

      // Duplicate Stderr handle to save initial value
      DuplicateHandle(hProcess, hStdErr, hProcess, out hStdErrDup, 0, true, DUPLICATE_SAME_ACCESS);

      // Attach to console window – this may modify the standard handles
      AttachConsole(ATTACH_PARENT_PROCESS);

      // Adjust the standard handles
      if (GetFileInformationByHandle(GetStdHandle(STD_INPUT_HANDLE), out bhfi))
        SetStdHandle(STD_INPUT_HANDLE, hStdInDup);
      else
        SetStdHandle(STD_INPUT_HANDLE, hStdIn);

      if (GetFileInformationByHandle(GetStdHandle(STD_OUTPUT_HANDLE), out bhfi))
        SetStdHandle(STD_OUTPUT_HANDLE, hStdOutDup);
      else
        SetStdHandle(STD_OUTPUT_HANDLE, hStdOut);

      if (GetFileInformationByHandle(GetStdHandle(STD_ERROR_HANDLE), out bhfi))
        SetStdHandle(STD_ERROR_HANDLE, hStdErrDup);
      else
        SetStdHandle(STD_ERROR_HANDLE, hStdErr);
    }

    public static void ReleaseConsole()
    {
      SetStdHandle(STD_INPUT_HANDLE, hStdIn);
      SetStdHandle(STD_OUTPUT_HANDLE, hStdOut);
      SetStdHandle(STD_ERROR_HANDLE, hStdErr);
      CloseHandle(hStdInDup);
      CloseHandle(hStdOutDup);
      CloseHandle(hStdErrDup);
      FreeConsole();
    }

    #endregion Console handling
  }

  #region SubClass Classing Handler Class

  public class SubClass : System.Windows.Forms.NativeWindow
  {
    public delegate int SubClassWndProcEventHandler(ref System.Windows.Forms.Message m);

    public event SubClassWndProcEventHandler SubClassedWndProc;

    private bool IsSubClassed = false;

    public SubClass(IntPtr Handle, bool _SubClass)
    {
      base.AssignHandle(Handle);
      this.IsSubClassed = _SubClass;
    }

    public bool SubClassed
    {
      get { return this.IsSubClassed; }
      set { this.IsSubClassed = value; }
    }

    protected override void WndProc(ref Message m)
    {
      if (this.IsSubClassed)
      {
        if (OnSubClassedWndProc(ref m) != 0)
          return;
      }
      base.WndProc(ref m);
    }

    public void CallDefaultWndProc(ref Message m)
    {
      base.WndProc(ref m);
    }

    private int OnSubClassedWndProc(ref Message m)
    {
      if (SubClassedWndProc != null)
      {
        return this.SubClassedWndProc(ref m);
      }

      return 0;
    }
  }

  #endregion SubClass Classing Handler Class
}
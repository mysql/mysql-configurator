// Copyright (c) 2016, 2019, Oracle and/or its affiliates. All rights reserved.
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License as
// published by the Free Software Foundation; version 2 of the
// License.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
// 02110-1301  USA

using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Core.Classes.MySqlWorkbench;
using MySql.Configurator.Core.Classes.VisualStyles;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Properties;
using MySql.Configurator.Core.Structs;
using MySql.Utility.Forms;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MySql.Configurator.Core.Forms
{
  /// <summary>
  /// Provides an interface to enter the password required by a MySQL connection.
  /// </summary>
  public partial class PasswordDialog : ValidatingBaseDialog
  {
    #region Constants

    /// <summary>
    /// The height in pixels of the dialog when used to enter a new password after an old one expired.
    /// </summary>
    public const int EXPANDED_DIALOG_HEIGHT = 325;

    /// <summary>
    /// The height in pixels of the dialog when used to ask for a connection's password.
    /// </summary>
    public const int REGULAR_DIALOG_HEIGHT = 255;

    /// <summary>
    /// The vertical space in pixels the top password label is shifted if the regular dialog is used.
    /// </summary>
    public const int TOP_LABEL_VERTICAL_DELTA = 5;

    #endregion Constants

    #region Fields

    /// <summary>
    /// Contains data about the password operation.
    /// </summary>
    private PasswordDialogFlags _passwordFlags;

    /// <summary>
    /// Flag indicating whether the password must be entered on the dialog.
    /// </summary>
    private bool _requirePassword;

    /// <summary>
    /// Flag indicating whether the connection is tested after setting the password.
    /// </summary>
    private readonly bool _testConnection;

    /// <summary>
    /// The connection to a MySQL server instance selected by users
    /// </summary>
    private MySqlWorkbenchConnection _wbConnection;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="PasswordDialog"/> class.
    /// </summary>
    /// <param name="wbConnection">A <see cref="MySqlWorkbenchConnection"/> object representing the connection to a MySQL server instance selected by users.</param>
    /// <param name="testConnection">Flag indicating whether the connection is tested after setting the password.</param>
    /// <param name="passwordExpired">Flag indicating if the dialog will be used to set a new password when an old one expired.</param>
    /// <param name="requirePassword">Flag indicating whether the password is required.</param>
    public PasswordDialog(MySqlWorkbenchConnection wbConnection, bool testConnection, bool passwordExpired, bool requirePassword = true)
    {
      _requirePassword = requirePassword;
      _testConnection = testConnection;
      _passwordFlags = new PasswordDialogFlags(wbConnection.Password);
      InitializeComponent();
      PasswordExpiredDialog = passwordExpired;
      _wbConnection = wbConnection;
      UserValueLabel.Text = _wbConnection.UserName;
      ConnectionValueLabel.Text = $@"{_wbConnection.Name} - {_wbConnection.HostIdentifier}";
      PasswordTextBox.Text = _wbConnection.Password;
      SetDialogInterface();
    }

    #region Static Properties

    /// <summary>
    /// Gets or sets the icon to be displayed in <see cref="PasswordDialog"/> dialogs, set by consumer applications.
    /// </summary>
    public static Icon ApplicationIcon { get; set; }

    /// <summary>
    /// Gets or sets the security logo to be displayed in <see cref="PasswordDialog"/> dialogs, set by consumer applications.
    /// </summary>
    public static Image SecurityLogo { get; set; }

    #endregion Static Properties

    #region Properties

    /// <summary>
    /// Gets a value indicating whether the dialog will be used to set a new password when an old one expired.
    /// </summary>
    public bool PasswordExpiredDialog { get; set; }

    /// <summary>
    /// Gets a structure with data about the password operation.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public PasswordDialogFlags PasswordFlags => _passwordFlags;

    /// <summary>
    /// Gets a value indicating whether the password is saved in the password vault.
    /// </summary>
    private bool StorePasswordSecurely => StorePasswordSecurelyCheckBox.Checked;

    #endregion Properties

    /// <summary>
    /// Shows the connection password dialog to users and returns the entered password.
    /// </summary>
    /// <param name="wbConnection">A <see cref="MySqlWorkbenchConnection"/> object representing the connection to a MySQL server instance selected by users.</param>
    /// <param name="testConnection">Flag indicating whether the connection is tested after setting the password.</param>
    /// <param name="requirePassword">Flag indicating whether the password is required.</param>
    /// <returns>A <see cref="PasswordDialogFlags"/> containing data about the operation.</returns>
    public static PasswordDialogFlags ShowConnectionPasswordDialog(MySqlWorkbenchConnection wbConnection, bool testConnection, bool requirePassword = true)
    {
      PasswordDialogFlags flags;
      using (var connectionPasswordDialog = new PasswordDialog(wbConnection, testConnection, false, requirePassword))
      {
        connectionPasswordDialog.ShowDialog();
        flags = connectionPasswordDialog.PasswordFlags;
      }

      return flags;
    }

    /// <summary>
    /// Shows the connection password dialog to users and returns the entered password.
    /// </summary>
    /// <param name="wbConnection">A <see cref="MySqlWorkbenchConnection"/> object representing the connection to a MySQL server instance selected by users.</param>
    /// <param name="testConnection">Flag indicating whether the connection is tested after setting the password.</param>
    /// <returns>A <see cref="PasswordDialogFlags"/> containing data about the operation.</returns>
    public static PasswordDialogFlags ShowExpiredPasswordDialog(MySqlWorkbenchConnection wbConnection, bool testConnection)
    {
      PasswordDialogFlags flags;
      using (var connectionPasswordDialog = new PasswordDialog(wbConnection, testConnection, true))
      {
        connectionPasswordDialog.ShowDialog();
        flags = connectionPasswordDialog.PasswordFlags;
      }

      return flags;
    }

    /// <summary>
    /// Handles the TextChanged event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    protected override void TextChangedHandler(object sender, EventArgs e)
    {
      // Looks like we could get rid of this empty override, but it is necessary to avoid an error of:
      // The method 'xxx' cannot be the method for an event because a class this class derives from already defines the method
      base.TextChangedHandler(sender, e);
    }

    /// <summary>
    /// Handles the TextValidated event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    /// <remarks>This event method is meant to be used with the <see cref="Control.Validated"/> event.</remarks>
    protected override void ValidatedHandler(object sender, EventArgs e)
    {
      // Looks like we could get rid of this empty override, but it is necessary to avoid an error of:
      // The method 'xxx' cannot be the method for an event because a class this class derives from already defines the method
      base.ValidatedHandler(sender, e);
    }

    /// <summary>
    /// Contains calls to methods that validate the given control's value.
    /// </summary>
    /// <returns>An error message or <c>null</c> / <see cref="string.Empty"/> if everything is valid.</returns>
    protected override string ValidateFields()
    {
      if (ErrorProviderControl == null)
      {
        return null;
      }

      string errorMessage = null;
      var text = ErrorProviderControl.Text;
      switch (ErrorProviderControl.Name)
      {
        case nameof(PasswordTextBox):
          errorMessage = _requirePassword && string.IsNullOrEmpty(text)
            ? Resources.PasswordDialogRequiredError
            : null;
          break;

        case nameof(NewPasswordTextBox):
        case nameof(ConfirmPasswordTextBox):
          errorMessage = _requirePassword && string.IsNullOrEmpty(text)
            ? Resources.PasswordDialogRequiredError
            : PasswordExpiredDialog
              && !string.Equals(NewPasswordTextBox.Text, ConfirmPasswordTextBox.Text, StringComparison.Ordinal)
                ? Resources.PasswordsMismatchErrorText
                : null;
          break;
      }

      return errorMessage;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="PasswordDialog"/> form is closing.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void PasswordDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (DialogResult == DialogResult.Cancel)
      {
        _passwordFlags.Cancelled = true;
        return;
      }

      if (PasswordExpiredDialog)
      {
        // Reset the password and if the reset is successful assign the new password to the local connection.
        _wbConnection.Password = PasswordTextBox.Text;
        try
        {
          _wbConnection.ResetPassword(ConfirmPasswordTextBox.Text);
        }
        catch (Exception ex)
        {
          Logger.LogException(ex, true, Resources.PasswordResetErrorDetailText, Resources.PasswordResetErrorTitleText);
          _passwordFlags.Cancelled = true;
          return;
        }

        _wbConnection.Password = ConfirmPasswordTextBox.Text;
      }
      else
      {
        _wbConnection.Password = PasswordTextBox.Text;
      }

      _passwordFlags.NewPassword = _wbConnection.Password;
      bool connectionSuccessful = false;
      if (_testConnection)
      {
        // Test the connection and if not successful revert the password to the one before the dialog was shown to the user.
        ConnectionResultType connectionResultType = _wbConnection.TestConnectionShowingErrors(false);
        _passwordFlags.ConnectionResultType = connectionResultType;
        switch(connectionResultType)
        {
          case ConnectionResultType.ConnectionSuccess:
          case ConnectionResultType.PasswordReset:
            connectionSuccessful = true;

            // If the password was reset within the connection test, then set it again in the new password flag.
            if (connectionResultType == ConnectionResultType.PasswordReset)
            {
              _passwordFlags.NewPassword = _wbConnection.Password;
            }

            break;

          case ConnectionResultType.PasswordExpired:
            // This status is set if the password was expired, and the dialog shown to the user to reset the password was cancelled, so exit.
            return;
        }
      }

      // If the connection was successful and the user chose to store the password, save it in the password vault.
      if (!StorePasswordSecurely
          || !connectionSuccessful
          || string.IsNullOrEmpty(_wbConnection.Password))
      {
        return;
      }

      string storedPassword = MySqlWorkbenchPasswordVault.FindPassword(_wbConnection.HostIdentifier, _wbConnection.UserName);
      if (storedPassword == null
          || storedPassword != _wbConnection.Password)
      {
        MySqlWorkbenchPasswordVault.StorePassword(_wbConnection.HostIdentifier, _wbConnection.UserName, _wbConnection.Password);
      }
    }

    /// <summary>
    /// Sets the dialog interface to use it to enter connection passwords or to enter a new password after an old one expired.
    /// </summary>
    private void SetDialogInterface()
    {
      if (ApplicationIcon != null)
      {
        Icon = ApplicationIcon;
      }

      LogoPictureBox.Image = SecurityLogo ?? Resources.MainLogo;
      Text = PasswordExpiredDialog
        ? Resources.ExpiredPasswordWindowTitleText
        : Resources.ConnectionPasswordWindowTitleText;
      EnterPasswordLabel.Text = PasswordExpiredDialog
        ? Resources.ExpiredPasswordLabelText
        : Resources.ConnectionPasswordLabelText;
      var standardDpiHeight = PasswordExpiredDialog
        ? EXPANDED_DIALOG_HEIGHT
        : REGULAR_DIALOG_HEIGHT;
      Height = HandleDpiSizeConversions
        ? (int)(this.GetDpiScaleY() * standardDpiHeight)
        : standardDpiHeight;
      EnterPasswordLabel.Height = PasswordExpiredDialog
        ? EnterPasswordLabel.Height
        : EnterPasswordLabel.Height / 2;
      EnterPasswordLabel.Location = new Point(EnterPasswordLabel.Location.X, EnterPasswordLabel.Location.Y + (PasswordExpiredDialog ? 0 : TOP_LABEL_VERTICAL_DELTA));
      PasswordTextBox.ReadOnly = PasswordExpiredDialog;
      NewPasswordLabel.Visible = PasswordExpiredDialog;
      NewPasswordTextBox.Visible = PasswordExpiredDialog;
      ConfirmPasswordLabel.Visible = PasswordExpiredDialog;
      ConfirmPasswordTextBox.Visible = PasswordExpiredDialog;
      PasswordLabel.Text = PasswordExpiredDialog
        ? Resources.OldPasswordLabelText
        : Resources.PasswordLabelText;
      StorePasswordSecurelyCheckBox.Location = PasswordExpiredDialog
        ?  StorePasswordSecurelyCheckBox.Location
        : NewPasswordTextBox.Location;
      UpdateAcceptButton();
    }
  }
}
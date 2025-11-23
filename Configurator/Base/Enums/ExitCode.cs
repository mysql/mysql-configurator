/* Copyright (c) 2024, 2025, Oracle and/or its affiliates.

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

using System.ComponentModel;

namespace MySql.Configurator.Base.Enums
{
  /// <summary>
  /// Defines the various exit codes when executing MySQL Configurator. 
  /// </summary>
  public enum ExitCode : uint
  {
    /// General exit codes.
    /// Reserved codes from 0 through 29.
    
    [Description("The operation completed successfully.")]
    Success = 0,

    [Description("Application must be executed with administrative privileges.")]
    NotRunningWithAdminPrivileges = 1,

    [Description("Invalid syntax at '{0}'.")]
    InvalidGeneralSyntax = 2,

    [Description("'{0}' is an invalid action name for option '{1}'")]
    InvalidAction = 3,

    [Description("'{0}' is an invalid option name.")]
    InvalidOption = 4,

    [Description("The value '{0}' is invalid for option '{1}'.")]
    InvalidOptionValue = 5,

    [Description("No value was provided for option '{0}'.")]
    OptionValueNotFound = 6,

    [Description("Invalid syntax at '{0}'. Syntax is expected to be in the form of '--option_name[=option_value]'.")]
    InvalidOptionSyntax = 7,

    [Description("Action '{0}' does not support option '{1}'.")]
    InvalidOptionForAction = 8,

    [Description("Option '{0}' is required.")]
    MissingRequiredOption = 9,

    [Description("A value was not provided for option '{0}'.")]
    MissingRequiredOptionValue = 10,

    [Description("Invalid syntax for a custom MySQL user at '{0}'.")]
    InvalidMySqlUserSyntax = 11,

    [Description("No argument was provided.")]
    NoArgument = 12,

    [Description("Option '{0}' is repeated.")]
    RepeatedOption = 13,

    [Description("Too many arguments provided. When not running in console mode only the 'action' option with values 'configure', 'reconfigure' or 'remove' is allowed.")]
    TooManyArguments = 14,

    [Description("Another instance of MySQL Configurator is already running.")]
    MultipleInstances = 15,

    [Description("An error ocurred when running MySQL Configurator. See the log for more details.")]
    GeneralError = 16,

    [Description("The operation has been aborted by the user.")]
    Abort = 17,

    [Description("An unexpected error has ocurred.")]
    BadImplementation = 18,

    [Description("The password file '{0}' does not exist.")]
    PasswordFileDoesNotExist = 19,

    [Description("The password file '{0}' could not be read. The following error was found: {1}.")]
    ErrorReadingPasswordFile = 20,

    [Description("The password file '{0}' is expected to only contain the 'password=' entry.")]
    PasswordFileInvalidContents = 21,

    [Description("Option '{0}' does not support being assigned a value.")]
    OptionDoesNotSupportValue = 22,

    [Description("Invalid syntax. Execute '--console --help' for details on the general syntax.")]
    InvalidGenericSyntax = 23,

    [Description("Option '{0}' can not be set to '{1}' because option '{2}' is already set to that value.")]
    ConflictingValues = 24,

    /// Configure related exit codes.
    /// Reserved codes from 30 through 39.

    [Description("The server has already been configured.")]
    ServerAlreadyConfigured = 30,

    [Description("The configuration operation failed.")]
    FailedConfiguration = 31,

    /// Reconfigure related exit codes.
    /// Reserved codes from 40 through 49.
    
    [Description("The server has not yet been configured.")]
    ServerNotConfigured = 40,

    [Description("The reconfiguration operation failed.")]
    FailedReconfiguration = 41,

    [Description("No options to reconfigure were provided.")]
    MissingOptionToReconfigure = 42,

    [Description("Failed to connect to the MySQL Server instance with error: {0}.")]
    FailedConnectionTestDuringReconfiguration = 43,

    /// Remove related exit codes.
    /// Reserved codes from 50 through 59.

    [Description("The server has not been configured, nothing to remove.")]
    NothingToRemove = 50,

    [Description("The remove operation failed.")]
    FailedRemoval = 51,

    /// Upgrade related exit codes.
    /// Reserved codes from 60 through 69.
    
    [Description("No running MySQL instances found. The instance that will be upgraded must be running.")]
    NoRunningInstancesFound = 60,

    [Description("Failed to connect to the specified MySQL Instance.")]
    FailedToConnect = 61,

    [Description("The upgrade operation failed.")]
    FailedUpgrade = 62,

    [Description("This upgrade scenario is not supported. Error is: {0}.")]
    UnsupportedUpgrade = 63,

    [Description("The specified ini file could not be read. Provide a valid server configuration file or select a different file to continue.")]
    InvalidIniFile = 64,

    /// Custom user related exit codes.
    /// Reserved codes from 70 through 89.
    
    [Description("An empty user block was provided for option '{0}'.")]
    EmptyUserBlock = 70,

    [Description("A user name must be enclosed in single quotes at '{0}' for option '{1}'.")]
    InvalidCustomUserUserNameValue = 71,

    [Description("A user password must be enclosed in single quotes at '{0}' for option '{1}'.")]
    InvalidCustomUserPasswordValue = 72,

    [Description("A user role must be enclosed in single quotes at '{0}' for option '{1}'.")]
    InvalidCustomUserRoleValue = 73,

    [Description("The user name, password/Windows Security Token, host, role and authentication plugin elements are mandatory at '{0}' for option '{1}'.")]
    InvalidCustomUserEmptyValue = 74,

    [Description("The Windows security token list is mandatory for users authentication with the Windows plugin at '{0}' for option '{1}'.")]
    InvalidCustomUserEmptyToken = 75,

    [Description("Expected a closing quote in the user block at '{0}' for option '{1}'.")]
    MissingCustomUserClosingQuote = 76,

    [Description("Too many elements found in the user block '{0}' for option '{1}'. A user block is expected to be in the form of '\"user_name\":\"password\":host:role[:windows_security_token]'.")]
    TooManyElementsInUserBlock = 77,

    [Description("The root user can't be added as a user because it already exists for option '{0}'.")]
    InvalidCustomUserRootUser = 78,

    [Description("'{0}' is an invalid user role for option '{1}'.")]
    InvalidCustomUserRole = 79,

    [Description("'{0}' is an invalid user name for option '{1}'. Error message is '{2}'.")]
    InvalidCustomUserName = 80,

    [Description("An invalid password has been provided for option '{0}'. Error message is '{1}'.")]
    InvalidCustomUserPassword = 81,

    [Description("The user or group name '{0}' could not be found for option '{1}'. Error message is '{2}'.")]
    CustomUserSecurityTokenNotFound = 82,

    /// Validation related exit codes.
    /// Reserved codes from 90 through 109.

    [Description("The provided root password is invalid. {0}.")]
    RootPasswordInvalidFormat = 90,

    [Description("The password file path '{0}' does not exist.")]
    ErrorPasswordFileDoesNotExist = 91,

    [Description("Option '{0}' can only be used when option '{1}' is set to '{2}'.")]
    ValueNotMatchingCondition = 92,

    [Description("Options server-file-full-permissions-list and/or server-file-no-permissions-list were not provided.")]
    NoAccessListsProvided = 93,

    [Description("The provided user/group name '{0}' is not a valid Windows user/group.")]
    InvalidUserGroupName = 94,
  }
}

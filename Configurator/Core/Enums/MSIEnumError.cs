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

using System.ComponentModel;

namespace MySql.Configurator.Core.Enums
{
  /// <summary>
  /// Common Installer database API return values defined.
  /// </summary>
  public enum MsiEnumError : uint
  {
    /// <summary>
    /// Success
    /// </summary>
    [Description("Success")]
    Success = 0,

    /// <summary>
    /// Invalid Handle
    /// </summary>
    [Description("Invalid Handle")]
    InvalidHandle = 6,

    /// <summary>
    /// Not enough memory
    /// </summary>
    [Description("Not enough memory")]
    NotEnoughMemory = 8,

    /// <summary>
    /// Invalid data
    /// </summary>
    [Description("Invalid data")]
    InvalidData = 13,

    /// <summary>
    /// Invalid parameter
    /// </summary>
    [Description("Invalid parameter")]
    InvalidParameter = 87,

    /// <summary>
    /// Call not implemented
    /// </summary>
    [Description("Call not implemented")]
    CallNotImplemented = 120,

    /// <summary>
    /// More data
    /// </summary>
    [Description("More data")]
    MoreData = 234,

    /// <summary>
    /// No more items
    /// </summary>
    [Description("No more items")]
    NoMoreItems = 259,

    /// <summary>
    /// Application help block
    /// </summary>
    [Description("Application help block")]
    ApphelpBlock = 1259,

    /// <summary>
    /// Install service failed
    /// </summary>
    [Description("Install service failed")]
    InstallServiceFailure = 1601,

    /// <summary>
    /// Install user exit
    /// </summary>
    [Description("Install user exit")]
    InstallUserexit = 1602,

    /// <summary>
    /// Install failure
    /// </summary>
    [Description("Install failure")]
    InstallFailure = 1603,

    /// <summary>
    /// Install suspect
    /// </summary>
    [Description("Install suspect")]
    InstallSuspend = 1604,

    /// <summary>
    /// Unknown product
    /// </summary>
    [Description("Unknown product")]
    UnknownProduct = 1605,

    /// <summary>
    /// Unknown feature
    /// </summary>
    [Description("Unknown feature")]
    UnknownFeature = 1606,

    /// <summary>
    /// Unknown component
    /// </summary>
    [Description("Unknown component")]
    UnknownComponent = 1607,

    /// <summary>
    /// Unknown property
    /// </summary>
    [Description("Unknown property")]
    UnknownProperty = 1608,

    /// <summary>
    /// Invalid Handle state
    /// </summary>
    [Description("Invalid Handle state")]
    InvalidHandleState = 1609,

    /// <summary>
    /// Bad configuration
    /// </summary>
    [Description("Bad configuration")]
    BadConfiguration = 1610,

    /// <summary>
    /// Absent index
    /// </summary>
    [Description("Absent index")]
    IndexAbsent = 1611,

    /// <summary>
    /// Missing install source
    /// </summary>
    [Description("Missing install source")]
    InstallSourceAbsent = 1612,

    /// <summary>
    /// Install package version
    /// </summary>
    [Description("Install package version")]
    InstallPackageVersion = 1613,

    /// <summary>
    /// Product uninstalled
    /// </summary>
    [Description("Product uninstalled")]
    ProductUninstalled = 1614,

    /// <summary>
    /// Bad query syntax
    /// </summary>
    [Description("Bad query syntax")]
    BadQuerySyntax = 1615,

    /// <summary>
    /// Invalid field
    /// </summary>
    [Description("Invalid field")]
    InvalidField = 1616,

    /// <summary>
    /// Install already running
    /// </summary>
    [Description("Install already running")]
    InstallAlreadyRunning = 1618,

    /// <summary>
    /// Install package open failed
    /// </summary>
    [Description("Install package open failed")]
    InstallPackageOpenFailed = 1619,

    /// <summary>
    /// Install package invalid
    /// </summary>
    [Description("Install package invalid")]
    InstallPackageInvalid = 1620,

    /// <summary>
    /// Install UI failure
    /// </summary>
    [Description("Install UI failure")]
    InstallUiFailure = 1621,

    /// <summary>
    /// Install log failure
    /// </summary>
    [Description("Install log failure")]
    InstallLogFailure = 1622,

    /// <summary>
    /// Install language unsupported
    /// </summary>
    [Description("Install language unsupported")]
    InstallLanguageUnsupported = 1623,

    /// <summary>
    /// Install transform failure
    /// </summary>
    [Description("Install transform failure")]
    InstallTransformFailure = 1624,

    /// <summary>
    /// Install package rejected
    /// </summary>
    [Description("Install package rejected")]
    InstallPackageRejected = 1625,

    /// <summary>
    /// Function not called
    /// </summary>
    [Description("Function not called")]
    FunctionNotCalled = 1626,

    /// <summary>
    /// Function failed
    /// </summary>
    [Description("Function failed")]
    FunctionFailed = 1627,

    /// <summary>
    /// Invalid table
    /// </summary>
    [Description("Invalid table")]
    InvalidTable = 1628,

    /// <summary>
    /// Data type mismatch
    /// </summary>
    [Description("Data type mismatch")]
    DatatypeMismatch = 1629,

    /// <summary>
    /// Unsupported type
    /// </summary>
    [Description("Unsupported type")]
    UnsupportedType = 1630,

    /// <summary>
    /// Create failed
    /// </summary>
    [Description("Create failed")]
    CreateFailed = 1631,

    /// <summary>
    /// Install temp un-writable
    /// </summary>
    [Description("Install temp un-writable")]
    InstallTempUnwritable = 1632,

    /// <summary>
    /// Install platform unsupported
    /// </summary>
    [Description("Install platform unsupported")]
    InstallPlatformUnsupported = 1633,

    /// <summary>
    /// Install not used
    /// </summary>
    [Description("Install not used")]
    InstallNotused = 1634,

    /// <summary>
    /// Patch package open failed
    /// </summary>
    [Description("Patch package open failed")]
    PatchPackageOpenFailed = 1635,

    /// <summary>
    /// Patch package invalid
    /// </summary>
    [Description("Patch package invalid")]
    PatchPackageInvalid = 1636,

    /// <summary>
    /// Patch package unsupported
    /// </summary>
    [Description("Patch package unsupported")]
    PatchPackageUnsupported = 1637,

    /// <summary>
    /// Product version
    /// </summary>
    [Description("Product version")]
    ProductVersion = 1638,

    /// <summary>
    /// Invalid command line
    /// </summary>
    [Description("Invalid command line")]
    InvalidCommandLine = 1639,

    /// <summary>
    /// Install remote disallowed
    /// </summary>
    [Description("Install remote disallowed")]
    InstallRemoteDisallowed = 1640,

    /// <summary>
    /// Success reboot initiated
    /// </summary>
    [Description("Success reboot initiated")]
    SuccessRebootInitiated = 1641,

    /// <summary>
    /// Patch target not found
    /// </summary>
    [Description("Patch target not found")]
    PatchTargetNotFound = 1642,

    /// <summary>
    /// Patch package rejected
    /// </summary>
    [Description("Patch package rejected")]
    PatchPackageRejected = 1643,

    /// <summary>
    /// Install transform rejected
    /// </summary>
    [Description("Install transform rejected")]
    InstallTransformRejected = 1644,

    /// <summary>
    /// Install remote prohibited
    /// </summary>
    [Description("Install remote prohibited")]
    InstallRemoteProhibited = 1645,

    /// <summary>
    /// Patch remove unsupported
    /// </summary>
    [Description("Patch remove unsupported")]
    PatchRemovalUnsupported = 1646,

    /// <summary>
    /// Unknown patch
    /// </summary>
    [Description("Unknown patch")]
    UnknownPatch = 1647,

    /// <summary>
    /// Patch no sequence
    /// </summary>
    [Description("Patch no sequence")]
    PatchNoSequence = 1648,

    /// <summary>
    /// Patch removal disallowed
    /// </summary>
    [Description("Patch removal disallowed")]
    PatchRemovalDisallowed = 1649,

    /// <summary>
    /// Invalid XML patch
    /// </summary>
    [Description("Invalid XML patch")]
    InvalidPatchXml = 1650,

    /// <summary>
    /// Patch managed advertised product
    /// </summary>
    [Description("Patch managed advertised product")]
    PatchManagedAdvertisedProduct = 1651,

    /// <summary>
    /// Install service safe reboot
    /// </summary>
    [Description("Install service safe reboot")]
    InstallServiceSafeboot = 1652,

    /// <summary>
    /// Rollback disabled
    /// </summary>
    [Description("Rollback disabled")]
    RollbackDisabled = 1653,

    /// <summary>
    /// Success reboot required.
    /// </summary>
    [Description("Success, reboot required.")]
    SuccessRebootRequired = 3010
  }

}

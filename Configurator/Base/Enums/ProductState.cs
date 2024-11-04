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

namespace MySql.Configurator.Base.Enums
{
  /// <summary>
  /// The current state of the product with regard to the location machine.
  /// </summary>
  public enum ProductState
  {
    /// <summary>
    /// Unknown state
    /// </summary>
    Unknown = -1,

    /// <summary>
    /// Available for remote download
    /// </summary>
    WebRemote = 1,

    /// <summary>
    /// Found in the Product cache
    /// </summary>
    FoundLocal,

    /// <summary>
    /// This product's package will perform an upgrade.
    /// </summary>
    WillPerformUpgrade,

    /// <summary>
    /// Server installation currently installed
    /// </summary>
    CurrentlyInstalled,

    /// <summary>
    /// Machine has a newer version of this product installed
    /// </summary>
    NewerInstalled,

    /// <summary>
    /// Download started
    /// </summary>
    DownloadStarted,

    /// <summary>
    /// Download in progress
    /// </summary>
    DownloadInProgress,

    /// <summary>
    /// Download success
    /// </summary>
    DownloadSuccess,

    /// <summary>
    /// Download error
    /// </summary>
    DownloadError,

    /// <summary>
    /// Download bad zip file
    /// </summary>
    DownloadBadArchive,

    /// <summary>
    /// Download canceled
    /// </summary>
    DownloadCancelled,

    /// <summary>
    /// Download remote path not found
    /// </summary>
    DownloadNoRemotePath,

    /// <summary>
    /// Install started
    /// </summary>
    InstallStarted,

    /// <summary>
    /// Install progress
    /// </summary>
    InstallInProgress,

    /// <summary>
    /// Install success
    /// </summary>
    InstallSuccess,

    /// <summary>
    /// Install error
    /// </summary>
    InstallError,

    /// <summary>
    /// Remove started
    /// </summary>
    RemoveStarted,

    /// <summary>
    /// Remote in progress
    /// </summary>
    RemoveInProgress,

    /// <summary>
    /// Remove successful
    /// </summary>
    RemoveSuccess,

    /// <summary>
    /// Remote failed
    /// </summary>
    RemoveFailed,

    /// <summary>
    /// Update started
    /// </summary>
    UpdateStarted,

    /// <summary>
    /// Update in progress
    /// </summary>
    UpdateInProgress,

    /// <summary>
    /// Update success
    /// </summary>
    UpdateSuccess,

    /// <summary>
    /// Update failed
    /// </summary>
    UpdateFailed,

    /// <summary>
    /// Requires reboot to continue
    /// </summary>
    RebootRequired
  }

}

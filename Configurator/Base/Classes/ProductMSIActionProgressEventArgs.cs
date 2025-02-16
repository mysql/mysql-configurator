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

using System;
using System.ComponentModel;
using System.Net;

namespace WexInstaller.Core
{

  ///// <summary>
  ///// (MSI) Action Progress
  ///// </summary>
  ///// <param name="sender">The sender</param>
  ///// <param name="pe">The arguments</param>
  //public delegate void ProductMSIActionProgressHandler(object sender, ProductMSIActionProgressEventArgs pe);

  ///// <summary>C:\Users\Reggie\Work\wex\installer\trunk-dev\WexInstaller\Classes\
  ///// (MSI) Action Complete
  ///// </summary>
  ///// <param name="sender">The sender</param>
  ///// <param name="pe">The arguments</param>
  //public delegate void ProductMSIActionCompleteHandler(object sender, ProductMSIActionCompletedEventArgs pe);

  ///// <summary>
  ///// Download Progress
  ///// </summary>
  ///// <param name="sender">The sender</param>
  ///// <param name="de">The arguments</param>
  //public delegate void DownloadProductProgressHandler(object sender, DownloadProgressChangedEventArgs de);

  ///// <summary>
  ///// Download Complete
  ///// </summary>
  ///// <param name="sender">The sender</param>
  ///// <param name="ae">The arguments</param>
  //public delegate void DownloadProductCompleteHandler(object sender, AsyncCompletedEventArgs ae);

  /// <summary>
  /// Product (MSI) Action event argument
  /// </summary>
  //public class ProductMSIActionProgressEventArgs : ProgressChangedEventArgs
  //{
  //  /// <summary>
  //  /// Initializes a new instance of the <see cref="ProductMSIActionProgressEventArgs" /> class.
  //  /// </summary>
  //  /// <param name="progressPercentage">Percent complete</param>
  //  /// <param name="userToken">User token object</param>
  //  public ProductMSIActionProgressEventArgs(int progressPercentage, object userToken)
  //    : base(progressPercentage, userToken)
  //  {
  //  }

  //  /// <summary>
  //  /// Gets or sets the event message
  //  /// </summary>
  //  public string Message { get; set; }

  //  /// <summary>
  //  /// Gets or sets a value indicating whether this event is verbose
  //  /// </summary>
  //  public bool Verbose { get; set; }
  //}
}

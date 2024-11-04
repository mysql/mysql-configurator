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
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using MySql.Configurator.Base.Enums;

namespace MySql.Configurator.Core.MSI
{
  /// <summary>
  /// Custom MSI UI Handler
  /// </summary>
  /// <param name="context">A context</param>
  /// <param name="messageType">The type of Message</param>
  /// <param name="message">The message</param>
  /// <returns>A DialogResult</returns>
  public delegate DialogResult InstallUIHandler(IntPtr context, uint messageType, string message);
  public delegate DialogResult InstallUIHandlerRecord(IntPtr context, uint messageType, int hRecord);

  /// <summary>
  /// Imports for the Installer (MSI) Database dynamic link library
  /// </summary>
  public class MsiInterop
  {
    public static string GetProperty(Guid packageCode, string property)
    {
      // set install location property
      StringBuilder buf = new StringBuilder(200);
      int len = 200;
      MsiEnumError ret = MsiGetProductInfo(packageCode.ToString("B"), property, buf, ref len);
      if (ret == MsiEnumError.MoreData)
      {
        buf.Capacity = ++len;
        ret = MsiGetProductInfo(packageCode.ToString("B"), property, buf, ref len);
      }

      if (ret == MsiEnumError.Success)
      {
        return buf.ToString();
      }

      // TODO:  throw exception here
      return null;
    }

    /// <summary>
    /// Used to get a certain property from an (MSI) file using it's FileName.
    /// </summary>
    /// <param name="msiFile">Full path to the MSI file.</param>
    /// <param name="propertyName">Name of the property</param>
    /// <returns>true on success, otherwise false</returns>
    public static string GetPropertyInfo(string msiFile, string propertyName)
    {
      int productInfoLength = 512;
      StringBuilder productInfo = new StringBuilder(productInfoLength);
      UIntPtr packageHandle = UIntPtr.Zero;
      MsiSetInternalUI(InstallUILevel.None, IntPtr.Zero);
      MsiEnumError status = MsiOpenPackageEx(msiFile, 1, ref packageHandle);
      if (status == MsiEnumError.Success)
      {
        status = MsiGetProductProperty(packageHandle, propertyName, productInfo, ref productInfoLength);
      }

      MsiCloseHandle(packageHandle);
      return (status == MsiEnumError.Success)
        ? productInfo.ToString()
        : string.Empty;
    }

    [DllImport("msi.dll")]
    public static extern int MsiSetInternalUI(InstallUILevel uiLevel, IntPtr hwnd);
    [DllImport("msi.dll", CharSet = CharSet.Unicode)]
    public static extern MsiEnumError MsiConfigureProductEx(string productCode, InstallLevel installLevel, InstallState installState, string commandLine);
    [DllImport("msi.dll")]
    internal static extern InstallState MsiQueryProductState(string productCode);
    [DllImport("msi.dll")]
    internal static extern MsiEnumError MsiOpenProduct(string productId, ref UIntPtr msiHandle);
    [DllImport("msi.dll")]
    internal static extern MsiEnumError MsiGetProductInfo(string productId, string propertyName, StringBuilder valueBuffer, ref int valueBufferLength);
    [DllImport("msi.dll")]
    internal static extern MsiEnumError MsiOpenPackageEx(string packagePath, int openOptions, ref UIntPtr msiHandle);
    [DllImport("msi.dll")]
    internal static extern MsiEnumError MsiGetProductProperty(UIntPtr msiHandle, string propertyName, StringBuilder valueBuffer, ref int valueBufferLength);
    [DllImport("msi.dll")]
    internal static extern MsiEnumError MsiCloseHandle(UIntPtr msiHandle);
    [DllImport("msi.dll")]
    public static extern MsiEnumError MsiEnumRelatedProducts(string upgradeCode, uint reserved, uint productIndex, StringBuilder productBuf);
    [DllImport("msi.dll")]
    internal static extern InstallState MsiQueryFeatureState(string productCode, string featureName);
    [DllImport("msi.dll")]
    internal static extern InstallState MsiGetComponentPath(string productCode, string componentId, StringBuilder path, ref uint pathLen);

    // Database functions
    [DllImport("msi.dll")]
    internal static extern UIntPtr MsiGetActiveDatabase(UIntPtr installHandle);
    [DllImport("msi.dll")]
    internal static extern MsiEnumError MsiDatabaseOpenView(UIntPtr database, string query, ref UIntPtr view);
    [DllImport("msi.dll")]
    internal static extern MsiEnumError MsiViewExecute(UIntPtr view, UIntPtr record);
    [DllImport("msi.dll")]
    internal static extern MsiEnumError MsiViewFetch(UIntPtr view, ref UIntPtr record);
    [DllImport("msi.dll")]
    internal static extern MsiEnumError MsiRecordGetString(UIntPtr record, uint field, StringBuilder valueBuf, ref int valueBufLength);
    [DllImport("msi.dll", EntryPoint = "MsiOpenDatabaseW", CharSet = CharSet.Unicode)]
    internal static extern MsiEnumError MsiOpenDatabase(string databasePath, IntPtr persist, out UIntPtr database);
  }
}

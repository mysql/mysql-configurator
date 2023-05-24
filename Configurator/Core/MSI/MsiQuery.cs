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
using System.Diagnostics;
using System.Text;
using MySql.Configurator.Core.Enums;

namespace MySql.Configurator.Core.MSI
{
  public class MsiQuery : IDisposable
  {
    private UIntPtr _msiHandle = UIntPtr.Zero;
    private UIntPtr _msiDb = UIntPtr.Zero;
    private UIntPtr _view = UIntPtr.Zero;
    private UIntPtr _results = UIntPtr.Zero;
    private Guid _productCode;

    /// <summary>
    /// Path to the MSI file to query.
    /// </summary>
    private string _msiPath;

    private static FileVersionInfo _msiVersion;

    static MsiQuery()
    {
      _msiVersion = FileVersionInfo.GetVersionInfo(Environment.SystemDirectory + "\\msi.dll");
    }

    /// <summary>
    /// Initializes an instance of this class with the provided product code.
    /// </summary>
    /// <param name="productCode">The product code.</param>
    public MsiQuery(Guid productCode)
    {
      _productCode = productCode;
    }

    /// <summary>
    /// Initializes an instance of this class with the provided path to the MSI file.
    /// </summary>
    /// <param name="msiPath">The path to the MSI file.</param>
    public MsiQuery(string msiPath)
    {
      _msiPath = msiPath;
    }

    /// <summary>
    /// Opens the database for an installed product if a product code has been previously provided,
    /// otherwise if a path to an MSI file was provided it searches for the specified file and opens the database.
    /// </summary>
    /// <returns><c>true</c> if the database was successfully opened; otherwise, <c>false</c>.</returns>
    private bool OpenDb()
    {
      InstallUILevel level = InstallUILevel.None;
      if (_msiVersion.FileMajorPart >= 5)
      {
        // Allow to installer to elevate, even in silent mode.
        level |= InstallUILevel.UACOnly;
      }

      MsiInterop.MsiSetInternalUI(level, IntPtr.Zero);
      MsiEnumError result;

      if (!string.IsNullOrEmpty(_msiPath))
      {
        //Open database as read only.
        int openMode = 0;
        var persist = new IntPtr(openMode);
        result = MsiInterop.MsiOpenDatabase(_msiPath, persist, out _msiHandle);
        _msiDb = _msiHandle;

        return result == MsiEnumError.Success;
      }
      else
      {
        result = MsiInterop.MsiOpenProduct(_productCode.ToString("B"), ref _msiHandle);
      }

      if (result != MsiEnumError.Success)
      {
        return false;
      }

      _msiDb = MsiInterop.MsiGetActiveDatabase(_msiHandle);
      if (_msiDb != UIntPtr.Zero)
      {
        return true;
      }

      Close();
      return false;
    }

    public bool OpenQuery(string sql)
    {
      if (string.IsNullOrEmpty(sql)
          || !OpenDb())
      {
        return false;
      }

      MsiEnumError result = MsiInterop.MsiDatabaseOpenView(_msiDb, sql, ref _view);
      if (result != MsiEnumError.Success)
      {
        Close();
        return false;
      }

      if (MsiEnumError.Success == MsiInterop.MsiViewExecute(_view, UIntPtr.Zero))
      {
        return true;
      }

      Close();
      return false;
    }

    public bool NextRow()
    {
      MsiEnumError result = MsiInterop.MsiViewFetch(_view, ref _results);
      return result == MsiEnumError.Success;
    }

    public string GetString(int index)
    {
      StringBuilder value = new StringBuilder(256);
      int valueLength = value.Capacity;

      while (true)
      {
        MsiEnumError result = MsiInterop.MsiRecordGetString(_results, (uint)index, value, ref valueLength);
        switch (result)
        {
          case MsiEnumError.MoreData:
            value = new StringBuilder(valueLength * 2);
            valueLength = value.Capacity;
            break;

          case MsiEnumError.Success:
            return value.ToString();

          default:
            return null;
        }
      }
    }

    public bool GetBool(int index)
    {
      StringBuilder value = new StringBuilder(256);
      int valueLength = value.Capacity;

      while (true)
      {
        MsiEnumError result = MsiInterop.MsiRecordGetString(_results, (uint)index, value, ref valueLength);
        switch (result)
        {
          case MsiEnumError.MoreData:
            value = new StringBuilder(valueLength * 2);
            valueLength = value.Capacity;
            break;

          case MsiEnumError.Success:
            return value.ToString() == "y";

          default:
            return false;
        }
      }
    }

    public void Close()
    {
      if (_results != UIntPtr.Zero)
      {
        MsiInterop.MsiCloseHandle(_results);
      }

      if (_view != UIntPtr.Zero)
      {
        MsiInterop.MsiCloseHandle(_view);
      }

      if (_msiDb != UIntPtr.Zero)
      {
        MsiInterop.MsiCloseHandle(_msiDb);
      }

      if (_msiHandle != UIntPtr.Zero)
      {
        MsiInterop.MsiCloseHandle(_msiHandle);
      }
    }

    public void Dispose()
    {
      Close();
    }
  }
}

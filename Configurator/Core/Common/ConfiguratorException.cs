/* Copyright (c) 2023, Oracle and/or its affiliates

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
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Enums;

namespace MySql.Configurator.Core.Common
{
  public class ConfiguratorException : Exception
  {
    /// <summary>
    /// The Configurator error code associated to this exception.
    /// </summary>
    public ConfiguratorError InstallerErrorCode { get; private set; }

    /// <summary>
    /// Initializes an Configurator exception using a general installer error code.
    /// </summary>
    /// <param name="errorCode">The Installer error code.</param>
    public ConfiguratorException(ConfiguratorError errorCode) : this(errorCode.GetDescription())
    {
      InstallerErrorCode = errorCode;
    }

    /// <summary>
    /// Initializes an Configurator exception using an MSI error code.
    /// </summary>
    /// <param name="errorCode">The MSI error code.</param>
    public ConfiguratorException(MsiEnumError errorCode) : this(errorCode.GetDescription())
    {
    }

    /// <summary>
    /// Intializes a generic Configurator exception with no associated error code.
    /// </summary>
    /// <param name="msg">The exception message.</param>
    public ConfiguratorException(string msg) : base(msg)
    {
    }

    /// <summary>
    /// Intializes a generic Configurator exception with no associated error code.
    /// </summary>
    /// <param name="errorCode">The Installer error code.</param>
    /// <param name="formatString">A string to replace in the error message.</param>
    public ConfiguratorException(ConfiguratorError errorCode, string formatString) : base(string.Format(errorCode.GetDescription(), formatString))
    {
    }
  }
}

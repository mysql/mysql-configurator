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

using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using MySql.Configurator.Properties;

namespace MySql.Configurator.Core.Controls
{
  /// <inheritdoc />
  /// <summary>
  /// A <see cref="ComboBox"/> with preloaded local IP addresses.
  /// </summary>
  public partial class LocalIpAddressesComboBox : ComboBox
  {
    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:MySql.Configurator.Core.Controls.LocalIpAddressesComboBox" />.
    /// </summary>
    public LocalIpAddressesComboBox()
    {
      HelperText = null;
      InitializeComponent();
    }

    #region Properties

    /// <summary>
    /// Gets text to show in case the items could  not be loaded into the control.
    /// </summary>
    public string HelperText { get; private set; }

    #endregion Properties

    /// <summary>
    /// Loads local IP addresses into this <see cref="LocalIpAddressesComboBox"/>.
    /// </summary>
    public void LoadLocalIpAddresses()
    {
      if (Items.Count > 0)
      {
        return;
      }

      if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
      {

        HelperText = Resources.NoNetworkConnectionNoIpAddressesLoadedText;
        return;
      }

      Items.Add(string.Empty);
      var host = Dns.GetHostEntry(Dns.GetHostName());
      foreach (var ip in host.AddressList)
      {
        if (ip.AddressFamily == AddressFamily.InterNetwork)
        {
          Items.Add(ip.ToString());
        }
      }

      Items.Add(Dns.GetHostName());

      if (Items.Count > 0)
      {
        SelectedIndex = 0;
      }
    }
  }
}

/* Copyright (c) 2015, 2018, Oracle and/or its affiliates.

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
using System.Collections.Generic;
using MySql.Configurator.Core.Classes;
using NetFwTypeLib;

namespace MySql.Configurator.Core.Firewall
{
  /// <summary>
  /// This class enables a read-only access to most of properties of firewall policy under Windows Vista
  /// </summary>
  /// <remarks>
  /// The Windows Firewall/Internet Connection Sharing service must be running to access this class.
  /// This class requires Windows Vista.
  /// </remarks>
  public class Policy
  {
    /// <summary>
    /// Contains an instance of the "HNetCfg.FwPolicy2" object.
    /// </summary>
    private readonly INetFwPolicy2 _fwPolicy2;

    /// <summary>
    /// Contains currently active profile type. All properies will be read for this profile types.
    /// </summary>
    private readonly NET_FW_PROFILE_TYPE2_ _fwCurrentProfileTypes;

    /// <summary>
    /// Creates an instance of <see cref="Policy"/> object.
    /// </summary>
    public Policy()
    {
      //if running on non Vista system then throw an Exception
      if (!Win32.IsVistaOrHigher)
        throw new InvalidOperationException("This class is designed only for Windows Vista and higher.");

      //Create an instance of "HNetCfg.FwPolicy2"
      Type tNetFwPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
      _fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(tNetFwPolicy2);
      //read Current Profile Types (only to increase Performace)
      //avoids access on CurrentProfileTypes from each Property
      _fwCurrentProfileTypes = (NET_FW_PROFILE_TYPE2_)_fwPolicy2.CurrentProfileTypes;
    }

    /// <summary>
    /// Indicates whether the firewall is enabled.
    /// </summary>
    /// <value>
    ///   <c>true</c> if enabled; otherwise, <c>false</c>.
    /// </value>
    public bool Enabled
    {
      get
      {
        return _fwPolicy2.FirewallEnabled[_fwCurrentProfileTypes];
      }
    }

    /// <summary>
    /// The read only list of Rules.
    /// </summary>
    public Rules Rules
    {
      get
      {
        return new Rules(_fwPolicy2.Rules);
      }
    }


    /// <summary>
    /// Indicates that inbound traffic should be blocked by the firewall.
    /// </summary>
    public bool BlockAllInboundTraffic
    {
      get
      {
        return _fwPolicy2.BlockAllInboundTraffic[_fwCurrentProfileTypes];
      }
    }
    /// <summary>
    /// Retrieves currently active profiles.
    /// </summary>
    public ProfileType CurrentProfileTypes
    {
      get
      {
        return (ProfileType)_fwPolicy2.CurrentProfileTypes;
      }
    }

    /// <summary>
    /// Specifies the default action for inbound traffic.
    /// </summary>
    public Action DefaultInboundAction
    {
      get
      {
        return (Action)_fwPolicy2.DefaultInboundAction[_fwCurrentProfileTypes];
      }
    }

    /// <summary>
    /// Specifies the default action for outbound.
    /// </summary>
    public Action DefaultOutboundAction
    {
      get
      {
        return (Action)_fwPolicy2.DefaultOutboundAction[_fwCurrentProfileTypes];
      }
    }

    /// <summary>
    /// A list of interfaces on which firewall settings are excluded.
    /// </summary>
    public IEnumerable<string> ExcludedInterfaces
    {
      get
      {
        return _fwPolicy2.ExcludedInterfaces[_fwCurrentProfileTypes] as IEnumerable<string>;
      }
    }

    /// <summary>
    /// Indicates whether interactive firewall notifications are disabled.
    /// </summary>
    public bool NotificationsDisabled
    {
      get
      {
        return _fwPolicy2.NotificationsDisabled[_fwCurrentProfileTypes];
      }
    }

    /// <summary>
    /// Access to the Windows Service Hardening (WSH) store.
    /// </summary>
    public ServiceRestriction ServiceRestriction
    {
      get
      {
        return new ServiceRestriction(_fwPolicy2.ServiceRestriction);
      }
    }

    /// <summary>
    /// Indicates whether unicast incoming responses to outgoing multicast and broadcast traffic are disabled.
    /// </summary>
    public bool UnicastResponsesToMulticastBroadcastDisabled
    {
      get
      {
        return _fwPolicy2.UnicastResponsesToMulticastBroadcastDisabled[_fwCurrentProfileTypes];
      }
    }
  }
}

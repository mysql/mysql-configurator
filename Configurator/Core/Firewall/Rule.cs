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

using System.Collections.Generic;
using NetFwTypeLib;

namespace MySql.Configurator.Core.Firewall
{
  /// <summary>
  /// Represents a single firewall Rule.
  /// </summary>
  public class Rule
  {
    private readonly INetFwRule _fwRule;

    /// <summary>
    /// Creates an Instance of the <see>Rule</see> class from a INetFwRule.
    /// </summary>
    /// <param name="fwRule"></param>
    internal Rule(INetFwRule fwRule)
    {
      _fwRule = fwRule;
    }

    /// <summary>
    /// Accesses the Action property of this rule.
    /// </summary>
    public Action Action
    {
      get
      {
        return (Action)_fwRule.Action;
      }
      set
      {
        _fwRule.Action = (NET_FW_ACTION_)value;
      }
    }
    /// <summary>
    ///  Accesses the ApplicationName property for this rule.
    /// </summary>
    public string ApplicationName
    {
      get
      {
        return _fwRule.ApplicationName;
      }
      set
      {
        _fwRule.ApplicationName = value;
      }
    }

    /// <summary>
    /// Accesses the Description property for this rule.
    /// </summary>
    /// <remarks>
    /// This property is optional. The string must not contain the "|" character.
    /// </remarks>
    public string Description
    {
      get
      {
        return _fwRule.Description;
      }
      set
      {
        _fwRule.Description = value;
      }
    }

    /// <summary>
    /// Accesses the Direction property for this rule.
    /// </summary>
    public RuleDirection Direction
    {
      get
      {
        return (RuleDirection)_fwRule.Direction;
      }
      set
      {
        _fwRule.Direction = (NET_FW_RULE_DIRECTION_)value;
      }
    }
    /// <summary>
    /// Accesses the EdgeTraversal property for this rule.
    /// </summary>
    public bool EdgeTraversal
    {
      get
      {
        return _fwRule.EdgeTraversal;
      }
      set
      {
        _fwRule.EdgeTraversal = value;
      }
    }
    /// <summary>
    /// Accesses the Enabled property for this rule.
    /// </summary>
    public bool Enabled
    {
      get
      {
        return _fwRule.Enabled;
      }
      set
      {
        _fwRule.Enabled = value;
      }
    }
    /// <summary>
    /// Accesses the Grouping property for this rule.
    /// </summary>
    public string Grouping
    {
      get
      {
        return _fwRule.Grouping;
      }
      set
      {
        _fwRule.Grouping = value;
      }
    }
    /// <summary>
    /// Accesses the IcmpTypesAndCodes property for this rule.
    /// </summary>
    public string IcmpTypesAndCodes
    {
      get
      {
        return _fwRule.IcmpTypesAndCodes;
      }
      set
      {
        _fwRule.IcmpTypesAndCodes = value;
      }
    }
    /// <summary>
    /// Accesses the Interfaces property for this rule.
    /// </summary>
    public IEnumerable<string> Interfaces
    {
      get
      {
        return _fwRule.Interfaces as IEnumerable<string>;
      }
      set
      {
        _fwRule.Interfaces = value;
      }
    }
    /// <summary>
    /// Accesses the InterfaceTypes property for this rule.
    /// </summary>
    public string InterfaceTypes
    {
      get
      {
        return _fwRule.InterfaceTypes;
      }
      set
      {
        _fwRule.InterfaceTypes = value;
      }
    }
    /// <summary>
    /// Accesses the LocalAddresses property for this rule.
    /// </summary>
    public string LocalAddresses
    {
      get
      {
        return _fwRule.LocalAddresses;
      }
      set
      {
        _fwRule.LocalAddresses = value;
      }
    }
    /// <summary>
    /// Accesses the LocalPorts property of this rule.
    /// </summary>
    public string LocalPorts
    {
      get
      {
        return _fwRule.LocalPorts;
      }
      set
      {
        _fwRule.LocalPorts = value;
      }
    }
    /// <summary>
    /// Accesses the Name property for this rule.
    /// </summary>
    public string Name
    {
      get
      {
        return _fwRule.Name;
      }
      set
      {
        _fwRule.Name = value;
      }
    }

    /// <summary>
    /// Accesses the Profiles property for this rule.
    /// </summary>
    public int Profiles
    {
      get
      {
        return _fwRule.Profiles;
      }
      set
      {
        _fwRule.Profiles = value;
      }
    }
    /// <summary>
    /// Accesses the Protocol property for this rule.
    /// </summary>
    public int Protocol
    {
      get
      {
        return _fwRule.Protocol;
      }
      set
      {
        _fwRule.Protocol = value;
      }
    }

    /// <summary>
    /// Accesses the RemoteAddresses property of this rule.
    /// </summary>
    public string RemoteAddresses
    {
      get
      {
        return _fwRule.RemoteAddresses;
      }
      set
      {
        _fwRule.RemoteAddresses = value;
      }
    }

    /// <summary>
    /// Accesses the RemotePorts property for this rule.
    /// </summary>
    public string RemotePorts
    {
      get
      {
        return _fwRule.RemotePorts;
      }
      set
      {
        _fwRule.RemotePorts = value;
      }
    }
    /// <summary>
    /// Accesses the ServiceaName property for this rule.
    /// </summary>
    public string ServiceName
    {
      get
      {
        return _fwRule.serviceName;
      }
      set
      {
        _fwRule.serviceName = value;
      }
    }
  }
}

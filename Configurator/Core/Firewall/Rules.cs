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

using System.Collections.Generic;
using System.Linq;
using NetFwTypeLib;

namespace MySql.Configurator.Core.Firewall
{
  /// <summary>
  /// The Read only list of  <see cref="Rule"/> objects.
  /// </summary>
  public class Rules : List<Rule>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Rules"/> class.
    /// </summary>
    /// <param name="list">The list.</param>
    internal Rules(IList<Rule> list) : base(list)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Rules"/> class.
    /// </summary>
    /// <param name="rules">The rules.</param>
    internal Rules(INetFwRules rules) : base(RulesToList(rules))
    {
    }

    /// <summary>
    /// Ruleses to list.
    /// </summary>
    /// <param name="rules">The rules.</param>
    /// <returns>List of rules</returns>
    private static IList<Rule> RulesToList(INetFwRules rules)
    {
      var list = new List<Rule>(rules.Count);
      list.AddRange(rules.Cast<INetFwRule>().Select(currentFwRule => new Rule(currentFwRule)));
      return list;
    }
  }
}

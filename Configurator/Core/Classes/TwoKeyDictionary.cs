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
using System.Collections.Generic;
using System.Linq;

namespace MySql.Configurator.Core.Classes
{
  public class TwoKeyDictionary<TKey1, TKey2, TValue> : Dictionary<TwoKey<TKey1, TKey2>, TValue>
  {
    public static TwoKey<TKey1, TKey2> Key(TKey1 key1, TKey2 key2)
    {
      return new TwoKey<TKey1, TKey2>(key1, key2);
    }

    public TValue this[TKey1 key1, TKey2 key2]
    {
      get { return this[Key(key1, key2)]; }
      set { this[Key(key1, key2)] = value; }
    }

    public void Add(TKey1 key1, TKey2 key2, TValue value)
    {
      Add(Key(key1, key2), value);
    }

    public bool ContainsKey(TKey1 key1, TKey2 key2)
    {
      return ContainsKey(Key(key1, key2));
    }

    public bool ContainsSecondKey(TKey2 key2)
    {
      return this.Any(k => k.Key.Item2.Equals(key2));
    }

    public bool ContainsFirstKey(TKey1 key1)
    {
      return this.Any(k => k.Key.Item1.Equals(key1));
    }
  }

  public class TwoKey<TKey1, TKey2> : Tuple<TKey1, TKey2>
  {
    public TwoKey(TKey1 item1, TKey2 item2) : base(item1, item2) { }

    public override string ToString()
    {
      return string.Format("({0},{1})", Item1, Item2);
    }
  }
}

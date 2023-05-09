/* Copyright (c) 2014, 2023, Oracle and/or its affiliates.

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

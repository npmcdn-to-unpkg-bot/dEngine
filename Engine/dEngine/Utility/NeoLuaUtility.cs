// NeoLuaUtility.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections.Generic;
using System.Linq;
using Neo.IronLua;

namespace dEngine.Utility
{
    /// <summary>
    /// Utility methods related to NeoLua.
    /// </summary>
    public static class NeoLuaUtility
    {
        /// <summary>
        /// Converts a <see cref="LuaTable" /> to a <see cref="HashSet{T}" />
        /// </summary>
        public static HashSet<T> ToHashSet<T>(this LuaTable result)
        {
            return new HashSet<T>(result.Values.Cast<T>());
        }

        /// <summary>
        /// Packs the dictionary into a <see cref="LuaTable" />
        /// </summary>
        public static LuaTable ToLuaTable<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            var table = new LuaTable();
            foreach (var kv in dictionary)
                table[kv.Key] = kv.Value;
            return table;
        }

        /// <summary>
        /// Packs the enumerable into a <see cref="LuaTable" />
        /// </summary>
        public static LuaTable ToLuaTable<T>(this IEnumerable<T> values)
        {
            var table = new LuaTable();
            var i = 0;
            foreach (var v in values)
                table.SetArrayValue(++i, v);
            return table;
        }

        /// <summary>
        /// Packs the enumerable into a <see cref="LuaTable" />.
        /// </summary>
        public static LuaTable ToLuaTable<TSource, TResult>(this IEnumerable<TSource> values,
            Func<TSource, TResult> func)
        {
            var table = new LuaTable();
            var i = 1;
            foreach (var v in values)
            {
                table[i] = func(v);
                i++;
            }
            return table;
        }
    }
}
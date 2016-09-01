// ConcurrentDictionaryExtensions.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Collections.Concurrent;

namespace dEngine.Utility
{
    internal static class ConcurrentDictionaryExtensions
    {
        /// <summary>
        /// Attempts to add the given key to a dictionary with a default value.
        /// </summary>
        public static bool TryAdd<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dict, TKey key)
        {
            return dict.TryAdd(key, default(TValue));
        }

        /// <summary>
        /// Attempts to remove an entry from a dictionary without getting the value.
        /// </summary>
        public static bool TryRemove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dict, TKey key)
        {
            TValue _;
            return dict.TryRemove(key, out _);
        }
    }
}
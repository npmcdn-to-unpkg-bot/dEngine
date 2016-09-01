// InstanceHelper.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections.Generic;
using dEngine.Instances;

namespace dEngine.Utility
{
    internal static class InstanceHelper
    {
        /// <summary>
        /// Calls func on the descendants of root.
        /// </summary>
        /// <param name="root">The instance to start at.</param>
        /// <param name="func">A callback for each item. If it returns false, the loop will break.</param>
        internal static void Recurse(this Instance root, Func<Instance, sbyte> func)
        {
            foreach (var kv in root.Children)
            {
                var result = func(kv);
                if (result == -1) break;
                if (result <= -2) continue;
                Recurse(kv, func);
            }
        }

        public static List<Instance> Where(this Instance root, Predicate<Instance> predicate, bool recurse = true)
        {
            var results = new List<Instance>();

            Recurse(root, x =>
            {
                if (predicate(x))
                    results.Add(x);
                return -1;
            });

            return results;
        }

        public static Instance First(this Instance root, Predicate<Instance> predicate, bool recurse = true)
        {
            Instance result = null;
            Recurse(root, x =>
            {
                if (predicate(x))
                {
                    result = x;
                    return -1;
                }
                return 01;
            });
            return result;
        }

        public static void ForEach(this Instance root, Action<Instance> action)
        {
            foreach (var kv in root.Children)
                action(kv);
        }

        internal class RecurseResult
        {
            public bool Continue;
            public bool RecurseThis;

            public RecurseResult(bool @continue, bool recurseThis = true)
            {
                Continue = @continue;
                RecurseThis = recurseThis;
            }
        }
    }
}
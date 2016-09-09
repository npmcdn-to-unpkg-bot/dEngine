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
    }
}
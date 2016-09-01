// Pages.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections.Generic;
using dEngine.Instances.Attributes;
using Neo.IronLua;

namespace dEngine.Instances
{
    /// <summary>
    /// A table of pages, where a page is a sorted list of key/value pairs.
    /// </summary>
    [TypeId(190)]
    [Uncreatable]
    public abstract class Pages : Instance
    {
        /// <summary>
        /// The current page.
        /// </summary>
        protected SortedList<string, object> _currentPage;

        /// <summary>
        /// Determines if there are any more pages.
        /// </summary>
        public bool IsFinished { get; protected set; }

        /// <summary>
        /// Returns the current page.
        /// </summary>
        /// <returns></returns>
        public LuaTable GetCurrentPage()
        {
            var table = new LuaTable();
            foreach (var pair in _currentPage)
                table[pair.Key] = pair.Value;
            return table;
        }

        /// <summary>
        /// Requests the next page.
        /// </summary>
        public virtual void AdvanceToNextPage()
        {
            if (IsFinished)
                throw new InvalidOperationException("Cannot advance to next page: there are no more pages.");
        }

        internal IEnumerable<SortedList<string, object>> EnumeratePages()
        {
            while (!IsFinished)
            {
                AdvanceToNextPage();
                yield return _currentPage;
            }
        }
    }
}
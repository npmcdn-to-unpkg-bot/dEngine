﻿// ChildrenCollection.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using dEngine.Instances;

#pragma warning disable 1591

namespace dEngine.Utility
{
    [DebuggerDisplay("Count = {Count}")]
    public sealed class ChildrenCollection : IEnumerable<Instance>
    {
        private readonly ConcurrentDictionary<Instance, byte> _internalCollection =
            new ConcurrentDictionary<Instance, byte>();

        public int Count => _internalCollection.Count;

        public IEnumerator<Instance> GetEnumerator()
        {
            return _internalCollection.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal bool Add(Instance instance)
        {
            var result = _internalCollection.TryAdd(instance, 0);
            return result;
        }

        internal bool Remove(Instance instance)
        {
            byte _;
            var result = _internalCollection.TryRemove(instance, out _);
            return result;
        }
    }

    public class ConverterEnumerator<TSource, TTarget> : IEnumerator<TTarget> where TTarget : TSource
    {
        private readonly IEnumerator<TSource> _enumerator;

        public ConverterEnumerator(IEnumerator<TSource> enumerator)
        {
            _enumerator = enumerator;
        }

        public void Dispose()
        {
            _enumerator.Dispose();
        }

        public bool MoveNext()
        {
            do
            {
                if (!_enumerator.MoveNext())
                    return false;
            } while (!(_enumerator.Current is TTarget));

            return true;
        }

        public void Reset()
        {
            _enumerator.Reset();
        }

        object IEnumerator.Current => _enumerator.Current;

        TTarget IEnumerator<TTarget>.Current => (TTarget)_enumerator.Current;
    }
}
// ConcurrentWorkQueue.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using C5;

#pragma warning disable 1591

namespace dEngine.Utility
{
    /// <summary>
    /// A concurrent queue which will take all elements every frame and work on them.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConcurrentWorkQueue<T>
    {
        private readonly Thread _creationThread;
        private readonly Action<T> _perform;
        private readonly ConcurrentQueue<T> _queue;

        /// <summary>
        /// Creates a work queue.
        /// </summary>
        /// <param name="perform">The action to call for each queue item.</param>
        public ConcurrentWorkQueue(Action<T> perform)
        {
            _queue = new ConcurrentQueue<T>();
            _perform = perform;
            _creationThread = Thread.CurrentThread;
        }

        /// <summary>
        /// Determines if <see cref="Enqueue" /> calls on the same thread as the creation thread should skip the queue.
        /// </summary>
        public bool SameThreadSkipsQueue { get; set; }

        public bool IsEmpty => _queue.IsEmpty;

        /// <inheritdoc />
        public int Count => _queue.Count;

        public void CopyTo(T[] array, int index)
        {
            _queue.CopyTo(array, index);
        }

        public T[] ToArray()
        {
            return _queue.ToArray();
        }

        /// <summary>
        /// Adds an item to the end of the queue.
        /// </summary>
        public void Enqueue(T item)
        {
            var currThread = Thread.CurrentThread;
            if ((currThread == _creationThread) && SameThreadSkipsQueue)
                _perform(item);
            else
                _queue.Enqueue(item);
        }

        public T Dequeue()
        {
            T result;
            _queue.TryDequeue(out result);
            return result;
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ((IFormattable)_queue).ToString(format, formatProvider);
        }

        public bool Show(StringBuilder stringbuilder, ref int rest, IFormatProvider formatProvider)
        {
            return ((IShowable)_queue).Show(stringbuilder, ref rest, formatProvider);
        }

        /// <summary>
        /// Performs work on the queue.
        /// </summary>
        public void Work()
        {
            if (_queue.IsEmpty)
                return;

            for (var i = 0; i < _queue.Count; i++)
            {
                var item = Dequeue();
                _perform(item);
            }
        }
    }
}
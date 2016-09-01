// ArrayListEx.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using C5;

#pragma warning disable 1591

namespace dEngine.Utility
{
    public class ArrayListEx<T> : ArrayList<T>
    {
        public ArrayListEx()
        {
        }

        public ArrayListEx(int capacity) : base(capacity)
        {
        }

        public T[] GetArray()
        {
            return array;
        }
    }
}
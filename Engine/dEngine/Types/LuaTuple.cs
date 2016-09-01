﻿// LuaTuple.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using Neo.IronLua;

namespace dEngine
{
    /// <summary>
    /// Represents multiple return values.
    /// </summary>
    public sealed class LuaTuple<T1>
    {
        private readonly LuaResult _result;

        internal LuaTuple(T1 arg1 = default(T1))
        {
            _result = new LuaResult(arg1);
        }

        /// <summary />
        public static implicit operator LuaResult(LuaTuple<T1> tuple)
        {
            return tuple._result;
        }

        /// <summary />
        public static implicit operator T1(LuaTuple<T1> tuple)
        {
            return (T1)tuple._result[0];
        }

        /// <summary />
        public static implicit operator Tuple<T1>(LuaTuple<T1> tuple)
        {
            return new Tuple<T1>((T1)tuple._result[0]);
        }

        /// <summary />
        public static implicit operator LuaTuple<T1>(Tuple<T1> tuple)
        {
            return new LuaTuple<T1>(tuple.Item1);
        }
    }

    /// <summary>
    /// Represents multiple return values.
    /// </summary>
    public sealed class LuaTuple<T1, T2>
    {
        private readonly LuaResult _result;

        internal LuaTuple(T1 arg1 = default(T1), T2 arg2 = default(T2))
        {
            _result = new LuaResult(arg1, arg2);
        }

        /// <summary />
        public static implicit operator LuaResult(LuaTuple<T1, T2> tuple)
        {
            return tuple._result;
        }

        /// <summary />
        public static implicit operator T1(LuaTuple<T1, T2> tuple)
        {
            return (T1)tuple._result[0];
        }

        /// <summary />
        public static implicit operator Tuple<T1, T2>(LuaTuple<T1, T2> tuple)
        {
            return new Tuple<T1, T2>((T1)tuple._result[0], (T2)tuple._result[1]);
        }

        /// <summary />
        public static implicit operator LuaTuple<T1, T2>(Tuple<T1, T2> tuple)
        {
            return new LuaTuple<T1, T2>(tuple.Item1, tuple.Item2);
        }
    }

    /// <summary>
    /// Represents multiple return values.
    /// </summary>
    public sealed class LuaTuple<T1, T2, T3>
    {
        private readonly LuaResult _result;

        internal LuaTuple(T1 arg1 = default(T1), T2 arg2 = default(T2), T3 arg3 = default(T3))
        {
            _result = new LuaResult(arg1, arg2, arg3);
        }

        /// <summary />
        public static implicit operator LuaResult(LuaTuple<T1, T2, T3> tuple)
        {
            return tuple._result;
        }

        /// <summary />
        public static implicit operator T1(LuaTuple<T1, T2, T3> tuple)
        {
            return (T1)tuple._result[0];
        }

        /// <summary />
        public static implicit operator Tuple<T1, T2, T3>(LuaTuple<T1, T2, T3> tuple)
        {
            return new Tuple<T1, T2, T3>((T1)tuple._result[0], (T2)tuple._result[1], (T3)tuple._result[2]);
        }

        /// <summary />
        public static implicit operator LuaTuple<T1, T2, T3>(Tuple<T1, T2, T3> tuple)
        {
            return new LuaTuple<T1, T2, T3>(tuple.Item1, tuple.Item2, tuple.Item3);
        }
    }

    /// <summary>
    /// Represents multiple return values.
    /// </summary>
    public sealed class LuaTuple<T1, T2, T3, T4>
    {
        private readonly LuaResult _result;

        internal LuaTuple(T1 arg1 = default(T1), T2 arg2 = default(T2), T3 arg3 = default(T3), T4 arg4 = default(T4))
        {
            _result = new LuaResult(arg1, arg2, arg3, arg4);
        }

        /// <summary />
        public static implicit operator LuaResult(LuaTuple<T1, T2, T3, T4> tuple)
        {
            return tuple._result;
        }

        /// <summary />
        public static implicit operator T1(LuaTuple<T1, T2, T3, T4> tuple)
        {
            return (T1)tuple._result[0];
        }

        /// <summary />
        public static implicit operator Tuple<T1, T2, T3, T4>(LuaTuple<T1, T2, T3, T4> tuple)
        {
            return new Tuple<T1, T2, T3, T4>((T1)tuple._result[0], (T2)tuple._result[1], (T3)tuple._result[2],
                (T4)tuple._result[3]);
        }

        /// <summary />
        public static implicit operator LuaTuple<T1, T2, T3, T4>(Tuple<T1, T2, T3, T4> tuple)
        {
            return new LuaTuple<T1, T2, T3, T4>(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
        }
    }

    /// <summary>
    /// Represents multiple return values.
    /// </summary>
    public sealed class LuaTuple<T1, T2, T3, T4, T5>
    {
        private readonly LuaResult _result;

        internal LuaTuple(T1 arg1 = default(T1), T2 arg2 = default(T2), T3 arg3 = default(T3), T4 arg4 = default(T4),
            T5 arg5 = default(T5))
        {
            _result = new LuaResult(arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary />
        public static implicit operator LuaResult(LuaTuple<T1, T2, T3, T4, T5> tuple)
        {
            return tuple._result;
        }

        /// <summary />
        public static implicit operator T1(LuaTuple<T1, T2, T3, T4, T5> tuple)
        {
            return (T1)tuple._result[0];
        }

        /// <summary />
        public static implicit operator Tuple<T1, T2, T3, T4, T5>(LuaTuple<T1, T2, T3, T4, T5> tuple)
        {
            return new Tuple<T1, T2, T3, T4, T5>((T1)tuple._result[0], (T2)tuple._result[1], (T3)tuple._result[2],
                (T4)tuple._result[3], (T5)tuple._result[4]);
        }

        /// <summary />
        public static implicit operator LuaTuple<T1, T2, T3, T4, T5>(Tuple<T1, T2, T3, T4, T5> tuple)
        {
            return new LuaTuple<T1, T2, T3, T4, T5>(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5);
        }
    }

    /// <summary>
    /// Represents multiple return values.
    /// </summary>
    public sealed class LuaTuple<T1, T2, T3, T4, T5, T6>
    {
        private readonly LuaResult _result;

        internal LuaTuple(T1 arg1 = default(T1), T2 arg2 = default(T2), T3 arg3 = default(T3), T4 arg4 = default(T4),
            T5 arg5 = default(T5), T6 arg6 = default(T6))
        {
            _result = new LuaResult(arg1, arg2, arg3, arg4, arg5, arg6);
        }

        /// <summary />
        public static implicit operator LuaResult(LuaTuple<T1, T2, T3, T4, T5, T6> tuple)
        {
            return tuple._result;
        }

        /// <summary />
        public static implicit operator T1(LuaTuple<T1, T2, T3, T4, T5, T6> tuple)
        {
            return (T1)tuple._result[0];
        }

        /// <summary />
        public static implicit operator Tuple<T1, T2, T3, T4, T5, T6>(LuaTuple<T1, T2, T3, T4, T5, T6> tuple)
        {
            return new Tuple<T1, T2, T3, T4, T5, T6>((T1)tuple._result[0], (T2)tuple._result[1], (T3)tuple._result[2],
                (T4)tuple._result[3], (T5)tuple._result[4], (T6)tuple._result[5]);
        }

        /// <summary />
        public static implicit operator LuaTuple<T1, T2, T3, T4, T5, T6>(Tuple<T1, T2, T3, T4, T5, T6> tuple)
        {
            return new LuaTuple<T1, T2, T3, T4, T5, T6>(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5,
                tuple.Item6);
        }
    }

    /// <summary>
    /// Represents multiple return values.
    /// </summary>
    public sealed class LuaTuple<T1, T2, T3, T4, T5, T6, T7>
    {
        private readonly LuaResult _result;

        internal LuaTuple(T1 arg1 = default(T1), T2 arg2 = default(T2), T3 arg3 = default(T3), T4 arg4 = default(T4),
            T5 arg5 = default(T5), T6 arg6 = default(T6), T7 arg7 = default(T7))
        {
            _result = new LuaResult(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        /// <summary />
        public static implicit operator LuaResult(LuaTuple<T1, T2, T3, T4, T5, T6, T7> tuple)
        {
            return tuple._result;
        }

        /// <summary />
        public static implicit operator T1(LuaTuple<T1, T2, T3, T4, T5, T6, T7> tuple)
        {
            return (T1)tuple._result[0];
        }
    }

    /// <summary>
    /// Represents multiple return values.
    /// </summary>
    public sealed class LuaTuple<T1, T2, T3, T4, T5, T6, T7, T8>
    {
        private readonly LuaResult _result;

        internal LuaTuple(T1 arg1 = default(T1), T2 arg2 = default(T2), T3 arg3 = default(T3), T4 arg4 = default(T4),
            T5 arg5 = default(T5), T6 arg6 = default(T6), T7 arg7 = default(T7), T8 arg8 = default(T8))
        {
            _result = new LuaResult(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        /// <summary />
        public static implicit operator LuaResult(LuaTuple<T1, T2, T3, T4, T5, T6, T7, T8> tuple)
        {
            return tuple._result;
        }

        /// <summary />
        public static implicit operator T1(LuaTuple<T1, T2, T3, T4, T5, T6, T7, T8> tuple)
        {
            return (T1)tuple._result[0];
        }
    }

    /// <summary>
    /// Represents multiple return values.
    /// </summary>
    public sealed class LuaTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>
    {
        private readonly LuaResult _result;

        internal LuaTuple(T1 arg1 = default(T1), T2 arg2 = default(T2), T3 arg3 = default(T3), T4 arg4 = default(T4),
            T5 arg5 = default(T5), T6 arg6 = default(T6), T7 arg7 = default(T7), T8 arg8 = default(T8),
            T9 arg9 = default(T9))
        {
            _result = new LuaResult(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        /// <summary />
        public static implicit operator LuaResult(LuaTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> tuple)
        {
            return tuple._result;
        }

        /// <summary />
        public static implicit operator T1(LuaTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> tuple)
        {
            return (T1)tuple._result[0];
        }
    }

    /// <summary>
    /// Represents multiple return values.
    /// </summary>
    public sealed class LuaTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    {
        private readonly LuaResult _result;

        internal LuaTuple(T1 arg1 = default(T1), T2 arg2 = default(T2), T3 arg3 = default(T3), T4 arg4 = default(T4),
            T5 arg5 = default(T5), T6 arg6 = default(T6), T7 arg7 = default(T7), T8 arg8 = default(T8),
            T9 arg9 = default(T9), T10 arg10 = default(T10))
        {
            _result = new LuaResult(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        /// <summary />
        public static implicit operator LuaResult(LuaTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> tuple)
        {
            return tuple._result;
        }

        /// <summary />
        public static implicit operator T1(LuaTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> tuple)
        {
            return (T1)tuple._result[0];
        }
    }

    /// <summary>
    /// Represents multiple return values.
    /// </summary>
    public sealed class LuaTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    {
        private readonly LuaResult _result;

        internal LuaTuple(T1 arg1 = default(T1), T2 arg2 = default(T2), T3 arg3 = default(T3), T4 arg4 = default(T4),
            T5 arg5 = default(T5), T6 arg6 = default(T6), T7 arg7 = default(T7), T8 arg8 = default(T8),
            T9 arg9 = default(T9), T10 arg10 = default(T10), T11 arg11 = default(T11))
        {
            _result = new LuaResult(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
        }

        /// <summary />
        public static implicit operator LuaResult(LuaTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> tuple)
        {
            return tuple._result;
        }

        /// <summary />
        public static implicit operator T1(LuaTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> tuple)
        {
            return (T1)tuple._result[0];
        }
    }

    /// <summary>
    /// Represents multiple return values.
    /// </summary>
    public sealed class LuaTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
    {
        private readonly LuaResult _result;

        internal LuaTuple(T1 arg1 = default(T1), T2 arg2 = default(T2), T3 arg3 = default(T3), T4 arg4 = default(T4),
            T5 arg5 = default(T5), T6 arg6 = default(T6), T7 arg7 = default(T7), T8 arg8 = default(T8),
            T9 arg9 = default(T9), T10 arg10 = default(T10), T11 arg11 = default(T11), T12 arg12 = default(T12))
        {
            _result = new LuaResult(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
        }

        /// <summary />
        public static implicit operator LuaResult(LuaTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> tuple)
        {
            return tuple._result;
        }

        /// <summary />
        public static implicit operator T1(LuaTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> tuple)
        {
            return (T1)tuple._result[0];
        }
    }
}
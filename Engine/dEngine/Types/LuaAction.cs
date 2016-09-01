﻿// LuaAction.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Runtime.CompilerServices;
using Dynamitey;
using Neo.IronLua;

namespace dEngine
{
    /// <summary>
    /// Represents a function in Lua.
    /// </summary>
    public sealed class LuaAction
    {
        private readonly Delegate _delegate;

        /// <summary />
        public LuaAction(Delegate del)
        {
            _delegate = del;
        }

        /// <summary>
        /// Invokes the action.
        /// </summary>
        public void Invoke()
        {
            _delegate.FastDynamicInvoke();
        }

        /// <summary />
        public static implicit operator Action(LuaAction action)
        {
            return action.Invoke;
        }

        /// <summary />
        public static implicit operator LuaAction(Action action)
        {
            return new LuaAction(action);
        }

        /// <summary />
        public static implicit operator LuaAction(Delegate del)
        {
            return new LuaAction(del);
        }

        /// <summary />
        public static implicit operator LuaAction(Func<LuaResult> del)
        {
            return new LuaAction(del);
        }
    }


    /// <summary>
    /// Represents multiple return values.
    /// </summary>
    public sealed class LuaAction<T1>
    {
        private readonly Delegate _delegate;
        private readonly bool _isAction;
        private readonly bool _isParams;

        /// <summary />
        public LuaAction(Delegate del)
        {
            var paramaters = del.Method.GetParameters();
            var paramCount = paramaters.Length;

            if (paramCount > 0)
                if (paramaters[0].ParameterType == typeof(Closure))
                    paramCount--;

            _isAction = paramCount == 0;
            _isParams = (paramCount == 1) && (paramaters[0].ParameterType == typeof(object[]));

            _delegate = del;
        }

        /// <summary>
        /// Invokes the action.
        /// </summary>
        public void Invoke(T1 arg1 = default(T1))
        {
            if (_isAction)
                _delegate.FastDynamicInvoke();
            else if (_isParams)
                _delegate.FastDynamicInvoke(new object[] {new object[] {arg1}});
            else
                _delegate.FastDynamicInvoke(arg1);
        }

        /// <summary />
        public static implicit operator Action<T1>(LuaAction<T1> action)
        {
            return action.Invoke;
        }

        /// <summary />
        public static implicit operator LuaAction<T1>(Delegate del)
        {
            return new LuaAction<T1>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1>(MulticastDelegate del)
        {
            return new LuaAction<T1>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1>(Func<LuaResult> del)
        {
            return new LuaAction<T1>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1>(Func<object, LuaResult> del)
        {
            return new LuaAction<T1>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1>(Func<object, object, LuaResult> del)
        {
            return new LuaAction<T1>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1>(Func<object, object, object, LuaResult> del)
        {
            return new LuaAction<T1>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1>(Func<object, object, object, object, LuaResult> del)
        {
            return new LuaAction<T1>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1>(Func<object, object, object, object, object, LuaResult> del)
        {
            return new LuaAction<T1>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1>(
            Func<object, object, object, object, object, object, LuaResult> del)
        {
            return new LuaAction<T1>(del);
        }
    }


    /// <summary>
    /// Represents multiple return values.
    /// </summary>
    public sealed class LuaAction<T1, T2>
    {
        private readonly Delegate _delegate;
        private readonly bool _isAction;
        private readonly bool _isParams;

        /// <summary />
        public LuaAction(Delegate del)
        {
            var paramaters = del.Method.GetParameters();
            var paramCount = paramaters.Length;

            if (paramCount > 0)
                if (paramaters[0].ParameterType == typeof(Closure))
                    paramCount--;

            _isAction = paramCount == 0;
            _isParams = (paramCount == 1) && (paramaters[0].ParameterType == typeof(object[]));

            _delegate = del;
        }

        /// <summary>
        /// Invokes the action.
        /// </summary>
        public void Invoke(T1 arg1 = default(T1), T2 arg2 = default(T2))
        {
            if (_isAction)
                _delegate.FastDynamicInvoke();
            else if (_isParams)
                _delegate.FastDynamicInvoke(new object[] {new object[] {arg1, arg2}});
            else
                _delegate.FastDynamicInvoke(arg1, arg2);
        }

        /// <summary />
        public static implicit operator Action<T1, T2>(LuaAction<T1, T2> action)
        {
            return action.Invoke;
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2>(Delegate del)
        {
            return new LuaAction<T1, T2>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2>(MulticastDelegate del)
        {
            return new LuaAction<T1, T2>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2>(Func<LuaResult> del)
        {
            return new LuaAction<T1, T2>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2>(Func<object, LuaResult> del)
        {
            return new LuaAction<T1, T2>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2>(Func<object, object, LuaResult> del)
        {
            return new LuaAction<T1, T2>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2>(Func<object, object, object, LuaResult> del)
        {
            return new LuaAction<T1, T2>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2>(Func<object, object, object, object, LuaResult> del)
        {
            return new LuaAction<T1, T2>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2>(Func<object, object, object, object, object, LuaResult> del)
        {
            return new LuaAction<T1, T2>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2>(
            Func<object, object, object, object, object, object, LuaResult> del)
        {
            return new LuaAction<T1, T2>(del);
        }
    }


    /// <summary>
    /// Represents multiple return values.
    /// </summary>
    public sealed class LuaAction<T1, T2, T3>
    {
        private readonly Delegate _delegate;
        private readonly bool _isAction;
        private readonly bool _isParams;

        /// <summary />
        public LuaAction(Delegate del)
        {
            var paramaters = del.Method.GetParameters();
            var paramCount = paramaters.Length;

            if (paramCount > 0)
                if (paramaters[0].ParameterType == typeof(Closure))
                    paramCount--;

            _isAction = paramCount == 0;
            _isParams = (paramCount == 1) && (paramaters[0].ParameterType == typeof(object[]));

            _delegate = del;
        }

        /// <summary>
        /// Invokes the action.
        /// </summary>
        public void Invoke(T1 arg1 = default(T1), T2 arg2 = default(T2), T3 arg3 = default(T3))
        {
            if (_isAction)
                _delegate.FastDynamicInvoke();
            else if (_isParams)
                _delegate.FastDynamicInvoke(new object[] {new object[] {arg1, arg2, arg3}});
            else
                _delegate.FastDynamicInvoke(arg1, arg2, arg3);
        }

        /// <summary />
        public static implicit operator Action<T1, T2, T3>(LuaAction<T1, T2, T3> action)
        {
            return action.Invoke;
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3>(Delegate del)
        {
            return new LuaAction<T1, T2, T3>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3>(MulticastDelegate del)
        {
            return new LuaAction<T1, T2, T3>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3>(Func<LuaResult> del)
        {
            return new LuaAction<T1, T2, T3>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3>(Func<object, LuaResult> del)
        {
            return new LuaAction<T1, T2, T3>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3>(Func<object, object, LuaResult> del)
        {
            return new LuaAction<T1, T2, T3>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3>(Func<object, object, object, LuaResult> del)
        {
            return new LuaAction<T1, T2, T3>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3>(Func<object, object, object, object, LuaResult> del)
        {
            return new LuaAction<T1, T2, T3>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3>(
            Func<object, object, object, object, object, LuaResult> del)
        {
            return new LuaAction<T1, T2, T3>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3>(
            Func<object, object, object, object, object, object, LuaResult> del)
        {
            return new LuaAction<T1, T2, T3>(del);
        }
    }


    /// <summary>
    /// Represents multiple return values.
    /// </summary>
    public sealed class LuaAction<T1, T2, T3, T4>
    {
        private readonly Delegate _delegate;
        private readonly bool _isAction;
        private readonly bool _isParams;

        /// <summary />
        public LuaAction(Delegate del)
        {
            var paramaters = del.Method.GetParameters();
            var paramCount = paramaters.Length;

            if (paramCount > 0)
                if (paramaters[0].ParameterType == typeof(Closure))
                    paramCount--;

            _isAction = paramCount == 0;
            _isParams = (paramCount == 1) && (paramaters[0].ParameterType == typeof(object[]));

            _delegate = del;
        }

        /// <summary>
        /// Invokes the action.
        /// </summary>
        public void Invoke(T1 arg1 = default(T1), T2 arg2 = default(T2), T3 arg3 = default(T3), T4 arg4 = default(T4))
        {
            if (_isAction)
                _delegate.FastDynamicInvoke();
            else if (_isParams)
                _delegate.FastDynamicInvoke(new object[] {new object[] {arg1, arg2, arg3, arg4}});
            else
                _delegate.FastDynamicInvoke(arg1, arg2, arg3, arg4);
        }

        /// <summary />
        public static implicit operator Action<T1, T2, T3, T4>(LuaAction<T1, T2, T3, T4> action)
        {
            return action.Invoke;
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4>(Delegate del)
        {
            return new LuaAction<T1, T2, T3, T4>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4>(MulticastDelegate del)
        {
            return new LuaAction<T1, T2, T3, T4>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4>(Func<LuaResult> del)
        {
            return new LuaAction<T1, T2, T3, T4>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4>(Func<object, LuaResult> del)
        {
            return new LuaAction<T1, T2, T3, T4>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4>(Func<object, object, LuaResult> del)
        {
            return new LuaAction<T1, T2, T3, T4>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4>(Func<object, object, object, LuaResult> del)
        {
            return new LuaAction<T1, T2, T3, T4>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4>(Func<object, object, object, object, LuaResult> del)
        {
            return new LuaAction<T1, T2, T3, T4>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4>(
            Func<object, object, object, object, object, LuaResult> del)
        {
            return new LuaAction<T1, T2, T3, T4>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4>(
            Func<object, object, object, object, object, object, LuaResult> del)
        {
            return new LuaAction<T1, T2, T3, T4>(del);
        }
    }


    /// <summary>
    /// Represents multiple return values.
    /// </summary>
    public sealed class LuaAction<T1, T2, T3, T4, T5>
    {
        private readonly Delegate _delegate;
        private readonly bool _isAction;
        private readonly bool _isParams;

        /// <summary />
        public LuaAction(Delegate del)
        {
            var paramaters = del.Method.GetParameters();
            var paramCount = paramaters.Length;

            if (paramCount > 0)
                if (paramaters[0].ParameterType == typeof(Closure))
                    paramCount--;

            _isAction = paramCount == 0;
            _isParams = (paramCount == 1) && (paramaters[0].ParameterType == typeof(object[]));

            _delegate = del;
        }

        /// <summary>
        /// Invokes the action.
        /// </summary>
        public void Invoke(T1 arg1 = default(T1), T2 arg2 = default(T2), T3 arg3 = default(T3), T4 arg4 = default(T4),
            T5 arg5 = default(T5))
        {
            if (_isAction)
                _delegate.FastDynamicInvoke();
            else if (_isParams)
                _delegate.FastDynamicInvoke(new object[] {new object[] {arg1, arg2, arg3, arg4, arg5}});
            else
                _delegate.FastDynamicInvoke(arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary />
        public static implicit operator Action<T1, T2, T3, T4, T5>(LuaAction<T1, T2, T3, T4, T5> action)
        {
            return action.Invoke;
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4, T5>(Delegate del)
        {
            return new LuaAction<T1, T2, T3, T4, T5>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4, T5>(MulticastDelegate del)
        {
            return new LuaAction<T1, T2, T3, T4, T5>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4, T5>(Func<LuaResult> del)
        {
            return new LuaAction<T1, T2, T3, T4, T5>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4, T5>(Func<object, LuaResult> del)
        {
            return new LuaAction<T1, T2, T3, T4, T5>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4, T5>(Func<object, object, LuaResult> del)
        {
            return new LuaAction<T1, T2, T3, T4, T5>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4, T5>(Func<object, object, object, LuaResult> del)
        {
            return new LuaAction<T1, T2, T3, T4, T5>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4, T5>(
            Func<object, object, object, object, LuaResult> del)
        {
            return new LuaAction<T1, T2, T3, T4, T5>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4, T5>(
            Func<object, object, object, object, object, LuaResult> del)
        {
            return new LuaAction<T1, T2, T3, T4, T5>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4, T5>(
            Func<object, object, object, object, object, object, LuaResult> del)
        {
            return new LuaAction<T1, T2, T3, T4, T5>(del);
        }
    }


    /// <summary>
    /// Represents multiple return values.
    /// </summary>
    public sealed class LuaAction<T1, T2, T3, T4, T5, T6>
    {
        private readonly Delegate _delegate;
        private readonly bool _isAction;
        private readonly bool _isParams;

        /// <summary />
        public LuaAction(Delegate del)
        {
            var paramaters = del.Method.GetParameters();
            var paramCount = paramaters.Length;

            if (paramCount > 0)
                if (paramaters[0].ParameterType == typeof(Closure))
                    paramCount--;

            _isAction = paramCount == 0;
            _isParams = (paramCount == 1) && (paramaters[0].ParameterType == typeof(object[]));

            _delegate = del;
        }

        /// <summary>
        /// Invokes the action.
        /// </summary>
        public void Invoke(T1 arg1 = default(T1), T2 arg2 = default(T2), T3 arg3 = default(T3), T4 arg4 = default(T4),
            T5 arg5 = default(T5), T6 arg6 = default(T6))
        {
            if (_isAction)
                _delegate.FastDynamicInvoke();
            else if (_isParams)
                _delegate.FastDynamicInvoke(new object[] {new object[] {arg1, arg2, arg3, arg4, arg5, arg6}});
            else
                _delegate.FastDynamicInvoke(arg1, arg2, arg3, arg4, arg5, arg6);
        }

        /// <summary />
        public static implicit operator Action<T1, T2, T3, T4, T5, T6>(LuaAction<T1, T2, T3, T4, T5, T6> action)
        {
            return action.Invoke;
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4, T5, T6>(Delegate del)
        {
            return new LuaAction<T1, T2, T3, T4, T5, T6>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4, T5, T6>(MulticastDelegate del)
        {
            return new LuaAction<T1, T2, T3, T4, T5, T6>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4, T5, T6>(Func<LuaResult> del)
        {
            return new LuaAction<T1, T2, T3, T4, T5, T6>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4, T5, T6>(Func<object, LuaResult> del)
        {
            return new LuaAction<T1, T2, T3, T4, T5, T6>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4, T5, T6>(Func<object, object, LuaResult> del)
        {
            return new LuaAction<T1, T2, T3, T4, T5, T6>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4, T5, T6>(Func<object, object, object, LuaResult> del)
        {
            return new LuaAction<T1, T2, T3, T4, T5, T6>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4, T5, T6>(
            Func<object, object, object, object, LuaResult> del)
        {
            return new LuaAction<T1, T2, T3, T4, T5, T6>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4, T5, T6>(
            Func<object, object, object, object, object, LuaResult> del)
        {
            return new LuaAction<T1, T2, T3, T4, T5, T6>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4, T5, T6>(
            Func<object, object, object, object, object, object, LuaResult> del)
        {
            return new LuaAction<T1, T2, T3, T4, T5, T6>(del);
        }
    }


    /// <summary>
    /// Represents multiple return values.
    /// </summary>
    public sealed class LuaAction<T1, T2, T3, T4, T5, T6, T7>
    {
        private readonly Delegate _delegate;
        private readonly bool _isAction;
        private readonly bool _isParams;

        /// <summary />
        public LuaAction(Delegate del)
        {
            var paramaters = del.Method.GetParameters();
            var paramCount = paramaters.Length;

            if (paramCount > 0)
                if (paramaters[0].ParameterType == typeof(Closure))
                    paramCount--;

            _isAction = paramCount == 0;
            _isParams = (paramCount == 1) && (paramaters[0].ParameterType == typeof(object[]));

            _delegate = del;
        }

        /// <summary>
        /// Invokes the action.
        /// </summary>
        public void Invoke(T1 arg1 = default(T1), T2 arg2 = default(T2), T3 arg3 = default(T3), T4 arg4 = default(T4),
            T5 arg5 = default(T5), T6 arg6 = default(T6), T7 arg7 = default(T7))
        {
            if (_isAction)
                _delegate.FastDynamicInvoke();
            else if (_isParams)
                _delegate.FastDynamicInvoke(new object[] {new object[] {arg1, arg2, arg3, arg4, arg5, arg6, arg7}});
            else
                _delegate.FastDynamicInvoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        /// <summary />
        public static implicit operator Action<T1, T2, T3, T4, T5, T6, T7>(LuaAction<T1, T2, T3, T4, T5, T6, T7> action)
        {
            return action.Invoke;
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4, T5, T6, T7>(Delegate del)
        {
            return new LuaAction<T1, T2, T3, T4, T5, T6, T7>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4, T5, T6, T7>(MulticastDelegate del)
        {
            return new LuaAction<T1, T2, T3, T4, T5, T6, T7>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4, T5, T6, T7>(Func<LuaResult> del)
        {
            return new LuaAction<T1, T2, T3, T4, T5, T6, T7>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4, T5, T6, T7>(Func<object, LuaResult> del)
        {
            return new LuaAction<T1, T2, T3, T4, T5, T6, T7>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4, T5, T6, T7>(Func<object, object, LuaResult> del)
        {
            return new LuaAction<T1, T2, T3, T4, T5, T6, T7>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4, T5, T6, T7>(
            Func<object, object, object, LuaResult> del)
        {
            return new LuaAction<T1, T2, T3, T4, T5, T6, T7>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4, T5, T6, T7>(
            Func<object, object, object, object, LuaResult> del)
        {
            return new LuaAction<T1, T2, T3, T4, T5, T6, T7>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4, T5, T6, T7>(
            Func<object, object, object, object, object, LuaResult> del)
        {
            return new LuaAction<T1, T2, T3, T4, T5, T6, T7>(del);
        }

        /// <summary />
        public static implicit operator LuaAction<T1, T2, T3, T4, T5, T6, T7>(
            Func<object, object, object, object, object, object, LuaResult> del)
        {
            return new LuaAction<T1, T2, T3, T4, T5, T6, T7>(del);
        }
    }
}
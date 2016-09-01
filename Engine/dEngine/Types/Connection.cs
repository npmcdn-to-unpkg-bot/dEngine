﻿// Connection.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using dEngine.Services;

namespace dEngine
{
    /// <summary>
    /// An object with represents the connection between a listener and a signal.
    /// </summary>
    public sealed class Connection
    {
        private readonly Signal _signal;
        private readonly LuaAction _action;

        internal Connection(Signal signal, LuaAction action)
        {
            _signal = signal;
            _action = action;
        }

        /// <summary>
        /// Invokes the listener.
        /// </summary>
        public void Invoke()
        {
            _action.Invoke();
        }

        /// <summary>
        /// Disconnects the listener from the signal.
        /// </summary>
        public bool disconnect()
        {
            lock (_signal.Locker)
            {
                return _signal.Connections.Remove(this);
            }
        }
    }


    /// <summary>
    /// An object with represents the connection between a listener and a signal.
    /// </summary>
    public sealed class Connection<T1>
    {
        private readonly Signal<T1> _signal;
        private readonly LuaAction<T1> _action;

        internal Connection(Signal<T1> signal, LuaAction<T1> action)
        {
            _signal = signal;
            _action = action;
        }

        /// <summary>
        /// Invokes the listener.
        /// </summary>
        public void Invoke(T1 arg1 = default(T1))
        {
            try
            {
                _action.Invoke(arg1);
            }
            catch (Exception e)
            {
                // HACK: gets the LuaTable from target locals - unsure how reliable this is. Alternative is to add LuaExceptionData check in HandeScriptException
                //var script = _func.Target.Locals[0].Value["script"];
                var script = ScriptService.CurrentScript;

                ScriptService.HandleException(e, script.InstanceId);
            }
        }

        /// <summary>
        /// Disconnects the listener from the signal.
        /// </summary>
        public bool disconnect()
        {
            lock (_signal.Locker)
            {
                return _signal.Connections.Remove(this);
            }
        }
    }

    /// <summary>
    /// An object with represents the connection between a listener and a signal.
    /// </summary>
    public sealed class Connection<T1, T2>
    {
        private readonly Signal<T1, T2> _signal;
        private readonly LuaAction<T1, T2> _action;

        internal Connection(Signal<T1, T2> signal, LuaAction<T1, T2> action)
        {
            _signal = signal;
            _action = action;
        }

        /// <summary>
        /// Invokes the listener.
        /// </summary>
        public void Invoke(T1 arg1 = default(T1), T2 arg2 = default(T2))
        {
            try
            {
                _action.Invoke(arg1, arg2);
            }
            catch (Exception e)
            {
                // HACK: gets the LuaTable from target locals - unsure how reliable this is. Alternative is to add LuaExceptionData check in HandeScriptException
                //var script = _func.Target.Locals[0].Value["script"];
                var script = ScriptService.CurrentScript;

                ScriptService.HandleException(e, script.InstanceId);
            }
        }

        /// <summary>
        /// Disconnects the listener from the signal.
        /// </summary>
        public bool disconnect()
        {
            lock (_signal.Locker)
            {
                return _signal.Connections.Remove(this);
            }
        }
    }

    /// <summary>
    /// An object with represents the connection between a listener and a signal.
    /// </summary>
    public sealed class Connection<T1, T2, T3>
    {
        private readonly Signal<T1, T2, T3> _signal;
        private readonly LuaAction<T1, T2, T3> _action;

        internal Connection(Signal<T1, T2, T3> signal, LuaAction<T1, T2, T3> action)
        {
            _signal = signal;
            _action = action;
        }

        /// <summary>
        /// Invokes the listener.
        /// </summary>
        public void Invoke(T1 arg1 = default(T1), T2 arg2 = default(T2), T3 arg3 = default(T3))
        {
            try
            {
                _action.Invoke(arg1, arg2, arg3);
            }
            catch (Exception e)
            {
                // HACK: gets the LuaTable from target locals - unsure how reliable this is. Alternative is to add LuaExceptionData check in HandeScriptException
                //var script = _func.Target.Locals[0].Value["script"];
                var script = ScriptService.CurrentScript;

                ScriptService.HandleException(e, script.InstanceId);
            }
        }

        /// <summary>
        /// Disconnects the listener from the signal.
        /// </summary>
        public bool disconnect()
        {
            lock (_signal.Locker)
            {
                return _signal.Connections.Remove(this);
            }
        }
    }

    /// <summary>
    /// An object with represents the connection between a listener and a signal.
    /// </summary>
    public sealed class Connection<T1, T2, T3, T4>
    {
        private readonly Signal<T1, T2, T3, T4> _signal;
        private readonly LuaAction<T1, T2, T3, T4> _action;

        internal Connection(Signal<T1, T2, T3, T4> signal, LuaAction<T1, T2, T3, T4> action)
        {
            _signal = signal;
            _action = action;
        }

        /// <summary>
        /// Invokes the listener.
        /// </summary>
        public void Invoke(T1 arg1 = default(T1), T2 arg2 = default(T2), T3 arg3 = default(T3), T4 arg4 = default(T4))
        {
            try
            {
                _action.Invoke(arg1, arg2, arg3, arg4);
            }
            catch (Exception e)
            {
                // HACK: gets the LuaTable from target locals - unsure how reliable this is. Alternative is to add LuaExceptionData check in HandeScriptException
                //var script = _func.Target.Locals[0].Value["script"];
                var script = ScriptService.CurrentScript;

                ScriptService.HandleException(e, script.InstanceId);
            }
        }

        /// <summary>
        /// Disconnects the listener from the signal.
        /// </summary>
        public bool disconnect()
        {
            lock (_signal.Locker)
            {
                return _signal.Connections.Remove(this);
            }
        }
    }

    /// <summary>
    /// An object with represents the connection between a listener and a signal.
    /// </summary>
    public sealed class Connection<T1, T2, T3, T4, T5>
    {
        private readonly Signal<T1, T2, T3, T4, T5> _signal;
        private readonly LuaAction<T1, T2, T3, T4, T5> _action;

        internal Connection(Signal<T1, T2, T3, T4, T5> signal, LuaAction<T1, T2, T3, T4, T5> action)
        {
            _signal = signal;
            _action = action;
        }

        /// <summary>
        /// Invokes the listener.
        /// </summary>
        public void Invoke(T1 arg1 = default(T1), T2 arg2 = default(T2), T3 arg3 = default(T3), T4 arg4 = default(T4),
            T5 arg5 = default(T5))
        {
            try
            {
                _action.Invoke(arg1, arg2, arg3, arg4, arg5);
            }
            catch (Exception e)
            {
                // HACK: gets the LuaTable from target locals - unsure how reliable this is. Alternative is to add LuaExceptionData check in HandeScriptException
                //var script = _func.Target.Locals[0].Value["script"];
                var script = ScriptService.CurrentScript;

                ScriptService.HandleException(e, script.InstanceId);
            }
        }

        /// <summary>
        /// Disconnects the listener from the signal.
        /// </summary>
        public bool disconnect()
        {
            lock (_signal.Locker)
            {
                return _signal.Connections.Remove(this);
            }
        }
    }

    /// <summary>
    /// An object with represents the connection between a listener and a signal.
    /// </summary>
    public sealed class Connection<T1, T2, T3, T4, T5, T6>
    {
        private readonly Signal<T1, T2, T3, T4, T5, T6> _signal;
        private readonly LuaAction<T1, T2, T3, T4, T5, T6> _action;

        internal Connection(Signal<T1, T2, T3, T4, T5, T6> signal, LuaAction<T1, T2, T3, T4, T5, T6> action)
        {
            _signal = signal;
            _action = action;
        }

        /// <summary>
        /// Invokes the listener.
        /// </summary>
        public void Invoke(T1 arg1 = default(T1), T2 arg2 = default(T2), T3 arg3 = default(T3), T4 arg4 = default(T4),
            T5 arg5 = default(T5), T6 arg6 = default(T6))
        {
            try
            {
                _action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
            }
            catch (Exception e)
            {
                // HACK: gets the LuaTable from target locals - unsure how reliable this is. Alternative is to add LuaExceptionData check in HandeScriptException
                //var script = _func.Target.Locals[0].Value["script"];
                var script = ScriptService.CurrentScript;

                ScriptService.HandleException(e, script.InstanceId);
            }
        }

        /// <summary>
        /// Disconnects the listener from the signal.
        /// </summary>
        public bool disconnect()
        {
            lock (_signal.Locker)
            {
                return _signal.Connections.Remove(this);
            }
        }
    }

    /// <summary>
    /// An object with represents the connection between a listener and a signal.
    /// </summary>
    public sealed class Connection<T1, T2, T3, T4, T5, T6, T7>
    {
        private readonly Signal<T1, T2, T3, T4, T5, T6, T7> _signal;
        private readonly LuaAction<T1, T2, T3, T4, T5, T6, T7> _action;

        internal Connection(Signal<T1, T2, T3, T4, T5, T6, T7> signal, LuaAction<T1, T2, T3, T4, T5, T6, T7> action)
        {
            _signal = signal;
            _action = action;
        }

        /// <summary>
        /// Invokes the listener.
        /// </summary>
        public void Invoke(T1 arg1 = default(T1), T2 arg2 = default(T2), T3 arg3 = default(T3), T4 arg4 = default(T4),
            T5 arg5 = default(T5), T6 arg6 = default(T6), T7 arg7 = default(T7))
        {
            try
            {
                _action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            }
            catch (Exception e)
            {
                // HACK: gets the LuaTable from target locals - unsure how reliable this is. Alternative is to add LuaExceptionData check in HandeScriptException
                //var script = _func.Target.Locals[0].Value["script"];
                var script = ScriptService.CurrentScript;

                ScriptService.HandleException(e, script.InstanceId);
            }
        }

        /// <summary>
        /// Disconnects the listener from the signal.
        /// </summary>
        public bool disconnect()
        {
            lock (_signal.Locker)
            {
                return _signal.Connections.Remove(this);
            }
        }
    }
}
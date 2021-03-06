// Signal.cs - dEngine
// Copyright � https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections.Generic;
using System.Diagnostics;
using dEngine.Instances;
using dEngine.Instances.Attributes;
using dEngine.Services;
using Neo.IronLua;

namespace dEngine
{
    /// <summary>
    /// A signal allows listener functions to be connected, which are called when the signal is fired.
    /// </summary>
    public class Signal
    {
        internal List<Connection> Connections;

        /// <summary>
        /// Initializes a new signal.
        /// </summary>
        /// <param name="host">The instance which this signal will be a member of.</param>
        public Signal(Instance host)
        {
            Connections = new List<Connection>();
            Owner = host;

            if (host.Destroyed != null) // if host.Destroyed is null, then this is the Destroyed signal.
                host.Destroyed.Event += Dispose;
        }

        /// <summary>
        /// The owner of this signal.
        /// </summary>
        public Instance Owner { get; protected set; }

        internal event Action Event;

        internal void Fire()
        {
            var connections = Connections;
            var count = connections.Count;

            for (var i = 0; i < count; i++)
                connections[i].Invoke();

            Event?.Invoke();
        }

        internal void Connect(Action action)
        {
            Event += action;
        }

        internal void Disconnect(Action action)
        {
            Event -= action;
        }

        /// <summary>
        /// Connects the given listener to the signal.
        /// </summary>
        public Connection connect(LuaAction listener)
        {
            if (listener == null)
                throw new ArgumentNullException(nameof(listener), "Listener function was null.");

            var connection = new Connection(this, listener);
            lock (Locker)
            {
                Connections.Add(connection);
            }
            return connection;
        }


        /// <summary>
        /// Yields the current thread until this signal next fires.
        /// </summary>
        [YieldFunction]
        public double wait()
        {
            var thread = (LuaThread)LuaThread.running().Values[0];
            var sw = Stopwatch.StartNew();

            Action callback = null;
            callback = () =>
            {
                ScriptService.ResumeThread(thread);
                Disconnect(callback);
            };

            Connect(callback);
            ScriptService.YieldThread();
            return sw.Elapsed.TotalSeconds;
        }

        /// <summary>
        /// Disconnects all connections and destroys the signal.
        /// </summary>
        public void Dispose()
        {
            lock (Locker)
            {
                var connections = Connections;
                for (var i = connections.Count - 1; i >= 0; i--)
                    connections[i].disconnect();
            }
            var e = Event;
            if (e == null) return;
            foreach (var d in e.GetInvocationList())
                Event -= (Action)d;
        }

        internal readonly object Locker = new object();
    }

    /// <summary>
    /// A signal allows listener functions to be connected, which are called when the signal is fired.
    /// </summary>
    public class Signal<T1> : IDisposable
    {
        internal List<Connection<T1>> Connections;

        /// <summary>
        /// Initializes a new signal.
        /// </summary>
        /// <param name="host">The instance which this signal will be a member of.</param>
        public Signal(Instance host)
        {
            Connections = new List<Connection<T1>>();
            Owner = host;
            host.Destroyed.Event += Dispose;
        }

        /// <summary>
        /// The owner of this signal.
        /// </summary>
        public Instance Owner { get; protected set; }

        /// <summary>
        /// Disconnects all connections and destroys the signal.
        /// </summary>
        public void Dispose()
        {
            lock (Locker)
            {
                var connections = Connections;
                for (var i = connections.Count - 1; i >= 0; i--)
                    connections[i].disconnect();
            }

            var e = Event;
            if (e == null) return;
            foreach (var d in e.GetInvocationList())
                Event -= d as Action<T1>;
        }

        internal event Action<T1> Event;

        internal void Connect(Action<T1> action)
        {
            Event += action;
        }

        internal void Disconnect(Action<T1> action)
        {
            Event -= action;
        }

        internal void Fire(T1 arg1 = default(T1))
        {
            lock (Locker)
            {
                var connections = Connections;
                var count = connections.Count;
                for (var i = 0; i < count; i++)
                    connections[i].Invoke(arg1);
            }

            Event?.Invoke(arg1);
        }

        internal void Fire(ref T1 arg1)
        {
            lock (Locker)
            {
                var connections = Connections;
                var count = connections.Count;
                for (var i = 0; i < count; i++)
                    connections[i].Invoke(arg1);
            }

            Event?.Invoke(arg1);
        }

        /// <summary>
        /// Connects the given listener to the signal.
        /// </summary>
        public Connection<T1> connect(LuaAction<T1> listener)
        {
            if (listener == null)
                throw new ArgumentNullException(nameof(listener), "Listener function was null.");

            var connection = new Connection<T1>(this, listener);

            lock (Locker)
            {
                Connections.Add(connection);
            }

            return connection;
        }

        /// <summary>
        /// Yields the current thread until this signal next fires.
        /// </summary>
        [YieldFunction]
        public double wait()
        {
            var thread = (LuaThread)LuaThread.running().Values[0];
            var sw = Stopwatch.StartNew();

            Action<T1> callback = null;
            callback = arg1 =>
            {
                ScriptService.ResumeThread(thread);
                Disconnect(callback);
            };

            Connect(callback);
            ScriptService.YieldThread();
            return sw.Elapsed.TotalSeconds;
        }

        internal readonly object Locker = new object();
    }

    /// <summary>
    /// A signal allows listener functions to be connected, which are called when the signal is fired.
    /// </summary>
    public class Signal<T1, T2> : IDisposable
    {
        internal List<Connection<T1, T2>> Connections;

        /// <summary>
        /// Initializes a new signal.
        /// </summary>
        /// <param name="host">The instance which this signal will be a member of.</param>
        public Signal(Instance host)
        {
            Connections = new List<Connection<T1, T2>>();
            Owner = host;
            host.Destroyed.Event += Dispose;
        }

        /// <summary>
        /// The owner of this signal.
        /// </summary>
        public Instance Owner { get; protected set; }

        /// <summary>
        /// Disconnects all connections and destroys the signal.
        /// </summary>
        public void Dispose()
        {
            lock (Locker)
            {
                var connections = Connections;
                for (var i = connections.Count - 1; i >= 0; i--)
                    connections[i].disconnect();
            }

            var e = Event;
            if (e == null) return;
            foreach (var d in e.GetInvocationList())
                Event -= d as Action<T1, T2>;
        }

        internal event Action<T1, T2> Event;

        internal void Connect(Action<T1, T2> action)
        {
            Event += action;
        }

        internal void Disconnect(Action<T1, T2> action)
        {
            Event -= action;
        }

        internal void Fire(T1 arg1 = default(T1), T2 arg2 = default(T2))
        {
            lock (Locker)
            {
                var connections = Connections;
                var count = connections.Count;
                for (var i = 0; i < count; i++)
                    connections[i].Invoke(arg1, arg2);
            }

            Event?.Invoke(arg1, arg2);
        }

        internal void Fire(ref T1 arg1, ref T2 arg2)
        {
            lock (Locker)
            {
                var connections = Connections;
                var count = connections.Count;
                for (var i = 0; i < count; i++)
                    connections[i].Invoke(arg1, arg2);
            }

            Event?.Invoke(arg1, arg2);
        }

        /// <summary>
        /// Connects the given listener to the signal.
        /// </summary>
        public Connection<T1, T2> connect(LuaAction<T1, T2> listener)
        {
            if (listener == null)
                throw new ArgumentNullException(nameof(listener), "Listener function was null.");

            var connection = new Connection<T1, T2>(this, listener);

            lock (Locker)
            {
                Connections.Add(connection);
            }

            return connection;
        }

        /// <summary>
        /// Yields the current thread until this signal next fires.
        /// </summary>
        [YieldFunction]
        public double wait()
        {
            var thread = (LuaThread)LuaThread.running().Values[0];
            var sw = Stopwatch.StartNew();

            Action<T1, T2> callback = null;
            callback = (arg1, arg2) =>
            {
                ScriptService.ResumeThread(thread);
                Disconnect(callback);
            };

            Connect(callback);
            ScriptService.YieldThread();
            return sw.Elapsed.TotalSeconds;
        }

        internal readonly object Locker = new object();
    }

    /// <summary>
    /// A signal allows listener functions to be connected, which are called when the signal is fired.
    /// </summary>
    public class Signal<T1, T2, T3> : IDisposable
    {
        internal List<Connection<T1, T2, T3>> Connections;

        /// <summary>
        /// Initializes a new signal.
        /// </summary>
        /// <param name="host">The instance which this signal will be a member of.</param>
        public Signal(Instance host)
        {
            Connections = new List<Connection<T1, T2, T3>>();
            Owner = host;
            host.Destroyed.Event += Dispose;
        }

        /// <summary>
        /// The owner of this signal.
        /// </summary>
        public Instance Owner { get; protected set; }

        /// <summary>
        /// Disconnects all connections and destroys the signal.
        /// </summary>
        public void Dispose()
        {
            lock (Locker)
            {
                var connections = Connections;
                for (var i = connections.Count - 1; i >= 0; i--)
                    connections[i].disconnect();
            }

            var e = Event;
            if (e == null) return;
            foreach (var d in e.GetInvocationList())
                Event -= d as Action<T1, T2, T3>;
        }

        internal event Action<T1, T2, T3> Event;

        internal void Connect(Action<T1, T2, T3> action)
        {
            Event += action;
        }

        internal void Disconnect(Action<T1, T2, T3> action)
        {
            Event -= action;
        }

        internal void Fire(T1 arg1 = default(T1), T2 arg2 = default(T2), T3 arg3 = default(T3))
        {
            lock (Locker)
            {
                var connections = Connections;
                var count = connections.Count;
                for (var i = 0; i < count; i++)
                    connections[i].Invoke(arg1, arg2, arg3);
            }

            Event?.Invoke(arg1, arg2, arg3);
        }

        internal void Fire(ref T1 arg1, ref T2 arg2, ref T3 arg3)
        {
            lock (Locker)
            {
                var connections = Connections;
                var count = connections.Count;
                for (var i = 0; i < count; i++)
                    connections[i].Invoke(arg1, arg2, arg3);
            }

            Event?.Invoke(arg1, arg2, arg3);
        }

        /// <summary>
        /// Connects the given listener to the signal.
        /// </summary>
        public Connection<T1, T2, T3> connect(LuaAction<T1, T2, T3> listener)
        {
            if (listener == null)
                throw new ArgumentNullException(nameof(listener), "Listener function was null.");

            var connection = new Connection<T1, T2, T3>(this, listener);

            lock (Locker)
            {
                Connections.Add(connection);
            }

            return connection;
        }

        /// <summary>
        /// Yields the current thread until this signal next fires.
        /// </summary>
        [YieldFunction]
        public double wait()
        {
            var thread = (LuaThread)LuaThread.running().Values[0];
            var sw = Stopwatch.StartNew();

            Action<T1, T2, T3> callback = null;
            callback = (arg1, arg2, arg3) =>
            {
                ScriptService.ResumeThread(thread);
                Disconnect(callback);
            };

            Connect(callback);
            ScriptService.YieldThread();
            return sw.Elapsed.TotalSeconds;
        }

        internal readonly object Locker = new object();
    }

    /// <summary>
    /// A signal allows listener functions to be connected, which are called when the signal is fired.
    /// </summary>
    public class Signal<T1, T2, T3, T4> : IDisposable
    {
        internal List<Connection<T1, T2, T3, T4>> Connections;

        /// <summary>
        /// Initializes a new signal.
        /// </summary>
        /// <param name="host">The instance which this signal will be a member of.</param>
        public Signal(Instance host)
        {
            Connections = new List<Connection<T1, T2, T3, T4>>();
            Owner = host;
            host.Destroyed.Event += Dispose;
        }

        /// <summary>
        /// The owner of this signal.
        /// </summary>
        public Instance Owner { get; protected set; }

        /// <summary>
        /// Disconnects all connections and destroys the signal.
        /// </summary>
        public void Dispose()
        {
            lock (Locker)
            {
                var connections = Connections;
                for (var i = connections.Count - 1; i >= 0; i--)
                    connections[i].disconnect();
            }

            var e = Event;
            if (e == null) return;
            foreach (var d in e.GetInvocationList())
                Event -= d as Action<T1, T2, T3, T4>;
        }

        internal event Action<T1, T2, T3, T4> Event;

        internal void Connect(Action<T1, T2, T3, T4> action)
        {
            Event += action;
        }

        internal void Disconnect(Action<T1, T2, T3, T4> action)
        {
            Event -= action;
        }

        internal void Fire(T1 arg1 = default(T1), T2 arg2 = default(T2), T3 arg3 = default(T3), T4 arg4 = default(T4))
        {
            lock (Locker)
            {
                var connections = Connections;
                var count = connections.Count;
                for (var i = 0; i < count; i++)
                    connections[i].Invoke(arg1, arg2, arg3, arg4);
            }

            Event?.Invoke(arg1, arg2, arg3, arg4);
        }

        internal void Fire(ref T1 arg1, ref T2 arg2, ref T3 arg3, ref T4 arg4)
        {
            lock (Locker)
            {
                var connections = Connections;
                var count = connections.Count;
                for (var i = 0; i < count; i++)
                    connections[i].Invoke(arg1, arg2, arg3, arg4);
            }

            Event?.Invoke(arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Connects the given listener to the signal.
        /// </summary>
        public Connection<T1, T2, T3, T4> connect(LuaAction<T1, T2, T3, T4> listener)
        {
            if (listener == null)
                throw new ArgumentNullException(nameof(listener), "Listener function was null.");

            var connection = new Connection<T1, T2, T3, T4>(this, listener);

            lock (Locker)
            {
                Connections.Add(connection);
            }

            return connection;
        }

        /// <summary>
        /// Yields the current thread until this signal next fires.
        /// </summary>
        [YieldFunction]
        public double wait()
        {
            var thread = (LuaThread)LuaThread.running().Values[0];
            var sw = Stopwatch.StartNew();

            Action<T1, T2, T3, T4> callback = null;
            callback = (arg1, arg2, arg3, arg4) =>
            {
                ScriptService.ResumeThread(thread);
                Disconnect(callback);
            };

            Connect(callback);
            ScriptService.YieldThread();
            return sw.Elapsed.TotalSeconds;
        }

        internal readonly object Locker = new object();
    }

    /// <summary>
    /// A signal allows listener functions to be connected, which are called when the signal is fired.
    /// </summary>
    public class Signal<T1, T2, T3, T4, T5> : IDisposable
    {
        internal List<Connection<T1, T2, T3, T4, T5>> Connections;

        /// <summary>
        /// Initializes a new signal.
        /// </summary>
        /// <param name="host">The instance which this signal will be a member of.</param>
        public Signal(Instance host)
        {
            Connections = new List<Connection<T1, T2, T3, T4, T5>>();
            Owner = host;
            host.Destroyed.Event += Dispose;
        }

        /// <summary>
        /// The owner of this signal.
        /// </summary>
        public Instance Owner { get; protected set; }

        /// <summary>
        /// Disconnects all connections and destroys the signal.
        /// </summary>
        public void Dispose()
        {
            lock (Locker)
            {
                var connections = Connections;
                for (var i = connections.Count - 1; i >= 0; i--)
                    connections[i].disconnect();
            }

            var e = Event;
            if (e == null) return;
            foreach (var d in e.GetInvocationList())
                Event -= d as Action<T1, T2, T3, T4, T5>;
        }

        internal event Action<T1, T2, T3, T4, T5> Event;

        internal void Connect(Action<T1, T2, T3, T4, T5> action)
        {
            Event += action;
        }

        internal void Disconnect(Action<T1, T2, T3, T4, T5> action)
        {
            Event -= action;
        }

        internal void Fire(T1 arg1 = default(T1), T2 arg2 = default(T2), T3 arg3 = default(T3), T4 arg4 = default(T4),
            T5 arg5 = default(T5))
        {
            lock (Locker)
            {
                var connections = Connections;
                var count = connections.Count;
                for (var i = 0; i < count; i++)
                    connections[i].Invoke(arg1, arg2, arg3, arg4, arg5);
            }

            Event?.Invoke(arg1, arg2, arg3, arg4, arg5);
        }

        internal void Fire(ref T1 arg1, ref T2 arg2, ref T3 arg3, ref T4 arg4, ref T5 arg5)
        {
            lock (Locker)
            {
                var connections = Connections;
                var count = connections.Count;
                for (var i = 0; i < count; i++)
                    connections[i].Invoke(arg1, arg2, arg3, arg4, arg5);
            }

            Event?.Invoke(arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Connects the given listener to the signal.
        /// </summary>
        public Connection<T1, T2, T3, T4, T5> connect(LuaAction<T1, T2, T3, T4, T5> listener)
        {
            if (listener == null)
                throw new ArgumentNullException(nameof(listener), "Listener function was null.");

            var connection = new Connection<T1, T2, T3, T4, T5>(this, listener);

            lock (Locker)
            {
                Connections.Add(connection);
            }

            return connection;
        }

        /// <summary>
        /// Yields the current thread until this signal next fires.
        /// </summary>
        [YieldFunction]
        public double wait()
        {
            var thread = (LuaThread)LuaThread.running().Values[0];
            var sw = Stopwatch.StartNew();

            Action<T1, T2, T3, T4, T5> callback = null;
            callback = (arg1, arg2, arg3, arg4, arg5) =>
            {
                ScriptService.ResumeThread(thread);
                Disconnect(callback);
            };

            Connect(callback);
            ScriptService.YieldThread();
            return sw.Elapsed.TotalSeconds;
        }

        internal readonly object Locker = new object();
    }

    /// <summary>
    /// A signal allows listener functions to be connected, which are called when the signal is fired.
    /// </summary>
    public class Signal<T1, T2, T3, T4, T5, T6> : IDisposable
    {
        internal List<Connection<T1, T2, T3, T4, T5, T6>> Connections;

        /// <summary>
        /// Initializes a new signal.
        /// </summary>
        /// <param name="host">The instance which this signal will be a member of.</param>
        public Signal(Instance host)
        {
            Connections = new List<Connection<T1, T2, T3, T4, T5, T6>>();
            Owner = host;
            host.Destroyed.Event += Dispose;
        }

        /// <summary>
        /// The owner of this signal.
        /// </summary>
        public Instance Owner { get; protected set; }

        /// <summary>
        /// Disconnects all connections and destroys the signal.
        /// </summary>
        public void Dispose()
        {
            lock (Locker)
            {
                var connections = Connections;
                for (var i = connections.Count - 1; i >= 0; i--)
                    connections[i].disconnect();
            }

            var e = Event;
            if (e == null) return;
            foreach (var d in e.GetInvocationList())
                Event -= d as Action<T1, T2, T3, T4, T5, T6>;
        }

        internal event Action<T1, T2, T3, T4, T5, T6> Event;

        internal void Connect(Action<T1, T2, T3, T4, T5, T6> action)
        {
            Event += action;
        }

        internal void Disconnect(Action<T1, T2, T3, T4, T5, T6> action)
        {
            Event -= action;
        }

        internal void Fire(T1 arg1 = default(T1), T2 arg2 = default(T2), T3 arg3 = default(T3), T4 arg4 = default(T4),
            T5 arg5 = default(T5), T6 arg6 = default(T6))
        {
            lock (Locker)
            {
                var connections = Connections;
                var count = connections.Count;
                for (var i = 0; i < count; i++)
                    connections[i].Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
            }

            Event?.Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
        }

        internal void Fire(ref T1 arg1, ref T2 arg2, ref T3 arg3, ref T4 arg4, ref T5 arg5, ref T6 arg6)
        {
            lock (Locker)
            {
                var connections = Connections;
                var count = connections.Count;
                for (var i = 0; i < count; i++)
                    connections[i].Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
            }

            Event?.Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
        }

        /// <summary>
        /// Connects the given listener to the signal.
        /// </summary>
        public Connection<T1, T2, T3, T4, T5, T6> connect(LuaAction<T1, T2, T3, T4, T5, T6> listener)
        {
            if (listener == null)
                throw new ArgumentNullException(nameof(listener), "Listener function was null.");

            var connection = new Connection<T1, T2, T3, T4, T5, T6>(this, listener);

            lock (Locker)
            {
                Connections.Add(connection);
            }

            return connection;
        }

        /// <summary>
        /// Yields the current thread until this signal next fires.
        /// </summary>
        [YieldFunction]
        public double wait()
        {
            var thread = (LuaThread)LuaThread.running().Values[0];
            var sw = Stopwatch.StartNew();

            Action<T1, T2, T3, T4, T5, T6> callback = null;
            callback = (arg1, arg2, arg3, arg4, arg5, arg6) =>
            {
                ScriptService.ResumeThread(thread);
                Disconnect(callback);
            };

            Connect(callback);
            ScriptService.YieldThread();
            return sw.Elapsed.TotalSeconds;
        }

        internal readonly object Locker = new object();
    }

    /// <summary>
    /// A signal allows listener functions to be connected, which are called when the signal is fired.
    /// </summary>
    public class Signal<T1, T2, T3, T4, T5, T6, T7> : IDisposable
    {
        internal List<Connection<T1, T2, T3, T4, T5, T6, T7>> Connections;

        /// <summary>
        /// Initializes a new signal.
        /// </summary>
        /// <param name="host">The instance which this signal will be a member of.</param>
        public Signal(Instance host)
        {
            Connections = new List<Connection<T1, T2, T3, T4, T5, T6, T7>>();
            Owner = host;
            host.Destroyed.Event += Dispose;
        }

        /// <summary>
        /// The owner of this signal.
        /// </summary>
        public Instance Owner { get; protected set; }

        /// <summary>
        /// Disconnects all connections and destroys the signal.
        /// </summary>
        public void Dispose()
        {
            lock (Locker)
            {
                var connections = Connections;
                for (var i = connections.Count - 1; i >= 0; i--)
                    connections[i].disconnect();
            }

            var e = Event;
            if (e == null) return;
            foreach (var d in e.GetInvocationList())
                Event -= d as Action<T1, T2, T3, T4, T5, T6, T7>;
        }

        internal event Action<T1, T2, T3, T4, T5, T6, T7> Event;

        internal void Connect(Action<T1, T2, T3, T4, T5, T6, T7> action)
        {
            Event += action;
        }

        internal void Disconnect(Action<T1, T2, T3, T4, T5, T6, T7> action)
        {
            Event -= action;
        }

        internal void Fire(T1 arg1 = default(T1), T2 arg2 = default(T2), T3 arg3 = default(T3), T4 arg4 = default(T4),
            T5 arg5 = default(T5), T6 arg6 = default(T6), T7 arg7 = default(T7))
        {
            lock (Locker)
            {
                var connections = Connections;
                var count = connections.Count;
                for (var i = 0; i < count; i++)
                    connections[i].Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            }

            Event?.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        internal void Fire(ref T1 arg1, ref T2 arg2, ref T3 arg3, ref T4 arg4, ref T5 arg5, ref T6 arg6, ref T7 arg7)
        {
            lock (Locker)
            {
                var connections = Connections;
                var count = connections.Count;
                for (var i = 0; i < count; i++)
                    connections[i].Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            }

            Event?.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        /// <summary>
        /// Connects the given listener to the signal.
        /// </summary>
        public Connection<T1, T2, T3, T4, T5, T6, T7> connect(LuaAction<T1, T2, T3, T4, T5, T6, T7> listener)
        {
            if (listener == null)
                throw new ArgumentNullException(nameof(listener), "Listener function was null.");

            var connection = new Connection<T1, T2, T3, T4, T5, T6, T7>(this, listener);

            lock (Locker)
            {
                Connections.Add(connection);
            }

            return connection;
        }

        /// <summary>
        /// Yields the current thread until this signal next fires.
        /// </summary>
        [YieldFunction]
        public double wait()
        {
            var thread = (LuaThread)LuaThread.running().Values[0];
            var sw = Stopwatch.StartNew();

            Action<T1, T2, T3, T4, T5, T6, T7> callback = null;
            callback = (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            {
                ScriptService.ResumeThread(thread);
                Disconnect(callback);
            };

            Connect(callback);
            ScriptService.YieldThread();
            return sw.Elapsed.TotalSeconds;
        }

        internal readonly object Locker = new object();
    }
}
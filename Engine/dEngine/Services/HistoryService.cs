﻿// HistoryService.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using dEngine.Instances;
using dEngine.Instances.Attributes;
using dEngine.Settings.Global;

namespace dEngine.Services
{
    /// <summary>
    /// A service for undo/redo.
    /// </summary>
    [TypeId(194)]
    [ExplorerOrder(-1)]
    public class HistoryService : Service
    {
        internal static HistoryService Service;
        private readonly C5.LinkedList<HistoryAction> _redoStack;
        private readonly C5.LinkedList<HistoryAction> _undoStack;
        private readonly List<string> _waypoints;

        private bool _enabled;

        /// <inheirtdoc />
        public HistoryService()
        {
            Service = this;

            _undoStack = new C5.LinkedList<HistoryAction>();
            _redoStack = new C5.LinkedList<HistoryAction>();
            _waypoints = new List<string>();

            Undone = new Signal<string>(this);
            Redone = new Signal<string>(this);
            WaypointSet = new Signal<string>(this);

            ContextActionService.Register("history.undo", () =>
            {
                if (CanUndo()) Undo();
            });
            ContextActionService.Register("history.redo", () =>
            {
                if (CanRedo()) Redo();
            });

            _enabled = true;
        }

        internal static event Action<HistoryAction> ActionExecuted;

        /// <summary>
        /// Sets whether or not the service is enabled.
        /// </summary>
        public void SetEnabled(bool enabled)
        {
            lock (Locker)
            {
                _enabled = enabled;

                if (!enabled)
                {
                    _undoStack.Clear();
                    _redoStack.Clear();
                }
            }
        }

        internal static void ExecuteAction(HistoryAction action)
        {
            lock (Service.Locker)
            {
                Service._redoStack.Clear();
                Service._undoStack.Push(action);

                action.Execute();
                ActionExecuted?.Invoke(action);

                if (DebugSettings.LogHistoryActions)
                    Service.Logger.Trace($"HistoryService: action pushed");
            }
        }

        internal static void Waypoint(string waypoint)
        {
            Service.SetWaypoint(waypoint);
        }

        /// <summary>
        /// Saves a waypoint in history.
        /// </summary>
        /// <param name="waypoint">The name of the waypoint.</param>
        public void SetWaypoint(string waypoint)
        {
            lock (Locker)
            {
                waypoint = waypoint ?? "";
                var actionCount = 0;

                for (var i = _undoStack.Count - 1; i >= 0; i--)
                {
                    var action = _undoStack[i];
                    if (action.Waypoint != -1)
                        break;
                    action.Waypoint = _waypoints.Count;
                    actionCount++;
                }
                _waypoints.Add(waypoint);

                WaypointSet.Fire(waypoint);

                if (DebugSettings.LogHistoryWaypoints)
                    Logger.Trace(
                        $"HistoryService: Waypoint #{_waypoints.Count - 1} \"{waypoint}\" set. ({actionCount} actions)");
            }
        }

        /// <summary>
        /// Executes the last undone action.
        /// </summary>
        public void Redo()
        {
            lock (Locker)
            {
                if (!CanRedoInternal().Item1)
                    throw new InvalidOperationException("Cannot redo: nothing to redo.");

                var targetWaypoint = -1;

                var actionCount = 0;
                while (!_redoStack.IsEmpty)
                {
                    var action = _redoStack.Last;

                    if ((action.Waypoint != -1) && (targetWaypoint == -1))
                        targetWaypoint = action.Waypoint;

                    if (action.Waypoint != targetWaypoint)
                        break;

                    _redoStack.RemoveLast();
                    _undoStack.Push(action);
                    action.Execute();
                    actionCount++;
                }

                if (DebugSettings.LogHistoryEvents)
                    Logger.Trace(
                        $"HistoryService: Redo Waypoint{targetWaypoint} ({_waypoints[targetWaypoint]}). ({actionCount} actions.)");

                Redone.Fire(_waypoints[targetWaypoint]);
            }
        }

        /// <summary>
        /// Undoes the last action.
        /// </summary>
        public void Undo()
        {
            lock (Locker)
            {
                if (!CanUndoInternal().Item1)
                    throw new InvalidOperationException("Cannot undo: nothing to undo.");

                var targetWaypoint = -1;

                var actionCount = 0;

                while (!_undoStack.IsEmpty)
                {
                    var action = _undoStack.Last;

                    if ((action.Waypoint != -1) && (targetWaypoint == -1))
                        targetWaypoint = action.Waypoint;

                    if (action.Waypoint != targetWaypoint)
                        break;

                    _undoStack.Remove(action);
                    _redoStack.Push(action);
                    action.Undo();
                    actionCount++;
                }

                Debug.Assert(actionCount > 0, "actionCount > 0");

                if (DebugSettings.LogHistoryEvents)
                    Logger.Trace(
                        $"HistoryService: Undo Waypoint{targetWaypoint} ({_waypoints[targetWaypoint]}). ({actionCount} actions.)");

                Undone.Fire(_waypoints[targetWaypoint]);
            }
        }

        /// <summary>
        /// Determines if <see cref="Undo" /> can be successfully called.
        /// </summary>
        public LuaTuple<bool, string> CanUndo()
        {
            return CanUndoInternal();
        }

        /// <summary>
        /// Determines if <see cref="Redo" /> can be successfully called.
        /// </summary>
        public LuaTuple<bool, string> CanRedo()
        {
            return CanRedoInternal();
        }

        internal static Tuple<bool, string> CanUndoInternal()
        {
            var stack = Service?._undoStack;
            var peek = PeekLatestWaypoint(stack);
            return peek != null
                ? new Tuple<bool, string>(true, Service._waypoints[peek.Waypoint])
                : new Tuple<bool, string>(false, null);
        }

        internal static Tuple<bool, string> CanRedoInternal()
        {
            var stack = Service?._redoStack;
            var peek = PeekLatestWaypoint(stack);
            return peek != null
                ? new Tuple<bool, string>(true, Service._waypoints[peek.Waypoint])
                : new Tuple<bool, string>(false, null);
        }

        private static T PeekLatestWaypoint<T>(IEnumerable<T> stack) where T : HistoryAction
        {
            return stack?.FirstOrDefault(action => action.Waypoint != -1);
        }

        internal static object GetExisting()
        {
            return DataModel.GetService<HistoryService>();
        }

        /// <summary>
        /// Fired when a waypoint is redone.
        /// </summary>
        public readonly Signal<string> Redone;

        /// <summary>
        /// Fired when a waypoint is undone.
        /// </summary>
        public readonly Signal<string> Undone;

        /// <summary>
        /// Fired when a waypoint is set.
        /// </summary>
        public readonly Signal<string> WaypointSet;

        internal abstract class HistoryAction
        {
            protected HistoryAction()
            {
                Waypoint = -1;
            }

            public int Waypoint { get; set; }
            public abstract void Undo();
            public abstract void Execute();
        }
    }
}
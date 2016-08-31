// ToolManager.cs - dEditor
// Copyright (C) 2016-2016 DanDev (dandev.sco@gmail.com)
// 
// This library is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// You should have received a copy of the GNU General Public
// along with this program. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using dEditor.Tools.Building;
using dEngine;
using dEngine.Instances;
using dEngine.Services;

namespace dEditor.Tools
{
    public static class ToolManager
    {
        private static Instance _primaryItem;
        private static StudioTool _selectedTool;

        static ToolManager()
        {
            MoveTool = new MoveTool();
            SelectTool = new SelectTool();
            ScaleTool = new ScaleTool();
            RotateTool = new RotateTool();

            SelectionService.Service.SelectionChanged.Event +=
                () => { ContextActionService.SetState("selectionEmpty", SelectionService.SelectionCount == 0); };

            ContextActionService.Register("tools.selectTool", () => SelectTool.IsEquipped = true);
            ContextActionService.Register("tools.moveTool", () => MoveTool.IsEquipped = true);
            ContextActionService.Register("tools.scaleTool", () => ScaleTool.IsEquipped = true);
            ContextActionService.Register("tools.rotateTool", () => RotateTool.IsEquipped = true);
            ContextActionService.Register("pickerInsideModels", PickerInsideModels);
            ContextActionService.Register("groupSelection", GroupSelection);
            ContextActionService.Register("ungroupSelection", UngroupSelection);
        }

        public static MoveTool MoveTool { get; }
        public static SelectTool SelectTool { get; }
        public static ScaleTool ScaleTool { get; }
        public static RotateTool RotateTool { get; }

        public static StudioTool SelectedTool
        {
            get { return _selectedTool; }
            internal set
            {
                _selectedTool = value;
                SelectedToolChanged?.Invoke(value);
            }
        }

        public static Instance PrimaryItem
        {
            get { return _primaryItem; }
            set
            {
                var last = _primaryItem;
                _primaryItem = value;
                PrimaryItemChanged?.Invoke(value, last);
            }
        }

        private static void GroupSelection()
        {
            if (SelectionService.Selection.Any(s => s.ParentLocked))
            {
                Editor.Current.Dispatcher.InvokeAsync(() =>
                {
                    MessageBox.Show("The selected objects cannot be grouped.", "dEditor");
                });
                return;
            }
            HistoryService.ExecuteAction(new GroupSelectionAction());
            HistoryService.Waypoint("Group");
        }

        private static void UngroupSelection()
        {
            HistoryService.ExecuteAction(new UngroupSelectionAction());
            HistoryService.Waypoint("Ungroup");
        }

        public static void PickerInsideModels(bool pressed)
        {
        }

        public static void RotateSelectionY()
        {
        }

        public static void RotateSelectionX()
        {
        }

        public static void ToggleCoordinates()
        {
        }

        public static event Action<Instance, Instance> PrimaryItemChanged;
        public static event Action<StudioTool> SelectedToolChanged;

        internal class UngroupSelectionAction : HistoryService.HistoryAction
        {
            private readonly IEnumerable<ModelState> _models;

            public UngroupSelectionAction()
            {
                _models = SelectionService.Selection.OfType<Model>().Select(m => new ModelState(m));
            }

            public override void Undo()
            {
                foreach (var state in _models)
                {
                    state.Model.Parent = state.Parent;
                    foreach (var child in state.Children)
                        child.Parent = state.Model;
                }
            }

            public override void Execute()
            {
                foreach (var state in _models)
                    foreach (var child in state.Children)
                        child.Parent = state.Parent;
            }

            private class ModelState
            {
                public ModelState(Model model)
                {
                    Model = model;
                    Parent = model.Parent;
                    Children = model.Children.ToList();
                }

                public readonly Model Model;
                public readonly Instance Parent;
                public readonly List<Instance> Children;
            }
        }

        internal class GroupSelectionAction : HistoryService.HistoryAction
        {
            private readonly Dictionary<Instance, Instance> _parentDictionary;
            private readonly Model _model;
            private readonly Instance _parent;

            public GroupSelectionAction()
            {
                _model = new Model();
                _parent = SelectionService.Unanimous(i => i.Parent) ?? Game.Workspace;
                _parentDictionary = SelectionService.Selection.ToDictionary(i => i, i => i.Parent);
            }

            public override void Undo()
            {
                foreach (var kv in _parentDictionary)
                    kv.Key.Parent = kv.Value;
                _model.Parent = null;
            }

            public override void Execute()
            {
                foreach (var kv in _parentDictionary)
                    kv.Key.Parent = _model;
                _model.Parent = _parent;
                Game.Selection.Select(_model, true, false);
            }
        }
    }
}
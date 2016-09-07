// SceneCommands.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Linq;
using System.Windows.Input;
using dEngine;
using dEngine.Instances;
using dEngine.Services;
using Key = System.Windows.Input.Key;

namespace dEditor.Framework.Commands
{
    public class InsertCommand : Command
    {
        public override string Name => "Insert";
        public override string Text => "Inserts an object.";

        public override bool CanExecute(object parameter)
        {
            var type = parameter as Type;
            return (type != null) && typeof(Instance).IsAssignableFrom(type);
        }

        public override void Execute(object parameter)
        {
            var typeName = (string)parameter;
            var sel = SelectionService.First(x => x is Model);
            var parent = sel ?? Game.Workspace;
            ScriptService.NewInstance(typeName, parent);
        }
    }

    public class SelectChildrenCommand : Command
    {
        public override string Name => "Select Children";
        public override string Text => "Selects the children of this object.";

        public override bool CanExecute(object parameter)
        {
            return SelectionService.Any(x => x.Children.Count > 0);
        }

        public override void Execute(object parameter)
        {
            var items = SelectionService.Where(x => x.Children.Count > 0);

            SelectionService.Service.ClearSelection();

            foreach (var child in items.SelectMany(item => item.Children))
                SelectionService.Service.Select(child);
        }
    }

    public class GroupCommand : Command
    {
        public override string Name => "Group";
        public override string Text => "Groups the selection into a model.";

        public override bool CanExecute(object parameter)
        {
            return SelectionService.Any();
        }

        public override void Execute(object parameter)
        {
            ContextActionService.InvokeContextAction("groupSelection");
        }
    }

    public class UngroupCommand : Command
    {
        public override string Name => "Ungroup";
        public override string Text => "Ungroups the selected model.";

        public override bool CanExecute(object parameter)
        {
            return SelectionService.Any(i => i is Model || i is Folder);
        }

        public override void Execute(object parameter)
        {
            ContextActionService.InvokeContextAction("ungroupSelection");
        }
    }

    public class ZoomToCommand : Command
    {
        public override string Name => "Zoom to";
        public override string Text => "Moves the camera to fit the selection.";

        public override bool CanExecute(object parameter)
        {
            return (Game.FocusedCamera != null) && SelectionService.Any();
        }

        public override void Execute(object parameter)
        {
            var camera = Game.Workspace.CurrentCamera;
            if (camera != null)
            {
                if (camera.CameraType != CameraType.Fixed)
                    camera.CameraType = CameraType.Fixed;

                (camera.CurrentBehaviour as FixedBehaviour)?.FocusOn(SelectionService.OfType<PVInstance>());
            }
        }
    }
}
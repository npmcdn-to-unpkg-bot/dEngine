// CsgCommands.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Linq;
using System.Windows.Input;
using dEngine;
using dEngine.Instances;
using dEngine.Services;
using MoreLinq;
using Key = System.Windows.Input.Key;

namespace dEditor.Framework.Commands
{
    public abstract class CsgCommand : Command
    {
        public CsgCommand()
        {
            Game.Selection.SelectionChanged.Event +=
                () => Editor.Current.Dispatcher.InvokeAsync(UpdateCanExecute);
        }
    }

    public class UnionCommand : CsgCommand
    {
        public override string Name => "Union";
        public override string Text => "Merge selected parts.";

        public override bool CanExecute(object parameter)
        {
            return SelectionService.Any(x => x is Part);
        }

        public override void Execute(object parameter)
        {
            var selection = SelectionService.OfType<Part>();
            SolidModelingManager.Union(selection);
        }
    }

    public class NegateCommand : CsgCommand
    {
        public override string Name => "Negate";
        public override string Text => "Negates the part.";

        public override bool CanExecute(object parameter)
        {
            return SelectionService.Any(x => x is Part);
        }

        public override void Execute(object parameter)
        {
            var selection = SelectionService.OfType<Part>();
            selection.ForEach(x => SolidModelingManager.MakeOp<NegateOperation>(x));
        }
    }

    public class IntersectCommand : CsgCommand
    {
        public override string Name => "Intersect";
        public override string Text => "The common area between two objects.";

        public override bool CanExecute(object parameter)
        {
            return SelectionService.Any(x => x is Part);
        }

        public override void Execute(object parameter)
        {
            var selection = SelectionService.OfType<Part>();
            selection.ForEach(x => SolidModelingManager.MakeOp<IntersectOperation>(x));
        }
    }

    public class SeparateCommand : CsgCommand
    {
        public override string Name => "Seperate";
        public override string Text => "Seperate parts from a fused selection.";

        public override bool CanExecute(object parameter)
        {
            return SelectionService.Any() && SelectionService.All(x => x is PartOperation);
        }

        public override void Execute(object parameter)
        {
            var selection = SelectionService.OfType<PartOperation>();
            SolidModelingManager.Seperate(selection.First());
        }
    }
}
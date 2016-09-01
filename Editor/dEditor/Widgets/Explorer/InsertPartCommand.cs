// InsertPartCommand.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Windows.Input;
using dEditor.Framework;
using dEngine.Instances;
using dEngine.Services;

namespace dEditor.Widgets.Explorer
{
    public class InsertPartCommand : Command
    {
        public override string Name => "Insert Part";
        public override string Text => "Inserts a Part into the selected object.";
        public override KeyGesture KeyGesture { get; }

        public override bool CanExecute(object parameter)
        {
            return SelectionService.SelectionCount > 0;
        }

        public override void Execute(object parameter)
        {
            // ReSharper disable once ObjectCreationAsStatement
            new Part {Parent = SelectionService.First()};
        }
    }
}
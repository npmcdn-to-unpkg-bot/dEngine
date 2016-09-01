// HistoryCommands.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Windows.Input;
using dEngine;
using dEngine.Services;
using Key = System.Windows.Input.Key;

namespace dEditor.Framework.Commands
{
    public abstract class UndoRedoCommand : Command
    {
        protected UndoRedoCommand()
        {
            if (Game.IsInitialized)
                HookHistoryEvents();
            else
                Game.Initialized += (s, e) => { HookHistoryEvents(); };
        }

        private void HookHistoryEvents()
        {
            HistoryService.Service.Undone.Event += w => UpdateCanExecute();
            HistoryService.Service.Redone.Event += w => UpdateCanExecute();
            HistoryService.Service.WaypointSet.Event += a => UpdateCanExecute();
        }
    }

    public class UndoCommand : UndoRedoCommand
    {
        public override string Name => "Undo";
        public override string Text => "Undoes the last action.";
        public override KeyGesture KeyGesture => new KeyGesture(Key.Z, ModifierKeys.Control);

        public override bool CanExecute(object parameter)
        {
            return HistoryService.CanUndoInternal().Item1;
        }

        public override void Execute(object parameter)
        {
            HistoryService.Service.Undo();
        }
    }

    public class RedoCommand : UndoRedoCommand
    {
        public override string Name => "Redo";
        public override string Text => "Redoes the last undone action.";
        public override KeyGesture KeyGesture => new KeyGesture(Key.Z, ModifierKeys.Control | ModifierKeys.Shift);

        public override bool CanExecute(object parameter)
        {
            return HistoryService.CanRedoInternal().Item1;
        }

        public override void Execute(object parameter)
        {
            HistoryService.Service.Redo();
        }
    }
}
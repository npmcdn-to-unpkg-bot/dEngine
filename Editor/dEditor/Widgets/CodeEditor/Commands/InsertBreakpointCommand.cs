// InsertBreakpointCommand.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Windows.Input;
using dEditor.Framework;
using dEngine.Instances;

namespace dEditor.Widgets.CodeEditor.Commands
{
    public class InsertBreakpointCommand : Command
    {
        private readonly CodeEditorViewModel _codeEditor;

        public InsertBreakpointCommand(CodeEditorViewModel codeEditor)
        {
            _codeEditor = codeEditor;
        }

        public override string Name => "Insert Breakpoint";
        public override string Text => "Inserts a breakpoint at the selected line.";

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            var script = _codeEditor.LuaSourceContainer as Script;
        }
    }
}
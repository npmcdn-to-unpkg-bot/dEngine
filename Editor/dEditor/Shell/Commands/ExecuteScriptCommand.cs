// ExecuteScriptCommand.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Windows.Input;
using Caliburn.Micro;
using dEditor.Framework;
using dEditor.Shell.CommandBar;
using dEngine.Utility.Extensions;
using Microsoft.Win32;

namespace dEditor.Shell.Commands
{
    public class ExecuteScriptCommand : Command
    {
        public override string Name { get; } = "Execute Script";
        public override string Text { get; } = "Executes a script from a file.";
        public override KeyGesture KeyGesture { get; } = new KeyGesture(Key.None);

        public override bool CanExecute(object parameter)
        {
            return Project.Current != null;
        }

        public override void Execute(object parameter)
        {
            var dialog = new OpenFileDialog {Filter = "Lua Script|*.lua;"};

            if (dialog.ShowDialog() == true)
                IoC.Get<ICommandBar>().Execute(dialog.OpenFile().ReadString(), true);
        }
    }
}
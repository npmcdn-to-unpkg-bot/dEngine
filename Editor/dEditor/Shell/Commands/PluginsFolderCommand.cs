// PluginsFolderCommand.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Diagnostics;
using System.Windows.Input;
using dEditor.Framework;

namespace dEditor.Shell.Commands
{
    public class PluginsFolderCommand : Command
    {
        public override string Name => "Plugins Folder";
        public override string Text => "Opens the plugins folder.";

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            Process.Start(Editor.Current.PluginsPath);
        }
    }
}
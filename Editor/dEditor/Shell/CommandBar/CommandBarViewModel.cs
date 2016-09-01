// CommandBarViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using dEngine;
using dEngine.Instances;

namespace dEditor.Shell.CommandBar
{
    [Export(typeof(ICommandBar))]
    public class CommandBarViewModel : ICommandBar
    {
        [Export]
        public IList<string> RecentCommands { get; } = new ObservableCollection<string>();

        public void Execute(string command, bool mute = false)
        {
            if (Editor.Current.Project == null)
            {
                Engine.Logger.Error("Command cannot be executed without an open project.");
                return;
            }

            RecentCommands.Add(command);

            var script = new Script
            {
                Archivable = false,
                Name = command,
                Source = command,
                ParentLocked = true,
                Identity = ScriptIdentity.Editor
            };

            if (!mute)
                script.Print($"> {command}", LogLevel.Info);

            script.Run();
            /*
#pragma warning disable 4014
            script.RunAsync().ContinueWith(x =>
            {
                script.ParentLocked = false;
                script.Destroy();
            });
#pragma warning restore 4014
*/
        }
    }
}
// MakePluginCommand.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.IO;
using System.Windows.Input;
using Caliburn.Micro;
using dEditor.Framework;
using dEngine.Instances;
using dEngine.Serializer.V1;

namespace dEditor.Widgets.Explorer
{
    public class MakePluginCommand : Command
    {
        public override string Name => "Make Plugin";
        public override string Text => "Saves the selected container as a plugin.";
        public override KeyGesture KeyGesture { get; }

        public override bool CanExecute(object parameter)
        {
            //var item = IoC.Get<IExplorer>().LastClickedInstance;
            //return item is Folder && item.Name.ValidateFileName();
            return true;
        }

        public override void Execute(object parameter)
        {
            var item = IoC.Get<IExplorer>().LastClickedInstance;

            if (item is Folder)
                using (var file = File.Create(Path.Combine(Editor.Current.PluginsPath, $"{item.Name}.plugin")))
                {
                    Inst.Serialize(item, file);
                }
        }
    }
}
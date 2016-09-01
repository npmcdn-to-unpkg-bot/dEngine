// Plugin.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Linq;
using dEditor.Widgets.CodeEditor;
using dEngine;
using dEngine.Instances;
using dEngine.Instances.Attributes;
using dEngine.Services;

#pragma warning disable 4014

namespace dEditor.Framework.Plugins
{
    [Uncreatable]
    public class Plugin : Instance
    {
        private Plugin()
        {
            Archivable = false;
            ParentLocked = true;

            Deactivated = new Signal(this);
        }

        [ScriptSecurity(ScriptIdentity.Plugin)]
        public void Activate(bool exclusiveMouse)
        {
            ScriptService.AssertIdentity(ScriptIdentity.Plugin);
        }

        /// <summary>
        /// Creates a new toolbar.
        /// </summary>
        /// <param name="name">The name of the toolbar.</param>
        [ScriptSecurity(ScriptIdentity.Plugin)]
        public Toolbar CreateToolbar(string name)
        {
            ScriptService.AssertIdentity(ScriptIdentity.Plugin);
            return new Toolbar(name) {Parent = this, ParentLocked = true};
        }

        /// <summary>
        /// Creates a new widget.
        /// </summary>
        /// <param name="name">The name of the widget.</param>
        [ScriptSecurity(ScriptIdentity.Plugin)]
        public Widget CreateWidget(string name)
        {
            ScriptService.AssertIdentity(ScriptIdentity.Plugin);
            return new Widget(name) {Parent = this, ParentLocked = true};
        }

        [ScriptSecurity(ScriptIdentity.Plugin)]
        public void OpenScript(LuaSourceContainer script, int lineNumber = 0)
        {
            ScriptService.AssertIdentity(ScriptIdentity.Plugin);
            CodeEditorViewModel.TryOpenScript(script, lineNumber);
        }

        public override void Destroy()
        {
            Editor.Current.Plugins.Remove(this);
            ParentLocked = false;
            base.Destroy();
        }

        public static Plugin Load(Instance container)
        {
            var plugin = new Plugin {Name = container.Name};
            container.Parent = plugin;

            try
            {
                foreach (var script in container.Children.OfType<Script>())
                {
                    script.Identity = ScriptIdentity.Plugin;
                    script.Tag = plugin;
                    script.Run();
                }
            }
            catch (Exception)
            {
                plugin.Destroy();
                throw;
            }

            Editor.Current.Plugins.Add(plugin);

            plugin.Logger.Info($"Plugin \"{plugin.Name}\" loaded.");
            return plugin;
        }

        public readonly Signal Deactivated;
    }
}
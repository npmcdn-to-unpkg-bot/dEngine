// Toolbar.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Windows;
using System.Windows.Controls;
using dEngine;
using dEngine.Instances.Attributes;
using dEngine.Services;
using WpfToolBar = System.Windows.Controls.ToolBar;

namespace dEditor.Framework.Plugins
{
    public class Toolbar : PluginItemBase
    {
        public Toolbar(string name)
        {
            Name = name;

            Editor.Current.Dispatcher.InvokeAsync(() =>
            {
                WpfToolBar = new WpfToolBar
                {
                    ToolTip = name,
                    Template = Application.Current.Resources["ToolBarTemplate"] as ControlTemplate
                };

                var shellView = Editor.Current.Shell.View;
                shellView.ToolBarTray.ToolBars.Add(WpfToolBar);
                IsLoaded = true;
                Loaded?.Invoke(WpfToolBar);
            });
        }

        public WpfToolBar WpfToolBar { get; private set; }

        internal bool IsLoaded { get; set; }

        internal event Action<WpfToolBar> Loaded;

        /// <summary>
        /// Creates and adds a button to the toolbar.
        /// </summary>
        /// <param name="text">The button text.</param>
        /// <param name="tooltip">The tooltip.</param>
        /// <param name="iconName">The path to the icon file relative to the plugin's folder.</param>
        /// <returns>The button.</returns>
        [ScriptSecurity(ScriptIdentity.Plugin)]
        public Button CreateButton(string text, string tooltip, string iconName)
        {
            ScriptService.AssertIdentity(ScriptIdentity.Plugin);
            return new Button(text, tooltip, iconName, this) {Parent = this, ParentLocked = true};
        }

        public override void Destroy()
        {
            ParentLocked = false;
            base.Destroy();

            Editor.Current.Dispatcher.InvokeAsync(() =>
            {
                var shellView = Editor.Current.Shell.View;
                shellView.ToolBarTray.ToolBars.Remove(WpfToolBar);
            });
        }
    }
}
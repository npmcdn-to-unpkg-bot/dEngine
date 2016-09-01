// SettingsViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using dEditor.Widgets.Properties;
using dEngine;

namespace dEditor.Dialogs.Settings
{
    public class SettingsViewModel : Conductor<PropertiesViewModel>
    {
        private dEngine.Settings.Settings _selectedSettings;

        public SettingsViewModel()
        {
            Properties = new PropertiesViewModel {UseSelectionService = false, UseHistoryService = false};
            Properties.FilteredCategories.Add("Data");
            Properties.FilteredCategories.Add("Behaviour");

            Settings =
                Engine.Settings.Children.OfType<dEngine.Settings.Settings>()
                    .Concat(Engine.UserSettings.Children.OfType<dEngine.Settings.Settings>());
        }

        public IEnumerable<dEngine.Settings.Settings> Settings { get; }

        public dEngine.Settings.Settings SelectedSettings
        {
            get { return _selectedSettings; }
            set
            {
                _selectedSettings = value;
                Properties.Target = value;
            }
        }

        public void ResetAllSettings()
        {
            Engine.Settings.RestoreDefaults();
        }

        public void ApplyAndClose()
        {
            TryClose(true);
        }

        public override void TryClose(bool? dialogResult = null)
        {
            base.TryClose(dialogResult);

            if (dialogResult == true)
            {
                Engine.Settings.Save();
                Engine.UserSettings.Save();
            }
        }

        protected override void OnActivate()
        {
            ActivateItem(Properties);
        }

        public readonly PropertiesViewModel Properties;
    }
}
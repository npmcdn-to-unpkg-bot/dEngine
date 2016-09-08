// OutputViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows;
using dEditor.Framework;
using dEngine;
using dEngine.Instances;
using dEngine.Services;

namespace dEditor.Modules.Widgets.Output
{
    [Export(typeof(IOutput))]
    public class OutputViewModel : Widget, IOutput
    {
        private bool _wordWrap;
        private ObservableCollection<OutputItem> _entries;

        public OutputViewModel()
        {
            WordWrap = true;
            Entries = new ObservableCollection<OutputItem>();

            var logService = DataModel.GetService<LogService>();
            logService.MessageOutput.Event += OnMessageOutputEvent;
        }

        public override string DisplayName => "Output";
        public override PaneLocation PreferredLocation => PaneLocation.Bottom;

        public ObservableCollection<OutputItem> Entries
        {
            get { return _entries; }
            set
            {
                if (Equals(value, _entries)) return;
                _entries = value;
                NotifyOfPropertyChange();
            }
        }

        [Export]
        public bool WordWrap
        {
            get { return _wordWrap; }
            set
            {
                if (value == _wordWrap) return;
                _wordWrap = value;
                NotifyOfPropertyChange();
            }
        }

        private void OnMessageOutputEvent(string msg, LogLevel level, string logger)
        {
            if (!logger.StartsWith("dEngine.Instances") &&
                !logger.StartsWith("dEngine.Services") &&
                !logger.StartsWith("I:"))
                return;

            Application.Current?.Dispatcher.InvokeAsync(
                () => { Entries.Add(new OutputItem(ref msg, ref level, ref logger)); });
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

            if (close)
                LogService.GetExisting().MessageOutput.Event -= OnMessageOutputEvent;
        }

        public void Clear()
        {
            Entries = new ObservableCollection<OutputItem>();
        }

        public override void SaveState(BinaryWriter writer)
        {
            writer.Write(WordWrap);
        }

        public override void LoadState(BinaryReader reader)
        {
            WordWrap = reader.ReadBoolean();
        }

        public void ToggleWordWrap()
        {
            WordWrap = !WordWrap;
        }
    }
}
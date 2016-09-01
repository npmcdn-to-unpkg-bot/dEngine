// PlaceItem.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.IO;
using Caliburn.Micro;
using dEditor.Framework;
using dEngine;

namespace dEditor.Dialogs.ProjectProperties
{
    public class PlaceItem : PropertyChangedBase
    {
        public PlaceItem(string file)
        {
            Name = Path.GetFileNameWithoutExtension(file);
            DateModified = File.GetLastWriteTime(file).ToString("yyyy/M/d, hh:mm:ss tt");
            Project.Current.StartupPlaceChanged += OnStartupPlaceChanged;
        }

        public string Name { get; }
        public string DateModified { get; }
        public bool IsCurrent => Game.Workspace.PlaceId == Name;
        public bool IsStartup => Project.Current.StartupPlace == Name;

        ~PlaceItem()
        {
            Project.Current.StartupPlaceChanged -= OnStartupPlaceChanged;
        }

        private void OnStartupPlaceChanged(string s)
        {
            NotifyOfPropertyChange(nameof(IsStartup));
        }
    }
}
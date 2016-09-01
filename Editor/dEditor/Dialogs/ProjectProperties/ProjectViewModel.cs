// ProjectViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using dEditor.Framework;
using dEditor.Framework.Commands;
using dEditor.Utility;
using dEditor.Widgets.Properties;
using dEngine;
using Microsoft.Scripting.Utils;
using MoreLinq;

namespace dEditor.Dialogs.ProjectProperties
{
    public class ProjectViewModel : Conductor<PropertiesViewModel>
    {
        private string _currentPage;
        private PlaceItem _selectedPlace;

        public ProjectViewModel()
        {
            NewPlaceCommand = new NewPlaceCommand();
            Places = new ObservableCollection<PlaceItem>();
            ActiveItem = Properties = new PropertiesViewModel
            {
                UseSelectionService = false,
                UseHistoryService = false,
                Target = Project.Current
            };
            Pages["Description"] = Visibility.Hidden;
            Pages["Places"] = Visibility.Hidden;
            UpdatePlaces();

            Game.Workspace.PlaceLoaded.Event += OnPlaceLoaded;
        }

        public NewPlaceCommand NewPlaceCommand { get; }

        public PropertiesViewModel Properties { get; }

        public ObservableCollection<PlaceItem> Places { get; private set; }

        public PlaceItem SelectedPlace
        {
            get { return _selectedPlace; }
            set
            {
                if (Equals(value, _selectedPlace)) return;
                _selectedPlace = value;
                NotifyOfPropertyChange();
            }
        }

        public override string DisplayName
        {
            get { return Project.Current.Name; }
            set { }
        }

        public Project Project => Project.Current;

        public string CurrentPage
        {
            get { return _currentPage; }
            set
            {
                if (value == _currentPage) return;
                _currentPage = value;

                Pages.Keys.ToArray().ForEach(k => Pages[k] = Visibility.Collapsed);
                Pages[value] = Visibility.Visible;
                NotifyOfPropertyChange();
            }
        }

        public ObservableDictionary<string, Visibility> Pages { get; } = new ObservableDictionary<string, Visibility>();

        protected override void OnActivate()
        {
            if (Editor.Current.Project == null)
                TryClose();
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            if (close)
                Game.Workspace.PlaceLoaded.Event -= OnPlaceLoaded;
        }

        private void OnPlaceLoaded(string name)
        {
            UpdatePlaces();
        }

        private void UpdatePlaces()
        {
            Places = new ObservableCollection<PlaceItem>();
            Places.AddRange(
                Directory.GetFiles(Path.Combine(Project.Current.ProjectPath, "Places"))
                    .Select(p => new PlaceItem(p)));
            SelectedPlace = Places.First(p => p.IsCurrent);
            NotifyOfPropertyChange(nameof(Places));
        }

        public IDictionary<string, object> GetDialogSettings()
        {
            return new Dictionary<string, object>
            {
                {"MinWidth", 700},
                {"MinHeight", 500},
                {"Title", "Project Settings"},
                {"WindowStyle", WindowStyle.ToolWindow}
            };
        }
    }
}
// NewProjectViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Caliburn.Micro;
using dEditor.Dialogs.NewPlace;
using dEditor.Framework;
using dEditor.Widgets.StartPage;
using Ookii.Dialogs.Wpf;

namespace dEditor.Dialogs.NewProject
{
    public class NewProjectViewModel : Screen
    {
        private bool _canCreate;
        private string _location;
        private string _name;

        public NewProjectViewModel()
        {
            _location = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "dEditor",
                "Projects");
            _name = "MyGame";

            DisplayName = "New Project";

            Templates = new ObservableCollection<ProjectTemplate>
            {
                new ProjectTemplate("Baseplate", NewPlaceViewModel.LoadBaseplate)
            };

            SelectedTemplate = Templates.First();
            UpdateCanCreate();
        }

        public ObservableCollection<ProjectTemplate> Templates { get; }

        public ProjectTemplate SelectedTemplate { get; set; }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyOfPropertyChange();
                UpdateCanCreate();
            }
        }

        public string Location
        {
            get { return _location; }
            set
            {
                _location = value;
                NotifyOfPropertyChange();
                UpdateCanCreate();
            }
        }

        public bool CanCreate
        {
            get { return _canCreate; }
            set
            {
                _canCreate = value;
                NotifyOfPropertyChange();
            }
        }

        private Stream GetTemplateStream(string name)
        {
            return
                Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream($"dEditor.Content.Templates.{name}.{name}.place");
        }

        private void UpdateCanCreate()
        {
            if (!string.IsNullOrWhiteSpace(_name) && (_name.Length < 43) && (SelectedTemplate != null))
                CanCreate = true;
            else
                CanCreate = false;
        }

        public void Browse()
        {
            var dialog = new VistaFolderBrowserDialog
            {
                SelectedPath = _location,
                Description = "Select Project Path",
                UseDescriptionForTitle = true
            };

            if (dialog.ShowDialog() == true)
                Location = dialog.SelectedPath;
        }

        public void Create()
        {
            if (Directory.Exists(Path.Combine(_location, _name)))
            {
                MessageBox.Show($"A folder with the name \"{Name}\" already exists in the given location.",
                    "Directory Exists", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            Project.Create(_name, _location);

            TryClose(true);
        }

        public void Cancel()
        {
            TryClose(false);
        }

        public IDictionary<string, object> GetDialogSettings()
        {
            return new Dictionary<string, object>
            {
                {"SizeToContent", SizeToContent.Manual},
                {"MinWidth", 784},
                {"MinHeight", 411},
                {"Width", 768},
                {"Height", 493},
                {"ShowInTaskbar", false},
                {"WindowStyle", WindowStyle.ToolWindow}
            };
        }
    }
}
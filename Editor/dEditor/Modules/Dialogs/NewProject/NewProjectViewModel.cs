// NewProjectViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using dEditor.Framework;
using dEditor.Modules.Dialogs.MeshImport;
using dEditor.Modules.Widgets.StartPage;
using Ookii.Dialogs.Wpf;

namespace dEditor.Modules.Dialogs.NewProject
{
    public class NewProjectViewModel : Screen, IDialog, IDataErrorInfo
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

            UpdateCanCreate();
        }

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
            CanCreate = this[nameof(Name)] == null && this[nameof(Location)] == null;
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

        public float MinWidth => 400;
        public float MinHeight => 250;
        public float MaxWidth => 400;
        public float MaxHeight => 250;
        public float StartingWidth => 400;
        public float StartingHeight => 250;
        public ICommand CloseCommand { get; }
        public bool IsVisible { get; set; }

        private static readonly Regex _invalidFileName = new Regex("(^(PRN|AUX|NUL|CON|COM[1-9]|LPT[1-9]|(\\.+)$)(\\..*)?$)|(([‌​\\x00-\\x1f\\\\?*:\"‌​;‌​|/<>])+)|([\\.]+)", RegexOptions.IgnoreCase);


        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(Name):
                        if (Name.Length > 25)
                            return "This name is too long.";
                        if (string.IsNullOrWhiteSpace(Name))
                            return "The project requires a name.";
                        if (_invalidFileName.IsMatch(Name))
                            return "This name cannot be used.";
                        if (Directory.Exists(Path.Combine(Location, Name)))
                            return "A project with this name already exists in the selected folder.";
                        break;
                }

                return null;
            }
        }

        public string Error => null;
    }
}
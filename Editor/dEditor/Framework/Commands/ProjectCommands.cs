// ProjectCommands.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Caliburn.Micro;
using dEditor.Modules.Dialogs.NewProject;
using dEditor.Modules.Shell;
using dEditor.Modules.Shell.StatusBar;
using Microsoft.Win32;

namespace dEditor.Framework.Commands
{
    public class NewPlaceCommand : Command
    {
        public override string Name => "New Place";
        public override string Text => "Creates a new place from a template.";

        public override bool CanExecute(object parameter)
        {
            return Project.Current != null;
        }

        public override void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }

    public class NewProjectCommand : Command
    {
        public override string Name => "New Project";
        public override string Text => "Opens a dialog for creating projects.";

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            var settings = new Dictionary<string, object>
            {
                {"SizeToContent", SizeToContent.Manual},
                {"MinWidth", 784},
                {"MinHeight", 411},
                {"Width", 939},
                {"Height", 621},
                {"ShowInTaskbar", false}
            };

            Editor.Current.Dispatcher.InvokeAsync(() =>
            {
                var newProjectVm = new NewProjectViewModel();
                Editor.Current.WindowManager.ShowDialog(newProjectVm, null, newProjectVm.GetDialogSettings());
            });
        }
    }

    public class SaveProjectCommand : Command
    {
        public SaveProjectCommand(ShellViewModel shell)
        {
            Editor.Current.ProjectChanged += delegate { UpdateCanExecute(); };
            shell.ActiveDocumentChanged += delegate { UpdateCanExecute(); };
        }

        public override string Name => $"Save Project";
        public override string Text => $"Saves the current project.";

        public override bool CanExecute(object parameter)
        {
            return Editor.Current.Project != null;
        }

        public override void Execute(object parameter)
        {
            var statusBar = IoC.Get<IStatusBar>();
            statusBar.Text = "Saving Project";
            statusBar.IsFrozen = true;
            Editor.Current.Project.Save();
            statusBar.Text = "Project Saved";
            statusBar.IsFrozen = false;
            /*
            try
			{
				Editor.Current.Project.Save();
			}
			catch (Exception e)
			{
				Engine.Logger.Error(e);
				MessageBox.Show("An error occurred while trying to save the project.", "Save Project",
					MessageBoxButton.OK,
					MessageBoxImage.Error);
				Engine.Logger.Error(e);
			}
            */
        }
    }

    public class OpenProjectCommand : Command
    {
        public override string Name => $"Open Project";
        public override string Text => $"Opens a project.";

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            var proj = parameter as Project;

            if (proj != null)
            {
                proj.Open();
                return;
            }

            if (parameter != null)
                return;

            var dialog = new OpenFileDialog
            {
                Filter = "Project File|*.dproj;",
                InitialDirectory = Path.Combine(Editor.Current.EditorDocumentsPath, "Projects"),
            };

            if (dialog.ShowDialog() != true)
                return;

            Project project = null;
            project = Project.Load(dialog.FileName);
            project.Open();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
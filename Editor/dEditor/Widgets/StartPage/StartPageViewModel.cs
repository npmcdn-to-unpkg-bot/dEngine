// StartPageViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Media.Imaging;
using dEditor.Framework;
using dEditor.Framework.Commands;

namespace dEditor.Widgets.StartPage
{
    [Export(typeof(IStartPage))]
    public class StartPageViewModel : Document, IStartPage
    {
        public StartPageViewModel()
        {
            DisplayName = "Start Page";

            RecentProjects = new ObservableCollection<Project>();

            NotifyOfPropertyChange(() => NoRecentProjects);
        }

        public ObservableCollection<Project> RecentProjects { get; set; }

        public bool NoRecentProjects => RecentProjects.Count == 0;

        public override BitmapSource IconSource { get; } =
            new BitmapImage(new Uri("/dEditor;component/Content/Icons/Toolbar/house_16xLG.png", UriKind.Relative));

        public OpenProjectCommand OpenProjectCommand => Editor.Current.Shell.OpenProjectCommand;
        public NewProjectCommand NewProjectCommand => Editor.Current.Shell.NewProjectCommand;
    }
}
// ProjectView.xaml.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Windows;
using System.Windows.Controls;
using dEditor.Framework;
using dEngine;

namespace dEditor.Dialogs.ProjectProperties
{
    /// <summary>
    /// Interaction logic for ProjectViewModel.xaml
    /// </summary>
    public partial class ProjectView : UserControl
    {
        public ProjectView()
        {
            InitializeComponent();
        }

        private void MakeStartup(object sender, RoutedEventArgs e)
        {
            Project.Current.StartupPlace = ((PlaceItem)sender).Name;
            Project.Current.Save(false);
        }

        private void Load(object sender, RoutedEventArgs e)
        {
            Game.Workspace.LoadPlace(((PlaceItem)sender).Name);
        }
    }
}
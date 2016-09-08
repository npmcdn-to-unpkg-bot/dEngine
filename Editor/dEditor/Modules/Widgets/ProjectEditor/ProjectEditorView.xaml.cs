using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using dEditor.Framework;
using dEngine.Services;
using Microsoft.Win32;

namespace dEditor.Modules.Widgets.ProjectEditor
{
    /// <summary>
    /// Interaction logic for ProjectEditorView.xaml
    /// </summary>
    public partial class ProjectEditorView : UserControl
    {
        public ProjectEditorView()
        {
            InitializeComponent();
            UpdateThumbnail();
        }

        private void EditTitleButton_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ProjectTitle.IsReadOnly = false;
            Dispatcher.BeginInvoke(DispatcherPriority.Input,
                new Action(delegate
                {
                    ProjectTitle.Focus();
                    Keyboard.Focus(ProjectTitle);
                    ProjectTitle.CaretIndex = ProjectTitle.Text.Length;
                }));
        }

        private void ProjectTitle_OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            ProjectTitle.IsReadOnly = true;
        }

        private void UpdateThumbnail()
        {
            var iconFile = Path.Combine(Project.Current.ProjectPath, "icon.png");

            if (!File.Exists(iconFile))
            {
                using (var input = ContentProvider.DownloadStream(new Uri("editor://Icons/default-place.png")).Result)
                using (var output = File.Create(iconFile))
                {
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(input));
                    encoder.Save(output);
                }
            }

            using (var input = File.OpenRead(iconFile))
            {
                var decoder = new PngBitmapDecoder(input, BitmapCreateOptions.None, BitmapCacheOption.None);
                ProjectImage.Source = decoder.Frames[0];
            }
        }

        private void ImageUploader_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs args)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png"
            };

            if (dialog.ShowDialog() != true) return;

            try
            {
                using (var output = File.Create(Path.Combine(Project.Current.ProjectPath, "icon.png")))
                using (var input = File.OpenRead(dialog.FileName))
                {
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(input));
                    encoder.Save(output);
                }
                UpdateThumbnail();
            }
            catch (Exception e)
            {
                Editor.Logger.Error(e);
                MessageBox.Show($"Could not import icon: {e.Message}", "dEditor", MessageBoxButton.OK);
            }
        }
    }
}
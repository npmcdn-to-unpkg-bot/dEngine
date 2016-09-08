// FBXImportViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System.ComponentModel.Composition;
using System.Threading;
using System.Windows.Input;
using Caliburn.Micro;
using dEditor.Framework.Content;
using dEditor.Framework.Services;
using dEditor.Widgets.ContentBrowser.Util;
using dEditor.Widgets.Properties;

namespace dEditor.Dialogs.ModelImport
{
    [Export(typeof(IMeshImport))]
    public class ModelImportViewModel : Conductor<PropertiesViewModel>, IMeshImport
    {
        private string _filePath;
        public PropertiesViewModel Properties;

        public ModelImportViewModel(ContentManager.ImportContext context)
        {
            FilePath = context.OutputDirectory;
            ImportResetEvent = new ManualResetEventSlim();
            ImportSettings = new MeshImportSettings();

            Properties = new PropertiesViewModel {UseSelectionService = false, UseHistoryService = false};

            ImportSettings.ImportAsSkeletalChanged += UpdateInspector;
            UpdateInspector(ImportSettings.ImportAsSkeletal);
        }

        public string FilePath
        {
            get { return _filePath; }
            private set
            {
                _filePath = value;
                NotifyOfPropertyChange();
            }
        }

        public MeshImportSettings ImportSettings { get; }

        public ManualResetEventSlim ImportResetEvent { get; }

        public bool ImportAll { get; set; }

        private void UpdateInspector(bool value)
        {
            if (value)
            {
                Properties.FilteredCategories.Remove("Skeletal");
                Properties.FilteredCategories.Add("Static");
            }
            else
            {
                Properties.FilteredCategories.Remove("Static");
                Properties.FilteredCategories.Add("Skeletal");
            }

            Properties.Target = null;
            Properties.Target = ImportSettings;
        }

        public void FinishImport()
        {
            ImportResetEvent.Set();
            TryClose(true);
        }

        public void FinishImportAll()
        {
            ImportAll = true;
            ImportResetEvent.Set();
            TryClose(true);
        }

        protected override void OnActivate()
        {
            ActivateItem(Properties);
        }

        public float MinWidth => 300;
        public float MinHeight => 300;
        public float MaxWidth => float.PositiveInfinity;
        public float MaxHeight => float.PositiveInfinity;
        public float Width => 400;
        public float Height => 600;
        public bool IsVisible { get; set; }
        public ICommand CloseCommand { get; }
    }

    /// <summary>
    /// Enum for normal import methods.
    /// </summary>
    public enum NormalImportMethod
    {
        /// <summary>
        /// The importer will compute normals and tangents.
        /// </summary>
        ComputeNormals,

        /// <summary>
        /// The importer will compute smooth normals and tangents.
        /// </summary>
        ComputeNormalsSmooth,

        /// <summary>
        /// The importer will import normals but compute tangents.
        /// </summary>
        ImportNormals,

        /// <summary>
        /// The importer will import normals and tangents.
        /// </summary>
        ImportNormalsAndTangents
    }

    public interface IDialog
    {
        string DisplayName { get; }
        float MinWidth { get; }
        float MinHeight { get; }
        float MaxWidth { get; }
        float MaxHeight { get; }
        float Width { get; }
        float Height { get; }
        ICommand CloseCommand { get; }
        bool IsVisible { get; set; }
    }
}
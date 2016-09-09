// CurveDialogViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System.Windows.Input;
using dEditor.Framework;
using dEditor.Modules.Dialogs.MeshImport;

namespace dEditor.Modules.Widgets.Properties.Inspectors
{
    public class CurveDialogViewModel : IDialog
    {
        public CurveDialogViewModel(dEngine.Instances.Instance instance, string propertyName)
        {
            DisplayName = $"{instance.GetFullName()}.{propertyName}";
            StartingWidth = MinWidth = 800;
            StartingHeight = MinHeight = 250;
        }

        public string DisplayName { get; }
        public float MinWidth { get; } = 800;
        public float MinHeight { get; } = 250;
        public float MaxWidth { get; } = 800;
        public float MaxHeight { get; } = 250;
        public float StartingWidth { get; } = 800;
        public float StartingHeight { get; } = 250;
        public ICommand CloseCommand { get; }
        public bool IsVisible { get; set; }
    }
}
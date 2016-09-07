// CurveDialogViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System.Windows.Input;
using dEditor.Dialogs.ModelImport;
using dEditor.Framework;
using dEditor.Widgets.ContentBrowser.Util;

namespace dEditor.Widgets.Properties.Inspectors
{
    public class CurveDialogViewModel : IDialog
    {
        public CurveDialogViewModel(dEngine.Instances.Instance instance, string propertyName)
        {
            DisplayName = $"{instance.GetFullName()}.{propertyName}";
        }

        public string DisplayName { get; }
        public float MinWidth => 800;
        public float MinHeight => 250;
        public float MaxWidth => float.MaxValue;
        public float MaxHeight => float.MaxValue;
        public float Width => 800;
        public float Height => 250;
        public ICommand CloseCommand { get; }
        public bool IsVisible { get; set; }
    }
}
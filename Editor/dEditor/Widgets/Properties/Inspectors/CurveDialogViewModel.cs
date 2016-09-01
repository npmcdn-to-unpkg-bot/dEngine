// CurveDialogViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEditor.Framework;

namespace dEditor.Widgets.Properties.Inspectors
{
    public class CurveDialogViewModel : Dialog
    {
        public CurveDialogViewModel(dEngine.Instances.Instance instance, string propertyName)
        {
            DisplayName = $"{instance.GetFullName()}.{propertyName}";
            Width = MinWidth = 800;
            Height = MinHeight = 250;
        }
    }
}
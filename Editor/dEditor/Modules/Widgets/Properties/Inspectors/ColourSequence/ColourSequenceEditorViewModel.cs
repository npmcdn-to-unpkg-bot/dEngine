// ColourSequenceEditorViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System.Windows.Media;
using dEngine.Serializer.V1;

namespace dEditor.Modules.Widgets.Properties.Inspectors.ColourSequence
{
    public class ColourSequenceEditorViewModel : SequenceEditorBase<dEngine.Colour>, ILabelled
    {
        private readonly dEngine.Instances.Instance _instance;
        private readonly Inst.CachedProperty _propDesc;

        public ColourSequenceEditorViewModel(object obj, Inst.CachedProperty propDesc) : base(obj, propDesc)
        {
            GradientStops = new GradientStopCollection();
            _instance = (dEngine.Instances.Instance)obj;
            _propDesc = propDesc;
        }

        public GradientStopCollection GradientStops { get; set; }

        public void OnClick()
        {
            OpenCurveDialog();
        }

        private void OpenCurveDialog()
        {
            var curveEditor = new CurveDialogViewModel(_instance, _propDesc.Name);
            Editor.ShowDialog(curveEditor);
        }
    }
}
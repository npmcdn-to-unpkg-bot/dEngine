// StudioBuildTool.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
namespace dEditor.Tools.Building
{
    public abstract class StudioBuildTool : StudioTool
    {
        private double? _increment;

        public abstract double[] IncrementOptions { get; }

        public double Increment
        {
            get { return _increment ?? IncrementOptions[0]; }
            set
            {
                if (value.Equals(_increment)) return;
                _increment = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
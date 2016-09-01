// RotateTool.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
namespace dEditor.Tools.Building
{
    public class RotateTool : StudioBuildTool
    {
        public override double[] IncrementOptions { get; } = {1, 5, 10, 15, 30, 45, 60, 90, 120};

        protected override void OnEquipped()
        {
            //throw new System.NotImplementedException();
        }

        protected override void OnUnequipped()
        {
            //throw new System.NotImplementedException();
        }
    }
}
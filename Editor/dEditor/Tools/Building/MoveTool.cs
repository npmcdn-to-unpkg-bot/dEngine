// MoveTool.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine;
using dEngine.Instances;
using dEngine.Utility.Extensions;
using SharpDX;
using Vector3 = SharpDX.Vector3;

namespace dEditor.Tools.Building
{
    public class MoveTool : StudioBuildTool
    {
        private readonly Part _marker; // a part which represents the bounding box of the entire selection
        private readonly AxisHandles _axisHandles;

        public MoveTool()
        {
            _marker = new Part {Name = "MoveToolMarker", Parent = Game.CoreEnvironment};

            _axisHandles = new AxisHandles
            {
                Name = "MoveTool_AxisHandles",
                Parent = Game.CoreGui,
                Style = HandlesStyle.Movement,
                Adornee = _marker,
                Visible = false
            };
            _axisHandles.MouseButton1Down.Connect(OnMouseButton1Down);
            _axisHandles.MouseButton1Up.Connect(OnMouseButton1Up);
            _axisHandles.MouseDrag.Connect(OnMouseDrag);

            IncrementOptions = new double[] {1, 2, 3, 4};
        }

        public override double[] IncrementOptions { get; }

        private void OnMouseDrag(NormalId arg1, float arg2)
        {
        }

        private void OnMouseButton1Up(NormalId obj)
        {
        }

        private void OnMouseButton1Down(NormalId obj)
        {
        }

        protected override void OnEquipped()
        {
        }

        protected override void OnUnequipped()
        {
        }

        protected override void UpdateHandles()
        {
            base.UpdateHandles();
            var boundingBox = new OrientedBoundingBox(Vector3.Zero, Vector3.Zero);
                //PVInstance.ComputeBoundingBoxFromCollection(SelectionService.Selection);
            _marker.Size = boundingBox.GetdEngineSize();
            _marker.CFrame = boundingBox.GetCFrame();
        }
    }
}
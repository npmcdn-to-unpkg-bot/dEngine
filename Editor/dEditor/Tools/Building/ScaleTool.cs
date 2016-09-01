// ScaleTool.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Collections.Concurrent;
using dEngine;
using dEngine.Instances;

namespace dEditor.Tools.Building
{
    public class ScaleTool : StudioBuildTool
    {
        private ConcurrentDictionary<Part, AxisHandles> _axisHandles;

        public override double[] IncrementOptions { get; } = {1, 10, 0.5, 0.25, 0.125, 0.0625, 0.03125};

        protected override void OnEquipped()
        {
        }

        protected override void OnUnequipped()
        {
        }

        protected override void OnItemSelected(Instance instance)
        {
            base.OnItemSelected(instance);

            var part = instance as Part;
            if (part != null)
            {
                var handle = new AxisHandles
                {
                    Name = "ResizeHandle",
                    Adornee = part,
                    Parent = Game.CoreGui,
                    Style = HandlesStyle.Resize
                };
                _axisHandles[part] = handle;
            }

            UpdateHandles();
        }

        private void UpdateHandles()
        {
            //throw new System.NotImplementedException();
        }

        protected override void OnItemDeselected(Instance instance)
        {
            base.OnItemDeselected(instance);

            var part = instance as Part;
            if (part != null)
            {
                AxisHandles handle;
                _axisHandles.TryRemove(part, out handle);
                handle.Destroy();
            }
        }
    }
}
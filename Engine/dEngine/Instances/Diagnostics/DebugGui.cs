// DebugGui.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;
using dEngine.Services;

#pragma warning disable 1591

namespace dEngine.Instances.Diagnostics
{
    [Uncreatable]
    [TypeId(196)]
    public abstract class DebugGui : ScreenGui
    {
        protected DebugGui(CoreGui coreGui)
        {
            Parent = coreGui;
            RunService.Service.Heartbeat.Event += OnHeartbeat;
        }

        private void OnHeartbeat(double d)
        {
            OnUpdate();
        }

        protected TextLabel MakeDebugTextLabel(Instance parent)
        {
            return new TextLabel
            {
                FontSize = 14,
                Size = new UDim2(1, 0, 0, 16),
                Parent = parent,
                BackgroundColour = Colour.Transparent,
                TextColour = Colour.White,
                TextStrokeColour = Colour.Black,
                TextAlignmentX = AlignmentX.Left
            };
        }

        protected abstract void OnUpdate();

        public override void Destroy()
        {
            base.Destroy();
            RunService.Service.Heartbeat.Event -= OnHeartbeat;
        }
    }
}
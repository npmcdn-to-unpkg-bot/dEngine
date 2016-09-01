// GeneralDebugGui.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;

#pragma warning disable 1591

namespace dEngine.Instances.Diagnostics
{
    [TypeId(197)]
    public sealed class GeneralDebugGui : DebugGui
    {
        private readonly TextLabel _gameTaskLabel;
        private readonly TextLabel _graphicsTaskLabel;
        private readonly Stack _stack;

        internal GeneralDebugGui(CoreGui coreGui) : base(coreGui)
        {
            _stack = new Stack
            {
                Parent = this,
                Size = new UDim2(0.3f, 0, 0.3f, 0),
                BackgroundColour = Colour.Transparent
            };

            _graphicsTaskLabel = MakeDebugTextLabel(_stack);
            _gameTaskLabel = MakeDebugTextLabel(_stack);
        }

        protected override void OnUpdate()
        {
        }

        public override void Destroy()
        {
            base.Destroy();
            _stack.Destroy();
        }
    }
}
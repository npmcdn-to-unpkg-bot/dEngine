// Button.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// An element made to visually look like a button.
    /// </summary>
    [TypeId(139)]
    [ExplorerOrder(68)]
    public class Button : GuiElement
    {
        private Colour _actualBgColour;
        private Colour _clickColour;
        private Colour _hoverColour;

        /// <inheritdoc />
        public Button()
        {
            MouseButton1Down.Event += OnMouseButton1Down;
            MouseButton1Up.Event += OnMouseButton1Up;
            MouseEnter.Event += OnMouseEnter;
            MouseLeave.Event += OnMouseLeave;
        }

        /// <inheritdoc />
        public override Colour BackgroundColour
        {
            get { return _actualBgColour; }
            set
            {
                _actualBgColour = value;
                base.BackgroundColour = value;
                _hoverColour = value.Darken(0.1f);
                _clickColour = value.Lighten(0.1f);
            }
        }

        private void OnMouseButton1Up(int x, int y)
        {
            base.BackgroundColour = IsMouseOver ? _clickColour : _actualBgColour;
        }

        private void OnMouseButton1Down(int x, int y)
        {
            base.BackgroundColour = _clickColour;
        }

        /// <inheritdoc />
        protected override void OnMouseEnter(int x, int y)
        {
            base.OnMouseEnter(x, y);
            base.BackgroundColour = _hoverColour;
        }

        /// <inheritdoc />
        protected override void OnMouseLeave(int x, int y)
        {
            base.OnMouseLeave(x, y);
            base.BackgroundColour = _actualBgColour;
        }
    }
}
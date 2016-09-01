// Message.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// Fills the entire screen with a semi-transparent grey background, and centered text in the middle of the screen.
    /// </summary>
    [TypeId(61)]
    [ExplorerOrder(1)]
    [Obsolete]
    public class Message : Instance
    {
        private static ScreenGui _messageScreen;
        /// <summary/>
        protected readonly TextLabel _label;

        /// <summary />
        public Message()
        {
            if (_messageScreen == null)
                _messageScreen = new ScreenGui {Name = "MessageContainer", Parent = Game.CoreGui};

            _label = new TextLabel
            {
                Parent = null,
                Name = "Message",
                Size = new UDim2(1, 0, 1, 0),
                BorderColour = Colour.Transparent,
                BackgroundColour = new Colour(0.3f, 0.3f, 0.3f, 0.7f),
                TextColour = Colour.White,
                TextStrokeColour = Colour.Black,
                Text = "",
                Visible = false,
                Tag = this
            };
        }

        /// <summary>
        /// The text to display.
        /// </summary>
        [InstMember(1)]
        [EditorVisible]
        public string Text
        {
            get { return _label.Text; }
            set
            {
                value = value ?? string.Empty;
                if (value == _label.Text)
                    return;
                _label.Text = value;
                _label.Visible = !string.IsNullOrEmpty(value);
                NotifyChanged();
            }
        }

        /// <summary />
        protected override void OnWorldChanged(IWorld newWorld, IWorld oldWorld)
        {
            base.OnWorldChanged(newWorld, oldWorld);
            if (newWorld == null)
                _label.Parent = null;
            else
                _label.Parent = _messageScreen;
        }

        /// <summary/>
        public override void Destroy()
        {
            base.Destroy();
            _label.Destroy();
        }
    }
}
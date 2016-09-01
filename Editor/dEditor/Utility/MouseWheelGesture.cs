// MouseWheelGesture.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Windows.Input;

namespace dEditor.Utility
{
    public class MouseWheelGesture : MouseGesture
    {
        public MouseWheelGesture() : base(MouseAction.WheelClick)
        {
        }

        public MouseWheelGesture(ModifierKeys modifiers) : base(MouseAction.WheelClick, modifiers)
        {
        }

        public WheelDirection Direction { get; set; }

        public static MouseWheelGesture Up
        {
            get { return new MouseWheelGesture {Direction = WheelDirection.Up}; }
        }

        public static MouseWheelGesture Down
        {
            get { return new MouseWheelGesture {Direction = WheelDirection.Down}; }
        }

        public static MouseWheelGesture CtrlUp
        {
            get { return new MouseWheelGesture(ModifierKeys.Control) {Direction = WheelDirection.Up}; }
        }

        public static MouseWheelGesture CtrlDown
        {
            get { return new MouseWheelGesture(ModifierKeys.Control) {Direction = WheelDirection.Down}; }
        }


        public override bool Matches(object targetElement, InputEventArgs inputEventArgs)
        {
            if (!base.Matches(targetElement, inputEventArgs)) return false;
            if (!(inputEventArgs is MouseWheelEventArgs)) return false;
            var args = (MouseWheelEventArgs)inputEventArgs;
            switch (Direction)
            {
                case WheelDirection.None:
                    return args.Delta == 0;
                case WheelDirection.Up:
                    return args.Delta > 0;
                case WheelDirection.Down:
                    return args.Delta < 0;
                default:
                    return false;
            }
        }


        public enum WheelDirection
        {
            None,
            Up,
            Down
        }
    }
}
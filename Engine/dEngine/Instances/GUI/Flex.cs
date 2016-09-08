// Flex.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using dEngine.Instances.Attributes;
#pragma warning disable 414
#pragma warning disable 1591

namespace dEngine.Instances
{
    /// <summary />
    [TypeId(152)]
    [ToolboxGroup("GUI")]
    [Obsolete("This class is not implemented.")]
    public class Flex : Frame
    {
        private float _flexGrow;
        private float _flexShrink;
        private float? _flexBasis;
        private Align _alignItems;
        private Align _alignContents;
        private Direction _direction;
        private FlexDirection _flexDirection;
        private Overflow _overflow;

        internal float FlexValue
        {
            get
            {
                if (_flexGrow > 0)

                    return _flexGrow;
                return _flexShrink > 0 ? _flexShrink : 0;
            }
            set
            {
                if (value == 0)
                {
                    _flexGrow = 0;
                    _flexShrink = 0;
                    _flexBasis = null;
                }
                else if (value > 0)
                {
                    _flexGrow = value;
                    _flexShrink = 0;
                    _flexBasis = 0;
                }
                else
                {
                    _flexGrow = 0;
                    _flexShrink = -value;
                    _flexBasis = null;
                }
            }
        }

        /// <summary />
        public Flex()
        {
            _flexGrow = 0;
            _flexShrink = 0;
            _flexBasis = null;
            _alignItems = Align.Stretch;
            _alignContents = Align.FlexStart;
            _direction = Direction.Inherit;
            _overflow = Overflow.Visible;
        }

        private void CalculateLayout()
        {

        }

        public enum Overflow
        {
            Visible,
            Hidden,
        }

        public enum Justify
        {
            FlexStart,
            Center,
            FlexEnd,
            SpaceBetween,
            SpaceAround,
        }

        public enum FlexDirection
        {
            Column,
            ColumnReverse,
            DirectionRow,
            DirectionRowReverse,
        }

        public enum Direction
        {
            Inherit,
            LeftToRight,
            RightToLeft,
        }

        public enum Align
        {
            Auto,
            FlexStart,
            Center,
            FlexEnd,
            Stretch,
        }

        public enum PositionType
        {
            Relative,
            Absolute,
        }

        public enum WrapType
        {
            NoWrap,
            Wrap,
        }

        public enum MeasureMode
        {
            Undefined,
            Exactly,
            AtMost,
            Count,
        }

        public enum Dimension
        {
            Width,
            Height,
        }

        public enum Edge
        {
            Left,
            Top,
            Right,
            Bottom,
            Start,
            End,
            Horizontal,
            Vertical,
            All,
            Count,
        }

        [Flags]
        public enum PrintOptions
        {
            Layout = 1,
            Style = 2,
            Children = 4,
        }
    }
}
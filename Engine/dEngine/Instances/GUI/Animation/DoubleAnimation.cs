// DoubleAnimation.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using dEngine.Instances.Attributes;
using dEngine.Utility;

namespace dEngine.Instances
{
    /// <summary>
    /// Tweens a double property.
    /// </summary>
    [TypeId(229)]
    public class DoubleAnimation : GuiAnimation
    {
        private double _from;
        private double _to;

        /// <summary>
        /// The starting number.
        /// </summary>
        [InstMember(1)]
        [EditorVisible]
        public double From
        {
            get { return _from; }
            set
            {
                if (value == _from)
                    return;
                _from = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// The ending number.
        /// </summary>
        [InstMember(2)]
        [EditorVisible]
        public double To
        {
            get { return _to; }
            set
            {
                if (value == _to)
                    return;
                _to = value;
                NotifyChanged();
            }
        }

        internal override Type TargetType { get; } = typeof(UDim2);

        internal override bool Update()
        {
            lock (Locker)
            {
                var delta = (float)GetDelta();

                TargetProperty.Set(TargetElement, Mathf.Lerp(_from, _to, delta));

                return delta < 1;
            }
        }
    }
}
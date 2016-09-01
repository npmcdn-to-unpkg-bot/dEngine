// UDim2Animation.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using dEngine.Instances.Attributes;
using dEngine.Utility;

namespace dEngine.Instances
{
    /// <summary>
    /// Tweens a UDim2 property.
    /// </summary>
    [TypeId(227)]
    public class UDim2Animation : GuiAnimation
    {
        private UDim2 _from;
        private UDim2 _to;

        /// <summary>
        /// The starting UDim2.
        /// </summary>
        [InstMember(1)]
        [EditorVisible]
        public UDim2 From
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
        /// The ending UDim2.
        /// </summary>
        [InstMember(2)]
        [EditorVisible]
        public UDim2 To
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

                var scaleX = Mathf.Lerp(_from.Scale.x, _to.Scale.x, delta);
                var scaleY = Mathf.Lerp(_from.Scale.y, _to.Scale.y, delta);
                var absX = Mathf.Lerp(_from.Absolute.x, _to.Absolute.x, delta);
                var absY = Mathf.Lerp(_from.Absolute.y, _to.Absolute.y, delta);

                TargetProperty.Set(TargetElement, new UDim2(scaleX, absX, scaleY, absY));

                return delta < 1;
            }
        }
    }
}
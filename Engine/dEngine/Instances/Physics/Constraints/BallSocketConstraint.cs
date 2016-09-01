// BallSocketConstraint.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using BulletSharp;
using dEngine.Instances.Attributes;
using dEngine.Utility;

namespace dEngine.Instances
{
    /// <summary>
    /// The Ball socket joint limits the translation so that the local pivot points of two parts match in world-space.
    /// </summary>
    [TypeId(106)]
    public sealed class BallSocketConstraint : Constraint
    {
        private Point2PointConstraint _point2point;
        private bool _limitsEnabled;
        private float _restitution;
        private float _upperAngle;

        /// <summary>
        /// Determines whether a limit is set on the rotation based on <see cref="UpperAngle" />.
        /// </summary>
        [InstMember(1)]
        [EditorVisible("Behaviour")]
        public bool LimitsEnabled
        {
            get { return _limitsEnabled; }
            set
            {
                if (value == _limitsEnabled) return;
                _limitsEnabled = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// How elastic the <see cref="Attachment" />s will be when they reach the end of the range specified by
        /// <see cref="UpperAngle" />.
        /// </summary>
        [InstMember(2)]
        [EditorVisible("Limits")]
        public float Restitution
        {
            get { return _restitution; }
            set
            {
                if (value == _restitution) return;
                _restitution = Mathf.Saturate(value);
                NotifyChanged();
            }
        }

        [InstMember(3)]
        [EditorVisible("Limits")]
        public float UpperAngle
        {
            get { return _upperAngle; }
            set
            {
                if (value == _upperAngle) return;
                _upperAngle = value;
                NotifyChanged();
            }
        }

        protected override void RebuildConstraint()
        {
            DestroyConstraint();
            if (Validate())
            {
            }
        }
    }
}
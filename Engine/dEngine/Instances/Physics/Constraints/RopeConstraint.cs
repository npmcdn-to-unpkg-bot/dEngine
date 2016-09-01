// RopeConstraint.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using BulletSharp;
using BulletSharp.Math;
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// Summary
    /// </summary>
    [TypeId(110)]
    public sealed class RopeConstraint : Constraint
    {
        private float _length;
        private float _restitution;
        private Generic6DofSpring2Constraint _spring;

        /// <summary>
        /// The current distance between the two attachments.
        /// </summary>
        [EditorVisible("Derived")]
        public float CurrentDistance => 0;

        /// <summary>
        /// The length of the rope.
        /// </summary>
        [InstMember(1)]
        [EditorVisible("Rope")]
        public float Length
        {
            get { return _length; }
            set
            {
                if (value.Equals(_length)) return;
                _length = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// Summary
        /// </summary>
        [InstMember(2)]
        [EditorVisible("Rope")]
        public float Restitution
        {
            get { return _restitution; }
            set
            {
                if (value.Equals(_restitution)) return;
                _restitution = value;
                NotifyChanged();
            }
        }

        protected override void RebuildConstraint()
        {
            DestroyConstraint();
            if (Validate())
            {
                _constraint =
                    _spring =
                        new Generic6DofSpring2Constraint(_attachment0, _attachment1, (Matrix)_attachment0.CFrame,
                            (Matrix)_attachment1.CFrame) {Userobject = this};
                _spring.SetStiffness(0, _restitution, true);
                _spring.SetStiffness(1, _restitution, true);
                World.Physics.AddConstraint(this);
            }
        }
    }
}
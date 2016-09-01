// BodyPosition.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// Applies a force on its parent to move towards a position.
    /// </summary>
    [TypeId(118)]
    public sealed class BodyPosition : BodyMover
    {
        private float _pressure;
        private float _dampening;
        private Vector3 _maxForce;
        private Vector3 _position;

        /// <summary />
        [Obsolete]
        public Vector3 position
        {
            get { return Position; }
            set { Position = value; }
        }

        /// <summary>
        /// The target position of the object.
        /// </summary>
        [InstMember(1)]
        [EditorVisible]
        public Vector3 Position
        {
            get { return _position; }
            set
            {
                if (value.Equals(_position)) return;
                _position = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// Summary
        /// </summary>
        [InstMember(2)]
        [EditorVisible]
        public Vector3 MaxForce
        {
            get { return _maxForce; }
            set
            {
                if (value.Equals(_maxForce)) return;
                _maxForce = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// The amount of dampening to apply.
        /// </summary>
        [InstMember(3)]
        [EditorVisible]
        public float Damping
        {
            get { return _dampening; }
            set
            {
                if (value.Equals(_dampening)) return;
                _dampening = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// Determines how aggressively the object tries to reach its goal.
        /// </summary>
        [InstMember(4)]
        [EditorVisible]
        public float Power
        {
            get { return _pressure; }
            set
            {
                if (value.Equals(_pressure)) return;
                _pressure = value;
                NotifyChanged();
            }
        }

        /// <summary/>
        protected override void OnStep(Part part)
        {
        }
    }
}
// BodyAngularVelocity.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// Applies angular force on an object.
    /// </summary>
    [TypeId(117)]
    public sealed class BodyAngularVelocity : BodyMover
    {
        private Vector3 _angularVelocity;
        private Vector3 _maxTorque;

        private float _power;

        /// <summary>
        /// Summary
        /// </summary>
        [InstMember(1)]
        [EditorVisible]
        public Vector3 AngularVelocity
        {
            get { return _angularVelocity; }
            set
            {
                if (value.Equals(_angularVelocity)) return;
                _angularVelocity = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// Summary
        /// </summary>
        [InstMember(2)]
        [EditorVisible]
        public Vector3 MaxTorque
        {
            get { return _maxTorque; }
            set
            {
                if (value.Equals(_maxTorque)) return;
                _maxTorque = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// How aggressively the object tries to reach its goal.
        /// </summary>
        [InstMember(3)]
        [EditorVisible]
        public float Power
        {
            get { return _power; }
            set
            {
                if (value == _power) return;
                _power = value;
                NotifyChanged();
            }
        }


        /// <inheritdoc />
        protected override void OnStep(Part part)
        {
            var radius = part.Size.magnitude/2;
        }
    }
}
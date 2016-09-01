// RocketPropulsion.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// Applies force to its parent in a similar manner to a rocket.
    /// </summary>
    [TypeId(132)]
    public sealed class RocketPropulsion : BodyMover
    {
        private float _cartoonFactor;
        private float _maxSpeed;
        private float _maxThrust;
        private Vector3 _maxTorque;
        private Part _target;
        private Vector3 _targetOffset;
        private float _targetRadius;
        private float _thrustD;
        private float _thrustP;
        private float _turnD;
        private float _turnP;

        /// <summary>
        /// Summary
        /// </summary>
        [InstMember(1)]
        [EditorVisible]
        public float CartoonFactor
        {
            get { return _cartoonFactor; }
            set
            {
                if (value.Equals(_cartoonFactor)) return;
                _cartoonFactor = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// Summary
        /// </summary>
        [InstMember(2)]
        [EditorVisible]
        public float MaxSpeed
        {
            get { return _maxSpeed; }
            set
            {
                if (value.Equals(_maxSpeed)) return;
                _maxSpeed = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// Summary
        /// </summary>
        [InstMember(3)]
        [EditorVisible]
        public float MaxThrust
        {
            get { return _maxThrust; }
            set
            {
                if (value.Equals(_maxThrust)) return;
                _maxThrust = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// The maximum amount of torque that can be applied.
        /// </summary>
        [InstMember(4)]
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
        /// Summary
        /// </summary>
        [InstMember(5)]
        [EditorVisible]
        public Part Target
        {
            get { return _target; }
            set
            {
                if (value.Equals(_target)) return;
                _target = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// Summary
        /// </summary>
        [InstMember(6)]
        [EditorVisible]
        public Vector3 TargetOffset
        {
            get { return _targetOffset; }
            set
            {
                if (value.Equals(_targetOffset)) return;
                _targetOffset = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// Summary
        /// </summary>
        [InstMember(7)]
        [EditorVisible]
        public float TargetRadius
        {
            get { return _targetRadius; }
            set
            {
                if (value.Equals(_targetRadius)) return;
                _targetRadius = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// Summary
        /// </summary>
        [InstMember(8)]
        [EditorVisible]
        public float ThrustD
        {
            get { return _thrustD; }
            set
            {
                if (value.Equals(_thrustD)) return;
                _thrustD = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// Summary
        /// </summary>
        [InstMember(10)]
        [EditorVisible]
        public float ThrustP
        {
            get { return _thrustP; }
            set
            {
                if (value.Equals(_thrustP)) return;
                _thrustP = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// Summary
        /// </summary>
        [InstMember(11)]
        [EditorVisible]
        public float TurnD
        {
            get { return _turnD; }
            set
            {
                if (value.Equals(_turnD)) return;
                _turnD = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// Summary
        /// </summary>
        [InstMember(12)]
        [EditorVisible]
        public float TurnP
        {
            get { return _turnP; }
            set
            {
                if (value.Equals(_turnP)) return;
                _turnP = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// Stops the rocket moving from its target, causing it to fall.
        /// </summary>
        public void Abort()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Makes the rocket fly towards its target.
        /// </summary>
        public void Fire()
        {
            throw new NotImplementedException();
        }

        /// <summary/>
        protected override void OnStep(Part part)
        {
        }
    }
}
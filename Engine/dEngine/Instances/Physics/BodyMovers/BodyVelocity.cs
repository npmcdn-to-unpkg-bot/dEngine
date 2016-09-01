// BodyVelocity.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;
using BulletVector3 = BulletSharp.Math.Vector3;


namespace dEngine.Instances
{
    /// <summary>
    /// Applies a force to maintain a certain velocity.
    /// </summary>
    [TypeId(131)]
    public sealed class BodyVelocity : BodyMover
    {
        private readonly float _gainIntegral = 1;
        private readonly float _d = 1;
        private float _p;
        private Vector3 _integralTerm;
        private Vector3 _maxForce;
        private Vector3 _velocity;
        private BulletVector3 _lastProcessVar;

        /// <summary />
        public BodyVelocity()
        {
            _maxForce = new Vector3(4000);
            _velocity = new Vector3(0, 2, 0);
        }

        /// <summary>
        /// The amount of force applied on each axis.
        /// </summary>
        [InstMember(1)]
        [EditorVisible]
        public Vector3 MaxForce
        {
            get { return _maxForce; }
            set
            {
                _maxForce = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// Determines how aggressively the object tries to reach its goal.
        /// </summary>
        [InstMember(2)]
        [EditorVisible]
        public float P
        {
            get { return _p; }
            set
            {
                if (value.Equals(_p)) return;
                _p = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// The desired velocity.
        /// </summary>
        [InstMember(3)]
        [EditorVisible]
        public Vector3 Velocity
        {
            get { return _velocity; }
            set
            {
                _velocity = value;
                NotifyChanged();
            }
        }

        /// <summary />
        protected override void OnStep(Part part)
        {
            var setPoint = _velocity;
            var processVar = (Vector3)part.RigidBody.LinearVelocity;
            var error = setPoint - processVar;

            var p0 = _velocity;

            var output = _p*error + p0;

            if (output > _maxForce)
                output = _maxForce;
            else if (output < -_maxForce)
                output = -_maxForce;

            part.RigidBody.LinearVelocity = (BulletVector3)output;
        }
    }
}
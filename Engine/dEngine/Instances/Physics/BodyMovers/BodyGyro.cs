// BodyGyro.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// BodyGyro attempts to keep a fixed orientation of its parent relative to <see cref="CFrame" />.
    /// </summary>
    [TypeId(116)]
    public sealed class BodyGyro : BodyMover
    {
        private CFrame _cframe;
        private Vector3 _maxTorque;
        private float _dampening;
        private float _pressure;

        /// <summary>
        /// The target orientation of the object.
        /// </summary>
        /// <remarks>
        /// Only the rotation of the CFrame is used.
        /// </remarks>
        [InstMember(1)]
        [EditorVisible]
        public CFrame CFrame
        {
            get { return _cframe; }
            set
            {
                if (value.Equals(_cframe)) return;
                _cframe = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// The amount of dampening to apply.
        /// </summary>
        [InstMember(2)]
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
        [InstMember(3)]
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

        /// <summary/>
        protected override void OnStep(Part part)
        {
        }
    }
}
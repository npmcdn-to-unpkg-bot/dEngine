// BodyThrust.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// Applies a force on its parent towards a direction.
    /// </summary>
    [TypeId(119)]
    public sealed class BodyThrust : BodyMover
    {
        private BulletSharp.Math.Vector3 _force;
        private BulletSharp.Math.Vector3 _location;

        /// <summary>
        /// The amount of force applied on each axis.
        /// </summary>
        [InstMember(1)]
        [EditorVisible]
        public Vector3 Force
        {
            get { return (Vector3)_force; }
            set
            {
                _force = (BulletSharp.Math.Vector3)value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// The offset from the center of the part where force is applied.
        /// </summary>
        [InstMember(2)]
        [EditorVisible]
        public Vector3 Location
        {
            get { return (Vector3)_location; }
            set
            {
                _location = (BulletSharp.Math.Vector3)value;
                NotifyChanged();
            }
        }

        protected override void OnStep(Part part)
        {
            part.RigidBody.ApplyForceRef(ref _force, ref _location);
        }
    }
}
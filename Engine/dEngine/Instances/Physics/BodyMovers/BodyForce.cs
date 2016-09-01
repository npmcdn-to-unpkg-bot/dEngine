// BodyForce.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;
using BulletVector3 = BulletSharp.Math.Vector3;


namespace dEngine.Instances
{
    /// <summary>
    /// Applies a constant force to the parent <see cref="Part" />.
    /// </summary>
    [TypeId(40)]
    [ToolboxGroup("Body Movers")]
    [ExplorerOrder(14)]
    public sealed class BodyForce : BodyMover
    {
        private Vector3 _force;
        private BulletVector3 _bulletForce;

        /// <summary>
        /// The force to be applied.
        /// </summary>
        [InstMember(1)]
        [EditorVisible]
        public Vector3 Force
        {
            get { return _force; }
            set
            {
                if (value == _force)
                    return;

                _force = value;
                _bulletForce = (BulletVector3)value;

                NotifyChanged(nameof(Force));
            }
        }

        /// <inheritdoc />
        public override void Destroy()
        {
            base.Destroy();
        }

        /// <summary/>
        protected override void OnStep(Part part)
        {
            part.RigidBody.ApplyCentralForceRef(ref _bulletForce);
        }
    }
}
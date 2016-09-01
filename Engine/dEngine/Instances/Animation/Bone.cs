// Bone.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// A bone.
    /// </summary>
    [TypeId(34)]
    public class Bone : Instance
    {
        private Bone _parent;
        private float _physicsBlendWeight;
        private bool _simulatePhysics;

        /// <summary>
        /// The transform of the bone relative to <see cref="Skeleton.RootBone" />.
        /// </summary>
        [InstMember(1)]
        public CFrame CFrame { get; set; }

        public void SimulatePhysics(bool value)
        {
            _simulatePhysics = value;
        }

        public void SetPhysicsBlendWeight(float weight)
        {
            _physicsBlendWeight = weight;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Name;
        }
    }
}
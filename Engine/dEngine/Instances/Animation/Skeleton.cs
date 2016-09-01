// Skeleton.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// A skeleton.
    /// </summary>
    [TypeId(33)]
    [ExplorerOrder(3)]
    public class Skeleton : Instance
    {
        /// <summary>
        /// The root bone of this skeleton.
        /// </summary>
        [InstMember(1)]
        public Bone RootBone { get; set; }
    }
}
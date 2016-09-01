// PVInstance.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using CSCore.XAudio2.X3DAudio;
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// A physical instance is an entity which occupies space in the 3D world.
    /// </summary>
    [TypeId(21)]
    public abstract class PVInstance : Instance, IListenable
    {
        /// <summary>
        /// The transform of the object.
        /// </summary>
        public abstract CFrame CFrame { get; set; }

        /// <summary>
        /// The size of the object.
        /// </summary>
        public abstract Vector3 Size { get; set; }

        internal bool MuteCFrameChanges { get; set; }
        internal bool MuteSizeChanges { get; set; }


        void IListenable.UpdateListener(ref Listener listener)
        {
            ((IListenable)CFrame).UpdateListener(ref listener);
        }
    }
}
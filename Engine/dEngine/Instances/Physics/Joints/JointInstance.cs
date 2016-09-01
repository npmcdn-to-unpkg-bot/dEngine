// JointInstance.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// Base class for joints.
    /// </summary>
    [TypeId(93)]
    [ToolboxGroup("Joints (Legacy)")]
    [Obsolete("Constraints should be used instead of joints.")]
    public abstract class JointInstance : Instance
    {
        private Part _part0;
        private Part _part1;

        /// <summary>
        /// The primary part.
        /// </summary>
        [InstMember(1)]
        [EditorVisible]
        public Part Part0
        {
            get { return _part0; }
            set
            {
                if (_part0 == value) return;
                _part0 = value;
                UpdateJoint();
                NotifyChanged();
            }
        }

        /// <summary>
        /// The secondary part.
        /// </summary>
        [InstMember(2)]
        [EditorVisible]
        public Part Part1
        {
            get { return _part1; }
            set
            {
                if (_part1 == value) return;
                _part1 = value;
                UpdateJoint();
                NotifyChanged();
            }
        }

        /// <inheritdoc />
        protected abstract void UpdateJoint();
    }
}
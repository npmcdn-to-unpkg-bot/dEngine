// RodConstraint.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// Summary
    /// </summary>
    [TypeId(108)]
    public sealed class RodConstraint : Constraint
    {
        private float _length;

        /// <summary>
        /// The current distance between the two attachments.
        /// </summary>
        [EditorVisible("Derived")]
        public float CurrentDistance => 0;

        /// <summary>
        /// Summary
        /// </summary>
        [InstMember(1)]
        [EditorVisible("Rod")]
        public float Length
        {
            get { return _length; }
            set
            {
                if (value.Equals(_length)) return;
                _length = value;
                NotifyChanged();
            }
        }

        /// <summary />
        protected override void RebuildConstraint()
        {
            DestroyConstraint();
            if (Validate())
            {
            }
        }
    }
}
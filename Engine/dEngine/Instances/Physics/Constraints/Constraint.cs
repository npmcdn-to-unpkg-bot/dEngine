﻿// Constraint.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using BulletSharp;
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// Base class for constraints.
    /// </summary>
    [TypeId(103)]
    [ToolboxGroup("Constraints")]
    public abstract class Constraint : Instance
    {
        /// <summary/>
        protected Attachment _attachment0;
        /// <summary/>
        protected Attachment _attachment1;
        /// <summary/>
        protected TypedConstraint _constraint;
        private bool _enabled;

        /// <summary>
        /// The <see cref="Attachment" /> which is connected to <see cref="Attachment1" />.
        /// </summary>
        [InstMember(1)]
        [EditorVisible("Attachments")]
        public Attachment Attachment0
        {
            get { return _attachment0; }
            set
            {
                lock (Locker)
                {
                    if (value == _attachment0)
                        return;

                    _attachment0?.Changed.Disconnect(AttachmentOnChanged);
                    _attachment0 = value;
                    value?.Changed.Connect(AttachmentOnChanged);
                    if (value?.Part != null)
                        RebuildConstraint();
                }
                NotifyChanged(nameof(Attachment0));
            }
        }

        /// <summary>
        /// The <see cref="Attachment" /> which is connected to <see cref="Attachment0" />.
        /// </summary>
        [InstMember(2)]
        [EditorVisible("Attachments")]
        public Attachment Attachment1
        {
            get { return _attachment1; }
            set
            {
                lock (Locker)
                {
                    if (value == _attachment1)
                        return;

                    _attachment1?.Changed.Disconnect(AttachmentOnChanged);
                    _attachment1 = value;
                    value?.Changed.Connect(AttachmentOnChanged);
                    if (value?.Part != null)
                        RebuildConstraint();
                }

                NotifyChanged(nameof(Attachment1));
            }
        }

        /// <summary>
        /// Toggles whether or not the constraint is enabled.
        /// </summary>
        [InstMember(3)]
        [EditorVisible("Behaviour")]
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                if (value == _enabled)
                    return;

                _enabled = value;
                NotifyChanged(nameof(Enabled));
            }
        }

        /// <summary />
        public static implicit operator TypedConstraint(Constraint constraint)
        {
            return constraint._constraint;
        }

        /// <summary>
        /// Determines if both attachments have a valid <see cref="Part" />.
        /// </summary>
        protected bool Validate()
        {
            var part0 = _attachment0?.Part;
            var part1 = _attachment1?.Part;
            var hisWorld = World;
            return (hisWorld != null) && (hisWorld.Physics != null) && (part0 != null) && (part1 != null) &&
                   (part0.World == hisWorld) && (part1.World == hisWorld);
        }

        /// <summary>
        /// Called when an attachment is set or changed.
        /// </summary>
        protected abstract void RebuildConstraint();

        /// <summary>
        /// Removes the current constraint from the world.
        /// </summary>
        protected void DestroyConstraint()
        {
            var constraint = _constraint;
            if (constraint != null)
            {
                World.Physics.World.RemoveConstraint(_constraint);
                _constraint.Dispose(); // TODO: check if dispose crashes
            }
        }

        private void AttachmentOnChanged(string property)
        {
            lock (Locker)
            {
                RebuildConstraint();
            }
        }
    }
}
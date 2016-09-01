// PVAdornment.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// A base class for adornments which can be attached to a <see cref="PVInstance" />.
    /// </summary>
    [TypeId(127)]
    public abstract class PVAdornment : GuiBase3D
    {
        private PVInstance _adornee;

        /// <summary>
        /// The object to adorn to.
        /// </summary>
        [InstMember(1)]
        [EditorVisible]
        public PVInstance Adornee
        {
            get { return _adornee; }
            set
            {
                if (_adornee == value) return;

                var oldAdornee = _adornee;
                if (oldAdornee != null)
                    oldAdornee.Destroyed.Event -= OnAdorneeDestroyed;

                if (IsDestroyed)
                    return;

                _adornee = value;

                if (value != null)
                    value.Destroyed.Event += OnAdorneeDestroyed;

                OnAdorneeSet(value, oldAdornee);
                NotifyChanged(nameof(Adornee));
            }
        }

        /// <summary>
        /// Invoked when the <see cref="Adornee" /> property is changed.
        /// </summary>
        protected virtual void OnAdorneeSet(PVInstance newAdornee, PVInstance oldAdornee)
        {
        }

        /// <inheritdoc />
        protected virtual void OnAdorneeDestroyed()
        {
            if (_adornee != null)
                _adornee.Destroyed.Event -= OnAdorneeDestroyed;
            Adornee = null;
        }

        /// <inheritdoc />
        public override void Destroy()
        {
            Adornee = null;
            base.Destroy();
        }
    }
}
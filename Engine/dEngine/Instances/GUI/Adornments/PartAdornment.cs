// PartAdornment.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// A base class for adornments which can be attached to a <see cref="Part" />.
    /// </summary>
    [TypeId(128)]
    public abstract class PartAdornment : GuiBase3D
    {
        private Part _adornee;

        /// <summary>
        /// The object to adorn to.
        /// </summary>
        [InstMember(1)]
        [EditorVisible]
        public Part Adornee
        {
            get { return _adornee; }
            set
            {
                if (_adornee == value) return;
                _adornee = value;

                if (value != null)
                    _adornee.Destroyed.Event += OnAdorneeDestroyed;

                NotifyChanged(nameof(Adornee));
            }
        }

        private void OnAdorneeDestroyed()
        {
            if (_adornee != null)
                _adornee.Destroyed.Event -= OnAdorneeDestroyed;
            Adornee = null;
        }

        /// <inheritdoc />
        public override void Destroy()
        {
            base.Destroy();

            Adornee = null;
        }
    }
}
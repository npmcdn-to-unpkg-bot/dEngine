// SurfaceGui.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// A GUI which is rendered on the face of a <see cref="Part" />.
    /// </summary>
    [TypeId(113)]
    [ToolboxGroup("GUI")]
    public class SurfaceGui : LayerCollector
    {
        private Part _adornee;
        private Axis _face;

        /// <summary>
        /// The face of the <see cref="Adornee" /> which this gui will be rendered to.
        /// </summary>
        public Axis Face
        {
            get { return _face; }
            set
            {
                if (value == _face) return;
                _face = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// The <see cref="Part" /> to render on.
        /// </summary>
        public Part Adornee
        {
            get { return _adornee; }
            set
            {
                if (value == _adornee)
                    return;

                _adornee = value;
                NotifyChanged(nameof(Adornee));
            }
        }

        /// <inheritdoc />
        public override Vector2 AbsolutePosition { get; internal set; }

        /// <inheritdoc />
        public override Vector2 AbsoluteSize { get; internal set; }
    }
}
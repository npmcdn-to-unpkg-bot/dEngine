// GuiBase3D.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;
using dEngine.Instances.Interfaces;
using SharpDX.Direct3D11;

namespace dEngine.Instances
{
    /// <summary>
    /// Base class for 3D GUI objects.
    /// </summary>
    [TypeId(130)]
    [ToolboxGroup("3D GUI")]
    public abstract class GuiBase3D : GuiBase, ICameraUser
    {
        protected Colour _colour;
        private float _transparency;
        private bool _visible;
        private Camera _camera;

        /// <inheritdoc />
        protected GuiBase3D()
        {
            _colour = Colour.Blue;
            _visible = true;
            _transparency = 0;
        }

        /// <summary>
        /// The colour of the gui.
        /// </summary>
        [InstMember(1)]
        [EditorVisible]
        public Colour Colour
        {
            get { return _colour; }
            set
            {
                if (value == _colour)
                    return;

                _colour = value;
                NotifyChanged(nameof(Colour));
            }
        }

        /// <summary>
        /// The transparency of the gui.
        /// </summary>
        [InstMember(2)]
        [EditorVisible]
        public float Transparency
        {
            get { return _transparency; }
            set
            {
                if (value == _transparency)
                    return;

                _transparency = value;
                NotifyChanged(nameof(Transparency));
            }
        }

        /// <summary>
        /// Determines if the gui object is visible.
        /// </summary>
        [InstMember(3)]
        [EditorVisible]
        public bool Visible
        {
            get { return _visible; }
            set
            {
                if (value == _visible)
                    return;

                _visible = value;

                NotifyChanged(nameof(Visible));
            }
        }

        Camera ICameraUser.Camera
        {
            get { return _camera; }
            set
            {
                var camera = _camera;
                if (camera != null)
                    lock (camera.Locker)
                    {
                        camera.Gui3Ds.Remove(this);
                    }

                _camera = value;

                if (value != null)
                    lock (value.Locker)
                    {
                        value.Gui3Ds.Add(this);
                    }
            }
        }

        internal abstract void Draw(ref DeviceContext context, ref Camera camera);
    }
}
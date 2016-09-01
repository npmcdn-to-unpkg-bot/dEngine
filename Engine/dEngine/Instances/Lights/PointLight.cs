// PointLight.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;
using dEngine.Services;

namespace dEngine.Instances
{
    /// <summary>
    /// A point light.
    /// </summary>
    /// <seealso cref="SpotLight" />
    [TypeId(98)]
    [ToolboxGroup("Lights")]
    [ExplorerOrder(3)]
    public sealed class PointLight : Light
    {
        private float _falloff;
        private float _intensity;
        private Part _parentPart;
        private float _range;

        /// <inheritdoc />
        public PointLight()
        {
            _range = 8;
            _falloff = 4;

            Lighting.AddPointLight(this);

            ParentChanged.Event += OnParentChanged;

            /*
            _updatePointLightJob = new RenderJob("UpdatePointLightData", context =>
            {
                var lightIndex = LightIndex;
                DataStream stream;
                var buffer = Renderer.PointLightBuffer;
                if (buffer == null)
                    return;
                context.MapSubresource(buffer, MapMode.Write, 0, out stream);
                stream.Position = PointLightData.Stride * lightIndex;
                stream.Write(new PointLightData
                {
                    Colour = (Color3)Colour,
                    Falloff = _falloff,
                    Range = _range,
                    Position = _parentPart.CFrame.p
                });
                context.UnmapSubresource(Renderer.PointLightBuffer, 0);
            });
            */
        }

        /// <summary>
        /// The range of the light.
        /// </summary>
        [InstMember(1)]
        [EditorVisible]
        public float Range
        {
            get { return _range; }
            set
            {
                if (value == _range) return;
                _range = value;
                NotifyChanged();
                UpdateLightData();
            }
        }

        /// <summary>
        /// The falloff distance for the light.
        /// </summary>
        [InstMember(2)]
        [EditorVisible]
        public float Falloff
        {
            get { return _falloff; }
            set
            {
                if (value == _falloff) return;
                _falloff = value;
                NotifyChanged();
                UpdateLightData();
            }
        }

        /// <summary>
        /// Total energy that the light emits.
        /// </summary>
        [InstMember(3)]
        [EditorVisible]
        public float Brightness
        {
            get { return _intensity; }
            set
            {
                if (value == _intensity) return;
                _intensity = value;
                NotifyChanged();
                UpdateLightData();
            }
        }

        private void OnParentChanged(Instance parent)
        {
            var part = parent as Part;

            if (_parentPart != null)
                _parentPart.Moved.Event -= OnParentMoved;

            if (part == null)
            {
                Lighting.RemovePointLight(this);
            }
            else
            {
                Lighting.AddPointLight(this);
                part.Moved.Event += OnParentMoved;
            }

            _parentPart = part;
        }

        private void OnParentMoved()
        {
            UpdateLightData();
        }

        internal override void UpdateLightData()
        {
            /*
            lock (Lighting.LightSync)
            {
                if (LightIndex == -1)
                    return;
                _updatePointLightJob.Do();
            }
            */
        }

        /// <inheritdoc />
        public override void Destroy()
        {
            base.Destroy();
            Lighting.RemovePointLight(this);
        }
    }
}
// PostEffect.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Collections.Generic;
using dEngine.Instances.Attributes;
using dEngine.Services;
using SharpDX.Direct3D11;

#pragma warning disable 1591

namespace dEngine.Instances
{
    /// <summary>
    /// Abstract class for post processing effects. PostEffects must be parented to the <see cref="Workspace.CurrentCamera" />.
    /// </summary>
    [TypeId(173)]
    public abstract class PostEffect : Instance
    {
        protected int _effectOrder;
        private bool _enabled;

        protected Camera Camera;

        /// <inheritdoc />
        protected PostEffect()
        {
            Enabled = true;
        }

        /// <summary>
        /// Determines if the effect is applied.
        /// </summary>
        [InstMember(1)]
        [EditorVisible]
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                if (value == _enabled)
                    return;

                _enabled = value;
                NotifyChanged();
            }
        }

        internal virtual void UpdateSize(Camera camera)
        {
        }

        internal void SetCamera(Camera camera)
        {
            Camera = camera;
            UpdateSize(camera);
        }

        internal abstract void Render(ref DeviceContext context);

        /// <inheritdoc />
        public override void Destroy()
        {
            base.Destroy();
            Camera?.PostEffects.Remove(this);
        }

        protected enum EffectPrority
        {
            AmbientOcclusion,
            MotionBlur,
            DepthOfField,
            Bloom,
            LensFlare,
            ToneMapping,
            ColourCorrection = 50000
        }

        internal class PostEffectSorter : IComparer<PostEffect>
        {
            public int Compare(PostEffect x, PostEffect y)
            {
                var comp = x._effectOrder.CompareTo(y._effectOrder);
                return comp == 0 ? x.GetHashCode().CompareTo(y.GetHashCode()) : comp;
            }
        }
    }
}
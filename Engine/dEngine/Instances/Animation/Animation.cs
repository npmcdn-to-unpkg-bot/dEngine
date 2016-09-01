// Animation.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Data;
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// An animation for a <see cref="SkeletalMesh" />.
    /// </summary>
    [TypeId(32)]
    [ExplorerOrder(22)]
    public class Animation : Instance
    {
        private Content<AnimationData> _animationContent;

        public Animation()
        {
            _animationContent = new Content<AnimationData>();
        }

        /// <summary>
        /// The content id for the animation data.
        /// </summary>
        public Content<AnimationData> AnimationId
        {
            get { return _animationContent.ContentId; }
            set
            {
                _animationContent = value;
                value.Subscribe(this);
                NotifyChanged();
            }
        }

        public void AdjustSpeed()
        {
        }
    }
}
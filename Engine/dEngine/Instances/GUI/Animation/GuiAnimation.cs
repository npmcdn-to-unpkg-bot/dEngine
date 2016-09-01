// GuiAnimation.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Reflection;
using dEngine.Instances.Attributes;
using dEngine.Serializer.V1;

namespace dEngine.Instances
{
    /// <summary>
    /// Base class for GUI animations.
    /// </summary>
    [TypeId(226)]
    public abstract class GuiAnimation : Instance
    {
        private TimeSpan _duration;

        internal double CurrentTime;

        internal GuiElement TargetElement;

        /// <summary>
        /// Determines if the animation is being played.
        /// </summary>
        public bool IsAnimating { get; internal set; }

        internal Inst.CachedProperty TargetProperty { get; set; }
        internal abstract Type TargetType { get; }

        /// <summary>
        /// The duration of the animation.
        /// </summary>
        [InstMember(1)]
        [EditorVisible]
        public TimeSpan Duration
        {
            get { return _duration; }
            set
            {
                if (value == _duration)
                    return;
                _duration = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// Gets/Sets the playback position of the animation.
        /// </summary>
        [InstMember(2)]
        [EditorVisible]
        public TimeSpan PlaybackPosition
        {
            get { return TimeSpan.FromSeconds(CurrentTime); }
            set
            {
                CurrentTime = value.totalSeconds;
                NotifyChanged();
            }
        }

        /// <summary>
        /// Returns the target element and property.
        /// </summary>
        /// <returns></returns>
        public LuaTuple<GuiElement, string> GetTarget()
        {
            return new LuaTuple<GuiElement, string>(TargetElement, TargetProperty?.Name);
        }

        internal double GetDelta()
        {
            if (CurrentTime >= _duration.totalSeconds)
                return 1;
            return CurrentTime/_duration.totalSeconds;
        }

        /// <summary>
        /// Sets the element and property that the animation will be applied to.
        /// </summary>
        public void SetTarget(GuiElement element, string property)
        {
            lock (Locker)
            {
                if (element == null)
                    throw new ArgumentException("The target element cannot be null.");
                if (string.IsNullOrEmpty(property))
                    throw new ArgumentException("The target property cannot be null or empty.");

                var prop = element.GetType().GetProperty(property);
                var instAttr = prop?.DeclaringType?.GetCustomAttribute<TypeIdAttribute>();
                var memberAttr = prop?.GetCustomAttribute<InstMemberAttribute>();

                if (prop == null)
                    throw new ArgumentException($"No property found for \"{property}\".");
                if (prop.PropertyType != TargetType)
                    throw new ArgumentException(
                        $"The animation type ({TargetType}) does not match the property type of \"{property}\" ({prop.PropertyType}).");
                if ((memberAttr == null) || (instAttr == null))
                    throw new ArgumentException($"The property {property} of {element} cannot be animated.");

                TargetProperty =
                    Inst.TypeDictionary[element.ClassName].TaggedProperties[
                        Inst.EncodePropertyTag(instAttr.Id, memberAttr.Tag)];
                TargetElement = element;
            }
        }


        internal abstract bool Update();
    }
}
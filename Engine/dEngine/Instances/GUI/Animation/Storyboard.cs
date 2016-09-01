// Storyboard.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Collections.Generic;
using dEngine.Instances.Attributes;
using dEngine.Services;

namespace dEngine.Instances
{
    /// <summary>
    /// A storyboard allows the organization of multiple GUI animations.
    /// </summary>
    [TypeId(147)]
    [ToolboxGroup("GUI")]
    public class Storyboard : Instance
    {
        private readonly object _storyboardLocker = new object();
        private readonly List<GuiAnimation> _animations;
        private bool _playing;

        /// <summary />
        public Storyboard()
        {
            _animations = new List<GuiAnimation>();
            Ended = new Signal(this);
        }

        /// <summary />
        protected override void OnChildAdded(Instance child)
        {
            base.OnChildAdded(child);
            GuiAnimation animation;
            if ((animation = child as GuiAnimation) != null)
                lock (_storyboardLocker)
                {
                    _animations.Add(animation);
                }
        }

        /// <summary />
        protected override void OnChildRemoved(Instance child)
        {
            base.OnChildRemoved(child);
            GuiAnimation animation;
            if ((animation = child as GuiAnimation) != null)
                lock (_storyboardLocker)
                {
                    _animations.Remove(animation);
                }
        }

        /// <summary>
        /// Begins the storyboard.
        /// </summary>
        public void Begin()
        {
            RunService.Service.Heartbeat.Connect(OnHeartbeat);
            _playing = true;
        }

        /// <summary>
        /// Ends the storyboard.
        /// </summary>
        public void End()
        {
            RunService.Service.Heartbeat.Disconnect(OnHeartbeat);
            _playing = false;
            Ended.Fire();
        }

        private void OnHeartbeat(double t)
        {
            lock (_storyboardLocker)
            {
                var count = _animations.Count;
                var finished = 0;
                for (var i = 0; i < count; i++)
                {
                    var anim = _animations[i];
                    anim.CurrentTime += t;
                    if (!anim.Update())
                        finished++;
                }
                if (finished == count)
                    End();
            }
        }

        /// <summary>
        /// Fired once all the animations have finished.
        /// </summary>
        public readonly Signal Ended;
    }
}
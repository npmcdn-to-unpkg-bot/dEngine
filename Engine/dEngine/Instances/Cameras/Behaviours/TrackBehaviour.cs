// TrackBehaviour.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;

namespace dEngine.Instances
{
    internal class TrackBehaviour : Camera.Behaviour
    {
        public TrackBehaviour(Camera camera) : base(camera)
        {
        }

        internal override void Update(double step)
        {
            throw new NotImplementedException();
        }
    }
}
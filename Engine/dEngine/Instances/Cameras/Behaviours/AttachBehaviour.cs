// AttachBehaviour.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;

namespace dEngine.Instances
{
    internal class AttachBehaviour : Camera.Behaviour
    {
        public AttachBehaviour(Camera camera) : base(camera)
        {
        }

        internal override void Update(double step)
        {
            throw new NotImplementedException();
        }
    }
}
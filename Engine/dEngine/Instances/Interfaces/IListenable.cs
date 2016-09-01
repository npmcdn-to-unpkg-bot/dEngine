// IListenable.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using CSCore.XAudio2.X3DAudio;

namespace dEngine.Instances
{
    public interface IListenable
    {
        void UpdateListener(ref Listener listener);
    }
}
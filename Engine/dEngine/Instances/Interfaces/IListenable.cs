// IListenable.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using CSCore.XAudio2.X3DAudio;

namespace dEngine.Instances
{
    /// <summary>
    /// Interface for objects which can be used an audio listener.
    /// </summary>
    public interface IListenable
    {
        /// <summary/>
        void UpdateListener(ref Listener listener);
    }
}
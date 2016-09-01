// ICameraSubject.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
namespace dEngine.Instances.Interfaces
{
    /// <summary>
    /// An interface for instances which can be a <see cref="Camera.CameraSubject" />.
    /// </summary>
    public interface ICameraSubject
    {
        /// <summary>
        /// Returns the velocity of the subject for use by the sound listener.
        /// </summary>
        Vector3 GetVelocity();
    }
}
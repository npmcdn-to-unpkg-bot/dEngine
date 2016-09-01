// DoFMethod.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
namespace dEngine
{
    /// <summary>
    /// Depth of Field methods.
    /// </summary>
    public enum DoFMethod
    {
        /// <summary>
        /// Gaussian DoF blurs the scene using a standard Guassian blur.
        /// </summary>
        Gaussian,

        /// <summary>
        /// Bokeh is the name of the shape that can be seen in photos or movies when objects are out of focus.
        /// </summary>
        Bokeh
    }
}
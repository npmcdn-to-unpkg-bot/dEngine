// Service.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances;
using dEngine.Instances.Attributes;

namespace dEngine.Services
{
    /// <summary>
    /// A service is a top-level singleton class.
    /// </summary>
    /// <remarks>
    /// Services can be retrived by using the <see cref="DataModel.GetService" /> method.
    /// </remarks>
    [TypeId(62)]
    [ToolboxGroup("Services")]
    public abstract class Service : Instance, ISingleton
    {
        /// <inheritdoc />
        protected Service()
        {
            Parent = Game.DataModel;

            ParentLocked = true;
        }
    }
}
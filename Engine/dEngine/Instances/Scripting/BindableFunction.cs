// BindableFunction.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using dEngine.Instances.Attributes;
using Dynamitey;
using Neo.IronLua;

namespace dEngine.Instances
{
    /// <summary>
    /// Acts as a custom function to allow communication between scripts.
    /// </summary>
    [TypeId(54)]
    [ToolboxGroup("Scripting")]
    [Warning(
         "BindableFunctions do not allows for communication between the server and client. If you are looking for this functionality use [RemoteFunctions](index?.htmltitle=RemoteFunction)."
     )]
    public class BindableFunction : Instance
    {
        private MulticastDelegate _onInvoke;

        /// <summary>
        /// A function to be invoked when <see cref="Invoke" /> is called.
        /// </summary>
        public MulticastDelegate OnInvoke
        {
            get { return _onInvoke; }
            set
            {
                _onInvoke = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// Invokes the <see cref="OnInvoke" /> method.
        /// </summary>
        /// <param name="args">The arguments to pass to the callback.</param>
        /// <returns>The return value of the callback.</returns>
        public LuaResult Invoke(params object[] args)
        {
            return (LuaResult)OnInvoke.FastDynamicInvoke(args);
        }
    }
}
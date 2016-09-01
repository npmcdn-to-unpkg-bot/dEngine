// RemoteFunction.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;
using Neo.IronLua;

namespace dEngine.Instances
{
    /// <summary>
    /// A networked method.
    /// </summary>
    [TypeId(45)]
    [ToolboxGroup("Scripting")]
    public sealed class RemoteFunction : Instance
    {
        private DeliveryMethod _deliveryMethod;

        /// <inheritdoc />
        public RemoteFunction()
        {
            _deliveryMethod = DeliveryMethod.ReliableUnordered;
        }

        /// <summary>
        /// The delivery method to use.
        /// </summary>
        [InstMember(1)]
        public DeliveryMethod DeliveryMethod
        {
            get { return _deliveryMethod; }
            set
            {
                if (value == _deliveryMethod) return;
                _deliveryMethod = value;
                NotifyChanged(nameof(DeliveryMethod));
            }
        }

        /// <summary>
        /// The callback for when the server invokes this function.
        /// </summary>
        /// <remarks>
        /// This function will execute on the client everytime the server invokes it with InvokeClient()
        /// </remarks>
        public LuaMethod OnClientInvoke { get; set; }

        /// <summary>
        /// The callback for when the client invokes this function.
        /// </summary>
        /// <remarks>
        /// This function will execute on the server everytime the client invokes it with InvokeServer()
        /// </remarks>
        public LuaMethod OnServerInvoke { get; set; }

        /// <summary>
        /// Invokes the function that the provided player has bound to this instance.
        /// </summary>
        public LuaResult InvokeClient(Player player, params dynamic[] args)
        {
            return null;
        }

        /// <summary>
        /// Invokes the function that the server has bound to this instance.
        /// </summary>
        public LuaResult InvokeServer(params dynamic[] args)
        {
            return null;
        }
    }
}
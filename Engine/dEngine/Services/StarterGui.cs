// StarterGui.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances;
using dEngine.Instances.Attributes;

namespace dEngine.Services
{
    /// <summary>
    /// A container for guis to be parented to players when they join.
    /// </summary>
    [TypeId(192)]
    [ExplorerOrder(6)]
    [Uncreatable]
    public class StarterGui : GuiContainerBase, ISingleton
    {
        private bool _resetGuisOnDeath;

        /// <summary>
        /// Determines whether guis will be reset when the player's character dies.
        /// </summary>
        [InstMember(1)]
        [EditorVisible("Behaviour")]
        public bool ResetGuisOnDeath
        {
            get { return _resetGuisOnDeath; }
            set
            {
                if (value == _resetGuisOnDeath)
                    return;

                _resetGuisOnDeath = value;
                NotifyChanged(nameof(ResetGuisOnDeath));
            }
        }

        internal static object GetExisting()
        {
            return Game.StarterGui;
        }
    }
}
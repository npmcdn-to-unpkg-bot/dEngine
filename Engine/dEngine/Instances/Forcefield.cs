// Forcefield.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// Protects a <see cref="Character" /> from damage by <see cref="Character.TakeDamage" />.
    /// </summary>
    [TypeId(15)]
    [ExplorerOrder(3)]
    [ToolboxGroup("Gameplay")]
    public sealed class Forcefield : Instance
    {
        private Character _character;

        /// <inheritdoc />
        public Forcefield()
        {
            ParentChanged.Event += OnParentChanged;
        }

        private void OnParentChanged(Instance parent)
        {
            var character = parent as Character;
            var lastCharacter = _character;
            _character = character;

            if (character != null)
            {
                character.ShieldCount++;
                Logger.Trace($"Character {character} gained a Forcefield.");
            }
            else if (lastCharacter != null)
            {
                lastCharacter.ShieldCount--;
                Logger.Trace($"Character {lastCharacter} lost a Forcefield.");
            }
        }
    }
}
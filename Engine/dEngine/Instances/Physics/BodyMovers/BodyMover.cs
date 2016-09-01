// BodyMover.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// Base class for physics force-related objects.
    /// </summary>
    [TypeId(115)]
    [ExplorerOrder(14)]
    [ToolboxGroup("Body movers")]
    public abstract class BodyMover : Instance
    {
        /// <summary>
        /// The part to apply force to.
        /// </summary>
        protected Part _parentPart;

        /// <summary>
        /// Called when the world this mover belongs to is stepped.
        /// </summary>
        /// <param name="part">The parent part at the time of the step.</param>
        protected abstract void OnStep(Part part);

        /// <inheritdoc />
        protected override void OnAncestryChanged(Instance child, Instance parent)
        {
            base.OnAncestryChanged(child, parent);
            var part = parent as Part;
            _parentPart = part;
            OnParentPartChanged(part);
        }

        /// <inheritdoc />
        protected override void OnWorldChanged(IWorld newWorld, IWorld oldWorld)
        {
            base.OnWorldChanged(newWorld, oldWorld);

            if (oldWorld != null)
                oldWorld.Physics.Stepped -= PhysicsOnStepped;

            if (newWorld != null)
                newWorld.Physics.Stepped += PhysicsOnStepped;
        }

        private void PhysicsOnStepped()
        {
            var part = _parentPart;
            if (part != null)
                OnStep(part);
        }

        /// <summary>
        /// Fired when <see cref="_parentPart" /> changes.
        /// </summary>
        /// <param name="parentPart"></param>
        protected virtual void OnParentPartChanged(Part parentPart)
        {
        }
    }
}
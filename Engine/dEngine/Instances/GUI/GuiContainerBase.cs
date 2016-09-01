// GuiContainerBase.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;
using dEngine.Instances.Interfaces;

namespace dEngine.Instances
{
    /// <summary>
    /// Base class for gui containers.
    /// </summary>
    [TypeId(37)]
    [ToolboxGroup("Containers")]
    public abstract class GuiContainerBase : Instance
    {
        /// <inheritdoc />
        protected GuiContainerBase()
        {
            Game.Workspace.CameraChanged.Event += OnWorkspaceCurrentCameraChanged;
        }

        private void OnWorkspaceCurrentCameraChanged(Camera camera)
        {
            foreach (var child in Children)
            {
                ICameraUser cameraUser;

                if ((cameraUser = child as ICameraUser) != null)
                    cameraUser.Camera = camera;
            }
        }

        /// <inheritdoc />
        protected override void OnChildAdded(Instance child)
        {
            base.OnChildAdded(child);

            var cameraUser = child as ICameraUser;
            if (cameraUser != null)
                cameraUser.Camera = Game.Workspace.CurrentCamera;
        }

        /// <inheritdoc />
        protected override void OnChildRemoved(Instance child)
        {
            base.OnChildRemoved(child);

            ICameraUser cameraUser;

            if ((cameraUser = child as ICameraUser) != null)
                cameraUser.Camera = null;
        }

        /// <inheritdoc />
        public override void Destroy()
        {
            base.Destroy();
            Game.Workspace.CameraChanged.Event -= OnWorkspaceCurrentCameraChanged;
        }
    }
}
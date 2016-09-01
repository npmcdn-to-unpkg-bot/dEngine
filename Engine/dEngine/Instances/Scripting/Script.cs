// Script.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;
using dEngine.Services;

namespace dEngine.Instances
{
    /// <summary>
    /// A script which is executed by the server.
    /// </summary>
    /// <remarks>
    /// A <see cref="Script" /> must be a descendant of an <see cref="IWorld" /> or <see cref="ServerScriptService" /> to run.
    /// </remarks>
    [TypeId(4)]
    [ToolboxGroup("Scripting")]
    [ExplorerOrder(3)]
    public class Script : LuaSourceContainer
    {
        private bool _disabled;
        private bool _validAncestry;

        /// <inheritdoc />
        public Script()
        {
            Identity = Engine.Mode == EngineMode.Server ? ScriptIdentity.Server : ScriptIdentity.Script;
        }

        /// <summary>
        /// Determines whether this script can execute code.
        /// </summary>
        /// <remarks>
        /// Setting Disabled to false at runtime will force the script to execute.
        /// </remarks>
        [InstMember(1)]
        [EditorVisible]
        public bool Disabled
        {
            get { return _disabled; }
            set
            {
                if (value == _disabled) return;

                _disabled = value;

                if (value)
                {
                    Stop();
                }
                else
                {
                    if (CheckCanRun())
                    {
#pragma warning disable 4014
                        Run();
#pragma warning restore 4014
                    }
                }

                NotifyChanged();
            }
        }

        /// <inheritdoc />
        protected override void OnInstanceIdChanged(string newId, string oldId)
        {
            base.OnInstanceIdChanged(newId, oldId);
            ScriptService.DeregisterScript(this, oldId);
            ScriptService.RegisterScript(this, newId);
        }

        /// <inheritdoc />
        protected override void OnAncestryChanged(Instance child, Instance parent)
        {
            base.OnAncestryChanged(child, parent);

            var prevAncestryValid = _validAncestry;

            _validAncestry = CheckIfAncestorValid();

            // if the new ancestry is valid, but the previous was not
            if ((RunService.SimulationState == SimulationState.Running) && _validAncestry && !prevAncestryValid)
                Run();
        }

        /// <inheritdoc />
        public override void Destroy()
        {
            base.Destroy();

            ScriptService.DeregisterScript(this, InstanceId);
        }

        /// <summary>
        /// Returns true if the script's ancestry is valid.
        /// </summary>
        protected virtual bool CheckIfAncestorValid()
        {
            return (World != null) || IsDescendantOf(ServerScriptService.Service);
        }

        internal bool CheckCanRun()
        {
            return (RunService.SimulationState == SimulationState.Running)
                   && !IsRunning
                   && _validAncestry;
        }

        /// <summary>
        /// Executes the script asynchronously.
        /// </summary>
        internal void Run(params object[] args)
        {
            if (Disabled)
                return;
            //return LuaResult.Empty;

            ScriptService.ExecuteAsync(this, args).ConfigureAwait(false);

            //return result;
        }

        /// <summary>
        /// Stops script execution.
        /// </summary>
        public void Stop()
        {
            IsRunning = false;
        }
    }
}
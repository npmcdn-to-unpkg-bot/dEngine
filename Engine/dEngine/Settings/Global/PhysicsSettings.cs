// PhysicsSettings.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Runtime.CompilerServices;
using BulletSharp;
using dEngine.Instances.Attributes;

namespace dEngine.Settings.Global
{
    /// <summary>
    /// Settings for the physics engine.
    /// </summary>
    [TypeId(200)]
    public class PhysicsSettings : Settings
    {
        private static bool _areAabbsShown;
        private static bool _areConstraintLimitsShown;
        private static bool _areConstraintsShown;
        private static bool _areContactPointsShown;
        private static bool _areNormalsShown;
        private static bool _showWireframe;
        private static float _linearSleepingThreshold;
        private static float _angularSleepingThreshold;

        private static bool _showDecompositionGeometry;

        /// <summary>
        /// The linear sleeping threshold for physics objects.
        /// </summary>
        [EditorVisible("Sleep", "Linear Sleeping Threshold")]
        public static float LinearSleepingThreshold
        {
            get { return _linearSleepingThreshold; }
            set
            {
                if (value == _linearSleepingThreshold) return;
                _linearSleepingThreshold = value;
                NotifyChangedStatic();
            }
        }

        /// <summary>
        /// The angular sleeping threshold for physics objects.
        /// </summary>
        [EditorVisible("Sleep", "Angular Sleeping Threshold")]
        public static float AngularSleepingThreshold
        {
            get { return _angularSleepingThreshold; }
            set
            {
                if (value == _angularSleepingThreshold) return;
                _angularSleepingThreshold = value;
                NotifyChangedStatic();
            }
        }

        /// <summary>
        /// Summary
        /// </summary>
        [EditorVisible]
        public static bool ShowDecompositionGeometry
        {
            get { return _showDecompositionGeometry; }
            set
            {
                if (value == _showDecompositionGeometry) return;
                _showDecompositionGeometry = value;
                NotifyChangedStatic();
            }
        }

        /// <summary>
        /// Determines if wireframes are drawn.
        /// </summary>
        [EditorVisible("Display", "Show Wireframe")]
        public static bool ShowWireframe
        {
            get { return _showWireframe; }
            set
            {
                _showWireframe = value;
                UpdateDisplay();
                NotifyChangedStatic();
            }
        }

        /// <summary>
        /// Determines if constraints are drawn.
        /// </summary>
        [EditorVisible("Display", "Display Constraints")]
        public static bool AreConstraintsShown
        {
            get { return _areConstraintsShown; }
            set
            {
                _areConstraintsShown = value;
                UpdateDisplay();
                NotifyChangedStatic();
            }
        }

        /// <summary>
        /// Determines if constraint limits are drawn.
        /// </summary>
        [EditorVisible("Display", "Display Constraint Limits")]
        public static bool AreConstraintLimitsShown
        {
            get { return _areConstraintLimitsShown; }
            set
            {
                _areConstraintLimitsShown = value;
                UpdateDisplay();
                NotifyChangedStatic();
            }
        }

        /// <summary>
        /// Determines if AABBs are drawn.
        /// </summary>
        [EditorVisible("Display", "Display Bounds")]
        public static bool AreAabbsShown
        {
            get { return _areAabbsShown; }
            set
            {
                _areAabbsShown = value;
                UpdateDisplay();
                NotifyChangedStatic();
            }
        }

        /// <summary>
        /// Determines if contact points are drawn.
        /// </summary>
        [EditorVisible("Display", "Display Contact Points")]
        public static bool AreContactPointsShown
        {
            get { return _areContactPointsShown; }
            set
            {
                _areContactPointsShown = value;
                UpdateDisplay();
                NotifyChangedStatic();
            }
        }

        /// <summary>
        /// Determines if normals are drawn.
        /// </summary>
        [EditorVisible("Display", "Display Normals")]
        public static bool AreNormalsShown
        {
            get { return _areNormalsShown; }
            set
            {
                _areNormalsShown = value;
                UpdateDisplay();
                NotifyChangedStatic();
            }
        }

        internal static DebugDrawModes DebugDrawModes { get; private set; }

        private static void NotifyChangedStatic([CallerMemberName] string propertyName = null)
        {
            Engine.GlobalSettings?.PhysicsSettings?.NotifyChanged(propertyName);
        }

        private static void UpdateDisplay()
        {
            var drawModes = DebugDrawModes.None;

            if (_areAabbsShown) drawModes |= DebugDrawModes.DrawAabb;
            if (_areConstraintLimitsShown) drawModes |= DebugDrawModes.DrawConstraintLimits;
            if (_areConstraintsShown) drawModes |= DebugDrawModes.DrawConstraints;
            if (_areContactPointsShown) drawModes |= DebugDrawModes.DrawContactPoints;
            if (_areNormalsShown) drawModes |= DebugDrawModes.DrawNormals;
            if (_showWireframe) drawModes |= DebugDrawModes.DrawWireframe;

            DebugDrawModes = drawModes;

            if (Game.IsInitialized)
                Game.Workspace.Physics.UpdateDebugDrawModes();
            else
                Game.Initialized += (sender, args) => { Game.Workspace.Physics.UpdateDebugDrawModes(); };
        }

        /// <summary />
        public override void RestoreDefaults()
        {
            LinearSleepingThreshold = 0.8f;
            AngularSleepingThreshold = 1.0f;

            AreAabbsShown = false;
            AreConstraintLimitsShown = false;
            AreConstraintsShown = false;
            AreContactPointsShown = false;
            AreNormalsShown = false;
            ShowWireframe = false;
            ShowDecompositionGeometry = false;
        }
    }
}
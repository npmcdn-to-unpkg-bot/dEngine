// UserSettings.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances;
using dEngine.Instances.Attributes;
using dEngine.Settings.User;

namespace dEngine.Settings
{
    /// <summary>
    /// User settings container.
    /// </summary>
    [TypeId(187)]
    public class UserSettings : GenericSettings, ISingleton
    {
        /// <summary />
        public UserSettings()
        {
            UserGameSettings = CreateSettings<UserGameSettings>("Game");
            UserAnalyticsSettings = CreateSettings<UserAnalyticsSettings>("Analytics");
        }

        /// <summary>
        /// The <see cref="UserGameSettings" />.
        /// </summary>
        public UserGameSettings UserGameSettings { get; }

        /// <summary>
        /// The <see cref="UserAnalyticsSettings" />.
        /// </summary>
        public UserAnalyticsSettings UserAnalyticsSettings { get; }

        internal static object GetExisting()
        {
            return Engine.UserSettings;
        }
    }
}
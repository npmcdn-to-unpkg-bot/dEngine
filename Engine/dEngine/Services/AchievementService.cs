// AchievementService.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using dEngine.Instances;
using dEngine.Instances.Attributes;
using Steamworks;

namespace dEngine.Services
{
    /// <summary>
    /// A service for awarding achievements.
    /// </summary>
    [TypeId(99)]
    public class AchievementService : Service
    {
        internal static AchievementService Service;

        /// <inheritdoc />
        public AchievementService()
        {
            Service = this;
        }

        /// <summary>
        /// Determines if the given achievement name is valid.
        /// </summary>
        /// <returns></returns>
        public bool IsValidAchievement(uint userId, string achievement)
        {
            var steamId = Player.SteamIdFromUserId(userId);
            bool _;
            return SteamGameServerStats.GetUserAchievement(steamId, achievement, out _);
        }

        /// <summary>
        /// Determines if the user has unlocked the given achievement.
        /// </summary>
        /// <param name="userId">The user to check.</param>
        /// <param name="achievement">The name of the achievement.</param>
        /// <returns></returns>
        public bool UserHasAchievement(uint userId, string achievement)
        {
            if (string.IsNullOrWhiteSpace(achievement))
                throw new InvalidOperationException("The achievement name cannot be empty.");

            var steamId = Player.SteamIdFromUserId(userId);
            bool achieved;
            if (!SteamGameServerStats.GetUserAchievement(steamId, achievement, out achieved))
                throw new InvalidOperationException($"Could not get achieved \"{achievement}\"");
            return achieved;
        }

        /// <summary>
        /// Unlocks the achievement for the given user.
        /// </summary>
        /// <param name="userId">The user to unlock for.</param>
        /// <param name="achievement">The name of the achievement.</param>
        public void UnlockAchievement(uint userId, string achievement)
        {
            if (string.IsNullOrWhiteSpace(achievement))
                throw new InvalidOperationException("The achievement name cannot be empty.");

            var steamId = Player.SteamIdFromUserId(userId);
            if (!SteamGameServerStats.SetUserAchievement(steamId, achievement))
                throw new InvalidOperationException(
                    $"An error occured while while unlocking achievement \"{achievement}\"");

            Logger.Info(
                $"Unlocked achievement \"{achievement}\" for {Game.Players.GetPlayerByUserId(userId)?.Name ?? "?"}{userId}");
        }

        /// <summary>
        /// Re-locks the achievement for the user.
        /// </summary>
        /// <param name="userId">The user to lock for.</param>
        /// <param name="achievement">The name of the achievement.</param>
        public void RevokeAchievement(uint userId, string achievement)
        {
            var steamId = Player.SteamIdFromUserId(userId);
            if (!SteamGameServerStats.ClearUserAchievement(steamId, achievement))
                throw new InvalidOperationException(
                    $"An error occured while while revoking achievement \"{achievement}\"");
        }

        internal static object GetExisting()
        {
            return DataModel.GetService<AchievementService>();
        }
    }
}
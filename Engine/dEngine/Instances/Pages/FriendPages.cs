// FriendPages.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections.Generic;
using dEngine.Instances.Attributes;
using dEngine.Services;
using Steamworks;

namespace dEngine.Instances
{
    /// <summary>
    /// A variation of pages used by <see cref="Players.GetFriendsAsync" />
    /// </summary>
    [TypeId(191)]
    public sealed class FriendPages : Pages
    {
        private readonly int _friendCount;
        private readonly EFriendFlags _friendFlags;
        private readonly CSteamID _steamId;
        private int _friendIndex;

        internal FriendPages(uint userId)
        {
            var accID = LoginService.SteamId.GetAccountID().m_AccountID;
            if (userId != accID)
                throw new NotSupportedException(
                    "GetFriendsAsync() can currently only check the friends of the LocalPlayer.");

            _steamId = new CSteamID(new AccountID_t(userId), EUniverse.k_EUniversePublic,
                EAccountType.k_EAccountTypeIndividual);
            _friendFlags = EFriendFlags.k_EFriendFlagImmediate | EFriendFlags.k_EFriendFlagBlocked;
            if ((_friendCount = SteamFriends.GetFriendCount(_friendFlags)) == 0)
                IsFinished = true;
        }

        /// <inheritdoc />
        public override void AdvanceToNextPage()
        {
            base.AdvanceToNextPage();

            var friend = SteamFriends.GetFriendByIndex(_friendIndex, _friendFlags);
            var friendId = friend.GetAccountID().m_AccountID;
            var personaState = SteamFriends.GetFriendPersonaState(friend);

            _currentPage = new SortedList<string, object>
            {
                {"Username", SteamFriends.GetFriendPersonaName(friend)},
                {"UserId", friendId},
                {"IsOnline", personaState != EPersonaState.k_EPersonaStateOffline},
                {"Status", (FriendStatus)personaState},
                {"AvatarUri", $"avatar://{friendId}"}
            };

            _friendIndex++;

            if (_friendIndex == _friendCount)
                IsFinished = true;
        }
    }
}
// ClanService.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Threading.Tasks;
using dEngine.Instances;
using dEngine.Instances.Attributes;
using Neo.IronLua;
using Steamworks;

namespace dEngine.Services
{
    /// <summary>
    /// A service for fetching information about clans.
    /// </summary>
    [TypeId(80)]
    public class ClanService : Service
    {
        internal static ClanService Service;

        /// <inheritdoc />
        public ClanService()
        {
            Service = this;
        }

        /// <summary>
        /// Returns a table containing information about the clan.
        /// </summary>
        /// <param name="clanId">The id of the clan.</param>
        [YieldFunction]
        public LuaTable GetClanInfoAsync(uint clanId)
        {
            var thread = ScriptService.CurrentThread;

            var table = new LuaTable();

            Task.Run(() =>
            {
                var steamId = SteamIdFromClanId(clanId);

                var owner = SteamFriends.GetClanOwner(steamId);

                table["Name"] = SteamFriends.GetClanName(steamId);
                table["Owner"] = new LuaTable
                {
                    ["Name"] = SteamFriends.GetFriendPersonaName(owner),
                    ["Id"] = owner.GetAccountID().m_AccountID
                };
                table["Tag"] = SteamFriends.GetClanTag(steamId);
                table["Roles"] = new LuaTable
                {
                    [1] = new LuaTable {[Name] = "Officer"}
                        [2] = new LuaTable {[Name] = "Moderator"}
                        [3] = new LuaTable {[Name] = "Member"}
                };
            }).ContinueWith(x => ScriptService.ResumeThread(thread));

            ScriptService.YieldThread();

            return table;
        }

        internal static CSteamID SteamIdFromClanId(uint userId)
        {
            return new CSteamID(new AccountID_t(userId), EUniverse.k_EUniversePublic,
                EAccountType.k_EAccountTypeClan);
        }

        internal static object GetExisting()
        {
            return DataModel.GetService<ClanService>();
        }
    }
}
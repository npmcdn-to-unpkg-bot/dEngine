// Teams.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections.Generic;
using System.Linq;
using dEngine.Instances;
using dEngine.Instances.Attributes;
using dEngine.Utility.Extensions;
using Neo.IronLua;

namespace dEngine.Services
{
    /// <summary>
    /// A service for managing teams.
    /// </summary>
    [TypeId(150)]
    [ToolboxGroup("Containers")]
    public class Teams : Service
    {
        internal static Teams Service;

        /// <summary />
        public Teams()
        {
            Service = this;
        }

        internal IEnumerable<Team> AllTeams => Children.OfType<Team>();

        private void RebalanceTeamsRandom()
        {
            var i = 0;

            var teams = AllTeams.ToArray();

            var newTeams =
                Players.Service.Children.OfType<Player>()
                    .Where(p => p.Team?.AutoAssignable == true)
                    .Split(teams.Length);

            foreach (var team in newTeams)
                foreach (var plr in team)
                    plr.Team = teams[i++];
        }

        private void RebalanceTeamsTrueSkill()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Rebalances players across teams.
        /// </summary>
        /// <param name="method"></param>
        public void RebalanceTeams(TeamBalanceMethod method = TeamBalanceMethod.Random)
        {
            switch (method)
            {
                case TeamBalanceMethod.Random:
                    RebalanceTeamsRandom();
                    break;
                case TeamBalanceMethod.TrueSkill:
                    RebalanceTeamsTrueSkill();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(method), method, null);
            }
        }

        /// <summary>
        /// Returns a list of existing teams.
        /// </summary>
        public LuaTable GetTeams()
        {
            var table = new LuaTable();
            var i = 0;
            foreach (var child in Children)
            {
                var team = child as Team;
                if (team != null)
                    table[++i] = team;
            }
            return table;
        }

        internal static Teams GetExisting()
        {
            return DataModel.GetService<Teams>();
        }
    }
}
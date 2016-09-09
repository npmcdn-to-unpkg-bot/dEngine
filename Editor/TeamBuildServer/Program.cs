// Program.cs - TeamBuildServer
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.IO;
using dEngine;
using dEngine.Instances;
using dEngine.Services.Networking;

namespace TeamBuildServer
{
    public class TeamBuild
    {
        public static int Port { get; private set; } = 32300;

        private static void Main(string[] args)
        {
        }

        public static void Start()
        {
            Engine.SaveGame = SaveGame;

            Engine.Start(EngineMode.Server, "dEditor");

            var networkServer = Game.NetworkServer = DataModel.GetService<NetworkServer>();
            networkServer.CustomMessageHandler = (typeId, message) => { return false; };
            networkServer.ConfigureAsTeamBuildServer(true);

            Game.DataModel.BindToClose(new Action(() =>
            {
                Game.DataModel.SaveGame(SaveFilter.SaveGame);
                Game.DataModel.SaveGame(SaveFilter.SaveWorld);
            }), 100000);

            //Game.Workspace.LoadPlace(project.Place);
        }

        private static void SaveGame(Stream stream)
        {
        }
    }
}
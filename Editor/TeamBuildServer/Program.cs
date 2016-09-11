// Program.cs - TeamBuildServer
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;
using dEngine;
using dEngine.Instances;
using dEngine.Services;
using dEngine.Services.Networking;

namespace TeamBuildServer
{
    public static class TeamBuild
    {
        private static Options _options;
        private static BlockingCollection<Tuple<string, ConsoleColor, ConsoleColor>> _logCollection;

        private static void Main(string[] args)
        {
            _logCollection = new BlockingCollection<Tuple<string, ConsoleColor, ConsoleColor>>();
            var parsed = Parser.Default.ParseArguments<Options>(args);
            parsed.WithNotParsed(NotParsed);
            parsed.WithParsed(Parsed);
        }

        private static void NotParsed(IEnumerable<Error> errors)
        {
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        private static void Parsed(Options options)
        {
            _options = options;
            if (!Directory.Exists(options.ProjectPath))
                Console.WriteLine("The provided project directory does not exist.");

            if (!File.Exists(options.ProjectFile))
                Console.WriteLine("The provided project directory does not contain a project file.");

            Start();
            while (true)
            {
                foreach (var log in _logCollection)
                {
                    Console.ForegroundColor = log.Item2;
                    Console.BackgroundColor = log.Item3;
                    Console.WriteLine(log.Item1);
                }
                _logCollection = new BlockingCollection<Tuple<string, ConsoleColor, ConsoleColor>>();
            }
        }

        public static void Start()
        {
            Engine.SaveGame = SaveGame;
        
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("// TeamBuildServer.exe ");
            Console.WriteLine("// Copyright © https://github.com/DanDevPC/");
            Console.WriteLine("// Licensed under the GNU GPL 3.0");
            Console.ForegroundColor = ConsoleColor.White;


            Console.WriteLine("Fire up the engine...");
            Engine.Start(EngineMode.Server, "dEditor");
            Console.WriteLine("Then it makes noise.");
            LogService.Service.MessageOutput.Connect(LogServiceOnMessageOutput);

            var networkServer = Game.NetworkServer = DataModel.GetService<NetworkServer>();
            networkServer.ConfigureAsTeamBuildServer(true);

            Game.DataModel.BindToClose(new Action(() =>
            {
                Game.DataModel.SaveGame(SaveFilter.SaveGame);
                Game.DataModel.SaveGame(SaveFilter.SaveWorld);
            }), 100000);

            Console.WriteLine($"Hosting project \"{_options.ProjectName}\" on port {_options.Port}");
            networkServer.Start(_options.Port);
        }

        private static void LogServiceOnMessageOutput(string msg, LogLevel level, string logger)
        {
            if (!logger.StartsWith("dEngine.Instances") &&
                  !logger.StartsWith("dEngine.Services") &&
                  !logger.StartsWith("I:"))
                return;

            var foreground = ConsoleColor.White;
            var background = ConsoleColor.Black;


            if (logger.StartsWith("I:Network"))
                foreground = ConsoleColor.Magenta;
            else
            {
                switch (level)
                {
                    case LogLevel.Fatal:
                    case LogLevel.Error:
                        background = ConsoleColor.Red;
                        foreground = ConsoleColor.White;
                        break;
                    case LogLevel.Warn:
                        foreground = ConsoleColor.Yellow;
                        break;
                    case LogLevel.Trace:
                        foreground = ConsoleColor.Blue;
                        break;
                    case LogLevel.Info:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(level), level, null);
                }
            }

            _logCollection.Add(new Tuple<string, ConsoleColor, ConsoleColor>(msg, foreground, background));
        }

        private static void SaveGame(Stream stream)
        {
        }
    }
}
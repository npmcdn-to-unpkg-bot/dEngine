// Options.cs - TeamBuildServer
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System.IO;
using CommandLine;

namespace TeamBuildServer
{
    public class Options
    {
        [Option('p', "project", Required = true, HelpText = "A path to the project folder.")]
        public string ProjectPath { get; set; }

        public string ProjectName => Path.GetFileNameWithoutExtension(ProjectPath);
        public string ProjectFile => Path.Combine(ProjectPath, ProjectName + ".dproj");

        [Option('k', "key", HelpText = "A key which clients require to access the server.")]
        public string AuthorizationKey { get; set; }

        [Option("port", Default = 3400, HelpText = "The port to run on.")]
        public int Port { get; set; }

    }
}
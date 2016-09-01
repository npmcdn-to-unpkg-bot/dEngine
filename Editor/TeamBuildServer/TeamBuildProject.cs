// TeamBuildProject.cs - TeamBuildServer
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
namespace TeamBuildServer
{
    public class TeamBuildProject
    {
        public TeamBuildProject(string name, uint appId, string place, string projectPath)
        {
            Name = name;
            AppId = appId;
            Place = place;
            ProjectPath = projectPath;
        }

        public string Name { get; }
        public uint AppId { get; }
        public string Place { get; }
        public string ProjectPath { get; }
    }
}
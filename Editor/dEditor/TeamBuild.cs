// TeamBuild.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Windows;

namespace dEditor
{
    public class TeamBuild
    {
        public static bool TryHost(bool skipDialog = false)
        {
            var result = true;

            if (!skipDialog)
                result = MessageBox.Show("Do you want to host a TeamBuild server?", "Host TeamBuild") ==
                         MessageBoxResult.Yes;

            return result;
        }
    }
}
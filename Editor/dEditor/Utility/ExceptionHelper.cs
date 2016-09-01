// ExceptionHelper.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Diagnostics;
using System.Windows;
using Ookii.Dialogs.Wpf;

namespace dEditor.Utility
{
    public static class ExceptionHelper
    {
        private static bool IsDialogOpen;

        public static void ShowCrashDialog(this Exception e)
        {
            if (IsDialogOpen)
                return;

            Editor.Logger.Error(e);

            e = e.InnerException ?? e;

            var dialog = new TaskDialog
            {
                WindowTitle = "dEditor",
                ButtonStyle = TaskDialogButtonStyle.CommandLinks,
                AllowDialogCancellation = false,
                MainInstruction = "dEditor has stopped working.",
                Content = "An unhandled exception was thrown, so the program needs to exit."
            };

            var restartButton = new TaskDialogButton("Restart the program");
            var closeButton = new TaskDialogButton("Close the program");

            dialog.Buttons.Add(restartButton);
            dialog.Buttons.Add(closeButton);

            dialog.ExpandFooterArea = true;

            dialog.ExpandedControlText = "Show exception details";
            dialog.ExpandedInformation = $"{e.Source} : {e.Message} {Environment.NewLine} {e.StackTrace}";


            IsDialogOpen = true;
            var resultButton = dialog.ShowDialog();
            IsDialogOpen = false;

            if (resultButton == restartButton)
            {
                Process.Start(Application.ResourceAssembly.Location);
                Environment.Exit(e.HResult);
            }
            else if (resultButton == closeButton)
            {
                Environment.Exit(e.HResult);
            }
        }
    }
}
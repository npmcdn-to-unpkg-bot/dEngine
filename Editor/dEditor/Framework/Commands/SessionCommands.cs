// SessionCommands.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Windows.Input;
using dEngine;
using dEngine.Graphics;
using dEngine.Services;
using Key = System.Windows.Input.Key;

namespace dEditor.Framework.Commands
{
    public abstract class SessionCommand : Command
    {
        protected SessionCommand()
        {
            Editor.Current.ProjectChanged += p => Editor.Current.Dispatcher.InvokeAsync(UpdateCanExecute);
            RunService.Service.SimulationStarted.Event +=
                () => Editor.Current.Dispatcher.InvokeAsync(UpdateCanExecute);
            RunService.Service.SimulationResumed.Event += () => Editor.Current.Dispatcher.InvokeAsync(UpdateCanExecute);
            RunService.Service.SimulationPaused.Event += () => Editor.Current.Dispatcher.InvokeAsync(UpdateCanExecute);
            RunService.Service.SimulationEnded.Event += () => Editor.Current.Dispatcher.InvokeAsync(UpdateCanExecute);
        }
    }

    public class PlaySoloCommand : SessionCommand
    {
        public override string Name => "Play";
        public override string Text => "Simulate the game as a player.";

        public override bool CanExecute(object parameter)
        {
            return (Editor.Current.Project != null) && (RunService.SimulationState != SimulationState.Running);
        }

        public override void Execute(object parameter)
        {
            Editor.PlaySoloScript.Run(LoginService.ProfileName, LoginService.UserId);
        }
    }

    public class RunCommand : SessionCommand
    {
        public override string Name => "Run";
        public override string Text => "Simulate the game without a player.";

        public override bool CanExecute(object parameter)
        {
            return (Editor.Current.Project != null) && (RunService.SimulationState != SimulationState.Running);
        }

        public override void Execute(object parameter)
        {
            RunService.Service.Run();
        }
    }

    public class PauseCommand : SessionCommand
    {
        public override string Name => "Pause";
        public override string Text => "Pauses the simulation.";

        public override bool CanExecute(object parameter)
        {
            return RunService.SimulationState == SimulationState.Running;
        }

        public override void Execute(object parameter)
        {
            RunService.Service.Pause();
        }
    }

    public class StopCommand : SessionCommand
    {
        public override string Name => "Stop";
        public override string Text => "Stops the simulation and resets the scene.";

        public override bool CanExecute(object parameter)
        {
            return RunService.SimulationState != SimulationState.Stopped;
        }

        public override void Execute(object parameter)
        {
            RunService.Service.Stop();
        }
    }
}
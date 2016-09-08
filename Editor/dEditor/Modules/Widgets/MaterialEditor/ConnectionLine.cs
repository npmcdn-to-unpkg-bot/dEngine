// ConnectionLine.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using System.Windows.Controls;
using dEngine;
using dEngine.Instances.Materials;

namespace dEditor.Modules.Widgets.MaterialEditor
{
    public class ConnectionLine : IDisposable
    {
        private readonly Canvas _canvas;
        private readonly BezierLine _bezier;
        private readonly Slot _input;
        private readonly Slot _output;

        public ConnectionLine(Canvas canvas, Slot input, Slot output)
        {
            _canvas = canvas;
            _bezier = new BezierLine();
            canvas.Children.Add(_bezier);
            _input = input;
            _output = output;
        }

        public void Dispose()
        {
            _canvas.Children.Remove(_bezier);
        }

        public void Update()
        {
            var inputPosition = _input.Node.Position + new Vector2(_input.Node.Size.x, 0);
            var outputPosition = _output.Node.Position + new Vector2(0, 0);

            _bezier.X1 = inputPosition.X;
            _bezier.Y1 = inputPosition.Y;

            _bezier.X2 = outputPosition.X;
            _bezier.Y2 = outputPosition.Y;
        }
    }
}
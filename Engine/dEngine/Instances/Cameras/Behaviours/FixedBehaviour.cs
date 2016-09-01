// FixedBehaviour.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections.Generic;
using dEngine.Settings.User;
using dEngine.Utility;
using SharpDX;

namespace dEngine.Instances
{
    internal class FixedBehaviour : Camera.Behaviour
    {
        private bool _focused;
        private bool _moveBackward;
        private bool _moveDown;

        private bool _moveForward;
        private bool _moveLeft;
        private bool _moveRight;
        private bool _moveUp;
        private bool _shiftHeld;

        public FixedBehaviour(Camera camera) : base(camera)
        {
        }

        protected override void OnKey(InputObject input)
        {
            base.OnKey(input);
            var held = input.InputState == InputState.Begin;

            switch (input.Key)
            {
                case Key.W:
                    _moveForward = held;
                    _focused = false;
                    break;
                case Key.S:
                    _moveBackward = held;
                    _focused = false;
                    break;
                case Key.A:
                    _moveLeft = held;
                    _focused = false;
                    break;
                case Key.D:
                    _moveRight = held;
                    _focused = false;
                    break;
                case Key.E:
                    _moveUp = held;
                    _focused = false;
                    break;
                case Key.Q:
                    _moveDown = held;
                    _focused = false;
                    break;
                case Key.LeftShift:
                    _shiftHeld = held;
                    break;
            }
        }

        internal override void Update(double step)
        {
            var dt = (float)step/(1/16.0f);
            var camera = _camera;

            var camCF = camera.CFrame;
            var focus = Vector3.Zero;

            if (_moveForward)
                focus += camCF.forward;
            else if (_moveBackward)
                focus += camCF.backward;

            if (_moveLeft)
                focus += camCF.left;
            else if (_moveRight)
                focus += camCF.right;

            if (_moveUp)
                focus += camCF.up;
            else if (_moveDown)
                focus += camCF.down;

            focus = focus*
                    dt*
                    (_shiftHeld
                        ? UserGameSettings.CameraShiftSpeed
                        : UserGameSettings.CameraSpeed) +
                    camera.Focus.p;

            var lookVector = camera.CFrame.lookVector;
            //Vector2.Multiply(ref _rotateInput, dt, out _rotateInput);
            RotateCamera(ref lookVector, ref _rotateInput, out lookVector);
            _rotateInput = Vector2.Zero;
            camera.Focus = new CFrame(focus);
            camera.CFrame = new CFrame(focus - lookVector*_zoom, focus);
        }


        internal void FocusOn(IEnumerable<Instance> instances)
        {
            //var boundingSphere = PVInstance.ComputeBoundingSphereFromCollection(instances);
            var boundingSphere = new BoundingSphere(SharpDX.Vector3.Zero, 5);

            var lookVector = _camera.CFrame.lookVector;
            var center = (Vector3)boundingSphere.Center;
            var zoom = Math.Abs(_camera.AspectRatio*boundingSphere.Radius/Mathf.Sin(_camera.FieldOfView/2));
            _camera.Focus = new CFrame(center - zoom*lookVector, center);

            _focused = true;
        }
    }
}
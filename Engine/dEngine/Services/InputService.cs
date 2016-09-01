// InputService.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Windows.Forms;
using dEngine.Data;
using dEngine.Graphics;
using dEngine.Instances;
using dEngine.Instances.Attributes;
using dEngine.Utility;
using dEngine.Utility.Native;
using Neo.IronLua;
using SharpDX.Multimedia;
using SharpDX.RawInput;

// ReSharper disable UnusedMember.Local

namespace dEngine.Services
{
    using static User32;

    /// <summary>
    /// A service for handling input.
    /// </summary>
    [TypeId(11)]
    [ExplorerOrder(-1)]
    public sealed class InputService : Service
    {
        private static ConcurrentQueue<RawInputEventArgs> _inputQueue;
        private static readonly ConcurrentDictionary<Key, byte> _heldKeys = new ConcurrentDictionary<Key, byte>();
        private static readonly bool[] _heldMouseButtons = new bool[6];

        private static GuiElement _focusedElement;
        private static GuiElement _lastMousedOver;

        private static readonly object _locker = new object();

        /// <summary>
        /// The service instance.
        /// </summary>
        public static InputService Service;

        private static Control _currentControl;

        internal static int CursorX;
        internal static int CursorY;

        private static Content<Texture> _cursor;
        private static int _deltaX;
        private static int _deltaY;
        private static MouseBehaviour _mouseBehaviour;
        private static Point _point;

        /// <inheritdoc />
        public InputService()
        {
            Service = this;

            PreviewInputBegan = new Signal<InputObject>(this);
            PreviewInputChanged = new Signal<InputObject>(this);
            PreviewInputEnded = new Signal<InputObject>(this);
            InputBegan = new Signal<InputObject>(this);
            InputChanged = new Signal<InputObject>(this);
            InputEnded = new Signal<InputObject>(this);
            WindowFocused = new Signal(this);
            WindowFocusReleased = new Signal(this);
            ViewportFocused = new Signal(this);
            ViewportFocusReleased = new Signal(this);

            KeyboardEnabled = true;
            MouseEnabled = true;

            _inputQueue = new ConcurrentQueue<RawInputEventArgs>();

            WindowFocusReleased.Event += () =>
            {
                var io = new InputObject(Key.Unknown, InputState.End, InputType.MouseButton2);
                Process(ref io);

                MouseBehaviour = MouseBehaviour.Default;
            };

            InputBegan.Event += OnMouseInputBegan;
            InputChanged.Event += OnMouseInputChanged;
            InputEnded.Event += OnMouseInputEnded;
        }

        /// <summary>
        /// The mouse behaviour.
        /// </summary>
        public MouseBehaviour MouseBehaviour
        {
            get { return _mouseBehaviour; }
            set
            {
                if (value == _mouseBehaviour) return;
                _mouseBehaviour = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// The icon to use for the custom cursor.
        /// </summary>
        public Content<Texture> CustomCursorIcon // TODO: custom cursors
        {
            get { return _cursor; }
            set
            {
                _cursor = value;
                value.Subscribe(this);
            }
        }

        /// <summary>
        /// Determines if an accelerometer is available on the current device.
        /// </summary>
        public bool AccelerometerEnabled { get; private set; }

        /// <summary>
        /// Determines if a gamepad is available on the current device.
        /// </summary>
        public bool GamepadEnabled { get; private set; }

        /// <summary>
        /// Determines if a gyroscope is available on the current device.
        /// </summary>
        public bool GyroscopeEnabled { get; private set; }

        /// <summary>
        /// Determines if a keyboard is available on the current device.
        /// </summary>
        public bool KeyboardEnabled { get; private set; }

        /// <summary>
        /// Determines if a mouse is available on the current device.
        /// </summary>
        public bool MouseEnabled { get; private set; }

        /// <summary>
        /// Determines if the current device has touch support.
        /// </summary>
        public bool TouchEnabled { get; private set; }

        /// <summary>
        /// Determines if a virtual reality headset is available on the current device.
        /// </summary>
        public bool VREnabled { get; private set; }

        internal static Control CurrentControl
        {
            get { return _currentControl; }
            set
            {
                if (value != null)
                    Hook(value.Handle);
                else if (_currentControl != null)
                    Unhook();

                _currentControl = value;
            }
        }

        /// <summary>
        /// The element which currently has input focus.
        /// </summary>
        internal static GuiElement FocusedElement
        {
            get { return _focusedElement; }
            set
            {
                var lastFocus = _focusedElement;
                if (lastFocus != null)
                {
                    lastFocus.Focused = false;
                    lastFocus.LostFocus.Fire();
                }
                _focusedElement = value;
                if (value != null)
                    value.Focused = true;
                _focusedElement?.GotFocus.Fire();
            }
        }

        internal static InputApi MouseInputApi { get; set; }

        internal static void Step()
        {
            var count = _inputQueue.Count;
            for (var i = 0; i < count; i++)
            {
                RawInputEventArgs args;
                KeyboardInputEventArgs rawKeyboard;
                MouseInputEventArgs rawMouse;
                if (!_inputQueue.TryDequeue(out args)) continue;
                if ((rawMouse = args as MouseInputEventArgs) != null)
                {
                    if (_currentControl == null)
                        return;

                    if ((rawMouse.X != 0) || (rawMouse.Y != 0))
                    {
                        if ((_mouseBehaviour == MouseBehaviour.Default) && (MouseInputApi == InputApi.RawInput))
                        {
                            CursorX += rawMouse.X; // todo: clamp
                            CursorY += rawMouse.Y;
                        }

                        var io = new InputObject(Key.Unknown, InputState.Change, InputType.MouseMovement,
                            new Vector3(CursorX, CursorY, rawMouse.WheelDelta),
                            new Vector3(rawMouse.X, rawMouse.Y, rawMouse.WheelDelta));
                        Process(ref io);
                    }

                    if (MouseInputApi != InputApi.RawInput)
                        return;
                    //if (!MouseWithinViewport(CursorX, CursorY) || (MouseInputApi != InputApi.RawInput))
                    // ignore if mouse action was outside the viewport
                    //return;

                    if (rawMouse.ButtonFlags != MouseButtonFlags.None)
                    {
                        InputType inputType;
                        InputState inputState;

                        switch (rawMouse.ButtonFlags)
                        {
                            case MouseButtonFlags.LeftButtonDown:
                                inputType = InputType.MouseButton1;
                                inputState = InputState.Begin;
                                _heldMouseButtons[1] = true;
                                break;
                            case MouseButtonFlags.LeftButtonUp:
                                inputType = InputType.MouseButton1;
                                inputState = InputState.End;
                                _heldMouseButtons[1] = false;
                                break;
                            case MouseButtonFlags.RightButtonDown:
                                inputType = InputType.MouseButton2;
                                inputState = InputState.Begin;
                                _heldMouseButtons[2] = true;
                                break;
                            case MouseButtonFlags.RightButtonUp:
                                inputType = InputType.MouseButton2;
                                inputState = InputState.End;
                                _heldMouseButtons[2] = false;
                                break;
                            case MouseButtonFlags.MiddleButtonDown:
                                inputType = InputType.MouseButton3;
                                inputState = InputState.Begin;
                                _heldMouseButtons[3] = true;
                                break;
                            case MouseButtonFlags.MiddleButtonUp:
                                inputType = InputType.MouseButton3;
                                inputState = InputState.End;
                                _heldMouseButtons[3] = false;
                                break;
                            case MouseButtonFlags.Button4Down:
                                inputType = InputType.MouseButton4;
                                inputState = InputState.Begin;
                                _heldMouseButtons[4] = true;
                                break;
                            case MouseButtonFlags.Button4Up:
                                inputType = InputType.MouseButton4;
                                inputState = InputState.End;
                                _heldMouseButtons[4] = false;
                                break;
                            case MouseButtonFlags.Button5Down:
                                inputType = InputType.MouseButton5;
                                inputState = InputState.Begin;
                                _heldMouseButtons[5] = true;
                                break;
                            case MouseButtonFlags.Button5Up:
                                inputType = InputType.MouseButton5;
                                inputState = InputState.End;
                                _heldMouseButtons[5] = false;
                                break;
                            case MouseButtonFlags.MouseWheel:
                                inputType = InputType.MouseWheel;
                                inputState = InputState.Change;
                                break;
                            default:
                                return;
                        }

                        var io = new InputObject(Key.Unknown, inputState, inputType,
                            new Vector3(CursorX, CursorY, rawMouse.WheelDelta),
                            new Vector3(rawMouse.X, rawMouse.Y, rawMouse.WheelDelta));
                        Process(ref io);
                    }
                }
                else if ((rawKeyboard = args as KeyboardInputEventArgs) != null)
                {
                    Key key;

                    if (rawKeyboard.Key == Keys.ControlKey)
                        if ((rawKeyboard.ScanCodeFlags == ScanCodeFlags.Make) ||
                            (rawKeyboard.ScanCodeFlags == ScanCodeFlags.Break))
                            key = Key.LeftControl;
                        else if (rawKeyboard.ScanCodeFlags.HasFlag(ScanCodeFlags.E0))
                            key = Key.RightControl;
                        else
                            throw new ArgumentOutOfRangeException(nameof(rawKeyboard.ScanCodeFlags),
                                $"ScanCodeFlag {rawKeyboard.ScanCodeFlags} was out of range");
                    else if (rawKeyboard.Key == Keys.ShiftKey)
                        key = rawKeyboard.MakeCode == 42 ? Key.LeftShift : Key.RightShift;
                    else if (rawKeyboard.Key == Keys.Menu)
                        if ((rawKeyboard.ScanCodeFlags == ScanCodeFlags.Make) ||
                            (rawKeyboard.ScanCodeFlags == ScanCodeFlags.Break))
                            key = Key.LeftAlt;
                        else if (rawKeyboard.ScanCodeFlags.HasFlag(ScanCodeFlags.E0))
                            key = Key.RightAlt;
                        else
                            throw new ArgumentOutOfRangeException(nameof(rawKeyboard.ScanCodeFlags),
                                $"ScanCodeFlag {rawKeyboard.ScanCodeFlags} was out of range");
                    else
                        key = RawKeyToKey(rawKeyboard.Key);

                    InputState state;

                    switch (rawKeyboard.State)
                    {
                        case KeyState.SystemKeyDown:
                        case KeyState.KeyDown:
                            state = InputState.Begin;
                            if (!_heldKeys.TryAdd(key))
                                return;
                            break;
                        case KeyState.SystemKeyUp:
                        case KeyState.KeyUp:
                            state = InputState.End;
                            lock (_locker)
                            {
                                if (!_heldKeys.TryRemove(key))
                                {
                                    /*
                                    keyboardInput.State = KeyState.KeyDown;
                                    OnRawKeyboardInput(this, keyboardInput);
                                    keyboardInput.State = KeyState.KeyUp;
                                    OnRawKeyboardInput(this, keyboardInput);
                                    return;
                                    */
                                }
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(rawKeyboard.State), "Unknown keyboard state.");
                    }

                    var io = new InputObject(key, state, InputType.Keyboard);
                    Process(ref io);
                }
            }
        }

        private static Key RawKeyToKey(Keys rawKey)
        {
            switch (rawKey)
            {
                case Keys.None:
                    return Key.None;
                case Keys.Back:
                    return Key.Backspace;
                case Keys.Tab:
                    return Key.Tab;
                case Keys.Return:
                    return Key.Enter;
                case Keys.ShiftKey:
                    return Key.LeftShift;
                case Keys.ControlKey:
                    return Key.LeftControl;
                case Keys.Menu:
                    return Key.Menu;
                case Keys.Pause:
                    return Key.Pause;
                case Keys.Capital:
                    return Key.CapsLock;
                case Keys.Escape:
                    return Key.Escape;
                case Keys.Space:
                    return Key.Space;
                case Keys.End:
                    return Key.End;
                case Keys.Home:
                    return Key.Home;
                case Keys.Left:
                    return Key.Left;
                case Keys.Up:
                    return Key.Up;
                case Keys.Right:
                    return Key.Right;
                case Keys.Down:
                    return Key.Down;
                case Keys.Print:
                    return Key.PrintScreen;
                case Keys.Insert:
                    return Key.Insert;
                case Keys.Delete:
                    return Key.Delete;
                case Keys.D0:
                    return Key.Num0;
                case Keys.D1:
                    return Key.Num1;
                case Keys.D2:
                    return Key.Num2;
                case Keys.D3:
                    return Key.Num3;
                case Keys.D4:
                    return Key.Num4;
                case Keys.D5:
                    return Key.Num5;
                case Keys.D6:
                    return Key.Num6;
                case Keys.D7:
                    return Key.Num7;
                case Keys.D8:
                    return Key.Num8;
                case Keys.D9:
                    return Key.Num9;
                case Keys.A:
                    return Key.A;
                case Keys.B:
                    return Key.B;
                case Keys.C:
                    return Key.C;
                case Keys.D:
                    return Key.D;
                case Keys.E:
                    return Key.E;
                case Keys.F:
                    return Key.F;
                case Keys.G:
                    return Key.G;
                case Keys.H:
                    return Key.H;
                case Keys.I:
                    return Key.I;
                case Keys.J:
                    return Key.J;
                case Keys.K:
                    return Key.K;
                case Keys.L:
                    return Key.L;
                case Keys.M:
                    return Key.M;
                case Keys.N:
                    return Key.N;
                case Keys.O:
                    return Key.O;
                case Keys.P:
                    return Key.P;
                case Keys.Q:
                    return Key.Q;
                case Keys.R:
                    return Key.R;
                case Keys.S:
                    return Key.S;
                case Keys.T:
                    return Key.T;
                case Keys.U:
                    return Key.U;
                case Keys.V:
                    return Key.V;
                case Keys.W:
                    return Key.W;
                case Keys.X:
                    return Key.X;
                case Keys.Y:
                    return Key.Y;
                case Keys.Z:
                    return Key.Z;
                case Keys.LWin:
                case Keys.RWin:
                    return Key.Win;
                case Keys.NumPad0:
                    return Key.Num0;
                case Keys.NumPad1:
                    return Key.Num1;
                case Keys.NumPad2:
                    return Key.Num2;
                case Keys.NumPad3:
                    return Key.Num3;
                case Keys.NumPad4:
                    return Key.Num4;
                case Keys.NumPad5:
                    return Key.Num5;
                case Keys.NumPad6:
                    return Key.Num6;
                case Keys.NumPad7:
                    return Key.Num7;
                case Keys.NumPad8:
                    return Key.Num8;
                case Keys.NumPad9:
                    return Key.Num9;
                case Keys.Add:
                    return Key.Equals;
                case Keys.Subtract:
                    return Key.Minus;
                case Keys.F1:
                    return Key.F1;
                case Keys.F2:
                    return Key.F2;
                case Keys.F3:
                    return Key.F3;
                case Keys.F4:
                    return Key.F4;
                case Keys.F5:
                    return Key.F5;
                case Keys.F6:
                    return Key.F6;
                case Keys.F7:
                    return Key.F7;
                case Keys.F8:
                    return Key.F8;
                case Keys.F9:
                    return Key.F9;
                case Keys.F10:
                    return Key.F10;
                case Keys.F11:
                    return Key.F11;
                case Keys.F12:
                    return Key.F12;
                case Keys.NumLock:
                    return Key.NumLock;
                case Keys.Scroll:
                    return Key.ScrollLock;
                case Keys.LShiftKey:
                    return Key.LeftShift;
                case Keys.RShiftKey:
                    return Key.RightShift;
                case Keys.LControlKey:
                    return Key.LeftControl;
                case Keys.RControlKey:
                    return Key.RightControl;
                case Keys.LMenu:
                    return Key.Menu;
                case Keys.RMenu:
                    return Key.Menu;
                case Keys.OemSemicolon:
                    return Key.Semicolon;
                case Keys.Oemplus:
                    return Key.Equals;
                case Keys.Oemcomma:
                    return Key.Comma;
                case Keys.OemMinus:
                    return Key.Minus;
                case Keys.OemPeriod:
                    return Key.FullStop;
                case Keys.OemQuestion:
                    return Key.Slash;
                case Keys.Oemtilde:
                    return Key.Hash;
                case Keys.OemOpenBrackets:
                    return Key.LeftBracket;
                case Keys.OemPipe:
                    return Key.Backslash;
                case Keys.OemCloseBrackets:
                    return Key.RightBracket;
                case Keys.OemQuotes:
                    return Key.Apostrophe;
                case Keys.OemBackslash:
                    return Key.Backslash;
                case Keys.Shift:
                    return Key.LeftShift;
                case Keys.Control:
                    return Key.LeftControl;
                case Keys.Alt:
                    return Key.LeftAlt;
                default:
                    return Key.Unknown;
            }
        }

        private static void Hook(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
                return;

            var control = Control.FromHandle(handle);

            Device.RegisterDevice(UsagePage.Generic, UsageId.GenericMouse, DeviceFlags.None, handle);
            Device.RegisterDevice(UsagePage.Generic, UsageId.GenericKeyboard, DeviceFlags.None, handle);

            Device.MouseInput += DeviceOnRawInput;
            Device.KeyboardInput += DeviceOnRawInput;

            control.MouseMove += ControlOnMouseMove;
            control.MouseDown += ControlOnMouseDown;
            control.MouseUp += ControlOnMouseUp;
            control.MouseWheel += ControlOnMouseWheel;
        }

        private static void DeviceOnRawInput(object sender, RawInputEventArgs input)
        {
            _inputQueue.Enqueue(input);
        }

        private static void Unhook()
        {
            Device.RawInput -= DeviceOnRawInput;
        }

        private static InputType MouseButtonToInputType(MouseButtons buttons)
        {
            switch (buttons)
            {
                case MouseButtons.None:
                    return InputType.None;
                case MouseButtons.Left:
                    return InputType.MouseButton1;
                case MouseButtons.Right:
                    return InputType.MouseButton2;
                case MouseButtons.Middle:
                    return InputType.MouseButton3;
                case MouseButtons.XButton1:
                    return InputType.MouseButton4;
                case MouseButtons.XButton2:
                    return InputType.MouseButton5;
                default:
                    throw new ArgumentOutOfRangeException(nameof(buttons), buttons, null);
            }
        }

        private static void ControlOnMouseWheel(object sender, MouseEventArgs args)
        {
            if (MouseInputApi != InputApi.Windows)
                return;

            var io = new InputObject(Key.Unknown, InputState.Change, InputType.MouseWheel,
                new Vector3(CursorX, CursorY, args.Delta), new Vector3(_deltaX, _deltaY, args.Delta));
            Process(ref io);
        }

        private static void ControlOnMouseDown(object sender, MouseEventArgs args)
        {
            if (MouseInputApi != InputApi.Windows)
                return;

            var inputType = MouseButtonToInputType(args.Button);

            switch (inputType)
            {
                case InputType.MouseButton1:
                    _heldMouseButtons[0] = true;
                    break;
                case InputType.MouseButton2:
                    _heldMouseButtons[1] = true;
                    break;
                case InputType.MouseButton3:
                    _heldMouseButtons[2] = true;
                    break;
                case InputType.MouseButton4:
                    _heldMouseButtons[3] = true;
                    break;
                case InputType.MouseButton5:
                    _heldMouseButtons[5] = true;
                    break;
            }

            var io = new InputObject(Key.Unknown, InputState.Begin, inputType, new Vector3(CursorX, CursorY, args.Delta),
                new Vector3(_deltaX, _deltaY, args.Delta));
            Process(ref io);
        }

        private static void ControlOnMouseUp(object sender, MouseEventArgs args)
        {
            var control = CurrentControl;

            if (control == null)
                return;

            if (MouseInputApi != InputApi.Windows)
                return;

            var inputType = MouseButtonToInputType(args.Button);

            switch (inputType)
            {
                case InputType.MouseButton1:
                    _heldMouseButtons[0] = false;
                    break;
                case InputType.MouseButton2:
                    _heldMouseButtons[1] = false;
                    break;
                case InputType.MouseButton3:
                    _heldMouseButtons[2] = false;
                    break;
                case InputType.MouseButton4:
                    _heldMouseButtons[3] = false;
                    break;
                case InputType.MouseButton5:
                    _heldMouseButtons[5] = false;
                    break;
            }

            var io = new InputObject(Key.Unknown, InputState.End, inputType, new Vector3(CursorX, CursorY, args.Delta),
                new Vector3(_deltaX, _deltaY, args.Delta));
            Process(ref io);
        }

        private static void ControlOnMouseMove(object sender, MouseEventArgs args)
        {
            switch (_mouseBehaviour)
            {
                case MouseBehaviour.LockCurrentPosition:
                    _point.X = CursorX;
                    _point.Y = CursorY;
                    ClientToScreen(_currentControl.Handle, ref _point);
                    SetCursorPos(_point.X, _point.Y);
                    break;
                case MouseBehaviour.LockCenter:
                    CursorX = _point.X = (int)(Renderer.ControlSize.x/2);
                    CursorY = _point.Y = (int)(Renderer.ControlSize.y/2);
                    ClientToScreen(_currentControl.Handle, ref _point);
                    SetCursorPos(_point.X, _point.Y);
                    break;
                case MouseBehaviour.Default:
                    _deltaX = args.X - CursorX;
                    _deltaY = args.Y - CursorY;
                    CursorX = args.X;
                    CursorY = args.Y;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(MouseBehaviour));
            }

            /*
			if (_deltaX != 0 || _deltaY != 0)
			{
				var io = new InputObject(Key.Unknown, InputState.Change, InputType.MouseMovement, new Vector3(CursorX, CursorY, args.Delta), new Vector3(_deltaX, _deltaY, args.Delta));
				Process(ref io);
			}
			*/
        }

        private static bool MouseWithinViewport(int x, int y)
        {
            var size = Game.FocusedCamera.ViewportSize;
            var pos = CurrentControl.PointToClient(new Point(x, y));
            return (pos.X > 0) && (pos.X < size.x) && (pos.Y > 0) && (pos.Y < size.y);
        }

        private void OnRawMouseInput(object sender, MouseInputEventArgs args)
        {
            lock (Locker)
            {
            }
        }

        internal static object GetExisting()
        {
            return DataModel.GetService<InputService>();
        }

        /// <summary>
        /// Determines whether the given gamepad supports the given key.
        /// </summary>
        public bool DoesGamepadSupport(InputType gamepadNum, Key keyCode)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a list of connected gamepads.
        /// </summary>
        public LuaTable GetConnectedGamepads(InputType gamepadNum, Key keyCode)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines if the given gamepad is currently connected.
        /// </summary>
        public bool IsGamepadConnected(InputType gamepadNum)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Returns the <see cref="InputState" /> of the given gamepad.
        /// </summary>
        public InputState GetGamepadState(InputType gamepadNum)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an array of currently pressed keys.
        /// </summary>
        /// <returns></returns>
        public LuaTable GetPressedKeys()
        {
            lock (_locker)
            {
                return _heldKeys.ToLuaTable();
            }
        }

        /// <summary>
        /// Returns true if the given key is held down.
        /// </summary>
        public bool IsKeyDown(Key key)
        {
            return _heldKeys.ContainsKey(key);
        }

        /// <summary>
        /// Returns true if the mouse button for the given number is held down.
        /// </summary>
        /// <param name="i">The MouseButton number, in the range of 1-4.</param>
        /// <returns></returns>
        public bool IsMouseButtonDown(int i)
        {
            if ((i < 1) || (i > 5))
                throw new ArgumentOutOfRangeException(nameof(i), "");
            return _heldMouseButtons[i - 1];
        }

        internal static void Process(ref InputObject obj)
        {
            switch (obj.InputState)
            {
                case InputState.Begin:
                    Service.InputBegan.Fire(obj);
                    break;
                case InputState.Change:
                    Service.InputChanged.Fire(obj);
                    break;
                case InputState.End:
                    Service.InputEnded.Fire(obj);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnMouseInputBegan(InputObject input)
        {
            var hitElement = GlobalHitTest();
            FocusedElement = hitElement;

            switch (input.InputType)
            {
                case InputType.MouseButton1:
                    hitElement?.Focus();
                    hitElement?.MouseButton1Down.Fire((int)input.Position.X, (int)input.Position.Y);
                    break;
                case InputType.MouseButton2:
                    hitElement?.MouseButton2Down.Fire((int)input.Position.X, (int)input.Position.Y);
                    break;
            }
        }

        private void OnMouseInputEnded(InputObject input)
        {
            var hitElement = GlobalHitTest();

            if (input.InputType == InputType.MouseButton1)
                hitElement?.MouseButton1Up.Fire((int)input.Position.X, (int)input.Position.Y);
            else if (input.InputType == InputType.MouseButton2)
                hitElement?.MouseButton2Up.Fire((int)input.Position.X, (int)input.Position.Y);
        }

        private void OnMouseInputChanged(InputObject input)
        {
            var x = (int)input.Position.X;
            var y = (int)input.Position.Y;

            var hitElement = GlobalHitTest();

            switch (input.InputType)
            {
                case InputType.MouseWheel:
                    hitElement?.MouseWheel.Fire(x, y, input.Delta.Z);
                    break;
                case InputType.MouseMovement:
                    if (hitElement != _lastMousedOver)
                    {
                        _lastMousedOver?.MouseLeave.Fire(x, y);
                        hitElement?.MouseEnter.Fire(x, y);
                        _lastMousedOver = hitElement;
                    }
                    break;
            }
        }

        internal static bool IsKeyDownInternal(Key key)
        {
            return _heldKeys.ContainsKey(key);
        }

        private static GuiElement GlobalHitTest()
        {
            var camera = Game.FocusedCamera;

            GuiElement hitElement = null;

            if (camera != null)
                lock (camera.Locker)
                {
                    var collectors = camera.LayerCollectors;
                    var count = collectors.Count;
                    for (var i = 0; i < count; i++)
                    {
                        collectors[i].HitTest(CursorX, CursorY, out hitElement);
                        if (hitElement != null)
                            break;
                    }
                }

            return hitElement;
        }

        internal readonly Signal<InputObject> PreviewInputBegan;
        internal readonly Signal<InputObject> PreviewInputChanged;
        internal readonly Signal<InputObject> PreviewInputEnded;

        /// <summary>
        /// Fires when an input begins. (Mouse button pressed, key pressed, etc.)
        /// </summary>
        /// <eventParam name="inputObject" />
        /// <eventParam name="gameProcessedEvent" />
        public readonly Signal<InputObject> InputBegan;

        /// <summary>
        /// Fires when an input changes. (Mouse move, scroll wheel, etc.)
        /// </summary>
        /// <eventParam name="inputObject" />
        /// <eventParam name="gameProcessedEvent" />
        public readonly Signal<InputObject> InputChanged;

        /// <summary>
        /// Fires when an input ends. (Mouse button released, key released etc)
        /// </summary>
        /// <eventParam name="inputObject" />
        /// <eventParam name="gameProcessedEvent" />
        public readonly Signal<InputObject> InputEnded;

        /// <summary>
        /// Fires when the game window is focused.
        /// </summary>
        public readonly Signal WindowFocused;

        /// <summary>
        /// Fires when the game window is unfocused.
        /// </summary>
        public readonly Signal WindowFocusReleased;

        /// <summary>
        /// Fires when the viewport is focused.
        /// </summary>
        public readonly Signal ViewportFocused;

        /// <summary>
        /// Fires when the viewport is unfocused.
        /// </summary>
        public readonly Signal ViewportFocusReleased;
    }
}
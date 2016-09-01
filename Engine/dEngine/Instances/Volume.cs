// Volume.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Linq;
using BulletSharp;
using BulletSharp.Math;
using dEngine.Instances.Attributes;
using dEngine.Utility;
using Neo.IronLua;

namespace dEngine.Instances
{
    /// <summary>
    /// A volume represents an area of space which can detect when a player enters it.
    /// </summary>
    [TypeId(199)]
    public class Volume : PVInstance
    {
        private readonly GhostObject _ghostObject;
        private CFrame _cframe;
        private Vector3 _size;

        private Shape _shape;

        /// <summary />
        public Volume()
        {
            _ghostObject = new PairCachingGhostObject {CollisionFlags = CollisionFlags.NoContactResponse};
        }

        /// <summary>
        /// The position/orientation of the volume.
        /// </summary>
        [InstMember(1)]
        public override CFrame CFrame
        {
            get { return _cframe; }
            set
            {
                if (value == _cframe) return;
                _cframe = value;
                Update();
                NotifyChanged();
            }
        }

        /// <summary>
        /// The position of <see cref="CFrame" />.
        /// </summary>
        [EditorVisible]
        public Vector3 Position
        {
            get { return _cframe.p; }
            set
            {
                if (value == _cframe.p) return;
                var rotation = _cframe - _cframe.p;
                CFrame = new CFrame(value)*rotation;
            }
        }

        /// <summary>
        /// The rotation of <see cref="CFrame" />.
        /// </summary>
        [EditorVisible]
        public Vector3 Rotation
        {
            get { return _cframe.getEulerAngles()*Mathf.Rad2Deg; }
            set
            {
                if (value == Rotation) return;
                var v3 = value*Mathf.Deg2Rad;
                var rotation = CFrame.Angles(v3.x, v3.y, v3.z);
                CFrame = new CFrame(_cframe.p)*rotation;
            }
        }

        /// <summary>
        /// The size of the volume.
        /// </summary>
        [InstMember(2)]
        [EditorVisible]
        public override Vector3 Size
        {
            get { return _size; }
            set
            {
                if (value == _size) return;
                _size = value;
                Update();
                NotifyChanged();
            }
        }

        /// <summary>
        /// Summary
        /// </summary>
        [InstMember(3)]
        [EditorVisible]
        public Shape Shape
        {
            get { return _shape; }
            set
            {
                if (value == _shape) return;
                _shape = value;
                NotifyChanged();
            }
        }

        /// <inheritdoc />
        public override void Destroy()
        {
            base.Destroy();
            _ghostObject?.Dispose();
        }

        /// <summary />
        protected override void OnWorldChanged(IWorld newWorld, IWorld oldWorld)
        {
            base.OnWorldChanged(newWorld, oldWorld);
            lock (PhysicsSimulation.Locker)
            {
                oldWorld?.Physics?.World.RemoveCollisionObject(_ghostObject);
                newWorld?.Physics?.World.AddCollisionObject(_ghostObject);
            }
        }

        public LuaTable GetOverlappingParts()
        {
            var table = new LuaTable();
            var pairs = _ghostObject.OverlappingPairs;

            return pairs.Select(pair => pair.UserObject).ToLuaTable();
        }

        private void Update()
        {
            _ghostObject.WorldTransform = (Matrix)_cframe;

            OnWorldChanged(null, World);

            CollisionShape collisionShape;

            switch (_shape)
            {
                case Shape.Cube:
                    collisionShape = new BoxShape(_size.x/2, _size.y/2, _size.z/2);
                    break;
                case Shape.Sphere:
                    var radius = Math.Min(_size.Y, Math.Min(_size.X, _size.Z))/2;
                    collisionShape = new SphereShape(radius);
                    break;
                case Shape.Cylinder:
                    var radiusCyl = Math.Min(_size.X, _size.Z)/2;
                    collisionShape = new CylinderShape(radiusCyl, _size.Y, radiusCyl);
                    break;
                case Shape.Wedge:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _ghostObject.CollisionShape = collisionShape;
            _ghostObject.CollisionFlags = CollisionFlags.CharacterObject;

            OnWorldChanged(World, null);
        }

        public static readonly Signal<Part> Entered;

        public static readonly Signal<Part> Left;
    }
}
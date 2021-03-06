﻿// BaseHandles.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections.Concurrent;
using dEngine.Instances.Attributes;
using dEngine.Instances.Interfaces;
using dEngine.Utility;

namespace dEngine.Instances
{
    /// <summary>
    /// An abstract class for drawing handles around the <see cref="PVAdornment.Adornee" />.
    /// </summary>
    [TypeId(164)]
    public abstract class BaseHandles : PVAdornment
    {
        /*
        private PVInstance _lastAdornee;

        private void OnHandlesChanged(string propertyName)
        {
            if (propertyName != nameof(Adornee))
                return;

            if (_lastAdornee != null)
            {
                _lastAdornee.Changed.Event -= OnPropertyOfAdorneeChanged;
            }

            if (Adornee != null)
            {
                Adornee.Changed.Event += OnPropertyOfAdorneeChanged;
            }

            _lastAdornee = Adornee;
            UpdateHandle();
        }

        /// <summary>
        /// Called when a property of the current Adornee is changed.
        /// </summary>
        private void OnPropertyOfAdorneeChanged(string propertyName)
        {
            if (propertyName == nameof(Adornee.PVCFrame) || propertyName == nameof(Adornee.PVSize))
            {
                UpdateHandle();
            }
        }
        */

        /// <summary>
        /// Determines whether any handles are blocking the mouse.
        /// </summary>
        internal static ConcurrentDictionary<BaseHandles, byte> HeldHandles { get; set; } =
            new ConcurrentDictionary<BaseHandles, byte>();

        internal static bool IsMouseOverHandle => HeldHandles.Count > 0;

        /// <summary/>
        protected override void OnAncestryChanged(Instance child, Instance parent)
        {
            base.OnAncestryChanged(child, parent);

            if (child == this)
                ChangeRenderObject();
        }


        internal void GetDistanceToNormals(float rel, float mul, ref CFrame adorneeCFrame, ref Vector3 adorneeSize,
            ref float[] distances)
        {
            var adorneePos = adorneeCFrame.p;
            var camPos = ((ICameraUser)this).Camera.CFrame.p;

            for (var i = 0; i < 6; i++)
            {
                float dist;

                var id = (NormalId)i;
                switch (id)
                {
                    case NormalId.Left:
                        dist = (camPos - (adorneePos + adorneeCFrame.left*adorneeSize.X/2)).mag2;
                        break;
                    case NormalId.Right:
                        dist = (camPos - (adorneePos + adorneeCFrame.right*adorneeSize.X/2)).mag2;
                        break;
                    case NormalId.Top:
                        dist = (camPos - (adorneePos + adorneeCFrame.up*adorneeSize.Y/2)).mag2;
                        break;
                    case NormalId.Bottom:
                        dist = (camPos - (adorneePos + adorneeCFrame.down*adorneeSize.Y/2)).mag2;
                        break;
                    case NormalId.Front:
                        dist = (camPos - (adorneePos + adorneeCFrame.forward*adorneeSize.Z/2)).mag2;
                        break;
                    case NormalId.Back:
                        dist = (camPos - (adorneePos + adorneeCFrame.backward*adorneeSize.Z/2)).mag2;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                distances[i] = Math.Max(1, Mathf.Sqrt(dist)/10*0.6f);
            }
        }

        /// <summary/>
        protected virtual void ChangeRenderObject()
        {
        }
    }
}
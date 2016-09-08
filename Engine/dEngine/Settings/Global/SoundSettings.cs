// SoundSettings.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Runtime.CompilerServices;
using dEngine.Instances.Attributes;

namespace dEngine.Settings.Global
{
    /// <summary>
    /// Settings for the sound engine.
    /// </summary>
    [TypeId(201)]
    public class SoundSettings : Settings
    {
        private static int _maxActiveSoundCount;

        private static int _sampleRate;
        private static bool _decodeFec;

        /// <summary>
        /// The maximum number of sounds that can be played at once.
        /// </summary>
        [EditorVisible("Sound", "Maximum Active Sounds")]
        public static int MaxActiveSoundCount
        {
            get { return _maxActiveSoundCount; }
            set
            {
                if (value == _maxActiveSoundCount) return;
                _maxActiveSoundCount = Math.Max(0, value);
                NotifyChangedStatic();
            }
        }

        /// <summary>
        /// The output sample rate. If set to zero, the default system sample rate will be used.
        /// </summary>
        [EditorVisible("Sound", "Sample Rate")]
        public static int SampleRate
        {
            get { return _sampleRate; }
            set
            {
                if (value == _sampleRate) return;
                _sampleRate = value;
                NotifyChangedStatic();
            }
        }

        internal static bool DecodeFec
        {
            get { return _decodeFec; }
            set
            {
                if (value == _decodeFec) return;
                _decodeFec = value;
                NotifyChangedStatic();
            }
        }

        private static void NotifyChangedStatic([CallerMemberName] string propertyName = null)
        {
            Engine.GlobalSettings?.SoundSettings?.NotifyChanged(propertyName);
        }

        /// <inheritdoc />
        public override void RestoreDefaults()
        {
            MaxActiveSoundCount = 500;
            SampleRate = 0;
        }
    }
}
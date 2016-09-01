// NVAPI.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Runtime.InteropServices;

namespace dEngine.Utility
{
    internal static class NvApi
    {
        public const int MaxPhysicalGpus = 64;
        public const int MaxUsagesPerGpu = 34;

        internal static int GpuCount;
        internal static IntPtr[] GpuHandles = new IntPtr[MaxPhysicalGpus];

        static unsafe NvApi()
        {
            Initialize = Marshal.GetDelegateForFunctionPointer<NvAPI_Initialize>(QueryInterface(0x0150E828));
            EnumPhysicalGpus = Marshal.GetDelegateForFunctionPointer<NvAPI_EnumPhysicalGPUs>(QueryInterface(0xE5AC921F));
            GetThermalSettings =
                Marshal.GetDelegateForFunctionPointer<NvAPI_GPU_GetThermalSettings>(QueryInterface(0xE3640A56));
            Initialize();
            UpdateGpuHandles();
        }

        internal static NvAPI_Initialize Initialize { get; }
        internal static NvAPI_EnumPhysicalGPUs EnumPhysicalGpus { get; }
        internal static NvAPI_GPU_GetThermalSettings GetThermalSettings { get; }

        internal static unsafe void UpdateGpuHandles()
        {
            var ptr = (int*)Marshal.AllocHGlobal(MaxPhysicalGpus*Marshal.SizeOf(typeof(IntPtr)));
            var ptr2 = (int*)Marshal.AllocHGlobal(4);
            var test = 0;

            EnumPhysicalGpus(ref ptr, ref test);

            var temp = new ThermalSettings();
            GetThermalSettings(IntPtr.Zero, 0, ref temp);

            for (var i = 0; i < (int)ptr2; i++)
                GpuHandles[i] = Marshal.ReadIntPtr((IntPtr)ptr, IntPtr.Size*i);
        }

        [DllImport("nvapi64.dll", EntryPoint = "nvapi_QueryInterface")]
        internal static extern IntPtr QueryInterface(uint offset);

        internal delegate int NvAPI_Initialize();

        internal unsafe delegate int NvAPI_EnumPhysicalGPUs(ref int* handles, ref int count);

        internal delegate int NvAPI_GPU_GetThermalSettings(IntPtr handle, int sensorIndex, ref ThermalSettings temp);

        [StructLayout(LayoutKind.Sequential, Size = 17)]
        public class Sensor
        {
            /// <summary>
            /// ADM1032, MAX6649...
            /// </summary>
            public int Controller;

            /// <summary>
            /// The current temperature value of the thermal sensor in degree Celsius
            /// </summary>
            public int CurrentTemp;

            /// <summary>
            /// The max default temperature value of the thermal sensor in degree Celsius
            /// </summary>
            public int DefaultMaxTemp;

            /// <summary>
            /// The min default temperature value of the thermal sensor in degree Celsius
            /// </summary>
            public int DefaultMinTemp;

            /// <summary>
            /// Thermal sensor targeted @ GPU, memory, chipset, powersupply, Visual Computing Device, etc
            /// </summary>
            public ThermalTarget Target;
        }

        [StructLayout(LayoutKind.Explicit, Size = 8 + 17*MaxPhysicalGpus)]
        public class ThermalSettings
        {
            [FieldOffset(4)] public int Count;
            [FieldOffset(8)] public Sensor[] Sensors;
            [FieldOffset(0)] public int Version;
        }

        public enum ThermalTarget
        {
            None = 0,
            Gpu = 1,
            Memory = 2,
            PowerSupply = 4,
            Board = 8,
            VcdBoard = 9,
            VcdInlet = 10,
            VcdOutlet = 11,
            All = 15,
            Unknown = -1
        }
    }
}
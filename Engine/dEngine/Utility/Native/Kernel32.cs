// Kernel32.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Runtime.InteropServices;

namespace dEngine.Utility.Native
{
    internal static class Kernel32
    {
        [DllImport("kernel32.dll")]
        public static extern void OutputDebugString(string outputString);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string moduleName);

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SetDllDirectory(string pathName);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool QueryProcessCycleTime(IntPtr ProcessHandle, out ulong CycleTime);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibraryEx(string fileName, IntPtr reservedNull, LoadLibraryFlags flags);

        [Flags]
        public enum LoadLibraryFlags : uint
        {
            DontResolveDllReferences = 0x00000001,
            LoadIgnoreCodeAuthzLevel = 0x00000010,
            LoadLibraryAsDatafile = 0x00000002,
            LoadLibraryAsDatafileExclusive = 0x00000040,
            LoadLibraryAsImageResource = 0x00000020,
            LoadWithAlteredSearchPath = 0x00000008
        }
    }
}
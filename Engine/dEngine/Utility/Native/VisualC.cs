// VisualC.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Runtime.InteropServices;

namespace dEngine.Utility.Native
{
    internal static class VisualC
    {
        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CharSet = CharSet.Ansi,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern int CopyMemory(IntPtr destination, IntPtr source, int length);

        [DllImport("msvcrt.dll", EntryPoint = "memcmp", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CompareMemory(char[] b1, char[] b2, long count);

        [DllImport("msvcrt.dll", EntryPoint = "memcmp", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CompareMemory(byte[] b1, char[] b2, long count);

        [DllImport("msvcrt.dll", EntryPoint = "memcmp", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CompareMemory(byte[] b1, byte[] b2, long count);

        [DllImport("msvcrt.dll", EntryPoint = "sscanf", CharSet = CharSet.Ansi,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern int ScanString(string buffer, string format, out int arg0, out int arg1);
    }
}
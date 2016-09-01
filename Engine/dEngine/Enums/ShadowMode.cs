// ShadowMode.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
#pragma warning disable 1591

namespace dEngine
{
    public enum ShadowMode
    {
        FixedSizePcf = 0,
        GridPcf = 1,
        RandomDiscPcf = 2,
        OptimizedPcf = 3,
        Vsm = 4,
        Evsm2 = 5,
        Evsm4 = 6,
        MsmHamburger = 7,
        MsmHausdorff = 8
    }
}
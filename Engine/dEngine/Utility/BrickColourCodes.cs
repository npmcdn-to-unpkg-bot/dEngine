﻿// BrickColourCodes.cs - dEngine
// Copyright (C) 2016-2016 DanDev (dandev.sco@gmail.com)
// 
// This library is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// You should have received a copy of the GNU Lesser General Public
// along with this program. If not, see <http://www.gnu.org/licenses/>.

using System.Collections.Generic;

namespace dEngine.Utility
{
    internal class BrickColourCodes
    {
        internal static readonly Dictionary<uint, Colour> RobloxColourCodes = new Dictionary<uint, Colour>
        {
            #region Colour Codes
            {0, Colour.Zero},
            {25, Colour.fromRGB(98, 71, 50)},
            {168, Colour.fromRGB(117, 108, 98)},
            {268, Colour.fromRGB(52, 43, 117)},
            {304, Colour.fromRGB(44, 101, 29)},
            {1024, Colour.fromRGB(175, 221, 255)},
            {190, Colour.fromRGB(249, 214, 46)},
            {18, Colour.fromRGB(204, 142, 105)},
            {349, Colour.fromRGB(233, 218, 218)},
            {149, Colour.fromRGB(22, 29, 50)},
            {225, Colour.fromRGB(235, 184, 127)},
            {1011, Colour.fromRGB(0, 32, 96)},
            {126, Colour.fromRGB(165, 165, 203)},
            {48, Colour.fromRGB(132, 182, 141)},
            {1006, Colour.fromRGB(180, 128, 255)},
            {358, Colour.fromRGB(171, 168, 158)},
            {50, Colour.fromRGB(236, 232, 222)},
            {217, Colour.fromRGB(124, 92, 70)},
            {360, Colour.fromRGB(150, 103, 102)},
            {151, Colour.fromRGB(120, 144, 130)},
            {1019, Colour.fromRGB(0, 255, 255)},
            {133, Colour.fromRGB(213, 115, 61)},
            {40, Colour.fromRGB(236, 236, 236)},
            {1007, Colour.fromRGB(163, 75, 75)},
            {320, Colour.fromRGB(202, 203, 209)},
            {150, Colour.fromRGB(171, 173, 172)},
            {343, Colour.fromRGB(212, 144, 189)},
            {323, Colour.fromRGB(148, 190, 129)},
            {344, Colour.fromRGB(150, 85, 85)},
            {324, Colour.fromRGB(168, 189, 153)},
            {330, Colour.fromRGB(255, 152, 220)},
            {29, Colour.fromRGB(161, 196, 140)},
            {308, Colour.fromRGB(61, 21, 133)},
            {1020, Colour.fromRGB(0, 255, 0)},
            {39, Colour.fromRGB(193, 202, 222)},
            {127, Colour.fromRGB(220, 188, 129)},
            {49, Colour.fromRGB(248, 241, 132)},
            {315, Colour.fromRGB(9, 137, 207)},
            {335, Colour.fromRGB(231, 231, 236)},
            {105, Colour.fromRGB(226, 155, 64)},
            {319, Colour.fromRGB(185, 196, 177)},
            {318, Colour.fromRGB(138, 171, 133)},
            {339, Colour.fromRGB(86, 36, 36)},
            {125, Colour.fromRGB(234, 184, 146)},
            {1010, Colour.fromRGB(0, 0, 255)},
            {103, Colour.fromRGB(199, 193, 183)},
            {356, Colour.fromRGB(160, 132, 79)},
            {101, Colour.fromRGB(218, 134, 122)},
            {1002, Colour.fromRGB(205, 205, 205)},
            {200, Colour.fromRGB(130, 138, 93)},
            {26, Colour.fromRGB(27, 42, 53)},
            {224, Colour.fromRGB(240, 213, 160)},
            {28, Colour.fromRGB(40, 127, 71)},
            {219, Colour.fromRGB(107, 98, 155)},
            {157, Colour.fromRGB(255, 246, 123)},
            {331, Colour.fromRGB(255, 89, 89)},
            {119, Colour.fromRGB(164, 189, 71)},
            {310, Colour.fromRGB(91, 154, 76)},
            {352, Colour.fromRGB(199, 172, 120)},
            {334, Colour.fromRGB(248, 217, 109)},
            {180, Colour.fromRGB(215, 169, 75)},
            {108, Colour.fromRGB(104, 92, 67)},
            {208, Colour.fromRGB(229, 228, 223)},
            {43, Colour.fromRGB(123, 182, 232)},
            {1018, Colour.fromRGB(18, 238, 212)},
            {361, Colour.fromRGB(86, 66, 54)},
            {102, Colour.fromRGB(110, 153, 202)},
            {1001, Colour.fromRGB(248, 248, 248)},
            {1003, Colour.fromRGB(17, 17, 17)},
            {232, Colour.fromRGB(125, 187, 221)},
            {106, Colour.fromRGB(218, 133, 65)},
            {141, Colour.fromRGB(39, 70, 45)},
            {36, Colour.fromRGB(243, 207, 155)},
            {1004, Colour.fromRGB(255, 0, 0)},
            {38, Colour.fromRGB(160, 95, 53)},
            {307, Colour.fromRGB(16, 42, 220)},
            {338, Colour.fromRGB(190, 104, 98)},
            {1008, Colour.fromRGB(193, 190, 66)},
            {1009, Colour.fromRGB(255, 255, 0)},
            {218, Colour.fromRGB(150, 112, 159)},
            {158, Colour.fromRGB(225, 164, 194)},
            {317, Colour.fromRGB(124, 156, 107)},
            {27, Colour.fromRGB(109, 110, 108)},
            {199, Colour.fromRGB(99, 95, 98)},
            {111, Colour.fromRGB(191, 183, 177)},
            {131, Colour.fromRGB(156, 163, 168)},
            {1017, Colour.fromRGB(255, 175, 0)},
            {362, Colour.fromRGB(126, 104, 63)},
            {1013, Colour.fromRGB(4, 175, 236)},
            {211, Colour.fromRGB(121, 181, 181)},
            {350, Colour.fromRGB(136, 62, 62)},
            {311, Colour.fromRGB(159, 161, 172)},
            {347, Colour.fromRGB(226, 220, 188)},
            {22, Colour.fromRGB(196, 112, 160)},
            {194, Colour.fromRGB(163, 162, 165)},
            {47, Colour.fromRGB(217, 133, 108)},
            {365, Colour.fromRGB(106, 57, 9)},
            {6, Colour.fromRGB(194, 218, 184)},
            {212, Colour.fromRGB(159, 195, 233)},
            {1028, Colour.fromRGB(204, 255, 204)},
            {353, Colour.fromRGB(202, 191, 163)},
            {118, Colour.fromRGB(183, 215, 213)},
            {354, Colour.fromRGB(187, 179, 178)},
            {332, Colour.fromRGB(117, 0, 0)},
            {351, Colour.fromRGB(188, 155, 93)},
            {312, Colour.fromRGB(89, 34, 89)},
            {221, Colour.fromRGB(205, 98, 152)},
            {1023, Colour.fromRGB(140, 91, 159)},
            {192, Colour.fromRGB(105, 64, 40)},
            {1022, Colour.fromRGB(127, 142, 100)},
            {220, Colour.fromRGB(167, 169, 206)},
            {143, Colour.fromRGB(207, 226, 247)},
            {1027, Colour.fromRGB(159, 243, 233)},
            {328, Colour.fromRGB(177, 229, 166)},
            {100, Colour.fromRGB(238, 196, 182)},
            {314, Colour.fromRGB(159, 173, 192)},
            {115, Colour.fromRGB(199, 210, 60)},
            {1026, Colour.fromRGB(177, 167, 255)},
            {140, Colour.fromRGB(32, 58, 86)},
            {37, Colour.fromRGB(75, 151, 75)},
            {359, Colour.fromRGB(175, 148, 131)},
            {1005, Colour.fromRGB(255, 175, 0)},
            {342, Colour.fromRGB(224, 178, 208)},
            {1032, Colour.fromRGB(255, 0, 191)},
            {345, Colour.fromRGB(116, 71, 71)},
            {321, Colour.fromRGB(167, 94, 155)},
            {340, Colour.fromRGB(241, 231, 199)},
            {301, Colour.fromRGB(80, 109, 84)},
            {1030, Colour.fromRGB(255, 204, 153)},
            {329, Colour.fromRGB(152, 194, 219)},
            {306, Colour.fromRGB(51, 88, 130)},
            {120, Colour.fromRGB(217, 228, 167)},
            {322, Colour.fromRGB(123, 47, 123)},
            {1031, Colour.fromRGB(98, 37, 209)},
            {303, Colour.fromRGB(0, 16, 176)},
            {341, Colour.fromRGB(254, 243, 187)},
            {302, Colour.fromRGB(91, 93, 105)},
            {223, Colour.fromRGB(220, 144, 149)},
            {1025, Colour.fromRGB(255, 201, 201)},
            {191, Colour.fromRGB(232, 171, 45)},
            {327, Colour.fromRGB(151, 0, 0)},
            {123, Colour.fromRGB(211, 111, 76)},
            {1016, Colour.fromRGB(255, 102, 204)},
            {41, Colour.fromRGB(205, 84, 75)},
            {363, Colour.fromRGB(105, 102, 92)},
            {1012, Colour.fromRGB(33, 84, 185)},
            {210, Colour.fromRGB(112, 149, 120)},
            {23, Colour.fromRGB(13, 105, 172)},
            {135, Colour.fromRGB(116, 134, 157)},
            {1015, Colour.fromRGB(170, 0, 170)},
            {9, Colour.fromRGB(232, 186, 200)},
            {213, Colour.fromRGB(108, 129, 183)},
            {5, Colour.fromRGB(215, 197, 154)},
            {148, Colour.fromRGB(87, 88, 87)},
            {128, Colour.fromRGB(174, 122, 89)},
            {116, Colour.fromRGB(85, 165, 175)},
            {193, Colour.fromRGB(207, 96, 36)},
            {1, Colour.fromRGB(242, 243, 243)},
            {313, Colour.fromRGB(31, 128, 29)},
            {196, Colour.fromRGB(35, 71, 139)},
            {136, Colour.fromRGB(135, 124, 144)},
            {3, Colour.fromRGB(249, 233, 153)},
            {146, Colour.fromRGB(149, 142, 163)},
            {12, Colour.fromRGB(203, 132, 66)},
            {113, Colour.fromRGB(228, 173, 200)},
            {44, Colour.fromRGB(247, 241, 141)},
            {364, Colour.fromRGB(90, 76, 66)},
            {124, Colour.fromRGB(146, 57, 120)},
            {178, Colour.fromRGB(180, 132, 85)},
            {154, Colour.fromRGB(123, 46, 47)},
            {337, Colour.fromRGB(255, 148, 148)},
            {1021, Colour.fromRGB(58, 125, 21)},
            {176, Colour.fromRGB(151, 105, 91)},
            {42, Colour.fromRGB(193, 223, 240)},
            {145, Colour.fromRGB(121, 136, 161)},
            {110, Colour.fromRGB(67, 84, 147)},
            {21, Colour.fromRGB(196, 40, 28)},
            {195, Colour.fromRGB(70, 103, 164)},
            {348, Colour.fromRGB(237, 234, 234)},
            {222, Colour.fromRGB(228, 173, 200)},
            {45, Colour.fromRGB(180, 210, 228)},
            {153, Colour.fromRGB(149, 121, 119)},
            {138, Colour.fromRGB(149, 138, 115)},
            {333, Colour.fromRGB(239, 184, 56)},
            {355, Colour.fromRGB(108, 88, 75)},
            {209, Colour.fromRGB(176, 142, 68)},
            {112, Colour.fromRGB(104, 116, 172)},
            {346, Colour.fromRGB(211, 190, 150)},
            {198, Colour.fromRGB(142, 66, 133)},
            {305, Colour.fromRGB(82, 124, 174)},
            {325, Colour.fromRGB(223, 223, 222)},
            {121, Colour.fromRGB(231, 172, 88)},
            {1014, Colour.fromRGB(170, 85, 0)},
            {2, Colour.fromRGB(161, 165, 162)},
            {134, Colour.fromRGB(216, 221, 86)},
            {24, Colour.fromRGB(245, 205, 48)},
            {107, Colour.fromRGB(0, 143, 156)},
            {226, Colour.fromRGB(253, 234, 141)},
            {309, Colour.fromRGB(52, 142, 64)},
            {11, Colour.fromRGB(128, 187, 219)},
            {147, Colour.fromRGB(147, 135, 103)},
            {1029, Colour.fromRGB(255, 255, 204)},
            {179, Colour.fromRGB(137, 135, 136)},
            {336, Colour.fromRGB(199, 212, 228)},
            {316, Colour.fromRGB(123, 0, 123)},
            {216, Colour.fromRGB(143, 76, 42)},
            {137, Colour.fromRGB(224, 152, 100)},
            {104, Colour.fromRGB(107, 50, 124)},
            {357, Colour.fromRGB(149, 137, 136)}
            #endregion
        };
    }
}
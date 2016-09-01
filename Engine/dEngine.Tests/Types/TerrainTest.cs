// TerrainTest.cs - dEngine.Tests
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dEngine.Tests
{
    [TestClass]
    public class TerrainTest
    {
        private readonly Terrain _terrain;

        public TerrainTest()
        {
            _terrain = new Terrain(null);
        }

        [TestMethod]
        public void MaxExtents()
        {
            var extents = _terrain.MaxExtents;
            var target = new Region3int16(new Vector3int16(-32000, -32000, -32000),
                new Vector3int16(32000, 32000, 32000));
            Assert.AreEqual(extents, target);
        }

        [TestMethod]
        public void CellCoords()
        {
            var wtc0 = _terrain.WorldToCell(new Vector3(8, 4, 4));
            Assert.AreEqual(wtc0, new Vector3int16(2, 1, 1));

            var wtc1 = _terrain.WorldToCell(new Vector3(8, -8, 4));
            Assert.AreEqual(wtc1, new Vector3int16(2, -2, 1));
        }
    }
}
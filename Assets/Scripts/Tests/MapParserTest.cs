using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class MapParserTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void MapParserTestSimple()
        {
            // Use the Assert class to test conditions
            var map = MapParser.GetMap(@"Assets\Maps\test");

            Assert.AreEqual(map.rows, 3);
            Assert.AreEqual(map.cols, 6);

            Assert.AreEqual(map.tiles.Count, 4);

            Assert.AreEqual(map.tiles[(0, 0)], MapParser.TileType.LaserRight);
            Assert.AreEqual(map.tiles[(0, 3)], MapParser.TileType.MirrorBackward);
            Assert.AreEqual(map.tiles[(2, 0)], MapParser.TileType.Wall);
            Assert.AreEqual(map.tiles[(2, 3)], MapParser.TileType.Target);
        }
    }
}

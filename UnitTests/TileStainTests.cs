using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using dmg.Domain;

namespace UnitTests
{
    [TestFixture]
    public class TileStainTests
    {
        [Test]
        public void TileStainConstructs()
        {
            TileStain ts = new TileStain(ConsoleColor.Red, ConsoleColor.DarkRed, 1);
            Assert.AreEqual(ts.HighColor, ConsoleColor.Red);
            Assert.AreEqual(ts.LowColor, ConsoleColor.DarkRed);
            Assert.AreEqual(ts.StainLevel, 1);
        }

        [Test]
        public void StainLevelConstrainsLow()
        {
            TileStain ts = new TileStain(ConsoleColor.Red, ConsoleColor.DarkRed, 1);
            ts.StainLevel = 0;
            Assert.AreEqual(ts.StainLevel, 0);
            ts.StainLevel = -5;
            Assert.AreEqual(ts.StainLevel, 0);
        }

        [Test]
        public void StainLevelConstrainsHigh()
        {
            TileStain ts = new TileStain(ConsoleColor.Red, ConsoleColor.DarkRed, 1);
            ts.StainLevel = 3;
            Assert.AreEqual(ts.StainLevel, 3);
            ts.StainLevel = 50;
            Assert.AreEqual(ts.StainLevel, 3);
        }

        [Test]
        public void InitTurnsUntilDecay()
        {
            TileStain ts = new TileStain(ConsoleColor.Red, ConsoleColor.DarkRed, 1);
            Assert.AreEqual(ts.TurnsUntilDecay, 10);
        }

        [Test]
        public void ChangingLevelResetsTurnsUntilDecay()
        {
            TileStain ts = new TileStain(ConsoleColor.Red, ConsoleColor.DarkRed, 1);
            Assert.AreEqual(ts.TurnsUntilDecay, 10);
            ts.StainLevel = 0;
            Assert.AreEqual(ts.TurnsUntilDecay, 20);
            ts.StainLevel = 5;
            Assert.AreEqual(ts.TurnsUntilDecay, 5);
        }
    }
}

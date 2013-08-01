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
    public class RedStainTests
    {
        [Test]
        public void RedStainConstructs()
        {
            RedStain rs = new RedStain(3);
            Assert.AreEqual(rs.HighColor, ConsoleColor.Red);
            Assert.AreEqual(rs.LowColor, ConsoleColor.DarkRed);
            Assert.AreEqual(rs.TurnsUntilDecay, rs.AgeThresholds[3]);
        }
    }
}

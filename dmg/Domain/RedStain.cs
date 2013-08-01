using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmg.Domain
{
    public class RedStain : TileStain
    {
        public RedStain(int stainLevel) : base(ConsoleColor.Red, ConsoleColor.DarkRed, stainLevel)
        {
        }
    }
}

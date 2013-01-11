using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmg.Domain
{
    public class Dude
    {
        public int XPos { get; set; }
        public int YPos { get; set; }
        public ConsoleColor Color { get; set; }
        public Char Char { get; set; }

        public Dude(int x, int y)
        {
            XPos = x;
            YPos = y;
            Color = ConsoleColor.White;
            Char = '@';
        }
    }
}
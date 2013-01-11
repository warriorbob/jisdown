using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmg.Domain
{
    class Baddie
    {
        public int XPos { get; set; }
        public int YPos { get;set; }
        public ConsoleColor Color { get; set; }
        public Char Char { get; set; }

        public Baddie(int x, int y)
        {
            XPos = x;
            YPos = y;
            Color = ConsoleColor.White;
            Char = 'J';
        }
    }
}

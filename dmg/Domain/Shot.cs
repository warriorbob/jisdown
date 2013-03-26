using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmg.Domain
{
    public class Shot
    {
        public int XSpeed { get; set; }
        public int YSpeed { get; set; }
        public char Char { get; set; }
        public ConsoleColor Color { get; set; }

        public Shot(int xSpeed, int ySpeed, char displayChar = '.', ConsoleColor color = ConsoleColor.White)
        {
            XSpeed = xSpeed;
            YSpeed = ySpeed;
            Char = displayChar;
            Color = color;
        }
    }
}

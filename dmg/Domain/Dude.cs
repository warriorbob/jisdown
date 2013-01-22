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
        public bool IsAlive { get; set; }

        public Dude(int x, int y)
        {
            XPos = x;
            YPos = y;
            Color = ConsoleColor.White;
            Char = '@';
        }

        public void Draw(ref ConsoleChar[,] screen, Map screenGrid)
        {
            screen[XPos, YPos].BackgroundColor = screenGrid.Grid[XPos, YPos].BackgroundColor;
            screen[XPos, YPos].ForegroundColor = Color;
            screen[XPos, YPos].Char = Char;
        }
    }
}
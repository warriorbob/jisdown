using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmg.Domain
{
    public class Shot
    {
        public int XPos { get; set; }
        public int YPos { get; set; }
        public int XSpeed { get; set; }
        public int YSpeed { get; set; }
        public char Char { get; set; }
        public ConsoleColor Color { get; set; }

        public Shot(int xPos, int yPos, int xSpeed, int ySpeed, char displayChar = '.', ConsoleColor color = ConsoleColor.White)
        {
            XPos = xPos;
            YPos = yPos;
            XSpeed = xSpeed;
            YSpeed = ySpeed;
            Char = displayChar;
            Color = color;
        }

        public void Draw(ref ConsoleChar[,] screen, Map screenGrid)
        {
            screen[XPos, YPos].BackgroundColor = screenGrid.Grid[XPos, YPos].BackgroundColor;
            screen[XPos, YPos].ForegroundColor = Color;
            screen[XPos, YPos].Char = Char;
        }
    }
}

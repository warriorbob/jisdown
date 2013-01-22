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

        public void Chase(int targetX, int targetY)
        {
            int deltaX;
            int deltaY;
            double angle;
            const double DIAGONAL_THRESHOLD = Math.PI / 8;

            deltaX = targetX - this.XPos;
            deltaY = targetY - this.YPos;

            angle = Math.Atan(Math.Abs((double)deltaY) / Math.Abs((double)deltaX));

            if (angle < DIAGONAL_THRESHOLD || Math.PI / 2 - angle < DIAGONAL_THRESHOLD)
            {
                //Move horizontally or vertically
                if (Math.Abs(deltaX) > Math.Abs(deltaY))
                {
                    this.XPos += Math.Sign(deltaX);
                }
                else if (Math.Abs(deltaY) > Math.Abs(deltaX))
                {
                    this.YPos += Math.Sign(deltaY);
                }
                else
                {
                    //Shouldn't be possible
                    throw new Exception("Horizontal movement in the diagonal threshold shouldn't be possible");
                }
            }
            else
            {
                //Move diagonally
                this.XPos += Math.Sign(deltaX);
                this.YPos += Math.Sign(deltaY);
            }
        }
    }
}
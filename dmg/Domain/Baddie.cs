using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmg.Domain
{
    public class Baddie : IBaddie
    {
        public int XPos { get; set; }
        public int YPos { get;set; }
        public ConsoleColor Color { get; set; }
        public char Char { get; set; }
        public bool Alive { get; set; }

        /// <summary>
        /// Baddie constructor
        /// </summary>
        /// <param name="x">Initial X location</param>
        /// <param name="y">Initial Y location</param>
        public Baddie(int x, int y)
        {
            XPos = x;
            YPos = y;
            Color = ConsoleColor.DarkCyan;
            Char = '☺';
            Alive = true;
        }

        /// <summary>
        /// Moves baddie towards the target position
        /// </summary>
        /// <param name="targetX">Target X coordinate</param>
        /// <param name="targetY">Target Y coordinate</param>
        public virtual void Chase(int targetX, int targetY)
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

        public virtual void Draw(ref ConsoleChar[,] screen, Map screenGrid)
        {
            screen[XPos, YPos].BackgroundColor = screenGrid.Grid[XPos, YPos].BackgroundColor;
            screen[XPos, YPos].ForegroundColor = Color;
            screen[XPos, YPos].Char = Char;
        }

        //Blarg!!
        public virtual void Blarg(int fromX, int fromY, ref Map map)
        {
            int xDirectionSign = Math.Sign(XPos - fromX);
            int yDirectionSign = Math.Sign(YPos - fromY);

            PaintBackground(ref map, XPos, YPos, ConsoleColor.Red); // Baddie location
            PaintBackground(ref map, XPos + xDirectionSign, YPos + yDirectionSign, ConsoleColor.DarkRed); //Right next
            PaintForeground(ref map, XPos + xDirectionSign, YPos + yDirectionSign, ConsoleColor.Red); //Right next foreground
            PaintForeground(ref map, XPos + 2 * xDirectionSign, YPos + 2 * yDirectionSign, ConsoleColor.DarkRed); //Right next next
            PaintBackground(ref map, XPos + xDirectionSign + yDirectionSign, YPos + yDirectionSign + xDirectionSign, ConsoleColor.DarkRed); //one side
            PaintBackground(ref map, XPos + xDirectionSign - yDirectionSign, YPos + yDirectionSign - xDirectionSign, ConsoleColor.DarkRed); //other side
            PaintForeground(ref map, XPos + 2 * xDirectionSign + yDirectionSign, YPos + 2 * yDirectionSign + xDirectionSign, ConsoleColor.DarkRed); //2 out, one side
            PaintForeground(ref map, XPos + 2 * xDirectionSign - yDirectionSign, YPos + 2 * yDirectionSign - xDirectionSign, ConsoleColor.DarkRed); //2 out, other side 

            Alive = false;
        }

        private void PaintBackground(ref Map map, int x, int y, ConsoleColor color)
        {
            //MAGIC NUMBERS
            if(x >= 0 && x < 80 && y >= 0 && y < 24)
                map.Grid[x, y].BackgroundColor = color;
        }

        private void PaintForeground(ref Map map, int x, int y, ConsoleColor color)
        {
            //MAGIC NUMBERS
            if (x >= 0 && x < 80 && y >= 0 && y < 24)
                map.Grid[x, y].ForegroundColor = color;
        }
    }
}
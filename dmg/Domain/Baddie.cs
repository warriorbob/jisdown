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

            Random rand = new Random();
            int randInt = rand.Next(100);

            if (randInt <= 20)
            {
                map.StainTile(XPos, YPos, new RedStain(2)); // Baddie location
                map.StainTile(XPos + xDirectionSign, YPos + yDirectionSign, new RedStain(0)); //Right next
            }
            else if (randInt <= 40)
            {
                map.StainTile(XPos, YPos, new RedStain(3)); // Baddie location
                map.StainTile(XPos + xDirectionSign, YPos + yDirectionSign, new RedStain(2)); //Right next
                map.StainTile(XPos + 2 * xDirectionSign, YPos + 2 * yDirectionSign, new RedStain(0)); //two out
                map.StainTile(XPos + xDirectionSign + yDirectionSign, YPos + yDirectionSign + xDirectionSign, new RedStain(0)); //one side
                map.StainTile(XPos + xDirectionSign - yDirectionSign, YPos + yDirectionSign - xDirectionSign, new RedStain(0)); //other side
            }
            else if (randInt <= 80)
            {
                map.StainTile(XPos, YPos, new RedStain(2)); // Baddie location
                map.StainTile(XPos + xDirectionSign, YPos + yDirectionSign, new RedStain(1)); //Right next
                map.StainTile(XPos + 2 * xDirectionSign, YPos + 2 * yDirectionSign, new RedStain(0)); //two out
                map.StainTile(XPos + xDirectionSign + yDirectionSign, YPos + yDirectionSign + xDirectionSign, new RedStain(0)); //one side
                map.StainTile(XPos + xDirectionSign - yDirectionSign, YPos + yDirectionSign - xDirectionSign, new RedStain(0)); //other side
            }
            else if (randInt <= 98)
            {
                map.StainTile(XPos, YPos, new RedStain(3)); // Baddie location
                map.StainTile(XPos + xDirectionSign, YPos + yDirectionSign, new RedStain(2)); //Right next
                map.StainTile(XPos + 2 * xDirectionSign, YPos + 2 * yDirectionSign, new RedStain(0)); //two out
                map.StainTile(XPos + xDirectionSign + yDirectionSign, YPos + yDirectionSign + xDirectionSign, new RedStain(1)); //one side
                map.StainTile(XPos + xDirectionSign - yDirectionSign, YPos + yDirectionSign - xDirectionSign, new RedStain(1)); //other side
                map.StainTile(XPos + 2 * xDirectionSign + yDirectionSign, YPos + 2 * yDirectionSign + xDirectionSign, new RedStain(0)); //2 out, one side
                map.StainTile(XPos + 2 * xDirectionSign - yDirectionSign, YPos + 2 * yDirectionSign - xDirectionSign, new RedStain(0)); //2 out, other side 
            }
            else
            {
                map.StainTile(XPos, YPos, new RedStain(2)); // Baddie location
                map.StainTile(XPos + yDirectionSign, YPos + xDirectionSign, new RedStain(0)); // side
                map.StainTile(XPos - yDirectionSign, YPos - xDirectionSign, new RedStain(0)); // other side
                map.StainTile(XPos + xDirectionSign, YPos + yDirectionSign, new RedStain(2)); //Right next
                map.StainTile(XPos + 2 * xDirectionSign, YPos + 2 * yDirectionSign, new RedStain(3)); //two out
                map.StainTile(XPos + xDirectionSign + yDirectionSign, YPos + yDirectionSign + xDirectionSign, new RedStain(3)); //one out, side
                map.StainTile(XPos + xDirectionSign - yDirectionSign, YPos + yDirectionSign - xDirectionSign, new RedStain(3)); //one out, other side
                map.StainTile(XPos + 2 * xDirectionSign + yDirectionSign, YPos + 2 * yDirectionSign + xDirectionSign, new RedStain(2)); //2 out, one side
                map.StainTile(XPos + 2 * xDirectionSign - yDirectionSign, YPos + 2 * yDirectionSign - xDirectionSign, new RedStain(2)); //2 out, other side 
                map.StainTile(XPos + 3 * xDirectionSign, YPos + 3 * yDirectionSign, new RedStain(1)); //3 out
                map.StainTile(XPos + 3 * xDirectionSign + 2 * yDirectionSign, YPos + 3 * yDirectionSign + 2 * xDirectionSign, new RedStain(1)); //3 out, 2 side
                map.StainTile(XPos + 3 * xDirectionSign - 2 * yDirectionSign, YPos + 3 * yDirectionSign - 2 * xDirectionSign, new RedStain(1)); //3 out, 2 other side
                map.StainTile(XPos + 3 * xDirectionSign + yDirectionSign, YPos + 3 * yDirectionSign + xDirectionSign, new RedStain(0)); //3 out, side
                map.StainTile(XPos + 3 * xDirectionSign - yDirectionSign, YPos + 3 * yDirectionSign - xDirectionSign, new RedStain(0)); //3 out, other side
                map.StainTile(XPos + 4 * xDirectionSign, YPos + 4 * yDirectionSign, new RedStain(0)); // 4 out
                map.StainTile(XPos + 4 * xDirectionSign + yDirectionSign, YPos + 4 * yDirectionSign + xDirectionSign, new RedStain(0)); //4 out, side
                map.StainTile(XPos + 4 * xDirectionSign - yDirectionSign, YPos + 4 * yDirectionSign - xDirectionSign, new RedStain(0)); //4 out, other side
                map.StainTile(XPos + 5 * xDirectionSign, YPos + 5 * yDirectionSign, new RedStain(0)); // 5 out
            }
            Alive = false;
        }
    }
}
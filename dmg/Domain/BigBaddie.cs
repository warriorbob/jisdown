using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmg.Domain
{
    public class BigBaddie : Baddie
    {
        public int Life { get; set; }
        public int Wait { get; set; }
        public int WaitBase { get; set; }

        public BigBaddie(int x, int y) : base(x,y)
        {
            Char = '☻';
            Life = 3;
            WaitBase = 1;
            Wait = WaitBase;
        }

        public override void Blarg(int fromX, int fromY, Map map)
        {
            Alive = (--Life > 0);
            
            int xDirectionSign = Math.Sign(XPos - fromX);
            int yDirectionSign = Math.Sign(YPos - fromY);

            if (Alive)
            {
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
            }
            else
            {
                Bigsplode(map, xDirectionSign, yDirectionSign);
            }
            

            if (Life == 1)
            {
                Char = '☺';
            }
        }

        public override void Chase(int targetX, int targetY)
        {
            if (Wait > 0)
            {
                Wait--;
            }
            else
            {
                base.Chase(targetX, targetY);
                Wait = WaitBase;
            }
        }
    }
}

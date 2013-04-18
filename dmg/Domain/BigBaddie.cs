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

        public override void Blarg(int fromX, int fromY, ref Map map)
        {
            base.Blarg(fromX, fromY, ref map);
            Alive = (--Life > 0);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmg.Domain
{
    public class AcidSlime : Baddie
    {
        public int Life { get; set; }

        public AcidSlime(int x, int y) : base(x,y)
        {
            Char = '▓';
            Color = ConsoleColor.Green;
            Life = 20;
        }

        public override void Blarg(int fromX, int fromY, ref Map map)
        {
            Alive = (--Life > 0);
        }

        public override void Chase(int targetX, int targetY)
        {
            //Do nothing; AcidSlime does not move.
        }
    }
}

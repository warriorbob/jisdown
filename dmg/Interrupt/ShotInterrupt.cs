using dmg.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmg.Interrupt
{
    public class ShotInterrupt : InterruptEvent
    {
        public override void DoStuff(Queue<InterruptEvent> queue, StateManager state, Map map)
        {
            foreach (Shot shot in state.Shots)
            {
                shot.XPos += shot.XSpeed;
                shot.YPos += shot.YSpeed;
            }

        }
    }
}

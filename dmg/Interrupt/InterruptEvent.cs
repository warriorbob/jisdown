using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmg.Interrupt
{
    public abstract class InterruptEvent
    {
        public abstract void DoStuff(Queue<InterruptEvent> queue, StateManager state);
    }
}

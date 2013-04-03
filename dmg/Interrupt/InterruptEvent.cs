using dmg.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmg.Interrupt
{
    public interface IInterruptEvent
    {
        void DoStuff(Queue<IInterruptEvent> queue, StateManager state, ref Map map);
    }
}

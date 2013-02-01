using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace dmg.Interrupt
{
    public class InterruptTest : InterruptEvent
    {
        public override void DoStuff(Queue<InterruptEvent> queue)
        {
            Console.WriteLine("test" + DateTime.Now.ToString());
            queue.Enqueue(this);
            Thread.Sleep(1000);
        }
    }
}

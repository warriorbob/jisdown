using dmg.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dmg.Interrupt
{
    public class ShotInterrupt : InterruptEvent
    {
        public int constraintWidth { get; set; }
        public int constraintHeight { get; set; }

        public ShotInterrupt(int width, int height)
        {
            constraintWidth = width;
            constraintHeight = height;
        }

        public override void DoStuff(Queue<InterruptEvent> queue, StateManager state, ref Map map)
        {
            foreach (Shot shot in state.Shots)
            {
                shot.XPos += shot.XSpeed;
                shot.YPos += shot.YSpeed;

                //Disappear shots at the edge of the screen
                if (shot.XPos < 0 || shot.XPos > constraintWidth - 1
                    || shot.YPos < 0 || shot.YPos > constraintHeight - 1)
                {
                    shot.Alive = false;
                }

                foreach(Baddie baddie in state.Baddies)
                {
                    if(baddie.XPos == shot.XPos && baddie.YPos == shot.YPos)
                    {
                        baddie.Blarg(state.Dude.XPos,state.Dude.YPos, ref map);
                        shot.Alive = false;
                    }
                }
            }

            for (int i = 0; i < state.Shots.Count; i++)
            {
                if (!state.Shots[i].Alive)
                {
                    state.Shots.Remove(state.Shots[i]);
                }
            }

            //Queue up another interrupt for the next frame of animation
            if (state.Shots.Count > 0)
            {
                Thread.Sleep(40);
                queue.Enqueue(this);
            }
        }
    }
}

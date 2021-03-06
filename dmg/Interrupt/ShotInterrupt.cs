﻿using dmg.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dmg.Interrupt
{
    public class ShotInterrupt : IInterruptEvent
    {
        public int constraintWidth { get; set; }
        public int constraintHeight { get; set; }
        private int threadSleepDuration { get; set; }

        public ShotInterrupt(int width, int height, int sleeptime)
        {
            constraintWidth = width;
            constraintHeight = height;
            threadSleepDuration = sleeptime;
        }

        public void DoStuff(Queue<IInterruptEvent> queue, StateManager state, ref Map map)
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
                        baddie.Blarg(state.Dude.XPos,state.Dude.YPos, map);
                        shot.Alive = false;
                        state.Score++;
                        break;
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
                Thread.Sleep(threadSleepDuration);
                queue.Enqueue(this);
            }
            else    //This is kind of a hack
            {
                state.MoveBaddies();
                state.EatBrains();
            }
        }
    }
}

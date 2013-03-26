using dmg.Domain;
using dmg.Interrupt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmg
{
    public class StateManager
    {
        public Dude Dude { get; set; }
        public List<Baddie> Baddies { get; set; }
        public Queue<InterruptEvent> InterruptEvents;
        public List<Shot> shots { get; set;}
        //public Map Map { get; set; }


        public void UpdateState(ref bool running, ConsoleKeyInfo keyInfo, int width, int height, Map map)
        {
            //Player
            Bang(keyInfo, map);
            MoveDude(keyInfo, width, height);
            
            //Game
            MoveBaddies();
            EatBrains(ref running);
            
            //Cleanup
            CleanBaddies();
        }

        //BANG
        public void Bang(ConsoleKeyInfo keyInfo, Map map)
        {
            int xDir = 0;
            int yDir = 0;

            //Set shot direction
            if (keyInfo.Key == ConsoleKey.L)    //Right
            {
                xDir = 1;
            }
            else if (keyInfo.Key == ConsoleKey.H)   //Left
            {
                xDir = -1;
            }
            else if (keyInfo.Key == ConsoleKey.K)   //Up
            {
                yDir = -1;
            }
            else if (keyInfo.Key == ConsoleKey.J)   //Down
            {
                yDir = 1;
            }

            if (xDir != 0 || yDir != 0)
            {
                //Load a list of possible targets to hit
                List<Baddie> targets = new List<Baddie>();
                int deltaX;
                int deltaY;
                foreach (Baddie baddie in Baddies)
                {
                    deltaX = baddie.XPos - Dude.XPos;
                    deltaY = baddie.YPos - Dude.YPos;
                    //If on the same horizontal line
                    if (Dude.YPos == baddie.YPos)
                    {
                        //...and the shot direction matches
                        if (Math.Sign(deltaX) == xDir)
                        {
                            targets.Add(baddie);
                        }
                    }
                    //else if on the same vertical line
                    else if (Dude.XPos == baddie.XPos)
                    {
                        //...and the shot direction matches
                        if (Math.Sign(deltaY) == yDir)
                        {
                            targets.Add(baddie);
                        }
                    }
                }

                //Go through the list and hit the closest one
                double distance;
                double lowestDistance = 999;
                int closestIndex = -1;
                int absX;
                int absY;
                for (int i = 0; i < targets.Count; i++)
                {
                    absX = Math.Abs(targets[i].XPos - Dude.XPos);
                    absY = Math.Abs(targets[i].YPos - Dude.YPos);

                    distance = Math.Sqrt(Math.Pow(absX, 2) + Math.Pow(absY, 2));
                    if (distance < lowestDistance)
                    {
                        lowestDistance = distance;
                        closestIndex = i;
                    }
                }
                if (closestIndex >= 0)
                {
                    targets[closestIndex].Blarg(Dude.XPos, Dude.YPos, ref map);
                }
            }
        }

        public void MoveDude(ConsoleKeyInfo keyInfo, int constraintWidth, int constraintHeight)
        {
            //Movement
            if (keyInfo.Key == ConsoleKey.W)
            {
                Dude.YPos--;
            }
            else if (keyInfo.Key == ConsoleKey.S)
            {
                Dude.YPos++;
            }
            else if (keyInfo.Key == ConsoleKey.A)
            {
                Dude.XPos--;
            }
            else if (keyInfo.Key == ConsoleKey.D)
            {
                Dude.XPos++;
            }

            //Constrain dimensions
            if (Dude.XPos < 0)
            {
                Dude.XPos = 0;
            }
            else if (Dude.XPos > constraintWidth - 1)
            {
                Dude.XPos = constraintWidth - 1;
            }
            if (Dude.YPos < 0)
            {
                Dude.YPos = 0;
            }
            else if (Dude.YPos > constraintHeight - 1)
            {
                Dude.YPos = constraintHeight - 1;
            }
        }

        public void CleanBaddies()
        {
            for (int i = 0; i < Baddies.Count; i++)
            {
                if (!Baddies[i].Alive)
                {
                    Baddies.Remove(Baddies[i]);
                }
            }
        }

        public void MoveBaddies()
        {
            foreach (Baddie baddie in Baddies)
            {
                if (baddie.Alive)
                {
                    baddie.Chase(Dude.XPos, Dude.YPos);
                }
            }
        }

        public void EatBrains(ref bool running)
        {
            foreach (Baddie baddie in Baddies)
            {
                if (baddie.Alive && baddie.XPos == Dude.XPos && baddie.YPos == Dude.YPos)
                {
                    running = false;
                }
            }
        }
    }
}

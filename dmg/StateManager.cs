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
        public List<IBaddie> Baddies { get; set; }
        public Queue<IInterruptEvent> InterruptEvents;
        public List<Shot> Shots { get; set;}
        public int SpawnTimer { get; set; }
        public int Score { get; set; }

        public StateManager(List<IBaddie> baddies, Queue<IInterruptEvent> interruptEvents, List<Shot> shots, int spawnTimer, int score)
        {
            Dude = new Dude(0, 0);
            Baddies = baddies;
            InterruptEvents = interruptEvents;
            Shots = shots;
            SpawnTimer = spawnTimer;
            Score = score;
        }

        public void UpdateState(ref bool running, ConsoleKeyInfo keyInfo, int width, int height, Map map)
        {
            Fire(keyInfo, map, width, height);
            MoveDude(keyInfo, width, height);
            MoveBaddies();
            EatBrains(ref running);
            CleanBaddies();
            SpawnBaddies(width, height);
        }

        private void SpawnBaddies(int width, int height)
        {
            Random rand = new Random();
            if (SpawnTimer == 3)
            {
                int newx, newy = 0;
                do
                {
                    newx = rand.Next(0, width);
                    newy = rand.Next(0, height);
                }
                while (newx == Dude.XPos && newy == Dude.YPos);

                Baddies.Add(new Baddie(newx, newy));
                SpawnTimer = 0;
            }
            else
            {
                SpawnTimer++;
            }
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

        //Like Bang() but spawns a shot object and lets that determine the outcome
        public void Fire(ConsoleKeyInfo keyInfo, Map map, int width, int height)
        {
            if (InterruptEvents.Count > 0)
                return;

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
                Shots.Add(new Shot(Dude.XPos, Dude.YPos, xDir, yDir));
                InterruptEvents.Enqueue(new ShotInterrupt(width, height));
            }
        }

        public void MoveDude(ConsoleKeyInfo keyInfo, int constraintWidth, int constraintHeight)
        {
            if (InterruptEvents.Count > 0)
                return;

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
            if (InterruptEvents.Count > 0)
                return;

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
            if (InterruptEvents.Count > 0)
                return;

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

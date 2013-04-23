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
        public GameStates CurrentGameState { get; set; }
        public SpawnManager spawnManager { get; set; }

        public enum GameStates { TitleScreen, Playing, Dead, Paused };

        public StateManager(List<IBaddie> baddies, Queue<IInterruptEvent> interruptEvents, List<Shot> shots, int spawnTimer, int score, GameStates initialGameState = GameStates.TitleScreen)
        {
            Dude = new Dude(0, 0);
            Baddies = baddies;
            InterruptEvents = interruptEvents;
            Shots = shots;
            SpawnTimer = spawnTimer;
            Score = score;
            CurrentGameState = initialGameState;
            spawnManager = new SpawnManager(0, 3);
        }

        public void UpdateState(ref bool running, ConsoleKeyInfo keyInfo, int width, int height, Map map)
        {
            Fire(keyInfo, map, width, height);
            MoveDude(keyInfo, width, height);
            MoveBaddies();
            CleanBaddies();
            SpawnBaddies(width, height);
            EatBrains();
        }

        private void SpawnBaddies(int width, int height)
        {
            Random rand = new Random();
            if (spawnManager.IsReady)
            {
                int newx, newy = 0;
                do
                {
                    newx = rand.Next(0, width);
                    newy = rand.Next(0, height);
                }
                while (newx == Dude.XPos && newy == Dude.YPos);

                Baddies.Add(spawnManager.PopBaddie(newx, newy));
            }
            else
            {
                spawnManager.Tick();
            }
        }

        //Spawns a shot object and lets that determine the outcome
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

        public void EatBrains()
        {
            foreach (Baddie baddie in Baddies)
            {
                if (baddie.Alive && baddie.XPos == Dude.XPos && baddie.YPos == Dude.YPos)
                {
                    Dude.Alive = false;
                }
            }
        }
    }
}

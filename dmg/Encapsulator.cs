﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dmg.Domain;
using dmg.Interrupt;

namespace dmg
{
    /// <summary>
    /// Encapsulates everything into an instanceable object
    /// </summary>
    class Encapsulator
    {
        private const int CONSOLE_WIDTH = 80;
        private const int CONSOLE_HEIGHT = 25;

        private int GRID_WIDTH = 80;
        private int GRID_HEIGHT = 24;

        private State state;

        private Map screenGrid;
        private Queue<InterruptEvent> interruptEvents;

        private ConsoleKeyInfo keyInfo;
        public ConsoleChar[,] newScreen;
        public ConsoleChar[,] previousScreen;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="width">ScreenGrid width</param>
        /// <param name="height">ScreenGrid height</param>
        public Encapsulator(int width, int height)
        {
            //Infrastructure
            state = new State();
            GRID_WIDTH = width;
            GRID_HEIGHT = height;
            screenGrid = new Map(GRID_WIDTH, GRID_HEIGHT);
            keyInfo = new ConsoleKeyInfo();
            newScreen = new ConsoleChar[CONSOLE_WIDTH, CONSOLE_HEIGHT];
            previousScreen = new ConsoleChar[CONSOLE_WIDTH, CONSOLE_HEIGHT];
            interruptEvents = new Queue<InterruptEvent>();

            //Initialize screenbuffers
            for (int w = 0; w < CONSOLE_WIDTH; w++)
            {
                for (int h = 0; h < CONSOLE_HEIGHT-1; h++)
                {
                    newScreen[w, h] = new ConsoleChar();
                    newScreen[w, h].Char = ' ';
                    newScreen[w, h].BackgroundColor = ConsoleColor.Black;
                    newScreen[w, h].ForegroundColor = ConsoleColor.White;
                    previousScreen[w, h] = new ConsoleChar();
                    previousScreen[w, h].Char = ' ';
                    previousScreen[w, h].BackgroundColor = ConsoleColor.Black;
                    previousScreen[w, h].ForegroundColor = ConsoleColor.White;
                }
            }

            //Entities
            state.Dude = new Dude(22, 0);
            state.Baddies = new List<Baddie>();
            state.Baddies.Add(new Baddie(20, 15));
            state.Baddies.Add(new Baddie(22, 15));
            state.Baddies.Add(new Baddie(25, 15));

            //interruptEvents.Enqueue(new InterruptTest());
        }

        /// <summary>
        /// This is the method with the main loop in it.
        /// </summary>
        public void Go()
        {
            //Draw once outside of the loop
            Draw();

            bool running = true;

            while (running == true) //Main loop
            {
                Draw();
                if (interruptEvents.Count > 0)
                {
                    interruptEvents.Dequeue().DoStuff(interruptEvents);
                }
                else
                {
                    GetInput(ref running);
                    UpdateState(ref running);
                }

                //Reinitialize input
                keyInfo = new ConsoleKeyInfo();
            }

            //"Press any key to continue" when we're done
            Console.SetCursorPosition(0, 24);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        //DRAWING ---------------------------------------------------------------------------------
        /// <summary>
        /// Draw all the things
        /// </summary>
        public void Draw()
        {
            BackbufferMap();
            BackbufferDude();
            BackbufferBaddies();
            
            DrawFromBuffers(newScreen, previousScreen);

            InitializeNewScreen();

            //Reposition cursor
            Console.SetCursorPosition(0, CONSOLE_HEIGHT-1);
        }

        //Reinitializes newScreen to black spaces
        private void InitializeNewScreen()
        {
            for (int w = 0; w < CONSOLE_WIDTH; w++)
            {
                for (int h = 0; h < CONSOLE_HEIGHT - 1; h++)
                {
                    newScreen[w, h] = new ConsoleChar();
                    newScreen[w, h].Char = ' ';
                    newScreen[w, h].BackgroundColor = ConsoleColor.Black;
                    newScreen[w, h].ForegroundColor = ConsoleColor.White;
                }
            }
        }

        public void DrawFromBuffers(ConsoleChar[,] newScreen, ConsoleChar[,] previousScreen)
        {
           List<ConsoleChar> changes = new List<ConsoleChar>();

            for (int w = 0; w < CONSOLE_WIDTH; w++)
            {
                for (int h = 0; h < CONSOLE_HEIGHT - 1; h++)
                {
                    if (!newScreen[w, h].Matches(previousScreen[w, h]))
                    {
                        changes.Add(new ConsoleChar
                        {
                            Char = newScreen[w, h].Char,
                            BackgroundColor = newScreen[w, h].BackgroundColor,
                            ForegroundColor = newScreen[w, h].ForegroundColor,
                            XPos = w,
                            YPos = h
                        });
                    }
                }
            }

            //Draw on only the updated locations
            foreach (ConsoleChar cc in changes)
            {
                Console.SetCursorPosition(cc.XPos, cc.YPos);
                Console.BackgroundColor = cc.BackgroundColor;
                Console.ForegroundColor = cc.ForegroundColor;
                Console.Write(cc.Char);
            }

            //Copy newScreen to previousScreen
            for (int w = 0; w < CONSOLE_WIDTH; w++)
            {
                for (int h = 0; h < CONSOLE_HEIGHT - 1; h++)
                {
                    this.previousScreen[w, h] = newScreen[w, h];
                }
            }
        }

        private void BackbufferMap()
        {
            //Draw map
            for (int w = 0; w < GRID_WIDTH; w++)
            {
                for (int h = 0; h < GRID_HEIGHT; h++)
                {
                    newScreen[w, h].BackgroundColor = screenGrid.Grid[w, h].BackgroundColor;
                    newScreen[w, h].ForegroundColor = screenGrid.Grid[w, h].ForegroundColor;
                    newScreen[w, h].Char = screenGrid.Grid[w, h].Char;
                }
            }
        }

        private void BackbufferBaddies()
        {
            foreach (Baddie baddie in state.Baddies)
            {
                baddie.Draw(ref newScreen, screenGrid);
            }
        }

        public void BackbufferDude()
        {
            state.Dude.Draw(ref newScreen, screenGrid);
        }

        //INPUT------------------------------------------------------------------------------------
        private void GetInput(ref bool running)
        {
            keyInfo = Console.ReadKey();
        }

        //UPDATE-----------------------------------------------------------------------------------
        private void UpdateState(ref bool running)
        {
            Bang();
            MoveDude();
            CleanBaddies();
            MoveBaddies();
            EatBrains(ref running);
            
            //Control-shift-Q to quit
            if (keyInfo.Key == ConsoleKey.Q
                && keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift)
                && keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control))
            {
                running = false;
            }
        }

        private void CleanBaddies()
        {
            for (int i = 0; i < state.Baddies.Count; i++)
            {
                if (!state.Baddies[i].Alive)
                {
                    state.Baddies.Remove(state.Baddies[i]);
                }
            }
        }

        //BANG
        private void Bang()
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
                foreach (Baddie baddie in state.Baddies)
                {
                    deltaX = baddie.XPos - state.Dude.XPos;
                    deltaY = baddie.YPos - state.Dude.YPos;
                    //If on the same horizontal line
                    if (state.Dude.YPos == baddie.YPos)
                    {
                        //...and the shot direction matches
                        if (Math.Sign(deltaX) == xDir)
                        {
                            targets.Add(baddie);
                        }
                    }
                    //else if on the same vertical line
                    else if (state.Dude.XPos == baddie.XPos)
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
                    absX = Math.Abs(targets[i].XPos - state.Dude.XPos);
                    absY = Math.Abs(targets[i].YPos - state.Dude.YPos);

                    distance = Math.Sqrt(Math.Pow(absX, 2) + Math.Pow(absY, 2));
                    if (distance < lowestDistance)
                    {
                        lowestDistance = distance;
                        closestIndex = i;
                    }
                }
                if (closestIndex >= 0)
                {
                    targets[closestIndex].Blarg(state.Dude.XPos, state.Dude.YPos, ref screenGrid);
                }
            }
        }

        private void EatBrains(ref bool running)
        {
            foreach (Baddie baddie in state.Baddies)
            {
                if (baddie.XPos == state.Dude.XPos && baddie.YPos == state.Dude.YPos)
                {
                    running = false;
                }
            }
        }

        private void MoveBaddies()
        {   
            foreach (Baddie baddie in state.Baddies)
            {
                baddie.Chase(state.Dude.XPos, state.Dude.YPos);
            }
        }

        private void MoveDude()
        {
            //Movement
            if (keyInfo.Key == ConsoleKey.W)
            {
                state.Dude.YPos--;
            }
            else if (keyInfo.Key == ConsoleKey.S)
            {
                state.Dude.YPos++;
            }
            else if (keyInfo.Key == ConsoleKey.A)
            {
                state.Dude.XPos--;
            }
            else if (keyInfo.Key == ConsoleKey.D)
            {
                state.Dude.XPos++;
            }

            //Constrain dimensions
            if (state.Dude.XPos < 0)
            {
                state.Dude.XPos = 0;
            }
            else if (state.Dude.XPos > GRID_WIDTH - 1)
            {
                state.Dude.XPos = GRID_WIDTH - 1;
            }
            if (state.Dude.YPos < 0)
            {
                state.Dude.YPos = 0;
            }
            else if (state.Dude.YPos > GRID_HEIGHT - 1)
            {
                state.Dude.YPos = GRID_HEIGHT - 1;
            }
        }
    }
}

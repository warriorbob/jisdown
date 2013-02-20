using System;
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
                    interruptEvents.Dequeue().DoStuff(interruptEvents, state);
                }
                else
                {
                    GetInput(ref running);
                    state.UpdateState(ref running, keyInfo, GRID_WIDTH, GRID_HEIGHT, screenGrid);

                    //Control-shift-Q to quit
                    if (keyInfo.Key == ConsoleKey.Q
                        && keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift)
                        && keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control))
                    {
                        running = false;
                    }
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
    }
}

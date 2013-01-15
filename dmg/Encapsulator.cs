using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dmg.Domain;

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
        
        private Dude dude;
        private List<Baddie> baddies;
        private ScreenGrid screenGrid;

        private ConsoleKeyInfo keyInfo;
        public ConsoleChar[,] newScreen;
        public ConsoleChar[,] previousScreen;

        public Encapsulator(int width, int height)
        {
            //Infrastructure
            GRID_WIDTH = width;
            GRID_HEIGHT = height;
            screenGrid = new ScreenGrid(GRID_WIDTH, GRID_HEIGHT);
            keyInfo = new ConsoleKeyInfo();
            newScreen = new ConsoleChar[CONSOLE_WIDTH, CONSOLE_HEIGHT];
            previousScreen = new ConsoleChar[CONSOLE_WIDTH, CONSOLE_HEIGHT];

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
            dude = new Dude(0, 0);
            baddies = new List<Baddie>();
            baddies.Add(new Baddie(20, 15));
            baddies.Add(new Baddie(22, 15));
            baddies.Add(new Baddie(25, 15));
        }

        public void Go()
        {
            bool running = true;

            while (running == true)
            {
                Draw();
                HandleInput(ref running);
                UpdateState();
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
        /// <param name="screenGrid"></param>
        /// <param name="dude"></param>
        public void Draw()
        {
            BufferMap();
            BufferBaddies();
            BufferDude();

            DrawFromBuffers(newScreen, previousScreen);
            
            //Reinitialize newScreen to black spaces
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

            //Reposition cursor
            Console.SetCursorPosition(0, CONSOLE_HEIGHT-1);
        }

        public void DrawFromBuffers(ConsoleChar[,] newScreen, ConsoleChar[,] previousScreen)
        {
           List<ConsoleChar> changes = new List<ConsoleChar>();

            for (int w = 0; w < CONSOLE_WIDTH; w++)
            {
                for (int h = 0; h < CONSOLE_HEIGHT - 1; h++)
                {
                    if (newScreen[w, h].Char != previousScreen[w, h].Char)
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

        private void BufferMap()
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

        private void BufferBaddies()
        {
            foreach (Baddie baddie in baddies)
            {
                newScreen[baddie.XPos, baddie.YPos].BackgroundColor = screenGrid.Grid[baddie.XPos, baddie.YPos].BackgroundColor;
                newScreen[baddie.XPos, baddie.YPos].ForegroundColor = baddie.Color;
                newScreen[baddie.XPos, baddie.YPos].Char = baddie.Char;
            }
        }

        public void BufferDude()
        {
            newScreen[dude.XPos, dude.YPos].BackgroundColor = screenGrid.Grid[dude.XPos, dude.YPos].BackgroundColor;
            newScreen[dude.XPos, dude.YPos].ForegroundColor = dude.Color;
            newScreen[dude.XPos, dude.YPos].Char = dude.Char;
        }

        //INPUT------------------------------------------------------------------------------------
        private void HandleInput(ref bool running)
        {
            keyInfo = Console.ReadKey();
            //Control-shift-Q to quit
            if (keyInfo.Key == ConsoleKey.Q
                && keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift)
                && keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control))
            {
                running = false;
            }
        }

        //UPDATE-----------------------------------------------------------------------------------
        private void UpdateState()
        {
            MoveDude();
            MoveBaddies();
        }

        private void MoveBaddies()
        {
            foreach (Baddie baddie in baddies)
            {
                //TODO: Cast a line to the dude, and move one space towards him (assuming nothing's in the way)
            }
        }

        private void MoveDude()
        {
            //Movement
            if (keyInfo.Key == ConsoleKey.W)
            {
                dude.YPos--;
            }
            else if (keyInfo.Key == ConsoleKey.S)
            {
                dude.YPos++;
            }
            else if (keyInfo.Key == ConsoleKey.A)
            {
                dude.XPos--;
            }
            else if (keyInfo.Key == ConsoleKey.D)
            {
                dude.XPos++;
            }

            //Constrain dimensions
            if (dude.XPos < 0)
            {
                dude.XPos = 0;
            }
            else if (dude.XPos > GRID_WIDTH - 1)
            {
                dude.XPos = GRID_WIDTH - 1;
            }

            if (dude.YPos < 0)
            {
                dude.YPos = 0;
            }
            else if (dude.YPos > GRID_HEIGHT - 1)
            {
                dude.YPos = GRID_HEIGHT - 1;
            }
        }
    }
}

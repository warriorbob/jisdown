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
        private ConsoleChar[,] newScreen;
        private ConsoleChar[,] previousScreen;

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
            dude = new Dude(78, 0);
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
        private void Draw()
        {
            DrawMap();
            DrawBaddies();
            DrawDude();
            DrawFromBuffers(newScreen, previousScreen);
        }

        private void DrawFromBuffers(ConsoleChar[,] newScreen, ConsoleChar[,] previousScreen)
        {
            for (int w = 0; w < CONSOLE_WIDTH; w++)
            {
                for (int h = 0; h < CONSOLE_HEIGHT-1; h++)
                {
                    Console.SetCursorPosition(w, h);
                    Console.BackgroundColor = newScreen[w, h].BackgroundColor;
                    Console.ForegroundColor = newScreen[w, h].ForegroundColor;
                    Console.Write(newScreen[w, h].Char);
                }
            }
        }

        private void DrawMap()
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

        private void DrawBaddies()
        {
            foreach (Baddie baddie in baddies)
            {
                newScreen[baddie.XPos, baddie.YPos].BackgroundColor = screenGrid.Grid[baddie.XPos, baddie.YPos].BackgroundColor;
                newScreen[baddie.XPos, baddie.YPos].ForegroundColor = baddie.Color;
                newScreen[baddie.XPos, baddie.YPos].Char = baddie.Char;
            }
        }

        private void DrawDude()
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

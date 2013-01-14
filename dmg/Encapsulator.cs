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
        private int GRID_WIDTH = 80;
        private int GRID_HEIGHT = 24;
        
        private Dude dude;
        private List<Baddie> baddies;
        private ScreenGrid screenGrid;

        private ConsoleKeyInfo keyInfo;

        public Encapsulator(int width, int height)
        {
            //Infrastructure
            GRID_WIDTH = width;
            GRID_HEIGHT = height;
            screenGrid = new ScreenGrid(GRID_WIDTH, GRID_HEIGHT);
            keyInfo = new ConsoleKeyInfo();
            
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
        }

        private void DrawMap()
        {
            //Draw map
            for (int w = 0; w < GRID_WIDTH; w++)
            {
                for (int h = 0; h < GRID_HEIGHT; h++)
                {
                    Console.SetCursorPosition(w, h);
                    Console.BackgroundColor = screenGrid.Grid[w, h].BackgroundColor;
                    Console.ForegroundColor = screenGrid.Grid[w, h].ForegroundColor;
                    Console.Write(screenGrid.Grid[w, h].Char);
                }
            }
        }

        private void DrawBaddies()
        {
            foreach (Baddie baddie in baddies)
            {
                Console.SetCursorPosition(baddie.XPos, baddie.YPos);
                Console.BackgroundColor = screenGrid.Grid[baddie.XPos, baddie.YPos].BackgroundColor;
                Console.ForegroundColor = baddie.Color;
                Console.Write(baddie.Char);
            }
        }

        private void DrawDude()
        {
            Console.SetCursorPosition(dude.XPos, dude.YPos);
            Console.BackgroundColor = screenGrid.Grid[dude.XPos, dude.YPos].BackgroundColor;
            Console.ForegroundColor = dude.Color;
            Console.Write(dude.Char);
            Console.SetCursorPosition(0, 24);
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

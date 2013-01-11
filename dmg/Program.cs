using dmg.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmg
{
    class Program
    {
        private const int GRID_WIDTH = 80;
        private const int GRID_HEIGHT = 24;

        private ConsoleKeyInfo keyInfo;

        static void Main(string[] args)
        {
            ScreenGrid screenGrid = new ScreenGrid(GRID_WIDTH, GRID_HEIGHT);
            
            //PREP SOME STUFF
            Dude dude = new Dude(78, 0);
            List<Baddie> baddies = new List<Baddie>();
            baddies.Add(new Baddie(20, 15));
            baddies.Add(new Baddie(22, 15));
            baddies.Add(new Baddie(25, 15));

            ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();
            bool running = true;

            while (running == true)
            {
                Draw(screenGrid, dude, baddies);
                HandleInput(ref keyInfo, ref running);
                UpdateState(keyInfo, ref dude, ref baddies);
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
        private static void Draw(ScreenGrid screenGrid, Dude dude, List<Baddie> baddies)
        {
            DrawMap(screenGrid);
            DrawBaddies(screenGrid, baddies);
            DrawDude(dude, screenGrid);
        }

        private static void DrawMap(ScreenGrid screenGrid)
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

        public static void DrawBaddies(ScreenGrid screenGrid, List<Baddie> baddies)
        {
            foreach (Baddie baddie in baddies)
            {
                Console.SetCursorPosition(baddie.XPos, baddie.YPos);
                Console.BackgroundColor = screenGrid.Grid[baddie.XPos, baddie.YPos].BackgroundColor;
                Console.ForegroundColor = baddie.Color;
                Console.Write(baddie.Char);
            }
        }

        public static void DrawDude(Dude dude, ScreenGrid screenGrid)
        {
            Console.SetCursorPosition(dude.XPos, dude.YPos);
            Console.BackgroundColor = screenGrid.Grid[dude.XPos, dude.YPos].BackgroundColor;
            Console.ForegroundColor = dude.Color;
            Console.Write(dude.Char);
            Console.SetCursorPosition(0, 24);
        }

        //INPUT------------------------------------------------------------------------------------
        private static void HandleInput(ref ConsoleKeyInfo keyInfo, ref bool running)
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
        private static void UpdateState(ConsoleKeyInfo keyInfo, ref Dude dude, ref List<Baddie> baddies)
        {
            MoveDude(keyInfo, ref dude);
            MoveBaddies(ref dude, ref baddies);
        }

        private static void MoveBaddies(ref Dude dude, ref List<Baddie> baddies)
        {
            foreach (Baddie baddie in baddies)
            {
                //TODO: Cast a line to the dude, and move one space towards him (assuming nothing's in the way)

            }
        }

        private static void MoveDude(ConsoleKeyInfo keyInfo, ref Dude dude)
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

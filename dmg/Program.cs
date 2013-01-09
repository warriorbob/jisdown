﻿using dmg.Domain;
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

        static void Main(string[] args)
        {
            ScreenGrid screenGrid = new ScreenGrid(GRID_WIDTH, GRID_HEIGHT);

            //Draw map
            for(int w = 0; w < GRID_WIDTH; w++)
            {
                for (int h = 0; h < GRID_HEIGHT; h++)
                {
                    Console.SetCursorPosition(w, h);
                    Console.BackgroundColor = screenGrid.Grid[w, h].BackgroundColor;
                    Console.ForegroundColor = screenGrid.Grid[w, h].ForegroundColor;
                    Console.Write(screenGrid.Grid[w, h].Char);
                }
            }

            //TODO: Draw dude
        }
    }
}

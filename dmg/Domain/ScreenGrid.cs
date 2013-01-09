using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmg.Domain
{
    /// <summary>
    /// Represents a grid of tiles
    /// </summary>
    public class ScreenGrid
    {
        public Tile[,] Grid;

        public ScreenGrid(int width, int height)
        {
            Grid = new Tile[width, height];
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    Grid[w, h] = new Tile {Char = '.'};
                }
            }
        }
    }
}

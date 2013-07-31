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
    public class Map
    {
        public Tile[,] Grid;
        public int Width { get { return Grid.GetLength(0); } }
        public int Height { get { return Grid.GetLength(1); } }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="width">Width of the grid</param>
        /// <param name="height">Height of the grid</param>
        public Map(int width, int height)
        {
            Grid = new Tile[width, height];
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    Grid[w, h] = new Tile('.', ConsoleColor.Black, ConsoleColor.DarkGray);
                }
            }
        }

        public void Draw(ref ConsoleChar[,] screen, int width, int height)
        {
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    screen[w, h].BackgroundColor = Grid[w, h].BackgroundColor;
                    screen[w, h].ForegroundColor = Grid[w, h].ForegroundColor;
                    screen[w, h].Char = Grid[w, h].Char;
                }
            }
        }

        public void StainBackground(int x, int y, ConsoleColor color)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
                Grid[x, y].BackgroundColor = color;
        }

        public void StainForeground(int x, int y, ConsoleColor color)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
                Grid[x, y].ForegroundColor = color;
        }
    }
}

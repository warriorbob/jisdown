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
    class ScreenGrid
    {
        public List<Tile>[,] Grid { get; set; }

        public ScreenGrid(int width, int height)
        {
            Grid = new List<Tile>[width, height];
            foreach (List<Tile> tileList in Grid)
            {
                tileList.Add(new Tile { Char = '.' });
            }
        }
    }
}

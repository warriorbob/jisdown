using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmg.Domain
{
    public class Tile
    {
        public char Char { get; set; }
        public TileStain[] Stains { get; set; }
        private ConsoleColor defaultBackgroundColor;
        private ConsoleColor defaultForegroundColor;
        public ConsoleColor BackgroundColor 
        { 
            get { return defaultBackgroundColor; }
            set { defaultBackgroundColor = value; }
        }
        public ConsoleColor ForegroundColor
        {
            get { return defaultForegroundColor; }
            set { defaultForegroundColor = value; }
        }

        public Tile(char initChar, ConsoleColor defaultBackColor, ConsoleColor defaultForeColor)
        {
            Char = initChar;
            defaultBackgroundColor = defaultBackColor;
            defaultForegroundColor = defaultForeColor;
        }
    }
}

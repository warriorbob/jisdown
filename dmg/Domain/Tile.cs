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
        public List<TileStain> Stains { get; set; }
        public ConsoleColor DefaultBackgroundColor;
        public ConsoleColor DefaultForegroundColor;
        
        //TODO: Latest stain color, if any, reported in place of default colors
        public ConsoleColor BackgroundColor 
        { 
            get { return DefaultBackgroundColor; }
            set { DefaultBackgroundColor = value; }
        }
        public ConsoleColor ForegroundColor
        {
            get { return DefaultForegroundColor; }
            set { DefaultForegroundColor = value; }
        }

        public Tile(char initChar, ConsoleColor defaultBackColor, ConsoleColor defaultForeColor)
        {
            Char = initChar;
            DefaultBackgroundColor = defaultBackColor;
            DefaultForegroundColor = defaultForeColor;
        }
    }
}

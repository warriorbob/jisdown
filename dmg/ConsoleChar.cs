using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmg
{
    public class ConsoleChar
    {
        public char Char { get; set; }
        public ConsoleColor BackgroundColor { get; set; }
        public ConsoleColor ForegroundColor { get; set; }
        public int XPos { get; set; }
        public int YPos { get; set; }

        public bool Matches(ConsoleChar other)
        {
            return (
                Char == other.Char
                && BackgroundColor == other.BackgroundColor
                && ForegroundColor == other.ForegroundColor
                );
        }
    }
}

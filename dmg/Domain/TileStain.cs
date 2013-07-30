using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmg.Domain
{
    public class TileStain
    {
        public ConsoleColor HighColor { get; set; }
        public ConsoleColor LowColor { get; set; }
        private int stainLevel;
        public int StainLevel
        {
            get { return stainLevel; }
            set 
            {
                if (value < 0)
                {
                    stainLevel = 0;
                }
                else if (value > 3)
                {
                    stainLevel = 3;
                }
                else
                {
                    stainLevel = value;
                }
            }   
        }

        public TileStain(ConsoleColor highColor, ConsoleColor lowColor, int stainLevel)
        {
            HighColor = highColor;
            LowColor = lowColor;
            StainLevel = stainLevel;
        }
    }
}

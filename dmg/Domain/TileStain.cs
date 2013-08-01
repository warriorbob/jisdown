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
        public int TurnsUntilDecay { get; set; }
        private Dictionary<int, int> AgeThresholds; //How long each level lasts before decaying
        private int stainLevel;
        public int StainLevel   //Should be constrained to [0-3]
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
                TurnsUntilDecay = AgeThresholds[stainLevel];
            }   
        }

        public TileStain(ConsoleColor highColor, ConsoleColor lowColor, int stainLevel)
        {
            HighColor = highColor;
            LowColor = lowColor;
            AgeThresholds = new Dictionary<int,int>();
            AgeThresholds.Add(3, 5);
            AgeThresholds.Add(2, 5);
            AgeThresholds.Add(1, 10);
            AgeThresholds.Add(0, 20);
            StainLevel = stainLevel;
        }

        public ConsoleColor ResultantBackgroundColor(ConsoleColor defaultColor)
        {
            throw new NotImplementedException();
        }

        public ConsoleColor ResultantForegroundColor(ConsoleColor defaultColor)
        {
            throw new NotImplementedException();
        }
    }
}

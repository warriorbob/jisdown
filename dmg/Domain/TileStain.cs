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
        public int Age { get; set; }
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
            }   
        }

        public TileStain(ConsoleColor highColor, ConsoleColor lowColor, int stainLevel)
        {
            HighColor = highColor;
            LowColor = lowColor;
            StainLevel = stainLevel;
            Age = 0;
            AgeThresholds = new Dictionary<int,int>();
            AgeThresholds.Add(0, 5);
            AgeThresholds.Add(1, 7);
            AgeThresholds.Add(2, 10);
            AgeThresholds.Add(3, 25);
        }

        public ConsoleColor ResultantBackgroundColor()
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmg
{
    public class HighScoreManager
    {
        public List<Tuple<string, int>> highScores { get; set; }

        public HighScoreManager()
        {
            highScores = new List<Tuple<string, int>>();
        }
    }
}

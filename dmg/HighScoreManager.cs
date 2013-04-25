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
        public int CursorPosition { get; set; }
        public string Initials { get; set; }

        public HighScoreManager()
        {
            highScores = new List<Tuple<string, int>>();
            ResetInitialsInput();
        }

        public bool IsInTopTen(int newScore)
        {
            return highScores.Count < 10 || newScore >= highScores[9].Item2;
        }

        public void ResetInitialsInput()
        {
            Initials = "░░░";
            CursorPosition = 0;
        }
    }
}
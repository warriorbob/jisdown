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
        private int _cursorPosition;
        public int CursorPosition {
            get { return _cursorPosition; }  
            set { _cursorPosition = value > 2 ? 2 : value; }
        }
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
            _cursorPosition = 0;
        }

        internal void AddInitial(char newInital)
        {
            var newInitials = Initials.ToCharArray();
            newInitials[_cursorPosition] = newInital;
            Initials = "";
            foreach (char c in newInitials)
            {
                Initials += c.ToString();
            }
            CursorPosition++;
        }
    }
}
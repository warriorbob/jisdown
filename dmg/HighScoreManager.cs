using System;
using System.Collections.Generic;
using System.IO;
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
            set 
            {
                if (value > 2)
                    _cursorPosition = 2;
                else if (value < 0)
                    _cursorPosition = 0;
                else
                    _cursorPosition = value;
            }
        }
        public string Initials { get; set; }
        private const string HIGHSCORE_FILE_NAME = "topten.lst";

        /// <summary>
        /// Constructor
        /// </summary>
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

        public void AddInitial(char newInital)
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

        public void BackUp()
        {
            var newInitials = Initials.ToCharArray();
            newInitials[_cursorPosition] = '░';
            Initials = "";
            foreach (char c in newInitials)
            {
                Initials += c.ToString();
            }
            CursorPosition--;
        }

        public void SortHighScores()
        {
            highScores.Sort((x, y) => x.Item2.CompareTo(y.Item2));
            highScores.Reverse();
        }
    }
}
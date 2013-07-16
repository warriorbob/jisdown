using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmg.Domain
{
    public interface IBaddie
    {
        int XPos { get; set; }
        int YPos { get; set; }
        ConsoleColor Color { get; set; }
        char Char { get; set; }
        bool Alive { get; set; }

        void Chase(int targetX, int targetY);
        void Draw(ref ConsoleChar[,] screen, Map screengrid);
        void Blarg(int fromX, int fromY, Map map);
    }
}

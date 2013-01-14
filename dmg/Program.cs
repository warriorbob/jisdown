using dmg.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmg
{
    class Program
    {
        private const int GRID_WIDTH = 80;
        private const int GRID_HEIGHT = 24;

        static void Main(string[] args)
        {
            Encapsulator enc = new Encapsulator(GRID_WIDTH, GRID_HEIGHT);
            enc.Go();
        }
    }
}

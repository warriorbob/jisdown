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
        private const int CONSOLE_WIDTH = 80;
        private const int CONSOLE_HEIGHT = 25;

        static void Main(string[] args)
        {
            ScreenGrid screenGrid = new ScreenGrid(CONSOLE_WIDTH, CONSOLE_HEIGHT);

            for(int i = 0; i < CONSOLE_WIDTH; i++)
            {
                for (int j = 0; j < CONSOLE_HEIGHT; j++)
                {

                }
            }
        }
    }
}

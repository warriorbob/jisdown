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
            //enc.BufferDude();
            //enc.DrawFromBuffers(enc.newScreen, enc.previousScreen);
            //enc.DrawFromBuffers(enc.previousScreen, enc.newScreen);
            //for (int w = 0; w < 80; w++)
            //{
            //    for (int h = 0; h < 25 - 1; h++)
            //    {
            //        Console.SetCursorPosition(w, h);
            //        Console.BackgroundColor = enc.previousScreen[w, h].BackgroundColor;
            //        Console.ForegroundColor = enc.previousScreen[w, h].ForegroundColor;
            //        Console.Write(enc.previousScreen[w, h].Char);
            //    }
            //}
        }
    }
}

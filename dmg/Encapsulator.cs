using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dmg.Domain;
using dmg.Interrupt;

namespace dmg
{
    /// <summary>
    /// Encapsulates everything into an instanceable object
    /// </summary>
    class Encapsulator
    {
        private const int CONSOLE_WIDTH = 80;
        private const int CONSOLE_HEIGHT = 25;

        private int GRID_WIDTH = 80;
        private int GRID_HEIGHT = 24;

        private const int INITIAL_BADDIE_COUNT = 10;
        private StateManager stateManager;

        private Map theMap;

        private ConsoleKeyInfo keyInfo;
        public ConsoleChar[,] newScreen;
        public ConsoleChar[,] previousScreen;

        /// <summary>
        /// Constructor. Sets up infrastructure data and initial state.
        /// </summary>
        /// <param name="width">ScreenGrid width</param>
        /// <param name="height">ScreenGrid height</param>
        public Encapsulator(int width, int height)
        {
            //Infrastructure
            stateManager = new StateManager();
            GRID_WIDTH = width;
            GRID_HEIGHT = height;
            theMap = new Map(GRID_WIDTH, GRID_HEIGHT);
            keyInfo = new ConsoleKeyInfo();
            newScreen = new ConsoleChar[CONSOLE_WIDTH, CONSOLE_HEIGHT];
            previousScreen = new ConsoleChar[CONSOLE_WIDTH, CONSOLE_HEIGHT];
            stateManager.InterruptEvents = new Queue<IInterruptEvent>();
            stateManager.SpawnTimer = 0;
            stateManager.Score = 0;

            //Initialize screenbuffers
            for (int w = 0; w < CONSOLE_WIDTH; w++)
            {
                for (int h = 0; h < CONSOLE_HEIGHT; h++)
                {
                    newScreen[w, h] = new ConsoleChar();
                    newScreen[w, h].Char = ' ';
                    newScreen[w, h].BackgroundColor = ConsoleColor.Black;
                    newScreen[w, h].ForegroundColor = ConsoleColor.White;
                    previousScreen[w, h] = new ConsoleChar();
                    previousScreen[w, h].Char = ' ';
                    previousScreen[w, h].BackgroundColor = ConsoleColor.Black;
                    previousScreen[w, h].ForegroundColor = ConsoleColor.White;
                }
            }

            //Entities
            Random rand = new Random();
            stateManager.Baddies = new List<Baddie>();
            for (int i = 0; i < INITIAL_BADDIE_COUNT; i++)
            {
                stateManager.Baddies.Add(new Baddie(rand.Next(0, GRID_WIDTH), rand.Next(0, GRID_HEIGHT)));
            }
            
            stateManager.Dude = new Dude(rand.Next(0,GRID_WIDTH), rand.Next(0,GRID_HEIGHT));
            foreach (Baddie baddie in stateManager.Baddies)
            {
                if (stateManager.Dude.XPos == baddie.XPos && stateManager.Dude.YPos == baddie.YPos)
                {
                    stateManager.Dude.XPos = rand.Next(0, GRID_WIDTH);
                    stateManager.Dude.YPos = rand.Next(0, GRID_HEIGHT);
                }
            }
            stateManager.Shots = new List<Shot>();
        }

        /// <summary>
        /// This is the method with the main loop in it.
        /// </summary>
        public void Go()
        {
            //Draw initial state once outside of the loop
            Draw();

            //Main loop
            bool running = true;
            while (running == true)
            {
                stateManager.CleanBaddies();
                Draw();
                if (stateManager.InterruptEvents.Count > 0)
                {
                    stateManager.InterruptEvents.Dequeue().DoStuff(stateManager.InterruptEvents, stateManager, ref theMap);
                }
                else
                {
                    GetInput(ref running);
                    stateManager.UpdateState(ref running, keyInfo, GRID_WIDTH, GRID_HEIGHT, theMap);

                    //Quit if the user presses Ctrl+Shift+q
                    if (keyInfo.Key == ConsoleKey.Q
                        && keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift)
                        && keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control))
                    {
                        running = false;
                    }
                }

                //Reinitialize input
                keyInfo = new ConsoleKeyInfo();
            }

            //"Press any key to continue" when we're done
            Console.SetCursorPosition(0, 24);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        //DRAWING ---------------------------------------------------------------------------------
        /// <summary>
        /// Draw all the things
        /// </summary>
        public void Draw()
        {
            BackbufferMap();
            BackbufferDude();
            BackbufferBaddies();
            BackbufferShots();
            backbufferMessageBar();
            
            DrawFromBuffers(newScreen, previousScreen);

            InitializeNewScreen();

            //Reposition cursor in the lower-left after drawing so
            //we can write status messages or whatever
            Console.SetCursorPosition(0, CONSOLE_HEIGHT-1);
        }

        private void BackbufferMap()
        {
            theMap.Draw(ref newScreen, GRID_WIDTH, GRID_HEIGHT);
        }

        public void BackbufferDude()
        {
            stateManager.Dude.Draw(ref newScreen, theMap);
        }

        private void BackbufferBaddies()
        {
            foreach (Baddie baddie in stateManager.Baddies)
            {
                baddie.Draw(ref newScreen, theMap);
            }
        }

        private void BackbufferShots()
        {
            foreach (Shot shot in stateManager.Shots)
            {
                shot.Draw(ref newScreen, theMap);
            }
        }

        private void backbufferMessageBar()
        {
            string scoreLabel = "SCORE: ";
            for (int i = 0; i < scoreLabel.Length; i++)
            {
                newScreen[i, CONSOLE_HEIGHT - 1].BackgroundColor = ConsoleColor.Black;
                newScreen[i, CONSOLE_HEIGHT - 1].ForegroundColor = ConsoleColor.White;
                newScreen[i, CONSOLE_HEIGHT - 1].Char = scoreLabel[i];
            }

            for (int i = scoreLabel.Length; i < scoreLabel.Length + stateManager.Score.ToString().Length; i++)
            {
                newScreen[i, CONSOLE_HEIGHT - 1].BackgroundColor = ConsoleColor.White;
                newScreen[i, CONSOLE_HEIGHT - 1].ForegroundColor = ConsoleColor.Black;
                newScreen[i, CONSOLE_HEIGHT - 1].Char = stateManager.Score.ToString()[i - scoreLabel.Length];
            }
        }

        //Fast console-drawing backbuffering technique
        public void DrawFromBuffers(ConsoleChar[,] newScreen, ConsoleChar[,] previousScreen)
        {
           List<ConsoleChar> changes = new List<ConsoleChar>();

            for (int w = 0; w < CONSOLE_WIDTH; w++)
            {
                for (int h = 0; h < CONSOLE_HEIGHT; h++)
                {
                    if (!newScreen[w, h].Matches(previousScreen[w, h]))
                    {
                        changes.Add(new ConsoleChar
                        {
                            Char = newScreen[w, h].Char,
                            BackgroundColor = newScreen[w, h].BackgroundColor,
                            ForegroundColor = newScreen[w, h].ForegroundColor,
                            XPos = w,
                            YPos = h
                        });
                    }
                }
            }

            //Draw on only the updated locations
            foreach (ConsoleChar cc in changes)
            {
                Console.SetCursorPosition(cc.XPos, cc.YPos);
                Console.BackgroundColor = cc.BackgroundColor;
                Console.ForegroundColor = cc.ForegroundColor;
                Console.Write(cc.Char);
            }

            //Copy newScreen to previousScreen
            for (int w = 0; w < CONSOLE_WIDTH; w++)
            {
                for (int h = 0; h < CONSOLE_HEIGHT - 1; h++)
                {
                    this.previousScreen[w, h] = newScreen[w, h];
                }
            }
        }

        ///Reinitializes newScreen to black spaces
        private void InitializeNewScreen()
        {
            for (int w = 0; w < CONSOLE_WIDTH; w++)
            {
                for (int h = 0; h < CONSOLE_HEIGHT; h++)
                {
                    newScreen[w, h] = new ConsoleChar();
                    newScreen[w, h].Char = ' ';
                    newScreen[w, h].BackgroundColor = ConsoleColor.Black;
                    newScreen[w, h].ForegroundColor = ConsoleColor.White;
                }
            }
        }

        //INPUT------------------------------------------------------------------------------------
        private void GetInput(ref bool running)
        {
            keyInfo = Console.ReadKey();
        }
    }
}

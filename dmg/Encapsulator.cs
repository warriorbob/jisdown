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
        private const int INITIAL_BADDIE_COUNT = 15;

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
            ResetState(StateManager.GameStates.TitleScreen);
            GRID_WIDTH = width;
            GRID_HEIGHT = height;
            keyInfo = new ConsoleKeyInfo();
            newScreen = new ConsoleChar[CONSOLE_WIDTH, CONSOLE_HEIGHT];
            previousScreen = new ConsoleChar[CONSOLE_WIDTH, CONSOLE_HEIGHT];

            InitializeScreenbuffers();
        }

        private void InitializeScreenbuffers()
        {
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
        }

        private void ResetState(StateManager.GameStates newGameState = StateManager.GameStates.TitleScreen)
        {
            stateManager = new StateManager(
                new List<IBaddie>(),
                new Queue<IInterruptEvent>(),
                new List<Shot>(),
                0,
                0,
                newGameState
            );

            theMap = new Map(GRID_WIDTH, GRID_HEIGHT);

            //Entities
            Random rand = new Random();
            for (int i = 0; i < INITIAL_BADDIE_COUNT; i++)
            {
                stateManager.Baddies.Add(new Baddie(rand.Next(0, GRID_WIDTH), rand.Next(0, GRID_HEIGHT)));
            }

            stateManager.Dude = new Dude(rand.Next(0, GRID_WIDTH), rand.Next(0, GRID_HEIGHT));
            foreach (Baddie baddie in stateManager.Baddies)
            {
                if (stateManager.Dude.XPos == baddie.XPos && stateManager.Dude.YPos == baddie.YPos)
                {
                    stateManager.Dude.XPos = rand.Next(0, GRID_WIDTH);
                    stateManager.Dude.YPos = rand.Next(0, GRID_HEIGHT);
                }
            }
        }

        /// <summary>
        /// This is the method with the main loop in it.
        /// </summary>
        public void Go()
        {
            //Main loop
            bool running = true;
            while (running == true)
            {
                switch(stateManager.CurrentGameState)
                {
                    case StateManager.GameStates.TitleScreen:
                        GoTitleScreen();
                        break;

                    case StateManager.GameStates.Playing:
                        GoPlaying(ref running);
                        break;

                    case StateManager.GameStates.Dead:
                        GoDead();
                        break;

                    default:
                        break;
                }
            }

            //"Press any key to continue" when we're done
            Console.SetCursorPosition(0, 24);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        private void GoTitleScreen()
        {
            DrawTitleScreen();
            GetInput();
            if (keyInfo.Key == ConsoleKey.Enter)
            {
                stateManager.CurrentGameState = StateManager.GameStates.Playing;
            }
        }

        private void GoPlaying(ref bool running)
        {
            stateManager.CleanBaddies();
            DrawPlaying();
            if (stateManager.InterruptEvents.Count > 0)
            {
                stateManager.InterruptEvents.Dequeue().DoStuff(stateManager.InterruptEvents, stateManager, ref theMap);
            }
            else
            {
                GetInput();
                stateManager.UpdateState(ref running, keyInfo, GRID_WIDTH, GRID_HEIGHT, theMap);

                //Quit if the user presses Ctrl+Shift+q
                if (keyInfo.Key == ConsoleKey.Q
                    && keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift)
                    && keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control))
                {
                    running = false;
                }

                //Reset if the user presses Ctrl+Shift+p
                if (keyInfo.Key == ConsoleKey.P
                    && keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift)
                    && keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control))
                {
                    ResetState(StateManager.GameStates.Playing);
                }
            }
            
            //End if user is dead
            if (stateManager.Dude.Alive == false)
            {
                stateManager.CurrentGameState = StateManager.GameStates.Dead;
            }

            //Reinitialize input
            keyInfo = new ConsoleKeyInfo();
        }

        private void GoDead()
        {
            stateManager.CleanBaddies();
            DrawDead();
            GetInput();
            if (keyInfo.Key == ConsoleKey.Enter)
            {
                ResetState(StateManager.GameStates.Playing);
                stateManager.CurrentGameState = StateManager.GameStates.Playing;
            }
        }

        //DRAWING ---------------------------------------------------------------------------------

        public void DrawTitleScreen()
        {
            string title = "((( J IS DOWN )))";
            int titleLeft = (CONSOLE_WIDTH - title.Length * 2) / 2;

            for (int i = 0; i < title.Length; i++)
            {
                newScreen[titleLeft + 2 * i, 5].Char = title[i];
            }

            string subtitle = "Press Enter to begin";
            int subtitleLeft = (CONSOLE_WIDTH - subtitle.Length) / 2;
            for (int i = 0; i < subtitle.Length; i++)
            {
                newScreen[subtitleLeft + i, 7].Char = subtitle[i];
            }

            string protip = "Protip: 'j' is down.";
            for (int i = 0; i < protip.Length; i++)
            {
                newScreen[i + 50, 24].ForegroundColor = ConsoleColor.DarkGray;
                newScreen[i + 50, 24].Char = protip[i];
            }

            DrawFromBuffers(newScreen, previousScreen);
            InitializeNewScreen();
            Console.SetCursorPosition(0, CONSOLE_HEIGHT - 1);
        }
        /// <summary>
        /// Draw all the things
        /// </summary>
        public void DrawPlaying()
        {
            BackbufferMap();
            BackbufferDude();
            BackbufferBaddies();
            BackbufferShots();
            backbufferMessageBar();
            
            DrawFromBuffers(newScreen, previousScreen);
            InitializeNewScreen();
            Console.SetCursorPosition(0, CONSOLE_HEIGHT-1);
        } 

        private void DrawDead()
        {
            BackbufferMap();
            BackbufferDude();
            BackbufferBaddies();
            BackbufferShots();
            backbufferMessageBar();

            string unfortunateNotification = "YOU ARE DEAD !";
            int posLeft = (CONSOLE_WIDTH - unfortunateNotification.Length) / 2;
            int posTop = 10;
            for (int i = 0; i < unfortunateNotification.Length; i++)
            {
                newScreen[posLeft + i, posTop].BackgroundColor = ConsoleColor.Black;
                newScreen[posLeft + i, posTop].ForegroundColor = ConsoleColor.Red;
                newScreen[posLeft + i, posTop].Char = unfortunateNotification[i];
            }

            unfortunateNotification = "Press enter to restart";
            posLeft = (CONSOLE_WIDTH - unfortunateNotification.Length) / 2;
            posTop = 16;
            for (int i = 0; i < unfortunateNotification.Length; i++)
            {
                newScreen[posLeft + i, posTop].BackgroundColor = ConsoleColor.Black;
                newScreen[posLeft + i, posTop].ForegroundColor = ConsoleColor.Gray;
                newScreen[posLeft + i, posTop].Char = unfortunateNotification[i];
            }

            DrawFromBuffers(newScreen, previousScreen);
            InitializeNewScreen();
            Console.SetCursorPosition(0, CONSOLE_HEIGHT - 1);
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
            for (int i = 0; i < CONSOLE_WIDTH; i++)
            {
                newScreen[i, CONSOLE_HEIGHT - 1].BackgroundColor = ConsoleColor.Black;
                newScreen[i, CONSOLE_HEIGHT - 1].ForegroundColor = ConsoleColor.White;
                newScreen[i, CONSOLE_HEIGHT - 1].Char = ' ';
            }

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
                    if (!newScreen[w, h].Matches(previousScreen[w, h]) 
                        || w == 0 && h == CONSOLE_HEIGHT - 1)   //Force the lower-left char where the console draws input
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
                for (int h = 0; h < CONSOLE_HEIGHT; h++)
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
        private void GetInput()
        {
            keyInfo = Console.ReadKey();
        }
    }
}

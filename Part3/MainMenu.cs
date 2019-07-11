using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Part3
{
    class MainMenu : IGameBase
    {
        public delegate void MenuHandler(object sender, MenuEventArgs e);

        public static event MenuHandler GameChoosed;
        public int Score { get; set; }
        public bool Pause { get; set; }
        public bool[,] FieldToDisplay { get; set; }
        public bool[,] FieldAdditional { get; set; }
        public bool EndGame { get; set; }
        public int FieldHeight { get; set; }
        public int FieldWidth { get; set; }

        private int selectedGame  = 0;
        private int selectedGameType = 1;

        private void printPreview()
        {
            for (int j = 7; j < 13; j++)
            {
                for (int i = 0; i < FieldWidth; i++)
                {
                    FieldToDisplay[i, j] = true;
                }
            }
        }

        private void printGameLetter()
        {
            foreach (var (X, Y) in GamePreview.Games[selectedGame].Letter)
            {
                FieldToDisplay[X + 3, Y + 1] = true;
            }
        }

        private void printGameType()
        {
            foreach ((int X, int Y) in GamePreview.digit0)
            {
                FieldToDisplay[X + 2, Y + 14] = true;
            }

            foreach (var (X, Y) in GamePreview.digit1)
            {
                FieldToDisplay[X + 6, Y + 14] = true;
            }
        }

        public MainMenu(int width = 10, int height = 20)
        {
            FieldToDisplay = new bool[width, height];
            FieldAdditional = new bool[4, 4];
            FieldWidth = width;
            FieldHeight = height;

            printGameLetter();
            printPreview();
            printGameType();

        }

        public void DownKey()
        {
            
        }

        public void FuncKeyPressed()
        {
            GameChoosed(this, new MenuEventArgs(selectedGame));
        }

        public void LeftKey()
        {
            if (selectedGame < GamePreview.Count - 1)
            {
                selectedGame++;
                for (int j = 0; j < FieldHeight; j++)
                {
                    for (int i = 0; i < FieldWidth; i++)
                    {
                        FieldToDisplay[i, j] = false;
                    }
                }
                printGameLetter();
                printPreview();
                printGameType();
            }
        }

        public void RightKey()
        {
            if (selectedGame > 0)
            {
                selectedGame--;
                for (int j = 0; j < FieldHeight; j++)
                {
                    for (int i = 0; i < FieldWidth; i++)
                    {
                        FieldToDisplay[i, j] = false;
                    }
                }
                printGameLetter();
                printPreview();
                printGameType();
            }
        }

        public void UpKey()
        {
            
        }

        public class MenuEventArgs : EventArgs
        {
            public int GameNumber;

            public MenuEventArgs(int gameNumber)
            {
                GameNumber = gameNumber;
            }
        }
        private class GamePreview
        {
            public (int X, int Y)[] Letter { get; set; }
            public (int X, int Y)[] Digit { get; set; }
            public int GameNumber { get; set; }
            public static int Count { get; set; } = 2;

            public static (int X, int Y)[] digit0 = { (0, 0), (1, 0), (2, 0), (0, 1), (2, 1), (0, 2), (2, 2), (0, 3), (2, 3), (0, 4), (1, 4), (2, 4) };
            public static (int X, int Y)[] digit1 = { (1, 0), (0, 1), (1, 1), (1, 2), (1, 3), (0, 4), (1, 4), (2, 4)};

            private static readonly (int X, int Y)[] letterA = { (1, 0), (2, 0), (0, 1), (3, 1), (0, 2), (3, 2), (0, 3), (1, 3), (2, 3), (3, 3), (0, 4), (3, 4) };
            private static readonly (int X, int Y)[] letterB = { (0, 0), (1, 0), (2, 0), (0, 1), (3, 1), (0, 2), (1, 2), (2, 2), (0, 3), (3, 3), (0, 4), (1, 4), (2, 4) };
            private static readonly (int X, int Y)[] letterC = { (1, 0), (2, 0), (0, 1), (3, 0), (0, 2), (0, 3), (3, 4), (1, 4), (2, 4) };
            private static readonly (int X, int Y)[] letterD = { (0, 0), (1, 0), (2, 0), (0, 1), (3, 1), (0, 2), (3, 2), (0, 3), (3, 3), (0, 4), (1, 4), (2, 4) };
            private static readonly (int X, int Y)[] letterE = { (0, 0), (1, 0), (2, 0), (3, 0), (0, 1), (0, 2), (1, 2), (2, 2), (0, 3), (0, 4), (1, 4), (2, 4), (3, 4) };

            public static GamePreview[] Games =
            {
                new GamePreview
                {
                    Letter = letterA,
                    GameNumber = 0
                },

                new GamePreview
                {
                    Letter = letterB,
                    GameNumber = 1
                }
            };

            public GamePreview this[int index]
            {
                get
                {
                    return Games[index];
                }
                set
                {
                    Games[index] = value;
                }
            }

        }

    }
}

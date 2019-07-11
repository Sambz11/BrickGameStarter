using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Part3
{
    class SnakeGameA : IGameBase
    {
        public event EventHandler GameOver;
        public int FieldWidth { get; set; }
        public int FieldHeight { get; set; }
        public int Score { get; set; }
        public bool Pause { get; set; }
        public bool[,] FieldToDisplay { get; set; }
        public bool[,] FieldAdditional { get; set; }
        private bool _endGame = false;
        public bool EndGame
        {
            get => _endGame;
            set
            {
                if (value)
                {
                    timer.Stop();
                    GameOver(this, null);
                } else
                {
                    timer.Start();
                }
                _endGame = value;
            }
        }

        private (int X, int Y) Food;

        System.Timers.Timer timer = new System.Timers.Timer(300);

        

        public SnakeGameA(int width = 10, int height = 20)
        {
            FieldWidth = width;
            FieldHeight = height;
            FieldToDisplay = new bool[width, height];
            FieldAdditional = new bool[4, 4];
            FieldToDisplay.Initialize();
            FieldAdditional.Initialize();
            Snake.Body.Enqueue((4, 17));
            Snake.Body.Enqueue((4, 16));
            Food = NewFood();
            FieldToDisplay[Food.X, Food.Y] = true;
            foreach (var item in Snake.Body)
            {
                FieldToDisplay[item.X, item.Y] = true;
            }
            timer.Elapsed += TimerTick;
            timer.Start();
        }

        private void TimerTick(object sender, System.Timers.ElapsedEventArgs e)
        {
            Debug.WriteLine("Tick1");
            Snake.MoveTo();
            if (Snake.Head == Food)
            {
                Snake.Add();
                Food = NewFood();
            }

            updateDisplay();

        }

        private (int X, int Y) NewFood()
        {


            int newX;
            int newY;

            do
            {
                int seed = new Random().Next(FieldWidth * FieldHeight);

                newX = seed / FieldHeight;
                newY = seed % FieldHeight;

            } while (FieldToDisplay[newX, newY]);

            Debug.WriteLine("New Food = ({0}, {1})", newX, newY);

            return (newX, newY);
        }

        private void updateDisplay()
        {
            for (int j = 0; j < FieldHeight; j++)
            {
                for (int i = 0; i < FieldWidth; i++)
                {
                    FieldToDisplay[i, j] = false;
                }
            }

            foreach (var item in Snake.Body)
            {
                try
                {
                    FieldToDisplay[item.X, item.Y] = true;
                } catch (IndexOutOfRangeException)
                {
                    EndGame = true;
                } 
            }
            FieldToDisplay[Food.X, Food.Y] = true;
        }

        public void DownKey()
        {
            Snake.Direction = 3;
            updateDisplay();
        }

        public void FuncKeyPressed()
        {
            Snake.MoveTo();
            updateDisplay();
        }

        public void LeftKey()
        {
            Snake.Direction = 2;
            updateDisplay();
        }

        public void RightKey()
        {
            Snake.Direction = 4;
            updateDisplay();
        }

        public void UpKey()
        {
            Snake.Direction = 1;
            updateDisplay();
        }

        private static class Snake
        {
            private static bool sectorAdded = false;
            private static int dx = 0;
            private static int dy = -1;
            private static int _direction = 1;
            public static int Direction {
                get => _direction;
                set
                {
                    _direction = value;
                    switch (value)
                    {
                        case 1:
                            dx = 0; dy = -1;
                            break;
                        case 2:
                            dx = -1; dy = 0;
                            break;
                        case 3:
                            dx = 0; dy = 1;
                            break;
                        case 4:
                            dx = 1; dy = 0;
                            break;
                        default:
                            dx = 0; dy = -1;
                            _direction = 1;
                            break;
                    }
                    
                }
            }
            public static Queue<(int X, int Y)> Body = new Queue<(int X, int Y)>();
            public static (int X, int Y) Head
            {
                get
                {
                    return Body.Last();
                }
            }
            public static (int X, int Y) Tail
            {
                get
                {
                    return Body.Peek();
                }
            }

            public static bool MoveTo()
            {
                Body.Enqueue((Head.X + dx, Head.Y + dy));
                if (!sectorAdded)
                    Body.Dequeue();
                else
                    sectorAdded = false;
                return true;
            }

            public static void Add()
            {
                sectorAdded = true;
            }
        }

    }

}

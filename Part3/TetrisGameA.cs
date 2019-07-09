using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Part3
{
    public class TetrisGameA : IGameBase
    {
        public int FieldWidth { get; set; }
        public int FieldHeight { get; set; }
        public bool[,] CurrentField { get; set; } 
        private Figure CurrentFigure { get; set; } = new Figure();
        private Figure NextFigure { get; set; } = new Figure();
        public int Score { get; set; }
        private bool _pause = false;
        public bool Pause
        {
            get { return _pause; }
            set
            {
                if (value)
                {
                    timer.Stop();
                }
                else
                {
                    timer.Start();
                }
                _pause = value;
            }
        }
        public bool[,] FieldToDisplay { get; set; }
        public bool[,] FieldAdditional { get; set; }
        public bool EndGame { get; set; }

        private Timer timer = new Timer(750);
        public int test = 0;

        public TetrisGameA(int width = 10, int height = 20)
        {
            CurrentField = new bool[width, height];
            FieldToDisplay = new bool[width, height];
            // Attention !!!
            FieldAdditional = new bool[4, 4];

            foreach (var (X, Y) in NextFigure.Points)
            {
                FieldAdditional[X, Y] = true;
            }

            FieldWidth = width;
            FieldHeight = height;
            Score = 0;
            timer.Elapsed += Timer_Elapsed;
            Pause = false;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!MoveDown())
            {
                //сохраняем 
                WriteToField();

                switch (CheckCrossRow())
                {
                    case 1:
                        Score += 10;
                        break;
                    case 2:
                        Score += 30;
                        break;
                    case 3:
                        Score += 70;
                        break;
                    case 4:
                        Score += 150;
                        break;
                    default:
                        break;
                }

                NextFigure.Points.CopyTo(CurrentFigure.Points, 0);
                
                NextFigure = new Figure();

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        FieldAdditional[i, j] = false;
                    }
                }

                foreach (var (X, Y) in NextFigure.Points)
                {
                    FieldAdditional[X, Y] = true;
                }


                if (CheckEndGame())
                {
                    EndGame = true;
                    timer.Stop();
                }
                PrepareForDisplay();
            }
            
        }

        private void PrepareForDisplay()
        {
            for (int j = 0; j < FieldHeight; j++)
            {
                for (int i = 0; i < FieldWidth; i++)
                {
                    FieldToDisplay[i, j] = CurrentField[i, j];
                }
            }

            for (int i = 0; i < CurrentFigure.Points.Length; i++)
            {
                FieldToDisplay[CurrentFigure.Points[i].X, CurrentFigure.Points[i].Y] = true;
            }
        }

        private void WriteToField()
        {
            for (int i = 0; i < CurrentFigure.Points.Length; i++)
            {
                CurrentField[CurrentFigure.Points[i].X, CurrentFigure.Points[i].Y] = true;
            }
        }
        private void Rotate()
        {
            (int, int)[] rescueFigure = new (int, int)[CurrentFigure.Points.Length];
            CurrentFigure.Points.CopyTo(rescueFigure, 0);
            bool wrongPoints = false;
            var centerPoint = CurrentFigure.Points[1];
            for (int i = 0; i < CurrentFigure.Points.Length; i++)
            {
                int x = CurrentFigure.Points[i].Y - centerPoint.Y;
                int y = CurrentFigure.Points[i].X - centerPoint.X;
                CurrentFigure.Points[i].X = centerPoint.X - x;
                CurrentFigure.Points[i].Y = centerPoint.Y + y;
                if (CurrentFigure.Points[i].X >= FieldWidth || CurrentFigure.Points[i].X < 0)
                {
                    wrongPoints = true;
                    break;
                }
                if (CurrentField[CurrentFigure.Points[i].X, CurrentFigure.Points[i].Y] || CurrentFigure.Points[i].Y >= FieldHeight)
                {
                    wrongPoints = true;
                    break;
                }
                    
            }

            if (wrongPoints)
                rescueFigure.CopyTo(CurrentFigure.Points, 0);

            PrepareForDisplay();
        }

        public bool MoveDown()
        {
            foreach (var (x, y) in CurrentFigure.Points)
            {
                if (y == FieldHeight - 1 || CurrentField[x, y + 1])
                    return false;
            }
            for (int i = 0; i < CurrentFigure.Points.Length; i++)
            {
                    CurrentFigure.Points[i].Y++;
            }
            PrepareForDisplay();
            return true;
        }

        public bool MoveLeft()
        {
            foreach (var (x, y) in CurrentFigure.Points)
            {
                if (x == 0 || CurrentField[x - 1, y])
                    return false;
            }
            for (int i = 0; i < CurrentFigure.Points.Length; i++)
            {
                CurrentFigure.Points[i].X--;
            }
            PrepareForDisplay();
            return true;
        }

        public bool MoveRight()
        {
            foreach (var (x, y) in CurrentFigure.Points)
            {
                if (x == FieldWidth - 1 || CurrentField[x+1, y])
                    return false;
            }
            for (int i = 0; i < CurrentFigure.Points.Length; i++)
            {
                CurrentFigure.Points[i].X++;
            }
            PrepareForDisplay();
            return true;
        }

        private int CheckCrossRow()
        {
            int crossed = 0;
            for (int j = 0; j < FieldHeight; j++)
            {
                int filled = 0;

                for (int i = 0; i < FieldWidth; i++)
                {
                    if (CurrentField[i, j])
                        filled++;
                    else
                        break;
                }

                if (filled == FieldWidth)
                {
                    for (int i = 0; i < FieldWidth; i++)
                    {
                        CurrentField[i, j] = false;
                    }
                    for (int i = j; i > 1; i--)
                    {
                        for (int k = 0; k < FieldWidth; k++)
                        {
                            CurrentField[k, i] = CurrentField[k, i - 1];
                        }
                    }
                    crossed++;
                }
            }

            return crossed;
        }

        public bool CheckEndGame()
        {
            foreach (var (X, Y) in NextFigure.Points)
            {
                if (CurrentField[X, Y])
                {
                    return true;
                }
            }
            return false;
        }

        //
        // Override methods
        //
        public void DownKey()
        {
            if (!_pause)
                MoveDown();
        }
        public void LeftKey()
        {
            if (!_pause)
                MoveLeft();
        }
        public void RightKey()
        {
            if (!_pause)
                MoveRight();
        }
        public void UpKey()
        {
            if (!_pause)
                Rotate();
        }
        public void FuncKeyPressed()
        {
            
        }
    }

    class Figure
    {

        public (int X, int Y)[] Points { get; set; }

        public Figure()
        {
            Points = GetRandomFigure();

        }

        private (int X, int Y)[] GetRandomFigure()
        {

            int seed = new Random(DateTime.Now.Millisecond).Next(7);
            //seed = 0;

            switch (seed)
            {
                case 0:
                    return new[] { (1, 0), (1, 1), (1, 2), (1, 3) };
                case 1:
                    return new[] { (0, 1), (0, 2), (1, 2), (1, 3) };
                case 2:
                    return new[] { (1, 1), (1, 2), (0, 2), (0, 3) };
                case 3:
                    return new[] { (1, 1), (1, 2), (0, 2), (1, 3) };
                case 4:
                    return new[] { (0, 1), (1, 1), (1, 2), (1, 3) };
                case 5:
                    return new[] { (1, 1), (1, 2), (1, 3), (0, 3) };
                case 6:
                    return new[] { (0, 1), (1, 1), (0, 2), (1, 2) };
                default:
                    return new[] { (0, 1), (1, 1), (0, 2), (1, 2) };

            }



        }

        
    }

}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Part3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        
        //IGameBase currentGame = new TetrisGameA();
        //IGameBase currentGame = new SnakeGameA();
        IGameBase currentGame = new MainMenu();
        

        System.Timers.Timer timer = new System.Timers.Timer(10);
        
        // ! Заменить на переменные !
        Rectangle[] displayDots = new Rectangle[10 * 20];
        Rectangle[] additionalDisplayDot = new Rectangle[4 * 4];

        public MainWindow()
        {
            InitializeComponent();

            for (int i = 0; i < displayDots.Length; i++)
            {
                displayDots[i] = new Rectangle();
                FieldGrid.Children.Add(displayDots[i]);
            }

            for (int i = 0; i < additionalDisplayDot.Length; i++)
            {
                additionalDisplayDot[i] = new Rectangle();
                AdditionalGrid.Children.Add(additionalDisplayDot[i]);
            }

            MainMenu.GameChoosed += MainMenu_GameChoosed;
            timer.Elapsed += Timer_Elapsed;

            timer.Start();
            
        }


        private void MainMenu_GameChoosed(object sender, MainMenu.MenuEventArgs e)
        {
            if (e.GameNumber == 0)
                currentGame = new TetrisGameA();
            if (e.GameNumber == 1)
                currentGame = new SnakeGameA();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            currentGame.EndGame = false;
        }
        
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                Dispatcher.Invoke(displayUpdate);
            }catch (TaskCanceledException)
            {

            }
        }

        void displayUpdate()
        {
            for (int j = 0; j < 20; j++)
            {
                for (int i = 0; i < 10; i++)
                {
                    displayDots[j * 10 + i].IsEnabled = currentGame.FieldToDisplay[i, j];
                }
            }

            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    additionalDisplayDot[j * 4 + i].IsEnabled = currentGame.FieldAdditional[i, j];
                }
            }

            ScoreLabel.Content = currentGame.Score.ToString("D6");
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                currentGame.DownKey();
                Dispatcher.Invoke(displayUpdate);
            }
            if (e.Key == Key.Right)
            {
                currentGame.RightKey();
                Dispatcher.Invoke(displayUpdate);

            }
            if (e.Key == Key.Left)
            {
                currentGame.LeftKey();
                Dispatcher.Invoke(displayUpdate);
            }
            if (e.Key == Key.Up)
            {
                currentGame.UpKey();
                Dispatcher.Invoke(displayUpdate);
            }
            if (e.Key == Key.P)
            {
                if (!currentGame.Pause)
                {
                    currentGame.Pause = true;
                }
                else
                    currentGame.Pause = false;
            }
            if (e.Key == Key.Space)
            {
                currentGame.FuncKeyPressed();
                Dispatcher.Invoke(displayUpdate);
            }
        }
    }
}

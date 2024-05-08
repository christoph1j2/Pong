using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace Pong
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer keyboardTimer = new DispatcherTimer();
        DispatcherTimer timer = new DispatcherTimer();

        bool boolGoingUp = true;
        int movement = 0;

        int counter1 = 0;
        int counter2 = 0;
        public MainWindow()
        {
            InitializeComponent();

            this.Title = "Pong!";
            this.ResizeMode = ResizeMode.CanMinimize;

            Random rd = new Random();
            movement = rd.Next(-15, 16);


            keyboardTimer.Tick += KeyboardTimer_Tick;
            keyboardTimer.Interval = TimeSpan.FromMilliseconds(25);
            keyboardTimer.Start();

            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(25);
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (boolGoingUp)
            {
                Canvas.SetTop(Ball, Canvas.GetTop(Ball) - 10);
            }
            else
            {
                Canvas.SetTop(Ball, Canvas.GetTop(Ball) + 10);
            }

            Canvas.SetLeft(Ball, Canvas.GetLeft(Ball) + movement);

            if (Kollision(Ball, rectTop))
            {
                UpdateScore(1);
                boolGoingUp = false;

                if (counter1 >= 5)
                {
                    timer.Stop();
                    MessageBoxResult result = MessageBox.Show("Player 1 wins!\nPlay Again?", "Pong", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        ResetGame();
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
            else if (Kollision(Ball, rectLeft) || Kollision(Ball, rectRight))
            {
                movement *= -1;
            }
            else if (Kollision(Ball, Player1))
            {
                Canvas.SetTop(Ball, Canvas.GetTop(Ball) - 10);
                boolGoingUp = true;
            }
            else if (Kollision(Ball,Player2))
            {
                Canvas.SetTop(Ball, Canvas.GetTop(Ball) + 10);
                boolGoingUp = false;
            }
            else if (Kollision(Ball, rectBottom))
            {
                UpdateScore(2);
                boolGoingUp = true;
                ////Spiel verloren
                //timer.Stop();
                //MessageBoxResult result = MessageBox.Show("Game Over!\nPlay Again?", "Pong", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                //if (result==MessageBoxResult.Yes)
                //{
                //    ResetGame();
                //}
                //else
                //{
                //    this.Close();
                //}
                if (counter2 >= 5)
                {
                    timer.Stop();
                    MessageBoxResult result = MessageBox.Show("Player 2 wins!\nPlay Again?", "Pong", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        ResetGame();
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
            
        }

        private void KeyboardTimer_Tick(object sender, EventArgs e)
        {
            //player1
            if (Keyboard.IsKeyDown(Key.Left))
            {
                Canvas.SetLeft(Player1, Canvas.GetLeft(Player1) - 20);
            }
            else if (Keyboard.IsKeyDown(Key.Right))
            {
                Canvas.SetLeft(Player1, Canvas.GetLeft(Player1) + 20);
            }
            //player2
            if (Keyboard.IsKeyDown(Key.A))
            {
                Canvas.SetLeft(Player2, Canvas.GetLeft(Player2) - 20);
            }
            else if (Keyboard.IsKeyDown(Key.D))
            {
                Canvas.SetLeft(Player2, Canvas.GetLeft(Player2) + 20);
            }

            //player1
            if (Canvas.GetLeft(Player1) < 0)
            {
                Canvas.SetLeft(Player1, 0);
            }
            else if (Canvas.GetLeft(Player1) + Player1.RenderSize.Width > Background.RenderSize.Width)
            {
                Canvas.SetLeft(Player1, Background.RenderSize.Width - Player1.RenderSize.Width);
            }
            //player2
            if (Canvas.GetLeft(Player2) < 0)
            {
                Canvas.SetLeft(Player2, 0);
            }
            else if (Canvas.GetLeft(Player2) + Player2.RenderSize.Width > Background.RenderSize.Width)
            {
                Canvas.SetLeft(Player2, Background.RenderSize.Width - Player2.RenderSize.Width);
            }
        }

        private bool Kollision(UIElement element1, UIElement element2)
        {
            Rect rect1 = new Rect(Canvas.GetLeft(element1), Canvas.GetTop(element1), element1.RenderSize.Width, element1.RenderSize.Height);
            Rect rect2 = new Rect(Canvas.GetLeft(element2), Canvas.GetTop(element2), element2.RenderSize.Width, element2.RenderSize.Height);

            return rect1.IntersectsWith(rect2);
        }

        public void UpdateScore(int player)
        {
            if (player == 1)
            {
                Label counterLabel1 = (Label)this.FindName("Counter1");
                counter1++;
                counterLabel1.Content = counter1;
            }
            else if (player == 2)
            {
                Label counterLabel2 = (Label)this.FindName("Counter2");
                counter2++;
                counterLabel2.Content = counter2;
            }
        }
        public void ResetScore()
        {
            counter1 = 0;
            counter2 = 0;
            Label counterLabel1 = (Label)this.FindName("Counter1");
            Label counterLabel2 = (Label)this.FindName("Counter2");
            counterLabel1.Content = counter1;
            counterLabel2.Content = counter2;
        }
        private void ResetGame()
        {
            ResetScore();
            boolGoingUp = true;
            Random rd = new Random();
            movement = rd.Next(-15, 16);
            Canvas.SetLeft(Ball, Background.ActualWidth / 2);
            Canvas.SetTop(Ball, Background.ActualHeight / 2);

            Canvas.SetLeft(Player1, Background.ActualWidth / 2 - Player1.RenderSize.Width / 2);
            Canvas.SetLeft(Player2, Background.ActualWidth / 2 - Player2.RenderSize.Width / 2);


            timer.Start();
        }
    }
}

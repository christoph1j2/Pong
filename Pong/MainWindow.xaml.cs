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

        bool boolAtTop = true;
        int movement = 0;
        public MainWindow()
        {
            InitializeComponent();

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
            if (boolAtTop)
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
                boolAtTop = false;
            }
            else if (Kollision(Ball, rectLeft) || Kollision(Ball, rectRight))
            {
                movement *= -1;
            }
            else if (Kollision(Ball, Player))
            {
                boolAtTop = true;
            }
            else if (Kollision(Ball, rectBottom))
            {
                //Spiel verloren
                timer.Stop();
                MessageBoxResult result = MessageBox.Show("Game Over!\nPlay Again?", "Pong", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result==MessageBoxResult.Yes)
                {
                    ResetGame();
                }
                else
                {
                    this.Close();
                }
            }
        }

        private void KeyboardTimer_Tick(object sender, EventArgs e)
        {

            if (Keyboard.IsKeyDown(Key.Left))
            {
                Canvas.SetLeft(Player, Canvas.GetLeft(Player) - 20);
            }
            else if (Keyboard.IsKeyDown(Key.Right))
            {
                Canvas.SetLeft(Player, Canvas.GetLeft(Player) + 20);
            }


            if (Canvas.GetLeft(Player) < 0)
            {
                Canvas.SetLeft(Player, 0);
            }
            else if (Canvas.GetLeft(Player) + Player.RenderSize.Width > Background.RenderSize.Width)
            {
                Canvas.SetLeft(Player, Background.RenderSize.Width - Player.RenderSize.Width);
            }
        }

        private bool Kollision(UIElement element1, UIElement element2)
        {
            Rect rect1 = new Rect(Canvas.GetLeft(element1), Canvas.GetTop(element1), element1.RenderSize.Width, element1.RenderSize.Height);
            Rect rect2 = new Rect(Canvas.GetLeft(element2), Canvas.GetTop(element2), element2.RenderSize.Width, element2.RenderSize.Height);

            return rect1.IntersectsWith(rect2);
        }

        private void ResetGame()
        {
            boolAtTop = true;
            Random rd = new Random();
            movement = rd.Next(-15, 16);
            Canvas.SetLeft(Ball, Background.ActualWidth / 2);
            Canvas.SetTop(Ball, Background.ActualHeight / 2);

            Canvas.SetLeft(Player, Background.ActualWidth / 2 - Player.RenderSize.Width / 2);

            timer.Start();
        }
    }
}

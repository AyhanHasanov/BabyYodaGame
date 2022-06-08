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

namespace BabyYodaGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string direction = "idle";
        Random rndm = new Random();
        private int step = 3;
        List<Key> keys = new List<Key>()
        {
            Key.A, Key.S, Key.D, Key.W,
            Key.Left, Key.Down, Key.Right, Key.Up
        };

        int[] indexes = new int[4] { 0, 0, 0, 0 }; // up down left right
        List<BitmapImage> spritesUp = new List<BitmapImage>();
        List<BitmapImage> spritesDown = new List<BitmapImage>();
        List<BitmapImage> spritesLeft = new List<BitmapImage>();
        List<BitmapImage> spritesRight = new List<BitmapImage>();

        /*private const int NumberOfColumns = 1;
        private const int NumberOfFrames = 23;
        private const int FrameWidth = 640;
        private const int FrameHeight = 640;
        public static readonly TimeSpan TimePerFrame = TimeSpan.FromSeconds(1 / 60f);
        private int currentFrame;
        private TimeSpan timeTillNextFrame;
        private void OnUpdate(object sender, object e)
        {
            this.timeTillNextFrame += TimeSpan.FromSeconds(1 / 60f);
            if (this.timeTillNextFrame > TimePerFrame)
            {
                this.currentFrame = (this.currentFrame + 1 + NumberOfFrames) % NumberOfFrames;
                var column = this.currentFrame % NumberOfColumns;
                var row = this.currentFrame / NumberOfColumns;

                this.SpriteSheetOffset.X = -column * FrameWidth;
                this.SpriteSheetOffset.Y = -row * FrameHeight;
            }
        }*/


        DispatcherTimer gameTimer = new DispatcherTimer();
        DispatcherTimer animationTimer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            MyCanvas.Focus();
            gameTimer.Interval = TimeSpan.FromMilliseconds(10);
            gameTimer.Tick += Engine;
            //gameTimer.Start();

            animationTimer.Interval = TimeSpan.FromMilliseconds(30);
            animationTimer.Tick += AnimateOnTick;
            //animationTimer.Start();

            imgBrush = new ImageBrush();
            imgBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/front1.png"));
            imgBrush.TileMode = TileMode.None;
            player.Fill = imgBrush;
            LoadSpritesInLists();
        }

        private void AnimateOnTick(object sender, EventArgs e)
        {
            Move();
        }

        private void Engine(object sender, EventArgs e)
        {

        }

        private void MyCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.A:
                case Key.Left:
                    direction = "left";
                    break;

                case Key.S:
                case Key.Down:
                    direction = "down";
                    break;
                case Key.D:
                case Key.Right:
                    direction = "right";
                    break;
                case Key.W:
                case Key.Up:
                    direction = "up";
                    break;

            }

            if (keys.Any(key => key.Equals(e.Key)))
                animationTimer.Start();
                
        }

        private void MyCanvas_KeyUp(object sender, KeyEventArgs e)
        {
            if (keys.Any(key => key.Equals(e.Key)))
                animationTimer.Stop();

        }

        ImageBrush animationBrush = new ImageBrush();
        private void Move()
        {
            try
            {
                switch (direction.ToLower())
                {

                    case "up":
                        //if (Canvas.GetTop(player) <= 0)
                        Canvas.SetTop(player, Canvas.GetTop(player) - step);
                        animationBrush.ImageSource = spritesUp[indexes[0]];
                        player.Fill = animationBrush;
                        indexes[0]++;
                        if (indexes[0] == spritesUp.Count-1)
                        {
                            indexes[0] = 0;
                        }
                        break;
                    case "down":
                        //if(Canvas.GetTop(player) >= MyCanvas.ActualHeight)
                        Canvas.SetTop(player, Canvas.GetTop(player) + step);
                        animationBrush.ImageSource = spritesDown[indexes[1]];
                        player.Fill = animationBrush;
                        indexes[1]++;
                        if (indexes[1] == spritesDown.Count - 1)
                        {
                            indexes[1] = 0;
                        }
                        break;
                    case "left":
                        //if (Canvas.GetLeft(player) <= 0)
                        Canvas.SetLeft(player, Canvas.GetLeft(player) - step);
                        animationBrush.ImageSource = spritesLeft[indexes[2]];
                        player.Fill = animationBrush;
                        indexes[2]++;
                        if (indexes[2] == spritesLeft.Count - 1)
                        {
                            indexes[2] = 0;
                        }
                        break;
                    case "right":
                        //if(Canvas.GetLeft(player) >= MyCanvas.ActualWidth)
                        
                        Canvas.SetLeft(player, Canvas.GetLeft(player) + step);
                        animationBrush.ImageSource = spritesRight[indexes[3]];
                        player.Fill = animationBrush;
                        indexes[3]++;
                        if (indexes[3] == spritesRight.Count - 1)
                        {
                            indexes[3] = 0;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "");
            }
        }

        private void LoadSpritesInLists()
        {
            spritesUp.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/back1.png")));
            spritesUp.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/back2.png")));
            spritesUp.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/back3.png")));
            spritesUp.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/back4.png")));
            spritesUp.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/back5.png")));
            spritesUp.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/back6.png")));
            /*spritesUp.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/back5.png")));
            spritesUp.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/back4.png")));
            spritesUp.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/back3.png")));
            spritesUp.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/back2.png")));*/

            spritesDown.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/front1.png")));
            spritesDown.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/front2.png")));
            spritesDown.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/front3.png")));
            spritesDown.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/front4.png")));
            spritesDown.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/front5.png")));
            spritesDown.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/front4.png")));
            spritesDown.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/front3.png")));
            spritesDown.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/front2.png")));

            spritesLeft.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/walking left 1.png")));
            spritesLeft.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/walking left 2.png")));
            spritesLeft.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/walking left 3.png")));
            spritesLeft.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/walking left 4.png")));
            spritesLeft.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/walking left 5.png")));
            spritesLeft.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/walking left 6.png")));

            spritesRight.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/walking right 1.png")));
            spritesRight.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/walking right 2.png")));
            spritesRight.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/walking right 3.png")));
            spritesRight.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/walking right 4.png")));
            spritesRight.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/walking right 5.png")));
            spritesRight.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/walking right 6.png")));
        }
    }
}

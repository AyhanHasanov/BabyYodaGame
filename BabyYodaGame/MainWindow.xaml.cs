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
       
        List<Key> keys = new List<Key>() { Key.A, Key.S, Key.D, Key.W, Key.Left, Key.Down, Key.Right, Key.Up };
        List<BitmapImage> spritesUp = new List<BitmapImage>();
        List<BitmapImage> spritesDown = new List<BitmapImage>();
        List<BitmapImage> spritesLeft = new List<BitmapImage>();
        List<BitmapImage> spritesRight = new List<BitmapImage>();
        List<BitmapImage> spritesCryingPenguin = new List<BitmapImage>();
        List<BitmapImage> spritesHappyPenguin = new List<BitmapImage>();
        ImageBrush animationBrush = new ImageBrush();
        DispatcherTimer gameTimer = new DispatcherTimer();
        DispatcherTimer animationTimer = new DispatcherTimer();
        DispatcherTimer staticAnimationTimer = new DispatcherTimer();
        Rectangle rec;
        Point playerPos;
        Point playerPosCenter;
        Point currentFishPos;
        string direction = "idle";
        Random rndm = new Random();
        private const int step = 3;
        bool isFishEaten = false;
        bool isFishGenerated = false;
        bool isWon = false;
        int[] indexes = new int[4] { 0, 0, 0, 0 }; // Used for animation ordering. Index order: up, down, left, right
        int objectiveNumber = 0;
        int fishEatenCount = 0;
        int indexPenguinAnimation = 0;
        public MainWindow()
        {
            InitializeComponent();

            StartWindow sw = new StartWindow();
            sw.Show();
            MyCanvas.Focus();
            objectiveNumber = sw.ObjectiveNumber;
            gameTimer.Interval = TimeSpan.FromMilliseconds(1500);
            gameTimer.Tick += Engine;
            gameTimer.Start();

            animationTimer.Interval = TimeSpan.FromMilliseconds(30);
            animationTimer.Tick += AnimateOnTick;

            playerImgBrush = new ImageBrush();
            playerImgBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/front1.png"));
            playerImgBrush.TileMode = TileMode.None;
            player.Fill = playerImgBrush;
            LoadSpritesInLists();

            staticAnimationTimer.Interval = TimeSpan.FromMilliseconds(10);
            staticAnimationTimer.Tick += PlayStaticAnimation;
            staticAnimationTimer.Start();
            

        }

        private void PlayStaticAnimation(object sender, EventArgs e)
        {
            /*if (!isWon)
            {
                indexPenguinAnimation++;
                if(indexPenguinAnimation == spritesCryingPenguin.Count - 1)
                penguin.Fill = new ImageBrush() { ImageSource = spritesCryingPenguin[indexPenguinAnimation] };
            }*/
        }

        private void AnimateOnTick(object sender, EventArgs e)
        {
            Walk();
            CheckIfEats();
        }

        private void Engine(object sender, EventArgs e)
        {
            GenerateFishes();
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

        private void Walk() //animation included
        {
            coords.Content = $"player {playerPos.X} {playerPos.Y}\nplayerPos Middle {playerPosCenter.X} {playerPosCenter.Y}\n ";
            coords.Content += $"fishPos {currentFishPos.X} {currentFishPos.Y}";
            //try
            //{
            switch (direction.ToLower())
            {

                case "up":
                    //if (Canvas.GetTop(player) <= 0)
                    Canvas.SetTop(player, Canvas.GetTop(player) - step);
                    animationBrush.ImageSource = spritesUp[indexes[0]];
                    player.Fill = animationBrush;
                    indexes[0]++;
                    if (indexes[0] == spritesUp.Count - 1)
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


            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "");
            //}
        }

        private void CheckIfEats()
        {
            playerPos = new Point(Canvas.GetLeft(player), Canvas.GetTop(player));
            playerPosCenter.X = playerPos.X + (player.ActualWidth / 2);
            playerPosCenter.Y = playerPos.Y + (player.ActualHeight / 2);

            bool check = playerPosCenter.X >= currentFishPos.X && playerPosCenter.X <= currentFishPos.X + 30 &&
                playerPosCenter.Y >= currentFishPos.Y && playerPosCenter.Y <= currentFishPos.Y + 30;

            if (check && !isFishEaten)
            {
                MyCanvas.Children.Remove(rec);
                isFishEaten = true;
                isFishGenerated = false;
                fishEatenCount++;
                scoreLbl.Content = $"Fish eaten:  {fishEatenCount}";
            }
        }

        private void GenerateFishes()
        {
            if (!isFishGenerated && fishEatenCount != objectiveNumber)
            {
                rec = new Rectangle()
                {
                    Width = 30,
                    Height = 30,
                    Fill = new ImageBrush() { ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/goldenFish.png")) },
                    Name = "Fish"
                };

                int x = rndm.Next(50, (int)this.ActualWidth - 64);
                int y = rndm.Next(50, (int)this.ActualHeight - 64);
                currentFishPos = new Point(x, y);
                Canvas.SetTop(rec, y);
                Canvas.SetLeft(rec, x);
                isFishGenerated = true;
                isFishEaten = false;
                MyCanvas.Children.Add(rec);
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
            
            
            spritesCryingPenguin.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/penguin-crying1.png")));
            spritesCryingPenguin.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/penguin-crying2.png")));
            spritesCryingPenguin.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/penguin-crying3.png")));
            spritesCryingPenguin.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/penguin-crying4.png")));
            
            spritesHappyPenguin.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/penguin_1.png")));
            spritesHappyPenguin.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/penguin_2.png")));
            spritesHappyPenguin.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/penguin_3.png")));
            spritesHappyPenguin.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/penguin_4.png")));
            spritesHappyPenguin.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/penguin_5.png")));
            spritesHappyPenguin.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/penguin_6.png")));
            spritesHappyPenguin.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/penguin_7.png")));
            spritesHappyPenguin.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/penguin_8.png")));
            spritesHappyPenguin.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/penguin_9.png")));
            spritesHappyPenguin.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/penguin_10.png")));
            spritesHappyPenguin.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/penguin_11.png")));
            spritesHappyPenguin.Add(new BitmapImage(new Uri("pack://application:,,,/Resources/penguin_12.png")));
        
        }
    
    
    }
}

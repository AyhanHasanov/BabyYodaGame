using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Media;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using BabyYodaGame.Properties;

namespace BabyYodaGame
{
    /// <summary>
    /// Interaction logic for WonWindow.xaml
    /// </summary>
    public partial class WonWindow : Window
    {
        public WonWindow()
        {
            InitializeComponent();
        }

        DispatcherTimer animationTimer = new DispatcherTimer();
        List<BitmapImage> spritesHappyPenguin;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Media.SoundPlayer player = new SoundPlayer();
            player.SoundLocation = "wap.wav";
            player.Play();

            /*
            MediaPlayer playMedia = new MediaPlayer(); // making a new instance of the media player
            var uri = new Uri("pack://application:,,,/Resources/wap.wav"); // browsing to the sound folder and then the WAV file location
            playMedia.Open(uri); // inserting the URI to the media player
            playMedia.Play();
            mediaElement.Play();*/
            animationTimer.Interval = TimeSpan.FromMilliseconds(50);
            animationTimer.Tick += PlayAnimation;
            animationTimer.Start();
            spritesHappyPenguin = new List<BitmapImage>();
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
        int index = 0;
        private void PlayAnimation(object sender, EventArgs e)
        {
            index++;
            if (index >= spritesHappyPenguin.Count)
                index = 0;

            ImageBrush br = new ImageBrush();
            br.ImageSource = spritesHappyPenguin[index];

            penguiny.Fill = br;
        }
    }
}

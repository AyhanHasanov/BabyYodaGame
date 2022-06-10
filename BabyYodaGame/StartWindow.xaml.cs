using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BabyYodaGame
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        public int ObjectiveNumber { get; private set; }
        Random random = new Random();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ObjectiveNumber = random.Next(10, 20);
            objectiveNumber.Content = $"Objective: Collect {ObjectiveNumber} fishes to save Penguiny!";
        }

        private void playBttn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            mw.objectiveNumber = ObjectiveNumber;
            this.Hide();
        }
    }
}

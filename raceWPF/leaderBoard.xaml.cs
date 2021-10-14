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
using Controller;

namespace raceWPF
{
    /// <summary>
    /// Interaction logic for leaderBoard.xaml
    /// </summary>
    public partial class leaderBoard : Window
    {
        public LeaderboardInfo leaderboardInfo;
        public leaderBoard()
        {
            InitializeComponent();
            leaderboardInfo = new LeaderboardInfo();
            DataContext = leaderboardInfo;

        }
    }
}

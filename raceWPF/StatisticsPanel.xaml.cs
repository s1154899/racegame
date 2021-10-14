using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
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
    /// Interaction logic for StatisticsPanel.xaml
    /// </summary>
    public partial class StatisticsPanel : Window
    {

        public AboutCompetition aboutCompetition;
        public StatisticsPanel()
        {

            InitializeComponent();
            aboutCompetition = new AboutCompetition();
            DataContext = aboutCompetition;
   
        }

        ~StatisticsPanel()
        {
            
        }
    }
}

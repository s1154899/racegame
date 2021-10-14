using Controller;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using model;
using Image = System.Windows.Controls.Image;

namespace raceWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>


    public partial class MainWindow : Window
    {
        private static Image _raceTrack;
        private static StatisticsPanel _statistics;
        private static leaderBoard _leaderboard;
        
        public MainWindow()
        {
            
            InitializeComponent();
            //ImageTest.Source = DrawTrack();
            _statistics = new StatisticsPanel();
            _leaderboard = new leaderBoard();

            _raceTrack = raceTrack;
            _startRace();

            VisualizeTrack track = new VisualizeTrack();

            track.mapTrack(Data.Currentrace.track);
            PictureHandler.ResizeEmptyGreen(track.height + 200,track.width + 200);
            

            raceTrack.Source = PictureHandler.CreateBitmapSourceFromGdiBitmap(track.DrawTrack(Data.Currentrace.track)); ;


            
            //    Thread.Sleep(500);

            //Uri uri = new Uri("fotos/cornerBottomToLeft.png", UriKind.Relative);
            //raceTrack.Source = PictureHandler.CreateBitmapSourceFromGdiBitmap((Bitmap)Bitmap.FromStream(Application.GetContentStream(uri).Stream));


        }

        public BitmapSource DrawTrack()
        {
            return PictureHandler.CreateBitmapSourceFromGdiBitmap(PictureHandler.GetImageBitmap("EmptyGreen"));

        }

        //runs after a driver changed section
        //redraws the track
        private static void Currentrace_onDriverChanged(object sender, Model.DriverChangedEventArgs eventArgs)
        {
            _statistics.aboutCompetition.refresh();
            _leaderboard.leaderboardInfo.refresh();
            _raceTrack.Dispatcher.BeginInvoke(DispatcherPriority.Render,new Action(() => {
                VisualizeTrack track = new VisualizeTrack();
                if (!object.Equals(eventArgs.track, null))
                {
                    track.mapTrack(eventArgs.track);
                    Bitmap emptyGreen = PictureHandler.GetImageBitmap("EmptyGreen");
                    if (track.height > emptyGreen.Height || track.width > emptyGreen.Width)
                    {
                        PictureHandler.ResizeEmptyGreen(track.width + 200, track.height + 200);

                    }

                    _raceTrack.Source =
                        PictureHandler.CreateBitmapSourceFromGdiBitmap(track.DrawTrack(eventArgs.track));
                }
            }));
            
            

        }

        

        //runs after a race is finished
        //stops the current race then starts a new one or displays the endscreen.
        private static void Currentrace_finishedTrack(object sender, RaceFinishedArgs eventArgs)
        {
            _stopRace();

            if (!_startRace())
            {
                _raceTrack.Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(() =>
                {
                    _raceTrack.Source = null;
                }));
                //   Visual.displayWinners(Data.MyProperty.winners);
            }
        }

        //stops the current race
        //save the results 
        //than stops the object from racing futher
        private static void _stopRace()
        {
            Data.MyProperty.addWinners(Data.Currentrace.track, Data.Currentrace.RankingList);

            Data.Currentrace.StopRace();
            Data.Currentrace.onDriverChanged -= Currentrace_onDriverChanged;
            Data.Currentrace.onRaceFinished -= Currentrace_finishedTrack;
            Data.Currentrace = null;


        }

        //starts the a race
        

            private static bool _startRace()
        {


            Data.NextRace();
            if (!Object.Equals(Data.Currentrace, null))
            {
                Data.Currentrace.onDriverChanged += Currentrace_onDriverChanged;
                Data.Currentrace.onRaceFinished += Currentrace_finishedTrack;
                Data.Currentrace.Start();
                return true;
            }

            return false;

        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItem_stats_Click(object sender, RoutedEventArgs e)
        {
            _statistics = new StatisticsPanel();
            Window win = _statistics;
            win.Show();
        }
        private void MenuItem_leaderBoard_Click(object sender, RoutedEventArgs e)
        {
            _leaderboard = new leaderBoard();
            Window win = _leaderboard;
            win.Show();
        }
    }
}

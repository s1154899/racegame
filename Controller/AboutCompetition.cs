using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Model;

namespace Controller
{
    public class AboutCompetition : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;



        public AboutCompetition()
        {

        }

        private static String _drivers;
        public String drivers
        {
            get => _drivers;
            set
            {
                _drivers = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("drivers"));
                }
            }
        }

        private static String _track;

        public String tracks
        {
            get => _track;
            set { _track = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("tracks"));
                }
            }
                


        }

        public void refresh()
        {


            String drive = "";
            var names = from Participants in Data.MyProperty.Participants orderby Participants.Points select new { Participants.Name, Participants.TeamColor, Participants.Points, Participants.Equipment };
            foreach (var name in names)
            {
                drive += $"{name.Name}:\nColor {name.TeamColor.ToString()} score: {name.Points}\n" +
                         $"equipment:\n" +
                         $"\tQuality {name.Equipment.Quality} \n" +
                         $"\tPerformance {name.Equipment.Performace}\n" +
                         $"\tSpeed {name.Equipment.Speed} \n" +
                         $"\tis broken {name.Equipment.isBroken.ToString()}\n";
            }

            Competition comp = new Competition();
            comp.AddTrack();

            this.drivers = drive;
            String trackName = "";
            var tracks = from track in comp.Tracks select new { track.Name, track.Sections.Count };
            foreach (var name in tracks)
            {
                trackName += $"{name.Name}:\nSections {name.Count} \n";
            }

            this.tracks = trackName;
        }


    }
}
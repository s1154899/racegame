using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Competition
    {
        public List<IParticipant> Participants { get; set; }
        public Queue<Track> Tracks { get; set; }

        public Dictionary<String, List<IParticipant>> winners;


        public Competition() {
            Participants = new List<IParticipant>();
            Tracks = new Queue<Track>();
            this.winners = new Dictionary<string, List<IParticipant>>();


        }

        public Track NextTrack() {
            try
            {
                return Tracks.Dequeue();
            }
            catch (System.InvalidOperationException e) {
                return null;
            }
        }
        public void AddParticipants()
        {
            Participants = new List<IParticipant>();
            var rand = new Random();
            Type type = typeof(TeamColors);
            var colors = Enum.GetValues(type);

            for (int i = 0; i < 5; i++)
            {

                TeamColors teamColor = (TeamColors)colors.GetValue(rand.Next(colors.Length));
                Driver driver = new Driver($"{i}-{teamColor.ToString()}",teamColor);
                driver.Equipment.randomEquipment();
                Participants.Add(driver);
            }

        }

        public void AddTrack()
        {
            Tracks = new Queue<Track>();

            SectionTypes[] routeHarderwijk =
            {
                SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.RightCorner,
                SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight,
                SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.LeftCorner,
                SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight,
                SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight,
                SectionTypes.RightCorner, SectionTypes.StartGrid, SectionTypes.StartGrid,
            };



            Tracks.Enqueue(new Track("4", routeHarderwijk));

            SectionTypes[] section = new SectionTypes[] { SectionTypes.RightCorner,SectionTypes.StartGrid, SectionTypes.RightCorner,SectionTypes.Straight, SectionTypes.RightCorner,SectionTypes.Finish ,SectionTypes.RightCorner ,SectionTypes.Straight};
            Tracks.Enqueue(new Track("rainbow Cup", section));
            section = new SectionTypes[] { SectionTypes.RightCorner, SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Finish, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight };
            Tracks.Enqueue(new Track("2", section));
            section = new SectionTypes[] { SectionTypes.LeftCorner, SectionTypes.StartGrid, SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.Finish, SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight };

            Tracks.Enqueue(new Track("3", section));

            
               
            
            /*
             Tracks.Enqueue(new Track("5", section));
            Tracks.Enqueue(new Track("6", section));
            */
        }

        public void addWinners(Track track, List<IParticipant> participant)
        {

            winners.Add(track.Name,participant);
        }

    }
}

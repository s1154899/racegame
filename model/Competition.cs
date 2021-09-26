using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Competition
    {
        public List<IParticipant> Participants { get; set; }
        public Queue<Track> Tracks { get; set; }


        public Competition() {
            Participants = new List<IParticipant>();
            Tracks = new Queue<Track>();

            
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
            Participants.Add(new Driver("1"));
            Participants.Add(new Driver("2"));
            Participants.Add(new Driver("3"));
            Participants.Add(new Driver("4"));
            Participants.Add(new Driver("5"));
        }

        public void AddTrack()
        {
            Tracks = new Queue<Track>();

            SectionTypes[] section = new SectionTypes[] { SectionTypes.RightCorner,SectionTypes.StartGrid, SectionTypes.RightCorner,SectionTypes.Straight, SectionTypes.RightCorner,SectionTypes.Finish ,SectionTypes.RightCorner ,SectionTypes.Straight};
            Tracks.Enqueue(new Track("rainbow Cup", section));
            Tracks.Enqueue(new Track("2", section));
            Tracks.Enqueue(new Track("3", section));
            Tracks.Enqueue(new Track("4", section));
            Tracks.Enqueue(new Track("5", section));
            Tracks.Enqueue(new Track("6", section));

        }

    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Model;
namespace Controller
{
    public class Race
    {
        public Track track { get; set; }
        public List<IParticipant> participants { get; set; }
        public DateTime StartTime { get; set; }
        private Random _random;

        private Dictionary<Section, SectionData> _positions;

        public Race(Track track) {
            this.track = track;
            participants = new List<IParticipant>();
            fillParticipant();
            _positions = new Dictionary<Section, SectionData>();
            setStartingPositions(track,participants);
        }

        public void fillParticipant()
        {
            participants.Add(new Driver("1"));
            participants.Add(new Driver("2"));
            participants.Add(new Driver("3"));
            participants.Add(new Driver("4"));
            participants.Add(new Driver("5"));
            participants.Add(new Driver("6"));
            participants.Add(new Driver("7"));
        }

        public SectionData GetSectionData(Section section) {
            SectionData sectionData;
            if (_positions.TryGetValue(section, out sectionData)) {
                return sectionData;
            }
            sectionData = new SectionData();

            _positions.Add(section, sectionData);
            return sectionData;
        }

        public Race(Track track, List<IParticipant> participants){
            this.track = track;
            this.participants = participants;
            this.StartTime = new DateTime();
            this._positions = new Dictionary<Section, SectionData>();
            this._random = new Random(DateTime.Now.Millisecond);


        }


        public void RandomizeEquipment() {
            foreach (IParticipant participant in participants) {
                participant.Equipment.Quality = _random.Next(100);
                participant.Equipment.Performace = _random.Next(100);
            }

        }

        private List<Section> _SectionsBeforeStart(Track track)
        {
            List<Section> sectionsBeforeStart = new List<Section>();
            LinkedListNode<Section> trackSection = null;
            Boolean beforeStart = false;
            for (int i = 0; i < track.Sections.Count; i++)
            {
                if (i == 0 || object.Equals(trackSection, null))
                {
                    trackSection = track.Sections.Last;
                }

                if (beforeStart == true)
                {
                    sectionsBeforeStart.Add(trackSection.Value);
                }

                if (trackSection.Value.SectionType == SectionTypes.StartGrid && beforeStart == false)
                {
                    beforeStart = true;
                    i = 0;
                }



                trackSection = trackSection.Previous;
            }

            return sectionsBeforeStart;
        }


        public void setStartingPositions(Track track, List<IParticipant> participants)
        {
            List<Section> sectionsBeforeStart = _SectionsBeforeStart(track);

            int sectionCount = 0;
            for (int i = 0; i < participants.Count; i++)
            {
                SectionData sectionData = GetSectionData(sectionsBeforeStart[sectionCount]);
                
                if (0 == i % 2)
                {

                    sectionData.Left = participants[i];

                }
                else
                {
                    sectionData.Right = participants[i];
                    sectionCount++;
                }
            }    

        }

    }
}

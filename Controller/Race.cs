using System;
using System.Collections.Generic;
using System.Timers;
using model;
using Model;

namespace Controller
{
    public delegate void DriversChanged(object sender, DriverChangedEventArgs eventArgs);

    public delegate void RaceFinished(object sender, RaceFinishedArgs eventArgs);

    public class Race
    {
        private readonly int sectionSize = 100;
        private readonly int moveAfrerEveryMilliseconds = 500;

        private readonly int _finishLine;
        private Dictionary<IParticipant, int> ParticipantLaps;

        private Dictionary<Section, SectionData> _positions;
        private Random _random;

        public List<IParticipant> RankingList;


        //inits the race
        public Race(Track track, List<IParticipant> participants)
        {
            this.participants = new List<IParticipant>(participants);
            _random = new Random();


            _positions = new Dictionary<Section, SectionData>();


            StartTime = new DateTime();
            _positions = new Dictionary<Section, SectionData>();
            _random = new Random(DateTime.Now.Millisecond);


            RankingList = new List<IParticipant>();
            this.track = track;


            setStartingPositions(track, participants);

            _finishLine = FindFinish(track);
            _Timer = new Timer(moveAfrerEveryMilliseconds);
            _Timer.Elapsed += TimerOnElapsed;

            ParticipantLaps = new Dictionary<IParticipant, int>();
        }

        public Track track { get; set; }
        public List<IParticipant> participants { get; set; }
        public DateTime StartTime { get; set; }
        private Timer _Timer { get; set; }


        public event DriversChanged onDriverChanged;
        public event RaceFinished onRaceFinished;


        //finds where the where the finish is located in the track 
        //returns the numeric location of the finish withing the track
        private int FindFinish(Track track)
        {
            var finishLine = 0;
            foreach (var section in track.Sections)
            {
                if (section.SectionType == SectionTypes.Finish) return finishLine;

                finishLine++;
            }

            return 0;
        }
/*
        private int getParticipantLaps(IParticipant participant)
        {
            int returnValue;
            if(!ParticipantLaps.TryGetValue(participant,out returnValue))
            {
                returnValue = 0;
                ParticipantLaps.Add(participant,returnValue);
            }
            return returnValue;
        }
*/
        //activates based on the moveAfrerEveryMilliseconds variable
        //starts the calculation of where every particpants wil go to
        public void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            perSectionMovement();
        }

        //starts the timer which starts the race
        public void Start()
        {
            _Timer.Start();
        }

        //gets the section data for a given section
        //gets from the _positions dictionary if it cant be found it will make a new entry of sectionData paired with its section
        public SectionData GetSectionData(Section section)
        {
            SectionData sectionData;
            _positions.TryGetValue(section, out sectionData);
            if (!Equals(sectionData, null)) return sectionData;
            sectionData = new SectionData();

            _positions.Add(section, sectionData);
            return sectionData;
        }


        //gets all the sections before the start line
        //returns a list with the sections infront of start
        private List<Section> _SectionsBeforeStart(Track track)
        {
            var sectionsBeforeStart = new List<Section>();
            LinkedListNode<Section> trackSection = null;
            var beforeStart = false;
            for (var i = 0; i < track.Sections.Count; i++)
            {
                if (i == 0 || Equals(trackSection, null)) trackSection = track.Sections.Last;

                if (beforeStart) sectionsBeforeStart.Add(trackSection.Value);

                if (trackSection.Value.SectionType == SectionTypes.StartGrid && beforeStart == false)
                {
                    beforeStart = true;
                    i = 0;
                }


                trackSection = trackSection.Previous;
            }

            return sectionsBeforeStart;
        }

        //calculates the movement for participant
        //moves thru the everysection checking for participants and calculates the movement
        public void perSectionMovement()
        {
            var AParticipantMoved = false;
            LinkedListNode<Section> sectionNode = null;

            for (var i = 0; i < track.Sections.Count; i++)
            {
                if (i == 0)
                    sectionNode = track.Sections.Last;
                else
                    sectionNode = sectionNode.Previous;

                var section = sectionNode.Value;
                var sectionData = GetSectionData(section);

                if (!Equals(sectionData.Left, null))
                {
                    
                    if (moveParticipant(sectionData.Left, ref sectionNode, ref sectionData, true, false, i))
                    {
                        AParticipantMoved = true;
                    }

                }

                if (!Equals(sectionData.Right, null))
                {
                    
                    if (moveParticipant(sectionData.Right, ref sectionNode, ref sectionData, false, true, i))
                    {
                        AParticipantMoved = true;
                    }
                    
                }
            }

            if (AParticipantMoved) onDriverChanged.Invoke(this, new DriverChangedEventArgs(track));
            if (RankingList.Count > participants.Count / 2) onRaceFinished.Invoke(this, new RaceFinishedArgs());
        }
        //moves a particapant to its correct place
        //returns true if a participant was moved
        private bool moveParticipant(IParticipant participant,ref  LinkedListNode<Section> sectionNode,ref SectionData sectionData ,bool validLeft,bool validRight, int sectionInt)
        {

            bool AParticipantMoved = false;
            if (validLeft)
            {
                sectionData.Left = null;
            }
            else
            {
                sectionData.Right = null;
            }


            int actualMoved = 0;

            var totalMovement = sectionData.DistanceLeft + _participant_calculateMovement(participant);

            var sectionsMoved = totalMovement / sectionSize;



            var validSection = ValidLocation(sectionNode, sectionsMoved, ref validLeft, ref validRight, ref actualMoved);


            var validData = GetSectionData(validSection);


            if (validLeft)
            {
                validData.Left = participant;
                validData.DistanceLeft = totalMovement % sectionSize;
                AParticipantMoved = true;
            }
            else if (validRight)
            {
                validData.Right = participant;
                validData.DistanceRight = totalMovement % sectionSize;
                AParticipantMoved = true;
            }

            if (participant.Equipment.isBroken == true)
            {
                participant.Equipment.randomEquipment();
            }

            {
                
            }

            if (_calculateOverFinish(sectionInt % track.Sections.Count, actualMoved))
            {
                
                participant.Points++;

                if (participant.Points > 3)
                {
                    validData.Left = null;
                    RankingList.Add(participant);
                }
            }

            participant.Equipment.breaks();

            return AParticipantMoved;
        }
        //returns a location where a participant could be placed
        private Section ValidLocation(LinkedListNode<Section> sectionNode, int sectionsMoved, ref bool validleft,
            ref bool validright, ref int actualMoved)
        {
            var lastValidSection = sectionNode.Value;
            LinkedListNode<Section> Node = null;
            for (var j = 0; j < sectionsMoved; j++)
            {
                if (j == 0)
                {
                    if (Equals(sectionNode.Next, null))
                        Node = sectionNode.List.First;
                    else
                        Node = sectionNode.Next;
                }
                else
                {
                    if (Equals(Node.Next, null))
                        Node = sectionNode.List.First;
                    else
                        Node = Node.Next;
                }

                var left = false;
                var right = false;

                findSpaceForParticpant(Node.Value, ref left, ref right);
                if (left || right)
                {
                    actualMoved = j + 1;
                    validright = right;
                    validleft = left;

                    lastValidSection = Node.Value;
                }
            }

            return lastValidSection;
        }

        //checks if there is space for a participant on the left or right lane
        private void findSpaceForParticpant(Section section, ref bool left, ref bool right)
        {
            left = false;
            right = false;

            var sectionData = GetSectionData(section);
            if (Equals(sectionData.Left, null)) left = true;

            if (Equals(sectionData.Right, null)) right = true;
        }
        //returns the speed at wich the participant will move
        private int _participant_calculateMovement(IParticipant participant)
        {
            return participant.Equipment.Speed * participant.Equipment.Performace;
        }

        //returns true if the participant has moved over the finish
        private bool _calculateOverFinish(int startingMoving, int amountMoved)
        {
            if (track.Sections.Count < startingMoving + amountMoved)
            {
                if (track.Sections.Count + _finishLine + 2 < startingMoving + amountMoved) return true;
            }
            else
            {
                if (_finishLine + 2 < startingMoving + amountMoved && startingMoving <= _finishLine + 2) return true;
            }

            return false;
        }

        //places the participants on their starting positions
        public void setStartingPositions(Track track, List<IParticipant> participants)
        {
            var sectionsBeforeStart = _SectionsBeforeStart(track);

            var sectionCount = 0;
            for (var i = 0; i < participants.Count; i++)
            {
                var sectionData = GetSectionData(sectionsBeforeStart[sectionCount]);

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

        //stops the race
        public void StopRace()
        {
            foreach (var par in participants)
            {
                par.Points = 0;
            }

            RankingList = null;
            track = null;
            participants = null;
            _random = null;
            _positions = null;
            _Timer.Stop();
            _Timer.Elapsed -= TimerOnElapsed;
            _Timer = null;
        }
    }
}
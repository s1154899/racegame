using System;
using System.Collections.Generic;
using model;
using Model;
using NUnit.Framework;

namespace ControllerTest
{
    [TestFixture]
    public class model_getters_setters
    {



        public void CarGettersSetters()
        {
            Car car = new Car();
        }

        [Test]
        public void CompetitionGettersSetters()
        {
            Competition competition = new Competition();

            Dictionary<string, List<IParticipant>>  winners = new Dictionary<string, List<IParticipant>>();
            List<IParticipant> participant = new List<IParticipant>();
            Queue<Track> tracks = new Queue<Track>();

            competition.winners = winners;
            competition.Participants = participant;
            competition.Tracks = tracks;

            Assert.AreEqual(winners , competition.winners);
            Assert.AreEqual(participant, competition.Participants);
            Assert.AreEqual(tracks, competition.Tracks);

        }
        [Test]
        public void DriverGettersSetters()
        {
            Driver driver = new Driver("1");
            Assert.AreEqual("1" , driver.Name);
            String testName = "2";
            int testPoints = 2;
            driver.Name = testName;
            driver.Points = testPoints;
            Assert.AreEqual(testName, driver.Name);
            Assert.AreEqual(testPoints, driver.Points);

        }

        [Test]
        public void EquipmentGettersSetters()
        {
            Equipment equipment = new Equipment();
            int prefprmace = 1;
            int quality = 1;
            int speed = 1;

            equipment.Performace = prefprmace;
            equipment.Quality = quality;
            equipment.Speed = speed;

            Assert.AreEqual(prefprmace, equipment.Performace);
            Assert.AreEqual(quality, equipment.Quality);
            Assert.AreEqual(speed, equipment.Speed);


        }


        [Test]
        public void SectionGettersSetters()
        {
            Section section = new Section(SectionTypes.Finish);
            section.SectionType = SectionTypes.LeftCorner;
            Assert.AreEqual(SectionTypes.LeftCorner , section.SectionType);
        }
        [Test]
        public void SectionDataGettersSetters()
        {
            SectionData sectionData = new SectionData();

            Driver driver = new Driver("1");
            Driver driver2 = new Driver("2");
            int distance = 2;
            int distance2 = 5;

            sectionData.Left = driver;
            sectionData.Right = driver2;
            sectionData.DistanceLeft = distance;
            sectionData.DistanceRight = distance2;

            Assert.AreEqual(driver, sectionData.Left);
            Assert.AreEqual(driver2, sectionData.Right);
            Assert.AreEqual(distance, sectionData.DistanceLeft);
            Assert.AreEqual(distance2, sectionData.DistanceRight);
        }


        [Test]
        public void TrackGettersSetters()
        {
            Track track = new Track();
            String name = "woop";
            var sections = new LinkedList<Section>();
            track.Name = name;
            track.Sections = sections;

            Assert.AreEqual(name, track.Name);
            Assert.AreEqual(sections, track.Sections);

        }
    }
}
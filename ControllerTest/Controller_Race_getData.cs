using System;
using System.Collections.Generic;
using System.Text;
using Controller;
using Model;
using NUnit.Framework;

namespace ControllerTest
{
    [TestFixture]
    class Controller_Race_getData
    {

        private Race race;
        [SetUp]
        public void SetUp()
        {
            List<IParticipant> Participants = new List<IParticipant>
            {
                new Driver("1"),
                new Driver("2")
            };
            SectionTypes[] section = new SectionTypes[] { SectionTypes.RightCorner, SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Finish, SectionTypes.RightCorner, SectionTypes.Straight };
            Track track = new Track("shiny", section);
            race = new Race(track, Participants);
        }

        [Test]
        public void getSectionData_shouldnt_be_null()
        {

            Section section = new Section(SectionTypes.Finish);
            Assert.IsNotNull(race.GetSectionData(section));
        }
    }
}

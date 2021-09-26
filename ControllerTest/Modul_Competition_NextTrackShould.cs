using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Model;

namespace ControllerTest
{
    [TestFixture]
    class Modul_Competition_NextTrackShould
    {
        private Competition _competition;

        [SetUp]
        public void SetUp()
        {
            _competition = new Competition();
            

        }

        [Test]
        public void NextTrack_EmptyQueue_ReturnNull() {
            var result = _competition.NextTrack();
            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_OneInQueue_ReturnTrack()
        {
            Track track = new Track();
            _competition.Tracks.Enqueue(track);
            var result = _competition.NextTrack();
            Assert.AreEqual(track , result);
        }

        [Test]
        public void NextTrack_OneInQueue_RemoveTrackFromQueue()
        {
            Track track = new Track();
            _competition.Tracks.Enqueue(track);
            var result = _competition.NextTrack();
            result = _competition.NextTrack();

            Assert.IsNull(result);

        }
        [Test]
        public void NextTrack_TwoInQueue_ReturnNextTrack()
        {
            Track track = new Track();
            Track track2 = new Track();
            _competition.Tracks.Enqueue(track);
            _competition.Tracks.Enqueue(track2);

            var result = _competition.NextTrack();
            result = _competition.NextTrack();

            Assert.AreEqual(track2, result);

        }


    }
}

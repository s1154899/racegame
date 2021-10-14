using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Controller;
using model;

namespace race
{
    class Program
    {
        static void Main(string[] args)
        {

            _startRace();




            for (; ; )
            {
                Thread.Sleep(500);

            }
        }

        //runs after a driver changed section
        //redraws the track
        private static void Currentrace_onDriverChanged(object sender, Model.DriverChangedEventArgs eventArgs)
        {
            
            Visual.DrawTrack(eventArgs.track);
            
            
        }


        //runs after a race is finished
        //stops the current race then starts a new one or displays the endscreen.
        private static void Currentrace_finishedTrack(object sender, RaceFinishedArgs eventArgs)
        {
            _stopRace();
            
            if(!_startRace())
            {
                Visual.displayWinners(Data.MyProperty.winners);
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

            Visual.ClearScreen();

        }

        //starts the a race
        private static bool _startRace()
        {
            Data.NextRace();
            if (!Object.Equals(Data.Currentrace, null))
            {
                Data.Currentrace.onDriverChanged += Currentrace_onDriverChanged;
                Data.Currentrace.onRaceFinished += Currentrace_finishedTrack;
                Visual.writeTrackName(Data.Currentrace.track.Name);
                Visual.DrawTrack(Data.Currentrace.track);
                Data.Currentrace.Start();
                return true;
            }

            return false;

        }
    }
}

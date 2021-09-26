using System;
using System.Threading;
using Controller;
namespace race
{
    class Program
    {
        static void Main(string[] args)
        {

            Data.NextRace();
            Console.WriteLine(Data.Currentrace.track.Name);
            Visual.DrawTrack(Data.Currentrace.track);
            for (; ; )
            {
                Thread.Sleep(100);
            }
        }
    }
}

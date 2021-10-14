using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Model;

namespace Controller
{
    public class Data
    {
        public static Competition MyProperty { get; set; }
        public static Race Currentrace { get; set; }



        public static void NextRace() {
            if (Object.Equals(MyProperty,null)) {
                MyProperty = new Competition();
                MyProperty.AddParticipants();
                MyProperty.AddTrack();
            }
            Track track = MyProperty.NextTrack();
            if (!Object.Equals(track,null)) {
                Currentrace = new Race(track,MyProperty.Participants);
            }
        }





    }
}

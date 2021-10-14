using System;
using System.Collections.Generic;
using System.Text;
using Model;

namespace Model
{
    public class DriverChangedEventArgs : EventArgs
    {
        public Track track;

        public DriverChangedEventArgs(Track track)
        {
            this.track = track;
        }

    }
}

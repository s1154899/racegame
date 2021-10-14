using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public interface IEquipment
    {
        int Quality { get; set; }
        int Performace { get; set; }
        int Speed { get; set; }
        bool isBroken { get; set; }

        void randomEquipment();
        void breaks();

    }
}

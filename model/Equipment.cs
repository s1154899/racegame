using System;
using Model;

namespace model
{
    public class Equipment : IEquipment
    {
        public int Quality { get; set; }
        public int Performace { get; set; }
        public int Speed { get; set; }
        public bool isBroken { get; set; }

        public void breaks()
        {
            int breakChance = Quality % 10;
            if (breakChance > new Random().Next(10))
            {
                isBroken = true;
                Performace = 0;
                Speed = 0;

            }

        }

        public void randomEquipment()
        {
            Random rand = new Random();
            Speed = rand.Next(20, 30);
            Performace = rand.Next(1, 5);
            Quality = rand.Next(1, 9);

            isBroken = false;

        }
    }
}
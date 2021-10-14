using System;
using System.Collections.Generic;
using System.Text;
using model;

namespace Model
{
    public class Driver : IParticipant
    {
        public string Name { get ; set ; }
        public int Points { get ; set ; }
        public IEquipment Equipment { get ; set ; }
        public TeamColors TeamColor { get ; set ; }


        public Driver(string name)
        {
            Name = name;
            Equipment = new Equipment();

            TeamColor = TeamColors.Red;
        }

        public Driver(String name, TeamColors teamColor):this(name)
        {
            TeamColor = teamColor;
        }
    }
}

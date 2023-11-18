﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dungeon
{
    class ArmourItem : Item
    {
        int Defence;
        public ArmourItem(string name, string description, int defence, int value, bool locked = false) : base(name, description, value, locked) 
        {
            Name = name;
            Description = description;
            Value = value;
            Defence = defence;
            IsArmour = true;
        }

        public int GetDefence() { return Defence; }

        public override string ToString()
        {
            return $"{Name}. -{Defence} enemy damage. Worth {Value} gold";
        }


    }
}

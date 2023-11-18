﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dungeon
{
    class SpellItem : Item
    {
        private int SpellDamage;
        private int ManaDrain;
        public SpellItem(string name, string description, int damage, int manaDrain, int value) : base(name, description, value) 
        { 
            Name = name;
            Description = description;
            Value = value;
            SpellDamage = damage;
            ManaDrain = manaDrain;
        }
        public int GetSpellDamage()
        {
            return SpellDamage;
        }
        public int GetManaDrain()
        {
            return ManaDrain;
        }
        public override string ToString()
        {
            return $"{Name}: {SpellDamage} damage: Worth {Value}";
        }
    }
}

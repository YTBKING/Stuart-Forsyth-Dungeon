﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dungeon
{
    class Celestial : BossCreature
    {
        public Celestial(string name, int health, int powerlvl, int xp, int gold) : base(name, health, xp, powerlvl, gold) { }
    }
}
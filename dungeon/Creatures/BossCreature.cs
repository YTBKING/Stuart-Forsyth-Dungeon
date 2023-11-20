﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dungeon
{
    class BossCreature : Creature
    {
        public BossCreature(string name, int health, double xp, int speed, int powerLvl, int gold) : base(name, health, xp, gold, speed, powerLvl) { }

        public override int GetAttackDamage(ArmourItem armour)
        {
            return (((random.Next(1, 10)) * PowerLvl) - armour.GetDefence());
        }

    }
}

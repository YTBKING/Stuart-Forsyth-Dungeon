using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dungeon
{
    class BossCreature : Creature
    {
        public BossCreature(string name, int health, Item drop, double xp, int powerLvl, Item drop2 = null) : base(name, health, xp, powerLvl, drop, drop2) { PowerLvl = powerLvl; }

        public override int GetAttackDamage(ArmourItem armour)
        {
            return ((random.Next(1, 10)) * PowerLvl - armour.GetDefence());
        }

    }
}

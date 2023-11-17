using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dungeon
{
    class DragonCreature : Creature
    {
        public DragonCreature(double xp, int powerLvl, Item drop = null, Item drop2 = null) : base("dragon", 100, xp, powerLvl, drop, drop2) { }


        public override int GetAttackDamage(ArmourItem armour)
        {
            if (random.Next(1, 3) == 1)
            {
                Console.WriteLine("The dragon engulfs you with his fiery breath");
                return 9999999;
            }
            else
            {
                return base.GetAttackDamage(armour);
            }
        }
        public override bool TakeSpellDamage(string spell, int damage)
        {
            if (spell == "frostbolt")
            {
                return true;
            }
            else
            {
                return base.TakeSpellDamage(spell, damage);
            }
        }
    }
}

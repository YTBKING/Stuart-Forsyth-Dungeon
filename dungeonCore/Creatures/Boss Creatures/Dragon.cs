using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
    class Dragon : Creature
    {
        public Dragon(double xp, int powerLvl, int speed, int gold) : base("dragon", 100, xp, gold, speed, powerLvl) { }


        public override int GetAttackDamage(ArmourItem armour)
        {
            if (random.Next(1, 100) == 1)
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

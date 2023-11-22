using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
    class DemonicCreature : Creature
    {
        public DemonicCreature(string name, int health, double xp, int gold, int speed = 1, int powerlvl = 0) : base(name, health, xp, gold, speed, powerlvl) { }

        public override bool TakeDamage(int damage, WeaponItem weapon = null)
        {
            if (weapon != null)
            {

                if (weapon.GetTrueRarity() == "Holy")
                {
                    Health -= damage * 2;
                }
                else
                {
                    Health -= damage;
                }
            }
            else
            {
                Health -= damage;
            }
            if (Health <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
    class BossCreature : Creature
    {
        protected string CreatureType;
        public BossCreature(string name, int health, double xp, int speed, int powerLvl, int gold, string creatureType = "Normal") : base(name, health, xp, gold, speed, powerLvl) 
        {
            CreatureType = creatureType;
        }

        public override int GetAttackDamage(ArmourItem armour)
        {
            return (((random.Next(1, 10)) * PowerLvl) - armour.GetDefence());
        }

        public override bool TakeDamage(int damage, WeaponItem weapon = null)
        {
            if (weapon != null) 
            { 
                switch (CreatureType)
                {
                    case "Frozen":
                        if (weapon.GetTrueRarity() == "Blazing")
                        {
                            damage *= 2;
                        }

                        break;
                    case "Blazing":
                        if (weapon.GetTrueRarity() == "Frozen")
                        {
                            damage *= 2;
                        }
                        break;
                    case "Demonic":
                        if (weapon.GetTrueRarity() == "Holy")
                        {
                            damage *= 2;
                        }
                        break;
                    case "Holy":
                        if (weapon.GetTrueRarity() == "Demonic")
                        {
                            damage *= 2;
                        }
                        break;
                    default: 
                        break;
                }


            }
            TrueDamage = damage;
            Health -= damage;
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

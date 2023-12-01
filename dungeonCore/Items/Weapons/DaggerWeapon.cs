using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
    class DaggerWeapon : WeaponItem
    {
        public DaggerWeapon(string name, string description, int damage, int value, int critchance = 5, string rarity = "Common", bool locked = false) : base(name, description, damage, value, rarity, 70, locked) 
        { 
            critChance = critchance;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dungeon
{
    class FoodItem : Item
    {
        private int HealAmount;
        public FoodItem(String name, String description, int heals, int value, bool locked = false) : base(name, description, value, locked, true)
        {
            HealAmount = heals;
        }
        public override int GetHeals()
        {
            return HealAmount;
        }
        public override string ToString()
        {
            return $"{Name}: +{HealAmount} health: Worth {Value} gold";
        }
    }
}

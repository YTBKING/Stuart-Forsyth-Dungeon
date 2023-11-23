using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
    class FoodItem : Item
    {
        #region "Properties"
        protected int HealAmount;
        protected int ManaAmount;
        #endregion
        public FoodItem(string name, String description, int heals, int manaheals, int value, bool locked = false) : base(name, description, value, locked, true)
        {
            HealAmount = heals;
            ManaAmount = manaheals;
        }

        #region "Get Properties"
        public override int GetHeals()
        {
            return HealAmount;
        }

        public override int GetManaHeals()
        {
            return ManaAmount; 
        }
        #endregion
        public override string ToString()
        {
            return $"{Name}: [italic red]+{HealAmount}[/] health: +{ManaAmount} mana: Worth {Value}g";
        }
    }
}

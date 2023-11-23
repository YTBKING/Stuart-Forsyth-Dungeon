using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
    class PotionItem : FoodItem
    {
        private bool PermaBuff = false;
        public PotionItem(string name, string description, int healamount, int manahealamount, int value, bool permaBuff = false) : base(name, description, healamount, manahealamount, value) 
        {
            PermaBuff = permaBuff;
        }

        public bool IsPermaBuff()
        {
            return PermaBuff;
        }
    }
}

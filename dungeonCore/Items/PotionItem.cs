using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
    class PotionItem : FoodItem
    {
        public PotionItem(string name, string description, int healamount, int manahealamount, int value) : base(name, description, healamount, manahealamount, value) { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dungeon
{
    class PotionItem : FoodItem
    {
        public PotionItem(string name, string description, int healamount, int value) : base(name, description, healamount, value) { }
    }
}

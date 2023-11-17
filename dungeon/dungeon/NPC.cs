using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dungeon
{
    class NPC
    {
        private string Name;
        public List<Item> SellingItems = new List<Item>();
        public List<int> SellingCosts = new List<int>();

        public NPC(string name)
        {
            Name = name;
        }

        public string GetName() { return Name; }

        public void AddVendorItems(Item item, int cost)
        {
            SellingItems.Add(item);
            SellingCosts.Add(cost);
        }

        public void BuyItem(Player player, string wanting)
        {
            Item itemWanted;
            bool itemFound = false;
            int intIndex = -1;
            for (int i = 0; i < SellingItems.Count; i++)
            {
                if (wanting == SellingItems[i].GetName())
                {
                    itemWanted = SellingItems[i];
                    itemFound = true;
                    intIndex = i;
                    break;
                }

            }
            if (itemFound)
            {
                if (player.gold >= SellingCosts[intIndex])
                {
                    player.AddItem(SellingItems[intIndex]);
                    player.gold -= SellingCosts[intIndex];
                    Console.WriteLine($"You bought a {SellingItems[intIndex].GetName()} for {SellingCosts[intIndex]}");
                }
            }


        }
    }
}

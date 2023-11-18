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
                Console.WriteLine("How many would you like to buy?");
                int num = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine($"This will cost {SellingCosts[intIndex] * num} gold. Are you sure (Y/N)");
                string answer = Console.ReadLine().ToLower();
                if (answer == "y")
                {
                    if (player.gold >= SellingCosts[intIndex] * num)
                    {
                        for (int i = 1; i <= num; i++)
                        {
                            player.AddItem(SellingItems[intIndex]);
                        }
                        player.gold -= SellingCosts[intIndex] * num;
                        Console.WriteLine($"You bought {num} {SellingItems[intIndex].GetName()} for {SellingCosts[intIndex] * num}");
                        /*SellingItems.Remove(SellingItems[intIndex] );
                        SellingCosts.Remove(SellingCosts[intIndex] );*/
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You cannot afford this");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                }
            }


        }

        public void UpgradeWeapon(Player player, WeaponItem weapon)
        {
            Console.WriteLine($"This will cost {weapon.GetUpgradeCost()} to upgrade\nIs that okay (Y/N)");
            string answer = Console.ReadLine().ToUpper();
            if (answer == "Y")
            {
                if (player.gold >= weapon.GetUpgradeCost()) 
                {
                    weapon.ImproveRarity(player, 1, weapon);
                    player.gold -= weapon.GetUpgradeCost();
                    weapon.AlterUpgradeCost(1.75);
                    Console.WriteLine("You upgraded your " + weapon.GetName());
                }
                else 
                { 
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You do not have enough gold for this");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                }
            }
        }

        public List<Item> GetVendorItems()
        {
            return SellingItems;
        }
        public List<int> GetVendorCosts()
        {
            return SellingCosts;
        }
    }
}

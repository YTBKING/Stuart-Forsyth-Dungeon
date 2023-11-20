using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dungeon
{
    class NPC
    {
        #region "Properties"
        protected string Name;
        protected string Dialogue;
        public List<Item> SellingItems = new List<Item>();
        public List<int> SellingCosts = new List<int>();
        public List<int> SellingAmount = new List<int>();
        public List<Item> Gifts = new List<Item>();
        #endregion

        public NPC(string name, string dialogue = "")
        {
            Name = name;
            Dialogue = dialogue;

        }

        #region "Get Properties
        public string GetName() { return Name; }
        public string GetDialogue() {  return Dialogue; }
        public List<Item> GetVendorItems()
        {
            return SellingItems;
        }
        public List<int> GetVendorCosts()
        {
            return SellingCosts;
        }
        public List<int> GetSellingAmount()
        {
            return SellingAmount;
        }
        public List<Item> GetGifts() { return Gifts; }
        #endregion

        #region "Gifts"
        public void AddGift(Item gift)
        {
            Gifts.Add(gift);
        }
        #endregion

        #region "Vendor Items"
        public void AddVendorItems(Item item, int cost, int num = -1)
        {
            SellingItems.Add(item);
            SellingCosts.Add(cost);
            SellingAmount.Add(num);
        }
        #endregion

        #region "Shop Interction"
        public bool BuyItem(Player player, string wanting)
        {
            Item itemWanted;
            bool itemFound = false;
            int intIndex = -1;
            int num = 1;
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
                if (SellingAmount[intIndex] == -1)
                {
                    Console.WriteLine($"How many would you like to buy?");
                    num = Convert.ToInt32(Console.ReadLine());
                }
                else
                {
                    if (SellingAmount[intIndex] == 1)
                    {
                        num = 1;
                    }
                    else
                    {
                        Console.WriteLine($"How many would you like to buy? 1 -> {SellingAmount[intIndex]}");
                        num = Convert.ToInt32(Console.ReadLine());
                        if (num > SellingAmount[intIndex])
                        {
                            Console.WriteLine($"{Name} does not have that many to sell");
                            return false;
                        }
                    }

                }

                Console.WriteLine($"This will cost {SellingCosts[intIndex] * num} gold. Are you sure (Y/N)");
                string answer = Console.ReadLine().ToLower();
                if (answer == "y")
                {
                    if (player.gold >= SellingCosts[intIndex] * num)
                    {
                        if (SellingAmount[intIndex] != -1)
                        {
                            for (int i = 1; i <= num; i++)
                            {
                                player.AddItem(SellingItems[intIndex]);
                            }
                            player.gold -= SellingCosts[intIndex] * num;
                            Console.WriteLine($"You bought {num} {SellingItems[intIndex].GetName()} for {SellingCosts[intIndex] * num}");
                            SellingAmount[intIndex] -= num;
                            if (SellingAmount[intIndex] == 0)
                            {
                                SellingItems.RemoveAt(intIndex );
                                SellingCosts.RemoveAt(intIndex );
                                SellingAmount.RemoveAt(intIndex );
                            }
                        }


                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You cannot afford this");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                }
    
            }
            return true;

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
        #endregion


    }
}

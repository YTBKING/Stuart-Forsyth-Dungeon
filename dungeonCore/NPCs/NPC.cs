using Dungeon;
using dungeonCore.Game;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dungeon
{
    
    public class NPC
    {
        #region "Properties"
        [JsonInclude]
        protected string Name;
        [JsonInclude]
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
        public void RemoveGift(Item gift)
        {
            Gifts.Remove(gift);
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
                if (wanting.ToLower() == SellingItems[i].GetName().ToLower())
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
                    while (true)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine($"How many would you like to buy?");
                        try
                        {
                            num = Convert.ToInt32(Console.ReadLine());
                            break;
                        }
                        catch(FormatException) { AnsiConsole.MarkupLine("[italic grey]Please enter a[/] [italic red]valid[/][italic grey] number[/]"); }
                    }
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
                        for (int i = 1; i <= num; i++)
                        {
                            player.AddItem(SellingItems[intIndex]);
                        }
                        player.gold -= SellingCosts[intIndex] * num;
                        Console.WriteLine($"You bought {num} {SellingItems[intIndex].GetName()} for {SellingCosts[intIndex] * num}");
                        if (SellingAmount[intIndex] != -1)
                        {
                            SellingAmount[intIndex] -= num;
                        }
                        if (SellingAmount[intIndex] == 0)
                        {
                            SellingItems.RemoveAt(intIndex);
                            SellingCosts.RemoveAt(intIndex);
                            SellingAmount.RemoveAt(intIndex);
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
        public void ReforgeWeapon(Player player, WeaponItem weapon)
        {
            if (weapon.Rarity == "Demonic" || weapon.Rarity == "Holy" || weapon.Rarity == "Frozen" || weapon.Rarity == "Blazing")
            {
                bool running = true;
                int ReforgeCost = 150;
                string oldRarity = weapon.GetRarity();
                string reforge = "";
                while (running)
                {
                    reforge = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Which modifier would you like?\n")
                    .PageSize(4)
                    .AddChoices(new[] {
                        "Demonic", 
                        "Holy", 
                        "Frozen", 
                        "Blazing",
                     }));
                    if (reforge == weapon.GetTrueRarity())
                    {
                        AnsiConsole.MarkupLine("[italic grey]Modifier cannot be the same as before[/]");
                        continue;
                    }
                    break;
                }
                string answer = AnsiConsole.Ask<string>($"[italic grey]This will cost [/][italic 178]{ReforgeCost}[/][italic grey] to reforge\nIs that okay (Y/N)[/] ").ToLower();
                if ( answer == "y" ) 
                {
                    weapon.Rarity = reforge;

                    AnsiConsole.MarkupLine($"[italic grey]You have changed from [/]{oldRarity}[italic grey] -> [/]{weapon.GetRarity()}");
                }
            }
            else { AnsiConsole.MarkupLine("[italic grey]This is not a valid weapon[/]"); }
        }
        public void UpgradeWeapon(Player player, WeaponItem weapon)
        {
            if (weapon.Rarity != "Demonic" && weapon.Rarity != "Holy" && weapon.Rarity != "Frozen" && weapon.Rarity != "Blazing")
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
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                AnsiConsole.MarkupLine("You cannot upgrade [bold darkred]this[/][italic red] weapon here[/]");
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
        }
        public void UpgradeSummon(Player player, SummonItem summon)
        {
            Console.WriteLine($"This will cost {summon.GetBuffCost()} to upgrade\nIs that okay (Y/N)");
            string answer = Console.ReadLine().ToUpper();
            if (answer == "Y")
            {
                if (player.gold >= summon.GetBuffCost())
                {
                    summon.AdjustLvl(1);
                    player.gold -= summon.GetBuffCost();
                    Console.WriteLine("You upgraded your " + summon.GetName());
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

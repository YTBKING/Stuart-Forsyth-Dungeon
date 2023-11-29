using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Dungeon
{
    public class Player
    {
        #region "Properties"
        private string Name;
        public int Health;
        public int MaxHealth;
        public int Strength = 0;
        public int Agility = 0;
        private string classChoice;
        private Room Location;
        public List<Item> Inventory = new List<Item>();
        public Dictionary<string, int> SpellBook = new Dictionary<string, int> {};
        private Random random;
        public WeaponItem EquipedWeapon;
        private Item key;
        private int level = 1;
        private double Experience = 0;
        private double xpNeeded = 150;
        private const double XP_INCREASE = 1.75;
        private int strengthChange = 5;
        private int HealthChange = 20;
        public int gold = 50;
        public ArmourItem Armour;
        private string InvDesc = "";
        public List<string> SpellNames = new List<string>();
        public List<int> SpellDamages = new List<int>();
        public int MaxMana = 100;
        public int Mana = 100;
        public List<int> ManaDrains = new List<int>();
        public NPC talking;
        public List<NoteItem> NoteBook = new List<NoteItem>();
        private bool IsBlocking = false;
        #endregion

        public Player(int health, WeaponItem equipedWeapon, ArmourItem armour, KeyItem Key)
        {
            Health = health;
            MaxHealth = health;
            random = new Random();
            EquipedWeapon = equipedWeapon;
            key = Key;
            Armour = armour;
            
        }
        public void SetName(string name)
        {
            Name = name;
        }
        public bool IsDead()
        {
            return Health == 0;
        }

        public bool CheckBlocking()
        {
            return IsBlocking;
        }
        public void ChangeBlocking(bool change) 
        { 
            IsBlocking = change;
        }

        #region "Stats and changing"
        public int GetAgility()
        {
            return Agility;
        }
        public void SetClass(string choice)
        {
            classChoice = choice;
        }

        public void SetAgility(int num)
        {
            Agility = num;
        }

        public int GetMana()
        {
            return Mana;
        }

        public void SetMana(int mana)
        {
            MaxMana = mana;
            Mana = mana;
        }

        public int GetStrength()
        {
            return Strength;
        }

        public void SetStrength(int num)
        {
            Strength = num;
        }
        public int GetHealth()
        {
            return Health;
        }
        public int AddHealth(int health)
        {
            Health += health;
            return Health;
        }
        public int RemoveHealth(int damage)
        {
            Health -= damage;
            return Health;
        }
        public void SetHealth(int health)
        {
            Health = health;
        }

        #endregion

        #region "Items, Armour and spells"
        public void AddGold(int Gold)
        {
            gold += Gold;
        }

        public Item GetKey()
        {
            return key;
        }

        public void AddNote(NoteItem note)
        {
            NoteBook.Add(note);
        }

        public List<NoteItem> GetNoteBook()
        {
            return NoteBook;
        }

        public void AddItem(Item item)
        {
            Inventory.Add(item);
        }

        public void AddSpell(SpellItem spell)
        {
            SpellBook.Add(spell.GetName(), spell.GetSpellDamage());
        }

        public void RemoveItem(Item item, int num = 1)
        {
            for (int i = 1; i <= num; i++)
            {
                Inventory.Remove(item);
            }

        }
        public ArmourItem GetArmour()
        {
            return Armour;
        }

        public void EquipItem(Item weapon)
        {
            try 
            { 
                EquipedWeapon = (WeaponItem)weapon;
                Console.WriteLine("You have equipped " +  weapon.GetName());
                    
            }
            catch(InvalidCastException) 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("This is not a weapon");
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
                
        }

    

        public void WearArmour(Item armour)
        {
            Armour = (ArmourItem)armour;

        }

        public bool ObtainedKey()
        {
            return Inventory.Contains(key);
        }
        public bool LearnSpell(Item spell)
        {   try
            {
                AddSpell((SpellItem)spell);
                UpdateSpellNamesList(spell.GetName());
                UpdateSpellDamageList(((SpellItem)spell).GetSpellDamage());
                UpdateManaDrainList(((SpellItem)spell).GetManaDrain());
                return true;
            }
            catch (InvalidCastException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("This is not a Spell book");
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            return false;
        }
        
        public void UpdateSpellNamesList(string spell)
        {
            SpellNames.Add(spell);
        }

        public void UpdateSpellDamageList (int value)
        {
            SpellDamages.Add(value);
        }

        public void UpdateManaDrainList (int value)
        { 
            ManaDrains.Add(value);
        }

        public List<string> GetSpellNames()
        {
            return SpellNames;
        }

        public List<int> GetManaDrain()
        {
            return ManaDrains;
        }

        public List<int> GetSpellDamages()
        {
            return SpellDamages;
        }
        public string GetInventory()
        {
            InvDesc = "You are holding:";


            string foods = "\n[italic underline red](Food Items)[/]\n";
            bool foodsC = false;

            string weapons = "\n[italic underline royalblue1](Weapons)[/]\n";
            bool weaponsC = false;

            string staffs = "\n[italic underline 39](Staffs)[/]\n";
            bool staffsC = false;

            string armours = "\n[italic underline deeppink4_1](Armours)[/]\n";
            bool armoursC = false;

            string spelltomes = "\n[italic underline 138](Spell Books)[/]\n";
            bool spelltomesC = false;

            string materials = "\n[italic underline green](Other Items)[/]\n";
            bool materialsC = false;

            string spells = "\n[italic underline 75](Spells)[/]\n";
            bool spellsC = false;

            string keyitems = "\n[italic underline 178](Key Items)[/]\n";
            bool keyitemsC = false;

            string notes = "\n[italic underline 94](Notes)[/]\n";
            bool notesC = false;

            string currWearing = $"\nEquipped Armour: {Armour}";
            string currUsing = $"\nEquipped Weapon: {EquipedWeapon}";

            for (int i = 0; i < Inventory.Count; i++)
            {
                if (Inventory[i] is FoodItem)
                {
                    if (!foods.Contains(Inventory[i].GetName()))
                    {
                        foods += $"{Inventory[i]}:";
                        foods += " x" + Inventory.Count(n => n == Inventory[i]) + "\n";

                    }
                    foodsC = true;
                }
                else if (Inventory[i] is StaffWeapon)
                {
                    staffs += $"{Inventory[i]}\n";
                    staffsC = true;
                }
                else if (Inventory[i] is WeaponItem)
                {
                    weapons += $"{Inventory[i]}\n";
                    weaponsC = true;
                }
                else if (Inventory[i] is ArmourItem)
                {
                    armours += $"{Inventory[i]}\n";
                    armoursC = true;
                }
                else if (Inventory[i] is SpellItem)
                {
                    spelltomes += $"{Inventory[i]}\n";
                    spelltomesC = true;
                }
                else if (Inventory[i] is KeyItem)
                {
                    if (!keyitems.Contains(Inventory[i].GetName()))
                    {
                        keyitems += $"{Inventory[i]}:";
                        keyitems += " x" + Inventory.Count(n => n == Inventory[i]) + "\n";
                    }
                    keyitemsC = true;
                }
                else if (Inventory[i] is NoteItem)
                {
                    notes += $"{Inventory[i]}";
                    notesC = true;
                }
                else
                {
                    if (!materials.Contains(Inventory[i].GetName()))
                    {
                        materials += $"{Inventory[i]}:";
                        materials += " x" + Inventory.Count(n => n == Inventory[i]) + "\n";
                    }
                    materialsC = true;
                }
                
            }
            if (!keyitemsC) 
            {
                keyitems = "\n[italic underline 178]You have no Key Items[/]\n";
            }
            if (!weaponsC)
            {
                weapons = "\n[italic underline royalblue1]You have no weapons... somehow?[/]\n";
            }
            if (!armoursC)
            {
                armours = "\n[italic underline deeppink4_1]You have no armours... Did you peel your skin off? Gross[/]\n";
            }
            if (!spelltomesC)
            {
                spelltomes = "\n[italic underline 138]You have no spell books[/]\n";
            }
            if (!materialsC)
            {
                materials = "\n[italic underline green]You have no other items[/]\n";
            }
            if (!foodsC)
            {
                foods = "\n[italic underline red]You have no food items[/]\n";
            }
            if (!staffsC)
            {
                staffs = "\n[italic underline 39]You have no staffs[/]\n";
            }
            if (!notesC)
            {
                notes = "\n[italic underline 94]You have no notes[/]\n";
            }
            for (int i = 0; i < SpellBook.Count; i++ )
            {
                spells += $"[italic 75]{GetSpellNames()[i]}[/]: [italic red]{GetSpellDamages()[i]}[/] damage: [italic 75]-{GetManaDrain()[i]}[/] mana\n";
                spellsC = true;
            }
            if (!spellsC)
            {
                spells = "\n[italic underline 75]You have no spells[/]\n";
            }

            // Ask for the user's inventory choice
            string selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold underline fuchsia]What inventory would you like to view?[/]")
                    .PageSize(10)
                    .AddChoices(new[] {
             "All", "Key Items", "Food Items", "Armours",
            "Weapons", "Staffs", "Spell Books",
            "Other Items", "Spells", "Notes",
                    }));

            // Echo the selection back to the terminal
            switch (selection)
            {
                case "Key Items":
                    return InvDesc + keyitems;
                case "Food Items":
                    return InvDesc + foods;
                case "Armours":
                    return InvDesc + armours + currWearing;
                case "Weapons":
                    return InvDesc + weapons + currUsing;
                case "Staffs":
                    return InvDesc + staffs;
                case "Spell Books":
                    return InvDesc + spelltomes;
                case "Other Items":
                    return InvDesc + materials;
                case "Spells":
                    return InvDesc + spells;
                case "Notes":
                    return InvDesc + notes;
                case "All":
                    return InvDesc + keyitems + foods + weapons + staffs + armours + spelltomes + materials + spells + notes + currUsing + currWearing;
                default:
                    AnsiConsole.MarkupLine("Sorry but that is not a valid choice");
                    break;
            }
            return InvDesc + keyitems + foods + weapons + staffs + armours + spelltomes + materials + spells + notes + currUsing + currWearing;
        }
        #endregion

        #region "Levelling and experience"
        public void AdjustExperience(double experience)
        {
            Experience += experience;
            while (Experience >= xpNeeded)
            {
                level++;
                xpNeeded *= XP_INCREASE;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("You Leveled Up");
                bool invested = false;
                while (!invested)
                {
                    AnsiConsole.MarkupLine("[italic aqua]Would you like to level up Strength, Health, Agility or Mana?[/] ");
                    Console.ForegroundColor = ConsoleColor.White;
                    string invest = Console.ReadLine().ToUpper();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    switch (invest)
                    {
                        case "STRENGTH":
                            Console.Write($"\nYou increased Strength: {Strength}");
                            Strength ++;
                            Console.Write($" -> {Strength}\n");

                            invested = true;
                            break;
                        case "HEALTH":
                            Console.Write($"\nYou increased Health: {MaxHealth}");
                            MaxHealth += HealthChange;
                            Health += HealthChange;
                            HealthChange = Convert.ToInt32(HealthChange * 1.5);
                            Console.Write($" -> {MaxHealth}\n");
                            invested = true;
                            break;
                        case "AGILITY":
                            Console.Write($"\nYou increased Agility: {Agility}");
                            Agility++;
                            Console.Write($" -> {Agility}\n");
                            invested = true;
                            break;
                        case "MANA":
                            Console.Write($"\nYou increased Mana: {Mana}");
                            Mana += 5;
                            Console.Write($" -> {Mana}\n");
                            invested = true;
                            break;
                        default:
                            Console.WriteLine("Please enter a valid option");
                            continue;



                    }
                }
            }
        }

        public double GetExperience()
        {
            return Experience;
        }

        public double GetXPNeeded()
        {
            return xpNeeded;
        }

        public int GetLevel()
        {
            return level;
        }
        #endregion

        #region "Location"
        public Room GetLocation()
        {
            return Location;
        }

        public void SetLocation(Room location)
        {
            Location = location;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\x1B[3m");
            AnsiConsole.Markup(Location.GetDescription());
            Console.WriteLine("\x1B[0m");


        }

        private void Move(String direction)
        {
            List<String> exits = Location.GetDirections();
            Boolean directionFound = false;
            for (int i = 0; i < exits.Count; i++)
            {
                if (direction == exits[i])
                {
                    directionFound = true;
                    if (!Location.GetConnections()[i].GoThrough(this, direction, key))
                    {
                        Console.WriteLine($"You can't go {direction}");
                    }

                }
            }


            if (!directionFound)
            {
                Console.WriteLine($"There is no exit to the {direction}");
            }

        }

        #endregion

        #region "Other commands"
        public void GetRecentNPC()
        {
            try
            {
                talking = Location.GetNPC()[0];
            } catch(ArgumentOutOfRangeException) { AnsiConsole.MarkupLine("No NPC to talk to"); }
        }
        public bool DoesDodge()
        {
            Random rand = new Random();
            int chance = 0;
            int tempChance = 0;
            for (int i = 0; i < Agility; i++) 
            { 
               tempChance = rand.Next(0, 1000);
                if (tempChance > chance)
                {
                    chance = tempChance;
                }
            }

            if (chance == 1000)
            {
                return true;
            }
            else return false;
        }

        public void Eat(string food)
        {
            int foodPosition;
            var found1 = Inventory.Find(item => item.GetName().ToLower() == food.ToLower());
            if (found1 != null && found1 is PotionItem)
            {
                PotionItem potion = found1 as PotionItem;
                if (potion.IsPermaBuff())
                {
                    MaxHealth += 20;
                }

            }

            for (foodPosition = 0; foodPosition < Inventory.Count + 1; foodPosition++)
            {
                if (foodPosition < Inventory.Count)
                {
                    if (Inventory[foodPosition].GetName().ToLower() == food.ToLower() && !Inventory[foodPosition].IsEdible)
                    {
                        Health = 0;
                        IsDead();
                        return;
                    }
                    if (Inventory[foodPosition].GetName().ToLower() == food.ToLower() && Inventory[foodPosition].IsEdible)
                    {
                        if (Health + Inventory[foodPosition].GetHeals() <= MaxHealth)
                        {

                            Health += Inventory[foodPosition].GetHeals();
                            
                        }
                        else
                        {
                            Health = MaxHealth;
                        }
                        if (Mana + Inventory[foodPosition].GetManaHeals() <= MaxMana)
                        {
                            Mana += Inventory[foodPosition].GetManaHeals();
                        }
                        else
                        {
                            Mana = MaxMana;
                            
                        }
                        Console.WriteLine($"You ate {food}");
                        Inventory.RemoveAt(foodPosition);
                        break;

                    }
                }
                else
                {
                    Console.WriteLine("You do not have this item");
                }
            }
        }

        public bool Open(Chest chest)
        {
            if (chest.KeyRequired())
            {
                if (ObtainedKey())
                {
                    string answer = AnsiConsole.Ask<string>("[italic red]Would you like to use the key? Y/N[/]\n> ").ToLower();

                    if (answer == "y")
                    {
                        chest.UnlockChest();
                    }

                }
                else { Console.WriteLine("You need a key to unlock the chest"); return false; }
            }
            if (chest.GetContents().Count != 0 && !chest.KeyRequired())
            {
                string contents = "";
                while (chest.GetContents().Count != 0)
                {
                    
                    if (!contents.Contains(chest.GetContents()[0].GetName()))
                    {
                        AnsiConsole.Markup($"[italic grey]You got[/] [italic 178]{chest.GetContents().Count(n => n == chest.GetContents()[0])} {chest.GetContents()[0].GetName()}[/]\n");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                    contents += chest.GetContents()[0].GetName();
                    AddItem(chest.GetContents()[0]);
                    chest.RemoveItem(chest.GetContents()[0]);
                }
                GetLocation().RemoveChest(chest);
                return true;
            }
            else if (chest.GetContents().Count == 0) { Console.WriteLine("The chest is already empty"); }
            return false;
        }
        public string Talk(NPC npc)
        {
            string shopui = "";
            if (npc.GetDialogue() != "")
            {

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\x1B[0m");
                Console.Write($"{npc.GetDialogue()}\n\n");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\x1B[0m");
            }

            if (npc.GetGifts().Count != 0)
            {
                int giftint = 0;
                Item gift = null;
                string giftui = "";
                while (npc.GetGifts().Count > 0)
                {

                    gift = npc.GetGifts()[giftint];
                    AddItem(gift);
                    npc.RemoveGift(gift);
                    if (gift is WeaponItem)
                    {
                        if (!giftui.Contains(gift.GetName()))
                        {
                            WeaponItem weapon = (WeaponItem)gift;
                            giftui += $"{weapon.GetRarity} [italic green]{gift.GetName()}[/]";
                            giftui += " x" + npc.GetGifts().Count(n => n == gift) + "\n";
                        }
                    }

                    if (!giftui.Contains(gift.GetName()))
                    {
                       
                        giftui += $"[italic green]{gift.GetName()}[/]";
                        giftui += " x" + npc.GetGifts().Count(n => n == gift) + "\n";
                    }
                }

                Console.WriteLine("Take this as a thank you for talking to me");
                Console.ForegroundColor = ConsoleColor.Green;
                AnsiConsole.Markup($"[italic blue]{npc.GetName()}[/] gave you {giftui}");
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }

            if (npc is SellerNPC)
            {
                for (int i = 0; i < npc.GetVendorItems().Count; i++)
                {
                    shopui += $"[italic grey]{i + 1}.[/]";
                    if (npc.GetVendorItems()[i] is WeaponItem)
                    {
                        WeaponItem weapon = (WeaponItem)npc.GetVendorItems()[i];
                        shopui += $" {weapon.GetRarity()}";
                    }
                    if (npc.GetSellingAmount()[i] != -1)
                    {

                        shopui += $" [italic grey]{npc.GetVendorItems()[i].GetName()}[/] - [italic 178]{npc.GetVendorCosts()[i]}g[/][italic grey] - x{npc.GetSellingAmount()[i]}[/]\n";
                    }
                    else
                    {
                        shopui += $" [italic grey]{npc.GetVendorItems()[i].GetName()}[/] - [italic 178]{npc.GetVendorCosts()[i]}g[/]\n";
                    }

                }
            }
            else if (npc is SmithNPC) 
            {
                shopui += $"You can upgrade your weapons here";
            }
            return shopui;

        }
        #endregion

    }
}

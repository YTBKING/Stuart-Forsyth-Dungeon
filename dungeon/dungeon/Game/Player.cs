using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace dungeon
{
    class Player
    {
        #region "Attributes and properties"
        private int Health;
        private int MaxHealth;
        private int Strength = 0;
        private int Agility = 0;
        private Room Location;
        private List<Item> Inventory = new List<Item>();
        private Dictionary<string, int> SpellBook = new Dictionary<string, int> {};
        private Random random;
        public WeaponItem EquipedWeapon;
        public bool KeyObtained = false;
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
        List<string> SpellNames = new List<string>();
        List<int> SpellDamages = new List<int>();
        public int MaxMana = 100;
        public int Mana = 100;
        private List<int> ManaDrains = new List<int>();
        private NPC talking;
        #endregion

        public Player(int health, WeaponItem equipedWeapon, ArmourItem armour, Item Key)
        {
            Health = health;
            MaxHealth = health;
            random = new Random();
            EquipedWeapon = equipedWeapon;
            key = Key;
            Armour = armour;
            
        }

        #region "Stats and changing"
        public int GetAgility()
        {
            return Agility;
        }

        public void AdjustAgility(int num)
        {
            Agility += num;
        }

        public int GetMana()
        {
            return Mana;
        }

        public int GetStrength()
        {
            return Strength;
        }
        public int GetHealth()
        {
            return Health;
        }
        public int AdjustHealth(int health)
        {
            Health += health;
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

        public void AddItem(Item item)
        {
            Inventory.Add(item);
        }

        public void AddSpell(SpellItem spell)
        {
            SpellBook.Add(spell.GetName(), spell.GetSpellDamage());
        }

        public void RemoveItem(Item item, int num)
        {
            for (int i = 1; i <= num; i++)
            {
                Inventory.Remove(item);
            }

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
            Console.WriteLine("You have put on " +  armour.GetName());
        }

        public bool ObtainedKey()
        {
            return KeyObtained;
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


            string foods = "\n(Food Items)\n";
            string weapons = "\n(Weapons)\n";
            string armours = "\n(Armours)\n";
            string spelltomes = "\n(Spell Books)\n";
            string materials = "\n(Other Items)\n";
            string currWearing = $"\nEquipped Armour: {Armour}";
            string currUsing = $"\nEquipped Weapon: {EquipedWeapon}";
            string spells = "\n(Spells)\n";
            string keyitems = "\n(Key Items)\n";
            for (int i = 0; i < Inventory.Count; i++)
            {
                if (Inventory[i] is FoodItem)
                {
                    if (!foods.Contains(Inventory[i].GetName()))
                    {
                        foods += $"{Inventory[i]}:";
                        foods += " x" + Inventory.Count(n => n == Inventory[i]) + "\n";
                    }

                }
                else if (Inventory[i] is WeaponItem)
                {
                    weapons += $"{Inventory[i]}\n";
                }
                else if (Inventory[i] is ArmourItem)
                {
                    armours += $"{Inventory[i]}\n";
                }
                else if (Inventory[i] is SpellItem)
                {
                    spelltomes += $"{Inventory[i]}\n";
                }
                else if (Inventory[i] is KeyItem)
                {
                    if (!keyitems.Contains(Inventory[i].GetName()))
                    {
                        keyitems += $"{Inventory[i]}:";
                        keyitems += " x" + Inventory.Count(n => n == Inventory[i]) + "\n";
                    }
                }
                else
                {
                    if (!materials.Contains(Inventory[i].GetName()))
                    {
                        materials += $"{Inventory[i]}:";
                        materials += " x" + Inventory.Count(n => n == Inventory[i]) + "\n";
                    }
                }
                
            }
            if (keyitems.Length <= 15) 
            {
                keyitems = "";
            }
            if (weapons.Length <= 13)
            {
                weapons = "";
            }
            if (armours.Length <= 13)
            {
                armours = "";
            }
            if (spelltomes.Length <= 17)
            {
                spelltomes = "";
            }
            if (materials.Length <= 17)
            {
                materials = "";
            }
            if (foods.Length <= 16)
            {
                foods = "";
            }
            for (int i = 0; i < SpellBook.Count; i++ )
            {
                spells += $"{GetSpellNames()[i]}: {GetSpellDamages()[i]} damage: -{GetManaDrain()[i]} mana\n";
            }

            return InvDesc + keyitems + foods + weapons + armours + spelltomes + materials + spells + currUsing + currWearing;
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
                while (!invested) {
                    Console.WriteLine("Would you like to level up Strength, Health, Agility or Mana? ");
                    Console.ForegroundColor = ConsoleColor.White;
                    string invest = Console.ReadLine().ToUpper();
                    if (invest == "STRENGTH")
                    {
                        Console.Write($"\nYou increased Strength: {Strength}");
                        Strength += strengthChange;
                        Console.Write($" -> {Strength}\n");
                        strengthChange = Convert.ToInt32(strengthChange * 1.5);
                        invested = true;
                    }
                    else if (invest == "HEALTH")
                    {
                        Console.Write($"\nYou increased Health: {Health}");
                        Health += HealthChange;
                        Console.Write($" -> {Health}\n");
                        invested = true;
                    }
                    else if (invest == "AGILITY")
                    {
                        Console.Write($"\nYou increased Agility: {Agility}");
                        Agility++;
                        Console.Write($" -> {Agility}\n");
                        invested = true;
                    }
                    else if (invest == "MANA")
                    {
                        Console.Write($"\nYou increased Mana: {Mana}");
                        Mana += 5;
                        Console.Write($" -> {Mana}\n");
                        invested = true;
                    }
                    else { Console.WriteLine("Please enter a valid option"); }
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
            Console.WriteLine(Location.GetDescription());
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
            talking = Location.GetNPC()[0];
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

            if (chance > 900)
            {
                return true;
            }
            else return false;
        }

        private void Eat(string food)
        {
            int foodPosition;
            if (food == "perma potion")
            {
                MaxHealth += 20;
            }

            for (foodPosition = 0; foodPosition < Inventory.Count + 1; foodPosition++)
            {
                if (foodPosition < Inventory.Count)
                {
                    if (Inventory[foodPosition].GetName() == food && Inventory[foodPosition].IsEdible)
                    {
                        if (Health + Inventory[foodPosition].GetHeals() <= MaxHealth)
                        {
                            Console.WriteLine($"You ate {food}");
                            Health += Inventory[foodPosition].GetHeals();
                            ;
                        }
                        else
                        {
                            Health = MaxHealth;
                        }
                        if (Mana + Inventory[foodPosition].GetHeals() <= MaxMana)
                        {
                            Console.WriteLine($"You ate {food}");
                            Mana += Inventory[foodPosition].GetHeals();
                        }
                        else
                        {
                            Mana = MaxMana;
                            Console.WriteLine($"You ate {food}");
                            
                        }
                        Inventory.RemoveAt(foodPosition);
                        break;

                    }
                    else if (Inventory[foodPosition].GetName() == food && !Inventory[foodPosition].IsEdible)
                    {
                        Console.WriteLine("You cant eat that");
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("You do not have this item");
                }
            }
        }

        private void Open(Chest chest)
        {
            if (chest.GetContents().Count != 0)
            {
                string contents = "";
                while (chest.GetContents().Count != 0)
                {
                    
                    if (!contents.Contains(chest.GetContents()[0].GetName()))
                    {
                        Console.WriteLine($"You got {chest.GetContents().Count(n => n == chest.GetContents()[0])} " + chest.GetContents()[0].GetName());
                    }
                    contents += chest.GetContents()[0].GetName();
                    AddItem(chest.GetContents()[0]);
                    chest.RemoveItem(chest.GetContents()[0]);
                }
            }
            else { Console.WriteLine("The chest is already empty"); }
        }
        private string Talk(NPC npc)
        {
            string shopui = "\n";
            if (npc is SellerNPC)
            {
                for (int i = 0; i < npc.GetVendorItems().Count; i++)
                {
                    shopui += $"{i + 1}. {npc.GetVendorItems()[i].GetName()} - {npc.GetVendorCosts()[i]}g\n";
                }
            }
            else if (npc is SmithNPC) 
            {
                shopui += $"You can upgrade your weapons here";
            }
            return shopui;

        }


        #endregion

        public bool DoCommand(String command)
        {
            if (command == "QUIT")
            {
                return true;
            }

            List<String> instructions = command.Split(' ').ToList();
            string instructionsToDo = "";
            for (int i = 1; i < instructions.Count; i++)
            {
                instructionsToDo += instructions[i];
                if (i != instructions.Count - 1)
                {
                    instructionsToDo += " ";
                }
            }

            switch (instructions[0])
            {
                case "look":
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\x1B[3m");
                    Console.WriteLine(Location.GetDescription());
                    Console.WriteLine("\x1B[0m");
                    break;
                case "health":
                case "h":
                    Console.WriteLine($"You have {Health} health.");
                    break;
                case "move":
                case "go":
                    if (instructions.Count <= 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Move where?");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                    else
                    {
                        Move(instructions[1]);
                    }
                    break;
                case "get":
                case "take":
                    Item WantedItem = null;
                    foreach (Item item in Location.GetContents())
                    {
                        if (item.GetName() == instructionsToDo)
                        {
                            WantedItem = item;
                        }
                    }
                    if (WantedItem != null)
                    {
                        Inventory.Add(Location.RemoveItem(instructionsToDo));
                        Console.WriteLine($"You picked up a {instructionsToDo}");
                        if (instructionsToDo == "key")
                        {
                            KeyObtained = true;
                        }

                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("This item does not exist here");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                    break;
                case "drop":
                    string itemToDrop = instructionsToDo;
                    Item droppedItem = null;

                    foreach (Item item in Inventory)
                    {
                        if (item.GetName() == itemToDrop)
                        {
                            droppedItem = item;
                        }
                    }
                    if (droppedItem != null && !droppedItem.Locked)
                    {
                        int number = 1;
                        if (Inventory.Count(n => n == droppedItem) > 1)
                        {

                            try
                            {
                                Console.WriteLine($"How many would you like to drop: 1 -> {Inventory.Count(n => n == droppedItem)}");
                                number = Convert.ToInt32(Console.ReadLine());
                                if (number > Inventory.Count(n => n == droppedItem))
                                {
                                    Console.WriteLine($"It must be a number between 1 and {Inventory.Count(n => n == droppedItem)}");
                                    number = 1;
                                }
                            }
                            catch (FormatException) { Console.WriteLine("Please enter a valid number"); }
                        }
                        RemoveItem(droppedItem, number);
                        Location.AddItem(droppedItem);
                        Console.WriteLine("You dropped " + instructionsToDo);
                        if (instructionsToDo == "key")
                        {
                            KeyObtained = false;
                        }
                    }
                    else if (droppedItem == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You don't have a " + itemToDrop);
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                    else if (droppedItem.Locked)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You cannot drop this");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }

                    break;
                case "attack":
                    Creature deadCreature = null;
                    if (Location.GetStaticCreatures().Count > 0)
                    {
                        foreach (StaticCreature stat in Location.GetStaticCreatures())
                        {
                            if (stat.GetName() == instructionsToDo)
                            {
                                int damage = EquipedWeapon.GetDamage() + Strength;
                                Console.WriteLine("Attacked");
                                bool dead = stat.TakeDamage(damage);
                                if (dead)
                                {
                                    Console.WriteLine($"Your attack destroyed the {stat.GetName()}");
                                    Location.AddConnection(stat.GetHiddens());
                                    Location.RemoveStatic(stat);
                                    break;
                                }
                            }
                        }
                    }
                    foreach (Creature creature in Location.GetCreatures())
                    {
                        if (creature.GetName() == instructionsToDo)
                        {
                            int damage = EquipedWeapon.GetDamage() + Strength;
                            bool dead = creature.TakeDamage(damage);
                            if (dead)
                            {
                                Console.WriteLine($"Your attack killed the {creature.GetName()}");
                                AdjustExperience(creature.GetXp());
                                foreach (Item drops in creature.GetDrops())
                                {
                                    Console.WriteLine($"The {creature.GetName()} dropped a {drops.GetName()}");
                                    Location.AddItem(drops);
                                }
                                gold += creature.GetGold();
                                Console.WriteLine($"You got {creature.GetGold()} gold");
                                deadCreature = creature;
                            }
                            else
                            {
                                Console.WriteLine($"Your attack caused the {creature.GetName()} to lose {damage} health.");
                                Console.WriteLine($"The {creature.GetName()} has {creature.GetHealth()} health left");
                                int damageTaken = creature.GetAttackDamage(Armour);
                                Console.WriteLine($"{creature.GetName()} attacks you and causes {damageTaken} damage.");
                                if (DoesDodge())
                                {
                                    Console.WriteLine($"You dodged the attack");
                                }
                                else
                                {
                                    Health -= damageTaken;
                                }
                                if (Health < 0)
                                {
                                    Console.WriteLine("You die.");
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    if (deadCreature != null)
                    {
                        Location.RemoveCreature(deadCreature);
                    }
                    break;
                case "cast":
                    if (instructions.Count < 3)
                    {
                        Console.WriteLine("Cast which spell at which target?");
                    }
                    else
                    {
                        string spell = instructions[1];
                        string target = instructions[2];
                        int drain = 0;
                        int drainindex = SpellNames.IndexOf(spell);
                        if (drainindex >= 0)
                        {
                            drain = ManaDrains[drainindex];
                        }

                        Creature creatureToRemove = null;
                        bool dead = false;



                        if (SpellBook.ContainsKey(spell))
                        {
                            foreach (Creature creature in Location.GetCreatures())
                            {
                                if (creature.GetName() == target)
                                {
                                    if (drain <= Mana)
                                    {
                                        dead = creature.TakeSpellDamage(spell, SpellBook[spell]);
                                        Mana -= drain;
                                    }

                                    if (dead)
                                    {
                                        Console.WriteLine($"Your {spell} killed the {creature.GetName()}");
                                        creatureToRemove = creature;
                                        AdjustExperience(creature.GetXp());
                                        foreach (Item drops in creature.GetDrops())
                                        {
                                            Console.WriteLine($"The {creature.GetName()} dropped a {drops.GetName()}");
                                            Location.AddItem(drops);
                                        }
                                        gold += creature.GetGold();
                                        Console.WriteLine($"The {creature.GetName()} dropped {creature.GetGold()} gold");
                                    }
                                    else
                                    {
                                        Health -= creature.GetAttackDamage(Armour);
                                        Console.WriteLine($"You dealt {SpellBook[spell]} to the {creature.GetName()}");
                                        Console.WriteLine($"The creature has {creature.GetHealth()} health left");
                                        if (Health < 0)
                                        {
                                            Console.WriteLine("You die.");
                                            return true;
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                }
                            }
                            if (creatureToRemove != null)
                            {
                                Location.RemoveCreature(creatureToRemove);
                            }
                        }
                        else
                        {
                            Console.WriteLine("You don't know that spell!");
                        }
                    }
                    break;
                case "examine":
                    if (instructions.Count <= 1)
                    {
                        Console.WriteLine("Examine what?");
                    }
                    else
                    {
                        for (int i = 0; i < Inventory.Count; i++)
                        {
                            if (instructionsToDo == Inventory[i].GetName())
                            {
                                Console.WriteLine(Inventory[i].GetDescription());
                            }
                        }

                    }

                    break;
                case "eat":
                    if (instructions.Count <= 1)
                    {
                        Console.WriteLine("Eat what?");
                    }
                    else
                    {
                        Eat(instructionsToDo);
                    }

                    break;
                case "inventory":
                case "i":
                    String items = "\n";

                    if (Inventory.Count > 0)
                    {
                        if (Inventory.Count == 1)
                        {
                            items += $"You have the following item: {Inventory[0].GetName()}.";
                        }
                        else
                        {
                            items += GetInventory();
                        }
                    }
                    else
                    {
                        items += "You aren't carrying anything.";
                    }
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(items);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case "equip":
                    Item EquippingItem = null;
                    foreach (Item item in Inventory)
                    {
                        if (item.GetName() == instructionsToDo)
                        {
                            EquippingItem = item;
                        }
                    }
                    if (EquippingItem != null)
                    {
                        EquipItem(EquippingItem);
                    }
                    break;
                case "wear":
                    Item WearingItem = null;
                    foreach (Item item in Inventory)
                    {
                        if (item is ArmourItem)
                        {
                            if (item.GetName() == instructionsToDo)
                            {
                                WearingItem = item;
                            }
                        }
                    }
                    if (WearingItem != null && WearingItem.IsArmour)
                    {
                        WearArmour(WearingItem);
                        Console.WriteLine("You have put on a " + instructionsToDo);
                    }
                    break;
                case "learn":
                    Item Learning = null;
                    for (int i = 0; i < Inventory.Count; i++)
                    {
                        if (Inventory[i].GetName() == instructionsToDo)
                        {
                            Learning = Inventory[i];
                        }
                    }

                    if (Learning != null)
                    {
                        bool learned = LearnSpell(Learning);
                        RemoveItem(Learning, 1);
                        if (learned)
                        {
                            Console.WriteLine("You learned " + instructionsToDo);
                        }
                    }
                    else
                    {
                        Console.WriteLine("You don't have a " + instructionsToDo);
                    }
                    break;
                case "xp":
                case "experience":
                    Console.WriteLine($"You Have: {GetExperience()} / {GetXPNeeded()}");
                    break;
                case "stats":
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Level " + level);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"Max Health: {MaxHealth}\nStrength: {Strength}\nAgility: {Agility}");
                    break;
                case "talk":
                    if (Location.HasNPC())
                    {
                        if (Location.GetNPC().Count == 1)
                        {
                            Console.WriteLine(Talk(Location.GetNPC()[0]));
                        }
                        else
                        {
                            if (instructions[1] == null)
                            {
                                Console.WriteLine("Which person?");
                            }
                            else
                            {
                                var found = Location.GetNPC().Find(item => item.GetName() == instructionsToDo);
                                if (found != null)
                                {
                                    // Downcasting, but better than holding base class pointer.
                                    // We don't want to always through away strong typing.
                                    talking = (NPC)found;
                                    Console.WriteLine(Talk(talking));
                                }
                                else
                                {
                                    Console.WriteLine($"Sorry could not find anyone with that name.");
                                }
                            }
                        }
                    }
                    else { Console.WriteLine("This room does not have anyone to talk to"); }
                    break;
                case "buy":
                    if (Location.HasNPC())
                    {
                        if (instructions.Count > 1)
                        {
                            if (talking is SellerNPC)
                            {
                                talking.BuyItem(this, instructionsToDo);
                            }
                        }
                        else { Console.WriteLine("Buy what?"); };
                    }
                    break;
                case "sell":
                    Random random = new Random();
                    string itemToSell = instructionsToDo;
                    int num = 1;
                    var SoldItem = Inventory.Find(item => item.GetName() == instructionsToDo);
                    if (SoldItem != null)
                    {
                        if (Inventory.Count(n => n == SoldItem) > 1)
                        {
                            try
                            {
                                while (true)
                                {
                                    Console.WriteLine($"How many would you like to sell: 1 -> {Inventory.Count(n => n == SoldItem)}");
                                    num = Convert.ToInt32(Console.ReadLine());

                                    if (num > Inventory.Count(n => n == SoldItem))
                                    {
                                        Console.WriteLine($"Must be a number between 1 and {Inventory.Count(n => n == SoldItem)}");

                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            catch (FormatException) { Console.WriteLine("Must be a valid number"); }

                            RemoveItem(SoldItem, num);
                            gold += SoldItem.GetValue() * num;
                            if (talking is SellerNPC)
                            {
                                talking.AddVendorItems(SoldItem, SoldItem.GetValue() + random.Next(1, 35));
                                Console.WriteLine($"You sold {num} {itemToSell} to {talking.GetName()} for {SoldItem.GetValue() * num} gold");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Sorry could not find {instructionsToDo}.");
                    }

                    break;
                case "open":
                    if (Location.GetChests().Count > 0)
                    {
                        Open(Location.GetChests()[0]);
                        Location.RemoveChest(Location.GetChests()[0]);
                    }
                    else { Console.WriteLine("There is nothing to open"); }
                    break;
                case "g":
                case "gold":
                    Console.WriteLine($"You have {gold} gold");
                    break;
                case "clear":
                    Console.Clear();
                    break;
                case "upgrade":
                    if (Location.HasNPC())
                    {
                            
                        var found = Inventory.Find(item => item.GetName() == instructionsToDo);
                        if (found != null)
                        {
                            if (talking is SmithNPC)
                            {
                                try
                                {
                                    talking.UpgradeWeapon(this, (WeaponItem)found);
                                }
                                catch (InvalidCastException)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("This is not a weapon");
                                    Console.ForegroundColor = ConsoleColor.DarkGray;
                                }
                            } 
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("You cannot upgrade weapons here");
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                            }

                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Sorry could not find {found.GetName()}.");
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                        }
                            
                    }
                    else { Console.WriteLine("You cannot upgrade weapons here"); }
                    break;
                case "mana":
                case "m":
                    Console.WriteLine("You have " + Mana + " mana left");
                    break;
                default:
                    Console.WriteLine("You can't do that!");
                    break;
            }
            return false; // they didn't quit
        }
    }
}

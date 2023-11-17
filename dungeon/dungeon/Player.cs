using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dungeon
{
    class Player
    {
        // attributes/properties
        private int Health;
        private int MaxHealth;
        private int Strength = 0;
        private Room Location;
        private List<Item> Inventory = new List<Item>();
        private Dictionary<string, int> SpellBook = new Dictionary<string, int> { { "frostbolt", 50 }, { "lightning", 80 } };
        private Random random;
        public Item EquipedWeapon;
        public bool KeyObtained = false;
        private Item key;
        private int level = 1;
        private double Experience = 0;
        private double xpNeeded = 150;
        private const double XP_INCREASE = 1.75;
        private int strengthChange = 5;
        private int HealthChange = 5;
        public int gold = 50;
        private NPC Vendor;
        public ArmourItem Armour;

        public Player(int health, Item equipedWeapon, ArmourItem armour, Item Key)
        {
            Health = health;
            MaxHealth = health;
            random = new Random();
            EquipedWeapon = equipedWeapon;
            key = Key;
            Armour = armour;
        }

        public void SetVendor(NPC vendor)
        {
            Vendor = vendor;
        }

        public void AddGold(int Gold)
        {
            gold += Gold;
        }

        public void AddItem(Item item)
        {
            Inventory.Add(item);
        }

        public void AddSpell(Item spell)
        {
            SpellBook.Add(spell.GetName(), spell.GetDamage());
        }

        public void RemoveItem(Item item)
        {
            Inventory.Remove(item);
        }

        public void EquipItem(Item weapon)
        {
            EquipedWeapon = weapon;
        }

        public void WearArmour(ArmourItem armour)
        {

            Armour = armour;

        }

        public int GetStrength()
        {
            return Strength;
        }

        public bool ObtainedKey()
        {
            return KeyObtained;
        }

        public void AdjustExperience(double experience)
        {
            Experience += experience;
            while (Experience >= xpNeeded)
            {
                level++;
                xpNeeded *= XP_INCREASE;
                Console.WriteLine("You Leveled Up");
                Strength += strengthChange;
                strengthChange = Convert.ToInt32(strengthChange * 1.5);
                Health += HealthChange;
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

        public int GetHealth()
        {
            return Health;
        }

        public void SetHealth(int health)
        {
            Health = health;
        }

        public Room GetLocation()
        {
            return Location;
        }

        public void SetLocation(Room location)
        {
            Location = location;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(Location.GetDescription());
            Console.ForegroundColor = ConsoleColor.White;

        }

        // other methods
        public int AdjustHealth(int health)
        {
            Health += health;
            return Health;
        }


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
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(Location.GetDescription());
                    Console.ForegroundColor = ConsoleColor.White;
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
                        Console.ForegroundColor = ConsoleColor.White;
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
                        Console.ForegroundColor = ConsoleColor.White;
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
                        Inventory.Remove(droppedItem);
                        Location.AddItem(droppedItem);
                        Console.WriteLine("You dropped " + instructionsToDo);
                        if (instructionsToDo == "key")
                        {
                            KeyObtained = false;
                        }
                    }
                    else if (droppedItem.Locked)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You cannot drop this");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You don't have a " + itemToDrop);
                        Console.ForegroundColor = ConsoleColor.White;
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
                                if (creature.MultiDrop())
                                {
                                    Console.WriteLine($"The {creature.GetName()} dropped a {creature.GetDrop1().GetName()}");
                                    Location.AddItem(creature.GetDrop1());
                                    Console.WriteLine($"The {creature.GetName()} also dropped a {creature.GetDrop2().GetName()}");
                                    Location.AddItem(creature.GetDrop2());
                                }
                                else if (creature.IsDrop())
                                {
                                    Console.WriteLine($"The {creature.GetName()} dropped a {creature.GetDrop1().GetName()}");
                                    Location.AddItem(creature.GetDrop1());
                                }



                                deadCreature = creature;
                            }
                            else
                            {
                                Console.WriteLine($"Your attack caused the {creature.GetName()} to lose {damage} health.");
                                int damageTaken = creature.GetAttackDamage(Armour);
                                Console.WriteLine($"{creature.GetName()} attacks you and causes {damageTaken} damage.");
                                Health -= damageTaken;
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
                        Creature creatureToRemove = null;

                        if (SpellBook.ContainsKey(spell))
                        {
                            foreach (Creature creature in Location.GetCreatures())
                            {
                                if (creature.GetName() == target)
                                {
                                    bool dead = creature.TakeSpellDamage(spell, SpellBook[spell]);
                                    if (dead)
                                    {
                                        Console.WriteLine($"Your {spell} killed the {creature.GetName()}");
                                        creatureToRemove = creature;
                                        AdjustExperience(creature.GetXp());

                                        if (creature.MultiDrop())
                                        {
                                            Console.WriteLine($"The {creature.GetName()} dropped a {creature.GetDrop1().GetName()}");
                                            Location.AddItem(creature.GetDrop1());
                                            Console.WriteLine($"The {creature.GetName()} also dropped a {creature.GetDrop2().GetName()}");
                                            Location.AddItem(creature.GetDrop2());
                                        }
                                        else if (creature.IsDrop())
                                        {
                                            Console.WriteLine($"The {creature.GetName()} dropped a {creature.GetDrop1().GetName()}");
                                            Location.AddItem(creature.GetDrop1());
                                        }
                                    }
                                    else
                                    {
                                        Health -= creature.GetAttackDamage(Armour);
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
                            items += $"You have the following items: {Inventory[0].GetName()}";
                            for (int i = 1; i < Inventory.Count - 1; i++)
                            {
                                items += ", " + Inventory[i].GetName();
                            }
                            items += $" and {Inventory[Inventory.Count - 1].GetName()}.";
                        }
                    }
                    else
                    {
                        items += "You aren't carrying anything.";
                    }
                    Console.WriteLine(items);
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
                        Console.WriteLine("You have equiped a " + instructionsToDo);
                    }
                    else
                    {
                        Console.WriteLine("You don't have a " + instructionsToDo);
                    }
                    break;
                case "wear":
                    ArmourItem WearingItem = null;
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
                    else if (!WearingItem.IsArmour) { Console.WriteLine("This is not armour"); }
                    else
                    {
                        Console.WriteLine("You don't have a " + instructionsToDo);
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

                    for (int i = 0; i < Inventory.Count; i++)
                    {
                        if (Inventory[i].GetName() == instructionsToDo)
                        {
                            Learning = Inventory[i];
                        }
                    }


                    if (Learning != null)
                    {
                        if (Learning.SpellBook)
                        {
                            AddSpell(Learning);
                            RemoveItem(Learning);
                            Console.WriteLine("You have learned a " + instructionsToDo);
                        }
                        else
                        {
                            Console.WriteLine("This is not a spell book");
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
                    Console.WriteLine($"Max Health: {MaxHealth}\nStrength: {Strength}");
                    break;
                case "talk":
                    if (Location.ContainsVendor())
                    {
                        Move("talk");
                    }
                    else { Console.WriteLine("This room does not have anyone to talk to"); }
                    break;
                case "buy":
                    if (Location.isVendor)
                    {
                        if (instructions.Count > 1)
                        {
                            Vendor.BuyItem(this, instructionsToDo);
                        }
                        else { Console.WriteLine("Buy what?"); };
                    }
                    break;
                case "leave":
                    if (Location.isVendor)
                    {
                        Move("leave");
                    }
                    else { Console.WriteLine("This room does not have anywhere to leave from"); }
                    break;
                case "sell":
                    Random random = new Random();
                    string itemToSell = instructionsToDo;
                    Item SoldItem = null;

                    foreach (Item item in Inventory)
                    {
                        if (item.GetName() == itemToSell)
                        {
                            SoldItem = item;
                        }
                    }
                    if (SoldItem != null && !SoldItem.Locked)
                    {
                        Inventory.Remove(SoldItem);
                        gold += SoldItem.GetValue();
                        Vendor.AddVendorItems(SoldItem, SoldItem.GetValue() + random.Next(1, 35));
                        Console.WriteLine($"You sold a {itemToSell} to {Vendor.GetName()} for {SoldItem.GetValue()} gold");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(Location.GetDescription());
                        Console.ForegroundColor = ConsoleColor.White;
                        if (instructionsToDo == "key")
                        {
                            KeyObtained = false;
                        }
                    }
                    else if (SoldItem.Locked) { Console.WriteLine("You cannot sell this"); }
                    else
                    {
                        Console.WriteLine("You don't have a " + itemToSell);
                    }
                    break;
                case "open":
                    if (Location.GetChests().Count > 0)
                    {
                        Open(Location.GetChests()[0]);

                    }
                    else { Console.WriteLine("There is nothing to open"); }
                    break;
                case "g":
                case "gold":
                    Console.WriteLine($"You have {gold} gold");
                    break;
                case "spells":
                    for (int i = 0; i < SpellBook.Count; i++)
                    {
                        //Console.WriteLine(SpellBook);
                    }
                    break;
                case "clear":
                    Console.Clear();
                    break;
                default:
                    Console.WriteLine("You can't do that!");
                    break;
            }
            return false; // they didn't quit
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
                            Inventory.RemoveAt(foodPosition);
                            break;
                        }
                        else
                        {
                            Health = MaxHealth;
                            Inventory.RemoveAt(foodPosition);
                            Console.WriteLine($"You ate {food}");
                            break;
                        }

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
                while (chest.GetContents().Count != 0)
                {
                    Console.WriteLine("You got a " + chest.GetContents()[0].GetName());
                    AddItem(chest.GetContents()[0]);
                    chest.RemoveItem(chest.GetContents()[0]);
                }
            }
            else { Console.WriteLine("The chest is already empty"); }
        }
    }
}

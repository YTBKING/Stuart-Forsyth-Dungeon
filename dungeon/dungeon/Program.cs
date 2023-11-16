using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Dynamic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;

namespace dungeon
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game dungeon = new Game();
            //instantiation - creating an object (instance of a class)
            //declaration - declaring the existence of a variable and it's type
            //initialisation - giving a variable it's initial or first value
            dungeon.PlayGame();
        }
    }

    class Game
    {
        private const int STARTING_HEALTH = 100;
        public void PlayGame()
        {
            String command = "";
            Boolean gameOver = false;





            // initialising the game
            Console.WriteLine("Welcome Message...");
            Room startRoom = new Room("You are in the starting cave.", false, false);
            Room lavaRoom = new Room("You are in a dark cave with a glowing river of lava.", true);
            Room chestRoom = new Room("You are in a cold room.", false);
            Room darkRoom = new Room("You are in a black room, with the only light source being a single torch in the middle.", false);
            Room wallRoom = new Room("You are in a room with solid brick walls surrounding it.\nOn the north side of the wall however, you can see a wall that looks as though it could be broken fairly easily", false);
            Room skipRoom = new Room("A simple room");

            //Room testRoom = new Room("A simple testing room", false);

            // food Items
            FoodItem apple = new FoodItem("apple", "a beautiful rosy red apple, it looks delicious.", 10, true, 1, 2);
            FoodItem bread = new FoodItem("bread", "A small slice of bread", 15, true, 0, 5);
            FoodItem staleBread = new FoodItem("stale bread", "Old stale bread. It doesnt look edible", -3, true, 5, 2);

            // potions
            FoodItem potionOfHealing = new FoodItem("health potion", "A elegant looking bottle with a purple liquid inside", 20, true, 6, 25);
            FoodItem potionOfPermaHealth = new FoodItem("perma potion", "A potion that will permanently raise health by 20 points", 20, true, 6, 500);

            // normal items
            Item stoneApple = new Item("apple", "a beautiful apple made of stone.", false, 4, 12);
            Item water = new FoodItem("water", "Everian, the best!.", 5, true, 0, 0);
            Item glass = new Item("glass", "glassy glass.", false, 4, 12);
            Item key = new Item("key", "A key that can be used to unlock doors", false, 2, 50);

            // weapons items
            Item fist = new Item("fists", "Your fists, hardened over time", false, 1, 0);
            Item woodenSword = new Item("wooden sword", "A splintered old sword", false, 5, 10);
            Item holySword = new Item("holy sword", "A sacred blade that was wielded by the paladins", false, 20, 20);
            // Item godSword = new Item("god", "Sword forged by the gods to test with", false, 10000000, 20000000);
            // Item axe = new Item("axe", "A sleek axe with an ash handle", false, 10, 20);
            // Item rustyAxe = new Item("rusty axe", "An old rusty axe with a splintered handle", false, 5, 10);

            // spell tomes
            Item engulf = new Item("engulf", "A spell book containing Engulf. This spell will engulf the target in a shadow of darkness, causing insanity on even the most mentally resilliant", false, 1, 2000, true, 25);
            
            //initialising shops
            NPC skipSeller = new NPC("Craig");
            NPCRoom skipShop = new NPCRoom(skipSeller);
            skipSeller.AddVendorItems(holySword, 45);
            skipSeller.AddVendorItems(potionOfPermaHealth, 650);
            skipRoom.AddNPC(skipSeller, skipShop, wallRoom, skipShop);

            // creatures
            DragonCreature dragon = new DragonCreature(350, potionOfPermaHealth, holySword);
            Creature orc = new Creature("orc", 20, 50);
            BossCreature kingGarlock = new BossCreature("garlock", 30, key, 100, 2, engulf);
            BossCreature testDummy = new BossCreature("test", 1, key, 150, 0);

            //fake wall connecting room
            Connection wallConnection = new Connection(wallRoom, skipRoom, "north", false);
            StaticCreature fakeWall = new StaticCreature("wall", 1, wallConnection);


            // start room add items
            startRoom.AddItem(water);
            startRoom.AddItem(glass);
            startRoom.AddItem(woodenSword);
            startRoom.AddCreature(testDummy);

            // other room add items
            lavaRoom.AddCreature(dragon);
            chestRoom.AddCreature(orc);
            darkRoom.AddCreature(kingGarlock);


            // start room connections
            startRoom.AddConnection(new Connection(startRoom, lavaRoom, "north", true));
            startRoom.AddConnection(new Connection(startRoom, chestRoom, "east", false));

            startRoom.AddStatic(fakeWall);

            // lava room connections
            lavaRoom.AddConnection(new Connection(lavaRoom, startRoom, "south", false));
            lavaRoom.AddConnection(new Connection(lavaRoom, chestRoom, "east", false));

            // chest room connections
            chestRoom.AddConnection(new Connection(chestRoom, startRoom, "west", false));
            chestRoom.AddConnection(new Connection(chestRoom, darkRoom, "south", false));
            chestRoom.AddConnection(new Connection(chestRoom, wallRoom, "north"));

            skipRoom.AddConnection(new Connection(skipRoom, wallRoom, "south"));

            // dark room connections
            darkRoom.AddConnection(new Connection(darkRoom, chestRoom, "north", false));

            wallRoom.AddConnection(new Connection(wallRoom, chestRoom, "south"));
            // hidden rooms
            wallRoom.AddStatic(fakeWall);


            // player setup
            Player pc = new Player(STARTING_HEALTH, fist, key);

            pc.SetLocation(startRoom);
            pc.AddItem(apple);
            pc.AddItem(stoneApple);
            pc.AddItem(fist);
            pc.AddItem(staleBread);


            // play the game
            while (!gameOver)
            {
                Console.WriteLine();
                Console.Write("What would you like to do? ");
                command = Console.ReadLine();
                gameOver = pc.DoCommand(command);
            }

            // finish off nicely and close down
            Console.WriteLine("Thank you for playing Dungeon! See you again soon, brave dungeoneer.");

            Console.ReadLine();
        }
    }

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
        public int gold = 50;
        private NPC Vendor;

        public Player(int health, Item equipedWeapon, Item Key)
        {
            Health = health;
            MaxHealth = health;
            random = new Random();
            EquipedWeapon = equipedWeapon;
            key = Key;
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
            }
        }

        public double GetExperience() { return Experience; }
        public double GetXPNeeded() { return xpNeeded; }

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
            Console.WriteLine(Location.GetDescription());

        }
        // other methods
        public int AdjustHealth(int health)
        {
            Health += health;
            return Health;
        }
        public Boolean DoCommand(String command)
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
                    Console.WriteLine(Location.GetDescription());
                    break;
                case "health":
                case "h":
                    Console.WriteLine($"You have {Health} health.");
                    break;
                case "move":
                case "go":
                    if (instructions.Count <= 1)
                    {
                        Console.WriteLine("Move where?");
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
                    else { Console.WriteLine("This room does not contain this item"); }


                    break;
                case "drop":
                    string itemToDrop = instructionsToDo;
                    Item droppedItem = null;

                    if (itemToDrop != "fist")
                    {


                        foreach (Item item in Inventory)
                        {
                            if (item.GetName() == itemToDrop)
                            {
                                droppedItem = item;
                            }
                        }
                        if (droppedItem != null)
                        {
                            Inventory.Remove(droppedItem);
                            Location.AddItem(droppedItem);
                            if (instructionsToDo == "key")
                            {
                                KeyObtained = false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("You don't have a " + itemToDrop);
                        }
                    } else { Console.WriteLine("You can't drop this"); }
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
                                int damageTaken = creature.GetAttackDamage();
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
                                        Health -= creature.GetAttackDamage();
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

                    if (itemToSell != "fist")
                    {


                        foreach (Item item in Inventory)
                        {
                            if (item.GetName() == itemToSell)
                            {
                                SoldItem = item;
                            }
                        }
                        if (SoldItem != null)
                        {
                            Inventory.Remove(SoldItem);
                            gold += SoldItem.GetValue();
                            Vendor.AddVendorItems(SoldItem, SoldItem.GetValue() + random.Next(1, 35));
                            Console.WriteLine($"You sold a {itemToSell} to {Vendor.GetName()} for {SoldItem.GetValue()} gold");
                            Console.WriteLine(Location.GetDescription());
                            if (instructionsToDo == "key")
                            {
                                KeyObtained = false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("You don't have a " + itemToSell);
                        }
                    }
                    else { Console.WriteLine("You can't sell this"); }
                    break;
                case "gold":
                    Console.WriteLine($"You have {gold} gold");
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
    }

    class Creature
    {
        private string Name;
        private int Health;
        protected Random random;
        protected Item Drop;
        protected Item Drop2;
        public double XP;

        public Creature(string name, int health, double xp, Item drop = null, Item drop2 = null)
        {
            Name = name;
            Health = health;
            random = new Random();
            Drop = drop;
            Drop2 = drop2;
            XP = xp;
        }

        public double GetXp() { return XP; }
        public Item GetDrop1()
        {
            return Drop;
        }
        public Item GetDrop2() { return Drop2; }

        public bool IsDrop() { return Drop != null; }

        public bool MultiDrop()
        {
            return Drop2 != null;
        }



        public virtual int GetAttackDamage()
        {
            return (random.Next(1, 10));
        }
        public int GetHealth()
        {
            return Health;
        }
        public string GetName()
        {
            return Name;
        }
        public void SetHealth(int amount)
        {
            Health = amount;
        }
        public bool TakeDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public virtual bool TakeSpellDamage(string spell, int damage)
        {
            Console.WriteLine($"{Name} takes {damage} from {spell}");

            return TakeDamage(damage);
        }
    }

    class BossCreature : Creature
    {
        int PowerLvl;
        public BossCreature(string name, int health, Item drop, double xp, int powerLvl, Item drop2 = null) : base(name, health, xp, drop, drop2) { PowerLvl = powerLvl; }

        public override int GetAttackDamage()
        {
            return ((random.Next(1, 10)) * PowerLvl);
        }

    }

    class DragonCreature : Creature
    {
        public DragonCreature(double xp, Item drop = null, Item drop2 = null) : base("dragon", 100, xp, drop, drop2) { }


        public override int GetAttackDamage()
        {
            if (random.Next(1, 3) == 1)
            {
                Console.WriteLine("The dragon engulfs you with his fiery breath");
                return 9999999;
            }
            else
            {
                return base.GetAttackDamage();
            }
        }
        public override bool TakeSpellDamage(string spell, int damage)
        {
            if (spell == "frostbolt")
            {
                return true;
            }
            else
            {
                return base.TakeSpellDamage(spell, damage);
            }
        }
    }
    class StaticCreature 
    {
        string Name;
        int Health;
        Connection HiddenConnection;
        public StaticCreature(string name, int health, Connection hiddenConnection)
        {
            Name = name;
            Health = health;
            HiddenConnection = hiddenConnection;
        }

        public string GetName()
        {
            return Name;
        }

        public bool TakeDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                return true;
            }
            return false;
        }

        public Connection GetHiddens()
        {
            return HiddenConnection;
        }
        

    }


    class Connection
    {
        private Room RoomFrom;
        private Room RoomTo;
        private String Direction;
        public bool KeyNeeded;
        private NPCRoom Vendor;


        public Connection(Room roomFrom, Room roomTo, String direction, bool keyNeeded = false, NPCRoom vendor = null)
        {
            RoomFrom = roomFrom;
            RoomTo = roomTo;
            Direction = direction;
            KeyNeeded = keyNeeded;
            Vendor = vendor;
        }
        public Boolean GoThrough(Player player, String direction, Item key)
        {
            
            
            if ((player.GetLocation() == RoomFrom) && (direction == Direction))
            {
                if ((KeyNeeded && player.KeyObtained) || !KeyNeeded)
                {
                    player.SetLocation(RoomTo);
                    if (Vendor != null)
                    {
                        Vendor.SetPlayerVendor(player);
                    }

                    if (KeyNeeded)
                    {
                        KeyNeeded = false;
                        player.KeyObtained = false;
                        player.RemoveItem(key);
                    }

                    return true;
                }
                else { Console.WriteLine("You need a key to acess this room") ; return false; }
            }
            else
            {
                return false;
            }
        }
        public String GetDirection()
        {
            return Direction;
        }
    }

    class Item
    {
        protected String Name;
        protected String Description;
        public bool IsEdible;
        protected int Damage;
        public bool SpellBook;
        public int SpellDamage;
        private int Value;

        public Item(String name, String description, bool isEdible, int damage, int value, bool spellBook = false, int spellDamage = 0)
        {
            Name = name;
            Description = description;
            IsEdible = isEdible;
            Damage = damage;
            SpellBook = spellBook;
            SpellDamage = spellDamage;
            Value = value;
        }
        public virtual int GetHeals()
        {
            return 0;
        }
        public String GetName()
        {
            return Name;
        }
        public int GetValue()
        {
            return Value;
        }
        public void SetName(String name)
        {
            Name = name;
        }
        public String GetDescription()
        {
            return Description;
        }
        public void SetDescription(String description)
        {
            Description = description;
        }

        public int GetDamage()
        {
            return Damage;
        }
        
        public int GetSpellDamage()
        {
            return SpellDamage;
        }

    }

    class FoodItem : Item
    {
        private int HealAmount;
        public FoodItem(String name, String description, int heals, bool isEdible, int minDamage, int value) : base(name, description, isEdible, minDamage, value)
        {
            HealAmount = heals;
        }
        public override int GetHeals()
        {
            return HealAmount;
        }
    }

    class Room
    {
        private String Description;
        private List<Item> Contents = new List<Item>();
        private List<Creature> Creatures = new List<Creature>();
        private List<Connection> Connections = new List<Connection>();
        private List<StaticCreature> StaticCreatures = new List<StaticCreature>();
        private List<NPC> NPCs = new List<NPC>();
        bool KeyNeeded;
        public bool isVendor;

        public Room(String description, bool keyNeeded = false, bool isVendorRoom = false)
        {
            Description = description;
            KeyNeeded = keyNeeded;
            isVendor = isVendorRoom;
        }
        public bool KeyNeccesary()
        {
            return KeyNeeded;
        }
        public void AddItem(Item item)
        {
            Contents.Add(item);
        }
        public void AddStatic(StaticCreature fake)
        {
            StaticCreatures.Add(fake);
        }
        public List<StaticCreature> GetStaticCreatures()
        {
            return StaticCreatures;
        }
        public void RemoveStatic(StaticCreature fake)
        {
            StaticCreatures.Remove(fake);
        }
        public void AddCreature(Creature creature)
        {
            Creatures.Add(creature);
        }
        public void AddNPC(NPC npc, Room vendorsRoom, Room npcStoringRoom, NPCRoom VendorRoom)
        {
            NPCs.Add(npc);
            AddConnection(new Connection(npcStoringRoom, vendorsRoom, "talk", false, VendorRoom));
            vendorsRoom.AddConnection(new Connection(vendorsRoom, npcStoringRoom, "leave", false));
        }



        public bool ContainsVendor()
        {
            return NPCs.Count > 0;
        }

        public List<NPC> GetNPC()
        {
            return NPCs;
        }

        public List<Creature> GetCreatures()
        {
            return Creatures;
        }
        public void RemoveCreature(Creature creature)
        {
            Creatures.Remove(creature);
        }
        public Item RemoveItem(String name)
        {
            foreach (Item item in Contents)
            {
                if (item.GetName() == name)
                {
                    Contents.Remove(item);
                    return item;
                }
            }

            return null;
        }
        public void AddConnection(Connection connection)
        {
            Connections.Add(connection);
        }
        public List<String> GetDirections()
        {
            // This returns the directions for each Connection in index order according to the Connections List
            List<String> directions = new List<String>();

            foreach (Connection connection in Connections)
            {
                directions.Add(connection.GetDirection());
            }

            return directions;
        }

        public List<Connection> GetConnections()
        {
            
            return Connections;
        }
        public virtual String GetDescription()
        {
            String items = "\n";
            if (Contents.Count > 0)
            {
                if (Contents.Count == 1)
                {
                    items += $"You can see the following item: {Contents[0].GetName()}.";
                }
                else
                {
                    items += $"You can see the following items: {Contents[0].GetName()}";
                    for (int i = 1; i < Contents.Count - 1; i++)
                    {
                        items += ", " + Contents[i].GetName();
                    }
                    items += $" and {Contents[Contents.Count - 1].GetName()}.";
                }
            }
            else
            {
                items = "";
            }
          

            String creatures = "\n";
            if (Creatures.Count > 0)
            {
                if (Creatures.Count == 1)
                {
                    creatures += $"You can see the following creature: {Creatures[0].GetName()}.";
                }
                else
                {
                    creatures += $"You can see the following creatures: {Creatures[0].GetName()}";
                    for (int i = 1; i < Creatures.Count - 1; i++)
                    {
                        creatures += ", " + Creatures[i].GetName();
                    }
                    creatures += $" and {Creatures[Creatures.Count - 1].GetName()}.";
                }
            }
            else
            {
                creatures = "";
            }

            String exits = "\n";

            int v = -1;
            
            for (int j = 0; j < Connections.Count; j++)
            {
                if (Connections[j].GetDirection() == "talk") { v = j; break; }
            }


            if (Connections.Count > 0)
            {
                if (Connections.Count == 1 )
                {
                    exits += $"There is an exit to the {Connections[0].GetDirection()}.";
                }
                else
                {
                    if (v != 0)
                    {
                        exits += $"There are exits to the {Connections[0].GetDirection()}";
                    }
                    else { 
                        exits += $"There are exits to the {Connections[1].GetDirection()}";
                        for (int i = 1; i < Connections.Count - 1; i++)
                        {
                            if (i != v)
                            {
                                if (i + 1 > Connections.Count)
                                {
                                    exits += $" and {Connections[i + 1].GetDirection()}.";
                                }
                                else
                                {
                                    exits += ", " + Connections[i].GetDirection();
                                }

                            }

                        }
                    }

                    for (int i = 1; i < Connections.Count - 1; i++)
                    {
                        if (i != v)
                        { 
                            if (i + 1 == v &  i + 2 >= Connections.Count)
                            {
                                exits += $" and {Connections[i].GetDirection()}.";
                            }
                            else
                            {
                                exits += ", " + Connections[i].GetDirection(); 
                            }
                             
                        }
                        
                    }
                    if (Connections.Count - 1 != v)
                    {
                        exits += $" and {Connections[Connections.Count - 1].GetDirection()}.";
                    }
                }
            }
            else
            {
                exits += "There are no visible exits.";
            }

            string vendors = "\n";
            if (NPCs.Count == 1)
            {
                vendors += "There is an npc you can talk to";
            }

            return Description + items + creatures + exits + vendors;
        }
        public List<Item> GetContents()
        {
            return Contents;
        }

        public void SetDescription(String description)
        {
            Description = description;
        }
        

    }

    class NPCRoom : Room
    {
        public NPC Vendor;
        string Description = "";
        public NPCRoom(NPC npc) : base("A simple shop", false, true)
        { 
            Vendor = npc;
        }
        public void SetPlayerVendor(Player player)
        {
            player.SetVendor(Vendor);
        }

        public override string GetDescription()
        {
            Description = $"{Vendor.GetName()} is selling:\n";
            for (int i = 0; i < Vendor.SellingItems.Count; i++)
            {
                Description += $"{i + 1}. {Vendor.SellingItems[i].GetName()} - {Vendor.SellingCosts[i]} gold\n";
            }

            return Description;
        }

    }

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
            SellingCosts.Add(cost );
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
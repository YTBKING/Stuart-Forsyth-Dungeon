using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace dungeon
{
    class Game
    {
        private const int STARTING_HEALTH = 100;
        public void PlayGame()
        {
            #region "Game Inital"
            string command = "";
            bool gameOver = false;
            #endregion

            #region "Room Creation"
            // initialising the game

            Room startRoom = new Room("You are in the starting cave.");
            Room lavaRoom = new Room("You are in a dark cave with a glowing river of lava.", true);
            Room chestRoom = new Room("You are in a cold room.");
            Room darkRoom = new Room("You are in a black room, with the only light source being a single torch in the middle.");
            Room wallRoom = new Room("You are in a room with solid brick walls surrounding it.\nOn the north side of the wall  you can see a cracked wall with a ray of light beaming through");
            Room skipRoom = new Room("You are in a simple room that has clearly not been entered in hundreds of years");
            Room restRoom = new Room("You are in a beautiful room, with a large fountain residing in the middle, chisseled out of the surroudning rock walls. You can also see carvings along the west wall in the shape of an archway.");

            //Room testRoom = new Room("A simple testing room", false);

            #endregion

            #region "Food Items"
            FoodItem apple = new FoodItem("apple", "a beautiful rosy red apple, it looks delicious.", 10, 20, 2);
            FoodItem bread = new FoodItem("bread", "A small slice of bread", 15, 3, 5);
            FoodItem staleBread = new FoodItem("stale bread", "Old stale bread. It doesnt look edible", -3, 1, 2);
            FoodItem cheese = new FoodItem("cheese", "Stinky old cheese that has clearly been here for years", 5, 10, 1);
            FoodItem water = new FoodItem("water", "Everian, the best!.", 5, 200, 0);
            #endregion

            #region "Potions"
            PotionItem potionOfHealing = new PotionItem("health potion", "A elegant looking bottle with a purple liquid inside", 20, 0, 25);
            FoodItem potionOfPermaHealth = new FoodItem("perma potion", "A potion that will permanently raise health by 20 points", 20, 0, 500);
            #endregion

            #region "Other Items"
            Item stoneApple = new Item("stone apple", "a beautiful apple made of stone.", 12);
            Item glass = new Item("glass", "glassy glass.", 12);

            #endregion

            #region "Key Items"
            KeyItem key = new KeyItem("key", "A key that can be used to unlock doors", 50);
            KeyItem gold = new KeyItem("gold", "Gold that can be used to buy and upgrade tools", 1, false);
            #endregion

            #region "Weapons"
            #region "Class Starting Weapons"
            // barbarian
            WeaponItem fist = new WeaponItem("fists", "Your fists. Saggy flesh and bone", 1, 0, "Common", 35, true);

            // kinght
            WeaponItem sword = new WeaponItem("sword", "A basic metal sword for a simple knight", 10, 50);
            #endregion
            WeaponItem hardenedFist = new WeaponItem("hardened fists", "Fists hardened by the passage of time", 50, 0, "Common", 65, true);
            WeaponItem blessedFist = new WeaponItem("blessed fists", "Fists that have been blessed with the souls of the dead", 200, 0, "Common", 150, true);
            WeaponItem godsFist = new WeaponItem("gods fists", "Fists that may as well have been ripped from a God.", 500, 0, "Common", 500, true);
            WeaponItem heavenlyFist = new WeaponItem("heavenly fist", "Fists from the heavens. Enchanted by the spirit of the Deity of Life", 750, 0, "Common", 1000, true);
            WeaponItem demonicFist = new WeaponItem("demonic fist", "Fist from the underworld. Enchanted by the spirit of the Deity of Death", 1000, 0, "Common", 10000, true);
            WeaponItem woodenSword = new WeaponItem("wooden sword", "A splintered old sword", 5, 10);
            WeaponItem holySword = new WeaponItem("holy sword", "A sacred blade that was wielded by the paladins", 20, 20, "Common", 90);
            WeaponItem demonicSword = new WeaponItem("demonic greatsword", "A greatsword made by a demonic king. The true form of the holy sword", 150, 500);
            WeaponItem steelSword = new WeaponItem("steel sword", "A sword made with strong steel", 50, 200);
            WeaponItem godSlayer = new WeaponItem("god slayer", "A sword made for killing the gods", 250, 700);
            // Item godSword = new Item("god", "Sword forged by the gods to test with", false, 10000000, 20000000);
            // Item axe = new Item("axe", "A sleek axe with an ash handle", false, 10, 20);
            // Item rustyAxe = new Item("rusty axe", "An old rusty axe with a splintered handle", false, 5, 10);

            #region "Weapon form upgrades"
            fist.AddBetterForm(hardenedFist);
            hardenedFist.AddBetterForm(blessedFist);
            blessedFist.AddBetterForm(godsFist);


            holySword.AddBetterForm(demonicSword);
            demonicSword.AddBetterForm(godSlayer);

            woodenSword.AddBetterForm(steelSword);

            #endregion

            #endregion

            #region "Staffs"
            StaffWeapon sacredStaff = new StaffWeapon("sacred staff", "A staff imbued with the dying breath of a star", 10, 100);
            #endregion

            #region "Armour"
            ArmourItem skin = new ArmourItem("skin", "You plain flesh and bones", 0, 0, true);
            ArmourItem helm = new ArmourItem("helm", "A simple metal helmet", 13, 20);
            #region "Class starter armour"
            // barbarian
            ArmourItem barbarianSet = new ArmourItem("barbarians set", "A flimsy leather waist strap with gloves and boots", 5, 20);

            // knight
            ArmourItem knightsSet = new ArmourItem("knights set", "A strong metal breastplate with boots and a helmet. Worn by simple knights", 10, 20);

            // mage
            ArmourItem magesRobes = new ArmourItem("mage robes", "A simple yet elegant gown. Said to improve mana control, however no evidence of this has been shown", 2, 50);
            #endregion
            #endregion

            #region "Spell Books"
            SpellItem frostBolt = new SpellItem("frostbolt", "A spell book containing FrostBolt. Shoot a bolt of ice at the target", 50, 15, 200);
            SpellItem engulf = new SpellItem("engulf", "A spell book containing Engulf. This spell will engulf the target in a shadow of darkness, causing insanity on even the most mentally resilliant", 50, 15, 2);
            SpellItem lightning = new SpellItem("lightning", "A spell book containg Lightning. Shoot lightning at your enemy", 80, 40, 250);

            #endregion

            #region "NPC Shop Creation"
            #region "StartRoom (test)
            SmithNPC testSmith = new SmithNPC("Juicy");

            SellerNPC testSeller = new SellerNPC("Saul");
            testSeller.AddVendorItems(woodenSword, 5);
            testSeller.AddVendorItems(gold, 2);
            testSeller.AddVendorItems(helm, 5, 3);

            #endregion
            #region "Skip Room"
            SellerNPC skipSeller = new SellerNPC("Craig");
            skipSeller.AddVendorItems(holySword, 45);
            skipSeller.AddVendorItems(potionOfPermaHealth, 650);
            skipRoom.AddNPC(skipSeller);
            #endregion
            #region "Rest Room"
            SellerNPC fleur = new SellerNPC("Fleur", "People used to say that there was a beautiful archway that led to a whole 'nother cave system. I wonder where it went");
            fleur.AddVendorItems(potionOfHealing, 30, 3);
            fleur.AddVendorItems(helm, 20, 1);
            fleur.AddGift(potionOfHealing);
            #endregion
            #endregion

            #region "Creatures
            #region "Bosses"
            DragonCreature dragon = new DragonCreature(350, 5, 2, 500);
            Garlock kingGarlock = new Garlock("garlock", 30, 5, 2, 100, 50);
            //BossCreature testDummy = new BossCreature("test", 1, 150, 0, 500);
            Celestial celestialKing = new Celestial("celestial", 300, 100, 30, 20000, 2000);
            #endregion
            #region "Regular"
            Creature orc = new Creature("orc", 20, 50, 15, 1, 10);
            #endregion
            #region "Static Creatures"
            Connection wallConnection = new Connection(wallRoom, skipRoom, "north", false);
            StaticCreature fakeWall = new StaticCreature("wall", 1, wallConnection);
            #endregion
            #endregion

            #region "Creature Drops"
            dragon.AddDrop(potionOfPermaHealth);
            dragon.AddDrop(holySword);

            kingGarlock.AddDrop(key);
            kingGarlock.AddDrop(engulf);

            #endregion

            #region "Chests"
            Chest startRoomChest = new Chest();
            startRoomChest.AddItem(cheese);
            startRoomChest.AddItem(bread);
            startRoomChest.AddItem(gold, 50);
            #endregion

            #region "Room Setup"
            #region "StartRoom"
            startRoom.AddItem(water);
            startRoom.AddItem(glass);
            startRoom.AddItem(woodenSword);
            startRoom.AddChest(startRoomChest);

            // connections
            startRoom.AddConnection(new Connection(startRoom, lavaRoom, "north", true));
            startRoom.AddConnection(new Connection(startRoom, chestRoom, "east", false));
            #endregion

            #region "LavaRoom"
            lavaRoom.AddCreature(dragon);

            // connections
            lavaRoom.AddConnection(new Connection(lavaRoom, startRoom, "south"));
            lavaRoom.AddConnection(new Connection(lavaRoom, skipRoom, "east"));
            lavaRoom.AddConnection(new Connection(lavaRoom, restRoom, "north"));
            #endregion

            #region "ChestRoom"
            chestRoom.AddCreature(orc);

            // connections
            chestRoom.AddConnection(new Connection(chestRoom, startRoom, "west", false));
            chestRoom.AddConnection(new Connection(chestRoom, darkRoom, "south", false));
            chestRoom.AddConnection(new Connection(chestRoom, wallRoom, "north"));
            #endregion

            #region "DarkRoom"
            darkRoom.AddCreature(kingGarlock);
            darkRoom.AddConnection(new Connection(darkRoom, chestRoom, "north", false));
            #endregion

            #region "SkipRoom"
            skipRoom.AddConnection(new Connection(skipRoom, wallRoom, "south"));
            skipRoom.AddConnection(new Connection(skipRoom, lavaRoom, "west"));
            #endregion

            #region "WallRoom"
            wallRoom.AddConnection(new Connection(wallRoom, chestRoom, "south"));
            
            // fake wall
            wallRoom.AddStatic(fakeWall);
            #endregion

            #region "RestRoom"
            restRoom.AddConnection(new Connection(restRoom, lavaRoom, "South"));

            restRoom.AddNPC(fleur);
            #endregion
            #endregion


            #region "Class Setup"
            AnsiConsole.Markup("[italic]Which Class would you like to play as:[/]\n[bold darkred_1]Barbarian[/]\n[bold grey69]Knight[/]\n[bold 153]Mage[/]\n");

            Player pc = new Player(100, fist, skin, key);
            bool unassigned = true;
            string classes = "";
            while (unassigned)
            {
                classes = Console.ReadLine().ToLower();
                switch (classes)
                {
                    case "barbarian":
                        pc = new Player(120, fist, barbarianSet, key);
                        pc.SetStrength(15);
                        pc.SetMana(25);
                        pc.SetAgility(8);
                        pc.AddItem(barbarianSet);

                        unassigned = false;
                        break;
                    case "knight":
                        pc = new Player(150, sword, knightsSet, key);
                        pc.SetStrength(5);
                        pc.SetMana(70);
                        pc.SetAgility(2);
                        pc.AddItem(knightsSet);
                        pc.AddItem(sword);
                        unassigned = false;
                        break;
                    case "mage":
                        pc = new Player(75, sacredStaff, magesRobes, key);
                        pc.SetStrength(0);
                        pc.SetMana(200);
                        pc.SetAgility(4);
                        pc.AddItem(sacredStaff);
                        pc.AddItem(magesRobes);
                        pc.LearnSpell(frostBolt);
                        pc.LearnSpell(lightning);
                        unassigned = false;
                        break;
                    default:
                        Console.WriteLine("Please input a valid class");
                        continue;

                }
            }
            pc.SetClass(classes);
            #endregion


            #region "Player Set Up"
            pc.SetLocation(startRoom);

            pc.AddItem(apple);
            pc.AddItem(stoneApple);
            pc.AddItem(fist);
            pc.AddItem(staleBread);
            pc.AddItem(skin);
            pc.AddItem(apple);
            pc.AddItem(stoneApple);
            pc.AddItem(stoneApple);
            #endregion

            #region "Game"
            while (!gameOver)
            {

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("What would you like to do? ");
                Console.ForegroundColor = ConsoleColor.White;
                command = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;

                Console.WriteLine("\x1B[3m");
                gameOver = pc.DoCommand(command);
                Console.WriteLine("\x1B[0m");
            }

            // finish off nicely and close down
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"
                                                                            
▀███▀   ▀██▀ ▄▄█▀▀██▄  ▀███▀   ▀███▀   ▀███▀▀▀██▄ ▀████▀███▀▀▀███▀███▀▀▀██▄  
  ███   ▄█ ▄██▀    ▀██▄ ██       █       ██    ▀██▄ ██   ██    ▀█  ██    ▀██▄
   ███ ▄█  ██▀      ▀██ ██       █       ██     ▀██ ██   ██   █    ██     ▀██
    ████   ██        ██ ██       █       ██      ██ ██   ██████    ██      ██
     ██    ██▄      ▄██ ██       █       ██     ▄██ ██   ██   █  ▄ ██     ▄██
     ██    ▀██▄    ▄██▀ ██▄     ▄█       ██    ▄██▀ ██   ██     ▄█ ██    ▄██▀
   ▄████▄    ▀▀████▀▀    ▀██████▀▀     ▄████████▀ ▄████▄██████████████████▀  
                                                                            
                                                                            

");

            Console.ReadLine();
            #endregion
        }
    }
}

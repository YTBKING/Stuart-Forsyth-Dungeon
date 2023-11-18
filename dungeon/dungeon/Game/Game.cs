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
            String command = "";
            bool gameOver = false;
            #endregion

            #region "Room Creation"
            // initialising the game

            Room startRoom = new Room("You are in the starting cave.");
            Room lavaRoom = new Room("You are in a dark cave with a glowing river of lava.", true);
            Room chestRoom = new Room("You are in a cold room.");
            Room darkRoom = new Room("You are in a black room, with the only light source being a single torch in the middle.");
            Room wallRoom = new Room("You are in a room with solid brick walls surrounding it.\nOn the north side of the wall however, you can see a wall that looks as though it could be broken fairly easily");
            Room skipRoom = new Room("A simple room");

            //Room testRoom = new Room("A simple testing room", false);

            #endregion

            #region "Food Items"
            FoodItem apple = new FoodItem("apple", "a beautiful rosy red apple, it looks delicious.", 10, 2);
            FoodItem bread = new FoodItem("bread", "A small slice of bread", 15, 5);
            FoodItem staleBread = new FoodItem("stale bread", "Old stale bread. It doesnt look edible", -3, 2);
            FoodItem cheese = new FoodItem("cheese", "Stinky old cheese that has clearly been here for years", 5, 1);
            #endregion

            #region "Potions"
            PotionItem potionOfHealing = new PotionItem("health potion", "A elegant looking bottle with a purple liquid inside", 20, 25);
            FoodItem potionOfPermaHealth = new FoodItem("perma potion", "A potion that will permanently raise health by 20 points", 20, 500);
            #endregion

            #region "Other Items"
            Item stoneApple = new Item("stone apple", "a beautiful apple made of stone.", 12);
            Item water = new FoodItem("water", "Everian, the best!.", 5, 0);
            Item glass = new Item("glass", "glassy glass.", 12);

            #endregion

            #region "Key Items"
            KeyItem key = new KeyItem("key", "A key that can be used to unlock doors", 50);
            KeyItem gold = new KeyItem("gold", "Gold that can be used to buy and upgrade tools", 1, false);
            #endregion

            #region "Weapons"
            WeaponItem fist = new WeaponItem("fists", "Your fists. Saggy flesh and bone", 1, 0, "Common", 35, true);
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

            #region "Armour"
            ArmourItem skin = new ArmourItem("skin", "You plain flesh and bones", 0, 0, true);
            ArmourItem helm = new ArmourItem("helm", "A simple metal helmet", 5, 20);
            #endregion

            #region "Spell Books"
            SpellItem frostBolt = new SpellItem("frostbolt", "A spell book containing FrostBolt. Shoot a bolt of ice at the target", 50, 15, 200);
            SpellItem engulf = new SpellItem("engulf", "A spell book containing Engulf. This spell will engulf the target in a shadow of darkness, causing insanity on even the most mentally resilliant", 50, 15, 2);
            SpellItem lightning = new SpellItem("lightning", "A spell book containg Lightning. Shoot lightning at your enemy", 80, 40, 250);

            #endregion

            #region "NPC Shop Creation"
            #region "StartRoom (test)
            SmithNPC testSmith = new SmithNPC("Juicy");
            startRoom.AddNPC(testSmith);
            SellerNPC testSeller = new SellerNPC("Saul");
            testSeller.AddVendorItems(woodenSword, 5);
            testSeller.AddVendorItems(gold, 2);
            startRoom.AddNPC(testSeller);
            #endregion
            #region "Skip Room"
            SellerNPC skipSeller = new SellerNPC("Craig");
            skipSeller.AddVendorItems(holySword, 45);
            skipSeller.AddVendorItems(potionOfPermaHealth, 650);
            skipRoom.AddNPC(skipSeller);
            #endregion
            #endregion

            #region "Creatures
            #region "Bosses"
            DragonCreature dragon = new DragonCreature(350, 5, 90);
            Garlock kingGarlock = new Garlock("garlock", 30, 2, 100, 50);
            BossCreature testDummy = new BossCreature("test", 1, 150, 0, 500);
            Celestial celestialKing = new Celestial("celestial", 300, 1000, 5, 2000);
            #endregion
            #region "Regular"
            Creature orc = new Creature("orc", 20, 50, 15);
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

            testDummy.AddDrop(key);
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
            #endregion

            #region "Player Set Up"
            Player pc = new Player(STARTING_HEALTH, fist, skin, key);
            pc.SetLocation(startRoom);

            pc.AddItem(apple);
            pc.AddItem(stoneApple);
            pc.AddItem(fist);
            pc.AddItem(staleBread);
            pc.AddItem(skin);
            pc.AddItem(apple);
            pc.AddItem(stoneApple);
            pc.AddItem(stoneApple);
            pc.LearnSpell(frostBolt);
            pc.LearnSpell(lightning);
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

                gameOver = pc.DoCommand(command);
            }

            // finish off nicely and close down
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"
                                                                            
▀███▀   ▀██▀ ▄▄█▀▀██▄ ▀███▀   ▀███▀   ▀███▀▀▀██▄ ▀████▀███▀▀▀███▀███▀▀▀██▄  
  ███   ▄█ ▄██▀    ▀██▄██       █       ██    ▀██▄ ██   ██    ▀█  ██    ▀██▄
   ███ ▄█  ██▀      ▀████       █       ██     ▀██ ██   ██   █    ██     ▀██
    ████   ██        ████       █       ██      ██ ██   ██████    ██      ██
     ██    ██▄      ▄████       █       ██     ▄██ ██   ██   █  ▄ ██     ▄██
     ██    ▀██▄    ▄██▀██▄     ▄█       ██    ▄██▀ ██   ██     ▄█ ██    ▄██▀
   ▄████▄    ▀▀████▀▀   ▀██████▀▀     ▄████████▀ ▄████▄██████████████████▀  
                                                                            
                                                                            

");

            Console.ReadLine();
            #endregion
        }
    }
}

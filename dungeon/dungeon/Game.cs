using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dungeon
{
    class Game
    {
        private const int STARTING_HEALTH = 100;
        public void PlayGame()
        {
            String command = "";
            bool gameOver = false;



            // initialising the game
            Console.WriteLine("Welcome Message...");
            Room startRoom = new Room("You are in the starting cave.");
            Room lavaRoom = new Room("You are in a dark cave with a glowing river of lava.", true);
            Room chestRoom = new Room("You are in a cold room.");
            Room darkRoom = new Room("You are in a black room, with the only light source being a single torch in the middle.");
            Room wallRoom = new Room("You are in a room with solid brick walls surrounding it.\nOn the north side of the wall however, you can see a wall that looks as though it could be broken fairly easily");
            Room skipRoom = new Room("A simple room");

            //Room testRoom = new Room("A simple testing room", false);

            // food Items
            FoodItem apple = new FoodItem("apple", "a beautiful rosy red apple, it looks delicious.", 10, 1, 2);
            FoodItem bread = new FoodItem("bread", "A small slice of bread", 15, 0, 5);
            FoodItem staleBread = new FoodItem("stale bread", "Old stale bread. It doesnt look edible", -3, 5, 2);
            FoodItem cheese = new FoodItem("cheese", "Stinky old cheese that has clearly been here for years", 5, 2, 1);

            // potions
            FoodItem potionOfHealing = new FoodItem("health potion", "A elegant looking bottle with a purple liquid inside", 20, 6, 25);
            FoodItem potionOfPermaHealth = new FoodItem("perma potion", "A potion that will permanently raise health by 20 points", 20, 6, 500);

            // normal items
            Item stoneApple = new Item("apple", "a beautiful apple made of stone.", 4, 12);
            Item water = new FoodItem("water", "Everian, the best!.", 5, 0, 0);
            Item glass = new Item("glass", "glassy glass.", 4, 12);
            Item key = new Item("key", "A key that can be used to unlock doors", 2, 50, true);

            // weapons items
            Item fist = new Item("fists", "Your fists, hardened over time", 1, 0, true);
            Item woodenSword = new Item("wooden sword", "A splintered old sword", 5, 10);
            Item holySword = new Item("holy sword", "A sacred blade that was wielded by the paladins", 20, 20);
            // Item godSword = new Item("god", "Sword forged by the gods to test with", false, 10000000, 20000000);
            // Item axe = new Item("axe", "A sleek axe with an ash handle", false, 10, 20);
            // Item rustyAxe = new Item("rusty axe", "An old rusty axe with a splintered handle", false, 5, 10);

            // armour items
            ArmourItem skin = new ArmourItem("skin", "You plain flesh and bones", 0, 0, true);
            ArmourItem helm = new ArmourItem("helm", "A simple metal helmet", 5, 20);


            // spell tomes
            Item engulf = new Item("engulf", "A spell book containing Engulf. This spell will engulf the target in a shadow of darkness, causing insanity on even the most mentally resilliant", 1, 2000, false, false, true, 25);

            //initialising shops
            NPC skipSeller = new NPC("Craig");
            NPCRoom skipShop = new NPCRoom(skipSeller);
            skipSeller.AddVendorItems(holySword, 45);
            skipSeller.AddVendorItems(potionOfPermaHealth, 650);
            skipRoom.AddNPC(skipSeller, skipShop, skipRoom, skipShop);

            // creatures
            DragonCreature dragon = new DragonCreature(350, 5, potionOfPermaHealth, holySword);
            Creature orc = new Creature("orc", 20, 50);
            BossCreature kingGarlock = new BossCreature("garlock", 30, key, 100, 2, engulf);
            BossCreature testDummy = new BossCreature("test", 1, key, 150, 0);

            //fake wall connecting room
            Connection wallConnection = new Connection(wallRoom, skipRoom, "north", false);
            StaticCreature fakeWall = new StaticCreature("wall", 1, wallConnection);


            Chest startRoomChest = new Chest();
            startRoomChest.AddItem(cheese);
            startRoomChest.AddItem(bread);



            // start room add items
            startRoom.AddItem(water);
            startRoom.AddItem(glass);
            startRoom.AddItem(woodenSword);
            startRoom.AddCreature(testDummy);
            startRoom.AddChest(startRoomChest);

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
            Player pc = new Player(STARTING_HEALTH, fist, skin, key);

            pc.SetLocation(startRoom);
            pc.AddItem(apple);
            pc.AddItem(stoneApple);
            pc.AddItem(fist);
            pc.AddItem(staleBread);
            pc.AddItem(skin);


            // play the game
            while (!gameOver)
            {

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("What would you like to do? ");
                Console.ForegroundColor = ConsoleColor.White;
                command = Console.ReadLine();

                gameOver = pc.DoCommand(command);
            }

            // finish off nicely and close down
            Console.WriteLine("Thank you for playing Dungeon! See you again soon, brave dungeoneer.");

            Console.ReadLine();
        }
    }
}

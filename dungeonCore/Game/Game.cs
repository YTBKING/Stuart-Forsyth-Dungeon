using dungeonCore.Game;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
    class Game
    {
        private const int STARTING_HEALTH = 100;
        public void PlayGame()
        {
            #region "Game Initalising"
            string command = "";
            bool gameOver = false;
            #endregion

            #region "Room Creation"
            // initialising the game

            
            Room startRoom = new Room("You are in the starting cave.");
            Room lavaRoom = new Room("You are in a dark cave with a glowing river of lava.", true);
            Room chestRoom = new Room("You are in a cold room.");
            Room darkRoom = new Room("You are in a black room, with the only light source being a single torch in the middle.");
            Room wallRoom = new Room("You are in a room with solid brick walls surrounding it.\nOn the north side of the wall you can see a cracked wall with a ray of light beaming through");
            Room skipRoom = new Room("You are in a simple room that has clearly not been entered in hundreds of years");
            Room restRoom = new Room("You are in a beautiful room, with a large fountain residing in the middle, chisseled out of the surroudning rock walls. You can also see carvings along the west wall in the shape of an archway.");
            Room archRoom = new Room("A clearly abandoned room with writing scrawled along the sides. There is a desk with a pen and quill");
            Room demonsCavern = new Room("You notice ice encasing the walls of the room. Frozen bodies lie dormant across the ground, however for some reason, you do not feel cold");


            //Room testRoom = new Room("A simple testing room", false);

            #endregion

            #region "Item Creation"

            #region "Food Items"
            FoodItem apple = new FoodItem("Apple", "a beautiful rosy red apple, it looks delicious.", 10, 20, 2);
            FoodItem bread = new FoodItem("Bread", "A small slice of bread", 15, 3, 5);
            FoodItem staleBread = new FoodItem("Stale Bread", "Old stale bread. It doesnt look edible", -3, 1, 2);
            FoodItem cheese = new FoodItem("Cheese", "Stinky old cheese that has clearly been here for years", 5, 10, 1);
            FoodItem water = new FoodItem("Water", "Everian, the best!.", 5, 200, 0);
            #endregion

            #region "Potions"
            PotionItem potionOfHealing = new PotionItem("Health Potion", "A elegant looking bottle with a purple liquid inside", 20, 0, 25);
            FoodItem potionOfPermaHealth = new FoodItem("Health++", "A potion that will permanently raise health by 20 points", 20, 0, 500, true);
            #endregion

            #region "Other Items"
            Item stoneApple = new Item("Stone Apple", "A beautiful apple made of stone.", 12);
            Item glass = new Item("Glass", "Glassy glass.", 12);

            #endregion

            #region "Key Items"
            KeyItem masterKey = new KeyItem("Keys", "A key that can be used to unlock doors", 50);
            KeyItem gold = new KeyItem("Gold", "Gold that can be used to buy and upgrade tools", 1, false);
            #endregion

            #region "Notes"
            NoteItem old_note = new NoteItem("Old note", "Day 86:\nI have been trapped here for 86 days now since the cave-in. What am i to do? I dont know if I will be rescued. Why has no one come for me yet. I swear I will have my revenge on that monster. Why has no one come for me?");
            #endregion

            #region "Weapons"
            #region "Class Starting Weapons"
            // hellspawn
            WeaponItem hellsDagger = new WeaponItem("Hells Dagger", "A dagger forged in the upper levels of hell. Not especially strong, but highly effective against holy creatures", 10, 30, "Demonic");

            // kinght
            WeaponItem sword = new WeaponItem("Sword", "A basic metal sword for a simple knight", 10, 50);

            // paladin
            WeaponItem hammer = new WeaponItem("Hammer", "A gracefull hammer wielded by paladins. Its very heavy, and is especially usefull against Demonic creatures", 10, 120, "Holy");

            // frostbitten
            WeaponItem frostbiteSword = new WeaponItem("Frostbite", "A jagged dagger with ice radiating off of it. It's cold to the touch", 13, 100, "Frozen");

            // spitfire
            WeaponItem blazingSword = new WeaponItem("Sword", "A blazing sword that is boiling to the touch. Liquid fire runs down the hilt, hitting your hand", 12, 120, "Blazing");
            #endregion
            WeaponItem fist = new WeaponItem("Fists", "Your fists. Saggy flesh and bone", 1, 0, "Common", 35, true);
            WeaponItem hardenedFist = new WeaponItem("Hardened Fists", "Fists hardened by the passage of time", 50, 0, "Common", 65, true);
            WeaponItem blessedFist = new WeaponItem("Blessed Fists", "Fists that have been blessed with the souls of the dead", 200, 0, "Common", 150, true);
            WeaponItem godsFist = new WeaponItem("Gods Fists", "Fists that may as well have been ripped from a God.", 500, 0, "Common", 500, true);
            WeaponItem heavenlyFist = new WeaponItem("Heavenly Fist", "Fists from the heavens. Enchanted by the spirit of the Deity of Life", 750, 0, "Common", 1000, true);
            WeaponItem demonicFist = new WeaponItem("Demonic Fist", "Fist from the underworld. Enchanted by the spirit of the Deity of Death", 1000, 0, "Common", 10000, true);
            
            WeaponItem woodenSword = new WeaponItem("Wooden Sword", "A splintered old sword", 5, 10);
            WeaponItem holySword = new WeaponItem("Sword", "A sacred blade that was wielded by the paladins", 20, 20, "Holy", 90);
            WeaponItem demonicSword = new WeaponItem("Demonic Greatsword", "A greatsword made by a demonic king. The true form of the holy sword", 150, 500);
            WeaponItem steelSword = new WeaponItem("Steel Sword", "A sword made with strong steel", 50, 200);
            WeaponItem godSlayer = new WeaponItem("God Slayer", "A sword made for killing the gods", 250, 700);
            
            WeaponItem demonicStraightSword = new WeaponItem("Straight Sword", "A sharpened blade filled with the anger of dead souls", 75, 200, "Demonic");

            WeaponItem godSword = new WeaponItem("Gods Greatsword", "Sword forged by the gods to test with", 10000000, 20000000, "Legendary");
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
            StaffWeapon sacredStaff = new StaffWeapon("Sacred Staff", "A staff imbued with the dying breath of a star", 10, 100);
            #endregion

            #region "Armour"
            ArmourItem skin = new ArmourItem("Skin", "You plain flesh and bones", 0, 0, true);
            ArmourItem helm = new ArmourItem("Helm", "A simple metal helmet", 13, 20);
            #region "Class starter armour"
            // hellspawn
            ArmourItem hellsSet = new ArmourItem("Hellspawn Set", "A flimsy leather waist strap with gloves and boots", 8, 20);

            // knight
            ArmourItem knightsSet = new ArmourItem("Knight's Set", "A strong metal breastplate with boots and a helmet. Worn by simple knights", 10, 20);

            // mage
            ArmourItem magesRobes = new ArmourItem("Mage's Robes", "A simple yet elegant gown. Said to improve mana control, however no evidence of this has been shown", 2, 50);

            // palladin
            ArmourItem paladingear = new ArmourItem("Paladin's Set", "A elegant headpiece with a gold accent running down. While beautiful, the defense is questionable", 7, 100);

            // frostbitten
            ArmourItem frostbiteSet = new ArmourItem("Frostbite Set", "A frozen chestpiece with icicles emerging from it. Doesn't look especially strong but looks expensive", 4, 200);


            // spitfire
            ArmourItem spitfireSet = new ArmourItem("Spitfire Set", "A flaming metal chestpiece. Doesn't look particularly strong but looks expensive", 5, 500);

            // god
            ArmourItem godsSet = new ArmourItem("Gods Set", "A chestpiece made for killing gods", 1000000, 1000000);
            #endregion
            #endregion

            #region "Spell Books"
            SpellItem frostBolt = new SpellItem("Frostbolt", "A spell book containing FrostBolt. Shoot a bolt of ice at the target", 50, 15, 200);
            SpellItem engulf = new SpellItem("Engulf", "A spell book containing Engulf. This spell will engulf the target in a shadow of darkness, causing insanity on even the most mentally resilliant", 50, 15, 2);
            SpellItem lightning = new SpellItem("Lightning", "A spell book containing Lightning. Shoot lightning at your enemy", 80, 40, 250);
            SpellItem flameSpear = new SpellItem("FlameSpear", "A spell book containing FlameSpear. Throw a spear of fire towards tour enemy", 30, 10, 50);

            #endregion

            #endregion

            #region "NPC Shop Creation"
            #region "StartRoom (test)
            SmithNPC testSmith = new SmithNPC("Juicy");

            SellerNPC testSeller = new SellerNPC("Saul");
            testSeller.AddVendorItems(woodenSword, 5);
            testSeller.AddVendorItems(gold, 2);
            testSeller.AddVendorItems(helm, 5, 3);
            //startRoom.AddNPC(testSmith);
            startRoom.AddNPC(testSeller);
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
            Dragon dragon = new Dragon(350, 10, 5, 500);
            BossCreature kingGarlock = new BossCreature("garlock", 30, 5, 100, 2, 50, "Holy");
            BossCreature testDummy = new BossCreature("test", 1, 150, 0, 0, 500);
            Celestial celestialKing = new Celestial("celestial", 300, 100, 30, 20000, 2000);
            BossCreature vengefullSpirit = new BossCreature("vengefull spirit", 35, 80, 3, 3, 62, "Demonic");

            #endregion
            #region "Regular"
            Creature orc = new Creature("orc", 20, 50, 15, 1, 10);
            orc.isAgressive();
            #endregion
            #region "Static Creatures"
            Connection wallRoomConnection = new Connection(wallRoom, skipRoom, "north");
            StaticCreature fakeWallWallRoom = new StaticCreature("north wall", 1, wallRoomConnection);

            Connection restRoomHiddenWall = new Connection(restRoom, archRoom, "west");
            StaticCreature restHiddenWall = new StaticCreature("west wall", 10, restRoomHiddenWall);
            #endregion
            #endregion

            #region "Creature Drops"
            dragon.AddDrop(potionOfPermaHealth);
            dragon.AddDrop(holySword);

            kingGarlock.AddDrop(masterKey);
            kingGarlock.AddDrop(engulf);

            vengefullSpirit.AddDrop(demonicStraightSword);

            #endregion

            #region "Chests"
            Chest startRoomChest = new Chest();
            startRoomChest.AddItem(cheese);
            startRoomChest.AddItem(bread);
            startRoomChest.AddItem(gold, 50);

            /*Chest testRoomChest = new Chest(true);
            testRoomChest.AddItem(cheese);
            testRoomChest.AddItem(bread);*/

            Chest demonsChest = new Chest();
            demonsChest.AddItem(frostbiteSword);
            demonsChest.AddItem(gold, 250);
            #endregion

            #region "Room Setup"
            #region "StartRoom"
            startRoom.AddItem(water);
            startRoom.AddItem(glass);

            startRoom.AddChest(startRoomChest);

            //startRoom.AddChest(testRoomChest);

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
            wallRoom.AddStatic(fakeWallWallRoom);
            #endregion

            #region "RestRoom"
            restRoom.AddConnection(new Connection(restRoom, lavaRoom, "South"));

            restRoom.AddNPC(fleur);
            restRoom.AddStatic(restHiddenWall);

            #endregion
            
            #region "ArchRoom"
            archRoom.AddItem(old_note);
            archRoom.AddCreature(vengefullSpirit);
            archRoom.AddConnection(new Connection(archRoom, restRoom, "east"));
            archRoom.AddConnection(new Connection(archRoom, demonsCavern, "north"));
            #endregion

            #region "Demons Cavern"
            demonsCavern.AddChest(demonsChest);


            #endregion
            #endregion

            #region "Class Setup"
            string name = AnsiConsole.Ask<string>("\n\nWake up adventurer. You seem to have drifted asleep.\n[italic blue]What is your name?[/]\n> ");
            
            
            
            string selection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Which class would you like to play as?")
            .PageSize(7)
            .AddChoices(new[] {
                "[bold darkred_1]HellSpawn[/]", "[bold grey69]Knight[/]", "[bold 153]Mage[/]", "[bold 11]Paladin[/]",
                "[bold 117]Frostbitten[/]", "[bold red3]Spitfire[/]", "[bold 138]Wretch[/]",
             }));


            Player pc = new Player(100, fist, skin, masterKey);
            bool unassigned = true;
            while (unassigned)
            {
                switch (selection)
                {
                    case "[bold darkred_1]HellSpawn[/]":
                        pc = new Player(120, hellsDagger, hellsSet, masterKey);
                        pc.SetStrength(15);
                        pc.SetMana(25);
                        pc.SetAgility(8);
                        pc.AddItem(hellsDagger);
                        pc.AddItem(hellsSet);
                        pc.AddItem(flameSpear);
                        unassigned = false;
                        break;
                    case "[bold grey69]Knight[/]":
                        pc = new Player(150, sword, knightsSet, masterKey);
                        pc.SetStrength(17);
                        pc.SetMana(70);
                        pc.SetAgility(2);
                        pc.AddItem(knightsSet);
                        pc.AddItem(sword);
                        unassigned = false;
                        break;
                    case "[bold 153]Mage[/]":
                        pc = new Player(75, sacredStaff, magesRobes, masterKey);
                        pc.SetStrength(0);
                        pc.SetMana(200);
                        pc.SetAgility(4);
                        pc.AddItem(sacredStaff);
                        pc.AddItem(magesRobes);
                        pc.LearnSpell(frostBolt);
                        pc.LearnSpell(lightning);
                        unassigned = false;
                        break;
                    case "[bold 11]Paladin[/]":
                        pc = new Player(110, hammer, paladingear, masterKey);
                        pc.SetStrength(5);
                        pc.SetAgility(0);
                        pc.SetMana(100);
                        pc.AddItem(paladingear);
                        pc.AddItem(hammer);
                        unassigned = false;
                        break;
                    case "[bold 117]Frostbitten[/]":
                        pc = new Player(100, frostbiteSword, frostbiteSet, masterKey);
                        pc.SetStrength(6);
                        pc.SetAgility(4);
                        pc.SetMana(20);
                        pc.AddItem(frostbiteSet);
                        pc.AddItem(frostbiteSword);
                        unassigned = false;
                        break;
                    case "[bold red3]Spitfire[/]":
                        pc = new Player(100, blazingSword, spitfireSet, masterKey);
                        pc.SetStrength(13);
                        pc.SetMana(75);
                        pc.SetAgility(3);
                        pc.AddItem(spitfireSet);
                        pc.AddItem(blazingSword);
                        unassigned = false;
                        break;
                    case "[bold 138]Wretch[/]":
                        pc = new Player(100, fist, skin, masterKey);
                        pc.SetStrength(0);
                        pc.SetMana(50);
                        pc.SetAgility(0);
                        unassigned = false;
                        break;
                    /*case "god":
                        pc = new Player(100, godSword, godsSet, doorKey);
                        pc.SetStrength(1000);
                        pc.SetMana(1000);
                        pc.SetAgility(1000);
                        pc.AddItem(godSword);
                        pc.AddItem(godsSet);
                        pc.AddItem(doorKey);
                        unassigned = false;
                        break; */

                }
            }
            pc.SetClass(selection);
            #endregion

            #region "Player Set Up"
            pc.SetLocation(startRoom);
            pc.SetName(name);
            pc.AddItem(apple);
            pc.AddItem(cheese);
            pc.AddItem(stoneApple);
            pc.AddItem(fist);
            pc.AddItem(staleBread);
            pc.AddItem(skin);
            pc.AddItem(apple);
            pc.AddItem(stoneApple);
            pc.AddItem(blazingSword);
            pc.AddItem(sword);
            pc.AddItem(stoneApple);

            #endregion

            #region "Game"
            while (!gameOver)
            {
                foreach (Creature creature in pc.GetLocation().GetCreatures())
                {
                    if (creature.GetAggression())
                    {
                        if (creature.GetSpeed() > pc.GetAgility()) 
                        {
                            if (!pc.CheckBlocking())
                            {
                                int damage = creature.GetAttackDamage(pc.GetArmour()); 
                                pc.RemoveHealth(damage);
                                AnsiConsole.MarkupLine($"[italic grey]The {creature.GetName()} attacked you suddenly, dealing[/] [italic red]{damage}[/][italic grey] damage[/]");
                            }
                            else { AnsiConsole.MarkupLine($"[italic grey]The {creature.GetName()} tried to attack you\nYou blocked the attack[/]"); }
                        }

                    }
                }
                pc.ChangeBlocking(false);
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("What would you like to do?\n");
                Console.ForegroundColor = ConsoleColor.White;
                command = AnsiConsole.Ask<string>("> ");
                Console.ForegroundColor = ConsoleColor.DarkGray;

                Console.WriteLine("\x1B[3m");
                gameOver = Command.Execute(command.Split(' ').ToList(), pc);
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

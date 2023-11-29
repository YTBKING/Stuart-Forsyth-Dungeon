using Dungeon;
using Spectre.Console;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace dungeonCore.Game
{

    public interface IGameAction
    {
        abstract static void DoAction(List<string> instructions, Player player);
    }

    // look
    public class Look : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(DoAction);

        public static void DoAction(List<string> instructions, Player player)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\x1B[3m");
            AnsiConsole.Markup(player.GetLocation().GetDescription());
            Console.WriteLine("\x1B[0m");
        }
    }

    // health
    public class Health : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(DoAction);
        public static void DoAction(List<string> instructions, Player player)
        {
            AnsiConsole.MarkupLine($"You have [italic red]{player.GetHealth()}[/][italic grey] health.[/]");
        }
    }

    // move
    public class Move : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(DoAction);

        public static void DoAction(List<string> instructions, Player player)
        {
            if (instructions.Count <= 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Move where?");
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            else
            {
                DoMove(instructions[1].ToLower(), player);
            }
        }
        private static void DoMove(string direction, Player player)
        {
            List<string> exits = player.GetLocation().GetDirections();
            bool directionFound = false;
            for (int i = 0; i < exits.Count; i++)
            {
                if (direction == exits[i].ToLower())
                {
                    directionFound = true;
                    if (!player.GetLocation().GetConnections()[i].GoThrough(player, direction, player.GetKey()))
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

    }

    // get
    public class Take : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(DoAction);

        public static void DoAction(List<string> instructions, Player player)
        {
            int num = 1;
            if (instructions[1].ToLower().Contains("demonic") || instructions[1].ToLower().Contains("frozen") || instructions[1].ToLower().Contains("blazing") || instructions[1].ToLower().Contains("holy") || instructions[1].ToLower().Contains("common") || instructions[1].ToLower().Contains("uncommon") || instructions[1].ToLower().Contains("rare") || instructions[1].ToLower().Contains("epic") || instructions[1].ToLower().Contains("legendary"))
            {
                num = 2;
            }
            string instructionsToDo = "";
            for (int i = num; i < instructions.Count; i++)
            {
                instructionsToDo += instructions[i];
                if (i != instructions.Count - 1)
                {
                    instructionsToDo += " ";
                }
            }
            Item WantedItem = null;
            foreach (Item item in player.GetLocation().GetContents())
            {
                if (item.GetName().ToLower() == instructionsToDo.ToLower())
                {
                    WantedItem = item;
                }
            }
            if (WantedItem != null)
            {
                player.Inventory.Add(player.GetLocation().RemoveItem(instructionsToDo));
                Console.WriteLine($"You picked up a {instructionsToDo}");
                if (WantedItem is NoteItem)
                {
                    player.AddNote((NoteItem)WantedItem);
                }

            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("This item does not exist here");
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
        }
    }

    // drop
    public class Drop : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(DoAction);
        public static void DoAction(List<string> instructions, Player player)
        {
            string rarity = null;
            int nums = 0;
            if (instructions[1].ToLower().Contains("demonic") || instructions[1].ToLower().Contains("frozen") || instructions[1].ToLower().Contains("blazing") || instructions[1].ToLower().Contains("holy") || instructions[1].ToLower().Contains("common") || instructions[1].ToLower().Contains("uncommon") || instructions[1].ToLower().Contains("rare") || instructions[1].ToLower().Contains("epic") || instructions[1].ToLower().Contains("legendary"))
            {
                nums = 2;
                rarity = instructions[1].ToLower();
            }
            string instructionsToDo = "";
            for (int i = nums; i < instructions.Count; i++)
            {
                instructionsToDo += instructions[i];
                if (i != instructions.Count - 1)
                {
                    instructionsToDo += " ";
                }
            }
            string itemToDrop = instructionsToDo;
            Item droppedItem = null;

            foreach (Item item in player.Inventory)
            {
                if (item.GetName().ToLower() == itemToDrop.ToLower())
                {
                    droppedItem = item;
                }
            }
            if (droppedItem != null && !droppedItem.Locked)
            {
                int number = 1;
                if (player.Inventory.Count(n => n == droppedItem) > 1)
                {
                    foreach (Item item in player.Inventory)
                    {
                        if (item.GetName().ToLower() == instructionsToDo.ToLower())
                        {
                            if (item is WeaponItem)
                            {
                                WeaponItem weaponItem = (WeaponItem)item;
                                if (weaponItem.GetTrueRarity().ToLower() == rarity)
                                {
                                    droppedItem = item;
                                }
                            }
                        }
                    }
                    if (droppedItem is WeaponItem && rarity == null)
                    {
                        Console.WriteLine("Which weapon would you like to drop. Please enter rarity as well");
                        return;
                    }
                    try
                    {
                        Console.WriteLine($"How many would you like to drop: 1 -> {player.Inventory.Count(n => n == droppedItem)}");
                        number = Convert.ToInt32(Console.ReadLine());
                        if (number > player.Inventory.Count(n => n == droppedItem))
                        {
                            Console.WriteLine($"It must be a number between 1 and {player.Inventory.Count(n => n == droppedItem)}");
                            number = 1;
                        }
                    }
                    catch (FormatException) { Console.WriteLine("Please enter a valid number"); }
                }
                if (droppedItem.GetName() == player.EquipedWeapon.GetName())
                {
                    Console.WriteLine("Cannot sell your equipped weapon");
                }
                else
                {
                    player.RemoveItem(droppedItem, number);
                    player.GetLocation().AddItem(droppedItem);
                    Console.WriteLine("You dropped " + instructionsToDo);
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
        }
    }

    // attack
    public class Attack : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(DoAction);
        private static Random random = new Random();
        public static void DoAction(List<string> instructions, Player player)
        {
            string instructionsToDo = "";
            for (int i = 1; i < instructions.Count; i++)
            {
                instructionsToDo += instructions[i];
                if (i != instructions.Count - 1)
                {
                    instructionsToDo += " ";
                }
            }
            Creature deadCreature = null;
            if (player.GetLocation().GetStaticCreatures().Count > 0)
            {
                foreach (StaticCreature stat in player.GetLocation().GetStaticCreatures())
                {
                    if (stat.GetName().ToLower() == instructionsToDo.ToLower())
                    {
                        int damage = player.EquipedWeapon.GetDamage() + player.Strength;
                        Console.WriteLine("Attacked");
                        bool dead = stat.TakeDamage(damage);
                        if (dead)
                        {
                            Console.WriteLine($"Your attack destroyed the {stat.GetName()}");
                            player.GetLocation().AddConnection(stat.GetHiddens());
                            player.GetLocation().RemoveStatic(stat);
                            break;
                        }
                    }
                }
            }
            foreach (Creature creature in player.GetLocation().GetCreatures())
            {
                if (creature.GetName().ToLower() == instructionsToDo.ToLower())
                {
                    bool dead = false;
                    int damage = player.EquipedWeapon.GetDamage() + player.Strength;
                    if (player.Agility >= creature.GetSpeed())
                    {

                        dead = creature.TakeDamage(damage, player.EquipedWeapon);
                        Console.WriteLine($"Your attack caused the {creature.GetName()} to lose {creature.TrueDamage} health.");

                        if (dead)
                        {
                            Console.WriteLine($"Your attack killed the {creature.GetName()}");
                            player.AdjustExperience(creature.GetXp());
                            foreach (Item drops in creature.GetDrops())
                            {
                                AnsiConsole.MarkupLine($"[italic grey]The [/][italic red]{creature.GetName()}[/][italic grey] dropped a {drops}[/]");
                                player.GetLocation().AddItem(drops);
                            }
                            player.gold += creature.GetGold();
                            AnsiConsole.MarkupLine($"You got {creature.GetGold()} gold");
                            deadCreature = creature;
                        }
                        else
                        {
                            Console.WriteLine($"The {creature.GetName()} has {creature.GetHealth()} health left");
                            int damageTaken = creature.GetAttackDamage(player.Armour);
                            if (player.DoesDodge())
                            {
                                Console.WriteLine($"You dodged the attack");
                            }
                            else
                            {
                                if (damageTaken < 0) { damageTaken = 0; }
                                Console.WriteLine($"{creature.GetName()} attacks you and causes {damageTaken} damage.");
                                player.Health -= damageTaken;
                            }
                            if (player.IsDead())
                            {
                                Console.WriteLine("You die.");
                                return;
                            }

                        }


                    }
                    else if (creature.GetSpeed() > player.Agility)
                    {
                        int damageTaken = creature.GetAttackDamage(player.Armour);
                        if (damageTaken < 0)
                        {
                            damageTaken = 0;
                        }
                        player.Health -= damageTaken;

                        AnsiConsole.MarkupLine($"{creature.GetName()} attacks you and causes {damageTaken} damage.");
                        if (player.DoesDodge())
                        {
                            Console.WriteLine($"You dodged the attack");
                        }

                        if (player.Health <= 0)
                        {
                            Console.WriteLine("You die.");
                            return;
                        }
                        dead = creature.TakeDamage(damage, player.EquipedWeapon);
                        AnsiConsole.MarkupLine($"Your attack caused the {creature.GetName()} to lose {creature.TrueDamage} health.");

                        if (dead)
                        {
                            AnsiConsole.MarkupLine($"Your attack killed the {creature.GetName()}");
                            player.AdjustExperience(creature.GetXp());
                            foreach (Item drops in creature.GetDrops())
                            {
                                AnsiConsole.MarkupLine($"[italic grey]The [/][italic red]{creature.GetName()}[/][italic grey] dropped a {drops}[/]");
                                player.GetLocation().AddItem(drops);
                            }
                            player.gold += creature.GetGold();
                            AnsiConsole.MarkupLine($"You got {creature.GetGold()} gold");
                            deadCreature = creature;
                        }
                        else { AnsiConsole.MarkupLine($"The {creature.GetName()} has {creature.GetHealth()} health left"); }

                    }

                }
            }
            if (deadCreature != null)
            {
                player.GetLocation().RemoveCreature(deadCreature);
            }
        }
    }

    // cast
    public class Cast
    {

        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(DoAction);
        public static void DoAction(List<string> instructions, Player player)
        {

            if (instructions.Count < 3)
            {
                Console.WriteLine("Cast which spell at which target?");
            }
            else
            {
                string spell = instructions[1];
                string target = "";
                for (int i = 2; i < instructions.Count(); i++)
                {
                    target += instructions[i];
                    if (i != instructions.Count() - 1)
                    {
                        target += " ";
                    }
                }

                int drain = 0;
                int drainindex = player.SpellNames.IndexOf(spell);
                if (drainindex >= 0)
                {
                    drain = player.ManaDrains[drainindex];
                }

                Creature creatureToRemove = null;
                bool dead = false;



                if (player.SpellBook.ContainsKey(spell))
                {
                    foreach (Creature creature in player.GetLocation().GetCreatures())
                    {
                        if (creature.GetName().ToLower() == target.ToLower())
                        {
                            if (drain <= player.Mana)
                            {
                                if (player.EquipedWeapon is StaffWeapon)
                                {
                                    StaffWeapon weapon = (StaffWeapon)player.EquipedWeapon;
                                    dead = creature.TakeSpellDamage(spell, player.SpellBook[spell] + weapon.GetSpellBuff());
                                }
                                else
                                {
                                    dead = creature.TakeSpellDamage(spell, player.SpellBook[spell]);
                                }
                                player.Mana -= drain;
                            }

                            if (dead)
                            {
                                Console.WriteLine($"Your {spell} killed the {creature.GetName()}");
                                creatureToRemove = creature;
                                player.AdjustExperience(creature.GetXp());
                                foreach (Item drops in creature.GetDrops())
                                {
                                    AnsiConsole.MarkupLine($"[italic grey]The [/][italic red]{creature.GetName()}[/][italic grey] dropped a {drops}[/]");
                                    player.GetLocation().AddItem(drops);
                                }
                                player.gold += creature.GetGold();
                                Console.WriteLine($"The {creature.GetName()} dropped [italic 178]{creature.GetGold()}g[/]");
                            }
                            else
                            {
                                player.Health -= creature.GetAttackDamage(player.Armour);
                                AnsiConsole.MarkupLine($"You dealt {player.SpellBook[spell]} to the [italic red]{creature.GetName()}[/]");
                                Console.WriteLine($"The [italic red]{creature.GetHealth()}[/][italic grey] has {creature.GetHealth()} health left[/]");
                                if (player.IsDead())
                                {
                                    Console.WriteLine("You die.");
                                    return;
                                }

                            }
                        }
                    }
                    if (creatureToRemove != null)
                    {
                        player.GetLocation().RemoveCreature(creatureToRemove);
                    }
                }
                else
                {
                    Console.WriteLine("You don't know that spell!");
                }
            }
        }
    }

    // examine
    public class Examine
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(Cast.DoAction);
        public static void DoAction(List<string> instructions, Player player)
        {
            int num = 1;
            if (instructions[1].ToLower().Contains("demonic") || instructions[1].ToLower().Contains("frozen") || instructions[1].ToLower().Contains("blazing") || instructions[1].ToLower().Contains("holy") || instructions[1].ToLower().Contains("common") || instructions[1].ToLower().Contains("uncommon") || instructions[1].ToLower().Contains("rare") || instructions[1].ToLower().Contains("epic") || instructions[1].ToLower().Contains("legendary"))
            {
                num = 2;
            }
            string instructionsToDo = "";
            for (int i = num; i < instructions.Count; i++)
            {
                instructionsToDo += instructions[i];
                if (i != instructions.Count - 1)
                {
                    instructionsToDo += " ";
                }
            }
            if (instructions.Count <= 1)
            {
                Console.WriteLine("Examine what?");
            }
            else
            {
                for (int i = 0; i < player.Inventory.Count; i++)
                {
                    if (instructionsToDo.ToLower() == player.Inventory[i].GetName().ToLower())
                    {
                        Console.WriteLine(player.Inventory[i].GetDescription());
                    }
                }

            }
        }
    }

    // eat
    public class Eat : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(DoAction);
        public static void DoAction(List<string> instructions, Player player)
        {
            string instructionsToDo = "";
            for (int i = 1; i < instructions.Count; i++)
            {
                instructionsToDo += instructions[i];
                if (i != instructions.Count - 1)
                {
                    instructionsToDo += " ";
                }
            }
            if (instructions.Count <= 1)
            {
                Console.WriteLine("Eat what?");
            }
            else
            {
                player.Eat(instructionsToDo);
            }
        }


    }

    // Inv
    public class Inventory : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(DoAction);
        public static void DoAction(List<string> instructions, Player player)
        {
            string items = "\n";

            if (player.Inventory.Count > 0)
            {
                if (player.Inventory.Count == 1)
                {
                    items += $"You have the following item: {player.Inventory[0].GetName()}.";
                }
                else
                {
                    items += player.GetInventory();
                }
            }
            else
            {
                items += "You aren't carrying anything.";
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            AnsiConsole.Markup(items);
            Console.ForegroundColor = ConsoleColor.DarkGray;
        }
    }

    // equip
    public class Equip : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(DoAction);
        public static void DoAction(List<string> instructions, Player player)
        {
            int num = 1;
            string rarity = null;
            try
            {
                if (instructions[1].ToLower().Contains("demonic") || instructions[1].ToLower().Contains("frozen") || instructions[1].ToLower().Contains("blazing") || instructions[1].ToLower().Contains("holy") || instructions[1].ToLower().Contains("common") || instructions[1].ToLower().Contains("uncommon") || instructions[1].ToLower().Contains("rare") || instructions[1].ToLower().Contains("epic") || instructions[1].ToLower().Contains("legendary"))
                {
                    num = 2;
                    rarity = instructions[1].ToLower();
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                AnsiConsole.MarkupLine("Please enter a [italic red]weapon[/]");
            }
            string instructionsToDo = "";
                for (int i = num; i < instructions.Count; i++)
                {
                    instructionsToDo += instructions[i];
                    if (i != instructions.Count - 1)
                    {
                        instructionsToDo += " ";
                    }
                }

            Item EquippingItem = null;
            foreach (Item item in player.Inventory)
            {
                if (item.GetName().ToLower() == instructionsToDo.ToLower())
                {
                    EquippingItem = item;
                }
            }
            if (EquippingItem != null)
            {
                foreach (Item item in player.Inventory)
                {
                    if (item.GetName().ToLower() == instructionsToDo.ToLower())
                    {
                        if (item is WeaponItem)
                        {
                            WeaponItem weaponItem = (WeaponItem)item;
                            if (weaponItem.GetTrueRarity().ToLower() == rarity)
                            {
                                EquippingItem = item;
                            }
                        }
                    }
                }
                if (EquippingItem is WeaponItem && rarity == null)
                {
                    Console.WriteLine("Which weapon would you like to equip. Please enter rarity as well");
                    return;
                }
                player.EquipItem(EquippingItem);
            }
        }
    }

    // wear
    public class Wear : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(DoAction);
        public static void DoAction(List<string> instructions, Player player)
        {
            string instructionsToDo = "";
            for (int i = 1; i < instructions.Count; i++)
            {
                instructionsToDo += instructions[i];
                if (i != instructions.Count - 1)
                {
                    instructionsToDo += " ";
                }
            }
            Item WearingItem = null;
            foreach (Item item in player.Inventory)
            {
                if (item is ArmourItem)
                {
                    if (item.GetName().ToLower() == instructionsToDo.ToLower())
                    {
                        WearingItem = item;
                    }
                }
            }
            if (WearingItem != null && WearingItem.IsArmour)
            {
                player.WearArmour(WearingItem);
                Console.WriteLine("You have put on a " + instructionsToDo);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Could not find this item");
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
        }
    }

    // learn spell
    public class Learn : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(DoAction);
        public static void DoAction(List<string> instructions, Player player)
        {
            string instructionsToDo = "";
            for (int i = 1; i < instructions.Count; i++)
            {
                instructionsToDo += instructions[i];
                if (i != instructions.Count - 1)
                {
                    instructionsToDo += " ";
                }
            }
            Item Learning = null;
            for (int i = 0; i < player.Inventory.Count; i++)
            {
                if (player.Inventory[i].GetName().ToLower() == instructionsToDo.ToLower())
                {
                    Learning = player.Inventory[i];
                }
            }

            if (Learning != null)
            {
                bool learned = player.LearnSpell(Learning);
                player.RemoveItem(Learning, 1);
                if (learned)
                {
                    Console.WriteLine("You learned " + instructionsToDo);
                }
            }
            else
            {
                Console.WriteLine("You don't have a " + instructionsToDo);
            }
        }
    }

    public class Experience : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(DoAction);
        public static void DoAction(List<string> instructions, Player player)
        {
            Console.WriteLine($"You Have: {player.GetExperience()} / {player.GetXPNeeded()}");
        }
    }

    // stats
    public class Stats : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(DoAction);
        public static void DoAction(List<string> instructions, Player player)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Level " + player.GetLevel());
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"Max Health: {player.MaxHealth}\nMax Mana: {player.MaxMana}\nStrength: {player.Strength}\nAgility: {player.Agility}");
        }
    }

    // talk
    public class Talk : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(DoAction);
        public static void DoAction(List<string> instructions, Player player)
        {
            string instructionsToDo = "";
            for (int i = 1; i < instructions.Count; i++)
            {
                instructionsToDo += instructions[i];
                if (i != instructions.Count - 1)
                {
                    instructionsToDo += " ";
                }
            }
            if (player.GetLocation().HasNPC())
            {
                if (player.GetLocation().GetNPC().Count == 1)
                {
                    player.talking = player.GetLocation().GetNPC()[0];
                    AnsiConsole.Markup(player.Talk(player.GetLocation().GetNPC()[0]));
                }
                else
                {
                    if (instructionsToDo == "")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Which person?");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                    else
                    {
                        var found = player.GetLocation().GetNPC().Find(item => item.GetName().ToLower() == instructionsToDo.ToLower());
                        if (found != null)
                        {
                            // Downcasting, but better than holding base class pointer.
                            // We don't want to always through away strong typing.
                            player.talking = found;
                            AnsiConsole.Markup(player.Talk(player.talking));
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Sorry could not find anyone with that name.");
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                        }
                    }
                }
            }
            else { Console.WriteLine("This room does not have anyone to talk to"); }
        }
    }

    // buy
    public class Buy : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(DoAction);
        public static void DoAction(List<string> instructions, Player player)
        {
            int num = 1;
            if (instructions[1].ToLower().Contains("demonic") || instructions[1].ToLower().Contains("frozen") || instructions[1].ToLower().Contains("blazing") || instructions[1].ToLower().Contains("holy") || instructions[1].ToLower().Contains("common") || instructions[1].ToLower().Contains("uncommon") || instructions[1].ToLower().Contains("rare") || instructions[1].ToLower().Contains("epic") || instructions[1].ToLower().Contains("legendary"))
            {
                num = 2;
            }
            string instructionsToDo = "";
            for (int i = num; i < instructions.Count; i++)
            {
                instructionsToDo += instructions[i];
                if (i != instructions.Count - 1)
                {
                    instructionsToDo += " ";
                }
            }
            if (player.GetLocation().HasNPC())
            {
                if (player.talking == null)
                {
                    player.GetRecentNPC();
                }
                if (instructions.Count > 1)
                {
                    if (player.talking is SellerNPC)
                    {
                        player.talking.BuyItem(player, instructionsToDo);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Cannot buy items here");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                }
                else { Console.WriteLine("Buy what?"); };
            }
            else
            {
                Console.WriteLine("This room does not have anyone to talk to");
            }
        }
    }

    // sell
    public class Sell : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(DoAction);
        public static void DoAction(List<string> instructions, Player player)
        {
            int nums = 1;
            string rarity = null;
            if (instructions[1].ToLower().Contains("demonic") || instructions[1].ToLower().Contains("frozen") || instructions[1].ToLower().Contains("blazing") || instructions[1].ToLower().Contains("holy") || instructions[1].ToLower().Contains("common") || instructions[1].ToLower().Contains("uncommon") || instructions[1].ToLower().Contains("rare") || instructions[1].ToLower().Contains("epic") || instructions[1].ToLower().Contains("legendary"))
            {
                nums = 2;
                rarity = instructions[1].ToLower();
            }
            string instructionsToDo = "";
            for (int i = nums; i < instructions.Count; i++)
            {
                instructionsToDo += instructions[i];
                if (i != instructions.Count - 1)
                {
                    instructionsToDo += " ";
                }
            }
            Random random = new Random();
            string itemToSell = instructionsToDo;
            int num = 1;
            if (player.talking == null)
            {
                player.GetRecentNPC();
            }
            var SoldItem = player.Inventory.Find(item => item.GetName().ToLower() == instructionsToDo.ToLower());
            if (SoldItem != null)
            {

                if (player.Inventory.Count(n => n == SoldItem) >= 1)
                {
                    foreach (Item item in player.Inventory)
                    {
                        if (item.GetName().ToLower() == instructionsToDo.ToLower())
                        {
                            if (item is WeaponItem)
                            {
                                WeaponItem weaponItem = (WeaponItem)item;
                                if (weaponItem.GetTrueRarity().ToLower() == rarity)
                                {
                                    SoldItem = item;
                                }
                            }
                        }
                    }
                    if (SoldItem is WeaponItem && rarity == null)
                    {
                        Console.WriteLine("Which weapon would you like to sell. Please enter rarity as well");
                        return;
                    }

                    try
                    {
                        while (true)
                        {
                            if (player.Inventory.Count(n => n == SoldItem) == 1)
                            {
                                num = 1;
                            }
                            else
                            {
                                Console.WriteLine($"How many would you like to sell: 1 -> {player.Inventory.Count(n => n == SoldItem)}");
                                num = Convert.ToInt32(Console.ReadLine());
                            }


                            if (num > player.Inventory.Count(n => n == SoldItem))
                            {
                                Console.WriteLine($"Must be a number between 1 and {player.Inventory.Count(n => n == SoldItem)}");

                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    catch (FormatException) { Console.WriteLine("Must be a valid number\n"); }
                    if (SoldItem.GetName() == player.EquipedWeapon.GetName())
                    {
                        Console.WriteLine("Cannot sell your equipped weapon");
                    }
                    else
                    {
                        player.RemoveItem(SoldItem, num);
                        player.gold += SoldItem.GetValue() * num;
                        if (player.talking is SellerNPC)
                        {
                            player.talking.AddVendorItems(SoldItem, SoldItem.GetValue() + random.Next(1, 35));
                            Console.WriteLine($"You sold {num} {itemToSell} to {player.talking.GetName()} for {SoldItem.GetValue() * num} gold");
                        }
                        else
                        {
                            Console.WriteLine($"You sold {num} {itemToSell} for {SoldItem.GetValue() * num} gold");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine($"Sorry could not find {instructionsToDo}.");
            }
        }
    }

    // open
    public class Open : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(DoAction);
        public static void DoAction(List<string> instructions, Player player)
        {

            if (player.GetLocation().GetChests().Count > 0)
            {
                try
                {
                    if (instructions[1] != null)
                    {
                        try
                        {
                            player.Open(player.GetLocation().GetChests()[Convert.ToInt32(instructions[1]) - 1]);
                        }
                        catch (FormatException) { Console.WriteLine($"Must be a valid number between 1 and {player.GetLocation().GetChests().Count}"); }
                        catch (ArgumentOutOfRangeException) { Console.WriteLine($"Must be a valid number between 1 and {player.GetLocation().GetChests().Count}"); }
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    player.Open(player.GetLocation().GetChests()[0]);
                }


            }
            else { Console.WriteLine("There is nothing to open"); }
        }
    }

    // gold
    public class Gold : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(DoAction);
        public static void DoAction(List<string> instructions, Player player)
        {
            AnsiConsole.Markup($"You have [italic 178]{player.gold} gold[/]");
        }
    }

    // clear screen
    public class Clear : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(DoAction);
        public static void DoAction(List<string> instructions, Player player)
        {
            Console.Clear();
        }
    }

    // upgrade
    public class Upgrade : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(DoAction);
        public static void DoAction(List<string> instructions, Player player)
        {
            int num = 1;
            if (instructions[1].ToLower().Contains("demonic") || instructions[1].ToLower().Contains("frozen") || instructions[1].ToLower().Contains("blazing") || instructions[1].ToLower().Contains("holy") || instructions[1].ToLower().Contains("common") || instructions[1].ToLower().Contains("uncommon") || instructions[1].ToLower().Contains("rare") || instructions[1].ToLower().Contains("epic") || instructions[1].ToLower().Contains("legendary"))
            {
                num = 2;
            }
            string instructionsToDo = "";
            for (int i = num; i < instructions.Count; i++)
            {
                instructionsToDo += instructions[i];
                if (i != instructions.Count - 1)
                {
                    instructionsToDo += " ";
                }
            }
            if (player.GetLocation().HasNPC())
            {
                if (player.talking == null)
                {
                    player.GetRecentNPC();
                }

                var found = player.Inventory.Find(item => item.GetName().ToLower() == instructionsToDo.ToLower());
                if (found != null)
                {
                    if (player.talking is SmithNPC)
                    {
                        try
                        {
                            player.talking.UpgradeWeapon(player, (WeaponItem)found);
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
        }
    }

    // mana
    public class Mana : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(DoAction);
        public static void DoAction(List<string> instructions, Player player)
        {
            AnsiConsole.Markup($"You have [italic 27]{player.Mana} mana [/] left");
        }
    }

    // read
    public class Read : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(DoAction);
        public static void DoAction(List<string> instructions, Player player)
        {
            string instructionsToDo = "";
            for (int i = 1; i < instructions.Count; i++)
            {
                instructionsToDo += instructions[i];
                if (i != instructions.Count - 1)
                {
                    instructionsToDo += " ";
                }
            }
            var found1 = player.Inventory.Find(item => item.GetName().ToLower() == instructionsToDo.ToLower());
            if (found1 != null && found1 is NoteItem)
            {
                // Downcasting, but better than holding base class pointer.
                // We don't want to always through away strong typing.
                NoteItem note = (NoteItem)found1;
                AnsiConsole.Markup($"You open your notebook and read the note labeled [italic 178]{note.GetName()}[/]\nIt reads:\n");
                AnsiConsole.Markup($"[italic grey]{note.GetDescription()}[/]");
            }
            else
            {
                Console.WriteLine("Cannot read this");
            }
        }
    }

    // notes
    public class Notes : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(DoAction);
        public static void DoAction(List<string> instructions, Player player)
        {
            Console.WriteLine("You open your notebook.");
            if (player.NoteBook.Count() == 0)
            {
                Console.WriteLine("Your notebook is empty");
            }
            else
            {
                foreach (NoteItem note in player.NoteBook)
                {
                    Console.WriteLine(note.GetName());
                }
            }
        }
    }

    // block
    public class Block : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(DoAction);
        public static void DoAction(List<string> instructions, Player player)
        {
            player.ChangeBlocking(true);
        }
    }

    // quit
    public class Quit : IGameAction
    {
        public static Action<List<string>, Player> Instance = new Action<List<string>, Player>(DoAction);
        public static void DoAction(List<string> instructions, Player player)
        {
            player.SetHealth(0);
        }
    }



    public class Command
    {

        private static Dictionary<string, Action<List<string>, Player>> actions
                = new Dictionary<string, Action<List<string>, Player>>
                {
                    {"l", Look.Instance },
                    {"look", Look.Instance },
                    {"h", Health.Instance },
                    {"health", Health.Instance },
                    {"move", Move.Instance },
                    {"go", Move.Instance },
                    {"t", Take.Instance },
                    {"take", Take.Instance },
                    {"get", Take.Instance },
                    {"d", Drop.Instance},
                    {"drop", Drop.Instance },
                    {"a", Attack.Instance},
                    {"attack", Attack.Instance },
                    {"c", Cast.Instance },
                    {"cast", Cast.Instance },
                    {"examine", Examine.Instance },
                    {"eat", Eat.Instance },
                    {"i", Inventory.Instance },
                    {"inventory", Inventory.Instance},
                    {"s", Stats.Instance },
                    {"stats", Stats.Instance},
                    {"quit", Quit.Instance },
                    {"q", Quit.Instance },
                    {"w", Wear.Instance },
                    {"wear", Wear.Instance},
                    {"equip", Equip.Instance },
                    {"learn", Learn.Instance },
                    {"xp", Experience.Instance },
                    {"experience", Experience.Instance },
                    {"talk", Talk.Instance },
                    {"buy", Buy.Instance },
                    {"sell", Sell.Instance },
                    {"open", Open.Instance},
                    {"g", Gold.Instance},
                    {"gold", Gold.Instance},
                    {"clear", Clear.Instance},
                    {"upgrade", Upgrade.Instance},
                    {"m", Mana.Instance},
                    {"mana", Mana.Instance},
                    {"r", Read.Instance},
                    {"read", Read.Instance},
                    {"notes", Notes.Instance },
                    {"block", Block.Instance},
                    {"mew", Block.Instance},

                };

        public static bool Execute(List<string> instructions, Player player)
        {
            if (actions.ContainsKey(instructions[0]))
            {
                actions[instructions[0]](instructions, player);
            }
            else
            {
                Console.WriteLine($"Sorry, I don't know how to do that");
            }

            return player.IsDead();
        }
    }
}

﻿using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dungeon
{
    class Room
    {
        #region "Properties"
        private string Description;
        private List<Item> Contents = new List<Item>();
        private List<Creature> Creatures = new List<Creature>();
        private List<Connection> Connections = new List<Connection>();
        private List<StaticCreature> StaticCreatures = new List<StaticCreature>();
        private List<Chest> Chests = new List<Chest>();
        private List<NPC> NPCs = new List<NPC>();
        bool KeyNeeded;
        private bool ContainsNPC = false;
        #endregion
        public Room(string description, bool keyNeeded = false)
        {
            Description = description;
            KeyNeeded = keyNeeded;
        }

        #region "NPC's"
        public bool HasNPC()
        {
            return ContainsNPC;
        }
        public void AddNPC(NPC npc)
        {
            NPCs.Add(npc);
            ContainsNPC = true;
        }
        public List<NPC> GetNPC()
        {
            return NPCs;
        }

        #endregion

        #region "Add Contents"
        #region "Items"
        public void AddItem(Item item)
        {
            Contents.Add(item);
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
        #endregion

        #region "Chests"
        public void AddChest(Chest chests)
        {
            Chests.Add(chests);
        }
        public List<Chest> GetChests()
        {
            return Chests;
        }
        public void RemoveChest(Chest chests)
        {
            Chests.Remove(chests);
        }

        #endregion

        #region "Static Creatures"
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
        #endregion

        #region "Creatures"
        public void AddCreature(Creature creature)
        {
            Creatures.Add(creature);
        }
        public void RemoveCreature(Creature creature)
        {
            Creatures.Remove(creature);
        }
        public List<Creature> GetCreatures()
        {
            return Creatures;
        }
        #endregion

        #region "Connections and Directions"
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
        #endregion
        #endregion

        #region "Description"
        public virtual string GetDescription()
        {
            #region "Items (Light Blue)"
            string items = "\n";
            if (Contents.Count > 0)
            {
                if (Contents.Count == 1)
                {
                    items += $"You can see the following item: [italic 27]{Contents[0].GetName()}[/][italic grey].[/]";
                }
                else
                {
                    items += $"[italic grey]You can see the following items: [/][italic 27]{Contents[0].GetName()}[/]";
                    for (int i = 1; i < Contents.Count - 1; i++)
                    {
                        items += "[italic grey], [/][italic 27]" + Contents[i].GetName() + "[/]";
                    }
                    items += $"[italic grey] and [/][italic 27]{Contents[Contents.Count - 1].GetName()}[/].";
                }
            }
            else
            {
                items = "";
            }
            #endregion

            #region "Creatures (lime)"
            string creatures = "\n";
            if (Creatures.Count > 0)
            {
                if (Creatures.Count == 1)
                {
                    creatures += $"[italic grey]You can see the following creature: [/][italic lime]{Creatures[0].GetName()}[/][italic grey].[/]";
                }
                else
                {
                    creatures += $"[italic grey]You can see the following creatures:[/] [italic lime]{Creatures[0].GetName()}[/]";
                    for (int i = 1; i < Creatures.Count - 1; i++)
                    {
                        creatures += "[italic grey], [/][italic lime]" + Creatures[i].GetName() + "[/]";
                    }
                    creatures += $"[italic grey] and [/][italic lime]{Creatures[Creatures.Count - 1].GetName()}[/][italic grey].[/]";
                }
            }
            else
            {
                creatures = "";
            }
            #endregion

            #region "Connections (Purple)"
            String exits = "\n";
            if (Connections.Count > 0)
            {
                if (Connections.Count == 1)
                {
                    exits += $"[italic grey]There is an exit to the [/][italic purple]{Connections[0].GetDirection()}[/][italic grey].[/]";
                }
                else
                {
                    exits += $"[italic grey]There are exits to the[/] [italic purple]{Connections[0].GetDirection()}[/]";
                    for (int i = 1; i < Connections.Count - 1; i++)
                    {
                        exits += "[italic grey], [/][italic purple]" + Connections[i].GetDirection() +"[/]";
                    }
                    exits += $"[italic grey] and [/][italic purple]{Connections[Connections.Count - 1].GetDirection()}[/][italic grey].[/]";
                }
            }
            #endregion

            #region "Vendors (Green)
            string vendors = "\n";
            if (NPCs.Count == 1)
            {
                vendors += ($"[italic grey]There is a person - [/][italic green]{NPCs[0].GetName()}[/]");
            }
            else if (NPCs.Count > 1)
            {
                vendors += "[italic grey]There are people you can talk to - [/]";
                for (int i = 0; i < NPCs.Count - 1; i++)
                {
                    vendors += $"[italic green]{NPCs[i].GetName()}[/][italic grey], [/]";
                }
                vendors += $"[italic grey]and[/] [italic green]{NPCs[NPCs.Count - 1].GetName()}[/]";
            }
            else { vendors = ""; };
            #endregion

            string chests = "\n";
            if (Chests.Count >= 1)
            {
                if (Chests.Count == 1)
                {
                    chests += "[italic grey]There is[/] [italic yellow]1 chest[/]";
                }
                else
                {
                    chests += "[italic grey]There are [/][italic yellow]" + Chests.Count + " chests[/]";
                }
            }
            else { chests = ""; }
            return Description + items + creatures + exits + vendors + chests;
        }
        public void SetDescription(string description)
        {
            Description = description;
        }
        #endregion

        #region "Get Contents"
        public List<Item> GetContents()
        {
            return Contents;
        }
        #endregion
    }
}

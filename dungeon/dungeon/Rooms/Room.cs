using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dungeon
{
    class Room
    {
        private string Description;
        private List<Item> Contents = new List<Item>();
        private List<Creature> Creatures = new List<Creature>();
        private List<Connection> Connections = new List<Connection>();
        private List<StaticCreature> StaticCreatures = new List<StaticCreature>();
        private List<Chest> Chests = new List<Chest>();
        private List<NPC> NPCs = new List<NPC>();
        bool KeyNeeded;
        private bool ContainsNPC = false;

        public Room(String description, bool keyNeeded = false)
        {
            Description = description;
            KeyNeeded = keyNeeded;
        }
        public bool KeyNeccesary()
        {
            return KeyNeeded;
        }
        public bool HasNPC()
        {
            return ContainsNPC;
        }
        public void AddItem(Item item)
        {
            Contents.Add(item);
        }
        public List<Chest> GetChests()
        {
            return Chests;
        }
        public void AddChest(Chest chests)
        {
            Chests.Add(chests);
        }
        public void RemoveChest(Chest chests)
        {
            Chests.Remove(chests);
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
        public void AddNPC(NPC npc)
        {
            NPCs.Add(npc);
            ContainsNPC = true;
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
                if (Connections.Count == 1)
                {
                    exits += $"There is an exit to the {Connections[0].GetDirection()}.";
                }
                else
                {
                    if (v != 0)
                    {
                        exits += $"There are exits to the {Connections[0].GetDirection()}";
                        for (int i = 1;i < Connections.Count; i++)
                        {
                            if (i == v)
                            {
                                continue;
                            }
                            else
                            {
                                if (i + 1 > Connections.Count)
                                {
                                    exits += $" and {Connections[i].GetDirection()}.";
                                }
                                else
                                {
                                    exits += ", " + Connections[i].GetDirection();
                                }
                            }
                        }
                    }
                    else
                    {
                        exits += $"There are exits to the {Connections[1].GetDirection()}";
                        for (int i = 2; i < Connections.Count; i++)
                        {

                            if (i + 1 > Connections.Count)
                            {
                                exits += $" and {Connections[i].GetDirection()}.";
                            }
                            else
                            {
                                exits += ", " + Connections[i].GetDirection();
                            }

                        }

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
                vendors += $"There is a person - {NPCs[0].GetName()}";
            }
            else if (NPCs.Count > 1)
            {
                vendors += "There are people you can talk to - ";
                for (int i = 0; i < NPCs.Count - 1; i++)
                {
                    vendors += $"{NPCs[i].GetName()}, ";
                }
                vendors += $"and {NPCs[NPCs.Count - 1].GetName()}";
            }
            else { vendors = ""; };

            string chests = "\n";
            if (Chests.Count >= 1)
            {
                if (Chests.Count == 1)
                {
                    chests += "There is 1 chest";
                }
                else
                {
                    chests += "There are " + Chests.Count + " chests";
                }
            }
            else { chests = ""; }
            return Description + items + creatures + exits + vendors + chests;
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
}

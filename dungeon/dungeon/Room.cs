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
        public List<Chest> GetChests()
        {
            return Chests;
        }
        public void AddChest(Chest chests)
        {
            Chests.Add(chests);
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
            int connectionCount = Connections.Count;

            for (int j = 0; j < Connections.Count; j++)
            {
                if (Connections[j].GetDirection() == "talk") { v = j; break; }
            }

            if (ContainsVendor())
            {
                connectionCount--;
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
                    }
                    else
                    {
                        exits += $"There are exits to the {Connections[1].GetDirection()}";
                        for (int i = 1; i < Connections.Count - 1; i++)
                        {
                            if (i != v)
                            {
                                if (i + 1 > Connections.Count)
                                {

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
                            if ((i + 1 == v && i + 2 >= Connections.Count) || (i + 1 >= Connections.Count()))
                            {
                                exits += $" and {Connections[i].GetDirection()}.";
                                break;
                            }
                            else
                            {
                                exits += ", " + Connections[i].GetDirection();
                            }

                        }

                    }
                    if (Connections.Count - 1 != v && connectionCount != 1)
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

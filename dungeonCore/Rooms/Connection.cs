using Dungeon.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
    public class Connection
    {
        #region "Properties"
        private Room RoomFrom;
        private Room RoomTo;
        private string Direction;
        public bool KeyNeeded;
        #endregion

        public Connection(Room roomFrom, Room roomTo, string direction, bool keyNeeded = false)
        {
            RoomFrom = roomFrom;
            RoomTo = roomTo;
            Direction = direction;
            KeyNeeded = keyNeeded;
        }
        public static void MakeConnection(Room roomFrom, Room roomTo, String direction)
        {


            string opposite = string.Empty;
            switch (direction)
            {
                case "north":
                    opposite = "south";
                    break;

                case "south":
                    opposite = "north";
                    break;

                case "west":
                    opposite = "east";
                    break;

                case "east":
                    opposite = "west";
                    break;
                default:
                    throw new Exception("Unknown direction building room.");
            }

            Connection from = new Connection(roomFrom, roomTo, direction);
            Connection to = new Connection(roomTo, roomFrom, opposite);
            roomFrom.AddConnection(from);
            roomTo.AddConnection(to);
        }

        #region "Directions"
        public bool GoThrough(Player player, string direction, Item key)
        {
            bool wantGo = true;

            if ((player.GetLocation() == RoomFrom) && (direction == Direction))
            {
                if ((KeyNeeded && player.ObtainedKey()) || !KeyNeeded)
                {
                    if (KeyNeeded) 
                    { 
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Would you like to use the key? ");
                        string answer = Console.ReadLine();
                        if (answer.ToUpper() == "no") 
                        { 
                            wantGo = false;
                        }
                    }
                    if (wantGo)
                    {
                        if (RoomTo is ChallengeRoom)
                        {
                            foreach (Creature creature in player.GetDeadCreatures())
                            {
                                if (!RoomTo.GetCreatures().Contains(creature))
                                {
                                    RoomTo.AddCreature(creature);
                                }

                            }

                        }
                        player.SetLocation(RoomTo);


                    }

                    if (KeyNeeded)
                    { 
                        KeyNeeded = false;
                        player.RemoveItem(key, 1);
                    }

                    return true;
                }
                else { Console.WriteLine("You need a key to acess this room"); return false; }
            }
            else
            {
                return false;
            }
        }
        public string GetDirection()
        {
            return Direction;
        }
        #endregion
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dungeon
{
    class Connection
    {
        private Room RoomFrom;
        private Room RoomTo;
        private String Direction;
        public bool KeyNeeded;


        public Connection(Room roomFrom, Room roomTo, String direction, bool keyNeeded = false)
        {
            RoomFrom = roomFrom;
            RoomTo = roomTo;
            Direction = direction;
            KeyNeeded = keyNeeded;
        }
        public Boolean GoThrough(Player player, String direction, Item key)
        {
            bool wantGo = true;

            if ((player.GetLocation() == RoomFrom) && (direction == Direction))
            {
                if ((KeyNeeded && player.KeyObtained) || !KeyNeeded)
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
                        player.SetLocation(RoomTo);
                    }

                    if (KeyNeeded)
                    { 
                        KeyNeeded = false;
                        player.KeyObtained = false;
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
        public String GetDirection()
        {
            return Direction;
        }
    }

}

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

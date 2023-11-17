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
        private NPCRoom Vendor;


        public Connection(Room roomFrom, Room roomTo, String direction, bool keyNeeded = false, NPCRoom vendor = null)
        {
            RoomFrom = roomFrom;
            RoomTo = roomTo;
            Direction = direction;
            KeyNeeded = keyNeeded;
            Vendor = vendor;
        }
        public Boolean GoThrough(Player player, String direction, Item key)
        {


            if ((player.GetLocation() == RoomFrom) && (direction == Direction))
            {
                if ((KeyNeeded && player.KeyObtained) || !KeyNeeded)
                {
                    player.SetLocation(RoomTo);
                    if (Vendor != null)
                    {
                        Vendor.SetPlayerVendor(player);
                    }

                    if (KeyNeeded)
                    {
                        KeyNeeded = false;
                        player.KeyObtained = false;
                        player.RemoveItem(key);
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

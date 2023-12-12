using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dungeon
{
    public class Chest
    {
        [JsonInclude]
        private bool KeyNeeded;
        [JsonInclude]
        private List<Item> Contents = new List<Item>();
        public Chest(bool keyNeeded = false) 
        {
            KeyNeeded = keyNeeded;
        }

        public void AddItem(Item item, int num = 1)
        {
            for (int i = 1; i <= num; i++)
            {
                Contents.Add(item);
            }

        }

        public bool KeyRequired() 
        {
            return KeyNeeded;
        }

        public void UnlockChest()
        {
            KeyNeeded = false;
        }

        public List<Item> GetContents()
        {
            return Contents;
        }

        public void RemoveItem(Item item)
        {
            Contents.Remove(item);
        }


    }
}

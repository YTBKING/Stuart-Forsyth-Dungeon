using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
    public class Chest
    {
        private List<Item> Contents = new List<Item>();
        public Chest() { }

        public void AddItem(Item item, int num = 1)
        {
            for (int i = 1; i <= num; i++)
            {
                Contents.Add(item);
            }

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

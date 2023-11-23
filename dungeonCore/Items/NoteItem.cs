using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
    public class NoteItem : Item
    {
        public NoteItem(string name, string note) : base(name, note, 1, true) { }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}

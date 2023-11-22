using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dungeon
{
    public class KeyItem : Item
    {
        public KeyItem(string name, string description, int value, bool locked = true) : base(name, description, value, locked, false) { }
    }
}

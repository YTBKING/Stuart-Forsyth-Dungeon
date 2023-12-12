using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dungeon
{
    public class ArmourItem : Item
    {
        #region "Properties"
        public int Defence;
        #endregion

        public ArmourItem(string name, string description, int defence, int value, bool locked = false) : base(name, description, value, locked) 
        {
            Name = name;
            Description = description;
            Value = value;
            Defence = defence;
            IsArmour = true;
        }

        #region "Get Properties"
        public int GetDefence() { return Defence; }
        #endregion

        public override string ToString()
        {
            return $"{Name}: [italic red]-{Defence}[/] enemy damage: Worth {Value}g";
        }


    }
}

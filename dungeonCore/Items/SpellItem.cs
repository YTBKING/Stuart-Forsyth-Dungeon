using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dungeon
{
    public class SpellItem : Item
    {
        [JsonInclude]
        private int SpellDamage;
        [JsonInclude]
        private int ManaDrain;
        public SpellItem(string name, string description, int damage, int manaDrain, int value) : base(name, description, value) 
        { 
            Name = name;
            Description = description;
            Value = value;
            SpellDamage = damage;
            ManaDrain = manaDrain;
        }
        public int GetSpellDamage()
        {
            return SpellDamage;
        }
        public int GetManaDrain()
        {
            return ManaDrain;
        }
        public override string ToString()
        {
            return $"{Name}: [italic red]{SpellDamage}[/] damage: -{ManaDrain} mana: Worth {Value}g";
        }
    }
}

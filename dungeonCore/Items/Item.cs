using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dungeon
{
    public class Item
    {
        #region "Properties"
        [JsonInclude]
        protected string Name;
        [JsonInclude]
        protected string Description;
        [JsonInclude]
        public bool IsEdible;
        [JsonInclude]
        protected int Value;
        public bool Locked;
        public bool IsArmour = false;
        #endregion
        public Item() { }
        public Item(string name, string description, int value, bool locked = false, bool isEdible = false)
        {
            Name = name;
            Description = description;
            IsEdible = isEdible;
            Value = value;
            Locked = locked;

        }

        #region "Get Properties"
        public virtual int GetHeals()
        {
            return 0;
        }
        public virtual int GetManaHeals() 
        { 
            return 0; 
        }
        public string GetName()
        {
            return Name;
        }
        public int GetValue()
        {
            return Value;
        }
        public string GetDescription()
        {
            return Description;
        }

        #endregion

        #region "Set Properties"
        public void SetName(string name)
        {
            Name = name;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }
        #endregion

        public override string ToString()
        {
            return ($"{Name}: Worth {Value}g ");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dungeon
{
    class Item
    {
        protected String Name;
        protected String Description;
        public bool IsEdible;
        protected int Damage;
        protected int Value;
        public bool Locked;
        public bool IsArmour = false;

        public Item(String name, String description, int value, bool locked = false, bool isEdible = false)
        {
            Name = name;
            Description = description;
            IsEdible = isEdible;
            Value = value;
            Locked = locked;

        }
        public virtual int GetHeals()
        {
            return 0;
        }
        public String GetName()
        {
            return Name;
        }
        public int GetValue()
        {
            return Value;
        }
        public void SetName(String name)
        {
            Name = name;
        }
        public String GetDescription()
        {
            return Description;
        }
        public void SetDescription(String description)
        {
            Description = description;
        }

        public override string ToString()
        {
            return $"{Name}: Worth {Value} gold";
        }
    }
}

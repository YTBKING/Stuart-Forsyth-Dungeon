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
        public bool SpellBook;
        public int SpellDamage;
        protected int Value;
        public bool Locked;
        public bool IsArmour = false;

        public Item(String name, String description, int damage, int value, bool locked = false, bool isEdible = false, bool spellBook = false, int spellDamage = 0)
        {
            Name = name;
            Description = description;
            IsEdible = isEdible;
            Damage = damage;
            SpellBook = spellBook;
            SpellDamage = spellDamage;
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

        public int GetDamage()
        {
            return Damage;
        }

        public int GetSpellDamage()
        {
            return SpellDamage;
        }

    }
}

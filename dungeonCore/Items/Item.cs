using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
    public class Item
    {
        #region "Properties"
        protected string Name;
        protected string Description;
        public bool IsEdible;
        protected int Damage;
        protected int Value;
        public bool Locked;
        public bool IsArmour = false;
        #endregion

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
            return ($"[italic green]{Name}[/]: Worth [italic 178]{Value}g[/]");
        }
    }
}

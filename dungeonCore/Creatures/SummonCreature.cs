using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dungeon.Creatures
{
    public class SummonCreature
    {
        [JsonInclude]
        private string Name;
        [JsonInclude]
        private string Description;
        [JsonInclude]
        private int Health;
        [JsonInclude]
        private int MaxHealth;
        [JsonInclude]
        private int Damage;
        [JsonInclude]
        private int ManaDrain;
        public double damageBuff = 1;

        public SummonCreature(string name, string description, int health, int damage, int manaDrain) 
        { 
            Name = name;
            Description = description;
            MaxHealth = health;
            Damage = damage;
            Health = MaxHealth;
            ManaDrain = manaDrain;
        }
        #region "Get Attributes"
        public string GetName()
        {
            return Name;
        }

        public string GetDescription()
        {
            return Description;
        }

        public int GetHealth()
        {
            return Health;
        }
        public int GetMaxHealth()
        {
            return MaxHealth;
        }
        public int GetDamage()
        {
            return Convert.ToInt32(Math.Round(Damage * damageBuff));
        }
        public int GetManaDrain()
        {
            return ManaDrain;
        }
        #endregion

        #region "Edit Attributes"
        #region "Health"
        public void AddHealth(int heal)
        {
            Health += heal;
        }
        public void MinusHealth(int damage) 
        { 
            Health -= damage;
        }
        public void SetHealth(int health)
        {
            Health = health;
        }
        #endregion
        #region "MaxHealth"
        public void AddMaxHealth(int heal)
        {
            MaxHealth += heal;
        }
        public void MinusMaxHealth(int damage)
        {
            MaxHealth -= damage;
        }
        public void SetMaxHealth(int health)
        {
            MaxHealth = health;
        }
        #endregion
        #endregion

        public override string ToString()
        {
            return $"{Name}: {Damage} damage. -{ManaDrain} mana: {Health} health";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dungeon
{
    public class Creature
    {
        #region "Properties"
        [JsonInclude]
        protected string Name;
        [JsonInclude]
        protected int Health;
        [JsonInclude]
        protected int MaxHealth;
        [JsonInclude]
        protected Random random;
        [JsonInclude]
        protected List<Item> Drops = new List<Item>();
        public double XP;
        [JsonInclude]
        protected int PowerLvl;
        [JsonInclude]
        protected int GoldGiven;
        [JsonInclude]
        protected int Speed;
        public int TrueDamage;
        [JsonInclude]
        protected bool IsAgressive = false;
        #endregion

        public Creature(string name, int health, double xp, int gold, int speed = 1, int powerLvl = 0)
        {
            Name = name;
            MaxHealth = health;
            Health = health;
            random = new Random();
            GoldGiven = gold;
            XP = xp;
            PowerLvl = powerLvl;
            Speed = speed;
        }

        #region "Get Properties"
        public void isAgressive()
        {
            IsAgressive = true;
        }
        public bool GetAggression()
        {
            return IsAgressive;
        }
        public int GetGold()
        {
            return GoldGiven;
        }
        public double GetXp() { return XP; }
        public List<Item> GetDrops()
        {
            return Drops;
        }
        public virtual int GetAttackDamage(ArmourItem armour)
        {
            return (random.Next(1 + PowerLvl, 20 + PowerLvl)  - armour.GetDefence());
        }
        public int GetHealth()
        {
            return Health;
        }
        public string GetName()
        {
            return Name;
        }
        public int GetSpeed()
        {
            return Speed;
        }
        public int GetPowerLvl()
        {
            return PowerLvl;
        }
        public int GetMaxHealth()
        {
            return MaxHealth;
        }
        #endregion

        #region "AddDrop"
        public void AddDrop(Item item) 
        { 
            Drops.Add(item);
        }
        #endregion

        #region "Change Health"
        public void SetHealth(int amount)
        {
            Health = amount;
        }
        public virtual bool TakeDamage(int damage, WeaponItem weapon = null)
        {
            TrueDamage = damage;
            Health -= damage;
            if (Health <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public virtual bool TakeSpellDamage(string spell, int damage)
        {
            Console.WriteLine($"{Name} takes {damage} from {spell}");

            return TakeDamage(damage);
        }
        #endregion

    }
}

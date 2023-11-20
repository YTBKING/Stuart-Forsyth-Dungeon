using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dungeon
{
    class Creature
    {
        #region "Properties"
        private string Name;
        private int Health;
        protected Random random;
        protected List<Item> Drops = new List<Item>();
        public double XP;
        protected int PowerLvl;
        protected int GoldGiven;
        protected int Speed;
        #endregion

        public Creature(string name, int health, double xp, int gold, int speed = 1, int powerLvl = 0)
        {
            Name = name;
            Health = health;
            random = new Random();
            GoldGiven = gold;
            XP = xp;
            PowerLvl = powerLvl;
            Speed = speed;
        }

        #region "Get Properties"
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
        public bool TakeDamage(int damage)
        {
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

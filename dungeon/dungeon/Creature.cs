using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dungeon
{
    class Creature
    {
        private string Name;
        private int Health;
        protected Random random;
        protected Item Drop;
        protected Item Drop2;
        public double XP;
        protected int PowerLvl;

        public Creature(string name, int health, double xp, int powerLvl = 0, Item drop = null, Item drop2 = null)
        {
            Name = name;
            Health = health;
            random = new Random();
            Drop = drop;
            Drop2 = drop2;
            XP = xp;
            PowerLvl = powerLvl;
        }

        public double GetXp() { return XP; }
        public Item GetDrop1()
        {
            return Drop;
        }
        public Item GetDrop2() { return Drop2; }
        public bool IsDrop() { return Drop != null; }
        public bool MultiDrop()
        {
            return Drop2 != null;
        }
        public virtual int GetAttackDamage(ArmourItem armour)
        {
            return (random.Next(1, 10) + PowerLvl - armour.GetDefence());
        }
        public int GetHealth()
        {
            return Health;
        }
        public string GetName()
        {
            return Name;
        }
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
    }
}

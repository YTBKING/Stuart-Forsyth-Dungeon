using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dungeon
{
    class StaticCreature
    {
        string Name;
        int Health;
        Connection HiddenConnection;
        public StaticCreature(string name, int health, Connection hiddenConnection)
        {
            Name = name;
            Health = health;
            HiddenConnection = hiddenConnection;
        }

        public string GetName()
        {
            return Name;
        }

        public bool TakeDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                return true;
            }
            return false;
        }

        public Connection GetHiddens()
        {
            return HiddenConnection;
        }
    }
}

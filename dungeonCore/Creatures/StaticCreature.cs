﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dungeon
{
    public class StaticCreature
    {
        [JsonInclude]
        private string Name;
        [JsonInclude]
        private int Health;
        public Connection HiddenConnection;
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

using Dungeon.Creatures;
using dungeonCore.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dungeon
{
    public class SummonItem : Item
    {
        [JsonInclude]
        private SummonCreature summon;
        [JsonInclude]
        private int Health;
        [JsonInclude]
        private int MaxHealth;
        [JsonInclude]
        private int Damage;
        [JsonInclude]
        private int ManaDrain;
        [JsonInclude]
        private int Level = 1;
        [JsonInclude]
        private double damageBuff = 1;
        [JsonInclude]
        private int buffCost = 150;
        public SummonItem(string name, string description, int health, int damage, int manaDrain) : base(name, description, 0, true, false) 
        {
            Health = health;
            MaxHealth = health;
            Damage = damage;
            ManaDrain = manaDrain;
            summon = new SummonCreature(name, description, health, damage, manaDrain);
        }

        public SummonCreature GetSummon()
        {
            return summon;
        }
        public double GetDmgBuff()
        {
            return damageBuff;
        }
        public int GetBuffCost()
        {
            return buffCost;
        }

        public int GetManaDrain()
        {
            return ManaDrain;
        }
        public void AdjustLvl(int amount)
        {
            for (int i = 1; i <= amount; i++)
            {
                Level += 1;
                summon.damageBuff += 0.05;
            }
            buffCost = Convert.ToInt32(Math.Round(1.75 * buffCost));
        }
        public override string ToString()
        {
            return $"{Name}: {Damage} damage. -{ManaDrain} mana: {Health} health";
        }
    }
}

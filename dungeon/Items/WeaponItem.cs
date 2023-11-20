using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dungeon
{
    class WeaponItem : Item
    {
        private string Rarity;
        private List<string> Rarities = new List<string>() { "Common", "Uncommon", "Rare", "Epic", "Legendary" };
        private int UpgradeCost;
        private WeaponItem FutureStage;
        public WeaponItem(string name, string description, int damage, int value, string rarity = "Common", int baseUpgradeCost = 65, bool locked = false) : base(name, description, value, locked)
        {
            Name = name;
            Description = description;
            Damage = damage;
            Value = value;
            Locked = locked;
            Rarity = rarity;
            UpgradeCost = baseUpgradeCost;
        }

        public string GetRarity()
        {
            return Rarity;
        }

        public int GetUpgradeCost()
        {
            return UpgradeCost;
        }

        public void AlterUpgradeCost(double num)
        {
            UpgradeCost = Convert.ToInt32(num * UpgradeCost);
        }

        public int GetRarityValue()
        {
            switch(Rarity)
            {
                case "Common":
                    return 0;
                case "Uncommon":
                    return 5;
                case "Rare":
                    return 10;
                case "Epic":
                    return 20;
                case "Legendary":
                    return 50;
                default:
                    return 0;

            }
        }

        public void ImproveRarity(Player player, int num, WeaponItem weapon)
        {
            for (int i = 0; i < Rarities.Count; i++)
            {
                if (Rarities[i] == Rarity) 
                { 
                    if (Rarities[i] == "Legendary")
                    {
                        if (FutureStage != null)
                        {
                            player.RemoveItem(weapon, 1);
                            player.AddItem(FutureStage);
                            player.EquipedWeapon = FutureStage;
                            Console.WriteLine($"Your {weapon} evolved");
                            Console.WriteLine($"{weapon} -> {FutureStage}");
                            Rarity = "Legendary";
                        } else { Console.WriteLine("You cannot upgrade this weapon any higher"); }
                    }
                    else
                    {
                        Rarity = Rarities[i + num];
                    }

                    break;
                }
            }
        }

        public int GetDamage()
        {
            
            return Damage + GetRarityValue();
        }

        public override string ToString()
        {
            return $"{Rarity} {Name}. +{GetDamage()} damage. Worth {Value} gold";
        }

        public void AddBetterForm(WeaponItem weapon)
        {
            FutureStage = weapon;
        }
    }
}

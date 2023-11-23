using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
    public class WeaponItem : Item
    {
        public string Rarity;
        private List<string> Rarities = new List<string>() { "Holy", "Demonic", "Frozen", "Blazing", "Common", "Uncommon", "Rare", "Epic", "Legendary" };
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
            if (Rarity == "Common")
            {
                return $"[italic bold silver]{Rarity}[/]";
            }
            else if (Rarity == "Uncommon")
            {
                return $"[italic bold green1]{Rarity}[/]";
            }
            else if (Rarity == "Rare")
            {
                return $"[italic bold 33]{Rarity}[/]";
            }
            else if (Rarity == "Epic")
            {
                return $"[bold purple_2]{Rarity}[/]";
            }
            else if (Rarity == "Legendary")
            {
                return $"[bold 220]{Rarity}[/]";
            }
            else if (Rarity == "Holy")
            {
                return $"[italic white]{Rarity}[/]";
            }
            else if (Rarity == "Demonic")
            {
                return $"[italic 52]{Rarity}[/]";
            }
            else if (Rarity == "Frozen")
            {
                return $"[italic 153]{Rarity}[/]";
            }
            else if (Rarity == "Blazing")
            {
                return $"[italic red3_1]{Rarity}[/]";
            }
            return Rarity;
        }

        public string GetTrueRarity()
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

        public double GetRarityValue()
        {
            switch(Rarity)
            {
                case "Common":
                    return 1;
                case "Uncommon":
                    return 1.25;
                case "Rare":
                    return 1.5;
                case "Epic":
                    return 2;
                case "Legendary":
                    return 3;
                default:
                    return 1;

            }
        }

        public bool ImproveRarity(Player player, int num, WeaponItem weapon)
        {
            if (weapon.Rarity != "Demonic" && weapon.Rarity != "Holy" && weapon.Rarity != "Frozen" && weapon.Rarity != "Blazing")
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
                                return true;
                            }
                            else { Console.WriteLine("You cannot upgrade this weapon any higher"); return true; }
                        }
                        else
                        {
                            Rarity = Rarities[i + num];
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public int GetDamage()
        {
            
            return Convert.ToInt32(Math.Round(Damage * GetRarityValue()));
        }
        public void AddBetterForm(WeaponItem weapon)
        {
            FutureStage = weapon;
        }

        public override string ToString()
        {
            return $"{GetRarity()} {Name}: [italic red]+{GetDamage()}[/] damage. Worth {Value}g";
        }


    }
}

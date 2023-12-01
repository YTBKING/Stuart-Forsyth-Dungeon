namespace Dungeon
{
    class StaffWeapon : WeaponItem
    {
        private int SpellDamageBuff;
        public StaffWeapon(string name, string description, int spellDamageBuff, int value, bool locked = false) : base(name, description, 4, value, "Common", 100, locked)
        {
            SpellDamageBuff = spellDamageBuff;
        }

        public int GetSpellBuff()
        {
            return SpellDamageBuff;
        }

        public override string ToString()
        {
            return $"{Name}: [italic red]+{SpellDamageBuff}[/] spell damage: Worth {Value}g";
        }
    }
}

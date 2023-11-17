using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dungeon
{
    class NPCRoom : Room
    {
        public NPC Vendor;
        string Description = "";
        public NPCRoom(NPC npc) : base("A simple shop", false, true)
        {
            Vendor = npc;
        }
        public void SetPlayerVendor(Player player)
        {
            player.SetVendor(Vendor);
        }

        public override string GetDescription()
        {
            Description = $"{Vendor.GetName()} is selling:\n";
            for (int i = 0; i < Vendor.SellingItems.Count; i++)
            {
                Description += $"{i + 1}. {Vendor.SellingItems[i].GetName()} - {Vendor.SellingCosts[i]} gold\n";
            }

            return Description;
        }

    }
}

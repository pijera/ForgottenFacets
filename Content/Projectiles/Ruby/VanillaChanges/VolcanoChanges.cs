using ForgottenFacets.Content.ModPlayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Projectiles.Ruby.VanillaChanges
{
    internal class VolcanoChanges : GlobalProjectile
    {
        public class FieryGreatswordChanges : GlobalItem
        {
            public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
            {
                if (item.type != ItemID.FieryGreatsword)
                    return;

                player.GetModPlayer<SuperHeatingWeapons>().AddHeat(20f);
            }
        }
    }
}

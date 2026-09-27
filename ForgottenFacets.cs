using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets
{
    // Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
    public class ForgottenFacets : Mod
    {
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            byte messageType = reader.ReadByte();

            if (messageType == 0)
            {
                int projectileIndex = reader.ReadInt32();
                int npcIndex = reader.ReadInt32();

                if (projectileIndex < 0 || projectileIndex >= Main.maxProjectiles)
                    return;

                if (npcIndex < 0 || npcIndex >= Main.maxNPCs)
                    return;

                Projectile projectile = Main.projectile[projectileIndex];
                NPC npc = Main.npc[npcIndex];

                if (!projectile.active || !npc.active)
                    return;

                if (projectile.owner != whoAmI)
                    return;

                if (npc.boss)
                    return;

                if (projectile.ModProjectile is Content.Projectiles.Ruby.VanillaChanges.SunfuryChanges sunfury)
                {
                    sunfury.SetVictim(npcIndex);
                }
            }
        }
    }
}
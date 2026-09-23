
using ForgottenFacets.Content.ModPlayers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Projectiles.Ruby.VanillaChanges
{
    internal class MoltenFuryChanges : ModProjectile
    {
        private const int arrowAmount = 5;
        private const int arrowDelay = 6;

        private int arrowTimer;

        public override string Texture => "ForgottenFacets/Assets/Misc/Invisible";

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.timeLeft = arrowAmount * arrowDelay + 10;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            SuperHeatingWeapons heat = player.GetModPlayer<SuperHeatingWeapons>();

            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }
            if (Projectile.ai[0] == 0) 
            {
                Projectile.ai[0] = 1;
                Projectile.velocity.Normalize();
            }
            arrowTimer++;

            if (arrowTimer >= arrowDelay) 
            {
                arrowTimer = 0;

                FireArrow(player);
                Projectile.ai[1]++;

                if (Projectile.ai[1] >= arrowAmount)
                    Projectile.Kill();
            }
        }

        private void FireArrow(Player player)
        {
            Vector2 velocity = Projectile.velocity * 12f;

            int arrow = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, velocity, ProjectileID.FireArrow, Projectile.damage, Projectile.knockBack, Projectile.owner);

            Main.projectile[arrow].GetGlobalProjectile<MoltenFuryArrowChanges>().NoHeat = true;
        }




    }

    public class MoltenFuryArrowChanges : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool MoltenFuryShot;
        public bool NoHeat;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse_WithAmmo itemSource && itemSource.Item.type == ItemID.MoltenFury)
                MoltenFuryShot = true;
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!MoltenFuryShot)
                return;

            if (NoHeat)
                return;

            Player player = Main.player[projectile.owner];

            if (player == null || !player.active)
                return;

            player.GetModPlayer<SuperHeatingWeapons>().AddHeat(5f);
        }
    }
}

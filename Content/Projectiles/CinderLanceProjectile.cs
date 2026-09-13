using ForgottenFacets.Content.ModPlayers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.Tile_Entities;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Projectiles
{
    internal class CinderLanceProjectile : ModProjectile
    {
        public override string Texture => "ForgottenFacets/Assets/Projectiles/CinderLanceProjectile";

        protected virtual float HoldoutRangeMin => 40f;
        protected virtual float HoldoutRangeMax => 120f;

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Spear);
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            int duration = player.itemAnimationMax;

            player.heldProj = Projectile.whoAmI; 

            if (Projectile.timeLeft > duration)
            {
                Projectile.timeLeft = duration;
            }

            Projectile.velocity = Vector2.Normalize(Projectile.velocity);

            Projectile.spriteDirection = Projectile.velocity.X >= 0 ? 1 : -1;

            float halfDuration = duration * 0.5f;
            float progress;

            if (Projectile.timeLeft < halfDuration)
            {
                progress = Projectile.timeLeft / halfDuration;
            }
            else
            {
                progress = (duration - Projectile.timeLeft) / halfDuration;
            }

            Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);

            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);
            }

            if (!Main.dedServ)
            {
                if (Main.rand.NextBool(3))
                {
                    Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GemRuby, Projectile.velocity.X * 2f, Projectile.velocity.Y * 2f, Alpha: 128, Scale: 1.2f);
                }

                if (Main.rand.NextBool(2))
                {
                    Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GemRuby, Alpha: 128, Scale: 0.3f);
                }
                for (int i = 0; i < 5; i++)
                {
                    Dust.NewDustPerfect(Projectile.Center, DustID.Torch, Main.rand.NextVector2Circular(3, 3), 0, default, 1.5f);
                }
            }



            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SuperHeatingWeapons HeatStacking = Main.player[Projectile.owner].GetModPlayer<SuperHeatingWeapons>();


            HeatStacking.AddHeat(2);
        }

    }

}

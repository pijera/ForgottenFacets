using ForgottenFacets.Content.Buffs;
using ForgottenFacets.Content.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Projectiles
{
    internal class AncientIceArmorProjectile : ModProjectile
    {
        public override string Texture => "ForgottenFacets/Assets/Misc/Invisible";
        private float Progress => Utils.Clamp(1 - Projectile.timeLeft / 25f, 0f, 1f);

        private float Radius => Projectile.ai[0] * Progress + 1;

        public override void SetDefaults()
        {
            Projectile.alpha = 255;

            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.DamageType = DamageClass.Ranged;

            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 20;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;

        }

        public override void AI()
        {
            for (int i = 0; i < 5; i++)
            {
                float rot = Main.rand.NextFloat(MathHelper.TwoPi);

                Vector2 direction = rot.ToRotationVector2();

                Dust.NewDustPerfect(Projectile.Center + direction * Radius, DustID.IceTorch,
                    direction * Main.rand.NextFloat(0.5f, 1.5f), 100, default, Main.rand.NextFloat(0.8f, 1.5f));

                Dust.NewDustPerfect(Projectile.Center + direction * Radius, ModContent.DustType<GlowDust>(),
                    direction * 0.5f, 80, Color.LightCyan, Main.rand.NextFloat(0.5f, 2f));
            }

        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)//pravi novi hitbox (koji je veliki krug) i chekuje da li se neprijatelj nalazi u njemu
        {
            Vector2 line = targetHitbox.Center.ToVector2() - Projectile.Center;
            line.Normalize();
            line *= Radius;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + line);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(950);
            Texture2D ring = TextureAssets.Extra[ExtrasID.KeybrandRing].Value;

            float reverseProgress = 1f - Progress;

            Main.spriteBatch.Draw(ring, Projectile.Center - Main.screenPosition, null, Color.LightCyan * (1 - Progress), 0f, ring.Size() / 2f, 0.035f * Radius, 0f, 0f);
            return false;
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<FrostbittenDebuff>(),150);
        }
    }
}

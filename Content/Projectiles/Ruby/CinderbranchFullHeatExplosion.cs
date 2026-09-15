using ForgottenFacets.Content.Buffs;
using ForgottenFacets.Content.Dusts;
using ForgottenFacets.Core;
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

namespace ForgottenFacets.Content.Projectiles.Ruby
{
    internal class CinderbranchFullHeatExplosion : ModProjectile
    {
        public override string Texture => "ForgottenFacets/Assets/Misc/Invisible";
        private float Progress => Utils.Clamp(1 - Projectile.timeLeft / 30f, 0f, 1f);

        private float Radius => Projectile.ai[0] * (Progress * Progress) + 5f;

        public override void SetDefaults()
        {
            Projectile.alpha = 255;

            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.DamageType = DamageClass.Magic;

            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 30;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;

        }

        public override void AI()
        {
            

            for (int i = 0; i < 15; i++)
            {

                float rot = Main.rand.NextFloat(MathHelper.TwoPi);

                Vector2 direction = rot.ToRotationVector2();

                Dust.NewDustPerfect(Projectile.Center + direction * Radius, DustID.Torch,
                    direction * Main.rand.NextFloat(0.5f, 1.5f), 0, Color.DarkOrange, Main.rand.NextFloat(0.8f, 1.5f));

                Dust.NewDustPerfect(Projectile.Center + direction * Radius, ModContent.DustType<GlowDust>(),
                    direction * 0.5f, 80, Color.Lerp(Color.DarkOrange,Color.OrangeRed,0.05f), Main.rand.NextFloat(0.5f, 2f));

                Dust.NewDustPerfect(Projectile.Center + direction * Radius, ModContent.DustType<SparkleDust>(),
                    direction * 0.5f, 120, Color.Lerp(Color.DarkOrange, Color.OrangeRed, 0.05f), Main.rand.NextFloat(0.3f, 1.2f));

            }

            for (int i = 0; i < 20; i++)
            {

                float rot = Main.rand.NextFloat(MathHelper.TwoPi);

                Vector2 direction = rot.ToRotationVector2();

                Dust.NewDustPerfect((Projectile.Center + direction * Radius)/2, DustID.Smoke,
                    direction * 0.5f, 200, Color.DarkGray, Main.rand.NextFloat(2f, 3f));
            }

            Projectile.frame = 3;
            Projectile.frameCounter = 3;

        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 line = targetHitbox.Center.ToVector2() - Projectile.Center;
            line.Normalize();
            line *= Radius;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + line);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(85);
            Texture2D ring = TextureAssets.Projectile[ProjectileID.Flames].Value;

            Rectangle frame = ring.Frame(1, 7, 0, Projectile.frame);

            Main.spriteBatch.Draw(ring, Projectile.Center - Main.screenPosition, frame, Color.OrangeRed * ((1 - Progress)), 0f, frame.Size() / 2f, 0.035f * Radius, SpriteEffects.None, 1f);

            return false;
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 240);
        }

    }
}

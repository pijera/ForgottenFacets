using ForgottenFacets.Content.Dusts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Projectiles.FrostBlasterProjectiles
{
    internal class FrostBlasterStrongProjectile : ModProjectile
    {
        public override string Texture => "ForgottenFacets/Assets/Projectiles/FrostBlasterProjectiles/FrostBlasterStrongProjectile";
        public override void SetDefaults()
        {
            Projectile.width = 25;
            Projectile.height = 25;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 600;

            Projectile.scale *= 1.3f;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            Vector2 velocity = -Projectile.velocity.SafeNormalize(Vector2.Zero);
            velocity += Main.rand.NextVector2Circular(1f, 1f);


            for (int i = 0; i < 15; i++)
            {
                Dust.NewDustPerfect(Projectile.Left, DustID.Frost, Main.rand.NextVector2Circular(5, 5), 50, default, Main.rand.NextFloat(0.9f, 1.3f)).noGravity = true;
            }

            if (Main.rand.NextBool(1))
            {
                Dust.NewDustPerfect(Projectile.Left, ModContent.DustType<SparkleDust>(), Main.rand.NextVector2Circular(4, 4), 0, Color.SkyBlue, Main.rand.NextFloat(0.8f, 1f)).noGravity = true;
                Dust.NewDustPerfect(Projectile.Left, ModContent.DustType<GlowDust>(), Main.rand.NextVector2Circular(4, 4), 0, Color.SkyBlue, Main.rand.NextFloat(0.8f, 1f)).noGravity = true;

            }


        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Item27 with { Volume = 1.2f, PitchRange = (0.8f, 1.2f) });
            for (int i = 0; i < 35; i++)
            {
                Dust.NewDustPerfect(Projectile.Center, DustID.Ice, Main.rand.NextVector2Circular(5, 5), 0, default, Main.rand.NextFloat(1f, 1.3f)).noGravity = true;
            }
            return true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(SoundID.Item27 with { Volume = 1.2f, PitchRange = (0.8f, 1.2f) });
            for (int i = 0; i < 35; i++)
            {
                Dust.NewDustPerfect(Projectile.Center, DustID.Ice, Main.rand.NextVector2Circular(5, 5), 0, default, Main.rand.NextFloat(1f, 1.3f)).noGravity = true;
            }

            for (int i = 0; i < 6; i++)
            {

                float angle = MathHelper.TwoPi / 5f * i;
                Vector2 velocity = angle.ToRotationVector2() * 4f;

                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, velocity,
                    ModContent.ProjectileType<FrostBlasterVeryWeakProjectile>(), (int)Projectile.damage / 3, 0);
            }

            target.AddBuff(BuffID.Frostburn, 180);

        }
    }
}


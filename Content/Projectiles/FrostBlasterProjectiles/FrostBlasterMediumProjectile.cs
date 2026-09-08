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
    internal class FrostBlasterMediumProjectile : ModProjectile
    {
        public override string Texture => "ForgottenFacets/Assets/Projectiles/FrostBlasterProjectiles/FrostBlasterMediumProjectile";
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 600;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            Vector2 velocity = -Projectile.velocity.SafeNormalize(Vector2.Zero);
            velocity += Main.rand.NextVector2Circular(0.5f, 0.5f);


            for (int i = 0; i < 5; i++)
            {
                Dust.NewDustPerfect(Projectile.Left, DustID.Frost, Main.rand.NextVector2Circular(4, 4), 100, default, Main.rand.NextFloat(0.7f, 1f)).noGravity = true;
            }

            if (Main.rand.NextBool(3))
                Dust.NewDustPerfect(Projectile.Left, ModContent.DustType<SparkleDust>(), Main.rand.NextVector2Circular(3, 3), 0, Color.SkyBlue, Main.rand.NextFloat(0.6f, 0.8f)).noGravity = true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Item27 with { Volume = 1.2f, PitchRange = (0.8f, 1.2f) });
            for (int i = 0; i < 20; i++)
            {
                Dust.NewDustPerfect(Projectile.Center, DustID.Ice, Main.rand.NextVector2Circular(3, 3), 0, default, Main.rand.NextFloat(0.8f, 1f)).noGravity = true;
            }
            return true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(SoundID.Item27 with { Volume = 1.2f, PitchRange = (0.8f, 1.2f) });
            for (int i = 0; i < 20; i++)
            {
                Dust.NewDustPerfect(Projectile.Center, DustID.Ice, Main.rand.NextVector2Circular(3, 3), 0, default, Main.rand.NextFloat(0.8f, 1f)).noGravity = true;
            }

            for (int i = 0; i < 3; i++)
            {

                float angle = MathHelper.TwoPi / 5f * i;
                Vector2 velocity = angle.ToRotationVector2() * 4f;

                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, velocity,
                    ModContent.ProjectileType<FrostBlasterVeryWeakProjectile>(), (int)Projectile.damage / 5, 0);
            }


        }
    }
}

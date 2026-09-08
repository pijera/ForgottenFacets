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
    internal class FrostBlasterVeryWeakProjectile : ModProjectile
    {
        public override string Texture => "ForgottenFacets/Assets/Projectiles/FrostBlasterProjectiles/FrostBlasterVeryWeakProjectile";
        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;

            Projectile.scale *= 0.4f;
        }
        public override void AI()
        {
            Projectile.velocity.Y += 0.3f;
            Projectile.rotation += 0.3f;

            Vector2 velocity = -Projectile.velocity.SafeNormalize(Vector2.Zero);
            velocity += Main.rand.NextVector2Circular(0.5f, 0.5f);


            Dust.NewDustPerfect(Projectile.Left, DustID.Frost, Main.rand.NextVector2Circular(2, 2), 200, default, Main.rand.NextFloat(0.4f, 0.6f)).noGravity = true;

            if (Main.rand.NextBool(10))
                Dust.NewDustPerfect(Projectile.Left, ModContent.DustType<SparkleDust>(), Main.rand.NextVector2Circular(1, 1), 0, Color.SkyBlue, Main.rand.NextFloat(0.3f, 0.5f)).noGravity = true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Item27 with { Volume = 1f, PitchRange = (0.8f, 1.2f) });
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDustPerfect(Projectile.Center, DustID.Ice, Main.rand.NextVector2Circular(2, 2), 0, default, Main.rand.NextFloat(0.8f, 1f)).noGravity = true;
            }
            return true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(SoundID.Item27 with { Volume = 1f, PitchRange = (0.8f, 1.2f) });
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDustPerfect(Projectile.Center, DustID.Ice, Main.rand.NextVector2Circular(2, 2), 0, default, Main.rand.NextFloat(0.8f, 1f)).noGravity = true;
            }
        }

    }
}

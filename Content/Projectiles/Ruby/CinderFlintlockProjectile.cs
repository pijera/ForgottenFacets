using ForgottenFacets.Content.Dusts;
using ForgottenFacets.Content.ModPlayers;
using ForgottenFacets.Content.Projectiles.Aquamarine.FrostBlasterProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Projectiles.Ruby
{
    internal class CinderFlintlockProjectile : ModProjectile
    {
        public override string Texture => "ForgottenFacets/Assets/Projectiles/CinderFlintlockProjectile";

        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(10, 10);
            Projectile.friendly = true; 
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 600;

            Projectile.scale *= 1.2f;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            Vector2 velocity = -Projectile.velocity.SafeNormalize(Vector2.Zero);
            velocity += Main.rand.NextVector2Circular(0.5f, 0.5f);


            for (int i = 0; i < 8; i++)
            {
                Dust.NewDustPerfect(Projectile.Left, DustID.Torch, Main.rand.NextVector2Circular(4, 4), 100, default, Main.rand.NextFloat(0.7f, 1f)).noGravity = true;
            }

        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Item10 with { Volume = 1.2f, PitchRange = (0.8f, 1.2f) });
            for (int i = 0; i < 30; i++)
            {
                Dust.NewDustPerfect(Projectile.Center, DustID.Torch, Main.rand.NextVector2Circular(3, 3), 0, default, Main.rand.NextFloat(0.8f, 1f)).noGravity = true;
            }
            return true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(SoundID.Item10 with { Volume = 1.2f, PitchRange = (0.8f, 1.2f) });

            SuperHeatingWeapons HeatStacking = Main.player[Projectile.owner].GetModPlayer<SuperHeatingWeapons>();
            HeatStacking.AddHeat(8f);

            for (int i = 0; i < 30; i++)
            {
                Dust.NewDustPerfect(Projectile.Center, DustID.Torch, Main.rand.NextVector2Circular(3, 3), 0, default, Main.rand.NextFloat(0.8f, 1f)).noGravity = true;
            }
        }

    }
}

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

namespace ForgottenFacets.Content.Projectiles.FrostBlasterProjectiles;

internal class FrostBlasterWeakProjectile : ModProjectile
{
    public override string Texture => "ForgottenFacets/Assets/Projectiles/FrostBlasterProjectiles/FrostBlasterWeakProjectile";
    public override void SetDefaults()
    {
        Projectile.width = 12;
        Projectile.height = 12;
        Projectile.friendly = true;
        Projectile.DamageType = DamageClass.Ranged;
        Projectile.penetrate = 1;
        Projectile.timeLeft = 600;
    }
    public override void AI()
    {
        Projectile.velocity.Y += 0.06f;
        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

        Vector2 velocity = -Projectile.velocity.SafeNormalize(Vector2.Zero);
        velocity += Main.rand.NextVector2Circular(0.5f, 0.5f);


        for (int i = 0; i < 3; i++)
        {
            Dust.NewDustPerfect(Projectile.Left, DustID.Frost, Main.rand.NextVector2Circular(3, 3), 200, default, Main.rand.NextFloat(0.6f, 0.9f)).noGravity = true;
        }

        if (Main.rand.NextBool(5))
            Dust.NewDustPerfect(Projectile.Left, ModContent.DustType<SparkleDust>(), Main.rand.NextVector2Circular(3, 3), 0, Color.SkyBlue, Main.rand.NextFloat(0.4f, 0.6f)).noGravity = true;
    }
    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        SoundEngine.PlaySound(SoundID.Item27 with { Volume = 1.2f, PitchRange = (0.8f, 1.2f) });
        for (int i = 0; i < 15; i++)
        {
            Dust.NewDustPerfect(Projectile.Center, DustID.Ice, Main.rand.NextVector2Circular(2, 2), 0, default, Main.rand.NextFloat(0.8f, 1f)).noGravity = true;
        }
        return true;
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        SoundEngine.PlaySound(SoundID.Item27 with { Volume = 1.2f, PitchRange = (0.8f, 1.2f) });
        for (int i = 0; i < 15; i++)
        {
            Dust.NewDustPerfect(Projectile.Center, DustID.Ice, Main.rand.NextVector2Circular(2, 2), 0, default, Main.rand.NextFloat(0.8f, 1f)).noGravity = true;
        }
    }
}

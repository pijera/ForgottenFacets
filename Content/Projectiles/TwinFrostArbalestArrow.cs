using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Projectiles
{
    internal class TwinFrostArbalestArrow : ModProjectile
    {
        public override string Texture => "ForgottenFacets/Assets/Projectiles/TwinFrostArbalestArrow";
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 600;
        }
        public override void AI()
        {
            Projectile.velocity.Y += 0.1f;
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;

            Vector2 velocity = -Projectile.velocity.SafeNormalize(Vector2.Zero);
            velocity += Main.rand.NextVector2Circular(0.5f, 0.5f);


            for (int i = 0; i < 15; i++)
            {
                Dust.NewDustPerfect(Projectile.Center, DustID.IceTorch, velocity, 0, Color.Lerp(Color.LightCyan,Color.Cyan,0.5f), Main.rand.NextFloat(0.7f,1.2f)).noGravity = false;
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 7; i++)
            {
                Dust.NewDustPerfect(Projectile.Bottom, DustID.WoodFurniture, null, 0, default, 1f).noGravity = false;
                Dust.NewDustPerfect(Projectile.Bottom, DustID.IceTorch, null, 0, default, 1f).noGravity = false;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(2))
                target.AddBuff(BuffID.Frostburn, 240);
        }

    }
}

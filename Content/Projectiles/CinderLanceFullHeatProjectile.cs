using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Projectiles
{
    internal class CinderLanceFullHeatProjectile : ModProjectile
    {
        public override string Texture => "ForgottenFacets/Assets/Projectiles/CinderLanceFullHeatProjectile";

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;

            Projectile.width = 100;
            Projectile.height = 30;

            Projectile.scale *= 2f;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.PiOver4;

            Vector2 velocity = -Projectile.velocity.SafeNormalize(Vector2.Zero);
            velocity += Main.rand.NextVector2Circular(1f, 1f);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Projectiles.Ruby
{
    internal class CinderbranchProjectile : ModProjectile
    {
        internal Color drawColor;

        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.Flames}";

        public override void SetDefaults()
        {
            Projectile.width = 25;
            Projectile.height = 25;
            Projectile.friendly = true;

            Projectile.penetrate = 2;
            Projectile.timeLeft = 30;
            Projectile.tileCollide = false;

            Projectile.rotation = Main.rand.NextFloat(6.28f);

            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 15;

           
        }

        public override void AI()
        {
            Projectile.rotation += Projectile.velocity.Length() * 0.02f;

            Lighting.AddLight(Projectile.Center, 0.8f, 0.81f, 0.31f);

            // Animate the 7 frames
            if (++Projectile.frameCounter >= 2)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;

                if (Projectile.frame >= 7)
                    Projectile.frame = 3;
            }

            if (Main.rand.NextBool(70))
                Dust.NewDustPerfect( Projectile.Center, DustID.Torch, Main.rand.NextVector2Circular(1.5f, 1.5f), Main.rand.Next(100, 201), default, Main.rand.NextFloat(0.5f, 1.5f));
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust poisonDust = Dust.NewDustPerfect(Projectile.Center, DustID.Torch, Main.rand.NextVector2Circular(1.5f, 1.5f), Main.rand.Next(100, 201), default, Main.rand.NextFloat(0.5f, 1.5f));
            }

            target.AddBuff(BuffID.OnFire, 150);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;

            Rectangle frame = tex.Frame(1, 7, 0, Projectile.frame);

            float progress = 1f - Projectile.timeLeft / 30f;

            drawColor = Color.Lerp(Color.Yellow, Color.Red, progress * 1.5f);

            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, frame, drawColor * 0.9f, Projectile.rotation, frame.Size() * MathHelper.Lerp(1f, 1.6f, progress) / 2f,
                Projectile.scale * MathHelper.Lerp(0.3f, 1.2f, progress), SpriteEffects.None, 0f);
            return false;

        }


    }
}
